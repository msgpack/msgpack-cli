#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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
using System.Runtime.InteropServices.ComTypes;

using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization
{
	/// <summary>
	/// 	<see cref="BuiltInSerializerProvider" /> for this assembly.
	/// </summary>
	internal sealed class DefaultBuiltInSerializerProvider : BuiltInSerializerProvider
	{
		private readonly Dictionary<RuntimeTypeHandle, Func<SerializationContext, Object>> _customFactories =
			new Dictionary<RuntimeTypeHandle, Func<SerializationContext, Object>>( 6 )
			{
				{ typeof( DateTime ).TypeHandle,		ownerContext => new DateTimeMessagePackSerializerProvider( ownerContext, false ) },
				{ typeof( DateTimeOffset ).TypeHandle,	ownerContext => new DateTimeOffsetMessagePackSerializerProvider( ownerContext, false ) },
				{ typeof( FILETIME ).TypeHandle,		ownerContext => new FileTimeMessagePackSerializerProvider( ownerContext, false ) },
				{ typeof( DateTime? ).TypeHandle,		ownerContext => new DateTimeMessagePackSerializerProvider( ownerContext, true ) },
				{ typeof( DateTimeOffset? ).TypeHandle,	ownerContext => new DateTimeOffsetMessagePackSerializerProvider( ownerContext, true ) },
				{ typeof( FILETIME? ).TypeHandle,		ownerContext => new FileTimeMessagePackSerializerProvider( ownerContext, true ) }
			};

		public DefaultBuiltInSerializerProvider() { }

		protected override object GetSerializer( SerializationContext context, Type serializerType )
		{
			Func<SerializationContext, Object> customFactory;
			if ( this._customFactories.TryGetValue( serializerType.TypeHandle, out customFactory ) )
			{
				return customFactory( context );
			}

			return base.GetSerializer( context, serializerType );
		}
	}
}
