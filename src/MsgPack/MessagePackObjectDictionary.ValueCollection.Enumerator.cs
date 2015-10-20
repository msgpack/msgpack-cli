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
		partial class ValueCollection
		{
			/// <summary>
			///		Enumerates the elements of a <see cref="MessagePackObjectDictionary.ValueCollection"/>.
			/// </summary>
			// ReSharper disable once MemberHidesStaticFromOuterClass
			public struct Enumerator : IEnumerator<MessagePackObject>
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
					get { return this._underlying.Current.Value; }
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
					get { return this._underlying.GetCurrentStrict().Value; }
				}

				internal Enumerator( MessagePackObjectDictionary dictionary )
				{
#if !UNITY
					Contract.Assert( dictionary != null, "dictionary != null" );
#endif // !UNITY

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