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
using System.IO;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		<see cref="PackerWriter"/> for <see cref="Stream"/>.
	/// </summary>
	internal sealed partial class StreamPackerWriter : PackerWriter
	{
		private readonly Stream _destination;
		private readonly bool _ownsStream;
		private readonly byte[] _scalarBuffer;

#if DEBUG
		internal Stream Destination { get { return this._destination; } }
#endif // DEBUG

		public StreamPackerWriter( Stream stream, PackerUnpackerStreamOptions streamOptions )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			var options = streamOptions ?? PackerUnpackerStreamOptions.None;
			this._destination = options.WrapStream( stream );
			this._ownsStream = options.OwnsStream;
			this._scalarBuffer = new byte[ sizeof( ulong ) + 1 ];
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing && this._ownsStream )
			{
				this._destination.Dispose();
			}

			base.Dispose( disposing );
		}

		public void Flush()
		{
			this._destination.Flush();
		}

		public override void WriteByte( byte value )
		{
			this._destination.WriteByte( value );
		}

		public override void WriteBytes( byte[] value )
		{
			this.WriteBytes( value, 0, value.Length );
		}

		private void WriteBytes( byte[] value, int startIndex, int count )
		{
			this._destination.Write( value, startIndex, count );
		}

#if FEATURE_TAP

		public Task FlushAsync( CancellationToken cancellationToken )
		{
			return this._destination.FlushAsync( cancellationToken );
		}

		public override Task WriteByteAsync( byte value, CancellationToken cancellationToken )
		{
			this._scalarBuffer[ 0 ] = value;
			return this._destination.WriteAsync( this._scalarBuffer, 0, sizeof( byte ), cancellationToken );
		}

		public override Task WriteBytesAsync( byte[] value, CancellationToken cancellationToken )
		{
			return this.WriteBytesAsync( value, 0, value.Length, cancellationToken );
		}

		private Task WriteBytesAsync( byte[] value, int startIndex, int count, CancellationToken cancellationToken )
		{
			return this._destination.WriteAsync( value, startIndex, count );
		}

#endif // FEATURE_TAP
	}
}
