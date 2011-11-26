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
using System.Reflection;
using System.ComponentModel;
using System.Collections;

namespace MsgPack.Serialization
{
	// FIXME: MsgPack.Serialization.Serializers namespace which contains types for custom serializer 
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Represents serialization context information for internal serialization logic.
	/// </summary>
	public sealed class SerializationContext
	{
		[Obsolete]
		internal static readonly MethodInfo MarshalTo1Method = typeof( SerializationContext ).GetMethod( "MarshalTo" );
		[Obsolete]
		internal static readonly MethodInfo UnmarshalFrom1Method = typeof( SerializationContext ).GetMethod( "UnmarshalFrom" );
		[Obsolete]
		internal static readonly MethodInfo MarshalArrayTo1Method = typeof( SerializationContext ).GetMethod( "MarshalArrayTo" );
		[Obsolete]
		internal static readonly MethodInfo UnmarshalArrayTo1Method = typeof( SerializationContext ).GetMethod( "UnmarshalArrayTo" );

		private static readonly SerializationContext _default = new SerializationContext( SerializerRepository.Default );

		private readonly SerializerRepository _serializers;

		/// <summary>
		///		Gets the current <see cref="SerializerRepository"/>.
		/// </summary>
		/// <value>
		///		The  current <see cref="SerializerRepository"/>.
		/// </value>
		public SerializerRepository Serializers
		{
			get { return this._serializers; }
		}

		public SerializationContext()
			: this( new SerializerRepository( SerializerRepository.Default ) )
		{

		}

		internal SerializationContext( SerializerRepository serializers )
		{
			this._serializers = serializers;
		}

		/// <summary>
		///		Marshals specified value to the packer.
		/// </summary>
		/// <typeparam name="T">Type of <paramref name="value"/>.</typeparam>
		/// <param name="packer">The packer to be passed marshaled <paramref name="value"/>.</param>
		/// <param name="value">The value to be marshaled.</param>
		[Obsolete]
		public void MarshalTo<T>( Packer packer, T value )
		{
			var serializer = this._serializers.Get<T>( this );
			if ( serializer == null )
			{
				var arraySerializer = this._serializers.GetArray<T>( this );
				if ( arraySerializer != null )
				{
					arraySerializer.MarshalTo( packer, value, this );
					return;
				}

				// TODO: Configurable
				serializer = new AutoMessagePackSerializer<T>( this );
				if ( !this._serializers.Register<T>( serializer ) )
				{
					serializer = this._serializers.Get<T>( this );
				}
			}

			if ( serializer != null )
			{
				serializer.PackTo( packer, value );
				return;
			}

			throw SerializationExceptions.NewTypeCannotSerialize( typeof( T ) );
		}

		[Obsolete]
		public void MarshalArrayTo<TCollection>( Packer packer, TCollection collection )
		{
			var arrayMarshaler = this._serializers.GetArray<TCollection>( this );
			if ( arrayMarshaler != null )
			{
				arrayMarshaler.MarshalTo( packer, collection, this );
				return;
			}

			throw SerializationExceptions.NewTypeCannotSerialize( typeof( TCollection ) );
		}

		/// <summary>
		///		Unmarshals from specified <paramref name="unpacker"/>.
		/// </summary>
		/// <typeparam name="T">Type of the unmarshaled value.</typeparam>
		/// <param name="unpacker">The unpacker to be queried unmarshaling value via <see cref="P:Unpacker.Data"/> property.</param>
		/// <returns>Unmarshaled value.</returns>
		[Obsolete]
		public T UnmarshalFrom<T>( Unpacker unpacker )
		{
			var serializer = this._serializers.Get<T>( this );
			if ( serializer == null )
			{

				var arrayMarshaler = this._serializers.GetArray<T>( this );
				if ( arrayMarshaler != null )
				{
					// FIXME: Unify
					T collection = typeof( T ).IsArray ? ( T )( object )Array.CreateInstance( typeof( T ).GetElementType(), unpacker.Data.Value.AsInt32() ) : Activator.CreateInstance<T>();
					arrayMarshaler.UnmarshalTo( unpacker, collection, this );
					return collection;
				}

				// TODO: Configurable
				serializer = new AutoMessagePackSerializer<T>( this );
				if ( !this._serializers.Register<T>( serializer ) )
				{
					serializer = this._serializers.Get<T>( this );
				}
			}

			if ( serializer != null )
			{
				return serializer.UnpackFrom( unpacker );
			}

			throw SerializationExceptions.NewTypeCannotDeserialize( typeof( T ) );
		}

		[Obsolete]
		public void UnmarshalArrayTo<TCollection>( Unpacker unpacker, TCollection collection )
		{
			var arrayMarshaler = this._serializers.GetArray<TCollection>( this );
			if ( arrayMarshaler != null )
			{
				arrayMarshaler.UnmarshalTo( unpacker, collection, this );
				return;
			}

			throw SerializationExceptions.NewTypeCannotDeserialize( typeof( TCollection ) );
		}

		public MessagePackSerializer<T> Get<T>()
		{
			var serializer = this._serializers.Get<T>( this );
			if ( serializer == null )
			{
				// FIXME: Unify
				var arrayMarshaler = this._serializers.GetArray<T>( this );
				if ( arrayMarshaler != null )
				{
					return new ShimArraySerializer<T>( arrayMarshaler, this );
				}

				// TODO: Configurable
				serializer = new AutoMessagePackSerializer<T>( this );
				if ( !this._serializers.Register<T>( serializer ) )
				{
					serializer = this._serializers.Get<T>( this );
				}
			}

			return serializer;
		}

		[Obsolete( "Unify to Register" )]
		public MessagePackSerializer<T> GetArray<T>()
		{
			// FIXME: Unify
			var arrayMarshaler = this._serializers.GetArray<T>( this );
			if ( arrayMarshaler != null )
			{
				return new ShimArraySerializer<T>( arrayMarshaler, this );
			}

			return null;
		}

	}

	[Obsolete]
	internal sealed class ShimArraySerializer<T> : MessagePackSerializer<T>
	{
		private readonly MessagePackArraySerializer<T> _underying;
		[Obsolete]
		private readonly SerializationContext _context;

		public ShimArraySerializer( SerializationContext context )
		{
			this._underying = context.Serializers.GetArray<T>( context );
			this._context = context;
		}

		public ShimArraySerializer( MessagePackArraySerializer<T> arrayMarshaler, SerializationContext serializationContext )
		{
			this._underying = arrayMarshaler;
			this._context = serializationContext;
		}

		protected override void PackToCore( Packer packer, T objectTree )
		{
			this._underying.MarshalTo( packer, objectTree, this._context );
		}

		protected override T UnpackFromCore( Unpacker unpacker )
		{
			T collection = typeof( T ).IsArray ? ( T )( object )Array.CreateInstance( typeof( T ).GetElementType(), unpacker.Data.Value.AsUInt32() ) : Closures.Construct<T>()();
			this._underying.UnmarshalTo( unpacker, collection, this._context );
			return collection;
		}

		protected override void UnpackToCore( Unpacker unpacker, T collection )
		{
			this._underying.UnmarshalTo( unpacker, collection, this._context );
		}
	}
}
