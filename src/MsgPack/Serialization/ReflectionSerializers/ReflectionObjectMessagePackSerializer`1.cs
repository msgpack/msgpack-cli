#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2015 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections;
using System.Collections.Generic;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Implements reflection-based object serializer for restricted platforms.
	/// </summary>
	internal class ReflectionObjectMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private static readonly PropertyInfo DictionaryEntryKeyProperty = typeof( DictionaryEntry ).GetProperty( "Key" );
		private static readonly PropertyInfo DictionaryEntryValueProperty = typeof( DictionaryEntry ).GetProperty( "Value" );

		private readonly Func<object, object>[] _getters;
		private readonly Action<object, object>[] _setters;
		private readonly MemberInfo[] _memberInfos;
		private readonly DataMemberContract[] _contracts;
		private readonly Dictionary<string, int> _memberIndexes;
		private readonly IMessagePackSerializer[] _serializers;
		private readonly ParameterInfo[] _constructorParameters;
		private readonly Dictionary<int, int> _constructorArgumentIndexes;

		public ReflectionObjectMessagePackSerializer( SerializationContext context )
			: base( context )
		{
			SerializationTarget.VerifyType( typeof( T ) );
			var target = SerializationTarget.Prepare( context, typeof( T ) );
			ReflectionSerializerHelper.GetMetadata( target.Members, context, out this._getters, out this._setters, out this._memberInfos, out this._contracts, out this._serializers );
			this._memberIndexes =
				this._contracts
					.Select( ( contract, index ) => new KeyValuePair<string, int>( contract.Name, index ) )
					.Where( kv => kv.Key != null )
					.ToDictionary( kv => kv.Key, kv => kv.Value );
			this._constructorParameters =
				( !typeof( IUnpackable ).IsAssignableFrom( typeof( T ) ) && target.IsConstructorDeserialization )
					? target.DeserializationConstructor.GetParameters()
					: null;
			if ( this._constructorParameters != null )
			{
				this._constructorArgumentIndexes = new Dictionary<int, int>( this._memberIndexes.Count );
				foreach ( var member in target.Members )
				{
					int index =
#if SILVERLIGHT && !WINDOWS_PHONE
						this._constructorParameters.FindIndex( 
#else
							Array.FindIndex( this._constructorParameters,
#endif // SILVERLIGHT && !WINDOWS_PHONE
							item => item.Name.Equals( member.Contract.Name, StringComparison.OrdinalIgnoreCase ) && item.ParameterType == member.Member.GetMemberValueType()
						);
					if ( index >= 0 )
					{
						this._constructorArgumentIndexes.Add( index, this._memberIndexes[ member.Contract.Name ] );
					}
				}
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			var asPackable = objectTree as IPackable;
			if ( asPackable != null )
			{
				asPackable.PackToMessage( packer, null );
				return;
			}

			if ( this.OwnerContext.SerializationMethod != SerializationMethod.Map )
			{
				packer.PackArrayHeader( this._serializers.Length );
				for ( int i = 0; i < this._serializers.Length; i++ )
				{
					this.PackMemberValue( packer, objectTree, i );
				}
			}
			else
			{
				packer.PackMapHeader( this._serializers.Length );
				for ( int i = 0; i < this._serializers.Length; i++ )
				{
					packer.PackString( this._contracts[ i ].Name );
					this.PackMemberValue( packer, objectTree, i );
				}
			}
		}

		private void PackMemberValue( Packer packer, T objectTree, int index )
		{
			if ( this._getters[ index ] == null )
			{
				// missing member should be treated as nil.
				packer.PackNull();
				return;
			}

			var value = this._getters[ index ]( objectTree );

			var nilImplication =
				ReflectionNilImplicationHandler.Instance.OnPacking(
					new ReflectionSerializerNilImplicationHandlerParameter(
						this._memberInfos[ index ].GetMemberValueType(),
						this._contracts[ index ].Name ),
					this._contracts[ index ].NilImplication
				);
			if ( nilImplication != null )
			{
				nilImplication( value );
			}

			this._serializers[ index ].PackTo( packer, value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			object result =
				this._constructorParameters == null
					? ReflectionExtensions.CreateInstancePreservingExceptionType( typeof( T ) )
					: this._constructorParameters.Select( p =>
						p.GetHasDefaultValue()
						? p.DefaultValue
						: p.ParameterType.GetIsValueType()
						? ReflectionExtensions.CreateInstancePreservingExceptionType( p.ParameterType )
						: null
					).ToArray();

			var unpacked = 0;

			var asUnpackable = result as IUnpackable;
			if ( asUnpackable != null )
			{
				asUnpackable.UnpackFromMessage( unpacker );
				return ( T )result;
			}

			if ( unpacker.IsArrayHeader )
			{
				var itemsCount = UnpackHelpers.GetItemsCount( unpacker );

				for ( int i = 0; i < itemsCount; i++ )
				{
					result = this.UnpackMemberValue( result, unpacker, itemsCount, ref unpacked, i, i );
				}
			}
			else
			{
#if DEBUG && !UNITY
				Contract.Assert( unpacker.IsMapHeader, "unpacker.IsMapHeader" );
#endif // DEBUG && !UNITY
				var itemsCount = UnpackHelpers.GetItemsCount( unpacker );

				for ( int i = 0; i < itemsCount; i++ )
				{
					string name;
					if ( !unpacker.ReadString( out name ) )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					if ( name == null )
					{
						// missing member, drain the value and discard it.
						if ( !unpacker.Read() )
						{
							throw SerializationExceptions.NewMissingItem( i );
						}
						continue;
					}

					int index;
					if ( !this._memberIndexes.TryGetValue( name, out index ) )
					{
						// key does not exist in the object, skip the associated value
						if ( unpacker.Skip() == null )
						{
							throw SerializationExceptions.NewMissingItem( i );
						}
						continue;
					}

					result = this.UnpackMemberValue( result, unpacker, itemsCount, ref unpacked, index, i );
				}
			}

			if ( this._constructorParameters == null )
			{
				return ( T )result;
			}
			else
			{
				return ReflectionExtensions.CreateInstancePreservingExceptionType<T>( typeof( T ), result as object[] );
			}
		}

		private object UnpackMemberValue( object objectGraph, Unpacker unpacker, int itemsCount, ref int unpacked, int index, int unpackerOffset )
		{
			object nullable = null;

			var setter = index < this._setters.Length ? this._setters[ index ] : null;
			if ( unpacked < itemsCount )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( unpackerOffset );
				}

				if ( !unpacker.LastReadData.IsNil )
				{
					if ( setter != null || this._constructorParameters != null )
					{
						nullable = this.UnpackSingleValue( unpacker, index );
					}
					else if ( this._getters[ index ] != null ) // null getter supposes undeclared member (should be treated as nil)
					{
						this.UnpackAndAddCollectionItem( objectGraph, unpacker, index );
					}
				}
			}

			if ( this._constructorParameters != null )
			{
#if DEBUG && !UNITY
				Contract.Assert( objectGraph is object[], "objectGraph is object[]" );
#endif // !UNITY

				int argumentIndex;
				if ( this._constructorArgumentIndexes.TryGetValue( index, out argumentIndex ) )
				{
					if ( nullable == null )
					{
						ReflectionNilImplicationHandler.Instance.OnUnpacked(
							new ReflectionSerializerNilImplicationHandlerOnUnpackedParameter(
								this._memberInfos[ index ].GetMemberValueType(),
								// ReSharper disable once PossibleNullReferenceException
								value => ( objectGraph as object[] )[ argumentIndex ] = nullable,
								this._contracts[ index ].Name,
								this._memberInfos[ index ].DeclaringType
							),
							this._contracts[ index ].NilImplication
						)( null );
					}
					else
					{
						( objectGraph as object[] )[ argumentIndex ] = nullable;
					}
				}
			}
			else if ( setter != null )
			{
				if ( nullable == null )
				{
					ReflectionNilImplicationHandler.Instance.OnUnpacked(
						new ReflectionSerializerNilImplicationHandlerOnUnpackedParameter(
							this._memberInfos[ index ].GetMemberValueType(),
							value => setter( objectGraph, nullable ),
							this._contracts[ index ].Name,
							this._memberInfos[ index ].DeclaringType
						),
						this._contracts[ index ].NilImplication
					)( null );
				}
				else
				{
					setter( objectGraph, nullable );
				}
			}

			unpacked++;

			return objectGraph;
		}

		private object UnpackSingleValue( Unpacker unpacker, int index )
		{
			if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
			{
				return this._serializers[ index ].UnpackFrom( unpacker );
			}
			else
			{
				using ( var subtreeUnpacker = unpacker.ReadSubtree() )
				{
					return this._serializers[ index ].UnpackFrom( subtreeUnpacker );
				}
			}
		}

		private void UnpackAndAddCollectionItem( object objectGraph, Unpacker unpacker, int index )
		{
			var destination = this._getters[ index ]( objectGraph );
			if ( destination == null )
			{
				throw SerializationExceptions.NewReadOnlyMemberItemsMustNotBeNull( this._contracts[ index ].Name );
			}

			var traits = destination.GetType().GetCollectionTraits();
			if ( traits.AddMethod == null )
			{
				throw SerializationExceptions.NewUnpackToIsNotSupported( destination.GetType(), null );
			}

			var source = this._serializers[ index ].UnpackFrom( unpacker ) as IEnumerable;
			if ( source != null )
			{
				switch ( traits.DetailedCollectionType )
				{
					case CollectionDetailedKind.GenericDictionary:
					{
						// item should be KeyValuePair<TKey, TValue>
						var arguments = new object[ 2 ];
						var key = default( PropertyInfo );
						var value = default( PropertyInfo );
						foreach ( var item in source )
						{
							if ( key == null )
							{
								key = item.GetType().GetProperty( "Key" );
								value = item.GetType().GetProperty( "Value" );
							}

							arguments[ 0 ] = key.GetValue( item, null );
							arguments[ 1 ] = value.GetValue( item, null );
							traits.AddMethod.InvokePreservingExceptionType( destination, arguments );
						}
						break;
					}
					case CollectionDetailedKind.NonGenericDictionary:
					{
						// item should be DictionaryEntry
						var arguments = new object[ 2 ];
						foreach ( var item in source )
						{
							arguments[ 0 ] = DictionaryEntryKeyProperty.GetValue( item, null );
							arguments[ 1 ] = DictionaryEntryValueProperty.GetValue( item, null );
							traits.AddMethod.InvokePreservingExceptionType( destination, arguments );
						}
						break;
					}
					default:
					{
						var arguments = new object[ 1 ];
						foreach ( var item in source )
						{
							arguments[ 0 ] = item;
							traits.AddMethod.InvokePreservingExceptionType( destination, arguments );
						}
						break;
					}
				}
			}
		}
	}
}
