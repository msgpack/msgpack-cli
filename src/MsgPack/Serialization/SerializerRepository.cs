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
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using MsgPack.Serialization.DefaultSerializers;
using System.Collections;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Repository of known <see cref="MessagePackSerializer{T}"/>s.
	/// </summary>
	public sealed partial class SerializerRepository : IDisposable
	{
		private readonly TypeKeyRepository _repository;

		/// <summary>
		/// Initializes a new empty instance of the <see cref="SerializerRepository"/> class.
		/// </summary>
		public SerializerRepository()
		{
			this._repository = new TypeKeyRepository();
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializerRepository"/> class  which has copied serializers.
		/// </summary>
		/// <param name="copiedFrom">The repository which will be copied its contents.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="copiedFrom"/> is <c>null</c>.
		/// </exception>
		public SerializerRepository( SerializerRepository copiedFrom )
		{
			if ( copiedFrom == null )
			{
				throw new ArgumentNullException( "copiedFrom" );
			}

			this._repository = new TypeKeyRepository( copiedFrom._repository );
		}

		private SerializerRepository( Dictionary<RuntimeTypeHandle, object> table )
		{
			this._repository = new TypeKeyRepository( table );
			this._repository.Freeze();
		}

		/// <summary>
		///		Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this._repository.Dispose();
		}

		/// <summary>
		///		Gets the registered <see cref="MessagePackSerializer{T}"/> from this repository.
		/// </summary>
		/// <typeparam name="T">Type of the object to be marshaled/unmarshaled.</typeparam>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>. If no appropriate mashalers has benn registered, then <c>null</c>.
		/// </returns>
		public MessagePackSerializer<T> Get<T>( SerializationContext context )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			if ( typeof( T ).GetIsEnum() )
			{
				return new EnumMessagePackSerializer<T>();
			}

			if ( typeof( T ).GetIsGenericType() && typeof( T ).GetGenericTypeDefinition() == typeof( Nullable<> ) )
			{
				return new NullableMessagePackSerializer<T>( context );
			}

			return this._repository.Get<T, MessagePackSerializer<T>>( context );
		}

		/// <summary>
		///		Register <see cref="MessagePackSerializer{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of serialization target.</typeparam>
		/// <param name="serializer"><see cref="MessagePackSerializer{T}"/> instance.</param>
		/// <returns>
		///		<c>true</c> if success to register; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="serializer"/> is <c>null</c>.
		/// </exception>
		public bool Register<T>( MessagePackSerializer<T> serializer )
		{
			if ( serializer == null )
			{
				throw new ArgumentNullException( "serializer" );
			}

			return this._repository.Register<T>( serializer );
		}

		private static readonly SerializerRepository _default = new SerializerRepository( InitializeDefaultTable() );

		/// <summary>
		///		Gets the system default repository.
		/// </summary>
		/// <value>
		///		The system default repository.
		///		This value will not be <c>null</c>.
		///		Note that the repository is frozen.
		/// </value>
		public static SerializerRepository Default
		{
			get { return _default; }
		}
	}
}