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

namespace MsgPack.Serialization
{
	/// <summary>
	///		<see cref="MessagePackSerializer{T}"/> based on reflection, opt-out based.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class AutoMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		// public bool IsPublicOnly { get; set; }

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
			this._context = new SerializationContext( marshalers ?? MarshalerRepository.Default, serializers ?? SerializerRepository.Default );

			var fastPacking = MarshalerRepository.GetFastMarshalDelegate<T>();
			if ( fastPacking != null )
			{
				var fastUnpacking = MarshalerRepository.GetFastUnmarshalDelegate<T>();
				Contract.Assert( fastUnpacking != null );
				this._packing = ( packer, value, context ) => fastPacking( packer, value );
				this._unpacking = ( unpacker, context ) => fastUnpacking( unpacker );
				return;
			}

			var marshaler = this._context.Marshalers.Get<T>( this._context.Serializers );
			if ( marshaler != null )
			{
				this._packing = ( packer, value, context ) => marshaler.MarshalTo( packer, value );
				this._unpacking = 
					( unpacker, context ) =>
					{
						if ( !unpacker.Read() )
						{
							throw SerializationExceptions.NewUnexpectedEndOfStream();
						}

						return marshaler.UnmarshalFrom( unpacker );
					};
				return;
			}

			var serializer = this._context.Serializers.Get<T>( this._context.Marshalers );
			if ( serializer != null )
			{
				this._packing = ( packer, value, context ) => serializer.PackTo( packer, value );
				this._unpacking =
					( unpacker, context ) =>
					{
						if ( !unpacker.Read() )
						{
							throw SerializationExceptions.NewUnexpectedEndOfStream();
						}

						return serializer.UnpackFrom( unpacker );
					};
				return;
			}

			// TODO: Pluggable
			var builder = new EmittingMemberBinder<T>() { Trace = new StringWriter() };
			if ( !builder.CreateProcedures( SerializationMemberOption.OptOut, out this._packing, out this._unpacking ) )
			{
				Tracer.Emit.TraceData( Tracer.EventType.ILTrace, Tracer.EventId.ILTrace, builder.Trace.ToString() );
				throw SerializationExceptions.NewTypeIsNotSerializable( typeof( T ) );
			}
			Tracer.Emit.TraceData( Tracer.EventType.ILTrace, Tracer.EventId.ILTrace, builder.Trace.ToString() );
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