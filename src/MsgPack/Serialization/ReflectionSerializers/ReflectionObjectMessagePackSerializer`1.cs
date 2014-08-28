#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if DEBUG && !UNITY
using System.Diagnostics.Contracts;
#endif // DEBUG && !UNITY
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Implements reflection-based object serializer for restricted platforms.
	/// </summary>
	internal class ReflectionObjectMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly Func<object, object>[] _getters;
		private readonly Action<object, object>[] _setters;
		private readonly MemberInfo[] _memberInfos;
		private readonly DataMemberContract[] _contracts;
		private readonly Dictionary<string, int> _memberIndexes;
		private readonly IMessagePackSerializer[] _serializers;

		public ReflectionObjectMessagePackSerializer( SerializationContext context )
			: base( context )
		{
			ReflectionSerializerHelper.GetMetadata( context, typeof( T ), out this._getters, out this._setters, out this._memberInfos, out this._contracts, out this._serializers );
			this._memberIndexes =
				this._contracts
					.Select( ( contract, index ) => new KeyValuePair<string, int>( contract.Name, index ) )
					.Where( kv => kv.Key != null )
					.ToDictionary( kv => kv.Key, kv => kv.Value );
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
			var result = Activator.CreateInstance<T>();
			var unpacked = 0;

			var asUnpackable = result as IUnpackable;
			if ( asUnpackable != null )
			{
				asUnpackable.UnpackFromMessage( unpacker );
				return result;
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
				Contract.Assert( unpacker.IsMapHeader );
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

					result = this.UnpackMemberValue( result, unpacker, itemsCount, ref unpacked, this._memberIndexes[ name ], i );
				}
			}

			return result;
		}

		private T UnpackMemberValue( T objectGraph, Unpacker unpacker, int itemsCount, ref int unpacked, int index, int unpackerOffset )
		{
			object nullable = null;
			var setter = this._setters[ index ];
			if ( unpacked < itemsCount )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( unpackerOffset );
				}

				if ( !unpacker.LastReadData.IsNil )
				{
					if ( setter != null )
					{
						if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
						{
							nullable = this._serializers[ index ].UnpackFrom( unpacker );
						}
						else
						{
							using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
							{
								nullable = this._serializers[ index ].UnpackFrom( subtreeUnpacker );
							}
						}
					}
					else if ( this._getters[ index ] != null ) // null getter supposes undeclared member (should be treated as nil)
					{
						var collection = this._getters[ index ]( objectGraph );
						if ( collection == null )
						{
							throw SerializationExceptions.NewReadOnlyMemberItemsMustNotBeNull( this._contracts[ index ].Name );
						}
						using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
						{
							this._serializers[ index ].UnpackTo( subtreeUnpacker, collection );
						}
					}
				}
			}

			if ( setter != null )
			{
				if ( nullable == null )
				{
					ReflectionNilImplicationHandler.Instance.OnUnpacked(
						new ReflectionSerializerNilImplicationHandlerOnUnpackedParameter(
							this._memberInfos[ index ].GetMemberValueType(),
							value => SetMemverValue( objectGraph, setter, value ),
							this._contracts[ index ].Name,
							this._memberInfos[ index ].DeclaringType
							),
						this._contracts[ index ].NilImplication
						)( null );
				}
				else
				{
					objectGraph = SetMemverValue( objectGraph, setter, nullable );
				}
			}

			unpacked++;

			return objectGraph;
		}

		private static T SetMemverValue( T result, Action<object, object> setter, object value )
		{
			object boxed = result;
			setter( boxed, value );
			return ( T )boxed;
		}
	}


}
