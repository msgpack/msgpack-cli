#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if !UNITY
using System.Collections.Generic;
#endif // !UNITY
using System.Reflection;

namespace MsgPack.Serialization.DefaultSerializers
{
#if !UNITY
	// ReSharper disable once InconsistentNaming
	internal sealed class System_Collections_Generic_KeyValuePair_2MessagePackSerializer<TKey, TValue> : MessagePackSerializer<KeyValuePair<TKey, TValue>>
	{
		private readonly MessagePackSerializer<TKey> _keySerializer;
		private readonly MessagePackSerializer<TValue> _valueSerializer;

		public System_Collections_Generic_KeyValuePair_2MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext )
		{
			this._keySerializer = ownerContext.GetSerializer<TKey>();
			this._valueSerializer = ownerContext.GetSerializer<TValue>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override void PackToCore( Packer packer, KeyValuePair<TKey, TValue> objectTree )
		{
			packer.PackArrayHeader( 2 );
			this._keySerializer.PackTo( packer, objectTree.Key );
			this._valueSerializer.PackTo( packer, objectTree.Value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override KeyValuePair<TKey, TValue> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.Read() )
			{
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}

			var key = unpacker.LastReadData.IsNil ? default( TKey ) : this._keySerializer.UnpackFrom( unpacker );

			if ( !unpacker.Read() )
			{
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}

			var value = unpacker.LastReadData.IsNil ? default( TValue ) : this._valueSerializer.UnpackFrom( unpacker );

			return new KeyValuePair<TKey, TValue>( key, value );
		}
	}
#else
	// ReSharper disable once InconsistentNaming
	internal sealed class System_Collections_Generic_KeyValuePair_2MessagePackSerializer : NonGenericMessagePackSerializer
	{
		private readonly IMessagePackSingleObjectSerializer _keySerializer;
		private readonly IMessagePackSingleObjectSerializer _valueSerializer;
		private readonly MethodInfo _getKey;
		private readonly MethodInfo _getValue;

		public System_Collections_Generic_KeyValuePair_2MessagePackSerializer( SerializationContext ownerContext, Type targetType )
			: base( ownerContext, targetType )
		{
			var genericArguments = targetType.GetGenericArguments();
			this._keySerializer = ownerContext.GetSerializer( genericArguments[ 0 ] );
			this._valueSerializer = ownerContext.GetSerializer( genericArguments[ 1 ] );
			this._getKey = targetType.GetProperty( "Key" ).GetGetMethod();
			this._getValue = targetType.GetProperty( "Value" ).GetGetMethod();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override void PackToCore( Packer packer, object objectTree )
		{
			packer.PackArrayHeader( 2 );
			this._keySerializer.PackTo( packer, this._getKey.InvokePreservingExceptionType( objectTree ) );
			this._valueSerializer.PackTo( packer, this._getValue.InvokePreservingExceptionType( objectTree ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override object UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.Read() )
			{
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}

			var key =
				unpacker.LastReadData.IsNil ? null : this._keySerializer.UnpackFrom( unpacker );

			if ( !unpacker.Read() )
			{
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}

			var value = unpacker.LastReadData.IsNil ? null : this._valueSerializer.UnpackFrom( unpacker );

			return ReflectionExtensions.CreateInstancePreservingExceptionType( this.TargetType, key, value );
		}
	}
#endif // !UNITY
}
