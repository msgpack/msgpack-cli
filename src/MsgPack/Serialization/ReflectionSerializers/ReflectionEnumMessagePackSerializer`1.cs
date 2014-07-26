#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
#if DEBUG && !UNITY_ANDROID && !UNITY_IPHONE
using System.Diagnostics.Contracts;
#endif // DEBUG && !UNITY_ANDROID && !UNITY_IPHONE
using System.Reflection;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Implements reflection-based enum serializer for restricted platforms.
	/// </summary>
	internal class ReflectionEnumMessagePackSerializer<T> : EnumMessagePackSerializer<T>
		where T : struct
	{
		private readonly IMessagePackSerializer _underlyingValueSerializer;
		private readonly Dictionary<T, object> _underlyingValues;
		private readonly Dictionary<object, T> _enumValues;
		private readonly Type _underlyingType;

		public ReflectionEnumMessagePackSerializer( SerializationContext context )
			: base( context, context.EnumSerializationMethod )
		{
			this._underlyingType = Enum.GetUnderlyingType( typeof( T ) );
			this._underlyingValueSerializer = context.GetSerializer( this._underlyingType );
			var enumValues = Enum.GetValues( typeof( T ) );
			this._underlyingValues = new Dictionary<T, object>( enumValues.Length );
			this._enumValues = new Dictionary<object, T>( enumValues.Length );
#if !NETFX_CORE
			var value = typeof( T ).GetField( "value__", BindingFlags.Instance | BindingFlags.NonPublic );
#else
			var value = typeof( T ).GetRuntimeField( "value__" );
#endif
#if DEBUG && !UNITY_ANDROID && !UNITY_IPHONE
			Contract.Assert( value != null );
#endif // DEBUG && !UNITY_ANDROID && !UNITY_IPHONE
			foreach ( var enumValue in enumValues )
			{
				var underlyingValue = value.GetValue( enumValue );
				this._underlyingValues[ ( T )enumValue ] = underlyingValue;
				this._enumValues[ underlyingValue ] = ( T )enumValue;
			}
		}


		protected internal override void PackUnderlyingValueTo( Packer packer, T enumValue )
		{
			this._underlyingValueSerializer.PackTo( packer, this._underlyingValues[ enumValue ] );
		}

		protected internal override T UnpackFromUnderlyingValue( MessagePackObject messagePackObject )
		{
			return this._enumValues[ ReflectionEnumMessagePackSerializer.ToBoxedValue( messagePackObject, this._underlyingType ) ];
		}
	}
}
