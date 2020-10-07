// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
#warning TODO: CompatibilityAdapter support
	/// <summary>
	///		Marks that the type has special meaning for serialization process in previous version,
	///		so it is required to specified adapter type to handle serialization and/or deserialization.
	/// </summary>
	/// <remarks>
	///		Namely, this type decouples <c>I[Async][Un]packable</c> interfaces from this assembly.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Interface)]
	internal sealed class CompatibilityAdapterAttribute : Attribute
	{
		/// <summary>
		///		Gets the adapter type which handle custom serialization and/or deserialization for the marked type.
		/// </summary>
		/// <value>
		///		The <see cref="Type"/> of the adapter type which handle custom serialization and/or deserialization for the marked type.
		/// </value>
		public Type AdapterType { get; }

		/// <summary>
		///		Gets the kind of the adapter.
		/// </summary>
		/// <value>
		///		The kind of the adapter.
		/// </value>
		public CompatibilityAdapterKind Kind { get; }

		/// <summary>
		///		Initializes a new instance of <see cref="CompatibilityAdapterAttribute"/> object.
		/// </summary>
		/// <param name="kind"></param>
		/// <param name="adapterType"></param>
		public CompatibilityAdapterAttribute(CompatibilityAdapterKind kind, Type adapterType)
		{
			this.Kind = kind;
			this.AdapterType = adapterType;
		}
	}

}
