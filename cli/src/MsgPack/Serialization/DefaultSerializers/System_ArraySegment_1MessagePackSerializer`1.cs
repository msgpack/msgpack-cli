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

namespace MsgPack.Serialization.DefaultSerializers
{
	internal class System_ArraySegment_1MessagePackSerializer<T> : MessagePackSerializer<ArraySegment<T>>
	{
		private static readonly Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> _packing;
		private static readonly Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> _unpacking;

		static System_ArraySegment_1MessagePackSerializer()
		{
			if ( typeof( T ) == typeof( byte ) )
			{
				_packing =
					Delegate.CreateDelegate(
						typeof( Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> ),
						ArraySegmentMessageSerializer.PackByteArraySegmentToMethod
					) as Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>;
				_unpacking =
					Delegate.CreateDelegate(
						typeof( Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> ),
						ArraySegmentMessageSerializer.UnpackByteArraySegmentFromMethod
					) as Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>;
			}
			else if ( typeof( T ) == typeof( char ) )
			{
				_packing = Delegate.CreateDelegate(
					typeof( Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> ),
					ArraySegmentMessageSerializer.PackCharArraySegmentToMethod
				) as Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>;
				_unpacking = Delegate.CreateDelegate(
					typeof( Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> ),
					ArraySegmentMessageSerializer.UnpackCharArraySegmentFromMethod
				) as Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>;
			}
			else
			{
				_packing =
					Delegate.CreateDelegate(
						typeof( Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> ),
						ArraySegmentMessageSerializer.PackGenericArraySegmentTo1Method.MakeGenericMethod( typeof( T ) )
					) as Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>;
				_unpacking =
					Delegate.CreateDelegate(
						typeof( Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> ),
						ArraySegmentMessageSerializer.UnpackGenericArraySegmentFrom1Method.MakeGenericMethod( typeof( T ) )
					) as Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>;
			}
		}

		private readonly MessagePackSerializer<T> _itemSerializer;

		public System_ArraySegment_1MessagePackSerializer( SerializationContext context )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			this._itemSerializer = context.GetSerializer<T>();
		}

		protected sealed override void PackToCore( Packer packer, ArraySegment<T> objectTree )
		{
			_packing( packer, objectTree, this._itemSerializer );
		}

		protected sealed override ArraySegment<T> UnpackFromCore( Unpacker unpacker )
		{
			return _unpacking( unpacker, this._itemSerializer );
		}
	}
}
