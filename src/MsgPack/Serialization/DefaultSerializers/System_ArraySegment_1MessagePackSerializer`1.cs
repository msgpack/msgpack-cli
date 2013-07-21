#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
#if NETFX_CORE
using System.Linq.Expressions;
using System.Reflection;
#endif

namespace MsgPack.Serialization.DefaultSerializers
{
	internal class System_ArraySegment_1MessagePackSerializer<T> : MessagePackSerializer<ArraySegment<T>>
	{
		private static readonly Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> _packing = InitializePacking();
		private static readonly Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> _unpacking = InitializeUnacking();

		private static Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> InitializePacking()
		{
#if !NETFX_CORE
			if ( typeof( T ) == typeof( byte ) )
			{
				return
					Delegate.CreateDelegate(
						typeof( Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> ),
						ArraySegmentMessageSerializer.PackByteArraySegmentToMethod
					) as Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>;
			}
			else if ( typeof( T ) == typeof( char ) )
			{
				return
					Delegate.CreateDelegate(
						typeof( Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> ),
						ArraySegmentMessageSerializer.PackCharArraySegmentToMethod
					) as Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>;
			}
			else
			{
				return
					Delegate.CreateDelegate(
						typeof( Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> ),
						ArraySegmentMessageSerializer.PackGenericArraySegmentTo1Method.MakeGenericMethod( typeof( T ) )
					) as Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>;
			}
#else
			MethodInfo packingMethod;
			if ( typeof( T ).Equals( typeof( byte ) ) )
			{
				packingMethod = ArraySegmentMessageSerializer.PackByteArraySegmentToMethod;
			}
			else if ( typeof( T ).Equals( typeof( char ) ) )
			{
				packingMethod = ArraySegmentMessageSerializer.PackCharArraySegmentToMethod;
			}
			else
			{
				packingMethod = ArraySegmentMessageSerializer.PackGenericArraySegmentTo1Method.MakeGenericMethod( typeof( T ) );
			}

			var packerParameter = Expression.Parameter( typeof( Packer ), "packer" );
			var targetParameter = Expression.Parameter( typeof( ArraySegment<T> ), "target" );
			var serializerParameter = Expression.Parameter( typeof( MessagePackSerializer<T> ), "serializer" );
			return
				Expression.Lambda<Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>>(
					Expression.Call(
						null,
						packingMethod,
						packerParameter, targetParameter, serializerParameter
					),
					packerParameter, targetParameter, serializerParameter
				).Compile();
#endif
		}

		private static Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> InitializeUnacking()
		{
#if !NETFX_CORE
			if ( typeof( T ) == typeof( byte ) )
			{
				return
					Delegate.CreateDelegate(
						typeof( Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> ),
						ArraySegmentMessageSerializer.UnpackByteArraySegmentFromMethod
					) as Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>;
			}
			else if ( typeof( T ) == typeof( char ) )
			{

				return
					Delegate.CreateDelegate(
						typeof( Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> ),
						ArraySegmentMessageSerializer.UnpackCharArraySegmentFromMethod
					) as Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>;
			}
			else
			{
				return
					Delegate.CreateDelegate(
						typeof( Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> ),
						ArraySegmentMessageSerializer.UnpackGenericArraySegmentFrom1Method.MakeGenericMethod( typeof( T ) )
					) as Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>;
			}
#else
			MethodInfo unpackingMethod;
			if ( typeof( T ).Equals( typeof( byte ) ) )
			{
				unpackingMethod = ArraySegmentMessageSerializer.UnpackByteArraySegmentFromMethod;
			}
			else if ( typeof( T ).Equals( typeof( char ) ) )
			{
				unpackingMethod = ArraySegmentMessageSerializer.UnpackCharArraySegmentFromMethod;
			}
			else
			{
				unpackingMethod = ArraySegmentMessageSerializer.UnpackGenericArraySegmentFrom1Method.MakeGenericMethod( typeof( T ) );
			}

			var unpackerParameter = Expression.Parameter( typeof( Unpacker ), "unpacker" );
			var serializerParameter = Expression.Parameter( typeof( MessagePackSerializer<T> ), "serializer" );
			return
				Expression.Lambda<Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>>(
					Expression.Call(
						null,
						unpackingMethod,
						unpackerParameter, serializerParameter 
					),
					unpackerParameter, serializerParameter
				).Compile();
#endif
		}

		private readonly MessagePackSerializer<T> _itemSerializer;

		public System_ArraySegment_1MessagePackSerializer( SerializationContext context )
			: base( ( context ?? SerializationContext.Default ).CompatibilityOptions.PackerCompatibilityOptions )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			this._itemSerializer = context.GetSerializer<T>();
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
