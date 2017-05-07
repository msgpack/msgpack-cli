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
using System.Globalization;
using System.IO;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		<see cref="UnpackerReader"/> for <see cref="Stream"/>.
	/// </summary>
	internal sealed partial class StreamUnpackerReader : UnpackerReader
	{
		private readonly byte[] _oneByteBuffer = new byte[ 1 ];
		private readonly byte[] _scalarBuffer = new byte[ 8 ];
		private readonly Stream _source;
		private readonly bool _useStreamPosition;
		private readonly bool _ownsStream;

#if DEBUG
		internal Stream DebugSource
		{
			get { return this._source; }
		}

		internal bool DebugOwnsStream
		{
			get { return this._ownsStream; }
		}
#endif // DEBUG

		/// <summary>
		///		An position of seekable <see cref="Stream"/> or offset from start of this instance.
		/// </summary>
		private long _offset;

		public override long Offset
		{
			get { return this._offset; }
		}


		/// <summary>
		///		An position of seekable <see cref="Stream"/> or offset from start of this instance before last operation.
		/// </summary>
		private long _lastOffset;

		public override bool GetPreviousPosition( out long offsetOrPosition )
		{
			offsetOrPosition = this._lastOffset;
			return this._useStreamPosition;
		}

		public StreamUnpackerReader( Stream stream, PackerUnpackerStreamOptions streamOptions )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			var options = streamOptions ?? PackerUnpackerStreamOptions.None;
			this._source = options.WrapStream( stream );
			this._ownsStream = options.OwnsStream;
			this._useStreamPosition = stream.CanSeek;
			this._offset = this._useStreamPosition ? stream.Position : 0L;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( this._ownsStream )
				{
					this._source.Dispose();
				}
			}

			base.Dispose( disposing );
		}

		private void ThrowEofException( long reading )
		{
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );
			throw new InvalidMessagePackStreamException(
					String.Format(
						CultureInfo.CurrentCulture,
						isRealOffset
							? "Stream unexpectedly ends. Cannot read {0:#,0} bytes from stream at position {1:#,0}."
							: "Stream unexpectedly ends. Cannot read {0:#,0} bytes from stream at offset {1:#,0}.",
						reading,
						offsetOrPosition
					)
				);
		}

		private void ThrowBadUtf8Exception()
		{
			long offsetOrPosition;
			var isRealOffset = this.GetPreviousPosition( out offsetOrPosition );
			throw new InvalidMessagePackStreamException(
					String.Format(
						CultureInfo.CurrentCulture,
						isRealOffset
							? "Stream has invalid UTF-8 sequence. Last code point from stream at position {0:#,0} is not completed."
							: "Stream has invalid UTF-8 sequence. Last code point from stream at offset {0:#,0} is not completed.",
						offsetOrPosition
					)
				);
		}
	}
}
