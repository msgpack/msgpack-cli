#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2018 FUJIWARA, Yusuke
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
using System.Reflection;

namespace MsgPack.Serialization
{
#if UNITY && DEBUG
	public
#else
	internal
#endif
	struct CollectionTraits
	{
		public static readonly CollectionTraits NotCollection = new CollectionTraits( CollectionDetailedKind.NotCollection, null, null, null, null );
		public static readonly CollectionTraits Unserializable = new CollectionTraits( CollectionDetailedKind.Unserializable, null, null, null, null );

		public readonly Type ElementType;

		public readonly CollectionDetailedKind DetailedCollectionType;

		public CollectionKind CollectionType
		{
			get
			{
				switch ( this.DetailedCollectionType )
				{
					case CollectionDetailedKind.Array:
					case CollectionDetailedKind.GenericCollection:
					case CollectionDetailedKind.GenericEnumerable:
					case CollectionDetailedKind.GenericList:
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
					case CollectionDetailedKind.GenericReadOnlyCollection:
					case CollectionDetailedKind.GenericReadOnlyList:
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
#if !NET35 && !UNITY
					case CollectionDetailedKind.GenericSet:
#endif // !NET35 && !UNITY
					case CollectionDetailedKind.NonGenericCollection:
					case CollectionDetailedKind.NonGenericEnumerable:
					case CollectionDetailedKind.NonGenericList:
					{
						return CollectionKind.Array;
					}
					case CollectionDetailedKind.GenericDictionary:
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
					case CollectionDetailedKind.GenericReadOnlyDictionary:
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
					case CollectionDetailedKind.NonGenericDictionary:
					{
						return CollectionKind.Map;
					}
					case CollectionDetailedKind.NotCollection:
					{
						return CollectionKind.NotCollection;
					}
					case CollectionDetailedKind.Unserializable:
					{
						return CollectionKind.Unserializable;
					}
					default:
					{
						throw new NotSupportedException( "Unknown detailed type:" + this.DetailedCollectionType );
					}
				}
			}
		}

		public readonly MethodInfo GetEnumeratorMethod;
		public readonly MethodInfo AddMethod;
		public readonly MethodInfo CountPropertyGetter;

		public CollectionTraits( CollectionDetailedKind type, Type elementType, MethodInfo getEnumeratorMethod, MethodInfo addMethod, MethodInfo countPropertyGetter )
		{
			this.DetailedCollectionType = type;
			this.ElementType = elementType;
			this.GetEnumeratorMethod = getEnumeratorMethod;
			this.AddMethod = addMethod;
			this.CountPropertyGetter = countPropertyGetter;
		}
	}
}
