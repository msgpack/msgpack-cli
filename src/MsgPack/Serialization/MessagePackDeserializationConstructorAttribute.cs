// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Marks this constructor used as deserialization constructor.
	/// </summary>
	/// <remarks>
	///		<para>
	///			This attribute only used once per type.
	///			If there are multiple constructors marked with this attribute in the type, <see cref="System.Runtime.Serialization.SerializationException"/> will be onccured in serializer generation.
	///		</para>
	///		<para>
	///			Marking with this attribute is not required to generate serializer which uses constructor in deserialization, but this attribute is available in following purposes:
	///			<list type="bullet">
	///				<item>
	///					<para>
	///						Indicating force constructor deserialization instead of member deserialization. 
	///						The serializer generation prefer member (that is using property setters and fields) deserialization as possible.
	///						You can indicate to forcibly use constructor even when there are any setters/writable fields.
	///					</para>
	///					<para>
	///						If you do not specify this attribute in this case, member deserialization strategy will be used.
	///					</para>
	///				</item>
	///				<item>
	///					<para>
	///						Clarify the constructor which wil be used in deserialization when there are multiple constructors declared in the type. 
	///						Although the serializer generator avoids default constructor and non-public constructors, it cannot resolve target constructor when there are multiple candidates (that is, public, parameterized constructors).
	///					</para>
	///					<para>
	///						If you do not specify this attribute in this case, serializer generator throws <see cref="System.Runtime.Serialization.SerializationException"/>.
	///					</para>
	///				</item>
	///			</list>
	///		</para>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Constructor)]
	public sealed class MessagePackDeserializationConstructorAttribute : Attribute
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackDeserializationConstructorAttribute"/> class.
		/// </summary>
		public MessagePackDeserializationConstructorAttribute() { }
	}
}
