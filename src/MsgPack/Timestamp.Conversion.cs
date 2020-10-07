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
using System.Buffers;
using System.Buffers.Binary;
using System.Globalization;
using System.Runtime.CompilerServices;
using MsgPack.Internal;

namespace MsgPack
{
	partial struct Timestamp
	{
		private long ToTicks(Type destination)
		{
			if (this.unixEpochSeconds < MinUnixEpochSecondsForTicks)
			{
				throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "This value is too small for '{0}'.", destination));
			}

			if (this.unixEpochSeconds > MaxUnixEpochSecondsForTicks)
			{
				throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "This value is too large for '{0}'.", destination));
			}

			return (UnixEpochInSeconds + this.unixEpochSeconds) * SecondsToTicks + this.nanoseconds / NanoToTicks;
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
			return new DateTime(this.ToTicks(typeof(DateTime)), DateTimeKind.Utc);
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
			return new DateTimeOffset(this.ToTicks(typeof(DateTimeOffset)), TimeSpan.Zero);
		}

		/// <summary>
		///		Encodes this instance to a <see cref="MessagePackExtendedTypeObject"/>.
		/// </summary>
		/// <returns>A <see cref="MessagePackExtendedTypeObject"/> which equivalant to this instance.</returns>
		public MessagePackExtendedTypeObject Encode()
		{
			Span<byte> buffer = stackalloc byte[12];
			var used = this.Encode(buffer);
			return MessagePackExtendedTypeObject.Unpack(TypeCode, buffer.Slice(0, used).ToArray());
		}

		public int Encode(Span<byte> buffer)
		{
			if ((this.unixEpochSeconds >> 34) != 0)
			{
				if (buffer.Length < 12)
				{
					Throw.TooSmallBuffer(nameof(buffer), 12);
				}

				// timestamp 96
				BinaryPrimitives.WriteUInt32BigEndian(buffer, this.nanoseconds);
				buffer = buffer.Slice(4);
				BinaryPrimitives.WriteInt64BigEndian(buffer, this.unixEpochSeconds);
				return 12;
			}
			else
			{
				var encoded = (((ulong)this.nanoseconds) << 34) | unchecked((ulong)this.unixEpochSeconds);
				if ((encoded & 0xFFFFFFFF00000000L) == 0)
				{
					if (buffer.Length < 4)
					{
						Throw.TooSmallBuffer(nameof(buffer), 4);
					}

					// timestamp 32
					BinaryPrimitives.WriteUInt32BigEndian(buffer, unchecked((uint)encoded));

					return 4;
				}
				else
				{
					// timestamp 64
					BinaryPrimitives.WriteUInt64BigEndian(buffer, encoded);

					return 8;
				}
			}
		}

		private static void FromDateTimeTicks(long ticks, out long unixEpocSeconds, out int nanoSeconds)
		{
			FromOffsetTicks(ticks - UnixEpochTicks, out unixEpocSeconds, out nanoSeconds);
		}

		private static void FromOffsetTicks(long ticks, out long unixEpocSeconds, out int nanoSeconds)
		{
			long remaining;
			unixEpocSeconds = DivRem(ticks, SecondsToTicks, out remaining);
			nanoSeconds = unchecked((int)remaining) * 100;
			if (nanoSeconds < 0)
			{
				// In this case, we must adjust these values
				// from "negative nanosec from nearest larger negative integer"
				// to "positive nanosec from nearest smaller nagative integer".
				unixEpocSeconds -= 1;
				nanoSeconds = (MaxNanoSeconds + 1) + nanoSeconds;
			}
		}

		/// <summary>
		///		Gets an equivalant <see cref="Timestamp"/> to specified <see cref="DateTime"/>.
		/// </summary>
		/// <param name="value">A <see cref="DateTime"/>.</param>
		/// <returns>An equivalant <see cref="Timestamp"/> to specified <see cref="DateTime"/></returns>
		public static Timestamp FromDateTime(DateTime value)
		{
			long unixEpocSeconds;
			int nanoSeconds;
			FromDateTimeTicks((value.Kind == DateTimeKind.Local ? value.ToUniversalTime() : value).Ticks, out unixEpocSeconds, out nanoSeconds);
			return new Timestamp(unixEpocSeconds, nanoSeconds);
		}

		/// <summary>
		///		Gets an equivalant <see cref="Timestamp"/> to specified <see cref="DateTimeOffset"/>.
		/// </summary>
		/// <param name="value">A <see cref="DateTimeOffset"/>.</param>
		/// <returns>An equivalant <see cref="Timestamp"/> to specified <see cref="DateTimeOffset"/></returns>
		public static Timestamp FromDateTimeOffset(DateTimeOffset value)
		{
			long unixEpocSeconds;
			int nanoSeconds;
			FromDateTimeTicks(value.UtcTicks, out unixEpocSeconds, out nanoSeconds);
			return new Timestamp(unixEpocSeconds, nanoSeconds);
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
		public static Timestamp Decode(MessagePackExtendedTypeObject value)
		{
			if (value.TypeCode != TypeCode)
			{
				Throw.InvalidTimestampTypeCode(value.TypeCode, nameof(value));
			}

			return DecodeCore(new ReadOnlySequence<byte>(value.Body));
		}
			

		public static Timestamp Decode(ExtensionTypeObject value)
		{
			if (value.Type.Tag != unchecked((sbyte)TypeCode))
			{
				Throw.InvalidTimestampTypeCode(value.Type.Tag, nameof(value));
			}

			return DecodeCore(value.Body);
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static Timestamp DecodeCore(in ReadOnlySequence<byte> body)
		{ 
			switch (body.Length)
			{
				case 4:
				{
					// timespan32 format
					Span<byte> buffer = stackalloc byte[4];
					body.CopyTo(buffer);
					return new Timestamp(BinaryPrimitives.ReadUInt32BigEndian(buffer), 0);
				}
				case 8:
				{
					// timespan64 format
					Span<byte> buffer = stackalloc byte[8];
					body.CopyTo(buffer);
					var payload = BinaryPrimitives.ReadUInt64BigEndian(buffer);
					return new Timestamp(unchecked((long)(payload & 0x00000003ffffffffL)), unchecked((int)(payload >> 34)));
				}
				case 12:
				{
					// timespan96 format
					Span<byte> buffer = stackalloc byte[12];
					body.CopyTo(buffer);
					return new Timestamp(BinaryPrimitives.ReadInt64BigEndian(buffer.Slice(sizeof(int))), BinaryPrimitives.ReadInt32BigEndian(buffer));
				}
				default:
				{
					Throw.InvalidTimestampLength(body.Length, "value");
					return default; // Never reaches
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
		public static explicit operator DateTime(Timestamp value)
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
		public static explicit operator DateTimeOffset(Timestamp value)
		{
			return value.ToDateTimeOffset();
		}

		/// <summary>
		///		Converts a <see cref="Timestamp"/> value to a <see cref="MessagePackExtendedTypeObject"/>.
		/// </summary>
		/// <param name="value">A <see cref="Timestamp"/>.</param>
		/// <returns>A <see cref="MessagePackExtendedTypeObject"/>.</returns>
		public static implicit operator MessagePackExtendedTypeObject(Timestamp value)
		{
			return value.Encode();
		}

		/// <summary>
		///		Converts a <see cref="DateTime"/> value to a <see cref="Timestamp"/>.
		/// </summary>
		/// <param name="value">A <see cref="DateTime"/>.</param>
		/// <returns>A <see cref="Timestamp"/>.</returns>
		public static implicit operator Timestamp(DateTime value)
		{
			return FromDateTime(value);
		}

		/// <summary>
		///		Converts a <see cref="DateTimeOffset"/> value to a <see cref="Timestamp"/>.
		/// </summary>
		/// <param name="value">A <see cref="DateTimeOffset"/>.</param>
		/// <returns>A <see cref="Timestamp"/>.</returns>
		public static implicit operator Timestamp(DateTimeOffset value)
		{
			return FromDateTimeOffset(value);
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
		public static explicit operator Timestamp(MessagePackExtendedTypeObject value)
		{
			return Decode(value);
		}
	}
}
