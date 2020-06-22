// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

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
		/// Private mapping of types &amp; their member skip list, which needs to ignore as part of serialization.
		/// </summary>
		private readonly IDictionary<Type, IEnumerable<string>> _typeIgnoringMembersMap = new Dictionary<Type, IEnumerable<string>>();

		/// <summary>
		/// Sets the member skip list for a specific target type.
		/// </summary>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="memberSkipList">The member skip list.</param>
		public void SetIgnoringMembers(Type targetType, IEnumerable<string> memberSkipList)
		{
			lock (this._typeIgnoringMembersMap)
			{
				if (this._typeIgnoringMembersMap.ContainsKey(targetType))
				{
					this._typeIgnoringMembersMap[targetType] = memberSkipList;
				}
				else
				{
					this._typeIgnoringMembersMap.Add(targetType, memberSkipList);
				}
			}
		}

		/// <summary>
		/// Gets the member skip list for a specific target type.
		/// </summary>
		/// <param name="targetType">Type of the target.</param>
		/// <returns>Returns member skip list for a specific target type.</returns>
		public IEnumerable<string> GetIgnoringMembers(Type targetType)
		{
			lock (this._typeIgnoringMembersMap)
			{
				if (this._typeIgnoringMembersMap.ContainsKey(targetType))
				{
					return this._typeIgnoringMembersMap[targetType];
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
			lock (this._typeIgnoringMembersMap)
			{
				return this._typeIgnoringMembersMap.ToDictionary(item => item.Key, item => (IEnumerable<string>)item.Value.ToArray());
			}
		}
	}
}
