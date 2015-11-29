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
using System.Collections.Generic;
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
	// This file was generated from PackerTest.PackBinary.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit PackerTest.PackBinary.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class PackerTest_Pack
	{
#if !NETFX_CORE

		[Test]
		public void TestPackString_0_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 0 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 0 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_1_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 1 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 1 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_2_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 2 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA2 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 2 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_3_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 3 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA3 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 3 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_4_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 4 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA4 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 4 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_5_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 5 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA5 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 5 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_6_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 6 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA6 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 6 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_7_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 7 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA7 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 7 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_8_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 8 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA8 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 8 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_9_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 9 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA9 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 9 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_10_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 10 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAA } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 10 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_11_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 11 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAB } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 11 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_12_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 12 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAC } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 12 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_13_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 13 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAD } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 13 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_14_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 14 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAE } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 14 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_15_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 15 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 15 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_16_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 16 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 16 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_17_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 17 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 17 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_18_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 18 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB2 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 18 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_19_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 19 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB3 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 19 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_20_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 20 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB4 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 20 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_21_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 21 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB5 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 21 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_22_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 22 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB6 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 22 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_23_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 23 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB7 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 23 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_24_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 24 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB8 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 24 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_25_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 25 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB9 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 25 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_26_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 26 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBA } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 26 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_27_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 27 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBB } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 27 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_28_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 28 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBC } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 28 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_29_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 29 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBD } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 29 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_30_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 30 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBE } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 30 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_31_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 31 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 31 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_32_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 32 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 32 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_32_EnumerableOfChar_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackString( new String( 'A', 32 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackStringHeader_32_EnumerableOfChar_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackStringHeader( 32 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed,
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
			}
		}

		[Test]
		public void TestPackString_255_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 255 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 255 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_255_EnumerableOfChar_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackString( new String( 'A', 255 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackStringHeader_255_EnumerableOfChar_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackStringHeader( 255 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed,
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
			}
		}

		[Test]
		public void TestPackString_256_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 256 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 256 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_65535_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 65535 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 65535 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_65536_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 65536 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 65536 ).ToArray() )
				);
			}
		}
