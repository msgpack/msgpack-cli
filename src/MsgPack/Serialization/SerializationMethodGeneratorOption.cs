// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif // !UNITY || MSGPACK_UNITY_FULL

namespace MsgPack.Serialization
{
	/// <summary>
	///		Define options of serializer generation.
	/// </summary>
	[Obsolete("This type has not been used.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public enum SerializationMethodGeneratorOption
	{
		/// <summary>
		///		The generated method IL can be dumped to the current directory.
		///		It is intended for the runtime, you cannot use this option.
		/// </summary>
		[EditorBrowsable( EditorBrowsableState.Never )]
		CanDump,

		// TODO: AssemblyLoadContext support for CanCollect work properly.
		/// <summary>
		///		The entire generated method can be collected by GC when it is no longer used.
		/// </summary>
		CanCollect,

		/// <summary>
		///		Prefer performance. This options is default.
		/// </summary>
		Fast
	}
}
