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
using System.Reflection;

namespace MsgPack.Serialization
{
	internal sealed class CollectionTraits
	{
		public static readonly CollectionTraits NotCollection = new CollectionTraits( CollectionKind.NotCollection, null, null, null, null );
		public static readonly CollectionTraits Unserializable = new CollectionTraits( CollectionKind.Unserializable, null, null, null, null );

		public readonly MethodInfo GetEnumeratorMethod;
		public readonly MethodInfo AddMethod;
		public readonly PropertyInfo CountProperty;
		public readonly Type ElementType;

		public readonly CollectionKind CollectionType;

		public CollectionTraits( CollectionKind type, MethodInfo addMethod, MethodInfo getEnumeratorMethod, PropertyInfo countProperty, Type elementType )
		{
			this.CollectionType = type;
			this.GetEnumeratorMethod = getEnumeratorMethod; 
			this.AddMethod = addMethod;
			this.CountProperty = countProperty;
			this.ElementType = elementType;
		}
	}
}
