// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MsgPack
{
	internal static class CollectionOperation
	{
		public static void CopyTo<T>(IEnumerable<T> source, int sourceCount, int index, T[] array, int arrayIndex, int count)
		{
			ValidateCopyToArguments(sourceCount, index, array, arrayIndex, count);

			var i = 0;
			foreach (var item in source.Skip(index).Take(count))
			{
				array[i + arrayIndex] = item;
				i++;
			}
		}

		public static void CopyTo<TSource, TDestination>(IEnumerable<TSource> source, int sourceCount, int index, TDestination[] array, int arrayIndex, int count, Func<TSource, TDestination> converter)
		{
			ValidateCopyToArguments(sourceCount, index, array, arrayIndex, count);
			Debug.Assert(converter != null, "converter != null");

			int i = 0;
			foreach (var item in source.Skip(index).Take(count))
			{
				array[i + arrayIndex] = converter(item);
				i++;
			}
		}

		// ReSharper disable UnusedParameter.Local
		private static void ValidateCopyToArguments<T>(int sourceCount, int index, T[] array, int arrayIndex, int count)
		// ReSharper restore UnusedParameter.Local
		{
			Ensure.NotNull(array);
			Ensure.IsNotNegative(index);

			if (0 < sourceCount && sourceCount <= index)
			{
				throw new ArgumentException(
					"'index' is too large to finish copying.",
					"index"
				);
			}

			Ensure.IsNotNegative(arrayIndex);

			if (array.Length - arrayIndex < count)
			{
				throw new ArgumentException(
					"'array' is too small to finish copying.",
					"array"
				);
			}
		}

		public static void CopyTo<T>(IEnumerable<T> source, int sourceCount, Array array, int arrayIndex)
		{
			Ensure.NotNull(array);

			if (array.Rank != 1 || array.GetLowerBound(0) != 0)
			{
				throw new ArgumentException("array is not zero-based one dimensional array.", "array");
			}

			Ensure.IsNotNegative(arrayIndex);

			if (array.Length - arrayIndex < sourceCount)
			{
				throw new ArgumentException(
					"array is too small to finish copying.",
					"array"
				);
			}

			var asVector = array as T[];
			if (asVector != null)
			{
				CopyTo(source, sourceCount, 0, asVector, arrayIndex, asVector.Length);
				return;
			}

			int i = 0;
			foreach (var item in source)
			{
				try
				{
					array.SetValue(item, i + arrayIndex);
					i++;
				}
				catch (InvalidCastException)
				{
					throw new ArgumentException(
						"The type of destination array is not compatible to the type of items in the collection.",
						"array"
					);
				}
			}
		}
	}
}
