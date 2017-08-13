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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestCaseAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.DataRowAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack
{
	// This file was generated from ByteArrayPackerTest.Allocation.tt T4Template.
	// Do not modify this file. Edit ByteArrayPackerTest.Allocation.tt instead.

	partial class ByteArrayPackerTest
	{
		private const int DefaultAllocationSize = 65536;
		private const int FixedSizeAllocationSize = 8;
		private const int CustomAllocationSize = 16;

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_Scalar_TooShortSize( int offset )
		{
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, false ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				Assert.Throws<InvalidOperationException>(
					() => target.Pack( 0x123456789AL )
				);
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_Scalar_EnoughSize( int offset )
		{
			var buffer = new byte[ 9 + offset ];
			using ( var target = CreatePacker( buffer, offset, false ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_Binary_TooShortSize( int offset )
		{
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, false ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				Assert.Throws<InvalidOperationException>(
					() => target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() )
				);
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_Binary_EnoughSize( int offset )
		{
			var buffer = new byte[ 34 + offset ];
			using ( var target = CreatePacker( buffer, offset, false ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_String_TooShortSize( int offset )
		{
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, false ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				Assert.Throws<InvalidOperationException>(
					() => target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) )
				);
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_String_EnoughSize( int offset )
		{
			var buffer = new byte[ 34 + offset ];
			using ( var target = CreatePacker( buffer, offset, false ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Default_Scalar_TooShortSize( int offset )
		{
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, true ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns different array.
				Assert.That( bytes.Array, Is.Not.Null );
				Assert.That( bytes.Array, Is.Not.SameAs( buffer ) );

				Assert.That( target.GetFinalBuffer(), Is.Not.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Default_Scalar_EnoughSize( int offset )
		{
			var buffer = new byte[ 9 + offset ];
			using ( var target = CreatePacker( buffer, offset, true ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Default_Binary_TooShortSize( int offset )
		{
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, true ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns different array.
				Assert.That( bytes.Array, Is.Not.Null );
				Assert.That( bytes.Array, Is.Not.SameAs( buffer ) );

				Assert.That( target.GetFinalBuffer(), Is.Not.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Default_Binary_EnoughSize( int offset )
		{
			var buffer = new byte[ 34 + offset ];
			using ( var target = CreatePacker( buffer, offset, true ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Default_String_TooShortSize( int offset )
		{
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, true ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns different array.
				Assert.That( bytes.Array, Is.Not.Null );
				Assert.That( bytes.Array, Is.Not.SameAs( buffer ) );

				Assert.That( target.GetFinalBuffer(), Is.Not.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Default_String_EnoughSize( int offset )
		{
			var buffer = new byte[ 34 + offset ];
			using ( var target = CreatePacker( buffer, offset, true ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Custom_Scalar_TooShortSize( int offset )
		{
			var allocator = new Allocator();
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns different array.
				Assert.That( bytes.Array, Is.Not.Null );
				Assert.That( bytes.Array, Is.Not.SameAs( buffer ) );

				Assert.That( target.GetFinalBuffer(), Is.Not.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Custom_Scalar_EnoughSize( int offset )
		{
			var allocator = new Allocator();
			var buffer = new byte[ 9 + offset ];
			using ( var target = CreatePacker( buffer, offset, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Custom_Binary_TooShortSize( int offset )
		{
			var allocator = new Allocator();
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns different array.
				Assert.That( bytes.Array, Is.Not.Null );
				Assert.That( bytes.Array, Is.Not.SameAs( buffer ) );

				Assert.That( target.GetFinalBuffer(), Is.Not.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Custom_Binary_EnoughSize( int offset )
		{
			var allocator = new Allocator();
			var buffer = new byte[ 34 + offset ];
			using ( var target = CreatePacker( buffer, offset, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Custom_String_TooShortSize( int offset )
		{
			var allocator = new Allocator();
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns different array.
				Assert.That( bytes.Array, Is.Not.Null );
				Assert.That( bytes.Array, Is.Not.SameAs( buffer ) );

				Assert.That( target.GetFinalBuffer(), Is.Not.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Custom_String_EnoughSize( int offset )
		{
			var allocator = new Allocator();
			var buffer = new byte[ 34 + offset ];
			using ( var target = CreatePacker( buffer, offset, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.False );
		}

#if FEATURE_TAP

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_Scalar_TooShortSizeAsync( int offset )
		{
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, false ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				AssertEx.ThrowsAsync<InvalidOperationException>(
					async () => await target.PackAsync( 0x123456789AL )
				);
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Fixed_Scalar_EnoughSizeAsync( int offset )
		{
			var buffer = new byte[ 9 + offset ];
			using ( var target = CreatePacker( buffer, offset, false ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_Binary_TooShortSizeAsync( int offset )
		{
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, false ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				AssertEx.ThrowsAsync<InvalidOperationException>(
					async () => await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() )
				);
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Fixed_Binary_EnoughSizeAsync( int offset )
		{
			var buffer = new byte[ 34 + offset ];
			using ( var target = CreatePacker( buffer, offset, false ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_String_TooShortSizeAsync( int offset )
		{
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, false ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				AssertEx.ThrowsAsync<InvalidOperationException>(
					async () => await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) )
				);
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Fixed_String_EnoughSizeAsync( int offset )
		{
			var buffer = new byte[ 34 + offset ];
			using ( var target = CreatePacker( buffer, offset, false ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Default_Scalar_TooShortSizeAsync( int offset )
		{
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, true ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns different array.
				Assert.That( bytes.Array, Is.Not.Null );
				Assert.That( bytes.Array, Is.Not.SameAs( buffer ) );

				Assert.That( target.GetFinalBuffer(), Is.Not.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Default_Scalar_EnoughSizeAsync( int offset )
		{
			var buffer = new byte[ 9 + offset ];
			using ( var target = CreatePacker( buffer, offset, true ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Default_Binary_TooShortSizeAsync( int offset )
		{
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, true ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns different array.
				Assert.That( bytes.Array, Is.Not.Null );
				Assert.That( bytes.Array, Is.Not.SameAs( buffer ) );

				Assert.That( target.GetFinalBuffer(), Is.Not.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Default_Binary_EnoughSizeAsync( int offset )
		{
			var buffer = new byte[ 34 + offset ];
			using ( var target = CreatePacker( buffer, offset, true ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Default_String_TooShortSizeAsync( int offset )
		{
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, true ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns different array.
				Assert.That( bytes.Array, Is.Not.Null );
				Assert.That( bytes.Array, Is.Not.SameAs( buffer ) );

				Assert.That( target.GetFinalBuffer(), Is.Not.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Default_String_EnoughSizeAsync( int offset )
		{
			var buffer = new byte[ 34 + offset ];
			using ( var target = CreatePacker( buffer, offset, true ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Custom_Scalar_TooShortSizeAsync( int offset )
		{
			var allocator = new Allocator();
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns different array.
				Assert.That( bytes.Array, Is.Not.Null );
				Assert.That( bytes.Array, Is.Not.SameAs( buffer ) );

				Assert.That( target.GetFinalBuffer(), Is.Not.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Custom_Scalar_EnoughSizeAsync( int offset )
		{
			var allocator = new Allocator();
			var buffer = new byte[ 9 + offset ];
			using ( var target = CreatePacker( buffer, offset, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Custom_Binary_TooShortSizeAsync( int offset )
		{
			var allocator = new Allocator();
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns different array.
				Assert.That( bytes.Array, Is.Not.Null );
				Assert.That( bytes.Array, Is.Not.SameAs( buffer ) );

				Assert.That( target.GetFinalBuffer(), Is.Not.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Custom_Binary_EnoughSizeAsync( int offset )
		{
			var allocator = new Allocator();
			var buffer = new byte[ 34 + offset ];
			using ( var target = CreatePacker( buffer, offset, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Custom_String_TooShortSizeAsync( int offset )
		{
			var allocator = new Allocator();
			var buffer = new byte[ 2 + offset ];
			using ( var target = CreatePacker( buffer, offset, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns different array.
				Assert.That( bytes.Array, Is.Not.Null );
				Assert.That( bytes.Array, Is.Not.SameAs( buffer ) );

				Assert.That( target.GetFinalBuffer(), Is.Not.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Custom_String_EnoughSizeAsync( int offset )
		{
			var allocator = new Allocator();
			var buffer = new byte[ 34 + offset ];
			using ( var target = CreatePacker( buffer, offset, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };
				Assert.That( target.BytesUsed, Is.EqualTo( expected.Length ) );

				var bytes = target.GetResultBytes();
				Assert.That( bytes.Offset, Is.EqualTo( target.InitialBufferOffset ) );
				Assert.That( bytes.Count, Is.EqualTo( target.BytesUsed ) );
				// Returns same array if buffer contains single array and its segment refers entire array.
				Assert.That( target.GetResultBytes().Array, Is.SameAs( bytes.Array ) );
				// Returns same array if no allocation has been ocurred.
				Assert.That( bytes.Array, Is.SameAs( buffer ) );

				// Returns same array if no allocation has been ocurred.
				Assert.That( target.GetFinalBuffer(), Is.SameAs( buffer ) );

				// Only used contents are returned.
				Assert.That( bytes.ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.False );
		}

#endif // FEATURE_TAP

	}
}
