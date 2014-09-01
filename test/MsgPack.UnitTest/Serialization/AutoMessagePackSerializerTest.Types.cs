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
#if !NETFX_35 && !WINDOWS_PHONE
using System.Numerics;
#endif
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
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
#if !NETFX_35 && !WINDOWS_PHONE
		private BigInteger _BigIntegerField;
		
		public BigInteger BigIntegerField
		{
			get { return this._BigIntegerField; }
			set { this._BigIntegerField = value; }
		}
#endif // !NETFX_35 && !WINDOWS_PHONE
#if !NETFX_35 && !WINDOWS_PHONE
		private Complex _ComplexField;
		
		public Complex ComplexField
		{
			get { return this._ComplexField; }
			set { this._ComplexField = value; }
		}
#endif // !NETFX_35 && !WINDOWS_PHONE
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
#if !NETFX_35 && !WINDOWS_PHONE
		private KeyValuePair<String, Complex> _KeyValuePairStringComplexField;
		
		public KeyValuePair<String, Complex> KeyValuePairStringComplexField
		{
			get { return this._KeyValuePairStringComplexField; }
			set { this._KeyValuePairStringComplexField = value; }
		}
#endif // !NETFX_35 && !WINDOWS_PHONE
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
#if !NETFX_35 && !WINDOWS_PHONE
			this._BigIntegerField = new BigInteger( UInt64.MaxValue ) + UInt64.MaxValue;
#endif // !NETFX_35 && !WINDOWS_PHONE
#if !NETFX_35 && !WINDOWS_PHONE
			this._ComplexField = new Complex( 1.3, 2.4 );
#endif // !NETFX_35 && !WINDOWS_PHONE
			this._DictionaryEntryField = new DictionaryEntry( new MessagePackObject( "Key" ), new MessagePackObject( "Value" ) );
			this._KeyValuePairStringDateTimeOffsetField = new KeyValuePair<String, DateTimeOffset>( "Key", DateTimeOffset.UtcNow );
#if !NETFX_35 && !WINDOWS_PHONE
			this._KeyValuePairStringComplexField = new KeyValuePair<String, Complex>( "Key", new Complex( 1.3, 2.4 ) );
#endif // !NETFX_35 && !WINDOWS_PHONE
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
#if !NETFX_35 && !WINDOWS_PHONE
			AutoMessagePackSerializerTest.Verify( expected._BigIntegerField, this._BigIntegerField );
#endif // !NETFX_35 && !WINDOWS_PHONE
#if !NETFX_35 && !WINDOWS_PHONE
			AutoMessagePackSerializerTest.Verify( expected._ComplexField, this._ComplexField );
#endif // !NETFX_35 && !WINDOWS_PHONE
			AutoMessagePackSerializerTest.Verify( expected._DictionaryEntryField, this._DictionaryEntryField );
			AutoMessagePackSerializerTest.Verify( expected._KeyValuePairStringDateTimeOffsetField, this._KeyValuePairStringDateTimeOffsetField );
#if !NETFX_35 && !WINDOWS_PHONE
			AutoMessagePackSerializerTest.Verify( expected._KeyValuePairStringComplexField, this._KeyValuePairStringComplexField );
#endif // !NETFX_35 && !WINDOWS_PHONE
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
#endif // !NETFX_CORE && !SILVERLIGHT
#if !NETFX_CORE && !SILVERLIGHT
			AutoMessagePackSerializerTest.Verify( expected._HashtableField, this._HashtableField );
