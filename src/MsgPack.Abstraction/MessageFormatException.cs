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
	public sealed class MessageFormatException : DecodeException
	{
		public MessageFormatException(long position)
			: this(position, "Input has invalid byte sequence.") { }

		public MessageFormatException(long position, string? message)
			: base(position, message) { }

		public MessageFormatException(long position, string? message, Exception? innerException)
			: base(position, message, innerException) { }

#if FEATURE_BINARY_SERIALIZATION

		private MessageFormatException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

#endif // FEATURE_BINARY_SERIALIZATION
	}
}
