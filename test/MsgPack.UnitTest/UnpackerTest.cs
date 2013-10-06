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
	public class UnpackerTest
	{
		[Test]
		public void TestRead_ScalarSequence_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "1st" );
				Assert.That( rootUnpacker.LastReadData.Equals( 1 ) );
				Assert.That( rootUnpacker.Read(), "2nd" );
				Assert.That( rootUnpacker.LastReadData.Equals( 2 ) );
				Assert.That( rootUnpacker.Read(), "3rd" );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
			}
		}

		[Test]
		public void TestRead_Array_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x93, 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
#pragma warning disable 612,618
				Assert.That( rootUnpacker.Read(), "1st" );
				Assert.That( rootUnpacker.IsArrayHeader );
				Assert.That( rootUnpacker.IsMapHeader, Is.False );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) ); // == Length
				Assert.That( rootUnpacker.Read(), "2nd" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 1 ) );
				Assert.That( rootUnpacker.Read(), "3rd" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 2 ) );
				Assert.That( rootUnpacker.Read(), "4th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
#pragma warning restore 612,618
			}
		}


		[Test]
		public void TestRead_Map_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x83, 0x1, 0x1, 0x2, 0x2, 0x3, 0x3 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
#pragma warning disable 612,618
				Assert.That( rootUnpacker.Read(), "1st" );
				Assert.That( rootUnpacker.IsArrayHeader, Is.False );
				Assert.That( rootUnpacker.IsMapHeader );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) ); // == Length
				Assert.That( rootUnpacker.Read(), "2nd" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 1 ) );
				Assert.That( rootUnpacker.Read(), "3rd" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 1 ) );
				Assert.That( rootUnpacker.Read(), "4th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 2 ) );
				Assert.That( rootUnpacker.Read(), "5th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 2 ) );
				Assert.That( rootUnpacker.Read(), "6th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
				Assert.That( rootUnpacker.Read(), "7th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
#pragma warning restore 612,618
			}
		}

		[Test]
		public void TestRead_ReadInTail_NoEffect()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "1st" );
				Assert.That( rootUnpacker.LastReadData.Equals( 1 ) );
				Assert.That( rootUnpacker.Read(), "2nd" );
				Assert.That( rootUnpacker.LastReadData.Equals( 2 ) );
				Assert.That( rootUnpacker.Read(), "3rd" );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
				Assert.That( rootUnpacker.Read(), Is.False, "Tail" );
				// Data should be last read.
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
			}
		}

		[Test]
		public void TestRead_ReadInSubtreeTail_NoEffect()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "Top Level" );
				Assert.That( rootUnpacker.IsArrayHeader );

				using ( var subTreeReader = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeReader.Read(), "1st" );
					Assert.That( subTreeReader.LastReadData.Equals( 1 ) );
					Assert.That( subTreeReader.Read(), "2nd" );
					Assert.That( subTreeReader.LastReadData.Equals( 2 ) );
					Assert.That( subTreeReader.Read(), Is.False, "Tail" );
					// Data should be last read.
					Assert.That( subTreeReader.LastReadData.Equals( 2 ) );
				}

				Assert.That( rootUnpacker.Read(), "3rd" );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
			}
		}

		[Test]
		public void TestRead_InSubtreeMode_Fail()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "Failed to first read" );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					// To be failed.
					Assert.Throws<InvalidOperationException>( () => rootUnpacker.Read() );
				}
			}
		}

		[Test]
		public void TestReadSubtree_IsScalar_Fail()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "Failed to first read" );

				// To be failed.
				Assert.Throws<InvalidOperationException>( () =>
					{
						using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
						{
							Assert.Fail();
						}
					}
				);
			}
		}

		[Test]
		public void TestReadSubtree_NestedArray_Success()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x94, 0x91, 0x1, 0x90, 0xC0, 0x92, 0x1, 0x2, 0x91, 0x1 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeUnpacker.Read(), Is.True );

					using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( subSubtreeUnpacker.Read(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 1 ) );
						Assert.That( subSubtreeUnpacker.Read(), Is.False );
					}

					Assert.That( subTreeUnpacker.Read(), Is.True );

					using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( subSubtreeUnpacker.Read(), Is.False );
					}

					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.IsNil );

					Assert.That( subTreeUnpacker.Read(), Is.True );

					using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( subSubtreeUnpacker.Read(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 1 ) );
						Assert.That( subSubtreeUnpacker.Read(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 2 ) );
						Assert.That( subSubtreeUnpacker.Read(), Is.False );
					}

					Assert.That( subTreeUnpacker.Read(), Is.False );
				}

				Assert.That( rootUnpacker.Read(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) );
					Assert.That( subTreeUnpacker.Read(), Is.False );
				}

				Assert.That( rootUnpacker.Read(), Is.False );
			}
		}

		[Test]
		public void TestReadSubtree_NestedMap_Success()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x84, 0x1, 0x81, 0x1, 0x1, 0x2, 0x80, 0x3, 0xC0, 0x4, 0x82, 0x1, 0x1, 0x2, 0x2, 0x81, 0x1, 0x1 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) );
					Assert.That( subTreeUnpacker.Read(), Is.True );

					using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( subSubtreeUnpacker.Read(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 1 ) );
						Assert.That( subSubtreeUnpacker.Read(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 1 ) );
						Assert.That( subSubtreeUnpacker.Read(), Is.False );
					}

					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 2 ) );
					Assert.That( subTreeUnpacker.Read(), Is.True );

					using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( subSubtreeUnpacker.Read(), Is.False );
					}

					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 3 ) );
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.IsNil );

					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 4 ) );
					Assert.That( subTreeUnpacker.Read(), Is.True );

					using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( subSubtreeUnpacker.Read(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 1 ) );
						Assert.That( subSubtreeUnpacker.Read(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 1 ) );
						Assert.That( subSubtreeUnpacker.Read(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 2 ) );
						Assert.That( subSubtreeUnpacker.Read(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 2 ) );
						Assert.That( subSubtreeUnpacker.Read(), Is.False );
					}

					Assert.That( subTreeUnpacker.Read(), Is.False );
				}

				Assert.That( rootUnpacker.Read(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) );
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) );
					Assert.That( subTreeUnpacker.Read(), Is.False );
				}

				Assert.That( rootUnpacker.Read(), Is.False );
			}
		}

		[Test]
		public void TestReadSubtree_Nested_ReadGrandchildren_Success()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x92, 0x1, 0x91, 0x1, 0x2 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 2 ) ); // Array Length
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) ); // Value in grand children
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) ); // Array Length
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) ); // Value in grand children
					Assert.That( subTreeUnpacker.Read(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 2 ) ); // Value in children
					Assert.That( subTreeUnpacker.Read(), Is.False );
				}

				Assert.That( rootUnpacker.Read(), Is.False );
			}
		}

		[Test]
		public void TestReadSubtree_InLeafBody_Fail()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var rootUnpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( rootUnpacker.Read(), "Failed to first read" );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( subTreeUnpacker.Read(), "Failed to move to first body." );
					// To be failed
					Assert.Throws<InvalidOperationException>( () =>
						{
							using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
							{
								Assert.Fail();
							}
						}
					);
				}
			}
		}


		[Test]
		public void TestCreate_StreamIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => { using ( Unpacker.Create( null ) ) { } } );
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

		[Test]
		public void TestRead_UnderSkipping()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xD1, 0x1 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				Assert.That( target.Skip(), Is.Null, "Precondition" );
				Assert.Throws<InvalidOperationException>( () => target.Read() );
			}
		}

		[Test]
		public void TestGetEnumerator_UnderSkipping()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xD1, 0x1 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				Assert.That( target.Skip(), Is.Null, "Precondition" );
				Assert.Throws<InvalidOperationException>( () =>
					{
						foreach ( var item in target )
						{

						}
					}
				);
			}
		}

		[Test]
		public void TestReadSubtree_UnderSkipping()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xD1, 0x1 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				Assert.That( target.Skip(), Is.Null, "Precondition" );
				Assert.Throws<InvalidOperationException>( () => target.ReadSubtree() );
			}
		}

		[Test]
		public void TestRead_UnderEnumerating()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				foreach ( var item in target )
				{
					Assert.Throws<InvalidOperationException>( () => target.Read() );
				}
			}
		}

		[Test]
		public void TestSkip_UnderEnumerating()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				foreach ( var item in target )
				{
					Assert.Throws<InvalidOperationException>( () => target.Skip() );
				}
			}
		}

		[Test]
		public void TestReadSubtree_UnderEnumerating()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				foreach ( var item in target )
				{
					Assert.Throws<InvalidOperationException>( () => target.ReadSubtree() );
				}
			}
		}

		[Test]
		public void TestReadSubtree_InRootHead_Success()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				Assert.That( target.Read() );
				Assert.That( target.IsArrayHeader );

				using ( var subTreeUnpacker = target.ReadSubtree() )
				{
					Assert.That( subTreeUnpacker.Read() );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 0x1 ) );
				}
			}
		}

		[Test]
		public void TestReadSubtree_InScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xD0, 0x1 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				Assert.Throws<InvalidOperationException>( () => target.ReadSubtree() );
			}
		}

		[Test]
		public void TestReadSubtree_InNestedScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x81, 0x1, 0x91, 0x1 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				Assert.That( target.Read() );
				Assert.That( target.IsMapHeader, Is.True );
				Assert.That( target.Read() );
				Assert.That( target.IsMapHeader, Is.False );
				Assert.Throws<InvalidOperationException>( () => target.ReadSubtree() );
			}
		}

		[Test]
		public void TestReadData_OneScalar_AsScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				var result = target.ReadItem();
				Assert.That( result.HasValue );
				Assert.That( result.Value == 1, result.Value.ToString() );
				Assert.That( target.ReadItem(), Is.Null );
			}
		}

		[Test]
		public void TestReadData_TwoScalar_AsTwoScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				var result1 = target.ReadItem();
				Assert.That( result1.HasValue );
				Assert.That( result1.Value == 1, result1.Value.ToString() );

				var result2 = target.ReadItem();
				Assert.That( result2.HasValue );
				Assert.That( result2.Value == 2, result2.Value.ToString() );

				Assert.That( target.ReadItem(), Is.Null );
			}
		}

		[Test]
		public void TestReadData_Empty_Null()
		{
			using ( var buffer = new MemoryStream( new byte[ 0 ] ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				var result = target.ReadItem();
				Assert.That( result.HasValue, Is.False );
			}
		}

		[Test]
		public void TestReadData_Array_AsSingleArray()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x1, 0x2 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				var result = target.ReadItem();
				Assert.That( result.HasValue );
				var array = result.Value.AsList();
				Assert.That( array.Count, Is.EqualTo( 2 ), result.Value.ToString() );
				Assert.That( array[ 0 ] == 1, result.Value.ToString() );
				Assert.That( array[ 1 ] == 2, result.Value.ToString() );
				Assert.That( target.ReadItem(), Is.Null );
			}
		}

		[Test]
		public void TestReadData_Map_AsSingleMap()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x82, 0x1, 0x2, 0x3, 0x4 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				var result = target.ReadItem();
				Assert.That( result.HasValue );
				var map = result.Value.AsDictionary();
				Assert.That( map.Count, Is.EqualTo( 2 ), result.Value.ToString() );
				Assert.That( map[ 1 ] == 2, result.Value.ToString() );
				Assert.That( map[ 3 ] == 4, result.Value.ToString() );
				Assert.That( target.ReadItem(), Is.Null );
			}
		}

		[Test]
		public void TestReadData_ArrayFollowingScalar_AsSingleArrayAndScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x1, 0x2, 0x3 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				var result = target.ReadItem();
				Assert.That( result.HasValue );
				var array = result.Value.AsList();
				Assert.That( array.Count, Is.EqualTo( 2 ), result.Value.ToString() );
				Assert.That( array[ 0 ] == 1, result.Value.ToString() );
				Assert.That( array[ 1 ] == 2, result.Value.ToString() );

				var scalar = target.ReadItem();
				Assert.That( scalar.HasValue );
				Assert.That( scalar.Value == 3, result.Value.ToString() );
				Assert.That( target.ReadItem(), Is.Null );
			}
		}

		[Test]
		public void TestReadData_MapFollowingScalar_AsSingleMapAndScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x82, 0x1, 0x2, 0x3, 0x4, 0x5 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				var result = target.ReadItem();
				Assert.That( result.HasValue );
				var map = result.Value.AsDictionary();
				Assert.That( map.Count, Is.EqualTo( 2 ), result.Value.ToString() );
				Assert.That( map[ 1 ] == 2, result.Value.ToString() );
				Assert.That( map[ 3 ] == 4, result.Value.ToString() );

				var scalar = target.ReadItem();
				Assert.That( scalar.HasValue );
				Assert.That( scalar.Value == 5, result.Value.ToString() );
				Assert.That( target.ReadItem(), Is.Null );
			}
		}

		[Test]
		public void TestReadData_ArrayOfArray_AsSingleArray()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x92, 11, 12, 0x92, 21, 22 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				var result = target.ReadItem();
				Assert.That( result.HasValue );
				var array = result.Value.AsList();
				Assert.That( array.Count, Is.EqualTo( 2 ), result.Value.ToString() );
				Assert.That( array[ 0 ].IsArray, result.Value.ToString() );
				Assert.That( array[ 0 ].AsList()[ 0 ] == 11, result.Value.ToString() );
				Assert.That( array[ 0 ].AsList()[ 1 ] == 12, result.Value.ToString() );
				Assert.That( array[ 1 ].IsArray, result.Value.ToString() );
				Assert.That( array[ 1 ].AsList()[ 0 ] == 21, result.Value.ToString() );
				Assert.That( array[ 1 ].AsList()[ 1 ] == 22, result.Value.ToString() );
				Assert.That( target.ReadItem(), Is.Null );
			}
		}


		[Test]
		public void TestReadData_MapOfMap_AsSingleMap()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x82, 1, 0x82, 11, 1, 12, 2, 2, 0x82, 21, 1, 22, 2 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				var result = target.ReadItem();
				Assert.That( result.HasValue );
				var map = result.Value.AsDictionary();
				Assert.That( map.Count, Is.EqualTo( 2 ), result.Value.ToString() );
				Assert.That( map[ 1 ].IsMap, result.Value.ToString() );
				Assert.That( map[ 1 ].AsDictionary()[ 11 ] == 1, result.Value.ToString() );
				Assert.That( map[ 1 ].AsDictionary()[ 12 ] == 2, result.Value.ToString() );
				Assert.That( map[ 2 ].IsMap, result.Value.ToString() );
				Assert.That( map[ 2 ].AsDictionary()[ 21 ] == 1, result.Value.ToString() );
				Assert.That( map[ 2 ].AsDictionary()[ 22 ] == 2, result.Value.ToString() );
				Assert.That( target.ReadItem(), Is.Null );
			}
		}

		[Test]
		public void TestReadString_Clob()
		{
			var str = String.Concat( Enumerable.Range( 0, 0x1FFFF ).Where( i => i < 0xD800 || 0xDFFF < i ).Select( Char.ConvertFromUtf32 ) );
			var encoded = Encoding.UTF8.GetBytes( str );
			using ( var buffer =
				new MemoryStream(
					new byte[] { MessagePackCode.Raw32 }.Concat(
						BitConverter.IsLittleEndian ? BitConverter.GetBytes( encoded.Length ).Reverse() : BitConverter.GetBytes( encoded.Length )
					).Concat( encoded )
					.ToArray()
				)
			)
			using ( var target = Unpacker.Create( buffer ) )
			{
				string result;
				Assert.That( target.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( str ) );
			}
		}

		[Test]
		public void TestRead_EmptyArray_RecognizeEmptyArray()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x90, 0x1 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				Assert.That( target.Read() );
				Assert.That( target.IsArrayHeader );
				Assert.That( target.LastReadData == 0, target.LastReadData.ToString() );
				Assert.That( target.ItemsCount, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestReadArrayLength_EmptyArray_RecognizeEmptyArray()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x90, 0x1 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				long result;
				Assert.That( target.ReadArrayLength( out result ) );
				Assert.That( target.IsArrayHeader );
				Assert.That( result, Is.EqualTo( 0 ) );
				Assert.That( target.LastReadData == 0, target.LastReadData.ToString() );
				Assert.That( target.ItemsCount, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestRead_EmptyMap_RecognizeEmptyMap()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x80, 0x1 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				Assert.That( target.Read() );
				Assert.That( target.IsMapHeader );
				Assert.That( target.LastReadData == 0, target.LastReadData.ToString() );
				Assert.That( target.ItemsCount, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestReadMapLength_EmptyMap_RecognizeEmptyMap()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x80, 0x1 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				long result;
				Assert.That( target.ReadMapLength( out result ) );
				Assert.That( target.IsMapHeader );
				Assert.That( result, Is.EqualTo( 0 ) );
				Assert.That( target.LastReadData == 0, target.LastReadData.ToString() );
				Assert.That( target.ItemsCount, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestInvalidHeader_MessageTypeException()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xC1 } ) )
			using ( var target = Unpacker.Create( buffer ) )
			{
				Assert.Throws<UnassignedMessageTypeException>( () => target.Read() );
			}
		}
	}
}
