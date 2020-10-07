// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Serialization
{
	/// <summary>
	///		Boxed representation for <see cref="DateTimeConversionMethod"/> to reduce allocation.
	/// </summary>
	internal sealed class BoxedEnumSerializationMethod
	{
		/// <summary>
		///		Enums are serialized with their name. It is more torelant to versioning but less efficient.
		/// </summary>
		public static BoxedEnumSerializationMethod ByName { get; } = new BoxedEnumSerializationMethod(EnumSerializationMethod.ByName);

		/// <summary>
		///		Enums are serialized with their underlying value. It is more efficient but less torelant to versioning.
		/// </summary>
		public static BoxedEnumSerializationMethod ByUnderlyingValue { get; } = new BoxedEnumSerializationMethod(EnumSerializationMethod.ByUnderlyingValue);

		/// <summary>
		///		Gets an equivalent <see cref="EnumSerializationMethod"/> value.
		/// </summary>
		/// <value>An equivalent <see cref="EnumSerializationMethod"/> value.</value>
		public EnumSerializationMethod Value { get; }

		private BoxedEnumSerializationMethod(EnumSerializationMethod value)
		{
			this.Value = value;
		}

		/// <summary>
		///		Gets an equivalent <see cref="BoxedEnumSerializationMethod"/> instance for specified <see cref="EnumSerializationMethod"/> value.
		/// </summary>
		/// <param name="enumValue"><see cref="DateTimeConversionMethod"/> value or <c>null</c>.</param>
		/// <returns>
		///		An equivalent <see cref="BoxedEnumSerializationMethod"/> instance for specified <see cref="EnumSerializationMethod"/> value.
		///		<c>null</c> when <paramref name="enumValue"/> is <c>null</c>.
		/// </returns>
		public static BoxedEnumSerializationMethod? Get(EnumSerializationMethod? enumValue)
			=> enumValue switch
			{
				EnumSerializationMethod.ByName => ByName,
				EnumSerializationMethod.ByUnderlyingValue => ByUnderlyingValue,
				_ => null
			};
	}
}
