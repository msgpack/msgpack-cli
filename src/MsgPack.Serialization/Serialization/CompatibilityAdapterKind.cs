// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents a type of the compatibility adapter.
	/// </summary>
	internal enum CompatibilityAdapterKind
	{
		/// <summary>
		///		Synchronous serialization.
		/// </summary>
		Serialization,

		/// <summary>
		///		Synchronous deserialization.
		/// </summary>
		Deserialization,

		/// <summary>
		///		Asynchronous serialization.
		/// </summary>
		AsyncSerialization,

		/// <summary>
		///		Asynchronous deserialization.
		/// </summary>
		AsyncDeserialization
	}

}
