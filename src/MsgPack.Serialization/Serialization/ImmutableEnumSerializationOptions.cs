// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Diagnostics;
using MsgPack.Codecs;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Immutable implementation of <see cref="IEnumSerializationOptions"/>.
	/// </summary>
	internal sealed class ImmutableEnumSerializationOptions : IEnumSerializationOptions
	{
		private readonly EnumSerializationMethod? _serializationMethod;

		/// <inheritdoc />
		public EnumSerializationMethod GetSerializationMethod(CodecFeatures codecFeatures)
		{
			Debug.Assert(codecFeatures != null);
			return this._serializationMethod ?? codecFeatures.PreferredEnumSerializationMethod;
		}

		/// <inheritdoc />
		public Func<string, string> NameTransformer { get; }

		internal ImmutableEnumSerializationOptions(EnumSerializationOptionsBuilder builder)
		{
			this._serializationMethod = builder.SerializationMethod;
			this.NameTransformer = builder.NameTransformer ?? KeyNameTransformers.AsIs;
		}
	}

}
