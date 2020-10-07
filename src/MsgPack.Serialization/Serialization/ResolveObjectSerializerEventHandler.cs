// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines event handler delegate for <see cref="SerializerProvider.ResolveObjectSerializer"/> event.
	/// </summary>
	/// <param name="e">
	///		A reference of <see cref="ResolveObjectSerializerEventArgs"/> which holds inputs and a result of the event handler.
	///	</param>
	public delegate void ResolveObjectSerializerEventHandler(ref ResolveObjectSerializerEventArgs e);
}
