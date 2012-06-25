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
#if !NETFX_CORE
using MsgPack.Serialization.EmittingSerializers;
#else
using MsgPack.Serialization.ExpressionSerializers;
#endif
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack.Serialization
{
	[TestFixture]
	public class MessagePackSerializerTTest
	{
		private static MessagePackSerializer<T> CreateTarget<T>()
		{
#if !NETFX_CORE
			return new AutoMessagePackSerializer<T>( new SerializationContext(), c => new MapEmittingSerializerBuilder<T>( c ) );
#else
			return new AutoMessagePackSerializer<T>( new SerializationContext(), c => new ExpressionSerializerBuilder<T>( c ) );
#endif
		}

		[Test]
		public void TestPack_StreamIsNull()
		{
			var target = CreateTarget<int>();
			Assert.Throws<ArgumentNullException>( () => target.Pack( null, 0 ) );
		}


		[Test]
		public void TestPackTo_PackerIsNull()
		{
			var target = CreateTarget<int>();
			Assert.Throws<ArgumentNullException>( () => target.PackTo( null, 0 ) );
		}


		[Test]
		public void TestIMessagePackSerializerPackTo_PackerIsNull()
		{
			IMessagePackSerializer target = CreateTarget<int>();
			Assert.Throws<ArgumentNullException>( () => target.PackTo( null, 0 ) );
		}

		[Test]
		public void TestIMessagePackSerializerPackTo_Valid_Success()
		{
			IMessagePackSerializer target = CreateTarget<int>();
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				target.PackTo( packer, 1 );
				Assert.That( buffer.ToArray(), Is.EqualTo( new byte[] { 0x1 } ) );
			}
		}

		[Test]
		public void TestIMessagePackSerializerPackTo_ObjectTreeIsNull_ValueType_AsNil()
		{
			IMessagePackSerializer target = CreateTarget<int>();
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				Assert.Throws<SerializationException>( () => target.PackTo( packer, null ) );
			}
		}

		[Test]
		public void TestIMessagePackSerializerPackTo_ObjectTreeIsNull_NullableValueType_AsNil()
		{
			IMessagePackSerializer target = CreateTarget<int?>();
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
			IMessagePackSerializer target = CreateTarget<string>();
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				target.PackTo( packer, null );
				Assert.That( buffer.ToArray(), Is.EqualTo( new byte[] { 0xC0 } ) );
			}
		}

		[Test]
		public void TestIMessagePackSerializerPackTo_ObjectTreeIsOtherType()
		{
			IMessagePackSerializer target = CreateTarget<string>();
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				Assert.Throws<ArgumentException>( () => target.PackTo( packer, Int64.MaxValue ) );
			}
		}


		[Test]
		public void TestUnpack_StreamIsNull()
		{
			var target = CreateTarget<int>();
			Assert.Throws<ArgumentNullException>( () => target.Unpack( null ) );
		}


		[Test]
		public void TestUnpackFrom_UnpackerIsNull()
		{
			var target = CreateTarget<int>();
			Assert.Throws<ArgumentNullException>( () => target.UnpackFrom( null ) );
		}

		[Test]
		public void TestUnpackFrom_StreamIsEmpty()
		{
			var target = CreateTarget<int>();
			using ( var buffer = new MemoryStream( new byte[ 0 ] ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<SerializationException>( () => target.UnpackFrom( unpacker ) );
			}
		}

		[Test]
		public void TestUnpackFrom_StreamIsNullButTypeIsValueType()
		{
			var target = CreateTarget<int>();
			using ( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.Throws<SerializationException>( () => target.UnpackFrom( unpacker ) );
			}
		}


		[Test]
		public void TestIMessagePackSerializerUnpackFrom_UnpackerIsNull()
		{
			IMessagePackSerializer target = CreateTarget<int>();
			Assert.Throws<ArgumentNullException>( () => target.UnpackFrom( null ) );
		}

		[Test]
		public void TestIMessagePackSerializerUnpackFrom_Valid_Success()
		{
			IMessagePackSerializer target = CreateTarget<int>();
			using ( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var result = target.UnpackFrom( unpacker );
				Assert.That( result, Is.EqualTo( 1 ) );
			}
		}

		[Test]
		public void TestIMessagePackSerializerUnpackFrom_Invalid()
		{
			IMessagePackSerializer target = CreateTarget<int>();
			using ( var buffer = new MemoryStream( new byte[] { 0xC2 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				Assert.Throws<SerializationException>( () => target.UnpackFrom( unpacker ) );
			}
		}


		[Test]
		public void TestUnpackTo_UnpackerIsNull()
		{
			var target = CreateTarget<int[]>();
			Assert.Throws<ArgumentNullException>( () => target.UnpackTo( null, new int[ 1 ] ) );
		}

		[Test]
		public void TestUnpackTo_CollectionIsNull()
		{
			var target = CreateTarget<int[]>();
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				Assert.Throws<ArgumentNullException>( () => target.UnpackTo( unpacker, null ) );
			}
		}

		[Test]
		public void TestUnpackTo_IsNotCollectionType()
		{
			var target = CreateTarget<int>();
			using ( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				Assert.Throws<NotSupportedException>( () => target.UnpackTo( unpacker, 0 ) );
			}
		}

		[Test]
		public void TestUnpackTo_StreamContentIsEmpty()
		{
			var target = CreateTarget<int[]>();
			using ( var buffer = new MemoryStream( new byte[ 0 ] ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var collection = new int[ 1 ];
				Assert.Throws<SerializationException>( () => target.UnpackTo( unpacker, collection ) );
			}
		}

		[Test]
		public void TestUnpackTo_StreamContainsNull()
		{
			var target = CreateTarget<int[]>();
			using ( var buffer = new MemoryStream( new byte[] { 0xC0 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var collection = new int[ 0 ];
				target.UnpackTo( unpacker, collection );
			}
		}


		[Test]
		public void TestIMessagePackSerializerUnpackTo_UnpackerIsNull()
		{
			IMessagePackSerializer target = CreateTarget<int[]>();
			Assert.Throws<ArgumentNullException>( () => target.UnpackTo( null, new int[ 1 ] ) );
		}

		[Test]
		public void TestIMessagePackSerializerUnpackTo_CollectionIsNull()
		{
			IMessagePackSerializer target = CreateTarget<int[]>();
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				Assert.Throws<ArgumentNullException>( () => target.UnpackTo( unpacker, null ) );
			}
		}

		[Test]
		public void TestIMessagePackSerializerUnpackTo_Valid_Success()
		{
			IMessagePackSerializer target = CreateTarget<int[]>();
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
		public void TestIMessagePackSerializerUnpackTo_CollectionTypeIsInvalid()
		{
			IMessagePackSerializer target = CreateTarget<int[]>();
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				var collection = new bool[ 1 ];
				Assert.Throws<ArgumentException>( () => target.UnpackTo( unpacker, collection ) );
			}
		}

		[Test]
		public void TestIMessagePackSerializerUnpackTo_StreamContentIsInvalid()
		{
			IMessagePackSerializer target = CreateTarget<int[]>();
			using ( var buffer = new MemoryStream( new byte[] { 0x1 } ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var collection = new int[ 1 ];
				Assert.Throws<SerializationException>( () => target.UnpackTo( unpacker, collection ) );
			}
		}

		[Test]
		public void TestIMessagePackSerializerUnpackTo_StreamContentIsEmpty()
		{
			IMessagePackSerializer target = CreateTarget<int[]>();
			using ( var buffer = new MemoryStream( new byte[ 0 ] ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				unpacker.Read();
				var collection = new int[ 1 ];
				Assert.Throws<SerializationException>( () => target.UnpackTo( unpacker, collection ) );
			}
		}

		[Test]
		public void TestIMessagePackSerializerUnpackTo_StreamContainsNull()
		{
			IMessagePackSerializer target = CreateTarget<int[]>();
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
