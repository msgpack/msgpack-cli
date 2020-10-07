// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using MsgPack.Codecs;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Marks the type should be serialized with specified <see cref="T:SerializationMethod"/> if available on the underlying codec.
	/// </summary>
	/// <remarks>
	///		This attribute supersedes the option in <see cref="SerializerGenerationOptionsBuilder"/>, but will be ignored when the value is not available on underlying codec.
	///		The availability of the codec can be checked with <see cref="CodecFeatures.AvailableSerializationMethods"/>.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public sealed class MessagePackSerializationMethodAttribute : Attribute
	{
		/// <summary>
		///		Gets a value which indicates <see cref="T:SerializationMethod"/> for the qualified type, if available on the underlying codec.
		/// </summary>
		/// <value>
		///		A value which indicates <see cref="T:SerializationMethod"/> for the qualified type, if available on the underlying codec.
		/// </value>
		public SerializationMethod SerializationMethod { get; }

		/// <summary>
		///		Initialize a new instance of <see cref="MessagePackSerializationMethodAttribute"/> type.
		/// </summary>
		/// <param name="serializationMethod">
		///		A value which indicates <see cref="T:SerializationMethod"/> for the qualified type, if available on the underlying codec.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="serializationMethod"/> is not defined as <see cref="SerializationMethod"/> enumeration.
		/// </exception>
		public MessagePackSerializationMethodAttribute(SerializationMethod serializationMethod)
		{
			switch (serializationMethod)
			{
				case SerializationMethod.Array:
				case SerializationMethod.Map:
				{
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(serializationMethod));
				}
			}

			this.SerializationMethod = serializationMethod;
		}
	}
}
