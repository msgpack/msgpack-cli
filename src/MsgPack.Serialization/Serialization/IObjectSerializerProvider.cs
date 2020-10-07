// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.ComponentModel;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines an interface for an object which provides <see cref="ObjectSerializer"/> for the specified parameter.
	/// </summary>
	public interface IObjectSerializerProvider
	{
		/// <summary>
		///		Gets 
		/// </summary>
		/// <param name="targetType"></param>
		/// <param name="providerParameter"></param>
		/// <returns></returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		ObjectSerializer GetSerializer(Type targetType, object? providerParameter);
	}
}
