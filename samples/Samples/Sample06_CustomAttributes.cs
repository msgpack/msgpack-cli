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
using System.IO;
using System.Runtime.Serialization;

using MsgPack.Serialization;

namespace Samples
{
	// You can tweak serialization behavior via custom attributes.
	public class MessagePackMemberSample
	{
		[MessagePackMember(
			0, // Specify 0 based index for serialized array. You should specify this value to ensure interoperability with other platform bindings.
			NilImplication = NilImplication.MemberDefault // Specify the behavior when unpacked value is nil -- default is Null for reference types (set null), Prohibit for value types (throw exception).
		)]
		public long FileId { get; set; }

		[MessagePackMember( 1 )] // NilImplication = Null
		public string Name { get; set; }

		[MessagePackMember( 2 )] // NilImplication = Prohibit
		[MessagePackEnumMember( SerializationMethod = EnumMemberSerializationMethod.ByUnderlyingValue )]// Specify enum serialization method for this member. This value overrides the setting of the context and the setting of the enum type itself.
		public FileAttributes Attributes { get; set; }

		// When there are marked members, non-marked members are excluded from serialization/deserialization candidates.
		public int PropertyWillNotBeSerialized { get; set; }
	}

	// ... Or you can use DataMemberAttribute for compabitility among other libraries.
	public class DataContractSample
	{
		[DataMember( Name = "Id", Order = 1 )] // Order can be used to interop too.
		public long Id { get; set; }

		[DataMember( Name = "Title", Order = 2 )]
		public string Title { get; set; }
	}

	// MessagePackMember
	// MessagePackEnumMember
}

namespace System.Runtime.Serialization
{
	// Tips: You don't have to refer System.Runtime.Serialization assembly. MessagePack for CLI just see Type.FullName.
	public sealed class DataMemberAttribute : Attribute
	{
		public string Name { get; set; }
		public int Order { get; set; }
	}
}
