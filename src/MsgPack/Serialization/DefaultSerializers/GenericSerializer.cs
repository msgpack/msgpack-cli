#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2012-2015 FUJIWARA, Yusuke
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
#if !UNITY
using System.Collections;
#endif // !UNITY
using System.Collections.Generic;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		Defines serializer factory for well known structured types.
	/// </summary>
	internal static class GenericSerializer
	{
		public static MessagePackSerializer<T> Create<T>( SerializationContext context, PolymorphismSchema schema )
		{
#if !UNITY
			return Create( context, typeof( T ), schema ) as MessagePackSerializer<T>;
#else
			return MessagePackSerializer.Wrap<T>( context, Create( context, typeof( T ), schema ) );
#endif // !UNITY
		}

		public static IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
		{
			if ( targetType.IsArray )
			{
				return CreateArraySerializer( context, targetType, ( schema ?? PolymorphismSchema.Default ).ItemSchema );
			}

			Type nullableUnderlyingType;
			if ( ( nullableUnderlyingType = Nullable.GetUnderlyingType( targetType ) ) != null )
			{
#if !UNITY
				return CreateNullableSerializer( context, nullableUnderlyingType, schema );
#else
				return CreateNullableSerializer( context, targetType, nullableUnderlyingType );
#endif // !UNITY
			}

			if ( targetType.GetIsGenericType() )
			{
				var typeDefinition = targetType.GetGenericTypeDefinition();
				if ( typeDefinition == typeof( List<> ) )
				{
#if !UNITY
					return CreateListSerializer( context, targetType.GetGenericArguments()[ 0 ], schema );
#else
					return CreateListSerializer( context, targetType, targetType.GetCollectionTraits(), schema );
#endif // !UNITY
				}

				if ( typeDefinition == typeof( Dictionary<,> ) )
				{
					var genericTypeArguments = targetType.GetGenericArguments();
#if !UNITY
					return CreateDictionarySerializer( context, genericTypeArguments[ 0 ], genericTypeArguments[ 1 ], schema );
#else
					return CreateDictionarySerializer( context, targetType, targetType.GetCollectionTraits(), genericTypeArguments[ 0 ], genericTypeArguments[ 1 ], schema );
#endif // !UNITY
				}
			}

#if !UNITY
			return TryCreateImmutableCollectionSerializer( context, targetType, schema );
#else
			return null;
#endif // !UNITY
		}

		private static IMessagePackSingleObjectSerializer CreateArraySerializer( SerializationContext context, Type targetType, PolymorphismSchema itemsSchema )
		{
#if DEBUG && !UNITY
			Contract.Assert( targetType.IsArray, "targetType.IsArray" );
#endif // DEBUG && !UNITY
			return ArraySerializer.Create( context, targetType, itemsSchema );
		}

#if !UNITY
		private static IMessagePackSingleObjectSerializer CreateNullableSerializer( SerializationContext context, Type underlyingType, PolymorphismSchema schema )
		{
			return
				ReflectionExtensions.CreateInstancePreservingExceptionType<IGenericBuiltInSerializerFactory>(
					typeof( NullableInstanceFactory<> ).MakeGenericType( underlyingType )
				).Create( context, schema );
		}
#else
		private static IMessagePackSingleObjectSerializer CreateNullableSerializer( SerializationContext context, Type nullableType, Type underlyingType )
		{
			return new NullableMessagePackSerializer( context, nullableType, underlyingType );
		}
#endif // !UNITY

#if !UNITY
		private static IMessagePackSingleObjectSerializer CreateListSerializer( SerializationContext context, Type itemType, PolymorphismSchema schema )
		{
#if DEBUG && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
			if ( SerializerDebugging.AvoidsGenericSerializer )
			{
				return null;
			}
#endif // DEBUG && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
			return
				ReflectionExtensions.CreateInstancePreservingExceptionType<IGenericBuiltInSerializerFactory>(
					typeof( ListInstanceFactory<> ).MakeGenericType( itemType )
				).Create( context, schema );
		}
#else
		private static IMessagePackSingleObjectSerializer CreateListSerializer( SerializationContext context, Type targetType, CollectionTraits traits, PolymorphismSchema schema )
		{
			return new System_Collections_Generic_List_1MessagePackSerializer( context, targetType, traits, ( schema ?? PolymorphismSchema.Default ).ItemSchema );
		}
#endif // !UNITY

#if !UNITY
		private static IMessagePackSingleObjectSerializer CreateDictionarySerializer( SerializationContext context, Type keyType, Type valueType, PolymorphismSchema schema )
		{
#if DEBUG && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
			if ( SerializerDebugging.AvoidsGenericSerializer )
			{
				return null;
			}
#endif // DEBUG && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
			return
				ReflectionExtensions.CreateInstancePreservingExceptionType<IGenericBuiltInSerializerFactory>(
					typeof( DictionaryInstanceFactory<,> ).MakeGenericType( keyType, valueType )
				).Create( context, schema );
		}
#else
		private static IMessagePackSingleObjectSerializer CreateDictionarySerializer( SerializationContext context, Type targetType, CollectionTraits traits, Type keyType, Type valueType, PolymorphismSchema schema )
		{
			var itemSchema = schema ?? PolymorphismSchema.Default;
			return
				new System_Collections_Generic_Dictionary_2MessagePackSerializer(
					context,
					targetType,
					traits,
					keyType,
					valueType,
					itemSchema.KeySchema,
					itemSchema.ItemSchema 
				);
		}
#endif // !UNITY

#if !UNITY
#if SILVERLIGHT || NETFX_35 || NETFX_40
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "context", Justification = "Used in other platform" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "targetType", Justification = "Used in other platform" )]
#endif // SILVERLIGHT
		// ReSharper disable UnusedParameter.Local
		private static IMessagePackSingleObjectSerializer TryCreateImmutableCollectionSerializer( SerializationContext context, Type targetType, PolymorphismSchema schema )
		{
#if NETFX_35 || NETFX_40 || SILVERLIGHT
			// ImmutableCollections does not support above platforms.
			return null;
#else
			if ( targetType.Namespace != "System.Collections.Immutable" )
			{
				return null;
			}

			if ( !targetType.GetIsGenericType() )
			{
				return null;
			}

			var itemSchema = ( schema ?? PolymorphismSchema.Default );
			switch ( DetermineImmutableCollectionType(targetType) )
			{
				case ImmutableCollectionType.ImmutableArray:
				case ImmutableCollectionType.ImmutableList:
				case ImmutableCollectionType.ImmutableHashSet:
				case ImmutableCollectionType.ImmutableSortedSet:
				case ImmutableCollectionType.ImmutableQueue:
				{
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IGenericBuiltInSerializerFactory>(
							typeof( ImmutableCollectionSerializerFactory<,> ).MakeGenericType( targetType, targetType.GetGenericArguments()[ 0 ] )
						).Create( context, itemSchema );
				}
				case ImmutableCollectionType.ImmutableStack:
				{
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IGenericBuiltInSerializerFactory>(
							typeof( ImmutableStackSerializerFactory<,> ).MakeGenericType( targetType, targetType.GetGenericArguments()[ 0 ] )
						).Create( context, itemSchema );
				}
				case ImmutableCollectionType.ImmutableDictionary:
				case ImmutableCollectionType.ImmutableSortedDictionary:
				{
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IGenericBuiltInSerializerFactory>(
							typeof( ImmutableDictionarySerializerFactory<,,> ).MakeGenericType( targetType, targetType.GetGenericArguments()[ 0 ], targetType.GetGenericArguments()[ 1 ] )
						).Create( context, itemSchema );
				}
				default:
				{
#if DEBUG && !UNITY
					Contract.Assert( false, "Unknown type:" + targetType );
#endif // DEBUG && !UNITY
					// ReSharper disable HeuristicUnreachableCode
					return null;
					// ReSharper restore HeuristicUnreachableCode
				}
			}
#endif // NETFX_35 || NETFX_40 || SILVERLIGHT
		}