#endif // !NETFX_CORE

		[Test]
		public void TestPackString_0_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 0 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 0 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_1_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 1 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 1 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_2_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 2 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA2 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 2 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_3_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 3 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA3 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 3 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_4_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 4 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA4 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 4 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_5_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 5 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA5 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 5 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_6_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 6 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA6 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 6 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_7_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 7 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA7 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 7 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_8_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 8 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA8 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 8 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_9_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 9 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA9 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 9 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_10_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 10 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAA } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 10 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_11_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 11 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAB } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 11 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_12_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 12 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAC } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 12 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_13_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 13 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAD } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 13 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_14_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 14 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAE } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 14 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_15_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 15 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 15 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_16_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 16 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 16 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_17_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 17 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 17 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_18_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 18 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB2 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 18 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_19_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 19 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB3 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 19 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_20_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 20 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB4 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 20 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_21_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 21 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB5 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 21 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_22_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 22 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB6 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 22 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_23_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 23 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB7 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 23 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_24_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 24 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB8 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 24 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_25_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 25 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB9 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 25 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_26_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 26 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBA } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 26 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_27_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 27 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBB } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 27 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_28_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 28 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBC } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 28 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_29_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 29 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBD } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 29 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_30_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 30 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBE } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 30 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_31_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 31 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 31 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_32_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 32 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 32 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_32_String_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackString( new String( 'A', 32 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackStringHeader_32_String_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackStringHeader( 32 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed,
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
			}
		}

		[Test]
		public void TestPackString_255_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 255 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 255 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_255_String_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackString( new String( 'A', 255 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackStringHeader_255_String_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackStringHeader( 255 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed,
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
			}
		}

		[Test]
		public void TestPackString_256_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 256 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 256 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_65535_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 65535 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 65535 ).ToArray() )
				);
			}
		}

		[Test]
		public void TestPackString_65536_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackString( new String( 'A', 65536 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 65536 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackRaw_0_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 0 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 0 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_0_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 0 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 0 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_0_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 0 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 0 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_0_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 0 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 0 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_1_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 1 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 1 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_1_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 1 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 1 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_1_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 1 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 1 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_1_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 1 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 1 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_31_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 31 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 31 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_31_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 31 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 31 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_31_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 31 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 31 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_31_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 31 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 31 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_32_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 32 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 32 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_32_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 32 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 32 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_32_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 32 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 32 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_32_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 32 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 32 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_255_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 255 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 255 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_255_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 255 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 255 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_255_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 255 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 255 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_255_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 255 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 255 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_256_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 256 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 256 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_256_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 256 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 256 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_256_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 256 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 256 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_256_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 256 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 256 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_65535_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65535 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 65535 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_65535_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65535 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 65535 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_65535_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65535 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 65535 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_65535_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65535 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 65535 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_65536_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65536 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 65536 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_65536_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65536 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 65536 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_65536_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65536 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 65536 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_65536_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65536 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 65536 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_0_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_0_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_0_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_0_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_1_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_1_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_1_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x1 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_1_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_31_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_31_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_31_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x1F } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_31_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_32_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_32_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_32_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_32_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_255_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_255_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_255_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_255_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_256_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_256_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_256_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC5, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_256_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_65535_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_65535_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_65535_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC5, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_65535_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_65536_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_65536_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_65536_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC6, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_65536_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_0_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_0_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_0_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_0_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_1_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_1_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_1_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x1 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_1_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_31_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_31_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_31_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x1F } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_31_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_32_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_32_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_32_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_32_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_255_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_255_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_255_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_255_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_256_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_256_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_256_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC5, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_256_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_65535_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_65535_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_65535_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC5, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_65535_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_65536_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_65536_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_65536_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC6, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_65536_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}		

		[Test]
		public void TestPackRaw_0_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_0_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_0_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_0_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public void TestPackRawHeader_0_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRawHeader( 0 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}
		
		[Test]
		public void TestPackRawHeader_0_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRawHeader( 0 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public void TestPackBinaryHeader_0_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinaryHeader( 0 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC4, 0x0 } ) 
				);
			}
		}
		
		[Test]
		public void TestPackBinaryHeader_0_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinaryHeader( 0 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}

		[Test]
		public void TestPackRaw_1_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_1_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_1_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x1 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_1_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public void TestPackRawHeader_1_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRawHeader( 1 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
			}
		}
		
		[Test]
		public void TestPackRawHeader_1_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRawHeader( 1 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public void TestPackBinaryHeader_1_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinaryHeader( 1 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC4, 0x1 } ) 
				);
			}
		}
		
		[Test]
		public void TestPackBinaryHeader_1_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinaryHeader( 1 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
			}
		}

		[Test]
		public void TestPackRaw_31_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_31_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_31_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x1F } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_31_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public void TestPackRawHeader_31_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRawHeader( 31 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
			}
		}
		
		[Test]
		public void TestPackRawHeader_31_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRawHeader( 31 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public void TestPackBinaryHeader_31_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinaryHeader( 31 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC4, 0x1F } ) 
				);
			}
		}
		
		[Test]
		public void TestPackBinaryHeader_31_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinaryHeader( 31 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
			}
		}

		[Test]
		public void TestPackRaw_32_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_32_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_32_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_32_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public void TestPackRawHeader_32_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRawHeader( 32 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xD9, 0x20 } ) 
				);
			}
		}
		
		[Test]
		public void TestPackRawHeader_32_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRawHeader( 32 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public void TestPackBinaryHeader_32_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinaryHeader( 32 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC4, 0x20 } ) 
				);
			}
		}
		
		[Test]
		public void TestPackBinaryHeader_32_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinaryHeader( 32 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
			}
		}

		[Test]
		public void TestPackRaw_255_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_255_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_255_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_255_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public void TestPackRawHeader_255_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRawHeader( 255 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xD9, 0xFF } ) 
				);
			}
		}
		
		[Test]
		public void TestPackRawHeader_255_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRawHeader( 255 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public void TestPackBinaryHeader_255_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinaryHeader( 255 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC4, 0xFF } ) 
				);
			}
		}
		
		[Test]
		public void TestPackBinaryHeader_255_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinaryHeader( 255 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
			}
		}

		[Test]
		public void TestPackRaw_256_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_256_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_256_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC5, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_256_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public void TestPackRawHeader_256_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRawHeader( 256 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
			}
		}
		
		[Test]
		public void TestPackRawHeader_256_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRawHeader( 256 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public void TestPackBinaryHeader_256_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinaryHeader( 256 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC5, 0x1, 0x0 } ) 
				);
			}
		}
		
		[Test]
		public void TestPackBinaryHeader_256_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinaryHeader( 256 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
			}
		}

		[Test]
		public void TestPackRaw_65535_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_65535_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_65535_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC5, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_65535_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public void TestPackRawHeader_65535_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRawHeader( 65535 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
			}
		}
		
		[Test]
		public void TestPackRawHeader_65535_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRawHeader( 65535 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public void TestPackBinaryHeader_65535_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinaryHeader( 65535 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC5, 0xFF, 0xFF } ) 
				);
			}
		}
		
		[Test]
		public void TestPackBinaryHeader_65535_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinaryHeader( 65535 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
			}
		}

		[Test]
		public void TestPackRaw_65536_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackRaw_65536_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRaw( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}


		[Test]
		public void TestPackBinary_65536_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC6, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public void TestPackBinary_65536_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinary( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public void TestPackRawHeader_65536_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackRawHeader( 65536 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
			}
		}
		
		[Test]
		public void TestPackRawHeader_65536_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackRawHeader( 65536 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public void TestPackBinaryHeader_65536_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				target.PackBinaryHeader( 65536 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC6, 0x0, 0x1, 0x0, 0x0 } ) 
				);
			}
		}
		
		[Test]
		public void TestPackBinaryHeader_65536_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				target.PackBinaryHeader( 65536 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
			}
		}

		[Test]
		public void TestPackRawHeader_MinusOne()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
