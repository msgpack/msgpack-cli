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
using System.Collections.Generic;
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
using System.IO;

namespace MsgPack
{
	[TestFixture]
	[Timeout( 1000 )]
	public class SubtreeUnpackerTest
	{
		[Test]
		public void TestNestedArray()
		{
			using ( var stream = new MemoryStream() )
			{
				var packer = Packer.Create( stream );
				packer.PackArray(
					new[] 
					{ 
						new MessagePackObject( new[] { new MessagePackObject( "1-1" ), new MessagePackObject( "1-2" ), new MessagePackObject( "1-3" ) } ),
						new MessagePackObject( new[] { new MessagePackObject( "2-1" ), new MessagePackObject( "2-2" ), new MessagePackObject( "2-3" ) } ),
						new MessagePackObject( new[] { new MessagePackObject( "3-1" ), new MessagePackObject( "3-2" ), new MessagePackObject( "3-3" ) } ),
					}
				);
				stream.Position = 0;
				var unpacker = Unpacker.Create( stream );
				Assert.That( unpacker.Read() );
				using ( Unpacker subtreeReader1 = unpacker.ReadSubtree() )
				{
					Assert.That( subtreeReader1.IsArrayHeader );
					Assert.That( subtreeReader1.ItemsCount, Is.EqualTo( 3 ) );

					for ( int i = 1; subtreeReader1.Read(); i++ )
					{
						using ( Unpacker subtreeReader2 = subtreeReader1.ReadSubtree() )
						{
							Assert.That( subtreeReader2.IsArrayHeader );
							Assert.That( subtreeReader2.ItemsCount, Is.EqualTo( 3 ) );
							for ( int j = 1; subtreeReader2.Read(); j++ )
							{
								Assert.That( subtreeReader2.LastReadData.AsString(), Is.EqualTo( i + "-" + j ) );
							}
						}
					}
				}
			}
		}

		[Test]
		public void TestDeepNestedArray()
		{
			using ( var stream = new MemoryStream() )
			{
				var packer = Packer.Create( stream );
				packer.PackArray(
					new[] 
					{ 
						new MessagePackObject( 
							new[] 
							{
								new MessagePackObject(
									new [] { new MessagePackObject( "1-1-1" ), new MessagePackObject( "1-1-2" ), new MessagePackObject( "1-1-3" ) }
								),
								new MessagePackObject(
									new [] { new MessagePackObject( "1-2-1" ), new MessagePackObject( "1-2-2" ), new MessagePackObject( "1-2-3" ) }
								),
								new MessagePackObject(
									new [] { new MessagePackObject( "1-3-1" ), new MessagePackObject( "1-3-2" ), new MessagePackObject( "1-3-3" ) }
								),
							}
						),
						new MessagePackObject( 
							new[] 
							{
								new MessagePackObject(
									new [] { new MessagePackObject( "2-1-1" ), new MessagePackObject( "2-1-2" ), new MessagePackObject( "2-1-3" ) }
								),
								new MessagePackObject(
									new [] { new MessagePackObject( "2-2-1" ), new MessagePackObject( "2-2-2" ), new MessagePackObject( "2-2-3" ) }
								),
								new MessagePackObject(
									new [] { new MessagePackObject( "2-3-1" ), new MessagePackObject( "2-3-2" ), new MessagePackObject( "2-3-3" ) }
								),
							}
						),
						new MessagePackObject( 
							new[] 
							{
								new MessagePackObject(
									new [] { new MessagePackObject( "3-1-1" ), new MessagePackObject( "3-1-2" ), new MessagePackObject( "3-1-3" ) }
								),
								new MessagePackObject(
									new [] { new MessagePackObject( "3-2-1" ), new MessagePackObject( "3-2-2" ), new MessagePackObject( "3-2-3" ) }
								),
								new MessagePackObject(
									new [] { new MessagePackObject( "3-3-1" ), new MessagePackObject( "3-3-2" ), new MessagePackObject( "3-3-3" ) }
								),
							}
						),
					}
				);
				stream.Position = 0;
				var unpacker = Unpacker.Create( stream );
				Assert.That( unpacker.Read() );
				using ( Unpacker subtreeReader1 = unpacker.ReadSubtree() )
				{
					Assert.That( subtreeReader1.IsArrayHeader );
					Assert.That( subtreeReader1.ItemsCount, Is.EqualTo( 3 ) );

					for ( int i = 1; subtreeReader1.Read(); i++ )
					{
						using ( Unpacker subtreeReader2 = subtreeReader1.ReadSubtree() )
						{
							Assert.That( subtreeReader2.IsArrayHeader );
							Assert.That( subtreeReader2.ItemsCount, Is.EqualTo( 3 ) );
							for ( int j = 1; subtreeReader2.Read(); j++ )
							{
								using ( Unpacker subtreeReader3 = subtreeReader2.ReadSubtree() )
								{
									Assert.That( subtreeReader3.IsArrayHeader );
									Assert.That( subtreeReader3.ItemsCount, Is.EqualTo( 3 ) );
									for ( int k = 1; subtreeReader3.Read(); k++ )
									{
										Assert.That( subtreeReader3.LastReadData.AsString(), Is.EqualTo( i + "-" + j + "-" + k ) );
									}
								}
							}
						}
					}
				}
			}
		}

