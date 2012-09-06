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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace MsgPack
{
	internal sealed class EnumerableStream : Stream
	{
		private readonly IEnumerator<byte> _underlyingEnumerator;
		private readonly IList<byte> _underlyingList;
		private bool _isDisposed;

		public sealed override bool CanRead
		{
			get { return !this._isDisposed; }
		}

		public sealed override bool CanSeek
		{
			get { return !this._isDisposed && this._underlyingList != null; }
		}

		public sealed override bool CanWrite
		{
			get { return false; }
		}

		public sealed override long Length
		{
			get
			{
				this.VerifyCanSeek();
				return this._underlyingList.Count;
			}
		}

		private int _position;

		public override long Position
		{
			get
			{
				this.VerifyCanSeek();
				return this._position;
			}
			set
			{
				this.VerifyCanSeek();
				if ( value < 0 || Int32.MaxValue < value )
				{
					throw new ArgumentOutOfRangeException( "value" );
				}

				if ( this.Length <= value )
				{
					this.SetLength( value );
				}

				this._position = unchecked( ( int )value );
			}
		}

		public EnumerableStream( IEnumerable<byte> source )
		{
			Contract.Assert( source != null );
			this._underlyingList = source as IList<byte>;
			if ( this._underlyingList == null )
			{
				this._underlyingEnumerator = source.GetEnumerator();
			}
			else
			{
				this._underlyingEnumerator = null;
			}
		}

		protected sealed override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
			if ( disposing )
			{
				if ( this._underlyingEnumerator != null )
				{
					this._underlyingEnumerator.Dispose();
				}
			}

			this._isDisposed = true;

		}

		private void VerifyCanSeek()
		{
			this.VerifyIsNotDisposed();

			if ( !this.CanSeek )
			{
				throw new NotSupportedException();
			}
		}

		private void VerifyIsNotDisposed()
		{
			if ( this._isDisposed )
			{
				throw new ObjectDisposedException( this.GetType().FullName );
			}
		}

		public sealed override int Read( byte[] buffer, int offset, int count )
		{
			this.VerifyIsNotDisposed();

			if ( this._underlyingList != null )
			{
				int remains = this._underlyingList.Count - this._position;
				if ( remains == 0 )
				{
					return 0;
				}

				int actualCount = Math.Min( count, remains );
				byte[] asArray;
				List<byte> asList;
				if ( ( asArray = this._underlyingList as byte[] ) != null )
				{
					Buffer.BlockCopy( asArray, this._position, buffer, offset, actualCount );
				}
				else if ( ( asList = this._underlyingList as List<byte> ) != null )
				{
					asList.CopyTo( this._position, buffer, offset, actualCount );
				}
				else
				{
					int i = 0;
					foreach ( var b in this._underlyingList.Skip( this._position ).Take( actualCount ) )
					{
						buffer[ i + offset ] = b;
						i++;
					}
				}

				return actualCount;
			}
			else
			{
				int read = 0;
				for ( ; read < count && this._underlyingEnumerator.MoveNext(); read++ )
				{
					buffer[ read + offset ] = this._underlyingEnumerator.Current;
				}

				return read;
			}
		}

		public sealed override int ReadByte()
		{
			this.VerifyIsNotDisposed();
			if ( this._underlyingList != null )
			{
				if ( this._position < this._underlyingList.Count - 1 )
				{
					this._position++;
					return this._underlyingList[ this._position ];
				}
			}
			else
			{
				if ( this._underlyingEnumerator.MoveNext() )
				{
					return this._underlyingEnumerator.Current;
				}
			}

			return -1;
		}

		public sealed override long Seek( long offset, SeekOrigin origin )
		{
			this.VerifyCanSeek();

			switch ( origin )
			{
				case SeekOrigin.Begin:
				{
					return this.Position = offset;
				}
				case SeekOrigin.Current:
				{
					return this.Position = this.Position + offset;
				}
				case SeekOrigin.End:
				{
					return this.Position = this.Length + offset;
				}
			}

			throw new ArgumentOutOfRangeException( "origin" );
		}

		public sealed override void SetLength( long value )
		{
			throw new NotSupportedException();
		}

		public sealed override void Write( byte[] buffer, int offset, int count )
		{
			throw new NotSupportedException();
		}

		public sealed override void Flush()
		{
			// nop
		}
	}
}
