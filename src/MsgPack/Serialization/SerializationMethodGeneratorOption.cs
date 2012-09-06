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
using System.ComponentModel;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Define options of serializer generation.
	/// </summary>
	public enum SerializationMethodGeneratorOption
	{
#if !SILVERLIGHT
		/// <summary>
		///		The generated method IL can be dumped to the current directory.
		///		It is intended for the runtime, you cannot use this option.
		/// </summary>
		[EditorBrowsable( EditorBrowsableState.Never )]
		CanDump,

		/// <summary>
		///		The entire generated method can be collected by GC when it is no longer used.
		/// </summary>
		CanCollect,
#endif

		/// <summary>
		///		Prefer performance. This options is default.
		/// </summary>
		Fast
	}
}
