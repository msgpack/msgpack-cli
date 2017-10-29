#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2016 FUJIWARA, Yusuke and contributors
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
// Contributors:
//    Samuel Cragg
//
#endregion -- License Terms --

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections;
using System.Collections.Generic;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Linq;
using System.Reflection;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Implements reflection-based object serializer for restricted platforms.
	/// </summary>
	[Preserve( AllMembers = true )]
	internal class ReflectionObjectMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly Func<object, object>[] _getters;
		private readonly Action<object, object>[] _setters;
		private readonly MemberInfo[] _memberInfos;
		private readonly DataMemberContract[] _contracts;
		private readonly Dictionary<string, int> _memberIndexes;
		private readonly MessagePackSerializer[] _serializers;
		private readonly ParameterInfo[] _constructorParameters;
		private readonly Dictionary<int, int> _constructorArgumentIndexes;

		public ReflectionObjectMessagePackSerializer( SerializationContext context, SerializationTarget target, SerializerCapabilities capabilities )
			: base( context, capabilities )
		{
			ReflectionSerializerHelper.GetMetadata( typeof( T ), target.Members, context, out this._getters, out this._setters, out this._memberInfos, out this._contracts, out this._serializers );
			this._memberIndexes =
				this._contracts
					.Select( ( contract, index ) => new KeyValuePair<string, int>( contract.Name, index ) )
					.Where( kv => kv.Key != null )
					// Set key as transformed.
					.ToDictionary( kv => context.DictionarySerlaizationOptions.SafeKeyTransformer( kv.Key ), kv => kv.Value );
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
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
				if ( this.OwnerContext.DictionarySerlaizationOptions.OmitNullEntry
#if DEBUG
					&& !SerializerDebugging.UseLegacyNullMapEntryHandling
#endif // DEBUG
				)
				{
					// Skipping causes the entries count header reducing, so count up null entries first.
					var nullCount = 0;
					for ( int i = 0; i < this._serializers.Length; i++ )
					{
						// Set key as transformed.
						if ( this.IsNull( objectTree, i ) )
						{
							nullCount++;
						}
					}

					packer.PackMapHeader( this._serializers.Length - nullCount );
					for ( int i = 0; i < this._serializers.Length; i++ )
					{
						if ( this.IsNull( objectTree, i ) )
						{
							continue;
						}

						// Set key as transformed.
						packer.PackString( this.OwnerContext.DictionarySerlaizationOptions.SafeKeyTransformer( this._contracts[ i ].Name ) );
						this.PackMemberValue( packer, objectTree, i );
					}
				}
				else
				{
					packer.PackMapHeader( this._serializers.Length );
					for ( int i = 0; i < this._serializers.Length; i++ )
					{
						// Set key as transformed.
						packer.PackString( this.OwnerContext.DictionarySerlaizationOptions.SafeKeyTransformer( this._contracts[ i ].Name ) );
						this.PackMemberValue( packer, objectTree, i );
					}
				}
			}
		}

		private bool IsNull( T objectTree, int index )
		{
			if ( this._getters[ index ] == null )
			{
				// missing member should be treated as nil.
				return true;
			}

			return this._getters[ index ]( objectTree ) == null;
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
						this._contracts[ index ].Name
					),
					this._contracts[ index ].NilImplication
				);
			if ( nilImplication != null )
			{
				nilImplication( value );
			}

			this._serializers[ index ].PackTo( packer, value );
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, T objectTree, CancellationToken cancellationToken )
		{
			var asAsyncPackable = objectTree as IAsyncPackable;
			if ( asAsyncPackable != null )
			{
				await asAsyncPackable.PackToMessageAsync( packer, null, cancellationToken ).ConfigureAwait( false );
				return;
			}

			if ( this.OwnerContext.SerializationMethod != SerializationMethod.Map )
			{
				await packer.PackArrayHeaderAsync( this._serializers.Length, cancellationToken ).ConfigureAwait( false );
				for ( int i = 0; i < this._serializers.Length; i++ )
				{
					await this.PackMemberValueAsync( packer, objectTree, i, cancellationToken ).ConfigureAwait( false );
				}
			}
			else
			{
				if ( this.OwnerContext.DictionarySerlaizationOptions.OmitNullEntry
#if DEBUG
					&& !SerializerDebugging.UseLegacyNullMapEntryHandling
#endif // DEBUG
				)
				{
					// Skipping causes the entries count header reducing, so count up null entries first.
					var nullCount = 0;
					for ( int i = 0; i < this._serializers.Length; i++ )
					{
						// Set key as transformed.
						if ( this.IsNull( objectTree, i ) )
						{
							nullCount++;
						}
					}

					await packer.PackMapHeaderAsync( this._serializers.Length - nullCount, cancellationToken ).ConfigureAwait( false );
					for ( int i = 0; i < this._serializers.Length; i++ )
					{
						if ( this.IsNull( objectTree, i ) )
						{
							continue;
						}

						// Set key as transformed.
						await packer.PackStringAsync( this.OwnerContext.DictionarySerlaizationOptions.SafeKeyTransformer( this._contracts[ i ].Name ), cancellationToken ).ConfigureAwait( false );
						await this.PackMemberValueAsync( packer, objectTree, i, cancellationToken ).ConfigureAwait( false );
					}
				}
				else
				{
					await packer.PackMapHeaderAsync( this._serializers.Length, cancellationToken ).ConfigureAwait( false );
					for ( int i = 0; i < this._serializers.Length; i++ )
					{
						// Set key as transformed.
						await packer.PackStringAsync( this.OwnerContext.DictionarySerlaizationOptions.SafeKeyTransformer( this._contracts[ i ].Name ), cancellationToken ).ConfigureAwait( false );
						await this.PackMemberValueAsync( packer, objectTree, i, cancellationToken ).ConfigureAwait( false );
					}
				}
			}
		}

		private async Task PackMemberValueAsync( Packer packer, T objectTree, int index, CancellationToken cancellationToken )
		{
			if ( this._getters[ index ] == null )
			{
				// missing member should be treated as nil.
				await packer.PackNullAsync( cancellationToken ).ConfigureAwait( false );
				return;
			}

			var value = this._getters[ index ]( objectTree );

			var nilImplication =
				ReflectionNilImplicationHandler.Instance.OnPacking(
					new ReflectionSerializerNilImplicationHandlerParameter(
						this._memberInfos[ index ].GetMemberValueType(),
						this._contracts[ index ].Name
					),
					this._contracts[ index ].NilImplication
				);
			if ( nilImplication != null )
			{
				nilImplication( value );
			}

			await this._serializers[ index ].PackToAsync( packer, value, cancellationToken ).ConfigureAwait( false );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
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
					result = this.UnpackMemberValue( result, unpacker, itemsCount, unpacked, i, i );
					unpacked++;
				}
			}
			else
			{
#if DEBUG
				Contract.Assert( unpacker.IsMapHeader, "unpacker.IsMapHeader" );
#endif // DEBUG
				var itemsCount = UnpackHelpers.GetItemsCount( unpacker );

				for ( int i = 0; i < itemsCount; i++ )
				{
					string name;
					if ( !unpacker.ReadString( out name ) )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					if ( name == null )
					{
						// missing member, drain the value and discard it.
						if ( !unpacker.Read() && i < itemsCount - 1 )
						{
							SerializationExceptions.ThrowMissingKey( i + 1, unpacker );
						}
						continue;
					}

					int index;
					if ( !this._memberIndexes.TryGetValue( name, out index ) )
					{
						// key does not exist in the object, skip the associated value
						if ( unpacker.Skip() == null )
						{
							SerializationExceptions.ThrowMissingItem( i, unpacker );
						}
						continue;
					}

					result = this.UnpackMemberValue( result, unpacker, itemsCount, unpacked, index, i );
					unpacked++;
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

		private object UnpackMemberValue( object objectGraph, Unpacker unpacker, int itemsCount, int unpacked, int index, int unpackerOffset )
		{
			object nullable = null;

			var setter = index < this._setters.Length ? this._setters[ index ] : null;
			if ( unpacked < itemsCount )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( unpackerOffset, unpacker );
				}

				if ( !unpacker.LastReadData.IsNil )
				{
					if ( setter != null || this._constructorParameters != null )
					{
						nullable = this.UnpackSingleValue( unpacker, index );
					}
					else if ( index < this._getters.Length && this._getters[ index ] != null ) // null getter supposes undeclared member (should be treated as nil)
					{
						this.UnpackAndAddCollectionItem( objectGraph, unpacker, index );
					}
				}
			}

			if ( this._constructorParameters != null )
			{
#if DEBUG
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

			return objectGraph;
		} // UnpackMemberValue

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

			var traits = destination.GetType().GetCollectionTraits( CollectionTraitOptions.WithAddMethod, this.OwnerContext.CompatibilityOptions.AllowNonCollectionEnumerableTypes );
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
							arguments[ 0 ] = ReflectionSerializerHelper.DictionaryEntryKeyProperty.GetValue( item, null );
							arguments[ 1 ] = ReflectionSerializerHelper.DictionaryEntryValueProperty.GetValue( item, null );
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
		} // UnpackAndAddCollectionItem


