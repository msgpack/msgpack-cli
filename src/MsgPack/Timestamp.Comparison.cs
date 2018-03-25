#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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
	partial struct Timestamp : IComparable<Timestamp>, IEquatable<Timestamp> , IComparable
	{
		/// <summary>
		///		Compares this instance to the specified <see cref="Timestamp"/>.
		/// </summary>
		/// <param name="other">A <see cref="Timestamp"/> to be compared.</param>
		/// <returns>
		///		If this instance is greater than the <paramref name="other"/>, then <c>1</c>.
		///		If this instance is less than the <paramref name="other"/>, then <c>-1</c>.
		///		Else, this instance is equal to the <paramref name="other"/>, then <c>0</c>.
		/// </returns>
		public int CompareTo( Timestamp other )
		{
			var result = this.unixEpochSeconds.CompareTo( other.unixEpochSeconds );
			if ( result != 0 )
			{
				return result;
			}

			return this.nanoseconds.CompareTo( other.nanoseconds );
		}

		/// <summary>
		///		Compares two <see cref="Timestamp"/> instances.
		/// </summary>
		/// <param name="left">A <see cref="Timestamp"/> to be compared.</param>
		/// <param name="right">A <see cref="Timestamp"/> to be compared.</param>
		/// <returns>
		///		If the <paramref name="left"/> is greater than the <paramref name="right"/>, then <c>1</c>.
		///		If the <paramref name="left"/> is less than the <paramref name="right"/>, then <c>-1</c>.
		///		Else, the <paramref name="left"/> is equal to the <paramref name="right"/>, then <c>0</c>.
		/// </returns>
		public static int Compare( Timestamp left, Timestamp right )
		{
			return left.CompareTo( right );
		}

		/// <summary>
		///		Compares this instance to the specified object.
		/// </summary>
		/// <param name="obj">An <see cref="Object"/> to be compared.</param>
		/// <returns>
		///		If this instance is greater than the <paramref name="obj"/> or the <paramref name="obj"/> is <c>null</c>, then <c>1</c>.
		///		If this instance is less than the <paramref name="obj"/>, then <c>-1</c>.
		///		Else, this instance is equal to the <paramref name="obj"/>, then <c>0</c>.
		/// </returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="obj"/> is not a boxed <see cref="Timestamp"/>.
		/// </exception>
		int IComparable.CompareTo( object obj )
		{
			if ( obj == null )
			{
				return 1;
			}

			if ( !( obj is Timestamp ) )
			{
				throw new ArgumentException( "obj is not MsgPack.Timestamp object.", "obj" );
			}

			return this.CompareTo( ( Timestamp )obj );
		}

		/// <summary>
		///		Determines the specified <see cref="Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">An <see cref="Object"/> to be compared.</param>
		/// <returns>
		///		<c>true</c>, if the <paramref name="obj"/> is boxed <see cref="Timestamp"/> and its value is equal to this instance;
		///		otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals( object obj )
		{
			if ( !( obj is Timestamp ) )
			{
				return false;
			}

			return this.Equals( ( Timestamp )obj );
		}

		/// <summary>
		///		Determines the specified <see cref="Timestamp"/> is equal to this instance.
		/// </summary>
		/// <param name="other">A <see cref="Timestamp"/> to be compared.</param>
		/// <returns>
		///		<c>true</c>, if the <paramref name="other"/> is equal to this instance;
		///		otherwise, <c>false</c>.
		/// </returns>
		public bool Equals( Timestamp other )
		{
			return this.unixEpochSeconds == other.unixEpochSeconds && this.nanoseconds == other.nanoseconds;
		}

		/// <summary>
		///		Gets a hash code of this instance.
		/// </summary>
		/// <returns>A hash code of this instance.</returns>
		public override int GetHashCode()
		{
			return this.unixEpochSeconds.GetHashCode() ^ this.nanoseconds.GetHashCode();
		}

		/// <summary>
		///		Determines the <paramref name="left"/> is greater than the <paramref name="right"/>.
		/// </summary>
		/// <param name="left">A <see cref="Timestamp"/>.</param>
		/// <param name="right">A <see cref="Timestamp"/>.</param>
		/// <returns>
		///		<c>true</c>, if <paramref name="left"/> is greater than the <paramref name="right"/>;
		///		Otherwise, <c>false</c>.
		/// </returns>
		public static bool operator >( Timestamp left, Timestamp right )
		{
			return left.CompareTo( right ) > 0;
		}

		/// <summary>
		///		Determines the <paramref name="left"/> is less than the <paramref name="right"/>.
		/// </summary>
		/// <param name="left">A <see cref="Timestamp"/>.</param>
		/// <param name="right">A <see cref="Timestamp"/>.</param>
		/// <returns>
		///		<c>true</c>, if <paramref name="left"/> is less than the <paramref name="right"/>;
		///		Otherwise, <c>false</c>.
		/// </returns>
		public static bool operator <( Timestamp left, Timestamp right )
		{
			return left.CompareTo( right ) < 0;
		}

		/// <summary>
		///		Determines the <paramref name="left"/> is greater than or equal to the <paramref name="right"/>.
		/// </summary>
		/// <param name="left">A <see cref="Timestamp"/>.</param>
		/// <param name="right">A <see cref="Timestamp"/>.</param>
		/// <returns>
		///		<c>true</c>, if <paramref name="left"/> is greater than or equal to the <paramref name="right"/>;
		///		Otherwise, <c>false</c>.
		/// </returns>
		public static bool operator >=( Timestamp left, Timestamp right )
		{
			return left.CompareTo( right ) >= 0;
		}

		/// <summary>
		///		Determines the <paramref name="left"/> is less than or equal to the <paramref name="right"/>.
		/// </summary>
		/// <param name="left">A <see cref="Timestamp"/>.</param>
		/// <param name="right">A <see cref="Timestamp"/>.</param>
		/// <returns>
		///		<c>true</c>, if <paramref name="left"/> is less than or equal to the <paramref name="right"/>;
		///		Otherwise, <c>false</c>.
		/// </returns>
		public static bool operator <=( Timestamp left, Timestamp right )
		{
			return left.CompareTo( right ) <= 0;
		}

		/// <summary>
		///		Determines two <see cref="Timestamp"/> instances are equal.
		/// </summary>
		/// <param name="left">A <see cref="Timestamp"/>.</param>
		/// <param name="right">A <see cref="Timestamp"/>.</param>
		/// <returns>
		///		<c>true</c>, if <paramref name="left"/> is equal to <paramref name="right"/>;
		///		Otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==( Timestamp left, Timestamp right )
		{
			return left.Equals( right );
		}

		/// <summary>
		///		Determines two <see cref="Timestamp"/> instances are not equal.
		/// </summary>
		/// <param name="left">A <see cref="Timestamp"/>.</param>
		/// <param name="right">A <see cref="Timestamp"/>.</param>
		/// <returns>
		///		<c>true</c>, if <paramref name="left"/> is not equal to <paramref name="right"/>;
		///		Otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=( Timestamp left, Timestamp right )
		{
			return !left.Equals( right );
		}
	}
}
