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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
	// This file was generated from ByteArrayUnpackerTest.Raw.tt T4Template.
	// Do not modify this file. Edit ByteArrayUnpackerTest.Raw.tt instead.

	partial class ByteArrayUnpackerTest
	{

		[Test]
		public void TestRead_FixStr_0_AsString_Extra()
		{
			var data =
				new byte[] { 0xA0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_FixStr_0_Extra()
		{
			var data =
				new byte[] { 0xA0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_FixStr_31_AsString_Extra()
		{
			var data =
				new byte[] { 0xBF }
				.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_FixStr_31_Extra()
		{
			var data =
				new byte[] { 0xBF }
				.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Str8_0_AsString_Extra()
		{
			var data =
				new byte[] { 0xD9, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_Str8_0_Extra()
		{
			var data =
				new byte[] { 0xD9, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Str8_255_AsString_Extra()
		{
			var data =
				new byte[] { 0xD9, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_Str8_255_Extra()
		{
			var data =
				new byte[] { 0xD9, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Str16_0_AsString_Extra()
		{
			var data =
				new byte[] { 0xDA, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_Str16_0_Extra()
		{
			var data =
				new byte[] { 0xDA, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Str16_65535_AsString_Extra()
		{
			var data =
				new byte[] { 0xDA, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_Str16_65535_Extra()
		{
			var data =
				new byte[] { 0xDA, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Str32_0_AsString_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_Str32_0_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Str32_65536_AsString_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_Str32_65536_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Bin8_0_AsString_Extra()
		{
			var data =
				new byte[] { 0xC4, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_Bin8_0_Extra()
		{
			var data =
				new byte[] { 0xC4, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Bin8_255_AsString_Extra()
		{
			var data =
				new byte[] { 0xC4, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_Bin8_255_Extra()
		{
			var data =
				new byte[] { 0xC4, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Bin16_0_AsString_Extra()
		{
			var data =
				new byte[] { 0xC5, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_Bin16_0_Extra()
		{
			var data =
				new byte[] { 0xC5, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Bin16_65535_AsString_Extra()
		{
			var data =
				new byte[] { 0xC5, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_Bin16_65535_Extra()
		{
			var data =
				new byte[] { 0xC5, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Bin32_0_AsString_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_Bin32_0_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Bin32_65536_AsString_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadString_Bin32_65536_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				Assert.IsTrue( unpacker.ReadString( out result ) );

				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_FixStr_0_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xA0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_FixStr_0_Extra()
		{
			var data =
				new byte[] { 0xA0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_FixStr_31_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xBF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_FixStr_31_Extra()
		{
			var data =
				new byte[] { 0xBF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Str8_0_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xD9, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_Str8_0_Extra()
		{
			var data =
				new byte[] { 0xD9, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Str8_255_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xD9, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_Str8_255_Extra()
		{
			var data =
				new byte[] { 0xD9, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Str16_0_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xDA, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16_0_Extra()
		{
			var data =
				new byte[] { 0xDA, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Str16_65535_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xDA, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16_65535_Extra()
		{
			var data =
				new byte[] { 0xDA, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Str32_0_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32_0_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Str32_65536_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32_65536_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Bin8_0_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xC4, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin8_0_Extra()
		{
			var data =
				new byte[] { 0xC4, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Bin8_255_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xC4, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin8_255_Extra()
		{
			var data =
				new byte[] { 0xC4, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Bin16_0_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xC5, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_0_Extra()
		{
			var data =
				new byte[] { 0xC5, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Bin16_65535_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xC5, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_65535_Extra()
		{
			var data =
				new byte[] { 0xC5, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Bin32_0_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_0_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Bin32_65536_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_65536_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				Assert.IsTrue( unpacker.ReadBinary( out result ) );

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

#if FEATURE_TAP

		[Test]
		public async Task TestRead_FixStr_0Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xA0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_FixStrAsync_0_Extra()
		{
			var data =
				new byte[] { 0xA0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_FixStr_31Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xBF }
				.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_FixStrAsync_31_Extra()
		{
			var data =
				new byte[] { 0xBF }
				.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_0Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xD9, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_Str8Async_0_Extra()
		{
			var data =
				new byte[] { 0xD9, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_255Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xD9, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_Str8Async_255_Extra()
		{
			var data =
				new byte[] { 0xD9, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_0Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xDA, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_0_Extra()
		{
			var data =
				new byte[] { 0xDA, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_65535Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xDA, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_65535_Extra()
		{
			var data =
				new byte[] { 0xDA, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_0Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_0_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_65536Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_65536_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_0Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xC4, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_Bin8Async_0_Extra()
		{
			var data =
				new byte[] { 0xC4, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_255Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xC4, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_Bin8Async_255_Extra()
		{
			var data =
				new byte[] { 0xC4, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_0Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xC5, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_0_Extra()
		{
			var data =
				new byte[] { 0xC5, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_65535Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xC5, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_65535_Extra()
		{
			var data =
				new byte[] { 0xC5, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_0Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_0_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_65536Async_AsString_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_65536_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				String result;

				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_FixStr_0Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xA0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_FixStrAsync_0_Extra()
		{
			var data =
				new byte[] { 0xA0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_FixStr_31Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xBF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_FixStrAsync_31_Extra()
		{
			var data =
				new byte[] { 0xBF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_0Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xD9, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Str8Async_0_Extra()
		{
			var data =
				new byte[] { 0xD9, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_255Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xD9, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Str8Async_255_Extra()
		{
			var data =
				new byte[] { 0xD9, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_0Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xDA, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_0_Extra()
		{
			var data =
				new byte[] { 0xDA, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_65535Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xDA, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_65535_Extra()
		{
			var data =
				new byte[] { 0xDA, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_0Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_0_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_65536Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );


				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;

				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_65536_Extra()
		{
			var data =
				new byte[] { 0xDB, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_0Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xC4, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin8Async_0_Extra()
		{
			var data =
				new byte[] { 0xC4, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_255Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xC4, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin8Async_255_Extra()
		{
			var data =
				new byte[] { 0xC4, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_0Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xC5, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_0_Extra()
		{
			var data =
				new byte[] { 0xC5, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_65535Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xC5, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_65535_Extra()
		{
			var data =
				new byte[] { 0xC5, 0xFF, 0xFF }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_0Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_0_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 0, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_65536Async_AsBinary_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

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

				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_65536_Extra()
		{
			var data =
				new byte[] { 0xC6, 0, 1, 0, 0 }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Byte[] result;

				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

#endif // FEATURE_TAP

	}
}
