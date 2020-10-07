// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Internal <see cref="IBindingOptions"/> implementation.
	/// </summary>
	internal sealed class ImmutableBindingOptions : IBindingOptions
	{
		public IReadOnlyDictionary<Type, IEnumerable<string>> AllIgnoringMembers { get; }

		/// <inheritdoc />
		public bool IsPrivilegedAccessDisabled { get; }

		/// <inheritdoc />
		public bool AssumesInternalsVisibleTo { get; }

		/// <inheritdoc />
		public IEnumerable<string> GetIgnoringMembers(Type targetType)
			=> this.AllIgnoringMembers.TryGetValue(targetType, out var result) ? result : Enumerable.Empty<string>();

		public ImmutableBindingOptions(BindingOptionsBuilder builder)
		{
			this.AllIgnoringMembers = new Dictionary<Type, IEnumerable<string>>(builder.IgnoringMembers);
			this.AssumesInternalsVisibleTo = builder.AssumesInternalsVisibleTo;
			this.IsPrivilegedAccessDisabled = builder.IsPrivilegedAccessDisabled;
		}
	}

}
