// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace MsgPack.Serialization
{
	internal readonly struct CollectionTraits
	{
		public static readonly CollectionTraits NotCollection = new CollectionTraits(CollectionDetailedKind.NotCollection, null, null, null, null, null, null);
		public static readonly CollectionTraits Unserializable = new CollectionTraits(CollectionDetailedKind.Unserializable, null, null, null, null, null, null);

		public Type? ElementType { get; }

		public CollectionDetailedKind DetailedCollectionType { get; }

		public CollectionKind CollectionType
		{
			get
			{
				switch (this.DetailedCollectionType)
				{
					case CollectionDetailedKind.Array:
					case CollectionDetailedKind.GenericCollection:
					case CollectionDetailedKind.GenericEnumerable:
					case CollectionDetailedKind.GenericList:
#if !NET35 && !UNITY && !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
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
#if !NET35 && !UNITY && !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
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
						throw new NotSupportedException("Unknown detailed type:" + this.DetailedCollectionType);
					}
				}
			}
		}

		public MethodInfo? GetEnumeratorMethod { get; }
		public MethodInfo? AddMethod { get; }
		public MethodInfo? CountPropertyGetter { get; }

		public ConstructorInfo? ConstructorWithCapacity { get; }
		public ConstructorInfo? DefaultConstructor { get; }

		public CollectionTraits(CollectionDetailedKind type, ConstructorInfo? constructorWithCapacity, ConstructorInfo? defaultConstructor, Type? elementType, MethodInfo? getEnumeratorMethod, MethodInfo? addMethod, MethodInfo? countPropertyGetter)
		{
			this.DetailedCollectionType = type;
			this.ElementType = elementType;
			this.GetEnumeratorMethod = getEnumeratorMethod;
			this.AddMethod = addMethod;
			this.CountPropertyGetter = countPropertyGetter;
			this.ConstructorWithCapacity = constructorWithCapacity;
			this.DefaultConstructor = defaultConstructor;
		}

		public (Type? KeyType, Type? ValueType) GetKeyValueType()
		{
			if (this.ElementType == null || this.CollectionType != CollectionKind.Map)
			{
				return (null, null);
			}

			if (this.ElementType == typeof(DictionaryEntry))
			{
				return (typeof(object), typeof(object));
			}

			Debug.Assert(this.ElementType.IsGenericType, $"this.ElementType.IsGenericType ({this.ElementType})");
			Debug.Assert(this.ElementType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>), $"this.ElementType.GetGenericTypeDefinition()({this.ElementType}) == typeof(KeyValuePair<,>)");

			var typeArguments = this.ElementType.GetGenericTypeParameters();
			Debug.Assert(typeArguments.Length == 2, $"typeArguments.Length ({typeArguments.Length}) == 2");

			return (typeArguments[0], typeArguments[1]);
		}
	}
}
