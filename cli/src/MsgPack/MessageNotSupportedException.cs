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
	///		Exception occurs when serialized stream contains structures or features which will never be supported by MsgPack/CLI implementation.
	/// </summary>
#if !SILVERLIGHT
	[Serializable]
#endif
	public sealed class MessageNotSupportedException : Exception
	{
		/// <summary>
		///		Initialize new instance with default message.
		/// </summary>
		public MessageNotSupportedException() : this( null ) { }

		/// <summary>
		///		Initialize new instance with specified message.
		/// </summary>
		/// <param name="message">
		///		Message to desribe this error.
		/// </param>
		public MessageNotSupportedException( string message ) : this( message, null ) { }

		/// <summary>
		///		Initialize new instance with specified message and inner exception which caused this exception.
		/// </summary>
		/// <param name="message">
		///		Message to desribe this error.
		/// </param>
		/// <param name="inner">
		///		Exception which caused this exception.
		/// </param>
		public MessageNotSupportedException( string message, Exception inner ) : base( message ?? "Specified object is not supported.", inner ) { }

#if !SILVERLIGHT
		private MessageNotSupportedException( SerializationInfo info, StreamingContext context )
			: base( info, context ) { }
#endif
	}
}