#pragma warning disable 0618
				Assert.Throws<ArgumentOutOfRangeException>( () => target.PackRawHeader( -1 ) );
#pragma warning restore 0618
			}
		}

		[Test]
		public void TestPackStringHeader_MinusOne()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentOutOfRangeException>( () => target.PackStringHeader( -1 ) );
			}
		}

		[Test]
		public void TestPackBinaryHeader_MinusOne()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentOutOfRangeException>( () => target.PackBinaryHeader( -1 ) );
			}
		}

		[Test]
		public void TestPackRaw_IList_Null()
		{
			IList<byte> value = null;
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackRaw( value );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public void TestPackRaw_IEnumerable_Null()
		{
			IEnumerable<byte> value = null;
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackRaw( value );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public void TestPackRaw_Array_Null()
		{
			byte[] value = null;
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackRaw( value );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public void TestPackRawBody_IList_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackRawBody( new List<byte>{ 1, 2, 3 } );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 1, 2, 3 } ) 
				);
			}
		}


		[Test]
		public void TestPackRawBody_IList_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackRawBody( new List<byte>( 0 ) );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[ 0 ] ) 
				);
			}
		}

		[Test]
		public void TestPackRawBody_IList_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentNullException>( () => target.PackRawBody( default( IList<byte> ) ) );
			}
		}
		[Test]
		public void TestPackRawBody_IEnumerable_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackRawBody( Enumerable.Range( 1, 3 ).Select( i => ( byte )i ) );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 1, 2, 3 } ) 
				);
			}
		}


		[Test]
		public void TestPackRawBody_IEnumerable_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackRawBody( Enumerable.Empty<byte>() );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[ 0 ] ) 
				);
			}
		}

		[Test]
		public void TestPackRawBody_IEnumerable_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentNullException>( () => target.PackRawBody( default( IEnumerable<byte> ) ) );
			}
		}
		[Test]
		public void TestPackRawBody_Array_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackRawBody( new byte[] { 1, 2, 3 } );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 1, 2, 3 } ) 
				);
			}
		}


		[Test]
		public void TestPackRawBody_Array_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackRawBody( new byte[ 0 ] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[ 0 ] ) 
				);
			}
		}

		[Test]
		public void TestPackRawBody_Array_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentNullException>( () => target.PackRawBody( default( byte[] ) ) );
			}
		}
		[Test]
		public void TestPackString_IEnumerable_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackString( new char[] { 'A', 'B', 'C' } );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA3, ( byte )'A', ( byte )'B', ( byte )'C' } ) 
				);
			}
		}

		[Test]
		public void TestPackString_IEnumerable_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackString( new char[ 0 ] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}

		[Test]
		public void TestPackString_IEnumerable_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackString( null );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public void TestPackString_IEnumerable_WithEncoding_NonNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackString( new char[] { 'A', 'B', 'C' }, Encoding.Unicode );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA6, ( byte )'A', 0, ( byte )'B', 0, ( byte )'C', 0 } ) 
				);
			}
		}

		[Test]
		public void TestPackString_IEnumerable_WithEncoding_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackString( new char[ 0 ], Encoding.Unicode );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}

		[Test]
		public void TestPackString_IEnumerable_WithEncoding_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackString( null, Encoding.Unicode );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public void TestPackString_IEnumerable_NullEncoding()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentNullException>( () => target.PackString( new char[] { 'A', 'B', 'C' }, null ) );
			}
		}
		[Test]
		public void TestPackString_String_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackString( "ABC" );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA3, ( byte )'A', ( byte )'B', ( byte )'C' } ) 
				);
			}
		}

		[Test]
		public void TestPackString_String_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackString( String.Empty );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}

		[Test]
		public void TestPackString_String_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackString( null );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public void TestPackString_String_WithEncoding_NonNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackString( "ABC", Encoding.Unicode );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA6, ( byte )'A', 0, ( byte )'B', 0, ( byte )'C', 0 } ) 
				);
			}
		}

		[Test]
		public void TestPackString_String_WithEncoding_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackString( String.Empty, Encoding.Unicode );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}

		[Test]
		public void TestPackString_String_WithEncoding_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				target.PackString( null, Encoding.Unicode );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public void TestPackString_String_NullEncoding()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentNullException>( () => target.PackString( "ABC", null ) );
			}
		}
