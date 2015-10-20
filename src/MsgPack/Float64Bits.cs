#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Runtime.InteropServices;

namespace MsgPack
{
	/// <summary>
	///		Provides bit access for <see cref="Double"/>.
	/// </summary>
	[StructLayout( LayoutKind.Explicit )]
	internal struct Float64Bits
	{
		/// <summary>
		///		Value as <see cref="Double"/>.
		/// </summary>
		[FieldOffset( 0 )]
		public readonly double Value;

		/// <summary>
		///		Most significant byte of current endian.
		/// </summary>
		[FieldOffset( 0 )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Union" )]
		public readonly Byte Byte0;

		/// <summary>
		///		2nd bit from most significant byte of current endian.
		/// </summary>
		[FieldOffset( 1 )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Union" )]
		public readonly Byte Byte1;

		/// <summary>
		///		3rd byte from most significant byte of current endian.
		/// </summary>
		[FieldOffset( 2 )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Union" )]
		public readonly Byte Byte2;

		/// <summary>
		///		4th byte from most significant byte of current endian.
		/// </summary>
		[FieldOffset( 3 )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Union" )]
		public readonly Byte Byte3;

		/// <summary>
		///		5th byte from most significant byte of current endian.
		/// </summary>
		[FieldOffset( 4 )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Union" )]
		public readonly Byte Byte4;

		/// <summary>
		///		6th byte from most significant byte of current endian.
		/// </summary>
		[FieldOffset( 5 )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Union" )]
		public readonly Byte Byte5;

		/// <summary>
		///		7th byte from most significant byte of current endian.
		/// </summary>
		[FieldOffset( 6 )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Union" )]
		public readonly Byte Byte6;

		/// <summary>
		///		Least significant byte of current endian.
		/// </summary>
		[FieldOffset( 7 )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Union" )]
		public readonly Byte Byte7;

		/// <summary>
		///		Initializes a new instance of the <see cref="Float64Bits"/> type from specified <see cref="Byte"/>[] which is big endian.
		/// </summary>
		/// <param name="bigEndianBytes">Array of <see cref="Byte"/> which contains bytes in big endian.</param>
		/// <param name="offset">Offset to read.</param>
		public Float64Bits( byte[] bigEndianBytes, int offset )
		{
#if !UNITY && DEBUG
			Contract.Assert( bigEndianBytes != null, "bigEndianBytes != null" );
			Contract.Assert( bigEndianBytes.Length - offset >= 8, bigEndianBytes.Length + "-" + offset + ">= 4" );
#endif // !UNITY && DEBUG

			this = default( Float64Bits );

			if ( BitConverter.IsLittleEndian )
			{
				this.Byte0 = bigEndianBytes[ offset + 7 ];
				this.Byte1 = bigEndianBytes[ offset + 6 ];
				this.Byte2 = bigEndianBytes[ offset + 5 ];
				this.Byte3 = bigEndianBytes[ offset + 4 ];
				this.Byte4 = bigEndianBytes[ offset + 3 ];
				this.Byte5 = bigEndianBytes[ offset + 2 ];
				this.Byte6 = bigEndianBytes[ offset + 1 ];
				this.Byte7 = bigEndianBytes[ offset ];
			}
			else
			{
				this.Byte0 = bigEndianBytes[ offset ];
				this.Byte1 = bigEndianBytes[ offset + 1 ];
				this.Byte2 = bigEndianBytes[ offset + 2 ];
				this.Byte3 = bigEndianBytes[ offset + 3 ];
				this.Byte4 = bigEndianBytes[ offset + 4 ];
				this.Byte5 = bigEndianBytes[ offset + 5 ];
				this.Byte6 = bigEndianBytes[ offset + 6 ];
				this.Byte7 = bigEndianBytes[ offset + 7 ];
			}
		}
	}
}
