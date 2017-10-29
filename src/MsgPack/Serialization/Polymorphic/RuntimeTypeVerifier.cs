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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

#if DEBUG
#define ASSERT
#endif // DEBUG

using System;
using System.Collections.Generic;
#if ASSERT
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
#endif // ASSERT
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;

namespace MsgPack.Serialization.Polymorphic
{
	internal static class RuntimeTypeVerifier
	{
		private const int CacheSize = 1000;

		private static readonly ReaderWriterLockSlim _resultCacheLock = new ReaderWriterLockSlim( LockRecursionPolicy.NoRecursion );
		private static readonly Dictionary<KeyValuePair<string, string>, bool> _resultCache = new Dictionary<KeyValuePair<string, string>, bool>( CacheSize );
		private static readonly Queue<KeyValuePair<string, string>> _histories = new Queue<KeyValuePair<string, string>>( CacheSize );

		public static void Verify( AssemblyName assemblyName, string typeFullName, Func<PolymorphicTypeVerificationContext, bool> typeVerifier )
		{
			var assemblyFullName = assemblyName.FullName;
			if ( !VerifyCore( assemblyName, assemblyFullName, typeFullName, typeVerifier ) )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "Type verifier rejects type '{0}'", typeFullName + ", " + assemblyFullName ) );
			}
		}

		private static bool VerifyCore( AssemblyName assemblyName, string assemblyFullName, string typeFullName, Func<PolymorphicTypeVerificationContext, bool> typeVerifier )
		{
			var key  =new KeyValuePair<string, string>( assemblyFullName, typeFullName );

			_resultCacheLock.EnterReadLock();
			try
			{
				bool cachedResult;
				if ( _resultCache.TryGetValue( key, out cachedResult ) )
				{
					return cachedResult;
				}
			}
			finally
			{
				_resultCacheLock.ExitReadLock();
			}

			bool result = typeVerifier( new PolymorphicTypeVerificationContext( typeFullName, assemblyName, assemblyFullName ) );

			_resultCacheLock.EnterWriteLock();
			try
			{
				int count = _resultCache.Count;
				_resultCache[ key ] = result;
				if ( count < _resultCache.Count && CacheSize < _resultCache.Count )
				{
					// Added. Start eviction.
					var removalKey = _histories.Dequeue();
					var removed = _resultCache.Remove( removalKey );
#if ASSERT
					Contract.Assert( removed );
#endif // ASSERT
				}

#if ASSERT
				Contract.Assert( _histories.Count < 1000 );
#endif // ASSERT
				_histories.Enqueue( key );
			}
			finally
			{
				_resultCacheLock.ExitWriteLock();
			}

			return result;
		}

#if UNITY && !UNITY_FULL
		// from https://github.com/dotnet/corefx/blob/master/src/System.Collections/src/System/Collections/Generic/Queue.cs
		private class Queue<T>
		{
			private T[] _array;
			private int _head;       // The index from which to dequeue if the queue isn't empty.
			private int _tail;       // The index at which to enqueue if the queue isn't full.
			private int _size;       // Number of elements.

			private const int MinimumGrow = 4;
			private const int GrowFactor = 200;  // double each time

			// Creates a queue with room for capacity objects. The default grow factor
			// is used.
			//
			public Queue( int capacity )
			{
				_array = new T[ capacity ];
			}

			public int Count
			{
				get { return _size; }
			}

			// Adds item to the tail of the queue.
			//
			public void Enqueue( T item )
			{
				if ( _size == _array.Length )
				{
					int newcapacity = (int)((long)_array.Length * (long)GrowFactor / 100);
					if ( newcapacity < _array.Length + MinimumGrow )
					{
						newcapacity = _array.Length + MinimumGrow;
					}
					SetCapacity( newcapacity );
				}

				_array[ _tail ] = item;
				MoveNext( ref _tail );
				_size++;
			}

			// Removes the object at the head of the queue and returns it. If the queue
			// is empty, this method simply returns null.
			public T Dequeue()
			{
				if ( _size == 0 )
					throw new InvalidOperationException( "Queue is empty." );

				T removed = _array[_head];
				_array[ _head ] = default( T );
				MoveNext( ref _head );
				_size--;
				return removed;
			}

			// PRIVATE Grows or shrinks the buffer to hold capacity objects. Capacity
			// must be >= _size.
			private void SetCapacity( int capacity )
			{
				T[] newarray = new T[capacity];
				if ( _size > 0 )
				{
					if ( _head < _tail )
					{
						Array.Copy( _array, _head, newarray, 0, _size );
					}
					else
					{
						Array.Copy( _array, _head, newarray, 0, _array.Length - _head );
						Array.Copy( _array, 0, newarray, _array.Length - _head, _tail );
					}
				}

				_array = newarray;
				_head = 0;
				_tail = ( _size == capacity ) ? 0 : _size;
			}

			// Increments the index wrapping it if necessary.
			private void MoveNext( ref int index )
			{
				// It is tempting to use the remainder operator here but it is actually much slower 
				// than a simple comparison and a rarely taken branch.   
				int tmp = index + 1;
				index = ( tmp == _array.Length ) ? 0 : tmp;
			}
		}
#endif // UNITY && !UNITY_FULL
	}
}
