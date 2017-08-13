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
using System.Globalization;
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
	// This file was generated from UnpackerTest.Skip.Veriations.tt T4Template.
	// Do not modify this file. Edit UnpackerTest.Skip.Veriations.tt instead.

	// ReSharper disable once InconsistentNaming
	partial class UnpackerTest
	{

		[Test]
		public void TestSkip_NilValue()
		{
			var binary = new byte[]{ 0xC0, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_TrueValue()
		{
			var binary = new byte[]{ 0xC3, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FalseValue()
		{
			var binary = new byte[]{ 0xC2, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_SignedInt8()
		{
			var binary = new byte[]{ 0xD0, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_UnsignedInt8()
		{
			var binary = new byte[]{ 0xCC, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_SignedInt16()
		{
			var binary = new byte[]{ 0xD1, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_UnsignedInt16()
		{
			var binary = new byte[]{ 0xCD, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_SignedInt32()
		{
			var binary = new byte[]{ 0xD2, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_UnsignedInt32()
		{
			var binary = new byte[]{ 0xCE, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_SignedInt64()
		{
			var binary = new byte[]{ 0xD3, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_UnsignedInt64()
		{
			var binary = new byte[]{ 0xCF, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Real32()
		{
			var binary = new byte[]{ 0xCA, 0x00, 0x00, 0x00, 0x00, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Real64()
		{
			var binary = new byte[]{ 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixExt1()
		{
			var binary = new byte[]{ 0xD4, 0x7F, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixExt2()
		{
			var binary = new byte[]{ 0xD5, 0x7F, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixExt4()
		{
			var binary = new byte[]{ 0xD6, 0x7F, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixExt8()
		{
			var binary = new byte[]{ 0xD7, 0x7F, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixExt16()
		{
			var binary = new byte[]{ 0xD8, 0x7F, 0x10, 0x0F, 0x0E, 0x0D, 0x0C, 0x0B, 0x0A, 0x09, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixInt_Zero()
		{
			var binary = new byte[]{ 0x00, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixInt_Min()
		{
			var binary = new byte[]{ 0xE0, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixInt_Max()
		{
			var binary = new byte[]{ 0x7F, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixedArray_Zero()
		{
			var binary = new byte[]{ 0x90, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixedArray_Min()
		{
			var binary = new byte[]{ 0x91, 0x41, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixedArray_Max()
		{
			var binary = new byte[]{ 0x9F, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixedMap_Zero()
		{
			var binary = new byte[]{ 0x80, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixedMap_Min()
		{
			var binary = new byte[]{ 0x81, 0x41, 0x41, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixedMap_Max()
		{
			var binary = new byte[]{ 0x8F, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixedRaw_Zero()
		{
			var binary = new byte[]{ 0xA0, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixedRaw_Min()
		{
			var binary = new byte[]{ 0xA1, 0x41, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_FixedRaw_Max()
		{
			var binary = new byte[]{ 0xBF, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Array16_Zero()
		{
			var binary = new byte[] { 0xDC }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Array16_Min()
		{
			var binary = new byte[] { 0xDC }.Concat( BitConverter.GetBytes( ( ushort )0x20 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x20 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Array16_Max()
		{
			var binary = new byte[] { 0xDC }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0xFFFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Array32_Zero()
		{
			var binary = new byte[] { 0xDD }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Array32_Min()
		{
			var binary = new byte[] { 0xDD }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x10000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Array32_Max()
		{
			var binary = new byte[] { 0xDD }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x10001 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Map16_Zero()
		{
			var binary = new byte[] { 0xDE }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Map16_Min()
		{
			var binary = new byte[] { 0xDE }.Concat( BitConverter.GetBytes( ( ushort )0x20 ).Reverse() ).Concat( Enumerable.Range( 0x0, 0x40 ).SelectMany( i => new byte[] { 0xA5 }.Concat( Encoding.UTF8.GetBytes( i.ToString( "X5", CultureInfo.InvariantCulture ) ) ) ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Map16_Max()
		{
			var binary = new byte[] { 0xDE }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( Enumerable.Range( 0x0, 0x1FFFE ).SelectMany( i => new byte[] { 0xA5 }.Concat( Encoding.UTF8.GetBytes( i.ToString( "X5", CultureInfo.InvariantCulture ) ) ) ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Map32_Zero()
		{
			var binary = new byte[] { 0xDF }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Map32_Min()
		{
			var binary = new byte[] { 0xDF }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( Enumerable.Range( 0x0, 0x20000 ).SelectMany( i => new byte[] { 0xA5 }.Concat( Encoding.UTF8.GetBytes( i.ToString( "X5", CultureInfo.InvariantCulture ) ) ) ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Map32_Max()
		{
			var binary = new byte[] { 0xDF }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( Enumerable.Range( 0x0, 0x20002 ).SelectMany( i => new byte[] { 0xA5 }.Concat( Encoding.UTF8.GetBytes( i.ToString( "X5", CultureInfo.InvariantCulture ) ) ) ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Str8_Zero()
		{
			var binary = new byte[] { 0xD9 }.Concat( new byte[] { 0x0 } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Str8_Min()
		{
			var binary = new byte[] { 0xD9 }.Concat( new byte[] { 0x20 } ).Concat( Enumerable.Repeat( 0x0, 0x20 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Str8_Max()
		{
			var binary = new byte[] { 0xD9 }.Concat( new byte[] { 0xFF } ).Concat( Enumerable.Repeat( 0x0, 0xFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Raw16_Zero()
		{
			var binary = new byte[] { 0xDA }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Raw16_Min()
		{
			var binary = new byte[] { 0xDA }.Concat( BitConverter.GetBytes( ( ushort )0x100 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x100 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Raw16_Max()
		{
			var binary = new byte[] { 0xDA }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0xFFFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Raw32_Zero()
		{
			var binary = new byte[] { 0xDB }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Raw32_Min()
		{
			var binary = new byte[] { 0xDB }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x10000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Raw32_Max()
		{
			var binary = new byte[] { 0xDB }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x10001 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Bin8_Zero()
		{
			var binary = new byte[] { 0xC4 }.Concat( new byte[] { 0x0 } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Bin8_Min()
		{
			var binary = new byte[] { 0xC4 }.Concat( new byte[] { 0x20 } ).Concat( Enumerable.Repeat( 0x0, 0x20 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Bin8_Max()
		{
			var binary = new byte[] { 0xC4 }.Concat( new byte[] { 0xFF } ).Concat( Enumerable.Repeat( 0x0, 0xFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Bin16_Zero()
		{
			var binary = new byte[] { 0xC5 }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Bin16_Min()
		{
			var binary = new byte[] { 0xC5 }.Concat( BitConverter.GetBytes( ( ushort )0x100 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x100 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Bin16_Max()
		{
			var binary = new byte[] { 0xC5 }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0xFFFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Bin32_Zero()
		{
			var binary = new byte[] { 0xC6 }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Bin32_Min()
		{
			var binary = new byte[] { 0xC6 }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x10000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Bin32_Max()
		{
			var binary = new byte[] { 0xC6 }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x10001 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Ext8_Zero()
		{
			var binary = new byte[] { 0xC7 }.Concat( new byte[] { 0x0 } ).Concat( new byte[] { 0x7F } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Ext8_Min()
		{
			var binary = new byte[] { 0xC7 }.Concat( new byte[] { 0x1 } ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0x1 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Ext8_Max()
		{
			var binary = new byte[] { 0xC7 }.Concat( new byte[] { 0xFF } ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0xFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Ext16_Zero()
		{
			var binary = new byte[] { 0xC8 }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Ext16_Min()
		{
			var binary = new byte[] { 0xC8 }.Concat( BitConverter.GetBytes( ( ushort )0x100 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0x100 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Ext16_Max()
		{
			var binary = new byte[] { 0xC8 }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0xFFFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Ext32_Zero()
		{
			var binary = new byte[] { 0xC9 }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Ext32_Min()
		{
			var binary = new byte[] { 0xC9 }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0x10000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public void TestSkip_Ext32_Max()
		{
			var binary = new byte[] { 0xC9 }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0x10001 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

#if FEATURE_TAP

		[Test]
		public async Task TestSkipAsync_NilValue()
		{
			var binary = new byte[]{ 0xC0, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_TrueValue()
		{
			var binary = new byte[]{ 0xC3, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FalseValue()
		{
			var binary = new byte[]{ 0xC2, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_SignedInt8()
		{
			var binary = new byte[]{ 0xD0, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_UnsignedInt8()
		{
			var binary = new byte[]{ 0xCC, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_SignedInt16()
		{
			var binary = new byte[]{ 0xD1, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_UnsignedInt16()
		{
			var binary = new byte[]{ 0xCD, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_SignedInt32()
		{
			var binary = new byte[]{ 0xD2, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_UnsignedInt32()
		{
			var binary = new byte[]{ 0xCE, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_SignedInt64()
		{
			var binary = new byte[]{ 0xD3, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_UnsignedInt64()
		{
			var binary = new byte[]{ 0xCF, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Real32()
		{
			var binary = new byte[]{ 0xCA, 0x00, 0x00, 0x00, 0x00, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Real64()
		{
			var binary = new byte[]{ 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixExt1()
		{
			var binary = new byte[]{ 0xD4, 0x7F, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixExt2()
		{
			var binary = new byte[]{ 0xD5, 0x7F, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixExt4()
		{
			var binary = new byte[]{ 0xD6, 0x7F, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixExt8()
		{
			var binary = new byte[]{ 0xD7, 0x7F, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixExt16()
		{
			var binary = new byte[]{ 0xD8, 0x7F, 0x10, 0x0F, 0x0E, 0x0D, 0x0C, 0x0B, 0x0A, 0x09, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixInt_Zero()
		{
			var binary = new byte[]{ 0x00, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixInt_Min()
		{
			var binary = new byte[]{ 0xE0, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixInt_Max()
		{
			var binary = new byte[]{ 0x7F, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixedArray_Zero()
		{
			var binary = new byte[]{ 0x90, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixedArray_Min()
		{
			var binary = new byte[]{ 0x91, 0x41, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixedArray_Max()
		{
			var binary = new byte[]{ 0x9F, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixedMap_Zero()
		{
			var binary = new byte[]{ 0x80, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixedMap_Min()
		{
			var binary = new byte[]{ 0x81, 0x41, 0x41, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixedMap_Max()
		{
			var binary = new byte[]{ 0x8F, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixedRaw_Zero()
		{
			var binary = new byte[]{ 0xA0, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixedRaw_Min()
		{
			var binary = new byte[]{ 0xA1, 0x41, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_FixedRaw_Max()
		{
			var binary = new byte[]{ 0xBF, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Array16_Zero()
		{
			var binary = new byte[] { 0xDC }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Array16_Min()
		{
			var binary = new byte[] { 0xDC }.Concat( BitConverter.GetBytes( ( ushort )0x20 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x20 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Array16_Max()
		{
			var binary = new byte[] { 0xDC }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0xFFFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Array32_Zero()
		{
			var binary = new byte[] { 0xDD }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Array32_Min()
		{
			var binary = new byte[] { 0xDD }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x10000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Array32_Max()
		{
			var binary = new byte[] { 0xDD }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x10001 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Map16_Zero()
		{
			var binary = new byte[] { 0xDE }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Map16_Min()
		{
			var binary = new byte[] { 0xDE }.Concat( BitConverter.GetBytes( ( ushort )0x20 ).Reverse() ).Concat( Enumerable.Range( 0x0, 0x40 ).SelectMany( i => new byte[] { 0xA5 }.Concat( Encoding.UTF8.GetBytes( i.ToString( "X5", CultureInfo.InvariantCulture ) ) ) ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Map16_Max()
		{
			var binary = new byte[] { 0xDE }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( Enumerable.Range( 0x0, 0x1FFFE ).SelectMany( i => new byte[] { 0xA5 }.Concat( Encoding.UTF8.GetBytes( i.ToString( "X5", CultureInfo.InvariantCulture ) ) ) ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Map32_Zero()
		{
			var binary = new byte[] { 0xDF }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Map32_Min()
		{
			var binary = new byte[] { 0xDF }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( Enumerable.Range( 0x0, 0x20000 ).SelectMany( i => new byte[] { 0xA5 }.Concat( Encoding.UTF8.GetBytes( i.ToString( "X5", CultureInfo.InvariantCulture ) ) ) ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Map32_Max()
		{
			var binary = new byte[] { 0xDF }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( Enumerable.Range( 0x0, 0x20002 ).SelectMany( i => new byte[] { 0xA5 }.Concat( Encoding.UTF8.GetBytes( i.ToString( "X5", CultureInfo.InvariantCulture ) ) ) ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Str8_Zero()
		{
			var binary = new byte[] { 0xD9 }.Concat( new byte[] { 0x0 } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Str8_Min()
		{
			var binary = new byte[] { 0xD9 }.Concat( new byte[] { 0x20 } ).Concat( Enumerable.Repeat( 0x0, 0x20 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Str8_Max()
		{
			var binary = new byte[] { 0xD9 }.Concat( new byte[] { 0xFF } ).Concat( Enumerable.Repeat( 0x0, 0xFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Raw16_Zero()
		{
			var binary = new byte[] { 0xDA }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Raw16_Min()
		{
			var binary = new byte[] { 0xDA }.Concat( BitConverter.GetBytes( ( ushort )0x100 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x100 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Raw16_Max()
		{
			var binary = new byte[] { 0xDA }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0xFFFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Raw32_Zero()
		{
			var binary = new byte[] { 0xDB }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Raw32_Min()
		{
			var binary = new byte[] { 0xDB }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x10000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Raw32_Max()
		{
			var binary = new byte[] { 0xDB }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x10001 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Bin8_Zero()
		{
			var binary = new byte[] { 0xC4 }.Concat( new byte[] { 0x0 } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Bin8_Min()
		{
			var binary = new byte[] { 0xC4 }.Concat( new byte[] { 0x20 } ).Concat( Enumerable.Repeat( 0x0, 0x20 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Bin8_Max()
		{
			var binary = new byte[] { 0xC4 }.Concat( new byte[] { 0xFF } ).Concat( Enumerable.Repeat( 0x0, 0xFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Bin16_Zero()
		{
			var binary = new byte[] { 0xC5 }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Bin16_Min()
		{
			var binary = new byte[] { 0xC5 }.Concat( BitConverter.GetBytes( ( ushort )0x100 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x100 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Bin16_Max()
		{
			var binary = new byte[] { 0xC5 }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0xFFFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Bin32_Zero()
		{
			var binary = new byte[] { 0xC6 }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Bin32_Min()
		{
			var binary = new byte[] { 0xC6 }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x10000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Bin32_Max()
		{
			var binary = new byte[] { 0xC6 }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( Enumerable.Repeat( 0x0, 0x10001 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Ext8_Zero()
		{
			var binary = new byte[] { 0xC7 }.Concat( new byte[] { 0x0 } ).Concat( new byte[] { 0x7F } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Ext8_Min()
		{
			var binary = new byte[] { 0xC7 }.Concat( new byte[] { 0x1 } ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0x1 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Ext8_Max()
		{
			var binary = new byte[] { 0xC7 }.Concat( new byte[] { 0xFF } ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0xFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Ext16_Zero()
		{
			var binary = new byte[] { 0xC8 }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Ext16_Min()
		{
			var binary = new byte[] { 0xC8 }.Concat( BitConverter.GetBytes( ( ushort )0x100 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0x100 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Ext16_Max()
		{
			var binary = new byte[] { 0xC8 }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0xFFFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Ext32_Zero()
		{
			var binary = new byte[] { 0xC9 }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Ext32_Min()
		{
			var binary = new byte[] { 0xC9 }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0x10000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

		[Test]
		public async Task TestSkipAsync_Ext32_Max()
		{
			var binary = new byte[] { 0xC9 }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0x10001 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = this.CreateUnpacker( buffer ) )
			{
				var result = await target.SkipAsync();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				if ( this.ShouldCheckStreamPosition )
				{
					Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
					// Verify centinel value still exists in the stream.
					Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
				}
			}
		}

#endif // FEATURE_TAP

	}
}
