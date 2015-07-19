#region -- License Terms --
//  MessagePack for CLI
// 
//  Copyright (C) 2015-2015 FUJIWARA, Yusuke
// 
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
// 
//         http://www.apache.org/licenses/LICENSE-2.0
// 
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
#endregion -- License Terms --

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents children items type of <see cref="PolymorphismSchema"/>
	/// </summary>
	internal enum PolymorphismSchemaChildrenType
	{
		/// <summary>
		///		Leaf, that is no children schema.
		/// </summary>
		None = 0,

		/// <summary>
		///		Collection items, so children count is 1.
		/// </summary>
		CollectionItems,

		/// <summary>
		///		Dictionary keys and values, so children count is 2, index 0 is for keys, 1 is for values.
		/// </summary>
		DictionaryKeyValues,

#if !NETFX_35 && !UNITY
		/// <summary>
		///		Tuple items, so chidren count is equal to tuple's arity.
		/// </summary>
		TupleItems
#endif // !NETFX_35 && !UNITY
	}
}