#if !NETFX_CORE

		[Test]
		public async Task TestPackStringAsync_0_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 0 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 0 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_1_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 1 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 1 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_2_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 2 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA2 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 2 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_3_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 3 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA3 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 3 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_4_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 4 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA4 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 4 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_5_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 5 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA5 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 5 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_6_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 6 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA6 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 6 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_7_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 7 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA7 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 7 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_8_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 8 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA8 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 8 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_9_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 9 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA9 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 9 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_10_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 10 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAA } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 10 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_11_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 11 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAB } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 11 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_12_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 12 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAC } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 12 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_13_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 13 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAD } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 13 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_14_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 14 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAE } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 14 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_15_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 15 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 15 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_16_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 16 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 16 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_17_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 17 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 17 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_18_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 18 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB2 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 18 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_19_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 19 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB3 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 19 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_20_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 20 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB4 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 20 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_21_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 21 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB5 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 21 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_22_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 22 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB6 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 22 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_23_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 23 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB7 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 23 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_24_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 24 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB8 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 24 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_25_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 25 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB9 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 25 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_26_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 26 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBA } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 26 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_27_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 27 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBB } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 27 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_28_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 28 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBC } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 28 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_29_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 29 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBD } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 29 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_30_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 30 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBE } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 30 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_31_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 31 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 31 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_32_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 32 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 32 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_32_EnumerableOfChar_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackStringAsync( new String( 'A', 32 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackStringHeaderAsync_32_EnumerableOfChar_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackStringHeaderAsync( 32 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed,
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_255_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 255 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 255 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_255_EnumerableOfChar_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackStringAsync( new String( 'A', 255 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackStringHeaderAsync_255_EnumerableOfChar_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackStringHeaderAsync( 255 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed,
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_256_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 256 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 256 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_65535_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 65535 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 65535 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_65536_EnumerableOfChar_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 65536 ) as IEnumerable<char> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 65536 ).ToArray() )
				);
			}
		}
