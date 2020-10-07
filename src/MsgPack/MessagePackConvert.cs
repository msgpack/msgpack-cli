// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack
{
	/// <summary>
	///		Define common convert rountines specific to MessagePack.
	/// </summary>
	public static class MessagePackConvert
	{
		private const long _ticksToMilliseconds = 10000;

		private static readonly DateTime _unixEpocUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		///		Convert specified <see cref="Int64"/> to <see cref="DateTimeOffset"/>.
		/// </summary>
		/// <param name="value">
		///		<see cref="Int64"/> value which is unpacked from packed message and may represent date-time value.
		///	</param>
		/// <returns>
		///		<see cref="DateTimeOffset"/>. Offset of this value always 0.
		/// </returns>
		public static DateTimeOffset ToDateTimeOffset(long value) => _unixEpocUtc.AddTicks(value * _ticksToMilliseconds);

		/// <summary>
		///		Convert specified <see cref="Int64"/> to <see cref="DateTime"/>.
		/// </summary>
		/// <param name="value">
		///		<see cref="Int64"/> value which is unpacked from packed message and may represent date-time value.
		///	</param>
		/// <returns>
		///		<see cref="DateTime"/>. This value is always UTC.
		/// </returns>
		public static DateTime ToDateTime(long value) => _unixEpocUtc.AddTicks(value * _ticksToMilliseconds);

		/// <summary>
		///		Convert specified <see cref="DateTimeOffset"/> to <see cref="Int64"/> as MessagePack defacto-standard.
		/// </summary>
		/// <param name="value"><see cref="DateTimeOffset"/>.</param>
		/// <returns>
		///		UTC epoc time from 1970/1/1 0:00:00, in milliseconds.
		/// </returns>
		public static long FromDateTimeOffset(DateTimeOffset value)
			// Note: microseconds and nanoseconds should always truncated, so deviding by integral is suitable.
			=> value.ToUniversalTime().Subtract(_unixEpocUtc).Ticks / _ticksToMilliseconds;

		/// <summary>
		///		Convert specified <see cref="DateTime"/> to <see cref="Int64"/> as MessagePack defacto-standard.
		/// </summary>
		/// <param name="value"><see cref="DateTime"/>.</param>
		/// <returns>
		///		UTC epoc time from 1970/1/1 0:00:00, in milliseconds.
		/// </returns>
		public static long FromDateTime(DateTime value)
			// Note: microseconds and nanoseconds should always truncated, so deviding by integral is suitable.
			=> value.ToUniversalTime().Subtract(_unixEpocUtc).Ticks / _ticksToMilliseconds;
	}
}
