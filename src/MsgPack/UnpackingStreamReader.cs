#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.IO;
using System.Text;

namespace MsgPack
{
	/// <summary>
	///		Implements <see cref="TextReader"/> which reads raw binary <see cref="Stream"/> with specific <see cref="Encoding"/>.
	/// </summary>
	public abstract class UnpackingStreamReader : StreamReader
	{
		private readonly long _byteLength;

		/// <summary>
		///		Gets the length of the underlying raw binary length.
		/// </summary>
		/// <value>
		///		The length of the underlying raw binary length.
		///		This value will not be negative.
		/// </value>
		public long ByteLength
		{
			get { return this._byteLength; }
		}

		internal UnpackingStreamReader( Stream stream, Encoding encoding, long byteLength )
			: base( stream, encoding, true )
		{
			this._byteLength = byteLength;
		}
	}
}
