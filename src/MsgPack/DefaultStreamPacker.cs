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
	///		Public interface for stream based MessagePack packer.
	/// </summary>
	internal sealed partial class DefaultStreamPacker : Packer
	{
		private readonly MessagePackPacker<StreamPackerWriter> _core;

		public MessagePackPacker<StreamPackerWriter> Core
		{
			get { return this._core; }
		}

		public DefaultStreamPacker( Stream stream, PackerUnpackerStreamOptions streamOptions, PackerCompatibilityOptions compatibilityOptions )
			: base( compatibilityOptions )
		{
			this._core = new MessagePackPacker<StreamPackerWriter>( new StreamPackerWriter( stream, streamOptions ), compatibilityOptions );
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				this._core.Dispose();
			}

			base.Dispose( disposing );
		}

		public override void Flush()
		{
			this.Core.Writer.Flush();
		}

		protected override void WriteByte( byte value )
		{
			this._core.Writer.WriteByte( value );
		}

		protected override void WriteBytes( byte[] value, bool isImmutable )
		{
			this._core.Writer.WriteBytes( value );
		}

		protected override void WriteBytes( ICollection<byte> value )
		{
			this._core.Writer.WriteBytes( value as byte[] ?? value.ToArray() );
		}

#if FEATURE_TAP

		public override Task FlushAsync( CancellationToken cancellationToken )
		{
			return this.Core.Writer.FlushAsync( cancellationToken );
		}

		protected override Task WriteByteAsync( byte value, CancellationToken cancellationToken )
		{
			return this._core.Writer.WriteByteAsync( value, cancellationToken );
		}

		protected override Task WriteBytesAsync( byte[] value, bool isImmutable, CancellationToken cancellationToken )
		{
			return this._core.Writer.WriteBytesAsync( value, cancellationToken );
		}

		protected override Task WriteBytesAsync( ICollection<byte> value, CancellationToken cancellationToken )
		{
			return this._core.Writer.WriteBytesAsync( value as byte[] ?? value.ToArray(), cancellationToken );
		}

#endif // FEATURE_TAP
	}
}
