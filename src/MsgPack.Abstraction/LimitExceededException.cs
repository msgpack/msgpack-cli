// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack
{
	public sealed class LimitExceededException : Exception
	{
		public LimitExceededException()
			: this("Some limit is exeeded.") { }

		public LimitExceededException(string? message)
			: base(message) { }

		public LimitExceededException(string? message, Exception? innerException)
			: base(message, innerException) { }
	}
}
