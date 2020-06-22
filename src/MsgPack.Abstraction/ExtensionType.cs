// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.ComponentModel;

namespace MsgPack
{
	/// <summary>
	///		Represents a type of extension.
	/// </summary>
	public readonly struct ExtensionType : IComparable<ExtensionType>, IEquatable<ExtensionType>
	{
		/// <summary>
		///		Gets raw tag data.
		/// </summary>
		/// <value>Raw tag data. Note that the codec might not allow full <see cref="UInt64"/> length.</value>
		/// <remarks>
		///		Valid range and sementics depend on serialization codec.
		/// </remarks>
		public ulong Tag { get; }

		/// <summary>
		///		Initializes new instance.
		///		This constructor should not be called from application directly.
		/// </summary>
		/// <param name="tag">Raw tag data.</param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ExtensionType(ulong tag)
		{
			this.Tag = tag;
		}

		/// <inheritdoc />
		public bool Equals(ExtensionType other)
			=> this.Tag == other.Tag;

		/// <inheritdoc />
		public int CompareTo(ExtensionType other)
			=> this.Tag.CompareTo(other.Tag);

		/// <inheritdoc />
		public override bool Equals(object? obj)
			=> (obj is ExtensionType other) ? this.Equals(other) : false;

		/// <inheritdoc />
		public override int GetHashCode()
			=> this.Tag.GetHashCode();

		/// <summary>
		///		Determines whether two specified instances of <see cref="ExtensionType"/> are equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="left"/> and <paramref name="right"/> represent the same type; otherwise, <c>false</c>.
		///	</returns>
		public static bool operator ==(ExtensionType left, ExtensionType right)
			=> left.Tag == right.Tag;

		/// <summary>
		///		Determines whether two specified instances of <see cref="ExtensionType"/> are not equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="left"/> and <paramref name="right"/> represent the different type; otherwise, <c>false</c>.
		///	</returns>
		public static bool operator !=(ExtensionType left, ExtensionType right)
			=> left.Tag != right.Tag;

		/// <summary>
		///		Determines whether one specified <see cref="ExtensionType"/> object is less than a second specified <see cref="ExtensionType"/> object.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="left"/> is less than <paramref name="right"/> in their tag; otherwise, <c>false</c>.
		///	</returns>
		public static bool operator <(ExtensionType left, ExtensionType right)
			=> left.Tag < right.Tag;

		/// <summary>
		///		Determines whether one specified <see cref="ExtensionType"/> object is greater than a second specified <see cref="ExtensionType"/> object.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/> in their tag; otherwise, <c>false</c>.
		///	</returns>
		public static bool operator >(ExtensionType left, ExtensionType right)
			=> left.Tag > right.Tag;

		/// <summary>
		///		Determines whether one specified <see cref="ExtensionType"/> object is less than or equal to a second specified <see cref="ExtensionType"/> object.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/> in their tag; otherwise, <c>false</c>.
		///	</returns>
		public static bool operator <=(ExtensionType left, ExtensionType right)
			=> left.Tag <= right.Tag;

		/// <summary>
		///		Determines whether one specified <see cref="ExtensionType"/> object is greater than or equal to a second specified <see cref="ExtensionType"/> object.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/> in their tag; otherwise, <c>false</c>.
		///	</returns>
		public static bool operator >=(ExtensionType left, ExtensionType right)
			=> left.Tag >= right.Tag;
	}
}
