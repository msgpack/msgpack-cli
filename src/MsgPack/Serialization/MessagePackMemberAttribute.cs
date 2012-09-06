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
				Contract.Ensures( Enum.IsDefined( typeof( NilImplication ), Contract.Result<NilImplication>() ) );

				return this._nilImplication;
			}
			set
			{
				switch ( value )
				{
					case Serialization.NilImplication.MemberDefault:
					case Serialization.NilImplication.Null:
					case Serialization.NilImplication.Prohibit:
					{
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException( "value" );
					}
				}

				Contract.EndContractBlock();

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