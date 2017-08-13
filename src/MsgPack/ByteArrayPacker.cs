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
using System.Linq;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		Defines interface and basic functionality for byte array based <see cref="Packer"/>.
	/// </summary>
	public abstract class ByteArrayPacker : Packer
	{
		/// <summary>
		///		Gets the bytes used by this instance.
		/// </summary>
		/// <value>
		///		The bytes used by this instance. The initial state is <c>0</c>.
		/// </value>
		public abstract int BytesUsed { get; }

		/// <summary>
		///		Gets the initial offset of the destination buffer.
		/// </summary>
		/// <value>
		///		The initial index of the destination buffer.
		///		This value is greater than or equal to <c>0</c> and less than the destination buffer's length.
		/// </value>
		public abstract int InitialBufferOffset { get; }

		/// <summary>
		///		Initializes a new instance of the <see cref="ByteArrayPacker"/> class.
		/// </summary>
		protected ByteArrayPacker() { }

		/// <summary>
		///		Initializes a new instance of the <see cref="Packer"/> class with specified <see cref="PackerCompatibilityOptions"/>.
		/// </summary>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		protected ByteArrayPacker( PackerCompatibilityOptions compatibilityOptions )
			: base( compatibilityOptions ) { }

		/// <summary>
		///		Gets the final effective (written) bytes as single array segment.
		/// </summary>
		/// <returns>The final buffers as single <see cref="ArraySegment{T}"/>. Its size will be <see cref="BytesUsed"/>.</returns>
		/// <remarks>
		///		The result segment contains the array returned from <see cref="GetFinalBuffer()"/>, and reflects <see cref="InitialBufferOffset"/> and <see cref="BytesUsed"/>.
		/// </remarks>
		public ArraySegment<byte> GetResultBytes()
		{
			return new ArraySegment<byte>( this.GetFinalBuffer(), this.InitialBufferOffset, this.BytesUsed );
		}

		/// <summary>
		///		Gets the final buffer which may be reallocated.
		/// </summary>
		/// <returns>The final buffer which may be reallocated.</returns>
		/// <remarks>
		///		If the packer was allowed re-allocation, you can get new byte array from this method.
		///		Otherwise, the returned buffer should be same as the array passed in the constructor.
		/// </remarks>
		public abstract byte[] GetFinalBuffer();
	}
}
