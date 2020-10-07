// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;

namespace MsgPack
{
	// Thanks to Mono implementation.

	/// <summary>
	///		Debugger type proxy for <see cref="ICollection{T}"/>.
	/// </summary>
	/// <typeparam name="T">The element type of the collection.</typeparam>
	internal sealed class CollectionDebuggerProxy<T>
	{
		private readonly ICollection<T> _collection;

		public CollectionDebuggerProxy(ICollection<T> target)
		{
			this._collection = target;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[]? Items
		{
			get
			{
				if (this._collection == null)
				{
					return null;
				}

				var result = new T[this._collection.Count];
				this._collection.CopyTo(result, 0);
				return result;
			}
		}
	}
}
