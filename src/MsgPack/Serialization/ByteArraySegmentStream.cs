#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.IO;

namespace MsgPack.Serialization
{
	/// <summary>
	///		A simple wrapper for array segment.
	/// </summary>
	internal sealed class ByteArraySegmentStream : Stream
	{
		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		private readonly ArraySegment<byte> _underlying;

		public override long Length
		{
			get { return this._underlying.Count; }
		}

		private int _position;

		public override long Position
		{
			get { return this._position; }
			set
			{
				if ( value > Int32.MaxValue || value < 0 )
				{
					throw new ArgumentOutOfRangeException( "value" );
				}

				this._position = unchecked( ( int )value );
			}
		}

		public ByteArraySegmentStream( ArraySegment<byte> underlying )
		{
			Contract.Assert( underlying.Array != null );
			this._underlying = underlying;
		}

		public override int ReadByte()
		{
			if ( this._position >= this._underlying.Count )
			{
				return -1;
			}

			byte result = this._underlying.Array[ this._position + this._underlying.Offset ];
			this._position++;

			return result;
		}

		public override int Read( byte[] buffer, int offset, int count )
		{
			int copyingLength = Math.Min( this._underlying.Count - this._position, count );
			Buffer.BlockCopy(
				this._underlying.Array,
				this._position + this._underlying.Offset,
				buffer,
				offset,
				copyingLength
			);

			this._position += copyingLength;

			return copyingLength;
		}

		public override long Seek( long offset, SeekOrigin origin )
		{
			long target64;
			switch ( origin )
			{
				case SeekOrigin.Begin:
				{
					target64 = offset;
					break;
				}
				case SeekOrigin.Current:
				{
					target64 = offset + this._position;
					break;
				}
				case SeekOrigin.End:
				{
					target64 = this.Length + offset;
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "origin" );
				}
			}

			if ( target64 < 0 )
			{
				throw new IOException( "Cannot set position before head of the stream." );
			}

			if ( target64 > this.Length )
			{
				throw new NotSupportedException( "Cannot extend this read only stream." );
			}

			return ( this.Position = target64 );
		}

		public override void SetLength( long value )
		{
			throw NewReadOnlyException();
		}

		public override void Write( byte[] buffer, int offset, int count )
		{
			throw NewReadOnlyException();
		}

		private static Exception NewReadOnlyException()
		{
			return new NotSupportedException("This stream is read only.");
		}

		public override void Flush()
		{
			// nop
		}
	}
}
