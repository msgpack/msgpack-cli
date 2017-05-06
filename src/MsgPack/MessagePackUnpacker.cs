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
using System.Linq;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		Defines non generic portion of <see cref="MessagePackUnpacker{TReader}"/>.
	/// </summary>
	internal abstract partial class MessagePackUnpacker : IDisposable
	{
		public uint ItemsCount { get; set; }

		public CollectionType CollectionType { get; set; }

        public MessagePackObject Data { get; set; }

		protected MessagePackUnpacker() { }

		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing ) { }

		public abstract bool Read();

		public abstract long? Skip();

#if FEATURE_TAP
		public abstract Task<bool> ReadAsync( CancellationToken cancellationToken );

		public abstract Task<long?> SkipAsync( CancellationToken cancellationToken );
#endif // FEATURE_TAP

		protected enum ReadValueResult : uint
		{
			// +-----------+-------+------+-----+-----+
			// | 31  -  16 | 15-14 | 13-8 | 7-5 | 4-0 |
			// +-----------+-------+------+-----+-----+
			// |(reserved) | CLMSK | type | LoL | FxL |
			// +-----------+-------+------+-----+-----+
			// CLMSK: Mask for CollectionType (11 or 00)
			// LoL: Length of variable Length(byte)
			// FxL: Fixed Length
			None		=   0,
			Eof			=   1	<< 8,
			Nil			=   2	<< 8,
			Boolean		=   3	<< 8,
			SByte		=   4	<< 8,
			Byte		=   5	<< 8,
			Int16		=   6	<< 8,
			UInt16		=   7	<< 8,
			Int32		=   8	<< 8,
			UInt32		=   9	<< 8,
			Int64		=   10	<< 8,
			UInt64		=   11	<< 8,
			Single		=   12	<< 8,
			Double		=   13	<< 8,
			ArrayLength	=   14	<< 8 | 0xC000,
			MapLength	=   15	<< 8 | 0xC000,
			String		=   16	<< 8,
			Binary		=   17	<< 8,
			FixExt1		= ( 18	<< 8 ) | 0x1,
			FixExt2		= ( 19	<< 8 ) | 0x2,
			FixExt4		= ( 20	<< 8 ) | 0x4,
			FixExt8		= ( 21	<< 8 ) | 0x8,
			FixExt16	= ( 22	<< 8 ) | 0x10,
			Ext8		= ( 23	<< 8 ) | ( 0x1 << 5 ),
			Ext16		= ( 24	<< 8 ) | ( 0x2 << 5 ),
			Ext32		= ( 25	<< 8 ) | ( 0x4 << 5 ),
			FixedLengthMask = 0x1F,
			VariableLengthMask = 0xE0,
			ValidRangeMask = 0xFFFF
		}

		// Index = read-header + 1
		// Value(short) << 16 | ReadValueResult
		protected static readonly ReadValueResult[] EncodedTypes =
			new [] { ReadValueResult.Eof } // for -1
			.Concat(
				Enumerable.Range( 0, 0x80 ).Select( i => unchecked( ( ReadValueResult )( uint )( i << 16 | ( uint )ReadValueResult.Byte ) ) )
			).Concat(
				Enumerable.Range( 0x80, 0x10 ).Select( i => unchecked( ( ReadValueResult )( uint )( ( i - 0x80 ) << 16 | ( uint )ReadValueResult.MapLength ) ) )
			).Concat(
				Enumerable.Range( 0x90, 0x10 ).Select( i => unchecked( ( ReadValueResult )( uint )( ( i - 0x90 ) << 16 | ( uint )ReadValueResult.ArrayLength ) ) )
			).Concat(
				Enumerable.Range( 0xA0, 0x20 ).Select( i => unchecked( ( ReadValueResult )( uint )( ( i - 0xA0 ) << 16 | ( uint )ReadValueResult.String ) ) )
			).Concat(
				new []
				{
					ReadValueResult.Nil,
					ReadValueResult.None, // reserved
					ReadValueResult.Boolean, // false
					unchecked ( ( ReadValueResult )( 0x10000 | ( uint )ReadValueResult.Boolean ) )// true
				}
			).Concat(
				Enumerable.Repeat( ReadValueResult.None, ( 0x20 - 4 ) )
			).Concat(
				Enumerable.Range( 0xE0, 0x20 ).Select( b => unchecked ( ( ReadValueResult )( uint )( ( 0xFF00 | b ) << 16 | ( uint )ReadValueResult.SByte ) ) )
			).ToArray();

#if FEATURE_TAP

		protected struct AsyncReadValueResult
		{
			public ReadValueResult type;
			public byte header;
			public long integral;
			public float real32;
			public double real64;
		}

#endif // FEATURE_TAP
	}
}
