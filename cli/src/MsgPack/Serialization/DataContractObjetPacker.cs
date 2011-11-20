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
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<see cref="MessagePackSerializer{T}"/> based on opt-in <see cref="DataContractAttribute"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Obsolete]
	public class DataContractObjetPacker<T> : MessagePackSerializer<T>
	{
		private readonly Action<Packer, T, SerializationContext> _packing;
		private readonly Func<Unpacker, SerializationContext, T> _unpacking;
		private readonly SerializationContext _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="AutoMessagePackSerializer&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="marshalers">The marshalers.</param>
		/// <param name="serializers">The serializers.</param>
		public DataContractObjetPacker( MarshalerRepository marshalers, SerializerRepository serializers )
		{
			this._context = new SerializationContext( marshalers, serializers );

			if ( !Attribute.IsDefined( typeof( T ), typeof( DataContractAttribute ) ) )
			{
				throw SerializationExceptions.NewTypeIsNotSerializableBecauesNotDataContract( typeof( T ) );
			}

			// TODO: Pluggable
			var builder = new EmittingMemberBinder<T>();
			if ( !builder.CreateProcedures( SerializationMemberOption.OptIn, out this._packing, out this._unpacking ) )
			{
				throw SerializationExceptions.NewTypeIsNotSerializable( typeof( T ) );
			}
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
