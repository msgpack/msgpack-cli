 
 
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

#if MSTEST
#pragma warning disable 162
#endif // MSTEST

using System;
using System.IO;
using System.Linq;
using System.Text;
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
	// This file was generated from UnpackerTest.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit UnpackerTest.tt and StreamingUnapkcerBase.ttinclude instead.

	public abstract partial class UnpackerTest
	{
		protected abstract bool ShouldCheckStreamPosition { get; }

		protected abstract bool ShouldCheckSubtreeUnpacker { get; }

		protected abstract bool CanReadFromEmptySource { get; }

		protected abstract bool MayFailToRollback { get; }

		protected abstract Unpacker CreateUnpacker( MemoryStream stream );

		protected abstract bool CanRevert( Unpacker unpacker );

		protected abstract long GetOffset( Unpacker unpacker );

		[Test]
		public void TestRead_ScalarSequence_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
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
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
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
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
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
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
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
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
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
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
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
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
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
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x94, 0x91, 0x1, 0x90, 0xC0, 0x92, 0x1, 0x2, 0x91, 0x1 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
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
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x84, 0x1, 0x81, 0x1, 0x1, 0x2, 0x80, 0x3, 0xC0, 0x4, 0x82, 0x1, 0x1, 0x2, 0x2, 0x81, 0x1, 0x1 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
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
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x92, 0x1, 0x91, 0x1, 0x2 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
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
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
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
		public void TestRead_UnderSkipping()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0xD1, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				Assert.That( target.Skip(), Is.Null, "Precondition" );
				Assert.Throws<InvalidOperationException>( () => target.Read() );
			}
		}

		[Test]
		public void TestGetEnumerator_UnderSkipping()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0xD1, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
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
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0xD1, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				Assert.That( target.Skip(), Is.Null, "Precondition" );
				Assert.Throws<InvalidOperationException>( () => target.ReadSubtree() );
			}
		}

		[Test]
		public void TestRead_UnderEnumerating()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
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
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
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
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
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
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
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
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0xD0, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				Assert.Throws<InvalidOperationException>( () => target.ReadSubtree() );
			}
		}

		[Test]
		public void TestReadSubtree_InNestedScalar()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x81, 0x1, 0x91, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				Assert.That( target.Read() );
				Assert.That( target.IsMapHeader, Is.True );
				Assert.That( target.Read() );
				Assert.That( target.IsMapHeader, Is.False );
				Assert.Throws<InvalidOperationException>( () => target.ReadSubtree() );
			}
		}

		[Test]
		public void TestReadItem_OneScalar_AsScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.ReadItem();
				Assert.That( result.HasValue );
				Assert.That( result.Value == 1, result.Value.ToString() );
				Assert.That( target.ReadItem(), Is.Null );
			}
		}

		[Test]
		public void TestReadItem_TwoScalar_AsTwoScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
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
		public void TestReadItem_Empty_Null()
		{
			if ( this.CanReadFromEmptySource )
			{
				using ( var buffer = new MemoryStream( new byte[ 0 ] ) )
				using ( var target = this.CreateUnpacker( buffer ) )
				{
					var result = target.ReadItem();
					Assert.That( result.HasValue, Is.False );
				}
			}
			else
			{
				using ( var buffer = new MemoryStream( new byte[ 0 ] ) )
				{
					Assert.Throws<ArgumentException>(
						() => this.CreateUnpacker( buffer )
					);
				}
			}
		}

		[Test]
		public void TestReadItem_Array_AsSingleArray()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x1, 0x2 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
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
		public void TestReadItem_Map_AsSingleMap()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x82, 0x1, 0x2, 0x3, 0x4 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
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
		public void TestReadItem_ArrayFollowingScalar_AsSingleArrayAndScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x1, 0x2, 0x3 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
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
		public void TestReadItem_MapFollowingScalar_AsSingleMapAndScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x82, 0x1, 0x2, 0x3, 0x4, 0x5 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
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
		public void TestReadItem_ArrayOfArray_AsSingleArray()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x92, 11, 12, 0x92, 21, 22 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
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
		public void TestReadItem_MapOfMap_AsSingleMap()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x82, 1, 0x82, 11, 1, 12, 2, 2, 0x82, 21, 1, 22, 2 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
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
			var str = String.Concat( Enumerable.Range( 0, 0x1FFFF ).Where( i => i < 0xD800 || 0xDFFF < i ).Select( ConvertFromUtf32 ) );
			var encoded = Encoding.UTF8.GetBytes( str );
			using ( var buffer =
				new MemoryStream(
					new byte[] { MessagePackCode.Raw32 }.Concat(
						BitConverter.IsLittleEndian ? BitConverter.GetBytes( encoded.Length ).Reverse() : BitConverter.GetBytes( encoded.Length )
					).Concat( encoded )
					.ToArray()
				)
			)
			using ( var target = this.CreateUnpacker( buffer ) )
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
			using ( var target = this.CreateUnpacker( buffer ) )
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
			using ( var target = this.CreateUnpacker( buffer ) )
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
			using ( var target = this.CreateUnpacker( buffer ) )
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
			using ( var target = this.CreateUnpacker( buffer ) )
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
		public void TestRead_InvalidHeader_MessageTypeException()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xC1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				Assert.Throws<UnassignedMessageTypeException>( () => target.Read() );
			}
		}

