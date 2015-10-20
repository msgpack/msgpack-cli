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

// #define HEAVY_TEST

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ExplicitAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.IgnoreAttribute;
using IgnoreAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.IgnoreAttribute;
#endif

namespace MsgPack
{
	[TestFixture]
	[Timeout( 1000 )]
	public partial class PackUnpackTest
	{
		private static TextWriter Console
		{
			get
			{
#if !NETFX_CORE && !SILVERLIGHT
				return System.Console.Out;
#else
				return TextWriter.Null;
#endif
			}
		}

		private static bool _traceUnpackingBytes = false;

		private static MessagePackObject UnpackOne( MemoryStream output )
		{
			output.Seek( 0L, SeekOrigin.Begin );
			if ( _traceUnpackingBytes )
			{
				Console.WriteLine( "Unpaking:" );
				foreach ( var b in output.ToArray().Take( 100 ) )
				{
					Console.Write( b.ToString( "x2" ) );
				}
				Console.WriteLine();
			}

			return Unpacking.UnpackObject( output );
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
			var sb = new StringBuilder( 1000 * 1000 * 20 );
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
		[Timeout( 3000000 )]
#if !HEAVY_TEST
		[Ignore]
#endif
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
			Console.WriteLine( "Medium String ({1:#,###.0} chars): {0:0.###} msec/object", sw.ElapsedMilliseconds / 100.0, avg );
		}

#if !NETFX_35
		[Test]
		[Timeout( 3000000 )]
#if !HEAVY_TEST
		[Ignore]
#endif
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
			Console.WriteLine( "Large String ({1:#,###.0} chars): {0:0.###} msec/object", sw.ElapsedMilliseconds / 10.0, avg );
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
					var cp = random.Next( 0x10ffff );
					if ( 0xd800 <= cp && cp <= 0xdfff )
					{
						cp /= 2;
					}

					sb.Append( Char.ConvertFromUtf32( cp ) );
				}
				avg = ( avg + sb.Length ) / 2.0;
				TestString( sb.ToString() );
			}
			sw.Stop();
			Console.WriteLine( "Medium String (Non-ASCII) ({1:#.0}): {0:0.###} msec/object", sw.ElapsedMilliseconds / 100.0, avg );
		}
