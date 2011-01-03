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
	///		Represents generic unpacking error.
	/// </summary>
	[Serializable]
	public sealed class UnpackException : Exception
	{
		/// <summary>
		///		Initialize new instance with default message.
		/// </summary>
		public UnpackException() : this( "Failed to unpacking." ) { }

		/// <summary>
		///		Initialize new instance with specified message.
		/// </summary>
		/// <param name="message">Develiper friendly message to describe detail information.</param>
		public UnpackException( string message ) : base( message ) { }

		/// <summary>
		///		Initialize new instance with specified message and underlying error.
		/// </summary>
		/// <param name="message">Develiper friendly message to describe detail information.</param>
		/// <param name="inner">Underlying error.</param>
		public UnpackException( string message, Exception inner ) : base( message, inner ) { }

		private UnpackException( SerializationInfo info, StreamingContext context ) : base( info, context ) { }
	}
}
