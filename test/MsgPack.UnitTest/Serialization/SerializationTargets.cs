#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace MsgPack.Serialization
{
	// Note: Following types use private for non-public instead of internal because internal member accessibility checks may be affected with InternalsVisibleTo attribute.

#pragma warning disable 0414
	// ReSharper disable UnusedMember.Local
	// ReSharper disable NotAccessedField.Local
	// ReSharper disable MemberHidesStaticFromOuterClass
	public class PlainClass
	{
		public static int StaticProperty { get; set; }

		public int PublicProperty { get; set; }
		public int PublicField;

		private readonly int _publicReadOnlyProperty;
		public int PublicReadOnlyProperty { get { return this._publicReadOnlyProperty; } }
		public readonly int PublicReadOnlyField;
		private int NonPublicProperty { get; set; }
		private int NonPublicField;

		private List<int> NonPublicCollectionProperty { get; set; }
		private List<int> NonPublicCollectionField;
		private readonly List<int> _nonPublicCollectionReadOnlyProperty;
		private List<int> NonPublicCollectionReadOnlyProperty { get { return this._nonPublicCollectionReadOnlyProperty; } }
		private readonly List<int> NonPublicCollectionReadOnlyField;

		private Dictionary<string, int> NonPublicDictionaryProperty { get; set; }
		private Dictionary<string, int> NonPublicDictionaryField;
		private readonly Dictionary<string, int> _nonPublicDictionaryReadOnlyProperty;
		private Dictionary<string, int> NonPublicDictionaryReadOnlyProperty { get { return this._nonPublicDictionaryReadOnlyProperty; } }
		private readonly Dictionary<string, int> NonPublicDictionaryReadOnlyField;

		private System.Collections.Hashtable NonPublicHashtableProperty { get; set; }
		private System.Collections.Hashtable NonPublicHashtableField;
		private readonly System.Collections.Hashtable _nonPublicHashtableReadOnlyProperty;
		private System.Collections.Hashtable NonPublicHashtableReadOnlyProperty { get { return this._nonPublicHashtableReadOnlyProperty; } }
		private readonly System.Collections.Hashtable NonPublicHashtableReadOnlyField;

#if !NETFX_CORE && !SILVERLIGHT
		[NonSerialized]
		public int NonSerializedPublicField;

		[NonSerialized]
		public readonly int NonSerializedPublicReadOnlyField;

		[NonSerialized]
		// ReSharper disable once InconsistentNaming
		private int NonSerializedNonPublicField;
#endif // !NETFX_CORE && !SILVERLIGHT
		private readonly List<int> _collectionReadOnlyProperty;
		public List<int> CollectionReadOnlyProperty { get { return this._collectionReadOnlyProperty; } }

		public PlainClass()
		{
			StaticProperty = -1;
			this.PublicProperty = 1;
			this.PublicField = 2;
			this._publicReadOnlyProperty = 3;
			this.PublicReadOnlyField = 4;
			this.NonPublicProperty = 5;
			this.NonPublicField = 6;
#if !NETFX_CORE && !SILVERLIGHT
			this.NonSerializedPublicField = 7;
			this.NonSerializedPublicReadOnlyField = 8;
			this.NonSerializedNonPublicField = 9;
#endif // !NETFX_CORE && !SILVERLIGHT
			this._collectionReadOnlyProperty = new List<int>();
			this.NonPublicCollectionProperty = new List<int>();
			this.NonPublicCollectionField = new List<int>();
			this._nonPublicCollectionReadOnlyProperty = new List<int>();
			this.NonPublicCollectionReadOnlyField = new List<int>();
			this.NonPublicDictionaryProperty = new Dictionary<string, int>();
			this.NonPublicDictionaryField = new Dictionary<string, int>();
			this._nonPublicDictionaryReadOnlyProperty = new Dictionary<string, int>();
			this.NonPublicDictionaryReadOnlyField = new Dictionary<string, int>();
			this.NonPublicHashtableProperty = new System.Collections.Hashtable();
			this.NonPublicHashtableField = new System.Collections.Hashtable();
			this._nonPublicHashtableReadOnlyProperty = new System.Collections.Hashtable();
			this.NonPublicHashtableReadOnlyField = new System.Collections.Hashtable();
		}

		public void InitializeCollectionMembers()
		{
			this.CollectionReadOnlyProperty.Add( 51 );
			this.CollectionReadOnlyProperty.Add( 52 );
			this.NonPublicCollectionProperty.Add( 61 );
			this.NonPublicCollectionProperty.Add( 62 );
			this.NonPublicCollectionField.Add( 63 );
			this.NonPublicCollectionField.Add( 64 );
			this._nonPublicCollectionReadOnlyProperty.Add( 65 );
			this._nonPublicCollectionReadOnlyProperty.Add( 66 );
			this.NonPublicCollectionReadOnlyField.Add( 67 );
			this.NonPublicCollectionReadOnlyField.Add( 68 );
			this.NonPublicDictionaryProperty.Add( "71", 71 );
			this.NonPublicDictionaryProperty.Add( "72", 72 );
			this.NonPublicDictionaryField.Add( "73", 73 );
			this.NonPublicDictionaryField.Add( "74", 74 );
			this._nonPublicDictionaryReadOnlyProperty.Add( "75", 75 );
			this._nonPublicDictionaryReadOnlyProperty.Add( "76", 76 );
			this.NonPublicDictionaryReadOnlyField.Add( "77", 77 );
			this.NonPublicDictionaryReadOnlyField.Add( "78", 78 );
			this.NonPublicHashtableProperty.Add( "81", 81 );
			this.NonPublicHashtableProperty.Add( "82", 82 );
			this.NonPublicHashtableField.Add( "83", 83 );
			this.NonPublicHashtableField.Add( "84", 84 );
			this._nonPublicHashtableReadOnlyProperty.Add( "85", 85 );
			this._nonPublicHashtableReadOnlyProperty.Add( "86", 86 );
			this.NonPublicHashtableReadOnlyField.Add( "87", 87 );
			this.NonPublicHashtableReadOnlyField.Add( "88", 88 );
		}
	}

	public class AnnotatedClass
	{
		public static int StaticProperty { get; set; }

		[MessagePackMember( 0, Name = "Alias" )]
		public int PublicProperty { get; set; }
		[MessagePackMember( 1 )]
		public int PublicField;

		private int _PublicReadOnlyProperty;
		[MessagePackMember( 2 )]
		public int PublicReadOnlyProperty { get { return this._PublicReadOnlyProperty; } }
		[MessagePackMember( 3 )]
		public readonly int PublicReadOnlyField;
		[MessagePackMember( 4 )]
		private int NonPublicProperty { get; set; }
		[MessagePackMember( 5 )]
		private int NonPublicField;

#if !NETFX_CORE && !SILVERLIGHT
		[NonSerialized]
		[MessagePackMember( 6 )]
		public int NonSerializedPublicField;

		[NonSerialized]
		[MessagePackMember( 7 )]
		public readonly int NonSerializedPublicReadOnlyField;

		[NonSerialized]
		[MessagePackMember( 8 )]
		// ReSharper disable once InconsistentNaming
		private int NonSerializedNonPublicField;
#endif // !NETFX_CORE && !SILVERLIGHT
		private readonly List<int> _collectionReadOnlyProperty;
		[MessagePackMember( 9 )]
		public List<int> CollectionReadOnlyProperty { get { return this._collectionReadOnlyProperty; } }

		[MessagePackMember( 10 )]
		private List<int> NonPublicCollectionProperty { get; set; }
		[MessagePackMember( 11 )]
		private List<int> NonPublicCollectionField;
		private readonly List<int> _nonPublicCollectionReadOnlyProperty;
		[MessagePackMember( 12 )]
		private List<int> NonPublicCollectionReadOnlyProperty { get { return this._nonPublicCollectionReadOnlyProperty; } }
		[MessagePackMember( 13 )]
		private readonly List<int> NonPublicCollectionReadOnlyField;

		[MessagePackMember( 14 )]
		private Dictionary<string, int> NonPublicDictionaryProperty { get; set; }
		[MessagePackMember( 15 )]
		private Dictionary<string, int> NonPublicDictionaryField;
		private readonly Dictionary<string, int> _nonPublicDictionaryReadOnlyProperty;
		[MessagePackMember( 16 )]
		private Dictionary<string, int> NonPublicDictionaryReadOnlyProperty { get { return this._nonPublicDictionaryReadOnlyProperty; } }
		[MessagePackMember( 17 )]
		private readonly Dictionary<string, int> NonPublicDictionaryReadOnlyField;

		[MessagePackMember( 18 )]
		private System.Collections.Hashtable NonPublicHashtableProperty { get; set; }
		[MessagePackMember( 19 )]
		private System.Collections.Hashtable NonPublicHashtableField;
		private readonly System.Collections.Hashtable _nonPublicHashtableReadOnlyProperty;
		[MessagePackMember( 20 )]
		private System.Collections.Hashtable NonPublicHashtableReadOnlyProperty { get { return this._nonPublicHashtableReadOnlyProperty; } }
		[MessagePackMember( 21 )]
		private readonly System.Collections.Hashtable NonPublicHashtableReadOnlyField;

		public int PublicPropertyPlain { get; set; }
		public int PublicFieldPlain;
		private readonly int _publicReadOnlyPropertyPlain;
		public int PublicReadOnlyPropertyPlain { get { return this._publicReadOnlyPropertyPlain; } }
		public int PublicReadOnlyFieldPlain;
		private int NonPublicPropertyPlain { get; set; }
		private int NonPublicFieldPlain;

		private List<int> NonPublicCollectionPropertyPlain { get; set; }
		private List<int> NonPublicCollectionFieldPlain;
		private readonly List<int> _nonPublicCollectionReadOnlyPropertyPlain;
		private List<int> NonPublicCollectionReadOnlyPropertyPlain { get { return this._nonPublicCollectionReadOnlyPropertyPlain; } }
		private readonly List<int> NonPublicCollectionReadOnlyFieldPlain;

		private Dictionary<string, int> NonPublicDictionaryPropertyPlain { get; set; }
		private Dictionary<string, int> NonPublicDictionaryFieldPlain;
		private readonly Dictionary<string, int> _nonPublicDictionaryReadOnlyPropertyPlain;
		private Dictionary<string, int> NonPublicDictionaryReadOnlyPropertyPlain { get { return this._nonPublicDictionaryReadOnlyPropertyPlain; } }
		private readonly Dictionary<string, int> NonPublicDictionaryReadOnlyFieldPlain;

		private System.Collections.Hashtable NonPublicHashtablePropertyPlain { get; set; }
		private System.Collections.Hashtable NonPublicHashtableFieldPlain;
		private readonly System.Collections.Hashtable _nonPublicHashtableReadOnlyPropertyPlain;
		private System.Collections.Hashtable NonPublicHashtableReadOnlyPropertyPlain { get { return this._nonPublicHashtableReadOnlyPropertyPlain; } }
		private readonly System.Collections.Hashtable NonPublicHashtableReadOnlyFieldPlain;

#if !NETFX_CORE && !SILVERLIGHT
		[NonSerialized]
		public int NonSerializedPublicFieldPlain;

		[NonSerialized]
		public readonly int NonSerializedPublicReadOnlyFieldPlain;

		[NonSerialized]
		// ReSharper disable once InconsistentNaming
		private int NonSerializedNonPublicFieldPlain;
#endif // !NETFX_CORE && !SILVERLIGHT

		public AnnotatedClass()
		{
			StaticProperty = -1;
			this.PublicProperty = 1;
			this.PublicField = 2;
			this._PublicReadOnlyProperty = 3;
			this.PublicReadOnlyField = 4;
			this.NonPublicProperty = 5;
			this.NonPublicField = 6;
#if !NETFX_CORE && !SILVERLIGHT
			this.NonSerializedPublicField = 7;
			this.NonSerializedPublicReadOnlyField = 8;
			this.NonSerializedNonPublicField = 9;
#endif // !NETFX_CORE && !SILVERLIGHT
			this._collectionReadOnlyProperty = new List<int>();
			this.PublicPropertyPlain = 11;
			this.PublicFieldPlain = 12;
			this._publicReadOnlyPropertyPlain = 13;
			this.PublicReadOnlyFieldPlain = 14;
			this.NonPublicPropertyPlain = 15;
			this.NonPublicFieldPlain = 16;
#if !NETFX_CORE && !SILVERLIGHT
			this.NonSerializedPublicFieldPlain = 17;
			this.NonSerializedPublicReadOnlyFieldPlain = 18;
			this.NonSerializedNonPublicFieldPlain = 19;
#endif // !NETFX_CORE && !SILVERLIGHT
			this.NonPublicCollectionProperty = new List<int>();
			this.NonPublicCollectionField = new List<int>();
			this._nonPublicCollectionReadOnlyProperty = new List<int>();
			this.NonPublicCollectionReadOnlyField = new List<int>();
			this.NonPublicDictionaryProperty = new Dictionary<string, int>();
			this.NonPublicDictionaryField = new Dictionary<string, int>();
			this._nonPublicDictionaryReadOnlyProperty = new Dictionary<string, int>();
			this.NonPublicDictionaryReadOnlyField = new Dictionary<string, int>();
			this.NonPublicHashtableProperty = new System.Collections.Hashtable();
			this.NonPublicHashtableField = new System.Collections.Hashtable();
			this._nonPublicHashtableReadOnlyProperty = new System.Collections.Hashtable();
			this.NonPublicHashtableReadOnlyField = new System.Collections.Hashtable();
			this.NonPublicCollectionPropertyPlain = new List<int>();
			this.NonPublicCollectionFieldPlain = new List<int>();
			this._nonPublicCollectionReadOnlyPropertyPlain = new List<int>();
			this.NonPublicCollectionReadOnlyFieldPlain = new List<int>();
			this.NonPublicDictionaryPropertyPlain = new Dictionary<string, int>();
			this.NonPublicDictionaryFieldPlain = new Dictionary<string, int>();
			this._nonPublicDictionaryReadOnlyPropertyPlain = new Dictionary<string, int>();
			this.NonPublicDictionaryReadOnlyFieldPlain = new Dictionary<string, int>();
			this.NonPublicHashtablePropertyPlain = new System.Collections.Hashtable();
			this.NonPublicHashtableFieldPlain = new System.Collections.Hashtable();
			this._nonPublicHashtableReadOnlyPropertyPlain = new System.Collections.Hashtable();
			this.NonPublicHashtableReadOnlyFieldPlain = new System.Collections.Hashtable();
		}

		public void InitializeCollectionMembers()
		{
			this.CollectionReadOnlyProperty.Add( 51 );
			this.CollectionReadOnlyProperty.Add( 52 );
			this.NonPublicCollectionProperty.Add( 61 );
			this.NonPublicCollectionProperty.Add( 62 );
			this.NonPublicCollectionField.Add( 63 );
			this.NonPublicCollectionField.Add( 64 );
			this._nonPublicCollectionReadOnlyProperty.Add( 65 );
			this._nonPublicCollectionReadOnlyProperty.Add( 66 );
			this.NonPublicCollectionReadOnlyField.Add( 67 );
			this.NonPublicCollectionReadOnlyField.Add( 68 );
			this.NonPublicDictionaryProperty.Add( "71", 71 );
			this.NonPublicDictionaryProperty.Add( "72", 72 );
			this.NonPublicDictionaryField.Add( "73", 73 );
			this.NonPublicDictionaryField.Add( "74", 74 );
			this._nonPublicDictionaryReadOnlyProperty.Add( "75", 75 );
			this._nonPublicDictionaryReadOnlyProperty.Add( "76", 76 );
			this.NonPublicDictionaryReadOnlyField.Add( "77", 77 );
			this.NonPublicDictionaryReadOnlyField.Add( "78", 78 );
			this.NonPublicHashtableProperty.Add( "81", 81 );
			this.NonPublicHashtableProperty.Add( "82", 82 );
			this.NonPublicHashtableField.Add( "83", 83 );
			this.NonPublicHashtableField.Add( "84", 84 );
			this._nonPublicHashtableReadOnlyProperty.Add( "85", 85 );
			this._nonPublicHashtableReadOnlyProperty.Add( "86", 86 );
			this.NonPublicHashtableReadOnlyField.Add( "87", 87 );
			this.NonPublicHashtableReadOnlyField.Add( "88", 88 );
			this.NonPublicCollectionPropertyPlain.Add( 91 );
			this.NonPublicCollectionPropertyPlain.Add( 92 );
			this.NonPublicCollectionFieldPlain.Add( 93 );
			this.NonPublicCollectionFieldPlain.Add( 94 );
			this._nonPublicCollectionReadOnlyPropertyPlain.Add( 95 );
			this._nonPublicCollectionReadOnlyPropertyPlain.Add( 96 );
			this.NonPublicCollectionReadOnlyFieldPlain.Add( 97 );
			this.NonPublicCollectionReadOnlyFieldPlain.Add( 98 );
			this.NonPublicDictionaryPropertyPlain.Add( "101", 101 );
			this.NonPublicDictionaryPropertyPlain.Add( "102", 102 );
			this.NonPublicDictionaryFieldPlain.Add( "103", 103 );
			this.NonPublicDictionaryFieldPlain.Add( "104", 104 );
			this._nonPublicDictionaryReadOnlyPropertyPlain.Add( "105", 105 );
			this._nonPublicDictionaryReadOnlyPropertyPlain.Add( "106", 106 );
			this.NonPublicDictionaryReadOnlyFieldPlain.Add( "107", 197 );
			this.NonPublicDictionaryReadOnlyFieldPlain.Add( "108", 108 );
			this.NonPublicHashtablePropertyPlain.Add( "111", 111 );
			this.NonPublicHashtablePropertyPlain.Add( "112", 112 );
			this.NonPublicHashtableFieldPlain.Add( "113", 113 );
			this.NonPublicHashtableFieldPlain.Add( "114", 114 );
			this._nonPublicHashtableReadOnlyPropertyPlain.Add( "115", 115 );
			this._nonPublicHashtableReadOnlyPropertyPlain.Add( "116", 116 );
			this.NonPublicHashtableReadOnlyFieldPlain.Add( "117", 117 );
			this.NonPublicHashtableReadOnlyFieldPlain.Add( "118", 118 );
		}
	}

	[DataContract]
	public class DataMamberClass
	{
		public static int StaticProperty { get; set; }

		[DataMember( Order = 0, Name = "Alias" )]
		public int PublicProperty { get; set; }
		[DataMember( Order = 1 )]
		public int PublicField;

		private int _PublicReadOnlyProperty;
		[DataMember( Order = 2 )]
		public int PublicReadOnlyProperty { get { return this._PublicReadOnlyProperty; } }
		[DataMember( Order = 3 )]
		public readonly int PublicReadOnlyField;
		[DataMember( Order = 4 )]
		private int NonPublicProperty { get; set; }
		[DataMember( Order = 5 )]
		private int NonPublicField;

#if !NETFX_CORE && !SILVERLIGHT
		[NonSerialized]
		[DataMember( Order = 6 )]
		public int NonSerializedPublicField;

		[NonSerialized]
		[DataMember( Order = 7 )]
		public readonly int NonSerializedPublicReadOnlyField;

		[NonSerialized]
		[DataMember( Order = 8 )]
		// ReSharper disable once InconsistentNaming
		private int NonSerializedNonPublicField;
#endif // !NETFX_CORE && !SILVERLIGHT
		private readonly List<int> _collectionReadOnlyProperty;
		[DataMember( Order = 9 )]
		public List<int> CollectionReadOnlyProperty { get { return this._collectionReadOnlyProperty; } }

		[DataMember( Order = 10 )]
		private List<int> NonPublicCollectionProperty { get; set; }
		[DataMember( Order = 11 )]
		private List<int> NonPublicCollectionField;

		private readonly List<int> _nonPublicCollectionReadOnlyProperty;
		[DataMember( Order = 12 )]
		private List<int> NonPublicCollectionReadOnlyProperty { get { return this._nonPublicCollectionReadOnlyProperty; } }
		[DataMember( Order = 13 )]
		private readonly List<int> NonPublicCollectionReadOnlyField;

		[DataMember( Order = 14 )]
		private Dictionary<string, int> NonPublicDictionaryProperty { get; set; }
		[DataMember( Order = 15 )]
		private Dictionary<string, int> NonPublicDictionaryField;
		private readonly Dictionary<string, int> _nonPublicDictionaryReadOnlyProperty;
		[DataMember( Order = 16 )]
		private Dictionary<string, int> NonPublicDictionaryReadOnlyProperty { get { return this._nonPublicDictionaryReadOnlyProperty; } }
		[DataMember( Order = 17 )]
		private readonly Dictionary<string, int> NonPublicDictionaryReadOnlyField;

		[DataMember( Order = 18 )]
		private System.Collections.Hashtable NonPublicHashtableProperty { get; set; }
		[DataMember( Order = 19 )]
		private System.Collections.Hashtable NonPublicHashtableField;
		private readonly System.Collections.Hashtable _nonPublicHashtableReadOnlyProperty;
		[DataMember( Order = 20 )]
		private System.Collections.Hashtable NonPublicHashtableReadOnlyProperty { get { return this._nonPublicHashtableReadOnlyProperty; } }
		[DataMember( Order = 21 )]
		private readonly System.Collections.Hashtable NonPublicHashtableReadOnlyField;

		public int PublicPropertyPlain { get; set; }
		public int PublicFieldPlain;
		private readonly int _publicReadOnlyPropertyPlain;
		public int PublicReadOnlyPropertyPlain { get { return this._publicReadOnlyPropertyPlain; } }
		public int PublicReadOnlyFieldPlain;
		private int NonPublicPropertyPlain { get; set; }
		private int NonPublicFieldPlain;

		private List<int> NonPublicCollectionPropertyPlain { get; set; }
		private List<int> NonPublicCollectionFieldPlain;
		private readonly List<int> _nonPublicCollectionReadOnlyPropertyPlain;
		private List<int> NonPublicCollectionReadOnlyPropertyPlain { get { return this._nonPublicCollectionReadOnlyPropertyPlain; } }
		private readonly List<int> NonPublicCollectionReadOnlyFieldPlain;

		private Dictionary<string, int> NonPublicDictionaryPropertyPlain { get; set; }
		private Dictionary<string, int> NonPublicDictionaryFieldPlain;
		private readonly Dictionary<string, int> _nonPublicDictionaryReadOnlyPropertyPlain;
		private Dictionary<string, int> NonPublicDictionaryReadOnlyPropertyPlain { get { return this._nonPublicDictionaryReadOnlyPropertyPlain; } }
		private readonly Dictionary<string, int> NonPublicDictionaryReadOnlyFieldPlain;

		private System.Collections.Hashtable NonPublicHashtablePropertyPlain { get; set; }
		private System.Collections.Hashtable NonPublicHashtableFieldPlain;
		private readonly System.Collections.Hashtable _nonPublicHashtableReadOnlyPropertyPlain;
		private System.Collections.Hashtable NonPublicHashtableReadOnlyPropertyPlain { get { return this._nonPublicHashtableReadOnlyPropertyPlain; } }
		private readonly System.Collections.Hashtable NonPublicHashtableReadOnlyFieldPlain;

#if !NETFX_CORE && !SILVERLIGHT
		[NonSerialized]
		public int NonSerializedPublicFieldPlain;

		[NonSerialized]
		public readonly int NonSerializedPublicReadOnlyFieldPlain;

		[NonSerialized]
		// ReSharper disable once InconsistentNaming
		private int NonSerializedNonPublicFieldPlain;
#endif // !NETFX_CORE && !SILVERLIGHT

		public DataMamberClass()
		{
			StaticProperty = -1;
			this.PublicProperty = 1;
			this.PublicField = 2;
			this._PublicReadOnlyProperty = 3;
			this.PublicReadOnlyField = 4;
			this.NonPublicProperty = 5;
			this.NonPublicField = 6;
#if !NETFX_CORE && !SILVERLIGHT
			this.NonSerializedPublicField = 7;
			this.NonSerializedPublicReadOnlyField = 8;
			this.NonSerializedNonPublicField = 9;
#endif // !NETFX_CORE && !SILVERLIGHT
			this._collectionReadOnlyProperty = new List<int>();
			this.PublicPropertyPlain = 11;
			this.PublicFieldPlain = 12;
			this._publicReadOnlyPropertyPlain = 13;
			this.PublicReadOnlyFieldPlain = 14;
			this.NonPublicPropertyPlain = 15;
			this.NonPublicFieldPlain = 16;
#if !NETFX_CORE && !SILVERLIGHT
			this.NonSerializedPublicFieldPlain = 17;
			this.NonSerializedPublicReadOnlyFieldPlain = 18;
			this.NonSerializedNonPublicFieldPlain = 19;
#endif // !NETFX_CORE && !SILVERLIGHT
			this.NonPublicCollectionProperty = new List<int>();
			this.NonPublicCollectionField = new List<int>();
			this._nonPublicCollectionReadOnlyProperty = new List<int>();
			this.NonPublicCollectionReadOnlyField = new List<int>();
			this.NonPublicDictionaryProperty = new Dictionary<string, int>();
			this.NonPublicDictionaryField = new Dictionary<string, int>();
			this._nonPublicDictionaryReadOnlyProperty = new Dictionary<string, int>();
			this.NonPublicDictionaryReadOnlyField = new Dictionary<string, int>();
			this.NonPublicHashtableProperty = new System.Collections.Hashtable();
			this.NonPublicHashtableField = new System.Collections.Hashtable();
			this._nonPublicHashtableReadOnlyProperty = new System.Collections.Hashtable();
			this.NonPublicHashtableReadOnlyField = new System.Collections.Hashtable();
			this.NonPublicCollectionPropertyPlain = new List<int>();
			this.NonPublicCollectionFieldPlain = new List<int>();
			this._nonPublicCollectionReadOnlyPropertyPlain = new List<int>();
			this.NonPublicCollectionReadOnlyFieldPlain = new List<int>();
			this.NonPublicDictionaryPropertyPlain = new Dictionary<string, int>();
			this.NonPublicDictionaryFieldPlain = new Dictionary<string, int>();
			this._nonPublicDictionaryReadOnlyPropertyPlain = new Dictionary<string, int>();
			this.NonPublicDictionaryReadOnlyFieldPlain = new Dictionary<string, int>();
			this.NonPublicHashtablePropertyPlain = new System.Collections.Hashtable();
			this.NonPublicHashtableFieldPlain = new System.Collections.Hashtable();
			this._nonPublicHashtableReadOnlyPropertyPlain = new System.Collections.Hashtable();
			this.NonPublicHashtableReadOnlyFieldPlain = new System.Collections.Hashtable();
		}

		public void InitializeCollectionMembers()
		{
			this.CollectionReadOnlyProperty.Add( 51 );
			this.CollectionReadOnlyProperty.Add( 52 );
			this.NonPublicCollectionProperty.Add( 61 );
			this.NonPublicCollectionProperty.Add( 62 );
			this.NonPublicCollectionField.Add( 63 );
			this.NonPublicCollectionField.Add( 64 );
			this._nonPublicCollectionReadOnlyProperty.Add( 65 );
			this._nonPublicCollectionReadOnlyProperty.Add( 66 );
			this.NonPublicCollectionReadOnlyField.Add( 67 );
			this.NonPublicCollectionReadOnlyField.Add( 68 );
			this.NonPublicDictionaryProperty.Add( "71", 71 );
			this.NonPublicDictionaryProperty.Add( "72", 72 );
			this.NonPublicDictionaryField.Add( "73", 73 );
			this.NonPublicDictionaryField.Add( "74", 74 );
			this._nonPublicDictionaryReadOnlyProperty.Add( "75", 75 );
			this._nonPublicDictionaryReadOnlyProperty.Add( "76", 76 );
			this.NonPublicDictionaryReadOnlyField.Add( "77", 77 );
			this.NonPublicDictionaryReadOnlyField.Add( "78", 78 );
			this.NonPublicHashtableProperty.Add( "81", 81 );
			this.NonPublicHashtableProperty.Add( "82", 82 );
			this.NonPublicHashtableField.Add( "83", 83 );
			this.NonPublicHashtableField.Add( "84", 84 );
			this._nonPublicHashtableReadOnlyProperty.Add( "85", 85 );
			this._nonPublicHashtableReadOnlyProperty.Add( "86", 86 );
			this.NonPublicHashtableReadOnlyField.Add( "87", 87 );
			this.NonPublicHashtableReadOnlyField.Add( "88", 88 );
			this.NonPublicCollectionPropertyPlain.Add( 91 );
			this.NonPublicCollectionPropertyPlain.Add( 92 );
			this.NonPublicCollectionFieldPlain.Add( 93 );
			this.NonPublicCollectionFieldPlain.Add( 94 );
			this._nonPublicCollectionReadOnlyPropertyPlain.Add( 95 );
			this._nonPublicCollectionReadOnlyPropertyPlain.Add( 96 );
			this.NonPublicCollectionReadOnlyFieldPlain.Add( 97 );
			this.NonPublicCollectionReadOnlyFieldPlain.Add( 98 );
			this.NonPublicDictionaryPropertyPlain.Add( "101", 101 );
			this.NonPublicDictionaryPropertyPlain.Add( "102", 102 );
			this.NonPublicDictionaryFieldPlain.Add( "103", 103 );
			this.NonPublicDictionaryFieldPlain.Add( "104", 104 );
			this._nonPublicDictionaryReadOnlyPropertyPlain.Add( "105", 105 );
			this._nonPublicDictionaryReadOnlyPropertyPlain.Add( "106", 106 );
			this.NonPublicDictionaryReadOnlyFieldPlain.Add( "107", 197 );
			this.NonPublicDictionaryReadOnlyFieldPlain.Add( "108", 108 );
			this.NonPublicHashtablePropertyPlain.Add( "111", 111 );
			this.NonPublicHashtablePropertyPlain.Add( "112", 112 );
			this.NonPublicHashtableFieldPlain.Add( "113", 113 );
			this.NonPublicHashtableFieldPlain.Add( "114", 114 );
			this._nonPublicHashtableReadOnlyPropertyPlain.Add( "115", 115 );
			this._nonPublicHashtableReadOnlyPropertyPlain.Add( "116", 116 );
			this.NonPublicHashtableReadOnlyFieldPlain.Add( "117", 117 );
			this.NonPublicHashtableReadOnlyFieldPlain.Add( "118", 118 );
		}
	}

	public class IgnoreAttributesTester
	{
		[NonSerialized]
		public string NonSerialized;

		[MessagePackIgnore]
		public string MessagePackIgnore;

		[IgnoreDataMember]
		public string IgnoreDataMember;

		public string Vanilla;
	}

	public class WithIndexerOverload
	{
		public string this[ int index ]
		{
			get { return null; }
			set { }
		}

		public string this[ string key ]
		{
			get { return null; }
			set { }
		}
	}

	public class WithKeyDuplicate
	{
		[MessagePackMember( 0, Name = "Boo" )]
		public string Foo { get; set; }

		[MessagePackMember( 1, Name = "Boo" )]
		public int Bar { get; set; }
	}

	// ReSharper restore MemberHidesStaticFromOuterClass
	// ReSharper restore NotAccessedField.Local
	// ReSharper restore UnusedMember.Local
#pragma warning restore 0414
}
