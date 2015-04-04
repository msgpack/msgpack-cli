
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

		#region -- Polymorphism --
		#region ---- KnownType ----

		#region ------ KnownType.NormalTypes ------

		public class PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteProperty
		{
			private Version _Reference;

			public Version Reference
			{
				get { return this._Reference; }
				 set { this._Reference = value; }
			}

			private PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteProperty( Version Reference ) 
			{
				this._Reference = Reference;
			}

			public PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteProperty()
			{
				this._Reference = null;
			}

			public static PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteProperty( new Version( 1, 2, 3, 4 ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject
		{
			private object _Reference;

			public object Reference
			{
				get { return this._Reference; }
				 set { this._Reference = value; }
			}

			private PolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject( object Reference ) 
			{
				this._Reference = Reference;
			}

			public PolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject()
			{
				this._Reference = null;
			}

			public static PolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ReferenceReadWritePropertyAsObject( new Version( 1, 2, 3, 4 ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteField
		{
			public  Version Reference;

			private PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteField( Version Reference ) 
			{
				this.Reference = Reference;
			}

			public PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteField()
			{
				this.Reference = null;
			}

			public static PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Normal_ReferenceReadWriteField( new Version( 1, 2, 3, 4 ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject
		{
			public  object Reference;

			private PolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject( object Reference ) 
			{
				this.Reference = Reference;
			}

			public PolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject()
			{
				this.Reference = null;
			}

			public static PolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ReferenceReadWriteFieldAsObject( new Version( 1, 2, 3, 4 ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ReferenceGetOnlyPropertyAndConstructor
		{
			private Version _Reference;

			public Version Reference
			{
				get { return this._Reference; }
			}

			public PolymorphicMemberTypeKnownType_Normal_ReferenceGetOnlyPropertyAndConstructor( Version Reference ) 
			{
				this._Reference = Reference;
			}
			public PolymorphicMemberTypeKnownType_Normal_ReferenceGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_ReferenceGetOnlyPropertyAndConstructorAsObject
		{
			private object _Reference;

			public object Reference
			{
				get { return this._Reference; }
			}

			public PolymorphicMemberTypeKnownType_ReferenceGetOnlyPropertyAndConstructorAsObject( object Reference ) 
			{
				this._Reference = Reference;
			}
			public PolymorphicMemberTypeKnownType_ReferenceGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ReferencePrivateSetterPropertyAndConstructor
		{
			private Version _Reference;

			public Version Reference
			{
				get { return this._Reference; }
				private set { this._Reference = value; }
			}

			public PolymorphicMemberTypeKnownType_Normal_ReferencePrivateSetterPropertyAndConstructor( Version Reference ) 
			{
				this._Reference = Reference;
			}
			public PolymorphicMemberTypeKnownType_Normal_ReferencePrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_ReferencePrivateSetterPropertyAndConstructorAsObject
		{
			private object _Reference;

			public object Reference
			{
				get { return this._Reference; }
				private set { this._Reference = value; }
			}

			public PolymorphicMemberTypeKnownType_ReferencePrivateSetterPropertyAndConstructorAsObject( object Reference ) 
			{
				this._Reference = Reference;
			}
			public PolymorphicMemberTypeKnownType_ReferencePrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ReferenceReadOnlyFieldAndConstructor
		{
			public readonly Version Reference;

			public PolymorphicMemberTypeKnownType_Normal_ReferenceReadOnlyFieldAndConstructor( Version Reference ) 
			{
				this.Reference = Reference;
			}
			public PolymorphicMemberTypeKnownType_Normal_ReferenceReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_ReferenceReadOnlyFieldAndConstructorAsObject
		{
			public readonly object Reference;

			public PolymorphicMemberTypeKnownType_ReferenceReadOnlyFieldAndConstructorAsObject( object Reference ) 
			{
				this.Reference = Reference;
			}
			public PolymorphicMemberTypeKnownType_ReferenceReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty
		{
			private DateTime _Value;

			public DateTime Value
			{
				get { return this._Value; }
				 set { this._Value = value; }
			}

			private PolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty( DateTime Value ) 
			{
				this._Value = Value;
			}

			public PolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty()
			{
				this._Value = default( DateTime );
			}

			public static PolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject
		{
			private object _Value;

			public object Value
			{
				get { return this._Value; }
				 set { this._Value = value; }
			}

			private PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject( object Value ) 
			{
				this._Value = Value;
			}

			public PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject()
			{
				this._Value = default( DateTime );
			}

			public static PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ValueReadWriteField
		{
			public  DateTime Value;

			private PolymorphicMemberTypeKnownType_Normal_ValueReadWriteField( DateTime Value ) 
			{
				this.Value = Value;
			}

			public PolymorphicMemberTypeKnownType_Normal_ValueReadWriteField()
			{
				this.Value = default( DateTime );
			}

			public static PolymorphicMemberTypeKnownType_Normal_ValueReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Normal_ValueReadWriteField( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject
		{
			public  object Value;

			private PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject( object Value ) 
			{
				this.Value = Value;
			}

			public PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject()
			{
				this.Value = default( DateTime );
			}

			public static PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ValueGetOnlyPropertyAndConstructor
		{
			private DateTime _Value;

			public DateTime Value
			{
				get { return this._Value; }
			}

			public PolymorphicMemberTypeKnownType_Normal_ValueGetOnlyPropertyAndConstructor( DateTime Value ) 
			{
				this._Value = Value;
			}
			public PolymorphicMemberTypeKnownType_Normal_ValueGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_ValueGetOnlyPropertyAndConstructorAsObject
		{
			private object _Value;

			public object Value
			{
				get { return this._Value; }
			}

			public PolymorphicMemberTypeKnownType_ValueGetOnlyPropertyAndConstructorAsObject( object Value ) 
			{
				this._Value = Value;
			}
			public PolymorphicMemberTypeKnownType_ValueGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ValuePrivateSetterPropertyAndConstructor
		{
			private DateTime _Value;

			public DateTime Value
			{
				get { return this._Value; }
				private set { this._Value = value; }
			}

			public PolymorphicMemberTypeKnownType_Normal_ValuePrivateSetterPropertyAndConstructor( DateTime Value ) 
			{
				this._Value = Value;
			}
			public PolymorphicMemberTypeKnownType_Normal_ValuePrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_ValuePrivateSetterPropertyAndConstructorAsObject
		{
			private object _Value;

			public object Value
			{
				get { return this._Value; }
				private set { this._Value = value; }
			}

			public PolymorphicMemberTypeKnownType_ValuePrivateSetterPropertyAndConstructorAsObject( object Value ) 
			{
				this._Value = Value;
			}
			public PolymorphicMemberTypeKnownType_ValuePrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_ValueReadOnlyFieldAndConstructor
		{
			public readonly DateTime Value;

			public PolymorphicMemberTypeKnownType_Normal_ValueReadOnlyFieldAndConstructor( DateTime Value ) 
			{
				this.Value = Value;
			}
			public PolymorphicMemberTypeKnownType_Normal_ValueReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_ValueReadOnlyFieldAndConstructorAsObject
		{
			public readonly object Value;

			public PolymorphicMemberTypeKnownType_ValueReadOnlyFieldAndConstructorAsObject( object Value ) 
			{
				this.Value = Value;
			}
			public PolymorphicMemberTypeKnownType_ValueReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteProperty
		{
			private int _Primitive;

			public int Primitive
			{
				get { return this._Primitive; }
				 set { this._Primitive = value; }
			}

			private PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteProperty( int Primitive ) 
			{
				this._Primitive = Primitive;
			}

			public PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteProperty()
			{
				this._Primitive = default( int );
			}

			public static PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteProperty( 123 );
			}
		}

		public class PolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject
		{
			private object _Primitive;

			public object Primitive
			{
				get { return this._Primitive; }
				 set { this._Primitive = value; }
			}

			private PolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject( object Primitive ) 
			{
				this._Primitive = Primitive;
			}

			public PolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject()
			{
				this._Primitive = default( int );
			}

			public static PolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_PrimitiveReadWritePropertyAsObject( 123 );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteField
		{
			public  int Primitive;

			private PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteField( int Primitive ) 
			{
				this.Primitive = Primitive;
			}

			public PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteField()
			{
				this.Primitive = default( int );
			}

			public static PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Normal_PrimitiveReadWriteField( 123 );
			}
		}

		public class PolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject
		{
			public  object Primitive;

			private PolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject( object Primitive ) 
			{
				this.Primitive = Primitive;
			}

			public PolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject()
			{
				this.Primitive = default( int );
			}

			public static PolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_PrimitiveReadWriteFieldAsObject( 123 );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PrimitiveGetOnlyPropertyAndConstructor
		{
			private int _Primitive;

			public int Primitive
			{
				get { return this._Primitive; }
			}

			public PolymorphicMemberTypeKnownType_Normal_PrimitiveGetOnlyPropertyAndConstructor( int Primitive ) 
			{
				this._Primitive = Primitive;
			}
			public PolymorphicMemberTypeKnownType_Normal_PrimitiveGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_PrimitiveGetOnlyPropertyAndConstructorAsObject
		{
			private object _Primitive;

			public object Primitive
			{
				get { return this._Primitive; }
			}

			public PolymorphicMemberTypeKnownType_PrimitiveGetOnlyPropertyAndConstructorAsObject( object Primitive ) 
			{
				this._Primitive = Primitive;
			}
			public PolymorphicMemberTypeKnownType_PrimitiveGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PrimitivePrivateSetterPropertyAndConstructor
		{
			private int _Primitive;

			public int Primitive
			{
				get { return this._Primitive; }
				private set { this._Primitive = value; }
			}

			public PolymorphicMemberTypeKnownType_Normal_PrimitivePrivateSetterPropertyAndConstructor( int Primitive ) 
			{
				this._Primitive = Primitive;
			}
			public PolymorphicMemberTypeKnownType_Normal_PrimitivePrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_PrimitivePrivateSetterPropertyAndConstructorAsObject
		{
			private object _Primitive;

			public object Primitive
			{
				get { return this._Primitive; }
				private set { this._Primitive = value; }
			}

			public PolymorphicMemberTypeKnownType_PrimitivePrivateSetterPropertyAndConstructorAsObject( object Primitive ) 
			{
				this._Primitive = Primitive;
			}
			public PolymorphicMemberTypeKnownType_PrimitivePrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PrimitiveReadOnlyFieldAndConstructor
		{
			public readonly int Primitive;

			public PolymorphicMemberTypeKnownType_Normal_PrimitiveReadOnlyFieldAndConstructor( int Primitive ) 
			{
				this.Primitive = Primitive;
			}
			public PolymorphicMemberTypeKnownType_Normal_PrimitiveReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_PrimitiveReadOnlyFieldAndConstructorAsObject
		{
			public readonly object Primitive;

			public PolymorphicMemberTypeKnownType_PrimitiveReadOnlyFieldAndConstructorAsObject( object Primitive ) 
			{
				this.Primitive = Primitive;
			}
			public PolymorphicMemberTypeKnownType_PrimitiveReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_StringReadWriteProperty
		{
			private string _String;

			public string String
			{
				get { return this._String; }
				 set { this._String = value; }
			}

			private PolymorphicMemberTypeKnownType_Normal_StringReadWriteProperty( string String ) 
			{
				this._String = String;
			}

			public PolymorphicMemberTypeKnownType_Normal_StringReadWriteProperty()
			{
				this._String = null;
			}

			public static PolymorphicMemberTypeKnownType_Normal_StringReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Normal_StringReadWriteProperty( "ABC" );
			}
		}

		public class PolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject
		{
			private object _String;

			public object String
			{
				get { return this._String; }
				 set { this._String = value; }
			}

			private PolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject( object String ) 
			{
				this._String = String;
			}

			public PolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject()
			{
				this._String = null;
			}

			public static PolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_StringReadWritePropertyAsObject( "ABC" );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_StringReadWriteField
		{
			public  string String;

			private PolymorphicMemberTypeKnownType_Normal_StringReadWriteField( string String ) 
			{
				this.String = String;
			}

			public PolymorphicMemberTypeKnownType_Normal_StringReadWriteField()
			{
				this.String = null;
			}

			public static PolymorphicMemberTypeKnownType_Normal_StringReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Normal_StringReadWriteField( "ABC" );
			}
		}

		public class PolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject
		{
			public  object String;

			private PolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject( object String ) 
			{
				this.String = String;
			}

			public PolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject()
			{
				this.String = null;
			}

			public static PolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_StringReadWriteFieldAsObject( "ABC" );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_StringGetOnlyPropertyAndConstructor
		{
			private string _String;

			public string String
			{
				get { return this._String; }
			}

			public PolymorphicMemberTypeKnownType_Normal_StringGetOnlyPropertyAndConstructor( string String ) 
			{
				this._String = String;
			}
			public PolymorphicMemberTypeKnownType_Normal_StringGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_StringGetOnlyPropertyAndConstructorAsObject
		{
			private object _String;

			public object String
			{
				get { return this._String; }
			}

			public PolymorphicMemberTypeKnownType_StringGetOnlyPropertyAndConstructorAsObject( object String ) 
			{
				this._String = String;
			}
			public PolymorphicMemberTypeKnownType_StringGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_StringPrivateSetterPropertyAndConstructor
		{
			private string _String;

			public string String
			{
				get { return this._String; }
				private set { this._String = value; }
			}

			public PolymorphicMemberTypeKnownType_Normal_StringPrivateSetterPropertyAndConstructor( string String ) 
			{
				this._String = String;
			}
			public PolymorphicMemberTypeKnownType_Normal_StringPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_StringPrivateSetterPropertyAndConstructorAsObject
		{
			private object _String;

			public object String
			{
				get { return this._String; }
				private set { this._String = value; }
			}

			public PolymorphicMemberTypeKnownType_StringPrivateSetterPropertyAndConstructorAsObject( object String ) 
			{
				this._String = String;
			}
			public PolymorphicMemberTypeKnownType_StringPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_StringReadOnlyFieldAndConstructor
		{
			public readonly string String;

			public PolymorphicMemberTypeKnownType_Normal_StringReadOnlyFieldAndConstructor( string String ) 
			{
				this.String = String;
			}
			public PolymorphicMemberTypeKnownType_Normal_StringReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_StringReadOnlyFieldAndConstructorAsObject
		{
			public readonly object String;

			public PolymorphicMemberTypeKnownType_StringReadOnlyFieldAndConstructorAsObject( object String ) 
			{
				this.String = String;
			}
			public PolymorphicMemberTypeKnownType_StringReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteProperty
		{
			private FileSystemEntry _Polymorphic;

			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public FileSystemEntry Polymorphic
			{
				get { return this._Polymorphic; }
				 set { this._Polymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteProperty( FileSystemEntry Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}

			public PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteProperty()
			{
				this._Polymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteProperty( new FileEntry { Name = "file", Size = 1 } );
			}
		}

		public class PolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObject
		{
			private object _Polymorphic;

			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public object Polymorphic
			{
				get { return this._Polymorphic; }
				 set { this._Polymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObject( object Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}

			public PolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObject()
			{
				this._Polymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_PolymorphicReadWritePropertyAsObject( new FileEntry { Name = "file", Size = 1 } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteField
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public  FileSystemEntry Polymorphic;

			private PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteField( FileSystemEntry Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}

			public PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteField()
			{
				this.Polymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Normal_PolymorphicReadWriteField( new FileEntry { Name = "file", Size = 1 } );
			}
		}

		public class PolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public  object Polymorphic;

			private PolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObject( object Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}

			public PolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObject()
			{
				this.Polymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_PolymorphicReadWriteFieldAsObject( new FileEntry { Name = "file", Size = 1 } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PolymorphicGetOnlyPropertyAndConstructor
		{
			private FileSystemEntry _Polymorphic;

			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public FileSystemEntry Polymorphic
			{
				get { return this._Polymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Normal_PolymorphicGetOnlyPropertyAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_Normal_PolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_PolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private object _Polymorphic;

			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public object Polymorphic
			{
				get { return this._Polymorphic; }
			}

			public PolymorphicMemberTypeKnownType_PolymorphicGetOnlyPropertyAndConstructorAsObject( object Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_PolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PolymorphicPrivateSetterPropertyAndConstructor
		{
			private FileSystemEntry _Polymorphic;

			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public FileSystemEntry Polymorphic
			{
				get { return this._Polymorphic; }
				private set { this._Polymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Normal_PolymorphicPrivateSetterPropertyAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_Normal_PolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_PolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private object _Polymorphic;

			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public object Polymorphic
			{
				get { return this._Polymorphic; }
				private set { this._Polymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_PolymorphicPrivateSetterPropertyAndConstructorAsObject( object Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_PolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Normal_PolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public readonly FileSystemEntry Polymorphic;

			public PolymorphicMemberTypeKnownType_Normal_PolymorphicReadOnlyFieldAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_Normal_PolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownType( 0, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( DirectoryEntry ) )]
			public readonly object Polymorphic;

			public PolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObject( object Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}
		#endregion ------ KnownType.NormalTypes ------

		#region ------ KnownType.CollectionTypes ------

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				 set { this._ListStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteProperty( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteProperty()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteProperty( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemReadWritePropertyAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				 set { this._ListStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_ListStaticItemReadWritePropertyAsObject( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_ListStaticItemReadWritePropertyAsObject()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_ListStaticItemReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListStaticItemReadWritePropertyAsObject( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteField
		{
			public  IList<string> ListStaticItem;

			private PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteField( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteField()
			{
				this.ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadWriteField( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemReadWriteFieldAsObject
		{
			public  IList<string> ListStaticItem;

			private PolymorphicMemberTypeKnownType_ListStaticItemReadWriteFieldAsObject( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_ListStaticItemReadWriteFieldAsObject()
			{
				this.ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_ListStaticItemReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListStaticItemReadWriteFieldAsObject( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyCollectionProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}

			private PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyCollectionProperty( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyCollectionProperty()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListStaticItemGetOnlyCollectionProperty( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyCollectionPropertyAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}

			private PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyCollectionPropertyAsObject( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyCollectionPropertyAsObject()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListStaticItemGetOnlyCollectionPropertyAsObject( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterCollectionProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private set { this._ListStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterCollectionProperty( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterCollectionProperty()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListStaticItemPrivateSetterCollectionProperty( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterCollectionPropertyAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private set { this._ListStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterCollectionPropertyAsObject( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterCollectionPropertyAsObject()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListStaticItemPrivateSetterCollectionPropertyAsObject( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadOnlyCollectionField
		{
			public readonly IList<string> ListStaticItem;

			private PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadOnlyCollectionField( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadOnlyCollectionField()
			{
				this.ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListStaticItemReadOnlyCollectionField( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListStaticItemReadOnlyCollectionFieldAsObject
		{
			public readonly IList<string> ListStaticItem;

			private PolymorphicMemberTypeKnownType_ListStaticItemReadOnlyCollectionFieldAsObject( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_ListStaticItemReadOnlyCollectionFieldAsObject()
			{
				this.ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_ListStaticItemReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListStaticItemReadOnlyCollectionFieldAsObject( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteProperty
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				 set { this._ListPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteProperty( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteProperty()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteProperty( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWritePropertyAsObject
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				 set { this._ListPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWritePropertyAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWritePropertyAsObject()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWritePropertyAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteField
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public  IList<FileSystemEntry> ListPolymorphicItem;

			private PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteField( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteField()
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadWriteField( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWriteFieldAsObject
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public  IList<FileSystemEntry> ListPolymorphicItem;

			private PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWriteFieldAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWriteFieldAsObject()
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListPolymorphicItemReadWriteFieldAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyCollectionProperty
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}

			private PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyCollectionProperty( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyCollectionProperty()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemGetOnlyCollectionProperty( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}

			private PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private set { this._ListPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private set { this._ListPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadOnlyCollectionField
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;

			private PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadOnlyCollectionField( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadOnlyCollectionField()
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItemReadOnlyCollectionField( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItemReadOnlyCollectionFieldAsObject
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;

			private PolymorphicMemberTypeKnownType_ListPolymorphicItemReadOnlyCollectionFieldAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItemReadOnlyCollectionFieldAsObject()
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_ListPolymorphicItemReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListPolymorphicItemReadOnlyCollectionFieldAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteProperty
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				 set { this._ListPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteProperty( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteProperty()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWritePropertyAsObject
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				 set { this._ListPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWritePropertyAsObject( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWritePropertyAsObject()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWritePropertyAsObject( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteField
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public  IList<string> ListPolymorphicItself;

			private PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteField( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteField()
			{
				this.ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadWriteField( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWriteFieldAsObject
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public  IList<string> ListPolymorphicItself;

			private PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWriteFieldAsObject( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWriteFieldAsObject()
			{
				this.ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadWriteFieldAsObject( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}

			private PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}

			private PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private set { this._ListPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private set { this._ListPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadOnlyCollectionField
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public readonly IList<string> ListPolymorphicItself;

			private PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadOnlyCollectionField( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadOnlyCollectionField()
			{
				this.ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Collection_ListPolymorphicItselfReadOnlyCollectionField( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadOnlyCollectionFieldAsObject
		{
			[MessagePackKnownType( 0, typeof( Collection<string> ) )]
			[MessagePackKnownType( 1, typeof( List<string> ) )]
			public readonly IList<string> ListPolymorphicItself;

			private PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadOnlyCollectionFieldAsObject( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadOnlyCollectionFieldAsObject()
			{
				this.ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_ListPolymorphicItselfReadOnlyCollectionFieldAsObject( new Collection<string>{ "A", "B" } );
			}
		}
		#endregion ------ KnownType.CollectionTypes ------

		#region ------ KnownType.DictionaryTypes ------

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteProperty
		{
			private IDictionary<string, string> _DictionaryStaticKeyAndStaticItem;

			public IDictionary<string, string> DictionaryStaticKeyAndStaticItem
			{
				get { return this._DictionaryStaticKeyAndStaticItem; }
				 set { this._DictionaryStaticKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteProperty( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this._DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteProperty()
			{
				this._DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadWritePropertyAsObject
		{
			private IDictionary<string, string> _DictionaryStaticKeyAndStaticItem;

			public IDictionary<string, string> DictionaryStaticKeyAndStaticItem
			{
				get { return this._DictionaryStaticKeyAndStaticItem; }
				 set { this._DictionaryStaticKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadWritePropertyAsObject( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this._DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadWritePropertyAsObject()
			{
				this._DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadWritePropertyAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteField
		{
			public  IDictionary<string, string> DictionaryStaticKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteField( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this.DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteField()
			{
				this.DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadWriteFieldAsObject
		{
			public  IDictionary<string, string> DictionaryStaticKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadWriteFieldAsObject( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this.DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadWriteFieldAsObject()
			{
				this.DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadWriteFieldAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemGetOnlyCollectionProperty
		{
			private IDictionary<string, string> _DictionaryStaticKeyAndStaticItem;

			public IDictionary<string, string> DictionaryStaticKeyAndStaticItem
			{
				get { return this._DictionaryStaticKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemGetOnlyCollectionProperty( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this._DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemGetOnlyCollectionProperty()
			{
				this._DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemGetOnlyCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemGetOnlyCollectionPropertyAsObject
		{
			private IDictionary<string, string> _DictionaryStaticKeyAndStaticItem;

			public IDictionary<string, string> DictionaryStaticKeyAndStaticItem
			{
				get { return this._DictionaryStaticKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemGetOnlyCollectionPropertyAsObject( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this._DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemGetOnlyCollectionPropertyAsObject()
			{
				this._DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemGetOnlyCollectionPropertyAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionProperty
		{
			private IDictionary<string, string> _DictionaryStaticKeyAndStaticItem;

			public IDictionary<string, string> DictionaryStaticKeyAndStaticItem
			{
				get { return this._DictionaryStaticKeyAndStaticItem; }
				private set { this._DictionaryStaticKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionProperty( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this._DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionProperty()
			{
				this._DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionPropertyAsObject
		{
			private IDictionary<string, string> _DictionaryStaticKeyAndStaticItem;

			public IDictionary<string, string> DictionaryStaticKeyAndStaticItem
			{
				get { return this._DictionaryStaticKeyAndStaticItem; }
				private set { this._DictionaryStaticKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionPropertyAsObject( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this._DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionPropertyAsObject()
			{
				this._DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionPropertyAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadOnlyCollectionField
		{
			public readonly IDictionary<string, string> DictionaryStaticKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadOnlyCollectionField( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this.DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadOnlyCollectionField()
			{
				this.DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndStaticItemReadOnlyCollectionField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadOnlyCollectionFieldAsObject
		{
			public readonly IDictionary<string, string> DictionaryStaticKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadOnlyCollectionFieldAsObject( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this.DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadOnlyCollectionFieldAsObject()
			{
				this.DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndStaticItemReadOnlyCollectionFieldAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteProperty
		{
			private IDictionary<FileSystemEntry, string> _DictionaryPolymorphicKeyAndStaticItem;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem
			{
				get { return this._DictionaryPolymorphicKeyAndStaticItem; }
				 set { this._DictionaryPolymorphicKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteProperty( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this._DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteProperty()
			{
				this._DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteProperty( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadWritePropertyAsObject
		{
			private IDictionary<FileSystemEntry, string> _DictionaryPolymorphicKeyAndStaticItem;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem
			{
				get { return this._DictionaryPolymorphicKeyAndStaticItem; }
				 set { this._DictionaryPolymorphicKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadWritePropertyAsObject( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this._DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadWritePropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadWritePropertyAsObject( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteField
		{
			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			public  IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteField( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this.DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteField()
			{
				this.DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteField( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadWriteFieldAsObject
		{
			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			public  IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadWriteFieldAsObject( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this.DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadWriteFieldAsObject()
			{
				this.DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadWriteFieldAsObject( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionProperty
		{
			private IDictionary<FileSystemEntry, string> _DictionaryPolymorphicKeyAndStaticItem;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem
			{
				get { return this._DictionaryPolymorphicKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionProperty( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this._DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionProperty()
			{
				this._DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionProperty( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionPropertyAsObject
		{
			private IDictionary<FileSystemEntry, string> _DictionaryPolymorphicKeyAndStaticItem;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem
			{
				get { return this._DictionaryPolymorphicKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionPropertyAsObject( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this._DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionPropertyAsObject( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty
		{
			private IDictionary<FileSystemEntry, string> _DictionaryPolymorphicKeyAndStaticItem;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem
			{
				get { return this._DictionaryPolymorphicKeyAndStaticItem; }
				private set { this._DictionaryPolymorphicKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this._DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty()
			{
				this._DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionPropertyAsObject
		{
			private IDictionary<FileSystemEntry, string> _DictionaryPolymorphicKeyAndStaticItem;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem
			{
				get { return this._DictionaryPolymorphicKeyAndStaticItem; }
				private set { this._DictionaryPolymorphicKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionPropertyAsObject( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this._DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionPropertyAsObject( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionField
		{
			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			public readonly IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionField( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this.DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionField()
			{
				this.DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionField( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionFieldAsObject
		{
			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			public readonly IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionFieldAsObject( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this.DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionFieldAsObject()
			{
				this.DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionFieldAsObject( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteProperty
		{
			private IDictionary<string, FileSystemEntry> _DictionaryStaticKeyAndPolymorphicItem;

			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem
			{
				get { return this._DictionaryStaticKeyAndPolymorphicItem; }
				 set { this._DictionaryStaticKeyAndPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteProperty( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this._DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteProperty()
			{
				this._DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteProperty( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadWritePropertyAsObject
		{
			private IDictionary<string, FileSystemEntry> _DictionaryStaticKeyAndPolymorphicItem;

			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem
			{
				get { return this._DictionaryStaticKeyAndPolymorphicItem; }
				 set { this._DictionaryStaticKeyAndPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadWritePropertyAsObject( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this._DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadWritePropertyAsObject()
			{
				this._DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadWritePropertyAsObject( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteField
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public  IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem;

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteField( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this.DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteField()
			{
				this.DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteField( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadWriteFieldAsObject
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public  IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem;

			private PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadWriteFieldAsObject( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this.DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadWriteFieldAsObject()
			{
				this.DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadWriteFieldAsObject( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionProperty
		{
			private IDictionary<string, FileSystemEntry> _DictionaryStaticKeyAndPolymorphicItem;

			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem
			{
				get { return this._DictionaryStaticKeyAndPolymorphicItem; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionProperty( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this._DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionProperty()
			{
				this._DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionProperty( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionPropertyAsObject
		{
			private IDictionary<string, FileSystemEntry> _DictionaryStaticKeyAndPolymorphicItem;

			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem
			{
				get { return this._DictionaryStaticKeyAndPolymorphicItem; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionPropertyAsObject( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this._DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionPropertyAsObject()
			{
				this._DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionPropertyAsObject( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty
		{
			private IDictionary<string, FileSystemEntry> _DictionaryStaticKeyAndPolymorphicItem;

			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem
			{
				get { return this._DictionaryStaticKeyAndPolymorphicItem; }
				private set { this._DictionaryStaticKeyAndPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this._DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty()
			{
				this._DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionPropertyAsObject
		{
			private IDictionary<string, FileSystemEntry> _DictionaryStaticKeyAndPolymorphicItem;

			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem
			{
				get { return this._DictionaryStaticKeyAndPolymorphicItem; }
				private set { this._DictionaryStaticKeyAndPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionPropertyAsObject( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this._DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionPropertyAsObject()
			{
				this._DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionPropertyAsObject( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionField
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public readonly IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem;

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionField( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this.DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionField()
			{
				this.DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionField( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionFieldAsObject
		{
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public readonly IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem;

			private PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionFieldAsObject( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this.DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionFieldAsObject()
			{
				this.DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionFieldAsObject( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItem;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem
			{
				get { return this._DictionaryPolymorphicKeyAndItem; }
				 set { this._DictionaryPolymorphicKeyAndItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this._DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteProperty()
			{
				this._DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadWritePropertyAsObject
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItem;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem
			{
				get { return this._DictionaryPolymorphicKeyAndItem; }
				 set { this._DictionaryPolymorphicKeyAndItem = value; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadWritePropertyAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this._DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadWritePropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadWritePropertyAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteField
		{
			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public  IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem;

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteField( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this.DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteField()
			{
				this.DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteField( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadWriteFieldAsObject
		{
			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public  IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem;

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadWriteFieldAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this.DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadWriteFieldAsObject()
			{
				this.DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadWriteFieldAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemGetOnlyCollectionProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItem;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem
			{
				get { return this._DictionaryPolymorphicKeyAndItem; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemGetOnlyCollectionProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this._DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemGetOnlyCollectionProperty()
			{
				this._DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemGetOnlyCollectionProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemGetOnlyCollectionPropertyAsObject
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItem;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem
			{
				get { return this._DictionaryPolymorphicKeyAndItem; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemGetOnlyCollectionPropertyAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this._DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemGetOnlyCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemGetOnlyCollectionPropertyAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItem;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem
			{
				get { return this._DictionaryPolymorphicKeyAndItem; }
				private set { this._DictionaryPolymorphicKeyAndItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this._DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionProperty()
			{
				this._DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionPropertyAsObject
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItem;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem
			{
				get { return this._DictionaryPolymorphicKeyAndItem; }
				private set { this._DictionaryPolymorphicKeyAndItem = value; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionPropertyAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this._DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionPropertyAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadOnlyCollectionField
		{
			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public readonly IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem;

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadOnlyCollectionField( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this.DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadOnlyCollectionField()
			{
				this.DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemReadOnlyCollectionField( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadOnlyCollectionFieldAsObject
		{
			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public readonly IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem;

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadOnlyCollectionFieldAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this.DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadOnlyCollectionFieldAsObject()
			{
				this.DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemReadOnlyCollectionFieldAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadWriteProperty
		{
			private IDictionary<string, string> _DictionaryPolymorphicItself;

			[MessagePackKnownType( 0, typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( 1, typeof( SortedDictionary<string, string> ) )]
			public IDictionary<string, string> DictionaryPolymorphicItself
			{
				get { return this._DictionaryPolymorphicItself; }
				 set { this._DictionaryPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadWriteProperty( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this._DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadWriteProperty()
			{
				this._DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadWriteProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadWritePropertyAsObject
		{
			private IDictionary<string, string> _DictionaryPolymorphicItself;

			[MessagePackKnownType( 0, typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( 1, typeof( SortedDictionary<string, string> ) )]
			public IDictionary<string, string> DictionaryPolymorphicItself
			{
				get { return this._DictionaryPolymorphicItself; }
				 set { this._DictionaryPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadWritePropertyAsObject( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this._DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadWritePropertyAsObject()
			{
				this._DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadWritePropertyAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadWriteField
		{
			[MessagePackKnownType( 0, typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( 1, typeof( SortedDictionary<string, string> ) )]
			public  IDictionary<string, string> DictionaryPolymorphicItself;

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadWriteField( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this.DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadWriteField()
			{
				this.DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadWriteField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadWriteFieldAsObject
		{
			[MessagePackKnownType( 0, typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( 1, typeof( SortedDictionary<string, string> ) )]
			public  IDictionary<string, string> DictionaryPolymorphicItself;

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadWriteFieldAsObject( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this.DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadWriteFieldAsObject()
			{
				this.DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadWriteFieldAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfGetOnlyCollectionProperty
		{
			private IDictionary<string, string> _DictionaryPolymorphicItself;

			[MessagePackKnownType( 0, typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( 1, typeof( SortedDictionary<string, string> ) )]
			public IDictionary<string, string> DictionaryPolymorphicItself
			{
				get { return this._DictionaryPolymorphicItself; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfGetOnlyCollectionProperty( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this._DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfGetOnlyCollectionProperty()
			{
				this._DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfGetOnlyCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfGetOnlyCollectionPropertyAsObject
		{
			private IDictionary<string, string> _DictionaryPolymorphicItself;

			[MessagePackKnownType( 0, typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( 1, typeof( SortedDictionary<string, string> ) )]
			public IDictionary<string, string> DictionaryPolymorphicItself
			{
				get { return this._DictionaryPolymorphicItself; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfGetOnlyCollectionPropertyAsObject( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this._DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfGetOnlyCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfGetOnlyCollectionPropertyAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfPrivateSetterCollectionProperty
		{
			private IDictionary<string, string> _DictionaryPolymorphicItself;

			[MessagePackKnownType( 0, typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( 1, typeof( SortedDictionary<string, string> ) )]
			public IDictionary<string, string> DictionaryPolymorphicItself
			{
				get { return this._DictionaryPolymorphicItself; }
				private set { this._DictionaryPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfPrivateSetterCollectionProperty( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this._DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfPrivateSetterCollectionProperty()
			{
				this._DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfPrivateSetterCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfPrivateSetterCollectionPropertyAsObject
		{
			private IDictionary<string, string> _DictionaryPolymorphicItself;

			[MessagePackKnownType( 0, typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( 1, typeof( SortedDictionary<string, string> ) )]
			public IDictionary<string, string> DictionaryPolymorphicItself
			{
				get { return this._DictionaryPolymorphicItself; }
				private set { this._DictionaryPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfPrivateSetterCollectionPropertyAsObject( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this._DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfPrivateSetterCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfPrivateSetterCollectionPropertyAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadOnlyCollectionField
		{
			[MessagePackKnownType( 0, typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( 1, typeof( SortedDictionary<string, string> ) )]
			public readonly IDictionary<string, string> DictionaryPolymorphicItself;

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadOnlyCollectionField( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this.DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadOnlyCollectionField()
			{
				this.DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicItselfReadOnlyCollectionField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadOnlyCollectionFieldAsObject
		{
			[MessagePackKnownType( 0, typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( 1, typeof( SortedDictionary<string, string> ) )]
			public readonly IDictionary<string, string> DictionaryPolymorphicItself;

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadOnlyCollectionFieldAsObject( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this.DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadOnlyCollectionFieldAsObject()
			{
				this.DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicItselfReadOnlyCollectionFieldAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItemAndItself;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself
			{
				get { return this._DictionaryPolymorphicKeyAndItemAndItself; }
				 set { this._DictionaryPolymorphicKeyAndItemAndItself = value; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteProperty()
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadWritePropertyAsObject
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItemAndItself;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself
			{
				get { return this._DictionaryPolymorphicKeyAndItemAndItself; }
				 set { this._DictionaryPolymorphicKeyAndItemAndItself = value; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadWritePropertyAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadWritePropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadWritePropertyAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteField
		{
			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public  IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself;

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteField( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteField()
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteField( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadWriteFieldAsObject
		{
			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public  IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself;

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadWriteFieldAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadWriteFieldAsObject()
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadWriteFieldAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItemAndItself;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself
			{
				get { return this._DictionaryPolymorphicKeyAndItemAndItself; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionProperty()
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionPropertyAsObject
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItemAndItself;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself
			{
				get { return this._DictionaryPolymorphicKeyAndItemAndItself; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionPropertyAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionPropertyAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItemAndItself;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself
			{
				get { return this._DictionaryPolymorphicKeyAndItemAndItself; }
				private set { this._DictionaryPolymorphicKeyAndItemAndItself = value; }
			}

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionProperty()
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionPropertyAsObject
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItemAndItself;

			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself
			{
				get { return this._DictionaryPolymorphicKeyAndItemAndItself; }
				private set { this._DictionaryPolymorphicKeyAndItemAndItself = value; }
			}

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionPropertyAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionPropertyAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionField
		{
			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public readonly IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself;

			private PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionField( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionField()
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionField( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionFieldAsObject
		{
			[MessagePackKnownDictionaryKeyType( 0, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( 0, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( DirectoryEntry ) )]
			public readonly IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself;

			private PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionFieldAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionFieldAsObject()
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionFieldAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}
		#endregion ------ KnownType.DictionaryTypes ------

		#region ------ KnownType.TupleTypes ------

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteProperty
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
				 set { this._Tuple1Static = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteProperty( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteProperty()
			{
				this._Tuple1Static = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteProperty( Tuple.Create( "1" ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1StaticReadWritePropertyAsObject
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
				 set { this._Tuple1Static = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple1StaticReadWritePropertyAsObject( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}

			public PolymorphicMemberTypeKnownType_Tuple1StaticReadWritePropertyAsObject()
			{
				this._Tuple1Static = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple1StaticReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple1StaticReadWritePropertyAsObject( Tuple.Create( "1" ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteField
		{
			public  Tuple<string> Tuple1Static;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteField( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteField()
			{
				this.Tuple1Static = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadWriteField( Tuple.Create( "1" ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1StaticReadWriteFieldAsObject
		{
			public  Tuple<string> Tuple1Static;

			private PolymorphicMemberTypeKnownType_Tuple1StaticReadWriteFieldAsObject( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}

			public PolymorphicMemberTypeKnownType_Tuple1StaticReadWriteFieldAsObject()
			{
				this.Tuple1Static = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple1StaticReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple1StaticReadWriteFieldAsObject( Tuple.Create( "1" ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
			}

			public PolymorphicMemberTypeKnownType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeKnownType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
				private set { this._Tuple1Static = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
				private set { this._Tuple1Static = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeKnownType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string> Tuple1Static;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1StaticReadOnlyFieldAndConstructorAsObject
		{
			public readonly Tuple<string> Tuple1Static;

			public PolymorphicMemberTypeKnownType_Tuple1StaticReadOnlyFieldAndConstructorAsObject( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeKnownType_Tuple1StaticReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
				 set { this._Tuple1Polymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty()
			{
				this._Tuple1Polymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWritePropertyAsObject
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
				 set { this._Tuple1Polymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWritePropertyAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWritePropertyAsObject()
			{
				this._Tuple1Polymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWritePropertyAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public  Tuple<FileSystemEntry> Tuple1Polymorphic;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField()
			{
				this.Tuple1Polymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public  Tuple<FileSystemEntry> Tuple1Polymorphic;

			private PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWriteFieldAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWriteFieldAsObject()
			{
				this.Tuple1Polymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadWriteFieldAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
				private set { this._Tuple1Polymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
				private set { this._Tuple1Polymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public readonly Tuple<FileSystemEntry> Tuple1Polymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry> ) )]
			public readonly Tuple<FileSystemEntry> Tuple1Polymorphic;

			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
				 set { this._Tuple7AllStatic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteProperty( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteProperty()
			{
				this._Tuple7AllStatic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteProperty( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWritePropertyAsObject
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
				 set { this._Tuple7AllStatic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWritePropertyAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}

			public PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWritePropertyAsObject()
			{
				this._Tuple7AllStatic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWritePropertyAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteField
		{
			public  Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteField( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteField()
			{
				this.Tuple7AllStatic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadWriteField( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWriteFieldAsObject
		{
			public  Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			private PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWriteFieldAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}

			public PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWriteFieldAsObject()
			{
				this.Tuple7AllStatic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple7AllStaticReadWriteFieldAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
				private set { this._Tuple7AllStatic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
				private set { this._Tuple7AllStatic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject
		{
			public readonly Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			public PolymorphicMemberTypeKnownType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
				 set { this._Tuple7FirstPolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty()
			{
				this._Tuple7FirstPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWritePropertyAsObject
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
				 set { this._Tuple7FirstPolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWritePropertyAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWritePropertyAsObject()
			{
				this._Tuple7FirstPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWritePropertyAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public  Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField()
			{
				this.Tuple7FirstPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public  Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWriteFieldAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWriteFieldAsObject()
			{
				this.Tuple7FirstPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadWriteFieldAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
				private set { this._Tuple7FirstPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
				private set { this._Tuple7FirstPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public readonly Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, string, string, string, string, string, string> ) )]
			public readonly Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
				 set { this._Tuple7LastPolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty()
			{
				this._Tuple7LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWritePropertyAsObject
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
				 set { this._Tuple7LastPolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWritePropertyAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWritePropertyAsObject()
			{
				this._Tuple7LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWritePropertyAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public  Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField()
			{
				this.Tuple7LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public  Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWriteFieldAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWriteFieldAsObject()
			{
				this.Tuple7LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadWriteFieldAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
				private set { this._Tuple7LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
				private set { this._Tuple7LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public readonly Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<string, string, string, string, string, string, FileSystemEntry> ) )]
			public readonly Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
				 set { this._Tuple7IntermediatePolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty()
			{
				this._Tuple7IntermediatePolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
				 set { this._Tuple7IntermediatePolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject()
			{
				this._Tuple7IntermediatePolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public  Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteField( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this.Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteField()
			{
				this.Tuple7IntermediatePolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadWriteField( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public  Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this.Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject()
			{
				this.Tuple7IntermediatePolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
				private set { this._Tuple7IntermediatePolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
				private set { this._Tuple7IntermediatePolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public readonly Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this.Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<string, string, string, FileSystemEntry, string, string, string> ) )]
			public readonly Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this.Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
				 set { this._Tuple7AllPolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty()
			{
				this._Tuple7AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWritePropertyAsObject
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
				 set { this._Tuple7AllPolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWritePropertyAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWritePropertyAsObject()
			{
				this._Tuple7AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWritePropertyAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField()
			{
				this.Tuple7AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWriteFieldAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWriteFieldAsObject()
			{
				this.Tuple7AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadWriteFieldAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
				private set { this._Tuple7AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
				private set { this._Tuple7AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> ) )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
				 set { this._Tuple8AllStatic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteProperty( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteProperty()
			{
				this._Tuple8AllStatic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteProperty( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWritePropertyAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
				 set { this._Tuple8AllStatic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWritePropertyAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}

			public PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWritePropertyAsObject()
			{
				this._Tuple8AllStatic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWritePropertyAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteField
		{
			public  Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteField( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteField()
			{
				this.Tuple8AllStatic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadWriteField( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWriteFieldAsObject
		{
			public  Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			private PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWriteFieldAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}

			public PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWriteFieldAsObject()
			{
				this.Tuple8AllStatic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple8AllStaticReadWriteFieldAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
				private set { this._Tuple8AllStatic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
				private set { this._Tuple8AllStatic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject
		{
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			public PolymorphicMemberTypeKnownType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
				 set { this._Tuple8LastPolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty()
			{
				this._Tuple8LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWritePropertyAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
				 set { this._Tuple8LastPolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWritePropertyAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWritePropertyAsObject()
			{
				this._Tuple8LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWritePropertyAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public  Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField()
			{
				this.Tuple8LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public  Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWriteFieldAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWriteFieldAsObject()
			{
				this.Tuple8LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadWriteFieldAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
				private set { this._Tuple8LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
				private set { this._Tuple8LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> ) )]
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
				 set { this._Tuple8AllPolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty()
			{
				this._Tuple8AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWritePropertyAsObject
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
				 set { this._Tuple8AllPolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWritePropertyAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWritePropertyAsObject()
			{
				this._Tuple8AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWritePropertyAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField()
			{
				this.Tuple8AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWriteFieldAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWriteFieldAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWriteFieldAsObject()
			{
				this.Tuple8AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadWriteFieldAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
				private set { this._Tuple8AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
				private set { this._Tuple8AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownTupleItemType( 1, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 2, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 3, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 4, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 5, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 6, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 7, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			[MessagePackKnownTupleItemType( 8, 0, typeof( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> ) )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}
		#endregion ------ KnownType.TupleTypes ------

		#endregion ---- KnownType ----
		#region ---- RuntimeType ----

		#region ------ RuntimeType.NormalTypes ------

		public class PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteProperty
		{
			private Version _Reference;

			public Version Reference
			{
				get { return this._Reference; }
				 set { this._Reference = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteProperty( Version Reference ) 
			{
				this._Reference = Reference;
			}

			public PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteProperty()
			{
				this._Reference = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteProperty( new Version( 1, 2, 3, 4 ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject
		{
			private object _Reference;

			public object Reference
			{
				get { return this._Reference; }
				 set { this._Reference = value; }
			}

			private PolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject( object Reference ) 
			{
				this._Reference = Reference;
			}

			public PolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject()
			{
				this._Reference = null;
			}

			public static PolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ReferenceReadWritePropertyAsObject( new Version( 1, 2, 3, 4 ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteField
		{
			public  Version Reference;

			private PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteField( Version Reference ) 
			{
				this.Reference = Reference;
			}

			public PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteField()
			{
				this.Reference = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadWriteField( new Version( 1, 2, 3, 4 ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject
		{
			public  object Reference;

			private PolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject( object Reference ) 
			{
				this.Reference = Reference;
			}

			public PolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject()
			{
				this.Reference = null;
			}

			public static PolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ReferenceReadWriteFieldAsObject( new Version( 1, 2, 3, 4 ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ReferenceGetOnlyPropertyAndConstructor
		{
			private Version _Reference;

			public Version Reference
			{
				get { return this._Reference; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_ReferenceGetOnlyPropertyAndConstructor( Version Reference ) 
			{
				this._Reference = Reference;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_ReferenceGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_ReferenceGetOnlyPropertyAndConstructorAsObject
		{
			private object _Reference;

			public object Reference
			{
				get { return this._Reference; }
			}

			public PolymorphicMemberTypeRuntimeType_ReferenceGetOnlyPropertyAndConstructorAsObject( object Reference ) 
			{
				this._Reference = Reference;
			}
			public PolymorphicMemberTypeRuntimeType_ReferenceGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ReferencePrivateSetterPropertyAndConstructor
		{
			private Version _Reference;

			public Version Reference
			{
				get { return this._Reference; }
				private set { this._Reference = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_ReferencePrivateSetterPropertyAndConstructor( Version Reference ) 
			{
				this._Reference = Reference;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_ReferencePrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_ReferencePrivateSetterPropertyAndConstructorAsObject
		{
			private object _Reference;

			public object Reference
			{
				get { return this._Reference; }
				private set { this._Reference = value; }
			}

			public PolymorphicMemberTypeRuntimeType_ReferencePrivateSetterPropertyAndConstructorAsObject( object Reference ) 
			{
				this._Reference = Reference;
			}
			public PolymorphicMemberTypeRuntimeType_ReferencePrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadOnlyFieldAndConstructor
		{
			public readonly Version Reference;

			public PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadOnlyFieldAndConstructor( Version Reference ) 
			{
				this.Reference = Reference;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_ReferenceReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_ReferenceReadOnlyFieldAndConstructorAsObject
		{
			public readonly object Reference;

			public PolymorphicMemberTypeRuntimeType_ReferenceReadOnlyFieldAndConstructorAsObject( object Reference ) 
			{
				this.Reference = Reference;
			}
			public PolymorphicMemberTypeRuntimeType_ReferenceReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty
		{
			private DateTime _Value;

			public DateTime Value
			{
				get { return this._Value; }
				 set { this._Value = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty( DateTime Value ) 
			{
				this._Value = Value;
			}

			public PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty()
			{
				this._Value = default( DateTime );
			}

			public static PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject
		{
			private object _Value;

			public object Value
			{
				get { return this._Value; }
				 set { this._Value = value; }
			}

			private PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject( object Value ) 
			{
				this._Value = Value;
			}

			public PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject()
			{
				this._Value = default( DateTime );
			}

			public static PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField
		{
			public  DateTime Value;

			private PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField( DateTime Value ) 
			{
				this.Value = Value;
			}

			public PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField()
			{
				this.Value = default( DateTime );
			}

			public static PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject
		{
			public  object Value;

			private PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject( object Value ) 
			{
				this.Value = Value;
			}

			public PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject()
			{
				this.Value = default( DateTime );
			}

			public static PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject( new DateTime( 1982, 1, 29, 15, 46, 12 ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ValueGetOnlyPropertyAndConstructor
		{
			private DateTime _Value;

			public DateTime Value
			{
				get { return this._Value; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_ValueGetOnlyPropertyAndConstructor( DateTime Value ) 
			{
				this._Value = Value;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_ValueGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_ValueGetOnlyPropertyAndConstructorAsObject
		{
			private object _Value;

			public object Value
			{
				get { return this._Value; }
			}

			public PolymorphicMemberTypeRuntimeType_ValueGetOnlyPropertyAndConstructorAsObject( object Value ) 
			{
				this._Value = Value;
			}
			public PolymorphicMemberTypeRuntimeType_ValueGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ValuePrivateSetterPropertyAndConstructor
		{
			private DateTime _Value;

			public DateTime Value
			{
				get { return this._Value; }
				private set { this._Value = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_ValuePrivateSetterPropertyAndConstructor( DateTime Value ) 
			{
				this._Value = Value;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_ValuePrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_ValuePrivateSetterPropertyAndConstructorAsObject
		{
			private object _Value;

			public object Value
			{
				get { return this._Value; }
				private set { this._Value = value; }
			}

			public PolymorphicMemberTypeRuntimeType_ValuePrivateSetterPropertyAndConstructorAsObject( object Value ) 
			{
				this._Value = Value;
			}
			public PolymorphicMemberTypeRuntimeType_ValuePrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_ValueReadOnlyFieldAndConstructor
		{
			public readonly DateTime Value;

			public PolymorphicMemberTypeRuntimeType_Normal_ValueReadOnlyFieldAndConstructor( DateTime Value ) 
			{
				this.Value = Value;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_ValueReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_ValueReadOnlyFieldAndConstructorAsObject
		{
			public readonly object Value;

			public PolymorphicMemberTypeRuntimeType_ValueReadOnlyFieldAndConstructorAsObject( object Value ) 
			{
				this.Value = Value;
			}
			public PolymorphicMemberTypeRuntimeType_ValueReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteProperty
		{
			private int _Primitive;

			public int Primitive
			{
				get { return this._Primitive; }
				 set { this._Primitive = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteProperty( int Primitive ) 
			{
				this._Primitive = Primitive;
			}

			public PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteProperty()
			{
				this._Primitive = default( int );
			}

			public static PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteProperty( 123 );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject
		{
			private object _Primitive;

			public object Primitive
			{
				get { return this._Primitive; }
				 set { this._Primitive = value; }
			}

			private PolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject( object Primitive ) 
			{
				this._Primitive = Primitive;
			}

			public PolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject()
			{
				this._Primitive = default( int );
			}

			public static PolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_PrimitiveReadWritePropertyAsObject( 123 );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteField
		{
			public  int Primitive;

			private PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteField( int Primitive ) 
			{
				this.Primitive = Primitive;
			}

			public PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteField()
			{
				this.Primitive = default( int );
			}

			public static PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadWriteField( 123 );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject
		{
			public  object Primitive;

			private PolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject( object Primitive ) 
			{
				this.Primitive = Primitive;
			}

			public PolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject()
			{
				this.Primitive = default( int );
			}

			public static PolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_PrimitiveReadWriteFieldAsObject( 123 );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PrimitiveGetOnlyPropertyAndConstructor
		{
			private int _Primitive;

			public int Primitive
			{
				get { return this._Primitive; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_PrimitiveGetOnlyPropertyAndConstructor( int Primitive ) 
			{
				this._Primitive = Primitive;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_PrimitiveGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_PrimitiveGetOnlyPropertyAndConstructorAsObject
		{
			private object _Primitive;

			public object Primitive
			{
				get { return this._Primitive; }
			}

			public PolymorphicMemberTypeRuntimeType_PrimitiveGetOnlyPropertyAndConstructorAsObject( object Primitive ) 
			{
				this._Primitive = Primitive;
			}
			public PolymorphicMemberTypeRuntimeType_PrimitiveGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PrimitivePrivateSetterPropertyAndConstructor
		{
			private int _Primitive;

			public int Primitive
			{
				get { return this._Primitive; }
				private set { this._Primitive = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_PrimitivePrivateSetterPropertyAndConstructor( int Primitive ) 
			{
				this._Primitive = Primitive;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_PrimitivePrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_PrimitivePrivateSetterPropertyAndConstructorAsObject
		{
			private object _Primitive;

			public object Primitive
			{
				get { return this._Primitive; }
				private set { this._Primitive = value; }
			}

			public PolymorphicMemberTypeRuntimeType_PrimitivePrivateSetterPropertyAndConstructorAsObject( object Primitive ) 
			{
				this._Primitive = Primitive;
			}
			public PolymorphicMemberTypeRuntimeType_PrimitivePrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadOnlyFieldAndConstructor
		{
			public readonly int Primitive;

			public PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadOnlyFieldAndConstructor( int Primitive ) 
			{
				this.Primitive = Primitive;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_PrimitiveReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_PrimitiveReadOnlyFieldAndConstructorAsObject
		{
			public readonly object Primitive;

			public PolymorphicMemberTypeRuntimeType_PrimitiveReadOnlyFieldAndConstructorAsObject( object Primitive ) 
			{
				this.Primitive = Primitive;
			}
			public PolymorphicMemberTypeRuntimeType_PrimitiveReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteProperty
		{
			private string _String;

			public string String
			{
				get { return this._String; }
				 set { this._String = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteProperty( string String ) 
			{
				this._String = String;
			}

			public PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteProperty()
			{
				this._String = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteProperty( "ABC" );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject
		{
			private object _String;

			public object String
			{
				get { return this._String; }
				 set { this._String = value; }
			}

			private PolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject( object String ) 
			{
				this._String = String;
			}

			public PolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject()
			{
				this._String = null;
			}

			public static PolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_StringReadWritePropertyAsObject( "ABC" );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteField
		{
			public  string String;

			private PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteField( string String ) 
			{
				this.String = String;
			}

			public PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteField()
			{
				this.String = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Normal_StringReadWriteField( "ABC" );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject
		{
			public  object String;

			private PolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject( object String ) 
			{
				this.String = String;
			}

			public PolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject()
			{
				this.String = null;
			}

			public static PolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_StringReadWriteFieldAsObject( "ABC" );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_StringGetOnlyPropertyAndConstructor
		{
			private string _String;

			public string String
			{
				get { return this._String; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_StringGetOnlyPropertyAndConstructor( string String ) 
			{
				this._String = String;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_StringGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_StringGetOnlyPropertyAndConstructorAsObject
		{
			private object _String;

			public object String
			{
				get { return this._String; }
			}

			public PolymorphicMemberTypeRuntimeType_StringGetOnlyPropertyAndConstructorAsObject( object String ) 
			{
				this._String = String;
			}
			public PolymorphicMemberTypeRuntimeType_StringGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_StringPrivateSetterPropertyAndConstructor
		{
			private string _String;

			public string String
			{
				get { return this._String; }
				private set { this._String = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_StringPrivateSetterPropertyAndConstructor( string String ) 
			{
				this._String = String;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_StringPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_StringPrivateSetterPropertyAndConstructorAsObject
		{
			private object _String;

			public object String
			{
				get { return this._String; }
				private set { this._String = value; }
			}

			public PolymorphicMemberTypeRuntimeType_StringPrivateSetterPropertyAndConstructorAsObject( object String ) 
			{
				this._String = String;
			}
			public PolymorphicMemberTypeRuntimeType_StringPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_StringReadOnlyFieldAndConstructor
		{
			public readonly string String;

			public PolymorphicMemberTypeRuntimeType_Normal_StringReadOnlyFieldAndConstructor( string String ) 
			{
				this.String = String;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_StringReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_StringReadOnlyFieldAndConstructorAsObject
		{
			public readonly object String;

			public PolymorphicMemberTypeRuntimeType_StringReadOnlyFieldAndConstructorAsObject( object String ) 
			{
				this.String = String;
			}
			public PolymorphicMemberTypeRuntimeType_StringReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteProperty
		{
			private FileSystemEntry _Polymorphic;

			[MessagePackRuntimeType]
			public FileSystemEntry Polymorphic
			{
				get { return this._Polymorphic; }
				 set { this._Polymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteProperty( FileSystemEntry Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteProperty()
			{
				this._Polymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteProperty( new FileEntry { Name = "file", Size = 1 } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObject
		{
			private object _Polymorphic;

			[MessagePackRuntimeType]
			public object Polymorphic
			{
				get { return this._Polymorphic; }
				 set { this._Polymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObject( object Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObject()
			{
				this._Polymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_PolymorphicReadWritePropertyAsObject( new FileEntry { Name = "file", Size = 1 } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteField
		{
			[MessagePackRuntimeType]
			public  FileSystemEntry Polymorphic;

			private PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteField( FileSystemEntry Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteField()
			{
				this.Polymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadWriteField( new FileEntry { Name = "file", Size = 1 } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeType]
			public  object Polymorphic;

			private PolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObject( object Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObject()
			{
				this.Polymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_PolymorphicReadWriteFieldAsObject( new FileEntry { Name = "file", Size = 1 } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PolymorphicGetOnlyPropertyAndConstructor
		{
			private FileSystemEntry _Polymorphic;

			[MessagePackRuntimeType]
			public FileSystemEntry Polymorphic
			{
				get { return this._Polymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicGetOnlyPropertyAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_PolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private object _Polymorphic;

			[MessagePackRuntimeType]
			public object Polymorphic
			{
				get { return this._Polymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_PolymorphicGetOnlyPropertyAndConstructorAsObject( object Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_PolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PolymorphicPrivateSetterPropertyAndConstructor
		{
			private FileSystemEntry _Polymorphic;

			[MessagePackRuntimeType]
			public FileSystemEntry Polymorphic
			{
				get { return this._Polymorphic; }
				private set { this._Polymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicPrivateSetterPropertyAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_PolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private object _Polymorphic;

			[MessagePackRuntimeType]
			public object Polymorphic
			{
				get { return this._Polymorphic; }
				private set { this._Polymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_PolymorphicPrivateSetterPropertyAndConstructorAsObject( object Polymorphic ) 
			{
				this._Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_PolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeType]
			public readonly FileSystemEntry Polymorphic;

			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadOnlyFieldAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Normal_PolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_PolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeType]
			public readonly object Polymorphic;

			public PolymorphicMemberTypeRuntimeType_PolymorphicReadOnlyFieldAndConstructorAsObject( object Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_PolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}
		#endregion ------ RuntimeType.NormalTypes ------

		#region ------ RuntimeType.CollectionTypes ------

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				 set { this._ListStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteProperty( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteProperty()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteProperty( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemReadWritePropertyAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				 set { this._ListStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_ListStaticItemReadWritePropertyAsObject( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_ListStaticItemReadWritePropertyAsObject()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListStaticItemReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListStaticItemReadWritePropertyAsObject( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteField
		{
			public  IList<string> ListStaticItem;

			private PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteField( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteField()
			{
				this.ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadWriteField( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemReadWriteFieldAsObject
		{
			public  IList<string> ListStaticItem;

			private PolymorphicMemberTypeRuntimeType_ListStaticItemReadWriteFieldAsObject( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_ListStaticItemReadWriteFieldAsObject()
			{
				this.ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListStaticItemReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListStaticItemReadWriteFieldAsObject( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyCollectionProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyCollectionProperty( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyCollectionProperty()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemGetOnlyCollectionProperty( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyCollectionPropertyAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}

			private PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyCollectionPropertyAsObject( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyCollectionPropertyAsObject()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListStaticItemGetOnlyCollectionPropertyAsObject( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterCollectionProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private set { this._ListStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterCollectionProperty( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterCollectionProperty()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemPrivateSetterCollectionProperty( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterCollectionPropertyAsObject
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private set { this._ListStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterCollectionPropertyAsObject( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterCollectionPropertyAsObject()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListStaticItemPrivateSetterCollectionPropertyAsObject( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadOnlyCollectionField
		{
			public readonly IList<string> ListStaticItem;

			private PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadOnlyCollectionField( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadOnlyCollectionField()
			{
				this.ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListStaticItemReadOnlyCollectionField( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListStaticItemReadOnlyCollectionFieldAsObject
		{
			public readonly IList<string> ListStaticItem;

			private PolymorphicMemberTypeRuntimeType_ListStaticItemReadOnlyCollectionFieldAsObject( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_ListStaticItemReadOnlyCollectionFieldAsObject()
			{
				this.ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListStaticItemReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListStaticItemReadOnlyCollectionFieldAsObject( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteProperty
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				 set { this._ListPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteProperty( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteProperty()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteProperty( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWritePropertyAsObject
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				 set { this._ListPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWritePropertyAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWritePropertyAsObject()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWritePropertyAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteField
		{
			[MessagePackRuntimeCollectionItemType]
			public  IList<FileSystemEntry> ListPolymorphicItem;

			private PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteField( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteField()
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadWriteField( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWriteFieldAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			public  IList<FileSystemEntry> ListPolymorphicItem;

			private PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWriteFieldAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWriteFieldAsObject()
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadWriteFieldAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyCollectionProperty
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyCollectionProperty( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyCollectionProperty()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemGetOnlyCollectionProperty( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}

			private PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemGetOnlyCollectionPropertyAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private set { this._ListPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemPrivateSetterCollectionProperty( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private set { this._ListPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemPrivateSetterCollectionPropertyAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadOnlyCollectionField
		{
			[MessagePackRuntimeCollectionItemType]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;

			private PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadOnlyCollectionField( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadOnlyCollectionField()
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItemReadOnlyCollectionField( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadOnlyCollectionFieldAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;

			private PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadOnlyCollectionFieldAsObject( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadOnlyCollectionFieldAsObject()
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListPolymorphicItemReadOnlyCollectionFieldAsObject( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteProperty
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackRuntimeType]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				 set { this._ListPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteProperty( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteProperty()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWritePropertyAsObject
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackRuntimeType]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				 set { this._ListPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWritePropertyAsObject( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWritePropertyAsObject()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWritePropertyAsObject( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteField
		{
			[MessagePackRuntimeType]
			public  IList<string> ListPolymorphicItself;

			private PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteField( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteField()
			{
				this.ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadWriteField( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWriteFieldAsObject
		{
			[MessagePackRuntimeType]
			public  IList<string> ListPolymorphicItself;

			private PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWriteFieldAsObject( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWriteFieldAsObject()
			{
				this.ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadWriteFieldAsObject( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackRuntimeType]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}

			private PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfGetOnlyCollectionProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackRuntimeType]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}

			private PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfGetOnlyCollectionPropertyAsObject( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackRuntimeType]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private set { this._ListPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfPrivateSetterCollectionProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackRuntimeType]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private set { this._ListPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfPrivateSetterCollectionPropertyAsObject( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadOnlyCollectionField
		{
			[MessagePackRuntimeType]
			public readonly IList<string> ListPolymorphicItself;

			private PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadOnlyCollectionField( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadOnlyCollectionField()
			{
				this.ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Collection_ListPolymorphicItselfReadOnlyCollectionField( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadOnlyCollectionFieldAsObject
		{
			[MessagePackRuntimeType]
			public readonly IList<string> ListPolymorphicItself;

			private PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadOnlyCollectionFieldAsObject( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadOnlyCollectionFieldAsObject()
			{
				this.ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_ListPolymorphicItselfReadOnlyCollectionFieldAsObject( new Collection<string>{ "A", "B" } );
			}
		}
		#endregion ------ RuntimeType.CollectionTypes ------

		#region ------ RuntimeType.DictionaryTypes ------

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteProperty
		{
			private IDictionary<string, string> _DictionaryStaticKeyAndStaticItem;

			public IDictionary<string, string> DictionaryStaticKeyAndStaticItem
			{
				get { return this._DictionaryStaticKeyAndStaticItem; }
				 set { this._DictionaryStaticKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteProperty( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this._DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteProperty()
			{
				this._DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadWritePropertyAsObject
		{
			private IDictionary<string, string> _DictionaryStaticKeyAndStaticItem;

			public IDictionary<string, string> DictionaryStaticKeyAndStaticItem
			{
				get { return this._DictionaryStaticKeyAndStaticItem; }
				 set { this._DictionaryStaticKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadWritePropertyAsObject( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this._DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadWritePropertyAsObject()
			{
				this._DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadWritePropertyAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteField
		{
			public  IDictionary<string, string> DictionaryStaticKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteField( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this.DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteField()
			{
				this.DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadWriteField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadWriteFieldAsObject
		{
			public  IDictionary<string, string> DictionaryStaticKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadWriteFieldAsObject( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this.DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadWriteFieldAsObject()
			{
				this.DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadWriteFieldAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemGetOnlyCollectionProperty
		{
			private IDictionary<string, string> _DictionaryStaticKeyAndStaticItem;

			public IDictionary<string, string> DictionaryStaticKeyAndStaticItem
			{
				get { return this._DictionaryStaticKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemGetOnlyCollectionProperty( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this._DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemGetOnlyCollectionProperty()
			{
				this._DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemGetOnlyCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemGetOnlyCollectionPropertyAsObject
		{
			private IDictionary<string, string> _DictionaryStaticKeyAndStaticItem;

			public IDictionary<string, string> DictionaryStaticKeyAndStaticItem
			{
				get { return this._DictionaryStaticKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemGetOnlyCollectionPropertyAsObject( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this._DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemGetOnlyCollectionPropertyAsObject()
			{
				this._DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemGetOnlyCollectionPropertyAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionProperty
		{
			private IDictionary<string, string> _DictionaryStaticKeyAndStaticItem;

			public IDictionary<string, string> DictionaryStaticKeyAndStaticItem
			{
				get { return this._DictionaryStaticKeyAndStaticItem; }
				private set { this._DictionaryStaticKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionProperty( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this._DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionProperty()
			{
				this._DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionPropertyAsObject
		{
			private IDictionary<string, string> _DictionaryStaticKeyAndStaticItem;

			public IDictionary<string, string> DictionaryStaticKeyAndStaticItem
			{
				get { return this._DictionaryStaticKeyAndStaticItem; }
				private set { this._DictionaryStaticKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionPropertyAsObject( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this._DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionPropertyAsObject()
			{
				this._DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemPrivateSetterCollectionPropertyAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadOnlyCollectionField
		{
			public readonly IDictionary<string, string> DictionaryStaticKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadOnlyCollectionField( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this.DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadOnlyCollectionField()
			{
				this.DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndStaticItemReadOnlyCollectionField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadOnlyCollectionFieldAsObject
		{
			public readonly IDictionary<string, string> DictionaryStaticKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadOnlyCollectionFieldAsObject( IDictionary<string, string> DictionaryStaticKeyAndStaticItem ) 
			{
				this.DictionaryStaticKeyAndStaticItem = DictionaryStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadOnlyCollectionFieldAsObject()
			{
				this.DictionaryStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndStaticItemReadOnlyCollectionFieldAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteProperty
		{
			private IDictionary<FileSystemEntry, string> _DictionaryPolymorphicKeyAndStaticItem;

			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem
			{
				get { return this._DictionaryPolymorphicKeyAndStaticItem; }
				 set { this._DictionaryPolymorphicKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteProperty( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this._DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteProperty()
			{
				this._DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteProperty( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadWritePropertyAsObject
		{
			private IDictionary<FileSystemEntry, string> _DictionaryPolymorphicKeyAndStaticItem;

			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem
			{
				get { return this._DictionaryPolymorphicKeyAndStaticItem; }
				 set { this._DictionaryPolymorphicKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadWritePropertyAsObject( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this._DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadWritePropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadWritePropertyAsObject( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteField
		{
			[MessagePackRuntimeDictionaryKeyType]
			public  IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteField( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this.DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteField()
			{
				this.DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadWriteField( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadWriteFieldAsObject
		{
			[MessagePackRuntimeDictionaryKeyType]
			public  IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadWriteFieldAsObject( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this.DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadWriteFieldAsObject()
			{
				this.DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadWriteFieldAsObject( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionProperty
		{
			private IDictionary<FileSystemEntry, string> _DictionaryPolymorphicKeyAndStaticItem;

			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem
			{
				get { return this._DictionaryPolymorphicKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionProperty( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this._DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionProperty()
			{
				this._DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionProperty( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionPropertyAsObject
		{
			private IDictionary<FileSystemEntry, string> _DictionaryPolymorphicKeyAndStaticItem;

			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem
			{
				get { return this._DictionaryPolymorphicKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionPropertyAsObject( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this._DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemGetOnlyCollectionPropertyAsObject( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty
		{
			private IDictionary<FileSystemEntry, string> _DictionaryPolymorphicKeyAndStaticItem;

			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem
			{
				get { return this._DictionaryPolymorphicKeyAndStaticItem; }
				private set { this._DictionaryPolymorphicKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this._DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty()
			{
				this._DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionPropertyAsObject
		{
			private IDictionary<FileSystemEntry, string> _DictionaryPolymorphicKeyAndStaticItem;

			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem
			{
				get { return this._DictionaryPolymorphicKeyAndStaticItem; }
				private set { this._DictionaryPolymorphicKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionPropertyAsObject( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this._DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemPrivateSetterCollectionPropertyAsObject( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionField
		{
			[MessagePackRuntimeDictionaryKeyType]
			public readonly IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionField( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this.DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionField()
			{
				this.DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionField( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionFieldAsObject
		{
			[MessagePackRuntimeDictionaryKeyType]
			public readonly IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionFieldAsObject( IDictionary<FileSystemEntry, string> DictionaryPolymorphicKeyAndStaticItem ) 
			{
				this.DictionaryPolymorphicKeyAndStaticItem = DictionaryPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionFieldAsObject()
			{
				this.DictionaryPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndStaticItemReadOnlyCollectionFieldAsObject( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteProperty
		{
			private IDictionary<string, FileSystemEntry> _DictionaryStaticKeyAndPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem
			{
				get { return this._DictionaryStaticKeyAndPolymorphicItem; }
				 set { this._DictionaryStaticKeyAndPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteProperty( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this._DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteProperty()
			{
				this._DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteProperty( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadWritePropertyAsObject
		{
			private IDictionary<string, FileSystemEntry> _DictionaryStaticKeyAndPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem
			{
				get { return this._DictionaryStaticKeyAndPolymorphicItem; }
				 set { this._DictionaryStaticKeyAndPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadWritePropertyAsObject( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this._DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadWritePropertyAsObject()
			{
				this._DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadWritePropertyAsObject( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteField
		{
			[MessagePackRuntimeCollectionItemType]
			public  IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem;

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteField( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this.DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteField()
			{
				this.DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadWriteField( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadWriteFieldAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			public  IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem;

			private PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadWriteFieldAsObject( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this.DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadWriteFieldAsObject()
			{
				this.DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadWriteFieldAsObject( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionProperty
		{
			private IDictionary<string, FileSystemEntry> _DictionaryStaticKeyAndPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem
			{
				get { return this._DictionaryStaticKeyAndPolymorphicItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionProperty( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this._DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionProperty()
			{
				this._DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionProperty( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionPropertyAsObject
		{
			private IDictionary<string, FileSystemEntry> _DictionaryStaticKeyAndPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem
			{
				get { return this._DictionaryStaticKeyAndPolymorphicItem; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionPropertyAsObject( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this._DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionPropertyAsObject()
			{
				this._DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemGetOnlyCollectionPropertyAsObject( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty
		{
			private IDictionary<string, FileSystemEntry> _DictionaryStaticKeyAndPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem
			{
				get { return this._DictionaryStaticKeyAndPolymorphicItem; }
				private set { this._DictionaryStaticKeyAndPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this._DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty()
			{
				this._DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionPropertyAsObject
		{
			private IDictionary<string, FileSystemEntry> _DictionaryStaticKeyAndPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem
			{
				get { return this._DictionaryStaticKeyAndPolymorphicItem; }
				private set { this._DictionaryStaticKeyAndPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionPropertyAsObject( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this._DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionPropertyAsObject()
			{
				this._DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemPrivateSetterCollectionPropertyAsObject( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionField
		{
			[MessagePackRuntimeCollectionItemType]
			public readonly IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem;

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionField( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this.DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionField()
			{
				this.DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionField( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionFieldAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			public readonly IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem;

			private PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionFieldAsObject( IDictionary<string, FileSystemEntry> DictionaryStaticKeyAndPolymorphicItem ) 
			{
				this.DictionaryStaticKeyAndPolymorphicItem = DictionaryStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionFieldAsObject()
			{
				this.DictionaryStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryStaticKeyAndPolymorphicItemReadOnlyCollectionFieldAsObject( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItem;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem
			{
				get { return this._DictionaryPolymorphicKeyAndItem; }
				 set { this._DictionaryPolymorphicKeyAndItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this._DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteProperty()
			{
				this._DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadWritePropertyAsObject
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItem;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem
			{
				get { return this._DictionaryPolymorphicKeyAndItem; }
				 set { this._DictionaryPolymorphicKeyAndItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadWritePropertyAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this._DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadWritePropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadWritePropertyAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteField
		{
			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public  IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem;

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteField( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this.DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteField()
			{
				this.DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadWriteField( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadWriteFieldAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public  IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem;

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadWriteFieldAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this.DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadWriteFieldAsObject()
			{
				this.DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadWriteFieldAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemGetOnlyCollectionProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItem;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem
			{
				get { return this._DictionaryPolymorphicKeyAndItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemGetOnlyCollectionProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this._DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemGetOnlyCollectionProperty()
			{
				this._DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemGetOnlyCollectionProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemGetOnlyCollectionPropertyAsObject
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItem;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem
			{
				get { return this._DictionaryPolymorphicKeyAndItem; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemGetOnlyCollectionPropertyAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this._DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemGetOnlyCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemGetOnlyCollectionPropertyAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItem;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem
			{
				get { return this._DictionaryPolymorphicKeyAndItem; }
				private set { this._DictionaryPolymorphicKeyAndItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this._DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionProperty()
			{
				this._DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionPropertyAsObject
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItem;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem
			{
				get { return this._DictionaryPolymorphicKeyAndItem; }
				private set { this._DictionaryPolymorphicKeyAndItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionPropertyAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this._DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemPrivateSetterCollectionPropertyAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadOnlyCollectionField
		{
			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public readonly IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem;

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadOnlyCollectionField( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this.DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadOnlyCollectionField()
			{
				this.DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemReadOnlyCollectionField( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadOnlyCollectionFieldAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public readonly IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem;

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadOnlyCollectionFieldAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItem ) 
			{
				this.DictionaryPolymorphicKeyAndItem = DictionaryPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadOnlyCollectionFieldAsObject()
			{
				this.DictionaryPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemReadOnlyCollectionFieldAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadWriteProperty
		{
			private IDictionary<string, string> _DictionaryPolymorphicItself;

			[MessagePackRuntimeType]
			public IDictionary<string, string> DictionaryPolymorphicItself
			{
				get { return this._DictionaryPolymorphicItself; }
				 set { this._DictionaryPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadWriteProperty( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this._DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadWriteProperty()
			{
				this._DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadWriteProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadWritePropertyAsObject
		{
			private IDictionary<string, string> _DictionaryPolymorphicItself;

			[MessagePackRuntimeType]
			public IDictionary<string, string> DictionaryPolymorphicItself
			{
				get { return this._DictionaryPolymorphicItself; }
				 set { this._DictionaryPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadWritePropertyAsObject( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this._DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadWritePropertyAsObject()
			{
				this._DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadWritePropertyAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadWriteField
		{
			[MessagePackRuntimeType]
			public  IDictionary<string, string> DictionaryPolymorphicItself;

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadWriteField( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this.DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadWriteField()
			{
				this.DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadWriteField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadWriteFieldAsObject
		{
			[MessagePackRuntimeType]
			public  IDictionary<string, string> DictionaryPolymorphicItself;

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadWriteFieldAsObject( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this.DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadWriteFieldAsObject()
			{
				this.DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadWriteFieldAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfGetOnlyCollectionProperty
		{
			private IDictionary<string, string> _DictionaryPolymorphicItself;

			[MessagePackRuntimeType]
			public IDictionary<string, string> DictionaryPolymorphicItself
			{
				get { return this._DictionaryPolymorphicItself; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfGetOnlyCollectionProperty( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this._DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfGetOnlyCollectionProperty()
			{
				this._DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfGetOnlyCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfGetOnlyCollectionPropertyAsObject
		{
			private IDictionary<string, string> _DictionaryPolymorphicItself;

			[MessagePackRuntimeType]
			public IDictionary<string, string> DictionaryPolymorphicItself
			{
				get { return this._DictionaryPolymorphicItself; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfGetOnlyCollectionPropertyAsObject( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this._DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfGetOnlyCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfGetOnlyCollectionPropertyAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfPrivateSetterCollectionProperty
		{
			private IDictionary<string, string> _DictionaryPolymorphicItself;

			[MessagePackRuntimeType]
			public IDictionary<string, string> DictionaryPolymorphicItself
			{
				get { return this._DictionaryPolymorphicItself; }
				private set { this._DictionaryPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfPrivateSetterCollectionProperty( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this._DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfPrivateSetterCollectionProperty()
			{
				this._DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfPrivateSetterCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfPrivateSetterCollectionPropertyAsObject
		{
			private IDictionary<string, string> _DictionaryPolymorphicItself;

			[MessagePackRuntimeType]
			public IDictionary<string, string> DictionaryPolymorphicItself
			{
				get { return this._DictionaryPolymorphicItself; }
				private set { this._DictionaryPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfPrivateSetterCollectionPropertyAsObject( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this._DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfPrivateSetterCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfPrivateSetterCollectionPropertyAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadOnlyCollectionField
		{
			[MessagePackRuntimeType]
			public readonly IDictionary<string, string> DictionaryPolymorphicItself;

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadOnlyCollectionField( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this.DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadOnlyCollectionField()
			{
				this.DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicItselfReadOnlyCollectionField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadOnlyCollectionFieldAsObject
		{
			[MessagePackRuntimeType]
			public readonly IDictionary<string, string> DictionaryPolymorphicItself;

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadOnlyCollectionFieldAsObject( IDictionary<string, string> DictionaryPolymorphicItself ) 
			{
				this.DictionaryPolymorphicItself = DictionaryPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadOnlyCollectionFieldAsObject()
			{
				this.DictionaryPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicItselfReadOnlyCollectionFieldAsObject( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItemAndItself;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself
			{
				get { return this._DictionaryPolymorphicKeyAndItemAndItself; }
				 set { this._DictionaryPolymorphicKeyAndItemAndItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteProperty()
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadWritePropertyAsObject
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItemAndItself;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself
			{
				get { return this._DictionaryPolymorphicKeyAndItemAndItself; }
				 set { this._DictionaryPolymorphicKeyAndItemAndItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadWritePropertyAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadWritePropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadWritePropertyAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteField
		{
			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public  IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself;

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteField( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteField()
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadWriteField( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadWriteFieldAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public  IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself;

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadWriteFieldAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadWriteFieldAsObject()
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadWriteFieldAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItemAndItself;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself
			{
				get { return this._DictionaryPolymorphicKeyAndItemAndItself; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionProperty()
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionPropertyAsObject
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItemAndItself;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself
			{
				get { return this._DictionaryPolymorphicKeyAndItemAndItself; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionPropertyAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfGetOnlyCollectionPropertyAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItemAndItself;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself
			{
				get { return this._DictionaryPolymorphicKeyAndItemAndItself; }
				private set { this._DictionaryPolymorphicKeyAndItemAndItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionProperty()
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionPropertyAsObject
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictionaryPolymorphicKeyAndItemAndItself;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself
			{
				get { return this._DictionaryPolymorphicKeyAndItemAndItself; }
				private set { this._DictionaryPolymorphicKeyAndItemAndItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionPropertyAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionPropertyAsObject()
			{
				this._DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionPropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfPrivateSetterCollectionPropertyAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionField
		{
			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public readonly IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself;

			private PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionField( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionField()
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dictionary_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionField( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionFieldAsObject
		{
			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public readonly IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself;

			private PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionFieldAsObject( IDictionary<FileSystemEntry, FileSystemEntry> DictionaryPolymorphicKeyAndItemAndItself ) 
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = DictionaryPolymorphicKeyAndItemAndItself;
			}

			public PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionFieldAsObject()
			{
				this.DictionaryPolymorphicKeyAndItemAndItself = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_DictionaryPolymorphicKeyAndItemAndItselfReadOnlyCollectionFieldAsObject( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}
		#endregion ------ RuntimeType.DictionaryTypes ------

		#region ------ RuntimeType.TupleTypes ------

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteProperty
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
				 set { this._Tuple1Static = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteProperty( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteProperty()
			{
				this._Tuple1Static = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteProperty( Tuple.Create( "1" ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWritePropertyAsObject
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
				 set { this._Tuple1Static = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWritePropertyAsObject( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWritePropertyAsObject()
			{
				this._Tuple1Static = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWritePropertyAsObject( Tuple.Create( "1" ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteField
		{
			public  Tuple<string> Tuple1Static;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteField( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteField()
			{
				this.Tuple1Static = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadWriteField( Tuple.Create( "1" ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWriteFieldAsObject
		{
			public  Tuple<string> Tuple1Static;

			private PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWriteFieldAsObject( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWriteFieldAsObject()
			{
				this.Tuple1Static = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple1StaticReadWriteFieldAsObject( Tuple.Create( "1" ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple1StaticGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
				private set { this._Tuple1Static = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string> _Tuple1Static;

			public Tuple<string> Tuple1Static
			{
				get { return this._Tuple1Static; }
				private set { this._Tuple1Static = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject( Tuple<string> Tuple1Static ) 
			{
				this._Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple1StaticPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string> Tuple1Static;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1StaticReadOnlyFieldAndConstructorAsObject
		{
			public readonly Tuple<string> Tuple1Static;

			public PolymorphicMemberTypeRuntimeType_Tuple1StaticReadOnlyFieldAndConstructorAsObject( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple1StaticReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteProperty
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
				 set { this._Tuple1Polymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteProperty( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteProperty()
			{
				this._Tuple1Polymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteProperty( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWritePropertyAsObject
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
				 set { this._Tuple1Polymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWritePropertyAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWritePropertyAsObject()
			{
				this._Tuple1Polymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWritePropertyAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public  Tuple<FileSystemEntry> Tuple1Polymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteField( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteField()
			{
				this.Tuple1Polymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadWriteField( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public  Tuple<FileSystemEntry> Tuple1Polymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWriteFieldAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWriteFieldAsObject()
			{
				this.Tuple1Polymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadWriteFieldAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
				private set { this._Tuple1Polymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry> Tuple1Polymorphic
			{
				get { return this._Tuple1Polymorphic; }
				private set { this._Tuple1Polymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this._Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public readonly Tuple<FileSystemEntry> Tuple1Polymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public readonly Tuple<FileSystemEntry> Tuple1Polymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple1PolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
				 set { this._Tuple7AllStatic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteProperty( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteProperty()
			{
				this._Tuple7AllStatic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteProperty( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWritePropertyAsObject
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
				 set { this._Tuple7AllStatic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWritePropertyAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWritePropertyAsObject()
			{
				this._Tuple7AllStatic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWritePropertyAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteField
		{
			public  Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteField( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteField()
			{
				this.Tuple7AllStatic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadWriteField( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWriteFieldAsObject
		{
			public  Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			private PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWriteFieldAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWriteFieldAsObject()
			{
				this.Tuple7AllStatic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadWriteFieldAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7" ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
				private set { this._Tuple7AllStatic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string> _Tuple7AllStatic;

			public Tuple<string, string, string, string, string, string, string> Tuple7AllStatic
			{
				get { return this._Tuple7AllStatic; }
				private set { this._Tuple7AllStatic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this._Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject
		{
			public readonly Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7AllStaticReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteProperty
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
				 set { this._Tuple7FirstPolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteProperty( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteProperty()
			{
				this._Tuple7FirstPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteProperty( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWritePropertyAsObject
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
				 set { this._Tuple7FirstPolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWritePropertyAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWritePropertyAsObject()
			{
				this._Tuple7FirstPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWritePropertyAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public  Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteField( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteField()
			{
				this.Tuple7FirstPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadWriteField( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public  Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWriteFieldAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWriteFieldAsObject()
			{
				this.Tuple7FirstPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadWriteFieldAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, "2", "3", "4", "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
				private set { this._Tuple7FirstPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic
			{
				get { return this._Tuple7FirstPolymorphic; }
				private set { this._Tuple7FirstPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this._Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public readonly Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public readonly Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7FirstPolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackRuntimeTupleItemType( 7 )]
			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
				 set { this._Tuple7LastPolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteProperty( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteProperty()
			{
				this._Tuple7LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteProperty( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWritePropertyAsObject
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackRuntimeTupleItemType( 7 )]
			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
				 set { this._Tuple7LastPolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWritePropertyAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWritePropertyAsObject()
			{
				this._Tuple7LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWritePropertyAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			public  Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteField( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteField()
			{
				this.Tuple7LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadWriteField( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			public  Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWriteFieldAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWriteFieldAsObject()
			{
				this.Tuple7LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadWriteFieldAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackRuntimeTupleItemType( 7 )]
			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackRuntimeTupleItemType( 7 )]
			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackRuntimeTupleItemType( 7 )]
			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
				private set { this._Tuple7LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackRuntimeTupleItemType( 7 )]
			public Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic
			{
				get { return this._Tuple7LastPolymorphic; }
				private set { this._Tuple7LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this._Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			public readonly Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 7 )]
			public readonly Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7LastPolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
				 set { this._Tuple7IntermediatePolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty()
			{
				this._Tuple7IntermediatePolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteProperty( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
				 set { this._Tuple7IntermediatePolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject()
			{
				this._Tuple7IntermediatePolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWritePropertyAsObject( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			public  Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteField( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this.Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteField()
			{
				this.Tuple7IntermediatePolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadWriteField( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			public  Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this.Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject()
			{
				this.Tuple7IntermediatePolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadWriteFieldAsObject( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
				private set { this._Tuple7IntermediatePolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7IntermediatePolymorphic;

			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic
			{
				get { return this._Tuple7IntermediatePolymorphic; }
				private set { this._Tuple7IntermediatePolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this._Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			public readonly Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this.Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			public readonly Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7IntermediatePolymorphic ) 
			{
				this.Tuple7IntermediatePolymorphic = Tuple7IntermediatePolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7IntermediatePolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteProperty
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
				 set { this._Tuple7AllPolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteProperty( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteProperty()
			{
				this._Tuple7AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteProperty( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWritePropertyAsObject
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
				 set { this._Tuple7AllPolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWritePropertyAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWritePropertyAsObject()
			{
				this._Tuple7AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWritePropertyAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteField( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteField()
			{
				this.Tuple7AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadWriteField( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWriteFieldAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWriteFieldAsObject()
			{
				this.Tuple7AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadWriteFieldAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
				private set { this._Tuple7AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic
			{
				get { return this._Tuple7AllPolymorphic; }
				private set { this._Tuple7AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this._Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple7AllPolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
				 set { this._Tuple8AllStatic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteProperty( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteProperty()
			{
				this._Tuple8AllStatic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteProperty( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWritePropertyAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
				 set { this._Tuple8AllStatic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWritePropertyAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWritePropertyAsObject()
			{
				this._Tuple8AllStatic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWritePropertyAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteField
		{
			public  Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteField( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteField()
			{
				this.Tuple8AllStatic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadWriteField( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWriteFieldAsObject
		{
			public  Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			private PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWriteFieldAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWriteFieldAsObject()
			{
				this.Tuple8AllStatic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadWriteFieldAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", "8" ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
				private set { this._Tuple8AllStatic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<string>> _Tuple8AllStatic;

			public Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic
			{
				get { return this._Tuple8AllStatic; }
				private set { this._Tuple8AllStatic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this._Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject
		{
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple8AllStaticReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackRuntimeTupleItemType( 8 )]
			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
				 set { this._Tuple8LastPolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteProperty( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteProperty()
			{
				this._Tuple8LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteProperty( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWritePropertyAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackRuntimeTupleItemType( 8 )]
			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
				 set { this._Tuple8LastPolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWritePropertyAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWritePropertyAsObject()
			{
				this._Tuple8LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWritePropertyAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			public  Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteField( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteField()
			{
				this.Tuple8LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadWriteField( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			public  Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWriteFieldAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWriteFieldAsObject()
			{
				this.Tuple8LastPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadWriteFieldAsObject( Tuple.Create( "1", "2", "3", "4", "5", "6", "7", new FileEntry { Name = "8", Size = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackRuntimeTupleItemType( 8 )]
			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackRuntimeTupleItemType( 8 )]
			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackRuntimeTupleItemType( 8 )]
			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
				private set { this._Tuple8LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackRuntimeTupleItemType( 8 )]
			public Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic
			{
				get { return this._Tuple8LastPolymorphic; }
				private set { this._Tuple8LastPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this._Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 8 )]
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple8LastPolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteProperty
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
				 set { this._Tuple8AllPolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteProperty( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteProperty()
			{
				this._Tuple8AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteProperty( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWritePropertyAsObject
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
				 set { this._Tuple8AllPolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWritePropertyAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWritePropertyAsObject()
			{
				this._Tuple8AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWritePropertyAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWritePropertyAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteField( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteField()
			{
				this.Tuple8AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadWriteField( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWriteFieldAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public  Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWriteFieldAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWriteFieldAsObject()
			{
				this.Tuple8AllPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWriteFieldAsObject Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadWriteFieldAsObject( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileSystemEntry, new DirectoryEntry { Name = "2", ChildCount = 2 } as FileSystemEntry, new FileEntry { Name = "3", Size = 3 } as FileSystemEntry, new DirectoryEntry { Name = "4", ChildCount = 4 } as FileSystemEntry, new FileEntry { Name = "5", Size = 5 } as FileSystemEntry, new DirectoryEntry { Name = "6", ChildCount = 6 } as FileSystemEntry, new FileEntry { Name = "7", Size = 7 } as FileSystemEntry, new DirectoryEntry { Name = "8", ChildCount = 8 } as FileSystemEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicGetOnlyPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
				private set { this._Tuple8AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic
			{
				get { return this._Tuple8AllPolymorphic; }
				private set { this._Tuple8AllPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this._Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructorAsObject() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			[MessagePackRuntimeTupleItemType( 2 )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			[MessagePackRuntimeTupleItemType( 5 )]
			[MessagePackRuntimeTupleItemType( 6 )]
			[MessagePackRuntimeTupleItemType( 7 )]
			[MessagePackRuntimeTupleItemType( 8 )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple8AllPolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}
		#endregion ------ RuntimeType.TupleTypes ------

		#endregion ---- RuntimeType ----
		public class PolymorphicMemberTypeMixed
		{
			public string NormalVanilla { get; set; }
			[MessagePackRuntimeType]
			public FileSystemEntry NormalRuntime { get; set; }
			[MessagePackKnownType( 1, typeof( FileEntry ) )]
			[MessagePackKnownType( 2, typeof( DirectoryEntry ) )]
			public FileSystemEntry NormalKnown { get; set; }
			[MessagePackRuntimeType]
			public Object ObjectRuntime { get; set; }
			public IList<string> ListVanilla { get; set; }
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 2, typeof( DirectoryEntry ) )]
			public IList<FileSystemEntry> ListKnownItem { get; set; }
			[MessagePackKnownType( 1, typeof( Collection<FileSystemEntry> ) )]
			[MessagePackKnownType( 2, typeof( List<FileSystemEntry> ) )]
			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> ListKnwonContainerRuntimeItem { get; set; }
			[MessagePackRuntimeCollectionItemType]
			public IList<object> ListObjectRuntimeItem { get; set; }
			public IDictionary<string, string> DictionaryVanilla { get; set; }
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 2, typeof( DirectoryEntry ) )]
			public IDictionary<string, FileSystemEntry> DictionaryKnownValue { get; set; }
			[MessagePackKnownType( 1, typeof( SortedDictionary<string, FileSystemEntry> ) )]
			[MessagePackKnownType( 2, typeof( Dictionary<string, FileSystemEntry> ) )]
			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, FileSystemEntry> DictionaryKnownContainerRuntimeValue { get; set; }
			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, object> DictionaryObjectRuntimeValue { get; set; }
			[MessagePackKnownTupleItemType( 2, 1, typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, 2, typeof( DirectoryEntry ) )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<string, FileSystemEntry, FileSystemEntry, object> Tuple { get; set; }

			public PolymorphicMemberTypeMixed() { }
		}

		public class AbstractClassMemberNoAttribute
		{
			public AbstractFileSystemEntry Value { get; set; }

			public AbstractClassMemberNoAttribute() { }
		}

		public class AbstractClassMemberKnownType
		{
			[MessagePackKnownType( 1, typeof( FileEntry ) )]
			public AbstractFileSystemEntry Value { get; set; }

			public AbstractClassMemberKnownType() { }
		}

		public class AbstractClassMemberRuntimeType
		{
			[MessagePackRuntimeType]
			public AbstractFileSystemEntry Value { get; set; }

			public AbstractClassMemberRuntimeType() { }
		}

		public class AbstractClassCollectionItemNoAttribute
		{
			public IList<AbstractFileSystemEntry> Value { get; set; }

			public AbstractClassCollectionItemNoAttribute() { }
		}

		public class AbstractClassCollectionItemKnownType
		{
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			public IList<AbstractFileSystemEntry> Value { get; set; }

			public AbstractClassCollectionItemKnownType() { }
		}

		public class AbstractClassCollectionItemRuntimeType
		{
			[MessagePackRuntimeCollectionItemType]
			public IList<AbstractFileSystemEntry> Value { get; set; }

			public AbstractClassCollectionItemRuntimeType() { }
		}

		public class AbstractClassDictionaryKeyNoAttribute
		{
			public IDictionary<AbstractFileSystemEntry, string> Value { get; set; }

			public AbstractClassDictionaryKeyNoAttribute() { }
		}

		public class AbstractClassDictionaryKeyKnownType
		{
			[MessagePackKnownDictionaryKeyType( 1, typeof( FileEntry ) )]
			public IDictionary<AbstractFileSystemEntry, string> Value { get; set; }

			public AbstractClassDictionaryKeyKnownType() { }
		}

		public class AbstractClassDictionaryKeyRuntimeType
		{
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<AbstractFileSystemEntry, string> Value { get; set; }

			public AbstractClassDictionaryKeyRuntimeType() { }
		}

		public class InterfaceMemberNoAttribute
		{
			public IFileSystemEntry Value { get; set; }

			public InterfaceMemberNoAttribute() { }
		}

		public class InterfaceMemberKnownType
		{
			[MessagePackKnownType( 1, typeof( FileEntry ) )]
			public IFileSystemEntry Value { get; set; }

			public InterfaceMemberKnownType() { }
		}

		public class InterfaceMemberRuntimeType
		{
			[MessagePackRuntimeType]
			public IFileSystemEntry Value { get; set; }

			public InterfaceMemberRuntimeType() { }
		}

		public class InterfaceCollectionItemNoAttribute
		{
			public IList<IFileSystemEntry> Value { get; set; }

			public InterfaceCollectionItemNoAttribute() { }
		}

		public class InterfaceCollectionItemKnownType
		{
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			public IList<IFileSystemEntry> Value { get; set; }

			public InterfaceCollectionItemKnownType() { }
		}

		public class InterfaceCollectionItemRuntimeType
		{
			[MessagePackRuntimeCollectionItemType]
			public IList<IFileSystemEntry> Value { get; set; }

			public InterfaceCollectionItemRuntimeType() { }
		}

		public class InterfaceDictionaryKeyNoAttribute
		{
			public IDictionary<IFileSystemEntry, string> Value { get; set; }

			public InterfaceDictionaryKeyNoAttribute() { }
		}

		public class InterfaceDictionaryKeyKnownType
		{
			[MessagePackKnownDictionaryKeyType( 1, typeof( FileEntry ) )]
			public IDictionary<IFileSystemEntry, string> Value { get; set; }

			public InterfaceDictionaryKeyKnownType() { }
		}

		public class InterfaceDictionaryKeyRuntimeType
		{
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<IFileSystemEntry, string> Value { get; set; }

			public InterfaceDictionaryKeyRuntimeType() { }
		}

	public class EchoKeyedCollection<T> : KeyedCollection<T, T>
	{
		protected override T GetKeyForItem( T item )
		{
			return item;
		}
	}

		public class AbstractClassCollectionNoAttribute
		{
			public KeyedCollection<string, string> Value { get; set; }

			public AbstractClassCollectionNoAttribute() { }
		}

		public class AbstractClassCollectionKnownType
		{
			[MessagePackKnownType( 1, typeof( EchoKeyedCollection<string> ) )]
			public KeyedCollection<string, string> Value { get; set; }

			public AbstractClassCollectionKnownType() { }
		}

		public class AbstractClassCollectionRuntimeType
		{
			[MessagePackRuntimeType]
			public KeyedCollection<string, string> Value { get; set; }

			public AbstractClassCollectionRuntimeType() { }
		}

		public class InterfaceCollectionNoAttribute
		{
			public IList<string> Value { get; set; }

			public InterfaceCollectionNoAttribute() { }
		}

		public class InterfaceCollectionKnownType
		{
			[MessagePackKnownType( 1, typeof( EchoKeyedCollection<string> ) )]
			public IList<string> Value { get; set; }

			public InterfaceCollectionKnownType() { }
		}

		public class InterfaceCollectionRuntimeType
		{
			[MessagePackRuntimeType]
			public IList<string> Value { get; set; }

			public InterfaceCollectionRuntimeType() { }
		}

		public class TupleAbstractType
		{
			[MessagePackKnownTupleItemType( 1, 1, typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, 1, typeof( FileEntry ) )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<AbstractFileSystemEntry, IFileSystemEntry, AbstractFileSystemEntry, IFileSystemEntry> Value { get; set; }

			public TupleAbstractType() { }
		}

		public class DuplicatedKnownMember
		{
			[MessagePackKnownType( 1, typeof( FileEntry ) )]
			[MessagePackKnownType( 1, typeof( FileEntry ) )]
			public FileSystemEntry Value  { get; set; }

			public DuplicatedKnownMember() { }
		}

		public class DuplicatedKnownCollectionItem
		{
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			public IList<FileSystemEntry> Value  { get; set; }

			public DuplicatedKnownCollectionItem() { }
		}

		public class DuplicatedKnownDictionaryKey
		{
			[MessagePackKnownDictionaryKeyType( 1, typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( 1, typeof( FileEntry ) )]
			public IDictionary<FileSystemEntry, string> Value  { get; set; }

			public DuplicatedKnownDictionaryKey() { }
		}

		public class DuplicatedKnownTupleItem
		{
			[MessagePackKnownTupleItemType( 1, 1, typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, 1, typeof( FileEntry ) )]
			public Tuple<FileSystemEntry> Value  { get; set; }

			public DuplicatedKnownTupleItem() { }
		}

		public class KnownAndRuntimeMember
		{
			[MessagePackKnownType( 1, typeof( FileEntry ) )]
			[MessagePackRuntimeType]
			public FileSystemEntry Value  { get; set; }

			public KnownAndRuntimeMember() { }
		}

		public class KnownAndRuntimeCollectionItem
		{
			[MessagePackKnownCollectionItemType( 1, typeof( FileEntry ) )]
			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> Value  { get; set; }

			public KnownAndRuntimeCollectionItem() { }
		}

		public class KnownAndRuntimeDictionaryKey
		{
			[MessagePackKnownDictionaryKeyType( 1, typeof( FileEntry ) )]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, string> Value  { get; set; }

			public KnownAndRuntimeDictionaryKey() { }
		}

		public class KnownAndRuntimeTupleItem
		{
			[MessagePackKnownTupleItemType( 1, 1, typeof( FileEntry ) )]
			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry> Value  { get; set; }

			public KnownAndRuntimeTupleItem() { }
		}

		public interface IFileSystemEntry { }

		public abstract class AbstractFileSystemEntry : IFileSystemEntry { }

		public class FileSystemEntry : AbstractFileSystemEntry, IComparable<FileSystemEntry>
		{
			public string Name { get; set; }

			public override bool Equals( object obj )
			{
				var other = obj as FileSystemEntry;
				if ( Object.ReferenceEquals( other, null ) )
				{
					return false;
				}

				return this.Name == other.Name;
			}

			public override int GetHashCode()
			{
				return ( this.Name ?? String.Empty ).GetHashCode();
			}

			int IComparable<FileSystemEntry>.CompareTo( FileSystemEntry other )
			{
				return String.Compare( this.Name, other.Name, StringComparison.Ordinal );
			}
		}

		public class FileEntry : FileSystemEntry
		{
			public long Size { get; set; }

			public override bool Equals( object obj )
			{
				var other = obj as FileEntry;
				if ( Object.ReferenceEquals( other, null ) )
				{
					return false;
				}

				return this.Name == other.Name && this.Size == other.Size;
			}

			public override int GetHashCode()
			{
				return ( this.Name ?? String.Empty ).GetHashCode() ^ this.Size.GetHashCode();
			}

			public override string ToString()
			{
				return "File(Name=" + this.Name + ", Size=" + this.Size + ")";
			}
		}

		public class DirectoryEntry : FileSystemEntry
		{
			public int ChildCount { get; set; }

			public override bool Equals( object obj )
			{
				var other = obj as DirectoryEntry;
				if ( Object.ReferenceEquals( other, null ) )
				{
					return false;
				}

				return this.Name == other.Name && this.ChildCount == other.ChildCount;
			}

			public override int GetHashCode()
			{
				return ( this.Name ?? String.Empty ).GetHashCode() ^ this.ChildCount.GetHashCode();
			}

			public override string ToString()
			{
				return "Directory(Name=" + this.Name + ", ChildCount=" + this.ChildCount + ")";
			}
		}

		#endregion -- Polymorphism --
}