#if FEATURE_TAP

		[Test]
		public async Task TestReadAsync_ScalarSequence_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await rootUnpacker.ReadAsync(), "1st" );
				Assert.That( rootUnpacker.LastReadData.Equals( 1 ) );
				Assert.That( await rootUnpacker.ReadAsync(), "2nd" );
				Assert.That( rootUnpacker.LastReadData.Equals( 2 ) );
				Assert.That( await rootUnpacker.ReadAsync(), "3rd" );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_Array_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x93, 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
			{
#pragma warning disable 612,618
				Assert.That( await rootUnpacker.ReadAsync(), "1st" );
				Assert.That( rootUnpacker.IsArrayHeader );
				Assert.That( rootUnpacker.IsMapHeader, Is.False );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) ); // == Length
				Assert.That( await rootUnpacker.ReadAsync(), "2nd" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 1 ) );
				Assert.That( await rootUnpacker.ReadAsync(), "3rd" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 2 ) );
				Assert.That( await rootUnpacker.ReadAsync(), "4th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
#pragma warning restore 612,618
			}
		}


		[Test]
		public async Task TestReadAsync_Map_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x83, 0x1, 0x1, 0x2, 0x2, 0x3, 0x3 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
			{
#pragma warning disable 612,618
				Assert.That( await rootUnpacker.ReadAsync(), "1st" );
				Assert.That( rootUnpacker.IsArrayHeader, Is.False );
				Assert.That( rootUnpacker.IsMapHeader );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) ); // == Length
				Assert.That( await rootUnpacker.ReadAsync(), "2nd" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 1 ) );
				Assert.That( await rootUnpacker.ReadAsync(), "3rd" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 1 ) );
				Assert.That( await rootUnpacker.ReadAsync(), "4th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 2 ) );
				Assert.That( await rootUnpacker.ReadAsync(), "5th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 2 ) );
				Assert.That( await rootUnpacker.ReadAsync(), "6th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
				Assert.That( await rootUnpacker.ReadAsync(), "7th" );
				Assert.That( rootUnpacker.Data, Is.Not.Null );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
#pragma warning restore 612,618
			}
		}

		[Test]
		public async Task TestReadAsync_ReadInTail_NoEffect()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await rootUnpacker.ReadAsync(), "1st" );
				Assert.That( rootUnpacker.LastReadData.Equals( 1 ) );
				Assert.That( await rootUnpacker.ReadAsync(), "2nd" );
				Assert.That( rootUnpacker.LastReadData.Equals( 2 ) );
				Assert.That( await rootUnpacker.ReadAsync(), "3rd" );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
				Assert.That( await rootUnpacker.ReadAsync(), Is.False, "Tail" );
				// Data should be last read.
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_ReadInSubtreeTail_NoEffect()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				await Task.Delay( 0 );
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x1, 0x2, 0x3 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await rootUnpacker.ReadAsync(), "Top Level" );
				Assert.That( rootUnpacker.IsArrayHeader );

				using ( var subTreeReader = rootUnpacker.ReadSubtree() )
				{
					Assert.That( await subTreeReader.ReadAsync(), "1st" );
					Assert.That( subTreeReader.LastReadData.Equals( 1 ) );
					Assert.That( await subTreeReader.ReadAsync(), "2nd" );
					Assert.That( subTreeReader.LastReadData.Equals( 2 ) );
					Assert.That( await subTreeReader.ReadAsync(), Is.False, "Tail" );
					// Data should be last read.
					Assert.That( subTreeReader.LastReadData.Equals( 2 ) );
				}

				Assert.That( await rootUnpacker.ReadAsync(), "3rd" );
				Assert.That( rootUnpacker.LastReadData.Equals( 3 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_InSubtreeMode_Fail()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				await Task.Delay( 0 );
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await rootUnpacker.ReadAsync(), "Failed to first read" );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					// To be failed.
					AssertEx.ThrowsAsync<InvalidOperationException>( async () => await rootUnpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadSubtreeAsync_IsScalar_Fail()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				await Task.Delay( 0 );
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await rootUnpacker.ReadAsync(), "Failed to first read" );

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
		public async Task TestReadSubtreeAsync_NestedArray_Success()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				await Task.Delay( 0 );
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x94, 0x91, 0x1, 0x90, 0xC0, 0x92, 0x1, 0x2, 0x91, 0x1 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await rootUnpacker.ReadAsync(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );

					using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 1 ) );
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.False );
					}

					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );

					using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.False );
					}

					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.IsNil );

					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );

					using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 1 ) );
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 2 ) );
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.False );
					}

					Assert.That( await subTreeUnpacker.ReadAsync(), Is.False );
				}

				Assert.That( await rootUnpacker.ReadAsync(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) );
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.False );
				}

				Assert.That( await rootUnpacker.ReadAsync(), Is.False );
			}
		}

		[Test]
		public async Task TestReadSubtreeAsync_NestedMap_Success()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				await Task.Delay( 0 );
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x84, 0x1, 0x81, 0x1, 0x1, 0x2, 0x80, 0x3, 0xC0, 0x4, 0x82, 0x1, 0x1, 0x2, 0x2, 0x81, 0x1, 0x1 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await rootUnpacker.ReadAsync(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) );
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );

					using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 1 ) );
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 1 ) );
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.False );
					}

					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 2 ) );
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );

					using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.False );
					}

					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 3 ) );
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.IsNil );

					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 4 ) );
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );

					using ( var subSubtreeUnpacker = subTreeUnpacker.ReadSubtree() )
					{
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 1 ) );
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 1 ) );
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 2 ) );
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.True );
						Assert.That( subSubtreeUnpacker.LastReadData.Equals( 2 ) );
						Assert.That( await subSubtreeUnpacker.ReadAsync(), Is.False );
					}

					Assert.That(await  subTreeUnpacker.ReadAsync(), Is.False );
				}

				Assert.That( await rootUnpacker.ReadAsync(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) );
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) );
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.False );
				}

				Assert.That( await rootUnpacker.ReadAsync(), Is.False );
			}
		}

		[Test]
		public async Task TestReadSubtreeAsync_Nested_ReadGrandchildren_Success()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				await Task.Delay( 0 );
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x92, 0x1, 0x91, 0x1, 0x2 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await rootUnpacker.ReadAsync(), Is.True );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 2 ) ); // Array Length
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) ); // Value in grand children
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) ); // Array Length
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 1 ) ); // Value in grand children
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.True );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 2 ) ); // Value in children
					Assert.That( await subTreeUnpacker.ReadAsync(), Is.False );
				}

				Assert.That( await rootUnpacker.ReadAsync(), Is.False );
			}
		}

		[Test]
		public async Task TestReadSubtreeAsync_InLeafBody_Fail()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				await Task.Delay( 0 );
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var rootUnpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await rootUnpacker.ReadAsync(), "Failed to first read" );

				using ( var subTreeUnpacker = rootUnpacker.ReadSubtree() )
				{
					Assert.That( await subTreeUnpacker.ReadAsync(), "Failed to move to first body." );
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
		public async Task TestReadAsync_UnderSkipping()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				await Task.Delay( 0 );
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0xD1, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await target.SkipAsync(), Is.Null, "Precondition" );
				AssertEx.ThrowsAsync<InvalidOperationException>( async () => await target.ReadAsync() );
			}
		}

		[Test]
		public async Task TestGetEnumeratorAsync_UnderSkipping()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				await Task.Delay( 0 );
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0xD1, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await target.SkipAsync(), Is.Null, "Precondition" );
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
		public async Task TestReadSubtreeAsync_UnderSkipping()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				await Task.Delay( 0 );
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0xD1, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await target.SkipAsync(), Is.Null, "Precondition" );
				Assert.Throws<InvalidOperationException>( () => target.ReadSubtree() );
			}
		}

		[Test]
		public void TestReadAsync_UnderEnumerating()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				foreach ( var item in target )
				{
					AssertEx.ThrowsAsync<InvalidOperationException>( async () => await target.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestSkipAsync_UnderEnumerating()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				foreach ( var item in target )
				{
					AssertEx.ThrowsAsync<InvalidOperationException>( async () => await target.SkipAsync() );
				}
			}
		}

		[Test]
		public void TestReadSubtreeAsync_UnderEnumerating()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				foreach ( var item in target )
				{
					Assert.Throws<InvalidOperationException>( () => target.ReadSubtree() );
				}
			}
		}

		[Test]
		public async Task TestReadSubtreeAsync_InRootHead_Success()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				await Task.Delay( 0 );
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await target.ReadAsync() );
				Assert.That( target.IsArrayHeader );

				using ( var subTreeUnpacker = target.ReadSubtree() )
				{
					Assert.That( await subTreeUnpacker.ReadAsync() );
					Assert.That( subTreeUnpacker.LastReadData.Equals( 0x1 ) );
				}
			}
		}


		[Test]
		public async Task TestReadSubtreeAsync_InNestedScalar()
		{
			if ( !this.ShouldCheckSubtreeUnpacker )
			{
#if MSTEST
				// MSTEST cannot handle inconclusive in async test correctly.
				await Task.Delay( 0 );
				return;
#endif // MSTEST
				Assert.Ignore( "Cannot test subtree unpacker in " + this.GetType().Name );
			}

			using ( var buffer = new MemoryStream( new byte[] { 0x81, 0x1, 0x91, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await target.ReadAsync() );
				Assert.That( target.IsMapHeader, Is.True );
				Assert.That(await  target.ReadAsync() );
				Assert.That( target.IsMapHeader, Is.False );
				Assert.Throws<InvalidOperationException>( () => target.ReadSubtree() );
			}
		}

		[Test]
		public async Task TestReadItemAsync_OneScalar_AsScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.ReadItemAsync();
				Assert.That( result.HasValue );
				Assert.That( result.Value == 1, result.Value.ToString() );
				Assert.That( await target.ReadItemAsync(), Is.Null );
			}
		}

		[Test]
		public async Task TestReadItemAsync_TwoScalar_AsTwoScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x2 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result1 = await target.ReadItemAsync();
				Assert.That( result1.HasValue );
				Assert.That( result1.Value == 1, result1.Value.ToString() );

				var result2 = await target.ReadItemAsync();
				Assert.That( result2.HasValue );
				Assert.That( result2.Value == 2, result2.Value.ToString() );

				Assert.That( await target.ReadItemAsync(), Is.Null );
			}
		}

		[Test]
		public void TestReadItemAsync_Empty_Null()
		{
			if ( this.CanReadFromEmptySource )
			{
				using ( var buffer = new MemoryStream( new byte[ 0 ] ) )
				using ( var target = this.CreateUnpacker( buffer ) )
				{
					var result = target.ReadItemAsync().Result;
					Assert.That( result.HasValue, Is.False );
				}
			}
			else
			{
				using ( var buffer = new MemoryStream( new byte[ 0 ] ) )
				{
					Assert.Throws<ArgumentException>(
						() => this.CreateUnpacker( buffer )
					);
				}
			}
		}

		[Test]
		public async Task TestReadItemAsync_Array_AsSingleArray()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x1, 0x2 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.ReadItemAsync();
				Assert.That( result.HasValue );
				var array = result.Value.AsList();
				Assert.That( array.Count, Is.EqualTo( 2 ), result.Value.ToString() );
				Assert.That( array[ 0 ] == 1, result.Value.ToString() );
				Assert.That( array[ 1 ] == 2, result.Value.ToString() );
				Assert.That( await target.ReadItemAsync(), Is.Null );
			}
		}

		[Test]
		public async Task TestReadItemAsync_Map_AsSingleMap()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x82, 0x1, 0x2, 0x3, 0x4 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.ReadItemAsync();
				Assert.That( result.HasValue );
				var map = result.Value.AsDictionary();
				Assert.That( map.Count, Is.EqualTo( 2 ), result.Value.ToString() );
				Assert.That( map[ 1 ] == 2, result.Value.ToString() );
				Assert.That( map[ 3 ] == 4, result.Value.ToString() );
				Assert.That( await target.ReadItemAsync(), Is.Null );
			}
		}

		[Test]
		public async Task TestReadItemAsync_ArrayFollowingScalar_AsSingleArrayAndScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x1, 0x2, 0x3 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.ReadItemAsync();
				Assert.That( result.HasValue );
				var array = result.Value.AsList();
				Assert.That( array.Count, Is.EqualTo( 2 ), result.Value.ToString() );
				Assert.That( array[ 0 ] == 1, result.Value.ToString() );
				Assert.That( array[ 1 ] == 2, result.Value.ToString() );

				var scalar = await target.ReadItemAsync();
				Assert.That( scalar.HasValue );
				Assert.That( scalar.Value == 3, result.Value.ToString() );
				Assert.That( await target.ReadItemAsync(), Is.Null );
			}
		}

		[Test]
		public async Task TestReadItemAsync_MapFollowingScalar_AsSingleMapAndScalar()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x82, 0x1, 0x2, 0x3, 0x4, 0x5 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result =await  target.ReadItemAsync();
				Assert.That( result.HasValue );
				var map = result.Value.AsDictionary();
				Assert.That( map.Count, Is.EqualTo( 2 ), result.Value.ToString() );
				Assert.That( map[ 1 ] == 2, result.Value.ToString() );
				Assert.That( map[ 3 ] == 4, result.Value.ToString() );

				var scalar = await target.ReadItemAsync();
				Assert.That( scalar.HasValue );
				Assert.That( scalar.Value == 5, result.Value.ToString() );
				Assert.That( await target.ReadItemAsync(), Is.Null );
			}
		}

		[Test]
		public async Task TestReadItemAsync_ArrayOfArray_AsSingleArray()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x92, 0x92, 11, 12, 0x92, 21, 22 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.ReadItemAsync();
				Assert.That( result.HasValue );
				var array = result.Value.AsList();
				Assert.That( array.Count, Is.EqualTo( 2 ), result.Value.ToString() );
				Assert.That( array[ 0 ].IsArray, result.Value.ToString() );
				Assert.That( array[ 0 ].AsList()[ 0 ] == 11, result.Value.ToString() );
				Assert.That( array[ 0 ].AsList()[ 1 ] == 12, result.Value.ToString() );
				Assert.That( array[ 1 ].IsArray, result.Value.ToString() );
				Assert.That( array[ 1 ].AsList()[ 0 ] == 21, result.Value.ToString() );
				Assert.That( array[ 1 ].AsList()[ 1 ] == 22, result.Value.ToString() );
				Assert.That( await target.ReadItemAsync(), Is.Null );
			}
		}


		[Test]
		public async Task TestReadItemAsync_MapOfMap_AsSingleMap()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x82, 1, 0x82, 11, 1, 12, 2, 2, 0x82, 21, 1, 22, 2 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.ReadItemAsync();
				Assert.That( result.HasValue );
				var map = result.Value.AsDictionary();
				Assert.That( map.Count, Is.EqualTo( 2 ), result.Value.ToString() );
				Assert.That( map[ 1 ].IsMap, result.Value.ToString() );
				Assert.That( map[ 1 ].AsDictionary()[ 11 ] == 1, result.Value.ToString() );
				Assert.That( map[ 1 ].AsDictionary()[ 12 ] == 2, result.Value.ToString() );
				Assert.That( map[ 2 ].IsMap, result.Value.ToString() );
				Assert.That( map[ 2 ].AsDictionary()[ 21 ] == 1, result.Value.ToString() );
				Assert.That( map[ 2 ].AsDictionary()[ 22 ] == 2, result.Value.ToString() );
				Assert.That( await target.ReadItemAsync(), Is.Null );
			}
		}

		[Test]
		public async Task TestReadStringAsync_Clob()
		{
			var str = String.Concat( Enumerable.Range( 0, 0x1FFFF ).Where( i => i < 0xD800 || 0xDFFF < i ).Select( ConvertFromUtf32 ) );
			var encoded = Encoding.UTF8.GetBytes( str );
			using ( var buffer =
				new MemoryStream(
					new byte[] { MessagePackCode.Raw32 }.Concat(
						BitConverter.IsLittleEndian ? BitConverter.GetBytes( encoded.Length ).Reverse() : BitConverter.GetBytes( encoded.Length )
					).Concat( encoded )
					.ToArray()
				)
			)
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				string result;
				var ret = await target.ReadStringAsync();
				Assert.That( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( str ) );
			}
		}

		[Test]
		public async Task TestReadAsync_EmptyArray_RecognizeEmptyArray()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x90, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await target.ReadAsync() );
				Assert.That( target.IsArrayHeader );
				Assert.That( target.LastReadData == 0, target.LastReadData.ToString() );
				Assert.That( target.ItemsCount, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadArrayLengthAsync_EmptyArray_RecognizeEmptyArray()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x90, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				long result;
				var ret = await target.ReadArrayLengthAsync();
				Assert.That( ret.Success );
				result = ret.Value;
				Assert.That( target.IsArrayHeader );
				Assert.That( result, Is.EqualTo( 0 ) );
				Assert.That( target.LastReadData == 0, target.LastReadData.ToString() );
				Assert.That( target.ItemsCount, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadAsync_EmptyMap_RecognizeEmptyMap()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x80, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				Assert.That( await target.ReadAsync() );
				Assert.That( target.IsMapHeader );
				Assert.That( target.LastReadData == 0, target.LastReadData.ToString() );
				Assert.That( target.ItemsCount, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public async Task TestReadMapLengthAsync_EmptyMap_RecognizeEmptyMap()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x80, 0x1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				long result;
				var ret = await target.ReadMapLengthAsync();
				Assert.That( ret.Success );
				result = ret.Value;
				Assert.That( target.IsMapHeader );
				Assert.That( result, Is.EqualTo( 0 ) );
				Assert.That( target.LastReadData == 0, target.LastReadData.ToString() );
				Assert.That( target.ItemsCount, Is.EqualTo( 0 ) );
			}
		}

		[Test]
		public void TestReadAsync_InvalidHeader_MessageTypeException()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xC1 } ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				AssertEx.ThrowsAsync<UnassignedMessageTypeException>( async () => await target.ReadAsync() );
			}
		}

