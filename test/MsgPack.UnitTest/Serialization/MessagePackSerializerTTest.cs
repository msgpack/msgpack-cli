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
		// UInt32 consists of only invalid header value.
		private const uint _invalidHeaderValue = 0xC6C6C6C6;

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
			Assert.Throws<ArgumentNullException>( () => target.Pack( default( Stream ), 0 ) );
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

		// byte[] Pack();

		[Test]
		public void TestPack_ReturningByteArray()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var actual = target.Pack( value );
			Assert.That( actual, Is.EqualTo( expected ) );
		}

		// int Pack(byte[], T);

		[Test]
		public void TestPack_FillingByteArray_Exact_Success()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = new byte[ expected.Length ];
			var used = target.Pack( buffer, value );
			Assert.That( buffer, Is.EqualTo( expected ) );
			Assert.That( used, Is.EqualTo( expected.Length ) );
		}

		[Test]
		public void TestPack_FillingByteArray_Longer_RemainingIsPreserved()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = Enumerable.Repeat( ( byte )( 0xC6 ), expected.Length + 1 ).ToArray();
			var used = target.Pack( buffer, value );
			Assert.That( buffer.Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			Assert.That( used, Is.EqualTo( expected.Length ) );
		}

		[Test]
		public void TestPack_FillingByteArray_Shorter_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = new byte[ expected.Length - 1 ];
			Assert.Throws<SerializationException>( () => target.Pack( buffer, value ) );
		}

		[Test]
		public void TestPack_FillingByteArray_Null_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			Assert.Throws<ArgumentNullException>( () => target.Pack( default( byte[] ), value ) );
		}

		// int Pack(byte[], int, T);

		[Test]
		public void TestPack_FillingByteArrayWithOffset_Exact_0_Success()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = new byte[ expected.Length ];
			var used = target.Pack( buffer, 0, value );
			Assert.That( buffer, Is.EqualTo( expected ) );
			Assert.That( used, Is.EqualTo( expected.Length ) );
		}

		[Test]
		public void TestPack_FillingByteArrayWithOffset_Exact_1_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = new byte[ expected.Length ];
			Assert.Throws<SerializationException>( () => target.Pack( buffer, 1, value ) );
		}

		[Test]
		public void TestPack_FillingByteArrayWithOffset_0_Longer_RemainingIsPreserved()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = Enumerable.Repeat( ( byte )( 0xC6 ), expected.Length + 1 ).ToArray();
			var used = target.Pack( buffer, 0, value );
			Assert.That( buffer.Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			Assert.That( used, Is.EqualTo( expected.Length ) );
		}

		[Test]
		public void TestPack_FillingByteArrayWithOffset_1_Longer_RemainingIsPreserved()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = Enumerable.Repeat( ( byte )( 0xC6 ), expected.Length + 1 ).ToArray();
			var used = target.Pack( buffer, 1, value );
			Assert.That( buffer.Skip( 1 ).ToArray(), Is.EqualTo( expected ) );
			Assert.That( used, Is.EqualTo( expected.Length ) );
		}

		[Test]
		public void TestPack_FillingByteArrayWithOffset_Shorter_0_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = new byte[ expected.Length - 1 ];
			Assert.Throws<SerializationException>( () => target.Pack( buffer, 0, value ) );
		}

		[Test]
		public void TestPack_FillingByteArrayWithOffset_Shorter_1_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = new byte[ expected.Length - 1 ];
			Assert.Throws<SerializationException>( () => target.Pack( buffer, 1, value ) );
		}

		[Test]
		public void TestPack_FillingByteArrayWithOffset_Null_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			Assert.Throws<ArgumentNullException>( () => target.Pack( null, 0, value ) );
		}

		[Test]
		public void TestPack_FillingByteArrayWithOffset_NegativeOffset_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			Assert.Throws<ArgumentOutOfRangeException>( () => target.Pack( new byte[ 16 ], -1, value ) );
		}


		// int Pack(ArraySegment<byte>, T);

		[Test]
		public void TestPack_ArraySegment_Exact_0_Success()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = new ArraySegment<byte>( new byte[ expected.Length ] );
			var used = target.Pack( buffer, value );
			Assert.That( buffer, Is.EqualTo( expected ) );
			Assert.That( used.Array, Is.SameAs( buffer.Array ) );
			Assert.That( used.Offset, Is.EqualTo( expected.Length ) );
			Assert.That( used.Count, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestPack_ArraySegment_Exact_1_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = new ArraySegment<byte>( new byte[ expected.Length ], 1, expected.Length - 1 );
			Assert.Throws<SerializationException>( () => target.Pack( buffer, value ) );
		}

		[Test]
		public void TestPack_ArraySegment_Offset0_Longer_RemainingIsPreserved()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = new ArraySegment<byte>( Enumerable.Repeat( ( byte )( 0xC6 ), expected.Length + 1 ).ToArray() );
			var used = target.Pack( buffer, value );
			Assert.That( buffer.Array.Take( expected.Length ).ToArray(), Is.EqualTo( expected ) );
			Assert.That( used.Offset, Is.EqualTo( expected.Length ) );
			Assert.That( used.Count, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestPack_ArraySegment_Offset1_Longer_RemainingIsPreserved()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = new ArraySegment<byte>( Enumerable.Repeat( ( byte )( 0xC6 ), expected.Length + 1 ).ToArray(), 1, expected.Length );
			var used = target.Pack( buffer, value );
			Assert.That( buffer.Array.Skip( 1 ).ToArray(), Is.EqualTo( expected ) );
			Assert.That( used.Offset, Is.EqualTo( expected.Length + 1 ) );
			Assert.That( used.Count, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestPack_ArraySegment_Shorter_Offset0_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = new ArraySegment<byte>( new byte[ expected.Length - 1 ] );
			Assert.Throws<SerializationException>( () => target.Pack( buffer, value ) );
		}

		[Test]
		public void TestPack_ArraySegment_Shorter_Offset1_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var buffer = new ArraySegment<byte>( new byte[ expected.Length - 1 ], 1, expected.Length - 2 );
			Assert.Throws<SerializationException>( () => target.Pack( buffer, value ) );
		}

		[Test]
		public void TestPack_ArraySegment_Default_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			Assert.Throws<ArgumentException>( () => target.Pack( default( ArraySegment<byte> ), value ) );
		}

		// T Unpack(byte[], ref int)

		[Test]
		public void TestUnpack_ByteArrayWithOffset_Exact_0_Success()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			var offset = 0;
			var actual = target.Unpack( input, ref offset );
			Assert.That( actual, Is.EqualTo( expected ) );
			Assert.That( offset, Is.EqualTo( input.Length ) );
		}

		[Test]
		public void TestUnpack_ByteArrayWithOffset_Extra_0_NotAMatter()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			var offset = 0;
			var actual = target.Unpack( input.Concat( new byte[] { 0x1 } ).ToArray(), ref offset );
			Assert.That( actual, Is.EqualTo( expected ) );
			Assert.That( offset, Is.EqualTo( input.Length ) );
		}

		[Test]
		public void TestUnpack_ByteArrayWithOffset_Extra_1_Success()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			var offset = 1;
			var actual = target.Unpack( new byte[] { 0x1 }.Concat( input ).ToArray(), ref offset );
			Assert.That( actual, Is.EqualTo( expected ) );
			Assert.That( offset, Is.EqualTo( input.Length + 1 ) );
		}

		[Test]
		public void TestUnpack_ByteArrayWithOffset_Shorten_0_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			var offset = 0;
			Assert.Throws<InvalidMessagePackStreamException>( () => target.Unpack( input.Take( input.Length - 1 ).ToArray(), ref offset ) );
		}

		[Test]
		public void TestUnpack_ByteArrayWithOffset_Exact_1_Fail()
		{
			var target = CreateTarget<UInt32>();
			var expected = _invalidHeaderValue;
			var input = GetBytes( target, expected );
			var offset = 1;
			Assert.Throws<MessageTypeException>( () => target.Unpack( input, ref offset ) );
		}

		[Test]
		public void TestUnpack_ByteArrayWithOffset_Null_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var offset = 0;
			Assert.Throws<ArgumentNullException>( () => target.Unpack( default( byte[] ), ref offset ) );
		}

		[Test]
		public void TestUnpack_ByteArrayWithOffset_Negative_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			var offset = -1;
			Assert.Throws<ArgumentOutOfRangeException>( () => target.Unpack( input, ref offset ) );
		}

		// T Unpack(ref ArraySegment<byte>)

		[Test]
		public void TestUnpack_ArraySegment_Exact_0_Success()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			var buffer = new ArraySegment<byte>( input );
			var actual = target.Unpack( ref buffer );
			Assert.That( actual, Is.EqualTo( expected ) );
			Assert.That( buffer.Array, Is.SameAs( input ) );
			Assert.That( buffer.Offset, Is.EqualTo( input.Length ) );
			Assert.That( buffer.Count, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestUnpack_ArraySegment_Extra_Offset0_NotAMatter()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			var buffer = new ArraySegment<byte>( input.Concat( new byte[] { 0x1 } ).ToArray(), 0, input.Length + 1 );
			var actual = target.Unpack( ref buffer );
			Assert.That( actual, Is.EqualTo( expected ) );
			Assert.That( buffer.Offset, Is.EqualTo( input.Length ) );
			Assert.That( buffer.Count, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestUnpack_ArraySegment_Extra_Offset1_Success()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			var buffer = new ArraySegment<byte>( new byte[] { 0x1 }.Concat( input ).ToArray(), 1, input.Length );
			var actual = target.Unpack( ref  buffer );
			Assert.That( actual, Is.EqualTo( expected ) );
			Assert.That( buffer.Offset, Is.EqualTo( input.Length + 1 ) );
			Assert.That( buffer.Count, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestUnpack_ArraySegment_Shorten_Offset0_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			var buffer = new ArraySegment<byte>( input, 0, input.Length - 1 );
			Assert.Throws<InvalidMessagePackStreamException>( () => target.Unpack( ref buffer ) );
		}

		[Test]
		public void TestUnpack_ArraySegment_Exact_Offset1_Fail()
		{
			var target = CreateTarget<UInt32>();
			var expected = _invalidHeaderValue;
			var input = GetBytes( target, expected );
			var buffer = new ArraySegment<byte>( input, 1, input.Length - 1 );
			Assert.Throws<MessageTypeException>( () => target.Unpack( ref buffer ) );
		}

		[Test]
		public void TestUnpack_ArraySegment_Default_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var buffer = default( ArraySegment<byte> );
			Assert.Throws<ArgumentException>( () => target.Unpack( ref buffer ) );
		}

		private static byte[] GetBytes<T>( MessagePackSerializer<T> target, T value )
		{
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				return buffer.ToArray();
			}
		}
	}
}
