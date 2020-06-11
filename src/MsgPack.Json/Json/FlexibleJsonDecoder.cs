// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;

namespace MsgPack.Json
{
	/// <summary>
	///		Simple <see cref="JsonDecoder"/> implementation which can handle various resource consuming relaxations.
	/// </summary>
	internal sealed partial class FlexibleJsonDecoder : JsonDecoder
	{
		public FlexibleJsonDecoder(JsonDecoderOptions options)
			: base(options) { }

		protected override bool TryReadNull(in SequenceReader<byte> source)
		{
			if (source.IsNext(JsonTokens.Null, advancePast: true))
			{
				return true;
			}

			if ((this.Options.ParseOptions & JsonParseOptions.AllowUndefined) != 0
				&& source.IsNext(JsonTokens.Undefined, advancePast: true))
			{
				return true;
			}

			return false;
		}
	}
}
