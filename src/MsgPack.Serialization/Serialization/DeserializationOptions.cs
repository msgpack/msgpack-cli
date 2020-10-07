// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Text;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines each deserialization operations level options. This object is immutable.
	/// </summary>
	/// <seealso cref="DeserializationOptionsBuilder"/>
	public sealed class DeserializationOptions
	{
		public static DeserializationOptions Default { get; } = new DeserializationOptionsBuilder().Create();

		/// <summary>
		///		Gets the allowed maximum length of the serialized array.
		/// </summary>
		/// <value>
		///		The allowed maximum length of the serialized array. Default is <c>1,048,576</c> (<c>1Mi</c>).
		/// </value>
		public int MaxArrayLength { get; }

		/// <summary>
		///		Gets the allowed maximum count of the serialized map entries.
		/// </summary>
		/// <value>
		///		The allowed maximum count of the serialized map entries. Default is <c>1,048,576</c> (<c>1Mi</c>).
		/// </value>
		public int MaxMapCount { get; }

		/// <summary>
		///		Gets the allowed maximum length of each serialized map keys.
		/// </summary>
		/// <value>
		///		The allowed maximum length of each serialized map keys. Default is <c>256</c>.
		/// </value>
		public int MaxPropertyKeyLength { get; }

		/// <summary>
		///		Gets the allowed maximum depth of the serialized object tree.
		/// </summary>
		/// <value>
		///		The allowed maximum depth of the serialized object tree. Default is <c>100</c>.
		/// </value>
		public int MaxDepth { get; }

		/// <summary>
		///		Gets the default string encoding for string members or map keys which are not specified explicitly.
		/// </summary>
		/// <value>
		///		The default string encoding for string members or map keys which are not specified explicitly.
		///		Default value is <c>null</c>, which means using default encoding of underlying codec.
		/// </value>
		public Encoding? DefaultStringEncoding { get; }

		/// <summary>
		///		Gets the <see cref="ArrayPool{T}"/> of <see cref="System.Byte"/> which will be used as buffer for codec.
		/// </summary>
		/// <value>
		///		The <see cref="ArrayPool{T}"/> of <see cref="System.Byte"/> which will be used as buffer for codec.
		/// </value>
		public ArrayPool<byte> ByteBufferPool { get; }

		/// <summary>
		///		Gets the value which indicates whether the buffer from <see cref="ByteBufferPool"/> should be cleared on return.
		/// </summary>
		/// <value>
		///		<c>true</c>, if the buffer should be zero-cleared; <c>false</c>, otherwise. The default is <c>false</c>.
		/// </value>
		/// <remarks>
		///		If <c>false</c>, a malicious serializer can snoof buffer contents, so it might be security vulnerability.
		///		If <c>true</c>, security is improved, but significant performance hit may be occurred for large buffer.
		/// </remarks>
		public bool ClearsByteBufferOnReturn { get; }

		internal DeserializationOptions(DeserializationOptionsBuilder builder)
		{
			this.MaxArrayLength = builder.MaxArrayLength;
			this.MaxMapCount = builder.MaxMapCount;
			this.MaxPropertyKeyLength = builder.MaxPropertyKeyLength;
			this.MaxDepth = builder.MaxDepth;
			this.DefaultStringEncoding = builder.DefaultStringEncoding;
			this.ByteBufferPool = builder.ByteBufferPool ?? ArrayPool<byte>.Shared;
			this.ClearsByteBufferOnReturn = builder.ClearsByteBufferOnReturn;
		}
	}
}
