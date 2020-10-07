// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Immutable implementation of <see cref="IDictionarySerializationOptions"/>.
	/// </summary>
	internal sealed class ImmutableDictionarySerializationOptions : IDictionarySerializationOptions
	{
		/// <summary>
		///		Gets an instance with default settings.
		/// </summary>
		internal static ImmutableDictionarySerializationOptions Default { get; } = new ImmutableDictionarySerializationOptions(new DictionarySerializationOptionsBuilder());

		/// <inheritdoc />
		public bool OmitsNullEntries { get; }

		/// <inheritdoc />
		public Func<string, string> KeyTransformer { get; }

		/// <summary>
		///		Initializes a new instance of <see cref="ImmutableDictionarySerializationOptions"/> object.
		/// </summary>
		/// <param name="builder"><see cref="DictionarySerializationOptionsBuilder"/>.</param>
		internal ImmutableDictionarySerializationOptions(DictionarySerializationOptionsBuilder builder)
		{
			this.OmitsNullEntries = builder.OmitsNullEntries;
			this.KeyTransformer = builder.KeyTransformer ?? KeyNameTransformers.AsIs;
		}
	}
}
