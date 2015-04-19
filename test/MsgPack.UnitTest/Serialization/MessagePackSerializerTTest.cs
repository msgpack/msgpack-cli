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
using System.Runtime.Serialization;
using MsgPack.Serialization.DefaultSerializers;
#if !XAMIOS && !XAMDROID && !UNITY
#if !NETFX_CORE && !SILVERLIGHT
using MsgPack.Serialization.EmittingSerializers;
#else
using MsgPack.Serialization.ExpressionSerializers;
#endif // !NETFX_CORE && !WINDOWS_PHONE
#endif // !XAMIOS && !XAMDROID && !UNITY
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
using IgnoreAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.IgnoreAttribute;
#endif

namespace MsgPack.Serialization
{
	[TestFixture]
	public class MessagePackSerializerTTest
	{
		private static MessagePackSerializer<T> CreateTarget<T>()
		{
#if XAMIOS || XAMDROID || UNITY_ANDROID || UNITY_IPHONE
			return PreGeneratedSerializerActivator.CreateContext( SerializationMethod.Array, SerializationContext.Default.CompatibilityOptions.PackerCompatibilityOptions ).GetSerializer<T>();
#elif !NETFX_CORE && !SILVERLIGHT
			return new SerializationContext { EmitterFlavor = EmitterFlavor.FieldBased }.GetSerializer<T>();
#else
			return new SerializationContext { EmitterFlavor = EmitterFlavor.ExpressionBased }.GetSerializer<T>();
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

		// TestUnpackTo_StreamContentIsEmpty has been no effect.

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
				Assert.That( collection, Is.EqualTo( new[] { 1, 0 } ) );
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


		// TestIMessagePackSerializerUnpackTo_StreamContentIsEmpty
		// It has been no effect.

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

		// byte[] PackSingleObject(T);

		[Test]
		public void TestPackSingleObject_Normal_Success()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			var actual = target.PackSingleObject( value );
			Assert.That( actual, Is.EqualTo( expected ) );
		}

		// T UnpackSingleObject(byte[])

		[Test]
		public void TestUnpackSingleObject_Success()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			var actual = target.UnpackSingleObject( input );
			Assert.That( actual, Is.EqualTo( expected ) );
		}

