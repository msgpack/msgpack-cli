#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MsgPack
{
	partial class MessagePackObjectDictionary
	{
#if !WINDOWS_PHONE
		partial class KeySet
#else
		partial class KeyCollection
#endif
		{
#if !WINDOWS_PHONE
			/// <summary>
			///		Enumerates the elements of a <see cref="MessagePackObjectDictionary.KeySet"/>.
			/// </summary>
			public struct Enumerator : IEnumerator<MessagePackObject>
#else
			/// <summary>
			///		Enumerates the elements of a <see cref="MessagePackObjectDictionary.KeyCollection"/>.
			/// </summary>
			public struct Enumerator : IEnumerator<MessagePackObject>
#endif
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
				public MessagePackObject Current
				{
					get { return this._underlying.Current.Key; }
				}

				/// <summary>
				///		Gets the element at the current position of the enumerator.
				/// </summary>
				/// <value>
				///		The element in the collection at the current position of the enumerator, as an <see cref="Object"/>.
				/// </value>
				/// <exception cref="InvalidOperationException">
				///		The enumerator is positioned before the first element of the collection or after the last element. 
				/// </exception>
				object IEnumerator.Current
				{
					get { return this._underlying.GetCurrentStrict().Key; }
				}

				internal Enumerator( MessagePackObjectDictionary dictionary )
				{
					Contract.Assert( dictionary != null );

					this._underlying = dictionary.GetEnumerator();
				}

				/// <summary>
				///		Releases all resources used by the this instance.
				/// </summary>
				public void Dispose()
				{
					this._underlying.Dispose();
				}

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
				void IEnumerator.Reset()
				{
					this._underlying.ResetCore();
				}
			}
		}
	}
}