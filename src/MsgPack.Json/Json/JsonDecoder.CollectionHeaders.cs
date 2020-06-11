// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using MsgPack.Internal;

namespace MsgPack.Json
{
	public partial class JsonDecoder
	{
		public override CollectionType DecodeArrayOrMapHeader(in SequenceReader<byte> source, out long itemsCount, out int requestHint)
			=> JsonThrow.CollectionHeaderDecodingIsNotSupported(out itemsCount, out requestHint);

		public override long DecodeArrayHeader(in SequenceReader<byte> source, out int requestHint)
			=> JsonThrow.CollectionHeaderDecodingIsNotSupported(out requestHint);

		public override long DecodeMapHeader(in SequenceReader<byte> source, out int requestHint)
			=> JsonThrow.CollectionHeaderDecodingIsNotSupported(out requestHint);
	}
}
