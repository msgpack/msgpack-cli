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

namespace MsgPack.Serialization.DefaultSerializers
{
#if !UNITY
	// ReSharper disable once InconsistentNaming
	internal class System_ArraySegment_1MessagePackSerializer<T> : MessagePackSerializer<ArraySegment<T>>
	{
		private static readonly Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> _packing = InitializePacking();
		private static readonly Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> _unpacking = InitializeUnpacking();

		private static Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> InitializePacking()
		{
			if ( typeof( T ) == typeof( byte ) )
			{
				return
					new Action<Packer, ArraySegment<byte>, MessagePackSerializer<byte>>(
						ArraySegmentMessageSerializer.PackByteArraySegmentTo
					) as Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>;
			}
			else if ( typeof( T ) == typeof( char ) )
			{
				return
					new Action<Packer, ArraySegment<char>, MessagePackSerializer<char>>(
						ArraySegmentMessageSerializer.PackCharArraySegmentTo
					) as Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>;
			}
			else
			{
				return ArraySegmentMessageSerializer.PackGenericArraySegmentTo;
			}
		}

		private static Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> InitializeUnpacking()
		{
			if ( typeof( T ) == typeof( byte ) )
			{
				return
					new Func<Unpacker, MessagePackSerializer<byte>, ArraySegment<byte>>(
							ArraySegmentMessageSerializer.UnpackByteArraySegmentFrom
						) as Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>;
			}
			else if ( typeof( T ) == typeof( char ) )
			{
				return
					new Func<Unpacker, MessagePackSerializer<char>, ArraySegment<char>>(
							ArraySegmentMessageSerializer.UnpackCharArraySegmentFrom
						) as Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>;
			}
			else
			{
				return ArraySegmentMessageSerializer.UnpackGenericArraySegmentFrom;
			}
		}

		private readonly MessagePackSerializer<T> _itemSerializer;

		public System_ArraySegment_1MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext )
		{
			this._itemSerializer = ownerContext.GetSerializer<T>();
		}

		protected internal sealed override void PackToCore( Packer packer, ArraySegment<T> objectTree )
		{
			_packing( packer, objectTree, this._itemSerializer );
		}

		protected internal sealed override ArraySegment<T> UnpackFromCore( Unpacker unpacker )
		{
			return _unpacking( unpacker, this._itemSerializer );
		}
	}
#else
	// ReSharper disable once InconsistentNaming
	internal class System_ArraySegment_1MessagePackSerializer : NonGenericMessagePackSerializer
	{
		private readonly Type _elementType;
		private readonly IMessagePackSingleObjectSerializer _itemSerializer;
		private readonly Action<Packer, object, IMessagePackSingleObjectSerializer> _packing;
		private readonly Func<Unpacker, Type, IMessagePackSingleObjectSerializer, object> _unpacking;
		
		public System_ArraySegment_1MessagePackSerializer( SerializationContext ownerContext, Type targetType )
			: base( ownerContext, targetType )
		{
			var elementType = targetType.GetGenericArguments()[ 0 ];
			this._elementType = elementType;
			this._itemSerializer = ownerContext.GetSerializer( elementType );
			this._packing = InitializePacking( elementType );
			this._unpacking = InitializeUnpacking( elementType );
		}

		private static Action<Packer, object, IMessagePackSingleObjectSerializer> InitializePacking( Type elementType )
		{
			if ( elementType == typeof( byte ) )
			{
				return ArraySegmentMessageSerializer.PackByteArraySegmentTo;
			}
			else if ( elementType == typeof( char ) )
			{
				return ArraySegmentMessageSerializer.PackCharArraySegmentTo;
			}
			else
			{
				return ArraySegmentMessageSerializer.PackGenericArraySegmentTo;
			}
		}

		private static Func<Unpacker, Type, IMessagePackSingleObjectSerializer, object> InitializeUnpacking( Type elementType )
		{
			if ( elementType == typeof( byte ) )
			{
				return ArraySegmentMessageSerializer.UnpackByteArraySegmentFrom;
			}
			else if ( elementType == typeof( char ) )
			{
				return ArraySegmentMessageSerializer.UnpackCharArraySegmentFrom;
			}
			else
			{
				return ArraySegmentMessageSerializer.UnpackGenericArraySegmentFrom;
			}
		}

		protected internal sealed override void PackToCore( Packer packer, object objectTree )
		{
			this._packing( packer, objectTree, this._itemSerializer );
		}

		protected internal sealed override object UnpackFromCore( Unpacker unpacker )
		{
			return this._unpacking( unpacker, this._elementType, this._itemSerializer );
		}
	}
#endif // !UNITY
}