#if !NETFX_35 && !NETFX_40 && !SILVERLIGHT
		private static ImmutableCollectionType DetermineImmutableCollectionType( Type targetType )
		{
			if ( targetType.Namespace != "System.Collections.Immutable" )
			{
				return ImmutableCollectionType.Unknown;
			}

			if ( !targetType.GetIsGenericType() )
			{
				return ImmutableCollectionType.Unknown;
			}

			switch ( targetType.GetGenericTypeDefinition().Name )
			{
				case "ImmutableArray`1":
				{
					return ImmutableCollectionType.ImmutableArray;
				}
				case "ImmutableList`1":
				{
					return ImmutableCollectionType.ImmutableList;
				}
				case "ImmutableHashSet`1":
				{
					return ImmutableCollectionType.ImmutableHashSet;
				}
				case "ImmutableSortedSet`1":
				{
					return ImmutableCollectionType.ImmutableSortedSet;
				}
				case "ImmutableQueue`1":
				{
					return ImmutableCollectionType.ImmutableQueue;
				}
				case "ImmutableStack`1":
				{
					return ImmutableCollectionType.ImmutableStack;
				}
				case "ImmutableDictionary`2":
				{
					return ImmutableCollectionType.ImmutableDictionary;
				}
				case "ImmutableSortedDictionary`2":
				{
					return ImmutableCollectionType.ImmutableSortedDictionary;
				}
				default:
				{
#if DEBUG && !UNITY
					Contract.Assert( false, "Unknown type:" + targetType );
#endif // DEBUG && !UNITY
					// ReSharper disable HeuristicUnreachableCode
					return ImmutableCollectionType.Unknown;
					// ReSharper restore HeuristicUnreachableCode
				}
			}
		}
