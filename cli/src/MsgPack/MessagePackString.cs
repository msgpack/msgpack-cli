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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace MsgPack
{
#warning Use Dictionary Based approach
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
				using ( var hash =
#if SILVERLIGHT
 new SHA1Managed()
#else
 new SHA1Cng()
#endif
 )
				{
					// TODO: caching etc.
					var hash128 = hash.ComputeHash( this._encoded );
					return
						( hash128[ 0 ] << 24 | hash128[ 1 ] << 16 | hash128[ 2 ] << 8 | hash128[ 3 ] )
						^ ( hash128[ 4 ] << 24 | hash128[ 5 ] << 16 | hash128[ 6 ] << 8 | hash128[ 7 ] )
						^ ( hash128[ 8 ] << 24 | hash128[ 9 ] << 16 | hash128[ 10 ] << 8 | hash128[ 11 ] )
						^ ( hash128[ 12 ] << 24 | hash128[ 13 ] << 16 | hash128[ 14 ] << 8 | hash128[ 15 ] )
						^ ( hash128[ 16 ] << 16 | hash128[ 17 ] );
				}
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

			if ( left._encoded.Length != right._encoded.Length )
			{
				return false;
			}

#if SILVERLIGHT
			for ( int i = 0; i < left._encoded.Length; i++ )
#else
			for ( long i = 0; i < left._encoded.LongLength; i++ )
#endif
			{
				if ( left._encoded[ i ] != right._encoded[ i ] )
				{
					return false;
				}
			}

			return true;
		}

		private enum BinaryType : byte
		{
			Unknwon = 0,
			String,
			Blob
		}

	}
}
