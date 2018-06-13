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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
#if !NET35 && !UNITY
#if !WINDOWS_PHONE
#if !UNITY || MSGPACK_UNITY_FULL
using System.Numerics;
#endif // !WINDOWS_PHONE
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NET35 && !UNITY

namespace MsgPack
{
	partial struct Timestamp
	{
#if !NET35 && !UNITY
#if !WINDOWS_PHONE
#if !UNITY || MSGPACK_UNITY_FULL
		private static readonly BigInteger NanoToSecondsAsBigInteger = new BigInteger( 1000 * 1000 * 1000 );
#endif // !WINDOWS_PHONE
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NET35 && !UNITY

		/// <summary>
		///		Adds a specified <see cref="TimeSpan"/> to this instance.
		/// </summary>
		/// <param name="offset">A <see cref="TimeSpan"/> which represents offset. Note that this value can be negative.</param>
		/// <returns>The result <see cref="Timestamp"/>.</returns>
		/// <exception cref="OverflowException">
		///		The result of calculation overflows <see cref="MaxValue"/> or underflows <see cref="MinValue"/>.
		/// </exception>
		public Timestamp Add( TimeSpan offset )
		{
			long secondsOffset;
			int nanosOffset;
			FromOffsetTicks( offset.Ticks, out secondsOffset, out nanosOffset );
			var seconds = checked( this.unixEpochSeconds + secondsOffset );
			var nanos = this.nanoseconds + nanosOffset;
			if ( nanos > MaxNanoSeconds )
			{
				checked
				{
					seconds++;
				}
				nanos -= ( MaxNanoSeconds + 1 );
			}
			else if ( nanos < 0 )
			{
				checked
				{
					seconds--;
				}
				nanos = ( MaxNanoSeconds + 1 ) + nanos;
			}

			return new Timestamp( seconds, unchecked(( int )nanos) );
		}

		/// <summary>
		///		Subtracts a specified <see cref="TimeSpan"/> from this instance.
		/// </summary>
		/// <param name="offset">A <see cref="TimeSpan"/> which represents offset. Note that this value can be negative.</param>
		/// <returns>The result <see cref="Timestamp"/>.</returns>
		/// <exception cref="OverflowException">
		///		The result of calculation overflows <see cref="MaxValue"/> or underflows <see cref="MinValue"/>.
		/// </exception>
		public Timestamp Subtract( TimeSpan offset )
		{
			return this.Add( -offset );
		}

#if !NET35 && !UNITY
#if !WINDOWS_PHONE
#if !UNITY || MSGPACK_UNITY_FULL

		/// <summary>
		///		Adds a specified nanoseconds represented as a <see cref="BigInteger"/> from this instance.
		/// </summary>
		/// <param name="offsetNanoseconds">A <see cref="BigInteger"/> which represents offset. Note that this value can be negative.</param>
		/// <returns>The result <see cref="Timestamp"/>.</returns>
		/// <exception cref="OverflowException">
		///		The result of calculation overflows <see cref="MaxValue"/> or underflows <see cref="MinValue"/>.
		/// </exception>
		public Timestamp Add( BigInteger offsetNanoseconds )
		{
			BigInteger nanosecondsOffset;
			var secondsOffset = ( long )BigInteger.DivRem( offsetNanoseconds, NanoToSecondsAsBigInteger, out nanosecondsOffset );

			var seconds = checked( this.unixEpochSeconds + secondsOffset );
			var nanos = this.nanoseconds + unchecked( ( int )nanosecondsOffset );
			if ( nanos > MaxNanoSeconds )
			{
				checked
				{
					seconds++;
				}
				nanos -= ( MaxNanoSeconds + 1 );
			}
			else if ( nanos < 0 )
			{
				checked
				{
					seconds--;
				}
				nanos = ( MaxNanoSeconds + 1 ) + nanos;
			}

			return new Timestamp( seconds, unchecked(( int )nanos) );
		}

		/// <summary>
		///		Subtracts a specified nanoseconds represented as a <see cref="BigInteger"/> from this instance.
		/// </summary>
		/// <param name="offsetNanoseconds">A <see cref="BigInteger"/> which represents offset. Note that this value can be negative.</param>
		/// <returns>The result <see cref="Timestamp"/>.</returns>
		/// <exception cref="OverflowException">
		///		The result of calculation overflows <see cref="MaxValue"/> or underflows <see cref="MinValue"/>.
		/// </exception>
		public Timestamp Subtract( BigInteger offsetNanoseconds )
		{
			return this.Add( -offsetNanoseconds );
		}

