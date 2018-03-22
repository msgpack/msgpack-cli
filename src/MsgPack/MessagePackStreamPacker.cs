#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017-2018 FUJIWARA, Yusuke
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
using System.IO;
using System.Linq;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		Implementation for stream based MessagePack packer.
	/// </summary>
#if UNITY && DEBUG
	public
#else
	internal
#endif
	sealed partial class MessagePackStreamPacker : Packer
	{
		private readonly Stream _destination;
		private readonly byte[] _scalarBuffer;
		private readonly bool _ownsStream;

#if DEBUG
#if UNITY && DEBUG
		public
#else
		internal
#endif
		Stream Destination { get { return this._destination; } }
#endif // DEBUG

		public MessagePackStreamPacker( Stream stream, PackerUnpackerStreamOptions streamOptions, PackerCompatibilityOptions compatibilityOptions )
			: base( compatibilityOptions )
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

		public override void Flush()
		{
			this._destination.Flush();
		}

		protected override void WriteByte( byte value )
		{
			this._destination.WriteByte( value );
		}

		protected override void WriteBytes( byte[] value, bool isImmutable )
		{
			this.WriteBytes( value );
		}

		protected override void WriteBytes( ICollection<byte> value )
		{
			this.WriteBytes( value as byte[] ?? value.ToArray() );
		}

		private void WriteBytes( byte[] value )
		{
			this.WriteBytes( value, 0, value.Length );
		}

		private void WriteBytes( byte[] value, int startIndex, int count )
		{
			this._destination.Write( value, startIndex, count );
		}

#if FEATURE_TAP

		public override Task FlushAsync( CancellationToken cancellationToken )
		{
			return this._destination.FlushAsync( cancellationToken );
		}

		protected override Task WriteByteAsync( byte value, CancellationToken cancellationToken )
		{
			this._scalarBuffer[ 0 ] = value;
			return this._destination.WriteAsync( this._scalarBuffer, 0, sizeof( byte ), cancellationToken );
		}

		protected override Task WriteBytesAsync( byte[] value, bool isImmutable, CancellationToken cancellationToken )
		{
			return this.WriteBytesAsync( value, cancellationToken );
		}

		protected override Task WriteBytesAsync( ICollection<byte> value, CancellationToken cancellationToken )
		{
			return this.WriteBytesAsync( value as byte[] ?? value.ToArray(), cancellationToken );
		}

		private Task WriteBytesAsync( byte[] value, CancellationToken cancellationToken )
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
