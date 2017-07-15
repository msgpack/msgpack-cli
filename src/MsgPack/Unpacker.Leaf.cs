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

namespace MsgPack
{
	// This file was generated from Unpacker.Leaf.tt and Core.ttinclude T4Template.
	// Do not modify this file. Edit Unpacker.Leaf.tt and Core.ttinclude instead.

	internal sealed class FastStreamUnpacker : MessagePackStreamUnpacker
	{
		public FastStreamUnpacker( Stream stream, PackerUnpackerStreamOptions streamOptions )
			: base( stream, streamOptions ) { }

		protected override Unpacker ReadSubtreeCore()
		{
			this.BeginReadSubtree();
			return this;
		}
	}

	internal sealed class CollectionValidatingStreamUnpacker : MessagePackStreamUnpacker
	{
		// ReSharper disable once RedundantDefaultFieldInitializer
		private bool _isSubtreeReading = false;

		public CollectionValidatingStreamUnpacker( Stream stream, PackerUnpackerStreamOptions streamOptions )
			: base( stream, streamOptions ) { }

		internal override Unpacker InternalReadSubtree()
		{
			if ( !this.IsCollectionHeader )
			{
				ThrowCannotBeSubtreeModeException();
			}

			if ( this._isSubtreeReading )
			{
				ThrowInSubtreeModeException();
			}

			var subtreeReader = this.ReadSubtreeCore();
			this._isSubtreeReading = !ReferenceEquals( subtreeReader, this );
			return subtreeReader;
		}

		protected override Unpacker ReadSubtreeCore()
		{
			return new SubtreeUnpacker( this );
		}

		protected internal override void EndReadSubtree()
		{
			this._isSubtreeReading = false;
			base.EndReadSubtree();
		}

		internal override void EnsureNotInSubtreeMode()
		{
			this.VerifyMode( UnpackerMode.Streaming );
			if ( this._isSubtreeReading )
			{
				ThrowInSubtreeModeException();
			}
		}

		internal override void BeginSkipCore()
		{
			if ( this._isSubtreeReading )
			{
				ThrowInSubtreeModeException();
			}
		}
	}

	internal sealed class FastByteArrayUnpacker : MessagePackByteArrayUnpacker
	{
		public FastByteArrayUnpacker( byte[] source, int startOffset )
			: base( source, startOffset ) { }

		protected override Unpacker ReadSubtreeCore()
		{
			return this;
		}
	}

	internal sealed class CollectionValidatingByteArrayUnpacker : MessagePackByteArrayUnpacker
	{
		// ReSharper disable once RedundantDefaultFieldInitializer
		private bool _isSubtreeReading = false;

		public CollectionValidatingByteArrayUnpacker( byte[] source, int startOffset )
			: base( source, startOffset ) { }

		internal override Unpacker InternalReadSubtree()
		{
			if ( !this.IsCollectionHeader )
			{
				ThrowCannotBeSubtreeModeException();
			}

			if ( this._isSubtreeReading )
			{
				ThrowInSubtreeModeException();
			}

			var subtreeReader = this.ReadSubtreeCore();
			this._isSubtreeReading = !ReferenceEquals( subtreeReader, this );
			return subtreeReader;
		}

		protected override Unpacker ReadSubtreeCore()
		{
			return new SubtreeUnpacker( this );
		}

		protected internal override void EndReadSubtree()
		{
			this._isSubtreeReading = false;
			base.EndReadSubtree();
		}

		internal override void EnsureNotInSubtreeMode()
		{
			this.VerifyMode( UnpackerMode.Streaming );
			if ( this._isSubtreeReading )
			{
				ThrowInSubtreeModeException();
			}
		}

		internal override void BeginSkipCore()
		{
			if ( this._isSubtreeReading )
			{
				ThrowInSubtreeModeException();
			}
		}
	}
}