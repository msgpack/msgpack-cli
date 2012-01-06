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
using System.IO;
using NUnit.Framework;

namespace MsgPack
{
	[TestFixture]
	public class UnpackerTest
	{
		[Test]
		public void TestRead_ScalarSequence_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "1st" );
				Assert.That( rootUnpacker.Data.Value.Equals( 1 ) );
				Assert.That( rootUnpacker.Read(), "2nd" );
				Assert.That( rootUnpacker.Data.Value.Equals( 2 ) );
				Assert.That( rootUnpacker.Read(), "3rd" );
				Assert.That( rootUnpacker.Data.Value.Equals( 3 ) );
			}
		}

		[Test]
		public void TestRead_Array_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x93, 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "1st" );
				Assert.That( rootUnpacker.IsArrayHeader );
				Assert.That( rootUnpacker.IsMapHeader, Is.False );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.Data.Value.Equals( 3 ) ); // == Length
				Assert.That( rootUnpacker.Read(), "2nd" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.Data.Value.Equals( 1 ) );
				Assert.That( rootUnpacker.Read(), "3rd" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.Data.Value.Equals( 2 ) );
				Assert.That( rootUnpacker.Read(), "4th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.Data.Value.Equals( 3 ) );
			}
		}


		[Test]
		public void TestRead_Map_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x83, 0x1, 0x1, 0x2, 0x2, 0x3, 0x3 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "1st" );
				Assert.That( rootUnpacker.IsArrayHeader, Is.False );
				Assert.That( rootUnpacker.IsMapHeader );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.Data.Value.Equals( 3 ) ); // == Length
				Assert.That( rootUnpacker.Read(), "2nd" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.Data.Value.Equals( 1 ) );
				Assert.That( rootUnpacker.Read(), "3rd" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.Data.Value.Equals( 1 ) );
				Assert.That( rootUnpacker.Read(), "4th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.Data.Value.Equals( 2 ) );
				Assert.That( rootUnpacker.Read(), "5th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.Data.Value.Equals( 2 ) );
				Assert.That( rootUnpacker.Read(), "6th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.Data.Value.Equals( 3 ) );
				Assert.That( rootUnpacker.Read(), "7th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.Data.Value.Equals( 3 ) );
			}
		}

		[Test]
		public void TestRead_ReadInTail_NoEffect()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "1st" );
				Assert.That( rootUnpacker.Data.Value.Equals( 1 ) );
				Assert.That( rootUnpacker.Read(), "2nd" );
				Assert.That( rootUnpacker.Data.Value.Equals( 2 ) );
				Assert.That( rootUnpacker.Read(), "3rd" );
				Assert.That( rootUnpacker.Data.Value.Equals( 3 ) );
				Assert.That( rootUnpacker.Read(), Is.False, "Tail" );
				// Data should be last read.
				Assert.That( rootUnpacker.Data.Value.Equals( 3 ) );
			}
		}

		[Test]
		public void TestRead_ReadInSubTreeTail_NoEffect()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "Top Level" );
				Assert.That( rootUnpacker.IsArrayHeader );

				using ( var subTreeReader = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeReader.Read(), "1st" );
					Assert.That( subTreeReader.Data.Value.Equals( 1 ) );
					Assert.That( subTreeReader.Read(), "2nd" );
					Assert.That( subTreeReader.Data.Value.Equals( 2 ) );
					Assert.That( subTreeReader.Read(), Is.False, "Tail" );
					// Data should be last read.
					Assert.That( subTreeReader.Data.Value.Equals( 2 ) );
				}

				Assert.That( rootUnpacker.Read(), "3rd" );
				Assert.That( rootUnpacker.Data.Value.Equals( 3 ) );
			}
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestRead_InSubTreeMode_Fail()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "Failed to first read" );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					// To be failed.
					rootUnpacker.Read();
					Assert.Fail();
				}
			}
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestReadSubTree_IsScalar_Fail()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "Failed to first read" );

				// To be failed.
				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.Fail();
				}
			}
		}

		[Test]
		public void TestReadSubTree_NestedArray_Success()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x94, 0x91, 0x1, 0x90, 0xC0, 0x92, 0x1, 0x2, 0x91, 0x1 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeUnpacker.Read(), Is.True );

					using ( var subSubTreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( subSubTreeUnpacker.Read(), Is.True );
						Assert.That( subSubTreeUnpacker.Data.Value.Equals( 1 ) );
						Assert.That( subSubTreeUnpacker.Read(), Is.False );
					}

					Assert.That( subTreeUnpacker.Read(), Is.True );

					using ( var subSubTreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( subSubTreeUnpacker.Read(), Is.False );
					}

					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.IsNil );

					Assert.That( subTreeUnpacker.Read(), Is.True );

					using ( var subSubTreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( subSubTreeUnpacker.Read(), Is.True );
						Assert.That( subSubTreeUnpacker.Data.Value.Equals( 1 ) );
						Assert.That( subSubTreeUnpacker.Read(), Is.True );
						Assert.That( subSubTreeUnpacker.Data.Value.Equals( 2 ) );
						Assert.That( subSubTreeUnpacker.Read(), Is.False );
					}

					Assert.That( subTreeUnpacker.Read(), Is.False );
				}

				Assert.That( rootUnpacker.Read(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.Equals( 1 ) );
					Assert.That( subTreeUnpacker.Read(), Is.False );
				}

				Assert.That( rootUnpacker.Read(), Is.False );
			}
		}

		[Test]
		public void TestReadSubTree_NestedMap_Success()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x84, 0x1, 0x81, 0x1, 0x1, 0x2, 0x80, 0x3, 0xC0, 0x4, 0x82, 0x1, 0x1, 0x2, 0x2, 0x81, 0x1, 0x1 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.Equals( 1 ) );
					Assert.That( subTreeUnpacker.Read(), Is.True );

					using ( var subSubTreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( subSubTreeUnpacker.Read(), Is.True );
						Assert.That( subSubTreeUnpacker.Data.Value.Equals( 1 ) );
						Assert.That( subSubTreeUnpacker.Read(), Is.True );
						Assert.That( subSubTreeUnpacker.Data.Value.Equals( 1 ) );
						Assert.That( subSubTreeUnpacker.Read(), Is.False );
					}

					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.Equals( 2 ) );
					Assert.That( subTreeUnpacker.Read(), Is.True );

					using ( var subSubTreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( subSubTreeUnpacker.Read(), Is.False );
					}

					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.Equals( 3 ) );
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.IsNil );

					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.Equals( 4 ) );
					Assert.That( subTreeUnpacker.Read(), Is.True );

					using ( var subSubTreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( subSubTreeUnpacker.Read(), Is.True );
						Assert.That( subSubTreeUnpacker.Data.Value.Equals( 1 ) );
						Assert.That( subSubTreeUnpacker.Read(), Is.True );
						Assert.That( subSubTreeUnpacker.Data.Value.Equals( 1 ) );
						Assert.That( subSubTreeUnpacker.Read(), Is.True );
						Assert.That( subSubTreeUnpacker.Data.Value.Equals( 2 ) );
						Assert.That( subSubTreeUnpacker.Read(), Is.True );
						Assert.That( subSubTreeUnpacker.Data.Value.Equals( 2 ) );
						Assert.That( subSubTreeUnpacker.Read(), Is.False );
					}

					Assert.That( subTreeUnpacker.Read(), Is.False );
				}

				Assert.That( rootUnpacker.Read(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.Equals( 1 ) );
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.Equals( 1 ) );
					Assert.That( subTreeUnpacker.Read(), Is.False );
				}

				Assert.That( rootUnpacker.Read(), Is.False );
			}
		}

		[Test]
		public void TestReadSubTree_Nested_ReadGrandchildren_Success()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x92, 0x1, 0x91, 0x1, 0x2 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.Equals( 2 ) ); // Array Length
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.Equals( 1 ) ); // Value in grand children
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.Equals( 1 ) ); // Array Length
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.Equals( 1 ) ); // Value in grand children
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.Data.Value.Equals( 2 ) ); // Value in children
					Assert.That( subTreeUnpacker.Read(), Is.False );
				}

				Assert.That( rootUnpacker.Read(), Is.False );
			}
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestReadSubTree_InLeafBody_Fail()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "Failed to first read" );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( rootUnpacker.Read(), "Failed to second read" );
					// To be failed
					using ( var subSubTreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.Fail();
					}
				}
			}
		}


		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestCreate_StreamIsNull()
		{
			using ( Unpacker.Create( null ) ) { }
		}

		[Test]
		public void TestCreate_OwnsStreamisFalse_NotDisposeStream()
		{
			using ( var stream = new MemoryStream() )
			{
				using ( Unpacker.Create( stream, false ) ) { }

				// Should not throw ObjectDisposedException.
				stream.WriteByte( 1 );
			}
		}

		// TODO: Consider remove Feeding API and Create()
	}
}
