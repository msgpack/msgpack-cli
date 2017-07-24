#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
	// This file was generated from UnpackerTest.Raw.tt T4Template.
	// Do not modify this file. Edit UnpackerTest.Raw.tt instead.

	partial class UnpackerTest
	{

		[Test]
		public void TestRead_FixStr_0_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_FixStr_0_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_FixStr_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestReadString_FixStr_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestRead_FixStr_1_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_FixStr_1_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixStr_1_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixStr_1_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_FixStr_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_FixStr_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_FixStr_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_FixStr_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestRead_FixStr_31_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_FixStr_31_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixStr_31_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixStr_31_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_FixStr_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_FixStr_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_FixStr_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_FixStr_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestRead_Str8_0_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_0_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str8_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str8_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestRead_Str8_1_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_1_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_1_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_1_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str8_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str8_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str8_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str8_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestRead_Str8_31_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_31_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_31_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_31_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str8_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str8_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str8_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str8_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestRead_Str8_32_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_32_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_32_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_32_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str8_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str8_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str8_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str8_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestRead_Str8_255_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_255_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_255_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_255_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str8_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str8_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str8_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str8_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestRead_Str16_0_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_0_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str16_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestRead_Str16_1_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_1_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_1_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_1_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str16_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str16_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str16_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestRead_Str16_31_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_31_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_31_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_31_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str16_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str16_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str16_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestRead_Str16_32_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_32_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_32_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_32_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str16_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str16_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str16_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestRead_Str16_255_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_255_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_255_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_255_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str16_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str16_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str16_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestRead_Str16_256_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_256_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_256_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_256_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str16_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str16_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str16_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestRead_Str16_65535_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_65535_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_65535_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_65535_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str16_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str16_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str16_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestRead_Str32_0_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_0_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str32_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestRead_Str32_1_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_1_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_1_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_1_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str32_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestRead_Str32_31_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_31_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_31_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_31_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str32_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestRead_Str32_32_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_32_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_32_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_32_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str32_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestRead_Str32_255_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_255_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_255_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_255_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str32_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestRead_Str32_256_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_256_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_256_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_256_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str32_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestRead_Str32_65535_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_65535_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_65535_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_65535_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str32_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestRead_Str32_65536_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_65536_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_65536_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_65536_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Str32_65536_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32_65536_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_65536_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Str32_65536_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin8_0_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_0_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin8_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin8_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin8_1_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_1_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_1_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_1_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin8_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin8_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin8_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin8_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin8_31_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_31_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_31_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_31_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin8_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin8_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin8_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin8_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin8_32_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_32_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_32_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_32_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin8_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin8_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin8_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin8_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin8_255_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_255_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_255_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_255_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin8_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin8_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin8_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin8_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin16_0_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_0_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin16_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin16_1_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_1_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_1_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_1_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin16_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin16_31_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_31_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_31_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_31_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin16_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin16_32_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_32_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_32_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_32_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin16_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin16_255_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_255_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_255_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_255_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin16_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin16_256_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_256_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_256_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_256_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin16_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin16_65535_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_65535_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_65535_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_65535_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin16_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin32_0_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_0_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin32_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin32_1_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_1_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_1_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_1_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin32_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin32_31_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_31_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_31_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_31_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin32_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin32_32_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_32_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_32_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_32_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin32_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin32_255_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_255_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_255_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_255_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin32_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin32_256_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_256_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_256_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_256_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin32_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin32_65535_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_65535_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_65535_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_65535_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin32_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestRead_Bin32_65536_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_65536_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_65536_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_65536_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadString_Bin32_65536_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32_65536_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_65536_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadString( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadString( out result ) );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32_65536_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				Assert.IsTrue( unpacker.ReadString( out result ) );
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestRead_FixStr_0_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_FixStr_0_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_FixStr_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_FixStr_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_FixStr_1_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_FixStr_1_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixStr_1_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixStr_1_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_FixStr_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_FixStr_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_FixStr_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_FixStr_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_FixStr_31_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_FixStr_31_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixStr_31_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_FixStr_31_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_FixStr_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_FixStr_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_FixStr_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_FixStr_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str8_0_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_0_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str8_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str8_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str8_1_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_1_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_1_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_1_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str8_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str8_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str8_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str8_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str8_31_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_31_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_31_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_31_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str8_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str8_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str8_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str8_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str8_32_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_32_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_32_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_32_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str8_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str8_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str8_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str8_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str8_255_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_255_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_255_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_255_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str8_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str8_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str8_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str8_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str16_0_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_0_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str16_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str16_1_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_1_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_1_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_1_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str16_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str16_31_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_31_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_31_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_31_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str16_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str16_32_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_32_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_32_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_32_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str16_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str16_255_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_255_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_255_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_255_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str16_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str16_256_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_256_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_256_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_256_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str16_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str16_65535_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_65535_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_65535_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_65535_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str16_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str32_0_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_0_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str32_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str32_1_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_1_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_1_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_1_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str32_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str32_31_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_31_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_31_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_31_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str32_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str32_32_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_32_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_32_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_32_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str32_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str32_255_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_255_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_255_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_255_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str32_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str32_256_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_256_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_256_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_256_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str32_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str32_65535_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_65535_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_65535_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_65535_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str32_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Str32_65536_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_65536_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_65536_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_65536_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Str32_65536_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32_65536_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_65536_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32_65536_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin8_0_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_0_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin8_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin8_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin8_1_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_1_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_1_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_1_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin8_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin8_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin8_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin8_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin8_31_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_31_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_31_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_31_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin8_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin8_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin8_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin8_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin8_32_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_32_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_32_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_32_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin8_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin8_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin8_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin8_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin8_255_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_255_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_255_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_255_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin8_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin8_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin8_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin8_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin16_0_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_0_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin16_1_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_1_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_1_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_1_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin16_31_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_31_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_31_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_31_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin16_32_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_32_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_32_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_32_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin16_255_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_255_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_255_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_255_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin16_256_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_256_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_256_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_256_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin16_65535_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_65535_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_65535_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_65535_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin32_0_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_0_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin32_1_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_1_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_1_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_1_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin32_31_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_31_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_31_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_31_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin32_32_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_32_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_32_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_32_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin32_255_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_255_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_255_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_255_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin32_256_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_256_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_256_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_256_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin32_65535_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_65535_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_65535_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_65535_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestRead_Bin32_65536_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_65536_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_65536_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.Read() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.Read() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_65536_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( unpacker.Read() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_65536_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32_65536_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_65536_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinary( out result ) );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					Assert.Throws<InvalidMessagePackStreamException>( () => unpacker.ReadBinary( out result ) );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32_65536_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				Assert.IsTrue( unpacker.ReadBinary( out result ) );
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

#if FEATURE_TAP

		[Test]
		public async Task TestRead_FixStr_0Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_FixStr_0Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_FixStrAsync_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestReadString_FixStrAsync_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestRead_FixStr_1Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_FixStr_1Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_FixStr_1Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_FixStr_1Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_FixStrAsync_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_FixStrAsync_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_FixStrAsync_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_FixStrAsync_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public async Task TestRead_FixStr_31Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_FixStr_31Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_FixStr_31Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_FixStr_31Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_FixStrAsync_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_FixStrAsync_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_FixStrAsync_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_FixStrAsync_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_0Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_Str8_0Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str8Async_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestReadString_Str8Async_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_1Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_1Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_1Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str8_1Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str8Async_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str8Async_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str8Async_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str8Async_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_31Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_31Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_31Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str8_31Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str8Async_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str8Async_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str8Async_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str8Async_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_32Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_32Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_32Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str8_32Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str8Async_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str8Async_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str8Async_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str8Async_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_255Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_255Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_255Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str8_255Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str8Async_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str8Async_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str8Async_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str8Async_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_0Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_Str16_0Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_1Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_1Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_1Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str16_1Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16Async_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str16Async_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_31Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_31Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_31Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str16_31Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16Async_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str16Async_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_32Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_32Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_32Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str16_32Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16Async_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str16Async_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_255Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_255Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_255Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str16_255Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16Async_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str16Async_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_256Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_256Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_256Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str16_256Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16Async_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str16Async_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_65535Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_65535Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_65535Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str16_65535Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str16Async_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str16Async_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str16Async_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_0Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_Str32_0Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 0 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_1Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_1Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_1Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_1Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 1 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32Async_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str32Async_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_31Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_31Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_31Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_31Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 31 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32Async_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str32Async_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_32Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_32Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_32Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_32Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 32 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32Async_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str32Async_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_255Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_255Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_255Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_255Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 255 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32Async_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str32Async_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_256Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_256Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_256Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_256Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 256 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32Async_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str32Async_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_65535Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_65535Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_65535Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_65535Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65535 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32Async_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str32Async_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_65536Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_65536Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_65536Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_65536Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( String )result.Value, Is.EqualTo( new String( 'A', 65536 ) ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( String ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_65536_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestReadString_Str32Async_65536_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Str32Async_65536_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Str32Async_65536_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_0Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_Bin8_0Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin8Async_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestReadString_Bin8Async_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_1Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_1Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_1Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin8_1Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin8Async_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin8Async_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin8Async_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin8Async_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_31Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_31Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_31Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin8_31Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin8Async_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin8Async_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin8Async_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin8Async_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_32Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_32Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_32Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin8_32Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin8Async_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin8Async_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin8Async_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin8Async_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_255Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_255Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_255Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin8_255Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin8Async_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin8Async_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin8Async_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin8Async_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_0Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_Bin16_0Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_1Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_1Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_1Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin16_1Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16Async_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16Async_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_31Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_31Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_31Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin16_31Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16Async_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16Async_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_32Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_32Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_32Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin16_32Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16Async_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16Async_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_255Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_255Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_255Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin16_255Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16Async_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16Async_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_256Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_256Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_256Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin16_256Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16Async_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16Async_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_65535Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_65535Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_65535Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin16_65535Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin16Async_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin16Async_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin16Async_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_0Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_Bin32_0Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 0 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_1Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_1Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_1Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_1Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32Async_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32Async_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )'A', 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 1 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_31Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_31Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_31Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_31Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32Async_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32Async_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 31 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_32Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_32Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_32Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_32Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32Async_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32Async_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )'A', 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 32 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_255Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_255Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_255Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_255Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32Async_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32Async_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 255 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_256Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_256Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_256Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_256Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32Async_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32Async_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 256 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_65535Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_65535Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_65535Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_65535Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32Async_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32Async_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 65535 ) ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_65536Async_AsString_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_65536Async_AsString_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_65536Async_AsString_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_65536Async_AsString_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_65536_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public void TestReadString_Bin32Async_65536_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public void TestReadString_Bin32Async_65536_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadStringAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadStringAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadString_Bin32Async_65536_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )'A', 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				String result;
				var ret = await unpacker.ReadStringAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( new String( 'A', 65536 ) ) );
			}
		}

		[Test]
		public async Task TestRead_FixStr_0Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_FixStr_0Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_FixStrAsync_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestReadBinary_FixStrAsync_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_FixStr_1Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_FixStr_1Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_FixStr_1Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_FixStr_1Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_FixStrAsync_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_FixStrAsync_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_FixStrAsync_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_FixStrAsync_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xA1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_FixStr_31Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_FixStr_31Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_FixStr_31Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_FixStr_31Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_FixStrAsync_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_FixStrAsync_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_FixStrAsync_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_FixStrAsync_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xBF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_0Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_Str8_0Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str8Async_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Str8Async_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_1Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_1Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_1Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str8_1Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str8Async_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str8Async_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str8Async_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str8Async_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_31Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_31Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_31Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str8_31Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str8Async_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str8Async_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str8Async_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str8Async_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_32Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_32Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_32Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str8_32Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str8Async_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str8Async_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str8Async_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str8Async_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str8_255Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str8_255Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str8_255Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str8_255Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str8Async_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str8Async_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str8Async_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str8Async_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xD9, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_0Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_Str16_0Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_1Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_1Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_1Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str16_1Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16Async_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16Async_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_31Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_31Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_31Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str16_31Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16Async_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16Async_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_32Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_32Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_32Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str16_32Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16Async_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16Async_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_255Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_255Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_255Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str16_255Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16Async_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16Async_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_256Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_256Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_256Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str16_256Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16Async_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16Async_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str16_65535Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str16_65535Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str16_65535Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str16_65535Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str16Async_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str16Async_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str16Async_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDA, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_0Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_Str32_0Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				var asString = ( String )result.Value;
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault() );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_1Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_1Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_1Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_1Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_31Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_31Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_31Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_31Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_32Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_32Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_32Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_32Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_255Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_255Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_255Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_255Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_256Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_256Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_256Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_256Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_65535Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_65535Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_65535Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_65535Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Str32_65536Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Str32_65536Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Str32_65536Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Str32_65536Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_65536_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_65536_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Str32Async_65536_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Str32Async_65536_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xDB, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_0Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_Bin8_0Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin8Async_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin8Async_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_1Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_1Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_1Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin8_1Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin8Async_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin8Async_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin8Async_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin8Async_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_31Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_31Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_31Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin8_31Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin8Async_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin8Async_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin8Async_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin8Async_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_32Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_32Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_32Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin8_32Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin8Async_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin8Async_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin8Async_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin8Async_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin8_255Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin8_255Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin8_255Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin8_255Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin8Async_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin8Async_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin8Async_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin8Async_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC4, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_0Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_Bin16_0Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_1Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_1Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_1Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin16_1Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16Async_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16Async_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_31Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_31Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_31Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin16_31Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16Async_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16Async_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_32Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_32Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_32Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin16_32Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16Async_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16Async_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_255Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_255Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_255Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin16_255Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16Async_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16Async_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_256Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_256Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_256Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin16_256Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16Async_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16Async_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin16_65535Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin16_65535Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin16_65535Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin16_65535Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin16Async_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin16Async_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin16Async_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC5, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_0Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestRead_Bin32_0Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_0_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_0_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 0 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_1Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_1Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_1Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_1Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_1_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 1 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_1_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_1_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 0 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_1_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 1 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 2 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 1 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_31Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_31Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_31Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_31Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_31_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_31_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_31_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 30 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_31_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x1F }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 31 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_32Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_32Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_32Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_32Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_32_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 32 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_32_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_32_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 31 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_32_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0x20 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 33 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 32 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_255Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_255Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_255Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_255Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_255_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_255_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_255_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 254 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_255_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 255 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_256Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_256Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_256Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_256Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_256_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 256 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_256_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_256_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 255 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_256_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 1, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 257 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 256 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_65535Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_65535Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_65535Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_65535Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_65535_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_65535_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_65535_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65534 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_65535_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 0, 0xFF, 0xFF }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65535 ).ToArray() ) );
			}
		}

		[Test]
		public async Task TestRead_Bin32_65536Async_AsBinary_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public void TestRead_Bin32_65536Async_AsBinary_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public void TestRead_Bin32_65536Async_AsBinary_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadAsync().GetAwaiter().GetResult() );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadAsync() );
				}
			}
		}

		[Test]
		public async Task TestRead_Bin32_65536Async_AsBinary_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Assert.IsTrue( await unpacker.ReadAsync() );
