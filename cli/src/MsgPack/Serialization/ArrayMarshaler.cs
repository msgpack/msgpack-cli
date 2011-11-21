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
using System.Linq;
using System.Text;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Define generic factory methods for generic <see cref="ArrayMarshaler{TCollection}"/> classes.
	/// </summary>
	public static class ArrayMarshaler
	{
		public static ArrayMarshaler<T> Create<T>( MarshalerRepository marshalerRepository, SerializerRepository serializerRepository )
		{
			var traits = typeof( T ).GetCollectionTraits();

			if ( traits.CollectionType == CollectionKind.Array )
			{
				// TODO: Configurable
				return new EmittingArrayMarshaler<T>( traits );
			}
			else
			{
				return null;
			}
		}
	}
}
