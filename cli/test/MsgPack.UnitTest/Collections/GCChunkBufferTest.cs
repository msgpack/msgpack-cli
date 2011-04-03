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
		[Timeout( 5000 )]
		public void TestClip()
		{
			// Odd case (3 segments), head/tail are edge of segments.
			TestClipCore( new[] { 5, 6, 7 }, 0, 18 ); // whole
			TestClipCore( new[] { 4, 5, 6, 7, 8 }, 4, 18 );
			// Odd case (3 segments), head is tail of the segment, tail is head of the segment.
			TestClipCore( new[] { 4, 5, 6, 7, 8 }, 8, 8 );
			// Odd case (3 segments), head/tail are midpoint of segments.
			TestClipCore( new[] { 4, 5, 6, 7, 8 }, 6, 10 );
			// Even case (2 segments), head/tail are edge of segments.
			TestClipCore( new[] { 5, 6 }, 0, 11 ); // whole
			TestClipCore( new[] { 4, 5, 6, 7 }, 4, 11 );
			// Even case (2 segments), head is tail of the segment, tail is head of the segment.
			TestClipCore( new[] { 4, 5, 6, 7 }, 8, 2 );
			// Even case (2 segments), head/tail are midpoint of segments.
			TestClipCore( new[] { 4, 5, 6, 7 }, 6, 6 );
			// 1 segment, head/tail are edge of segment.
			TestClipCore( new[] { 7 }, 0, 7 );
			// 1 segment, head/tail are midpoint of segment.
			TestClipCore( new[] { 7 }, 1, 5 );
			// Clip head to mid.
			TestClipCore( new[] { 4, 5, 6, 7, 8 }, 0, 10 );
			// Clip mid to tail.
			TestClipCore( new[] { 4, 5, 6, 7, 8 }, 20, 10 );
			// Random test.
			var random = new Random();
			for ( int i = 0; i < 100; i++ )
			{
				var segmentLengthes = new List<int>();
				var segmentCount = random.Next( 99 ) + 1;
				for ( int j = 0; j < segmentCount; j++ )
				{
					segmentLengthes.Add( random.Next( 99 ) + 1 );
				}
				int offset = random.Next( segmentLengthes.Sum() );
				int length = random.Next( segmentLengthes.Sum() - offset );
				try { }
				catch
				{
					Console.WriteLine( "SegmentLenghtes:" );
					for ( int k = 0; k < segmentLengthes.Count; k++ )
					{
						Console.WriteLine( "\t[{0}]:{1}", k, segmentLengthes[ k ] );
					}

					Console.WriteLine( "offset:{0}", offset );
					Console.WriteLine( "length:{0}", length );

					throw;
				}
			}
		}

		private static void TestClipCore( IList<int> segmentLengthes, int offset, int length )
		{
			// one array pattern
			{
				var segments = new List<ArraySegment<byte>>( segmentLengthes.Count );
				var underlying = Enumerable.Range( 0, segmentLengthes.Sum() ).Select( item => ( byte )( item % Byte.MaxValue ) ).ToArray();
				int count = 0;
				foreach ( var segmentLength in segmentLengthes )
				{
					segments.Add( new ArraySegment<byte>( underlying, count, segmentLength ) );
					count += segmentLength;
				}

				// Do test
				TestClipCore( segmentLengthes, offset, length, segments );
			}

			// multi array pattern
			{
				var segments = new List<ArraySegment<byte>>( segmentLengthes.Count );
				var underlyings = new List<byte[]>( segmentLengthes.Sum() / 8 + 1 );
				int count = 0;
				foreach ( var segmentLength in segmentLengthes )
				{
					for ( int i = 0; i < ( segmentLength / 8 ); i++ )
					{
						var underlying = Enumerable.Range( count, 8 ).Select( item => ( byte )( item % Byte.MaxValue ) ).ToArray();
						segments.Add( new ArraySegment<byte>( underlying, 0, 8 ) );
						count += 8;
						underlyings.Add( underlying );
					}

					if ( segmentLength % 8 > 0 )
					{
						var underlying = Enumerable.Range( count, segmentLength % 8 ).Select( item => ( byte )( item % Byte.MaxValue ) ).ToArray();
						segments.Add( new ArraySegment<byte>( underlying, 0, segmentLength % 8 ) );
						count += segmentLength % 8;
						underlyings.Add( underlying );
					}
				}

				// Do test.
				TestClipCore( segmentLengthes, offset, length, segments );
			}
		}

		private static void TestClipCore( IList<int> segmentLengthes, int offset, int length, List<ArraySegment<byte>> segments )
		{
			var target = new GCChunkBuffer( segments, segmentLengthes.Sum() );
			var actualClipped = target.Clip( offset, length ).ToArray();
			var expectedClipped = Enumerable.Range( offset, length ).ToArray();
			CollectionAssert.AreEquivalent( expectedClipped, actualClipped.ReadAll(), ToString( actualClipped.ReadAll() ) );
			var actualRemains = target.ReadAll().ToArray();
			Assert.AreEqual( segmentLengthes.Sum() - length, actualRemains.Length, ToString( actualRemains ) );

			for ( int i = 0; i < offset; i++ )
			{
				Assert.AreEqual( ( byte )( i % Byte.MaxValue ), actualRemains[ i ], "[{0}] ([{1}])", i, ToString( actualRemains ) );
			}

			for ( int i = offset; i < segmentLengthes.Sum() - length; i++ )
			{
				Assert.AreEqual( ( byte )( ( i + length ) % Byte.MaxValue ), actualRemains[ i ], "[{0}] ([{1}])", i, ToString( actualRemains ) );
			}
		}

		private static string ToString<T>( IEnumerable<T> source )
		{
			try
			{
				return "[" + source.Select( item => item.ToString() ).Aggregate( ( left, right ) => left + ", " + right ) + "]";
			}
			catch ( InvalidOperationException )
			{
				return "[]";
			}
		}
	}
}
