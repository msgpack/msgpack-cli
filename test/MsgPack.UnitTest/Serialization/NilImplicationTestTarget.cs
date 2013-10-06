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
using System.Collections.Generic;

namespace MsgPack.Serialization
{
	public class NilImplicationTestTargetForValueTypeMemberDefault
	{
		[MessagePackMember( 0, NilImplication = NilImplication.MemberDefault )]
		public int MemberDefault = 1;
	}

	// Illegal
	public class NilImplicationTestTargetForValueTypeNull
	{
		[MessagePackMember( 0, NilImplication = NilImplication.Null )]
		public int Null = 1;
	}

	public class NilImplicationTestTargetForValueTypeProhibit
	{
		[MessagePackMember( 0, NilImplication = NilImplication.Prohibit )]
		public int Prohibit = 1;
	}

	public class NilImplicationTestTargetForNullableValueType
	{
		[MessagePackMember( 0, NilImplication = NilImplication.MemberDefault )]
		public int? MemberDefault = 1;

		[MessagePackMember( 1, NilImplication = NilImplication.Null )]
		public int? Null = 2;

		[MessagePackMember( 2, NilImplication = NilImplication.Prohibit )]
		public int? Prohibit = 3;
	}

	public class NilImplicationTestTargetForReferenceType
	{
		[MessagePackMember( 0, NilImplication = NilImplication.MemberDefault )]
		public string MemberDefault = "1";

		[MessagePackMember( 1, NilImplication = NilImplication.Null )]
		public string Null = "2";

		[MessagePackMember( 2, NilImplication = NilImplication.Prohibit )]
		public string Prohibit = "3";
	}

	public class NilImplicationTestTargetForReadOnlyCollectionPropertyMemberDefault
	{
		private List<int> _memberDefault = new List<int>() { 0 };

		[MessagePackMember( 0, NilImplication = NilImplication.MemberDefault )]
		public List<int> MemberDefault
		{
			get { return this._memberDefault; }
			// This property should not set by generated serializer.
			internal set { this._memberDefault = value; }
		}
	}

	// Illgal
	public class NilImplicationTestTargetForReadOnlyCollectionPropertyNull
	{
		private List<int> _null = new List<int>() { 0 };

		[MessagePackMember( 0, NilImplication = NilImplication.Null )]
		public List<int> Null
		{
			get { return this._null; }
			// This property should not set by generated serializer.
			internal set { this._null = value; }
		}
	}

	// Illegal
	public class NilImplicationTestTargetForReadOnlyCollectionPropertyProhibit
	{
		private List<int> _prohibit = new List<int>() { 0 };

		[MessagePackMember( 0, NilImplication = NilImplication.Prohibit )]
		public List<int> Prohibit
		{
			get { return this._prohibit; }
			// This property should not set by generated serializer.
			internal set { this._prohibit = value; }
		}
	}
}