#endif // !NETFX_CORE && !SILVERLIGHT
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

	public struct ListValueType<T> : IList<T>
	{
		private readonly List<T> _underlying;

		public T this[ int index ]
		{
			get
			{
				if ( this._underlying == null )
				{
					throw new ArgumentOutOfRangeException( "index" );
				}

				return this._underlying[ index ];
			}
			set
			{
				if ( this._underlying == null )
				{
					throw new ArgumentOutOfRangeException( "index" );
				}

				this._underlying[ index ] = value;
			}
		}

		public int Count
		{
			get { return this._underlying == null ? 0 : this._underlying.Count; }
		}

		public ListValueType( int capacity )
		{
			this._underlying = new List<T>( capacity );
		}

		public void Add( T item )
		{
			this._underlying.Add( item );
		}

		public void CopyTo( T[] array, int arrayIndex )
		{
			this._underlying.CopyTo( array, arrayIndex );
		}

		public IEnumerator<T> GetEnumerator()
		{
			if ( this._underlying == null )
			{
				yield break;
			}

			foreach ( var item in this._underlying )
			{
				yield return item;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public int IndexOf( T item )
		{
			throw new NotImplementedException();
		}

		public void Insert( int index, T item )
		{
			throw new NotImplementedException();
		}

		public void RemoveAt( int index )
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains( T item )
		{
			throw new NotImplementedException();
		}

		public bool IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		public bool Remove( T item )
		{
			throw new NotImplementedException();
		}
	}

	public struct DictionaryValueType<TKey, TValue> : IDictionary<TKey, TValue>
	{
		private readonly Dictionary<TKey, TValue> _underlying;

		public int Count
		{
			get { return this._underlying == null ? 0 : this._underlying.Count; }
		}

		public TValue this[ TKey key ]
		{
			get
			{
				if ( this._underlying == null )
				{
					throw new NotSupportedException();
				}

				return this._underlying[ key ];
			}
			set
			{
				if ( this._underlying == null )
				{
					throw new NotSupportedException();
				}

				this._underlying[ key ] = value;
			}
		}

		public DictionaryValueType( int capacity )
		{
			this._underlying = new Dictionary<TKey, TValue>( capacity );
		}

		public void Add( TKey key, TValue value )
		{
			if ( this._underlying == null )
			{
				throw new NotSupportedException();
			}

			this._underlying.Add( key, value );
		}

		public void CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
		{
			if ( this._underlying == null )
			{
				return;
			}

			( this._underlying as ICollection<KeyValuePair<TKey, TValue>> ).CopyTo( array, arrayIndex );
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if ( this._underlying == null )
			{
				yield break;
			}

			foreach ( var entry in this._underlying )
			{
				yield return entry;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public bool ContainsKey( TKey key )
		{
			throw new NotImplementedException();
		}

		public ICollection<TKey> Keys
		{
			get { throw new NotImplementedException(); }
		}

		public bool Remove( TKey key )
		{
			throw new NotImplementedException();
		}

		public bool TryGetValue( TKey key, out TValue value )
		{
			throw new NotImplementedException();
		}

		public ICollection<TValue> Values
		{
			get { throw new NotImplementedException(); }
		}

		public void Add( KeyValuePair<TKey, TValue> item )
		{
			this.Add( item.Key, item.Value );
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains( KeyValuePair<TKey, TValue> item )
		{
			throw new NotImplementedException();
		}


		public bool IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		public bool Remove( KeyValuePair<TKey, TValue> item )
		{
			throw new NotImplementedException();
		}
	}

	
	internal static class AutoMessagePackSerializerTest
	{
		internal static void Verify<T>( T expected, T actual )
		{
			if ( expected == null )
			{
				Assert.That( actual, Is.Null );
				return;
			}

#if !NETFX_CORE
			if ( expected.GetType().IsGenericType && expected.GetType().GetGenericTypeDefinition() == typeof( ArraySegment<> ) )
#else
			if ( expected.GetType().GetTypeInfo().IsGenericType && expected.GetType().GetGenericTypeDefinition() == typeof( ArraySegment<> ) )
#endif
			{
				AssertArraySegmentEquals( expected, actual );
				return;
			}

			if ( expected is DateTime )
			{
				Assert.That(
					MillisecondsDateTimeComparer.Instance.Equals( ( DateTime )( object )expected, ( DateTime )( object )actual ),
					"Expected:{1}({2},{3}){0}Actual :{4}({5},{6})",
					Environment.NewLine,
					expected,
					expected == null ? "(null)" : expected.GetType().FullName,
					( ( DateTime )( object )expected ).Kind,
					actual,
					actual == null ? "(null)" : actual.GetType().FullName,
					( ( DateTime )( object )actual ).Kind
					);
				return;
			}
			else if ( expected is DateTimeOffset )
			{
				Assert.That(
					MillisecondsDateTimeOffsetComparer.Instance.Equals( ( DateTimeOffset )( object )expected, ( DateTimeOffset )( object )actual ),
				"Expected:{1}({2}){0}Actual :{3}({4})",
				Environment.NewLine,
				expected,
				expected == null ? "(null)" : expected.GetType().FullName,
				actual,
				actual == null ? "(null)" : actual.GetType().FullName
				);
				return;
			}

			if ( expected is IDictionary )
			{
				var actuals = ( IDictionary )actual;
				foreach ( DictionaryEntry entry in ( ( IDictionary )expected ) )
				{
					Assert.That( actuals.Contains( entry.Key ), "'{0}' is not in '[{1}]'", entry.Key, String.Join( ", ", actuals.Keys.OfType<object>().Select( o => o == null ? String.Empty : o.ToString() ).ToArray() ) );
					Verify( entry.Value, actuals[ entry.Key ] );
				}
				return;
			}

			if ( expected is IEnumerable )
			{
				var expecteds = ( ( IEnumerable )expected ).Cast<Object>().ToArray();
				var actuals = ( ( IEnumerable )actual ).Cast<Object>().ToArray();
				Assert.That( expecteds.Length, Is.EqualTo( actuals.Length ) );
				for ( int i = 0; i < expecteds.Length; i++ )
				{
					Verify( expecteds[ i ], actuals[ i ] );
				}
				return;
			}

#if !NETFX_35
			if ( expected is IStructuralEquatable )
			{
				Assert.That(
					( ( IStructuralEquatable )expected ).Equals( actual, EqualityComparer<object>.Default ),
					"Expected:{1}({2}){0}Actual :{3}({4})",
					Environment.NewLine,
					expected,
					expected == null ? "(null)" : expected.GetType().FullName,
					actual,
					actual == null ? "(null)" : actual.GetType().FullName
				);
				return;
			}
#endif

			if ( expected is FILETIME )
			{
				var expectedFileTime = ( FILETIME )( object )expected;
				var actualFileTime = ( FILETIME )( object )actual;
				Verify(
					DateTime.FromFileTimeUtc( ( ( ( long )expectedFileTime.dwHighDateTime ) << 32 ) | ( expectedFileTime.dwLowDateTime & 0xffffffff ) ),
					DateTime.FromFileTimeUtc( ( ( ( long )actualFileTime.dwHighDateTime ) << 32 ) | ( actualFileTime.dwLowDateTime & 0xffffffff ) )
				);
				return;
			}

#if !NETFX_CORE
			if ( expected.GetType().IsGenericType && expected.GetType().GetGenericTypeDefinition() == typeof( KeyValuePair<,> ) )
#else
			if ( expected.GetType().GetTypeInfo().IsGenericType && expected.GetType().GetGenericTypeDefinition() == typeof( KeyValuePair<,> ) )
#endif
			{
#if !NETFX_35 && !XAMIOS && !UNITY_IPHONE
				Verify( ( ( dynamic )expected ).Key, ( ( dynamic )actual ).Key );
				Verify( ( ( dynamic )expected ).Value, ( ( dynamic )actual ).Value );
#else
				Assert.Inconclusive( ".NET 3.5 does not support dynamic." );
#endif // !NETFX_35 && !XAMIOS && !UNITY_IPHONE
				return;
			}

			if ( expected is DictionaryEntry )
			{
				var expectedEntry = ( DictionaryEntry )( object )expected;
				var actualEntry = ( DictionaryEntry )( object )actual;

				if ( expectedEntry.Key == null )
				{
					Assert.That( ( ( MessagePackObject )actualEntry.Key ).IsNil );
				}
				else
				{
					Verify( ( MessagePackObject )expectedEntry.Key, ( MessagePackObject )actualEntry.Key );
				}


				if ( expectedEntry.Value == null )
				{
					Assert.That( ( ( MessagePackObject )actualEntry.Value ).IsNil );
				}
				else
				{
					Verify( ( MessagePackObject )expectedEntry.Value, ( MessagePackObject )actualEntry.Value );
				}

				return;
			}

			Assert.That(
				EqualityComparer<T>.Default.Equals( expected, actual ),
				"Expected:{1}({2}){0}Actual :{3}({4})",
				Environment.NewLine,
				expected,
				expected == null ? "(null)" : expected.GetType().FullName,
				actual,
				actual == null ? "(null)" : actual.GetType().FullName
			);
		}
		
		private static void AssertArraySegmentEquals( object x, object y )
		{
#if !NETFX_CORE
			var type = typeof( ArraySegmentEqualityComparer<> ).MakeGenericType( x.GetType().GetGenericArguments()[ 0 ] );
			Assert.That(
				( bool )type.InvokeMember( "Equals", BindingFlags.InvokeMethod, null, Activator.CreateInstance( type ), new[] { x, y } ),
				"Expected:{1}{0}Actual :{2}",
				Environment.NewLine,
				x,
				y
			);
#else
			var elementType = x.GetType().GetTypeInfo().GenericTypeArguments[ 0 ];
			var type = typeof( ArraySegmentEqualityComparer<> ).MakeGenericType( elementType );
			Assert.That(
				( bool )type.GetRuntimeMethod( "Equals", new[] { x.GetType(), x.GetType() } ).Invoke( Activator.CreateInstance( type ), new[] { x, y } ),
				"Expected:{1}{0}Actual :{2}",
				Environment.NewLine,
				x,
				y
			);
#endif
		}
	}
	
	public class WithAbstractInt32Collection
	{
		public IList<int> Collection { get; set; }
	}
	
	public class WithAbstractStringCollection
	{
		public IList<string> Collection { get; set; }
	}

	public class WithAbstractNonCollection
	{
		public Stream NonCollection { get; set; }
	}

	public class HasEnumerable
	{
		public IEnumerable<int> Numbers { get; set; }
	}
}