#endif // !NETFX_35 && !NETFX_40 && !SILVERLIGHT
#endif // !UNITY

		public static IMessagePackSingleObjectSerializer TryCreateAbstractCollectionSerializer( SerializationContext context, Type abstractType, Type concreteType, PolymorphismSchema schema )
		{
			if ( concreteType == null )
			{
				return null;
			}

			return
				TryCreateAbstractCollectionSerializer(
					context,
					abstractType,
					concreteType,
					schema,
					abstractType.GetCollectionTraits()
				);
		}

		internal static IMessagePackSingleObjectSerializer TryCreateAbstractCollectionSerializer( SerializationContext context, Type abstractType, Type concreteType, PolymorphismSchema schema, CollectionTraits traits )
		{
			switch ( traits.DetailedCollectionType )
			{
				case CollectionDetailedKind.GenericList:
#if !NETFX_35 && !UNITY
				case CollectionDetailedKind.GenericSet:
#endif // !NETFX_35 && !UNITY
				case CollectionDetailedKind.GenericCollection:
				{
#if !UNITY
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantSerializerFactory>(
							typeof( CollectionSerializerFactory<,> ).MakeGenericType( abstractType, traits.ElementType )
						).Create( context, concreteType, schema );
#else
					return new AbstractCollectionMessagePackSerializer( context, abstractType, concreteType, traits, schema );
#endif
				}
#if !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyList:
				case CollectionDetailedKind.GenericReadOnlyCollection:
				{
#if !UNITY
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantSerializerFactory>(
							typeof( ReadOnlyCollectionSerializerFactory<,> ).MakeGenericType( abstractType, traits.ElementType )
						).Create( context, concreteType, schema );
#else
					return new AbstractCollectionMessagePackSerializer( context, abstractType, concreteType, traits, schema );
#endif
				}
#endif // !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericEnumerable:
				{
#if !UNITY
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantSerializerFactory>(
							typeof( EnumerableSerializerFactory<,> ).MakeGenericType( abstractType, traits.ElementType )
						).Create( context, concreteType, schema );
#else
					return new AbstractEnumerableMessagePackSerializer( context, abstractType, concreteType, traits, schema );
#endif
				}
				case CollectionDetailedKind.GenericDictionary:
				{
					var genericArgumentOfKeyValuePair = traits.ElementType.GetGenericArguments();
#if !UNITY
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantSerializerFactory>(
							typeof( DictionarySerializerFactory<,,> ).MakeGenericType(
								abstractType,
								genericArgumentOfKeyValuePair[ 0 ],
								genericArgumentOfKeyValuePair[ 1 ]
							)
						).Create( context, concreteType, schema );
#else
					return new AbstractDictionaryMessagePackSerializer( context, abstractType, concreteType, genericArgumentOfKeyValuePair[ 0 ], genericArgumentOfKeyValuePair[ 1 ], traits, schema );
#endif
				}
#if !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyDictionary:
				{
					var genericArgumentOfKeyValuePair = traits.ElementType.GetGenericArguments();
#if !UNITY
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantSerializerFactory>(
							typeof( ReadOnlyDictionarySerializerFactory<,,> ).MakeGenericType(
								abstractType,
								genericArgumentOfKeyValuePair[ 0 ],
								genericArgumentOfKeyValuePair[ 1 ]
							)
						).Create( context, concreteType, schema );
#else
					return new AbstractDictionaryMessagePackSerializer( context, abstractType, concreteType, genericArgumentOfKeyValuePair[ 0 ], genericArgumentOfKeyValuePair[ 1 ], traits, schema );
#endif
				}
#endif // !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.NonGenericList:
				{
#if !UNITY
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantSerializerFactory>(
							typeof( NonGenericListSerializerFactory<> ).MakeGenericType( abstractType )
						).Create( context, concreteType, schema );
#else
					return new AbstractNonGenericListMessagePackSerializer( context, abstractType, concreteType, schema );
#endif
				}
				case CollectionDetailedKind.NonGenericCollection:
				{
#if !UNITY
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantSerializerFactory>(
							typeof( NonGenericCollectionSerializerFactory<> ).MakeGenericType( abstractType )
						).Create( context, concreteType, schema );
#else
					return new AbstractNonGenericCollectionMessagePackSerializer( context, abstractType, concreteType, schema );
#endif
				}
				case CollectionDetailedKind.NonGenericEnumerable:
				{
#if !UNITY
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantSerializerFactory>(
							typeof( NonGenericEnumerableSerializerFactory<> ).MakeGenericType( abstractType )
						).Create( context, concreteType, schema );
#else
					return new AbstractNonGenericEnumerableMessagePackSerializer( context, abstractType, concreteType, schema );
#endif
				}
				case CollectionDetailedKind.NonGenericDictionary:
				{
#if !UNITY
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantSerializerFactory>(
							typeof( NonGenericDictionarySerializerFactory<> ).MakeGenericType( abstractType )
						).Create( context, concreteType, schema );
#else
					return new AbstractNonGenericDictionaryMessagePackSerializer( context, abstractType, concreteType, schema );
#endif
				}
				default:
				{
					return null;
				}
			}
		}


		/// <summary>
		///		Determines whether the specified type is supported from this class.
		/// </summary>
		/// <param name="type">The type to be determined.</param>
		/// <param name="traits">The known <see cref="CollectionTraits"/> of the <paramref name="type"/>.</param>
		/// <param name="preferReflectionBasedSerializer"><c>true</c> to prefer reflection based collection serializers instead of dyhnamic generated serializers.</param>
		/// <returns><c>true</c> when the <paramref name="type"/> is supported; otherwise, <c>false</c>.</returns>
		internal static bool IsSupported( Type type, CollectionTraits traits, bool preferReflectionBasedSerializer )
		{
			if ( type.IsArray )
			{
				return true;
			}

			if ( Nullable.GetUnderlyingType( type ) != null )
			{
				return true;
			}

			if ( type.GetIsGenericType() )
			{
				var typeDefinition = type.GetGenericTypeDefinition();
				if ( typeDefinition == typeof( List<> ) )
				{
					return true;
				}

				if ( typeDefinition == typeof( Dictionary<,> ) )
				{
					return true;
				}
			}

#if !UNITY && !NETFX_35 && !NETFX_40 && !SILVERLIGHT
			// ImmutableCollections does not support above platforms.
			if ( DetermineImmutableCollectionType( type ) != ImmutableCollectionType.Unknown )
			{
				return true;
			}
#endif // !UNITY && !NETFX_35 && !NETFX_40 && !SILVERLIGHT

			if ( preferReflectionBasedSerializer )
			{
				return
					traits.DetailedCollectionType != CollectionDetailedKind.NotCollection
					&& traits.DetailedCollectionType != CollectionDetailedKind.Unserializable;
			}

			return false;
		}

