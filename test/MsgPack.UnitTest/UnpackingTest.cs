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
using System.IO;
using System.Linq;
using System.Text;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack
{
	[TestFixture]
	public partial class UnpackingTest_Misc
	{
		[Test]
		public void TestUnpackArrayLength_ArrayLengthIsGreaterThanInt32MaxValue()
		{
			var result = Unpacking.UnpackArrayLength( new byte[] { 0xDD, 0x80, 0x00, 0x00, 0x00, 0xFF } );
			Assert.That( result.ReadCount, Is.EqualTo( 5 ) );
			Assert.That( result.Value, Is.EqualTo( Int32.MaxValue + 1L ) );
		}

		[Test]
		public void TestUnpackArray_ArrayLengthIsGreaterThanInt32MaxValue()
		{
			Assert.Throws<MessageNotSupportedException>( () => Unpacking.UnpackArray( new byte[] { 0xDD, 0x80, 0x00, 0x00, 0x00, 0xFF } ) );
		}

		[Test]
		public void TestUnpackDictionaryCount_DictionaryCountIsGreaterThanInt32MaxValue()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xDF, 0x80, 0x00, 0x00, 0x00, 0xFF } );
			Assert.That( result.ReadCount, Is.EqualTo( 5 ) );
			Assert.That( result.Value, Is.EqualTo( Int32.MaxValue + 1L ) );
		}

		[Test]
		public void TestUnpackDictionary_DictionaryCountIsGreaterThanInt32MaxValue()
		{
			Assert.Throws<MessageNotSupportedException>( () => Unpacking.UnpackDictionary( new byte[] { 0xDF, 0x80, 0x00, 0x00, 0x00, 0xFF } ) );
		}

		[Test]
		public void TestUnpackBinary_BinaryLengthIsGreaterThanInt32MaxValue()
		{
			Assert.Throws<MessageNotSupportedException>( () => Unpacking.UnpackBinary( new byte[] { 0xDB, 0x80, 0x00, 0x00, 0x00, 0xFF } ) );
		}

		[Test]
		public void TestUnpackBinary_Stream_ReadOnlyStream()
		{
			using ( var stream = new WrapperStream( new MemoryStream(), canRead: false ) )
			{
				Assert.Throws<ArgumentException>( () => Unpacking.UnpackBinary( stream ) );
			}
		}


		[Test]
		public void TestUnpackString_ByteArray_Empty_AsIsAndBounded()
		{
			var result = Unpacking.UnpackString( new byte[] { 0xA0, 0xFF } );
			Assert.That( result.ReadCount, Is.EqualTo( 1 ) );
			Assert.That( result.Value, Is.EqualTo( String.Empty ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_1Char_AsIsAndBounded()
		{
			var result = Unpacking.UnpackString( new byte[] { 0xA1, ( byte )'A', 0xFF } );
			Assert.That( result.ReadCount, Is.EqualTo( 2 ) );
			Assert.That( result.Value, Is.EqualTo( "A" ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_1ByteNonUtf8String_ExceptionInReaderOperation()
		{
			Assert.Throws<MessageTypeException>( () => Unpacking.UnpackString( new byte[] { 0xA1, 0xFF } ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackString( default( byte[] ) ) );
		}


		[Test]
		public void TestUnpackString_ByteArray_Encoding_Empty_AsIsAndBounded()
		{
#if !NETFX_CORE && !SILVERLIGHT
			var result = Unpacking.UnpackString( new byte[] { 0xA0, 0xFF }, Encoding.UTF32 );
#else
			var result = Unpacking.UnpackString( new byte[] { 0xA0, 0xFF }, Encoding.UTF8 );
#endif // !NETFX_CORE && !SILVERLIGHT
			Assert.That( result.ReadCount, Is.EqualTo( 1 ) );
			Assert.That( result.Value, Is.EqualTo( String.Empty ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_Encoding_1Byte_AsIsAndBounded()
		{
#if !NETFX_CORE && !SILVERLIGHT
			var result = Unpacking.UnpackString( new byte[] { 0xA4, 0x00, 0x00, 0x00, ( byte )'A', 0xFF }, new UTF32Encoding( bigEndian: true, byteOrderMark: false, throwOnInvalidCharacters: true ) );
			Assert.That( result.ReadCount, Is.EqualTo( 5 ) );
			Assert.That( result.Value, Is.EqualTo( "A" ) );
#else
			var result = Unpacking.UnpackString( new byte[] { 0xA2, 0x00, ( byte )'A', 0xFF }, new UnicodeEncoding( bigEndian: true, byteOrderMark: false, throwOnInvalidBytes: true ) );
			Assert.That( result.ReadCount, Is.EqualTo( 3 ) );
			Assert.That( result.Value, Is.EqualTo( "A" ) );
#endif // !NETFX_CORE && !SILVERLIGHT
		}

		[Test]
		public void TestUnpackString_ByteArray_Encoding_1ByteNonSpecifiedString()
		{
#if MONO || XAMDROID
			Assert.Inconclusive( "UTF32Encoding does not throw exception on Mono FCL." );
#elif !NETFX_CORE && !SILVERLIGHT
			Assert.Throws<MessageTypeException>( () => Unpacking.UnpackString( new byte[] { 0xA4, 0x7F, 0x7F, 0x7F, 0x7F }, new UTF32Encoding( bigEndian: true, byteOrderMark: false, throwOnInvalidCharacters: true ) ) );
#else
			Assert.Throws<MessageTypeException>( () => Unpacking.UnpackString( new byte[] { 0xA5, 0xF8, 0x88, 0x80, 0x80, 0x80 }, new UTF8Encoding( encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true ) ) );
#endif
		}

		[Test]
		public void TestUnpackString_ByteArray_ByteArrayIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackString( default( byte[] ) ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_ByteArrayIsEmpty()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackString( new byte[ 0 ] ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_Int32_ByteArrayIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackString( null, 0 ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_Int32_ByteArrayIsEmpty()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackString( new byte[ 0 ], 0 ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_Int32_OffsetIsTooBig()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackString( new byte[] { 0x1 }, 1 ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_Int32_OffsetIsNegative()
		{
			Assert.Throws<ArgumentOutOfRangeException>( () => Unpacking.UnpackString( new byte[] { 0x1 }, -1 ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_Int32_Encoding_ByteArrayIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackString( null, 0, Encoding.UTF8 ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_Int32_Encoding_ByteArrayIsEmpty()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackString( new byte[ 0 ], 0, Encoding.UTF8 ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_Int32_Encoding_OffsetIsTooBig()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackString( new byte[] { 0x1 }, 1, Encoding.UTF8 ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_Int32_Encoding_OffsetIsNegative()
		{
			Assert.Throws<ArgumentOutOfRangeException>( () => Unpacking.UnpackString( new byte[] { 0x1 }, -1, Encoding.UTF8 ) );
		}

		[Test]
		public void TestUnpackString_ByteArray_Int32_Encoding_EncodingIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackString( new byte[] { 0x1 }, 0, null ) );
		}


		[Test]
		public void TestUnpackString_Stream_Empty_AsIsAndBounded()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA0, 0xFF } ) )
			{
				var result = Unpacking.UnpackString( stream );
				Assert.That( stream.Position, Is.EqualTo( 1 ) );
				Assert.That( result, Is.EqualTo( String.Empty ) );
			}
		}

		[Test]
		public void TestUnpackString_Stream_1Char_AsIsAndBounded()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA1, ( byte )'A', 0xFF } ) )
			{
				var result = Unpacking.UnpackString( stream );
				Assert.That( stream.Position, Is.EqualTo( 2 ) );
				Assert.That( result, Is.EqualTo( "A" ) );
			}
		}

		[Test]
		public void TestUnpackString_Stream_1ByteNonUtf8String()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA1, 0xFF } ) )
			{
				Assert.Throws<MessageTypeException>( () => Unpacking.UnpackString( stream ) );
			}
		}

		[Test]
		public void TestUnpackString_Stream_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackString( default( Stream ) ) );
		}


		[Test]
		public void TestUnpackString_Stream_Encoding_Empty_AsIsAndBounded()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA0, 0xFF } ) )
			{
#if !NETFX_CORE && !SILVERLIGHT
				var result = Unpacking.UnpackString( stream, Encoding.UTF32 );
#else
				var result = Unpacking.UnpackString( stream, Encoding.UTF8 );
#endif // !NETFX_CORE && !SILVERLIGHT
				Assert.That( stream.Position, Is.EqualTo( 1 ) );
				Assert.That( result, Is.EqualTo( String.Empty ) );
			}
		}

		[Test]
		public void TestUnpackString_Stream_Encoding_1Byte_AsIsAndBounded()
		{
#if !NETFX_CORE && !SILVERLIGHT
			using ( var stream = new MemoryStream( new byte[] { 0xA4, 0x00, 0x00, 0x00, ( byte )'A', 0xFF } ) )
			{
				var result = Unpacking.UnpackString( stream, new UTF32Encoding( bigEndian: true, byteOrderMark: false, throwOnInvalidCharacters: true ) );
				Assert.That( stream.Position, Is.EqualTo( 5 ) );
				Assert.That( result, Is.EqualTo( "A" ) );
			}
#else
			using ( var stream = new MemoryStream( new byte[] { 0xA2, 0x00, ( byte )'A', 0xFF } ) )
			{
				var result = Unpacking.UnpackString( stream, new UnicodeEncoding( bigEndian: true, byteOrderMark: false, throwOnInvalidBytes: true ) );
				Assert.That( stream.Position, Is.EqualTo( 3 ) );
				Assert.That( result, Is.EqualTo( "A" ) );
			}
#endif // !NETFX_CORE && !SILVERLIGHT
		}

		[Test]
		public void TestUnpackString_Stream_Encoding_1ByteNonSpecifiedString()
		{
#if MONO || XAMDROID
			Assert.Inconclusive( "UTF32Encoding does not throw exception on Mono FCL." );
#endif

#if !NETFX_CORE && !SILVERLIGHT
			using ( var stream = new MemoryStream( new byte[] { 0xA4, 0x7F, 0x7F, 0x7F, 0x7F } ) )
			{
				Assert.Throws<MessageTypeException>( () => Unpacking.UnpackString( stream, new UTF32Encoding( bigEndian: true, byteOrderMark: false, throwOnInvalidCharacters: true ) ) );
			}
#else
			using ( var stream = new MemoryStream( new byte[] { 0xA5, 0xF8, 0x88, 0x80, 0x80, 0x80 } ) )
			{
				Assert.Throws<MessageTypeException>( () => Unpacking.UnpackString( stream, new UTF8Encoding( encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true ) ) );
			}
#endif // !NETFX_CORE && !SILVERLIGHT
		}

		[Test]
		public void TestUnpackString_Stream_Encoding_StreamIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackString( default( Stream ), Encoding.UTF8 ) );
		}

		[Test]
		public void TestUnpackString_Stream_Encoding_EncodingIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackString( new MemoryStream( new byte[] { 0xA1, ( byte )'A' } ), null ) );
		}


		private static void AssertRawStream( Stream target, int length )
		{
			Assert.That( target, Is.Not.Null );
			Assert.That( target.Length, Is.EqualTo( length ) );

			byte[] buffer = new byte[ length ];

			if ( length > 0 )
			{
				int readCount = target.Read( buffer, 0, length );
				Assert.That( readCount, Is.EqualTo( length ) );
				Assert.That( buffer, Is.All.EqualTo( 0xFF ) );
			}

			int readCountExtra = target.Read( buffer, 0, length );
			Assert.That( readCountExtra, Is.EqualTo( 0 ) );
		}

