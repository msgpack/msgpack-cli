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
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace MsgPack
{
	/// <summary>
	///		Provides bit access for <see cref="Single"/>.
	/// </summary>

	[StructLayout( LayoutKind.Explicit )]
	internal struct Float32Bits
	{
		/// <summary>
		///		Value as <see cref="Single"/>.
		/// </summary>
		[FieldOffset( 0 )]
		public readonly float Value;

		/// <summary>
		///		Most significant byte of current endian.
		/// </summary>
		[FieldOffset( 0 )]
		public readonly Byte Byte0;

		/// <summary>
		///		2nd bit from most significant byte of current endian.
		/// </summary>
		[FieldOffset( 1 )]
		public readonly Byte Byte1;


		/// <summary>
		///		3rd byte from most significant byte of current endian.
		/// </summary>
		[FieldOffset( 2 )]
		public readonly Byte Byte2;

		/// <summary>
		///		Least byte of current endian.
		/// </summary>
		[FieldOffset( 3 )]
		public readonly Byte Byte3;

		/// <summary>
		///		Initializes a new instance of the <see cref="Float32Bits"/> type from specified <see cref="Single"/>.
		/// </summary>
		/// <param name="value">Value of <see cref="Single"/>.</param>
		public Float32Bits( float value )
		{
			this = default( Float32Bits );
			this.Value = value;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="Float32Bits"/> type from specified <see cref="Byte"/>[] which is big endian.
		/// </summary>
		/// <param name="bigEndianBytes">Array of <see cref="Byte"/> which contains bytes in big endian.</param>
		/// <param name="offset">Offset to read.</param>
		public Float32Bits( byte[] bigEndianBytes, int offset )
		{
			Contract.Assume( bigEndianBytes != null );
			Contract.Assume( bigEndianBytes.Length - offset >= 4, bigEndianBytes.Length.ToString() + "-" + offset.ToString() + ">= 4" );

			this = default( Float32Bits );

			if ( BitConverter.IsLittleEndian )
			{
				this.Byte0 = bigEndianBytes[ offset + 3 ];
				this.Byte1 = bigEndianBytes[ offset + 2 ];
				this.Byte2 = bigEndianBytes[ offset + 1 ];
				this.Byte3 = bigEndianBytes[ offset ];
			}
			else
			{
				this.Byte0 = bigEndianBytes[ offset ];
				this.Byte1 = bigEndianBytes[ offset + 1 ];
				this.Byte2 = bigEndianBytes[ offset + 2 ];
				this.Byte3 = bigEndianBytes[ offset + 3 ];
			}
		}
	}
}
