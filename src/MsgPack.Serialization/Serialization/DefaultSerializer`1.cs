// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using MsgPack.Codecs;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Default implementation of <see cref="Serializer{T}"/>.
	/// </summary>
	/// <typeparam name="T">The target type.</typeparam>
	internal sealed class DefaultSerializer<T> : Serializer<T>
	{
		public DefaultSerializer(
			CodecProvider codecProvider,
			ObjectSerializer<T> underlying,
			SerializationOptions serializationOptions,
			DeserializationOptions deserializationOptions
		)
			: base(codecProvider, underlying, serializationOptions, deserializationOptions)
		{
		}
	}
}
