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

namespace MsgPack
{
	/// <summary>
	///		Defines interface and basic functionality for byte array based <see cref="Unpacker"/>.
	/// </summary>
	public abstract class ByteArrayUnpacker : Unpacker
	{
		/// <summary>
		///		Gets the source bytes as <see cref="ArraySegment{T}"/>.
		/// </summary>
		/// <value>
		///		The source bytes as <see cref="ArraySegment{T}"/>. The value will have non <c>null</c> array.
		/// </value>
		protected ArraySegment<byte> Source { get; private set; }

		/// <summary>
		///		Gets or sets the bytes used by this instance.
		/// </summary>
		/// <value>
		///		The bytes used by this instance. The initial state is <c>0</c>.
		///		This value only set from derived types.
		/// </value>
		public int BytesUsed { get; protected set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="ByteArrayUnpacker"/> class.
		/// </summary>
		/// <param name="source">Source byte array.</param>
		/// <exception cref="ArgumentException"><paramref name="source"/> does not have non <c>null</c> array.</exception>
		protected ByteArrayUnpacker( ArraySegment<byte> source )
		{
			if ( source.Array == null )
			{
				throw new ArgumentException( "Source cannot be empty.", "source" );
			}

			this.Source = source;
		}

		/// <summary>
		///		Gets the remaining bytes of <see cref="Source"/> as new <see cref="ArraySegment{T}"/>.
		/// </summary>
		/// <returns>
		///		The remaining bytes as new <see cref="ArraySegment{T}"/>.
		///		This value will contain valid array, but its <see cref="ArraySegment{T}.Count"/> can be <c>0</c>.
		///	</returns>
		public ArraySegment<byte> GetRemainingBytes()
		{
			return new ArraySegment<byte>( this.Source.Array, this.Source.Offset + this.BytesUsed, this.Source.Count - this.BytesUsed );
		}
	}
}
