// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
#endif // FEATURE_MPCONTRACT

namespace MsgPack.Serialization
{
#warning TODO: Qualify MessagsePackObject with this attribute.
	/// <summary>
	///		Indicates that the default value of this value type means <c>null</c> even if the declared member type is not <see cref="Nullable{T}"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Struct)]
	public sealed class MessagePackNullableAttribute : Attribute { }
}
