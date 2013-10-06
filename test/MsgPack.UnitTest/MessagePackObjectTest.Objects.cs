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
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif
using System.Linq;

namespace MsgPack
{
	[TestFixture]
	public class MessagePackObjectTest_Object
	{
		[Test]
		public void TestFromObject_Zero_Success()
		{
			TestFromObjectCore( 0, 0 );
		}

		[Test]
		public void TestFromObject_Null_Success()
		{
			TestFromObjectCore( default( object ), MessagePackObject.Nil );
		}

		[Test]
		public void TestFromObject_True_Success()
		{
			TestFromObjectCore( true, true );
		}

		[Test]
		public void TestFromObject_False_Success()
		{
			TestFromObjectCore( false, false );
		}

		[Test]
		public void TestFromObject_TinyPositiveInteger_Success()
		{
			TestFromObjectCore( 1, 1 );
		}

		[Test]
		public void TestFromObject_TinyNegativeIngeter_Success()
		{
			TestFromObjectCore( -1, -1 );
		}

		[Test]
		public void TestFromObject_MessagePackObject_Success()
		{
			TestFromObjectCore( new MessagePackObject( 1 ), new MessagePackObject( 1 ) );
		}

		[Test]
		public void TestFromObject_Int8_Success()
		{
			TestFromObjectCore( SByte.MinValue, SByte.MinValue );
		}

		[Test]
		public void TestFromObject_Int16_Success()
		{
			TestFromObjectCore( Int16.MinValue, Int16.MinValue );
		}

		[Test]
		public void TestFromObject_Int32_Success()
		{
			TestFromObjectCore( Int32.MinValue, Int32.MinValue );
		}

		[Test]
		public void TestFromObject_Int64_Success()
		{
			TestFromObjectCore( Int64.MinValue, Int64.MinValue );
		}

		[Test]
		public void TestFromObject_UInt8_Success()
		{
			TestFromObjectCore( Byte.MaxValue, Byte.MaxValue );
		}

		[Test]
		public void TestFromObject_UInt16_Success()
		{
			TestFromObjectCore( UInt16.MaxValue, UInt16.MaxValue );
		}

		[Test]
		public void TestFromObject_UInt32_Success()
		{
			TestFromObjectCore( UInt32.MaxValue, UInt32.MaxValue );
		}

		[Test]
		public void TestFromObject_UInt64_Success()
		{
			TestFromObjectCore( UInt64.MinValue, UInt64.MinValue );
		}

		[Test]
		public void TestFromObject_Single_Success()
		{
			TestFromObjectCore( Single.Epsilon, Single.Epsilon );
		}

		[Test]
		public void TestFromObject_Double_Success()
		{
			TestFromObjectCore( Double.Epsilon, Double.Epsilon );
		}

		private static Nullable<T> AsNullable<T>( T value )
			where T : struct
		{
			return new Nullable<T>( value );
		}

		[Test]
		public void TestFromObject_NullableInt8NotNull_Success()
		{
			TestFromObjectCore( AsNullable( SByte.MinValue ), SByte.MinValue );
		}

		[Test]
		public void TestFromObject_NullableInt16NotNull_Success()
		{
			TestFromObjectCore( AsNullable( Int16.MinValue ), Int16.MinValue );
		}

		[Test]
		public void TestFromObject_NullableInt32NotNull_Success()
		{
			TestFromObjectCore( AsNullable( Int32.MinValue ), Int32.MinValue );
		}

		[Test]
		public void TestFromObject_NullableInt64NotNull_Success()
		{
			TestFromObjectCore( AsNullable( Int64.MinValue ), Int64.MinValue );
		}

		[Test]
		public void TestFromObject_NullableUInt8NotNull_Success()
		{
			TestFromObjectCore( AsNullable( Byte.MaxValue ), Byte.MaxValue );
		}

		[Test]
		public void TestFromObject_NullableUInt16NotNull_Success()
		{
			TestFromObjectCore( AsNullable( UInt16.MaxValue ), UInt16.MaxValue );
		}