		/// <summary>
		///		Calculates a difference this instance and a specified <see cref="Timestamp"/> in nanoseconds.
		/// </summary>
		/// <param name="other">A <see cref="Timestamp"/> to be differentiated.</param>
		/// <returns>A <see cref="BigInteger"/> which represents difference in nanoseconds.</returns>
		public BigInteger Subtract( Timestamp other )
		{
			var seconds = new BigInteger( this.unixEpochSeconds ) - other.unixEpochSeconds;
			var nanos = ( long )this.nanoseconds - other.nanoseconds;
#if DEBUG
			Contract.Assert( nanos <= MaxNanoSeconds, nanos + " <= MaxNanoSeconds" );
#endif // DEBUG
			if ( nanos < 0 )
			{
				// move down
				seconds--;
				nanos = ( MaxNanoSeconds + 1 ) + nanos;
			}

			return seconds * SecondsToNanos + nanos;
		}

#endif // !WINDOWS_PHONE
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NET35 && !UNITY

		/// <summary>
		///		Calculates a <see cref="Timestamp"/> with specified <see cref="Timestamp"/> and an offset represented as <see cref="TimeSpan"/>.
		/// </summary>
		/// <param name="value">A <see cref="Timestamp"/>.</param>
		/// <param name="offset">An offset in <see cref="TimeSpan"/>. This value can be negative.</param>
		/// <returns>A <see cref="Timestamp"/>.</returns>
		public static Timestamp operator +( Timestamp value, TimeSpan offset )
		{
			return value.Add( offset );
		}

		/// <summary>
		///		Calculates a <see cref="Timestamp"/> with specified <see cref="Timestamp"/> and an offset represented as <see cref="TimeSpan"/>.
		/// </summary>
		/// <param name="value">A <see cref="Timestamp"/>.</param>
		/// <param name="offset">An offset in <see cref="TimeSpan"/>. This value can be negative.</param>
		/// <returns>A <see cref="Timestamp"/>.</returns>
		public static Timestamp operator -( Timestamp value, TimeSpan offset )
		{
			return value.Subtract( offset );
		}

#if !NET35 && !UNITY
#if !WINDOWS_PHONE
#if !UNITY || MSGPACK_UNITY_FULL

		/// <summary>
		///		Calculates a <see cref="Timestamp"/> with specified <see cref="Timestamp"/> and a nanoseconds offset represented as <see cref="BigInteger"/>.
		/// </summary>
		/// <param name="value">A <see cref="Timestamp"/>.</param>
		/// <param name="offsetNanoseconds">An offset in nanoseconds as <see cref="BigInteger"/>. This value can be negative.</param>
		/// <returns>A <see cref="Timestamp"/>.</returns>
		public static Timestamp operator +( Timestamp value, BigInteger offsetNanoseconds )
		{
			return value.Add( offsetNanoseconds );
		}

		/// <summary>
		///		Calculates a <see cref="Timestamp"/> with specified <see cref="Timestamp"/> and a nanoseconds represented as <see cref="BigInteger"/>.
		/// </summary>
		/// <param name="value">A <see cref="Timestamp"/>.</param>
		/// <param name="offsetNanoseconds">An offset in nanoseconds as <see cref="BigInteger"/>. This value can be negative.</param>
		/// <returns>A <see cref="Timestamp"/>.</returns>
		public static Timestamp operator -( Timestamp value, BigInteger offsetNanoseconds )
		{
			return value.Subtract( offsetNanoseconds );
		}
		
		/// <summary>
		///		Calculates a difference between specified two <see cref="Timestamp"/>s in nanoseconds.
		/// </summary>
		/// <param name="left">A <see cref="Timestamp"/>.</param>
		/// <param name="right">A <see cref="Timestamp"/>.</param>
		/// <returns>A <see cref="BigInteger"/> which represents difference in nanoseconds.</returns>
		public static BigInteger operator -( Timestamp left, Timestamp right )
		{
			return left.Subtract( right );
		}

#endif // !WINDOWS_PHONE
#endif // !UNITY || MSGPACK_UNITY_FULL
#endif // !NET35 && !UNITY
	}
}
