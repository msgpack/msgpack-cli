// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Text;
using MsgPack.Internal;
using MsgPack.Serialization;

namespace MsgPack.Codecs
{
	/// <summary>
	///		A builder object for <see cref="CodecFeatures"/>.
	/// </summary>
	public sealed class CodecFeaturesBuilder
	{
		/// <summary>
		///		Gets a unique name of the underlying codec.
		/// </summary>
		/// <value>
		///		A unique name of the underlying codec which is non-blank string.
		/// </value>
		public string Name { get; }

		/// <summary>
		///		Gets or sets a value which indicates the underlying codec supports collection length.
		/// </summary>
		/// <value>
		///		<c>true</c>, if the underlyng codec supports collection length; <c>false</c>, otherwise.
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
		public bool CanCountCollectionItems { get; set; }

		/// <summary>
		///		Gets or sets a value which indicates the underlying codec supports custom string encoding.
		/// </summary>
		/// <value>
		///		<c>true</c>, if the underlyng format supports custom string encoding; <c>false</c>, otherwise.
		/// </value>
		/// <remarks>
		///		When this property returns <c>false</c>, <see cref="System.Text.Encoding"/> typed parameters in <see cref="Encoder{TExtensionType}"/> and <see cref="Decoder{TExtensionType}"/> methods will be ignored.
		/// </remarks>
		public bool CanSpecifyStringEncoding { get; set; }

		/// <summary>
		///		Gets or sets a value which indicates the underlying codec supports extension types.
		/// </summary>
		/// <value>
		///		<c>true</c>, if the underlyng codec supports extension types; <c>false</c>, otherwise.
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
		public bool SupportsExtensionTypes { get; set; }

		/// <summary>
		///		Gets a value which indicates preferred <see cref="SerializationMethod"/> for the underlying codec.
		/// </summary>
		/// <value>
		///		A value which indicates preferred <see cref="SerializationMethod"/> for the underlying codec.
		/// </value>
		public SerializationMethod PreferredObjectSerializationMethod { get; private set; }

		private EnumSerializationMethod _preferredEnumSerializationMethod;

		/// <summary>
		///		Gets or sets a value which indicates preferred <see cref="EnumSerializationMethod"/> for the underlying codec.
		/// </summary>
		/// <value>
		///		A value which indicates preferred <see cref="EnumSerializationMethod"/> for the underlying codec.
		/// </value>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		Setting value is not a defined <see cref="EnumSerializationMethod"/> value.
		/// </exception>
		public EnumSerializationMethod PreferredEnumSerializationMethod
		{
			get => this._preferredEnumSerializationMethod;
			set => this._preferredEnumSerializationMethod = Ensure.Defined(value);
		}

		private DateTimeConversionMethod _preferredDateTimeConversionMethod;

		/// <summary>
		///		Gets or sets a value which indicates preferred <see cref="DateTimeConversionMethod"/> for the underlying codec.
		/// </summary>
		/// <value>
		///		A value which indicates preferred <see cref="DateTimeConversionMethod"/> for the underlying codec.
		/// </value>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		Setting value is not a defined <see cref="DateTimeConversionMethod"/> value.
		/// </exception>
		public DateTimeConversionMethod PreferredDateTimeConversionMethod
		{
			get => this._preferredDateTimeConversionMethod;
			set => this._preferredDateTimeConversionMethod = Ensure.Defined(value);
		}

		/// <summary>
		///		Gets a value which indicates available <see cref="SerializationMethod"/> set as <see cref="AvailableSerializationMethods"/>.
		/// </summary>
		/// <value>
		///		A value which indicates available <see cref="SerializationMethod"/> set as <see cref="AvailableSerializationMethods"/>.
		/// </value>
		public AvailableSerializationMethods AvailableSerializationMethods { get; private set; }

		/// <summary>
		///		Gets the default encoding of strings.
		/// </summary>
		/// <value>
		///		The default encoding of strings.
		/// </value>
		public Encoding DefaultStringEncoding { get; private set; } = Utf8EncodingNonBomStrict.Instance;


		/// <summary>
		///		Gets the precision of fraction portion on ISO 8601 date time string as specified by this codec.
		/// </summary>
		/// <value>
		///		The precision of fraction portion on ISO 8601 date time string as specified by this codec.
		///		<c>null</c> means that the value will be determined by serialization option or serializer implementation.
		/// </value>
		/// <remarks>
		///		<include file='../../Common/Remarks.xml' path='docs/doc[@name="DateTimeConversion"]'/>
		/// </remarks>
		public int? Iso8601FractionOfSecondsPrecision { get; private set; }

		/// <summary>
		///		Gets a separator char for fraction portion for the underlying codec.
		/// </summary>
		/// <value>
		///		A separator char for fraction portion.
		///		<c>null</c> means that the value will be determined by serialization option or serializer implementation.
		/// </value>
		public char? Iso8601DecimalSeparator { get; private set; }

