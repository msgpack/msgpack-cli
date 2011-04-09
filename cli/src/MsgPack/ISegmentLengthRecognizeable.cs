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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MsgPack
{
	/// <summary>
	///		Common interface for objects which can recognize segment length and optimize its internal behavior.
	/// </summary>
	public interface ISegmentLengthRecognizeable
	{
		/// <summary>
		///		Notify estimated length of current segment to current iterator. 
		/// </summary>
		/// <param name="lengthFromCurrent">Estimated length from current position.</param>
		/// <remarks>
		///		Stream source might optimize internal resource management with specified hint.
		///		For example, when unpacker process BLOB data, it can hint length of binary to deserializer since it must know size of raw data.
		///		And then, the deserializer can use hint to reallocate receiving buffer efficiently.
		/// </remarks>
		void NotifySegmentLength( long lengthFromCurrent );
	}
}
