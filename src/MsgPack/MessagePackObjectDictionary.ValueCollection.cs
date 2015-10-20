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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY

namespace MsgPack
{
	partial class MessagePackObjectDictionary
	{
		/// <summary>
		///		Represents the collection of values in a <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
#if !SILVERLIGHT && !NETFX_CORE
		[Serializable]
#endif
		[DebuggerDisplay( "Count={Count}" )]
		[DebuggerTypeProxy( typeof( CollectionDebuggerProxy<> ) )]
		[SuppressMessage( "Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "ICollection implementing dictionary should return ICollection implementing values." )]
		public sealed partial class ValueCollection : ICollection<MessagePackObject>, ICollection
		{
			private readonly MessagePackObjectDictionary _dictionary;

			/// <summary>
			///		Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
			/// </summary>
			/// <returns>
			///		The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
			///   </returns>
			public int Count
			{
				get { return this._dictionary.Count; }
			}

			bool ICollection<MessagePackObject>.IsReadOnly
			{
				get { return true; }
			}

			bool ICollection.IsSynchronized
			{
				get { return false; }
			}

			object ICollection.SyncRoot
			{
				get { return this; }
			}

			internal ValueCollection( MessagePackObjectDictionary dictionary )
			{
#if !UNITY
				Contract.Assert( dictionary != null, "dictionary != null" );
#endif // !UNITY

				this._dictionary = dictionary;
			}

			/// <summary>
			///		Copies the entire collection to a compatible one-dimensional array, starting at the beginning of the target array. 
			/// </summary>
			/// <param name="array">
			///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this dictionary.
			///		The <see cref="Array"/> must have zero-based indexing.
			/// </param>
			public void CopyTo( MessagePackObject[] array )
			{
				if ( array == null )
				{
					throw new ArgumentNullException( "array" );
				}

#if !UNITY
				Contract.EndContractBlock();
#endif // !UNITY


				CollectionOperation.CopyTo( this, this.Count, 0, array, 0, this.Count );
			}

			/// <summary>
			///		Copies the entire collection to a compatible one-dimensional array, 
			///		starting at the specified index of the target array. 
			/// </summary>
			/// <param name="array">
			///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this dictionary.
			///		The <see cref="Array"/> must have zero-based indexing.
			/// </param>
			/// <param name="arrayIndex">
			///		The zero-based index in <paramref name="array"/> at which copying begins. 
			/// </param>
			public void CopyTo( MessagePackObject[] array, int arrayIndex )
			{
				CollectionOperation.CopyTo( this, this.Count, 0, array, arrayIndex, this.Count );
			}

			/// <summary>
			///		Copies a range of elements from this collection to a compatible one-dimensional array, 
			///		starting at the specified index of the target array. 
			/// </summary>
			/// <param name="index">
			///		The zero-based index in the source dictionary at which copying begins. 
			///	</param>
			/// <param name="array">
			///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this dictionary.
			///		The <see cref="Array"/> must have zero-based indexing.
			/// </param>
			/// <param name="arrayIndex">
			///		The zero-based index in <paramref name="array"/> at which copying begins. 
			/// </param>
			/// <param name="count">
			///		The number of elements to copy.
			/// </param>
			public void CopyTo( int index, MessagePackObject[] array, int arrayIndex, int count )
			{
				if ( array == null )
				{
					throw new ArgumentNullException( "array" );
				}

				if ( index < 0 )
				{
					throw new ArgumentOutOfRangeException( "index" );
				}

				if ( 0 < this.Count && this.Count <= index )
				{
					throw new ArgumentException( "Specified array is too small to complete copy operation.", "array" );
				}

				if ( arrayIndex < 0 )
				{
					throw new ArgumentOutOfRangeException( "arrayIndex" );
				}

				if ( count < 0 )
				{
					throw new ArgumentOutOfRangeException( "count" );
				}

				if ( array.Length - count <= arrayIndex )
				{
					throw new ArgumentException( "Specified array is too small to complete copy operation.", "array" );
				}

#if !UNITY
				Contract.EndContractBlock();
#endif // !UNITY

				CollectionOperation.CopyTo( this, this.Count, index, array, arrayIndex, count );
			}

			void ICollection.CopyTo( Array array, int arrayIndex )
			{
				CollectionOperation.CopyTo( this, this.Count, array, arrayIndex );
			}

			/// <summary>
			///		Determines whether this collection contains a specific value.
			/// </summary>
			/// <param name="item">
			///		The object to locate in this collection.</param>
			/// <returns>
			///		<c>true</c> if <paramref name="item"/> is found in this collection; otherwise, <c>false</c>.
			/// </returns>
			bool ICollection<MessagePackObject>.Contains( MessagePackObject item )
			{
				return this._dictionary.ContainsValue( item );
			}

			void ICollection<MessagePackObject>.Add( MessagePackObject item )
			{
				throw new NotSupportedException();
			}

			void ICollection<MessagePackObject>.Clear()
			{
				throw new NotSupportedException();
			}

			bool ICollection<MessagePackObject>.Remove( MessagePackObject item )
			{
				throw new NotSupportedException();
			}

			/// <summary>
			///		Returns an enumerator that iterates through this collction.
			/// </summary>
			/// <returns>
			///		Returns an enumerator that iterates through this collction.
			/// </returns>
			public Enumerator GetEnumerator()
			{
				return new Enumerator( this._dictionary );
			}

			IEnumerator<MessagePackObject> IEnumerable<MessagePackObject>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
	}
}