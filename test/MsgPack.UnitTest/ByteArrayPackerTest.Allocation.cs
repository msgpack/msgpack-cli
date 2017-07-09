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
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

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
			var buffer = new ArraySegment<byte>( new byte[ 9 + offset ], offset, 9 );
			using ( var target = CreatePacker( buffer, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_Binary_TooShortSize( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

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
			var buffer = new ArraySegment<byte>( new byte[ 34 + offset ], offset, 34 );
			using ( var target = CreatePacker( buffer, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_String_TooShortSize( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

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
			var buffer = new ArraySegment<byte>( new byte[ 34 + offset ], offset, 34 );
			using ( var target = CreatePacker( buffer, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Default_Scalar_TooShortSize( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should be changed because of re-allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length ) );


				var bytes = target.GetResultBytes();
				// Returns different array even if single array mode.
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Default_Scalar_EnoughSize( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 9 + offset ], offset, 9 );
			using ( var target = CreatePacker( buffer, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Default_Binary_TooShortSize( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should be changed because of re-allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length ) );


				var bytes = target.GetResultBytes();
				// Returns different array even if single array mode.
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Default_Binary_EnoughSize( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 34 + offset ], offset, 34 );
			using ( var target = CreatePacker( buffer, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Default_String_TooShortSize( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should be changed because of re-allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length ) );


				var bytes = target.GetResultBytes();
				// Returns different array even if single array mode.
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Default_String_EnoughSize( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 34 + offset ], offset, 34 );
			using ( var target = CreatePacker( buffer, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Custom_Scalar_TooShortSize( int offset )
		{
			var allocator = new Allocator();
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should be changed because of re-allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length ) );


				var bytes = target.GetResultBytes();
				// Returns different array even if single array mode.
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Custom_Scalar_EnoughSize( int offset )
		{
			var allocator = new Allocator();
			var buffer = new ArraySegment<byte>( new byte[ 9 + offset ], offset, 9 );
			using ( var target = CreatePacker( buffer, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Custom_Binary_TooShortSize( int offset )
		{
			var allocator = new Allocator();
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should be changed because of re-allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length ) );


				var bytes = target.GetResultBytes();
				// Returns different array even if single array mode.
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Custom_Binary_EnoughSize( int offset )
		{
			var allocator = new Allocator();
			var buffer = new ArraySegment<byte>( new byte[ 34 + offset ], offset, 34 );
			using ( var target = CreatePacker( buffer, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Custom_String_TooShortSize( int offset )
		{
			var allocator = new Allocator();
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should be changed because of re-allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length ) );


				var bytes = target.GetResultBytes();
				// Returns different array even if single array mode.
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Custom_String_EnoughSize( int offset )
		{
			var allocator = new Allocator();
			var buffer = new ArraySegment<byte>( new byte[ 34 + offset ], offset, 34 );
			using ( var target = CreatePacker( buffer, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Fixed_Scalar_TooShortSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				Assert.Throws<InvalidOperationException>(
					() => target.Pack( 0x123456789AL )
				);
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Fixed_Scalar_EnoughSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 5 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 4 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			buffers.Add( new ArraySegment<byte>( new byte[ 1 ] ) );
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Fixed_Binary_TooShortSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				Assert.Throws<InvalidOperationException>(
					() => target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() )
				);
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Fixed_Binary_EnoughSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Fixed_String_TooShortSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				Assert.Throws<InvalidOperationException>(
					() => target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) )
				);
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Fixed_String_EnoughSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Default_Scalar_TooShortSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				var expectedSizeInAllocated = expected.Length - 2;
				// So, we subtract 1 for identical length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + ( expected.Length > 2 ? 1 : 0 ) + ( expectedSizeInAllocated / DefaultAllocationSize ) - ( expectedSizeInAllocated % DefaultAllocationSize == 0 ? 1 : 0 ) ) );
				// Tail for identical length data, intermediate (1) for other length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( ( expectedSizeInAllocated % DefaultAllocationSize == 0 ) ? DefaultAllocationSize : ( expectedSizeInAllocated % DefaultAllocationSize ) ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( DefaultAllocationSize ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Default_Scalar_EnoughSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 5 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 4 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			buffers.Add( new ArraySegment<byte>( new byte[ 1 ] ) );
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Default_Binary_TooShortSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				var expectedSizeInAllocated = expected.Length - 2;
				// So, we subtract 1 for identical length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + ( expected.Length > 2 ? 1 : 0 ) + ( expectedSizeInAllocated / DefaultAllocationSize ) - ( expectedSizeInAllocated % DefaultAllocationSize == 0 ? 1 : 0 ) ) );
				// Tail for identical length data, intermediate (1) for other length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( ( expectedSizeInAllocated % DefaultAllocationSize == 0 ) ? DefaultAllocationSize : ( expectedSizeInAllocated % DefaultAllocationSize ) ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( DefaultAllocationSize ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Default_Binary_EnoughSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Default_String_TooShortSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				var expectedSizeInAllocated = expected.Length - 2;
				// So, we subtract 1 for identical length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + ( expected.Length > 2 ? 1 : 0 ) + ( expectedSizeInAllocated / DefaultAllocationSize ) - ( expectedSizeInAllocated % DefaultAllocationSize == 0 ? 1 : 0 ) ) );
				// Tail for identical length data, intermediate (1) for other length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( ( expectedSizeInAllocated % DefaultAllocationSize == 0 ) ? DefaultAllocationSize : ( expectedSizeInAllocated % DefaultAllocationSize ) ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( DefaultAllocationSize ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Default_String_EnoughSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_FixedSize_Scalar_TooShortSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, FixedSizeAllocationSize ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				var expectedSizeInAllocated = expected.Length - 2;
				// So, we subtract 1 for identical length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + ( expected.Length > 2 ? 1 : 0 ) + ( expectedSizeInAllocated / FixedSizeAllocationSize ) - ( expectedSizeInAllocated % FixedSizeAllocationSize == 0 ? 1 : 0 ) ) );
				// Tail for identical length data, intermediate (1) for other length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( ( expectedSizeInAllocated % FixedSizeAllocationSize == 0 ) ? FixedSizeAllocationSize : ( expectedSizeInAllocated % FixedSizeAllocationSize ) ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( FixedSizeAllocationSize ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_FixedSize_Scalar_EnoughSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 5 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 4 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			buffers.Add( new ArraySegment<byte>( new byte[ 1 ] ) );
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, FixedSizeAllocationSize ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_FixedSize_Binary_TooShortSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, FixedSizeAllocationSize ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				var expectedSizeInAllocated = expected.Length - 2;
				// So, we subtract 1 for identical length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + ( expected.Length > 2 ? 1 : 0 ) + ( expectedSizeInAllocated / FixedSizeAllocationSize ) - ( expectedSizeInAllocated % FixedSizeAllocationSize == 0 ? 1 : 0 ) ) );
				// Tail for identical length data, intermediate (1) for other length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( ( expectedSizeInAllocated % FixedSizeAllocationSize == 0 ) ? FixedSizeAllocationSize : ( expectedSizeInAllocated % FixedSizeAllocationSize ) ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( FixedSizeAllocationSize ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_FixedSize_Binary_EnoughSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, FixedSizeAllocationSize ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_FixedSize_String_TooShortSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, FixedSizeAllocationSize ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				var expectedSizeInAllocated = expected.Length - 2;
				// So, we subtract 1 for identical length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + ( expected.Length > 2 ? 1 : 0 ) + ( expectedSizeInAllocated / FixedSizeAllocationSize ) - ( expectedSizeInAllocated % FixedSizeAllocationSize == 0 ? 1 : 0 ) ) );
				// Tail for identical length data, intermediate (1) for other length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( ( expectedSizeInAllocated % FixedSizeAllocationSize == 0 ) ? FixedSizeAllocationSize : ( expectedSizeInAllocated % FixedSizeAllocationSize ) ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( FixedSizeAllocationSize ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_FixedSize_String_EnoughSize( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, FixedSizeAllocationSize ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Custom_Scalar_TooShortSize( int startIndex, int startOffset )
		{
			var allocator = new Allocator();
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, allocator.Allocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// This should be startIndex + 1 because our testing allocator do best job.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + 1 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length - 2 /* length of head buffer */ ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( allocator.LastAllocationSize ) );
			}
			Assert.That( allocator.IsOnlyAllocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Custom_Scalar_EnoughSize( int startIndex, int startOffset )
		{
			var allocator = new Allocator();
			var buffers = new List<ArraySegment<byte>>( 5 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 4 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			buffers.Add( new ArraySegment<byte>( new byte[ 1 ] ) );
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, allocator.Allocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyAllocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Custom_Binary_TooShortSize( int startIndex, int startOffset )
		{
			var allocator = new Allocator();
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, allocator.Allocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// This should be startIndex + 1 because our testing allocator do best job.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + 1 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length - 2 /* length of head buffer */ ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( allocator.LastAllocationSize ) );
			}
			Assert.That( allocator.IsOnlyAllocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Custom_Binary_EnoughSize( int startIndex, int startOffset )
		{
			var allocator = new Allocator();
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, allocator.Allocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyAllocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Custom_String_TooShortSize( int startIndex, int startOffset )
		{
			var allocator = new Allocator();
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, allocator.Allocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// This should be startIndex + 1 because our testing allocator do best job.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + 1 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length - 2 /* length of head buffer */ ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( allocator.LastAllocationSize ) );
			}
			Assert.That( allocator.IsOnlyAllocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Custom_String_EnoughSize( int startIndex, int startOffset )
		{
			var allocator = new Allocator();
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, allocator.Allocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				target.Pack( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyAllocateCalled(), Is.False );
		}

#if FEATURE_TAP

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_Scalar_TooShortSizeAsync( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

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
			var buffer = new ArraySegment<byte>( new byte[ 9 + offset ], offset, 9 );
			using ( var target = CreatePacker( buffer, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_Binary_TooShortSizeAsync( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

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
			var buffer = new ArraySegment<byte>( new byte[ 34 + offset ], offset, 34 );
			using ( var target = CreatePacker( buffer, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public void TestSingleAllocation_Fixed_String_TooShortSizeAsync( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

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
			var buffer = new ArraySegment<byte>( new byte[ 34 + offset ], offset, 34 );
			using ( var target = CreatePacker( buffer, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Default_Scalar_TooShortSizeAsync( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should be changed because of re-allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length ) );


				var bytes = target.GetResultBytes();
				// Returns different array even if single array mode.
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Default_Scalar_EnoughSizeAsync( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 9 + offset ], offset, 9 );
			using ( var target = CreatePacker( buffer, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Default_Binary_TooShortSizeAsync( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should be changed because of re-allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length ) );


				var bytes = target.GetResultBytes();
				// Returns different array even if single array mode.
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Default_Binary_EnoughSizeAsync( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 34 + offset ], offset, 34 );
			using ( var target = CreatePacker( buffer, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Default_String_TooShortSizeAsync( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should be changed because of re-allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length ) );


				var bytes = target.GetResultBytes();
				// Returns different array even if single array mode.
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Default_String_EnoughSizeAsync( int offset )
		{
			var buffer = new ArraySegment<byte>( new byte[ 34 + offset ], offset, 34 );
			using ( var target = CreatePacker( buffer, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Custom_Scalar_TooShortSizeAsync( int offset )
		{
			var allocator = new Allocator();
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should be changed because of re-allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length ) );


				var bytes = target.GetResultBytes();
				// Returns different array even if single array mode.
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Custom_Scalar_EnoughSizeAsync( int offset )
		{
			var allocator = new Allocator();
			var buffer = new ArraySegment<byte>( new byte[ 9 + offset ], offset, 9 );
			using ( var target = CreatePacker( buffer, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( 0x123456789AL );
				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Custom_Binary_TooShortSizeAsync( int offset )
		{
			var allocator = new Allocator();
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should be changed because of re-allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length ) );


				var bytes = target.GetResultBytes();
				// Returns different array even if single array mode.
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Custom_Binary_EnoughSizeAsync( int offset )
		{
			var allocator = new Allocator();
			var buffer = new ArraySegment<byte>( new byte[ 34 + offset ], offset, 34 );
			using ( var target = CreatePacker( buffer, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );
				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Custom_String_TooShortSizeAsync( int offset )
		{
			var allocator = new Allocator();
			var buffer = new ArraySegment<byte>( new byte[ 2 + offset ], offset, 2 );
			using ( var target = CreatePacker( buffer, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should be changed because of re-allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length ) );


				var bytes = target.GetResultBytes();
				// Returns different array even if single array mode.
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0 )]
		[TestCase( 1 )]
		public async Task TestSingleAllocation_Custom_String_EnoughSizeAsync( int offset )
		{
			var allocator = new Allocator();
			var buffer = new ArraySegment<byte>( new byte[ 34 + offset ], offset, 34 );
			using ( var target = CreatePacker( buffer, allocator.Reallocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );
				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( 0 ) );

				// CurrentBufferIndex is not shift for single array mode.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( 0 ) );
				// Base offset should not be changed because no allocation.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( offset + expected.Length ) );


				var bytes = target.GetResultBytes();
				if ( offset == 0 )
				{
					// Returns same array if buffer contains single array and its segment refers entire array.
					Assert.That( target.GetResultBytes(), Is.SameAs( bytes ) );
					// Returns same array if no allocation has been ocurred.
					Assert.That( target.GetResultBytes(), Is.SameAs( buffer.Array ) );
				}
				else
				{
					// Returns different array even if single array mode.
					Assert.That( bytes, Is.Not.Null );
					Assert.That( bytes, Is.Not.SameAs( buffer.Array ) );
				}

				// Only used contents are returned.
				Assert.That( bytes, Is.EqualTo( expected ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				Assert.That( list.Count, Is.EqualTo( 1 ) );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyReallocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Fixed_Scalar_TooShortSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				AssertEx.ThrowsAsync<InvalidOperationException>(
					async () => await target.PackAsync( 0x123456789AL )
				);
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Fixed_Scalar_EnoughSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 5 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 4 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			buffers.Add( new ArraySegment<byte>( new byte[ 1 ] ) );
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Fixed_Binary_TooShortSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				AssertEx.ThrowsAsync<InvalidOperationException>(
					async () => await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() )
				);
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Fixed_Binary_EnoughSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public void TestMultiAllocation_Fixed_String_TooShortSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				AssertEx.ThrowsAsync<InvalidOperationException>(
					async () => await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) )
				);
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Fixed_String_EnoughSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, false ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Default_Scalar_TooShortSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				var expectedSizeInAllocated = expected.Length - 2;
				// So, we subtract 1 for identical length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + ( expected.Length > 2 ? 1 : 0 ) + ( expectedSizeInAllocated / DefaultAllocationSize ) - ( expectedSizeInAllocated % DefaultAllocationSize == 0 ? 1 : 0 ) ) );
				// Tail for identical length data, intermediate (1) for other length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( ( expectedSizeInAllocated % DefaultAllocationSize == 0 ) ? DefaultAllocationSize : ( expectedSizeInAllocated % DefaultAllocationSize ) ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( DefaultAllocationSize ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Default_Scalar_EnoughSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 5 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 4 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			buffers.Add( new ArraySegment<byte>( new byte[ 1 ] ) );
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Default_Binary_TooShortSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				var expectedSizeInAllocated = expected.Length - 2;
				// So, we subtract 1 for identical length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + ( expected.Length > 2 ? 1 : 0 ) + ( expectedSizeInAllocated / DefaultAllocationSize ) - ( expectedSizeInAllocated % DefaultAllocationSize == 0 ? 1 : 0 ) ) );
				// Tail for identical length data, intermediate (1) for other length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( ( expectedSizeInAllocated % DefaultAllocationSize == 0 ) ? DefaultAllocationSize : ( expectedSizeInAllocated % DefaultAllocationSize ) ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( DefaultAllocationSize ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Default_Binary_EnoughSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Default_String_TooShortSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				var expectedSizeInAllocated = expected.Length - 2;
				// So, we subtract 1 for identical length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + ( expected.Length > 2 ? 1 : 0 ) + ( expectedSizeInAllocated / DefaultAllocationSize ) - ( expectedSizeInAllocated % DefaultAllocationSize == 0 ? 1 : 0 ) ) );
				// Tail for identical length data, intermediate (1) for other length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( ( expectedSizeInAllocated % DefaultAllocationSize == 0 ) ? DefaultAllocationSize : ( expectedSizeInAllocated % DefaultAllocationSize ) ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( DefaultAllocationSize ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Default_String_EnoughSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, true ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_FixedSize_Scalar_TooShortSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, FixedSizeAllocationSize ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				var expectedSizeInAllocated = expected.Length - 2;
				// So, we subtract 1 for identical length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + ( expected.Length > 2 ? 1 : 0 ) + ( expectedSizeInAllocated / FixedSizeAllocationSize ) - ( expectedSizeInAllocated % FixedSizeAllocationSize == 0 ? 1 : 0 ) ) );
				// Tail for identical length data, intermediate (1) for other length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( ( expectedSizeInAllocated % FixedSizeAllocationSize == 0 ) ? FixedSizeAllocationSize : ( expectedSizeInAllocated % FixedSizeAllocationSize ) ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( FixedSizeAllocationSize ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_FixedSize_Scalar_EnoughSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 5 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 4 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			buffers.Add( new ArraySegment<byte>( new byte[ 1 ] ) );
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, FixedSizeAllocationSize ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_FixedSize_Binary_TooShortSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, FixedSizeAllocationSize ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				var expectedSizeInAllocated = expected.Length - 2;
				// So, we subtract 1 for identical length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + ( expected.Length > 2 ? 1 : 0 ) + ( expectedSizeInAllocated / FixedSizeAllocationSize ) - ( expectedSizeInAllocated % FixedSizeAllocationSize == 0 ? 1 : 0 ) ) );
				// Tail for identical length data, intermediate (1) for other length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( ( expectedSizeInAllocated % FixedSizeAllocationSize == 0 ) ? FixedSizeAllocationSize : ( expectedSizeInAllocated % FixedSizeAllocationSize ) ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( FixedSizeAllocationSize ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_FixedSize_Binary_EnoughSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, FixedSizeAllocationSize ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_FixedSize_String_TooShortSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, FixedSizeAllocationSize ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				var expectedSizeInAllocated = expected.Length - 2;
				// So, we subtract 1 for identical length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + ( expected.Length > 2 ? 1 : 0 ) + ( expectedSizeInAllocated / FixedSizeAllocationSize ) - ( expectedSizeInAllocated % FixedSizeAllocationSize == 0 ? 1 : 0 ) ) );
				// Tail for identical length data, intermediate (1) for other length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( ( expectedSizeInAllocated % FixedSizeAllocationSize == 0 ) ? FixedSizeAllocationSize : ( expectedSizeInAllocated % FixedSizeAllocationSize ) ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( FixedSizeAllocationSize ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_FixedSize_String_EnoughSizeAsync( int startIndex, int startOffset )
		{
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, FixedSizeAllocationSize ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Custom_Scalar_TooShortSizeAsync( int startIndex, int startOffset )
		{
			var allocator = new Allocator();
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, allocator.Allocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// This should be startIndex + 1 because our testing allocator do best job.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + 1 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length - 2 /* length of head buffer */ ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( allocator.LastAllocationSize ) );
			}
			Assert.That( allocator.IsOnlyAllocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Custom_Scalar_EnoughSizeAsync( int startIndex, int startOffset )
		{
			var allocator = new Allocator();
			var buffers = new List<ArraySegment<byte>>( 5 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 4 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			buffers.Add( new ArraySegment<byte>( new byte[ 1 ] ) );
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, allocator.Allocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( 0x123456789AL );

				var expected = new byte[] { 0xD3, 0, 0, 0, 0x12, 0x34, 0x56, 0x78, 0x9A };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyAllocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Custom_Binary_TooShortSizeAsync( int startIndex, int startOffset )
		{
			var allocator = new Allocator();
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, allocator.Allocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// This should be startIndex + 1 because our testing allocator do best job.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + 1 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length - 2 /* length of head buffer */ ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( allocator.LastAllocationSize ) );
			}
			Assert.That( allocator.IsOnlyAllocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Custom_Binary_EnoughSizeAsync( int startIndex, int startOffset )
		{
			var allocator = new Allocator();
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, allocator.Allocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( Enumerable.Range( 0, 32 ).Select( x => ( byte )x ).ToArray() );

				var expected = new byte[] { 0xC4, 0x20, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyAllocateCalled(), Is.False );
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Custom_String_TooShortSizeAsync( int startIndex, int startOffset )
		{
			var allocator = new Allocator();
			var buffers = new List<ArraySegment<byte>>( 2 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 1 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, allocator.Allocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// This should be startIndex + 1 because our testing allocator do best job.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + 1 ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length - 2 /* length of head buffer */ ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should be adjusted by the allocator.
				Assert.That( buffers.Count, Is.Not.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
				// Check last allocation size is expected (even if it is implementation detail.)
				Assert.That( buffers.Last().Count, Is.EqualTo( allocator.LastAllocationSize ) );
			}
			Assert.That( allocator.IsOnlyAllocateCalled(), Is.True );
		}

		[Test]
		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		public async Task TestMultiAllocation_Custom_String_EnoughSizeAsync( int startIndex, int startOffset )
		{
			var allocator = new Allocator();
			var buffers = new List<ArraySegment<byte>>( 18 + startIndex );
			
			for ( var i = 0; i < startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}

			buffers.Add( new ArraySegment<byte>( new byte[ 2 + startOffset ], startOffset, 2 ) );

			for ( var i = ( startIndex + 1 ); i < 17 + startIndex; i++ )
			{
				buffers.Add( new ArraySegment<byte>( new byte[ 2 ] ) );
			}
			
			var initialBuffersSize = buffers.Count;

			using ( var target = CreatePacker( buffers, startIndex, startOffset, allocator.Allocate ) )
			{
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex ) );
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( startOffset ) );

				await target.PackAsync( new string( Enumerable.Range( ( int )'A', 32 ).Select( x => ( char )x ).ToArray() ) );

				var expected = new byte[] { 0xD9, 0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F, 0x60 };

				// InitialBufferIndex is immutable.
				Assert.That( target.InitialBufferIndex, Is.EqualTo( startIndex ) );

				// Note that the 'pointer' can be tail of the current buffer to avoid allocation which will not be required.
				// So, we subtract 1 for even length data.
				Assert.That( target.CurrentBufferIndex, Is.EqualTo( startIndex + expected.Length / 2  - ( expected.Length % 2 == 0 ? 1 : 0 ) ) );
				// Tail for even length data, intermediate (1) for odd length data.
				Assert.That( target.CurrentBufferOffset, Is.EqualTo( expected.Length % 2 == 0 ? 2 : 1 ) );

				var list = target.GetFinalBuffers();
				Assert.That( list, Is.Not.Null.And.Not.Empty );
				// Note that startOffset should be considered in ArraySegmemt<T>.ToArray()
				Assert.That( list.SelectMany( b => b.ToArray() ).Skip( startIndex * 2 ).Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
				// Buffer LIST should be touched by the allocator.
				Assert.That( list, Is.SameAs( buffers ) );
				// Buffer should not be adjusted by the allocator.
				Assert.That( buffers.Count, Is.EqualTo( initialBuffersSize ) );

				var bytes = target.GetResultBytes();
				// Only used contents are returned.
				Assert.That( target.GetResultBytes(), Is.Not.SameAs( bytes ) );
				Assert.That( bytes, Is.Not.Null );
				Assert.That( bytes, Is.EqualTo( expected ) );
			}
			Assert.That( allocator.IsOnlyAllocateCalled(), Is.False );
		}

#endif // FEATURE_TAP

	}
}