		[Test]
		public void TestFromObject_NullableUInt32NotNull_Success()
		{
			TestFromObjectCore( AsNullable( UInt32.MaxValue ), UInt32.MaxValue );
		}

		[Test]
		public void TestFromObject_NullableUInt64NotNull_Success()
		{
			TestFromObjectCore( AsNullable( UInt64.MinValue ), UInt64.MinValue );
		}

		[Test]
		public void TestFromObject_NullableSingleNotNull_Success()
		{
			TestFromObjectCore( AsNullable( Single.Epsilon ), Single.Epsilon );
		}

		[Test]
		public void TestFromObject_NullableDoubleNotNull_Success()
		{
			TestFromObjectCore( AsNullable( Double.Epsilon ), Double.Epsilon );
		}

		[Test]
		public void TestFromObject_NullableInt8Null_IsNil()
		{
			TestFromObjectCore( default( sbyte? ), MessagePackObject.Nil );
		}

		[Test]
		public void TestFromObject_NullableInt16Null_IsNil()
		{
			TestFromObjectCore( default( short? ), MessagePackObject.Nil );
		}

		[Test]
		public void TestFromObject_NullableInt32Null_IsNil()
		{
			TestFromObjectCore( default( int? ), MessagePackObject.Nil );
		}

		[Test]
		public void TestFromObject_NullableInt64Null_IsNil()
		{
			TestFromObjectCore( default( long? ), MessagePackObject.Nil );
		}

		[Test]
		public void TestFromObject_NullableUInt8Null_IsNil()
		{
			TestFromObjectCore( default( byte? ), MessagePackObject.Nil );
		}

		[Test]
		public void TestFromObject_NullableUInt16Null_IsNil()
		{
			TestFromObjectCore( default( ushort? ), MessagePackObject.Nil );
		}

		[Test]
		public void TestFromObject_NullableUInt32Null_IsNil()
		{
			TestFromObjectCore( default( uint? ), MessagePackObject.Nil );
		}

		[Test]
		public void TestFromObject_NullableUInt64Null_IsNil()
		{
			TestFromObjectCore( default( ulong? ), MessagePackObject.Nil );
		}

		[Test]
		public void TestFromObject_NullableSingleNull_IsNil()
		{
			TestFromObjectCore( default( float? ), MessagePackObject.Nil );
		}

		[Test]
		public void TestFromObject_NullableDoubleNull_Nil()
		{
			TestFromObjectCore( default( double? ), MessagePackObject.Nil );
		}

		[Test]
		public void TestFromObject_String_Success()
		{
			TestFromObjectCore( "A", "A" );
		}

		[Test]
		public void TestFromObject_Bytes_Success()
		{
			var value = new byte[] { 1, 2 };
			TestFromObjectCore( value, value );
		}

		[Test]
		public void TestFromObject_Array_Success()
		{
			var value = new MessagePackObject[] { 1, 2 };
			TestFromObjectCore( value, value );
		}

		[Test]
		public void TestFromObject_Dictionary_Success()
		{
			var value = new MessagePackObjectDictionary() { { "1", 1 }, { "2", 2 } };
			TestFromObjectCore( value, new MessagePackObject( value ) );
		}

		[Test]
		public void TestFromObject_ByteSequence_Success()
		{
			TestFromObjectCore( Enumerable.Range( 1, 3 ).Select( i => ( byte )i ), new MessagePackObject( new byte[] { 1, 2, 3 } ) );
		}

		[Test]
		public void TestFromObject_CharSequence_Success()
		{
			TestFromObjectCore( Enumerable.Range( 'A', 3 ).Select( i => ( char )i ), new MessagePackObject( "ABC" ) );
		}

		[Test]
		public void TestFromObject_MessagePackObjectSequence_Success()
		{
			TestFromObjectCore( Enumerable.Range( 1, 3 ).Select( i => new MessagePackObject( i ) ), new MessagePackObject( new MessagePackObject[] { 1, 2, 3 } ) );
		}

		[Test]
		public void TestFromObject_ExtendedTypeObject_Success()
		{
			var value = new MessagePackExtendedTypeObject( 1, new byte[] {1} );
			TestFromObjectCore( value, value );
		}

