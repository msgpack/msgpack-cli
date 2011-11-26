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
		private readonly MessagePackSerializer<T> _underlying;

		/// <summary>
		///		Initializes a new instance of the <see cref="AutoMessagePackSerializer&lt;T&gt;"/> class.
		/// </summary>
		public AutoMessagePackSerializer( SerializationContext context )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			if ( ( typeof( T ).Assembly == typeof( object ).Assembly || typeof( T ).Assembly == typeof( Enumerable ).Assembly )
				&& typeof( T ).IsPublic && typeof( T ).Name.StartsWith( "Tuple`" ) )
			{
				throw new NotImplementedException( "Tuple is not supported yet." );
			}

			var serializer = context.Serializers.Get<T>( context );
			if ( serializer != null )
			{
				this._underlying = serializer;
				return;
			}

			var traits = typeof( T ).GetCollectionTraits();
			switch ( traits.CollectionType )
			{
				case CollectionKind.Array:
				{
					serializer = new EmittingSerializerBuilder<T>( context ).CreateArraySerializer();
					break;
				}
				case CollectionKind.Map:
				{
					serializer = new EmittingSerializerBuilder<T>( context ).CreateMapSerializer();
					break;
				}
				case CollectionKind.NotCollection:
				{
					serializer = new EmittingSerializerBuilder<T>( context ).CreateSerializer( Attribute.IsDefined( typeof( T ), typeof( DataContractAttribute ) ) ? SerializationMemberOption.OptIn : SerializationMemberOption.OptOut );
					break;
				}
			}

			if ( serializer != null )
			{
				if ( !context.Serializers.Register<T>( serializer ) )
				{
					serializer = context.Serializers.Get<T>( context );
					Contract.Assert( serializer != null );
				}

				this._underlying = serializer;
				return;
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
			this._underlying.PackTo( packer, objectTree );
		}

		/// <summary>
		///		Deserialize object with specified <see cref="Unpacker"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <returns>Deserialized object.</returns>
		protected sealed override T UnpackFromCore( Unpacker unpacker )
		{
			return this._underlying.UnpackFrom( unpacker );
		}

		/// <summary>
		///		Deserialize collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <param name="collection">Collection that the items to be stored. This value will not be <c>null</c>.</param>
		/// <exception cref="SerializationException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not collection.
		/// </exception>
		protected sealed override void UnpackToCore( Unpacker unpacker, T collection )
		{
			this._underlying.UnpackTo( unpacker, collection );
		}
	}
}