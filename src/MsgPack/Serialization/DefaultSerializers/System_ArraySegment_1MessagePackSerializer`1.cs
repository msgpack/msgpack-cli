#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal class System_ArraySegment_1MessagePackSerializer<T> : MessagePackSerializer<ArraySegment<T>>
	{
		private static readonly Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> _packing = InitializePacking();
		private static readonly Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> _unpacking = InitializeUnacking();

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

		private static Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> InitializeUnacking()
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
}
