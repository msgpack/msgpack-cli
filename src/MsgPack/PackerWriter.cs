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
	///		Encapsulates writing to underlying destination.
	/// </summary>
	internal abstract class PackerWriter : IDisposable
	{
		protected PackerWriter() { }

		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing ) { }

		public abstract void WriteByte( byte value );

		public abstract void WriteBytes( byte header, byte value );

		public abstract void WriteBytes( byte header, ushort value );

		public abstract void WriteBytes( byte header, uint value );

		public abstract void WriteBytes( byte header, ulong value );

		public abstract void WriteBytes( byte header, float value );

		public abstract void WriteBytes( byte header, double value );

		// TODO: ReadOnlySpan<byte>
		public abstract void WriteBytes( byte[] value );

		// TODO: ReadOnlySpan<char>
		public abstract void WriteBytes( string value, bool allowStr8 ); // For conversion efficiency

#if FEATURE_TAP

		public abstract Task WriteByteAsync( byte value, CancellationToken cancellationToken );

		public abstract Task WriteBytesAsync( byte header, byte value, CancellationToken cancellationToken );

		public abstract Task WriteBytesAsync( byte header, ushort value, CancellationToken cancellationToken );

		public abstract Task WriteBytesAsync( byte header, uint value, CancellationToken cancellationToken );

		public abstract Task WriteBytesAsync( byte header, ulong value, CancellationToken cancellationToken );

		public abstract Task WriteBytesAsync( byte header, float value, CancellationToken cancellationToken );

		public abstract Task WriteBytesAsync( byte header, double value, CancellationToken cancellationToken );

		// TODO: ReadOnlySpan<byte>
		public abstract Task WriteBytesAsync( byte[] value, CancellationToken cancellationToken );

		// TODO: ReadOnlySpan<char>
		public abstract Task WriteBytesAsync( string value, bool allowStr8, CancellationToken cancellationToken ); // For Convert efficiency

#endif // FEATURE_TAP

		protected static int ToBits( float value )
		{
			var bits = new Float32Bits( value );
			var result = default( int );

			// Float32Bits usage is effectively pointer dereference operation rather than shifting operators, so we must consider endianness here.
			if ( BitConverter.IsLittleEndian )
			{
				result = bits.Byte3 << 24;
				result |= bits.Byte2 << 16;
				result |= bits.Byte1 << 8;
				result |= bits.Byte0;
			}
			else
			{
				result = bits.Byte0 << 24;
				result |= bits.Byte1 << 16;
				result |= bits.Byte2 << 8;
				result |= bits.Byte3;
			}

			return result;
		}

		protected static long ToBits( double value )
		{
			return BitConverter.DoubleToInt64Bits( value );
		}
	}
}
