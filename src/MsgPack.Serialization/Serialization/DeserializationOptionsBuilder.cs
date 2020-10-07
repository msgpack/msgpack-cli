// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Text;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	/// <summary>
	///		A builder object for <see cref="DeserializationOptions"/> which defines options for each deserialization operations.
	/// </summary>
	/// <seealso cref="DeserializationOptions"/>
	public sealed class DeserializationOptionsBuilder
	{
		/// <summary>
		///		Gets the allowed maximum length of the serialized array.
		/// </summary>
		/// <value>
		///		The allowed maximum length of the serialized array. Default is <c>1,048,576</c> (<c>1Mi</c>).
		/// </value>
		public int MaxArrayLength { get; private set; } = OptionsDefaults.MaxArrayLength;

		/// <summary>
		///		Gets the allowed maximum count of the serialized map entries.
		/// </summary>
		/// <value>
		///		The allowed maximum count of the serialized map entries. Default is <c>1,048,576</c> (<c>1Mi</c>).
		/// </value>
		public int MaxMapCount { get; private set; } = OptionsDefaults.MaxMapCount;

		/// <summary>
		///		Gets the allowed maximum length of each serialized map keys.
		/// </summary>
		/// <value>
		///		The allowed maximum length of each serialized map keys. Default is <c>256</c>.
		/// </value>
		public int MaxPropertyKeyLength { get; private set; } = OptionsDefaults.MaxPropertyKeyLength;

		/// <summary>
		///		Gets the allowed maximum depth of the serialized object tree.
		/// </summary>
		/// <value>
		///		The allowed maximum depth of the serialized object tree. Default is <c>100</c>.
		/// </value>
		public int MaxDepth { get; private set; } = OptionsDefaults.MaxDepth;

		/// <summary>
		///		Gets the default string encoding for string members or map keys which are not specified explicitly.
		/// </summary>
		/// <value>
		///		The default string encoding for string members or map keys which are not specified explicitly.
		///		Default value is <c>null</c>, which means using default encoding of underlying codec.
		/// </value>
		public Encoding? DefaultStringEncoding { get; private set; }

		/// <summary>
		///		Gets the <see cref="ArrayPool{T}"/> of <see cref="System.Byte"/> which will be used as buffer for codec.
		/// </summary>
		/// <value>
		///		The <see cref="ArrayPool{T}"/> of <see cref="System.Byte"/> which will be used as buffer for codec.
		///		<c>null</c> means usage of the default pool which is defined by serialization runtime implementation.
		///		Note that the pool will be shared all serializers and codecs.
		/// </value>
		public ArrayPool<byte>? ByteBufferPool { get; private set; }

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
		public bool ClearsByteBufferOnReturn { get; private set; } = OptionsDefaults.ClearsBufferOnReturn;

		/// <summary>
		///		Initializes a new instance of <see cref="DeserializationOptionsBuilder"/> object.
		/// </summary>
		public DeserializationOptionsBuilder() { }

		/// <summary>
		///		Creates a new instance of <see cref="DeserializationOptions"/> object from current state of this instance.
		/// </summary>
		/// <returns>A new instance of <see cref="DeserializationOptions"/> object.</returns>
		public DeserializationOptions Create()
			=> new DeserializationOptions(this.MaxArrayLength, this.MaxMapCount, this.MaxPropertyKeyLength, this.MaxDepth, this.DefaultStringEncoding, this.ByteBufferPool ?? ArrayPool<byte>.Shared, this.ClearsByteBufferOnReturn);

		/// <summary>
		///		Indicates the allowed maximum length of the serialized array to the default value.
		/// </summary>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="MaxDepth"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public DeserializationOptionsBuilder UseDefaultMaxArrayLength()
			=> this.UseMaxArrayLength(OptionsDefaults.MaxArrayLength);

		/// <summary>
		///		Sets the allowed maximum length of the serialized array.
		/// </summary>
		/// <param name="value">A value to be set.</param>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentNullException">
		///		<paramref name="value"/> is less than or equal to <c>0</c>, or exceeds <c>0x7FEFFFFF</c>.
		/// </exception>
		public DeserializationOptionsBuilder UseMaxArrayLength(int value)
		{
			this.MaxArrayLength = Ensure.IsBetween(value, 0, OptionsDefaults.MaxMultiByteCollectionLength);
			return this;
		}

		/// <summary>
		///		Indicates the allowed maximum count of the serialized map entries to the default value.
		/// </summary>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="MaxDepth"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public DeserializationOptionsBuilder UseDefaultMaxMapCount()
			=> this.UseMaxMapCount(OptionsDefaults.MaxMapCount);

		/// <summary>
		///		Sets the allowed maximum count of the serialized map entries.
		/// </summary>
		/// <param name="value">A value to be set.</param>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentNullException">
		///		<paramref name="value"/> is less than or equal to <c>0</c>, or exceeds <c>0x7FEFFFFF</c>.
		/// </exception>
		public DeserializationOptionsBuilder UseMaxMapCount(int value)
		{
			this.MaxMapCount = Ensure.IsBetween(value, 0, OptionsDefaults.MaxMultiByteCollectionLength);
			return this;
		}

		/// <summary>
		///		Indicates the allowed maximum length of each serialized map keys to the default value.
		/// </summary>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="MaxDepth"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public DeserializationOptionsBuilder UseDefaultMaxPropertyKeyLength()
			=> this.UseMaxPropertyKeyLength(OptionsDefaults.MaxPropertyKeyLength);

		/// <summary>
		///		Sets the allowed maximum length of each serialized map keys.
		/// </summary>
		/// <param name="value">A value to be set.</param>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentNullException">
		///		<paramref name="value"/> is less than or equal to <c>0</c>, or exceeds <c>0x7FEFFFFF</c>.
		/// </exception>
		public DeserializationOptionsBuilder UseMaxPropertyKeyLength(int value)
		{
			this.MaxPropertyKeyLength = Ensure.IsBetween(value, 1, OptionsDefaults.MaxMultiByteCollectionLength);
			return this;
		}

		/// <summary>
		///		Indicates the allowed maximum depth of serialized object tree to the default value.
		/// </summary>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="MaxDepth"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public DeserializationOptionsBuilder UseDefaultMaxDepth()
			=> this.UseMaxDepth(OptionsDefaults.MaxDepth);

		/// <summary>
		///		Sets the allowed maximum depth of serialized object tree.
		/// </summary>
		/// <param name="value">A value to be set.</param>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentNullException">
		///		<paramref name="value"/> is less than or equal to <c>0</c>, or exceeds <c>0x7FEFFFFF</c>.
		/// </exception>
		public DeserializationOptionsBuilder UseMaxDepth(int value)
		{
			this.MaxDepth = Ensure.IsBetween(value, 1, OptionsDefaults.MaxMultiByteCollectionLength); ;
			return this;
		}

		/// <summary>
		///		Indicates the default string encoding for string members or map keys to use default encoding of the codec.
		/// </summary>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="DefaultStringEncoding"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public DeserializationOptionsBuilder UseCodecDefaultStringEncoding()
		{
			this.DefaultStringEncoding = null;
			return this;
		}

		/// <summary>
		///		Sets the default string encoding for string members or map keys which are not specified explicitly.
		/// </summary>
		/// <param name="value">An <see cref="Encoding"/> to use encode/decode string value.</param>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
		/// <remarks>
		///		This method should be called only when you needs to interoperability with system which uses non-default encoding for the codec.
		/// </remarks>
		public DeserializationOptionsBuilder UseCustomDefaultStringEncoding(Encoding value)
		{
			this.DefaultStringEncoding = Ensure.NotNull(value);
			return this;
		}

		/// <summary>
		///		Indicates the default <see cref="ArrayPool{T}"/> of <see cref="System.Byte"/> which will be used as buffer for codec.
		/// </summary>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="ByteBufferPool"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public DeserializationOptionsBuilder UseDefaultByteBufferPool()
		{
			this.ByteBufferPool = null;
			return this;
		}

		/// <summary>
		///		Sets the <see cref="ArrayPool{T}"/> of <see cref="System.Byte"/> which will be used as buffer for codec.
		/// </summary>
		/// <param name="value">The <see cref="ArrayPool{T}"/> of <see cref="System.Byte"/> which will be used as buffer for codec.</param>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
		/// <remarks>
		///		Note that the pool will be shared all serializers and codecs.
		/// </remarks>
		public DeserializationOptionsBuilder UseCustomByteBufferPool(ArrayPool<byte> value)
		{
			this.ByteBufferPool = Ensure.NotNull(value);
			return this;
		}

		/// <summary>
		///		Indicates the buffer from <see cref="ByteBufferPool"/> should be cleared on return.
		/// </summary>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method prevents that a malicious serializer can snoof buffer contents, so it might be security vulnerability.
		///		However, significant performance hit may be occurred for large buffer.
		/// </remarks>
		public DeserializationOptionsBuilder WithClearingByteBufferOnReturn()
		{
			this.ClearsByteBufferOnReturn = true;
			return this;
		}

		/// <summary>
		///		Indicates the buffer from <see cref="ByteBufferPool"/> should NOT be cleared on return.
		/// </summary>
		/// <returns>This <see cref="DeserializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="ClearsByteBufferOnReturn"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public DeserializationOptionsBuilder WithoutClearingByteBufferOnReturn()
		{
			this.ClearsByteBufferOnReturn = false;
			return this;
		}
	}
}
