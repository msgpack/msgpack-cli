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
	public sealed class LimitExceededException : DecodeException
	{
		public LimitExceededException(long potision)
			: this(potision, "Some limit is exeeded.") { }

		public LimitExceededException(long potision, string? message)
			: base(potision, message) { }

		public LimitExceededException(long potision, string? message, Exception? innerException)
			: base(potision, message, innerException) { }

#if FEATURE_BINARY_SERIALIZATION

		private LimitExceededException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

#endif // FEATURE_BINARY_SERIALIZATION
	}
}
