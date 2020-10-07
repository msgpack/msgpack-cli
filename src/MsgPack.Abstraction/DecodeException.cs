// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
#if FEATURE_BINARY_SERIALIZATION
using System.Runtime.Serialization;
#endif // FEATURE_BINARY_SERIALIZATION

namespace MsgPack
{
#warning TODO: -> ParseException
	/// <summary>
	///		Defines common exception for decoding error which is caused by invalid input byte sequence.
	/// </summary>
#if FEATURE_BINARY_SERIALIZATION
	[Serializable]
#endif // FEATURE_BINARY_SERIALIZATION
	public abstract class DecodeException : Exception
	{
		/// <summary>
		///		Gets the position of the input sequence.
		/// </summary>
		/// <value>
		///		The position of the input sequence.
		/// </value>
		/// <remarks>
		///		This value MAY NOT represents actual position of source byte sequence espetially in asynchronous deserialization 
		///		because the position stored in this property may reflect the offset in the buffer which is gotten from underlying byte stream rather than the stream position.
		///		So, the serializer, which is consumer of the decoder,  must provide appropriate exception message with calculated position information with this property.
		///		
		///		<note>Because of above limitation, this property's value will not included in <see cref="P:Message"/> property nand <see cref="M:ToString()"/> result.</note>
		/// </remarks>
		public long Position { get; }

		protected DecodeException(long position)
			: base()
		{
			this.Position = position;
		}

		protected DecodeException(long position, string? message)
			: base(message)
		{
			this.Position = position;
		}

		protected DecodeException(long position, string? message, Exception? innerException)
			: base(message, innerException)
		{
			this.Position = position;
		}

#if FEATURE_BINARY_SERIALIZATION
		protected DecodeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.Position = info.GetInt64("Position");
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Position", this.Position);
		}

#endif // FEATURE_BINARY_SERIALIZATION
	}
}
