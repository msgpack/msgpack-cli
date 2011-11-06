#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;

namespace MsgPack
{
	// Portion of convenient string API.

	partial class Unpacking
	{
		/// <summary>
		///		Unpack entire raw binary from specified <see cref="IEnumerable&lt;Byte&gt;" /> and decodes with UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <param name="source">Source <see cref="IEnumerable&lt;Byte&gt;"/>.</param>
		/// <returns>Converted string value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static string UnpackString( Stream source )
		{
			return UnpackString( source, Encoding.UTF8 );
		}

		/// <summary>
		///		Unpack entire raw binary from specified <see cref="IEnumerable&lt;Byte&gt;" /> and decodes with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="source">Source <see cref="IEnumerable&lt;Byte&gt;"/>.</param>
		/// <param name="encoding"><see cref="Encoding"/> to decode.</param>
		/// <returns>Converted char value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="encoding"/> is null.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static string UnpackString( Stream source, Encoding encoding )
		{
			var buffer = new StringBuilder();
			foreach ( char c in UnpackCharStream( source, encoding ) )
			{
				buffer.Append( c );
			}
			return buffer.ToString();
		}

		/// <summary>
		///		Unpack entire raw binary from specified <see cref="IEnumerable&lt;Byte&gt;" /> and decodes with UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <param name="source">Source <see cref="IEnumerable&lt;Byte&gt;"/>.</param>
		/// <param name="length">Length of raw bainary in bytes.</param>
		/// <returns>Converted string value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static string UnpackStringBody( Stream source, long length )
		{
			return UnpackStringBody( source, Encoding.UTF8, length );
		}

		/// <summary>
		///		Unpack entire raw binary from specified <see cref="IEnumerable&lt;Byte&gt;" /> and decodes with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="source">Source <see cref="IEnumerable&lt;Byte&gt;"/>.</param>
		/// <param name="encoding"><see cref="Encoding"/> to decode.</param>
		/// <param name="length">Length of raw bainary in bytes.</param>
		/// <returns>Converted char value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="encoding"/> is null.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static string UnpackStringBody( Stream source, Encoding encoding, long length )
		{
			var buffer = new StringBuilder( unchecked( ( int )length ) );
			foreach ( char c in UnpackCharStreamBody( source, encoding, length ) )
			{
				buffer.Append( c );
			}
			return buffer.ToString();
		}

		/// <summary>
		///		Unpack entire raw binary from specified <see cref="IEnumerable&lt;Byte&gt;" /> and decodes with UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <param name="source">Source <see cref="IEnumerable&lt;Byte&gt;"/>.</param>
		/// <returns>Converted char value stream.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static IEnumerable<char> UnpackCharStream( Stream source )
		{
			return UnpackCharStream( source, Encoding.UTF8 );
		}

		/// <summary>
		///		Unpack entire raw binary from specified <see cref="IEnumerable&lt;Byte&gt;" /> and decodes with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="source">Source <see cref="IEnumerable&lt;Byte&gt;"/>.</param>
		/// <param name="encoding"><see cref="Encoding"/> to decode.</param>
		/// <returns>Converted char value stream.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="encoding"/> is null.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static IEnumerable<char> UnpackCharStream( Stream source, Encoding encoding )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( encoding == null )
			{
				throw new ArgumentNullException( "encoding" );
			}

			Contract.EndContractBlock();

			return UnpackCharStreamCore( source, encoding );
		}

		/// <summary>
		///		Unpack entire raw binary from specified <see cref="IEnumerable&lt;Byte&gt;" /> and decodes with UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <param name="source">Source <see cref="IEnumerable&lt;Byte&gt;"/>.</param>
		/// <param name="length">Length of raw binary.</param>
		/// <returns>Converted char value stream.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static IEnumerable<char> UnpackCharStreamBody( Stream source, long length )
		{
			return UnpackCharStreamBody( source, Encoding.UTF8, length );
		}

		/// <summary>
		///		Unpack entire raw binary from specified <see cref="IEnumerable&lt;Byte&gt;" /> and decodes with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="source">Source <see cref="IEnumerable&lt;Byte&gt;"/>.</param>
		/// <param name="encoding"><see cref="Encoding"/> to decode.</param>
		/// <param name="length">Length of raw binary.</param>
		/// <returns>Converted char value stream.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="encoding"/> is null.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static IEnumerable<char> UnpackCharStreamBody( Stream source, Encoding encoding, long length )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( encoding == null )
			{
				throw new ArgumentNullException( "encoding" );
			}

			Contract.EndContractBlock();

