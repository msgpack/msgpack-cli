// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Marks that this <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, or <see cref="Timestamp"/> typed member has special characteristics on MessagePack serialization.
	/// </summary>
	/// <remarks>
	///		If this attributes is used for incompatible typed members, this attribute will be ignored.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class MessagePackDateTimeMemberAttribute : Attribute
	{
		/// <summary>
		///		Gets or sets the default serialization method for this enum typed member.
		/// </summary>
		/// <value>
		///		The default serialization method for this enum typed member.
		///		Note that the method for the enum type will be overrided with this.
		/// </value>
		public DateTimeMemberConversionMethod DateTimeConversionMethod { get; set; }

		/// <summary>
		///		Gets or sets the precision of fraction portion on ISO 8601 date time string.
		/// </summary>
		/// <value>
		///		The precision of fraction portion on ISO 8601 date time string.
		///		<c>null</c> means that the value will be determined by serialization option or serializer implementation.
		/// </value>
		/// <remarks>
		///		<include file='../../Common/Remarks.xml' path='docs/doc[@name="DateTimeConversion"]'/>
		/// </remarks>
		public int? Iso8601SubsecondsPrecision { get; set; }

		private char? _iso8601DecimalMark;

		/// <summary>
		///		Gets or sets the separator char for fraction portion.
		/// </summary>
		/// <value>
		///		The separator char for fraction portion.
		///		<c>null</c> means that the value will be determined by serialization option or serializer implementation.
		/// </value>
		public char? Iso8601DecimalMark
		{
			get => this._iso8601DecimalMark;
			set => this._iso8601DecimalMark = value switch
			{
				',' => ',',
				'.' => '.',
				_ => Throw.InvalidIso8601DecimalMark(value)
			};
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackDateTimeMemberAttribute"/> class.
		/// </summary>
		public MessagePackDateTimeMemberAttribute() { }
	}
}
