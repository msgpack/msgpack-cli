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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if CORE_CLR || UNITY || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || UNITY || NETSTANDARD1_1
using System.Globalization;
using System.Linq;
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		<see cref="UnpackerReader"/> for <see cref="ArraySegment{T}"/> or array of bytes.
	/// </summary>
	internal sealed partial class ByteArrayUnpackerReader : UnpackerReader
	{
		private readonly byte[] _scalarBuffer = new byte[ 8 ];
		// TODO: Use Span<byte> and keep ArraySegment<byte>[] as _originalByteArraySegments for GetRemainingBytes();
		private readonly IList<ArraySegment<byte>> _sources;

		private int _currentSourceIndex;

		// TODO: Use Span<byte>
		private byte[] _currentSource;
		private int _currentSourceOffset;
		private int _currentSourceRemains;

#if DEBUG

		internal ArraySegment<byte> DebugSource
		{
			get { return this._sources[ this._currentSourceIndex ]; }
		}

		internal IList<ArraySegment<byte>> DebugBuffers
		{
			get { return this._sources; }
		}

#endif // DEBUG

		public override long Offset
		{
			get
			{
				return
					this._sources.Take( this._currentSourceIndex ).Sum( x => x.Count )
					+ this._currentSourceOffset - this._sources[ this._currentSourceIndex ].Offset;
			}
		}

		public int CurrentSourceOffset
		{
			get { return this._currentSourceOffset; }
		}

		public int CurrentSourceIndex
		{
			get { return this._currentSourceIndex; }
		}

		public override bool GetPreviousPosition( out long offsetOrPosition )
		{
			offsetOrPosition = this._sources[ this._currentSourceIndex ].Offset;
			return false;
		}

		// TODO: Use Span<byte>
		public ByteArrayUnpackerReader( ArraySegment<byte> source )
		{
			if ( source.Array == null || source.Count == 0)
			{
				throw new ArgumentException( "Source must have non null, non-empty Array.", "source" );
			}

			this._sources = new[] { source };
			this._currentSourceIndex = 0;
			this._currentSource = source.Array;
			this._currentSourceOffset = source.Offset;
			this._currentSourceRemains = source.Count;
		}

		// TODO: Use Span<byte>
		public ByteArrayUnpackerReader( IList<ArraySegment<byte>> sources, int startIndex, int startOffset )
		{
			if ( sources == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( startIndex < 0 )
			{
				throw new ArgumentOutOfRangeException( "The value cannot be negative.", "startIndex" );
			}

			if ( startOffset < 0 )
			{
				throw new ArgumentOutOfRangeException( "The value cannot be negative.", "startOffset" );
			}

			if ( sources.Count == 0 )
			{
				throw new ArgumentException( "Sources cannot be empty.", "sources" );
			}

			if ( sources.Any( x => x.Array == null || x.Count == 0 ) )
			{
				throw new ArgumentException( "Sources contains null or empty Array.", "sources" );
			}

			if ( sources.Count <= startIndex )
			{
				throw new ArgumentException( "Sources is too small." );
			}

			this._sources = sources;
			this._currentSourceIndex = startIndex;
			var startSource = this._sources[ startIndex ];
			var skip = startOffset - startSource.Offset;
			if ( skip < 0 )
			{
				throw new ArgumentException( "The value cannot be smaller than the array segment Offset.", "startOffset" );
			}

			if ( skip > startSource.Count )
			{
				throw new ArgumentException( "The offset cannot exceed the array segment Count.", "startOffset" );
			}

			this._currentSource = startSource.Array;
			this._currentSourceOffset = startSource.Offset + skip;
			this._currentSourceRemains = startSource.Count - skip;
		}

		private bool ShiftSourceIfNeeded( ref byte[] currentSource, ref int currentSourceOffset, ref int currentSourceRemains, ref int currentSourceIndex )
		{
			if ( currentSourceRemains == 0 )
			{
				// try shift to next buffer
				currentSourceIndex++;
				if ( this._sources.Count == currentSourceIndex )
				{
					return false;
				}

				currentSource = this._sources[ currentSourceIndex ].Array;
				currentSourceOffset = this._sources[ currentSourceIndex ].Offset;
				currentSourceRemains = this._sources[ currentSourceIndex ].Count;
			}

			return true;
		}

		// TODO: Use Span<byte>
		public bool TryRead( byte[] buffer, int requestedSize )
		{
			if ( requestedSize == 0 )
			{
				return true;
			}
			
			if ( this._currentSourceRemains >= requestedSize )
			{
				// fast path
				// TODO: Use currentSource.CopyTo( buffer, requestedSize );
				Buffer.BlockCopy( this._currentSource, this._currentSourceOffset, buffer, 0, requestedSize );
				this._currentSourceOffset += requestedSize;
				this._currentSourceRemains -= requestedSize;
				return true;
			}

			return this.TryReadSlow( buffer, requestedSize );
		}

		// TODO: Use Span<T>
		private bool TryReadSlow( byte[] buffer, int requestedSize )
		{
			var currentSource = this._currentSource;
			var currentSourceOffset = this._currentSourceOffset;
			var currentSourceRemains = this._currentSourceRemains;
			var currentSourceIndex = this._currentSourceIndex;
			if ( !this.ShiftSourceIfNeeded( ref currentSource, ref currentSourceOffset, ref currentSourceRemains, ref currentSourceIndex ) )
			{
				return false;
			}

			var remaining = requestedSize;
			var destinationOffset = 0;

			do
			{
				var copying = Math.Min( currentSourceRemains, remaining );
				// TODO: Use currentSource.CopyTo( buffer, copying );
				Buffer.BlockCopy( currentSource, currentSourceOffset, buffer, destinationOffset, copying );
				remaining -= copying;
				currentSourceOffset += copying;
				currentSourceRemains -= copying;
#if DEBUG
				Contract.Assert( remaining >= 0, "remaining >= 0" );
#endif // DEBUG

				if ( remaining <= 0 )
				{
					// Finish
					break;
				}

				destinationOffset += copying;

				if ( !this.ShiftSourceIfNeeded( ref currentSource, ref currentSourceOffset, ref currentSourceRemains, ref currentSourceIndex ) )
				{
					return false;
				}
			} while ( true );

			this._currentSourceIndex = currentSourceIndex;
			this._currentSource = currentSource;
			this._currentSourceOffset = currentSourceOffset;
			this._currentSourceRemains = currentSourceRemains;
			return true;
		}

		// TODO: Use Span<T>
		public override void Read( byte[] buffer, int size )
		{
			if ( !this.TryRead( buffer, size ) )
			{
				this.ThrowEofException( size );
			}
		}

#if FEATURE_TAP

		public override Task ReadAsync( byte[] buffer, int size, CancellationToken cancellationToken )
		{
			this.Read( buffer, size );
			return TaskAugument.CompletedTask;
		}

#endif // FEATURE_TAP

		public override string ReadString( int length )
		{
			if ( length == 0 )
			{
				return String.Empty;
			}
			
			if ( this._currentSourceRemains >= length )
			{
				// fast path
				var result = Encoding.UTF8.GetString( this._currentSource, this._currentSourceOffset, length );
				this._currentSourceOffset += length;
				this._currentSourceRemains -= length;
				return result;
			}

			// Slow path
			return this.ReadStringSlow( length );
		}

		private string ReadStringSlow( int requestedSize )
		{
			var currentSource = this._currentSource;
			var currentSourceOffset = this._currentSourceOffset;
			var currentSourceRemains = this._currentSourceRemains;
			var currentSourceIndex = this._currentSourceIndex;

			if ( !this.ShiftSourceIfNeeded( ref currentSource, ref currentSourceOffset, ref currentSourceRemains, ref currentSourceIndex ) )
			{
				this.ThrowEofException( requestedSize );
			}

			var remaining = requestedSize;

			var decoder = Encoding.UTF8.GetDecoder();
			var charBuffer = BufferManager.NewCharBuffer( requestedSize );
			var result = new StringBuilder( requestedSize );
			bool isCompleted;
			do
			{
				var decoding = Math.Min( currentSourceRemains, remaining );
				isCompleted = decoder.DecodeString( currentSource, currentSourceOffset, decoding, charBuffer, result );
				remaining -= decoding;
				currentSourceOffset += decoding;
				currentSourceRemains -= decoding;
#if DEBUG
				Contract.Assert( remaining >= 0, "remaining >= 0" );
#endif // DEBUG

				if ( remaining <= 0 )
				{
					// Finish
					break;
				}

				if ( !this.ShiftSourceIfNeeded( ref currentSource, ref currentSourceOffset, ref currentSourceRemains, ref currentSourceIndex ) )
				{
					this.ThrowEofException( requestedSize );
				}
			} while ( true );

			if ( !isCompleted )
			{
				this.ThrowBadUtf8Exception();
			}

			this._currentSourceIndex = currentSourceIndex;
			this._currentSource = currentSource;
			this._currentSourceOffset = currentSourceOffset;
			this._currentSourceRemains = currentSourceRemains;
			return result.ToString();
		}


#if FEATURE_TAP

		public override Task<string> ReadStringAsync( int length, CancellationToken cancellationToken )
		{
			return Task.FromResult( this.ReadString( length ) );
		}

#endif // FEATURE_TAP

		public override bool Drain( uint size )
		{
			if ( size == 0 )
			{
				// 0 byte drain always success.
				return true;
			}

			long remaining = size;

			var currentSource = this._currentSource;
			var currentSourceOffset = this._currentSourceOffset;
			var currentSourceRemains = this._currentSourceRemains;
			var currentSourceIndex = this._currentSourceIndex;

			if ( !this.ShiftSourceIfNeeded( ref currentSource, ref currentSourceOffset, ref currentSourceRemains, ref currentSourceIndex ) )
			{
				return false;
			}
			
			while ( remaining > 0 )
			{
				if ( remaining <= currentSourceRemains )
				{
					this._currentSourceIndex = currentSourceIndex;
					this._currentSourceOffset = currentSourceOffset + unchecked( ( int )remaining );
					this._currentSourceRemains = currentSourceRemains - unchecked( ( int )remaining );
					return true;
				}

				remaining -= currentSourceRemains;
				// try shift to next buffer
				currentSourceIndex++;
				if ( this._sources.Count == currentSourceIndex )
				{
					break;
				}

				currentSource = this._sources[ currentSourceIndex ].Array;
				currentSourceOffset = this._sources[ currentSourceIndex ].Offset;
				currentSourceRemains = this._sources[ currentSourceIndex ].Count;
			}

			return false;
		}

#if FEATURE_TAP

		public override Task<bool> DrainAsync( uint size, CancellationToken cancellationToken )
		{
			return Task.FromResult( this.Drain( size ) );
		}

#endif // FEATURE_TAP

		private void ThrowEofException( long reading )
		{
			throw new InvalidMessagePackStreamException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Data source unexpectedly ends. Cannot read {0:#,0} bytes at offset {1:#,0}, buffer index {2}.",
					reading,
					this._currentSourceOffset,
					this._currentSourceIndex
				)
			);
		}

		private void ThrowBadUtf8Exception()
		{
			throw new InvalidMessagePackStreamException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Data source has invalid UTF-8 sequence. Last code point at offset {1:#,0}, buffer index {2} is not completed.",
					this._currentSourceOffset,
					this._currentSourceIndex
				)
			);
		}
	}
}
