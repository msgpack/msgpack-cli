#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

using System;
using System.Runtime.Serialization;

namespace MsgPack
{
	/// <summary>
	///		Represents unpacking error when message type is not valid because 0xC1 will never be assigned.
	/// </summary>
#if !SILVERLIGHT && !NETFX_CORE
	[Serializable]
#endif
	public sealed class UnassignedMessageTypeException : MessageTypeException
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="UnassignedMessageTypeException"/> class with the default error message.
		/// </summary>
		public UnassignedMessageTypeException() : this( null ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="UnassignedMessageTypeException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error. </param>
		public UnassignedMessageTypeException( string message ) : this( message, null ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="UnassignedMessageTypeException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception. 
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="inner">
		///		The exception that is the cause of the current exception, or a <c>null</c> if no inner exception is specified.
		/// </param>
		public UnassignedMessageTypeException( string message, Exception inner ) : base( message ?? "Invalid message type.", inner ) { }

#if !SILVERLIGHT && !NETFX_CORE
		/// <summary>
		///		Initializes a new instance of the <see cref="UnassignedMessageTypeException"/> class with serialized data.
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
		private UnassignedMessageTypeException( SerializationInfo info, StreamingContext context ) : base( info, context ) { }
#endif
	}
}
