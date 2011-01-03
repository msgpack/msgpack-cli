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
using System.Diagnostics.Contracts;
using System.IO;
using MsgPack.Linq;

namespace MsgPack
{
	// For integer direct conversions, see Unpackaging.Integers.tt & .cs.
	// For other direct conversions, see Unpackaging.Others.tt & .cs.
	// For string convenient APIs, see Unpackaging.String.cs.
	// This file defines common utility and indirect conversion.

	/// <summary>
	///		Defines direct conversion value from/to Message Pack binary stream without intermediate <see cref="MessagePackObject"/>.
	/// </summary>
	public static partial class Unpacking
	{
		private static float ReadSingle( byte[] source, int offset )
		{
			if ( source.Length < offset + sizeof( float ) )
			{
				throw new UnpackException( "Insufficient array buffer length." );
			}

			return ReadSingleCore( source[ offset ], source[ offset + 1 ], source[ offset + 2 ], source[ offset + 3 ] );
		}

		private static float ReadSingle( Stream source )
		{
			if ( source.Length < source.Position + sizeof( float ) )
			{
				throw new UnpackException( "Insufficient stream length." );
			}

			return unchecked( ReadSingleCore( ( byte )source.ReadByte(), ( byte )source.ReadByte(), ( byte )source.ReadByte(), ( byte )source.ReadByte() ) );
		}

		private static float ReadSingleCore( byte b0, byte b1, byte b2, byte b3 )
		{
			// It might be faster by avoding byte array allocation.
			var buffer = new byte[ sizeof( float ) ];
			buffer[ 0 ] = b0;
			buffer[ 1 ] = b1;
			buffer[ 2 ] = b2;
			buffer[ 3 ] = b3;

			if ( BitConverter.IsLittleEndian )
			{
				Array.Reverse( buffer );
			}

			return BitConverter.ToSingle( buffer, 0 );
		}

		private static double ReadDouble( byte[] source, int offset )
		{
			if ( source.Length < offset + sizeof( double ) )
			{
				throw new UnpackException( "Insufficient array buffer length." );
			}

			return ReadDoubleCore( source[ offset ], source[ offset + 1 ], source[ offset + 2 ], source[ offset + 3 ], source[ offset + 4 ], source[ offset + 5 ], source[ offset + 6 ], source[ offset + 7 ] );
		}

		private static double ReadDouble( Stream source )
		{
			if ( source.Length < source.Position + sizeof( double ) )
			{
				throw new UnpackException( "Insufficient stream length." );
			}

			return unchecked( ReadDoubleCore( ( byte )source.ReadByte(), ( byte )source.ReadByte(), ( byte )source.ReadByte(), ( byte )source.ReadByte(), ( byte )source.ReadByte(), ( byte )source.ReadByte(), ( byte )source.ReadByte(), ( byte )source.ReadByte() ) );
		}

		private static double ReadDoubleCore( byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7 )
		{
			long buffer = 0;
			unchecked
			{
				buffer |= ( long )( ( long )b0 << 56 );
				buffer |= ( long )( ( long )b1 << 48 );
				buffer |= ( long )( ( long )b2 << 40 );
				buffer |= ( long )( ( long )b3 << 32 );
				buffer |= ( long )( ( long )b4 << 24 );
				buffer |= ( long )( ( long )b5 << 16 );
				buffer |= ( long )( ( long )b6 << 8 );
				buffer |= b7;
			}

			return BitConverter.Int64BitsToDouble( buffer );
		}

		/// <summary>
		///		Unpack specified stream as <see cref="MessagePackObject"/>.
		/// </summary>
		/// <param name="source">Source <see cref="Stream"/>.</param>
		/// <returns>
		///		<see cref="MessagePackObject"/>.
		///		If <paramref name="source"/> does not contain enough bytes, this value may be null.
		///	</returns>
		///	<exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <remarks>
		///		You can determine actual data type with <see cref="MessagePackObject"/>.
		///		This method is useful when you don't have any knowledge about actual data type of input stream.
		/// </remarks>
		public static MessagePackObject? UnpackObject( Stream source )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return new StreamingUnpacker().Unpack( source.AsEnumerable() );
		}

		/// <summary>
		///		Unpack specified bytes as <see cref="MessagePackObject"/>.
		/// </summary>
		/// <param name="source">Source <see cref="IEnumerable&lt;T&gt;">IEnumerable</see>&lt;<see cref="Byte"/>&gt;.</param>
		/// <returns>
		///		<see cref="MessagePackObject"/>.
		///		If <paramref name="source"/> does not contain enough bytes, this value may be null.
		///	</returns>
		///	<exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <remarks>
		///		You can determine actual data type with <see cref="MessagePackObject"/>.
		///		This method is useful when you don't have any knowledge about actual data type of input stream.
		/// </remarks>
		public static MessagePackObject? UnpackObject( IEnumerable<byte> source )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return new StreamingUnpacker().Unpack( source );
		}
	}
}
