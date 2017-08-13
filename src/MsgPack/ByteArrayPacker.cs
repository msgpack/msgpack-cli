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
		public abstract long BytesUsed { get; }

		/// <summary>
		///		Gets the initial index of destination buffers.
		/// </summary>
		/// <value>
		///		The initial index of destination buffers.
		///		This value is greater than or equal to <c>0</c>,
		///		and less than the <see cref="ICollection{T}.Count"/> of the result of <see cref="GetFinalBuffers()"/>.
		/// </value>
		public abstract int InitialBufferIndex { get; }

		/// <summary>
		///		Gets the current index of destination buffers.
		/// </summary>
		/// <value>
		///		The current index of destination buffers.
		///		This value is greater than or equal to <c>0</c>,
		///		and less than the <see cref="ICollection{T}.Count"/> of the result of <see cref="GetFinalBuffers()"/>.
		/// </value>
		public abstract int CurrentBufferIndex { get; }

		/// <summary>
		///		Gets the current offset of the current destination buffer.
		/// </summary>
		/// <value>
		///		The current offset of the current destination buffer.
		///		This value is greater than or equal to <see cref="ArraySegment{T}.Offset"/> of an item of <see cref="GetFinalBuffers()"/> in index <see cref="CurrentBufferIndex"/>
		///		and less than the length of the <see cref="ArraySegment{T}.Array"/> of it.
		/// </value>
		public abstract int CurrentBufferOffset { get; }

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
		///		Gets the final effective (written) bytes as single array.
		/// </summary>
		/// <returns>The final buffers as single array. Its size will be <see cref="BytesUsed"/>.</returns>
		/// <exception cref="InvalidOperationException">Buffer is too large for single array.</exception>
		/// <remarks>
		///		If the packer was allowed re-allocation, you can get final byte array from this method.
		///		Otherwise, the returned buffer contains only last successful operation.
		///		Note that there are no guarantee about the returned array is copied from the original buffer.
		/// </remarks>
		public byte[] GetResultBytes()
		{
			var bytesUsed = this.BytesUsed;
			if ( bytesUsed == 0 )
			{
				return Binary.Empty;
			}

			var list = this.GetFinalBuffers();
			switch ( list.Count )
			{
				case 0:
				{
					return Binary.Empty;
				}
				case 1:
				{
					var segment = list[ 0 ];
					if ( segment.Offset == 0 && segment.Count == segment.Array.Length && segment.Count == bytesUsed )
					{
						return segment.Array;
					}
					else
					{
						var result = new byte[ bytesUsed ];
						Buffer.BlockCopy( segment.Array, segment.Offset, result, 0, unchecked( ( int )bytesUsed) );
						return result;
					}
				}
				default:
				{
					if ( bytesUsed > Int32.MaxValue )
					{
						throw new InvalidOperationException( "Buffer is too large for single array." );
					}

					var result = new byte[ bytesUsed ];
					var offset = 0;
					foreach ( var segment in list.Skip( this.InitialBufferIndex ) )
					{
						var copying = Math.Min( segment.Count, result.Length - offset );
						Buffer.BlockCopy( segment.Array, segment.Offset, result, offset, copying );
						offset += copying;

						if( offset == result.Length )
						{
							break;
						}
					}

					return result;
				}
			}
		}

		/// <summary>
		///		Gets the final buffers as list of byte array segments.
		/// </summary>
		/// <returns>The final buffers as list of byte array segments.</returns>
		/// <remarks>
		///		If the packer was allowed re-allocation, you can get adjusted byte array segments list from this method.
		///		Otherwise, the returned buffer should be same as list passed in the constructor.
		///		The result buffers may contain extra bytes.
		/// </remarks>
		public abstract IList<ArraySegment<byte>> GetFinalBuffers();
	}
}
