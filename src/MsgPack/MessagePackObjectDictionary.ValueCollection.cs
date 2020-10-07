// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MsgPack
{
	public partial class MessagePackObjectDictionary
	{
		/// <summary>
		///		Represents the collection of values in a <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		[Serializable]
		[DebuggerDisplay("Count={Count}")]
		[DebuggerTypeProxy(typeof(CollectionDebuggerProxy<>))]
		[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "ICollection implementing dictionary should return ICollection implementing values.")]
		public sealed partial class ValueCollection : ICollection<MessagePackObject>, ICollection
		{
			private readonly MessagePackObjectDictionary _dictionary;

			/// <summary>
			///		Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
			/// </summary>
			/// <returns>
			///		The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
			///   </returns>
			public int Count => this._dictionary.Count;

			bool ICollection<MessagePackObject>.IsReadOnly => true;

			bool ICollection.IsSynchronized => false;

			object ICollection.SyncRoot => this;

			internal ValueCollection(MessagePackObjectDictionary dictionary)
			{
				Debug.Assert(dictionary != null, "dictionary != null");

				this._dictionary = dictionary;
			}

			/// <summary>
			///		Copies the entire collection to a compatible one-dimensional array, starting at the beginning of the target array. 
			/// </summary>
			/// <param name="array">
			///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this dictionary.
			///		The <see cref="Array"/> must have zero-based indexing.
			/// </param>
			public void CopyTo(MessagePackObject[] array)
				=> CollectionOperation.CopyTo(this, this.Count, 0, array, 0, this.Count);

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
			public void CopyTo(MessagePackObject[] array, int arrayIndex)
				=> CollectionOperation.CopyTo(this, this.Count, 0, array, arrayIndex, this.Count);

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
			public void CopyTo(int index, MessagePackObject[] array, int arrayIndex, int count)
			{
				Ensure.NotNull(array);
				Ensure.IsNotNegative(index);

				if (0 < this.Count && this.Count <= index)
				{
					throw new ArgumentException("Specified array is too small to complete copy operation.", "array");
				}

				Ensure.IsNotNegative(arrayIndex);
				Ensure.IsNotNegative(count);

				if (array.Length - count <= arrayIndex)
				{
					throw new ArgumentException("Specified array is too small to complete copy operation.", "array");
				}

				CollectionOperation.CopyTo(this, this.Count, index, array, arrayIndex, count);
			}

			void ICollection.CopyTo(Array array, int arrayIndex)
				=> CollectionOperation.CopyTo(this, this.Count, array, arrayIndex);

			/// <summary>
			///		Determines whether this collection contains a specific value.
			/// </summary>
			/// <param name="item">
			///		The object to locate in this collection.</param>
			/// <returns>
			///		<c>true</c> if <paramref name="item"/> is found in this collection; otherwise, <c>false</c>.
			/// </returns>
			bool ICollection<MessagePackObject>.Contains(MessagePackObject item)
				=> this._dictionary.ContainsValue(item);

			void ICollection<MessagePackObject>.Add(MessagePackObject item)
				=> throw new NotSupportedException();

			void ICollection<MessagePackObject>.Clear()
				=> throw new NotSupportedException();

			bool ICollection<MessagePackObject>.Remove(MessagePackObject item)
				=> throw new NotSupportedException();

			/// <summary>
			///		Returns an enumerator that iterates through this collction.
			/// </summary>
			/// <returns>
			///		Returns an enumerator that iterates through this collction.
			/// </returns>
			public Enumerator GetEnumerator() => new Enumerator(this._dictionary);

			IEnumerator<MessagePackObject> IEnumerable<MessagePackObject>.GetEnumerator()
				=> this.GetEnumerator();

			IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
		}
	}
}
