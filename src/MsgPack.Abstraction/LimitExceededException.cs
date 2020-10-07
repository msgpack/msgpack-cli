// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
#if FEATURE_BINARY_SERIALIZATION
using System.Runtime.Serialization;
#endif // FEATURE_BINARY_SERIALIZATION

namespace MsgPack
{
#if FEATURE_BINARY_SERIALIZATION
	[Serializable]
#endif // FEATURE_BINARY_SERIALIZATION
	public sealed class LimitExceededException : Exception
	{
		public LimitExceededException()
			: this("Some limit is exeeded.") { }

		public LimitExceededException(string? message)
			: base(message) { }

		public LimitExceededException(string? message, Exception? innerException)
			: base(message, innerException) { }
#if FEATURE_BINARY_SERIALIZATION

		private LimitExceededException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

#endif // FEATURE_BINARY_SERIALIZATION
	}
}
