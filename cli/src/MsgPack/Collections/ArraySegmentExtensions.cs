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
using System.Diagnostics.Contracts;
using System.Globalization;

namespace MsgPack.Collections
{
	/// <summary>
	///		Define extension methods for <see cref="ArraySegment&lt;T&gt;"/>.
	/// </summary>
	public static class ArraySegmentExtensions
	{
		/// <summary>
		///		Get sub-segment which has identical offset and specified length for the original <see cref="ArraySegment&lt;T&gt;"/>.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="source">Original <see cref="ArraySegment&lt;T&gt;"/>.</param>
		/// <param name="length">
		///		Length of new sub-segment. 
		///		This value must be lessor than <see cref="ArraySegment&lt;T&gt;.Count">Count</see> of <paramref name="source"/>.
		///	</param>
		/// <returns>Sub-segment.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="length"/> is negative or grator than <see cref="ArraySegment&lt;T&gt;.Count">Count</see> of <paramref name="source"/>.
		/// </exception>
		public static ArraySegment<T> SubSegment<T>( this ArraySegment<T> source, int length )
		{
			if ( length < 0 || length > source.Count )
			{
				throw new ArgumentOutOfRangeException( "length" );
			}

			Contract.EndContractBlock();

			return new ArraySegment<T>( source.Array, source.Offset, length );
		}

		/// <summary>
		///		Devied <see cref="ArraySegment&lt;T&gt;"/> to two sub-segment at specified point.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="source"><see cref="ArraySegment&lt;T&gt;"/> to be devided.</param>
		/// <param name="leastSize">
		///		New length of <paramref name="least"/>.
		///		This value must be lessor than <see cref="ArraySegment&lt;T&gt;.Count">Count</see> of <paramref name="source"/>.
		/// </param>
		/// <param name="least">
		///		Least sub-segment will be stored to this argument.
		/// </param>
		/// <param name="most">
		///		Most sub-segment will be stored to this argument.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="leastSize"/> is negative or grator than <see cref="ArraySegment&lt;T&gt;.Count">Count</see> of <paramref name="source"/>.
		/// </exception>
		public static void Devide<T>( this ArraySegment<T> source, int leastSize, out ArraySegment<T> least, out ArraySegment<T> most )
		{
			if ( leastSize < 0 || leastSize > source.Count )
			{
				throw new ArgumentOutOfRangeException( "leastSize" );
			}

			Contract.EndContractBlock();

			least = new ArraySegment<T>( source.Array, source.Offset, leastSize );
			most = new ArraySegment<T>( source.Array, source.Offset + leastSize, source.Count - leastSize );
		}

		/// <summary>
		///		Get item at specified 'segment' index.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="source"><see cref="ArraySegment&lt;T&gt;"/>.</param>
		/// <param name="indexInSegment">Index. This value is offset from <see cref="ArraySegment&lt;T&gt;.Offset">Offset</see> of <paramref name="source"/>.</param>
		/// <returns>Item at <paramref name="indexInSegment"/> of <paramref name="source"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="indexInSegment"/> is negative or grator than <see cref="ArraySegment&lt;T&gt;.Count">Count</see> of <paramref name="source"/>.
		/// </exception>
		public static T Get<T>( this ArraySegment<T> source, int indexInSegment )
		{
			if ( indexInSegment < 0 || indexInSegment >= source.Count )
			{
				throw new ArgumentOutOfRangeException( "indexInSegment", String.Format( CultureInfo.CurrentCulture, "Index must be between {0} and {1}, but specified value is {2}.", 0, source.Count, indexInSegment ) );
			}

			Contract.EndContractBlock();

			return source.Array[ source.Offset + indexInSegment ];
		}

		/// <summary>
		///		Set item at specified 'segment' index.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="source"><see cref="ArraySegment&lt;T&gt;"/>.</param>
		/// <param name="indexInSegment">Index. This value is offset from <see cref="ArraySegment&lt;T&gt;.Offset">Offset</see> of <paramref name="source"/>.</param>
		/// <param name="value">Value to be set.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="indexInSegment"/> is negative or grator than <see cref="ArraySegment&lt;T&gt;.Count">Count</see> of <paramref name="source"/>.
		/// </exception>
		public static void Set<T>( this ArraySegment<T> source, int indexInSegment, T value )
		{
			if ( indexInSegment < 0 || indexInSegment >= source.Count )
			{
				throw new ArgumentOutOfRangeException( "indexInSegment", String.Format( CultureInfo.CurrentCulture, "Index must be between {0} and {1}, but specified value is {2}.", 0, source.Count, indexInSegment ) );
			}

			Contract.EndContractBlock();

			source.Array[ source.Offset + indexInSegment ] = value;
		}
	}
}