#pragma warning disable 612,618
				var result = unpacker.Data;
#pragma warning restore 612,618
				Assert.IsTrue( result.HasValue );
				Assert.That( ( Byte[] )result.Value, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
				Assert.That( result.Value.UnderlyingType, Is.EqualTo( typeof( Byte[] ) ) );

				// raw/str always can be byte[]
				var asBinary = ( byte[] )result.Value;
				Assert.Throws<InvalidOperationException>( () => { var asString = ( String )result.Value; } );
				Assert.That( result.Value.IsTypeOf( typeof( string ) ).GetValueOrDefault(), Is.False );
				Assert.That( result.Value.IsTypeOf( typeof( byte[] ) ).GetValueOrDefault() );
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_65536_JustLength()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65536 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_65536_TooShort()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public void TestReadBinary_Bin32Async_65536_TooShort_NotSeekable()
		{
			if ( !this.MayFailToRollback )
			{
				// skip because this test is not neccessary.
				return;
			}

			using( var buffer =
				new NonSeekableStream(
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65535 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				if ( this.CanRevert( unpacker ) )
				{
					// Just fail and revert.
					var initialOffset = this.GetOffset( unpacker );
					Assert.IsFalse( unpacker.ReadBinaryAsync().GetAwaiter().GetResult().Success );
					Assert.That( this.GetOffset( unpacker ), Is.EqualTo( initialOffset ) );
				}
				else
				{
					AssertEx.ThrowsAsync<InvalidMessagePackStreamException>( async () => await unpacker.ReadBinaryAsync() );
				}
			}
		}

		[Test]
		public async Task TestReadBinary_Bin32Async_65536_HasExtra()
		{
			using( var buffer =
				new MemoryStream( 
					new byte[] { 0xC6, 0, 1, 0, 0 }
					.Concat( Enumerable.Repeat( ( byte )0xFF, 65537 ) ).ToArray()
				)
			)
			using( var unpacker = this.CreateUnpacker( buffer ) )
			{
				Byte[] result;
				var ret = await unpacker.ReadBinaryAsync();
				Assert.IsTrue( ret.Success );
				result = ret.Value;
				Assert.That( result, Is.EqualTo( Enumerable.Repeat( 0xFF, 65536 ).ToArray() ) );
			}
		}

#endif // FEATURE_TAP

	}
}
