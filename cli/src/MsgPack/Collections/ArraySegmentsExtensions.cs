#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MsgPack.Collections
{
	/// <summary>
	///		Define extension methods for collection of <see cref="ArraySegment&lt;T&gt;"/>.
	/// </summary>
	internal static class ArraySegmentsExtensions
	{
		/// <summary>
		///		Returns all items as <see cref="IEnumerable&lt;T&gt;"/> from specified <see cref="ArraySegment&lt;T&gt;"/> collection.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="source"><see cref="ArraySegment&lt;T&gt;"/> collection.</param>
		/// <returns>All items from <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is null.
		/// </exception>
		public static IEnumerable<T> ReadAll<T>( this IEnumerable<ArraySegment<T>> source )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			foreach ( var segment in source )
			{
				for ( int i = 0; i < segment.Count; i++ )
				{
					yield return segment.Array[ i + segment.Offset ];
				}
			}
		}

		/// <summary>
		///		Fill by items from specified <see cref="IEnumerable&lt;T&gt;"/> to specified <see cref="ArraySegment&lt;T&gt;"/> collection.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="source"><see cref="ArraySegment&lt;T&gt;"/> collection to set.</param>
		/// <param name="values">Data source of filling items.</param>
		/// <returns>Count of items which was gotten from <paramref name="values"/> and set to <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> or <paramref name="values"/> is null.
		/// </exception>
		public static long Fill<T>( this IEnumerable<ArraySegment<T>> source, IEnumerable<T> values )
		{
			return Fill( source, values, 0 );
		}

		/// <summary>
		///		Fill by items from specified <see cref="IEnumerable&lt;T&gt;"/> to specified <see cref="ArraySegment&lt;T&gt;"/> collection.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="source"><see cref="ArraySegment&lt;T&gt;"/> collection to set.</param>
		/// <param name="values">Data source of filling items.</param>
		/// <param name="skipCount">Skip count of items until first item to be stored.</param>
		/// <returns>Count of items which was gotten from <paramref name="values"/> and set to <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> or <paramref name="values"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="skipCount"/> is negative.
		/// </exception>
		public static long Fill<T>( this IEnumerable<ArraySegment<T>> source, IEnumerable<T> values, long skipCount )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( values == null )
			{
				throw new ArgumentNullException( "values" );
			}

			if ( skipCount < 0 )
			{
				throw new ArgumentOutOfRangeException( "skipCount", "'skipCount' must not be negative." );
			}

			Contract.EndContractBlock();

			long filled = 0;
			long skipped = 0L;

			using ( var iterator = values.GetEnumerator() )
			{
				foreach ( var segment in source )
				{					
					for ( int i = 0; i < segment.Count; i++ )
					{
						if ( skipped < skipCount )
						{
							skipped++;
							continue;
						}

						if ( !iterator.MoveNext() )
						{
							return filled;
						}

						segment.Array[ i + segment.Offset ] = iterator.Current;
						filled++;
					}
				}
			}

			return filled;
		}
	}
}