#endif // !NETFX_CORE

		[Test]
		public async Task TestPackStringAsync_0_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 0 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 0 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_1_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 1 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 1 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_2_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 2 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA2 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 2 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_3_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 3 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA3 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 3 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_4_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 4 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA4 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 4 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_5_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 5 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA5 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 5 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_6_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 6 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA6 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 6 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_7_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 7 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA7 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 7 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_8_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 8 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA8 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 8 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_9_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 9 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA9 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 9 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_10_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 10 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAA } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 10 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_11_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 11 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAB } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 11 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_12_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 12 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAC } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 12 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_13_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 13 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAD } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 13 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_14_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 14 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAE } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 14 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_15_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 15 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xAF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 15 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_16_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 16 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 16 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_17_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 17 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 17 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_18_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 18 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB2 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 18 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_19_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 19 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB3 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 19 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_20_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 20 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB4 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 20 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_21_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 21 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB5 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 21 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_22_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 22 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB6 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 22 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_23_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 23 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB7 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 23 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_24_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 24 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB8 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 24 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_25_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 25 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xB9 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 25 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_26_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 26 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBA } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 26 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_27_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 27 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBB } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 27 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_28_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 28 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBC } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 28 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_29_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 29 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBD } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 29 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_30_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 30 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBE } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 30 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_31_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 31 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 31 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_32_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 32 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 32 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_32_String_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackStringAsync( new String( 'A', 32 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackStringHeaderAsync_32_String_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackStringHeaderAsync( 32 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed,
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_255_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 255 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 255 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_255_String_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackStringAsync( new String( 'A', 255 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackStringHeaderAsync_255_String_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackStringHeaderAsync( 255 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed,
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_256_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 256 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 256 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_65535_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 65535 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 65535 ).ToArray() )
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_65536_String_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackStringAsync( new String( 'A', 65536 ) as string );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )'A', 65536 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackRawAsync_0_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 0 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 0 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_0_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 0 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 0 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_0_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 0 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 0 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_0_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 0 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 0 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_1_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 1 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 1 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_1_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 1 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 1 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_1_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 1 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 1 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_1_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 1 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 1 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_31_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 31 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 31 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_31_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 31 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 31 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_31_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 31 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 31 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_31_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 31 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 31 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_32_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 32 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 32 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_32_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 32 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 32 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_32_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 32 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 32 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_32_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 32 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 32 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_255_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 255 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 255 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_255_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 255 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 255 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_255_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 255 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 255 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_255_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 255 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 255 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_256_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 256 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 256 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_256_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 256 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 256 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_256_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 256 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 256 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_256_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 256 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 256 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_65535_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 65535 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_65535_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 65535 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_65535_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 65535 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_65535_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 65535 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_65536_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 65536 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_65536_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 65536 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_65536_UncountableEnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Bin32 }.Concat( BitConverter.GetBytes( 65536 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_65536_UncountableEnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ) as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { MessagePackCode.Raw32 }.Concat( BitConverter.GetBytes( 65536 ).Reverse() ).ToArray() )
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_0_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_0_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_0_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_0_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_1_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_1_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_1_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x1 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_1_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_31_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_31_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_31_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x1F } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_31_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_32_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_32_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_32_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_32_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_255_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_255_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_255_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_255_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_256_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_256_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_256_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC5, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_256_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_65535_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_65535_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_65535_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC5, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_65535_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_65536_EnumerableOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_65536_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_65536_EnumerableOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC6, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_65536_EnumerableOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IEnumerable<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_0_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_0_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_0_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_0_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_1_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_1_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_1_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x1 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_1_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_31_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_31_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_31_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x1F } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_31_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_32_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_32_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_32_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_32_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_255_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_255_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_255_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_255_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_256_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_256_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_256_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC5, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_256_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_65535_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_65535_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_65535_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC5, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_65535_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_65536_ListOfByte_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_65536_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_65536_ListOfByte_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC6, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_65536_ListOfByte_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as IList<byte> );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}		

		[Test]
		public async Task TestPackRawAsync_0_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_0_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_0_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_0_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 0 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public async Task TestPackRawHeaderAsync_0_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawHeaderAsync( 0 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackRawHeaderAsync_0_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawHeaderAsync( 0 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public async Task TestPackBinaryHeaderAsync_0_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryHeaderAsync( 0 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC4, 0x0 } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryHeaderAsync_0_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryHeaderAsync( 0 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackRawAsync_1_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_1_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_1_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x1 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_1_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 1 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public async Task TestPackRawHeaderAsync_1_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawHeaderAsync( 1 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackRawHeaderAsync_1_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawHeaderAsync( 1 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public async Task TestPackBinaryHeaderAsync_1_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryHeaderAsync( 1 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC4, 0x1 } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryHeaderAsync_1_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryHeaderAsync( 1 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA1 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackRawAsync_31_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_31_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_31_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x1F } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_31_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 1 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
				Assert.That( 
					packed.Skip( 1 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public async Task TestPackRawHeaderAsync_31_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawHeaderAsync( 31 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackRawHeaderAsync_31_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawHeaderAsync( 31 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public async Task TestPackBinaryHeaderAsync_31_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryHeaderAsync( 31 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC4, 0x1F } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryHeaderAsync_31_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryHeaderAsync( 31 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xBF } ) 
				);
			}
		}

		[Test]
		public async Task TestPackRawAsync_32_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_32_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_32_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_32_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public async Task TestPackRawHeaderAsync_32_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawHeaderAsync( 32 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xD9, 0x20 } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackRawHeaderAsync_32_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawHeaderAsync( 32 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public async Task TestPackBinaryHeaderAsync_32_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryHeaderAsync( 32 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC4, 0x20 } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryHeaderAsync_32_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryHeaderAsync( 32 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0x20 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackRawAsync_255_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xD9, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_255_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_255_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 2 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC4, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_255_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 255 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public async Task TestPackRawHeaderAsync_255_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawHeaderAsync( 255 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xD9, 0xFF } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackRawHeaderAsync_255_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawHeaderAsync( 255 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public async Task TestPackBinaryHeaderAsync_255_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryHeaderAsync( 255 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC4, 0xFF } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryHeaderAsync_255_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryHeaderAsync( 255 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x0, 0xFF } ) 
				);
			}
		}

		[Test]
		public async Task TestPackRawAsync_256_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_256_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_256_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC5, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_256_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 256 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public async Task TestPackRawHeaderAsync_256_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawHeaderAsync( 256 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackRawHeaderAsync_256_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawHeaderAsync( 256 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public async Task TestPackBinaryHeaderAsync_256_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryHeaderAsync( 256 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC5, 0x1, 0x0 } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryHeaderAsync_256_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryHeaderAsync( 256 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0x1, 0x0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackRawAsync_65535_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_65535_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_65535_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC5, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_65535_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 3 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
				Assert.That( 
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65535 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public async Task TestPackRawHeaderAsync_65535_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawHeaderAsync( 65535 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackRawHeaderAsync_65535_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawHeaderAsync( 65535 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public async Task TestPackBinaryHeaderAsync_65535_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryHeaderAsync( 65535 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC5, 0xFF, 0xFF } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryHeaderAsync_65535_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryHeaderAsync( 65535 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDA, 0xFF, 0xFF } ) 
				);
			}
		}

		[Test]
		public async Task TestPackRawAsync_65536_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackRawAsync_65536_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}


		[Test]
		public async Task TestPackBinaryAsync_65536_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xC6, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryAsync_65536_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryAsync( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() as byte[] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed.Take( 5 ).ToArray(), 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
				Assert.That( 
					packed.Skip( 5 ).ToArray(),
					Is.EqualTo( Enumerable.Repeat( ( byte )0xFF, 65536 ).ToArray() )
				);
			}
		}		

#pragma warning disable 0618 
		[Test]
		public async Task TestPackRawHeaderAsync_65536_ByteArray_WithoutCompatibilityOptions_AsStrStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackRawHeaderAsync( 65536 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackRawHeaderAsync_65536_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackRawHeaderAsync( 65536 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
			}
		}
#pragma warning restore 0618 

		[Test]
		public async Task TestPackBinaryHeaderAsync_65536_ByteArray_WithoutCompatibilityOptions_AsBinStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				await target.PackBinaryHeaderAsync( 65536 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC6, 0x0, 0x1, 0x0, 0x0 } ) 
				);
			}
		}
		
		[Test]
		public async Task TestPackBinaryHeaderAsync_65536_ByteArray_WithBinaryAsRawCompatibilityOptions_AsRawStream()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer, PackerCompatibilityOptions.PackBinaryAsRaw ) )
			{
				await target.PackBinaryHeaderAsync( 65536 );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xDB, 0x0, 0x1, 0x0, 0x0 } ) 
				);
			}
		}

		[Test]
		public void TestPackRawHeaderAsync_MinusOne()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
