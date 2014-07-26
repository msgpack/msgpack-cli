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

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Implements reflection-based object serializer for restricted platforms.
	/// </summary>
	internal class ReflectionObjectMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly MemberInfo[] _getters;
		private readonly MemberInfo[] _setters;
		private readonly DataMemberContract[] _contracts;
		private readonly Dictionary<string, int> _memberIndexes;
		private readonly IMessagePackSerializer[] _serializers;

		public ReflectionObjectMessagePackSerializer( SerializationContext context )
			: base( context )
		{
			ReflectionSerializerHelper.GetMetadata( context, typeof(T), out this._getters, out this._setters, out this._contracts, out this._serializers );
			this._memberIndexes =
				this._contracts
					.Select( ( contract, index ) => new KeyValuePair<string, int>( contract.Name, index ) )
					.ToDictionary( kv => kv.Key, kv => kv.Value );
		}

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
					this._serializers[ i ].PackTo( packer, this._contracts[ i ].Name );
					this.PackMemberValue( packer, objectTree, i );
				}
			}
		}

		private void PackMemberValue( Packer packer, T objectTree, int index )
		{
			var value = GetMemberValue( objectTree, this._getters[ index ] );
			var nilImplication =
				ReflectionNilImplicationHandler.Instance.OnPacking(
					new ReflectionSerializerNilImplicationHandlerParameter(
						this._getters[ index ].GetMemberValueType(),
						this._contracts[ index ].Name ),
					this._contracts[ index ].NilImplication
					);
			if ( nilImplication != null )
			{
				nilImplication( value );
			}
			this._serializers[ index ].PackTo( packer, value );
		}

		private static object GetMemberValue( T objectTree, MemberInfo memberInfo )
		{
			FieldInfo field;
			if ( ( field = memberInfo as FieldInfo ) != null )
			{
				return field.GetValue( objectTree );
			}

			var getter = memberInfo as MethodBase;
#if DEBUG
			Contract.Assert( getter != null );
#endif
			return getter.Invoke( objectTree, null );
		}

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
					this.UnpackMemberValue( result, unpacker, itemsCount, ref unpacked, i );
				}
			}
			else
			{
#if DEBUG
				Contract.Assert( unpacker.IsMapHeader );
#endif
				var itemsCount = UnpackHelpers.GetItemsCount( unpacker );

				for ( int i = 0; i < itemsCount; i++ )
				{
					string name;
					if ( !unpacker.ReadString( out name ) )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					this.UnpackMemberValue( result, unpacker, itemsCount, ref unpacked, this._memberIndexes[ name ] );
				}
			}

			return result;
		}

		private void UnpackMemberValue( T objectGraph, Unpacker unpacker, int itemsCount, ref int unpacked, int index )
		{
			object nullable = null;
			if ( unpacked < itemsCount )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( index );
				}

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

			if ( nullable == null )
			{
				ReflectionNilImplicationHandler.Instance.OnUnpacked(
					new ReflectionSerializerNilImplicationHandlerOnUnpackedParameter(
						this._getters[ index ].GetMemberValueType(),
						value => SetMemverValue( objectGraph, this._setters[ index ], value ),
						this._contracts[ index ].Name,
						this._setters[ index ].DeclaringType
					),
					this._contracts[ index ].NilImplication
				)( null );
			}
			else
			{
				SetMemverValue( objectGraph, this._setters[ index ], nullable );
			}

			unpacked++;
		}

		private static void SetMemverValue( T result, MemberInfo memberInfo, object value )
		{
			FieldInfo field;
			if ( ( field = memberInfo as FieldInfo ) != null )
			{
				field.SetValue( result, value );
			}
			else
			{
				var setter = memberInfo as MethodBase;
#if DEBUG
				Contract.Assert( setter != null );
#endif
				setter.Invoke( result, new[] { value } );
			}
		}
	}


}
