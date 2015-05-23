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

namespace MsgPack.Serialization.CollectionSerializers
{
	/// <summary>
	///		Defines common interface for serializers which can genereate new empty instance.
	/// </summary>
	/// <remarks>
	///		All custom manually implemented or automatically generated serializers which treat collection (that is, they return objects implementing <see cref="System.Collections.IEnumerable"/> except few exceptions like <see cref="string"/>).
	/// </remarks>
	public interface ICollectionInstanceFactory
	{
		/// <summary>
		///		Creates a new collection instance with specified initial capacity.
		/// </summary>
		/// <param name="initialCapacity">
		///		The initial capacy of creating collection.
		///		Note that this parameter may <c>0</c> for non-empty collection.
		/// </param>
		/// <returns>New collection instance. This value will not be <c>null</c>.</returns>
		/// <remarks>
		///		An author of <see cref="Unpacker"/> could implement unpacker for non-MessagePack format,
		///		so implementer of this interface should not rely on that <paramref name="initialCapacity"/> reflects actual items count.
		///		For example, JSON unpacker cannot supply collection items count efficiently.
		/// </remarks>
		object CreateInstance( int initialCapacity );
	}
}