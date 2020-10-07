// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using MsgPack.Codecs;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Builder object for enum serialization options.
	/// </summary>
	public sealed class EnumSerializationOptionsBuilder
	{
		/// <summary>
		///		Gets an internal cached default instance.
		/// </summary>
		/// <value>
		///		An internal cached default instance.
		/// </value>
		internal static EnumSerializationOptionsBuilder Default { get; } = new EnumSerializationOptionsBuilder();

		/// <summary>
		///		Gets the <see cref="EnumSerializationMethod"/> to determine default enum serialization method.
		/// </summary>
		/// <param name="codecFeatures"><see cref="CodecFeatures"/> which holds default enum serialization method of the codec.</param>
		/// <returns>
		///		The <see cref="EnumSerializationMethod"/> to determine default enum serialization method.
		/// </returns>
		/// <remarks>
		///		<include file='../../Common/Remarks.xml' path='docs/doc[@name="EnumSerialization"]'/>
		/// </remarks>
		public EnumSerializationMethod? SerializationMethod { get; private set; }

		/// <summary>
		///		Gets the name handler which enables customization of enum values serialization by their names.
		/// </summary>
		/// <value>
		///		The name handler which enables customization of enum values serialization by their names.
		///		The default value is <c>null</c>, which indicates that value is not transformed.
		/// </value>
		/// <remarks>
		///		This value will only affect when <see cref="P:SerializationMethod"/> is set to <see cref="EnumSerializationMethod.ByName"/>.
		///		In addition, deserialization is always done by case insensitive manner.
		/// </remarks>
		public Func<string, string>? NameTransformer { get; private set; }

		/// <summary>
		///		Gets the value whether of member names are ignored in deserialization or not.
		/// </summary>
		/// <value>
		///		<c>true</c>, if casing of member names are ignored in deserialization; <c>false</c>, otherwise.
		/// </value>
		public bool IgnoresCaseOnDeserialization { get; private set; }

		/// <summary>
		///		Initializes a new instance of <see cref="EnumSerializationOptionsBuilder"/> object.
		/// </summary>
		public EnumSerializationOptionsBuilder() { }

		/// <summary>
		///		Directly sets <see cref="SerializationMethod"/> property for compatibility layer.
		/// </summary>
		/// <param name="value">Value to set.</param>
		internal void InternalSetSerializationMethod(EnumSerializationMethod? value)
			=> this.SerializationMethod = value;

		/// <summary>
		///		Indicates enum values will be serialized with their name by default.
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method sets <see cref="SerializationMethod"/> property to <see cref="EnumSerializationMethod.ByName"/>.
		/// </remarks>
		public EnumSerializationOptionsBuilder SerializeByName()
		{
			this.SerializationMethod = EnumSerializationMethod.ByName;
			return this;
		}

		/// <summary>
		///		Indicates enum values will be serialized with their underlying value by default.
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method sets <see cref="SerializationMethod"/> property to <see cref="EnumSerializationMethod.ByUnderlyingValue"/>.
		/// </remarks>
		public EnumSerializationOptionsBuilder SerializeByUnderlyingValue()
		{
			this.SerializationMethod = EnumSerializationMethod.ByUnderlyingValue;
			return this;
		}

		/// <summary>
		///		Indicates enum values will be serialized with <see cref="CodecFeatures.PreferredEnumSerializationMethod"/>.
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="SerializationMethod"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public EnumSerializationOptionsBuilder SerializeByCodecPreferredMethod()
		{
			this.SerializationMethod = null;
			return this;
		}

		/// <summary>
		///		Indicates no name tranformation is used.
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="NameTransformer"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public EnumSerializationOptionsBuilder UseDefaultNameTransformer()
		{
			this.NameTransformer = null;
			return this;
		}

		/// <summary>
		///		Sets <see cref="NameTransformer"/> property with specified transformer delegate.
		/// </summary>
		/// <param name="value">
		///		A delegate which accept original member name and then returns serialized name.
		/// </param>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="value"/> is <c>null</c>.
		/// </exception>
		public EnumSerializationOptionsBuilder UseCustomNameTransformer(Func<string, string> value)
		{
			this.NameTransformer = Ensure.NotNull(value);
			return this;
		}

		/// <summary>
		///		Indicates using name tranformation between PascalCasing (UpperCamelCasing) and lowerCamelCasing.
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		public EnumSerializationOptionsBuilder UseLowerCamelCasingNameTransformer()
		{
			this.NameTransformer = KeyNameTransformers.ToLowerCamel;
			return this;
		}

		/// <summary>
		///		Indicates using name tranformation between PascalCasing (UpperCamelCasing) and UPPER_SNAKE_CASING.
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		public EnumSerializationOptionsBuilder UseUpperSnakeCasingNameTransformer()
		{
			this.NameTransformer = KeyNameTransformers.ToUpperSnake;
			return this;
		}

		/// <summary>
		///		Indicates using name tranformation between PascalCasing (UpperCamelCasing) and lower_snake_casing.
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		public EnumSerializationOptionsBuilder UseLowerSnakeCasingNameTransformer()
		{
			this.NameTransformer = KeyNameTransformers.ToUpperSnake;
			return this;
		}

		/// <summary>
		///		Indicates ingoring casing on deserialization.
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		public EnumSerializationOptionsBuilder IgnoreCaseOnDeserialization()
		{
			this.IgnoresCaseOnDeserialization = true;
			return this;
		}

		/// <summary>
		///		Indicates distinct casing on deserialization.
		/// </summary>
		/// <returns>This <see cref="SerializationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="IgnoresCaseOnDeserialization"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public EnumSerializationOptionsBuilder DistinctCaseOnDeserialization()
		{
			this.IgnoresCaseOnDeserialization = false;
			return this;
		}
	}

}
