#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015 FUJIWARA, Yusuke
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
	///		Specifies <see cref="MessagePackKnownTypeAttribute"/> or <see cref="MessagePackRuntimeTypeAttribute"/> target.
	/// </summary>
	internal enum PolymorphismTarget
	{
		/// <summary>
		///		Applies to member type itself.
		///		This option disables <see cref="SerializationContext.DefaultCollectionTypes"/> settings.
		/// </summary>
		Member = 0,

		/// <summary>
		///		Applies to items of collection member type (values for dictionary).
		///		This options causes entire attribute will be ignored for non-collection types.
		/// </summary>
		CollectionItem = 1,

		/// <summary>
		///		Applies to keys of dictionary member type.
		///		This options causes entire attribute will be ignored for non-dictionary types.
		/// </summary>
		DictionaryKey = 2,

		/// <summary>
		///		Applies to keys of dictionary member type.
		///		This options causes entire attribute will be ignored for non-dictionary types.
		/// </summary>
		TupleItem = 3,
	}
}