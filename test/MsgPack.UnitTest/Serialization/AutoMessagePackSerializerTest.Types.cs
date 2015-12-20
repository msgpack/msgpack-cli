
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

#pragma warning disable 3003
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
		private CultureInfo _InvariantCultureField;
		
		public CultureInfo InvariantCultureField
		{
			get { return this._InvariantCultureField; }
			set { this._InvariantCultureField = value; }
		}
		private CultureInfo _CurrentCultureField;
		
		public CultureInfo CurrentCultureField
		{
			get { return this._CurrentCultureField; }
			set { this._CurrentCultureField = value; }
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
		private HashSet<DateTime> _HashSetDateTimeField = new HashSet<DateTime>( DictionaryTestHelper.GetEqualityComparer<DateTime>() );
		
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
		private ISet<DateTime> _ISetDateTimeField = new HashSet<DateTime>( DictionaryTestHelper.GetEqualityComparer<DateTime>() );
		
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
		private System.Collections.Generic.Dictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> _Dictionary_MessagePackObject_MessagePackObjectField = new Dictionary<MessagePackObject, MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() );
		
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
		private System.Collections.Generic.HashSet<MsgPack.MessagePackObject> _HashSet_MessagePackObjectField = new HashSet<MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() );
		
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
		private System.Collections.Generic.ISet<MsgPack.MessagePackObject> _ISet_MessagePackObjectField = new HashSet<MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() );
		
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
		private System.Collections.Generic.IDictionary<MsgPack.MessagePackObject, MsgPack.MessagePackObject> _IDictionary_MessagePackObject_MessagePackObjectField = new Dictionary<MessagePackObject, MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() );
		
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
			this._InvariantCultureField = CultureInfo.InvariantCulture;
			this._CurrentCultureField = CultureInfo.CurrentCulture;
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
			this._HashSetDateTimeField = new HashSet<DateTime>( DictionaryTestHelper.GetEqualityComparer<DateTime>() ){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
			this._ICollectionDateTimeField = new SimpleCollection<DateTime>(){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
#if !NETFX_35
			this._ISetDateTimeField = new HashSet<DateTime>( DictionaryTestHelper.GetEqualityComparer<DateTime>() ){ DateTime.UtcNow.Subtract( TimeSpan.FromDays( 1 ) ), DateTime.UtcNow };
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
			this._Dictionary_MessagePackObject_MessagePackObjectField = new Dictionary<MessagePackObject, MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() ){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } };
			this._Collection_MessagePackObjectField = new Collection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._StringKeyedCollection_MessagePackObjectField = new StringKeyedCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#if !NETFX_35
			this._ObservableCollection_MessagePackObjectField = new ObservableCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#endif // !NETFX_35
			this._HashSet_MessagePackObjectField = new HashSet<MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() ){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._ICollection_MessagePackObjectField = new SimpleCollection<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#if !NETFX_35
			this._ISet_MessagePackObjectField = new HashSet<MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() ){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
#endif // !NETFX_35
			this._IList_MessagePackObjectField = new List<MessagePackObject>(){ new MessagePackObject( 1 ), new MessagePackObject( 2 ) };
			this._IDictionary_MessagePackObject_MessagePackObjectField = new Dictionary<MessagePackObject, MessagePackObject>( DictionaryTestHelper.GetEqualityComparer<MessagePackObject>() ){ { new MessagePackObject( "1" ), new MessagePackObject( 1 ) }, { new MessagePackObject( "2" ), new MessagePackObject( 2 ) } };
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
			AutoMessagePackSerializerTest.Verify( expected._InvariantCultureField, this._InvariantCultureField );
			AutoMessagePackSerializerTest.Verify( expected._CurrentCultureField, this._CurrentCultureField );
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
			this._underlying = new Dictionary<TKey, TValue>( capacity, DictionaryTestHelper.GetEqualityComparer<TKey>() );
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
					( DateTime )( object )expected == ( DateTime )( object )actual,
					"Expected:{1:O}({2},{3}){0}Actual :{4:O}({5},{6})",
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
					( DateTimeOffset )( object )expected == ( DateTimeOffset )( object )actual,
					"Expected:{1:O}({2}){0}Actual :{3:O}({4})",
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
#if !UNITY
				EqualityComparer<T>.Default.Equals( expected, actual ),
#else
				AotHelper.GetEqualityComparer<T>().Equals( expected, actual ),
#endif // !UNITY
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

	public class AnnotatedDateTimes
	{
		public DateTime VanillaDateTimeProperty { get; set; }

		public DateTime VanillaDateTimeField;


		[MessagePackDateTimeMember( DateTimeConversionMethod = DateTimeMemberConversionMethod.Default )]
		public DateTime DefaultDateTimeProperty { get; set; }

		[MessagePackDateTimeMember( DateTimeConversionMethod = DateTimeMemberConversionMethod.Default )]
		public DateTime DefaultDateTimeField;


		[MessagePackDateTimeMember( DateTimeConversionMethod = DateTimeMemberConversionMethod.Native )]
		public DateTime NativeDateTimeProperty { get; set; }

		[MessagePackDateTimeMember( DateTimeConversionMethod = DateTimeMemberConversionMethod.Native )]
		public DateTime NativeDateTimeField;


		[MessagePackDateTimeMember( DateTimeConversionMethod = DateTimeMemberConversionMethod.UnixEpoc )]
		public DateTime UnixEpocDateTimeProperty { get; set; }

		[MessagePackDateTimeMember( DateTimeConversionMethod = DateTimeMemberConversionMethod.UnixEpoc )]
		public DateTime UnixEpocDateTimeField;

		
		
		public DateTimeOffset VanillaDateTimeOffsetProperty { get; set; }
		
		public DateTimeOffset VanillaDateTimeOffsetField;
		
		
		[MessagePackDateTimeMember( DateTimeConversionMethod = DateTimeMemberConversionMethod.Default )]
		public DateTimeOffset DefaultDateTimeOffsetProperty { get; set; }
		
		[MessagePackDateTimeMember( DateTimeConversionMethod = DateTimeMemberConversionMethod.Default )]
		public DateTimeOffset DefaultDateTimeOffsetField;
		
		
		[MessagePackDateTimeMember( DateTimeConversionMethod = DateTimeMemberConversionMethod.Native )]
		public DateTimeOffset NativeDateTimeOffsetProperty { get; set; }
		
		[MessagePackDateTimeMember( DateTimeConversionMethod = DateTimeMemberConversionMethod.Native )]
		public DateTimeOffset NativeDateTimeOffsetField;
		
		
		[MessagePackDateTimeMember( DateTimeConversionMethod = DateTimeMemberConversionMethod.UnixEpoc )]
		public DateTimeOffset UnixEpocDateTimeOffsetProperty { get; set; }
		
		[MessagePackDateTimeMember( DateTimeConversionMethod = DateTimeMemberConversionMethod.UnixEpoc )]
		public DateTimeOffset UnixEpocDateTimeOffsetField;



		public AnnotatedDateTimes() {}

		public AnnotatedDateTimes( DateTimeOffset now )
		{
			VanillaDateTimeProperty = now.DateTime;
			VanillaDateTimeField = now.DateTime.AddDays( 1 );
			DefaultDateTimeProperty = now.DateTime.AddDays( 2 );
			DefaultDateTimeField = now.DateTime.AddDays( 3 );
			NativeDateTimeProperty = now.DateTime.AddDays( 4 );
			NativeDateTimeField = now.DateTime.AddDays( 5 );
			UnixEpocDateTimeProperty = now.DateTime.AddDays( 6 );
			UnixEpocDateTimeField = now.DateTime.AddDays( 7 );
			VanillaDateTimeOffsetProperty = now.AddDays( 8 );
			VanillaDateTimeOffsetField = now.AddDays( 9 );
			DefaultDateTimeOffsetProperty = now.AddDays( 10 );
			DefaultDateTimeOffsetField = now.AddDays( 11 );
			NativeDateTimeOffsetProperty = now.AddDays( 12 );
			NativeDateTimeOffsetField = now.AddDays( 13 );
			UnixEpocDateTimeOffsetProperty = now.AddDays( 14 );
			UnixEpocDateTimeOffsetField = now.AddDays( 15 );
		}
	}

	internal class DictionaryTestHelper
	{
		public static IEqualityComparer<T> GetEqualityComparer<T>()
		{
#if !UNITY
			return EqualityComparer<T>.Default;
#else
			return AotHelper.GetEqualityComparer<T>();
#endif // !UNITY
		}
	}

#if !UNITY
	// Mono 2.7.3 AOT fails when these classes are used...
	// Issue 119
	public abstract class GenericClass<T>
	{
		public T GenericField;
		public virtual T GenericProperty { get; internal set; }

		public GenericClass() {}
	}

	public class GenericValueClass : GenericClass<int> { }

	public class GenericReferenceClass : GenericClass<string> { }

	public abstract class GenericRecordClass<T>
	{
		public readonly T GenericField;

		private readonly T _genericProperty;
		public virtual T GenericProperty { get { return this._genericProperty; } }

		public GenericRecordClass( T genericField, T genericProperty )
		{
			this.GenericField = genericField;
			this._genericProperty = genericProperty;
		}
	}

	public class GenericRecordValueClass : GenericRecordClass<int>
	{
		public GenericRecordValueClass( int genericField, int genericProperty )
			: base( genericField, genericProperty )	{ }
	}

	public class GenericRecordReferenceClass : GenericRecordClass<string>
	{
		public GenericRecordReferenceClass( string genericField, string genericProperty )
			: base( genericField, genericProperty )	{ }
	}
#endif // !UNITY

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
				return new PolymorphicMemberTypeKnownType_Normal_ValueReadWriteProperty( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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
				return new PolymorphicMemberTypeKnownType_ValueReadWritePropertyAsObject( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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
				return new PolymorphicMemberTypeKnownType_Normal_ValueReadWriteField( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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
				return new PolymorphicMemberTypeKnownType_ValueReadWriteFieldAsObject( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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

			[MessagePackKnownType( "0", typeof( FileEntry ) )]
			[MessagePackKnownType( "1", typeof( DirectoryEntry ) )]
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

			[MessagePackKnownType( "0", typeof( FileEntry ) )]
			[MessagePackKnownType( "1", typeof( DirectoryEntry ) )]
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
			[MessagePackKnownType( "0", typeof( FileEntry ) )]
			[MessagePackKnownType( "1", typeof( DirectoryEntry ) )]
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
			[MessagePackKnownType( "0", typeof( FileEntry ) )]
			[MessagePackKnownType( "1", typeof( DirectoryEntry ) )]
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

			[MessagePackKnownType( "0", typeof( FileEntry ) )]
			[MessagePackKnownType( "1", typeof( DirectoryEntry ) )]
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

			[MessagePackKnownType( "0", typeof( FileEntry ) )]
			[MessagePackKnownType( "1", typeof( DirectoryEntry ) )]
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

			[MessagePackKnownType( "0", typeof( FileEntry ) )]
			[MessagePackKnownType( "1", typeof( DirectoryEntry ) )]
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

			[MessagePackKnownType( "0", typeof( FileEntry ) )]
			[MessagePackKnownType( "1", typeof( DirectoryEntry ) )]
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
			[MessagePackKnownType( "0", typeof( FileEntry ) )]
			[MessagePackKnownType( "1", typeof( DirectoryEntry ) )]
			public readonly FileSystemEntry Polymorphic;

			public PolymorphicMemberTypeKnownType_Normal_PolymorphicReadOnlyFieldAndConstructor( FileSystemEntry Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_Normal_PolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObject
		{
			[MessagePackKnownType( "0", typeof( FileEntry ) )]
			[MessagePackKnownType( "1", typeof( DirectoryEntry ) )]
			public readonly object Polymorphic;

			public PolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObject( object Polymorphic ) 
			{
				this.Polymorphic = Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_PolymorphicReadOnlyFieldAndConstructorAsObject() {}
		}
		#endregion ------ KnownType.NormalTypes ------

		#region ------ KnownType.CollectionTypes ------

		public class PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				 set { this._ListStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteProperty( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteProperty()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteProperty( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteField
		{
			public  IList<string> ListStaticItem;

			private PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteField( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteField()
			{
				this.ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListStaticItemReadWriteField( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListStaticItemGetOnlyCollectionProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}

			private PolymorphicMemberTypeKnownType_List_ListStaticItemGetOnlyCollectionProperty( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListStaticItemGetOnlyCollectionProperty()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListStaticItemGetOnlyCollectionProperty( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListStaticItemPrivateSetterCollectionProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private set { this._ListStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_List_ListStaticItemPrivateSetterCollectionProperty( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListStaticItemPrivateSetterCollectionProperty()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListStaticItemPrivateSetterCollectionProperty( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListStaticItemReadOnlyCollectionField
		{
			public readonly IList<string> ListStaticItem;

			private PolymorphicMemberTypeKnownType_List_ListStaticItemReadOnlyCollectionField( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListStaticItemReadOnlyCollectionField()
			{
				this.ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListStaticItemReadOnlyCollectionField( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteProperty
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				 set { this._ListPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteProperty( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteProperty()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteProperty( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteField
		{
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public  IList<FileSystemEntry> ListPolymorphicItem;

			private PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteField( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteField()
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadWriteField( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListPolymorphicItemGetOnlyCollectionProperty
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}

			private PolymorphicMemberTypeKnownType_List_ListPolymorphicItemGetOnlyCollectionProperty( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListPolymorphicItemGetOnlyCollectionProperty()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListPolymorphicItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListPolymorphicItemGetOnlyCollectionProperty( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListPolymorphicItemPrivateSetterCollectionProperty
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private set { this._ListPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeKnownType_List_ListPolymorphicItemPrivateSetterCollectionProperty( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListPolymorphicItemPrivateSetterCollectionProperty()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListPolymorphicItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListPolymorphicItemPrivateSetterCollectionProperty( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadOnlyCollectionField
		{
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;

			private PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadOnlyCollectionField( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadOnlyCollectionField()
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListPolymorphicItemReadOnlyCollectionField( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteProperty
		{
			private IList<object> _ListObjectItem;

			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IList<object> ListObjectItem
			{
				get { return this._ListObjectItem; }
				 set { this._ListObjectItem = value; }
			}

			private PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteProperty( IList<object> ListObjectItem ) 
			{
				this._ListObjectItem = ListObjectItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteProperty()
			{
				this._ListObjectItem = new List<object>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteProperty( new List<object>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteField
		{
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public  IList<object> ListObjectItem;

			private PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteField( IList<object> ListObjectItem ) 
			{
				this.ListObjectItem = ListObjectItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteField()
			{
				this.ListObjectItem = new List<object>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListObjectItemReadWriteField( new List<object>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListObjectItemGetOnlyCollectionProperty
		{
			private IList<object> _ListObjectItem;

			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IList<object> ListObjectItem
			{
				get { return this._ListObjectItem; }
			}

			private PolymorphicMemberTypeKnownType_List_ListObjectItemGetOnlyCollectionProperty( IList<object> ListObjectItem ) 
			{
				this._ListObjectItem = ListObjectItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListObjectItemGetOnlyCollectionProperty()
			{
				this._ListObjectItem = new List<object>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListObjectItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListObjectItemGetOnlyCollectionProperty( new List<object>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListObjectItemPrivateSetterCollectionProperty
		{
			private IList<object> _ListObjectItem;

			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IList<object> ListObjectItem
			{
				get { return this._ListObjectItem; }
				private set { this._ListObjectItem = value; }
			}

			private PolymorphicMemberTypeKnownType_List_ListObjectItemPrivateSetterCollectionProperty( IList<object> ListObjectItem ) 
			{
				this._ListObjectItem = ListObjectItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListObjectItemPrivateSetterCollectionProperty()
			{
				this._ListObjectItem = new List<object>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListObjectItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListObjectItemPrivateSetterCollectionProperty( new List<object>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListObjectItemReadOnlyCollectionField
		{
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public readonly IList<object> ListObjectItem;

			private PolymorphicMemberTypeKnownType_List_ListObjectItemReadOnlyCollectionField( IList<object> ListObjectItem ) 
			{
				this.ListObjectItem = ListObjectItem;
			}

			public PolymorphicMemberTypeKnownType_List_ListObjectItemReadOnlyCollectionField()
			{
				this.ListObjectItem = new List<object>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListObjectItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListObjectItemReadOnlyCollectionField( new List<object>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteProperty
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackKnownType( "0", typeof( Collection<string> ) )]
			[MessagePackKnownType( "1", typeof( List<string> ) )]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				 set { this._ListPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteProperty( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteProperty()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteField
		{
			[MessagePackKnownType( "0", typeof( Collection<string> ) )]
			[MessagePackKnownType( "1", typeof( List<string> ) )]
			public  IList<string> ListPolymorphicItself;

			private PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteField( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteField()
			{
				this.ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadWriteField( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfGetOnlyCollectionProperty
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackKnownType( "0", typeof( Collection<string> ) )]
			[MessagePackKnownType( "1", typeof( List<string> ) )]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}

			private PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfGetOnlyCollectionProperty( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfGetOnlyCollectionProperty()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfGetOnlyCollectionProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfPrivateSetterCollectionProperty
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackKnownType( "0", typeof( Collection<string> ) )]
			[MessagePackKnownType( "1", typeof( List<string> ) )]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private set { this._ListPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfPrivateSetterCollectionProperty( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfPrivateSetterCollectionProperty()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfPrivateSetterCollectionProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadOnlyCollectionField
		{
			[MessagePackKnownType( "0", typeof( Collection<string> ) )]
			[MessagePackKnownType( "1", typeof( List<string> ) )]
			public readonly IList<string> ListPolymorphicItself;

			private PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadOnlyCollectionField( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadOnlyCollectionField()
			{
				this.ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListPolymorphicItselfReadOnlyCollectionField( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteProperty
		{
			private object _ListObjectItself;

			[MessagePackKnownType( "0", typeof( Collection<string> ) )]
			[MessagePackKnownType( "1", typeof( List<string> ) )]
			public object ListObjectItself
			{
				get { return this._ListObjectItself; }
				 set { this._ListObjectItself = value; }
			}

			private PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteProperty( object ListObjectItself ) 
			{
				this._ListObjectItself = ListObjectItself;
			}

			public PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteProperty()
			{
				this._ListObjectItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteField
		{
			[MessagePackKnownType( "0", typeof( Collection<string> ) )]
			[MessagePackKnownType( "1", typeof( List<string> ) )]
			public  object ListObjectItself;

			private PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteField( object ListObjectItself ) 
			{
				this.ListObjectItself = ListObjectItself;
			}

			public PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteField()
			{
				this.ListObjectItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListObjectItselfReadWriteField( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListObjectItselfGetOnlyCollectionProperty
		{
			private object _ListObjectItself;

			[MessagePackKnownType( "0", typeof( Collection<string> ) )]
			[MessagePackKnownType( "1", typeof( List<string> ) )]
			public object ListObjectItself
			{
				get { return this._ListObjectItself; }
			}

			private PolymorphicMemberTypeKnownType_List_ListObjectItselfGetOnlyCollectionProperty( object ListObjectItself ) 
			{
				this._ListObjectItself = ListObjectItself;
			}

			public PolymorphicMemberTypeKnownType_List_ListObjectItselfGetOnlyCollectionProperty()
			{
				this._ListObjectItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListObjectItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListObjectItselfGetOnlyCollectionProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListObjectItselfPrivateSetterCollectionProperty
		{
			private object _ListObjectItself;

			[MessagePackKnownType( "0", typeof( Collection<string> ) )]
			[MessagePackKnownType( "1", typeof( List<string> ) )]
			public object ListObjectItself
			{
				get { return this._ListObjectItself; }
				private set { this._ListObjectItself = value; }
			}

			private PolymorphicMemberTypeKnownType_List_ListObjectItselfPrivateSetterCollectionProperty( object ListObjectItself ) 
			{
				this._ListObjectItself = ListObjectItself;
			}

			public PolymorphicMemberTypeKnownType_List_ListObjectItselfPrivateSetterCollectionProperty()
			{
				this._ListObjectItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListObjectItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListObjectItselfPrivateSetterCollectionProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeKnownType_List_ListObjectItselfReadOnlyCollectionField
		{
			[MessagePackKnownType( "0", typeof( Collection<string> ) )]
			[MessagePackKnownType( "1", typeof( List<string> ) )]
			public readonly object ListObjectItself;

			private PolymorphicMemberTypeKnownType_List_ListObjectItselfReadOnlyCollectionField( object ListObjectItself ) 
			{
				this.ListObjectItself = ListObjectItself;
			}

			public PolymorphicMemberTypeKnownType_List_ListObjectItselfReadOnlyCollectionField()
			{
				this.ListObjectItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeKnownType_List_ListObjectItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_List_ListObjectItselfReadOnlyCollectionField( new Collection<string>{ "A", "B" } );
			}
		}
		#endregion ------ KnownType.CollectionTypes ------

		#region ------ KnownType.DictionaryTypes ------

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteProperty
		{
			private IDictionary<string, string> _DictStaticKeyAndStaticItem;

			public IDictionary<string, string> DictStaticKeyAndStaticItem
			{
				get { return this._DictStaticKeyAndStaticItem; }
				 set { this._DictStaticKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteProperty( IDictionary<string, string> DictStaticKeyAndStaticItem ) 
			{
				this._DictStaticKeyAndStaticItem = DictStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteProperty()
			{
				this._DictStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteField
		{
			public  IDictionary<string, string> DictStaticKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteField( IDictionary<string, string> DictStaticKeyAndStaticItem ) 
			{
				this.DictStaticKeyAndStaticItem = DictStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteField()
			{
				this.DictStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadWriteField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty
		{
			private IDictionary<string, string> _DictStaticKeyAndStaticItem;

			public IDictionary<string, string> DictStaticKeyAndStaticItem
			{
				get { return this._DictStaticKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty( IDictionary<string, string> DictStaticKeyAndStaticItem ) 
			{
				this._DictStaticKeyAndStaticItem = DictStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty()
			{
				this._DictStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty
		{
			private IDictionary<string, string> _DictStaticKeyAndStaticItem;

			public IDictionary<string, string> DictStaticKeyAndStaticItem
			{
				get { return this._DictStaticKeyAndStaticItem; }
				private set { this._DictStaticKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty( IDictionary<string, string> DictStaticKeyAndStaticItem ) 
			{
				this._DictStaticKeyAndStaticItem = DictStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty()
			{
				this._DictStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField
		{
			public readonly IDictionary<string, string> DictStaticKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField( IDictionary<string, string> DictStaticKeyAndStaticItem ) 
			{
				this.DictStaticKeyAndStaticItem = DictStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField()
			{
				this.DictStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty
		{
			private IDictionary<FileSystemEntry, string> _DictPolymorphicKeyAndStaticItem;

			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem
			{
				get { return this._DictPolymorphicKeyAndStaticItem; }
				 set { this._DictPolymorphicKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty( IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem ) 
			{
				this._DictPolymorphicKeyAndStaticItem = DictPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty()
			{
				this._DictPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField
		{
			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			public  IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField( IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem ) 
			{
				this.DictPolymorphicKeyAndStaticItem = DictPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField()
			{
				this.DictPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty
		{
			private IDictionary<FileSystemEntry, string> _DictPolymorphicKeyAndStaticItem;

			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem
			{
				get { return this._DictPolymorphicKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty( IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem ) 
			{
				this._DictPolymorphicKeyAndStaticItem = DictPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty()
			{
				this._DictPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty
		{
			private IDictionary<FileSystemEntry, string> _DictPolymorphicKeyAndStaticItem;

			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem
			{
				get { return this._DictPolymorphicKeyAndStaticItem; }
				private set { this._DictPolymorphicKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty( IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem ) 
			{
				this._DictPolymorphicKeyAndStaticItem = DictPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty()
			{
				this._DictPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField
		{
			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			public readonly IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField( IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem ) 
			{
				this.DictPolymorphicKeyAndStaticItem = DictPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField()
			{
				this.DictPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteProperty
		{
			private IDictionary<object, string> _DictObjectKeyAndStaticItem;

			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<object, string> DictObjectKeyAndStaticItem
			{
				get { return this._DictObjectKeyAndStaticItem; }
				 set { this._DictObjectKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteProperty( IDictionary<object, string> DictObjectKeyAndStaticItem ) 
			{
				this._DictObjectKeyAndStaticItem = DictObjectKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteProperty()
			{
				this._DictObjectKeyAndStaticItem = new Dictionary<object, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteProperty( new Dictionary<object, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteField
		{
			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			public  IDictionary<object, string> DictObjectKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteField( IDictionary<object, string> DictObjectKeyAndStaticItem ) 
			{
				this.DictObjectKeyAndStaticItem = DictObjectKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteField()
			{
				this.DictObjectKeyAndStaticItem = new Dictionary<object, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadWriteField( new Dictionary<object, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty
		{
			private IDictionary<object, string> _DictObjectKeyAndStaticItem;

			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<object, string> DictObjectKeyAndStaticItem
			{
				get { return this._DictObjectKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty( IDictionary<object, string> DictObjectKeyAndStaticItem ) 
			{
				this._DictObjectKeyAndStaticItem = DictObjectKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty()
			{
				this._DictObjectKeyAndStaticItem = new Dictionary<object, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty( new Dictionary<object, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty
		{
			private IDictionary<object, string> _DictObjectKeyAndStaticItem;

			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<object, string> DictObjectKeyAndStaticItem
			{
				get { return this._DictObjectKeyAndStaticItem; }
				private set { this._DictObjectKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty( IDictionary<object, string> DictObjectKeyAndStaticItem ) 
			{
				this._DictObjectKeyAndStaticItem = DictObjectKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty()
			{
				this._DictObjectKeyAndStaticItem = new Dictionary<object, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty( new Dictionary<object, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField
		{
			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			public readonly IDictionary<object, string> DictObjectKeyAndStaticItem;

			private PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField( IDictionary<object, string> DictObjectKeyAndStaticItem ) 
			{
				this.DictObjectKeyAndStaticItem = DictObjectKeyAndStaticItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField()
			{
				this.DictObjectKeyAndStaticItem = new Dictionary<object, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField( new Dictionary<object, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty
		{
			private IDictionary<string, FileSystemEntry> _DictStaticKeyAndPolymorphicItem;

			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem
			{
				get { return this._DictStaticKeyAndPolymorphicItem; }
				 set { this._DictStaticKeyAndPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty( IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem ) 
			{
				this._DictStaticKeyAndPolymorphicItem = DictStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty()
			{
				this._DictStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField
		{
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public  IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem;

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField( IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem ) 
			{
				this.DictStaticKeyAndPolymorphicItem = DictStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField()
			{
				this.DictStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty
		{
			private IDictionary<string, FileSystemEntry> _DictStaticKeyAndPolymorphicItem;

			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem
			{
				get { return this._DictStaticKeyAndPolymorphicItem; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty( IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem ) 
			{
				this._DictStaticKeyAndPolymorphicItem = DictStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty()
			{
				this._DictStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty
		{
			private IDictionary<string, FileSystemEntry> _DictStaticKeyAndPolymorphicItem;

			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem
			{
				get { return this._DictStaticKeyAndPolymorphicItem; }
				private set { this._DictStaticKeyAndPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty( IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem ) 
			{
				this._DictStaticKeyAndPolymorphicItem = DictStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty()
			{
				this._DictStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField
		{
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public readonly IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem;

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField( IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem ) 
			{
				this.DictStaticKeyAndPolymorphicItem = DictStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField()
			{
				this.DictStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteProperty
		{
			private IDictionary<string, object> _DictStaticKeyAndObjectItem;

			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<string, object> DictStaticKeyAndObjectItem
			{
				get { return this._DictStaticKeyAndObjectItem; }
				 set { this._DictStaticKeyAndObjectItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteProperty( IDictionary<string, object> DictStaticKeyAndObjectItem ) 
			{
				this._DictStaticKeyAndObjectItem = DictStaticKeyAndObjectItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteProperty()
			{
				this._DictStaticKeyAndObjectItem = new Dictionary<string, object>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteProperty( new Dictionary<string, object>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteField
		{
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public  IDictionary<string, object> DictStaticKeyAndObjectItem;

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteField( IDictionary<string, object> DictStaticKeyAndObjectItem ) 
			{
				this.DictStaticKeyAndObjectItem = DictStaticKeyAndObjectItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteField()
			{
				this.DictStaticKeyAndObjectItem = new Dictionary<string, object>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadWriteField( new Dictionary<string, object>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty
		{
			private IDictionary<string, object> _DictStaticKeyAndObjectItem;

			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<string, object> DictStaticKeyAndObjectItem
			{
				get { return this._DictStaticKeyAndObjectItem; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty( IDictionary<string, object> DictStaticKeyAndObjectItem ) 
			{
				this._DictStaticKeyAndObjectItem = DictStaticKeyAndObjectItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty()
			{
				this._DictStaticKeyAndObjectItem = new Dictionary<string, object>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty( new Dictionary<string, object>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty
		{
			private IDictionary<string, object> _DictStaticKeyAndObjectItem;

			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<string, object> DictStaticKeyAndObjectItem
			{
				get { return this._DictStaticKeyAndObjectItem; }
				private set { this._DictStaticKeyAndObjectItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty( IDictionary<string, object> DictStaticKeyAndObjectItem ) 
			{
				this._DictStaticKeyAndObjectItem = DictStaticKeyAndObjectItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty()
			{
				this._DictStaticKeyAndObjectItem = new Dictionary<string, object>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty( new Dictionary<string, object>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField
		{
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public readonly IDictionary<string, object> DictStaticKeyAndObjectItem;

			private PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField( IDictionary<string, object> DictStaticKeyAndObjectItem ) 
			{
				this.DictStaticKeyAndObjectItem = DictStaticKeyAndObjectItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField()
			{
				this.DictStaticKeyAndObjectItem = new Dictionary<string, object>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField( new Dictionary<string, object>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictPolymorphicKeyAndItem;

			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem
			{
				get { return this._DictPolymorphicKeyAndItem; }
				 set { this._DictPolymorphicKeyAndItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem ) 
			{
				this._DictPolymorphicKeyAndItem = DictPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteProperty()
			{
				this._DictPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteField
		{
			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public  IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem;

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteField( IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem ) 
			{
				this.DictPolymorphicKeyAndItem = DictPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteField()
			{
				this.DictPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadWriteField( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictPolymorphicKeyAndItem;

			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem
			{
				get { return this._DictPolymorphicKeyAndItem; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem ) 
			{
				this._DictPolymorphicKeyAndItem = DictPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty()
			{
				this._DictPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictPolymorphicKeyAndItem;

			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem
			{
				get { return this._DictPolymorphicKeyAndItem; }
				private set { this._DictPolymorphicKeyAndItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem ) 
			{
				this._DictPolymorphicKeyAndItem = DictPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty()
			{
				this._DictPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField
		{
			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public readonly IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem;

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField( IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem ) 
			{
				this.DictPolymorphicKeyAndItem = DictPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField()
			{
				this.DictPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteProperty
		{
			private IDictionary<object, object> _DictObjectKeyAndItem;

			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<object, object> DictObjectKeyAndItem
			{
				get { return this._DictObjectKeyAndItem; }
				 set { this._DictObjectKeyAndItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteProperty( IDictionary<object, object> DictObjectKeyAndItem ) 
			{
				this._DictObjectKeyAndItem = DictObjectKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteProperty()
			{
				this._DictObjectKeyAndItem = new Dictionary<object, object>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteProperty( new Dictionary<object, object>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteField
		{
			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public  IDictionary<object, object> DictObjectKeyAndItem;

			private PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteField( IDictionary<object, object> DictObjectKeyAndItem ) 
			{
				this.DictObjectKeyAndItem = DictObjectKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteField()
			{
				this.DictObjectKeyAndItem = new Dictionary<object, object>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadWriteField( new Dictionary<object, object>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty
		{
			private IDictionary<object, object> _DictObjectKeyAndItem;

			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<object, object> DictObjectKeyAndItem
			{
				get { return this._DictObjectKeyAndItem; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty( IDictionary<object, object> DictObjectKeyAndItem ) 
			{
				this._DictObjectKeyAndItem = DictObjectKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty()
			{
				this._DictObjectKeyAndItem = new Dictionary<object, object>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty( new Dictionary<object, object>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty
		{
			private IDictionary<object, object> _DictObjectKeyAndItem;

			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public IDictionary<object, object> DictObjectKeyAndItem
			{
				get { return this._DictObjectKeyAndItem; }
				private set { this._DictObjectKeyAndItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty( IDictionary<object, object> DictObjectKeyAndItem ) 
			{
				this._DictObjectKeyAndItem = DictObjectKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty()
			{
				this._DictObjectKeyAndItem = new Dictionary<object, object>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty( new Dictionary<object, object>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadOnlyCollectionField
		{
			[MessagePackKnownDictionaryKeyType( "0", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownCollectionItemType( "0", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( DirectoryEntry ) )]
			public readonly IDictionary<object, object> DictObjectKeyAndItem;

			private PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadOnlyCollectionField( IDictionary<object, object> DictObjectKeyAndItem ) 
			{
				this.DictObjectKeyAndItem = DictObjectKeyAndItem;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadOnlyCollectionField()
			{
				this.DictObjectKeyAndItem = new Dictionary<object, object>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectKeyAndItemReadOnlyCollectionField( new Dictionary<object, object>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteProperty
		{
			private IDictionary<string, string> _DictPolymorphicItself;

			[MessagePackKnownType( "0", typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( "1", typeof( SortedDictionary<string, string> ) )]
			public IDictionary<string, string> DictPolymorphicItself
			{
				get { return this._DictPolymorphicItself; }
				 set { this._DictPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteProperty( IDictionary<string, string> DictPolymorphicItself ) 
			{
				this._DictPolymorphicItself = DictPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteProperty()
			{
				this._DictPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteField
		{
			[MessagePackKnownType( "0", typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( "1", typeof( SortedDictionary<string, string> ) )]
			public  IDictionary<string, string> DictPolymorphicItself;

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteField( IDictionary<string, string> DictPolymorphicItself ) 
			{
				this.DictPolymorphicItself = DictPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteField()
			{
				this.DictPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadWriteField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty
		{
			private IDictionary<string, string> _DictPolymorphicItself;

			[MessagePackKnownType( "0", typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( "1", typeof( SortedDictionary<string, string> ) )]
			public IDictionary<string, string> DictPolymorphicItself
			{
				get { return this._DictPolymorphicItself; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty( IDictionary<string, string> DictPolymorphicItself ) 
			{
				this._DictPolymorphicItself = DictPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty()
			{
				this._DictPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty
		{
			private IDictionary<string, string> _DictPolymorphicItself;

			[MessagePackKnownType( "0", typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( "1", typeof( SortedDictionary<string, string> ) )]
			public IDictionary<string, string> DictPolymorphicItself
			{
				get { return this._DictPolymorphicItself; }
				private set { this._DictPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty( IDictionary<string, string> DictPolymorphicItself ) 
			{
				this._DictPolymorphicItself = DictPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty()
			{
				this._DictPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadOnlyCollectionField
		{
			[MessagePackKnownType( "0", typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( "1", typeof( SortedDictionary<string, string> ) )]
			public readonly IDictionary<string, string> DictPolymorphicItself;

			private PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadOnlyCollectionField( IDictionary<string, string> DictPolymorphicItself ) 
			{
				this.DictPolymorphicItself = DictPolymorphicItself;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadOnlyCollectionField()
			{
				this.DictPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictPolymorphicItselfReadOnlyCollectionField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteProperty
		{
			private object _DictObjectItself;

			[MessagePackKnownType( "0", typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( "1", typeof( SortedDictionary<string, string> ) )]
			public object DictObjectItself
			{
				get { return this._DictObjectItself; }
				 set { this._DictObjectItself = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteProperty( object DictObjectItself ) 
			{
				this._DictObjectItself = DictObjectItself;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteProperty()
			{
				this._DictObjectItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteField
		{
			[MessagePackKnownType( "0", typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( "1", typeof( SortedDictionary<string, string> ) )]
			public  object DictObjectItself;

			private PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteField( object DictObjectItself ) 
			{
				this.DictObjectItself = DictObjectItself;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteField()
			{
				this.DictObjectItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadWriteField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectItselfGetOnlyCollectionProperty
		{
			private object _DictObjectItself;

			[MessagePackKnownType( "0", typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( "1", typeof( SortedDictionary<string, string> ) )]
			public object DictObjectItself
			{
				get { return this._DictObjectItself; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictObjectItselfGetOnlyCollectionProperty( object DictObjectItself ) 
			{
				this._DictObjectItself = DictObjectItself;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectItselfGetOnlyCollectionProperty()
			{
				this._DictObjectItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectItselfGetOnlyCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectItselfPrivateSetterCollectionProperty
		{
			private object _DictObjectItself;

			[MessagePackKnownType( "0", typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( "1", typeof( SortedDictionary<string, string> ) )]
			public object DictObjectItself
			{
				get { return this._DictObjectItself; }
				private set { this._DictObjectItself = value; }
			}

			private PolymorphicMemberTypeKnownType_Dict_DictObjectItselfPrivateSetterCollectionProperty( object DictObjectItself ) 
			{
				this._DictObjectItself = DictObjectItself;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectItselfPrivateSetterCollectionProperty()
			{
				this._DictObjectItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectItselfPrivateSetterCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadOnlyCollectionField
		{
			[MessagePackKnownType( "0", typeof( Dictionary<string, string> ) )]
			[MessagePackKnownType( "1", typeof( SortedDictionary<string, string> ) )]
			public readonly object DictObjectItself;

			private PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadOnlyCollectionField( object DictObjectItself ) 
			{
				this.DictObjectItself = DictObjectItself;
			}

			public PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadOnlyCollectionField()
			{
				this.DictObjectItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Dict_DictObjectItselfReadOnlyCollectionField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}
		#endregion ------ KnownType.DictionaryTypes ------

#if !NETFX_35 && !UNITY
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string> Tuple1Static;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteProperty
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<FileSystemEntry> _Tuple1Polymorphic;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			public readonly Tuple<FileSystemEntry> Tuple1Polymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry> Tuple1Polymorphic ) 
			{
				this.Tuple1Polymorphic = Tuple1Polymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1PolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteProperty
		{
			private Tuple<object> _Tuple1ObjectItem;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			public Tuple<object> Tuple1ObjectItem
			{
				get { return this._Tuple1ObjectItem; }
				 set { this._Tuple1ObjectItem = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteProperty( Tuple<object> Tuple1ObjectItem ) 
			{
				this._Tuple1ObjectItem = Tuple1ObjectItem;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteProperty()
			{
				this._Tuple1ObjectItem = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteProperty( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as object ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			public  Tuple<object> Tuple1ObjectItem;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteField( Tuple<object> Tuple1ObjectItem ) 
			{
				this.Tuple1ObjectItem = Tuple1ObjectItem;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteField()
			{
				this.Tuple1ObjectItem = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadWriteField( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as object ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor
		{
			private Tuple<object> _Tuple1ObjectItem;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			public Tuple<object> Tuple1ObjectItem
			{
				get { return this._Tuple1ObjectItem; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor( Tuple<object> Tuple1ObjectItem ) 
			{
				this._Tuple1ObjectItem = Tuple1ObjectItem;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor
		{
			private Tuple<object> _Tuple1ObjectItem;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			public Tuple<object> Tuple1ObjectItem
			{
				get { return this._Tuple1ObjectItem; }
				private set { this._Tuple1ObjectItem = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor( Tuple<object> Tuple1ObjectItem ) 
			{
				this._Tuple1ObjectItem = Tuple1ObjectItem;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			public readonly Tuple<object> Tuple1ObjectItem;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor( Tuple<object> Tuple1ObjectItem ) 
			{
				this.Tuple1ObjectItem = Tuple1ObjectItem;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteProperty
		{
			private object _Tuple1ObjectItself;

			[MessagePackRuntimeType]
			public object Tuple1ObjectItself
			{
				get { return this._Tuple1ObjectItself; }
				 set { this._Tuple1ObjectItself = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteProperty( object Tuple1ObjectItself ) 
			{
				this._Tuple1ObjectItself = Tuple1ObjectItself;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteProperty()
			{
				this._Tuple1ObjectItself = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteProperty( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteField
		{
			[MessagePackRuntimeType]
			public  object Tuple1ObjectItself;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteField( object Tuple1ObjectItself ) 
			{
				this.Tuple1ObjectItself = Tuple1ObjectItself;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteField()
			{
				this.Tuple1ObjectItself = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadWriteField( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileEntry ) );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor
		{
			private object _Tuple1ObjectItself;

			[MessagePackRuntimeType]
			public object Tuple1ObjectItself
			{
				get { return this._Tuple1ObjectItself; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor( object Tuple1ObjectItself ) 
			{
				this._Tuple1ObjectItself = Tuple1ObjectItself;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor
		{
			private object _Tuple1ObjectItself;

			[MessagePackRuntimeType]
			public object Tuple1ObjectItself
			{
				get { return this._Tuple1ObjectItself; }
				private set { this._Tuple1ObjectItself = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor( object Tuple1ObjectItself ) 
			{
				this._Tuple1ObjectItself = Tuple1ObjectItself;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeType]
			public readonly object Tuple1ObjectItself;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor( object Tuple1ObjectItself ) 
			{
				this.Tuple1ObjectItself = Tuple1ObjectItself;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor() {}
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteProperty
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, string, string, string, string, string, string> _Tuple7FirstPolymorphic;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			public readonly Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, string, string, string, string, string, string> Tuple7FirstPolymorphic ) 
			{
				this.Tuple7FirstPolymorphic = Tuple7FirstPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7FirstPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, FileSystemEntry> _Tuple7LastPolymorphic;

			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
			public readonly Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, FileSystemEntry> Tuple7LastPolymorphic ) 
			{
				this.Tuple7LastPolymorphic = Tuple7LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7LastPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteProperty
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7MidPolymorphic;

			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic
			{
				get { return this._Tuple7MidPolymorphic; }
				 set { this._Tuple7MidPolymorphic = value; }
			}

			private PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteProperty( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic ) 
			{
				this._Tuple7MidPolymorphic = Tuple7MidPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteProperty()
			{
				this._Tuple7MidPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteProperty( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			public  Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic;

			private PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteField( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic ) 
			{
				this.Tuple7MidPolymorphic = Tuple7MidPolymorphic;
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteField()
			{
				this.Tuple7MidPolymorphic = null;
			}

			public static PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadWriteField( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7MidPolymorphic;

			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic
			{
				get { return this._Tuple7MidPolymorphic; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic ) 
			{
				this._Tuple7MidPolymorphic = Tuple7MidPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7MidPolymorphic;

			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic
			{
				get { return this._Tuple7MidPolymorphic; }
				private set { this._Tuple7MidPolymorphic = value; }
			}

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic ) 
			{
				this._Tuple7MidPolymorphic = Tuple7MidPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			public readonly Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic ) 
			{
				this.Tuple7MidPolymorphic = Tuple7MidPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7MidPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteProperty
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 2, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 3, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 3, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 5, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 5, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 6, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 6, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 2, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 3, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 3, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 5, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 5, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 6, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 6, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 2, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 3, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 3, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 5, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 5, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 6, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 6, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> _Tuple7AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 2, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 3, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 3, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 5, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 5, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 6, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 6, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 2, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 3, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 3, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 5, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 5, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 6, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 6, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry> Tuple7AllPolymorphic ) 
			{
				this.Tuple7AllPolymorphic = Tuple7AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple7AllPolymorphicReadOnlyFieldAndConstructor() {}
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteProperty
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackKnownTupleItemType( 8, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 8, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 8, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 8, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackKnownTupleItemType( 8, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 8, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> _Tuple8LastPolymorphic;

			[MessagePackKnownTupleItemType( 8, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 8, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 8, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 8, "1", typeof( DirectoryEntry ) )]
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<FileSystemEntry>> Tuple8LastPolymorphic ) 
			{
				this.Tuple8LastPolymorphic = Tuple8LastPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8LastPolymorphicReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteProperty
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 2, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 3, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 3, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 5, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 5, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 6, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 6, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 8, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 8, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadWriteField
		{
			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 2, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 3, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 3, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 5, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 5, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 6, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 6, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 8, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 8, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 2, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 3, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 3, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 5, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 5, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 6, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 6, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 8, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 8, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> _Tuple8AllPolymorphic;

			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 2, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 3, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 3, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 5, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 5, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 6, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 6, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 8, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 8, "1", typeof( DirectoryEntry ) )]
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

		public class PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackKnownTupleItemType( 1, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 2, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 3, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 3, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 4, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 4, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 5, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 5, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 6, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 6, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 7, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 7, "1", typeof( DirectoryEntry ) )]
			[MessagePackKnownTupleItemType( 8, "0", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 8, "1", typeof( DirectoryEntry ) )]
			public readonly Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic;

			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor( Tuple<FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, FileSystemEntry, Tuple<FileSystemEntry>> Tuple8AllPolymorphic ) 
			{
				this.Tuple8AllPolymorphic = Tuple8AllPolymorphic;
			}
			public PolymorphicMemberTypeKnownType_Tuple_Tuple8AllPolymorphicReadOnlyFieldAndConstructor() {}
		}
		#endregion ------ KnownType.TupleTypes ------
#endif // #if !NETFX_35 && !UNITY

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
				return new PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteProperty( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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
				return new PolymorphicMemberTypeRuntimeType_ValueReadWritePropertyAsObject( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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
				return new PolymorphicMemberTypeRuntimeType_Normal_ValueReadWriteField( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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
				return new PolymorphicMemberTypeRuntimeType_ValueReadWriteFieldAsObject( new DateTime( 1982, 1, 29, 15, 46, 12, DateTimeKind.Utc ) );
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

		public class PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				 set { this._ListStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteProperty( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteProperty()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteProperty( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteField
		{
			public  IList<string> ListStaticItem;

			private PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteField( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteField()
			{
				this.ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadWriteField( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListStaticItemGetOnlyCollectionProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListStaticItemGetOnlyCollectionProperty( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListStaticItemGetOnlyCollectionProperty()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListStaticItemGetOnlyCollectionProperty( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListStaticItemPrivateSetterCollectionProperty
		{
			private IList<string> _ListStaticItem;

			public IList<string> ListStaticItem
			{
				get { return this._ListStaticItem; }
				private set { this._ListStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListStaticItemPrivateSetterCollectionProperty( IList<string> ListStaticItem ) 
			{
				this._ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListStaticItemPrivateSetterCollectionProperty()
			{
				this._ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListStaticItemPrivateSetterCollectionProperty( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadOnlyCollectionField
		{
			public readonly IList<string> ListStaticItem;

			private PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadOnlyCollectionField( IList<string> ListStaticItem ) 
			{
				this.ListStaticItem = ListStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadOnlyCollectionField()
			{
				this.ListStaticItem = new List<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListStaticItemReadOnlyCollectionField( new List<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteProperty
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				 set { this._ListPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteProperty( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteProperty()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteProperty( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteField
		{
			[MessagePackRuntimeCollectionItemType]
			public  IList<FileSystemEntry> ListPolymorphicItem;

			private PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteField( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteField()
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadWriteField( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemGetOnlyCollectionProperty
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemGetOnlyCollectionProperty( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemGetOnlyCollectionProperty()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemGetOnlyCollectionProperty( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemPrivateSetterCollectionProperty
		{
			private IList<FileSystemEntry> _ListPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> ListPolymorphicItem
			{
				get { return this._ListPolymorphicItem; }
				private set { this._ListPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemPrivateSetterCollectionProperty( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this._ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemPrivateSetterCollectionProperty()
			{
				this._ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemPrivateSetterCollectionProperty( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadOnlyCollectionField
		{
			[MessagePackRuntimeCollectionItemType]
			public readonly IList<FileSystemEntry> ListPolymorphicItem;

			private PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadOnlyCollectionField( IList<FileSystemEntry> ListPolymorphicItem ) 
			{
				this.ListPolymorphicItem = ListPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadOnlyCollectionField()
			{
				this.ListPolymorphicItem = new List<FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItemReadOnlyCollectionField( new List<FileSystemEntry>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteProperty
		{
			private IList<object> _ListObjectItem;

			[MessagePackRuntimeCollectionItemType]
			public IList<object> ListObjectItem
			{
				get { return this._ListObjectItem; }
				 set { this._ListObjectItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteProperty( IList<object> ListObjectItem ) 
			{
				this._ListObjectItem = ListObjectItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteProperty()
			{
				this._ListObjectItem = new List<object>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteProperty( new List<object>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteField
		{
			[MessagePackRuntimeCollectionItemType]
			public  IList<object> ListObjectItem;

			private PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteField( IList<object> ListObjectItem ) 
			{
				this.ListObjectItem = ListObjectItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteField()
			{
				this.ListObjectItem = new List<object>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadWriteField( new List<object>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListObjectItemGetOnlyCollectionProperty
		{
			private IList<object> _ListObjectItem;

			[MessagePackRuntimeCollectionItemType]
			public IList<object> ListObjectItem
			{
				get { return this._ListObjectItem; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListObjectItemGetOnlyCollectionProperty( IList<object> ListObjectItem ) 
			{
				this._ListObjectItem = ListObjectItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListObjectItemGetOnlyCollectionProperty()
			{
				this._ListObjectItem = new List<object>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListObjectItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListObjectItemGetOnlyCollectionProperty( new List<object>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListObjectItemPrivateSetterCollectionProperty
		{
			private IList<object> _ListObjectItem;

			[MessagePackRuntimeCollectionItemType]
			public IList<object> ListObjectItem
			{
				get { return this._ListObjectItem; }
				private set { this._ListObjectItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListObjectItemPrivateSetterCollectionProperty( IList<object> ListObjectItem ) 
			{
				this._ListObjectItem = ListObjectItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListObjectItemPrivateSetterCollectionProperty()
			{
				this._ListObjectItem = new List<object>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListObjectItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListObjectItemPrivateSetterCollectionProperty( new List<object>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadOnlyCollectionField
		{
			[MessagePackRuntimeCollectionItemType]
			public readonly IList<object> ListObjectItem;

			private PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadOnlyCollectionField( IList<object> ListObjectItem ) 
			{
				this.ListObjectItem = ListObjectItem;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadOnlyCollectionField()
			{
				this.ListObjectItem = new List<object>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListObjectItemReadOnlyCollectionField( new List<object>{ new FileEntry { Name = "file", Size = 1L }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteProperty
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackRuntimeType]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				 set { this._ListPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteProperty( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteProperty()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteField
		{
			[MessagePackRuntimeType]
			public  IList<string> ListPolymorphicItself;

			private PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteField( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteField()
			{
				this.ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadWriteField( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfGetOnlyCollectionProperty
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackRuntimeType]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfGetOnlyCollectionProperty( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfGetOnlyCollectionProperty()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfGetOnlyCollectionProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfPrivateSetterCollectionProperty
		{
			private IList<string> _ListPolymorphicItself;

			[MessagePackRuntimeType]
			public IList<string> ListPolymorphicItself
			{
				get { return this._ListPolymorphicItself; }
				private set { this._ListPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfPrivateSetterCollectionProperty( IList<string> ListPolymorphicItself ) 
			{
				this._ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfPrivateSetterCollectionProperty()
			{
				this._ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfPrivateSetterCollectionProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadOnlyCollectionField
		{
			[MessagePackRuntimeType]
			public readonly IList<string> ListPolymorphicItself;

			private PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadOnlyCollectionField( IList<string> ListPolymorphicItself ) 
			{
				this.ListPolymorphicItself = ListPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadOnlyCollectionField()
			{
				this.ListPolymorphicItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListPolymorphicItselfReadOnlyCollectionField( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteProperty
		{
			private object _ListObjectItself;

			[MessagePackRuntimeType]
			public object ListObjectItself
			{
				get { return this._ListObjectItself; }
				 set { this._ListObjectItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteProperty( object ListObjectItself ) 
			{
				this._ListObjectItself = ListObjectItself;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteProperty()
			{
				this._ListObjectItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteField
		{
			[MessagePackRuntimeType]
			public  object ListObjectItself;

			private PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteField( object ListObjectItself ) 
			{
				this.ListObjectItself = ListObjectItself;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteField()
			{
				this.ListObjectItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadWriteField( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListObjectItselfGetOnlyCollectionProperty
		{
			private object _ListObjectItself;

			[MessagePackRuntimeType]
			public object ListObjectItself
			{
				get { return this._ListObjectItself; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListObjectItselfGetOnlyCollectionProperty( object ListObjectItself ) 
			{
				this._ListObjectItself = ListObjectItself;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListObjectItselfGetOnlyCollectionProperty()
			{
				this._ListObjectItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListObjectItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListObjectItselfGetOnlyCollectionProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListObjectItselfPrivateSetterCollectionProperty
		{
			private object _ListObjectItself;

			[MessagePackRuntimeType]
			public object ListObjectItself
			{
				get { return this._ListObjectItself; }
				private set { this._ListObjectItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_List_ListObjectItselfPrivateSetterCollectionProperty( object ListObjectItself ) 
			{
				this._ListObjectItself = ListObjectItself;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListObjectItselfPrivateSetterCollectionProperty()
			{
				this._ListObjectItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListObjectItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListObjectItselfPrivateSetterCollectionProperty( new Collection<string>{ "A", "B" } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadOnlyCollectionField
		{
			[MessagePackRuntimeType]
			public readonly object ListObjectItself;

			private PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadOnlyCollectionField( object ListObjectItself ) 
			{
				this.ListObjectItself = ListObjectItself;
			}

			public PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadOnlyCollectionField()
			{
				this.ListObjectItself = new Collection<string>();
			}

			public static PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_List_ListObjectItselfReadOnlyCollectionField( new Collection<string>{ "A", "B" } );
			}
		}
		#endregion ------ RuntimeType.CollectionTypes ------

		#region ------ RuntimeType.DictionaryTypes ------

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteProperty
		{
			private IDictionary<string, string> _DictStaticKeyAndStaticItem;

			public IDictionary<string, string> DictStaticKeyAndStaticItem
			{
				get { return this._DictStaticKeyAndStaticItem; }
				 set { this._DictStaticKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteProperty( IDictionary<string, string> DictStaticKeyAndStaticItem ) 
			{
				this._DictStaticKeyAndStaticItem = DictStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteProperty()
			{
				this._DictStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteField
		{
			public  IDictionary<string, string> DictStaticKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteField( IDictionary<string, string> DictStaticKeyAndStaticItem ) 
			{
				this.DictStaticKeyAndStaticItem = DictStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteField()
			{
				this.DictStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadWriteField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty
		{
			private IDictionary<string, string> _DictStaticKeyAndStaticItem;

			public IDictionary<string, string> DictStaticKeyAndStaticItem
			{
				get { return this._DictStaticKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty( IDictionary<string, string> DictStaticKeyAndStaticItem ) 
			{
				this._DictStaticKeyAndStaticItem = DictStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty()
			{
				this._DictStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemGetOnlyCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty
		{
			private IDictionary<string, string> _DictStaticKeyAndStaticItem;

			public IDictionary<string, string> DictStaticKeyAndStaticItem
			{
				get { return this._DictStaticKeyAndStaticItem; }
				private set { this._DictStaticKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty( IDictionary<string, string> DictStaticKeyAndStaticItem ) 
			{
				this._DictStaticKeyAndStaticItem = DictStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty()
			{
				this._DictStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemPrivateSetterCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField
		{
			public readonly IDictionary<string, string> DictStaticKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField( IDictionary<string, string> DictStaticKeyAndStaticItem ) 
			{
				this.DictStaticKeyAndStaticItem = DictStaticKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField()
			{
				this.DictStaticKeyAndStaticItem = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndStaticItemReadOnlyCollectionField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty
		{
			private IDictionary<FileSystemEntry, string> _DictPolymorphicKeyAndStaticItem;

			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem
			{
				get { return this._DictPolymorphicKeyAndStaticItem; }
				 set { this._DictPolymorphicKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty( IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem ) 
			{
				this._DictPolymorphicKeyAndStaticItem = DictPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty()
			{
				this._DictPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteProperty( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField
		{
			[MessagePackRuntimeDictionaryKeyType]
			public  IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField( IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem ) 
			{
				this.DictPolymorphicKeyAndStaticItem = DictPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField()
			{
				this.DictPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadWriteField( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty
		{
			private IDictionary<FileSystemEntry, string> _DictPolymorphicKeyAndStaticItem;

			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem
			{
				get { return this._DictPolymorphicKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty( IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem ) 
			{
				this._DictPolymorphicKeyAndStaticItem = DictPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty()
			{
				this._DictPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemGetOnlyCollectionProperty( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty
		{
			private IDictionary<FileSystemEntry, string> _DictPolymorphicKeyAndStaticItem;

			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem
			{
				get { return this._DictPolymorphicKeyAndStaticItem; }
				private set { this._DictPolymorphicKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty( IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem ) 
			{
				this._DictPolymorphicKeyAndStaticItem = DictPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty()
			{
				this._DictPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemPrivateSetterCollectionProperty( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField
		{
			[MessagePackRuntimeDictionaryKeyType]
			public readonly IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField( IDictionary<FileSystemEntry, string> DictPolymorphicKeyAndStaticItem ) 
			{
				this.DictPolymorphicKeyAndStaticItem = DictPolymorphicKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField()
			{
				this.DictPolymorphicKeyAndStaticItem = new Dictionary<FileSystemEntry, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndStaticItemReadOnlyCollectionField( new Dictionary<FileSystemEntry, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteProperty
		{
			private IDictionary<object, string> _DictObjectKeyAndStaticItem;

			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<object, string> DictObjectKeyAndStaticItem
			{
				get { return this._DictObjectKeyAndStaticItem; }
				 set { this._DictObjectKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteProperty( IDictionary<object, string> DictObjectKeyAndStaticItem ) 
			{
				this._DictObjectKeyAndStaticItem = DictObjectKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteProperty()
			{
				this._DictObjectKeyAndStaticItem = new Dictionary<object, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteProperty( new Dictionary<object, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteField
		{
			[MessagePackRuntimeDictionaryKeyType]
			public  IDictionary<object, string> DictObjectKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteField( IDictionary<object, string> DictObjectKeyAndStaticItem ) 
			{
				this.DictObjectKeyAndStaticItem = DictObjectKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteField()
			{
				this.DictObjectKeyAndStaticItem = new Dictionary<object, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadWriteField( new Dictionary<object, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty
		{
			private IDictionary<object, string> _DictObjectKeyAndStaticItem;

			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<object, string> DictObjectKeyAndStaticItem
			{
				get { return this._DictObjectKeyAndStaticItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty( IDictionary<object, string> DictObjectKeyAndStaticItem ) 
			{
				this._DictObjectKeyAndStaticItem = DictObjectKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty()
			{
				this._DictObjectKeyAndStaticItem = new Dictionary<object, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemGetOnlyCollectionProperty( new Dictionary<object, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty
		{
			private IDictionary<object, string> _DictObjectKeyAndStaticItem;

			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<object, string> DictObjectKeyAndStaticItem
			{
				get { return this._DictObjectKeyAndStaticItem; }
				private set { this._DictObjectKeyAndStaticItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty( IDictionary<object, string> DictObjectKeyAndStaticItem ) 
			{
				this._DictObjectKeyAndStaticItem = DictObjectKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty()
			{
				this._DictObjectKeyAndStaticItem = new Dictionary<object, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemPrivateSetterCollectionProperty( new Dictionary<object, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField
		{
			[MessagePackRuntimeDictionaryKeyType]
			public readonly IDictionary<object, string> DictObjectKeyAndStaticItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField( IDictionary<object, string> DictObjectKeyAndStaticItem ) 
			{
				this.DictObjectKeyAndStaticItem = DictObjectKeyAndStaticItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField()
			{
				this.DictObjectKeyAndStaticItem = new Dictionary<object, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndStaticItemReadOnlyCollectionField( new Dictionary<object, string>{ { new FileEntry { Name = "file", Size = 1L }, "A" }, { new DirectoryEntry { Name = "dir", ChildCount = 1 }, "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty
		{
			private IDictionary<string, FileSystemEntry> _DictStaticKeyAndPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem
			{
				get { return this._DictStaticKeyAndPolymorphicItem; }
				 set { this._DictStaticKeyAndPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty( IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem ) 
			{
				this._DictStaticKeyAndPolymorphicItem = DictStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty()
			{
				this._DictStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteProperty( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField
		{
			[MessagePackRuntimeCollectionItemType]
			public  IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField( IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem ) 
			{
				this.DictStaticKeyAndPolymorphicItem = DictStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField()
			{
				this.DictStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadWriteField( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty
		{
			private IDictionary<string, FileSystemEntry> _DictStaticKeyAndPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem
			{
				get { return this._DictStaticKeyAndPolymorphicItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty( IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem ) 
			{
				this._DictStaticKeyAndPolymorphicItem = DictStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty()
			{
				this._DictStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemGetOnlyCollectionProperty( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty
		{
			private IDictionary<string, FileSystemEntry> _DictStaticKeyAndPolymorphicItem;

			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem
			{
				get { return this._DictStaticKeyAndPolymorphicItem; }
				private set { this._DictStaticKeyAndPolymorphicItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty( IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem ) 
			{
				this._DictStaticKeyAndPolymorphicItem = DictStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty()
			{
				this._DictStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemPrivateSetterCollectionProperty( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField
		{
			[MessagePackRuntimeCollectionItemType]
			public readonly IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField( IDictionary<string, FileSystemEntry> DictStaticKeyAndPolymorphicItem ) 
			{
				this.DictStaticKeyAndPolymorphicItem = DictStaticKeyAndPolymorphicItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField()
			{
				this.DictStaticKeyAndPolymorphicItem = new Dictionary<string, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndPolymorphicItemReadOnlyCollectionField( new Dictionary<string, FileSystemEntry>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteProperty
		{
			private IDictionary<string, object> _DictStaticKeyAndObjectItem;

			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, object> DictStaticKeyAndObjectItem
			{
				get { return this._DictStaticKeyAndObjectItem; }
				 set { this._DictStaticKeyAndObjectItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteProperty( IDictionary<string, object> DictStaticKeyAndObjectItem ) 
			{
				this._DictStaticKeyAndObjectItem = DictStaticKeyAndObjectItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteProperty()
			{
				this._DictStaticKeyAndObjectItem = new Dictionary<string, object>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteProperty( new Dictionary<string, object>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteField
		{
			[MessagePackRuntimeCollectionItemType]
			public  IDictionary<string, object> DictStaticKeyAndObjectItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteField( IDictionary<string, object> DictStaticKeyAndObjectItem ) 
			{
				this.DictStaticKeyAndObjectItem = DictStaticKeyAndObjectItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteField()
			{
				this.DictStaticKeyAndObjectItem = new Dictionary<string, object>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadWriteField( new Dictionary<string, object>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty
		{
			private IDictionary<string, object> _DictStaticKeyAndObjectItem;

			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, object> DictStaticKeyAndObjectItem
			{
				get { return this._DictStaticKeyAndObjectItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty( IDictionary<string, object> DictStaticKeyAndObjectItem ) 
			{
				this._DictStaticKeyAndObjectItem = DictStaticKeyAndObjectItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty()
			{
				this._DictStaticKeyAndObjectItem = new Dictionary<string, object>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemGetOnlyCollectionProperty( new Dictionary<string, object>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty
		{
			private IDictionary<string, object> _DictStaticKeyAndObjectItem;

			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, object> DictStaticKeyAndObjectItem
			{
				get { return this._DictStaticKeyAndObjectItem; }
				private set { this._DictStaticKeyAndObjectItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty( IDictionary<string, object> DictStaticKeyAndObjectItem ) 
			{
				this._DictStaticKeyAndObjectItem = DictStaticKeyAndObjectItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty()
			{
				this._DictStaticKeyAndObjectItem = new Dictionary<string, object>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemPrivateSetterCollectionProperty( new Dictionary<string, object>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField
		{
			[MessagePackRuntimeCollectionItemType]
			public readonly IDictionary<string, object> DictStaticKeyAndObjectItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField( IDictionary<string, object> DictStaticKeyAndObjectItem ) 
			{
				this.DictStaticKeyAndObjectItem = DictStaticKeyAndObjectItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField()
			{
				this.DictStaticKeyAndObjectItem = new Dictionary<string, object>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictStaticKeyAndObjectItemReadOnlyCollectionField( new Dictionary<string, object>{ { "A", new FileEntry { Name = "file", Size = 1L } }, { "B", new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictPolymorphicKeyAndItem;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem
			{
				get { return this._DictPolymorphicKeyAndItem; }
				 set { this._DictPolymorphicKeyAndItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem ) 
			{
				this._DictPolymorphicKeyAndItem = DictPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteProperty()
			{
				this._DictPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteField
		{
			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public  IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteField( IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem ) 
			{
				this.DictPolymorphicKeyAndItem = DictPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteField()
			{
				this.DictPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadWriteField( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictPolymorphicKeyAndItem;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem
			{
				get { return this._DictPolymorphicKeyAndItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem ) 
			{
				this._DictPolymorphicKeyAndItem = DictPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty()
			{
				this._DictPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemGetOnlyCollectionProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty
		{
			private IDictionary<FileSystemEntry, FileSystemEntry> _DictPolymorphicKeyAndItem;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem
			{
				get { return this._DictPolymorphicKeyAndItem; }
				private set { this._DictPolymorphicKeyAndItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty( IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem ) 
			{
				this._DictPolymorphicKeyAndItem = DictPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty()
			{
				this._DictPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemPrivateSetterCollectionProperty( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField
		{
			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public readonly IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField( IDictionary<FileSystemEntry, FileSystemEntry> DictPolymorphicKeyAndItem ) 
			{
				this.DictPolymorphicKeyAndItem = DictPolymorphicKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField()
			{
				this.DictPolymorphicKeyAndItem = new Dictionary<FileSystemEntry, FileSystemEntry>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicKeyAndItemReadOnlyCollectionField( new Dictionary<FileSystemEntry, FileSystemEntry>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteProperty
		{
			private IDictionary<object, object> _DictObjectKeyAndItem;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<object, object> DictObjectKeyAndItem
			{
				get { return this._DictObjectKeyAndItem; }
				 set { this._DictObjectKeyAndItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteProperty( IDictionary<object, object> DictObjectKeyAndItem ) 
			{
				this._DictObjectKeyAndItem = DictObjectKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteProperty()
			{
				this._DictObjectKeyAndItem = new Dictionary<object, object>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteProperty( new Dictionary<object, object>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteField
		{
			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public  IDictionary<object, object> DictObjectKeyAndItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteField( IDictionary<object, object> DictObjectKeyAndItem ) 
			{
				this.DictObjectKeyAndItem = DictObjectKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteField()
			{
				this.DictObjectKeyAndItem = new Dictionary<object, object>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadWriteField( new Dictionary<object, object>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty
		{
			private IDictionary<object, object> _DictObjectKeyAndItem;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<object, object> DictObjectKeyAndItem
			{
				get { return this._DictObjectKeyAndItem; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty( IDictionary<object, object> DictObjectKeyAndItem ) 
			{
				this._DictObjectKeyAndItem = DictObjectKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty()
			{
				this._DictObjectKeyAndItem = new Dictionary<object, object>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemGetOnlyCollectionProperty( new Dictionary<object, object>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty
		{
			private IDictionary<object, object> _DictObjectKeyAndItem;

			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<object, object> DictObjectKeyAndItem
			{
				get { return this._DictObjectKeyAndItem; }
				private set { this._DictObjectKeyAndItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty( IDictionary<object, object> DictObjectKeyAndItem ) 
			{
				this._DictObjectKeyAndItem = DictObjectKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty()
			{
				this._DictObjectKeyAndItem = new Dictionary<object, object>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemPrivateSetterCollectionProperty( new Dictionary<object, object>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadOnlyCollectionField
		{
			[MessagePackRuntimeCollectionItemType]
			[MessagePackRuntimeDictionaryKeyType]
			public readonly IDictionary<object, object> DictObjectKeyAndItem;

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadOnlyCollectionField( IDictionary<object, object> DictObjectKeyAndItem ) 
			{
				this.DictObjectKeyAndItem = DictObjectKeyAndItem;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadOnlyCollectionField()
			{
				this.DictObjectKeyAndItem = new Dictionary<object, object>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectKeyAndItemReadOnlyCollectionField( new Dictionary<object, object>{ { new FileEntry { Name = "A", Size = 1L }, new FileEntry { Name = "file", Size = 1L } }, { new DirectoryEntry { Name = "B", ChildCount = 1 }, new DirectoryEntry { Name = "dir", ChildCount = 1 } } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteProperty
		{
			private IDictionary<string, string> _DictPolymorphicItself;

			[MessagePackRuntimeType]
			public IDictionary<string, string> DictPolymorphicItself
			{
				get { return this._DictPolymorphicItself; }
				 set { this._DictPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteProperty( IDictionary<string, string> DictPolymorphicItself ) 
			{
				this._DictPolymorphicItself = DictPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteProperty()
			{
				this._DictPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteField
		{
			[MessagePackRuntimeType]
			public  IDictionary<string, string> DictPolymorphicItself;

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteField( IDictionary<string, string> DictPolymorphicItself ) 
			{
				this.DictPolymorphicItself = DictPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteField()
			{
				this.DictPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadWriteField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty
		{
			private IDictionary<string, string> _DictPolymorphicItself;

			[MessagePackRuntimeType]
			public IDictionary<string, string> DictPolymorphicItself
			{
				get { return this._DictPolymorphicItself; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty( IDictionary<string, string> DictPolymorphicItself ) 
			{
				this._DictPolymorphicItself = DictPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty()
			{
				this._DictPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfGetOnlyCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty
		{
			private IDictionary<string, string> _DictPolymorphicItself;

			[MessagePackRuntimeType]
			public IDictionary<string, string> DictPolymorphicItself
			{
				get { return this._DictPolymorphicItself; }
				private set { this._DictPolymorphicItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty( IDictionary<string, string> DictPolymorphicItself ) 
			{
				this._DictPolymorphicItself = DictPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty()
			{
				this._DictPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfPrivateSetterCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadOnlyCollectionField
		{
			[MessagePackRuntimeType]
			public readonly IDictionary<string, string> DictPolymorphicItself;

			private PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadOnlyCollectionField( IDictionary<string, string> DictPolymorphicItself ) 
			{
				this.DictPolymorphicItself = DictPolymorphicItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadOnlyCollectionField()
			{
				this.DictPolymorphicItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictPolymorphicItselfReadOnlyCollectionField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteProperty
		{
			private object _DictObjectItself;

			[MessagePackRuntimeType]
			public object DictObjectItself
			{
				get { return this._DictObjectItself; }
				 set { this._DictObjectItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteProperty( object DictObjectItself ) 
			{
				this._DictObjectItself = DictObjectItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteProperty()
			{
				this._DictObjectItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteField
		{
			[MessagePackRuntimeType]
			public  object DictObjectItself;

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteField( object DictObjectItself ) 
			{
				this.DictObjectItself = DictObjectItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteField()
			{
				this.DictObjectItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadWriteField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfGetOnlyCollectionProperty
		{
			private object _DictObjectItself;

			[MessagePackRuntimeType]
			public object DictObjectItself
			{
				get { return this._DictObjectItself; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfGetOnlyCollectionProperty( object DictObjectItself ) 
			{
				this._DictObjectItself = DictObjectItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfGetOnlyCollectionProperty()
			{
				this._DictObjectItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfGetOnlyCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfGetOnlyCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfPrivateSetterCollectionProperty
		{
			private object _DictObjectItself;

			[MessagePackRuntimeType]
			public object DictObjectItself
			{
				get { return this._DictObjectItself; }
				private set { this._DictObjectItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfPrivateSetterCollectionProperty( object DictObjectItself ) 
			{
				this._DictObjectItself = DictObjectItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfPrivateSetterCollectionProperty()
			{
				this._DictObjectItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfPrivateSetterCollectionProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfPrivateSetterCollectionProperty( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadOnlyCollectionField
		{
			[MessagePackRuntimeType]
			public readonly object DictObjectItself;

			private PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadOnlyCollectionField( object DictObjectItself ) 
			{
				this.DictObjectItself = DictObjectItself;
			}

			public PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadOnlyCollectionField()
			{
				this.DictObjectItself = new Dictionary<string, string>();
			}

			public static PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadOnlyCollectionField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Dict_DictObjectItselfReadOnlyCollectionField( new Dictionary<string, string>{ { "A", "A" }, { "B", "B" } } );
			}
		}
		#endregion ------ RuntimeType.DictionaryTypes ------

#if !NETFX_35 && !UNITY
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

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string> Tuple1Static;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor( Tuple<string> Tuple1Static ) 
			{
				this.Tuple1Static = Tuple1Static;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1StaticReadOnlyFieldAndConstructor() {}
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

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteProperty
		{
			private Tuple<object> _Tuple1ObjectItem;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<object> Tuple1ObjectItem
			{
				get { return this._Tuple1ObjectItem; }
				 set { this._Tuple1ObjectItem = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteProperty( Tuple<object> Tuple1ObjectItem ) 
			{
				this._Tuple1ObjectItem = Tuple1ObjectItem;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteProperty()
			{
				this._Tuple1ObjectItem = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteProperty( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as object ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public  Tuple<object> Tuple1ObjectItem;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteField( Tuple<object> Tuple1ObjectItem ) 
			{
				this.Tuple1ObjectItem = Tuple1ObjectItem;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteField()
			{
				this.Tuple1ObjectItem = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadWriteField( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as object ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor
		{
			private Tuple<object> _Tuple1ObjectItem;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<object> Tuple1ObjectItem
			{
				get { return this._Tuple1ObjectItem; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor( Tuple<object> Tuple1ObjectItem ) 
			{
				this._Tuple1ObjectItem = Tuple1ObjectItem;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor
		{
			private Tuple<object> _Tuple1ObjectItem;

			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<object> Tuple1ObjectItem
			{
				get { return this._Tuple1ObjectItem; }
				private set { this._Tuple1ObjectItem = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor( Tuple<object> Tuple1ObjectItem ) 
			{
				this._Tuple1ObjectItem = Tuple1ObjectItem;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 1 )]
			public readonly Tuple<object> Tuple1ObjectItem;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor( Tuple<object> Tuple1ObjectItem ) 
			{
				this.Tuple1ObjectItem = Tuple1ObjectItem;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItemReadOnlyFieldAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteProperty
		{
			private object _Tuple1ObjectItself;

			[MessagePackRuntimeType]
			public object Tuple1ObjectItself
			{
				get { return this._Tuple1ObjectItself; }
				 set { this._Tuple1ObjectItself = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteProperty( object Tuple1ObjectItself ) 
			{
				this._Tuple1ObjectItself = Tuple1ObjectItself;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteProperty()
			{
				this._Tuple1ObjectItself = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteProperty( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteField
		{
			[MessagePackRuntimeType]
			public  object Tuple1ObjectItself;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteField( object Tuple1ObjectItself ) 
			{
				this.Tuple1ObjectItself = Tuple1ObjectItself;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteField()
			{
				this.Tuple1ObjectItself = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadWriteField( Tuple.Create( new FileEntry { Name = "1", Size = 1 } as FileEntry ) );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor
		{
			private object _Tuple1ObjectItself;

			[MessagePackRuntimeType]
			public object Tuple1ObjectItself
			{
				get { return this._Tuple1ObjectItself; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor( object Tuple1ObjectItself ) 
			{
				this._Tuple1ObjectItself = Tuple1ObjectItself;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor
		{
			private object _Tuple1ObjectItself;

			[MessagePackRuntimeType]
			public object Tuple1ObjectItself
			{
				get { return this._Tuple1ObjectItself; }
				private set { this._Tuple1ObjectItself = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor( object Tuple1ObjectItself ) 
			{
				this._Tuple1ObjectItself = Tuple1ObjectItself;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeType]
			public readonly object Tuple1ObjectItself;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor( object Tuple1ObjectItself ) 
			{
				this.Tuple1ObjectItself = Tuple1ObjectItself;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple1ObjectItselfReadOnlyFieldAndConstructor() {}
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

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string, string, string, string, string, string, string> Tuple7AllStatic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string> Tuple7AllStatic ) 
			{
				this.Tuple7AllStatic = Tuple7AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7AllStaticReadOnlyFieldAndConstructor() {}
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

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteProperty
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7MidPolymorphic;

			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic
			{
				get { return this._Tuple7MidPolymorphic; }
				 set { this._Tuple7MidPolymorphic = value; }
			}

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteProperty( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic ) 
			{
				this._Tuple7MidPolymorphic = Tuple7MidPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteProperty()
			{
				this._Tuple7MidPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteProperty Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteProperty( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteField
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			public  Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic;

			private PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteField( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic ) 
			{
				this.Tuple7MidPolymorphic = Tuple7MidPolymorphic;
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteField()
			{
				this.Tuple7MidPolymorphic = null;
			}

			public static PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteField Initialize()
			{
				return new PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadWriteField( Tuple.Create( "1", "2", "3", new FileEntry { Name = "4", Size = 4 } as FileSystemEntry, "5", "6", "7") );
			}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicGetOnlyPropertyAndConstructor
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7MidPolymorphic;

			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic
			{
				get { return this._Tuple7MidPolymorphic; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicGetOnlyPropertyAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic ) 
			{
				this._Tuple7MidPolymorphic = Tuple7MidPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicGetOnlyPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicPrivateSetterPropertyAndConstructor
		{
			private Tuple<string, string, string, FileSystemEntry, string, string, string> _Tuple7MidPolymorphic;

			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic
			{
				get { return this._Tuple7MidPolymorphic; }
				private set { this._Tuple7MidPolymorphic = value; }
			}

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicPrivateSetterPropertyAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic ) 
			{
				this._Tuple7MidPolymorphic = Tuple7MidPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicPrivateSetterPropertyAndConstructor() {}
		}

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadOnlyFieldAndConstructor
		{
			[MessagePackRuntimeTupleItemType( 4 )]
			public readonly Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadOnlyFieldAndConstructor( Tuple<string, string, string, FileSystemEntry, string, string, string> Tuple7MidPolymorphic ) 
			{
				this.Tuple7MidPolymorphic = Tuple7MidPolymorphic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple7MidPolymorphicReadOnlyFieldAndConstructor() {}
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

		public class PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor
		{
			public readonly Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic;

			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor( Tuple<string, string, string, string, string, string, string, Tuple<string>> Tuple8AllStatic ) 
			{
				this.Tuple8AllStatic = Tuple8AllStatic;
			}
			public PolymorphicMemberTypeRuntimeType_Tuple_Tuple8AllStaticReadOnlyFieldAndConstructor() {}
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
		#endregion ------ RuntimeType.TupleTypes ------
#endif // #if !NETFX_35 && !UNITY

		#endregion ---- RuntimeType ----
		public class PolymorphicMemberTypeMixed
		{
			public string NormalVanilla { get; set; }
			[MessagePackRuntimeType]
			public FileSystemEntry NormalRuntime { get; set; }
			[MessagePackKnownType( "1", typeof( FileEntry ) )]
			[MessagePackKnownType( "2", typeof( DirectoryEntry ) )]
			public FileSystemEntry NormalKnown { get; set; }
			[MessagePackRuntimeType]
			public Object ObjectRuntime { get; set; }
			[MessagePackRuntimeType]
			public Object ObjectRuntimeOmittedType { get; set; }
			public IList<string> ListVanilla { get; set; }
			[MessagePackKnownCollectionItemType( "1", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "2", typeof( DirectoryEntry ) )]
			public IList<FileSystemEntry> ListKnownItem { get; set; }
			[MessagePackKnownType( "1", typeof( Collection<FileSystemEntry> ) )]
			[MessagePackKnownType( "2", typeof( List<FileSystemEntry> ) )]
			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> ListKnwonContainerRuntimeItem { get; set; }
			[MessagePackRuntimeCollectionItemType]
			public IList<object> ListObjectRuntimeItem { get; set; }
			public IDictionary<string, string> DictionaryVanilla { get; set; }
			[MessagePackKnownCollectionItemType( "1", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "2", typeof( DirectoryEntry ) )]
			public IDictionary<string, FileSystemEntry> DictionaryKnownValue { get; set; }
			[MessagePackKnownType( "1", typeof( SortedDictionary<string, FileSystemEntry> ) )]
			[MessagePackKnownType( "2", typeof( Dictionary<string, FileSystemEntry> ) )]
			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, FileSystemEntry> DictionaryKnownContainerRuntimeValue { get; set; }
			[MessagePackRuntimeCollectionItemType]
			public IDictionary<string, object> DictionaryObjectRuntimeValue { get; set; }
#if !NETFX_35 && !UNITY
			[MessagePackKnownTupleItemType( 2, "1", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, "2", typeof( DirectoryEntry ) )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<string, FileSystemEntry, FileSystemEntry, object> Tuple { get; set; }
#endif // !NETFX_35 && !UNITY

			public PolymorphicMemberTypeMixed() { }
		}

		public class AbstractClassMemberNoAttribute
		{
			public AbstractFileSystemEntry Value { get; set; }

			public AbstractClassMemberNoAttribute() { }
		}

		public class AbstractClassMemberKnownType
		{
			[MessagePackKnownType( "1", typeof( FileEntry ) )]
			public AbstractFileSystemEntry Value { get; set; }

			public AbstractClassMemberKnownType() { }
		}

		public class AbstractClassMemberRuntimeType
		{
			[MessagePackRuntimeType]
			public AbstractFileSystemEntry Value { get; set; }

			public AbstractClassMemberRuntimeType() { }
		}

		public class AbstractClassListItemNoAttribute
		{
			public IList<AbstractFileSystemEntry> Value { get; set; }

			public AbstractClassListItemNoAttribute() { }
		}

		public class AbstractClassListItemKnownType
		{
			[MessagePackKnownCollectionItemType( "1", typeof( FileEntry ) )]
			public IList<AbstractFileSystemEntry> Value { get; set; }

			public AbstractClassListItemKnownType() { }
		}

		public class AbstractClassListItemRuntimeType
		{
			[MessagePackRuntimeCollectionItemType]
			public IList<AbstractFileSystemEntry> Value { get; set; }

			public AbstractClassListItemRuntimeType() { }
		}

		public class AbstractClassDictKeyNoAttribute
		{
			public IDictionary<AbstractFileSystemEntry, string> Value { get; set; }

			public AbstractClassDictKeyNoAttribute() { }
		}

		public class AbstractClassDictKeyKnownType
		{
			[MessagePackKnownDictionaryKeyType( "1", typeof( FileEntry ) )]
			public IDictionary<AbstractFileSystemEntry, string> Value { get; set; }

			public AbstractClassDictKeyKnownType() { }
		}

		public class AbstractClassDictKeyRuntimeType
		{
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<AbstractFileSystemEntry, string> Value { get; set; }

			public AbstractClassDictKeyRuntimeType() { }
		}

		public class InterfaceMemberNoAttribute
		{
			public IFileSystemEntry Value { get; set; }

			public InterfaceMemberNoAttribute() { }
		}

		public class InterfaceMemberKnownType
		{
			[MessagePackKnownType( "1", typeof( FileEntry ) )]
			public IFileSystemEntry Value { get; set; }

			public InterfaceMemberKnownType() { }
		}

		public class InterfaceMemberRuntimeType
		{
			[MessagePackRuntimeType]
			public IFileSystemEntry Value { get; set; }

			public InterfaceMemberRuntimeType() { }
		}

		public class InterfaceListItemNoAttribute
		{
			public IList<IFileSystemEntry> Value { get; set; }

			public InterfaceListItemNoAttribute() { }
		}

		public class InterfaceListItemKnownType
		{
			[MessagePackKnownCollectionItemType( "1", typeof( FileEntry ) )]
			public IList<IFileSystemEntry> Value { get; set; }

			public InterfaceListItemKnownType() { }
		}

		public class InterfaceListItemRuntimeType
		{
			[MessagePackRuntimeCollectionItemType]
			public IList<IFileSystemEntry> Value { get; set; }

			public InterfaceListItemRuntimeType() { }
		}

		public class InterfaceDictKeyNoAttribute
		{
			public IDictionary<IFileSystemEntry, string> Value { get; set; }

			public InterfaceDictKeyNoAttribute() { }
		}

		public class InterfaceDictKeyKnownType
		{
			[MessagePackKnownDictionaryKeyType( "1", typeof( FileEntry ) )]
			public IDictionary<IFileSystemEntry, string> Value { get; set; }

			public InterfaceDictKeyKnownType() { }
		}

		public class InterfaceDictKeyRuntimeType
		{
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<IFileSystemEntry, string> Value { get; set; }

			public InterfaceDictKeyRuntimeType() { }
		}

	public class EchoKeyedCollection<TKey, T> : KeyedCollection<TKey, T>
	{
		protected override TKey GetKeyForItem( T item )
		{
			// should be same
			return ( TKey )( object )item;
		}
	}

		public class AbstractClassCollectionNoAttribute
		{
			public KeyedCollection<string, string> Value { get; set; }

			public AbstractClassCollectionNoAttribute() { }
		}

		public class AbstractClassCollectionKnownType
		{
			[MessagePackKnownType( "1", typeof( EchoKeyedCollection<string, string> ) )]
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
			[MessagePackKnownType( "1", typeof( EchoKeyedCollection<string, string> ) )]
			public IList<string> Value { get; set; }

			public InterfaceCollectionKnownType() { }
		}

		public class InterfaceCollectionRuntimeType
		{
			[MessagePackRuntimeType]
			public IList<string> Value { get; set; }

			public InterfaceCollectionRuntimeType() { }
		}
#if !NETFX_35 && !UNITY

		public class TupleAbstractType
		{
			[MessagePackKnownTupleItemType( 1, "1", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 2, "1", typeof( FileEntry ) )]
			[MessagePackRuntimeTupleItemType( 3 )]
			[MessagePackRuntimeTupleItemType( 4 )]
			public Tuple<AbstractFileSystemEntry, IFileSystemEntry, AbstractFileSystemEntry, IFileSystemEntry> Value { get; set; }

			public TupleAbstractType() { }
		}

#endif // !NETFX_35 && !UNITY

		public class DuplicatedKnownMember
		{
			[MessagePackKnownType( "1", typeof( FileEntry ) )]
			[MessagePackKnownType( "1", typeof( FileEntry ) )]
			public FileSystemEntry Value  { get; set; }

			public DuplicatedKnownMember() { }
		}

		public class DuplicatedKnownCollectionItem
		{
			[MessagePackKnownCollectionItemType( "1", typeof( FileEntry ) )]
			[MessagePackKnownCollectionItemType( "1", typeof( FileEntry ) )]
			public IList<FileSystemEntry> Value  { get; set; }

			public DuplicatedKnownCollectionItem() { }
		}

		public class DuplicatedKnownDictionaryKey
		{
			[MessagePackKnownDictionaryKeyType( "1", typeof( FileEntry ) )]
			[MessagePackKnownDictionaryKeyType( "1", typeof( FileEntry ) )]
			public IDictionary<FileSystemEntry, string> Value  { get; set; }

			public DuplicatedKnownDictionaryKey() { }
		}
#if !NETFX_35 && !UNITY
		public class DuplicatedKnownTupleItem
		{
			[MessagePackKnownTupleItemType( 1, "1", typeof( FileEntry ) )]
			[MessagePackKnownTupleItemType( 1, "1", typeof( FileEntry ) )]
			public Tuple<FileSystemEntry> Value  { get; set; }

			public DuplicatedKnownTupleItem() { }
		}
#endif // !NETFX_35 && !UNITY

		public class KnownAndRuntimeMember
		{
			[MessagePackKnownType( "1", typeof( FileEntry ) )]
			[MessagePackRuntimeType]
			public FileSystemEntry Value  { get; set; }

			public KnownAndRuntimeMember() { }
		}

		public class KnownAndRuntimeCollectionItem
		{
			[MessagePackKnownCollectionItemType( "1", typeof( FileEntry ) )]
			[MessagePackRuntimeCollectionItemType]
			public IList<FileSystemEntry> Value  { get; set; }

			public KnownAndRuntimeCollectionItem() { }
		}

		public class KnownAndRuntimeDictionaryKey
		{
			[MessagePackKnownDictionaryKeyType( "1", typeof( FileEntry ) )]
			[MessagePackRuntimeDictionaryKeyType]
			public IDictionary<FileSystemEntry, string> Value  { get; set; }

			public KnownAndRuntimeDictionaryKey() { }
		}
#if !NETFX_35 && !UNITY
		public class KnownAndRuntimeTupleItem
		{
			[MessagePackKnownTupleItemType( 1, "1", typeof( FileEntry ) )]
			[MessagePackRuntimeTupleItemType( 1 )]
			public Tuple<FileSystemEntry> Value  { get; set; }

			public KnownAndRuntimeTupleItem() { }
		}
#endif // !NETFX_35 && !UNITY

		public interface IFileSystemEntry { }

		public abstract class AbstractFileSystemEntry : IFileSystemEntry { }

		public abstract class FileSystemEntry : AbstractFileSystemEntry, IComparable<FileSystemEntry>
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

// Issue #108
namespace MsgPack.UnitTest.TestTypes
{
	public class OmittedType
	{
		public string Value { get; set; }

		public OmittedType() {}

		public override bool Equals( object obj )
		{
			var other = obj as OmittedType;
			if ( other == null )
			{
				return false;
			}

			return this.Value == other.Value;
		}

		public override int GetHashCode()
		{
			return this.Value == null ? 0 : this.Value.GetHashCode();
		}
	}
}

public class TypeInGlobalNamespace
{
	public string Value { get; set; }

	public TypeInGlobalNamespace() { }
}

// Issue 137
public class HasGlobalNamespaceType
{
	[MsgPack.Serialization.MessagePackRuntimeType]
	public TypeInGlobalNamespace GlobalType { get; set; }
}

