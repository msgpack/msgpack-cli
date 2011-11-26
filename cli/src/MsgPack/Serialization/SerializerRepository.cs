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
	public sealed partial class SerializerRepository
	{
		internal static MethodInfo Get1Method = typeof( SerializerRepository ).GetMethod( "Get", Type.EmptyTypes );
		internal static MethodInfo Register1Method = typeof( SerializerRepository ).GetMethod( "Register", Type.EmptyTypes );

		// TODO: Unification
		private readonly TypeKeyRepository _repository;
		private readonly TypeKeyRepository _arrayRepository;

		/// <summary>
		/// Initializes a new empty instance of the <see cref="SerializerRepository"/> class.
		/// </summary>
		public SerializerRepository()
		{
			this._repository = new TypeKeyRepository();
			this._arrayRepository = new TypeKeyRepository();
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
			this._arrayRepository = new TypeKeyRepository( copiedFrom._arrayRepository );
		}

		private SerializerRepository( Dictionary<RuntimeTypeHandle, object> table )
		{
			this._repository = new TypeKeyRepository( table );
			this._repository.Freeze();
			this._arrayRepository = new TypeKeyRepository();
			this._arrayRepository.Freeze();
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

			if ( typeof( T ).IsEnum )
			{
				return new EnumMessagePackSerializer<T>();
			}

			if ( typeof( T ).IsGenericType && typeof( T ).GetGenericTypeDefinition() == typeof( Nullable<> ) )
			{
				return new NullableMessagePackSerializer<T>( context );
			}

			return this._repository.Get<T, MessagePackSerializer<T>>( context );
		}

		[Obsolete]
		public MessagePackArraySerializer<T> GetArray<T>( SerializationContext context )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			var arrayMarshaler = this._arrayRepository.Get<T, MessagePackArraySerializer<T>>( context );
			if ( arrayMarshaler == null && typeof( T ) != typeof( string ) && typeof( T ) != typeof( MessagePackObject ) && typeof( IEnumerable ).IsAssignableFrom( typeof( T ) ) )
			{
				// TODO: Configurable
				arrayMarshaler = MessagePackArraySerializer.Create<T>( context );
				if ( arrayMarshaler != null )
				{
					if ( !this._arrayRepository.Register<T>( arrayMarshaler ) )
					{
						arrayMarshaler = this._arrayRepository.Get<T, MessagePackArraySerializer<T>>( context );
					}
				}
			}

			return arrayMarshaler;
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

		/// <summary>
		///		Register <see cref="MessagePackSerializer{T}"/> type for generic type definition.
		/// </summary>
		/// <param name="serializerType"><see cref="MessagePackSerializer{T}"/> type.</param>
		/// <returns>
		///		<c>true</c> if success to register; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="serializerType"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="serializerType"/> is not generic type definition.
		/// </exception>
		/// <remarks>
		///		Registering type is must be following:
		///		<list type="bullet">
		///			<item>It has public constructor which has an argument typed <see cref="SerializationContext"/>.</item>
		///		</list>
		/// </remarks>
		public bool RegisterSerializerType( Type serializerType )
		{
			if ( serializerType == null )
			{
				throw new ArgumentNullException( "serializerType" );
			}

			if ( !serializerType.IsGenericTypeDefinition )
			{
				throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' is not generic type definition.", serializerType ), "serializerType" );
			}

			return this._repository.RegisterType( serializerType );
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