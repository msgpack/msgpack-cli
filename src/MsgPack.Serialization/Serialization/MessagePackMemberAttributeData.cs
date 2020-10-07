// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	// This is a custom struct instead of raw MessagePackMemberAttribute to reduce GC.
	/// <summary>
	///		Represents a data of <see cref="MessagePackMemberAttribute"/>.
	/// </summary>
	public readonly struct MessagePackMemberAttributeData
	{
		/// <summary>
		///		Gets ID of the member.
		/// </summary>
		/// <value>
		///		ID of this member. This value will be array index on serialized stream. This value cannot be negative and must be unique in the type.
		///		This value may be <c>null</c>, in that case, the qualified member's will be ignored.
		/// </value>
		public int? Id { get; }

		/// <summary>
		///		Gets name of this member.
		/// </summary>
		/// <value>
		///		Name which will be used in map key on serialized stream.
		///		This value may be <c>null</c>, in that case, the qualified member's name will be used.
		///		This value must be unique in the type.
		/// </value>
		public string? Name { get; }

		/// <summary>
		///		Gets implication of nil value.
		/// </summary>
		/// <value>
		///		Implication of nil value.
		///		Default value is <see cref="F:NilImplication.MemberDefault"/>.
		/// </value>
		public NilImplication NilImplication { get; }

		/// <summary>
		///		Initializes a new instance of <see cref="MessagePackMemberAttributeData"/> object.
		/// </summary>
		/// <param name="id">ID of this member. This value will be array index on serialized stream. This value cannot be negative and must be unique in the type. This value may be <c>null</c>, in that case, the qualified member's will be ignored.</param>
		/// <param name="name">Name which will be used in map key on serialized stream. This value may be <c>null</c>, in that case, the qualified member's name will be used. This value must be unique in the type.</param>
		/// <param name="nilImplication">Implication of nil value.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="id"/> is negative, or the value of <paramref name="nilImplication" /> is not defined as <see cref="NilImplication"/> enumeration.
		/// </exception>
		public MessagePackMemberAttributeData(int? id, string? name, NilImplication nilImplication = NilImplication.MemberDefault)
		{
			this.Id = id is null ? default(int?) : Ensure.IsNotLessThan(id.GetValueOrDefault(), 0, nameof(id));
			this.Name = name;
			switch (nilImplication)
			{
				case NilImplication.MemberDefault:
				case NilImplication.Null:
				case NilImplication.Prohibit:
				{
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(nilImplication));
				}
			}

			this.NilImplication = nilImplication;
		}
	}
}
