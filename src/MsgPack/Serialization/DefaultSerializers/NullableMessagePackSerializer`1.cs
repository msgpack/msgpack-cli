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
#if DEBUG && !UNITY
using System.Diagnostics.Contracts;
#endif // DEBUG && !UNITY

namespace MsgPack.Serialization.DefaultSerializers
{
#if !UNITY
	internal class NullableMessagePackSerializer<T> : MessagePackSerializer<T?>
		where T : struct
	{
		private readonly MessagePackSerializer<T> _valueSerializer;

		public NullableMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext )
		{
			this._valueSerializer = ownerContext.GetSerializer<T>();
		}

		public NullableMessagePackSerializer( SerializationContext ownerContext, MessagePackSerializer<T> valueSerializer )
			: base( ownerContext )
		{
			this._valueSerializer = valueSerializer;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override void PackToCore( Packer packer, T? objectTree )
		{
#if DEBUG && !UNITY
			Contract.Assert( objectTree != null, "objectTree != null" );
#endif // DEBUG && !UNITY
			// null was handled in PackTo() method.
			this._valueSerializer.PackToCore( packer, objectTree.Value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override T? UnpackFromCore( Unpacker unpacker )
		{
#if DEBUG && !UNITY
			Contract.Assert( !unpacker.LastReadData.IsNil, "!unpacker.LastReadData.IsNil" );
#endif // DEBUG && !UNITY
			// nil was handled in UnpackFrom() method.
			return this._valueSerializer.UnpackFromCore( unpacker );
		}
	}
#else
	internal class NullableMessagePackSerializer : NonGenericMessagePackSerializer
	{
		private readonly IMessagePackSingleObjectSerializer _valueSerializer;

		public NullableMessagePackSerializer( SerializationContext ownerContext, Type nullableType, Type underlyingType )
			: base( ownerContext, nullableType )
		{
			this._valueSerializer = ownerContext.GetSerializer( underlyingType );
		}

		public NullableMessagePackSerializer( SerializationContext ownerContext, Type nullableType, IMessagePackSingleObjectSerializer valueSerializer )
			: base( ownerContext, nullableType )
		{
			this._valueSerializer = valueSerializer;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override void PackToCore( Packer packer, object objectTree )
		{
			// null was handled in PackTo() method.
			this._valueSerializer.PackTo( packer, objectTree );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override object UnpackFromCore( Unpacker unpacker )
		{
			// nil was handled in UnpackFrom() method.
			return this._valueSerializer.UnpackFrom( unpacker );
		}
	}
#endif // !UNITY
}
