// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.IO;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	public partial class AsyncDeserializationOperationContext
	{
		/// <summary>
		///		Returns new <see cref="ReadOnlyStreamSequence"/> which wraps specified <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> which holds input data, it may have over 32bit length.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"><paramref name="stream"/> is <c>null</c>.</exception>
		public ReadOnlyStreamSequence CreateSequence(Stream stream)
			=> new ReadOnlyStreamSequence(Ensure.NotNull(stream), this.Options.ByteBufferPool, 64 * 1024, this.Options.ClearsBuffer);
	}
}
