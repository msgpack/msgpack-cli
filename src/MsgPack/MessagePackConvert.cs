#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Text;

namespace MsgPack
{
	/// <summary>
	///		Define common convert rountines specific to MessagePack.
	/// </summary>
	public static class MessagePackConvert
	{
		private static readonly Encoding _utf8NonBomStrict = new UTF8Encoding( false, true );
		private static readonly Encoding _utf8NonBom = new UTF8Encoding( false, false );
		private const long _ticksToMilliseconds = 10000;

		internal static Encoding Utf8NonBom
		{
			get { return _utf8NonBom; }
		}

		internal static Encoding Utf8NonBomStrict
		{
			get { return _utf8NonBomStrict; }
		}

		/// <summary>
		///		Encode specified string by default encoding.
		/// </summary>
		/// <param name="value">String value.</param>
		/// <returns>Encoded <paramref name="value"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="value"/> is null.
		/// </exception>
		public static byte[] EncodeString( string value )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( "value" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			return _utf8NonBom.GetBytes( value );
		}

		/// <summary>
		///		Decode specified byte[] by default encoding.
		/// </summary>
		/// <param name="value">Byte[] value.</param>
		/// <returns>Decoded <paramref name="value"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="value"/> is null.
		/// </exception>
		/// <exception cref="System.Text.DecoderFallbackException">
		///		<paramref name="value"/> contains non-UTF-8 bits.
		/// </exception>
		public static string DecodeStringStrict( byte[] value )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( "value" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			return _utf8NonBomStrict.GetString( value, 0, value.Length );
		}

		private static readonly DateTime _unixEpocUtc = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );

		/// <summary>
		///		Convert specified <see cref="Int64"/> to <see cref="DateTimeOffset"/>.
		/// </summary>
		/// <param name="value">
		///		<see cref="Int64"/> value which is unpacked from packed message and may represent date-time value.
		///	</param>
		/// <returns>
		///		<see cref="DateTimeOffset"/>. Offset of this value always 0.
		/// </returns>
		public static DateTimeOffset ToDateTimeOffset( long value )
		{
			return _unixEpocUtc.AddTicks( value * _ticksToMilliseconds );
		}

		/// <summary>
		///		Convert specified <see cref="Int64"/> to <see cref="DateTime"/>.
		/// </summary>
		/// <param name="value">
		///		<see cref="Int64"/> value which is unpacked from packed message and may represent date-time value.
		///	</param>
		/// <returns>
		///		<see cref="DateTime"/>. This value is always UTC.
		/// </returns>
		public static DateTime ToDateTime( long value )
		{
			return _unixEpocUtc.AddTicks( value * _ticksToMilliseconds );
		}

		/// <summary>
		///		Convert specified <see cref="DateTimeOffset"/> to <see cref="Int64"/> as MessagePack defacto-standard.
		/// </summary>
		/// <param name="value"><see cref="DateTimeOffset"/>.</param>
		/// <returns>
		///		UTC epoc time from 1970/1/1 0:00:00, in milliseconds.
		/// </returns>
		public static long FromDateTimeOffset( DateTimeOffset value )
		{
			// Note: microseconds and nanoseconds should always truncated, so deviding by integral is suitable.
			return value.ToUniversalTime().Subtract( _unixEpocUtc ).Ticks / _ticksToMilliseconds;
		}

		/// <summary>
		///		Convert specified <see cref="DateTime"/> to <see cref="Int64"/> as MessagePack defacto-standard.
		/// </summary>
		/// <param name="value"><see cref="DateTime"/>.</param>
		/// <returns>
		///		UTC epoc time from 1970/1/1 0:00:00, in milliseconds.
		/// </returns>
		public static long FromDateTime( DateTime value )
		{
			// Note: microseconds and nanoseconds should always truncated, so deviding by integral is suitable.
			return value.ToUniversalTime().Subtract( _unixEpocUtc ).Ticks / _ticksToMilliseconds;
		}
	}
}
