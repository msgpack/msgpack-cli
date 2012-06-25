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

#pragma warning disable 3003
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack.Serialization
{
	partial class AutoMessagePackSerializerTest
	{
		[Test]
		public void TestNullFieldField()
		{
			this.TestCoreWithAutoVerify( default( object ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestNullFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( default( object ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestNullFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Object ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestNullFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestTrueFieldField()
		{
			this.TestCoreWithAutoVerify( true, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTrueFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( true, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestFalseFieldField()
		{
			this.TestCoreWithAutoVerify( false, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestFalseFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( false, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTinyByteFieldField()
		{
			this.TestCoreWithAutoVerify( 1, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTinyByteFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 1, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestByteFieldField()
		{
			this.TestCoreWithAutoVerify( 0x80, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestByteFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x80, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMaxByteFieldField()
		{
			this.TestCoreWithAutoVerify( 0xff, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMaxByteFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0xff, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTinyUInt16FieldField()
		{
			this.TestCoreWithAutoVerify( 0x100, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTinyUInt16FieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x100, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMaxUInt16FieldField()
		{
			this.TestCoreWithAutoVerify( 0xffff, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMaxUInt16FieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0xffff, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTinyInt32FieldField()
		{
			this.TestCoreWithAutoVerify( 0x10000, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTinyInt32FieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x10000, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMaxInt32FieldField()
		{
			this.TestCoreWithAutoVerify( Int32.MaxValue, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMaxInt32FieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MaxValue, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMinInt32FieldField()
		{
			this.TestCoreWithAutoVerify( Int32.MinValue, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMinInt32FieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MinValue, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTinyInt64FieldField()
		{
			this.TestCoreWithAutoVerify( 0x100000000, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTinyInt64FieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x100000000, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMaxInt64FieldField()
		{
			this.TestCoreWithAutoVerify( Int64.MaxValue, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMaxInt64FieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MaxValue, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMinInt64FieldField()
		{
			this.TestCoreWithAutoVerify( Int64.MinValue, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMinInt64FieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MinValue, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDateTimeFieldField()
		{
			this.TestCoreWithAutoVerify( DateTime.UtcNow, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDateTimeFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( DateTime.UtcNow, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDateTimeOffsetFieldField()
		{
			this.TestCoreWithAutoVerify( DateTimeOffset.UtcNow, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDateTimeOffsetFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( DateTimeOffset.UtcNow, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestUriFieldField()
		{
			this.TestCoreWithAutoVerify( new Uri( "http://example.com/" ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestUriFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Uri( "http://example.com/" ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestUriFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Uri ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestUriFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Uri[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestVersionFieldField()
		{
			this.TestCoreWithAutoVerify( new Version( 1, 2, 3, 4 ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestVersionFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Version( 1, 2, 3, 4 ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestVersionFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Version ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestVersionFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Version[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestFILETIMEFieldField()
		{
			this.TestCoreWithAutoVerify( ToFileTime( DateTime.UtcNow ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestFILETIMEFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( ToFileTime( DateTime.UtcNow ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTimeSpanFieldField()
		{
			this.TestCoreWithAutoVerify( TimeSpan.FromMilliseconds( 123456789 ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTimeSpanFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( TimeSpan.FromMilliseconds( 123456789 ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestGuidFieldField()
		{
			this.TestCoreWithAutoVerify( Guid.NewGuid(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestGuidFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Guid.NewGuid(), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCharFieldField()
		{
			this.TestCoreWithAutoVerify( '　', this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCharFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( '　', 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDecimalFieldField()
		{
			this.TestCoreWithAutoVerify( 123456789.0987654321m, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDecimalFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 123456789.0987654321m, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestBigIntegerFieldField()
		{
			this.TestCoreWithAutoVerify( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestBigIntegerFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestComplexFieldField()
		{
			this.TestCoreWithAutoVerify( new Complex( 1.3, 2.4 ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestComplexFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Complex( 1.3, 2.4 ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDictionaryEntryFieldField()
		{
			this.TestCoreWithAutoVerify( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDictionaryEntryFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestKeyValuePairStringComplexFieldField()
		{
			this.TestCoreWithAutoVerify( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestKeyValuePairStringComplexFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringFieldField()
		{
			this.TestCoreWithAutoVerify( "StringValue", this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( "StringValue", 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( String ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( String[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestByteArrayFieldField()
		{
			this.TestCoreWithAutoVerify( new Byte[]{ 1, 2, 3, 4 }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestByteArrayFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Byte[]{ 1, 2, 3, 4 }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestByteArrayFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Byte[] ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestByteArrayFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Byte[][] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestCharArrayFieldField()
		{
			this.TestCoreWithAutoVerify( "ABCD".ToCharArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCharArrayFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( "ABCD".ToCharArray(), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCharArrayFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Char[] ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCharArrayFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Char[][] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestArraySegmentByteFieldField()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestArraySegmentByteFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestArraySegmentInt32FieldField()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestArraySegmentInt32FieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestArraySegmentDecimalFieldField()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestArraySegmentDecimalFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestImage_FieldField()
		{
			this.TestCoreWithAutoVerify( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestImage_FieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestImage_FieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Image ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestImage_FieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Image[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestListDateTimeFieldField()
		{
			this.TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestListDateTimeFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestListDateTimeFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( List<DateTime> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestListDateTimeFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( List<DateTime>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestDictionaryStringDateTimeFieldField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<String, DateTime> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<String, DateTime>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestCollectionDateTimeFieldField()
		{
			this.TestCoreWithAutoVerify( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<DateTime> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<DateTime>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldField()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestObservableCollectionDateTimeFieldField()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<DateTime> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<DateTime>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestHashSetDateTimeFieldField()
		{
			this.TestCoreWithAutoVerify( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<DateTime> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<DateTime>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestICollectionDateTimeFieldField()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<DateTime> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<DateTime>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestISetDateTimeFieldField()
		{
			this.TestCoreWithAutoVerify( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestISetDateTimeFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestISetDateTimeFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<DateTime> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestISetDateTimeFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<DateTime>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestIListDateTimeFieldField()
		{
			this.TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIListDateTimeFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIListDateTimeFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IList<DateTime> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIListDateTimeFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IList<DateTime>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<String, DateTime> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<String, DateTime>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldField()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject( 1 ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Object ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestObjectArrayFieldField()
		{
			this.TestCoreWithAutoVerify( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObjectArrayFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObjectArrayFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObjectArrayFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[][] ), this.GetSerializationContextField() );
		}	
		
#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestArrayListFieldField()
		{
			this.TestCoreWithAutoVerify( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestArrayListFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestArrayListFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ArrayList ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestArrayListFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ArrayList[] ), this.GetSerializationContextField() );
		}	
		
#endif
#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestHashtableFieldField()
		{
			this.TestCoreWithAutoVerify( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestHashtableFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestHashtableFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Hashtable ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestHashtableFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Hashtable[] ), this.GetSerializationContextField() );
		}	
		
#endif
		[Test]
		public void TestListObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestListObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestListObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( List<Object> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestListObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( List<Object>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestDictionaryObjectObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<Object, Object> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<Object, Object>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestCollectionObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCollectionObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCollectionObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<Object> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCollectionObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<Object>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestObservableCollectionObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<Object> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<Object>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestHashSetObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestHashSetObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestHashSetObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<Object> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestHashSetObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<Object>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestICollectionObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestICollectionObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestICollectionObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<Object> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestICollectionObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<Object>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestISetObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestISetObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestISetObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<Object> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestISetObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<Object>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestIListObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIListObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIListObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IList<Object> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIListObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IList<Object>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestIDictionaryObjectObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<Object, Object> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<Object, Object>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestMessagePackObject_FieldField()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject( 1 ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestMessagePackObjectArray_FieldField()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[][] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestList_MessagePackObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestCollection_MessagePackObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestICollection_MessagePackObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestISet_MessagePackObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestIList_MessagePackObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldField()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject> ), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextField() );
		}	
		
		[Test]
		public void TestComplexTypeGeneratedEnclosure_Field()
		{
			var target = new ComplexTypeGeneratedEnclosure();
			target.Initialize();
			this.TestCoreWithVerifiable( target, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestComplexTypeGeneratedEnclosureArray_Field()
		{
			this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGeneratedEnclosure().Initialize() ).ToArray(), this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestComplexTypeGenerated_Field()
		{
			var target = new ComplexTypeGenerated();
			target.Initialize();
			this.TestCoreWithVerifiable( target, this.GetSerializationContextField() );
		}
		
		[Test]
		public void TestComplexTypeGeneratedArray_Field()
		{
			this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGenerated().Initialize() ).ToArray(), this.GetSerializationContextField() );
		}

		private SerializationContext GetSerializationContextField()
		{
			var result = this.GetSerializationContext();
			result.EmitterFlavor = EmitterFlavor.FieldBased;
			return result;
		}
		[Test]
		public void TestNullFieldContext()
		{
			this.TestCoreWithAutoVerify( default( object ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestNullFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( default( object ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestNullFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( Object ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestNullFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestTrueFieldContext()
		{
			this.TestCoreWithAutoVerify( true, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTrueFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( true, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestFalseFieldContext()
		{
			this.TestCoreWithAutoVerify( false, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestFalseFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( false, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTinyByteFieldContext()
		{
			this.TestCoreWithAutoVerify( 1, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTinyByteFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 1, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestByteFieldContext()
		{
			this.TestCoreWithAutoVerify( 0x80, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestByteFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x80, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMaxByteFieldContext()
		{
			this.TestCoreWithAutoVerify( 0xff, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMaxByteFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0xff, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTinyUInt16FieldContext()
		{
			this.TestCoreWithAutoVerify( 0x100, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTinyUInt16FieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x100, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMaxUInt16FieldContext()
		{
			this.TestCoreWithAutoVerify( 0xffff, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMaxUInt16FieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0xffff, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTinyInt32FieldContext()
		{
			this.TestCoreWithAutoVerify( 0x10000, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTinyInt32FieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x10000, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMaxInt32FieldContext()
		{
			this.TestCoreWithAutoVerify( Int32.MaxValue, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMaxInt32FieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MaxValue, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMinInt32FieldContext()
		{
			this.TestCoreWithAutoVerify( Int32.MinValue, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMinInt32FieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MinValue, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTinyInt64FieldContext()
		{
			this.TestCoreWithAutoVerify( 0x100000000, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTinyInt64FieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x100000000, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMaxInt64FieldContext()
		{
			this.TestCoreWithAutoVerify( Int64.MaxValue, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMaxInt64FieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MaxValue, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMinInt64FieldContext()
		{
			this.TestCoreWithAutoVerify( Int64.MinValue, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMinInt64FieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MinValue, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDateTimeFieldContext()
		{
			this.TestCoreWithAutoVerify( DateTime.UtcNow, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDateTimeFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( DateTime.UtcNow, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDateTimeOffsetFieldContext()
		{
			this.TestCoreWithAutoVerify( DateTimeOffset.UtcNow, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDateTimeOffsetFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( DateTimeOffset.UtcNow, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestUriFieldContext()
		{
			this.TestCoreWithAutoVerify( new Uri( "http://example.com/" ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestUriFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Uri( "http://example.com/" ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestUriFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( Uri ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestUriFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Uri[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestVersionFieldContext()
		{
			this.TestCoreWithAutoVerify( new Version( 1, 2, 3, 4 ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestVersionFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Version( 1, 2, 3, 4 ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestVersionFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( Version ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestVersionFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Version[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestFILETIMEFieldContext()
		{
			this.TestCoreWithAutoVerify( ToFileTime( DateTime.UtcNow ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestFILETIMEFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( ToFileTime( DateTime.UtcNow ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTimeSpanFieldContext()
		{
			this.TestCoreWithAutoVerify( TimeSpan.FromMilliseconds( 123456789 ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTimeSpanFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( TimeSpan.FromMilliseconds( 123456789 ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestGuidFieldContext()
		{
			this.TestCoreWithAutoVerify( Guid.NewGuid(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestGuidFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Guid.NewGuid(), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCharFieldContext()
		{
			this.TestCoreWithAutoVerify( '　', this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCharFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( '　', 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDecimalFieldContext()
		{
			this.TestCoreWithAutoVerify( 123456789.0987654321m, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDecimalFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 123456789.0987654321m, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestBigIntegerFieldContext()
		{
			this.TestCoreWithAutoVerify( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestBigIntegerFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestComplexFieldContext()
		{
			this.TestCoreWithAutoVerify( new Complex( 1.3, 2.4 ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestComplexFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Complex( 1.3, 2.4 ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDictionaryEntryFieldContext()
		{
			this.TestCoreWithAutoVerify( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDictionaryEntryFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestKeyValuePairStringComplexFieldContext()
		{
			this.TestCoreWithAutoVerify( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestKeyValuePairStringComplexFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringFieldContext()
		{
			this.TestCoreWithAutoVerify( "StringValue", this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( "StringValue", 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( String ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( String[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestByteArrayFieldContext()
		{
			this.TestCoreWithAutoVerify( new Byte[]{ 1, 2, 3, 4 }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestByteArrayFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Byte[]{ 1, 2, 3, 4 }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestByteArrayFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( Byte[] ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestByteArrayFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Byte[][] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestCharArrayFieldContext()
		{
			this.TestCoreWithAutoVerify( "ABCD".ToCharArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCharArrayFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( "ABCD".ToCharArray(), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCharArrayFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( Char[] ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCharArrayFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Char[][] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestArraySegmentByteFieldContext()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestArraySegmentByteFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestArraySegmentInt32FieldContext()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestArraySegmentInt32FieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestArraySegmentDecimalFieldContext()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestArraySegmentDecimalFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestImage_FieldContext()
		{
			this.TestCoreWithAutoVerify( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestImage_FieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestImage_FieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Image ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestImage_FieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Image[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestListDateTimeFieldContext()
		{
			this.TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestListDateTimeFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestListDateTimeFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( List<DateTime> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestListDateTimeFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( List<DateTime>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestDictionaryStringDateTimeFieldContext()
		{
			this.TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<String, DateTime> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<String, DateTime>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestCollectionDateTimeFieldContext()
		{
			this.TestCoreWithAutoVerify( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<DateTime> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<DateTime>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldContext()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestObservableCollectionDateTimeFieldContext()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<DateTime> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<DateTime>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestHashSetDateTimeFieldContext()
		{
			this.TestCoreWithAutoVerify( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<DateTime> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<DateTime>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestICollectionDateTimeFieldContext()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<DateTime> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<DateTime>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestISetDateTimeFieldContext()
		{
			this.TestCoreWithAutoVerify( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestISetDateTimeFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestISetDateTimeFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<DateTime> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestISetDateTimeFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<DateTime>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestIListDateTimeFieldContext()
		{
			this.TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIListDateTimeFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIListDateTimeFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( IList<DateTime> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIListDateTimeFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IList<DateTime>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldContext()
		{
			this.TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<String, DateTime> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<String, DateTime>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldContext()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject( 1 ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( Object ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestObjectArrayFieldContext()
		{
			this.TestCoreWithAutoVerify( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObjectArrayFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObjectArrayFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObjectArrayFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[][] ), this.GetSerializationContextContext() );
		}	
		
#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestArrayListFieldContext()
		{
			this.TestCoreWithAutoVerify( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestArrayListFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestArrayListFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( ArrayList ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestArrayListFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ArrayList[] ), this.GetSerializationContextContext() );
		}	
		
#endif
#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestHashtableFieldContext()
		{
			this.TestCoreWithAutoVerify( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestHashtableFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestHashtableFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( Hashtable ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestHashtableFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Hashtable[] ), this.GetSerializationContextContext() );
		}	
		
#endif
		[Test]
		public void TestListObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestListObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestListObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( List<Object> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestListObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( List<Object>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestDictionaryObjectObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<Object, Object> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<Object, Object>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestCollectionObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCollectionObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCollectionObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<Object> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCollectionObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<Object>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestObservableCollectionObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<Object> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<Object>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestHashSetObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestHashSetObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestHashSetObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<Object> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestHashSetObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<Object>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestICollectionObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestICollectionObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestICollectionObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<Object> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestICollectionObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<Object>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestISetObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestISetObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestISetObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<Object> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestISetObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<Object>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestIListObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIListObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIListObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( IList<Object> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIListObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IList<Object>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestIDictionaryObjectObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<Object, Object> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<Object, Object>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestMessagePackObject_FieldContext()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject( 1 ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestMessagePackObjectArray_FieldContext()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[][] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestList_MessagePackObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestCollection_MessagePackObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestICollection_MessagePackObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestISet_MessagePackObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestIList_MessagePackObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldContext()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldContextArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldContextNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject> ), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldContextArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextContext() );
		}	
		
		[Test]
		public void TestComplexTypeGeneratedEnclosure_Context()
		{
			var target = new ComplexTypeGeneratedEnclosure();
			target.Initialize();
			this.TestCoreWithVerifiable( target, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestComplexTypeGeneratedEnclosureArray_Context()
		{
			this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGeneratedEnclosure().Initialize() ).ToArray(), this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestComplexTypeGenerated_Context()
		{
			var target = new ComplexTypeGenerated();
			target.Initialize();
			this.TestCoreWithVerifiable( target, this.GetSerializationContextContext() );
		}
		
		[Test]
		public void TestComplexTypeGeneratedArray_Context()
		{
			this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGenerated().Initialize() ).ToArray(), this.GetSerializationContextContext() );
		}

		private SerializationContext GetSerializationContextContext()
		{
			var result = this.GetSerializationContext();
			result.EmitterFlavor = EmitterFlavor.ContextBased;
			return result;
		}
		[Test]
		public void TestNullFieldExpression()
		{
			this.TestCoreWithAutoVerify( default( object ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestNullFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( default( object ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestNullFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( Object ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestNullFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestTrueFieldExpression()
		{
			this.TestCoreWithAutoVerify( true, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTrueFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( true, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestFalseFieldExpression()
		{
			this.TestCoreWithAutoVerify( false, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestFalseFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( false, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTinyByteFieldExpression()
		{
			this.TestCoreWithAutoVerify( 1, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTinyByteFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 1, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestByteFieldExpression()
		{
			this.TestCoreWithAutoVerify( 0x80, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestByteFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x80, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMaxByteFieldExpression()
		{
			this.TestCoreWithAutoVerify( 0xff, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMaxByteFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0xff, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTinyUInt16FieldExpression()
		{
			this.TestCoreWithAutoVerify( 0x100, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTinyUInt16FieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x100, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMaxUInt16FieldExpression()
		{
			this.TestCoreWithAutoVerify( 0xffff, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMaxUInt16FieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0xffff, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTinyInt32FieldExpression()
		{
			this.TestCoreWithAutoVerify( 0x10000, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTinyInt32FieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x10000, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMaxInt32FieldExpression()
		{
			this.TestCoreWithAutoVerify( Int32.MaxValue, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMaxInt32FieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MaxValue, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMinInt32FieldExpression()
		{
			this.TestCoreWithAutoVerify( Int32.MinValue, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMinInt32FieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MinValue, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTinyInt64FieldExpression()
		{
			this.TestCoreWithAutoVerify( 0x100000000, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTinyInt64FieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x100000000, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMaxInt64FieldExpression()
		{
			this.TestCoreWithAutoVerify( Int64.MaxValue, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMaxInt64FieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MaxValue, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMinInt64FieldExpression()
		{
			this.TestCoreWithAutoVerify( Int64.MinValue, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMinInt64FieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MinValue, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDateTimeFieldExpression()
		{
			this.TestCoreWithAutoVerify( DateTime.UtcNow, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDateTimeFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( DateTime.UtcNow, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDateTimeOffsetFieldExpression()
		{
			this.TestCoreWithAutoVerify( DateTimeOffset.UtcNow, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDateTimeOffsetFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( DateTimeOffset.UtcNow, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestUriFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Uri( "http://example.com/" ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestUriFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Uri( "http://example.com/" ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestUriFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( Uri ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestUriFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Uri[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestVersionFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Version( 1, 2, 3, 4 ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestVersionFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Version( 1, 2, 3, 4 ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestVersionFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( Version ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestVersionFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Version[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestFILETIMEFieldExpression()
		{
			this.TestCoreWithAutoVerify( ToFileTime( DateTime.UtcNow ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestFILETIMEFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( ToFileTime( DateTime.UtcNow ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTimeSpanFieldExpression()
		{
			this.TestCoreWithAutoVerify( TimeSpan.FromMilliseconds( 123456789 ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTimeSpanFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( TimeSpan.FromMilliseconds( 123456789 ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestGuidFieldExpression()
		{
			this.TestCoreWithAutoVerify( Guid.NewGuid(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestGuidFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Guid.NewGuid(), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCharFieldExpression()
		{
			this.TestCoreWithAutoVerify( '　', this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCharFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( '　', 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDecimalFieldExpression()
		{
			this.TestCoreWithAutoVerify( 123456789.0987654321m, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDecimalFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 123456789.0987654321m, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestBigIntegerFieldExpression()
		{
			this.TestCoreWithAutoVerify( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestBigIntegerFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestComplexFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Complex( 1.3, 2.4 ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestComplexFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Complex( 1.3, 2.4 ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDictionaryEntryFieldExpression()
		{
			this.TestCoreWithAutoVerify( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDictionaryEntryFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestKeyValuePairStringComplexFieldExpression()
		{
			this.TestCoreWithAutoVerify( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestKeyValuePairStringComplexFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringFieldExpression()
		{
			this.TestCoreWithAutoVerify( "StringValue", this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( "StringValue", 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( String ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( String[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestByteArrayFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Byte[]{ 1, 2, 3, 4 }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestByteArrayFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Byte[]{ 1, 2, 3, 4 }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestByteArrayFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( Byte[] ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestByteArrayFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Byte[][] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestCharArrayFieldExpression()
		{
			this.TestCoreWithAutoVerify( "ABCD".ToCharArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCharArrayFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( "ABCD".ToCharArray(), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCharArrayFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( Char[] ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCharArrayFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Char[][] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestArraySegmentByteFieldExpression()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestArraySegmentByteFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestArraySegmentInt32FieldExpression()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestArraySegmentInt32FieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestArraySegmentDecimalFieldExpression()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestArraySegmentDecimalFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestImage_FieldExpression()
		{
			this.TestCoreWithAutoVerify( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestImage_FieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestImage_FieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Image ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestImage_FieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Image[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestListDateTimeFieldExpression()
		{
			this.TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestListDateTimeFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestListDateTimeFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( List<DateTime> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestListDateTimeFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( List<DateTime>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestDictionaryStringDateTimeFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<String, DateTime> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<String, DateTime>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestCollectionDateTimeFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<DateTime> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<DateTime>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldExpression()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestObservableCollectionDateTimeFieldExpression()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<DateTime> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<DateTime>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestHashSetDateTimeFieldExpression()
		{
			this.TestCoreWithAutoVerify( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<DateTime> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<DateTime>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestICollectionDateTimeFieldExpression()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<DateTime> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<DateTime>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestISetDateTimeFieldExpression()
		{
			this.TestCoreWithAutoVerify( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestISetDateTimeFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestISetDateTimeFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<DateTime> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestISetDateTimeFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<DateTime>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestIListDateTimeFieldExpression()
		{
			this.TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIListDateTimeFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIListDateTimeFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( IList<DateTime> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIListDateTimeFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IList<DateTime>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<String, DateTime> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<String, DateTime>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldExpression()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject( 1 ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( Object ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestObjectArrayFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObjectArrayFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObjectArrayFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObjectArrayFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[][] ), this.GetSerializationContextExpression() );
		}	
		
#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestArrayListFieldExpression()
		{
			this.TestCoreWithAutoVerify( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestArrayListFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestArrayListFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( ArrayList ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestArrayListFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ArrayList[] ), this.GetSerializationContextExpression() );
		}	
		
#endif
#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestHashtableFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestHashtableFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestHashtableFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( Hashtable ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestHashtableFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Hashtable[] ), this.GetSerializationContextExpression() );
		}	
		
#endif
		[Test]
		public void TestListObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestListObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestListObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( List<Object> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestListObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( List<Object>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestDictionaryObjectObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<Object, Object> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<Object, Object>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestCollectionObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCollectionObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCollectionObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<Object> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCollectionObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<Object>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestObservableCollectionObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<Object> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<Object>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestHashSetObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestHashSetObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestHashSetObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<Object> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestHashSetObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<Object>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestICollectionObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestICollectionObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestICollectionObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<Object> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestICollectionObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<Object>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestISetObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestISetObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestISetObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<Object> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestISetObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<Object>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestIListObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIListObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIListObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( IList<Object> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIListObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IList<Object>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestIDictionaryObjectObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<Object, Object> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<Object, Object>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestMessagePackObject_FieldExpression()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject( 1 ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestMessagePackObjectArray_FieldExpression()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[][] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestList_MessagePackObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestCollection_MessagePackObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestICollection_MessagePackObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestISet_MessagePackObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestIList_MessagePackObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldExpression()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldExpressionArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldExpressionNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject> ), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldExpressionArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContextExpression() );
		}	
		
		[Test]
		public void TestComplexTypeGeneratedEnclosure_Expression()
		{
			var target = new ComplexTypeGeneratedEnclosure();
			target.Initialize();
			this.TestCoreWithVerifiable( target, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestComplexTypeGeneratedEnclosureArray_Expression()
		{
			this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGeneratedEnclosure().Initialize() ).ToArray(), this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestComplexTypeGenerated_Expression()
		{
			var target = new ComplexTypeGenerated();
			target.Initialize();
			this.TestCoreWithVerifiable( target, this.GetSerializationContextExpression() );
		}
		
		[Test]
		public void TestComplexTypeGeneratedArray_Expression()
		{
			this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGenerated().Initialize() ).ToArray(), this.GetSerializationContextExpression() );
		}

		private SerializationContext GetSerializationContextExpression()
		{
			var result = this.GetSerializationContext();
			result.EmitterFlavor = EmitterFlavor.ExpressionBased;
			return result;
		}

		private void TestCoreWithAutoVerify<T>( T value, SerializationContext context )
		{
			var target = this.CreateTarget<T>( context );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Verify( value, unpacked );
			}
		}
		
		private void TestCoreWithVerifiable<T>( T value, SerializationContext context )
			where T : IVerifiable<T>
		{
			var target = this.CreateTarget<T>( context );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				unpacked.Verify( value );
			}
		}	
		
		private void TestCoreWithVerifiable<T>( T[] value, SerializationContext context )
			where T : IVerifiable<T>
		{
			var target = this.CreateTarget<T[]>( context );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				T[] unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.Length, Is.EqualTo( value.Length ) );
				for( int i = 0; i < unpacked.Length; i ++ )
				{
					try
					{
						unpacked[ i ].Verify( value[ i ] );
					}
#if MSTEST
					catch( Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssertFailedException ae )
					{
						throw new Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssertFailedException( i.ToString(), ae );
					}
#else
					catch( AssertionException ae )
					{
						throw new AssertionException( i.ToString(), ae );
					}
#endif
				}
			}
		}	
		
		private static FILETIME ToFileTime( DateTime dateTime )
		{
			var fileTime = dateTime.ToFileTimeUtc();
			return new FILETIME(){ dwHighDateTime = unchecked( ( int )( fileTime >> 32 ) ), dwLowDateTime = unchecked( ( int )( fileTime & 0xffffffff ) ) };
		}
	}
	
	public sealed class ComplexTypeGeneratedEnclosure : IVerifiable<ComplexTypeGeneratedEnclosure>
	{
		public string Name { get; set; }
		
		public ComplexTypeGenerated Nested { get; set; }
		
		public void Verify( ComplexTypeGeneratedEnclosure expected )
		{
			Assert.That( this.Name, Is.EqualTo( expected.Name ) );
			this.Nested.Verify( expected.Nested );
		}
		
		public ComplexTypeGeneratedEnclosure Initialize()
		{
			this.Name = "NAME";
			this.Nested = new ComplexTypeGenerated().Initialize();
			return this;
		}
	}
	
	public sealed class ComplexTypeGenerated : IVerifiable<ComplexTypeGenerated>
	{
		private Object _NullField;
		
		public Object NullField
		{
			get { return this._NullField; }
			set { this._NullField = value; }
		}
		private Boolean _TrueField;
		
		public Boolean TrueField
		{
			get { return this._TrueField; }
			set { this._TrueField = value; }
		}
		private Boolean _FalseField;
		
		public Boolean FalseField
		{
			get { return this._FalseField; }
			set { this._FalseField = value; }
		}
		private Byte _TinyByteField;
		
		public Byte TinyByteField
		{
			get { return this._TinyByteField; }
			set { this._TinyByteField = value; }
		}
		private Byte _ByteField;
		
		public Byte ByteField
		{
			get { return this._ByteField; }
			set { this._ByteField = value; }
		}
		private Byte _MaxByteField;
		
		public Byte MaxByteField
		{
			get { return this._MaxByteField; }
			set { this._MaxByteField = value; }
		}
		private UInt16 _TinyUInt16Field;
		
		public UInt16 TinyUInt16Field
		{
			get { return this._TinyUInt16Field; }
			set { this._TinyUInt16Field = value; }
		}
		private UInt16 _MaxUInt16Field;
		
		public UInt16 MaxUInt16Field
		{
			get { return this._MaxUInt16Field; }
			set { this._MaxUInt16Field = value; }
		}
		private Int32 _TinyInt32Field;
		
		public Int32 TinyInt32Field
		{
			get { return this._TinyInt32Field; }
			set { this._TinyInt32Field = value; }
		}
		private Int32 _MaxInt32Field;
		
		public Int32 MaxInt32Field
		{
			get { return this._MaxInt32Field; }
			set { this._MaxInt32Field = value; }
		}
		private Int32 _MinInt32Field;
		
		public Int32 MinInt32Field
		{
			get { return this._MinInt32Field; }
			set { this._MinInt32Field = value; }
		}
		private Int64 _TinyInt64Field;
		
		public Int64 TinyInt64Field
		{
			get { return this._TinyInt64Field; }
			set { this._TinyInt64Field = value; }
		}
		private Int64 _MaxInt64Field;
		
		public Int64 MaxInt64Field
		{
			get { return this._MaxInt64Field; }
			set { this._MaxInt64Field = value; }
		}
		private Int64 _MinInt64Field;
		
		public Int64 MinInt64Field
		{
			get { return this._MinInt64Field; }
			set { this._MinInt64Field = value; }
		}
		private DateTime _DateTimeField;
		
		public DateTime DateTimeField
		{
			get { return this._DateTimeField; }
			set { this._DateTimeField = value; }
		}
		private DateTimeOffset _DateTimeOffsetField;
		
		public DateTimeOffset DateTimeOffsetField
		{
			get { return this._DateTimeOffsetField; }
			set { this._DateTimeOffsetField = value; }
		}
		private Uri _UriField;
		
		public Uri UriField
		{
			get { return this._UriField; }
			set { this._UriField = value; }
		}
		private Version _VersionField;
		
		public Version VersionField
		{
			get { return this._VersionField; }
			set { this._VersionField = value; }
		}
		private FILETIME _FILETIMEField;
		
		public FILETIME FILETIMEField
		{
			get { return this._FILETIMEField; }
			set { this._FILETIMEField = value; }
		}
		private TimeSpan _TimeSpanField;
		
		public TimeSpan TimeSpanField
		{
			get { return this._TimeSpanField; }
			set { this._TimeSpanField = value; }
		}
		private Guid _GuidField;
		
		public Guid GuidField
		{
			get { return this._GuidField; }
			set { this._GuidField = value; }
		}
		private Char _CharField;
		
		public Char CharField
		{
			get { return this._CharField; }
			set { this._CharField = value; }
		}
		private Decimal _DecimalField;
		
		public Decimal DecimalField
		{
			get { return this._DecimalField; }
			set { this._DecimalField = value; }
		}
		private BigInteger _BigIntegerField;
		
		public BigInteger BigIntegerField
		{
			get { return this._BigIntegerField; }
			set { this._BigIntegerField = value; }
		}
		private Complex _ComplexField;
		
		public Complex ComplexField
		{
			get { return this._ComplexField; }
			set { this._ComplexField = value; }
		}
		private DictionaryEntry _DictionaryEntryField;
		
		public DictionaryEntry DictionaryEntryField
		{
			get { return this._DictionaryEntryField; }
			set { this._DictionaryEntryField = value; }
		}
		private KeyValuePair<String, Complex> _KeyValuePairStringComplexField;
		
		public KeyValuePair<String, Complex> KeyValuePairStringComplexField
		{
			get { return this._KeyValuePairStringComplexField; }
			set { this._KeyValuePairStringComplexField = value; }
		}
		private String _StringField;
		
		public String StringField
		{
			get { return this._StringField; }
			set { this._StringField = value; }
		}
		private Byte[] _ByteArrayField;
		
		public Byte[] ByteArrayField
		{
			get { return this._ByteArrayField; }
			set { this._ByteArrayField = value; }
		}
		private Char[] _CharArrayField;
		
		public Char[] CharArrayField
		{
			get { return this._CharArrayField; }
			set { this._CharArrayField = value; }
		}
		private ArraySegment<Byte> _ArraySegmentByteField;
		
		public ArraySegment<Byte> ArraySegmentByteField
		{
			get { return this._ArraySegmentByteField; }
			set { this._ArraySegmentByteField = value; }
		}
		private ArraySegment<Int32> _ArraySegmentInt32Field;
		
		public ArraySegment<Int32> ArraySegmentInt32Field
		{
			get { return this._ArraySegmentInt32Field; }
			set { this._ArraySegmentInt32Field = value; }
		}
		private ArraySegment<Decimal> _ArraySegmentDecimalField;
		
		public ArraySegment<Decimal> ArraySegmentDecimalField
		{
			get { return this._ArraySegmentDecimalField; }
			set { this._ArraySegmentDecimalField = value; }
		}
		private System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object> _Tuple_Int32_String_MessagePackObject_ObjectField;
		
		public System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object> Tuple_Int32_String_MessagePackObject_ObjectField
		{
			get { return this._Tuple_Int32_String_MessagePackObject_ObjectField; }
			set { this._Tuple_Int32_String_MessagePackObject_ObjectField = value; }
		}
		private MsgPack.Image _Image_Field;
		
		public MsgPack.Image Image_Field
		{
			get { return this._Image_Field; }
			set { this._Image_Field = value; }
		}
		private List<DateTime> _ListDateTimeField = new List<DateTime>();
		
		public List<DateTime> ListDateTimeField
		{
			get { return this._ListDateTimeField; }
		}
		private Dictionary<String, DateTime> _DictionaryStringDateTimeField = new Dictionary<String, DateTime>();
		
		public Dictionary<String, DateTime> DictionaryStringDateTimeField
		{
			get { return this._DictionaryStringDateTimeField; }
		}
		private Collection<DateTime> _CollectionDateTimeField = new Collection<DateTime>();
		
		public Collection<DateTime> CollectionDateTimeField
		{
			get { return this._CollectionDateTimeField; }
		}
		private MsgPack.Serialization.StringKeyedCollection<System.DateTime> _StringKeyedCollection_DateTimeField = new StringKeyedCollection<DateTime>();
		
		public MsgPack.Serialization.StringKeyedCollection<System.DateTime> StringKeyedCollection_DateTimeField
		{
			get { return this._StringKeyedCollection_DateTimeField; }
		}
		private ObservableCollection<DateTime> _ObservableCollectionDateTimeField = new ObservableCollection<DateTime>();
		
		public ObservableCollection<DateTime> ObservableCollectionDateTimeField
		{
			get { return this._ObservableCollectionDateTimeField; }
		}
		private HashSet<DateTime> _HashSetDateTimeField = new HashSet<DateTime>();
		
		public HashSet<DateTime> HashSetDateTimeField
		{
			get { return this._HashSetDateTimeField; }
		}
		private ICollection<DateTime> _ICollectionDateTimeField = new SimpleCollection<DateTime>();
		
		public ICollection<DateTime> ICollectionDateTimeField
		{
			get { return this._ICollectionDateTimeField; }
		}
		private ISet<DateTime> _ISetDateTimeField = new HashSet<DateTime>();
		
		public ISet<DateTime> ISetDateTimeField
		{
			get { return this._ISetDateTimeField; }
		}
		private IList<DateTime> _IListDateTimeField = new List<DateTime>();
		
		public IList<DateTime> IListDateTimeField
		{
			get { return this._IListDateTimeField; }
		}
		private IDictionary<String, DateTime> _IDictionaryStringDateTimeField = new Dictionary<String, DateTime>();
		
		public IDictionary<String, DateTime> IDictionaryStringDateTimeField
		{
			get { return this._IDictionaryStringDateTimeField; }
		}
		private MsgPack.Serialization.AddOnlyCollection<System.DateTime> _AddOnlyCollection_DateTimeField = new AddOnlyCollection<DateTime>();
		
		public MsgPack.Serialization.AddOnlyCollection<System.DateTime> AddOnlyCollection_DateTimeField
		{
			get { return this._AddOnlyCollection_DateTimeField; }
		}
		private Object _ObjectField;
		
		public Object ObjectField
		{
			get { return this._ObjectField; }
			set { this._ObjectField = value; }
		}
		private Object[] _ObjectArrayField;
		
		public Object[] ObjectArrayField
		{
			get { return this._ObjectArrayField; }
			set { this._ObjectArrayField = value; }
		}
#if !NETFX_CORE && !SILVERLIGHT
		private ArrayList _ArrayListField = new ArrayList();
		
		public ArrayList ArrayListField
		{
			get { return this._ArrayListField; }
		}
#endif
#if !NETFX_CORE && !SILVERLIGHT
		private Hashtable _HashtableField = new Hashtable();
		
		public Hashtable HashtableField
		{
			get { return this._HashtableField; }
		}
#endif
		private List<Object> _ListObjectField = new List<Object>();
		
		public List<Object> ListObjectField
		{
			get { return this._ListObjectField; }
		}
		private Dictionary<Object, Object> _DictionaryObjectObjectField = new Dictionary<Object, Object>();
		
		public Dictionary<Object, Object> DictionaryObjectObjectField
		{
			get { return this._DictionaryObjectObjectField; }
		}
		private Collection<Object> _CollectionObjectField = new Collection<Object>();
		
		public Collection<Object> CollectionObjectField
		{
			get { return this._CollectionObjectField; }
		}
		private MsgPack.Serialization.StringKeyedCollection<System.Object> _StringKeyedCollection_ObjectField = new StringKeyedCollection<Object>();
		
		public MsgPack.Serialization.StringKeyedCollection<System.Object> StringKeyedCollection_ObjectField
		{
			get { return this._StringKeyedCollection_ObjectField; }
		}
		private ObservableCollection<Object> _ObservableCollectionObjectField = new ObservableCollection<Object>();
		
		public ObservableCollection<Object> ObservableCollectionObjectField
		{
			get { return this._ObservableCollectionObjectField; }
		}
		private HashSet<Object> _HashSetObjectField = new HashSet<Object>();
		
		public HashSet<Object> HashSetObjectField
		{
			get { return this._HashSetObjectField; }
		}
		private ICollection<Object> _ICollectionObjectField = new SimpleCollection<Object>();
		
		public ICollection<Object> ICollectionObjectField
		{
			get { return this._ICollectionObjectField; }
		}
		private ISet<Object> _ISetObjectField = new HashSet<Object>();
		
		public ISet<Object> ISetObjectField
		{
			get { return this._ISetObjectField; }
		}
		private IList<Object> _IListObjectField = new List<Object>();
		
		public IList<Object> IListObjectField
		{
			get { return this._IListObjectField; }
		}
		private IDictionary<Object, Object> _IDictionaryObjectObjectField = new Dictionary<Object, Object>();
		
		public IDictionary<Object, Object> IDictionaryObjectObjectField
		{
			get { return this._IDictionaryObjectObjectField; }
		}
		private MsgPack.Serialization.AddOnlyCollection<System.Object> _AddOnlyCollection_ObjectField = new AddOnlyCollection<Object>();
		
		public MsgPack.Serialization.AddOnlyCollection<System.Object> AddOnlyCollection_ObjectField
		{
			get { return this._AddOnlyCollection_ObjectField; }
		}
		private MsgPack.MessagePackObject _MessagePackObject_Field;
		
		public MsgPack.MessagePackObject MessagePackObject_Field
		{
			get { return this._MessagePackObject_Field; }
			set { this._MessagePackObject_Field = value; }
		}
		private MsgPack.MessagePackObject[] _MessagePackObjectArray_Field;
		
		public MsgPack.MessagePackObject[] MessagePackObjectArray_Field
		{
			get { return this._MessagePackObjectArray_Field; }
			set { this._MessagePackObjectArray_Field = value; }
		}
		private System.Collections.Generic.List<MsgPack.MessagePackObject> _List_MessagePackObjectField = new List<MessagePackObject>();
		
		public System.Collections.Generic.List<MsgPack.MessagePackObject> List_MessagePackObjectField
		{
			get { return this._List_MessagePackObjectField; }
		}
		private System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> _Dictionary_MessagePackObject_MessagePackObjectField = new Dictionary<MessagePackObject, MessagePackObject>();
		
		public System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> Dictionary_MessagePackObject_MessagePackObjectField
		{
			get { return this._Dictionary_MessagePackObject_MessagePackObjectField; }
		}
		private System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject> _Collection_MessagePackObjectField = new Collection<MessagePackObject>();
		
		public System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject> Collection_MessagePackObjectField
		{
			get { return this._Collection_MessagePackObjectField; }
		}
		private MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject> _StringKeyedCollection_MessagePackObjectField = new StringKeyedCollection<MessagePackObject>();
		
		public MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject> StringKeyedCollection_MessagePackObjectField
		{
			get { return this._StringKeyedCollection_MessagePackObjectField; }
		}
		private System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject> _ObservableCollection_MessagePackObjectField = new ObservableCollection<MessagePackObject>();
		
		public System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject> ObservableCollection_MessagePackObjectField
		{
			get { return this._ObservableCollection_MessagePackObjectField; }
		}
		private System.Collections.Generic.HashSet<MsgPack.MessagePackObject> _HashSet_MessagePackObjectField = new HashSet<MessagePackObject>();
		
		public System.Collections.Generic.HashSet<MsgPack.MessagePackObject> HashSet_MessagePackObjectField
		{
			get { return this._HashSet_MessagePackObjectField; }
		}
		private System.Collections.Generic.ICollection<MsgPack.MessagePackObject> _ICollection_MessagePackObjectField = new SimpleCollection<MessagePackObject>();
		
		public System.Collections.Generic.ICollection<MsgPack.MessagePackObject> ICollection_MessagePackObjectField
		{
			get { return this._ICollection_MessagePackObjectField; }
		}
		private System.Collections.Generic.ISet<MsgPack.MessagePackObject> _ISet_MessagePackObjectField = new HashSet<MessagePackObject>();
		
		public System.Collections.Generic.ISet<MsgPack.MessagePackObject> ISet_MessagePackObjectField
		{
			get { return this._ISet_MessagePackObjectField; }
		}
		private System.Collections.Generic.IList<MsgPack.MessagePackObject> _IList_MessagePackObjectField = new List<MessagePackObject>();
		
		public System.Collections.Generic.IList<MsgPack.MessagePackObject> IList_MessagePackObjectField
		{
			get { return this._IList_MessagePackObjectField; }
		}
		private System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> _IDictionary_MessagePackObject_MessagePackObjectField = new Dictionary<MessagePackObject, MessagePackObject>();
		
		public System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> IDictionary_MessagePackObject_MessagePackObjectField
		{
			get { return this._IDictionary_MessagePackObject_MessagePackObjectField; }
		}
		private MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject> _AddOnlyCollection_MessagePackObjectField = new AddOnlyCollection<MessagePackObject>();
		
		public MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject> AddOnlyCollection_MessagePackObjectField
		{
			get { return this._AddOnlyCollection_MessagePackObjectField; }
		}

		public ComplexTypeGenerated Initialize()
		{
			this._NullField = null;
			this._TrueField = true;
			this._FalseField = false;
			this._TinyByteField = 1;
			this._ByteField = 0x80;
			this._MaxByteField = 0xff;
			this._TinyUInt16Field = 0x100;
			this._MaxUInt16Field = 0xffff;
			this._TinyInt32Field = 0x10000;
			this._MaxInt32Field = Int32.MaxValue;
			this._MinInt32Field = Int32.MinValue;
			this._TinyInt64Field = 0x100000000;
			this._MaxInt64Field = Int64.MaxValue;
			this._MinInt64Field = Int64.MinValue;
			this._DateTimeField = DateTime.UtcNow;
			this._DateTimeOffsetField = DateTimeOffset.UtcNow;
			this._UriField = new Uri( "http://example.com/" );
			this._VersionField = new Version( 1, 2, 3, 4 );
			this._FILETIMEField = ToFileTime( DateTime.UtcNow );
			this._TimeSpanField = TimeSpan.FromMilliseconds( 123456789 );
			this._GuidField = Guid.NewGuid();
			this._CharField = '　';
			this._DecimalField = 123456789.0987654321m;
			this._BigIntegerField = new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue;
			this._ComplexField = new Complex( 1.3, 2.4 );
			this._DictionaryEntryField = new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) );
			this._KeyValuePairStringComplexField = new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) );
			this._StringField = "StringValue";
			this._ByteArrayField = new Byte[]{ 1, 2, 3, 4 };
			this._CharArrayField = "ABCD".ToCharArray();
			this._ArraySegmentByteField = new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } );
			this._ArraySegmentInt32Field = new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } );
			this._ArraySegmentDecimalField = new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } );
			this._Tuple_Int32_String_MessagePackObject_ObjectField = new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) ;
			this._Image_Field = new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 };
			this._ListDateTimeField = new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._DictionaryStringDateTimeField = new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } };
			this._CollectionDateTimeField = new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._StringKeyedCollection_DateTimeField = new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._ObservableCollectionDateTimeField = new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._HashSetDateTimeField = new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._ICollectionDateTimeField = new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._ISetDateTimeField = new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._IListDateTimeField = new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._IDictionaryStringDateTimeField = new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } };
			this._AddOnlyCollection_DateTimeField = new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._ObjectField = new MessagePackObject( 1 );
			this._ObjectArrayField = new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#if !NETFX_CORE && !SILVERLIGHT
			this._ArrayListField = new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#endif
#if !NETFX_CORE && !SILVERLIGHT
			this._HashtableField = new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } };
#endif
			this._ListObjectField = new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._DictionaryObjectObjectField = new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } };
			this._CollectionObjectField = new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._StringKeyedCollection_ObjectField = new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._ObservableCollectionObjectField = new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._HashSetObjectField = new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._ICollectionObjectField = new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._ISetObjectField = new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._IListObjectField = new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._IDictionaryObjectObjectField = new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } };
			this._AddOnlyCollection_ObjectField = new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._MessagePackObject_Field = new MessagePackObject( 1 );
			this._MessagePackObjectArray_Field = new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._List_MessagePackObjectField = new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._Dictionary_MessagePackObject_MessagePackObjectField = new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } };
			this._Collection_MessagePackObjectField = new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._StringKeyedCollection_MessagePackObjectField = new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._ObservableCollection_MessagePackObjectField = new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._HashSet_MessagePackObjectField = new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._ICollection_MessagePackObjectField = new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._ISet_MessagePackObjectField = new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._IList_MessagePackObjectField = new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._IDictionary_MessagePackObject_MessagePackObjectField = new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } };
			this._AddOnlyCollection_MessagePackObjectField = new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			return this;
		}
		
		private static FILETIME ToFileTime( DateTime dateTime )
		{
			var fileTime = dateTime.ToFileTimeUtc();
			return new FILETIME(){ dwHighDateTime = unchecked( ( int )( fileTime >> 32 ) ), dwLowDateTime = unchecked( ( int )( fileTime & 0xffffffff ) ) };
		}
	
		public void Verify( ComplexTypeGenerated expected )
		{
			AutoMessagePackSerializerTest.Verify( expected._NullField, this._NullField );
			AutoMessagePackSerializerTest.Verify( expected._TrueField, this._TrueField );
			AutoMessagePackSerializerTest.Verify( expected._FalseField, this._FalseField );
			AutoMessagePackSerializerTest.Verify( expected._TinyByteField, this._TinyByteField );
			AutoMessagePackSerializerTest.Verify( expected._ByteField, this._ByteField );
			AutoMessagePackSerializerTest.Verify( expected._MaxByteField, this._MaxByteField );
			AutoMessagePackSerializerTest.Verify( expected._TinyUInt16Field, this._TinyUInt16Field );
			AutoMessagePackSerializerTest.Verify( expected._MaxUInt16Field, this._MaxUInt16Field );
			AutoMessagePackSerializerTest.Verify( expected._TinyInt32Field, this._TinyInt32Field );
			AutoMessagePackSerializerTest.Verify( expected._MaxInt32Field, this._MaxInt32Field );
			AutoMessagePackSerializerTest.Verify( expected._MinInt32Field, this._MinInt32Field );
			AutoMessagePackSerializerTest.Verify( expected._TinyInt64Field, this._TinyInt64Field );
			AutoMessagePackSerializerTest.Verify( expected._MaxInt64Field, this._MaxInt64Field );
			AutoMessagePackSerializerTest.Verify( expected._MinInt64Field, this._MinInt64Field );
			AutoMessagePackSerializerTest.Verify( expected._DateTimeField, this._DateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._DateTimeOffsetField, this._DateTimeOffsetField );
			AutoMessagePackSerializerTest.Verify( expected._UriField, this._UriField );
			AutoMessagePackSerializerTest.Verify( expected._VersionField, this._VersionField );
			AutoMessagePackSerializerTest.Verify( expected._FILETIMEField, this._FILETIMEField );
			AutoMessagePackSerializerTest.Verify( expected._TimeSpanField, this._TimeSpanField );
			AutoMessagePackSerializerTest.Verify( expected._GuidField, this._GuidField );
			AutoMessagePackSerializerTest.Verify( expected._CharField, this._CharField );
			AutoMessagePackSerializerTest.Verify( expected._DecimalField, this._DecimalField );
			AutoMessagePackSerializerTest.Verify( expected._BigIntegerField, this._BigIntegerField );
			AutoMessagePackSerializerTest.Verify( expected._ComplexField, this._ComplexField );
			AutoMessagePackSerializerTest.Verify( expected._DictionaryEntryField, this._DictionaryEntryField );
			AutoMessagePackSerializerTest.Verify( expected._KeyValuePairStringComplexField, this._KeyValuePairStringComplexField );
			AutoMessagePackSerializerTest.Verify( expected._StringField, this._StringField );
			AutoMessagePackSerializerTest.Verify( expected._ByteArrayField, this._ByteArrayField );
			AutoMessagePackSerializerTest.Verify( expected._CharArrayField, this._CharArrayField );
			AutoMessagePackSerializerTest.Verify( expected._ArraySegmentByteField, this._ArraySegmentByteField );
			AutoMessagePackSerializerTest.Verify( expected._ArraySegmentInt32Field, this._ArraySegmentInt32Field );
			AutoMessagePackSerializerTest.Verify( expected._ArraySegmentDecimalField, this._ArraySegmentDecimalField );
			AutoMessagePackSerializerTest.Verify( expected._Tuple_Int32_String_MessagePackObject_ObjectField, this._Tuple_Int32_String_MessagePackObject_ObjectField );
			AutoMessagePackSerializerTest.Verify( expected._Image_Field, this._Image_Field );
			AutoMessagePackSerializerTest.Verify( expected._ListDateTimeField, this._ListDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._DictionaryStringDateTimeField, this._DictionaryStringDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._CollectionDateTimeField, this._CollectionDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._StringKeyedCollection_DateTimeField, this._StringKeyedCollection_DateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._ObservableCollectionDateTimeField, this._ObservableCollectionDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._HashSetDateTimeField, this._HashSetDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._ICollectionDateTimeField, this._ICollectionDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._ISetDateTimeField, this._ISetDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._IListDateTimeField, this._IListDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._IDictionaryStringDateTimeField, this._IDictionaryStringDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._AddOnlyCollection_DateTimeField, this._AddOnlyCollection_DateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._ObjectField, this._ObjectField );
			AutoMessagePackSerializerTest.Verify( expected._ObjectArrayField, this._ObjectArrayField );
#if !NETFX_CORE && !SILVERLIGHT
			AutoMessagePackSerializerTest.Verify( expected._ArrayListField, this._ArrayListField );
#endif
#if !NETFX_CORE && !SILVERLIGHT
			AutoMessagePackSerializerTest.Verify( expected._HashtableField, this._HashtableField );
#endif
			AutoMessagePackSerializerTest.Verify( expected._ListObjectField, this._ListObjectField );
			AutoMessagePackSerializerTest.Verify( expected._DictionaryObjectObjectField, this._DictionaryObjectObjectField );
			AutoMessagePackSerializerTest.Verify( expected._CollectionObjectField, this._CollectionObjectField );
			AutoMessagePackSerializerTest.Verify( expected._StringKeyedCollection_ObjectField, this._StringKeyedCollection_ObjectField );
			AutoMessagePackSerializerTest.Verify( expected._ObservableCollectionObjectField, this._ObservableCollectionObjectField );
			AutoMessagePackSerializerTest.Verify( expected._HashSetObjectField, this._HashSetObjectField );
			AutoMessagePackSerializerTest.Verify( expected._ICollectionObjectField, this._ICollectionObjectField );
			AutoMessagePackSerializerTest.Verify( expected._ISetObjectField, this._ISetObjectField );
			AutoMessagePackSerializerTest.Verify( expected._IListObjectField, this._IListObjectField );
			AutoMessagePackSerializerTest.Verify( expected._IDictionaryObjectObjectField, this._IDictionaryObjectObjectField );
			AutoMessagePackSerializerTest.Verify( expected._AddOnlyCollection_ObjectField, this._AddOnlyCollection_ObjectField );
			AutoMessagePackSerializerTest.Verify( expected._MessagePackObject_Field, this._MessagePackObject_Field );
			AutoMessagePackSerializerTest.Verify( expected._MessagePackObjectArray_Field, this._MessagePackObjectArray_Field );
			AutoMessagePackSerializerTest.Verify( expected._List_MessagePackObjectField, this._List_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._Dictionary_MessagePackObject_MessagePackObjectField, this._Dictionary_MessagePackObject_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._Collection_MessagePackObjectField, this._Collection_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._StringKeyedCollection_MessagePackObjectField, this._StringKeyedCollection_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._ObservableCollection_MessagePackObjectField, this._ObservableCollection_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._HashSet_MessagePackObjectField, this._HashSet_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._ICollection_MessagePackObjectField, this._ICollection_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._ISet_MessagePackObjectField, this._ISet_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._IList_MessagePackObjectField, this._IList_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._IDictionary_MessagePackObject_MessagePackObjectField, this._IDictionary_MessagePackObject_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._AddOnlyCollection_MessagePackObjectField, this._AddOnlyCollection_MessagePackObjectField );
		
		}

	}
}
