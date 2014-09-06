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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Reflection;

namespace MsgPack.Serialization
{
	internal sealed class CollectionTraits
	{
		public static readonly CollectionTraits NotCollection = new CollectionTraits( CollectionDetailedKind.NotCollection, null, null, null, null );
		public static readonly CollectionTraits Unserializable = new CollectionTraits( CollectionDetailedKind.Unserializable, null, null, null, null );

#if UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields" )]
#endif // UNITY
		public readonly MethodInfo GetEnumeratorMethod;
		public readonly MethodInfo AddMethod;
#if UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields" )]
#endif // UNITY
		public readonly PropertyInfo CountProperty;
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
#if !NETFX_35 && !UNITY
					case CollectionDetailedKind.GenericSet:
#endif // !NETFX_35 && !UNITY
					case CollectionDetailedKind.NonGenericCollection:
					case CollectionDetailedKind.NonGenericEnumerable:
					case CollectionDetailedKind.NonGenericList:
					{
						return CollectionKind.Array;
					}
					case CollectionDetailedKind.GenericDictionary:
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

		public CollectionTraits( CollectionDetailedKind type, MethodInfo addMethod, MethodInfo getEnumeratorMethod, PropertyInfo countProperty, Type elementType )
		{
			this.DetailedCollectionType = type;
			this.GetEnumeratorMethod = getEnumeratorMethod;
			this.AddMethod = addMethod;
			this.CountProperty = countProperty;
			this.ElementType = elementType;
		}
	}
}
