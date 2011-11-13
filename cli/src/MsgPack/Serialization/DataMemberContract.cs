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
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents member's data contract.
	/// </summary>
	internal struct DataMemberContract
	{
		private readonly MemberInfo _member;
		private readonly DataMemberAttribute _attribute;

		public string Name
		{
			get { return this._attribute == null ? this._member.Name : ( this._attribute.Name ?? this._member.Name ); }
		}

		public int Order
		{
			get { return this._attribute == null ? -1 : this._attribute.Order; }
		}

		public bool IsRequired
		{
			get { return this._attribute == null ? false : this._attribute.IsRequired; }
		}

		public bool EmitDefaultValue
		{
			get { return this._attribute == null ? true : this._attribute.EmitDefaultValue; }
		}

		public DataMemberContract( MemberInfo member, DataMemberAttribute attribute )
		{
			this._member = member;
			this._attribute = attribute;
		}
	}
}
