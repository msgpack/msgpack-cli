// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
#if FEATURE_CODE_ACCESS_SECURITY
using System.Security;
#endif // FEATURE_CODE_ACCESS_SECURITY
using System.Text;
using System.Threading;
using MsgPack.Internal;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Scope = "member", Target = "MsgPack.MessagePackString.#.cctor()", Justification = "Just create as marker")]

namespace MsgPack
{
	// Dictionary based approach is better from memory usage and stability.
	/// <summary>
	///		Encapselates <see cref="String"/> and its serialized UTF-8 bytes.
	/// </summary>
#if FEATURE_BINARY_SERIALIZATION
#warning TODO: Enable in .NET Standard 2.0 in Changes.txt
	[Serializable]
#endif // FEATURE_BINARY_SERIALIZATION
#if FEATURE_CODE_ACCESS_SECURITY
	[SecuritySafeCritical]
#endif // FEATURE_CODE_ACCESS_SECURITY
	[DebuggerDisplay("{DebuggerDisplayString}")]
	[DebuggerTypeProxy(typeof(MessagePackStringDebuggerProxy))]
	internal sealed class MessagePackString
	{
		// marker to indicate this is definitively binary.
		private static readonly DecoderFallbackException IsBinary = new DecoderFallbackException("This value is not string.");
		private ReadOnlyMemory<byte> _encoded;
		private string? _decoded;
		private DecoderFallbackException? _decodingError;
		private BinaryType _type;

		// ReSharper disable once UnusedMember.Local
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For debugger")]
		private string DebuggerDisplayString
		{
			get { return new MessagePackStringDebuggerProxy(this).Value; }
		}

		public MessagePackString(string decoded)
		{
			Debug.Assert(decoded != null, "decoded != null");

			this._decoded = decoded;
			this._type = BinaryType.String;
		}

		public MessagePackString(ReadOnlyMemory<char> decoded)
		{
			this._decoded = decoded.ToString();
			this._type = BinaryType.String;
		}

		public MessagePackString(ReadOnlyMemory<byte> encoded, bool isBinary)
		{
			this._encoded = encoded;
			this._type = isBinary ? BinaryType.Blob : BinaryType.Unknown;
			if (isBinary)
			{
				this._decodingError = IsBinary;
			}
		}

		// Copy constructor for debugger proxy
		private MessagePackString(MessagePackString other)
		{
			this._encoded = other._encoded;
			this._decoded = other._decoded;
			this._decodingError = other._decodingError;
			this._type = other._type;
		}

		private void EncodeIfNeeded()
		{
			if (this._decoded == null)
			{
				return;
			}

			if (this._decoded.Length == 0)
			{
				return;
			}

			if (this._encoded.Length > 0)
			{
				// Already encoded
				return;
			}

			this._encoded = Utf8EncodingNonBom.Instance.GetBytes(this._decoded);
		}

		private void DecodeIfNeeded()
		{
			if (this._type != BinaryType.Unknown)
			{
				return;
			}

			try
			{
				this._decoded = Utf8EncodingNonBomStrict.Instance.GetString(this._encoded.Span);
				this._type = BinaryType.String;
			}
			catch (DecoderFallbackException ex)
			{
				this._decodingError = ex;
				this._type = BinaryType.Blob;
			}
		}

		public string? TryGetString()
		{
			this.DecodeIfNeeded();
			return this._decoded;
		}

		public string? GetString()
		{
			this.DecodeIfNeeded();
			if (this._decodingError != null)
			{
				throw new InvalidOperationException("This bytes is not UTF-8 string.", this._decodingError == IsBinary ? default(Exception) : this._decodingError);
			}

			return this._decoded;
		}

		public ReadOnlyMemory<char> GetCharMemory()
			=> (this.GetString()?.AsMemory()).GetValueOrDefault();

		public ReadOnlyMemory<byte> UnsafeGetMemory() => this._encoded;

		public string? UnsafeGetString() => this._decoded;

		public byte[] GetByteArray()
		{
			this.EncodeIfNeeded();
			return this._encoded.ToArray();
		}

		public ReadOnlyMemory<byte> GetByteMemory()
		{
			this.EncodeIfNeeded();
			return this._encoded;
		}

		public Type GetUnderlyingType()
		{
			this.DecodeIfNeeded();
			return this._type == BinaryType.String ? typeof(string) : typeof(byte[]);
		}

		public override string ToString()
		{
			if (this._decoded != null)
			{
				return this._decoded;
			}

			return Binary.ToHexString(this._encoded.Span);
		}

		public override int GetHashCode()
		{
			this.EncodeIfNeeded();
			return FarmHash.Hash32WithSeed(this._encoded.Span, FarmHash.DefaultSeed);
		}

		public override bool Equals(object? obj)
		{
			if (!(obj is MessagePackString other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			if (this._type == other._type && this._type == BinaryType.String)
			{
				// Compare between string and string.
				return this._decoded == other._decoded;
			}

			this.EncodeIfNeeded();
			other.EncodeIfNeeded();

			return this._encoded.Span.SequenceEqual(other._encoded.Span);
		}

#if !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
		[Serializable]
#endif // !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
		private enum BinaryType
		{
			Unknown = 0,
			String,
			Blob
		}

		internal sealed class MessagePackStringDebuggerProxy
		{
			private readonly MessagePackString _target;
			private string? _asByteArray;

			public MessagePackStringDebuggerProxy(MessagePackString target)
			{
				this._target = new MessagePackString(target);
			}

			public string Value
			{
				get
				{
					var asByteArray = Interlocked.CompareExchange(ref this._asByteArray, null, null);
					if (asByteArray != null)
					{
						return asByteArray;
					}

					switch (this._target._type)
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

			public string? AsString
			{
				get
				{
					var value = this._target.TryGetString();
					if (value == null)
					{
						return null;
					}

					if (!MustBeString(value))
					{
						this.CreateByteArrayString();
						return null;
					}

					return value;
				}
			}

			private static bool MustBeString(string value)
			{
				for (int i = 0; i < 128 && i < value.Length; i++)
				{
					var c = value[i];
					if (c < 0x20 && (c != 0x9 && c != 0xA && c != 0xD))
					{
						return false;
					}
					else if (0x7E < c && c < 0xA0)
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
					var value = Interlocked.CompareExchange(ref this._asByteArray, null, null);
					if (value == null)
					{
						value = this.CreateByteArrayString();
					}

					return value;
				}
			}

			private string CreateByteArrayString()
			{
				var bytes = this._target.GetByteMemory();
				var buffer = new StringBuilder((bytes.Length <= 128 ? bytes.Length * 3 : 128 * 3 + 3) + 4);
				buffer.Append('[');

				foreach (var b in bytes.Slice(0, 128).Span)
				{
					buffer.Append(' ');
					buffer.Append(b.ToString("X2", CultureInfo.InvariantCulture));
				}
				buffer.Append(" ]");

				var value = buffer.ToString();
				Interlocked.Exchange(ref this._asByteArray, value);
				return value;
			}
		}
	}
}
