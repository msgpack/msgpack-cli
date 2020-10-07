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
	///		Exception occurs when serialized stream contains structures or features which will never be supported by MsgPack/CLI implementation.
	/// </summary>
#if FEATURE_BINARY_SERIALIZATION
	[Serializable]
#endif // FEATURE_BINARY_SERIALIZATION
	public sealed class MessageNotSupportedException : Exception
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="MessageNotSupportedException"/> class with the default error message.
		/// </summary>
		public MessageNotSupportedException() : this(null) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessageNotSupportedException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error. </param>
		public MessageNotSupportedException(string? message) : this(message, null) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessageNotSupportedException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception. 
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="inner">
		///		The exception that is the cause of the current exception, or a <c>null</c> if no inner exception is specified.
		/// </param>
		public MessageNotSupportedException(string? message, Exception? inner) : base(message ?? "Specified object is not supported.", inner) { }

#if FEATURE_BINARY_SERIALIZATION
		/// <summary>
		///		Initializes a new instance of the <see cref="MessageNotSupportedException"/> class with serialized data.
		/// </summary>
		/// <param name="info">
		///		The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
		///	</param>
		/// <param name="context">
		///		The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		/// <exception cref="ArgumentNullException">
		///		The <paramref name="info"/> parameter is <c>null</c>.
		/// </exception>
		/// <exception cref="SerializationException">
		///		The class name is <c>null</c> or <see cref="P:HResult"/> is zero (0).
		///	</exception>
		private MessageNotSupportedException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

#endif // FEATURE_BINARY_SERIALIZATION
	}
}
