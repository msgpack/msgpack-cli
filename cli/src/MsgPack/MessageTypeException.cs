#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
	///		Represents unpacking error when message type is unknown or unavailable.
	/// </summary>
#if !SILVERLIGHT
	[Serializable]
#endif
	public sealed class MessageTypeException : Exception
	{
		/// <summary>
		///		Initialize new instance with default message.
		/// </summary>
		public MessageTypeException() : this( null ) { }

		/// <summary>
		///		Initialize new instance with specified message.
		/// </summary>
		/// <param name="message">Develiper friendly message to describe detail information.</param>
		public MessageTypeException( string message ) : this( message, null ) { }

		/// <summary>
		///		Initialize new instance with specified message and underlying error.
		/// </summary>
		/// <param name="message">Develiper friendly message to describe detail information.</param>
		/// <param name="inner">Underlying error.</param>
		public MessageTypeException( string message, Exception inner ) : base( message ?? "Invalid message type.", inner ) { }

#if !SILVERLIGHT
		private MessageTypeException( SerializationInfo info, StreamingContext context ) : base( info, context ) { }
#endif
	}
}
