#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
	// This file was generated from UnpackerTest.Ext.tt T4Template.
	// Do not modify this file. Edit UnpackerTest.Ext.tt instead.

	partial class UnpackerTest
	{

		[Test]
		public void TestRead_FixExt1_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestRead_FixExt1_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixExt1_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixExt1_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt1_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt1_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt1_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt1_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestRead_FixExt2_AndBinaryLengthIs2JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 2 ) );
			}
		}

		[Test]
		public void TestRead_FixExt2_AndBinaryLengthIs2TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixExt2_AndBinaryLengthIs2TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixExt2_AndBinaryLengthIs2HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 2 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt2_AndBinaryLengthIs2JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 2 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt2_AndBinaryLengthIs2TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt2_AndBinaryLengthIs2TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt2_AndBinaryLengthIs2HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 2 ) );
			}
		}

		[Test]
		public void TestRead_FixExt4_AndBinaryLengthIs4JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 4 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 4 ) );
			}
		}

		[Test]
		public void TestRead_FixExt4_AndBinaryLengthIs4TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixExt4_AndBinaryLengthIs4TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixExt4_AndBinaryLengthIs4HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 5 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 4 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt4_AndBinaryLengthIs4JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 4 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 4 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt4_AndBinaryLengthIs4TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt4_AndBinaryLengthIs4TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt4_AndBinaryLengthIs4HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 5 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 4 ) );
			}
		}

		[Test]
		public void TestRead_FixExt8_AndBinaryLengthIs8JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 8 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 8 ) );
			}
		}

		[Test]
		public void TestRead_FixExt8_AndBinaryLengthIs8TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 7 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixExt8_AndBinaryLengthIs8TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 7 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixExt8_AndBinaryLengthIs8HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 9 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 8 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt8_AndBinaryLengthIs8JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 8 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 8 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt8_AndBinaryLengthIs8TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 7 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt8_AndBinaryLengthIs8TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 7 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt8_AndBinaryLengthIs8HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 9 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 8 ) );
			}
		}

		[Test]
		public void TestRead_FixExt16_AndBinaryLengthIs16JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 16 ) );
			}
		}

		[Test]
		public void TestRead_FixExt16_AndBinaryLengthIs16TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 15 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixExt16_AndBinaryLengthIs16TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 15 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixExt16_AndBinaryLengthIs16HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 16 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt16_AndBinaryLengthIs16JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 16 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt16_AndBinaryLengthIs16TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 15 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt16_AndBinaryLengthIs16TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 15 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_FixExt16_AndBinaryLengthIs16HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 16 ) );
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs17TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs17TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs255TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext8_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs255TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext8_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs17TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs17TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs255TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs255TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs256JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs256TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs256TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs256HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs256JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs256TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs256TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs256HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs65535JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs65535TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs65535TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext16_AndBinaryLengthIs65535HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs65535JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs65535TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs65535TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext16_AndBinaryLengthIs65535HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs17TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs17TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs255TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs255TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs256JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs256TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs256TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs256HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs256JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs256TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs256TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs256HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs65535JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs65535TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs65535TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs65535HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs65535JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs65535TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs65535TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs65535HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs65536JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs65536TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs65536TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Ext32_AndBinaryLengthIs65536HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs65536JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs65536TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs65536TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObject_Ext32_AndBinaryLengthIs65536HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				Assert.IsTrue( unpacker.ReadMessagePackExtendedTypeObject( out result ) );
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65536 ) );
			}
		}