#endif // FEATURE_TAP


		private static string ConvertFromUtf32( int utf32 )
		{
#if !SILVERLIGHT
			return Char.ConvertFromUtf32( utf32 );
#else
			// From coreclr source: https://github.com/dotnet/coreclr/blob/master/src/mscorlib/shared/System/Char.cs

			// For UTF32 values from U+00D800 ~ U+00DFFF, we should throw.  They
			// are considered as irregular code unit sequence, but they are not illegal.
			if ( ( utf32 < 0 || utf32 > 0x10ffff ) || ( utf32 >= 0x00d800 && utf32 <= 0x00dfff ) )
			{
				throw new ArgumentOutOfRangeException( "utf32" );
			}

			if ( utf32 < 0x10000 )
			{
				// This is a BMP character.
				return ( Char.ToString( ( char )utf32 ) );
			}

			// This is a supplementary character.  Convert it to a surrogate pair in UTF-16.
			utf32 -= 0x10000;
			uint surrogate = 0; // allocate 2 chars worth of stack space
			char[] chars = new char[ 2 ];
			chars[0] = ( char )( ( utf32 / 0x400 ) + ( int )'\ud800' );
			chars[1] = ( char )( ( utf32 % 0x400 ) + ( int )'\udc00' );
			return new String( chars );
#endif // SILVERLIGHT
		}

		protected sealed class NonSeekableStream : MemoryStream
		{
			public override bool CanSeek
			{
				get { return false; }
			}

			public NonSeekableStream( byte[] buffer )
				: base( buffer ) { }
		}
	}
}
