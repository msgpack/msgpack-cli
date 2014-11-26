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

		public int PublicPropertyPlain { get; set; }
		public int PublicFieldPlain;
		private readonly int _publicReadOnlyPropertyPlain;
		public int PublicReadOnlyPropertyPlain { get { return this._publicReadOnlyPropertyPlain; } }
		public int PublicReadOnlyFieldPlain;
		private int NonPublicPropertyPlain { get; set; }
		private int NonPublicFieldPlain;

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

		public int PublicPropertyPlain { get; set; }
		public int PublicFieldPlain;
		private readonly int _publicReadOnlyPropertyPlain;
		public int PublicReadOnlyPropertyPlain { get { return this._publicReadOnlyPropertyPlain; } }
		public int PublicReadOnlyFieldPlain;
		private int NonPublicPropertyPlain { get; set; }
		private int NonPublicFieldPlain;

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
		}
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
