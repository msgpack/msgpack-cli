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

namespace MsgPack.Serialization
{
	public sealed partial class SerializerRepository
	{
		internal static MethodInfo Get1Method = typeof( SerializerRepository ).GetMethod( "Get", Type.EmptyTypes );

		private readonly TypeKeyRepository _repository;

		public SerializerRepository()
		{
			this._repository = new TypeKeyRepository();
		}

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

		public MessagePackSerializer<T> Get<T>( MarshalerRepository marshalerRepository )
		{
			return this._repository.Get<T, MessagePackSerializer<T>>( marshalerRepository ?? MarshalerRepository.Default, this );
		}

		public bool Register<T>( MessagePackSerializer<T> serializer )
		{
			if ( serializer == null )
			{
				throw new ArgumentNullException( "serializer" );
			}

			return this._repository.Register<T>( serializer );
		}


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

		public static SerializerRepository Default
		{
			get { return _default; }
		}
	}
}