#if !NETFX_CORE && !SILVERLIGHT && !XAMDROID
		[Test]
		public void TestUnpackByteStream_Stream_LengthIsGreaterThanInt32MaxValue_CanReadToEnd()
		{
			// Header + Body Length ( Int32.MaxValue + 1 )
			var bodyLength = Int32.MaxValue + 1L;
			var length = 1L + 4L + bodyLength;
			string filePath = Path.GetTempFileName();
			try
			{
				File.SetAttributes( filePath, FileAttributes.SparseFile );
				using ( var fileStream = new FileStream( filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 64 * 1024 ) )
				{
					fileStream.SetLength( length );
					fileStream.Position = 0;
					fileStream.Write( new byte[] { 0xDB, 0x80, 0x00, 0x00, 0x00 }, 0, 5 );
					fileStream.Flush();

					fileStream.Position = 0;

					using ( var target = Unpacking.UnpackByteStream( fileStream ) )
					{
						Assert.That( target.Length, Is.EqualTo( bodyLength ) );
						byte[] buffer = new byte[ 64 * 1024 ];
						long totalLength = 0;
						for ( int read = target.Read( buffer, 0, buffer.Length ); read > 0; read = target.Read( buffer, 0, buffer.Length ) )
						{
							totalLength += read;
						}

						Assert.That( totalLength, Is.EqualTo( bodyLength ) );
					}
				}
			}
			finally
			{
				File.Delete( filePath );
			}
		}
