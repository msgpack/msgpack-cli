#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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

namespace MsgPack
{
	/// <summary>
	///		Defines compatibility options for <see cref="Packer"/>.
	/// </summary>
	[Flags]
	public enum PackerCompatibilityOptions
	{
		/// <summary>
		///		No compatibility options. <see cref="Packer"/>s use newest behavior.
		/// </summary>
		None = 0,

		/// <summary>
		///		Packs byte array as raw(str) value, and also prohibits usage of str8 type for legacy unpacker implementations.
		/// </summary>
		PackBinaryAsRaw = 0x1,

		/// <summary>
		///		Prohibits usage of any ext types for legacy unpacker implementations.
		/// </summary>
		ProhibitExtendedTypeObjects = 0x2,

		/// <summary>
		///		<see cref="Packer"/>s should be use classic behavior. That is, do not use str8 and any ext types, and byte arrays must be packed as raw.
		/// </summary>
		Classic = PackBinaryAsRaw | ProhibitExtendedTypeObjects
	}
}
