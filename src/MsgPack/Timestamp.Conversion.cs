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
using System.Globalization;

namespace MsgPack
{
	partial struct Timestamp
	{
		private long ToTicks( Type destination )
		{
			if ( this.unixEpochSeconds < MinUnixEpochSecondsForTicks )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "This value is too small for '{0}'.", destination ) );
			}

			if ( this.unixEpochSeconds > MaxUnixEpochSecondsForTicks )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "This value is too large for '{0}'.", destination ) );
			}

			return ( UnixEpochInSeconds + this.unixEpochSeconds ) * SecondsToTicks + this.nanoseconds / NanoToTicks;
		}

		/// <summary>
		///		Converts this instance to equivalant <see cref="DateTime"/> instance with <see cref="DateTimeKind.Utc"/>.
		/// </summary>
		/// <returns>An equivalant <see cref="DateTime"/> instance with <see cref="DateTimeKind.Utc"/>.</returns>
		/// <exception cref="InvalidOperationException">
		///		This instance represents before <see cref="DateTime.MinValue"/> or after <see cref="DateTime.MaxValue"/>.
		/// </exception>
		public DateTime ToDateTime()
		{
			return new DateTime( this.ToTicks( typeof( DateTime ) ), DateTimeKind.Utc );
		}

		/// <summary>
		///		Converts this instance to equivalant <see cref="DateTimeOffset"/> instance with offset <c>0</c>.
		/// </summary>
		/// <returns>An equivalant <see cref="DateTimeOffset"/> instance with offset <c>0</c></returns>
		/// <exception cref="InvalidOperationException">
		///		This instance represents before <see cref="DateTimeOffset.MinValue"/> or after <see cref="DateTimeOffset.MaxValue"/>.
		/// </exception>
		public DateTimeOffset ToDateTimeOffset()
		{
			return new DateTimeOffset( this.ToTicks( typeof( DateTimeOffset ) ), TimeSpan.Zero );
		}

		/// <summary>
		///		Encodes this instance to a <see cref="MessagePackExtendedTypeObject"/>.
		/// </summary>
		/// <returns>A <see cref="MessagePackExtendedTypeObject"/> which equivalant to this instance.</returns>
		public MessagePackExtendedTypeObject Encode()
		{
			if ( ( this.unixEpochSeconds >> 34 ) != 0 )
			{
				// timestamp 96
				var value = this;
				var body = new byte[ 12 ];
				body[ 0 ] = unchecked( ( byte )( ( this.nanoseconds >> 24 ) & 0xFF ) );
				body[ 1 ] = unchecked( ( byte )( ( this.nanoseconds >> 16 ) & 0xFF ) );
				body[ 2 ] = unchecked( ( byte )( ( this.nanoseconds >> 8 ) & 0xFF ) );
				body[ 3 ] = unchecked( ( byte )( ( this.nanoseconds ) & 0xFF ) );
				body[ 4 ] = unchecked( ( byte )( ( this.unixEpochSeconds >> 56 ) & 0xFF ) );
				body[ 5 ] = unchecked( ( byte )( ( this.unixEpochSeconds >> 48 ) & 0xFF ) );
				body[ 6 ] = unchecked( ( byte )( ( this.unixEpochSeconds >> 40 ) & 0xFF ) );
				body[ 7 ] = unchecked( ( byte )( ( this.unixEpochSeconds >> 32 ) & 0xFF ) );
				body[ 8 ] = unchecked( ( byte )( ( this.unixEpochSeconds >> 24 ) & 0xFF ) );
				body[ 9 ] = unchecked( ( byte )( ( this.unixEpochSeconds >> 16 ) & 0xFF ) );
				body[ 10 ] = unchecked( ( byte )( ( this.unixEpochSeconds >> 8 ) & 0xFF ) );
				body[ 11 ] = unchecked( ( byte )( this.unixEpochSeconds & 0xFF ) );

				return MessagePackExtendedTypeObject.Unpack( TypeCode, body );
			}
			else
			{
				var encoded = ( ( ( ulong )this.nanoseconds ) << 34 ) | unchecked( ( ulong )this.unixEpochSeconds );
				if ( ( encoded & 0xFFFFFFFF00000000L ) == 0 )
				{
					// timestamp 32
					var value = unchecked( ( uint )encoded );
					var body = new byte[ 4 ];
					body[ 0 ] = unchecked( ( byte )( ( encoded >> 24 ) & 0xFF ) );
					body[ 1 ] = unchecked( ( byte )( ( encoded >> 16 ) & 0xFF ) );
					body[ 2 ] = unchecked( ( byte )( ( encoded >> 8 ) & 0xFF ) );
					body[ 3 ] = unchecked( ( byte )( encoded & 0xFF ) );

					return MessagePackExtendedTypeObject.Unpack( TypeCode, body );
				}
				else
				{
					// timestamp 64
					var body = new byte[ 8 ];
					body[ 0 ] = unchecked( ( byte )( ( encoded >> 56 ) & 0xFF ) );
					body[ 1 ] = unchecked( ( byte )( ( encoded >> 48 ) & 0xFF ) );
					body[ 2 ] = unchecked( ( byte )( ( encoded >> 40 ) & 0xFF ) );
					body[ 3 ] = unchecked( ( byte )( ( encoded >> 32 ) & 0xFF ) );
					body[ 4 ] = unchecked( ( byte )( ( encoded >> 24 ) & 0xFF ) );
					body[ 5 ] = unchecked( ( byte )( ( encoded >> 16 ) & 0xFF ) );
					body[ 6 ] = unchecked( ( byte )( ( encoded >> 8 ) & 0xFF ) );
					body[ 7 ] = unchecked( ( byte )( encoded & 0xFF ) );

					return MessagePackExtendedTypeObject.Unpack( TypeCode, body );
				}
			}
		}

		private static void FromDateTimeTicks( long ticks, out long unixEpocSeconds, out int nanoSeconds )
		{
			FromOffsetTicks( ticks - UnixEpochTicks, out unixEpocSeconds, out nanoSeconds );
		}

		private static void FromOffsetTicks( long ticks, out long unixEpocSeconds, out int nanoSeconds )
		{
			long remaining;
			unixEpocSeconds = DivRem( ticks, SecondsToTicks, out remaining );
			nanoSeconds = unchecked( ( int )remaining ) * 100;
		}

		/// <summary>
		///		Gets an equivalant <see cref="Timestamp"/> to specified <see cref="DateTime"/>.
		/// </summary>
		/// <param name="value">A <see cref="DateTime"/>.</param>
		/// <returns>An equivalant <see cref="Timestamp"/> to specified <see cref="DateTime"/></returns>
		public static Timestamp FromDateTime( DateTime value )
		{
			long unixEpocSeconds;
			int nanoSeconds;
			FromDateTimeTicks( ( value.Kind == DateTimeKind.Local ? value.ToUniversalTime() : value ).Ticks, out unixEpocSeconds, out nanoSeconds );
			return new Timestamp( unixEpocSeconds, nanoSeconds );
		}

		/// <summary>
		///		Gets an equivalant <see cref="Timestamp"/> to specified <see cref="DateTimeOffset"/>.
		/// </summary>
		/// <param name="value">A <see cref="DateTimeOffset"/>.</param>
		/// <returns>An equivalant <see cref="Timestamp"/> to specified <see cref="DateTimeOffset"/></returns>
		public static Timestamp FromDateTimeOffset( DateTimeOffset value )
		{
			long unixEpocSeconds;
			int nanoSeconds;
			FromDateTimeTicks( value.UtcTicks, out unixEpocSeconds, out nanoSeconds );
			return new Timestamp( unixEpocSeconds, nanoSeconds );
		}

		/// <summary>
		///		Decodes specified <see cref="MessagePackExtendedTypeObject"/> and returns an equivalant <see cref="Timestamp"/>.
		/// </summary>
		/// <param name="value"><see cref="MessagePackExtendedTypeObject"/> which is native representation of <see cref="Timestamp"/>.</param>
		/// <returns><see cref="Timestamp"/>.</returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="value"/> does not represent msgpack timestamp. Specifically, the type code is not equal to <see cref="TypeCode"/> value.
		///		Or, <paramref name="value"/> does not have valid msgpack timestamp structure.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="value"/> have invalid nanoseconds value.
		/// </exception>
		/// <remarks>
		///		A definition of valid msgpack time stamp is:
		///		<list type="bullet">
		///			<item>Its type code is <c>0xFF</c>(<c>-1</c>).</item>
		///			<item>Its length is 4, 8, or 12 bytes.</item>
		///			<item>Its nanoseconds part is between 0 and 999,999,999.</item>
		///		</list>
		/// </remarks>
		public static Timestamp Decode( MessagePackExtendedTypeObject value )
		{
			if ( value.TypeCode != TypeCode )
			{
				throw new ArgumentException( "The value's type code must be 0xFF.", "value" );
			}

			switch( value.Body.Length )
			{
				case 4:
				{
					// timespan32 format
					return new Timestamp( BigEndianBinary.ToUInt32( value.Body, 0 ), 0 );
				}
				case 8:
				{
					// timespan64 format
					var payload = BigEndianBinary.ToUInt64( value.Body, 0 );
					return new Timestamp( unchecked( ( long )( payload & 0x00000003ffffffffL ) ), unchecked( ( int )( payload >> 34 ) ) );
				}
				case 12:
				{
					// timespan96 format
					return new Timestamp( BigEndianBinary.ToInt64( value.Body, sizeof( int ) ), unchecked( ( int )BigEndianBinary.ToUInt32( value.Body, 0 ) ) );
				}
				default:
				{
					throw new ArgumentException( "The value's length is not valid.", "value" );
				}
			}
		}

		/// <summary>
		///		Converts a <see cref="Timestamp"/> value to a <see cref="DateTime"/> value with <see cref="DateTimeKind.Utc"/>.
		/// </summary>
		/// <param name="value">A <see cref="Timestamp"/>.</param>
		/// <returns>A <see cref="DateTime"/> with <see cref="DateTimeKind.Utc"/>.</returns>
		/// <exception cref="InvalidOperationException">
		///		This instance represents before <see cref="DateTime.MinValue"/> or after <see cref="DateTime.MaxValue"/>.
		/// </exception>
		public static explicit operator DateTime( Timestamp value )
		{
			return value.ToDateTime();
		}

		/// <summary>
		///		Converts a <see cref="Timestamp"/> value to a <see cref="DateTimeOffset"/> value with offset <c>0</c>.
		/// </summary>
		/// <param name="value">A <see cref="Timestamp"/>.</param>
		/// <returns>A <see cref="DateTimeOffset"/> value with offset <c>0</c>.</returns>
		/// <exception cref="InvalidOperationException">
		///		This instance represents before <see cref="DateTimeOffset.MinValue"/> or after <see cref="DateTimeOffset.MaxValue"/>.
		/// </exception>
		public static explicit operator DateTimeOffset( Timestamp value )
		{
			return value.ToDateTimeOffset();
		}

		/// <summary>
		///		Converts a <see cref="Timestamp"/> value to a <see cref="MessagePackExtendedTypeObject"/>.
		/// </summary>
		/// <param name="value">A <see cref="Timestamp"/>.</param>
		/// <returns>A <see cref="MessagePackExtendedTypeObject"/>.</returns>
		public static implicit operator MessagePackExtendedTypeObject( Timestamp value )
		{
			return value.Encode();
		}

		/// <summary>
		///		Converts a <see cref="DateTime"/> value to a <see cref="Timestamp"/>.
		/// </summary>
		/// <param name="value">A <see cref="DateTime"/>.</param>
		/// <returns>A <see cref="Timestamp"/>.</returns>
		public static implicit operator Timestamp( DateTime value )
		{
			return FromDateTime( value );
		}

		/// <summary>
		///		Converts a <see cref="DateTimeOffset"/> value to a <see cref="Timestamp"/>.
		/// </summary>
		/// <param name="value">A <see cref="DateTimeOffset"/>.</param>
		/// <returns>A <see cref="Timestamp"/>.</returns>
		public static implicit operator Timestamp( DateTimeOffset value )
		{
			return FromDateTimeOffset( value );
		}

		/// <summary>
		///		Converts a <see cref="MessagePackExtendedTypeObject"/> value to a <see cref="Timestamp"/>.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackExtendedTypeObject"/>.</param>
		/// <returns>A <see cref="MessagePackExtendedTypeObject"/>.</returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="value"/> does not represent msgpack timestamp. Specifically, the type code is not equal to <see cref="TypeCode"/> value.
		///		Or, <paramref name="value"/> does not have valid msgpack timestamp structure.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="value"/> have invalid nanoseconds value.
		/// </exception>
		public static explicit operator Timestamp( MessagePackExtendedTypeObject value )
		{
			return Decode( value );
		}
	}
}
