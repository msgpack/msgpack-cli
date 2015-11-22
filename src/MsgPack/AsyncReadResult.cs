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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;

namespace MsgPack
{
#if FEATURE_TAP
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
	}
#endif // FEATURE_TAP
}