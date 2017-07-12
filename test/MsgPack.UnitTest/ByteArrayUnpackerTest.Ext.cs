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
	// This file was generated from ByteArrayUnpackerTest.Ext.tt T4Template.
	// Do not modify this file. Edit ByteArrayUnpackerTest.Ext.tt instead.

	partial class ByteArrayUnpackerTest
	{

		[Test]
		public void TestRead_FixExt1_AndBinaryLengthIs1_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD4, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt1_AndBinaryLengthIs1_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD4, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_FixExt2_AndBinaryLengthIs2_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD5, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 2 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt2_AndBinaryLengthIs2_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD5, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 2 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_FixExt4_AndBinaryLengthIs4_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD6, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 4 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 4 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt4_AndBinaryLengthIs4_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD6, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 4 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 4 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_FixExt8_AndBinaryLengthIs8_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD7, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 8 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 8 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt8_AndBinaryLengthIs8_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD7, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 8 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 8 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_FixExt16_AndBinaryLengthIs16_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD8, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( unpacker.Read() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 16 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt16_AndBinaryLengthIs16_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD8, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 16 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs0_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC7, 0, typeCode }
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

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs0_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC7, 0, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs255_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC7, 0xFF, typeCode }
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

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs255_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC7, 0xFF, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs0_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC8, 0, 0, typeCode }
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

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs0_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC8, 0, 0, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs65535_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
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

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65535 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs65535_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs0_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC9, 0, 0, 0, 0, typeCode }
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

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs0_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC9, 0, 0, 0, 0, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs65536_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
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

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65536 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs65536_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65536 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

#if FEATURE_TAP

		[Test]
		public async Task TestReadAsync_FixExt1_AndBinaryLengthIs1_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD4, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt1_AndBinaryLengthIs1_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD4, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt2_AndBinaryLengthIs2_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD5, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 2 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt2_AndBinaryLengthIs2_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD5, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 2 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt4_AndBinaryLengthIs4_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD6, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 4 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 4 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt4_AndBinaryLengthIs4_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD6, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 4 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 4 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt8_AndBinaryLengthIs8_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD7, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 8 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 8 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt8_AndBinaryLengthIs8_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD7, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 8 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 8 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt16_AndBinaryLengthIs16_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD8, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				Assert.IsTrue( await unpacker.ReadAsync() );

#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 16 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt16_AndBinaryLengthIs16_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xD8, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 16 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext8_AndBinaryLengthIs0_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC7, 0, typeCode }
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

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs0_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC7, 0, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext8_AndBinaryLengthIs255_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC7, 0xFF, typeCode }
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

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs255_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC7, 0xFF, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs0_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC8, 0, 0, typeCode }
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

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs0_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC8, 0, 0, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs65535_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
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

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65535 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs65535_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs0_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC9, 0, 0, 0, 0, typeCode }
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

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs0_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC9, 0, 0, 0, 0, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs65536_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
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

				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65536 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs65536_Extra()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			var data =
				new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
				.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray();
			using( var unpacker = this.CreateUnpacker( PrependAppendExtra( data ), 1 ) )
			{
				// Verify initial offset (prepended bytes length)
				Assert.That( unpacker.Offset, Is.EqualTo( 1 ) );

				MessagePackExtendedTypeObject result;

				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;

				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65536 ) );

				// -1 is prepended extra bytes length
				Assert.That( unpacker.Offset - 1, Is.EqualTo( data.Length ) );
			}
		}

#endif // FEATURE_TAP

	}
}
