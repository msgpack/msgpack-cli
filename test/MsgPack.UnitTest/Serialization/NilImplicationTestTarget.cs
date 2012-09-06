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

using System;

namespace MsgPack.Serialization
{
	public class NilImplicationTestTarget
	{
		[MessagePackMember( 0, NilImplication = NilImplication.MemberDefault )]
		public int MemberDefault = 1;

		[MessagePackMember( 1, NilImplication = NilImplication.Null )]
		public int NullButValueType = 2;

		[MessagePackMember( 2, NilImplication = NilImplication.Null )]
		public int? NullAndNullableValueType = 3;

		[MessagePackMember( 3, NilImplication = NilImplication.Null )]
		public string NullAndReferenceType = "4";

		[MessagePackMember( 4, NilImplication = NilImplication.Prohibit )]
		public string ProhibitReferenceType = "5";
	}
}