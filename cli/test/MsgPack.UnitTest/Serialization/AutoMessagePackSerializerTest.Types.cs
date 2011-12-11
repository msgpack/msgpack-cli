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
using NUnit.Framework;

namespace MsgPack.Serialization
{
	partial class AutoMessagePackSerializerTest
	{
		[Test]
		public void TestNullField()
		{
			TestCoreWithAutoVerify( default( object ) );
		}
		
		[Test]
		public void TestNullFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( default( object ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestNullFieldNull()
		{
			TestCoreWithAutoVerify( default( Object ) );
		}
		
		[Test]
		public void TestNullFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( Object[] ) );
		}	
		
		[Test]
		public void TestTrueField()
		{
			TestCoreWithAutoVerify( true );
		}
		
		[Test]
		public void TestTrueFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( true, 2 ).ToArray() );
		}
		
		[Test]
		public void TestFalseField()
		{
			TestCoreWithAutoVerify( false );
		}
		
		[Test]
		public void TestFalseFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( false, 2 ).ToArray() );
		}
		
		[Test]
		public void TestTinyByteField()
		{
			TestCoreWithAutoVerify( 1 );
		}
		
		[Test]
		public void TestTinyByteFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( 1, 2 ).ToArray() );
		}
		
		[Test]
		public void TestByteField()
		{
			TestCoreWithAutoVerify( 0x80 );
		}
		
		[Test]
		public void TestByteFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( 0x80, 2 ).ToArray() );
		}
		
		[Test]
		public void TestMaxByteField()
		{
			TestCoreWithAutoVerify( 0xff );
		}
		
		[Test]
		public void TestMaxByteFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( 0xff, 2 ).ToArray() );
		}
		
		[Test]
		public void TestTinyUInt16Field()
		{
			TestCoreWithAutoVerify( 0x100 );
		}
		
		[Test]
		public void TestTinyUInt16FieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( 0x100, 2 ).ToArray() );
		}
		
		[Test]
		public void TestMaxUInt16Field()
		{
			TestCoreWithAutoVerify( 0xffff );
		}
		
		[Test]
		public void TestMaxUInt16FieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( 0xffff, 2 ).ToArray() );
		}
		
		[Test]
		public void TestTinyInt32Field()
		{
			TestCoreWithAutoVerify( 0x10000 );
		}
		
		[Test]
		public void TestTinyInt32FieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( 0x10000, 2 ).ToArray() );
		}
		
		[Test]
		public void TestMaxInt32Field()
		{
			TestCoreWithAutoVerify( Int32.MaxValue );
		}
		
		[Test]
		public void TestMaxInt32FieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MaxValue, 2 ).ToArray() );
		}
		
		[Test]
		public void TestMinInt32Field()
		{
			TestCoreWithAutoVerify( Int32.MinValue );
		}
		
		[Test]
		public void TestMinInt32FieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( Int32.MinValue, 2 ).ToArray() );
		}
		
		[Test]
		public void TestTinyInt64Field()
		{
			TestCoreWithAutoVerify( 0x100000000 );
		}
		
		[Test]
		public void TestTinyInt64FieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( 0x100000000, 2 ).ToArray() );
		}
		
		[Test]
		public void TestMaxInt64Field()
		{
			TestCoreWithAutoVerify( Int64.MaxValue );
		}
		
		[Test]
		public void TestMaxInt64FieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MaxValue, 2 ).ToArray() );
		}
		
		[Test]
		public void TestMinInt64Field()
		{
			TestCoreWithAutoVerify( Int64.MinValue );
		}
		
		[Test]
		public void TestMinInt64FieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( Int64.MinValue, 2 ).ToArray() );
		}
		
		[Test]
		public void TestDateTimeField()
		{
			TestCoreWithAutoVerify( DateTime.UtcNow );
		}
		
		[Test]
		public void TestDateTimeFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( DateTime.UtcNow, 2 ).ToArray() );
		}
		
		[Test]
		public void TestDateTimeOffsetField()
		{
			TestCoreWithAutoVerify( DateTimeOffset.UtcNow );
		}
		
		[Test]
		public void TestDateTimeOffsetFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( DateTimeOffset.UtcNow, 2 ).ToArray() );
		}
		
		[Test]
		public void TestUriField()
		{
			TestCoreWithAutoVerify( new Uri( "http://example.com/" ) );
		}
		
		[Test]
		public void TestUriFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Uri( "http://example.com/" ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestUriFieldNull()
		{
			TestCoreWithAutoVerify( default( Uri ) );
		}
		
		[Test]
		public void TestUriFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( Uri[] ) );
		}	
		
		[Test]
		public void TestVersionField()
		{
			TestCoreWithAutoVerify( new Version( 1, 2, 3, 4 ) );
		}
		
		[Test]
		public void TestVersionFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Version( 1, 2, 3, 4 ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestVersionFieldNull()
		{
			TestCoreWithAutoVerify( default( Version ) );
		}
		
		[Test]
		public void TestVersionFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( Version[] ) );
		}	
		
		[Test]
		public void TestFILETIMEField()
		{
			TestCoreWithAutoVerify( ToFileTime( DateTime.UtcNow ) );
		}
		
		[Test]
		public void TestFILETIMEFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( ToFileTime( DateTime.UtcNow ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestTimeSpanField()
		{
			TestCoreWithAutoVerify( TimeSpan.FromMilliseconds( 123456789 ) );
		}
		
		[Test]
		public void TestTimeSpanFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( TimeSpan.FromMilliseconds( 123456789 ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestGuidField()
		{
			TestCoreWithAutoVerify( Guid.NewGuid() );
		}
		
		[Test]
		public void TestGuidFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( Guid.NewGuid(), 2 ).ToArray() );
		}
		
		[Test]
		public void TestCharField()
		{
			TestCoreWithAutoVerify( '　' );
		}
		
		[Test]
		public void TestCharFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( '　', 2 ).ToArray() );
		}
		
		[Test]
		public void TestDecimalField()
		{
			TestCoreWithAutoVerify( 123456789.0987654321m );
		}
		
		[Test]
		public void TestDecimalFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( 123456789.0987654321m, 2 ).ToArray() );
		}
		
		[Test]
		public void TestBigIntegerField()
		{
			TestCoreWithAutoVerify( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue );
		}
		
		[Test]
		public void TestBigIntegerFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue, 2 ).ToArray() );
		}
		
		[Test]
		public void TestComplexField()
		{
			TestCoreWithAutoVerify( new Complex( 1.3, 2.4 ) );
		}
		
		[Test]
		public void TestComplexFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Complex( 1.3, 2.4 ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestDictionaryEntryField()
		{
			TestCoreWithAutoVerify( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ) );
		}
		
		[Test]
		public void TestDictionaryEntryFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestKeyValuePairStringComplexField()
		{
			TestCoreWithAutoVerify( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ) );
		}
		
		[Test]
		public void TestKeyValuePairStringComplexFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestStringField()
		{
			TestCoreWithAutoVerify( "StringValue" );
		}
		
		[Test]
		public void TestStringFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( "StringValue", 2 ).ToArray() );
		}
		
		[Test]
		public void TestStringFieldNull()
		{
			TestCoreWithAutoVerify( default( String ) );
		}
		
		[Test]
		public void TestStringFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( String[] ) );
		}	
		
		[Test]
		public void TestByteArrayField()
		{
			TestCoreWithAutoVerify( new Byte[]{ 1, 2, 3, 4 } );
		}
		
		[Test]
		public void TestByteArrayFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Byte[]{ 1, 2, 3, 4 }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestByteArrayFieldNull()
		{
			TestCoreWithAutoVerify( default( Byte[] ) );
		}
		
		[Test]
		public void TestByteArrayFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( Byte[][] ) );
		}	
		
		[Test]
		public void TestCharArrayField()
		{
			TestCoreWithAutoVerify( "ABCD".ToCharArray() );
		}
		
		[Test]
		public void TestCharArrayFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( "ABCD".ToCharArray(), 2 ).ToArray() );
		}
		
		[Test]
		public void TestCharArrayFieldNull()
		{
			TestCoreWithAutoVerify( default( Char[] ) );
		}
		
		[Test]
		public void TestCharArrayFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( Char[][] ) );
		}	
		
		[Test]
		public void TestArraySegmentByteField()
		{
			TestCoreWithAutoVerify( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ) );
		}
		
		[Test]
		public void TestArraySegmentByteFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Byte>( new Byte[]{ 1, 2, 3, 4 } ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestArraySegmentInt32Field()
		{
			TestCoreWithAutoVerify( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ) );
		}
		
		[Test]
		public void TestArraySegmentInt32FieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Int32>( new Int32[]{ 1, 2, 3, 4 } ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestArraySegmentDecimalField()
		{
			TestCoreWithAutoVerify( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ) );
		}
		
		[Test]
		public void TestArraySegmentDecimalFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new ArraySegment<Decimal>( new Decimal[]{ 1, 2, 3, 4 } ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectField()
		{
			TestCoreWithAutoVerify( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) )  );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Tuple<Int32, String, MessagePackObject, Object>( 1, "ABC", new MessagePackObject( "abc" ), new MessagePackObject( "123" ) ) , 2 ).ToArray() );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object> ) );
		}
		
		[Test]
		public void TestTuple_Int32_String_MessagePackObject_ObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( System.Tuple<System.Int32, System.String, MsgPack.MessagePackObject, System.Object>[] ) );
		}	
		
		[Test]
		public void TestImage_Field()
		{
			TestCoreWithAutoVerify( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 } );
		}
		
		[Test]
		public void TestImage_FieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Image(){ uri = "http://example.com/logo.png", title = "logo", width = 160, height = 120, size = 13612 }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestImage_FieldNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Image ) );
		}
		
		[Test]
		public void TestImage_FieldArrayNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Image[] ) );
		}	
		
		[Test]
		public void TestListDateTimeField()
		{
			TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow } );
		}
		
		[Test]
		public void TestListDateTimeFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestListDateTimeFieldNull()
		{
			TestCoreWithAutoVerify( default( List<DateTime> ) );
		}
		
		[Test]
		public void TestListDateTimeFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( List<DateTime>[] ) );
		}	
		
		[Test]
		public void TestDictionaryStringDateTimeField()
		{
			TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } } );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldNull()
		{
			TestCoreWithAutoVerify( default( Dictionary<String, DateTime> ) );
		}
		
		[Test]
		public void TestDictionaryStringDateTimeFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( Dictionary<String, DateTime>[] ) );
		}	
		
		[Test]
		public void TestCollectionDateTimeField()
		{
			TestCoreWithAutoVerify( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow } );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldNull()
		{
			TestCoreWithAutoVerify( default( Collection<DateTime> ) );
		}
		
		[Test]
		public void TestCollectionDateTimeFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( Collection<DateTime>[] ) );
		}	
		
		[Test]
		public void TestStringKeyedCollection_DateTimeField()
		{
			TestCoreWithAutoVerify( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow } );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime> ) );
		}
		
		[Test]
		public void TestStringKeyedCollection_DateTimeFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.DateTime>[] ) );
		}	
		
		[Test]
		public void TestObservableCollectionDateTimeField()
		{
			TestCoreWithAutoVerify( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow } );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldNull()
		{
			TestCoreWithAutoVerify( default( ObservableCollection<DateTime> ) );
		}
		
		[Test]
		public void TestObservableCollectionDateTimeFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( ObservableCollection<DateTime>[] ) );
		}	
		
		[Test]
		public void TestHashSetDateTimeField()
		{
			TestCoreWithAutoVerify( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow } );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldNull()
		{
			TestCoreWithAutoVerify( default( HashSet<DateTime> ) );
		}
		
		[Test]
		public void TestHashSetDateTimeFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( HashSet<DateTime>[] ) );
		}	
		
		[Test]
		public void TestICollectionDateTimeField()
		{
			TestCoreWithAutoVerify( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow } );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldNull()
		{
			TestCoreWithAutoVerify( default( ICollection<DateTime> ) );
		}
		
		[Test]
		public void TestICollectionDateTimeFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( ICollection<DateTime>[] ) );
		}	
		
		[Test]
		public void TestISetDateTimeField()
		{
			TestCoreWithAutoVerify( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow } );
		}
		
		[Test]
		public void TestISetDateTimeFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestISetDateTimeFieldNull()
		{
			TestCoreWithAutoVerify( default( ISet<DateTime> ) );
		}
		
		[Test]
		public void TestISetDateTimeFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( ISet<DateTime>[] ) );
		}	
		
		[Test]
		public void TestIListDateTimeField()
		{
			TestCoreWithAutoVerify( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow } );
		}
		
		[Test]
		public void TestIListDateTimeFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new List<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestIListDateTimeFieldNull()
		{
			TestCoreWithAutoVerify( default( IList<DateTime> ) );
		}
		
		[Test]
		public void TestIListDateTimeFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( IList<DateTime>[] ) );
		}	
		
		[Test]
		public void TestIDictionaryStringDateTimeField()
		{
			TestCoreWithAutoVerify( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } } );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<String, DateTime>(){ { "Yesterday", DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ) }, { "Today", DateTime.UtcNow } }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldNull()
		{
			TestCoreWithAutoVerify( default( IDictionary<String, DateTime> ) );
		}
		
		[Test]
		public void TestIDictionaryStringDateTimeFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( IDictionary<String, DateTime>[] ) );
		}	
		
		[Test]
		public void TestAddOnlyCollection_DateTimeField()
		{
			TestCoreWithAutoVerify( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow } );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime> ) );
		}
		
		[Test]
		public void TestAddOnlyCollection_DateTimeFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.DateTime>[] ) );
		}	
		
		[Test]
		public void TestObjectField()
		{
			TestCoreWithAutoVerify( new MessagePackObject( 1 ) );
		}
		
		[Test]
		public void TestObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( Object ) );
		}
		
		[Test]
		public void TestObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( Object[] ) );
		}	
		
		[Test]
		public void TestObjectArrayField()
		{
			TestCoreWithAutoVerify( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestObjectArrayFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Object []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestObjectArrayFieldNull()
		{
			TestCoreWithAutoVerify( default( Object[] ) );
		}
		
		[Test]
		public void TestObjectArrayFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( Object[][] ) );
		}	
		
		[Test]
		public void TestArrayListField()
		{
			TestCoreWithAutoVerify( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestArrayListFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestArrayListFieldNull()
		{
			TestCoreWithAutoVerify( default( ArrayList ) );
		}
		
		[Test]
		public void TestArrayListFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( ArrayList[] ) );
		}	
		
		[Test]
		public void TestHashtableField()
		{
			TestCoreWithAutoVerify( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } } );
		}
		
		[Test]
		public void TestHashtableFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestHashtableFieldNull()
		{
			TestCoreWithAutoVerify( default( Hashtable ) );
		}
		
		[Test]
		public void TestHashtableFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( Hashtable[] ) );
		}	
		
		[Test]
		public void TestListObjectField()
		{
			TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestListObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestListObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( List<Object> ) );
		}
		
		[Test]
		public void TestListObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( List<Object>[] ) );
		}	
		
		[Test]
		public void TestDictionaryObjectObjectField()
		{
			TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } } );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( Dictionary<Object, Object> ) );
		}
		
		[Test]
		public void TestDictionaryObjectObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( Dictionary<Object, Object>[] ) );
		}	
		
		[Test]
		public void TestCollectionObjectField()
		{
			TestCoreWithAutoVerify( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestCollectionObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestCollectionObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( Collection<Object> ) );
		}
		
		[Test]
		public void TestCollectionObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( Collection<Object>[] ) );
		}	
		
		[Test]
		public void TestStringKeyedCollection_ObjectField()
		{
			TestCoreWithAutoVerify( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object> ) );
		}
		
		[Test]
		public void TestStringKeyedCollection_ObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<System.Object>[] ) );
		}	
		
		[Test]
		public void TestObservableCollectionObjectField()
		{
			TestCoreWithAutoVerify( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( ObservableCollection<Object> ) );
		}
		
		[Test]
		public void TestObservableCollectionObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( ObservableCollection<Object>[] ) );
		}	
		
		[Test]
		public void TestHashSetObjectField()
		{
			TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestHashSetObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestHashSetObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( HashSet<Object> ) );
		}
		
		[Test]
		public void TestHashSetObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( HashSet<Object>[] ) );
		}	
		
		[Test]
		public void TestICollectionObjectField()
		{
			TestCoreWithAutoVerify( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestICollectionObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestICollectionObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( ICollection<Object> ) );
		}
		
		[Test]
		public void TestICollectionObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( ICollection<Object>[] ) );
		}	
		
		[Test]
		public void TestISetObjectField()
		{
			TestCoreWithAutoVerify( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestISetObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestISetObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( ISet<Object> ) );
		}
		
		[Test]
		public void TestISetObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( ISet<Object>[] ) );
		}	
		
		[Test]
		public void TestIListObjectField()
		{
			TestCoreWithAutoVerify( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestIListObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new List<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestIListObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( IList<Object> ) );
		}
		
		[Test]
		public void TestIListObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( IList<Object>[] ) );
		}	
		
		[Test]
		public void TestIDictionaryObjectObjectField()
		{
			TestCoreWithAutoVerify( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } } );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<Object, Object>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( IDictionary<Object, Object> ) );
		}
		
		[Test]
		public void TestIDictionaryObjectObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( IDictionary<Object, Object>[] ) );
		}	
		
		[Test]
		public void TestAddOnlyCollection_ObjectField()
		{
			TestCoreWithAutoVerify( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<Object>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object> ) );
		}
		
		[Test]
		public void TestAddOnlyCollection_ObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<System.Object>[] ) );
		}	
		
		[Test]
		public void TestMessagePackObject_Field()
		{
			TestCoreWithAutoVerify( new MessagePackObject( 1 ) );
		}
		
		[Test]
		public void TestMessagePackObject_FieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject( 1 ), 2 ).ToArray() );
		}
		
		[Test]
		public void TestMessagePackObject_FieldNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.MessagePackObject ) );
		}
		
		[Test]
		public void TestMessagePackObject_FieldArrayNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ) );
		}	
		
		[Test]
		public void TestMessagePackObjectArray_Field()
		{
			TestCoreWithAutoVerify( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new MessagePackObject []{ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[] ) );
		}
		
		[Test]
		public void TestMessagePackObjectArray_FieldArrayNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.MessagePackObject[][] ) );
		}	
		
		[Test]
		public void TestList_MessagePackObjectField()
		{
			TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject> ) );
		}
		
		[Test]
		public void TestList_MessagePackObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.List<MsgPack.MessagePackObject>[] ) );
		}	
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectField()
		{
			TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } } );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ) );
		}
		
		[Test]
		public void TestDictionary_MessagePackObject_MessagePackObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ) );
		}	
		
		[Test]
		public void TestCollection_MessagePackObjectField()
		{
			TestCoreWithAutoVerify( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject> ) );
		}
		
		[Test]
		public void TestCollection_MessagePackObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.ObjectModel.Collection<MsgPack.MessagePackObject>[] ) );
		}	
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectField()
		{
			TestCoreWithAutoVerify( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject> ) );
		}
		
		[Test]
		public void TestStringKeyedCollection_MessagePackObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Serialization.StringKeyedCollection<MsgPack.MessagePackObject>[] ) );
		}	
		
		[Test]
		public void TestObservableCollection_MessagePackObjectField()
		{
			TestCoreWithAutoVerify( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject> ) );
		}
		
		[Test]
		public void TestObservableCollection_MessagePackObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.ObjectModel.ObservableCollection<MsgPack.MessagePackObject>[] ) );
		}	
		
		[Test]
		public void TestHashSet_MessagePackObjectField()
		{
			TestCoreWithAutoVerify( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject> ) );
		}
		
		[Test]
		public void TestHashSet_MessagePackObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.HashSet<MsgPack.MessagePackObject>[] ) );
		}	
		
		[Test]
		public void TestICollection_MessagePackObjectField()
		{
			TestCoreWithAutoVerify( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject> ) );
		}
		
		[Test]
		public void TestICollection_MessagePackObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.ICollection<MsgPack.MessagePackObject>[] ) );
		}	
		
		[Test]
		public void TestISet_MessagePackObjectField()
		{
			TestCoreWithAutoVerify( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new HashSet<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject> ) );
		}
		
		[Test]
		public void TestISet_MessagePackObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.ISet<MsgPack.MessagePackObject>[] ) );
		}	
		
		[Test]
		public void TestIList_MessagePackObjectField()
		{
			TestCoreWithAutoVerify( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject> ) );
		}
		
		[Test]
		public void TestIList_MessagePackObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.IList<MsgPack.MessagePackObject>[] ) );
		}	
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectField()
		{
			TestCoreWithAutoVerify( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } } );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new Dictionary<MessagePackObject, MessagePackObject>(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> ) );
		}
		
		[Test]
		public void TestIDictionary_MessagePackObject_MessagePackObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject>[] ) );
		}	
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectField()
		{
			TestCoreWithAutoVerify( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) } );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldArray()
		{
			TestCoreWithAutoVerify( Enumerable.Repeat( new AddOnlyCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) }, 2 ).ToArray() );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject> ) );
		}
		
		[Test]
		public void TestAddOnlyCollection_MessagePackObjectFieldArrayNull()
		{
			TestCoreWithAutoVerify( default( MsgPack.Serialization.AddOnlyCollection<MsgPack.MessagePackObject>[] ) );
		}	
		
		[Test]
		public void TestComplexTypeGeneratedEnclosure()
		{
			var target = new ComplexTypeGeneratedEnclosure();
			target.Initialize();
			TestCoreWithVerifiable1( target );
		}
		
		[Test]
		public void TestComplexTypeGeneratedEnclosureArray()
		{
			TestCoreWithVerifiable1( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGeneratedEnclosure().Initialize() ).ToArray() );
		}
		
		[Test]
		public void TestComplexTypeGenerated()
		{
			var target = new ComplexTypeGenerated();
			target.Initialize();
			TestCoreWithVerifiable1( target );
		}
		
		[Test]
		public void TestComplexTypeGeneratedArray()
		{
			TestCoreWithVerifiable1( Enumerable.Repeat( 0, 2 ).Select( _ => new ComplexTypeGenerated().Initialize() ).ToArray() );
		}

		private static void TestCoreWithAutoVerify<T>( T value )
		{
			var target = new AutoMessagePackSerializer<T>( GetSerializationContext() );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Verify( value, unpacked );
			}
		}
		
		private static void TestCoreWithVerifiable1<T>( T value )
			where T : IVerifiable<T>
		{
			var target = new AutoMessagePackSerializer<T>( GetSerializationContext() );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				unpacked.Verify( value );
			}
		}	
		
		private static void TestCoreWithVerifiable1<T>( T[] value )
			where T : IVerifiable<T>
		{
			var target = new AutoMessagePackSerializer<T[]>( GetSerializationContext() );
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
					catch( AssertionException ae )
					{
						throw new AssertionException( i.ToString(), ae );
					}
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

		private ArrayList _ArrayListField = new ArrayList();
		
		public ArrayList ArrayListField
		{
			get { return this._ArrayListField; }
		}

		private Hashtable _HashtableField = new Hashtable();
		
		public Hashtable HashtableField
		{
			get { return this._HashtableField; }
		}

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
			this._ArrayListField = new ArrayList(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._HashtableField = new Hashtable(){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } };
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
			AutoMessagePackSerializerTest.Verify( expected._ArrayListField, this._ArrayListField );
			AutoMessagePackSerializerTest.Verify( expected._HashtableField, this._HashtableField );
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
