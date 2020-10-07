// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MsgPack
{
	/// <summary>
	///		Implements basic (maybe naive) implementation for common Set&lt;T&gt; operation.
	/// </summary>
	internal static class SetOperation
	{
#if NET35
		public static bool IsProperSubsetOf<T>(ICollection<T> set, IEnumerable<T> other)
#else
		public static bool IsProperSubsetOf<T>(ISet<T> set, IEnumerable<T> other)
#endif
		{
			Debug.Assert(set != null, "set != null ");
			Ensure.NotNull(other);

			if (other is ICollection<T> asCollection)
			{
				if (set.Count == 0)
				{
					return 0 < asCollection.Count;
				}

				if (asCollection.Count <= set.Count)
				{
					return false;
				}
			}

			if (!IsSubsetOfCore(set, other, out var otherCount))
			{
				return false;
			}

			return set.Count < otherCount;
		}

#if NET35
		public static bool IsSubsetOf<T>(ICollection<T> set, IEnumerable<T> other)
#else
		public static bool IsSubsetOf<T>(ISet<T> set, IEnumerable<T> other)
#endif
		{
			Debug.Assert(set != null, "set != null");
			Ensure.NotNull(other);

			if (set.Count == 0)
			{
				return true;
			}

			if (other is ICollection<T> asCollection && asCollection.Count < set.Count)
			{
				return false;
			}

			int checkedCount;
			return IsSubsetOfCore(set, other, out checkedCount);
		}

#if NET35
		private static bool IsSubsetOfCore<T>(ICollection<T> set, IEnumerable<T> other, out int otherCount)
#else
		private static bool IsSubsetOfCore<T>(ISet<T> set, IEnumerable<T> other, out int otherCount)
#endif
		{
			otherCount = 0;

			// Other must be set to handle duplicated items.
			// e.x., [1,2,3] is proper subset of [1,2,3,4,1] but not [1,1,1,1,1]
#if NET35
			var asSet = other as HashSet<T> ?? new HashSet<T>(other);
#else
			var asSet = other as ISet<T> ?? new HashSet<T>(other);
#endif
			int matchCount = 0;

			foreach (var item in asSet)
			{
				otherCount++;

				if (set.Contains(item))
				{
					matchCount++;
				}
			}

			// At least, other contains all items in set, but might be equal.
			return set.Count <= matchCount;
		}

#if NET35
		public static bool IsProperSupersetOf<T>(ICollection<T> set, IEnumerable<T> other)
#else
		public static bool IsProperSupersetOf<T>(ISet<T> set, IEnumerable<T> other)
#endif
		{
			Debug.Assert(set != null, "set != null");
			Ensure.NotNull(other);

			if (other is ICollection<T> asCollection)
			{
				if (asCollection.Count == 0)
				{
					return 0 < set.Count;
				}
			}

			if (!IsSupersetOfCore(set, other, out var checkedCount))
			{
				return false;
			}

			return checkedCount < set.Count;
		}

#if NET35
		public static bool IsSupersetOf<T>(ICollection<T> set, IEnumerable<T> other)
#else
		public static bool IsSupersetOf<T>(ISet<T> set, IEnumerable<T> other)
#endif
		{
			Debug.Assert(set != null, "set != null");
			Ensure.NotNull(other);

			if (other is ICollection<T> asCollection && asCollection.Count < set.Count)
			{
				if (asCollection.Count == 0)
				{
					return true;
				}

				if (set.Count <= asCollection.Count)
				{
					return false;
				}
			}

			return IsSupersetOfCore(set, other, out _);
		}

#if NET35
		private static bool IsSupersetOfCore<T>(ICollection<T> set, IEnumerable<T> other, out int otherCount)
#else
		private static bool IsSupersetOfCore<T>(ISet<T> set, IEnumerable<T> other, out int otherCount)
#endif
		{
			otherCount = 0;

			// Other must be set to handle duplicated items.
			// e.x., [1,2,3] is proper superset of [1,2] and [1,2,1]
#if NET35
			var asSet = other as HashSet<T> ?? new HashSet<T>(other);
#else
			var asSet = other as ISet<T> ?? new HashSet<T>(other);
#endif

			foreach (var item in asSet)
			{
				otherCount++;

				if (!set.Contains(item))
				{
					return false;
				}
			}

			// At least, set contains all items in other, but might be equal.
			return true;
		}

#if NET35
		public static bool Overlaps<T>(ICollection<T> set, IEnumerable<T> other)
#else
		public static bool Overlaps<T>(ISet<T> set, IEnumerable<T> other)
#endif
		{
			Debug.Assert(set != null, "set != null");
			Ensure.NotNull(other);

			if (set.Count == 0)
			{
				return false;
			}

			return other.Any(item => set.Contains(item));
		}

#if NET35
		public static bool SetEquals<T>(ICollection<T> set, IEnumerable<T> other)
#else
		public static bool SetEquals<T>(ISet<T> set, IEnumerable<T> other)
#endif
		{
			Debug.Assert(set != null, "set != null");
			Ensure.NotNull(other);

			if (set.Count == 0)
			{
				if (other is ICollection<T> asCollection)
				{
					return asCollection.Count == 0;
				}
			}

			// Cannot use other.All() here because it always returns true for empty source.
#if NET35
			var asSet = other as HashSet<T> ?? new HashSet<T>(other);
#else
			var asSet = other as ISet<T> ?? new HashSet<T>(other);
#endif
			int matchCount = 0;
			foreach (var item in asSet)
			{
				if (!set.Contains(item))
				{
					return false;
				}
				else
				{
					matchCount++;
				}
			}

			return matchCount == set.Count;
		}
	}
}
