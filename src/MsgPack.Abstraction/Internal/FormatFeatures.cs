// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Internal
{
	public sealed class FormatFeatures
	{
		public bool IsContextful { get; }
		public bool CanCountCollectionItems { get; }
		public bool CanSpecifyStringEncoding { get; }

		internal FormatFeatures(FormatFeaturesBuilder builder)
		{
			this.IsContextful = builder.IsContextful;
			this.CanCountCollectionItems = builder.CanCountCollectionItems;
			this.CanSpecifyStringEncoding = builder.CanSpecifyStringEncoding;
		}
	}
}
