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
using System.Diagnostics.Contracts;

namespace MsgPack.Collections
{
	/// <summary>
	///		Defines extension methods for <see cref="ArraySegment&lt;T&gt;">ArraySegment</see>&lt;<see cref="Byte"/>&gt;.
	/// </summary>
	internal static class ByteArraySegmentExtensions
	{
		/// <summary>
		///		Copy bytes from specified array to segment. 
		/// </summary>
		/// <param name="source"><see cref="ArraySegment&lt;T&gt;">ArraySegment</see>&lt;<see cref="Byte"/>&gt; to be copied to.</param>
		/// <param name="sourceOffsetInSegment">
		///		Start offset of <paramref name="source"/> to be copied. 
		///		Note that this value is offset from <paramref name="source"/>.<see cref="ArraySegment&lt;T&gt;.Offset">Offset</see>.
		///	</param>
		/// <param name="array">Array which cointains bytes to be copied from.</param>
		/// <param name="offset">Offset to start copy in <paramref name="array"/>.</param>
		/// <param name="count">Length of bytes to be copied.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="sourceOffsetInSegment"/> is negative.
		///		Or <paramref name="sourceOffsetInSegment"/> is greator than or equal to <see cref="ArraySegment&lt;T&gt;.Count">Count</see> of <paramref name="source"/>.
		/// </exception>
		public static void CopyFrom( this ArraySegment<byte> source, int sourceOffsetInSegment, byte[] array, int offset, int count )
		{
			Validation.ValidateBuffer( array, offset, count, "array", "count", true );

			if ( sourceOffsetInSegment < 0 || sourceOffsetInSegment >= source.Count )
			{
				throw new ArgumentOutOfRangeException( "sourceOffsetInSegment" );
			}

			if ( source.Count < count )
			{
				throw new ArgumentException( "source", "'source' is too small to copy." );
			}

			Contract.EndContractBlock();

			Buffer.BlockCopy( array, offset, source.Array, source.Offset + sourceOffsetInSegment, count );
		}
	}
}
