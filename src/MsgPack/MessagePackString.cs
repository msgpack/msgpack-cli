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
using System.Diagnostics;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Scope = "member", Target = "MsgPack.MessagePackString.#.cctor()", Justification = "Just create as marker" )]

namespace MsgPack
{
	// Dictionary based approach is better from memory usage and stability.
	/// <summary>
	///		Encapselates <see cref="String"/> and its serialized UTF-8 bytes.
	/// </summary>
#if !SILVERLIGHT && !NETFX_CORE
	[Serializable]
#endif // if !SILVERLIGHT && !NETFX_CORE
#if !NETFX_35 && !UNITY
	[SecuritySafeCritical]
#endif // !NETFX_35 && !UNITY
	[DebuggerDisplay( "{DebuggerDisplayString}" )]
	[DebuggerTypeProxy( typeof( MessagePackStringDebuggerProxy ) )]
	internal sealed class MessagePackString
	{
		// TODO: CLOB support?
		// marker to indicate this is definitively binary.
		private static readonly DecoderFallbackException IsBinary = new DecoderFallbackException( "This value is not string." );
		private byte[] _encoded;
		private string _decoded;
		private DecoderFallbackException _decodingError;
		private BinaryType _type;

		// ReSharper disable once UnusedMember.Local
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For debugger" )]
		private string DebuggerDisplayString
		{
			get { return new MessagePackStringDebuggerProxy( this ).Value; }
		}

		public MessagePackString( string decoded )
		{
#if !UNITY
			Contract.Assert( decoded != null, "decoded != null" );
#endif // !UNITY
			this._decoded = decoded;
			this._type = BinaryType.String;
		}

		public MessagePackString( byte[] encoded, bool isBinary )
		{
#if !UNITY
			Contract.Assert( encoded != null, "encoded != null" );
#endif // !UNITY
			this._encoded = encoded;
			this._type = isBinary ? BinaryType.Blob : BinaryType.Unknwon;
			if ( isBinary )
			{
				this._decodingError = IsBinary;
			}
		}

		// Copy constructor for debugger proxy
		private MessagePackString( MessagePackString other )
		{
			this._encoded = other._encoded;
			this._decoded = other._decoded;
			this._decodingError = other._decodingError;
			this._type = other._type;
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
			catch ( DecoderFallbackException ex )
			{
				this._decodingError = ex;
				this._type = BinaryType.Blob;
			}
		}

		public string TryGetString()
		{
			this.DecodeIfNeeded();
			return this._decoded;
		}

		public string GetString()
		{
			this.DecodeIfNeeded();
			if ( this._decodingError != null )
			{
				throw new InvalidOperationException( "This bytes is not UTF-8 string.", this._decodingError == IsBinary ? default( Exception ) : this._decodingError );
			}

			return this._decoded;
		}

		public byte[] UnsafeGetBuffer()
		{
			return this._encoded;
		}

		public string UnsafeGetString()
		{
			return this._decoded;
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

		public override string ToString()
		{
			if ( this._decoded != null )
			{
				return this._decoded;
			}

			if ( this._encoded != null )
			{
				return Binary.ToHexString( this._encoded );
			}

			return String.Empty;
		}

		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyFieldInGetHashCode
			if ( this._decoded != null )
			{
				return this._decoded.GetHashCode();
			}

			this.DecodeIfNeeded();
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
			// ReSharper restore NonReadonlyFieldInGetHashCode
		}

