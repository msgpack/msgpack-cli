// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace MsgPack
{
#warning TODO: UNITY KEYSET
	public partial class MessagePackObjectDictionary
	{
#if !UNITY
		public partial class KeySet
#else
		public partial class KeyCollection
#endif // UNITY
		{
#if !UNITY
			/// <summary>
			///		Enumerates the elements of a <see cref="MessagePackObjectDictionary.KeySet"/>.
			/// </summary>
			// ReSharper disable once MemberHidesStaticFromOuterClass
			public struct Enumerator : IEnumerator<MessagePackObject>
#else
			/// <summary>
			///		Enumerates the elements of a <see cref="MessagePackObjectDictionary.KeyCollection"/>.
			/// </summary>
			public struct Enumerator : IEnumerator<MessagePackObject>
#endif // !UNITY
			{
				// This field must not be readonly because it will cause infinite loop to the user of this type 
				// due to C# compiler emit ldfld instead of ldflda and state of this field will never change.
				private MessagePackObjectDictionary.Enumerator _underlying;

				/// <summary>
				///		Gets the element at the current position of the enumerator.
				/// </summary>
				/// <value>
				///		The element in the underlying collection at the current position of the enumerator.
				/// </value>
				public MessagePackObject Current => this._underlying.Current.Key;

				/// <summary>
				///		Gets the element at the current position of the enumerator.
				/// </summary>
				/// <value>
				///		The element in the collection at the current position of the enumerator, as an <see cref="Object"/>.
				/// </value>
				/// <exception cref="InvalidOperationException">
				///		The enumerator is positioned before the first element of the collection or after the last element. 
				/// </exception>
				object IEnumerator.Current => this._underlying.GetCurrentStrict().Key;

				internal Enumerator(MessagePackObjectDictionary dictionary)
				{
					Debug.Assert(dictionary != null, "dictionary != null");

					this._underlying = dictionary.GetEnumerator();
				}

				/// <summary>
				///		Releases all resources used by the this instance.
				/// </summary>
				public void Dispose() => this._underlying.Dispose();

				/// <summary>
				///		Advances the enumerator to the next element of the underlying collection.
				/// </summary>
				/// <returns>
				///		<c>true</c> if the enumerator was successfully advanced to the next element; 
				///		<c>false</c> if the enumerator has passed the end of the collection.
				/// </returns>
				/// <exception cref="T:System.InvalidOperationException">
				///		The collection was modified after the enumerator was created. 
				///	</exception>
				public bool MoveNext()
				{
					this._underlying.VerifyVersion();
					return this._underlying.MoveNext();
				}

				/// <summary>
				///		Sets the enumerator to its initial position, which is before the first element in the collection.
				/// </summary>
				/// <exception cref="T:System.InvalidOperationException">
				///		The collection was modified after the enumerator was created. 
				///	</exception>
				void IEnumerator.Reset() => this._underlying.ResetCore();
			}
		}
	}
}
