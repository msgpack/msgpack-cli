#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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

using System;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents member's data contract.
	/// </summary>
#if !UNITY
	internal struct DataMemberContract
#else
	internal sealed class DataMemberContract
#endif // !UNITY
	{
#if UNITY
		/// <summary>
		///		Null object.
		/// </summary>
		public static readonly DataMemberContract Null = new DataMemberContract();
#endif

		internal const int UnspecifiedId = -1;

		private readonly string _name;

		/// <summary>
		///		Gets the name of the member.
		/// </summary>
		/// <value>
		///		The name of the member.
		/// </value>
		public string Name
		{
			get
			{
#if !UNITY
				Contract.Ensures( !String.IsNullOrEmpty( Contract.Result<string>() ) );
#endif // !UNITY

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
#if !UNITY
				Contract.Ensures( Contract.Result<int>() >= -1 );
#endif // !UNITY

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

#if UNITY
		public DataMemberContract()
		{
			this._name = null;
			this._nilImplication = NilImplication.MemberDefault;
			this._id = UnspecifiedId;
		}
#endif // UNITY

		/// <summary>
		///		Initializes a new instance of the <see cref="DataMemberContract"/> struct.
		/// </summary>
		/// <param name="member">The target member.</param>
		public DataMemberContract( MemberInfo member )
		{
#if !UNITY
			Contract.Requires( member != null );
#endif // !UNITY

			this._name = member.Name;
			this._nilImplication = NilImplication.MemberDefault;
			this._id = UnspecifiedId;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="DataMemberContract"/> struct.
		/// </summary>
		/// <param name="member">The target member.</param>
		/// <param name="name">The name of member.</param>
		/// <param name="nilImplication">The implication of the nil value for the member.</param>
		/// <param name="id">The ID of the member. This value cannot be negative and must be unique in the type.</param>
		public DataMemberContract( MemberInfo member, string name, NilImplication nilImplication, int? id )
		{
#if !UNITY
			Contract.Requires( member != null );
#endif // !UNITY

			if ( id < 0 )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The member ID cannot be negative. The member is '{0}' in the '{1}' type.", member.Name, member.DeclaringType ) );
			}

			this._name = String.IsNullOrEmpty( name ) ? member.Name : name;
			this._nilImplication = nilImplication;
			this._id = id ?? UnspecifiedId;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="DataMemberContract"/> struct from <see cref="MessagePackMemberAttribute"/>.
		/// </summary>
		/// <param name="member">The target member.</param>
		/// <param name="attribute">The MessagePack member attribute.</param>
		public DataMemberContract( MemberInfo member, MessagePackMemberAttribute attribute )
		{
#if !UNITY
			Contract.Requires( member != null );
			Contract.Requires( attribute != null );
#endif // !UNITY

			if ( attribute.Id < 0 )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The member ID cannot be negative. The member is '{0}' in the '{1}' type.", member.Name, member.DeclaringType ) );
			}

			this._name = String.IsNullOrEmpty( attribute.Name ) ? member.Name : attribute.Name;
			this._nilImplication = attribute.NilImplication;
			this._id = attribute.Id;
		}
	}
}
