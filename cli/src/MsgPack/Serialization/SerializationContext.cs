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
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Represents serialization context information for internal serialization logic.
	/// </summary>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public sealed class SerializationContext
	{
		internal static readonly MethodInfo MarshalTo1Method = typeof( SerializationContext ).GetMethod( "MarshalTo" );
		internal static readonly MethodInfo UnmarshalFrom1Method = typeof( SerializationContext ).GetMethod( "UnmarshalFrom" );
		internal static readonly MethodInfo MarshalArrayTo1Method = typeof( SerializationContext ).GetMethod( "MarshalArrayTo" );
		internal static readonly MethodInfo UnmarshalArrayTo1Method = typeof( SerializationContext ).GetMethod( "UnmarshalArrayTo" );

		private readonly MarshalerRepository _marshalers;

		/// <summary>
		///		Gets the current <see cref="MarshalerRepository"/>.
		/// </summary>
		/// <value>
		///		The  current <see cref="MarshalerRepository"/>.
		/// </value>
		public MarshalerRepository Marshalers
		{
			get { return this._marshalers; }
		}

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

		internal SerializationContext( MarshalerRepository marshalers, SerializerRepository serializers )
		{
			this._marshalers = marshalers;
			this._serializers = serializers;
		}

		/// <summary>
		///		Marshals specified value to the packer.
		/// </summary>
		/// <typeparam name="T">Type of <paramref name="value"/>.</typeparam>
		/// <param name="packer">The packer to be passed marshaled <paramref name="value"/>.</param>
		/// <param name="value">The value to be marshaled.</param>
		public void MarshalTo<T>( Packer packer, T value )
		{
			var marshaler = this._marshalers.Get<T>( this._serializers );
			if ( marshaler != null )
			{
				marshaler.MarshalTo( packer, value );
				return;
			}

			var serializer = this._serializers.Get<T>( this._marshalers );
			if ( serializer != null )
			{
				serializer.PackTo( packer, value );
				return;
			}

			throw SerializationExceptions.NewValueCannotMarshal( typeof( T ) );
		}

		public void MarshalArrayTo<TCollection>( Packer packer, TCollection collection )
		{
			var arrayMarshaler = this._marshalers.GetArrayMarshaler<TCollection>( this._serializers );
			if ( arrayMarshaler != null )
			{
				arrayMarshaler.MarshalTo( packer, collection, this );
				return;
			}

			throw SerializationExceptions.NewValueCannotMarshal( typeof( TCollection ) );
		}

		/// <summary>
		///		Unmarshals from specified <paramref name="unpacker"/>.
		/// </summary>
		/// <typeparam name="T">Type of the unmarshaled value.</typeparam>
		/// <param name="unpacker">The unpacker to be queried unmarshaling value via <see cref="P:Unpacker.Data"/> property.</param>
		/// <returns>Unmarshaled value.</returns>
		public T UnmarshalFrom<T>( Unpacker unpacker )
		{
			var marshaler = this._marshalers.Get<T>( this._serializers );
			if ( marshaler != null )
			{
				return marshaler.UnmarshalFrom( unpacker );
			}

			var serializer = this._serializers.Get<T>( this._marshalers );
			if ( serializer != null )
			{
				return serializer.UnpackFrom( unpacker );
			}

			throw SerializationExceptions.NewValueCannotUnmarshal( typeof( T ) );
		}


		public void UnmarshalArrayTo<TCollection>( Unpacker unpacker, TCollection collection )
		{
			var arrayMarshaler = this._marshalers.GetArrayMarshaler<TCollection>( this._serializers );
			if ( arrayMarshaler != null )
			{
				arrayMarshaler.UnmarshalTo( unpacker, collection, this );
				return;
			}

			throw SerializationExceptions.NewValueCannotUnmarshal( typeof( TCollection ) );
		}
	}
}
