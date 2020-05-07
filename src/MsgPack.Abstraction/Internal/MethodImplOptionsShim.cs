// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Runtime.CompilerServices;

namespace MsgPack.Internal
{
	/// <summary>
	///		Defines compatibility shims for legacy runtime.
	/// </summary>
	internal static class MethodImplOptionsShim
	{
		/// <summary>
		///		Tells JIT to inline aggressively.
		/// </summary>
		/// <remarks>
		///		This value is not defined in .NET Framework 3.5, but the runtime will ignore this flag value.
		/// </remarks>
		public const MethodImplOptions AggressiveInlining = (MethodImplOptions)256;
	}
}
