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
using System.Collections.Generic;

namespace MsgPack
{
	/// <summary>
	///		Represents asynchronous <see cref="Unpacker"/> reading result.
	/// </summary>
	/// <typeparam name="T">The type of read value.</typeparam>
	public struct AsyncReadResult<T> : IEquatable<AsyncReadResult<T>>
	{
		private readonly bool _success;

		/// <summary>
		///		Gets a value indicating whether the asynchronous reading was succeeded.
		/// </summary>
		/// <value>
		///   <c>true</c> if the asynchronous reading was succeeded; otherwise, <c>false</c>, that is the operation was failed because of stream end.
		/// </value>
		public bool Success { get { return this._success; } }

		private readonly T _value;

		/// <summary>
		///		Gets the read value.
		/// </summary>
		/// <value>
		///		The read value when <see cref="Success"/> is <c>true</c>; otherwise, default value of <typeparamref name="T"/>.
		/// </value>
		public T Value { get { return this._value; } }

		internal AsyncReadResult( T value, bool success )
		{
			this._value = value;
			this._success = success;
		}

		/// <summary>
		///		Determines whether the specified object, is equal to this instance.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals( object obj )
		{
			if ( !( obj is AsyncReadResult<T> ) )
			{
				return false;
			}

			return this.Equals( ( AsyncReadResult<T> )obj );
		}

		/// <summary>
		///		Determines whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		///		<c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals( AsyncReadResult<T> other )
		{
			return this._success == other._success && EqualityComparer<T>.Default.Equals( this._value, other._value );
		}

		/// <summary>
		///		Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		///		A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return this._success.GetHashCode() ^ ( this._value == null ? 0 : this._value.GetHashCode() );
		}

		/// <summary>
		///		Returns a string that represents this instance.
		/// </summary>
		/// <returns>
		///		A string that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this._success ? ( this._value == null ? String.Empty : this._value.ToString() ) : String.Empty;
		}

		/// <summary>
		///		Determines whether two <see cref="AsyncReadResult{T}"/> are equal.
		/// </summary>
		/// <param name="left">The <see cref="AsyncReadResult{T}"/>.</param>
		/// <param name="right">The <see cref="AsyncReadResult{T}"/>.</param>
		/// <returns>
		///		<c>true</c> if objects are equal each other; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==( AsyncReadResult<T> left, AsyncReadResult<T> right )
		{
			return left.Equals( right );
		}

		/// <summary>
		///		Determines whether two <see cref="AsyncReadResult{T}"/> are not equal.
		/// </summary>
		/// <param name="left">The <see cref="AsyncReadResult{T}"/>.</param>
		/// <param name="right">The <see cref="AsyncReadResult{T}"/>.</param>
		/// <returns>
		///		<c>true</c> if objects are not equal each other; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=( AsyncReadResult<T> left, AsyncReadResult<T> right )
		{
			return !left.Equals( right );
		}
	}
}