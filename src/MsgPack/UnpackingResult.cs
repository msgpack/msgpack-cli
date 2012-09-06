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
using System.Collections.Generic;
using System.Globalization;

namespace MsgPack
{
	/// <summary>
	///		Represents result of direct conversion from the byte array.
	/// </summary>
	/// <typeparam name="T">Type of value.</typeparam>
	public struct UnpackingResult<T> : IEquatable<UnpackingResult<T>>
	{
		private readonly int _readCount;

		/// <summary>
		///		Get read bytes count from input byte array.
		/// </summary>
		/// <value>
		///		Read bytes count from input byte array.
		///		If this value equals to old offset, then a value of <see cref="Value"/> property is not undifined.
		/// </value>
		public int ReadCount
		{
			get { return this._readCount; }
		}

		private readonly T _value;

		/// <summary>
		///		Get retrieved value from the byte array.
		/// </summary>
		/// <value>
		///		Retrieved value from the byte array.
		///		If <see cref="ReadCount"/> equals to old offset, then a value of this property is not undefined.
		/// </value>
		public T Value
		{
			get { return this._value; }
		}

		internal UnpackingResult( T value, int readCount )
		{
			this._value = value;
			this._readCount = readCount;
		}

		/// <summary>
		///		Compare two instances are equal.
		/// </summary>
		/// <param name="obj"><see cref="UnpackingResult&lt;T&gt;"/> instance.</param>
		/// <returns>
		///		If <paramref name="obj"/> is <see cref="UnpackingResult&lt;T&gt;"/> and its value is equal to this instance, then true.
		///		Otherwise false.
		/// </returns>
		public override bool Equals( object obj )
		{
			if ( !( obj is UnpackingResult<T> ) )
			{
				return false;
			}
			else
			{
				return this.Equals( ( UnpackingResult<T> )obj );
			}
		}

		/// <summary>
		///		Compare two instances are equal.
		/// </summary>
		/// <param name="other"><see cref="UnpackingResult&lt;T&gt;"/> instance.</param>
		/// <returns>
		///		Whether value of <paramref name="other"/> is equal to this instance or not.
		/// </returns>
		public bool Equals( UnpackingResult<T> other )
		{
			return this._readCount == other._readCount && EqualityComparer<T>.Default.Equals( this._value, other._value );
		}

		/// <summary>
		///		Get hash code of this instance.
		/// </summary>
		/// <returns>Hash code of this instance.</returns>
		public override int GetHashCode()
		{
			return this._readCount.GetHashCode() ^ ( this._value == null ? 0 : this._value.GetHashCode() );
		}

		/// <summary>
		///		Get string representation of this object.
		/// </summary>
		/// <returns>String representation of this object.</returns>
		/// <remarks>
		///		<note>
		///			DO NOT use this value programmically. 
		///			The purpose of this method is informational, so format of this value subject to change.
		///		</note>
		/// </remarks>
		public override string ToString()
		{
			return String.Format( CultureInfo.CurrentCulture, "{0}({1} bytes)", this._value, this._readCount );
		}

		/// <summary>
		///		Compare two instances are equal.
		/// </summary>
		/// <param name="left"><see cref="UnpackingResult&lt;T&gt;"/> instance.</param>
		/// <param name="right"><see cref="UnpackingResult&lt;T&gt;"/> instance.</param>
		/// <returns>
		///		Whether value of <paramref name="left"/> and <paramref name="right"/> are equal each other or not.
		/// </returns>
		public static bool operator ==( UnpackingResult<T> left, UnpackingResult<T> right )
		{
			return left.Equals( right );
		}

		/// <summary>
		///		Compare two instances are not equal.
		/// </summary>
		/// <param name="left"><see cref="UnpackingResult&lt;T&gt;"/> instance.</param>
		/// <param name="right"><see cref="UnpackingResult&lt;T&gt;"/> instance.</param>
		/// <returns>
		///		Whether value of <paramref name="left"/> and <paramref name="right"/> are not equal each other or are equal.
		/// </returns>		
		public static bool operator !=( UnpackingResult<T> left, UnpackingResult<T> right )
		{
			return !left.Equals( right );
		}
	}
}
