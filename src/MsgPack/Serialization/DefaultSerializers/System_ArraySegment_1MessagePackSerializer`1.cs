#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	[Preserve( AllMembers = true )]
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

#if FEATURE_TAP

		private static readonly Func<Packer, ArraySegment<T>, MessagePackSerializer<T>, CancellationToken, Task> _asyncPacking = InitializeAsyncPacking();
		private static readonly Func<Unpacker, MessagePackSerializer<T>, CancellationToken, Task<ArraySegment<T>>> _asyncUnpacking = InitializeAsyncUnpacking();

		private static Func<Packer, ArraySegment<T>, MessagePackSerializer<T>, CancellationToken, Task> InitializeAsyncPacking()
		{
			if ( typeof( T ) == typeof( byte ) )
			{
				return
					new Func<Packer, ArraySegment<byte>, MessagePackSerializer<byte>, CancellationToken, Task>(
						ArraySegmentMessageSerializer.PackByteArraySegmentToAsync
					) as Func<Packer, ArraySegment<T>, MessagePackSerializer<T>, CancellationToken, Task>;
			}
			else if ( typeof( T ) == typeof( char ) )
			{
				return
					new Func<Packer, ArraySegment<char>, MessagePackSerializer<char>, CancellationToken, Task>(
						ArraySegmentMessageSerializer.PackCharArraySegmentToAsync
					) as Func<Packer, ArraySegment<T>, MessagePackSerializer<T>, CancellationToken, Task>;
			}
			else
			{
				return ArraySegmentMessageSerializer.PackGenericArraySegmentToAsync;
			}
		}

		private static Func<Unpacker, MessagePackSerializer<T>, CancellationToken, Task<ArraySegment<T>>> InitializeAsyncUnpacking()
		{
			if ( typeof( T ) == typeof( byte ) )
			{
				return
					new Func<Unpacker, MessagePackSerializer<byte>, CancellationToken, Task<ArraySegment<byte>>>(
							ArraySegmentMessageSerializer.UnpackByteArraySegmentFromAsync
						) as Func<Unpacker, MessagePackSerializer<T>, CancellationToken, Task<ArraySegment<T>>>;
			}
			else if ( typeof( T ) == typeof( char ) )
			{
				return
					new Func<Unpacker, MessagePackSerializer<char>, CancellationToken, Task<ArraySegment<char>>>(
							ArraySegmentMessageSerializer.UnpackCharArraySegmentFromAsync
						) as Func<Unpacker, MessagePackSerializer<T>, CancellationToken, Task<ArraySegment<T>>>;
			}
			else
			{
				return ArraySegmentMessageSerializer.UnpackGenericArraySegmentFromAsync;
			}
		}

#endif // FEATURE_TAP

		private readonly MessagePackSerializer<T> _itemSerializer;

		public System_ArraySegment_1MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
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

#if FEATURE_TAP

		protected internal override Task PackToAsyncCore( Packer packer, ArraySegment<T> objectTree, CancellationToken cancellationToken )
		{
			return _asyncPacking( packer, objectTree, this._itemSerializer, cancellationToken );
		}

		protected internal override Task<ArraySegment<T>> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			return _asyncUnpacking( unpacker, this._itemSerializer, cancellationToken );
		}

#endif // FEATURE_TAP
	}
}
