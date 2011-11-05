#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;

namespace MsgPack
{
	// Dictionary based approach is better from memory usage and stability.
	/// <summary>
	///		Encapselates <see cref="String"/> and its serialized UTF-8 bytes.
	/// </summary>
	internal sealed class MessagePackString
	{
		// TODO: CLOB support?
		private byte[] _encoded;
		private string _decoded;
		private BinaryType _type;

		public MessagePackString( string decoded )
		{
			this._decoded = decoded;
			this._type = BinaryType.String;
		}

		public MessagePackString( byte[] encoded )
		{
			this._encoded = encoded;
		}

		private void EncodeIfNeeded()
		{
			if ( this._encoded != null )
			{
				return;
			}

			if ( this._decoded == null )
			{
				return;
			}

			this._encoded = MessagePackConvert.Utf8NonBom.GetBytes( this._decoded );
		}

		private void DecodeIfNeeded()
		{
			if ( this._decoded != null )
			{
				return;
			}

			if ( this._encoded == null )
			{
				return;
			}

			if ( this._type != BinaryType.Unknwon )
			{
				return;
			}

			try
			{
				this._decoded = MessagePackConvert.DecodeStringStrict( this._encoded );
				this._type = BinaryType.String;
			}
			catch ( DecoderFallbackException )
			{
				this._type = BinaryType.Blob;
			}
		}

		public string TryGetString()
		{
			this.DecodeIfNeeded();
			return this._decoded;
		}

		public byte[] UnsafeGetBuffer()
		{
			return this._encoded;
		}

		public byte[] GetBytes()
		{
			this.EncodeIfNeeded();
			return this._encoded;
		}

		public Type GetUnderlyingType()
		{
			this.DecodeIfNeeded();
			return this._type == BinaryType.String ? typeof( string ) : typeof( byte[] );
		}

		public sealed override string ToString()
		{
			if ( this._decoded != null )
			{
				return this._decoded;
			}

			if ( this._encoded != null )
			{
				// FIXME: hex
				return "<BLOB[" + this._encoded.Length + "]>";
			}

			return String.Empty;
		}

		public sealed override int GetHashCode()
		{
			if ( this._decoded != null )
			{
				return this._decoded.GetHashCode();
			}

			if ( this._encoded != null )
			{
				int hashCode = 0;
				for ( int i = 0; i < this._encoded.Length; i++ )
				{
					int value = this._encoded[ i ] << ( i % 4 ) * 8;
					hashCode ^= value;
				}

				return hashCode;
			}

			return 0;
		}

		public sealed override bool Equals( object obj )
		{
			var other = obj as MessagePackString;
			if ( Object.ReferenceEquals( obj, null ) )
			{
				return false;
			}

			if ( this._decoded != null && other._decoded != null )
			{
				return this._decoded == other._decoded;
			}

			if ( this._decoded == null && other._decoded == null )
			{
				return EqualsEncoded( this, other );
			}

			this.DecodeIfNeeded();
			other.DecodeIfNeeded();

			return this._decoded == other._decoded;
		}

		private static bool EqualsEncoded( MessagePackString left, MessagePackString right )
		{
			if ( left._encoded == null )
			{
				return right._encoded == null;
			}

			if ( left._encoded.Length == 0 )
			{
				return right._encoded.Length == 0;
			}

			if ( left._encoded.Length != right._encoded.Length )
			{
				return false;
			}

			if ( _isFastEqualsDisabled == 0 )
			{
				try
				{
					return FastEqualsShim( left._encoded, right._encoded );
				}
				catch ( SecurityException )
				{
					Interlocked.Exchange( ref _isFastEqualsDisabled, 1 );
				}
			}

			return SlowEquals( left._encoded, right._encoded );
		}

		private static bool SlowEquals( byte[] x, byte[] y )
		{
			for ( int i = 0; i < x.Length; i++ )
			{
				if ( x[ i ] != y[ i ] )
				{
					return false;
				}
			}

			return true;
		}

		private static int _isFastEqualsDisabled = 0;

		internal static bool IsFastEqualsDisabled
		{
			get { return _isFastEqualsDisabled != 0; }
		}

		[MethodImpl( MethodImplOptions.NoInlining )]
		private static bool FastEqualsShim( byte[] x, byte[] y )
		{
			if ( _isFastEqualsDisabled != 0 )
			{
				return SlowEquals( x, y );
			}

			return UnsafeFastEquals( x, y );
		}

		[MethodImpl( MethodImplOptions.NoInlining )]
		[SecuritySafeCritical]
		private static bool UnsafeFastEquals( byte[] x, byte[] y )
		{
			Contract.Assert( x != null );
			Contract.Assert( y != null );
			Contract.Assert( 0 < x.Length );
			Contract.Assert( x.Length == y.Length );

			int result;
			if ( !UnsafeNativeMethods.TryMemCmp( x, y, new UIntPtr( unchecked( ( uint )x.Length ) ), out result ) )
			{
				Interlocked.Exchange( ref _isFastEqualsDisabled, 1 );
				return SlowEquals( x, y );
			}

			return result == 0;
		}

		private enum BinaryType : byte
		{
			Unknwon = 0,
			String,
			Blob
		}
	}
}