		public override bool Equals( object obj )
		{
			var other = obj as MessagePackString;
			if ( ReferenceEquals( other, null ) )
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

#if !UNITY && !WINDOWS_PHONE && !NETFX_CORE
			if ( _isFastEqualsDisabled == 0 )
			{
				try
				{
					return UnsafeFastEquals( left._encoded, right._encoded );
				}
				catch ( SecurityException )
				{
					Interlocked.Exchange( ref _isFastEqualsDisabled, 1 );
				}
				catch ( MemberAccessException )
				{
					Interlocked.Exchange( ref _isFastEqualsDisabled, 1 );
				}
			}
#endif // if !UNITY && !WINDOWS_PHONE && !NETFX_CORE

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

#if !UNITY && !WINDOWS_PHONE && !NETFX_CORE
#if SILVERLIGHT
		private static int _isFastEqualsDisabled =
			System.Windows.Application.Current.HasElevatedPermissions ? 0 : 1;
#else
		private static int _isFastEqualsDisabled;
#endif // if SILVERLIGHT

#if DEBUG
		// for testing
		internal static bool IsFastEqualsDisabled
		{
			get { return _isFastEqualsDisabled != 0; }
		}
#endif

#if !NETFX_35 && !UNITY
		[SecuritySafeCritical]
#endif // !NETFX_35 && !UNITY
		private static bool UnsafeFastEquals( byte[] x, byte[] y )
		{
#if DEBUG
			Contract.Assert( x != null, "x != null" );
			Contract.Assert( y != null, "y != null" );
			Contract.Assert( 0 < x.Length, "0 < x.Length" );
			Contract.Assert( x.Length == y.Length, "x.Length == y.Length" );
#endif // if DEBUG
			int result;
			if ( !UnsafeNativeMethods.TryMemCmp( x, y, new UIntPtr( unchecked( ( uint )x.Length ) ), out result ) )
			{
				Interlocked.Exchange( ref _isFastEqualsDisabled, 1 );
				return SlowEquals( x, y );
			}

			return result == 0;
		}
#endif // if !UNITY && !WINDOWS_PHONE && !NETFX_CORE

#if !SILVERLIGHT && !NETFX_CORE
		[Serializable]
#endif // if !SILVERLIGHT && !NETFX_CORE
		private enum BinaryType
		{
			Unknwon = 0,
			String,
			Blob
		}

		internal sealed class MessagePackStringDebuggerProxy
		{
			private readonly MessagePackString _target;
			private string _asByteArray;

			public MessagePackStringDebuggerProxy( MessagePackString target )
			{
				this._target = new MessagePackString( target );
			}

			public string Value
			{
				get
				{
					var asByteArray = Interlocked.CompareExchange( ref this._asByteArray, null, null );
					if ( asByteArray != null )
					{
						return asByteArray;
					}

					switch ( this._target._type )
					{
						case BinaryType.Blob:
						{
							return this.AsByteArray;
						}
						case BinaryType.String:
						{
							return this.AsString ?? this.AsByteArray;
						}
						default:
						{
							this._target.DecodeIfNeeded();
							goto case BinaryType.String;
						}
					}
				}
			}

			public string AsString
			{
				get
				{
					var value = this._target.TryGetString();
					if ( value == null )
					{
						return null;
					}

					if ( !MustBeString( value ) )
					{
						this.CreateByteArrayString();
						return null;
					}

					return value;
				}
			}

			private static bool MustBeString( string value )
			{
				for ( int i = 0; i < 128 && i < value.Length; i++ )
				{
					var c = value[ i ];
					if ( c < 0x20 && ( c != 0x9 && c != 0xA && c != 0xD ) )
					{
						return false;
					}
					else if ( 0x7E < c && c < 0xA0 )
					{
						return false;
					}
				}

				return true;
			}

			public string AsByteArray
			{
				get
				{
					var value = Interlocked.CompareExchange( ref this._asByteArray, null, null );
					if ( value == null )
					{
						value = this.CreateByteArrayString();
					}

					return value;
				}
			}

			private string CreateByteArrayString()
			{
				var bytes = this._target.GetBytes();
				var buffer = new StringBuilder( ( bytes.Length <= 128 ? bytes.Length * 3 : 128 * 3 + 3 ) + 4 );
				buffer.Append( '[' );

				foreach ( var b in bytes.Take( 128 ) )
				{
					buffer.Append( ' ' );
					buffer.Append( b.ToString( "X2", CultureInfo.InvariantCulture ) );
				}
				buffer.Append( " ]" );

				var value = buffer.ToString();
				Interlocked.Exchange( ref this._asByteArray, value );
				return value;
			}
		}
	}
}
