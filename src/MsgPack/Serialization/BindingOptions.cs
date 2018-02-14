#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke and contributors
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
// Contributors:
//    Shrenik Jhaveri (ShrenikOne)
//
#endregion -- License Terms --


using System;
using System.Collections.Generic;
using System.Linq;

namespace MsgPack.Serialization
{
	/// <summary>
	/// The Binding Options provide serializer clue on including property/field as part of packing.
	/// </summary>
	public class BindingOptions
	{
		/// <summary>
		/// Private mapping of types & their member skip list, which needs to ignore as part of serialization.
		/// </summary>
		private readonly IDictionary<Type, IEnumerable<string>> typeIgnoringMembersMap = new Dictionary<Type, IEnumerable<string>>();

		/// <summary>
		/// Sets the member skip list for a specific target type.
		/// </summary>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="memberSkipList">The member skip list.</param>
		public void SetIgnoringMembers( Type targetType, IEnumerable<string> memberSkipList )
		{
			lock ( this.typeIgnoringMembersMap )
			{
				if ( this.typeIgnoringMembersMap.ContainsKey( targetType ) )
				{
					this.typeIgnoringMembersMap[ targetType ] = memberSkipList;
				}
				else
				{
					this.typeIgnoringMembersMap.Add( targetType, memberSkipList );
				}
			}
		}

		/// <summary>
		/// Gets the member skip list for a specific target type.
		/// </summary>
		/// <param name="targetType">Type of the target.</param>
		/// <returns>Returns member skip list for a specific target type.</returns>
		public IEnumerable<string> GetIgnoringMembers( Type targetType )
		{
			lock ( this.typeIgnoringMembersMap )
			{
				if ( this.typeIgnoringMembersMap.ContainsKey( targetType ) )
				{
					return this.typeIgnoringMembersMap[ targetType ];
				}
				else
				{
					return Enumerable.Empty<string>();
				}
			}
		}

		/// <summary>
		/// Gets all registered types specific ignoring members.
		/// </summary>
		/// <returns>Returns all registered types specific ignoring members.</returns>
		public IDictionary<Type, IEnumerable<string>> GetAllIgnoringMembers()
		{
			lock ( this.typeIgnoringMembersMap )
			{
				return this.typeIgnoringMembersMap.ToDictionary( item => item.Key, item => ( IEnumerable<string> )item.Value.ToArray() );
			}
		}
	}
}
