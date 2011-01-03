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
using System.IO;

namespace MsgPack.Linq
{
	/// <summary>
	///		Bridges <see cref="Stream"/> as <see cref="IEnumerable&lt;T&gt;">IEnumerable</see>&lt;<see cref="Byte"/>&gt;.
	/// </summary>
	public static class StreamEnumerable
	{
		/// <summary>
		///		Get <see cref="IEnumerable&lt;T&gt;">IEnumerable</see>&lt;<see cref="Byte"/>&gt;
		///		which represents contents of <see cref="Stream"/>.
		/// </summary>
		/// <param name="source">Source <see cref="Stream"/>.</param>
		/// <returns>
		///		<see cref="IEnumerable&lt;T&gt;">IEnumerable</see>&lt;<see cref="Byte"/>&gt;
		///		which represents contents of <paramref name="source"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<see cref="Stream.CanRead"/> of <paramref name="source"/> is false.
		/// </exception>
		/// <remarks>
		///		Note that enumerating return value affects underlying <see cref="Stream.Position"/>.
		///		So you cannot replay stream enumeration when <see cref="Stream.CanSeek"/> is false.
		/// </remarks>
		public static IEnumerable<byte> AsEnumerable( this Stream source )
		{
			return new StreamEnumerator( source );
		}

		private sealed class StreamEnumerator : IEnumerable<byte>
		{
			private readonly Stream _source;

			public StreamEnumerator( Stream source )
			{
				if ( source == null )
				{
					throw new ArgumentNullException( "source" );
				}

				if ( !source.CanRead )
				{
					throw new ArgumentException( "Stream must be readable.", "source" );
				}

				Contract.EndContractBlock();

				this._source = source;
			}

			public IEnumerator<byte> GetEnumerator()
			{
				while ( true )
				{
					int read = this._source.ReadByte();
					if ( read < 0 )
					{
						yield break;
					}

					Contract.Assume( read < Byte.MaxValue );
					yield return unchecked( ( byte )read );
				}
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
	}
}