		[Test]
		public void TestUnpackSingleObject_HasExtra_NotAMatter()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			var actual = target.UnpackSingleObject( input.Concat( new byte[] { 0x1 } ).ToArray() );
			Assert.That( actual, Is.EqualTo( expected ) );
		}

		[Test]
		public void TestUnpackSingleObject_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			Assert.Throws<InvalidMessagePackStreamException>( () => target.UnpackSingleObject( input.Take( input.Length - 1 ).ToArray() ) );
		}

		[Test]
		public void TestUnpackSingleObject_Null_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			Assert.Throws<ArgumentNullException>( () => target.UnpackSingleObject( default( byte[] ) ) );
		}


		// byte[] IMessagePackSingleObjectSerializer.PackSingleObject(object);

		[Test]
		public void TestIMessagePackSingleObjectSerializer_PackSingleObject_Normal_Success()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			var expected = GetBytes( target, value );
			IMessagePackSingleObjectSerializer iface = target;
			var actual = iface.PackSingleObject( value );
			Assert.That( actual, Is.EqualTo( expected ) );
		}

		[Test]
		public void TestIMessagePackSingleObjectSerializer_PackSingleObject_InvalidType_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			var value = TimeSpan.FromTicks( 12345 );
			IMessagePackSingleObjectSerializer iface = target;
			Assert.Throws<ArgumentException>( () => iface.PackSingleObject( value.ToString() ) );
		}

		[Test]
		public void TestIMessagePackSingleObjectSerializer_PackSingleObject_ValueTypeButNull_Fail()
		{
			var target = CreateTarget<TimeSpan>();
			IMessagePackSingleObjectSerializer iface = target;
			Assert.Throws<ArgumentException>( () => iface.PackSingleObject( null ) );
		}

		[Test]
		public void TestIMessagePackSingleObjectSerializer_PackSingleObject_ReferenceTypeNull_AsNil()
		{
			var target = CreateTarget<string>();
			IMessagePackSingleObjectSerializer iface = target;
			var result = iface.PackSingleObject( null );
			Assert.That( result, Is.EqualTo( new byte[] { 0xC0 } ) );// nil
		}

		// object IMessagePackSingleObjectSerializer.UnpackSingleObject(byte[])

		[Test]
		public void TestIMessagePackSingleObjectSerializer_UnpackSingleObject_Success()
		{
			var target = CreateTarget<TimeSpan>();
			var expected = TimeSpan.FromTicks( 12345 );
			var input = GetBytes( target, expected );
			var actual = target.UnpackSingleObject( input );
			Assert.That( actual, Is.EqualTo( expected ) );
		}

		private static byte[] GetBytes<T>( MessagePackSerializer<T> target, T value )
		{
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				return buffer.ToArray();
			}
		}


		[Test]
		public void TestIssue10_Null_ReadString()
		{
			TestIssue10_ReadXxxCore( new Inner { A = null, Bytes = null } );
		}

		[Test]
		public void TestIssue10_Empty_ReadString()
		{
			TestIssue10_ReadXxxCore( new Inner { A = String.Empty, Bytes = Binary.Empty } );
		}

		private void TestIssue10_ReadXxxCore( Inner inner )
		{
			var serializer = CreateTarget<Outer>();
			var outer = new Outer();
			outer.Inner = inner;
			var bytes = serializer.PackSingleObject( outer );
			var result = serializer.UnpackSingleObject( bytes );
			Assert.That( result.A, Is.EqualTo( outer.A ) );
			Assert.That( result.O, Is.EqualTo( outer.O ) );
			Assert.That( result.Inner, Is.Not.Null );
			Assert.That( result.Inner.A, Is.EqualTo( outer.Inner.A ) );
			Assert.That( result.Inner.Bytes, Is.EqualTo( outer.Inner.Bytes ) );
			Assert.That( result.Inner.C, Is.EqualTo( outer.Inner.C ) );
		}

		[Test]
		public void TestIssue10_Null_Reader()
		{
			TestIssue10_Reader( new Inner { A = null, Bytes = null } );
		}

		[Test]
		public void TestIssue10_Empty_Reader()
		{
			TestIssue10_Reader( new Inner { A = String.Empty, Bytes = Binary.Empty } );

		}

		[Test]
		public void TestIssue13_StringListMapAsMpoDictionary()
		{
			var target = MessagePackSerializer.Get<Dictionary<MessagePackObject, MessagePackObject>>( SerializationContext.Default, PolymorphismSchema.Default );
			using ( var buffer = new MemoryStream( Convert.FromBase64String( "gadyZXN1bHRzkss/8AAAAAAAAMtAAAAAAAAAAA==" ) ) )
			{
				var result = target.Unpack( buffer );
				Assert.That( result.Count, Is.EqualTo( 1 ) );
				Assert.That( result.First().Key == "results", "{0}.Key != results", result.First().Key );
				Assert.That( result.First().Value.IsList, "{0}.Value is not list", result.First().Value.UnderlyingType );
			}
		}

		[Test]
		public void TestIssue13_ListAsMpo()
		{
			var target = new MsgPack_MessagePackObjectMessagePackSerializer( new SerializationContext( PackerCompatibilityOptions.Classic ) );
			using ( var buffer = new MemoryStream( new byte[] { 0x91, 2 } ) )
			{
				var result = target.Unpack( buffer );
				Assert.That( result.IsList, "{0} is not list", result.UnderlyingType );
				Assert.That( result.AsList().Count, Is.EqualTo( 1 ) );
				Assert.That( result.AsList().First() == 2, "{0}[0] != 2", result );
			}
		}

		[Test]
		public void TestIssue13_MapAsMpo()
		{
			var target = new MsgPack_MessagePackObjectMessagePackSerializer( new SerializationContext( PackerCompatibilityOptions.Classic ) );
			using ( var buffer = new MemoryStream( new byte[] { 0x81, 2, 3 } ) )
			{
				var result = target.Unpack( buffer );
				Assert.That( result.IsDictionary, "{0} is not dictionary", result.UnderlyingType );
				Assert.That( result.AsDictionary().Count, Is.EqualTo( 1 ) );
				Assert.That( result.AsDictionary().First().Key == 2, "{0}.First().Key != 2", result );
				Assert.That( result.AsDictionary().First().Value == 3, "{0}.First().Value != 3", result );
			}
		}

		[Test]
		[Ignore] // Ignore until PR-30 is completed.
		public void TestIssue28()
		{
			var target = CreateTarget<WithReadOnlyProperty>();
			using ( var buffer = new MemoryStream() )
			{
				var value = new WithReadOnlyProperty { Number = 123 };
				target.Pack( buffer, value );
				buffer.Position = 0;
				var result = target.Unpack( buffer );
				Assert.That( value.Number, Is.EqualTo( result.Number ) );
			}
		}

		[Test]
		public void TestIssue41()
		{
			using ( var buffer = new MemoryStream( new byte[] { 0x84, 0x01, 0x81, 0x0a, 0x14, 0x02, 0x93, 0x14, 0x1e, 0x28, 0x03, MessagePackCode.NilValue, 0x04, 0x0 } ) )
			//using ( var unpacker = Unpacker.Create( buffer ) )
			{
				//unpacker.Read();
				var serializer = MessagePackSerializer.Get<MessagePackObjectDictionary>();
				//var result = serializer.UnpackFrom( unpacker );
				var result = serializer.Unpack( buffer );
				Assert.That( result.Count, Is.EqualTo( 4 ) );
				Assert.That( result[ 1 ].AsDictionary().Count, Is.EqualTo( 1 ) );
				Assert.That( result[ 1 ].AsDictionary()[ 10 ], Is.EqualTo( ( MessagePackObject )0x14 ) );
				Assert.That( result[ 2 ].AsList().Count, Is.EqualTo( 3 ) );
				Assert.That( result[ 2 ].AsList()[ 0 ], Is.EqualTo( ( MessagePackObject )0x14 ) );
				Assert.That( result[ 2 ].AsList()[ 1 ], Is.EqualTo( ( MessagePackObject )0x1E ) );
				Assert.That( result[ 2 ].AsList()[ 2 ], Is.EqualTo( ( MessagePackObject )0x28 ) );
				Assert.That( result[ 3 ].IsNil );
				Assert.That( result[ 4 ], Is.EqualTo( ( MessagePackObject )0x0 ) );
			}

		}

		private void TestIssue10_Reader( Inner inner )
		{
			var serializer = CreateTarget<Outer>();
			var outer = new Outer();
			outer.Inner = inner;
			var bytes = serializer.PackSingleObject( outer );
			using ( var buffer = new MemoryStream( bytes ) )
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				Action<Unpacker, MessagePackObject> assertion =
					( u, o ) =>
					{
						Assert.That( u.Read(), Is.True );
						Assert.That( u.LastReadData == o, "{0} == {1}", u.LastReadData, o );
					};

				assertion( unpacker, 3 );
				Assert.That( unpacker.IsArrayHeader );

				assertion( unpacker, outer.A );
				assertion( unpacker, 3 );
				Assert.That( unpacker.IsArrayHeader );

				using ( var subtreeUnpacker = unpacker.ReadSubtree() )
				{
					assertion( subtreeUnpacker, outer.Inner.A );
					assertion( subtreeUnpacker, outer.Inner.Bytes );
					assertion( subtreeUnpacker, outer.Inner.C );
				}

				assertion( unpacker, outer.O );
			}
		}
	}

	public class Outer
	{
		public string A = "A";
		public Inner Inner = new Inner();
		public string O = "O";
	}

	public class Inner
	{
		public string A = null;
		public byte[] Bytes = null;
		public string C = "C";
	}

	public class WithReadOnlyProperty
	{
		public int Number { get; set; }
		public string AsString { get { return this.Number.ToString(); } }
	}
}
