#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

#if !UNITY
using System;
using System.Collections.Generic;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Linq;


namespace MsgPack
{
	/// <summary>
	///		Implements basic (maybe naive) implementation for common Set&lt;T&gt; operation.
	/// </summary>
	internal static class SetOperation
	{
		[Pure]
#if NETFX_35
		public static bool IsProperSubsetOf<T>( ICollection<T> set, IEnumerable<T> other )
#else
		public static bool IsProperSubsetOf<T>( ISet<T> set, IEnumerable<T> other )
#endif
		{
			#region CONTRACT
#if DEBUG
			Contract.Assert( set != null, "set != null " );
#endif // DEBUG

			if ( other == null )
			{
				throw new ArgumentNullException( "other" );
			}
			#endregion CONTRACT

			var asCollection = other as ICollection<T>;
			if ( asCollection != null )
			{
				if ( set.Count == 0 )
				{
					return 0 < asCollection.Count;
				}

				if ( asCollection.Count <= set.Count )
				{
					return false;
				}
			}

			int otherCount;
			if ( !IsSubsetOfCore( set, other, out otherCount ) )
			{
				return false;
			}

			return set.Count < otherCount;
		}

		[Pure]
#if NETFX_35
		public static bool IsSubsetOf<T>( ICollection<T> set, IEnumerable<T> other )
#else
		public static bool IsSubsetOf<T>( ISet<T> set, IEnumerable<T> other )
#endif
		{
			#region CONTRACT
#if DEBUG
			Contract.Assert( set != null, "set != null" );
#endif // DEBUG

			if ( other == null )
			{
				throw new ArgumentNullException( "other" );
			}
			#endregion CONTRACT

			if ( set.Count == 0 )
			{
				return true;
			}

			var asCollection = other as ICollection<T>;
			if ( asCollection != null && asCollection.Count < set.Count )
			{
				return false;
			}

			int checkedCount;
			return IsSubsetOfCore( set, other, out checkedCount );
		}

		[Pure]
#if NETFX_35
		private static bool IsSubsetOfCore<T>( ICollection<T> set, IEnumerable<T> other, out int otherCount )
#else
		private static bool IsSubsetOfCore<T>( ISet<T> set, IEnumerable<T> other, out int otherCount )
#endif
		{
			otherCount = 0;

			// Other must be set to handle duplicated items.
			// e.x., [1,2,3] is proper subset of [1,2,3,4,1] but not [1,1,1,1,1]
#if NETFX_35
			var asSet = other as HashSet<T>;
#else
			var asSet = other as ISet<T>;
#endif
			if ( asSet == null )
			{
				asSet = new HashSet<T>( other );
			}

			int matchCount = 0;

			foreach ( var item in asSet )
			{
				otherCount++;

				if ( set.Contains( item ) )
				{
					matchCount++;
				}
			}

			// At least, other contains all items in set, but might be equal.
			return set.Count <= matchCount;
		}

		[Pure]
#if NETFX_35
		public static bool IsProperSupersetOf<T>( ICollection<T> set, IEnumerable<T> other )
#else
		public static bool IsProperSupersetOf<T>( ISet<T> set, IEnumerable<T> other )
#endif
		{
			#region CONTRACT
#if DEBUG
			Contract.Assert( set != null, "set != null" );
#endif // DEBUG

			if ( other == null )
			{
				throw new ArgumentNullException( "other" );
			}
			#endregion CONTRACT

			var asCollection = other as ICollection<T>;
			if ( asCollection != null )
			{
				if ( asCollection.Count == 0 )
				{
					return 0 < set.Count;
				}
			}

			int checkedCount;
			if ( !IsSupersetOfCore( set, other, out checkedCount ) )
			{
				return false;
			}

			return checkedCount < set.Count;
		}

		[Pure]
#if NETFX_35
		public static bool IsSupersetOf<T>( ICollection<T> set, IEnumerable<T> other )
#else
		public static bool IsSupersetOf<T>( ISet<T> set, IEnumerable<T> other )
#endif
		{
			#region CONTRACT
#if DEBUG
			Contract.Assert( set != null, "set != null" );
#endif // DEBUG

			if ( other == null )
			{
				throw new ArgumentNullException( "other" );
			}
			#endregion CONTRACT

			var asCollection = other as ICollection<T>;
			if ( asCollection != null && asCollection.Count < set.Count )
			{
				if ( asCollection.Count == 0 )
				{
					return true;
				}

				if ( set.Count <= asCollection.Count )
				{
					return false;
				}
			}

			int checkedCount;
			return IsSupersetOfCore( set, other, out checkedCount );
		}

		[Pure]
#if NETFX_35
		private static bool IsSupersetOfCore<T>( ICollection<T> set, IEnumerable<T> other, out int otherCount )
#else
		private static bool IsSupersetOfCore<T>( ISet<T> set, IEnumerable<T> other, out int otherCount )
#endif
		{
			otherCount = 0;

			// Other must be set to handle duplicated items.
			// e.x., [1,2,3] is proper superset of [1,2] and [1,2,1]
#if NETFX_35
			var asSet = other as HashSet<T>;
#else
			var asSet = other as ISet<T>;
#endif
			if ( asSet == null )
			{
				asSet = new HashSet<T>( other );
			}

			foreach ( var item in asSet )
			{
				otherCount++;

				if ( !set.Contains( item ) )
				{
					return false;
				}
			}

			// At least, set contains all items in other, but might be equal.
			return true;
		}

		[Pure]
#if NETFX_35
		public static bool Overlaps<T>( ICollection<T> set, IEnumerable<T> other )
#else
		public static bool Overlaps<T>( ISet<T> set, IEnumerable<T> other )
#endif
		{
			#region CONTRACT
#if DEBUG
			Contract.Assert( set != null, "set != null" );
#endif // DEBUG

			if ( other == null )
			{
				throw new ArgumentNullException( "other" );
			}
			#endregion CONTRACT

			if ( set.Count == 0 )
			{
				return false;
			}

			return other.Any( item => set.Contains( item ) );
		}

		[Pure]
#if NETFX_35
		public static bool SetEquals<T>( ICollection<T> set, IEnumerable<T> other )
#else
		public static bool SetEquals<T>( ISet<T> set, IEnumerable<T> other )
#endif
		{
			#region CONTRACT
#if DEBUG
			Contract.Assert( set != null, "set != null" );
#endif // DEBUG

			if ( other == null )
			{
				throw new ArgumentNullException( "other" );
			}
			#endregion CONTRACT

			if ( set.Count == 0 )
			{
				var asCollection = other as ICollection<T>;
				if ( asCollection != null )
				{
					return asCollection.Count == 0;
				}
			}

			// Cannot use other.All() here because it always returns true for empty source.
#if NETFX_35
			var asSet = other as HashSet<T> ?? new HashSet<T>( other );
#else
			var asSet = other as ISet<T> ?? new HashSet<T>( other );
#endif
			int matchCount = 0;
			foreach ( var item in asSet )
			{
				if ( !set.Contains( item ) )
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
#endif // !UNITY
