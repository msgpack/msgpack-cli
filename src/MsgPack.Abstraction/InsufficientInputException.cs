// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
#if FEATURE_BINARY_SERIALIZATION
using System.Runtime.Serialization;
#endif // FEATURE_BINARY_SERIALIZATION

namespace MsgPack
{
	/// <summary>
	///		An exception thrown if there are no more inputs when be requested.
	/// </summary>
#if FEATURE_BINARY_SERIALIZATION
	[Serializable]
#endif // FEATURE_BINARY_SERIALIZATION
	public sealed class InsufficientInputException : Exception
	{
		public InsufficientInputException()
			: this("There are no more inputs.") { }

		public InsufficientInputException(string? message)
			: base(message) { }

		public InsufficientInputException(string? message, Exception? innerException)
			: base(message, innerException) { }
#if FEATURE_BINARY_SERIALIZATION

		private InsufficientInputException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

#endif // FEATURE_BINARY_SERIALIZATION
	}
}
