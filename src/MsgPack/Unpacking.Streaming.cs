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
using System.IO;
using System.Text;

namespace MsgPack
{
	partial class Unpacking
	{
		///	<summary>
		///		Unpacks raw value from the specified <see cref="Stream"/> as <see cref="UnpackingStream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingStream"/> which represents raw value stream.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not raw binary.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		///			<see cref="UnpackingStream"/> does not own <paramref name="source"/>, so <paramref name="source"/> still should be closed.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingStream UnpackByteStream( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			return UnpackByteStreamCore( source );
		}

		private static UnpackingStream UnpackByteStreamCore( Stream source )
		{
			uint length = UnpackRawLengthCore( source );

			if ( source.CanSeek )
			{
				return new SeekableUnpackingStream( source, length );
			}
			else
			{
				return new UnseekableUnpackingStream( source, length );
			}
		}


		///	<summary>
		///		Unpacks raw value from the specified <see cref="Stream"/> as <see cref="UnpackingStreamReader"/> with UTF-8 encoding.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingStreamReader"/> which represents raw value stream as UTF-8 string.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not raw binary.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		///			if <paramref name="source"/> contains invalid sequence as UTF-8 encoding string,
		///			the <see cref="DecoderFallbackException"/> may occurs on read char.
		///		</para>
		///		<para>
		///			<see cref="UnpackingStreamReader"/> does not own <paramref name="source"/>, so <paramref name="source"/> still should be closed.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingStreamReader UnpackCharStream( Stream source )
		{
			return UnpackCharStream( source, MessagePackConvert.Utf8NonBomStrict );
		}

		///	<summary>
		///		Unpacks raw value from the specified <see cref="Stream"/> as <see cref="UnpackingStreamReader"/> with specified encoding.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<param name="encoding">The <see cref="Encoding"/> to decode binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingStreamReader"/> which represents raw value stream as UTF-8 string.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or, <paramref name="encoding"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not raw binary.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		///			if <paramref name="source"/> contains invalid sequence as specified encoding string,
		///			the <see cref="DecoderFallbackException"/> may occurs on read char.
		///		</para>
		///		<para>
		///			<see cref="UnpackingStreamReader"/> does not own <paramref name="source"/>, so <paramref name="source"/> still should be closed.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>		
		public static UnpackingStreamReader UnpackCharStream( Stream source, Encoding encoding )
		{
			ValidateStream( source );

			if ( encoding == null )
			{
				throw new ArgumentNullException( "encoding" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			var stream = UnpackByteStreamCore( source );
			return new DefaultUnpackingStreamReader( stream, encoding, stream.RawLength );
		}


		private sealed class SeekableUnpackingStream : UnpackingStream
		{
			private readonly long _initialPosition;

			public override bool CanSeek
			{
				get { return true; }
			}

			public override long Position
			{
				get { return this.CurrentOffset; }
				set { this.SeekTo( value ); }
			}

			public SeekableUnpackingStream( Stream underlying, long rawLength )
				: base( underlying, rawLength )
			{
#if !UNITY
				Contract.Assert( underlying.CanSeek, "underlying.CanSeek" );
#endif // !UNITY
				this._initialPosition = underlying.Position;
			}

			public override long Seek( long offset, SeekOrigin origin )
			{
				switch ( origin )
				{
					case SeekOrigin.Begin:
					{
						this.SeekTo( offset );
						break;
					}
					case SeekOrigin.End:
					{
						this.SeekTo( this.RawLength + offset );
						break;
					}
					case SeekOrigin.Current:
					{
						this.SeekTo( this.Position + offset );
						break;
					}
				}

				return this.Position;
			}

			private void SeekTo( long position )
			{
				if ( position < 0 )
				{
					throw new IOException( "An attempt was made to move the position before the beginning of the stream." );
				}

				if ( position > this.RawLength )
				{
					// Always thrown
					this.SetLength( position );
				}

				this.CurrentOffset = position;
				this.Underlying.Position = position + this._initialPosition;
			}
		}

		private sealed class UnseekableUnpackingStream : UnpackingStream
		{
			public override bool CanSeek
			{
				get { return false; }
			}

			public override long Position
			{
				get { throw new NotSupportedException(); }
				set { throw new NotSupportedException(); }
			}

			public UnseekableUnpackingStream( Stream underlying, long rawLength ) : base( underlying, rawLength ) { }

			public override long Seek( long offset, SeekOrigin origin )
			{
				throw new NotSupportedException();
			}
		}

		private sealed class DefaultUnpackingStreamReader : UnpackingStreamReader
		{
			public DefaultUnpackingStreamReader( Stream stream, Encoding encoding, long byteLength ) : base( stream, encoding, byteLength ) { }
		}
	}
}