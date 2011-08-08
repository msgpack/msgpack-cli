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

#define SKIP_LARGE_TEST

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MsgPack
{
	[TestFixture]
	[Timeout( 1000 )]
	public partial class PackUnpackTest
	{
		private static readonly Encoding _utf8NoBom =
			new UTF8Encoding( false, false );

		private static MessagePackObject UnpackOne( MemoryStream output )
		{
			return new Unpacker( output.GetBuffer() ).UnpackObject().Value;
		}

		[Test]
		public void TestNil()
		{
			var output = new MemoryStream();
			Packer.Create( output ).PackNull();
			Assert.IsTrue( UnpackOne( output ).IsNil );
		}

		[Test]
		public void TestBoolean()
		{
			TestBoolean( false );
			TestBoolean( true );
		}

		private static void TestBoolean( bool val )
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack( val );
			MessagePackObject obj = UnpackOne( output );
			Assert.AreEqual( val, obj.AsBoolean() );
			Assert.AreEqual( val, ( bool )obj );
			Assert.IsTrue( obj.IsTypeOf<bool>().GetValueOrDefault() );
		}

		[Test]
		public void TestStringShort()
		{
			TestStringStrict( "" );
			TestStringStrict( "a" );
			TestStringStrict( "ab" );
			TestStringStrict( "abc" );
			TestStringStrict( "\u30e1\u30c3\u30bb\u30fc\u30b8\u30d1\u30c3\u30af" );

			GC.Collect();

			var avg = 0.0;
			Random random = new Random();
			var sb = new StringBuilder( 1000 * 1000 * 200 );
			var sw = Stopwatch.StartNew();
			// small size string
			for ( int i = 0; i < 100; i++ )
			{
				sb.Length = 0;
				int len = ( int )random.Next() % 31 + 1;
				for ( int j = 0; j < len; j++ )
				{
					sb.Append( 'a' + ( ( int )random.Next() ) & 26 );
				}
				avg = ( avg + sb.Length ) / 2.0;
				TestString( sb.ToString() );
			}
			sw.Stop();
			Console.WriteLine( "Small String ({1:#,###.0}): {0:0.###} msec/object", sw.ElapsedMilliseconds / 100.0, avg );
		}

		[Test]
		[Explicit]
		public void TestStringMedium()
		{
			var sw = Stopwatch.StartNew();
			Random random = new Random();
			var sb = new StringBuilder( 1000 * 1000 * 200 );
			var avg = 0.0;
			// medium size string
			for ( int i = 0; i < 100; i++ )
			{
				sb.Length = 0;
				int len = ( int )random.Next() % 100 + ( 1 << 15 );
				for ( int j = 0; j < len; j++ )
				{
					sb.Append( 'a' + ( ( int )random.Next() ) & 26 );
				}
				avg = ( avg + sb.Length ) / 2.0;
				TestString( sb.ToString() );
			}
			sw.Stop();
			Console.WriteLine( "Medium String ({1:#,###.0}): {0:0.###} msec/object", sw.ElapsedMilliseconds / 100.0, avg );
		}

		[Test]
		[Explicit]
		public void TestStringLarge()
		{
			var sw = Stopwatch.StartNew();
			var avg = 0.0;
			Random random = new Random();
			var sb = new StringBuilder( 1000 * 1000 * 200 );

			// large size string
			for ( int i = 0; i < 10; i++ )
			{
				sb.Length = 0;
				int len = ( int )random.Next() % 100 + ( 1 << 24 );
				for ( int j = 0; j < len; j++ )
				{
					sb.Append( 'a' + ( ( int )random.Next() ) & 26 );
				}
				avg = ( avg + sb.Length ) / 2.0;
				TestString( sb.ToString() );
			}
			sw.Stop();
			Console.WriteLine( "Large String ({1:#,###.0}): {0:0.###} msec/object", sw.ElapsedMilliseconds / 10.0, avg );
			sw.Reset();

			GC.Collect();

			sw.Start();

			avg = 0.0;

			// large size string
			for ( int i = 0; i < 10; i++ )
			{
				sb.Length = 0;
				int len = ( int )random.Next() % 100 + ( 1 << 24 );
				for ( int j = 0; j < len; j++ )
				{
					sb.Append( 'a' + ( ( int )random.Next() ) & 26 );
				}
				avg = ( avg + sb.Length ) / 2.0;
				TestString( sb.ToString(), Encoding.Unicode );
			}
			sw.Stop();
			Console.WriteLine( "Large String (UTF-16LE) ({1:#,###.0}): {0:0.###} msec/object", sw.ElapsedMilliseconds / 10.0, avg );
			sw.Reset();

			GC.Collect();

			sw.Start();

			// Non-ASCII
			avg = 0.0;
			// medium size string
			for ( int i = 0; i < 100; i++ )
			{
				sb.Length = 0;
				int len = ( int )random.Next() % 100 + ( 1 << 8 );
				for ( int j = 0; j < len; j++ )
				{
					sb.Append( Encoding.UTF32.GetChars( BitConverter.GetBytes( random.Next( 0x10ffff ) ) ) );
				}
				avg = ( avg + sb.Length ) / 2.0;
				TestString( sb.ToString() );
			}
			sw.Stop();
			Console.WriteLine( "Medium String (Non-ASCII) ({1:#.0}): {0:0.###} msec/object", sw.ElapsedMilliseconds / 100.0, avg );
		}

		private static void TestStringStrict( String val )
		{
			var output = new MemoryStream();
			Packer.Create( output ).PackString( val );
			MessagePackObject obj = UnpackOne( output );
			Assert.AreEqual( val, obj.AsString() );
			Assert.AreEqual( val, ( string )obj );
			Assert.IsTrue( obj.IsTypeOf<string>().GetValueOrDefault() );
		}

		private static void TestString( String val )
		{
			var output = new MemoryStream();
			Packer.Create( output ).PackString( val );
			MessagePackObject obj = UnpackOne( output );
			Assert.AreEqual( val, obj.AsString() );
			Assert.IsTrue( obj.IsTypeOf<string>().GetValueOrDefault() );
		}

		private static void TestString( String val, Encoding encoding )
		{
			var output = new MemoryStream();
			Packer.Create( output ).PackString( val, encoding );
			MessagePackObject obj = UnpackOne( output );
			Assert.AreEqual( val, obj.AsString( encoding ) );
			Assert.IsTrue( obj.IsTypeOf<string>().GetValueOrDefault() );
		}

		[Test]
		[Timeout( 5000 )]
		public void TestArray()
		{
			var emptyList = new List<int>();
			{
				var output = new MemoryStream();
				Packer.Create( output ).Pack( emptyList );
				MessagePackObject obj = UnpackOne( output );
				Assert.IsFalse( obj.AsList().Any() );
			}

			{
				var output = new MemoryStream();
				Packer.Create( output ).Pack( new[] { 1 } );
				MessagePackObject obj = UnpackOne( output );
				var asList = obj.AsList();
				Assert.AreEqual( 1, asList.Count );
				Assert.AreEqual( 1, asList[ 0 ].AsInt32() );
			}

			{
				var output = new MemoryStream();
				Packer.Create( output ).Pack( new[] { 1, 2 } );
				MessagePackObject obj = UnpackOne( output );
				var asList = obj.AsList();
				Assert.AreEqual( 2, asList.Count );
				Assert.AreEqual( 1, asList[ 0 ].AsInt32() );
				Assert.AreEqual( 2, asList[ 1 ].AsInt32() );
			}

			var random = new Random();

			for ( int i = 0; i < 100; i++ )
			{
				var l = new List<int>();
				int len = ( int )random.Next( 100 );
				for ( int j = 0; j < len; j++ )
				{
					l.Add( j );
				}

				var output = new MemoryStream();
				Packer.Create( output ).Pack( l );
				MessagePackObject obj = UnpackOne( output );
				var list = obj.AsList();
				l.SequenceEqual( list.Select( item => item.AsInt32() ) );
			}

			for ( int i = 0; i < 100; i++ )
			{
				var l = new List<string>();
				int len = ( int )random.Next( 100 );
				for ( int j = 0; j < len; j++ )
				{
					l.Add( j.ToString() );
				}
				var output = new MemoryStream();
				Packer.Create( output ).Pack( l );
				MessagePackObject obj = UnpackOne( output );
				var list = obj.AsList();
				Assert.IsTrue( list.All( item => item.IsTypeOf<string>().GetValueOrDefault() ) );
				l.SequenceEqual( list.Select( item => item.ToString() ) );
			}
		}

		[Test]
		public void TestNestedArray()
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack( new[] { new int[ 0 ], new[] { 0 }, new[] { 0, 1 } } );
			MessagePackObject obj = UnpackOne( output );
			var outer = obj.AsList();
			Assert.AreEqual( 3, outer.Count );
			Assert.AreEqual( 0, outer[ 0 ].AsList().Count );
			Assert.AreEqual( 1, outer[ 1 ].AsList().Count );
			Assert.AreEqual( 0, outer[ 1 ].AsList()[ 0 ].AsInt32() ); // FIXME: remove AsInt32()
			Assert.AreEqual( 2, outer[ 2 ].AsList().Count );
			Assert.AreEqual( 0, outer[ 2 ].AsList()[ 0 ].AsInt32() ); // FIXME: remove AsInt32()
			Assert.AreEqual( 1, outer[ 2 ].AsList()[ 1 ].AsInt32() );
		}

		[Test]
		public void TestNestedMap()
		{
			var output = new MemoryStream();
			Packer.Create( output ).Pack(
				new Dictionary<string, Dictionary<int, bool>>()
				{
					{ "0", new Dictionary<int,bool>() },
					{ "1", new Dictionary<int,bool>(){ { 0, false } } },
					{ "2", new Dictionary<int,bool>(){ { 0, false }, { 1, true } } },
				}
			);
			MessagePackObject obj = UnpackOne( output );
			var outer = obj.AsDictionary();
			Assert.AreEqual( 3, outer.Count );
			Assert.AreEqual( 0, outer[ "0" ].AsDictionary().Count );
			Assert.AreEqual( 1, outer[ "1" ].AsDictionary().Count );
			Assert.AreEqual( false, outer[ "1" ].AsDictionary()[ 0 ].AsBoolean() ); // FIXME: remove AsBoolean()
			Assert.AreEqual( 2, outer[ "2" ].AsDictionary().Count );
			Assert.AreEqual( false, outer[ "2" ].AsDictionary()[ 0 ].AsBoolean() ); // FIXME: remove AsBoolean()
			Assert.AreEqual( true, outer[ "2" ].AsDictionary()[ 1 ].AsBoolean() ); // FIXME: remove AsBoolean()
		}

		[Test]
		[Timeout( 5000 )]
		public void TestHeteroArray()
		{
			var heteroList = new List<MessagePackObject>()
				{
					true,
					false,
					MessagePackObject.Nil,
					0,
					"",
					123456,
					-123456,
					new String( 'a', 40000 ),
					new String( 'a', 80000 ),
					new MessagePackObject(
						new Dictionary<MessagePackObject,MessagePackObject>()
						{
							//{ new MessagePackObject( "1" ), new MessagePackObject( "foo" ) },
							//{ new MessagePackObject( 2 ), MessagePackObject.Nil },
							//{ new MessagePackObject( 3333333 ), new MessagePackObject( -1 ) }
							{ "1", "foo" },
							{ 2, MessagePackObject.Nil },
							{ 3333333, -1 }
						}
					),
					new MessagePackObject( new MessagePackObject[]{ 1, 2, 3 } )
				};

			var output = new MemoryStream();
			Packer.Create( output ).Pack( heteroList );
			MessagePackObject obj = UnpackOne( output );
			bool isSuccess = false;
			try
			{
				var list = obj.AsList();
				Assert.AreEqual( heteroList[ 0 ], list[ 0 ] );
				Assert.AreEqual( heteroList[ 1 ], list[ 1 ] );
				Assert.AreEqual( heteroList[ 2 ], list[ 2 ] );
				Assert.AreEqual( heteroList[ 3 ], list[ 3 ] );
				Assert.AreEqual( heteroList[ 4 ], list[ 4 ] );
				Assert.AreEqual( heteroList[ 5 ], list[ 5 ] );
				Assert.AreEqual( heteroList[ 6 ], list[ 6 ] );
				Assert.AreEqual( heteroList[ 7 ], list[ 7 ] );
				Assert.AreEqual( heteroList[ 8 ], list[ 8 ] );
				// MsgPack supports string type as utf-8 encoded bytes...
				// TODO: usable wrapper dictionary.
				// FIXME: 1渡したときに、常にコンパクト側に行くようにする。
				Assert.AreEqual(
					//heteroList[ 9 ].AsDictionary()[ "1" ].AsString(),
					//list[ 9 ].AsDictionary()[ _utf8NoBom.GetBytes( "1" ) ].AsString()
					heteroList[ 9 ].AsDictionary()[ "1" ],
					list[ 9 ].AsDictionary()[ "1" ]
				);
				Assert.IsTrue( list[ 9 ].AsDictionary()[ 2 ].IsNil );
				Assert.AreEqual(
					heteroList[ 9 ].AsDictionary()[ 3333333 ],
					list[ 9 ].AsDictionary()[ 3333333 ]
				);
				Assert.AreEqual( heteroList[ 10 ].AsList()[ 0 ], list[ 10 ].AsList()[ 0 ] );
				Assert.AreEqual( heteroList[ 10 ].AsList()[ 1 ], list[ 10 ].AsList()[ 1 ] );
				Assert.AreEqual( heteroList[ 10 ].AsList()[ 2 ], list[ 10 ].AsList()[ 2 ] );
				isSuccess = true;
			}
			finally
			{
				if ( !isSuccess )
				{
					Console.WriteLine( Dump( obj ) );
				}
			}
		}

		private static string Dump( MessagePackObject obj )
		{
			var buffer = new StringBuilder();
			Dump( obj, buffer, 0 );
			return buffer.ToString();
		}

		private static void Dump( MessagePackObject obj, StringBuilder buffer, int indent )
		{
			if ( obj.IsNil )
			{
				buffer.Append( ' ', indent * 2 ).Append( "(null)" ).AppendLine();
			}
			else if ( obj.IsTypeOf<IList<MessagePackObject>>().GetValueOrDefault() )
			{
				buffer.Append( ' ', indent * 2 ).AppendLine( "(" );

				foreach ( var child in obj.AsList() )
				{
					Dump( child, buffer, indent + 1 );
				}

				buffer.Append( ' ', indent * 2 ).AppendLine( ")" );
			}
			else if ( obj.IsTypeOf<IDictionary<MessagePackObject, MessagePackObject>>().GetValueOrDefault() )
			{
				buffer.Append( ' ', indent * 2 ).AppendLine( "{" );

				foreach ( var child in obj.AsDictionary() )
				{
					Dump( child.Key, buffer, indent + 1 );
					buffer.Append( ' ', ( indent + 1 ) * 2 ).AppendLine( "= " );
					Dump( child.Value, buffer, indent + 2 );
				}

				buffer.Append( ' ', indent * 2 ).AppendLine( "}" );
			}
			else
			{
				buffer.Append( ' ', indent * 2 ).Append( obj ).Append( " : " ).Append( obj.GetUnderlyingType() ).AppendLine();
			}
		}

		[Test]
		[Timeout( 5000 )]
		public void TestDictionary()
		{
			var emptyDictionary = new Dictionary<int, int>();
			{
				var output = new MemoryStream();
				Packer.Create( output ).Pack( emptyDictionary );
				MessagePackObject obj = UnpackOne( output );
				Assert.IsFalse( obj.AsDictionary().Any() );
			}

			var random = new Random();

			for ( int i = 0; i < 100; i++ )
			{
				var d = new Dictionary<int, int>();
				int len = ( int )random.Next( 100 );
				for ( int j = 0; j < len; j++ )
				{
					d[ j ] = j;
				}
				var output = new MemoryStream();
				Packer.Create( output ).Pack( d );
				MessagePackObject obj = UnpackOne( output );
				var dictionary = obj.AsDictionary();
				CollectionAssert.AreEquivalent(
					d,
					dictionary.Select( item => new KeyValuePair<int, int>( item.Key.AsInt32(), item.Value.AsInt32() ) ),
					String.Join( ", ", dictionary.Select( item => "{ " + item.Key + " : " + item.Value + " }" ) )
				);
			}

			for ( int i = 0; i < 100; i++ )
			{
				var d = new Dictionary<string, int>();
				int len = ( int )random.Next( 100 );
				for ( int j = 0; j < len; j++ )
				{
					d[ j.ToString() ] = j;
				}
				var output = new MemoryStream();
				Packer.Create( output ).Pack( d );
				MessagePackObject obj = UnpackOne( output );
				var dictionary = obj.AsDictionary();
				CollectionAssert.AreEquivalent( d, dictionary.Select( item => new KeyValuePair<string, int>( item.Key.AsString(), item.Value.AsInt32() ) ).ToDictionary( item => item.Key, item => item.Value ) );
			}
		}
	}
}
