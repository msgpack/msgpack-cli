// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using MsgPack.Codecs;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines enum serialization options.
	/// </summary>
	internal interface IEnumSerializationOptions
	{
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
		EnumSerializationMethod GetSerializationMethod(CodecFeatures codecFeatures);

		/// <summary>
		///		Gets the name handler which enables customization of enum values serialization by their names.
		/// </summary>
		/// <value>
		///		The name handler which enables customization of enum values serialization by their names.
		/// </value>
		/// <remarks>
		///		This value will only affect when <see cref="P:SerializationMethod"/> is set to <see cref="EnumSerializationMethod.ByName"/>.
		///		In addition, deserialization is always done by case insensitive manner.
		/// </remarks>
		Func<string, string> NameTransformer { get; }

		/// <summary>
		///		Gets a value whether of member names are ignored in deserialization or not.
		/// </summary>
		/// <value>
		///		<c>true</c>, if casing of member names are ignored in deserialization; <c>false</c>, otherwise.
		/// </value>
		bool IgnoresCaseOnDeserialization { get; }
	}
}
