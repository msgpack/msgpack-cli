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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using MsgPack.Serialization.DefaultMarshalers;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Repository of known <see cref="MessageMarshaler{T}"/>s.
	/// </summary>
	public sealed partial class MarshalerRepository
	{
		internal static MethodInfo Get1Method = typeof( MarshalerRepository ).GetMethod( "Get", Type.EmptyTypes );

		private readonly TypeKeyRepository _repository;

		/// <summary>
		///		Initializes a new empty instance of the <see cref="MarshalerRepository"/> class.
		/// </summary>
		public MarshalerRepository()
		{
			this._repository = new TypeKeyRepository();
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="MarshalerRepository"/> class which has copied marshalers.
		/// </summary>
		/// <param name="copiedFrom">The repository which will be copied its contents.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="copiedFrom"/> is <c>null</c>.
		/// </exception>
		public MarshalerRepository( MarshalerRepository copiedFrom )
		{
			if ( copiedFrom == null )
			{
				throw new ArgumentNullException( "copiedFrom" );
			}

			this._repository = new TypeKeyRepository( copiedFrom._repository );
		}

		private MarshalerRepository( Dictionary<RuntimeTypeHandle, object> table )
		{
			this._repository = new TypeKeyRepository( table );
			this._repository.Freeze();
		}

		/// <summary>
		///		Gets the registered <see cref="MessageMarshaler{T}"/> from this repository.
		/// </summary>
		/// <typeparam name="T">Type of the object to be marshaled/unmarshaled.</typeparam>
		/// <returns>
		///		<see cref="MessageMarshaler{T}"/>. If no appropriate mashalers has benn registered, then <c>null</c>.
		/// </returns>
		/// <remarks>
		///		For the <see cref="Enum"/>s, the system always creates the enum marshaler, which marshals enum value as string value.
		///		Or, for the <see cref="Nullable{T}"/>s, the system attempts to create the marshalers using underlying types marhshaler,
		///		but it will fail if the underlying type marshaler is not registered.
		/// </remarks>
		public MessageMarshaler<T> Get<T>( SerializerRepository serializerRepository )
		{
			if ( typeof( T ).IsEnum )
			{
				return new EnumMarshaler<T>();
			}

			if ( typeof( T ).IsGenericType && typeof( T ).GetGenericTypeDefinition() == typeof( Nullable<> ) )
			{
				return new NullableMarshaler<T>();
			}

			return this._repository.Get<T, MessageMarshaler<T>>( this, serializerRepository ?? SerializerRepository.Default );
		}

		/// <summary>
		///		Gets the registered <see cref="ArrayMarshaler{T}"/> from this repository.
		/// </summary>
		/// <typeparam name="T">Type of the object to be marshaled/unmarshaled.</typeparam>
		/// <returns>
		///		<see cref="ArrayMarshaler{T}"/>. If no appropriate mashalers has benn registered, then <c>null</c>.
		/// </returns>
		public ArrayMarshaler<T> GetArrayMarshaler<T>( SerializerRepository serializerRepository )
		{
			var safeSerializerRepository = serializerRepository ?? SerializerRepository.Default;
			var arrayMarshaler = this._repository.Get<T, ArrayMarshaler<T>>( this, safeSerializerRepository );
			if ( arrayMarshaler == null && typeof( T ) != typeof( string ) && typeof( T ) != typeof( MessagePackObject ) && typeof( IEnumerable ).IsAssignableFrom( typeof( T ) ) )
			{
				// TODO: Configurable
				arrayMarshaler = ArrayMarshaler.Create<T>( this, safeSerializerRepository );
				if ( arrayMarshaler != null )
				{
					if ( !this._repository.Register<T>( arrayMarshaler ) )
					{
						arrayMarshaler = this._repository.Get<T, ArrayMarshaler<T>>( this, serializerRepository ?? SerializerRepository.Default );
					}
				}
			}

			return arrayMarshaler;
		}

		/// <summary>
		///		Register <see cref="MessageMarshaler{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of marshaling target.</typeparam>
		/// <param name="marshaler"><see cref="MessageMarshaler{T}"/> instance.</param>
		/// <returns>
		///		<c>true</c> if success to register; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="marshaler"/> is <c>null</c>.
		/// </exception>
		public bool Register<T>( MessageMarshaler<T> marshaler )
		{
			if ( marshaler == null )
			{
				throw new ArgumentNullException( "marshaler" );
			}

			return this._repository.Register<T>( marshaler );
		}

		/// <summary>
		///		Register <see cref="MessageMarshaler{T}"/> type for generic type definition.
		/// </summary>
		/// <param name="marshalerType"><see cref="MessageMarshaler{T}"/> type.</param>
		/// <returns>
		///		<c>true</c> if success to register; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="marshalerType"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="marshalerType"/> is not generic type definition.
		/// </exception>
		/// <remarks>
		///		Registering type is must be following:
		///		<list type="bullet">
		///			<item>It has public default constructor.</item>
		///		</list>
		/// </remarks>
		public bool RegisterMarshalerType( Type marshalerType )
		{
			if ( marshalerType == null )
			{
				throw new ArgumentNullException( "marshalerType" );
			}

			if ( !marshalerType.IsGenericTypeDefinition )
			{
				throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' is not generic type definition.", marshalerType ), "marshalerType" );
			}

			return this._repository.RegisterType( marshalerType );
		}

		private static readonly MarshalerRepository _default = new MarshalerRepository( InitializeDefaultTable() );

		/// <summary>
		///		Gets the system default repository.
		/// </summary>
		/// <value>
		///		The system default repository.
		///		This value will not be <c>null</c>.
		///		Note that the repository is frozen.
		/// </value>
		public static MarshalerRepository Default
		{
			get { return _default; }
		}
	}
}
