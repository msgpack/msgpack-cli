#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
using System.Collections.ObjectModel;

using MsgPack.Serialization.CollectionSerializers;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Provides default implementation for <see cref="EchoKeyedCollection{TKey, TValue}"/>.
	/// </summary>
	/// <typeparam name="TKey">The type of keys of the <see cref="EchoKeyedCollection{TKey, TValue}"/>.</typeparam>
	/// <typeparam name="T">The type of items of the <see cref="EchoKeyedCollection{TKey, TValue}"/>.</typeparam>
	// ReSharper disable once InconsistentNaming
	internal class EchoKeyedCollection_2MessagePackSerializer<TKey, T> : CollectionMessagePackSerializer<EchoKeyedCollection<TKey, T>, T>
	{
		public EchoKeyedCollection_2MessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema itemsSchema )
			: base( ownerContext, itemsSchema ) { }

		protected override EchoKeyedCollection<TKey, T> CreateInstance( int initialCapacity )
		{
			return new EchoKeyedCollection<TKey, T>();
		}
	}
}