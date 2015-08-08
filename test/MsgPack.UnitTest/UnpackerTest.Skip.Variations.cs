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

using System;
using System.IO;
using System.Linq;
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
	// ReSharper disable once InconsistentNaming
	partial class UnpackerTest_Skip
	{

		[Test]
		public void TestNilValue()
		{
			var binary = new byte[]{ 0xC0, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestTrueValue()
		{
			var binary = new byte[]{ 0xC3, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFalseValue()
		{
			var binary = new byte[]{ 0xC2, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestSignedInt8()
		{
			var binary = new byte[]{ 0xD0, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestUnsignedInt8()
		{
			var binary = new byte[]{ 0xCC, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestSignedInt16()
		{
			var binary = new byte[]{ 0xD1, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestUnsignedInt16()
		{
			var binary = new byte[]{ 0xCD, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestSignedInt32()
		{
			var binary = new byte[]{ 0xD2, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestUnsignedInt32()
		{
			var binary = new byte[]{ 0xCE, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestSignedInt64()
		{
			var binary = new byte[]{ 0xD3, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestUnsignedInt64()
		{
			var binary = new byte[]{ 0xCF, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestReal32()
		{
			var binary = new byte[]{ 0xCA, 0x00, 0x00, 0x00, 0x00, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestReal64()
		{
			var binary = new byte[]{ 0xCB, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixExt1()
		{
			var binary = new byte[]{ 0xD4, 0x7F, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixExt2()
		{
			var binary = new byte[]{ 0xD5, 0x7F, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixExt4()
		{
			var binary = new byte[]{ 0xD6, 0x7F, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixExt8()
		{
			var binary = new byte[]{ 0xD7, 0x7F, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixExt16()
		{
			var binary = new byte[]{ 0xD8, 0x7F, 0x10, 0x0F, 0x0E, 0x0D, 0x0C, 0x0B, 0x0A, 0x09, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixInt_Zero()
		{
			var binary = new byte[]{ 0x00, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixInt_Min()
		{
			var binary = new byte[]{ 0xE0, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixInt_Max()
		{
			var binary = new byte[]{ 0x7F, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixedArray_Zero()
		{
			var binary = new byte[]{ 0x90, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixedArray_Min()
		{
			var binary = new byte[]{ 0x91, 0x41, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixedArray_Max()
		{
			var binary = new byte[]{ 0x9F, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixedMap_Zero()
		{
			var binary = new byte[]{ 0x80, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixedMap_Min()
		{
			var binary = new byte[]{ 0x81, 0x41, 0x41, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixedMap_Max()
		{
			var binary = new byte[]{ 0x8F, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixedRaw_Zero()
		{
			var binary = new byte[]{ 0xA0, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixedRaw_Min()
		{
			var binary = new byte[]{ 0xA1, 0x41, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestFixedRaw_Max()
		{
			var binary = new byte[]{ 0xBF, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0xC2 };
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestArray16_Zero()
		{
			var binary = new byte[] { 0xDC }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestArray16_Min()
		{
			var binary = new byte[] { 0xDC }.Concat( BitConverter.GetBytes( ( ushort )0x20 ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x20 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestArray16_Max()
		{
			var binary = new byte[] { 0xDC }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0xFFFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestArray32_Zero()
		{
			var binary = new byte[] { 0xDD }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestArray32_Min()
		{
			var binary = new byte[] { 0xDD }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x10000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestArray32_Max()
		{
			var binary = new byte[] { 0xDD }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x10001 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestMap16_Zero()
		{
			var binary = new byte[] { 0xDE }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestMap16_Min()
		{
			var binary = new byte[] { 0xDE }.Concat( BitConverter.GetBytes( ( ushort )0x20 ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x40 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestMap16_Max()
		{
			var binary = new byte[] { 0xDE }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x1FFFE ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestMap32_Zero()
		{
			var binary = new byte[] { 0xDF }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestMap32_Min()
		{
			var binary = new byte[] { 0xDF }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x20000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestMap32_Max()
		{
			var binary = new byte[] { 0xDF }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x20002 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestStr8_Zero()
		{
			var binary = new byte[] { 0xD9 }.Concat( new byte[] { 0x0 } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestStr8_Min()
		{
			var binary = new byte[] { 0xD9 }.Concat( new byte[] { 0x20 } ).Concat( Enumerable.Repeat( 0x41, 0x20 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestStr8_Max()
		{
			var binary = new byte[] { 0xD9 }.Concat( new byte[] { 0xFF } ).Concat( Enumerable.Repeat( 0x41, 0xFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestRaw16_Zero()
		{
			var binary = new byte[] { 0xDA }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestRaw16_Min()
		{
			var binary = new byte[] { 0xDA }.Concat( BitConverter.GetBytes( ( ushort )0x100 ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x100 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestRaw16_Max()
		{
			var binary = new byte[] { 0xDA }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0xFFFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestRaw32_Zero()
		{
			var binary = new byte[] { 0xDB }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestRaw32_Min()
		{
			var binary = new byte[] { 0xDB }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x10000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestRaw32_Max()
		{
			var binary = new byte[] { 0xDB }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x10001 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestBin8_Zero()
		{
			var binary = new byte[] { 0xC4 }.Concat( new byte[] { 0x0 } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestBin8_Min()
		{
			var binary = new byte[] { 0xC4 }.Concat( new byte[] { 0x20 } ).Concat( Enumerable.Repeat( 0x41, 0x20 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestBin8_Max()
		{
			var binary = new byte[] { 0xC4 }.Concat( new byte[] { 0xFF } ).Concat( Enumerable.Repeat( 0x41, 0xFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestBin16_Zero()
		{
			var binary = new byte[] { 0xC5 }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestBin16_Min()
		{
			var binary = new byte[] { 0xC5 }.Concat( BitConverter.GetBytes( ( ushort )0x100 ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x100 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestBin16_Max()
		{
			var binary = new byte[] { 0xC5 }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0xFFFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestBin32_Zero()
		{
			var binary = new byte[] { 0xC6 }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestBin32_Min()
		{
			var binary = new byte[] { 0xC6 }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x10000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestBin32_Max()
		{
			var binary = new byte[] { 0xC6 }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( Enumerable.Repeat( 0x41, 0x10001 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestExt8_Zero()
		{
			var binary = new byte[] { 0xC7 }.Concat( new byte[] { 0x0 } ).Concat( new byte[] { 0x7F } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestExt8_Min()
		{
			var binary = new byte[] { 0xC7 }.Concat( new byte[] { 0x1 } ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0x1 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestExt8_Max()
		{
			var binary = new byte[] { 0xC7 }.Concat( new byte[] { 0xFF } ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0xFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestExt16_Zero()
		{
			var binary = new byte[] { 0xC8 }.Concat( BitConverter.GetBytes( ( ushort )0x0 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestExt16_Min()
		{
			var binary = new byte[] { 0xC8 }.Concat( BitConverter.GetBytes( ( ushort )0x100 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0x100 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestExt16_Max()
		{
			var binary = new byte[] { 0xC8 }.Concat( BitConverter.GetBytes( ( ushort )0xFFFF ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0xFFFF ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestExt32_Zero()
		{
			var binary = new byte[] { 0xC9 }.Concat( BitConverter.GetBytes( ( uint )0x0 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestExt32_Min()
		{
			var binary = new byte[] { 0xC9 }.Concat( BitConverter.GetBytes( ( uint )0x10000 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0x10000 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}

		[Test]
		public void TestExt32_Max()
		{
			var binary = new byte[] { 0xC9 }.Concat( BitConverter.GetBytes( ( uint )0x10001 ).Reverse() ).Concat( new byte[] { 0x7F } ).Concat( Enumerable.Repeat( 0x41, 0x10001 ).Select( i => ( byte )i ) ).Concat( new byte[] { 0xC2 } ).ToArray();
			using ( var buffer = new MemoryStream( binary ) )
			using ( var target = new ItemsUnpacker( buffer, false ) )
			{
				var result = target.Skip();
				Assert.That( result, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				Assert.That( buffer.Position, Is.EqualTo( binary.Length - 1 /* minus centinel byte */ ) );
				// Verify centinel value still exists in the stream.
				Assert.That( buffer.ReadByte(), Is.EqualTo( 0xC2 ) );
			}
		}
	}
}
