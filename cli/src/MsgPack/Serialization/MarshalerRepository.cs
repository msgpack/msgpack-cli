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
using System.Diagnostics.Contracts;
using System.Collections;
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

		public MarshalerRepository()
		{
			this._repository = new TypeKeyRepository();
		}

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

		public bool Register<T>( MessageMarshaler<T> marshaler )
		{
			if ( marshaler == null )
			{
				throw new ArgumentNullException( "marshaler" );
			}

			return this._repository.Register<T>( marshaler );
		}

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

		public static MarshalerRepository Default
		{
			get { return _default; }
		}
	}
}
