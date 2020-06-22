// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents serializer capabilities.
	/// </summary>
	[Flags]
	public enum SerializerCapabilities
	{
		/// <summary>
		///		None.
		/// </summary>
		None = 0,

		/// <summary>
		///		Caller can call <c>Pack</c> and <c>PackTo</c> method safely.
		///		If this flag is not set, the serializer may be deserialize only serializer.
		///		(v2) This value is synonym of <see cref="Serialize"/>.
		/// </summary>
		PackTo = 0x1,

		/// <summary>
		///		Caller can call <c>Serialize</c> method safely.
		///		If this flag is not set, the serializer may be deserialize only serializer.
		/// </summary>
		Serialize = 0x1,

		/// <summary>
		///		Caller can call <c>Unpack</c> and <c>UnpackFrom</c> method safely.
		///		If this flag is not set, the serializer may be serialize only serializer.
		///		(v2) This value is synonym of <see cref="Deserialize"/>.
		/// </summary>
		UnpackFrom = 0x10,

		/// <summary>
		///		Caller can call <c>Deserialize</c> method safely.
		///		If this flag is not set, the serializer may be serialize only serializer.
		/// </summary>
		Deserialize = 0x10,

		/// <summary>
		///		Caller can call <c>UnpackTo</c> method safely.
		///		If this flag is not set, the serializer should not be for mutable collection type.
		///		(v2) This value is synonym of <see cref="DeserializeTo"/>.
		/// </summary>
		UnpackTo = 0x20,

		/// <summary>
		///		Caller can call <c>DeserializeTo</c> method safely.
		///		If this flag is not set, the serializer should not be for mutable collection type.
		/// </summary>
		DeserializeTo = 0x20
	}
}
