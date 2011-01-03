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
	///		Exception occured when inbound stream is invalid as serialized Message Pack stream.
	/// </summary>
	[Serializable]
	public sealed class InvalidMessagePackStreamException : Exception
	{
		/// <summary>
		///		Initialize new instance with default message.
		/// </summary>
		public InvalidMessagePackStreamException()
			: this( "Stream is not valid as serialized Message Pack object." ) { }

		/// <summary>
		///		Initialize new instance with specified message.
		/// </summary>
		/// <param name="message">
		///		Message to desribe this error.
		/// </param>
		public InvalidMessagePackStreamException( string message ) : base( message ) { }

		/// <summary>
		///		Initialize new instance with specified message and inner exception which caused this exception.
		/// </summary>
		/// <param name="message">
		///		Message to desribe this error.
		/// </param>
		/// <param name="inner">
		///		Exception which caused this exception.
		/// </param>
		public InvalidMessagePackStreamException( string message, Exception inner ) : base( message, inner ) { }

		private InvalidMessagePackStreamException( SerializationInfo info, StreamingContext context )
			: base( info, context ) { }
	}
}
