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
using System.IO;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file was generated from Packer.Leaf.tt and Core.ttinclude T4Template.
	// Do not modify this file. Edit Packer.Leaf.tt and Core.ttinclude instead.


	partial class DefaultStreamPacker
	{
		protected override void PackCore( bool value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( byte value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( short value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( int value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( long value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( sbyte value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( ushort value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( uint value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( ulong value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( float value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( double value )
		{
			this._core.Pack( value );
		}

		protected override void PackRawCore( string value )
		{
			this._core.PackRaw( value );
		}

		protected override void PackRawCore( byte[] value )
		{
			this._core.PackRaw( value );
		}

		protected override void PackBinaryCore( byte[] value )
		{
			this._core.PackBinary( value );
		}

		protected override void PackArrayHeaderCore( int count )
		{
			this._core.PackArrayHeader( unchecked( ( uint )count ) );
		}

		protected override void PackMapHeaderCore( int count )
		{
			this._core.PackMapHeader( unchecked( ( uint )count ) );
		}

		protected override void PackBinaryHeaderCore( int count )
		{
			this._core.PackBinaryHeader( unchecked( ( uint )count ) );
		}

		protected override void PackStringHeaderCore( int count )
		{
			this._core.PackStringHeader( unchecked( ( uint )count ) );
		}

		protected override void PackExtendedTypeValueCore( byte typeCode, byte[] body )
		{
			this._core.PackExtendedTypeValue( typeCode, body );
		}

#if FEATURE_TAP

		protected override Task PackAsyncCore( bool value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( byte value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( short value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( int value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( long value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( sbyte value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( ushort value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( uint value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( ulong value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( float value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( double value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackRawAsyncCore( string value, CancellationToken cancellationToken )
		{
			return this._core.PackRawAsync( value, cancellationToken );
		}

		protected override Task PackRawAsyncCore( byte[] value, CancellationToken cancellationToken )
		{
			return this._core.PackRawAsync( value, cancellationToken );
		}

		protected override Task PackBinaryAsyncCore( byte[] value, CancellationToken cancellationToken )
		{
			return this._core.PackBinaryAsync( value, cancellationToken );
		}

		protected override Task PackArrayHeaderAsyncCore( int count, CancellationToken cancellationToken )
		{
			return this._core.PackArrayHeaderAsync( unchecked( ( uint )count ), cancellationToken );
		}

		protected override Task PackMapHeaderAsyncCore( int count, CancellationToken cancellationToken )
		{
			return this._core.PackMapHeaderAsync( unchecked( ( uint )count ), cancellationToken );
		}

		protected override Task PackBinaryHeaderAsyncCore( int count, CancellationToken cancellationToken )
		{
			return this._core.PackBinaryHeaderAsync( unchecked( ( uint )count ), cancellationToken );
		}

		protected override Task PackStringHeaderAsyncCore( int count, CancellationToken cancellationToken )
		{
			return this._core.PackStringHeaderAsync( unchecked( ( uint )count ), cancellationToken );
		}

		protected override Task PackExtendedTypeValueAsyncCore( byte typeCode, byte[] body, CancellationToken cancellationToken )
		{
			return this._core.PackExtendedTypeValueAsync( typeCode, body, cancellationToken );
		}

#endif // FEATURE_TAP
	}

	partial class DefaultByteArrayPacker
	{
		protected override void PackCore( bool value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( byte value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( short value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( int value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( long value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( sbyte value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( ushort value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( uint value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( ulong value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( float value )
		{
			this._core.Pack( value );
		}

		protected override void PackCore( double value )
		{
			this._core.Pack( value );
		}

		protected override void PackRawCore( string value )
		{
			this._core.PackRaw( value );
		}

		protected override void PackRawCore( byte[] value )
		{
			this._core.PackRaw( value );
		}

		protected override void PackBinaryCore( byte[] value )
		{
			this._core.PackBinary( value );
		}

		protected override void PackArrayHeaderCore( int count )
		{
			this._core.PackArrayHeader( unchecked( ( uint )count ) );
		}

		protected override void PackMapHeaderCore( int count )
		{
			this._core.PackMapHeader( unchecked( ( uint )count ) );
		}

		protected override void PackBinaryHeaderCore( int count )
		{
			this._core.PackBinaryHeader( unchecked( ( uint )count ) );
		}

		protected override void PackStringHeaderCore( int count )
		{
			this._core.PackStringHeader( unchecked( ( uint )count ) );
		}

		protected override void PackExtendedTypeValueCore( byte typeCode, byte[] body )
		{
			this._core.PackExtendedTypeValue( typeCode, body );
		}

#if FEATURE_TAP

		protected override Task PackAsyncCore( bool value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( byte value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( short value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( int value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( long value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( sbyte value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( ushort value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( uint value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( ulong value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( float value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackAsyncCore( double value, CancellationToken cancellationToken )
		{
			return this._core.PackAsync( value, cancellationToken );
		}

		protected override Task PackRawAsyncCore( string value, CancellationToken cancellationToken )
		{
			return this._core.PackRawAsync( value, cancellationToken );
		}

		protected override Task PackRawAsyncCore( byte[] value, CancellationToken cancellationToken )
		{
			return this._core.PackRawAsync( value, cancellationToken );
		}

		protected override Task PackBinaryAsyncCore( byte[] value, CancellationToken cancellationToken )
		{
			return this._core.PackBinaryAsync( value, cancellationToken );
		}

		protected override Task PackArrayHeaderAsyncCore( int count, CancellationToken cancellationToken )
		{
			return this._core.PackArrayHeaderAsync( unchecked( ( uint )count ), cancellationToken );
		}

		protected override Task PackMapHeaderAsyncCore( int count, CancellationToken cancellationToken )
		{
			return this._core.PackMapHeaderAsync( unchecked( ( uint )count ), cancellationToken );
		}

		protected override Task PackBinaryHeaderAsyncCore( int count, CancellationToken cancellationToken )
		{
			return this._core.PackBinaryHeaderAsync( unchecked( ( uint )count ), cancellationToken );
		}

		protected override Task PackStringHeaderAsyncCore( int count, CancellationToken cancellationToken )
		{
			return this._core.PackStringHeaderAsync( unchecked( ( uint )count ), cancellationToken );
		}

		protected override Task PackExtendedTypeValueAsyncCore( byte typeCode, byte[] body, CancellationToken cancellationToken )
		{
			return this._core.PackExtendedTypeValueAsync( typeCode, body, cancellationToken );
		}

#endif // FEATURE_TAP
	}
}