#if FEATURE_TAP

		[Test]
		public async Task TestReadAsync_FixExt1_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadAsync_FixExt1_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_FixExt1_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt1_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt1_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_FixExt1_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_FixExt1_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt1_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD4, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt2_AndBinaryLengthIs2JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 2 ) );
			}
		}

		[Test]
		public void TestReadAsync_FixExt2_AndBinaryLengthIs2TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_FixExt2_AndBinaryLengthIs2TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt2_AndBinaryLengthIs2HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 2 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt2_AndBinaryLengthIs2JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 2 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_FixExt2_AndBinaryLengthIs2TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_FixExt2_AndBinaryLengthIs2TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt2_AndBinaryLengthIs2HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD5, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 2 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt4_AndBinaryLengthIs4JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 4 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 4 ) );
			}
		}

		[Test]
		public void TestReadAsync_FixExt4_AndBinaryLengthIs4TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_FixExt4_AndBinaryLengthIs4TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt4_AndBinaryLengthIs4HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 5 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 4 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt4_AndBinaryLengthIs4JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 4 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 4 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_FixExt4_AndBinaryLengthIs4TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_FixExt4_AndBinaryLengthIs4TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 3 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt4_AndBinaryLengthIs4HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD6, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 5 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 4 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt8_AndBinaryLengthIs8JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 8 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 8 ) );
			}
		}

		[Test]
		public void TestReadAsync_FixExt8_AndBinaryLengthIs8TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 7 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_FixExt8_AndBinaryLengthIs8TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 7 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt8_AndBinaryLengthIs8HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 9 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 8 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt8_AndBinaryLengthIs8JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 8 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 8 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_FixExt8_AndBinaryLengthIs8TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 7 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_FixExt8_AndBinaryLengthIs8TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 7 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt8_AndBinaryLengthIs8HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD7, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 9 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 8 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt16_AndBinaryLengthIs16JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 16 ) );
			}
		}

		[Test]
		public void TestReadAsync_FixExt16_AndBinaryLengthIs16TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 15 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_FixExt16_AndBinaryLengthIs16TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 15 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_FixExt16_AndBinaryLengthIs16HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 16 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt16_AndBinaryLengthIs16JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 16 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_FixExt16_AndBinaryLengthIs16TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 15 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_FixExt16_AndBinaryLengthIs16TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 15 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_FixExt16_AndBinaryLengthIs16HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD8, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 16 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext8_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext8_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext8_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext8_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext8_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext8_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext8_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext8_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext8_AndBinaryLengthIs17TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext8_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs17TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext8_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext8_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext8_AndBinaryLengthIs255TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext8_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs255TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext8_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC7, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext16_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext16_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext16_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext16_AndBinaryLengthIs17TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs17TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext16_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext16_AndBinaryLengthIs255TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs255TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs256JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext16_AndBinaryLengthIs256TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext16_AndBinaryLengthIs256TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs256HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs256JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs256TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs256TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs256HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs65535JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext16_AndBinaryLengthIs65535TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext16_AndBinaryLengthIs65535TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext16_AndBinaryLengthIs65535HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs65535JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs65535TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs65535TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext16_AndBinaryLengthIs65535HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC8, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs0JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs0HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext32_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext32_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs1JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs1TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs1TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs1HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 1, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext32_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext32_AndBinaryLengthIs17TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs17JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 17 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs17TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs17TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 16 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs17HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 17, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 18 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 17 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext32_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext32_AndBinaryLengthIs255TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs255JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs255TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs255TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs255HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 255 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs256JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext32_AndBinaryLengthIs256TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext32_AndBinaryLengthIs256TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs256HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs256JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs256TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs256TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs256HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 1, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 256 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs65535JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext32_AndBinaryLengthIs65535TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext32_AndBinaryLengthIs65535TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs65535HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs65535JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs65535TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs65535TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs65535HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 0, 0xFF, 0xFF, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65535 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs65536JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestReadAsync_Ext32_AndBinaryLengthIs65536TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestReadAsync_Ext32_AndBinaryLengthIs65536TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadAsync_Ext32_AndBinaryLengthIs65536HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				var actual = ( MessagePackExtendedTypeObject )result;
				Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( actual.Body, Is.Not.Null );
				Assert.That( actual.Body.Length, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs65536JustLength_Success()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65536 ) );
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs65536TooShort_Fail()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public void TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs65536TooShort_Fail_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadMessagePackExtendedTypeObjectAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadMessagePackExtendedTypeObjectAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadExtendedTypeObjectAsync_Ext32_AndBinaryLengthIs65536HasExtra_NoProblem()
		{
			var typeCode = ( byte )( Math.Abs( Environment.TickCount ) % 128 );
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC9, 0, 1, 0, 0, typeCode }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				MessagePackExtendedTypeObject result;
				var ret = await unpacker.ReadMessagePackExtendedTypeObjectAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result.TypeCode, Is.EqualTo( typeCode ) );
				Assert.That( result.Body, Is.Not.Null );
				Assert.That( result.Body.Length, Is.EqualTo( 65536 ) );
			}
		}

#endif // FEATURE_TAP

	}
}
