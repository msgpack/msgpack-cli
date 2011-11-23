#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.IO;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Linq;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<see cref="MessagePackSerializer{T}"/> based on reflection, opt-out based.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class AutoMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly Action<Packer, T, SerializationContext> _packing;
		private readonly Func<Unpacker, SerializationContext, T> _unpacking;
		private readonly SerializationContext _context;

		/// <summary>
		///		Initializes a new instance of the <see cref="AutoMessagePackSerializer&lt;T&gt;"/> class.
		/// </summary>
		public AutoMessagePackSerializer()
			: this( null, null ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="AutoMessagePackSerializer&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="marshalers">The marshalers.</param>
		/// <param name="serializers">The serializers.</param>
		public AutoMessagePackSerializer( MarshalerRepository marshalers, SerializerRepository serializers )
		{
			if ( ( typeof( T ).Assembly == typeof( object ).Assembly || typeof( T ).Assembly == typeof( Enumerable ).Assembly )
				&& typeof( T ).IsPublic && typeof( T ).Name.StartsWith( "Tuple`" ) )
			{
				throw new NotImplementedException( "Tuple is not supported yet." );
			}

			this._context = new SerializationContext( marshalers ?? new MarshalerRepository( MarshalerRepository.Default ), serializers ?? new SerializerRepository( SerializerRepository.Default ) );

			var marshaler = this._context.Marshalers.Get<T>( this._context.Serializers );
			if ( marshaler != null )
			{
				this._packing = Closures.Pack<T>( marshaler.MarshalTo );
				this._unpacking = Closures.UnpackWithForwarding( marshaler.UnmarshalFrom );
				return;
			}

			var serializer = this._context.Serializers.Get<T>( this._context.Marshalers );
			if ( serializer != null )
			{
				this._packing = Closures.Pack<T>( serializer.PackTo );
				this._unpacking = Closures.UnpackWithForwarding( serializer.UnpackFrom );
				return;
			}

			var traits = typeof( T ).GetCollectionTraits();
			switch ( traits.CollectionType )
			{
				case CollectionKind.Array:
				{
					var arrayMarshaler = this._context.Marshalers.GetArrayMarshaler<T>( this._context.Serializers );
					if ( arrayMarshaler != null )
					{
						this._packing = arrayMarshaler.MarshalTo;
						this._unpacking = Closures.UnpackWithForwarding<T>( arrayMarshaler.UnmarshalTo );
						return;
					}

					break;
				}
				case CollectionKind.Map:
				{
					// TODO: Pluggable
					var builder = new EmittingMemberBinder<T>();
					if ( !builder.CreateMapProcedures( traits, out this._packing, out this._unpacking ) )
					{
						break;
					}

					return;
				}
				case CollectionKind.NotCollection:
				{
					// TODO: Pluggable
					var builder = new EmittingMemberBinder<T>();
					if ( !builder.CreateProcedures( Attribute.IsDefined( typeof( T ), typeof( DataContractAttribute ) ) ? SerializationMemberOption.OptIn : SerializationMemberOption.OptOut, out this._packing, out this._unpacking ) )
					{
						break;
					}

					return;
				}
			}

			throw SerializationExceptions.NewTypeCannotSerialize( typeof( T ) );
		}
		/// <summary>
		///		Serialize specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		protected sealed override void PackToCore( Packer packer, T objectTree )
		{
			this._packing( packer, objectTree, this._context );
		}

		/// <summary>
		///		Deserialize object with specified <see cref="Unpacker"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <returns>Deserialized object.</returns>
		protected sealed override T UnpackFromCore( Unpacker unpacker )
		{
			return this._unpacking( unpacker, this._context );
		}
	}
}