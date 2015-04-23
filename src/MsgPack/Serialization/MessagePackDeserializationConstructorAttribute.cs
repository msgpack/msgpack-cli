#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015-2015 FUJIWARA, Yusuke
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