#if FEATURE_TAP

		protected internal override async Task<T> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
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

			var asAsyncUnpackable = result as IAsyncUnpackable;
			if ( asAsyncUnpackable != null )
			{
				await asAsyncUnpackable.UnpackFromMessageAsync( unpacker, cancellationToken ).ConfigureAwait( false );
				return ( T )result;
			}

			var asUnpackable = result as IUnpackable;
			if ( asUnpackable != null )
			{
				await Task.Run( () => asUnpackable.UnpackFromMessage( unpacker ), cancellationToken ).ConfigureAwait( false );
				return ( T )result;
			}

			if ( unpacker.IsArrayHeader )
			{
				var itemsCount = UnpackHelpers.GetItemsCount( unpacker );

				for ( int i = 0; i < itemsCount; i++ )
				{
					result = await this.UnpackMemberValueAsync( result, unpacker, itemsCount, unpacked, i, i, cancellationToken ).ConfigureAwait( false );
					unpacked++;
				}
			}
			else
			{
#if DEBUG
				Contract.Assert( unpacker.IsMapHeader, "unpacker.IsMapHeader" );
#endif // DEBUG
				var itemsCount = UnpackHelpers.GetItemsCount( unpacker );

				for ( int i = 0; i < itemsCount; i++ )
				{
					var name = await unpacker.ReadStringAsync( cancellationToken ).ConfigureAwait( false );
					if ( !name.Success )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					if ( name.Value == null )
					{
						// missing member, drain the value and discard it.
						if ( !unpacker.Read() && i < itemsCount - 1 )
						{
							SerializationExceptions.ThrowMissingKey( i + 1, unpacker );
						}
						continue;
					}

					int index;
					if ( !this._memberIndexes.TryGetValue( name.Value, out index ) )
					{
						// key does not exist in the object, skip the associated value
						if ( unpacker.Skip() == null )
						{
							SerializationExceptions.ThrowMissingItem( i, unpacker );
						}
						continue;
					}

					result = await this.UnpackMemberValueAsync( result, unpacker, itemsCount, unpacked, index, i, cancellationToken ).ConfigureAwait( false );
					unpacked++;
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

		private async Task<object> UnpackMemberValueAsync( object objectGraph, Unpacker unpacker, int itemsCount, int unpacked, int index, int unpackerOffset, CancellationToken cancellationToken )
		{
			object nullable = null;

			var setter = index < this._setters.Length ? this._setters[ index ] : null;
			if ( unpacked < itemsCount )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( unpackerOffset, unpacker );
				}

				if ( !unpacker.LastReadData.IsNil )
				{
					if ( setter != null || this._constructorParameters != null )
					{
						nullable = await this.UnpackSingleValueAsync( unpacker, index, cancellationToken ).ConfigureAwait( false );
					}
					else if ( this._getters[ index ] != null ) // null getter supposes undeclared member (should be treated as nil)
					{
						await this.UnpackAndAddCollectionItemAsync( objectGraph, unpacker, index, cancellationToken ).ConfigureAwait( false );
					}
				}
			}

			if ( this._constructorParameters != null )
			{
#if DEBUG
				Contract.Assert( objectGraph is object[], "objectGraph is object[]" );
#endif // DEBUG

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

			return objectGraph;
		} // UnpackMemberValueAsync

		private Task<object> UnpackSingleValueAsync( Unpacker unpacker, int index, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
			{
				return this._serializers[ index ].UnpackFromAsync( unpacker, cancellationToken );
			}
			else
			{
				using ( var subtreeUnpacker = unpacker.ReadSubtree() )
				{
					return this._serializers[ index ].UnpackFromAsync( subtreeUnpacker, cancellationToken );
				}
			}
		}

		private async Task UnpackAndAddCollectionItemAsync( object objectGraph, Unpacker unpacker, int index, CancellationToken cancellationToken )
		{
			var destination = this._getters[ index ]( objectGraph );
			if ( destination == null )
			{
				throw SerializationExceptions.NewReadOnlyMemberItemsMustNotBeNull( this._contracts[ index ].Name );
			}

			var traits = destination.GetType().GetCollectionTraits( CollectionTraitOptions.WithAddMethod, this.OwnerContext.CompatibilityOptions.AllowNonCollectionEnumerableTypes );
			if ( traits.AddMethod == null )
			{
				throw SerializationExceptions.NewUnpackToIsNotSupported( destination.GetType(), null );
			}

			var source = await this._serializers[ index ].UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false ) as IEnumerable;
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
							arguments[ 0 ] = ReflectionSerializerHelper.DictionaryEntryKeyProperty.GetValue( item, null );
							arguments[ 1 ] = ReflectionSerializerHelper.DictionaryEntryValueProperty.GetValue( item, null );
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
		} // UnpackAndAddCollectionItemAsync

#endif // FEATURE_TAP
	}
}
