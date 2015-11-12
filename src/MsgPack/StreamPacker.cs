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
using System.IO;

namespace MsgPack
{
	/// <summary>
	///		Basic <see cref="Packer"/> implementation using managed <see cref="Stream"/>.
	/// </summary>
	internal class StreamPacker : Packer
	{
		private readonly Stream _stream;
		private readonly bool _ownsStream;

		public sealed override bool CanSeek
		{
			get { return this._stream.CanSeek; }
		}

		public sealed override long Position
		{
			get { return this._stream.Position; }
		}

		public StreamPacker( Stream stream, PackerCompatibilityOptions compatibilityOptions, PackerUnpackerStreamOptions streamOptions )
			: base( compatibilityOptions )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			var options = streamOptions ?? PackerUnpackerStreamOptions.None;

			this._stream = options.WrapStream( stream );

			this._ownsStream = options.OwnsStream;
		}

		protected sealed override void Dispose( bool disposing )
		{
			if ( this._ownsStream )
			{
				this._stream.Dispose();
			}

			base.Dispose( disposing );
		}

		protected sealed override void SeekTo( long offset )
		{
			if ( !this.CanSeek )
			{
				ThrowCannotSeekException();
			}

			this._stream.Seek( offset, SeekOrigin.Current );
		}

		protected sealed override void WriteByte( byte value )
		{
			this._stream.WriteByte( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected sealed override void WriteBytes( byte[] asArray, bool isImmutable )
		{
			this._stream.Write( asArray, 0, asArray.Length );
		}


		private static void ThrowCannotSeekException()
		{
			throw new NotSupportedException("The underlying stream does not support seeking.");
		}
	}
}
