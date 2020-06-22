// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization
{
	public readonly struct MessagePackMemberAttributeData
	{
		public int? Id { get; }
		public string? Name { get; }

		public MessagePackMemberAttributeData(int? id, string? name)
		{
			this.Id = id == null ? id : Ensure.IsNotLessThan(id.GetValueOrDefault(), 0, nameof(id));
			this.Name = name;
		}
	}

	internal sealed class SerializerGenerationOptions
	{
		public bool DisablesPrivilegedAccess { get; }
		public bool OneBoundDataMemberOrder { get; }
		public bool AllowsAsymmetricSerializer { get; }
		public ISet<string> IgnoreAttributeTypeNames { get; }
		public Func<Type, SerializerGenerationOptions, bool> SerializableAnywayInterfaceDetector { get; } 
		public Func<Type, SerializerGenerationOptions, bool> DeserializableInterfaceDetector { get; }
		public Func<IEnumerable<CustomAttributeData>, IEnumerable<CustomAttributeData>, MessagePackMemberAttributeData?>? MessagePackMemberAttributeCompatibilityProvider { get; }
		public IReadOnlyDictionary<Type, IEnumerable<string>> IgnoringMembers { get; }
	}

	public sealed class SerializerGenerationOptionsBuilder
	{
		private static readonly string[] DefaultIgnoreAttributeTypeNames =
			new[]
			{
				"MsgPack.Serialization.MessagePackIgnoreAttribute",
				"System.Runtime.Serialization.IgnoreDataMemberAttribute"
			};

#warning TODO: CompatiblityPackage:	typeof(IPackable).IsAssignableFrom(t) || typeof(IUnpackable).IsAssignableFrom(t) || (!o.WithAsync || typeof(IAsyncPackable).IsAssignableFrom(t) || typeof(IAsyncUnpackable).IsAssignableFrom(t))
		private static readonly Func<Type, SerializerGenerationOptions, bool>[] DefaultSerializableAnywayInterfaceDetectors = Array.Empty<Func<Type, SerializerGenerationOptions, bool>>();

#warning TODO: CompatiblityPackage: typeof(IUnpackable).IsAssignableFrom(t) && (!o.WithAsync || typeof(IAsyncUnpackable).IsAssignable(t))
		private static readonly Func<Type, SerializerGenerationOptions, bool>[] DefaultDeserializableInterfaceDetectors = Array.Empty<Func<Type, SerializerGenerationOptions, bool>>();

		internal IList<Func<Type, SerializerGenerationOptions, bool>> SerializableAnywayInterfaceDetectors { get; } = new List<Func<Type, SerializerGenerationOptions, bool>>(DefaultSerializableAnywayInterfaceDetectors);
		internal IList<Func<Type, SerializerGenerationOptions, bool>> DeserializableInterfaceDetectors { get; } = new List<Func<Type, SerializerGenerationOptions, bool>>(DefaultDeserializableInterfaceDetectors);

		public bool DisablesPrivilegedAccess { get; set; }
		public bool OneBoundDataMemberOrder { get; set; }
		public bool AllowsAsymmetricSerializer { get; set; }
		public ISet<string> IgnoreAttributeTypeNames { get; } = new HashSet<string>(DefaultIgnoreAttributeTypeNames, StringComparer.Ordinal);
		public Func<IEnumerable<CustomAttributeData>, IEnumerable<CustomAttributeData>, MessagePackMemberAttributeData?>? MessagePackMemberAttributeCompatibilityProvider { get; set; }
	}
}
