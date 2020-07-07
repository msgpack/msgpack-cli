// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using MsgPack.Internal;
using MsgPack.Serialization;

namespace MsgPack.Json
{
	internal static class JsonFormatFeatures
	{
		public static FormatFeatures Value { get; } =
			new FormatFeaturesBuilder("Json")
			{
				CanCountCollectionItems = false,
				CanSpecifyStringEncoding = false,
				IsContextful = false,
				PreferredSerializationMethod = SerializationMethod.Map,
				AvailableSerializationMethods = AvailableSerializationMethods.Map,
				SupportsExtensionTypes = false
			}.Build();
	}
}
