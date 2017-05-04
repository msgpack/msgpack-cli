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
			Eof			=   0,
			Nil			=   1	<< 8,
			Boolean		=   2	<< 8,
			SByte		=   3	<< 8,
			Byte		=   4	<< 8,
			Int16		=   5	<< 8,
			UInt16		=   6	<< 8,
			Int32		=   7	<< 8,
			UInt32		=   8	<< 8,
			Int64		=   9	<< 8,
			UInt64		=   10	<< 8,
			Single		=   11	<< 8,
			Double		=   12	<< 8,
			ArrayLength	=   13	<< 8,
			MapLength	=   14	<< 8,
			String		=   15	<< 8,
			Binary		=   16	<< 8,
			FixExt1		= ( 17	<< 8 ) | 0x1,
			FixExt2		= ( 18	<< 8 ) | 0x2,
			FixExt4		= ( 19	<< 8 ) | 0x4,
			FixExt8		= ( 20	<< 8 ) | 0x8,
			FixExt16	= ( 21	<< 8 ) | 0x10,
			Ext8		= ( 22	<< 8 ) | ( 0x1 << 5 ),
			Ext16		= ( 23	<< 8 ) | ( 0x2 << 5 ),
			Ext32		= ( 24	<< 8 ) | ( 0x4 << 5 ),
			FixedLengthMask = 0x1F,
			VariableLengthMask = 0xE0
		}

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
