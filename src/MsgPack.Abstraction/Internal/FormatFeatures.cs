// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Internal
{
	public sealed class FormatFeatures
	{
#warning TODO: REMOVE
		public bool IsContextful { get; }

		/// <summary>
		///		Gets a value which indicates the underlying format supports collection length.
		/// </summary>
		/// <value>
		///		<c>true</c>, if the underlyng format supports collection length; <c>false</c>, otherwise.
		/// </value>
		/// <remarks>
		///		When this property returns <c>false</c>, following methods should throw <see cref="System.NotSupportedException"/>.
		///		<list type="bullet">
		///			<item><see cref="FormatDecoder{TExtensionType}.DecodeArrayHeader(in System.Buffers.SequenceReader{System.Byte})"/></item>
		///			<item><see cref="FormatDecoder{TExtensionType}.DecodeArrayHeader(in System.Buffers.SequenceReader{System.Byte}, out System.Int32)"/></item>
		///			<item><see cref="FormatDecoder{TExtensionType}.DecodeMapHeader(in System.Buffers.SequenceReader{System.Byte})"/></item>
		///			<item><see cref="FormatDecoder{TExtensionType}.DecodeMapHeader(in System.Buffers.SequenceReader{System.Byte}, out System.Int32)"/></item>
		///			<item><see cref="FormatDecoder{TExtensionType}.DecodeArrayOrMapHeader(in System.Buffers.SequenceReader{System.Byte}, out System.Int64)"/></item>
		///			<item><see cref="FormatDecoder{TExtensionType}.DecodeArrayOrMapHeader(in System.Buffers.SequenceReader{System.Byte}, out System.Int64, out System.Int32)"/></item>
		///			<item><see cref="FormatDecoder{TExtensionType}.Drain(in System.Buffers.SequenceReader{System.Byte}, in CollectionContext, System.Int64, System.Threading.CancellationToken)"/></item>
		///			<item><see cref="FormatDecoder{TExtensionType}.Drain(in System.Buffers.SequenceReader{System.Byte}, in CollectionContext, System.Int64, out System.Int32, System.Threading.CancellationToken)"/></item>
		///		</list>
		/// </remarks>
		public bool CanCountCollectionItems { get; }

		/// <summary>
		///		Gets a value which indicates the underlying format supports custom string encoding.
		/// </summary>
		/// <value>
		///		<c>true</c>, if the underlyng format supports custom string encoding; <c>false</c>, otherwise.
		/// </value>
		/// <remarks>
		///		When this property returns <c>false</c>, <see cref="System.Text.Encoding"/> typed parameters in <see cref="Encoder{TExtensionType}"/> and <see cref="Decoder{TExtensionType}"/> methods will be ignored.
		/// </remarks>
		public bool CanSpecifyStringEncoding { get; }

		/// <summary>
		///		Gets a value which indicates the underlying format supports extension types.
		/// </summary>
		/// <value>
		///		<c>true</c>, if the underlyng format supports extension types; <c>false</c>, otherwise.
		/// </value>
		/// <remarks>
		///		When this property returns <c>false</c>, following methods should throw <see cref="System.NotSupportedException"/>.
		///		<list type="bullet">
		///			<item><see cref="FormatEncoder{TExtensionType}.EncodeExtension(TExtensionType, System.ReadOnlySpan{System.Byte}, System.Buffers.IBufferWriter{System.Byte})"/></item>
		///			<item><see cref="FormatEncoder{TExtensionType}.EncodeExtension(TExtensionType, System.Buffers.ReadOnlySequence{System.Byte}, System.Buffers.IBufferWriter{System.Byte})"/></item>
		///			<item><see cref="FormatDecoder{TExtensionType}.DecodeExtension(in System.Buffers.SequenceReader{System.Byte}, out TExtensionType, out System.Buffers.ReadOnlySequence{System.Byte}, out System.Int32, System.Threading.CancellationToken)(in System.Buffers.SequenceReader{System.Byte}, out System.Int32)"/></item>
		///		</list>
		///		In additon, <c>TExtentionType</c> type parameter should be <see cref="NullExtensionType"/>.
		/// </remarks>
		public bool SupportsExtensionTypes { get; }

		internal FormatFeatures(FormatFeaturesBuilder builder)
		{
			this.IsContextful = builder.IsContextful;
			this.CanCountCollectionItems = builder.CanCountCollectionItems;
			this.CanSpecifyStringEncoding = builder.CanSpecifyStringEncoding;
			this.SupportsExtensionTypes = builder.SupportsExtensionTypes;
		}
	}
}