#if !UNITY
		/// <summary>
		///		Defines non-generic factory method for built-in serializers which require generic type argument.
		/// </summary>
		private interface IGenericBuiltInSerializerFactory
		{
			IMessagePackSingleObjectSerializer Create( SerializationContext context, PolymorphismSchema schema );
		}

		private sealed class NullableInstanceFactory<T> : IGenericBuiltInSerializerFactory
			where T : struct
		{
			public NullableInstanceFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				return new NullableMessagePackSerializer<T>( context );
			}
		}

		private sealed class ListInstanceFactory<T> : IGenericBuiltInSerializerFactory
		{
			public ListInstanceFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new System_Collections_Generic_List_1MessagePackSerializer<T>( context, itemSchema.ItemSchema );
			}
		}

		private sealed class DictionaryInstanceFactory<TKey, TValue> : IGenericBuiltInSerializerFactory
		{
			public DictionaryInstanceFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new System_Collections_Generic_Dictionary_2MessagePackSerializer<TKey, TValue>( context, itemSchema.KeySchema, itemSchema.ItemSchema );
			}
		}

#if !NETFX_35 && !NETFX_40 && !SILVERLIGHT

		private sealed class ImmutableCollectionSerializerFactory<T, TItem> : IGenericBuiltInSerializerFactory
			where T : IEnumerable<TItem>
		{
			public ImmutableCollectionSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new ImmutableCollectionSerializer<T, TItem>( context, itemSchema.ItemSchema );
			}
		}

		private sealed class ImmutableStackSerializerFactory<T, TItem> : IGenericBuiltInSerializerFactory
			where T : IEnumerable<TItem>
		{
			public ImmutableStackSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new ImmutableStackSerializer<T, TItem>( context, itemSchema.ItemSchema );
			}
		}

		private sealed class ImmutableDictionarySerializerFactory<T, TKey, TValue> : IGenericBuiltInSerializerFactory
			where T : IDictionary<TKey, TValue>
		{
			public ImmutableDictionarySerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new ImmutableDictionarySerializer<T, TKey, TValue>( context, itemSchema.KeySchema, itemSchema.ItemSchema );
			}
		}

