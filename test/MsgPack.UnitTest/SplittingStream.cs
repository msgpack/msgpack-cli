using System;
using System.IO;

namespace MsgPack
{
	/// <summary>
	///		Simulates splitted stream.
	/// </summary>
	internal sealed class SplittingStream : Stream
	{
		private readonly Stream _underlying;
		private long _position;

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override long Length
		{
			get { return this._underlying.Length; }
		}

		public override long Position
		{
			get { return this._position; }
			set { throw new NotSupportedException(); }
		}

		public SplittingStream( Stream underlying )
		{
			if ( underlying == null )
			{
				throw new ArgumentNullException( "underlying" );
			}

			if ( !underlying.CanRead )
			{
				throw new ArgumentException( "underlyiing must be readable.", "underlying" );
			}

			this._underlying = underlying;
		}

		public override void Flush()
		{
			throw new NotSupportedException();
		}

		public override int Read( byte[] buffer, int offset, int count )
		{
			var read = this._underlying.Read( buffer, offset, count > 0 ? 1 : count );
			this._position += read;
			return read;
		}

		public override long Seek( long offset, SeekOrigin origin )
		{
			throw new NotSupportedException();
		}

		public override void SetLength( long value )
		{
			throw new NotSupportedException();
		}

		public override void Write( byte[] buffer, int offset, int count )
		{
			throw new NotSupportedException();
		}
	}
}