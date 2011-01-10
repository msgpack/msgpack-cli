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
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MsgPack.Collections
{
	[TestFixture]
	public sealed class GCChunkBufferTest
	{
		[Test]
		public void TestCopyTo()
		{
			byte[] source = Enumerable.Range( 1, 100 ).Select( i => ( byte )i ).ToArray();
			List<ArraySegment<byte>> expected = new List<ArraySegment<byte>>();
			expected.Add( new ArraySegment<byte>( source, 13, 10 ) );
			expected.Add( new ArraySegment<byte>( source, 1, 2 ) );
			expected.Add( new ArraySegment<byte>( source, 20, 70 ) );
			expected.Add( new ArraySegment<byte>( source, 98, 1 ) );

			using ( var result = new GCChunkBuffer( expected, expected.Sum( segment => segment.Count ) ) )
			{
				ArraySegment<byte>[] actual = new ArraySegment<byte>[ 6 ];
				result.CopyTo( actual, 1 );
				Assert.IsNull( actual[ 0 ].Array );
				Assert.AreEqual( expected[ 0 ], actual[ 1 ] );
				Assert.AreEqual( expected[ 1 ], actual[ 2 ] );
				Assert.AreEqual( expected[ 2 ], actual[ 3 ] );
				Assert.AreEqual( expected[ 3 ], actual[ 4 ] );
				Assert.IsNull( actual[ 5 ].Array );

				actual = new ArraySegment<byte>[ 4 ];
				result.CopyTo( actual, 0 );
				Assert.AreEqual( expected[ 0 ], actual[ 0 ] );
				Assert.AreEqual( expected[ 1 ], actual[ 1 ] );
				Assert.AreEqual( expected[ 2 ], actual[ 2 ] );
				Assert.AreEqual( expected[ 3 ], actual[ 3 ] );

				actual = new ArraySegment<byte>[ 5 ];
				result.CopyTo( actual, 1 );
				Assert.IsNull( actual[ 0 ].Array );
				Assert.AreEqual( expected[ 0 ], actual[ 1 ] );
				Assert.AreEqual( expected[ 1 ], actual[ 2 ] );
				Assert.AreEqual( expected[ 2 ], actual[ 3 ] );
				Assert.AreEqual( expected[ 3 ], actual[ 4 ] );
			}
		}
		
		[Test]
		public void TestGetEnumerator()
		{
			byte[] source = Enumerable.Range( 1, 100 ).Select( i => ( byte )i ).ToArray();
			List<ArraySegment<byte>> expected = new List<ArraySegment<byte>>();
			expected.Add( new ArraySegment<byte>( source, 10, 10 ) );
			expected.Add( new ArraySegment<byte>( source, 20, 1 ) );
			expected.Add( new ArraySegment<byte>( source, 50, 9 ) );
			expected.Add( new ArraySegment<byte>( source, 70, 20 ) );

			using ( var result = new GCChunkBuffer( expected, expected.Sum( segment => segment.Count ) ) )
			{
				CollectionAssert.AreEqual( ( expected.ReadAll() ).ToArray(), result.ReadAll().ToArray() );
			}
		}

		[Test]
		public void TestFeed()
		{
			byte[] source = Enumerable.Range( 1, 100 ).Select( i => ( byte )i ).ToArray();
			List<ArraySegment<byte>> expected = new List<ArraySegment<byte>>();
			expected.Add( new ArraySegment<byte>( source, 10, 10 ) );
			expected.Add( new ArraySegment<byte>( source, 20, 20 ) );
			expected.Add( new ArraySegment<byte>( source, 50, 10 ) );
			expected.Add( new ArraySegment<byte>( source, 90, 10 ) );

			List<ArraySegment<byte>> actual = new List<ArraySegment<byte>>();
			using ( var result = new GCChunkBuffer( actual, 0 ) )
			{
				long total = 0;
				for ( int i = 0; i < expected.Count; i++ )
				{
					result.Feed( expected[ i ] );
					total += expected[ i ].Count;
					Assert.AreEqual( total, result.TotalLength );
					Assert.AreEqual( i + 1, actual.Count );
					Assert.AreEqual( expected[ i ], actual[ i ] );
				}
			}
		}

		[Test]
		public void TestSubChanks()
		{
			Random random = new Random();

			using ( var result = ChunkBuffer.CreateDefault() )
			{
				Assert.NotNull( result );
				Assert.AreEqual( 8, result.Sum( item => item.Count ) );
				result.Fill( Enumerable.Range( 1, 8 ).Select( item => ( byte )( item % 256 ) ) );

				using ( var subChunk = result.SubChunks( 1, 2 ) )
				{
					Assert.AreNotSame( result, subChunk );
					Assert.AreEqual( 2, subChunk.Sum( item => item.Count ) );
					CollectionAssertEx.StartsWith( Enumerable.Range( 1, 8 ).Select( item => ( byte )( item % 256 ) ).Skip( 1 ).Take( 2 ), subChunk.ReadAll() );
				}

				using ( var subChunk = result.SubChunks( 1, 4 ) )
				{
					Assert.AreNotSame( result, subChunk );
					Assert.AreEqual( 4, subChunk.Sum( item => item.Count ) );
					CollectionAssertEx.StartsWith( Enumerable.Range( 1, 8 ).Select( item => ( byte )( item % 256 ) ).Skip( 1 ).Take( 4 ), subChunk.ReadAll() );
				}

				using ( var subChunk = result.SubChunks( 2, 4 ) )
				{
					Assert.AreNotSame( result, subChunk );
					Assert.AreEqual( 4, subChunk.Sum( item => item.Count ) );
					CollectionAssertEx.StartsWith( Enumerable.Range( 1, 8 ).Select( item => ( byte )( item % 256 ) ).Skip( 2 ).Take( 4 ), subChunk.ReadAll() );
				}

				using ( var subChunk = result.SubChunks( 3, 4 ) )
				{
					Assert.AreNotSame( result, subChunk );
					Assert.AreEqual( 4, subChunk.Sum( item => item.Count ) );
					CollectionAssertEx.StartsWith( Enumerable.Range( 1, 8 ).Select( item => ( byte )( item % 256 ) ).Skip( 3 ).Take( 4 ), subChunk.ReadAll() );
				}
			}

			for ( int i = 0; i < 10000; i++ )
			{
				int length = random.Next( 1, 256 );
				using ( var result = ChunkBuffer.CreateDefault() )
				{
					Assert.NotNull( result );
					Assert.AreEqual( length, result.Sum( item => item.Count ) );
					result.Fill( Enumerable.Range( 1, length ).Select( item => ( byte )( item % 256 ) ) );
					int offset = random.Next( length - 1 );
					int count = random.Next( length - offset );
					try
					{
						using ( var subChunk = result.SubChunks( offset, count ) )
						{
							Assert.AreNotSame( result, subChunk );
							Assert.AreEqual( count, subChunk.Sum( item => item.Count ) );
							CollectionAssertEx.StartsWith( Enumerable.Range( 1, length ).Select( item => ( byte )( item % 256 ) ).Skip( offset ).Take( count ), subChunk.ReadAll() );
						}
					}
					catch
					{
						Console.Error.WriteLine( "{0} -> ({1},{2})", length, offset, count );
						throw;
					}
				}
			}
		}
	}
}
