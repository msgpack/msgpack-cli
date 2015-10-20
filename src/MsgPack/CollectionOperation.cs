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

using System;
using System.Collections.Generic;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Linq;

namespace MsgPack
{
	internal static class CollectionOperation
	{
		public static void CopyTo<T>( IEnumerable<T> source, int sourceCount, int index, T[] array, int arrayIndex, int count )
		{
			ValidateCopyToArguments( sourceCount, index, array, arrayIndex, count );

			var i = 0;
			foreach ( var item in source.Skip( index ).Take( count ) )
			{
				array[ i + arrayIndex ] = item;
				i++;
			}
		}

		public static void CopyTo<TSource, TDestination>( IEnumerable<TSource> source, int sourceCount, int index, TDestination[] array, int arrayIndex, int count, Func<TSource, TDestination> converter )
		{
			ValidateCopyToArguments( sourceCount, index, array, arrayIndex, count );
#if !UNITY
			Contract.Assert( converter != null, "converter != null" );
#endif // !UNITY

			int i = 0;
			foreach ( var item in source.Skip( index ).Take( count ) )
			{
				array[ i + arrayIndex ] = converter( item );
				i++;
			}
		}

		// ReSharper disable UnusedParameter.Local
		private static void ValidateCopyToArguments<T>( int sourceCount, int index, T[] array, int arrayIndex, int count )
		// ReSharper restore UnusedParameter.Local
		{
			if ( array == null )
			{
				throw new ArgumentNullException( "array" );
			}

			if ( index < 0 )
			{
				throw new ArgumentOutOfRangeException( "index" );
			}

			if ( 0 < sourceCount && sourceCount <= index )
			{
				throw new ArgumentException(
					"index is too large to finish copying.",
					"index"
				);
			}

			if ( arrayIndex < 0 )
			{
				throw new ArgumentOutOfRangeException( "arrayIndex" );
			}

			if ( array.Length - arrayIndex < count )
			{
				throw new ArgumentException(
					"array is too small to finish copying.",
					"array"
				);
			}
		}

		public static void CopyTo<T>( IEnumerable<T> source, int sourceCount, Array array, int arrayIndex )
		{
			if ( array == null )
			{
				throw new ArgumentNullException( "array" );
			}

			if ( array.Rank != 1 || array.GetLowerBound( 0 ) != 0 )
			{
				throw new ArgumentException( "array is not zero-based one dimensional array.", "array" );
			}

			if ( arrayIndex < 0 )
			{
				throw new ArgumentOutOfRangeException( "arrayIndex" );
			}

			if ( array.Length - arrayIndex < sourceCount )
			{
				throw new ArgumentException(
					"array is too small to finish copying.",
					"array"
				);
			}

			var asVector = array as T[];
			if ( asVector != null )
			{
				CopyTo( source, sourceCount, 0, asVector, arrayIndex, asVector.Length );
				return;
			}

			int i = 0;
			foreach ( var item in source )
			{
				try
				{
					array.SetValue( item, i + arrayIndex );
					i++;
				}
				catch ( InvalidCastException )
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
