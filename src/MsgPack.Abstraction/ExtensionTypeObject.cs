// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Text;
using MsgPack.Internal;

namespace MsgPack
{
	/// <summary>
	///		Represents extension type object data.
	/// </summary>
	/// <remarks>
	///		Valid range and sementics of <see cref="Type"/> depend on serialization codec.
	///		In addition, some codec does not support extension type at all.
	/// </remarks>
	public readonly struct ExtensionTypeObject : IEquatable<ExtensionTypeObject>
	{
		/// <summary>
		///		Gets a type of this extension type data.
		/// </summary>
		/// <value>Codec specific type of this extension type data.</value>
		public ExtensionType Type { get; }

		/// <summary>
		///		Gets byte sequence which is body of this extension type data.
		/// </summary>
		/// <value>Byte sequence which is body of this extension type data.</value>
		public ReadOnlySequence<byte> Body { get; }

		/// <summary>
		///		Initializes a new <see cref="ExtensionTypeObject"/> instance.
		/// </summary>
		/// <param name="type">Codec specific type of this extension type data.</param>
		/// <param name="body">Byte sequence which is body of this extension type data.</param>
		public ExtensionTypeObject(ExtensionType type, ReadOnlySequence<byte> body)
		{
			this.Type = type;
			this.Body = body;
		}

		public override string ToString()
		{
			const int tokensLength = 3 + 8 + 3 + 20 + 3 + 4 + 4 + 3;
			var buffer = StringBuilderCache.Acquire((int)Math.Min(0x2000 + tokensLength, tokensLength + this.Body.Length * 2));
			try
			{
				this.ToString(buffer, false);
				return buffer.ToString();
			}
			finally
			{
				StringBuilderCache.Release(buffer);
			}
		}

		internal void ToString(StringBuilder buffer, bool isJson)
		{
			if (isJson)
			{
				buffer.Append("{ \"TypeCode\": ").Append(this.Type.Tag).Append(", \"Body\": \"");
				
				var body = this.Body.Slice(0, 0x2000);
	
				const int byteSpanLength = 96;
				const int charSpanLength = byteSpanLength / 3 * 4;
				Span<char> charSpan = stackalloc char[charSpanLength];
				Span<byte> byteSpan = stackalloc byte[byteSpanLength];
				while (!body.IsEmpty)
				{
					body.CopyTo(byteSpan);
					var byteLength = (int)Math.Min(body.Length, byteSpanLength);
					Convert.TryToBase64Chars(byteSpan.Slice(0, byteLength), charSpan, out var charsWritten, Base64FormattingOptions.None);
					buffer.Append(charSpan.Slice(0, charsWritten));
					body = body.Slice(byteLength);
				}

				buffer.Append("\" }");
			}
			else
			{
				buffer.Append("[").Append(this.Type.Tag).Append("]0x");
				var body = this.Body.Slice(0, 0x5000000);
				while (!body.IsEmpty)
				{
					Binary.ToHexString(body.FirstSpan, buffer);
					body = body.Slice(body.First.Length);
				}
			}
		}


		/// <inheritdoc />
		public override bool Equals(object? obj)
			=> obj is ExtensionTypeObject other ? this.Equals(other) : false;

		/// <inheritdoc />
		public bool Equals(ExtensionTypeObject other)
		{
			if (this.Type != other.Type)
			{
				return false;
			}

			if (this.Body.Length != other.Body.Length)
			{
				return false;
			}

			if (this.Body.IsSingleSegment && other.Body.IsSingleSegment)
			{
				return this.Body.FirstSpan.SequenceEqual(other.Body.FirstSpan);
			}

			var thisBody = this.Body;
			var otherBody = other.Body;
			do
			{
				var length = Math.Min(thisBody.FirstSpan.Length, otherBody.FirstSpan.Length);
				var left = thisBody.FirstSpan.Slice(0, length);
				var right = otherBody.FirstSpan.Slice(0, length);
				if (!left.SequenceEqual(right))
				{
					return false;
				}

				thisBody = thisBody.Slice(length);
				otherBody = otherBody.Slice(length);			
			} while (!thisBody.IsEmpty);

			Debug.Assert(otherBody.IsEmpty);

			return true;
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			var hashCode = this.Type.GetHashCode();
			var body = this.Body;
			while(!body.IsEmpty)
			{
				hashCode ^= FarmHash.Hash32WithSeed(body.FirstSpan, FarmHash.DefaultSeed);
				body = body.Slice(body.FirstSpan.Length);
			}

			return hashCode;
		}

		/// <summary>
		///		Determines whether two specified instances of <see cref="ExtensionTypeObject"/> are equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="left"/> and <paramref name="right"/> represent the same type; otherwise, <c>false</c>.
		///	</returns>
		public static bool operator ==(ExtensionTypeObject left, ExtensionTypeObject right)
			=> left.Equals(right);

		/// <summary>
		///		Determines whether two specified instances of <see cref="ExtensionTypeObject"/> are not equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="left"/> and <paramref name="right"/> represent the different type; otherwise, <c>false</c>.
		///	</returns>
		public static bool operator !=(ExtensionTypeObject left, ExtensionTypeObject right)
			=> !left.Equals(right);
	}
}
