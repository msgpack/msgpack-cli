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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Specifies nil implication in serialization/deserialization.
	/// </summary>
	public enum NilImplication
	{
		/// <summary>
		///		A nil is interpreted as default value of the member.
		/// </summary>
		/// <remarks>
		///		<para>
		///			This value affects only deserialization.
		///		</para>
		///		<para>
		///			If the unpacking value is nil, the serializer will not set any value to the member.
		///		</para>
		///		<para>
		///			This value corresponds to <c>optional</c> on the IDL.
		///		</para>
		///		<note>
		///			This is default option because the most safe option.
		///		</note>
		/// </remarks>
		MemberDefault = 0,

		/// <summary>
		///		A nil is interpreted as <c>null</c>.
		/// </summary>
		/// <remarks>
		///		<para>
		///			This value affects only deserialization.
		///		</para>
		///		<para>
		///			If the unpacking value is nil, the serializer will set <c>null</c> to the member.
		///			If the member is non-nullable value type and the packed value is nil, then <see cref="System.Runtime.Serialization.SerializationException"/> will be thrown.
		///		</para>
		///		<para>
		///			This value corresponds to <c>nullable required</c> on the IDL.
		///		</para>
		///		<note>
		///			If the destination end point sends nil for the value type member like <see cref="Int32"/> type,
		///			you can avoid the exception with change the type of the member to nullable value type.
		///		</note>
		/// </remarks>
		Null,

		/// <summary>
		///		A nil is prohibitted.
		/// </summary>
		/// <remarks>
		///		<para>
		///			This value affects both of serialization and deserialization.
		///		</para>
		///		<para>
		///			If the packing value is <c>null</c> or the unpacking value is nil,
		///			the serializer will throw exception.
		///		</para>
		///		<para>
		///			This value corresponds to <c>required</c> on the IDL.
		///		</para>
		///		<note>
		///			When you specify this value to newly added member,
		///			it means that you BREAK backword compatibility.
		///		</note>
		/// </remarks>
		Prohibit
	}
}