#endif

		/// <summary>
		///		Defines non-generic factory method for 'universal' serializers which use general collection features.
		/// </summary>
		private interface IVariantSerializerFactory
		{
			IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema );
		}

		// ReSharper disable MemberHidesStaticFromOuterClass
		private sealed class NonGenericEnumerableSerializerFactory<T> : IVariantSerializerFactory
			where T : IEnumerable
		{
			public NonGenericEnumerableSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractNonGenericEnumerableMessagePackSerializer<T>( context, targetType, schema );
			}
		}

		private sealed class NonGenericCollectionSerializerFactory<T> : IVariantSerializerFactory
			where T : ICollection
		{
			public NonGenericCollectionSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractNonGenericCollectionMessagePackSerializer<T>( context, targetType, schema );
			}
		}

		private sealed class NonGenericListSerializerFactory<T> : IVariantSerializerFactory
			where T : IList
		{
			public NonGenericListSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractNonGenericListMessagePackSerializer<T>( context, targetType, schema );
			}
		}

		private sealed class NonGenericDictionarySerializerFactory<T> : IVariantSerializerFactory
			where T : IDictionary
		{
			public NonGenericDictionarySerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractNonGenericDictionaryMessagePackSerializer<T>( context, targetType, schema );
			}
		}

		private sealed class EnumerableSerializerFactory<TCollection, TItem> : IVariantSerializerFactory
			where TCollection : IEnumerable<TItem>
		{
			public EnumerableSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractEnumerableMessagePackSerializer<TCollection, TItem>( context, targetType, schema );
			}
		}

		private sealed class CollectionSerializerFactory<TCollection, TItem> : IVariantSerializerFactory
			where TCollection : ICollection<TItem>
		{
			public CollectionSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractCollectionMessagePackSerializer<TCollection, TItem>( context, targetType, schema );
			}
		}

#if !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
		private sealed class ReadOnlyCollectionSerializerFactory<TCollection, TItem> : IVariantSerializerFactory
			where TCollection : IReadOnlyCollection<TItem>
		{
			public ReadOnlyCollectionSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractReadOnlyCollectionMessagePackSerializer<TCollection, TItem>( context, targetType, schema );
			}
		}
#endif // !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )

		private sealed class DictionarySerializerFactory<TDictionary, TKey, TValue> : IVariantSerializerFactory
			where TDictionary : IDictionary<TKey, TValue>
		{
			public DictionarySerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractDictionaryMessagePackSerializer<TDictionary, TKey, TValue>( context, targetType, schema );
			}
		}

#if !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
		private sealed class ReadOnlyDictionarySerializerFactory<TDictionary, TKey, TValue> : IVariantSerializerFactory
			where TDictionary : IReadOnlyDictionary<TKey, TValue>
		{
			public ReadOnlyDictionarySerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractReadOnlyDictionaryMessagePackSerializer<TDictionary, TKey, TValue>( context, targetType, schema );
			}
		}
#endif // !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
		// ReSharper restore MemberHidesStaticFromOuterClass
#endif // !UNITY

#if !NETFX_35 && !NETFX_40 && !SILVERLIGHT
		private enum ImmutableCollectionType
		{
			Unknown = 0,
			ImmutableArray,
			ImmutableDictionary,
			ImmutableHashSet,
			ImmutableList,
			ImmutableQueue,
			ImmutableSortedDictionary,
			ImmutableSortedSet,
			ImmutableStack,
		}
#endif // !NETFX_35 && !NETFX_40 && !SILVERLIGHT
	}
}
