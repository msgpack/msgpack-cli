#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
	partial class PackerTest_Pack
	{
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length0()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 0 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 3 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC7, 0x0, 0x1 } )
				);
				Assert.That(
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length0_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 0 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length0()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 0 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 3 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC7, 0x0, 0x1 } )
				);
				Assert.That(
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length0_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 0 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length1()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 1 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 2 ).ToArray(),
					Is.EqualTo( new byte[] { 0xD4, 0x1 } )
				);
				Assert.That(
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length1_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 1 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length1()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 1 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 2 ).ToArray(),
					Is.EqualTo( new byte[] { 0xD4, 0x1 } )
				);
				Assert.That(
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length1_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 1 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length2()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 2 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 2 ).ToArray(),
					Is.EqualTo( new byte[] { 0xD5, 0x1 } )
				);
				Assert.That(
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length2_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 2 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length2()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 2 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 2 ).ToArray(),
					Is.EqualTo( new byte[] { 0xD5, 0x1 } )
				);
				Assert.That(
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length2_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 2 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length3()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 3 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 3 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC7, 0x3, 0x1 } )
				);
				Assert.That(
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length3_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 3 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length3()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 3 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 3 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC7, 0x3, 0x1 } )
				);
				Assert.That(
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length3_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 3 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length4()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 4 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 2 ).ToArray(),
					Is.EqualTo( new byte[] { 0xD6, 0x1 } )
				);
				Assert.That(
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length4_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 4 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length4()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 4 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 2 ).ToArray(),
					Is.EqualTo( new byte[] { 0xD6, 0x1 } )
				);
				Assert.That(
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length4_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 4 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length5()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 5 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 3 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC7, 0x5, 0x1 } )
				);
				Assert.That(
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length5_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 5 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length5()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 5 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 3 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC7, 0x5, 0x1 } )
				);
				Assert.That(
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length5_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 5 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length8()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 8 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 2 ).ToArray(),
					Is.EqualTo( new byte[] { 0xD7, 0x1 } )
				);
				Assert.That(
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length8_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 8 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length8()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 8 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 2 ).ToArray(),
					Is.EqualTo( new byte[] { 0xD7, 0x1 } )
				);
				Assert.That(
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length8_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 8 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length9()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 9 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 3 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC7, 0x9, 0x1 } )
				);
				Assert.That(
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length9_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 9 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length9()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 9 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 3 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC7, 0x9, 0x1 } )
				);
				Assert.That(
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length9_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 9 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length16()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 16 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 2 ).ToArray(),
					Is.EqualTo( new byte[] { 0xD8, 0x1 } )
				);
				Assert.That(
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length16_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 16 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length16()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 16 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 2 ).ToArray(),
					Is.EqualTo( new byte[] { 0xD8, 0x1 } )
				);
				Assert.That(
					packed.Skip( 2 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length16_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 16 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length17()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 17 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 3 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC7, 0x11, 0x1 } )
				);
				Assert.That(
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length17_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 17 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length17()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 17 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 3 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC7, 0x11, 0x1 } )
				);
				Assert.That(
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length17_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 17 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length255()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 255 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 3 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC7, 0xFF, 0x1 } )
				);
				Assert.That(
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length255_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 255 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length255()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 255 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 3 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC7, 0xFF, 0x1 } )
				);
				Assert.That(
					packed.Skip( 3 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length255_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 255 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length256()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 256 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 4 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC8, 0x1, 0x0, 0x1 } )
				);
				Assert.That(
					packed.Skip( 4 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length256_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 256 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length256()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 256 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 4 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC8, 0x1, 0x0, 0x1 } )
				);
				Assert.That(
					packed.Skip( 4 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length256_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 256 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length65535()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 65535 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 4 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC8, 0xFF, 0xFF, 0x1 } )
				);
				Assert.That(
					packed.Skip( 4 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length65535_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 65535 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length65535()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 65535 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 4 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC8, 0xFF, 0xFF, 0x1 } )
				);
				Assert.That(
					packed.Skip( 4 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length65535_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 65535 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length65536()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 65536 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 6 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC9, 0x0, 0x1, 0x0, 0x0, 0x1 } )
				);
				Assert.That(
					packed.Skip( 6 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Length65536_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 65536 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( new MessagePackExtendedTypeObject( 1, body ) ) );
			}
		}

		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length65536()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None ) )
			{
				var body = Enumerable.Range( 1, 65536 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				packer.PackExtendedTypeValue( 1, body );
				var packed = buffer.ToArray();
				Assert.That(
					packed.Take( 6 ).ToArray(),
					Is.EqualTo( new byte[] { 0xC9, 0x0, 0x1, 0x0, 0x0, 0x1 } )
				);
				Assert.That(
					packed.Skip( 6 ).ToArray(),
					Is.EqualTo( body )
				);
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Length65536_WithCompatibilityOption()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer, PackerCompatibilityOptions.ProhibitExtendedTypeObjects ) )
			{
				var body = Enumerable.Range( 1, 65536 ).Select( i => ( byte )( i % Byte.MaxValue ) ).ToArray();
				Assert.Throws<InvalidOperationException>( () => packer.PackExtendedTypeValue( 1, body ) );
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Primitives_Null_ByteArray()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentNullException>( () => packer.PackExtendedTypeValue( 1, null ) );
			}
		}
		
		[Test]
		public void TestPack_ExtendedTypeObject_Object_Invalid()
		{
			using( var buffer = new MemoryStream() )
			using( var packer = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentException>( () => packer.PackExtendedTypeValue( default( MessagePackExtendedTypeObject ) ) );
			}
		}	
	}
}