		/// <summary>
		///		Initializes a new instance of <see cref="CodecFeaturesBuilder"/> object.
		/// </summary>
		/// <param name="name">A unique name of the underlying codec. Note that <see cref="Name"/> property will be trimmed value.</param>
		/// <exception cref="System.ArgumentNullException">
		///		<paramref name="name"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		///		<paramref name="name"/> is empty or contains only white spaces.
		/// </exception>
		public CodecFeaturesBuilder(string name)
		{
			this.Name = Ensure.NotBlankAndTrimmed(name);
		}

		/// <summary>
		///		Sets preferred and available serialization method for the underlying codec.
		/// </summary>
		/// <param name="preferredMethod">
		///		A value which indicates preferred <see cref="SerializationMethod"/> for the underlying codec.
		/// </param>
		/// <param name="availableMethods">
		///		A value which indicates available <see cref="SerializationMethod"/> set as <see cref="MsgPack.Serialization.AvailableSerializationMethods"/>.
		/// </param>
		/// <returns>This <see cref="CodecFeaturesBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		<paramref name="preferredMethod"/> value is not a defined <see cref="SerializationMethod"/> value.
		///		<paramref name="availableMethods"/> contains any undefined <see cref="MsgPack.Serialization.AvailableSerializationMethods"/> flag value.
		/// </exception>
		public CodecFeaturesBuilder SetObjectSerializationMethod(
			SerializationMethod preferredMethod,
			AvailableSerializationMethods availableMethods
		)
		{
			this.PreferredObjectSerializationMethod = Ensure.Defined(preferredMethod);
			this.AvailableSerializationMethods = Ensure.Defined(availableMethods);
			return this;
		}

		/// <summary>
		///		Sets the default encoding of strings.
		/// </summary>
		/// <param name="value">The default encoding of strings.</param>
		/// <returns>This <see cref="CodecFeaturesBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentNullException">
		///		<paramref name="value"/> is <c>null</c>.
		/// </exception>
		public CodecFeaturesBuilder SetDefaultStringEncoding(Encoding value)
		{
			this.DefaultStringEncoding = Ensure.NotNull(value);
			return this;
		}

		/// <summary>
		///		Resets the default encoding of strings.
		/// </summary>
		/// <returns>This <see cref="CodecFeaturesBuilder"/> instance.</returns>
		public CodecFeaturesBuilder ResetDefaultStringEncoding()
		{
			this.DefaultStringEncoding = Utf8EncodingNonBomStrict.Instance;
			return this;
		}

		/// <summary>
		///		Sets the precision of ISO 8601 fraction of seconds portion for the underlying codec.
		/// </summary>
		/// <param name="value">The precision.</param>
		/// <returns>This <see cref="CodecFeaturesBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		<paramref name="value"/> is negative value.
		/// </exception>
		/// <remarks>
		///		<include file='../../Common/Remarks.xml' path='docs/doc[@name="DateTimeConversion"]'/>
		/// </remarks>
		public CodecFeaturesBuilder SetIso8601FractionOfSecondsPrecision(int value)
		{
			this.Iso8601FractionOfSecondsPrecision = Ensure.IsNotLessThan(value, 0);
			return this;
		}

		/// <summary>
		///		Resets the precision of ISO 8601 fraction of seconds portion for the underlying codec.
		/// </summary>
		/// <returns>This <see cref="CodecFeaturesBuilder"/> instance.</returns>
		/// <remarks>
		///		<include file='../../Common/Remarks.xml' path='docs/doc[@name="DateTimeConversion"]'/>
		/// </remarks>
		public CodecFeaturesBuilder ResetIso8601FractionOfSecondsPrecision()
		{
			this.Iso8601FractionOfSecondsPrecision = null;
			return this;
		}

		/// <summary>
		///		Sets the decimal separator of ISO 8601 for the underlying codec.
		/// </summary>
		/// <param name="value">The precision.</param>
		/// <returns>This <see cref="CodecFeaturesBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		<paramref name="value"/> is not <c>,</c> nor <c>.</c>.
		/// </exception>
		public CodecFeaturesBuilder SetIso8601DecimalSeparator(char value)
		{
			switch(value)
			{
				case ',':
				case '.':
				{
					this.Iso8601DecimalSeparator = value;
					break;
				}
				default:
				{
					Throw.InvalidIso8601DecimalSeparator(value);
					break; // Never reaches
				}
			}

			return this;
		}

		/// <summary>
		///		Sets the decimal separator of ISO 8601 for the underlying codec.
		/// </summary>
		/// <returns>This <see cref="CodecFeaturesBuilder"/> instance.</returns>
		public CodecFeaturesBuilder ResetIso8601DecimalSeparator()
		{
			this.Iso8601DecimalSeparator = null;
			return this;
		}

		/// <summary>
		///		Builds new instance of immutable <see cref="CodecFeatures"/> object from this builder instance.
		/// </summary>
		/// <returns>New instance of immutable <see cref="CodecFeatures"/> object.</returns>
		public CodecFeatures Build() => new CodecFeatures(this);
	}
}