			return UnpackCharStreamBodyCore( source, encoding, length );
		}

		internal static IEnumerable<char> UnpackCharStreamCore( Stream source, Encoding encoding )
		{
			return UnpackCharStreamBodyCore( source, encoding, UnpackRawLength( source ) );
		}

		internal static IEnumerable<char> UnpackCharStreamBodyCore( Stream source, Encoding encoding, long length )
		{
			var decoder = encoding.GetDecoder();
			var byteBuffer = new byte[ encoding.GetMaxByteCount( 1 ) ];
			int byteBufferOffset = 0;
			var charBuffer = new char[ encoding.GetMaxCharCount( 1 ) ];

			// Bytes actually used to decode charactors.
			long totallyUsed = 0;
			// Bytes using decode current char.
			int locallyUsed = 0;
			long initialPosition = source.CanSeek ? source.Position : 0;
			bool completed;
			for ( long read = 0; read < length; read++ )
			{
				int readByte = source.ReadByte();
				if ( readByte < 0 )
				{
					// throw new UnpackException( "Insufficient stream length." );
					break;
				}

				byteBuffer[ byteBufferOffset ] = ( byte )readByte;
				byteBufferOffset++;

				// Bytes used by decoder including to store decoder internal states.
				int bytesUsed;
				// Chars actually decoded now.
				int charsUsed;

				decoder.Convert( byteBuffer, 0, byteBufferOffset, charBuffer, 0, charBuffer.Length, false, out bytesUsed, out charsUsed, out completed );
				if ( completed )
				{
					for ( int i = 0; i < charsUsed; i++ )
					{
						yield return charBuffer[ i ];
					}

					locallyUsed += bytesUsed;

					if ( charsUsed > 0 )
					{
						// Successfully decoded, so sum up used bytes for current char.
						totallyUsed += locallyUsed;
						locallyUsed = 0;
					}

					byteBufferOffset = 0;
				}
			}

			if ( totallyUsed < length )
			{
				if ( !source.CanSeek )
				{
					// Cannot seek, cannot rollback.
					throw new UnpackException( String.Format( CultureInfo.CurrentCulture, "Cannot decode last {0} bytes.", length - totallyUsed ) );
				}
				else
				{
					// Roll back.
					source.Position = totallyUsed + initialPosition;
				}
			}
		}

		/// <summary>
		///		Unpack entire raw binary from specified <see cref="Byte"/>[] and decodes with UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <param name="source">Source <see cref="Byte"/>[].</param>
		/// <returns>Converted string value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static string UnpackString( byte[] source )
		{
			return UnpackString( source, Encoding.UTF8 );
		}

		/// <summary>
		///		Unpack entire raw binary from specified <see cref="Byte"/>[] and decodes with UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <param name="source">Source <see cref="Byte"/>[].</param>
		/// <param name="offset">Offset.</param>
		/// <returns>Converted string value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static string UnpackString( byte[] source, int offset )
		{
			return UnpackString( source, offset, Encoding.UTF8 );
		}

		/// <summary>
		///		Unpack entire raw binary from specified <see cref="Byte"/>[] and decodes with specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="source">Source <see cref="Byte"/>[].</param>
		/// <param name="encoding"><see cref="Encoding"/> to decode.</param>
		/// <returns>Converted string value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="encoding"/> is null.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static string UnpackString( byte[] source, Encoding encoding )
		{
			return UnpackString( source, 0, encoding );
		}

		/// <summary>
		///		Unpack entire raw binary from specified <see cref="Byte"/>[] and decodes with UTF-8.
		/// </summary>
		/// <param name="source">Source <see cref="Byte"/>[].</param>
		/// <param name="offset">Offset.</param>
		/// <param name="encoding"><see cref="Encoding"/> to decode.</param>
		/// <returns>Converted string value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="encoding"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static string UnpackString( byte[] source, int offset, Encoding encoding )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( offset < 0 )
			{
				throw new ArgumentOutOfRangeException( "offset", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "offset" ) );
			}

			if ( encoding == null )
			{
				throw new ArgumentNullException( "encoding" );
			}

			if ( source.Length <= offset )
			{
				throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "'source' is too small. Length of 'source' is {0}, 'offset' is {1}", source.Length, offset ), "source" );
			}

			Contract.EndContractBlock();

			return UnpackStringCore( source, offset, encoding );
		}

		internal static string UnpackStringCore( byte[] source, int offset, Encoding encoding )
		{
			var length = UnpackRawLength( source, offset );
			if ( length.ReadCount == 0 )
			{
				throw new MessageTypeException( "Not raw." );
			}

			if ( length.Value > Int32.MaxValue )
			{
				throw new UnpackException( String.Format( CultureInfo.CurrentCulture, "Too long string. Maximum char length is {0} but actual is {1}.", Int32.MaxValue, length.Value ) );
			}

			return encoding.GetString( source, offset + length.ReadCount, unchecked( ( int )length.Value ) );
		}
	}
}
