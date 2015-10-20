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

namespace MsgPack
{
	/// <summary>
	///		Represents raw binary as read only <see cref="Stream"/>.
	/// </summary>
	/// <remarks>
	///		<para>
	///			This object behaves as wrapper of the underlying <see cref="Stream"/> which contains message pack encoded byte array.
	///			But, this object does not own the stream, so that stream is not closed when this stream is closed.
	///		</para>
	///		<para>
	///			The value of <see cref="M:Stream.CanSeek"/>, timeout, and async API depends on the underlying stream.
	///		</para>
	/// </remarks>
	public abstract class UnpackingStream : Stream
	{
		internal readonly Stream Underlying;
		internal readonly long RawLength;
		internal long CurrentOffset;

		/// <summary>
		///		Gets a value indicating whether the current stream supports reading.
		/// </summary>
		/// <value>Always <c>true</c>.</value>
		public sealed override bool CanRead
		{
			get { return true; }
		}

		/// <summary>
		///		Gets a value indicating whether the current stream supports writing.
		/// </summary>
		/// <value>Always <c>false</c>.</value>
		public sealed override bool CanWrite
		{
			get { return false; }
		}

		/// <summary>
		///		Gets the length in bytes of the stream.
		/// </summary>
		/// <value>
		///		A long value representing the length of the raw binary length.
		///		This value must be between 0 and <see cref="Int32.MaxValue"/>.
		///	</value>
		/// <exception cref="T:System.ObjectDisposedException">
		///		Methods were called after the stream was closed.
		///	</exception>
		///	<remarks>
		///		This property never throws <see cref="NotSupportedException"/> even if <see cref="M:Stream.CanSeek" /> is <c>false</c>.
		///	</remarks>
		public sealed override long Length
		{
			get { return this.RawLength; }
		}

		/// <summary>
		///		Gets a value that determines whether the current stream can time out.
		/// </summary>
		/// <value>
		///		A value that determines whether the current stream can time out.
		/// </value>
		/// <exception cref="T:System.ObjectDisposedException">
		///		Methods were called after the stream was closed.
		///	</exception>
		public sealed override bool CanTimeout
		{
			get { return this.Underlying.CanTimeout; }
		}

		internal UnpackingStream( Stream underlying, long rawLength )
		{
			this.Underlying = underlying;
			this.RawLength = rawLength;
		}

		/// <summary>
		///		Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
		/// </summary>
		/// <param name="buffer">
		///		An array of bytes. When this method returns, 
		///		the buffer contains the specified byte array with the values between <paramref name="offset"/> and ( <paramref name="offset"/> + <paramref name="count"/> - 1) 
		///		replaced by the bytes read from the current source. 
		///	</param>
		/// <param name="offset">
		///		The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.
		/// </param>
		/// <param name="count">
		///		The maximum number of bytes to be read from the current stream.
		/// </param>
		/// <returns>
		///		The total number of bytes read into the buffer. 
		///		This can be less than the number of bytes requested if that many bytes are not currently available,
		///		or zero (0) if the end of the stream has been reached.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">
		///		The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length.
		///	</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///		<paramref name="buffer"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///		<paramref name="offset"/> or <paramref name="count"/> is negative. 
		///	</exception>
		/// <exception cref="T:System.IO.IOException">
		///		An I/O error occurs.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///		Methods were called after the stream was closed.
		///	</exception>
		/// <remarks>
		///		<note>
		///			Arguments might be passed to the underlying <see cref="Stream"/> without any validation.
		///		</note>
		/// </remarks>
		public sealed override int Read( byte[] buffer, int offset, int count )
		{
			if ( count < 0 )
			{
				throw new ArgumentOutOfRangeException( "count" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			if ( this.CurrentOffset == this.RawLength )
			{
				return 0;
			}

			int realCount = count;
			var exceeds = this.CurrentOffset + count - this.RawLength;
			if ( exceeds > 0 )
			{
				realCount -= checked( ( int )exceeds );
			}

			var readCount = this.Underlying.Read( buffer, offset, realCount );
			this.CurrentOffset += readCount;
			return readCount;
		}

		/// <summary>
		///		Overrides <see cref="M:Stream.Flush()"/> so that no action is performed. 
		/// </summary>
		public sealed override void Flush()
		{
			// nop
		}

		/// <summary>
		///		Throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="value">Never used.</param>
		/// <exception cref="T:System.NotSupportedException">
		///		Always thrown.
		/// </exception>
		public sealed override void SetLength( long value )
		{
			throw new NotSupportedException();
		}

		/// <summary>
		///		Throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="buffer">Never used.</param>
		/// <param name="offset">Never used.</param>
		/// <param name="count">Never used.</param>
		/// <exception cref="T:System.NotSupportedException">
		///		Always thrown.
		/// </exception>
		public sealed override void Write( byte[] buffer, int offset, int count )
		{
			throw new NotSupportedException();
		}
	}
}