#endif // !NETFX_CORE && !SILVERLIGHT && !XAMDROID

		[Test]
		public void TestUnpackByteStream_Stream_Empty_AsIsAndBounded()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA0, 0x57 } ) )
			{
				using ( var result = Unpacking.UnpackByteStream( stream ) )
				{
					AssertRawStream( result, 0 );
				}

				// Assert is valid position on unerlying stream.
				Assert.That( Unpacking.UnpackInt32( stream ), Is.EqualTo( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackByteStream_Stream_1Byte_AsIsAndBounded()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA1, 0xFF, 0x57 } ) )
			{
				using ( var result = Unpacking.UnpackByteStream( stream ) )
				{
					AssertRawStream( result, 1 );
				}

				// Assert is valid position on unerlying stream.
				Assert.That( Unpacking.UnpackInt32( stream ), Is.EqualTo( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackByteStream_Stream_0x100Byte_AsIsAndBounded()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xDA, 0x01, 0x00 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x100 ) ).Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() ) )
			{
				using ( var result = Unpacking.UnpackByteStream( stream ) )
				{
					AssertRawStream( result, 0x100 );
				}

				// Assert is valid position on unerlying stream.
				Assert.That( Unpacking.UnpackInt32( stream ), Is.EqualTo( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackByteStream_Stream_0x10000Byte_AsIsAndBounded()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xDB, 0x00, 0x01, 0x00, 0x00 }.Concat( Enumerable.Repeat( ( byte )0xFF, 0x10000 ) ).Concat( Enumerable.Repeat( ( byte )0x57, 1 ) ).ToArray() ) )
			{
				using ( var result = Unpacking.UnpackByteStream( stream ) )
				{
					AssertRawStream( result, 0x10000 );
				}

				// Assert is valid position on unerlying stream.
				Assert.That( Unpacking.UnpackInt32( stream ), Is.EqualTo( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackByteStream_Stream_SeekableStream_CanSeekIsTrue()
		{
			using ( var stream = new WrapperStream( new MemoryStream( new byte[] { 0xA0, 0x57 } ), canRead: true, canSeek: true ) )
			{
				using ( var result = Unpacking.UnpackByteStream( stream ) )
				{
					Assert.That( result.CanSeek, Is.True );
				}
			}
		}

		[Test]
		public void TestUnpackByteStream_Stream_SeekableStream_CanSeekIsFalse()
		{
			using ( var stream = new WrapperStream( new MemoryStream( new byte[] { 0xA0, 0x57 } ), canRead: true, canSeek: false ) )
			{
				using ( var result = Unpacking.UnpackByteStream( stream ) )
				{
					Assert.That( result.CanSeek, Is.False );
				}
			}
		}

		[Test]
		public void TestUnpackByteStream_Stream_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackByteStream( null ) );
		}


		private static void AssertStringReader( UnpackingStreamReader target, int byteLength, string expected )
		{
			Assert.That( target, Is.Not.Null );
			Assert.That( target.ByteLength, Is.EqualTo( byteLength ) );

			for ( int i = 0; i < expected.Length; i++ )
			{
				int c = target.Read();
				Assert.That( c, Is.GreaterThanOrEqualTo( 0 ) );
				Assert.That( ( char )c, Is.EqualTo( expected[ i ] ) );
			}

			Assert.That( target.EndOfStream, Is.True );
			Assert.That( target.Read(), Is.EqualTo( -1 ) );
		}

		[Test]
		public void TestUnpackCharStream_Stream_Empty_AsIsAndBounded()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA0, 0xFF } ) )
			{
				using ( var result = Unpacking.UnpackCharStream( stream ) )
				{
					AssertStringReader( result, 0, String.Empty );
				}

				// Assert is valid position on unerlying stream.
				Assert.That( Unpacking.UnpackInt32( stream ), Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestUnpackCharStream_Stream_1Char_AsIsAndBounded()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA1, ( byte )'A', 0xFF } ) )
			{
				using ( var result = Unpacking.UnpackCharStream( stream ) )
				{
					AssertStringReader( result, 1, "A" );
				}

				// Assert is valid position on unerlying stream.
				Assert.That( Unpacking.UnpackInt32( stream ), Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestUnpackCharStream_Stream_1ByteNonUtf8String_ExceptionInReaderOperation()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA1, 0xFF } ) )
			{
				using ( var result = Unpacking.UnpackCharStream( stream ) )
				{
					Assert.Throws<DecoderFallbackException>( () => result.Read() );
				}
			}
		}

		[Test]
		public void TestUnpackCharStream_Stream_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackCharStream( null ) );
		}


		[Test]
		public void TestUnpackCharStream_Stream_Encoding_Empty_AsIsAndBounded()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA0, 0xFF } ) )
			{
#if !NETFX_CORE && !SILVERLIGHT
				using ( var result = Unpacking.UnpackCharStream( stream, Encoding.UTF32 ) )
#else
				using ( var result = Unpacking.UnpackCharStream( stream, Encoding.UTF8 ) )
#endif // !NETFX_CORE && !SILVERLIGHT
				{
					AssertStringReader( result, 0, String.Empty );
				}

				// Assert is valid position on unerlying stream.
				Assert.That( Unpacking.UnpackInt32( stream ), Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestUnpackCharStream_Stream_Encoding_1Byte_AsIsAndBounded()
		{
#if !NETFX_CORE && !SILVERLIGHT
			using ( var stream = new MemoryStream( new byte[] { 0xA4, 0x00, 0x00, 0x00, ( byte )'A', 0xFF } ) )
#else
			using ( var stream = new MemoryStream( new byte[] { 0xA2, 0x00, ( byte )'A', 0xFF } ) )
#endif // !NETFX_CORE && !SILVERLIGHT
			{
#if !NETFX_CORE && !SILVERLIGHT
				using ( var result = Unpacking.UnpackCharStream( stream, new UTF32Encoding( bigEndian: true, byteOrderMark: false, throwOnInvalidCharacters: true ) ) )
				{
					AssertStringReader( result, 4, "A" );
				}
#else
				using ( var result = Unpacking.UnpackCharStream( stream, new UnicodeEncoding( bigEndian: true, byteOrderMark: false, throwOnInvalidBytes: true ) ) )
				{
					AssertStringReader( result, 2, "A" );
				}
#endif // !NETFX_CORE && !SILVERLIGHT
				// Assert is valid position on unerlying stream.
				Assert.That( Unpacking.UnpackInt32( stream ), Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestUnpackCharStream_Stream_Encoding_1ByteNonSpecifiedString_ExceptionInReaderOperation()
		{
#if MONO || XAMDROID
			Assert.Inconclusive( "UTF32Encoding does not throw exception on Mono FCL." );
#endif
#if !NETFX_CORE && !SILVERLIGHT
			using ( var stream = new MemoryStream( new byte[] { 0xA4, 0x7F, 0x7F, 0x7F, 0x7F } ) )
			{
				using ( var result = Unpacking.UnpackCharStream( stream, new UTF32Encoding( bigEndian: true, byteOrderMark: false, throwOnInvalidCharacters: true ) ) )
				{
					Assert.Throws<DecoderFallbackException>( () => result.Read() );
				}
			}
#else
			using ( var stream = new MemoryStream( new byte[] { 0xA5, 0xF8, 0x88, 0x80, 0x80, 0x80 } ) )
			{
				using ( var result = Unpacking.UnpackCharStream( stream, new UTF8Encoding( encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true ) ) )
				{
					Assert.Throws<DecoderFallbackException>( () => result.Read() );
				}
			}
#endif // !NETFX_CORE && !SILVERLIGHT
		}

		[Test]
		public void TestUnpackCharStream_Stream_Encoding_StreamIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackCharStream( null, Encoding.UTF8 ) );
		}

		[Test]
		public void TestUnpackCharStream_Stream_Encoding_EncodingIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackCharStream( new MemoryStream( new byte[] { 0xA1, ( byte )'A' } ), null ) );
		}


		[Test]
		public void TestUnpackObject_ByteArray_Scalar_AsIs()
		{
			var result = Unpacking.UnpackObject( new byte[] { 0x1 } );
			Assert.That( result.ReadCount, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Equals( 1 ) );
		}

		[Test]
		public void TestUnpackObject_ByteArray_Int32_Scalar_AsIs()
		{
			var result = Unpacking.UnpackObject( new byte[] { 0xFF, 0x1, 0xFF }, 1 );
			Assert.That( result.ReadCount, Is.EqualTo( 1 ) );
			Assert.That( result.Value.Equals( 1 ) );
		}

		[Test]
		public void TestUnpackObject_ByteArray_ByteArrayIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackObject( default( byte[] ) ) );
		}

		[Test]
		public void TestUnpackObject_ByteArray_ByteArrayIsEmpty()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackObject( new byte[ 0 ] ) );
		}

		[Test]
		public void TestUnpackObject_ByteArray_Int32_ByteArrayIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackObject( null, 0 ) );
		}

		[Test]
		public void TestUnpackObject_ByteArray_Int32_ByteArrayIsEmpty()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackObject( new byte[ 0 ], 0 ) );
		}

		[Test]
		public void TestUnpackObject_ByteArray_Int32_OffsetIsTooBig()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackObject( new byte[] { 0x1 }, 1 ) );
		}

		[Test]
		public void TestUnpackObject_ByteArray_Int32_OffsetIsNegative()
		{
			Assert.Throws<ArgumentOutOfRangeException>( () => Unpacking.UnpackObject( new byte[] { 0x1 }, -1 ) );
		}

		[Test]
		public void TestUnpackObject_Stream_Scalar_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x1 } ) )
			{
				Assert.That( Unpacking.UnpackObject( stream ).Equals( 1 ) );
			}
		}

		[Test]
		public void TestUnpackObject_Stream_Nil_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xC0 } ) )
			{
				Assert.That( Unpacking.UnpackObject( stream ).IsNil );
			}
		}

		[Test]
		public void TestUnpackObject_Stream_EmptyArray_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x90 } ) )
			{
				var result = Unpacking.UnpackObject( stream );
				Assert.That( result.IsArray, Is.True );
				Assert.That( result.IsMap, Is.False );
				Assert.That( result.IsRaw, Is.False );
				Assert.That( result.AsList(), Is.Not.Null.And.Empty );
			}
		}

		[Test]
		public void TestUnpackObject_Stream_Array_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			{
				var result = Unpacking.UnpackObject( stream );
				Assert.That( result.IsArray, Is.True );
				Assert.That( result.IsMap, Is.False );
				Assert.That( result.IsRaw, Is.False );
				Assert.That( result.AsList(), Is.Not.Null.And.Length.EqualTo( 1 ).And.All.EqualTo( new MessagePackObject( 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackObject_Stream_EmptyMap_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x80 } ) )
			{
				var result = Unpacking.UnpackObject( stream );
				Assert.That( result.IsArray, Is.False );
				Assert.That( result.IsMap, Is.True );
				Assert.That( result.IsRaw, Is.False );
				Assert.That( result.AsDictionary(), Is.Not.Null.And.Empty );
			}
		}

		[Test]
		public void TestUnpackObject_Stream_Map_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x81, 0x1, 0x1 } ) )
			{
				var result = Unpacking.UnpackObject( stream );
				Assert.That( result.IsArray, Is.False );
				Assert.That( result.IsMap, Is.True );
				Assert.That( result.IsRaw, Is.False );
				Assert.That( result.AsDictionary(), Is.Not.Null.And.Count.EqualTo( 1 ).And.All.EqualTo( new KeyValuePair<MessagePackObject, MessagePackObject>( 1, 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackObject_Stream_EmptyRaw_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA0 } ) )
			{
				var result = Unpacking.UnpackObject( stream );
				Assert.That( result.IsArray, Is.False );
				Assert.That( result.IsMap, Is.False );
				Assert.That( result.IsRaw, Is.True );
				Assert.That( result.AsBinary(), Is.Not.Null.And.Empty );
				Assert.That( result.AsString(), Is.Not.Null.And.Empty );
			}
		}

		[Test]
		public void TestUnpackObject_Stream_Raw_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA1, ( byte )'A' } ) )
			{
				var result = Unpacking.UnpackObject( stream );
				Assert.That( result.IsArray, Is.False );
				Assert.That( result.IsMap, Is.False );
				Assert.That( result.IsRaw, Is.True );
				Assert.That( result.AsBinary(), Is.EqualTo( new byte[] { ( byte )'A' } ) );
				Assert.That( result.AsString(), Is.EqualTo( "A" ) );
			}
		}

		private static void AssertNested( IEnumerable<MessagePackObject> values )
		{
			var result = values.ToArray();
			Assert.That( result.Length, Is.EqualTo( 6 ) );
			Assert.That( result[ 0 ].AsList(), Is.Not.Null.And.Empty );
			Assert.That( result[ 1 ].AsList(), Is.Not.Null.And.Length.EqualTo( 1 ).And.All.EqualTo( new MessagePackObject( 1 ) ) );
			Assert.That( result[ 2 ].AsDictionary(), Is.Not.Null.And.Empty );
			Assert.That( result[ 3 ].AsDictionary(), Is.Not.Null.And.Count.EqualTo( 1 ).And.All.EqualTo( new KeyValuePair<MessagePackObject, MessagePackObject>( 1, 1 ) ) );
			Assert.That( result[ 4 ].AsString(), Is.Not.Null.And.Empty );
			Assert.That( result[ 5 ].AsString(), Is.EqualTo( "A" ) );
		}

		[Test]
		public void TestUnpackObject_Stream_NestedArray_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x96, 0x90, 0x91, 0x1, 0x80, 0x81, 0x1, 0x1, 0xA0, 0xA1, ( byte )'A' } ) )
			{
				var result = Unpacking.UnpackObject( stream );
				AssertNested( result.AsList() );
			}
		}

		[Test]
		public void TestUnpackObject_Stream_NestedMap_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x86, 0x1, 0x90, 0x2, 0x91, 0x1, 0x3, 0x80, 0x4, 0x81, 0x1, 0x1, 0x5, 0xA0, 0x6, 0xA1, ( byte )'A' } ) )
			{
				var result = Unpacking.UnpackObject( stream );
				var dictionary = result.AsDictionary();
				Assert.That( dictionary, Is.Not.Null.And.Count.EqualTo( 6 ) );
				AssertNested(
					new MessagePackObject[] 
					{ 
						dictionary [ 1 ],
						dictionary [ 2 ],
						dictionary [ 3 ],
						dictionary [ 4 ],
						dictionary [ 5 ],
						dictionary [ 6 ]
					}
				);
			}
		}

		[Test]
		public void TestUnpackObject_Stream_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackObject( default( Stream ) ) );
		}


		// Edge cases

		[Test]
		public void TestUnpackInt32_NotNumeric()
		{
			Assert.Throws<MessageTypeException>( () => Unpacking.UnpackInt32( new byte[] { 0x80 } ) );
		}

		[Test]
		public void TestUnpackInt32_Eof()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackInt32( new byte[] { 0xD0 } ) );
		}

		[Test]
		public void TestUnpackDictionary_KeyDuplicated()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackDictionary( new byte[] { 0x82, 0x1, 0x0, 0x1, 0x0 } ) );
		}

		[Test]
		public void TestUnpackArray_Eof()
		{
			Assert.Throws<UnpackException>( () => Unpacking.UnpackArray( new byte[] { 0x91 } ) );
		}

		[Test]
		public void TestUnpackBinary_EofInHeader()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( new byte[] { 0xDA, 0x1 } ) );
		}

		[Test]
		public void TestUnpackBinary_EofInBody()
		{
			Assert.Throws<InvalidMessagePackStreamException>( () => Unpacking.UnpackBinary( new byte[] { 0xA1 } ) );
		}

		[Test]
		public void TestUnpackByteStream_Empty()
		{
			Assert.Throws<UnpackException>( () => Unpacking.UnpackByteStream( new MemoryStream() ) );
		}

		[Test]
		public void TestUnpackByteStream_EofInHeader()
		{
			using ( var underlying = new MemoryStream( new byte[] { 0xDA, 0x1 } ) )
			{
				Assert.Throws<UnpackException>( () => Unpacking.UnpackByteStream( underlying ) );
			}
		}

		[Test]
		public void TestUnpackByteStream_EofInBody_CanFeed()
		{
			using ( var underlying = new MemoryStream() )
			{
				underlying.WriteByte( 0xA1 );
				underlying.Position = 0;
				var target = Unpacking.UnpackByteStream( underlying );
				Assert.That( target.Length, Is.EqualTo( 1 ) );
				// Check precondition
				var b = target.ReadByte();
				Assert.That( b, Is.EqualTo( -1 ) );

				// Feed
				// Assume that underlying supports Feed (appends bytes to tail, and does not move Position).
				underlying.WriteByte( 0x57 );
				underlying.Position -= 1;
				b = target.ReadByte();
				Assert.That( b, Is.EqualTo( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackByteStream_NotRaw()
		{
			Assert.Throws<MessageTypeException>( () => Unpacking.UnpackByteStream( new MemoryStream( new byte[] { 0x80 } ) ) );
		}

		[Test]
		public void TestUnpackByteStream_Nil_AsEmpty()
		{
			var target = Unpacking.UnpackByteStream( new MemoryStream( new byte[] { 0xC0 } ) );
			Assert.That( target, Is.Not.Null );
			Assert.That( target.Length, Is.EqualTo( 0 ) );
		}


		private static Stream CreateStreamForByteStreamTest()
		{
			// Positin == Value
			return Unpacking.UnpackByteStream( new MemoryStream( new byte[] { 0xA3, 0x0, 0x1, 0x2 } ) );
		}

		[Test]
		public void TestUnpackBinaryResultStreamIsNotWriteable_CanWrite_False()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				Assert.IsFalse( target.CanWrite );
			}
		}

		[Test]
		public void TestUnpackBinaryResultStreamIsNotWriteable_Flush_Nop()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				target.Flush();
			}
		}

		[Test]
		public void TestUnpackBinaryResultStreamIsNotWriteable_Write_Fail()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				Assert.Throws<NotSupportedException>( () => target.Write( new byte[] { 0x0 }, 0, 1 ) );
			}
		}

		[Test]
		public void TestUnpackBinaryResultStreamIsNotWriteable_WriteByte_Fail()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				Assert.Throws<NotSupportedException>( () => target.WriteByte( 0 ) );
			}
		}

		[Test]
		public void TestUnpackBinaryResultStreamIsNotWriteable_SetLength_Fail()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				Assert.Throws<NotSupportedException>( () => target.SetLength( 1 ) );
			}
		}


		private static void AssertStreamPosition( Stream target, int position )
		{
			// ReadByte() should return the value which equals to Position except tail.
			AssertStreamPosition( target, position, position );
		}

		private static void AssertStreamPosition( Stream target, int position, int expectedValue )
		{
			Assert.That( target.Position, Is.EqualTo( position ) );
			Assert.That( target.ReadByte(), Is.EqualTo( expectedValue ) );
		}

		[Test]
		public void TestSeekableByteStream_Seek_0_Begin_Head()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				// Forward.
				target.ReadByte();
				target.Seek( 0, SeekOrigin.Begin );
				AssertStreamPosition( target, 0 );
			}
		}

		[Test]
		public void TestSeekableByteStream_Seek_1_Begin_HeadPlus1()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				// Forward.
				target.ReadByte();
				target.Seek( 1, SeekOrigin.Begin );
				AssertStreamPosition( target, 1 );
			}
		}

		[Test]
		public void TestSeekableByteStream_Seek_Minus1_Begin_Fail()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				// Forward.
				target.ReadByte();
				Assert.Throws<IOException>( () => target.Seek( -1, SeekOrigin.Begin ) );
			}
		}

		[Test]
		public void TestSeekableByteStream_setPosition_0_Head()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				// Forward.
				target.ReadByte();
				target.Position = 0;
				AssertStreamPosition( target, 0 );
			}
		}

		[Test]
		public void TestSeekableByteStream_setPosition_1_HeadPlus1()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				// Forward.
				target.ReadByte();
				target.Position = 1;
				AssertStreamPosition( target, 1 );
			}
		}

		[Test]
		public void TestSeekableByteStream_setPosition_Minus1_Fail()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				// Forward.
				target.ReadByte();
				Assert.Throws<IOException>( () => target.Position = -1 );
			}
		}

		[Test]
		public void TestSeekableByteStream_Seek_0_Current_DoesNotMove()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				// Forward.
				target.ReadByte();
				target.Seek( 0, SeekOrigin.Current );
				AssertStreamPosition( target, 1 );
			}
		}

		[Test]
		public void TestSeekableByteStream_Seek_1_Current_Plus1()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				// Forward.
				target.ReadByte();
				target.Seek( 1, SeekOrigin.Current );
				AssertStreamPosition( target, 2 );
			}
		}

		[Test]
		public void TestSeekableByteStream_Seek_Minus1_Current_Minus1()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				// Forward.
				target.ReadByte();
				target.Seek( -1, SeekOrigin.Current );
				AssertStreamPosition( target, 0 );
			}
		}

		[Test]
		public void TestSeekableByteStream_Seek_0_End_Tail()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				// Forward.
				target.ReadByte();
				target.Seek( 0, SeekOrigin.End );
				AssertStreamPosition( target, 3, -1 );
			}
		}

		[Test]
		public void TestSeekableByteStream_Seek_1_End_Fail()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				// Forward.
				target.ReadByte();
				Assert.Throws<NotSupportedException>( () => target.Seek( 1, SeekOrigin.End ) );
			}
		}

		[Test]
		public void TestSeekableByteStream_Seek_Minus1_End_TailMinus1()
		{
			using ( var target = CreateStreamForByteStreamTest() )
			{
				// Forward.
				target.ReadByte();
				target.Seek( -1, SeekOrigin.End );
				AssertStreamPosition( target, 2 );
			}
		}


		[Test]
		public void TestUnseekableByteStream_Seek()
		{
			using ( var target = Unpacking.UnpackByteStream( new WrapperStream( new MemoryStream( new byte[] { 0xA1, 0xFF } ), canSeek: false ) ) )
			{
				Assert.Throws<NotSupportedException>( () => target.Seek( 0, SeekOrigin.Current ) );
			}
		}

		[Test]
		public void TestUnseekableByteStream_getPosition()
		{
			using ( var target = Unpacking.UnpackByteStream( new WrapperStream( new MemoryStream( new byte[] { 0xA1, 0xFF } ), canSeek: false ) ) )
			{
				Assert.Throws<NotSupportedException>( () => { var dummy = target.Position; } );
			}
		}

		[Test]
		public void TestUnseekableByteStream_setPosition()
		{
			using ( var target = Unpacking.UnpackByteStream( new WrapperStream( new MemoryStream( new byte[] { 0xA1, 0xFF } ), canSeek: false ) ) )
			{
				Assert.Throws<NotSupportedException>( () => target.Position = 0 );
			}
		}

		private sealed class WrapperStream : Stream
		{
			private readonly Stream _underlying;
			private bool _canRead;
			private bool _canWrite;
			private bool _canSeek;

			public override bool CanRead
			{
				get { return this._canRead; }
			}

			public override bool CanWrite
			{
				get { return this._canWrite; }
			}

			public override bool CanSeek
			{
				get { return this._canSeek; }
			}


			public override long Position
			{
				get
				{
					VerifyState( this.CanSeek );
					return this._underlying.Position;
				}
				set
				{
					VerifyState( this.CanSeek );
					this._underlying.Position = value;
				}
			}

			public override long Length
			{
				get
				{
					VerifyState( this.CanSeek );
					return this._underlying.Length;
				}
			}

			public WrapperStream( Stream stream, bool canRead = true, bool canWrite = true, bool canSeek = true )
			{
				this._underlying = stream;
				this._canRead = canRead;
				this._canWrite = canWrite;
				this._canSeek = canSeek;
			}

			protected override void Dispose( bool disposing )
			{
				if ( disposing )
				{
					this._underlying.Dispose();
				}

				base.Dispose( disposing );
			}

			public override int Read( byte[] buffer, int offset, int count )
			{
				VerifyState( this.CanRead );
				return this._underlying.Read( buffer, offset, count );
			}

			public override long Seek( long offset, SeekOrigin origin )
			{
				VerifyState( this.CanSeek );
				return this._underlying.Seek( offset, origin );
			}

			public override void Write( byte[] buffer, int offset, int count )
			{
				VerifyState( this.CanWrite );
				this._underlying.Write( buffer, offset, count );
			}

			public override void Flush()
			{
				this._underlying.Flush();
			}

			public override void SetLength( long length )
			{
				VerifyState( this.CanSeek );
				VerifyState( this.CanWrite );
				this._underlying.SetLength( length );
			}

			private static void VerifyState( bool precondition )
			{
				if ( !precondition )
				{
					throw new NotSupportedException();
				}
			}
		}
	}
}
