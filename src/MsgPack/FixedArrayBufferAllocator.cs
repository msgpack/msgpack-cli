#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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

namespace MsgPack
{
	/// <summary>
	///		An implementation of <see cref="ByteBufferAllocator"/> which does not do any allocation and forces reuse of original array.
	/// </summary>
	internal sealed class FixedArrayBufferAllocator : ByteBufferAllocator
	{
		public static readonly ByteBufferAllocator Instance = new FixedArrayBufferAllocator();

		private FixedArrayBufferAllocator() { }

		public override bool TryAllocate( byte[] oldBuffer, int requestSize, out byte[] newBuffer )
		{
			newBuffer = null;
			return false;
		}
	}
}