		[Test]
		public void TestDispose_Tail_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x91, 0x2, 0x3 } ) )
			using ( var root = Unpacker.Create( buffer ) )
			{
				int lastValue;
				Assert.That( root.ReadInt32( out lastValue ) );
				Assert.That( lastValue, Is.EqualTo( 1 ) );

				long length;
				Assert.That( root.ReadArrayLength( out length ) );
				Assert.That( length, Is.EqualTo( 1 ) );

				using ( var subtree = root.ReadSubtree() )
				{
					Assert.That( subtree.ReadInt32( out lastValue ) );
					Assert.That( lastValue, Is.EqualTo( 2 ) );
				}

				Assert.That( root.ReadInt32( out lastValue ) );
				Assert.That( lastValue, Is.EqualTo( 3 ) );
			}
		}

		[Test]
		public void TestDispose_NotTail_Drained()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x92, 0x2, 0x3, 0x4 } ) )
			using ( var root = Unpacker.Create( buffer ) )
			{
				int lastValue;
				Assert.That( root.ReadInt32( out lastValue ) );
				Assert.That( lastValue, Is.EqualTo( 1 ) );

				long length;
				Assert.That( root.ReadArrayLength( out length ) );
				Assert.That( length, Is.EqualTo( 2 ) );

				using ( var subtree = root.ReadSubtree() )
				{
					Assert.That( subtree.ReadInt32( out lastValue ) );
					Assert.That( lastValue, Is.EqualTo( 2 ) );
				}

				Assert.That( root.ReadInt32( out lastValue ) );
				Assert.That( lastValue, Is.EqualTo( 4 ) );
			}
		}

		[Test]
		public void TestReadSubtree_Twice_Error()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x91, 0x91, 0x2, 0x3 } ) )
			using ( var root = Unpacker.Create( buffer ) )
			{
				int lastValue;
				Assert.That( root.ReadInt32( out lastValue ) );
				Assert.That( lastValue, Is.EqualTo( 1 ) );

				long length;
				Assert.That( root.ReadArrayLength( out length ) );
				Assert.That( length, Is.EqualTo( 1 ) );

				using ( var subtree = root.ReadSubtree() )
				{
					Assert.That( subtree.ReadArrayLength( out length ) );
					Assert.That( length, Is.EqualTo( 1 ) );

					Assert.Throws<InvalidOperationException>( () => root.ReadSubtree() );
				}
			}
		}

		[Test]
		public void TestDispose_TailNested_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x91, 0x91, 0x2, 0x3 } ) )
			using ( var root = Unpacker.Create( buffer ) )
			{
				int lastValue;
				Assert.That( root.ReadInt32( out lastValue ) );
				Assert.That( lastValue, Is.EqualTo( 1 ) );

				long length;
				Assert.That( root.ReadArrayLength( out length ) );
				Assert.That( length, Is.EqualTo( 1 ) );

				using ( var subtree = root.ReadSubtree() )
				{
					Assert.That( subtree.ReadArrayLength( out length ) );
					Assert.That( length, Is.EqualTo( 1 ) );

					using ( var subsubtree = subtree.ReadSubtree() )
					{
						Assert.That( subsubtree.ReadInt32( out lastValue ) );
						Assert.That( lastValue, Is.EqualTo( 2 ) );
					}
				}

				Assert.That( root.ReadInt32( out lastValue ) );
				Assert.That( lastValue, Is.EqualTo( 3 ) );
			}
		}

		[Test]
		public void TestDispose_NotTailNested_Drained()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x1, 0x92, 0x92, 0x2, 0x3, 0x4, 0x5 } ) )
			using ( var root = Unpacker.Create( buffer ) )
			{
				int lastValue;
				Assert.That( root.ReadInt32( out lastValue ) );
				Assert.That( lastValue, Is.EqualTo( 1 ) );

				long length;
				Assert.That( root.ReadArrayLength( out length ) );
				Assert.That( length, Is.EqualTo( 2 ) );

				using ( var subtree = root.ReadSubtree() )
				{
					Assert.That( subtree.ReadArrayLength( out length ) );
					Assert.That( length, Is.EqualTo( 2 ) );

					using ( var subsubtree = subtree.ReadSubtree() )
					{
						Assert.That( subsubtree.ReadInt32( out lastValue ) );
						Assert.That( lastValue, Is.EqualTo( 2 ) );
					}
				}

				Assert.That( root.ReadInt32( out lastValue ) );
				Assert.That( lastValue, Is.EqualTo( 5 ) );
			}
		}
	}
}
