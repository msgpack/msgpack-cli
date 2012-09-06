
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
#endif

namespace MsgPack
{
	[TestFixture]
	public 	partial class UnpackingTest_Combinations_Map
	{
		[Test]
		public void TestUnpackDictionaryCount_ByteArray_FixMap0Value_AsFixMap0_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0x80 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.AreEqual( 0, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_FixMap0Value_AsFixMap0_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x80 } ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 1, buffer.Position );
				Assert.AreEqual( 0, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_FixMap0Value_AsFixMap0_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0x80 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.AreEqual( 0, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_FixMap0Value_AsFixMap0_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x80 } ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 1, buffer.Position );
				Assert.AreEqual( 0, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_FixMap0Value_AsMap16_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xDE, 0x00, 0x00 } );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.AreEqual( 0, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_FixMap0Value_AsMap16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDE, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.AreEqual( 0, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_FixMap0Value_AsMap16_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0xDE, 0x00, 0x00 } );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.AreEqual( 0, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_FixMap0Value_AsMap16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDE, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.AreEqual( 0, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_FixMap0Value_AsMap32_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x00 } );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.AreEqual( 0, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_FixMap0Value_AsMap32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.AreEqual( 0, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_FixMap0Value_AsMap32_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x00 } );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.AreEqual( 0, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_FixMap0Value_AsMap32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x00 } ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.AreEqual( 0, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_FixMap1Value_AsFixMap1_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0x81 }.Concat( CreateDictionaryBodyBinary( 1 ) ).ToArray() );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.AreEqual( 0x1, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_FixMap1Value_AsFixMap1_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x81 }.Concat( CreateDictionaryBodyBinary( 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 1, buffer.Position );
				Assert.AreEqual( 0x1, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_FixMap1Value_AsFixMap1_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0x81 }.Concat( CreateDictionaryBodyBinary( 1 ) ).ToArray() );
			Assert.AreEqual( 7, result.ReadCount );
			Assert.AreEqual( 0x1, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_FixMap1Value_AsFixMap1_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x81 }.Concat( CreateDictionaryBodyBinary( 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 7, buffer.Position );
				Assert.AreEqual( 0x1, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_FixMap1Value_AsMap16_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xDE, 0x00, 0x01 }.Concat( CreateDictionaryBodyBinary( 1 ) ).ToArray() );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.AreEqual( 0x1, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_FixMap1Value_AsMap16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDE, 0x00, 0x01 }.Concat( CreateDictionaryBodyBinary( 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.AreEqual( 0x1, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_FixMap1Value_AsMap16_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0xDE, 0x00, 0x01 }.Concat( CreateDictionaryBodyBinary( 1 ) ).ToArray() );
			Assert.AreEqual( 9, result.ReadCount );
			Assert.AreEqual( 0x1, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_FixMap1Value_AsMap16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDE, 0x00, 0x01 }.Concat( CreateDictionaryBodyBinary( 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 9, buffer.Position );
				Assert.AreEqual( 0x1, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_FixMap1Value_AsMap32_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x01 }.Concat( CreateDictionaryBodyBinary( 1 ) ).ToArray() );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.AreEqual( 0x1, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_FixMap1Value_AsMap32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x01 }.Concat( CreateDictionaryBodyBinary( 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.AreEqual( 0x1, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_FixMap1Value_AsMap32_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x01 }.Concat( CreateDictionaryBodyBinary( 1 ) ).ToArray() );
			Assert.AreEqual( 11, result.ReadCount );
			Assert.AreEqual( 0x1, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_FixMap1Value_AsMap32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x01 }.Concat( CreateDictionaryBodyBinary( 1 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 11, buffer.Position );
				Assert.AreEqual( 0x1, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_FixMapMaxValue_AsFixMap15_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0x8F }.Concat( CreateDictionaryBodyBinary( 0xF ) ).ToArray() );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.AreEqual( 0xF, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_FixMapMaxValue_AsFixMap15_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x8F }.Concat( CreateDictionaryBodyBinary( 0xF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 1, buffer.Position );
				Assert.AreEqual( 0xF, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_FixMapMaxValue_AsFixMap15_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0x8F }.Concat( CreateDictionaryBodyBinary( 0xF ) ).ToArray() );
			Assert.AreEqual( 91, result.ReadCount );
			Assert.AreEqual( 0xF, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_FixMapMaxValue_AsFixMap15_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x8F }.Concat( CreateDictionaryBodyBinary( 0xF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 91, buffer.Position );
				Assert.AreEqual( 0xF, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_FixMapMaxValue_AsMap16_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xDE, 0x00, 0x0F }.Concat( CreateDictionaryBodyBinary( 0xF ) ).ToArray() );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.AreEqual( 0xF, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_FixMapMaxValue_AsMap16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDE, 0x00, 0x0F }.Concat( CreateDictionaryBodyBinary( 0xF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.AreEqual( 0xF, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_FixMapMaxValue_AsMap16_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0xDE, 0x00, 0x0F }.Concat( CreateDictionaryBodyBinary( 0xF ) ).ToArray() );
			Assert.AreEqual( 93, result.ReadCount );
			Assert.AreEqual( 0xF, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_FixMapMaxValue_AsMap16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDE, 0x00, 0x0F }.Concat( CreateDictionaryBodyBinary( 0xF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 93, buffer.Position );
				Assert.AreEqual( 0xF, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_FixMapMaxValue_AsMap32_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x0F }.Concat( CreateDictionaryBodyBinary( 0xF ) ).ToArray() );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.AreEqual( 0xF, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_FixMapMaxValue_AsMap32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x0F }.Concat( CreateDictionaryBodyBinary( 0xF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.AreEqual( 0xF, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_FixMapMaxValue_AsMap32_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x0F }.Concat( CreateDictionaryBodyBinary( 0xF ) ).ToArray() );
			Assert.AreEqual( 95, result.ReadCount );
			Assert.AreEqual( 0xF, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_FixMapMaxValue_AsMap32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x0F }.Concat( CreateDictionaryBodyBinary( 0xF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 95, buffer.Position );
				Assert.AreEqual( 0xF, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Map16MinValue_AsMap16_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xDE, 0x00, 0x10 }.Concat( CreateDictionaryBodyBinary( 0x10 ) ).ToArray() );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.AreEqual( 0x10, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_Map16MinValue_AsMap16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDE, 0x00, 0x10 }.Concat( CreateDictionaryBodyBinary( 0x10 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.AreEqual( 0x10, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_Map16MinValue_AsMap16_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0xDE, 0x00, 0x10 }.Concat( CreateDictionaryBodyBinary( 0x10 ) ).ToArray() );
			Assert.AreEqual( 99, result.ReadCount );
			Assert.AreEqual( 0x10, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_Map16MinValue_AsMap16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDE, 0x00, 0x10 }.Concat( CreateDictionaryBodyBinary( 0x10 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 99, buffer.Position );
				Assert.AreEqual( 0x10, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Map16MinValue_AsMap32_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x10 }.Concat( CreateDictionaryBodyBinary( 0x10 ) ).ToArray() );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.AreEqual( 0x10, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_Map16MinValue_AsMap32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x10 }.Concat( CreateDictionaryBodyBinary( 0x10 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.AreEqual( 0x10, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_Map16MinValue_AsMap32_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x10 }.Concat( CreateDictionaryBodyBinary( 0x10 ) ).ToArray() );
			Assert.AreEqual( 101, result.ReadCount );
			Assert.AreEqual( 0x10, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_Map16MinValue_AsMap32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDF, 0x00, 0x00, 0x00, 0x10 }.Concat( CreateDictionaryBodyBinary( 0x10 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 101, buffer.Position );
				Assert.AreEqual( 0x10, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Map16MaxValue_AsMap16_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xDE, 0xFF, 0xFF }.Concat( CreateDictionaryBodyBinary( 0xFFFF ) ).ToArray() );
			Assert.AreEqual( 3, result.ReadCount );
			Assert.AreEqual( 0xFFFF, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_Map16MaxValue_AsMap16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDE, 0xFF, 0xFF }.Concat( CreateDictionaryBodyBinary( 0xFFFF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 3, buffer.Position );
				Assert.AreEqual( 0xFFFF, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_Map16MaxValue_AsMap16_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0xDE, 0xFF, 0xFF }.Concat( CreateDictionaryBodyBinary( 0xFFFF ) ).ToArray() );
			Assert.AreEqual( 393213, result.ReadCount );
			Assert.AreEqual( 0xFFFF, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_Map16MaxValue_AsMap16_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDE, 0xFF, 0xFF }.Concat( CreateDictionaryBodyBinary( 0xFFFF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 393213, buffer.Position );
				Assert.AreEqual( 0xFFFF, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Map16MaxValue_AsMap32_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xDF, 0x00, 0x00, 0xFF, 0xFF }.Concat( CreateDictionaryBodyBinary( 0xFFFF ) ).ToArray() );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.AreEqual( 0xFFFF, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_Map16MaxValue_AsMap32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDF, 0x00, 0x00, 0xFF, 0xFF }.Concat( CreateDictionaryBodyBinary( 0xFFFF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.AreEqual( 0xFFFF, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_Map16MaxValue_AsMap32_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0xDF, 0x00, 0x00, 0xFF, 0xFF }.Concat( CreateDictionaryBodyBinary( 0xFFFF ) ).ToArray() );
			Assert.AreEqual( 393215, result.ReadCount );
			Assert.AreEqual( 0xFFFF, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_Map16MaxValue_AsMap32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDF, 0x00, 0x00, 0xFF, 0xFF }.Concat( CreateDictionaryBodyBinary( 0xFFFF ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 393215, buffer.Position );
				Assert.AreEqual( 0xFFFF, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Map32MinValue_AsMap32_AsIs()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xDF, 0x00, 0x01, 0x00, 0x00 }.Concat( CreateDictionaryBodyBinary( 0x10000 ) ).ToArray() );
			Assert.AreEqual( 5, result.ReadCount );
			Assert.AreEqual( 0x10000, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_Map32MinValue_AsMap32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDF, 0x00, 0x01, 0x00, 0x00 }.Concat( CreateDictionaryBodyBinary( 0x10000 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionaryCount( buffer );
				Assert.AreEqual( 5, buffer.Position );
				Assert.AreEqual( 0x10000, result );
			}
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_Map32MinValue_AsMap32_AsIs()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0xDF, 0x00, 0x01, 0x00, 0x00 }.Concat( CreateDictionaryBodyBinary( 0x10000 ) ).ToArray() );
			Assert.AreEqual( 393221, result.ReadCount );
			Assert.AreEqual( 0x10000, result.Value.Count );
			for ( int i = 0; i < result.Value.Count; i++ )
			{
				MessagePackObject value;
				Assert.IsTrue( result.Value.TryGetValue( i + 1, out value ) );
				Assert.IsTrue( value.Equals( 0x57 ) );
			}
		}

		[Test]
		public void TestUnpackDictionary_Stream_Map32MinValue_AsMap32_AsIs()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0xDF, 0x00, 0x01, 0x00, 0x00 }.Concat( CreateDictionaryBodyBinary( 0x10000 ) ).ToArray() ) )
			{
				var result = Unpacking.UnpackDictionary( buffer );
				Assert.AreEqual( 393221, buffer.Position );
				Assert.AreEqual( 0x10000, result.Count );
				for ( int i = 0; i < result.Count; i++ )
				{
					MessagePackObject value;
					Assert.IsTrue( result.TryGetValue( i + 1, out value ) );
					Assert.IsTrue( value.Equals( 0x57 ) );
				}
			}
		}


		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Empty()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackDictionaryCount( new byte[ 0 ] ) );
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackDictionaryCount( default( byte[] ) ) );
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Offset_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackDictionaryCount( default( byte[] ), 0 ) );
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Offset_OffsetIsNegative()
		{
			Assert.Throws<ArgumentOutOfRangeException>( () => Unpacking.UnpackDictionaryCount( new byte[]{ 0x1 }, -1 ) );
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Offset_OffsetIsTooBig()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackDictionaryCount( new byte[]{ 0x1 }, 1 ) );
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Offset_Empty()
		{
			Assert.Throws<ArgumentException>( () => Unpacking.UnpackDictionaryCount( new byte[ 0 ], 0 ) );
		}

		[Test]
		public void TestUnpackDictionaryCount_Stream_Null()
		{
			Assert.Throws<ArgumentNullException>( () => Unpacking.UnpackDictionaryCount( default( Stream ) ) );
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Offset_OffsetIsValid_OffsetIsRespected()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xFF, 0x80, 0xFF }, 1 );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.AreEqual( 0, result.Value );
		}

		[Test]
		public void TestUnpackDictionaryCount_ByteArray_Null_Nil()
		{
			var result = Unpacking.UnpackDictionaryCount( new byte[] { 0xC0 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.IsNull( result.Value );
		}
	
		[Test]
		public void TestUnpackDictionaryCount_ByteArray_NotMap()
		{
			Assert.Throws<MessageTypeException>( () => Unpacking.UnpackDictionaryCount( new byte[] { 0x1 } ) );
		}

		[Test]
		public void TestUnpackDictionary_ByteArray_Null_Nil()
		{
			var result = Unpacking.UnpackDictionary( new byte[] { 0xC0 } );
			Assert.AreEqual( 1, result.ReadCount );
			Assert.IsNull( result.Value );
		}
	
		[Test]
		public void TestUnpackDictionary_ByteArray_NotMap()
		{
			Assert.Throws<MessageTypeException>( () => Unpacking.UnpackDictionary( new byte[] { 0x1 } ) );
		}

		private static IEnumerable<byte> CreateDictionaryBodyBinary( int count )
		{
			return 
				Enumerable.Range( 1, count )
				.SelectMany( i => 
					new byte[]{ 0xD2 } // Int32 header for key
					.Concat( BitConverter.GetBytes( i ).Reverse() ) // Key : i (Big-Endean)
					.Concat( new byte[] { 0x57 } ) // Value = 0x57
				);
		}

	}
}
