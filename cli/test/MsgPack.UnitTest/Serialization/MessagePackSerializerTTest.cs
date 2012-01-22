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
using System.IO;
using System.Runtime.Serialization;
using NUnit.Framework;

namespace MsgPack.Serialization
{
	[TestFixture]
	public class MessagePackSerializerTTest
	{
		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestPack_StreamIsNull()
		{
			var target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			target.Pack( null, 0 );
		}


		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestPackTo_PackerIsNull()
		{
			var target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			target.PackTo( null, 0 );
		}


		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestIMessagePackSerializerPackTo_PackerIsNull()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			target.PackTo( null, 0 );
		}

		[Test]
		public void TestIMessagePackSerializerPackTo_Valid_Success()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				target.PackTo( packer, 1 );
				Assert.That( buffer.ToArray(), Is.EqualTo( new byte[] { 0x1 } ) );
			}
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestIMessagePackSerializerPackTo_ObjectTreeIsNull_ValueType_AsNil()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				target.PackTo( packer, null );
			}
		}

		[Test]
		public void TestIMessagePackSerializerPackTo_ObjectTreeIsNull_NullableValueType_AsNil()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int?>( new SerializationContext() );
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				target.PackTo( packer, null );
				Assert.That( buffer.ToArray(), Is.EqualTo( new byte[] { 0xC0 } ) );
			}
		}

		[Test]
		public void TestIMessagePackSerializerPackTo_ObjectTreeIsNull_ReferenceType_AsNil()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<string>( new SerializationContext() );
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				target.PackTo( packer, null );
				Assert.That( buffer.ToArray(), Is.EqualTo( new byte[] { 0xC0 } ) );
			}
		}

		[Test]
		[ExpectedException( typeof( ArgumentException ) )]
		public void TestIMessagePackSerializerPackTo_ObjectTreeIsOtherType()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<string>( new SerializationContext() );
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				target.PackTo( packer, Int64.MaxValue );
			}
		}


		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestUnpack_StreamIsNull()
		{
			var target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			target.Unpack( null );
		}


		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestUnpackFrom_UnpackerIsNull()
		{
			var target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			target.UnpackFrom( null );
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestUnpackFrom_StreamIsEmpty()
		{
			var target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[ 0 ] ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				var result = target.UnpackFrom( unpacker );
			}
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestUnpackFrom_StreamIsNullButTypeIsValueType()
		{
			var target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				var result = target.UnpackFrom( unpacker );
			}
		}


		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestIMessagePackSerializerUnpackFrom_UnpackerIsNull()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			target.UnpackFrom( null );
		}

		[Test]
		public void TestIMessagePackSerializerUnpackFrom_Valid_Success()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var result = target.UnpackFrom( unpacker );
				Assert.That( result, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestIMessagePackSerializerUnpackFrom_Invalid()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var result = target.UnpackFrom( unpacker );
			}
		}


		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestUnpackTo_UnpackerIsNull()
		{
			var target = new AutoMessagePackSerializer<int[]>( new SerializationContext() );
			target.UnpackTo( null, new int[ 1 ] );
		}

		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestUnpackTo_CollectionIsNull()
		{
			var target = new AutoMessagePackSerializer<int[]>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				target.UnpackTo( unpacker, null );
			}
		}

		[Test]
		[ExpectedException( typeof( NotSupportedException ) )]
		public void TestUnpackTo_IsNotCollectionType()
		{
			var target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				target.UnpackTo( unpacker, 0 );
			}
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestUnpackTo_StreamContentIsEmpty()
		{
			var target = new AutoMessagePackSerializer<int[]>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[ 0 ] ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var collection = new int[ 1 ];
				target.UnpackTo( unpacker, collection );
			}
		}

		[Test]
		public void TestUnpackTo_StreamContainsNull()
		{
			var target = new AutoMessagePackSerializer<int[]>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var collection = new int[ 0 ];
				target.UnpackTo( unpacker, collection );
			}
		}


		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestIMessagePackSerializerUnpackTo_UnpackerIsNull()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int[]>( new SerializationContext() );
			target.UnpackTo( null, new int[ 1 ] );
		}

		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestIMessagePackSerializerUnpackTo_CollectionIsNull()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				target.UnpackTo( unpacker, null );
			}
		}

		[Test]
		public void TestIMessagePackSerializerUnpackTo_Valid_Success()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int[]>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var collection = new int[ 2 ];
				target.UnpackTo( unpacker, collection );
				// colection[1] is still 0.
				Assert.That( collection, Is.EqualTo( new int[] { 1, 0 } ) );
			}
		}

		[Test]
		[ExpectedException( typeof( ArgumentException ) )]
		public void TestIMessagePackSerializerUnpackTo_CollectionTypeIsInvalid()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int[]>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				var collection = new bool[ 1 ];
				target.UnpackTo( unpacker, collection );
			}
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestIMessagePackSerializerUnpackTo_StreamContentIsInvalid()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int[]>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var collection = new int[ 1 ];
				target.UnpackTo( unpacker, collection );
			}
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestIMessagePackSerializerUnpackTo_StreamContentIsEmpty()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int[]>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[ 0 ] ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var collection = new int[ 1 ];
				target.UnpackTo( unpacker, collection );
			}
		}

		[Test]
		public void TestIMessagePackSerializerUnpackTo_StreamContainsNull()
		{
			IMessagePackSerializer target = new AutoMessagePackSerializer<int[]>( new SerializationContext() );
			using ( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var collection = new int[ 0 ];
				target.UnpackTo( unpacker, collection );
			}
		}
	}
}
