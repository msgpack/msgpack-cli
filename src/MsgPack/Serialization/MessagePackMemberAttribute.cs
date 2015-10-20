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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Marks a field or a property to be serialized with MessagePack Serializer and defines some required informations to serialize.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
	public sealed class MessagePackMemberAttribute : Attribute
	{
		private readonly int _id;

		/// <summary>
		///		Gets the ID of the member.
		/// </summary>
		/// <value>
		///		The ID of the member.
		/// </value>
		public int Id
		{
			get { return this._id; }
		}

		private string _name;

		/// <summary>
		///		Gets or sets the name of this member.
		/// </summary>
		/// <value>
		///		The name which will be used in map key on serialized MessagePack stream.
		/// </value>
		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		private NilImplication _nilImplication;

		/// <summary>
		///		Gets or sets the implication of the nil value.
		/// </summary>
		/// <value>
		///		The implication of the nil value.
		///		Default value is <see cref="F:NilImplication.MemberDefault"/>.
		/// </value>
		public NilImplication NilImplication
		{
			get
			{
#if !UNITY
				Contract.Ensures( Enum.IsDefined( typeof( NilImplication ), Contract.Result<NilImplication>() ) );
#endif // !UNITY

				return this._nilImplication;
			}
			set
			{
				switch ( value )
				{
					case NilImplication.MemberDefault:
					case NilImplication.Null:
					case NilImplication.Prohibit:
					{
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException( "value" );
					}
				}

#if !UNITY
				Contract.EndContractBlock();
#endif // !UNITY


				this._nilImplication = value;
			}
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackMemberAttribute"/> class.
		/// </summary>
		/// <param name="id">
		///		The ID of the member. This value cannot be negative and must be unique in the type.
		///	</param>
		public MessagePackMemberAttribute( int id )
		{
			this._id = id;
		}
	}
}