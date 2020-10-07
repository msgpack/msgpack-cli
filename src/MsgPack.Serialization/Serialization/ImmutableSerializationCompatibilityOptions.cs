// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Immutable implementation of <see cref="ISerializationCompatibilityOptions"/>.
	/// </summary>
	internal sealed class ImmutableSerializationCompatibilityOptions : ISerializationCompatibilityOptions
	{
		internal static ImmutableSerializationCompatibilityOptions Default { get; } = new ImmutableSerializationCompatibilityOptions(new SerializationCompatibilityOptionsBuilder());

		/// <inheritdoc />
		public bool UsesOneBoundDataMemberOrder { get; }

		// Always false
		/// <inheritdoc />
		public bool IgnoresAdapterForCollection => false;

		/// <inheritdoc />
		public bool AllowsNonCollectionEnumerableTypes { get; }

		/// <inheritdoc />
		public IEnumerable<Func<Type, ISerializerGenerationOptions, bool>> SerializableInterfaceDetectors { get; }

		/// <inheritdoc />
		public IEnumerable<Func<Type, ISerializerGenerationOptions, bool>> DeserializableInterfaceDetectors { get; }

		/// <inheritdoc />
		public Func<IEnumerable<CustomAttributeData>, IEnumerable<CustomAttributeData>, MessagePackMemberAttributeData?> MessagePackMemberAttributeCompatibilityProvider { get; }

		/// <inheritdoc />
		public Func<IEnumerable<CustomAttributeData>, IEnumerable<CustomAttributeData>, MessagePackIgnoreAttributeData?> IgnoringAttributeCompatibilityProvider { get; }

		/// <summary>
		///		Initializes a new instance of <see cref="ImmutableSerializationCompatibilityOptions"/> object.
		/// </summary>
		/// <param name="builder"><see cref="SerializationContextBuilder"/>.</param>
		public ImmutableSerializationCompatibilityOptions(SerializationCompatibilityOptionsBuilder builder)
		{
			this.AllowsNonCollectionEnumerableTypes = builder.AllowsNonCollectionEnumerableTypes;
			this.UsesOneBoundDataMemberOrder = builder.UsesOneBoundDataMemberOrder;
			this.SerializableInterfaceDetectors = builder.SerializableAnywayInterfaceDetectors;
			this.DeserializableInterfaceDetectors = builder.DeserializableInterfaceDetectors;
			this.MessagePackMemberAttributeCompatibilityProvider = builder.MessagePackMemberAttributeCompatibilityProvider ?? DefaultMessagePackAttributeCompatibilityProviders.DefaultMessagePackMemberAttributeCompatibilityProvider;
			this.IgnoringAttributeCompatibilityProvider = builder.IgnoringAttributeCompatibilityProvider ?? DefaultMessagePackAttributeCompatibilityProviders.DefaultMessagePackIgnoreAttributeCompatibilityProvider;
		}
	}

}
