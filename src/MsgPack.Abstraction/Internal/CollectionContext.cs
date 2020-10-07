// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Internal
{
	public partial struct CollectionContext
	{
		public static CollectionContext Default =>
			new CollectionContext(
				OptionsDefaults.MaxArrayLength,
				OptionsDefaults.MaxMapCount,
				OptionsDefaults.MaxDepth,
				currentDepth: 0
			);

		public int MaxArrayLength { get; }
		public int MaxMapCount { get; }
		public int MaxDepth { get; }
		public int CurrentDepth { get; private set; }

		internal CollectionContext(int maxArrayLength, int maxMapCount, int maxDepth, int currentDepth)
		{
			this.MaxArrayLength = maxArrayLength;
			this.MaxMapCount = maxMapCount;
			this.MaxDepth = Ensure.IsNotLessThan(maxDepth, 1);
			this.CurrentDepth = currentDepth;
		}

		public int IncrementDepth(long position)
		{
			if (this.CurrentDepth == this.MaxDepth)
			{
				Throw.DepthExeeded(position, this.MaxDepth);
			}

			return this.CurrentDepth++;
		}

		public int DecrementDepth()
		{
			if (this.CurrentDepth == 0)
			{
				Throw.DepthUnderflow();
			}

			return this.CurrentDepth--;
		}
	}
}
