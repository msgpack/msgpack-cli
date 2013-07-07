#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;
using System.Linq;
#if NETFX_CORE
using System.Reflection;
#endif
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<see cref="MessagePackSerializer{T}"/> based on reflection, opt-out based.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class AutoMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly MessagePackSerializer<T> _underlying;

		/// <summary>
		///		Initializes a new instance of the <see cref="AutoMessagePackSerializer&lt;T&gt;"/> class.
		/// </summary>
		public AutoMessagePackSerializer( SerializationContext context, Func<SerializationContext, SerializerBuilder<T>> builderProvider )
			:base( context.CompatibilityOptions.PackerCompatibilityOptions )
		{
			Contract.Assert( context != null );

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
					serializer = builderProvider( context ).CreateArraySerializer();
					break;
				}
				case CollectionKind.Map:
				{
					serializer = builderProvider( context ).CreateMapSerializer();
					break;
				}
				case CollectionKind.NotCollection:
				{
					if ( ( typeof( T ).GetAssembly().Equals( typeof( object ).GetAssembly() ) || typeof( T ).GetAssembly().Equals( typeof( Enumerable ).GetAssembly() ) )
						&& typeof( T ).GetIsPublic() && typeof( T ).Name.StartsWith( "Tuple`", StringComparison.Ordinal ) )
					{
						serializer = builderProvider( context ).CreateTupleSerializer();
					}
					else
					{
						serializer = builderProvider( context ).CreateSerializer();
					}
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
		protected internal sealed override void PackToCore( Packer packer, T objectTree )
		{
			this._underlying.PackTo( packer, objectTree );
		}

		/// <summary>
		///		Deserialize object with specified <see cref="Unpacker"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <returns>Deserialized object.</returns>
		protected internal sealed override T UnpackFromCore( Unpacker unpacker )
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
		protected internal sealed override void UnpackToCore( Unpacker unpacker, T collection )
		{
			this._underlying.UnpackTo( unpacker, collection );
		}

		public override string ToString()
		{
			return this._underlying.ToString();
		}
	}
}