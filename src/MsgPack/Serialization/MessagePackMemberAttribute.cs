// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.


using System;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT

namespace MsgPack.Serialization
{
	/// <summary>
	///		Marks a field or a property to be serialized with MessagePack Serializer and defines some required informations to serialize.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class MessagePackMemberAttribute : Attribute
	{
		/// <summary>
		///		Gets the ID of this member.
		/// </summary>
		/// <value>
		///		The ID of this member. This value will be array index on serialized stream.
		/// </value>
		public int Id { get; }

		/// <summary>
		///		Gets or sets the name of this member.
		/// </summary>
		/// <value>
		///		The name which will be used in map key on serialized stream.
		///		If this value is not specified, the qualified member's name will be used.
		/// </value>
		public string? Name { get; set; }

		private NilImplication _nilImplication;

		/// <summary>
		///		Gets or sets the implication of nil value.
		/// </summary>
		/// <value>
		///		The implication of nil value.
		///		Default value is <see cref="F:NilImplication.MemberDefault"/>.
		/// </value>
		public NilImplication NilImplication
		{
			get
			{
#if DEBUG
				Contract.Ensures( Enum.IsDefined( typeof( NilImplication ), Contract.Result<NilImplication>() ) );
#endif // DEBUG

				return this._nilImplication;
			}
			set
			{
				switch (value)
				{
					case NilImplication.MemberDefault:
					case NilImplication.Null:
					case NilImplication.Prohibit:
					{
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException(nameof(value));
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
		public MessagePackMemberAttribute(int id)
		{
			this.Id = id;
		}
	}
}
