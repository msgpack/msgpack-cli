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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if UNITY
using System.Reflection;
#endif // UNITY

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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override void PackToCore( Packer packer, T? objectTree )
		{
			if ( !objectTree.HasValue )
			{
				packer.PackNull();
			}
			else
			{
				this._valueSerializer.PackToCore( packer, objectTree.Value );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override T? UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.LastReadData.IsNil ? default( T? ) : this._valueSerializer.UnpackFromCore( unpacker );
		}
	}
#else
	internal class NullableMessagePackSerializer : NonGenericMessagePackSerializer
	{
		private readonly MethodInfo _getValue;
		private readonly IMessagePackSingleObjectSerializer _valueSerializer;

		public NullableMessagePackSerializer( SerializationContext ownerContext, Type nullableType, Type underlyingType )
			: base( ownerContext, nullableType )
		{
			this._valueSerializer = ownerContext.GetSerializer( underlyingType );
			this._getValue = nullableType.GetProperty( "Value" ).GetGetMethod();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override void PackToCore( Packer packer, object objectTree )
		{
			if ( objectTree == null )
			{
				packer.PackNull();
			}
			else
			{
				this._valueSerializer.PackTo( packer, this._getValue.SafeInvoke( objectTree ) );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override object UnpackFromCore( Unpacker unpacker )
		{
			return unpacker.LastReadData.IsNil ? null : this._valueSerializer.UnpackFrom( unpacker );
		}
	}
#endif // !UNITY
}
