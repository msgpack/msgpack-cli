// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace MsgPack.Internal
{
	internal static partial class StringBuilderExtensions
	{
		public static ReadOnlySequence<char> ToSequence(this StringBuilder builder)
		{
			var runningIndex = 0L;
			var firstSegment = default(StringBuilderChunkSegment);
			var lastSegment = default(StringBuilderChunkSegment);
			foreach(var chunk in builder.GetChunks())
			{
				if (chunk.IsEmpty)
				{
					continue;
				}

				var segment = new StringBuilderChunkSegment(chunk, runningIndex);
				runningIndex += chunk.Length;
				if (lastSegment == null)
				{
					firstSegment = segment;
				}
				else
				{
					lastSegment.SetNext(segment);
				}

				lastSegment = segment;
			}

			if (lastSegment == null)
			{
				return ReadOnlySequence<char>.Empty;
			}

			return new ReadOnlySequence<char>(firstSegment!, 0, lastSegment, lastSegment.Memory.Length - 1);
		}

		private sealed class StringBuilderChunkSegment : ReadOnlySequenceSegment<char>
		{
			public void SetNext(StringBuilderChunkSegment next)
				=> this.Next = next;

			public StringBuilderChunkSegment(ReadOnlyMemory<char> chunk, long runningIndex)
			{
				this.Memory = chunk;
				this.RunningIndex = runningIndex;
			}
		}
	}
}
