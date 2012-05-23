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

using System;
using System.Collections.Generic;

namespace MsgPack.Serialization
{
	public class NilImplicationCollectionTestTarget
	{
		private List<int> _memberDefault = new List<int>() { 0 };

		[MessagePackMember( 0, NilImplication = NilImplication.MemberDefault )]
		public List<int> MemberDefault
		{
			get { return this._memberDefault; }
			internal set { this._memberDefault = value; }
		}

		private List<int> _null = new List<int>() { 1 };

		[MessagePackMember( 1, NilImplication = NilImplication.Null )]
		public List<int> Null
		{
			get { return this._null; }
			internal set { this._null = value; }
		}

		private List<int> _prohibit = new List<int>() { 2 };

		[MessagePackMember( 2, NilImplication = NilImplication.Prohibit )]
		public List<int> Prohibit
		{
			get { return this._prohibit; }
			internal set { this._prohibit = value; }
		}
	}
}
