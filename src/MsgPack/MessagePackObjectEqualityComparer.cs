// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;

namespace MsgPack
{
	/// <summary>
	///		Implements <see cref="EqualityComparer{T}"/> for <see cref="MessagePackObject"/>.
	/// </summary>
	[Serializable]
	internal sealed class MessagePackObjectEqualityComparer : IEqualityComparer<MessagePackObject>
	{
		internal static MessagePackObjectEqualityComparer Instance { get; } = new MessagePackObjectEqualityComparer();

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObjectEqualityComparer"/> class.
		/// </summary>
		public MessagePackObjectEqualityComparer() { }

		/// <summary>
		///		Determines whether two objects of type <see cref="MessagePackObject"/> are equal.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		///		<c>true</c> if the specified objects are equal; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(MessagePackObject x, MessagePackObject y) => x.Equals(y);

		/// <summary>
		///		Returns a hash code for the specified <see cref="MessagePackObject"/>.
		/// </summary>
		/// <param name="obj">The <see cref="MessagePackObject"/>.</param>
		/// <returns>
		///		A hash code for <paramref name="obj"/>, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public int GetHashCode(MessagePackObject obj) => obj.GetHashCode();
	}
}
