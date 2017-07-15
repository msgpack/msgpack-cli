#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015-2017 FUJIWARA, Yusuke
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

namespace MsgPack
{
	/// <summary>
	///		Provides static utility methods for <see cref="AsyncReadResult{T}"/>.
	/// </summary>
	public static class AsyncReadResult
	{
		/// <summary>
		///		Creates a new <see cref="AsyncReadResult{T}"/> instance which represents successful result with specified return value.
		/// </summary>
		/// <typeparam name="T">The type of return value.</typeparam>
		/// <param name="returnValue">The return value.</param>
		/// <returns>A new <see cref="AsyncReadResult{T}"/> instance which represents successful result with specified return value.</returns>
		public static AsyncReadResult<T> Success<T>( T returnValue )
		{
			return new AsyncReadResult<T>( returnValue, true );
		}

		/// <summary>
		///		Creates a new <see cref="AsyncReadResult{T}"/> instance which represents failure.
		/// </summary>
		/// <typeparam name="T">The type of return value which is returned when the operation is successful.</typeparam>
		/// <returns>A new <see cref="AsyncReadResult{T}"/> instance which represents failure.</returns>
		public static AsyncReadResult<T> Fail<T>()
		{
			// Fast-path
			return default( AsyncReadResult<T> );
		}

		internal static AsyncReadResult<Int32OffsetValue<T>> Success<T>( T returnValue, int offset )
		{
			return new AsyncReadResult<Int32OffsetValue<T>>( new Int32OffsetValue<T>( returnValue, offset ), true );
		}

		internal static AsyncReadResult<Int64OffsetValue<T>> Success<T>( T returnValue, long offset )
		{
			return new AsyncReadResult<Int64OffsetValue<T>>( new Int64OffsetValue<T>( returnValue, offset ), true );
		}
	}
}