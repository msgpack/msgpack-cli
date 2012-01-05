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
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents member's data contract.
	/// </summary>
	internal struct DataMemberContract
	{
		internal const int UnspecifiedOrder = -1;

		private readonly MemberInfo _member;
		private readonly DataMemberAttribute _attribute;
		
		/// <summary>
		///		Gets the name of the member.
		/// </summary>
		/// <value>
		///		The name of the member.
		/// </value>
		/// <seealso cref="System.Runtime.Serialization.DataMemberAttribute"/>
		public string Name
		{
			get { return this._attribute == null ? this._member.Name : ( this._attribute.Name ?? this._member.Name ); }
		}

		/// <summary>
		///		Gets the order of the member.
		/// </summary>
		/// <value>
		///		The order of the member. Default is <c>-1</c>.
		/// </value>
		/// <seealso cref="System.Runtime.Serialization.DataMemberAttribute"/>
		public int Order
		{
			get { return this._attribute == null ? UnspecifiedOrder : this._attribute.Order; }
		}

		// TODO: IsRequired
		/// <summary>
		///		Gets a value indicating whether this instance is required.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is required; otherwise, <c>false</c>.
		/// </value>
		/// <seealso cref="System.Runtime.Serialization.DataMemberAttribute"/>
		public bool IsRequired
		{
			get { return this._attribute == null ? false : this._attribute.IsRequired; }
		}

		// TODO: EmitDefaultValue
		/// <summary>
		///		Gets a value indicating whether emits default value or not.
		/// </summary>
		/// <value>
		///		<c>true</c> if emits default value; otherwise, <c>false</c>.
		/// </value>
		/// <seealso cref="System.Runtime.Serialization.DataMemberAttribute"/>
		public bool EmitDefaultValue
		{
			get { return this._attribute == null ? true : this._attribute.EmitDefaultValue; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="DataMemberContract"/> struct.
		/// </summary>
		/// <param name="member">The target member.</param>
		/// <param name="attribute">The data contract member attribute. This value can be <c>null</c>.</param>
		public DataMemberContract( MemberInfo member, DataMemberAttribute attribute )
		{
			Contract.Assert( member != null );

			this._member = member;
			this._attribute = attribute;
		}
	}
}
