#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
#if !NETFX_35
using System.Numerics;
#endif
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
		public void TestNullField()
		{
			this.TestCoreWithAutoVerify( default( object ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestNullFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( default( object ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestNullFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Object ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestNullFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestTrueField()
		{
			this.TestCoreWithAutoVerify( true, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTrueFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( true, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestFalseField()
		{
			this.TestCoreWithAutoVerify( false, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestFalseFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( false, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyByteField()
		{
			this.TestCoreWithAutoVerify( 1, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyByteFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 1, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestByteField()
		{
			this.TestCoreWithAutoVerify( 0x80, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestByteFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x80, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxByteField()
		{
			this.TestCoreWithAutoVerify( 0xff, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxByteFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0xff, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyUInt16Field()
		{
			this.TestCoreWithAutoVerify( 0x100, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyUInt16FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x100, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxUInt16Field()
		{
			this.TestCoreWithAutoVerify( 0xffff, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxUInt16FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0xffff, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyInt32Field()
		{
			this.TestCoreWithAutoVerify( 0x10000, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyInt32FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x10000, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxInt32Field()
		{
			this.TestCoreWithAutoVerify( Int32.MaxValue, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxInt32FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MaxValue, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMinInt32Field()
		{
			this.TestCoreWithAutoVerify( Int32.MinValue, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMinInt32FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MinValue, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyInt64Field()
		{
			this.TestCoreWithAutoVerify( 0x100000000, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTinyInt64FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 0x100000000, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxInt64Field()
		{
			this.TestCoreWithAutoVerify( Int64.MaxValue, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMaxInt64FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MaxValue, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMinInt64Field()
		{
			this.TestCoreWithAutoVerify( Int64.MinValue, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMinInt64FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MinValue, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDateTimeField()
		{
			this.TestCoreWithAutoVerify( DateTime.UtcNow, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( DateTime.UtcNow, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDateTimeOffsetField()
		{
			this.TestCoreWithAutoVerify( DateTimeOffset.UtcNow, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDateTimeOffsetFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( DateTimeOffset.UtcNow, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestUriField()
		{
			this.TestCoreWithAutoVerify( new Uri( "http://example.com/" ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestUriFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Uri( "http://example.com/" ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestUriFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Uri ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestUriFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Uri[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestVersionField()
		{
			this.TestCoreWithAutoVerify( new Version( 1, 2, 3, 4 ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestVersionFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Version( 1, 2, 3, 4 ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestVersionFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Version ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestVersionFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Version[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestFILETIMEField()
		{
			this.TestCoreWithAutoVerify( ToFileTime( DateTime.UtcNow ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestFILETIMEFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( ToFileTime( DateTime.UtcNow ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTimeSpanField()
		{
			this.TestCoreWithAutoVerify( TimeSpan.FromMilliseconds( 123456789 ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTimeSpanFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( TimeSpan.FromMilliseconds( 123456789 ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestGuidField()
		{
			this.TestCoreWithAutoVerify( Guid.NewGuid(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestGuidFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( Guid.NewGuid(), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCharField()
		{
			this.TestCoreWithAutoVerify( '　', this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCharFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( '　', 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDecimalField()
		{
			this.TestCoreWithAutoVerify( 123456789.0987654321m, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDecimalFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( 123456789.0987654321m, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
#if !NETFX_35
		[Test]
		public void TestBigIntegerField()
		{
			this.TestCoreWithAutoVerify( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestBigIntegerFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
#endif // !NETFX_35
#if !NETFX_35
		[Test]
		public void TestComplexField()
		{
			this.TestCoreWithAutoVerify( new Complex( 1.3, 2.4 ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestComplexFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Complex( 1.3, 2.4 ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
#endif // !NETFX_35
		[Test]
		public void TestDictionaryEntryField()
		{
			this.TestCoreWithAutoVerify( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryEntryFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestKeyValuePairStringDateTimeOffsetField()
		{
			this.TestCoreWithAutoVerify( new KeyValuePair<String, DateTimeOffset>( "Key", DateTimeOffset.UtcNow ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestKeyValuePairStringDateTimeOffsetFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new KeyValuePair<String, DateTimeOffset>( "Key", DateTimeOffset.UtcNow ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
#if !NETFX_35
		[Test]
		public void TestKeyValuePairStringComplexField()
		{
			this.TestCoreWithAutoVerify( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestKeyValuePairStringComplexFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
#endif // !NETFX_35
		[Test]
		public void TestStringField()
		{
			this.TestCoreWithAutoVerify( "StringValue", this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( "StringValue", 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringFieldNull()
		{
			this.TestCoreWithAutoVerify( default( String ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( String[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestByteArrayField()
		{
			this.TestCoreWithAutoVerify( new Byte[]{ 1, 2, 3, 4 }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestByteArrayFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Byte[]{ 1, 2, 3, 4 }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestByteArrayFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Byte[] ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestByteArrayFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Byte[][] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestCharArrayField()
		{
			this.TestCoreWithAutoVerify( "ABCD".ToCharArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCharArrayFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( "ABCD".ToCharArray(), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCharArrayFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Char[] ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCharArrayFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Char[][] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestArraySegmentByteField()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentByteFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentInt32Field()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentInt32FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentDecimalField()
		{
			this.TestCoreWithAutoVerify( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArraySegmentDecimalFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
#if !NETFX_35
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectField()
		{
			this.TestCoreWithAutoVerify( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestImage_Field()
		{
			this.TestCoreWithAutoVerify( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestImage_FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestImage_FieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Image ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestImage_FieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Image[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestListDateTimeField()
		{
			this.TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestListDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestListDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( List<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestListDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( List<DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestDictionaryStringDateTimeField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<String, DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<String, DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestCollectionDateTimeField()
		{
			this.TestCoreWithAutoVerify( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_DateTimeField()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime>[] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestObservableCollectionDateTimeField()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<DateTime>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestHashSetDateTimeField()
		{
			this.TestCoreWithAutoVerify( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestICollectionDateTimeField()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<DateTime>[] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestISetDateTimeField()
		{
			this.TestCoreWithAutoVerify( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISetDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISetDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISetDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<DateTime>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestIListDateTimeField()
		{
			this.TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIListDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIListDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IList<DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIListDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IList<DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestIDictionaryStringDateTimeField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<String, DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<String, DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_DateTimeField()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestObjectField()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject( 1 ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Object ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestObjectArrayField()
		{
			this.TestCoreWithAutoVerify( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectArrayFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectArrayFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Object[] ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObjectArrayFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Object[][] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestArrayListField()
		{
			this.TestCoreWithAutoVerify( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArrayListFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArrayListFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ArrayList ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestArrayListFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ArrayList[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_CORE && !SILVERLIGHT
#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestHashtableField()
		{
			this.TestCoreWithAutoVerify( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashtableFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashtableFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Hashtable ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashtableFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Hashtable[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestListObjectField()
		{
			this.TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestListObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestListObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( List<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestListObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( List<Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestDictionaryObjectObjectField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<Object, Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Dictionary<Object, Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestCollectionObjectField()
		{
			this.TestCoreWithAutoVerify( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollectionObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( Collection<Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_ObjectField()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object>[] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestObservableCollectionObjectField()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ObservableCollection<Object>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestHashSetObjectField()
		{
			this.TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSetObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( HashSet<Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestICollectionObjectField()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollectionObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ICollection<Object>[] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestISetObjectField()
		{
			this.TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISetObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISetObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISetObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( ISet<Object>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestIListObjectField()
		{
			this.TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIListObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIListObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IList<Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIListObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IList<Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestIDictionaryObjectObjectField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<Object, Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( IDictionary<Object, Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_ObjectField()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestMessagePackObject_Field()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject( 1 ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestMessagePackObjectArray_Field()
		{
			this.TestCoreWithAutoVerify( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[][] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestList_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestCollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestObservableCollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestHashSet_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestICollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
#if !NETFX_35
		[Test]
		public void TestISet_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
#endif // !NETFX_35
		[Test]
		public void TestIList_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectField()
		{
			this.TestCoreWithAutoVerify( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldArray()
		{
			this.TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject> ), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldArrayNull()
		{
			this.TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject>[] ), this.GetSerializationContext() );
		}	
		
		[Test]
		public void TestComplexTypeGeneratedEnclosure()
		{
			var target = new ComplexTypeGeneratedEnclosure();
			target.Initialize();
			this.TestCoreWithVerifiable( target, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestComplexTypeGeneratedEnclosureArray()
		{
			this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGeneratedEnclosure().Initialize() ).ToArray(), this.GetSerializationContext() );
		}
		
		[Test]
		public void TestComplexTypeGenerated()
		{
			var target = new ComplexTypeGenerated();
			target.Initialize();
			this.TestCoreWithVerifiable( target, this.GetSerializationContext() );
		}
		
		[Test]
		public void TestComplexTypeGeneratedArray()
		{
			this.TestCoreWithVerifiable( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGenerated().Initialize() ).ToArray(), this.GetSerializationContext() );
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
#if !NETFX_35
		private BigInteger _BigIntegerField;
		
		public BigInteger BigIntegerField
		{
			get { return this._BigIntegerField; }
			set { this._BigIntegerField = value; }
		}
#endif // !NETFX_35
#if !NETFX_35
		private Complex _ComplexField;
		
		public Complex ComplexField
		{
			get { return this._ComplexField; }
			set { this._ComplexField = value; }
		}
#endif // !NETFX_35
		private DictionaryEntry _DictionaryEntryField;
		
		public DictionaryEntry DictionaryEntryField
		{
			get { return this._DictionaryEntryField; }
			set { this._DictionaryEntryField = value; }
		}
		private KeyValuePair<String, DateTimeOffset> _KeyValuePairStringDateTimeOffsetField;
		
		public KeyValuePair<String, DateTimeOffset> KeyValuePairStringDateTimeOffsetField
		{
			get { return this._KeyValuePairStringDateTimeOffsetField; }
			set { this._KeyValuePairStringDateTimeOffsetField = value; }
		}
#if !NETFX_35
		private KeyValuePair<String, Complex> _KeyValuePairStringComplexField;
		
		public KeyValuePair<String, Complex> KeyValuePairStringComplexField
		{
			get { return this._KeyValuePairStringComplexField; }
			set { this._KeyValuePairStringComplexField = value; }
		}
#endif // !NETFX_35
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
#if !NETFX_35
		private System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object> _Tuple_Int32_String_MessagePackObject_ObjectField;
		
		public System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object> Tuple_Int32_String_MessagePackObject_ObjectField
		{
			get { return this._Tuple_Int32_String_MessagePackObject_ObjectField; }
			set { this._Tuple_Int32_String_MessagePackObject_ObjectField = value; }
		}
#endif // !NETFX_35
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
#if !NETFX_35
		private ObservableCollection<DateTime> _ObservableCollectionDateTimeField = new ObservableCollection<DateTime>();
		
		public ObservableCollection<DateTime> ObservableCollectionDateTimeField
		{
			get { return this._ObservableCollectionDateTimeField; }
		}
#endif // !NETFX_35
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
#if !NETFX_35
		private ISet<DateTime> _ISetDateTimeField = new HashSet<DateTime>();
		
		public ISet<DateTime> ISetDateTimeField
		{
			get { return this._ISetDateTimeField; }
		}
#endif // !NETFX_35
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
#endif // !NETFX_CORE && !SILVERLIGHT
#if !NETFX_CORE && !SILVERLIGHT
		private Hashtable _HashtableField = new Hashtable();
		
		public Hashtable HashtableField
		{
			get { return this._HashtableField; }
		}
#endif // !NETFX_CORE && !SILVERLIGHT
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
#if !NETFX_35
		private ObservableCollection<Object> _ObservableCollectionObjectField = new ObservableCollection<Object>();
		
		public ObservableCollection<Object> ObservableCollectionObjectField
		{
			get { return this._ObservableCollectionObjectField; }
		}
#endif // !NETFX_35
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
#if !NETFX_35
		private ISet<Object> _ISetObjectField = new HashSet<Object>();
		
		public ISet<Object> ISetObjectField
		{
			get { return this._ISetObjectField; }
		}
#endif // !NETFX_35
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
#if !NETFX_35
		private System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject> _ObservableCollection_MessagePackObjectField = new ObservableCollection<MessagePackObject>();
		
		public System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject> ObservableCollection_MessagePackObjectField
		{
			get { return this._ObservableCollection_MessagePackObjectField; }
		}
#endif // !NETFX_35
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
#if !NETFX_35
		private System.Collections.Generic.ISet<MsgPack.MessagePackObject> _ISet_MessagePackObjectField = new HashSet<MessagePackObject>();
		
		public System.Collections.Generic.ISet<MsgPack.MessagePackObject> ISet_MessagePackObjectField
		{
			get { return this._ISet_MessagePackObjectField; }
		}
#endif // !NETFX_35
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
#if !NETFX_35
			this._BigIntegerField = new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue;
#endif // !NETFX_35
#if !NETFX_35
			this._ComplexField = new Complex( 1.3, 2.4 );
#endif // !NETFX_35
			this._DictionaryEntryField = new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) );
			this._KeyValuePairStringDateTimeOffsetField = new KeyValuePair<String, DateTimeOffset>( "Key", DateTimeOffset.UtcNow );
#if !NETFX_35
			this._KeyValuePairStringComplexField = new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) );
#endif // !NETFX_35
			this._StringField = "StringValue";
			this._ByteArrayField = new Byte[]{ 1, 2, 3, 4 };
			this._CharArrayField = "ABCD".ToCharArray();
			this._ArraySegmentByteField = new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } );
			this._ArraySegmentInt32Field = new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } );
			this._ArraySegmentDecimalField = new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } );
#if !NETFX_35
			this._Tuple_Int32_String_MessagePackObject_ObjectField = new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) ;
#endif // !NETFX_35
			this._Image_Field = new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 };
			this._ListDateTimeField = new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._DictionaryStringDateTimeField = new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } };
			this._CollectionDateTimeField = new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._StringKeyedCollection_DateTimeField = new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
#if !NETFX_35
			this._ObservableCollectionDateTimeField = new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
#endif // !NETFX_35
			this._HashSetDateTimeField = new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._ICollectionDateTimeField = new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
#if !NETFX_35
			this._ISetDateTimeField = new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
#endif // !NETFX_35
			this._IListDateTimeField = new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._IDictionaryStringDateTimeField = new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } };
			this._AddOnlyCollection_DateTimeField = new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._ObjectField = new MessagePackObject( 1 );
			this._ObjectArrayField = new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#if !NETFX_CORE && !SILVERLIGHT
			this._ArrayListField = new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#endif // !NETFX_CORE && !SILVERLIGHT
#if !NETFX_CORE && !SILVERLIGHT
			this._HashtableField = new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } };
#endif // !NETFX_CORE && !SILVERLIGHT
			this._ListObjectField = new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._DictionaryObjectObjectField = new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } };
			this._CollectionObjectField = new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._StringKeyedCollection_ObjectField = new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#if !NETFX_35
			this._ObservableCollectionObjectField = new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#endif // !NETFX_35
			this._HashSetObjectField = new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._ICollectionObjectField = new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#if !NETFX_35
			this._ISetObjectField = new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#endif // !NETFX_35
			this._IListObjectField = new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._IDictionaryObjectObjectField = new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } };
			this._AddOnlyCollection_ObjectField = new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._MessagePackObject_Field = new MessagePackObject( 1 );
			this._MessagePackObjectArray_Field = new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._List_MessagePackObjectField = new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._Dictionary_MessagePackObject_MessagePackObjectField = new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } };
			this._Collection_MessagePackObjectField = new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._StringKeyedCollection_MessagePackObjectField = new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#if !NETFX_35
			this._ObservableCollection_MessagePackObjectField = new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#endif // !NETFX_35
			this._HashSet_MessagePackObjectField = new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._ICollection_MessagePackObjectField = new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#if !NETFX_35
			this._ISet_MessagePackObjectField = new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#endif // !NETFX_35
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
#if !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._BigIntegerField, this._BigIntegerField );
#endif // !NETFX_35
#if !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._ComplexField, this._ComplexField );
#endif // !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._DictionaryEntryField, this._DictionaryEntryField );
			AutoMessagePackSerializerTest.Verify( expected._KeyValuePairStringDateTimeOffsetField, this._KeyValuePairStringDateTimeOffsetField );
#if !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._KeyValuePairStringComplexField, this._KeyValuePairStringComplexField );
#endif // !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._StringField, this._StringField );
			AutoMessagePackSerializerTest.Verify( expected._ByteArrayField, this._ByteArrayField );
			AutoMessagePackSerializerTest.Verify( expected._CharArrayField, this._CharArrayField );
			AutoMessagePackSerializerTest.Verify( expected._ArraySegmentByteField, this._ArraySegmentByteField );
			AutoMessagePackSerializerTest.Verify( expected._ArraySegmentInt32Field, this._ArraySegmentInt32Field );
			AutoMessagePackSerializerTest.Verify( expected._ArraySegmentDecimalField, this._ArraySegmentDecimalField );
#if !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._Tuple_Int32_String_MessagePackObject_ObjectField, this._Tuple_Int32_String_MessagePackObject_ObjectField );
#endif // !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._Image_Field, this._Image_Field );
			AutoMessagePackSerializerTest.Verify( expected._ListDateTimeField, this._ListDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._DictionaryStringDateTimeField, this._DictionaryStringDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._CollectionDateTimeField, this._CollectionDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._StringKeyedCollection_DateTimeField, this._StringKeyedCollection_DateTimeField );
#if !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._ObservableCollectionDateTimeField, this._ObservableCollectionDateTimeField );
#endif // !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._HashSetDateTimeField, this._HashSetDateTimeField );
			AutoMessagePackSerializerTest.Verify( expected._ICollectionDateTimeField, this._ICollectionDateTimeField );
#if !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._ISetDateTimeField, this._ISetDateTimeField );
#endif // !NETFX_35
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
#if !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._ObservableCollectionObjectField, this._ObservableCollectionObjectField );
#endif // !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._HashSetObjectField, this._HashSetObjectField );
			AutoMessagePackSerializerTest.Verify( expected._ICollectionObjectField, this._ICollectionObjectField );
#if !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._ISetObjectField, this._ISetObjectField );
#endif // !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._IListObjectField, this._IListObjectField );
			AutoMessagePackSerializerTest.Verify( expected._IDictionaryObjectObjectField, this._IDictionaryObjectObjectField );
			AutoMessagePackSerializerTest.Verify( expected._AddOnlyCollection_ObjectField, this._AddOnlyCollection_ObjectField );
			AutoMessagePackSerializerTest.Verify( expected._MessagePackObject_Field, this._MessagePackObject_Field );
			AutoMessagePackSerializerTest.Verify( expected._MessagePackObjectArray_Field, this._MessagePackObjectArray_Field );
			AutoMessagePackSerializerTest.Verify( expected._List_MessagePackObjectField, this._List_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._Dictionary_MessagePackObject_MessagePackObjectField, this._Dictionary_MessagePackObject_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._Collection_MessagePackObjectField, this._Collection_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._StringKeyedCollection_MessagePackObjectField, this._StringKeyedCollection_MessagePackObjectField );
#if !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._ObservableCollection_MessagePackObjectField, this._ObservableCollection_MessagePackObjectField );
#endif // !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._HashSet_MessagePackObjectField, this._HashSet_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._ICollection_MessagePackObjectField, this._ICollection_MessagePackObjectField );
#if !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._ISet_MessagePackObjectField, this._ISet_MessagePackObjectField );
#endif // !NETFX_35
			AutoMessagePackSerializerTest.Verify( expected._IList_MessagePackObjectField, this._IList_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._IDictionary_MessagePackObject_MessagePackObjectField, this._IDictionary_MessagePackObject_MessagePackObjectField );
			AutoMessagePackSerializerTest.Verify( expected._AddOnlyCollection_MessagePackObjectField, this._AddOnlyCollection_MessagePackObjectField );
		
		}
	}
}
