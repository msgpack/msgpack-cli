// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.ComponentModel;
#if FEATURE_BINARY_SERIALIZATION
using System.Runtime.Serialization;
#endif // FEATURE_BINARY_SERIALIZATION

namespace MsgPack
{
	/// <summary>
	///		Represents unpacking error when message type is unknown or unavailable.
	/// </summary>
#if FEATURE_BINARY_SERIALIZATION
	[Serializable]
#endif // FEATURE_BINARY_SERIALIZATION
	public class MessageTypeException : DecodeException
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="MessageTypeException"/> class with the default error message.
		/// </summary>
		[Obsolete("Use .ctor(long) instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public MessageTypeException() : this(null) { }

		public MessageTypeException(long position) : this(position, null) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessageTypeException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error. </param>
		[Obsolete("Use .ctor(long, string?) instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public MessageTypeException(string? message) : this(message, null) { }

		public MessageTypeException(long position, string? message) : this(position, message, null) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessageTypeException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception. 
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="inner">
		///		The exception that is the cause of the current exception, or a <c>null</c> if no inner exception is specified.
		/// </param>
		[Obsolete("Use .ctor(long, string?, Exception?) instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public MessageTypeException(string? message, Exception? inner) : this(0, message ?? "Invalid message type.", inner) { }

		public MessageTypeException(long position, string? message, Exception? innerException)
			:base(position, message, innerException) { }

#if FEATURE_BINARY_SERIALIZATION

		/// <summary>
		///		Initializes a new instance of the <see cref="MessageTypeException"/> class with serialized data.
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
		protected MessageTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }

#endif // FEATURE_BINARY_SERIALIZATION
	}
}