#pragma warning disable 0618
				Assert.Throws<ArgumentOutOfRangeException>( async () => await target.PackRawHeaderAsync( -1 ) );
#pragma warning restore 0618
			}
		}

		[Test]
		public void TestPackStringHeaderAsync_MinusOne()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentOutOfRangeException>( async () => await target.PackStringHeaderAsync( -1 ) );
			}
		}

		[Test]
		public void TestPackBinaryHeaderAsync_MinusOne()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentOutOfRangeException>( async () => await target.PackBinaryHeaderAsync( -1 ) );
			}
		}

		[Test]
		public async Task TestPackRawAsync_IList_Null()
		{
			IList<byte> value = null;
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackRawAsync( value );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackRawAsync_IEnumerable_Null()
		{
			IEnumerable<byte> value = null;
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackRawAsync( value );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackRawAsync_Array_Null()
		{
			byte[] value = null;
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackRawAsync( value );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackRawBodyAsync_IList_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackRawBodyAsync( new List<byte>{ 1, 2, 3 } );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 1, 2, 3 } ) 
				);
			}
		}


		[Test]
		public async Task TestPackRawBodyAsync_IList_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackRawBodyAsync( new List<byte>( 0 ) );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[ 0 ] ) 
				);
			}
		}

		[Test]
		public void TestPackRawBodyAsync_IList_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentNullException>( async () => await target.PackRawBodyAsync( default( IList<byte> ) ) );
			}
		}
		[Test]
		public async Task TestPackRawBodyAsync_IEnumerable_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackRawBodyAsync( Enumerable.Range( 1, 3 ).Select( i => ( byte )i ) );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 1, 2, 3 } ) 
				);
			}
		}


		[Test]
		public async Task TestPackRawBodyAsync_IEnumerable_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackRawBodyAsync( Enumerable.Empty<byte>() );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[ 0 ] ) 
				);
			}
		}

		[Test]
		public void TestPackRawBodyAsync_IEnumerable_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentNullException>( async () => await target.PackRawBodyAsync( default( IEnumerable<byte> ) ) );
			}
		}
		[Test]
		public async Task TestPackRawBodyAsync_Array_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackRawBodyAsync( new byte[] { 1, 2, 3 } );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 1, 2, 3 } ) 
				);
			}
		}


		[Test]
		public async Task TestPackRawBodyAsync_Array_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackRawBodyAsync( new byte[ 0 ] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[ 0 ] ) 
				);
			}
		}

		[Test]
		public void TestPackRawBodyAsync_Array_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentNullException>( async () => await target.PackRawBodyAsync( default( byte[] ) ) );
			}
		}
		[Test]
		public async Task TestPackStringAsync_IEnumerable_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackStringAsync( new char[] { 'A', 'B', 'C' } );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA3, ( byte )'A', ( byte )'B', ( byte )'C' } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_IEnumerable_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackStringAsync( new char[ 0 ] );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_IEnumerable_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackStringAsync( null );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_IEnumerable_WithEncoding_NonNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackStringAsync( new char[] { 'A', 'B', 'C' }, Encoding.Unicode );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA6, ( byte )'A', 0, ( byte )'B', 0, ( byte )'C', 0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_IEnumerable_WithEncoding_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackStringAsync( new char[ 0 ], Encoding.Unicode );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_IEnumerable_WithEncoding_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackStringAsync( null, Encoding.Unicode );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public void TestPackStringAsync_IEnumerable_NullEncoding()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentNullException>( async () => await target.PackStringAsync( new char[] { 'A', 'B', 'C' }, null ) );
			}
		}
		[Test]
		public async Task TestPackStringAsync_String_NotNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackStringAsync( "ABC" );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA3, ( byte )'A', ( byte )'B', ( byte )'C' } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_String_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackStringAsync( String.Empty );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_String_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackStringAsync( null );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_String_WithEncoding_NonNull()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackStringAsync( "ABC", Encoding.Unicode );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA6, ( byte )'A', 0, ( byte )'B', 0, ( byte )'C', 0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_String_WithEncoding_Empty()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackStringAsync( String.Empty, Encoding.Unicode );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xA0 } ) 
				);
			}
		}

		[Test]
		public async Task TestPackStringAsync_String_WithEncoding_Null()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				await target.PackStringAsync( null, Encoding.Unicode );
				var packed = buffer.ToArray();
				Assert.That( 
					packed, 
					Is.EqualTo( new byte[] { 0xC0 } ) 
				);
			}
		}

		[Test]
		public void TestPackStringAsync_String_NullEncoding()
		{
			using ( var buffer = new MemoryStream() )
			using ( var target = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentNullException>( async () => await target.PackStringAsync( "ABC", null ) );
			}
		}
	}
}