#endif

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
		[Timeout( 10000 )]
		public void TestArray()
		{
			var sw = new Stopwatch();
			foreach (
				var count in
				new[]
				{
					0, // empty
					1, // only one
					2, // minimum multiple
					0xf, // max fix array size
					0x10, // min array16 size
					0xffff, // max array16 size
					0x10000, // min array32 size
				}
			)
			{
				sw.Restart();
				var output = new MemoryStream();
				Packer.Create( output ).PackCollection( Enumerable.Range( 0, count ).ToArray() );
				Assert.That(
					Enumerable.Range( 0, count ).ToArray(),
					Is.EqualTo( UnpackOne( output ).AsEnumerable().Select( item => item.AsInt32() ).ToArray() )
				);
				sw.Stop();
			}

			Console.WriteLine( "Array: {0:0.###} msec/item", sw.Elapsed.TotalMilliseconds / 0x10000 );
		}

		[Test]
		[Timeout( 10000 )]
		public void TestArray_Splitted()
		{
			foreach (
				var count in
					new[]
					{
						0, // empty
						1, // only one
						2, // minimum multiple
						0xf, // max fix array size
						0x10, // min array16 size
						0xffff, // max array16 size
						0x10000, // min array32 size
					}
			)
			{
				using ( var output = new MemoryStream() )
				{
					Packer.Create( output ).PackCollection( Enumerable.Range( 0, count ).ToArray() );
					output.Position = 0;
					using ( var splitted = new SplittingStream( output ) )
					{
						Assert.That(
							Enumerable.Range( 0, count ).ToArray(),
							Is.EqualTo( Unpacking.UnpackObject( splitted ).AsEnumerable().Select( item => item.AsInt32() ).ToArray() )
						);
					}
				}
			}
		}

		[Test]
		public void TestNestedArray()
		{
			var output = new MemoryStream();
			Packer.Create( output ).PackCollection( new[] { new int[ 0 ], new[] { 0 }, new[] { 0, 1 } } );
			MessagePackObject obj = UnpackOne( output );
			var outer = obj.AsList();
			Assert.AreEqual( 3, outer.Count );
			Assert.AreEqual( 0, outer[ 0 ].AsList().Count );
			Assert.AreEqual( 1, outer[ 1 ].AsList().Count );
			Assert.That( outer[ 1 ].AsList()[ 0 ].AsInt32(), Is.EqualTo( 0 ).With.TypeOf<int>() );
			Assert.AreEqual( 2, outer[ 2 ].AsList().Count );
			Assert.That( outer[ 2 ].AsList()[ 0 ].AsInt32(), Is.EqualTo( 0 ).With.TypeOf<int>() );
			Assert.AreEqual( 1, outer[ 2 ].AsList()[ 1 ].AsInt32() );
		}

		[Test]
		public void TestNestedMap()
		{
			var output = new MemoryStream();
			Packer.Create( output ).PackDictionary(
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
			Assert.That( outer[ "1" ].AsDictionary()[ 0 ].AsBoolean(), Is.False.With.TypeOf<bool>() );
			Assert.AreEqual( 2, outer[ "2" ].AsDictionary().Count );
			Assert.That( outer[ "2" ].AsDictionary()[ 0 ].AsBoolean(), Is.False.With.TypeOf<bool>() );
			Assert.That( outer[ "2" ].AsDictionary()[ 1 ].AsBoolean(), Is.True.With.TypeOf<bool>() );
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
						new MessagePackObjectDictionary()
						{
							{ "1", "foo" },
							{ 2, MessagePackObject.Nil },
							{ 3333333, -1 }
						}
					),
					new MessagePackObject( new MessagePackObject[]{ 1, 2, 3 } )
				};

			var output = new MemoryStream();
			Packer.Create( output ).PackCollection( heteroList );
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
				Assert.AreEqual(
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
				buffer.Append( ' ', indent * 2 ).Append( obj ).Append( " : " ).Append( obj.UnderlyingType ).AppendLine();
			}
		}

		[Test]
		[Timeout( 60000 )]
		public void TestDictionary()
		{
			var sw = new Stopwatch();
			foreach (
				var count in
				new[]
				{
					0, // empty
					1, // only one
					2, // minimum multiple
					0xf, // max fix map size
					0x10, // min map16 size
					0xffff, // max map16 size
					0x10000, // min map32 size
				}
			)
			{
				sw.Restart();
				var output = new MemoryStream();
				Packer.Create( output ).PackDictionary( Enumerable.Range( 0, count ).ToDictionary( item => item.ToString(), item => item ) );
				Assert.That(
					Enumerable.Range( 0, count ).ToDictionary( item => item.ToString(), item => item ),
					Is.EqualTo( UnpackOne( output ).AsDictionary().ToDictionary( kv => kv.Key.AsString(), kv => kv.Value.AsInt32() ) )
				);
				sw.Stop();
			}

			Console.WriteLine( "Map: {0:0.###} msec/item", sw.Elapsed.TotalMilliseconds / 0x10000 );
		}

		[Test]
		[Timeout( 60000 )]
		public void TestDictionary_Splitted()
		{
			foreach (
				var count in
					new[]
					{
						0, // empty
						1, // only one
						2, // minimum multiple
						0xf, // max fix map size
						0x10, // min map16 size
						0xffff, // max map16 size
						0x10000, // min map32 size
					}
			)
			{
				using ( var output = new MemoryStream() )
				{
					Packer.Create( output ).PackDictionary( Enumerable.Range( 0, count ).ToDictionary( item => item.ToString(), item => item ) );
					output.Position = 0;
					using ( var splitted = new SplittingStream( output ) )
					{
						Assert.That(
							Enumerable.Range( 0, count ).ToDictionary( item => item.ToString(), item => item ),
							Is.EqualTo( Unpacking.UnpackObject( splitted ).AsDictionary().ToDictionary( kv => kv.Key.AsString(), kv => kv.Value.AsInt32() ) )
						);
					}
				}
			}
		}

		[Test]
		[Timeout( 3000 )]
		public void TestBytes()
		{
			var sw = new Stopwatch();
			foreach (
				var count in
				new[]
				{
					0, // empty
					1, // only one
					2, // minimum multiple
					0xFF, // max bin8 size
					0x100, // min bin16 size
					0xffff, // max bin16 size
					0x10000, // min bin32 size
				}
			)
			{
				sw.Restart();
				var output = new MemoryStream();
				Packer.Create( output ).PackBinary( Enumerable.Range( 0, count ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray() );
				Assert.That(
					Enumerable.Range( 0, count ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray(),
					Is.EqualTo( UnpackOne( output ).AsBinary() )
				);
				sw.Stop();
			}

			Console.WriteLine( "Bytes: {0:0.###} msec/byte", sw.Elapsed.TotalMilliseconds / 0x10000 );
		}

		[Test]
		[Timeout( 3000 )]
		public void TestBytes_Splitted()
		{
			foreach (
				var count in
					new[]
					{
						0, // empty
						1, // only one
						2, // minimum multiple
						0xFF, // max bin8 size
						0x100, // min bin16 size
						0xffff, // max bin16 size
						0x10000, // min bin32 size
					}
			)
			{
				using ( var output = new MemoryStream() )
				{
					Packer.Create( output ).PackBinary( Enumerable.Range( 0, count ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray() );
					output.Position = 0;
					using ( var splitted = new SplittingStream( output ) )
					{
						Assert.That(
							Enumerable.Range( 0, count ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray(),
							Is.EqualTo( Unpacking.UnpackObject( splitted ).AsBinary() )
						);
					}
				}
			}
		}

		[Test]
		[Timeout( 3000 )]
		public void TestChars()
		{
			var sw = new Stopwatch();
			foreach (
				var count in
					new[]
					{
						0, // empty
						1, // only one
						2, // minimum multiple
						0x1f, // max fix raw size
						0x20, // min str8 size
						0xFF, // max str8 size
						0x100, // min str16 size
						0xffff, // max str16 size
						0x10000, // min str32 size
					}
			)
			{
				sw.Restart();
				var output = new MemoryStream();
				Packer.Create( output ).PackString( String.Concat( Enumerable.Range( 0, count ).Select( i => ( i % 10 ).ToString() ).ToArray() ) );
				Assert.AreEqual(
					String.Concat( Enumerable.Range( 0, count ).Select( i => ( i % 10 ).ToString() ).ToArray() ),
					UnpackOne( output ).AsString()
				);
				sw.Stop();
			}

			Console.WriteLine( "String: {0:0.###} msec/char", sw.Elapsed.TotalMilliseconds / 0x10000 );
		}

		[Test]
		[Timeout( 3000 )]
		public void TestChars_Splitted()
		{
			foreach (
				var count in
					new[]
					{
						0, // empty
						1, // only one
						2, // minimum multiple
						0x1f, // max fix raw size
						0x20, // min str8 size
						0xFF, // max str8 size
						0x100, // min str16 size
						0xffff, // max str16 size
						0x10000, // min str32 size
					}
			)
			{
				using ( var output = new MemoryStream() )
				{
					Packer.Create( output ).PackString( String.Concat( Enumerable.Range( 0, count ).Select( i => ( i % 10 ).ToString() ).ToArray() ) );
					output.Position = 0;
					using ( var splitted = new SplittingStream( output ) )
					{
						Assert.AreEqual(
							String.Concat( Enumerable.Range( 0, count ).Select( i => ( i % 10 ).ToString() ).ToArray() ),
							Unpacking.UnpackObject( splitted ).AsString()
						);
					}
				}
			}
		}

		[Test]
		[Timeout( 3000 )]
		public void TestExts()
		{
			var sw = new Stopwatch();
			foreach (
				var count in
					new[]
					{
						0, // empty
						1, // fixext1
						2, // fixext2
						4, // fixext4
						8, // fixext8
						16, // fixext16
						17, // min ext8 size
						0xff, // max ext8 size
						0x100, // min ext16 size
						0xffff, // max ext16 size
						0x10000, // min ext32 size
					}
			)
			{
				sw.Restart();
				var output = new MemoryStream();
				var value = new MessagePackExtendedTypeObject(
					1, Enumerable.Range( 0, count ).Select( i => ( byte ) ( i % 0x100 ) ).ToArray() );
				Packer.Create( output, PackerCompatibilityOptions.None ).PackExtendedTypeValue( value );
				Assert.AreEqual(
					value,
					UnpackOne( output ).AsMessagePackExtendedTypeObject()
				);
				sw.Stop();
			}

			Console.WriteLine( "Ext: {0:0.###} msec/byte", sw.Elapsed.TotalMilliseconds / 0x10000 );
		}

		[Test]
		[Timeout( 3000 )]
		public void TestExts_Splitted()
		{
			foreach (
				var count in
					new[]
					{
						0, // empty
						1, // fixext1
						2, // fixext2
						4, // fixext4
						8, // fixext8
						16, // fixext16
						17, // min ext8 size
						0xff, // max ext8 size
						0x100, // min ext16 size
						0xffff, // max ext16 size
						0x10000, // min ext32 size
					}
			)
			{
				using ( var output = new MemoryStream() )
				{
					var value = new MessagePackExtendedTypeObject(
						1, Enumerable.Range( 0, count ).Select( i => ( byte )( i % 0x100 ) ).ToArray() );
					Packer.Create( output, PackerCompatibilityOptions.None ).PackExtendedTypeValue( value );
					output.Position = 0;
					using ( var splitted = new SplittingStream( output ) )
					{
						Assert.AreEqual(
							value,
							Unpacking.UnpackObject( splitted ).AsMessagePackExtendedTypeObject()
						);
					}
				}
			}
		}

		[Test]
		public void TestMultipleObjectInStream()
		{
			using ( var stream = new MemoryStream() )
			{
				var packer = Packer.Create( stream );
				packer.Pack( 1 );
				packer.PackString( "1" );
				stream.Position = 0;
				var item = Unpacking.UnpackObject( stream );
				Assert.That( item, Is.Not.Null );
				Assert.That( item.IsTypeOf<int>().Value );
#if !NETFX_CORE
				Assert.That( item.UnderlyingType.IsPrimitive, Is.True );
#endif
				Assert.That( item.AsInt32(), Is.EqualTo( 1 ) );
				item = Unpacking.UnpackObject( stream );
				Assert.That( item, Is.Not.Null );
				Assert.That( item.UnderlyingType, Is.EqualTo( typeof( String ) ) );
				Assert.That( item.IsTypeOf<string>().Value );
				Assert.That( item.AsString(), Is.EqualTo( "1" ) );
			}
		}
	}
}
