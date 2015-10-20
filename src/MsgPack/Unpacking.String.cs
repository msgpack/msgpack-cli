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
using System.Globalization;
using System.IO;
using System.Text;

namespace MsgPack
{
	// Portion of convenient string API.

	partial class Unpacking
	{
		#region -- UnpackString --
		///	<summary>
		///		Unpacks <see cref="String" /> value from the head of specified byte array with UTF-8 encoding.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="String" /> which contains unpacked <see cref="String" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="String" />.
		///		Or, the unpacked result in the <paramref name="source"/> is invalid as UTF-8 encoded byte stream.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackString(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<string> UnpackString( byte[] source )
		{
			return UnpackString( source, 0 );
		}

		///	<summary>
		///		Unpacks <see cref="String" /> value from the head of specified byte array with specified encoding.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="encoding">The <see cref="Encoding"/> to decode binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="String" /> which contains unpacked <see cref="String" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or, <paramref name="encoding"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="String" />.
		///		Or, the unpacked result in the <paramref name="source"/> is invalid as UTF-8 encoded byte stream.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackString(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<string> UnpackString( byte[] source, Encoding encoding )
		{
			return UnpackString( source, 0, encoding );
		}

		///	<summary>
		///		Unpacks <see cref="String" /> value from specified offsetted byte array with UTF-8 encoding.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="String" /> which contains unpacked <see cref="String" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///		Or, the length of <paramref name="source"/> is not greater than <paramref name="offset"/>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative value.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="String" />.
		///		Or, the unpacked result in the <paramref name="source"/> is invalid as specified encoding byte stream.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackString(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<string> UnpackString( byte[] source, int offset )
		{
			return UnpackString( source, offset, MessagePackConvert.Utf8NonBomStrict );
		}

		///	<summary>
		///		Unpacks <see cref="String" /> value from specified offsetted byte array with specified encoding.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<param name="encoding">The <see cref="Encoding"/> to decode binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="String" /> which contains unpacked <see cref="String" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or, <paramref name="encoding"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///		Or, the length of <paramref name="source"/> is not greater than <paramref name="offset"/>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative value.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="String" />.
		///		Or, the unpacked result in the <paramref name="source"/> is invalid as specified encoding byte stream.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackString(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<string> UnpackString( byte[] source, int offset, Encoding encoding )
		{
			if ( encoding == null )
			{
				throw new ArgumentNullException( "encoding" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			try
			{
				var result = UnpackBinary( source, offset );
				return new UnpackingResult<string>( encoding.GetString( result.Value, 0, result.Value.Length ), result.ReadCount );
			}
			catch ( DecoderFallbackException ex )
			{
				throw NewInvalidEncodingException( encoding, ex );
			}
		}


		///	<summary>
		///		Unpacks <see cref="String"/> value from the specified <see cref="Stream"/> with UTF-8 encoding.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked <see cref="String"/> value.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not raw binary.
		///		Or, the unpacked result in the <paramref name="source"/> is invalid as UTF-8 encoded byte stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static string UnpackString( Stream source )
		{
			return UnpackString( source, MessagePackConvert.Utf8NonBomStrict );
		}

		///	<summary>
		///		Unpacks <see cref="String"/> value from the specified <see cref="Stream"/> with specified encoding.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<param name="encoding">The <see cref="Encoding"/> to decode binary stream.</param>
		///	<returns>
		///		The unpacked <see cref="String"/> value.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="encoding"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not raw binary.
		///		Or, the unpacked result in the <paramref name="source"/> is invalid as specified encoding byte stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static string UnpackString( Stream source, Encoding encoding )
		{
			if ( encoding == null )
			{
				throw new ArgumentNullException( "encoding" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			try
			{
				var result = UnpackBinary( source );
				return encoding.GetString( result, 0, result.Length );
			}
			catch ( DecoderFallbackException ex )
			{
				throw NewInvalidEncodingException( encoding, ex );
			}
		}
		#endregion -- UnpackString --


		private static Exception NewInvalidEncodingException( Encoding encoding, Exception innerException )
		{
			return new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "The stream cannot be decoded as {0} string.", encoding.WebName ), innerException );
		}
	}
}
