#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Linq;
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
	// This file was generated from UnpackerTest.Raw.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit UnpackerTest.Raw.tt and StreamingUnapkcerBase.ttinclude instead.

	[TestFixture]
	public partial class UnpackerTest_Raw
	{

		[Test]
		public void TestUnpackFixStr_0_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_0_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_0_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_0_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_0_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_0_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_0_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_0_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_0_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_1_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_1_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_1_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_1_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_1_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_1_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_1_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_1_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_1_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_1_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_1_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_1_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_31_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_31_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_31_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_31_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_31_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_31_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_31_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_31_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_31_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_31_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_31_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_31_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_32_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_32_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_32_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_32_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_32_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_32_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_32_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_32_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_32_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_32_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_32_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_32_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_255_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_255_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_255_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_255_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_255_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_255_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_255_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_255_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_255_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_255_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_255_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr8_255_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_0_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_0_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_0_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_0_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_0_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_0_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_0_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_0_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_1_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_1_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_1_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_1_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_1_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_1_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_1_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_1_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_1_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_1_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_1_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_1_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_31_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_31_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_31_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_31_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_31_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_31_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_31_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_31_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_31_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_31_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_31_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_31_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_32_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_32_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_32_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_32_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_32_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_32_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_32_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_32_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_32_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_32_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_32_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_32_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_255_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_255_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_255_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_255_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_255_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_255_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_255_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_255_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_255_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_255_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_255_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_255_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_256_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_256_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_256_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_256_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_256_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_256_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_256_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_256_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_256_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_256_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_256_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_256_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_0_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_0_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_0_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_0_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_0_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_0_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_0_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_0_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_1_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_1_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_1_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_1_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_1_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_1_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_1_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_1_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_1_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_1_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_1_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_1_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_31_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_31_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_31_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_31_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_31_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_31_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_31_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_31_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_31_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_31_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_31_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_31_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_32_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_32_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_32_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_32_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_32_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_32_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_32_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_32_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_32_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_32_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_32_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_32_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_255_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_255_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_255_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_255_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_255_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_255_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_255_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_255_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_255_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_255_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_255_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_255_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_256_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_256_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_256_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_256_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_256_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_256_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_256_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_256_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_256_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_256_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_256_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_256_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_0_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_0_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_0_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_0_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_0_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_0_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_0_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_0_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_1_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_1_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_1_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_1_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_1_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_1_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_1_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_1_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_1_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_1_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_1_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_1_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_31_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_31_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_31_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_31_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_31_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_31_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_31_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_31_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_31_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_31_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_31_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_31_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_32_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_32_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_32_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_32_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_32_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_32_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_32_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_32_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_32_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_32_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_32_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_32_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_255_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_255_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_255_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_255_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_255_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_255_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_255_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_255_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_255_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_255_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_255_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin8_255_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_0_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_0_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_0_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_0_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_0_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_0_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_0_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_0_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_1_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_1_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_1_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_1_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_1_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_1_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_1_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_1_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_1_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_1_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_1_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_1_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_31_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_31_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_31_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_31_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_31_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_31_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_31_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_31_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_31_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_31_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_31_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_31_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_32_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_32_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_32_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_32_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_32_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_32_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_32_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_32_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_32_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_32_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_32_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_32_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_255_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_255_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_255_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_255_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_255_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_255_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_255_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_255_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_255_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_255_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_255_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_255_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_256_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_256_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_256_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_256_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_256_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_256_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_256_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_256_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_256_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_256_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_256_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_256_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_0_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_0_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_0_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_0_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_0_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_0_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_0_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_0_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_1_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_1_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_1_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_1_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_1_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_1_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_1_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_1_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_1_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_1_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_1_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_1_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_31_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_31_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_31_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_31_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_31_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_31_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_31_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_31_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_31_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_31_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_31_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_31_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_32_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_32_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_32_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_32_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_32_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_32_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_32_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_32_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_32_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_32_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_32_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_32_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_255_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_255_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_255_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_255_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_255_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_255_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_255_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_255_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_255_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_255_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_255_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_255_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_256_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_256_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_256_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_256_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_256_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_256_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_256_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_256_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_256_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_256_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_256_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_256_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_AsString_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_AsString_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_AsString_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_AsString_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_AsString_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_AsString_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_ReadString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_ReadString_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_ReadString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_ReadString_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_ReadString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_ReadString_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_0_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_1_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackFixStr_31_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_0_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_0_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_0_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_0_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_0_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_0_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_0_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_0_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_1_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_1_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_1_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_1_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_1_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_1_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_1_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_1_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_1_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_1_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_1_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_1_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_31_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_31_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_31_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_31_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_31_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_31_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_31_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_31_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_31_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_31_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_31_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_31_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_32_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_32_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_32_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_32_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_32_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_32_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_32_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_32_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_32_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_32_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_32_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_32_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_255_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_255_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_255_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_255_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr8_255_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_255_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr8_255_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_255_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_255_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_255_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr8_255_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr8_255_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_0_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_0_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_0_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_0_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_0_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_0_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_0_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_0_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_1_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_1_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_1_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_1_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_1_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_1_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_1_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_1_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_1_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_1_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_1_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_1_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_31_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_31_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_31_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_31_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_31_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_31_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_31_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_31_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_31_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_31_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_31_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_31_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_32_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_32_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_32_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_32_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_32_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_32_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_32_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_32_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_32_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_32_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_32_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_32_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_255_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_255_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_255_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_255_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_255_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_255_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_255_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_255_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_255_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_255_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_255_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_255_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_256_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_256_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_256_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_256_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_256_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_256_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_256_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_256_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_256_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_256_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_256_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_256_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr16_65535_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_0_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_0_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_0_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_0_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_0_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_0_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_0_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_0_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_1_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_1_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_1_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_1_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_1_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_1_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_1_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_1_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_1_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_1_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_1_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_1_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_31_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_31_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_31_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_31_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_31_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_31_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_31_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_31_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_31_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_31_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_31_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_31_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_32_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_32_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_32_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_32_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_32_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_32_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_32_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_32_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_32_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_32_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_32_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_32_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_255_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_255_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_255_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_255_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_255_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_255_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_255_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_255_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_255_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_255_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_255_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_255_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_256_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_256_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_256_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_256_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_256_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_256_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_256_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_256_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_256_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_256_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_256_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_256_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65535_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackStr32_65536_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_0_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_0_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_0_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_0_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_0_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_0_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_0_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_0_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_1_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_1_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_1_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_1_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_1_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_1_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_1_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_1_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_1_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_1_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_1_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_1_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_31_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_31_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_31_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_31_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_31_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_31_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_31_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_31_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_31_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_31_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_31_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_31_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_32_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_32_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_32_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_32_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_32_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_32_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_32_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_32_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_32_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_32_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_32_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_32_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_255_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_255_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_255_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_255_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin8_255_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_255_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin8_255_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_255_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_255_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_255_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin8_255_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin8_255_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_0_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_0_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_0_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_0_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_0_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_0_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_0_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_0_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_1_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_1_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_1_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_1_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_1_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_1_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_1_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_1_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_1_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_1_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_1_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_1_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_31_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_31_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_31_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_31_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_31_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_31_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_31_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_31_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_31_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_31_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_31_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_31_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_32_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_32_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_32_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_32_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_32_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_32_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_32_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_32_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_32_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_32_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_32_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_32_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_255_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_255_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_255_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_255_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_255_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_255_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_255_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_255_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_255_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_255_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_255_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_255_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_256_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_256_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_256_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_256_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_256_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_256_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_256_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_256_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_256_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_256_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_256_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_256_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin16_65535_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_0_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_0_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_0_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_0_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_0_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_0_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_0_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_0_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_1_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_1_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_1_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_1_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_1_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_1_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_1_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_1_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_1_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_1_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_1_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_1_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_31_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_31_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_31_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_31_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_31_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_31_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_31_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_31_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_31_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_31_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_31_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_31_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_32_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_32_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_32_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_32_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_32_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_32_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_32_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_32_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_32_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_32_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_32_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_32_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_255_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_255_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_255_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_255_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_255_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_255_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_255_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_255_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_255_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_255_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_255_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_255_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_256_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_256_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_256_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_256_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_256_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_256_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_256_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_256_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_256_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_256_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_256_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_256_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65535_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_AsBinary_Read_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_AsBinary_Read_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_AsBinary_Read_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_AsBinary_Read_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_AsBinary_Read_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_AsBinary_Read_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_ReadBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_ReadBinary_JustLength_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_ReadBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_ReadBinary_TooShort_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_ReadBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = Unpacker.Create( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestUnpackBin32_65536_ReadBinary_HasExtra_Splitted()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var splitted = new SplittingStream( buffer ) )
			using( var unpacker = Unpacker.Create( splitted ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}
	}
}