		[Test]
		public void TestFromObject_NotSupported()
		{
			Assert.Throws<MessageTypeException>( () => MessagePackObject.FromObject( new Uri( "http://example.com" ) ) );
		}

		private static void TestFromObjectCore<T>( T value, MessagePackObject expected )
		{
			var actual = MessagePackObject.FromObject( value );
			Assert.AreEqual( expected, actual );
		}

		[Test]
		public void TestToObject_Zero_Success()
		{
			TestToObjectCore( 0, 0 );
		}

		[Test]
		public void TestToObject_Null_Success()
		{
			TestToObjectCore( MessagePackObject.Nil, default( object ) );
		}

		[Test]
		public void TestToObject_True_Success()
		{
			TestToObjectCore( true, true );
		}

		[Test]
		public void TestToObject_False_Success()
		{
			TestToObjectCore( false, false );
		}

		[Test]
		public void TestToObject_TinyPositiveInteger_Success()
		{
			TestToObjectCore( 1, 1 );
		}

		[Test]
		public void TestToObject_TinyNegativeIngeter_Success()
		{
			TestToObjectCore( -1, -1 );
		}

		[Test]
		public void TestToObject_MessagePackObject_Success()
		{
			TestToObjectCore( new MessagePackObject( 1 ), new MessagePackObject( 1 ) );
		}

		[Test]
		public void TestToObject_Int8_Success()
		{
			TestToObjectCore( SByte.MinValue, SByte.MinValue );
		}

		[Test]
		public void TestToObject_Int16_Success()
		{
			TestToObjectCore( Int16.MinValue, Int16.MinValue );
		}

		[Test]
		public void TestToObject_Int32_Success()
		{
			TestToObjectCore( Int32.MinValue, Int32.MinValue );
		}

		[Test]
		public void TestToObject_Int64_Success()
		{
			TestToObjectCore( Int64.MinValue, Int64.MinValue );
		}

		[Test]
		public void TestToObject_UInt8_Success()
		{
			TestToObjectCore( Byte.MaxValue, Byte.MaxValue );
		}

		[Test]
		public void TestToObject_UInt16_Success()
		{
			TestToObjectCore( UInt16.MaxValue, UInt16.MaxValue );
		}

		[Test]
		public void TestToObject_UInt32_Success()
		{
			TestToObjectCore( UInt32.MaxValue, UInt32.MaxValue );
		}

		[Test]
		public void TestToObject_UInt64_Success()
		{
			TestToObjectCore( UInt64.MinValue, UInt64.MinValue );
		}

		[Test]
		public void TestToObject_Single_Success()
		{
			TestToObjectCore( Single.Epsilon, Single.Epsilon );
		}

		[Test]
		public void TestToObject_Double_Success()
		{
			TestToObjectCore( Double.Epsilon, Double.Epsilon );
		}

		[Test]
		public void TestToObject_String_Success()
		{
			TestToObjectCore( "A", "A" );
		}

		[Test]
		public void TestToObject_Bytes_Success()
		{
			var value = new byte[] { 0xFF, 0xFF };
			TestToObjectCore( value, value );
		}

		[Test]
		public void TestToObject_Array_Success()
		{
			var value = new MessagePackObject[] { 1, 2 };
			TestToObjectCore( value, value );
		}

		[Test]
		public void TestToObject_Dictionary_Success()
		{
			var value = new MessagePackObjectDictionary() { { "1", 1 }, { "2", 2 } };
			TestToObjectCore( new MessagePackObject( value ), value );
		}

		[Test]
		public void TestToObject_ExtendedTypeObject_Success()
		{
			var value = new MessagePackExtendedTypeObject( 1, new byte[] { 1 } );
			TestToObjectCore( value, value );
		}

		private static void TestToObjectCore<T>( MessagePackObject target, T expected )
		{
			var actual = target.ToObject();
			Assert.AreEqual( MessagePackObject.FromObject( expected ), MessagePackObject.FromObject( actual ) );
		}
	}
}
