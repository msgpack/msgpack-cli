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

using System;
using System.IO;
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
	// ReSharper disable once InconsistentNaming
	[TestFixture]
	[Timeout( 1000 )]
	public partial class UnpackerTest_Skip
	{
		[Test]
		public void TestSkip_Empty_Null()
		{
			using ( var stream = new MemoryStream( new byte[ 0 ] ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.Null );
			}
		}

		[Test]
		public void TestSkip_FixNum_1()
		{
			using ( var stream = new MemoryStream( new byte[] { 1 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_Scalar_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xD2, 0x1, 0x2, 0x3, 0x4 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_ContinuousScalar_ReportedSeparately()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xD2, 0x1, 0x2, 0x3, 0x4, 0x1 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( 5L ) );
				Assert.That( target.Skip(), Is.EqualTo( 1L ) );
			}
		}

		[Test]
		public void TestSkip_RawEmpty_0()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA0 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_FixRaw_1()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA1, ( byte )'A' } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_Raw_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xDB, 0x0, 0x0, 0x0, 0x1, ( byte )'A' } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_ContinuousRaw_ReportedSeparately()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xA2, ( byte )'A', ( byte )'B', 0xA2, ( byte )'C', ( byte )'D' } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( 3L ) );
				Assert.That( target.Skip(), Is.EqualTo( 3L ) );
			}
		}


		[Test]
		public void TestSkip_FixArrayEmpty_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x90 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_FixArrayFixNum_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x92, 0x1, 0x2 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_ArrayEmpty_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xDD, 0x0, 0x0, 0x0, 0x0 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_ArrayScalar_AsIs()
		{
			using ( var stream = new MemoryStream() )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				stream.Feed( new byte[] { 0xDD, 0x0, 0x0, 0x0, 0x2, 0xD2, 0x1, 0x2, 0x3, 0x4, 0xD2, 0x1, 0x2, 0x3, 0x4 } );
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_CotinuousArray_ReportsSeparately()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x92, 0x1, 0x2, 0x91, 0x3 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( 3L ) );
				Assert.That( target.Skip(), Is.EqualTo( 2L ) );
			}
		}

		[Test]
		public void TestSkip_NestedArray_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x92, 0x91, 0x1, 0x91, 0x1 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_NestedArrayContainsEmpty_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x92, 0x90, 0x91, 0x1 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}


		[Test]
		public void TestSkip_FixMapEmpty_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x80 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_FixMapFixNum_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x82, 0x1, 0x1, 0x2, 0x2 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_MapEmpty_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0xDF, 0x0, 0x0, 0x0, 0x0 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_MapScalar_AsIs()
		{
			using ( var stream = new MemoryStream() )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				stream.Feed( new byte[] { 0xDE, 0x0, 0x2, 0xD0, 0x1, 0xD0, 0x1, 0xD0, 0x2, 0xD0, 0x2 } );
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}
		
		[Test]
		public void TestSkip_CotinuousMap_ReportsSeparately()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x82, 0x1, 0x1, 0x2, 0x2, 0x81, 0x3, 0x3 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( 5L ) );
				Assert.That( target.Skip(), Is.EqualTo( 3L ) );
			}
		}

		[Test]
		public void TestSkip_NestedMap_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x82, 0x1, 0x81, 0x2, 0x2, 0x3, 0x81, 0x4, 0x4 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}

		[Test]
		public void TestSkip_SubtreeReader_NestedMapContainsEmpty_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x82, 0x1, 0x80, 0x2, 0x81, 0x3, 0x3 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Skip(), Is.EqualTo( stream.Length ) );
			}
		}


		[Test]
		public void TestSkip_SubtreeReader_NestedArray_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x92, 0x91, 0x1, 0x91, 0x1 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Read() );
				Assert.That( target.Read() );

				using ( var subTreeReader = target.ReadSubtree() )
				{
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
				}

				Assert.That( target.Read() );

				using ( var subTreeReader = target.ReadSubtree() )
				{
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
				}
			}
		}

		[Test]
		public void TestSkip_SubtreeReader_NestedArrayContainsEmpty_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x92, 0x90, 0x91, 0x1 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Read() );
				Assert.That( target.Read() );

				using ( var subTreeReader = target.ReadSubtree() )
				{
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 0 ) );
				}

				Assert.That( target.Read() );

				using ( var subTreeReader = target.ReadSubtree() )
				{
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
				}
			}
		}

		[Test]
		public void TestSkip_BetweenSubtreeReader_NestedArray_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x93, 0x91, 0x1, 0x2, 0x91, 0x1 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Read() );
				Assert.That( target.Read() );

				using ( var subTreeReader = target.ReadSubtree() )
				{
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
				}

				Assert.That( target.Skip(), Is.EqualTo( 1 ) );
				Assert.That( target.Read() );

				using ( var subTreeReader = target.ReadSubtree() )
				{
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
				}
			}
		}

		[Test]
		public void TestSkip_SubtreeReader_NestedMap_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x82, 0x1, 0x81, 0x2, 0x2, 0x3, 0x81, 0x4, 0x4 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Read() );
				Assert.That( target.IsMapHeader );
				Assert.That( target.Read() );
				Assert.That( target.LastReadData.Equals( 0x1 ) );
				Assert.That( target.Read() );

				using ( var subTreeReader = target.ReadSubtree() )
				{
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
				}

				Assert.That( target.Read() );
				Assert.That( target.LastReadData.Equals( 0x3 ) );
				Assert.That( target.Read() );

				using ( var subTreeReader = target.ReadSubtree() )
				{
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
				}
			}
		}

		[Test]
		public void TestSkip_NestedMapContainsEmpty_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x82, 0x1, 0x80, 0x2, 0x81, 0x3, 0x3 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Read() );
				Assert.That( target.IsMapHeader );
				Assert.That( target.Read() );
				Assert.That( target.LastReadData.Equals( 0x1 ) );
				Assert.That( target.Read() );

				using ( var subTreeReader = target.ReadSubtree() )
				{
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 0 ) );
				}

				Assert.That( target.Read() );
				Assert.That( target.LastReadData.Equals( 0x2 ) );
				Assert.That( target.Read() );

				using ( var subTreeReader = target.ReadSubtree() )
				{
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
				}
			}
		}

		[Test]
		public void TestSkip_BetweenSubtreeReader_NestedMap_AsIs()
		{
			using ( var stream = new MemoryStream( new byte[] { 0x83, 0x1, 0x81, 0x2, 0x2, 0x3, 0x3, 0x4, 0x81, 0x4, 0x4 } ) )
			using ( var target = Unpacker.Create( stream, false ) )
			{
				Assert.That( target.Read() );
				Assert.That( target.Skip(), Is.EqualTo( 1 ) );
				Assert.That( target.Read() );

				using ( var subTreeReader = target.ReadSubtree() )
				{
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
				}

				Assert.That( target.Skip(), Is.EqualTo( 1 ) );
				Assert.That( target.Skip(), Is.EqualTo( 1 ) );
				Assert.That( target.Skip(), Is.EqualTo( 1 ) );
				Assert.That( target.Read() );

				using ( var subTreeReader = target.ReadSubtree() )
				{
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
					Assert.That( subTreeReader.Skip(), Is.EqualTo( 1 ) );
				}
			}
		}
	}

	// TODO: NLiblet
	internal static class StreamExtensions
	{
		public static void Write( this Stream source, byte[] buffer )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( buffer == null )
			{
				throw new ArgumentNullException( "buffer" );
			}

			source.Write( buffer, 0, buffer.Length );
		}

		public static void Feed( this Stream source, byte[] buffer )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( buffer == null )
			{
				throw new ArgumentNullException( "buffer" );
			}

			if ( !source.CanSeek )
			{
				throw new NotSupportedException();
			}

			source.Write( buffer, 0, buffer.Length );
			source.Position -= buffer.Length;
		}

		public static void Feed( this Stream source, byte value )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( !source.CanSeek )
			{
				throw new NotSupportedException();
			}

			source.WriteByte( value );
			source.Position--;
		}
	}
}
