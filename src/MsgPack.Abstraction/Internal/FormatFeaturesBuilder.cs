// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using MsgPack.Serialization;

namespace MsgPack.Internal
{
	public sealed class FormatFeaturesBuilder
	{
		public string Name { get; }
		public bool IsContextful { get; set; }
		public bool CanCountCollectionItems { get; set; }
		public bool CanSpecifyStringEncoding { get; set; }
		public bool SupportsExtensionTypes { get; set; }
		public SerializationMethod PreferredSerializationMethod { get; set; }
		public AvailableSerializationMethods AvailableSerializationMethods { get; set; }

		public FormatFeaturesBuilder(string name)
		{
			this.Name = Ensure.NotBlank(name).Trim();
		}

		public FormatFeatures Build() => new FormatFeatures(this);
	}
}
