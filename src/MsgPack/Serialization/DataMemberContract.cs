// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents member's data contract.
	/// </summary>
	internal readonly struct DataMemberContract
	{
		internal const int UnspecifiedId = -1;

		/// <summary>
		///		Gets the name of the member.
		/// </summary>
		/// <value>
		///		The name of the member.
		/// </value>
		public string Name { get; }

		/// <summary>
		///		Gets the ID of the member.
		/// </summary>
		/// <value>
		///		The ID of the member. Default is <c>-1</c>.
		/// </value>
		public int Id { get; }

		/// <summary>
		///		Gets the nil implication.
		/// </summary>
		/// <value>
		///		The nil implication.
		/// </value>
		public NilImplication NilImplication { get; }

		/// <summary>
		///		Initializes a new instance of the <see cref="DataMemberContract"/> struct.
		/// </summary>
		/// <param name="member">The target member.</param>
		public DataMemberContract(MemberInfo member)
		{
			Contract.Assert(member != null);

			this.Name = member.Name;
			this.NilImplication = NilImplication.MemberDefault;
			this.Id = UnspecifiedId;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="DataMemberContract"/> struct.
		/// </summary>
		/// <param name="member">The target member.</param>
		/// <param name="name">The name of member.</param>
		/// <param name="nilImplication">The implication of the nil value for the member.</param>
		/// <param name="id">The ID of the member. This value cannot be negative and must be unique in the type.</param>
		public DataMemberContract(MemberInfo member, string? name, NilImplication nilImplication, int? id)
		{
			Contract.Assert(member != null);

			if (id < 0)
			{
				throw new SerializationException(String.Format(CultureInfo.CurrentCulture, "The member ID cannot be negative. The member is '{0}' in the '{1}' type.", member.Name, member.DeclaringType));
			}

			this.Name = String.IsNullOrEmpty(name) ? member.Name : name;
			this.NilImplication = nilImplication;
			this.Id = id ?? UnspecifiedId;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="DataMemberContract"/> struct from <see cref="MessagePackMemberAttribute"/>.
		/// </summary>
		/// <param name="member">The target member.</param>
		/// <param name="attribute">The MessagePack member attribute.</param>
		public DataMemberContract(MemberInfo member, MessagePackMemberAttribute attribute)
		{
			Contract.Assert(member != null);
			Contract.Assert(attribute != null);

			if (attribute.Id < 0)
			{
				throw new SerializationException(String.Format(CultureInfo.CurrentCulture, "The member ID cannot be negative. The member is '{0}' in the '{1}' type.", member.Name, member.DeclaringType));
			}

			this.Name = String.IsNullOrEmpty(attribute.Name) ? member.Name : attribute.Name;
			this.NilImplication = attribute.NilImplication;
			this.Id = attribute.Id;
		}
	}
}
