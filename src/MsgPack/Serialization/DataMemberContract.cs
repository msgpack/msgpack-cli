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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents member's data contract.
	/// </summary>
	internal struct DataMemberContract
	{
		internal const int UnspecifiedId = -1;

		private readonly string _name;

		/// <summary>
		///		Gets the name of the member.
		/// </summary>
		/// <value>
		///		The name of the member.
		/// </value>
		/// <seealso cref="System.Runtime.Serialization.DataMemberAttribute"/>
		public string Name
		{
			get
			{
				Contract.Ensures( !String.IsNullOrEmpty( Contract.Result<string>() ) );

				return this._name;
			}
		}

		private readonly int _id;

		/// <summary>
		///		Gets the ID of the member.
		/// </summary>
		/// <value>
		///		The ID of the member. Default is <c>-1</c>.
		/// </value>
		public int Id
		{
			get
			{
				Contract.Ensures( Contract.Result<int>() >= -1 );

				return this._id;
			}
		}

		private readonly NilImplication _nilImplication;

		/// <summary>
		///		Gets the nil implication.
		/// </summary>
		/// <value>
		///		The nil implication.
		/// </value>
		public NilImplication NilImplication
		{
			get { return this._nilImplication; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="DataMemberContract"/> struct.
		/// </summary>
		/// <param name="member">The target member.</param>
		public DataMemberContract( MemberInfo member )
		{
			Contract.Requires( member != null );

			this._name = member.Name;
			this._nilImplication = Serialization.NilImplication.MemberDefault;
			this._id = UnspecifiedId;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="DataMemberContract"/> struct from <see cref="DataMemberAttribute"/>.
		/// </summary>
		/// <param name="member">The target member.</param>
		/// <param name="attribute">The data contract member attribute. This value can be <c>null</c>.</param>
		public DataMemberContract( MemberInfo member, DataMemberAttribute attribute )
		{
			Contract.Requires( member != null );
			Contract.Requires( attribute != null );

			this._name = String.IsNullOrEmpty( attribute.Name ) ? member.Name : attribute.Name;
			this._nilImplication = Serialization.NilImplication.MemberDefault;
			this._id = attribute.Order;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="DataMemberContract"/> struct from <see cref="MessagePackMemberAttribute"/>.
		/// </summary>
		/// <param name="member">The target member.</param>
		/// <param name="attribute">The MessagePack member attribute. This value can be <c>null</c>.</param>
		public DataMemberContract( MemberInfo member, MessagePackMemberAttribute attribute )
		{
			Contract.Requires( member != null );
			Contract.Requires( attribute != null );

			if ( attribute.Id < 0 )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The member ID cannot be negative. The member is '{0}' in the '{1}' type.", member.Name, member.DeclaringType ) );
			}

			this._name = member.Name;
			this._nilImplication = attribute.NilImplication;
			this._id = attribute.Id;
		}
	}
}
