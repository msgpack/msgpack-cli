#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2012-2016 FUJIWARA, Yusuke and contributors
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
// Contributors:
//    Samuel Cragg
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
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT

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

		public static MessagePackSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
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
					return CreateListSerializer( context, targetType, targetType.GetCollectionTraits( CollectionTraitOptions.WithAddMethod, context.CompatibilityOptions.AllowNonCollectionEnumerableTypes ), schema );
#endif // !UNITY
				}

				if ( typeDefinition == typeof( Dictionary<,> ) )
				{
					var genericTypeArguments = targetType.GetGenericArguments();
#if !UNITY
					return CreateDictionarySerializer( context, genericTypeArguments[ 0 ], genericTypeArguments[ 1 ], schema );
#else
					return CreateDictionarySerializer( context, targetType, targetType.GetCollectionTraits( CollectionTraitOptions.WithAddMethod, context.CompatibilityOptions.AllowNonCollectionEnumerableTypes ), genericTypeArguments[ 0 ], genericTypeArguments[ 1 ], schema );
#endif // !UNITY
				}
			}

#if !UNITY
			return TryCreateImmutableCollectionSerializer( context, targetType, schema );
#else
			return null;
#endif // !UNITY
		}

		private static MessagePackSerializer CreateArraySerializer( SerializationContext context, Type targetType, PolymorphismSchema itemsSchema )
		{
#if DEBUG
			Contract.Assert( targetType.IsArray, "targetType.IsArray" );
#endif // DEBUG
			return ArraySerializer.Create( context, targetType, itemsSchema );
		}

#if !UNITY
		private static MessagePackSerializer CreateNullableSerializer( SerializationContext context, Type underlyingType, PolymorphismSchema schema )
		{
			return
				ReflectionExtensions.CreateInstancePreservingExceptionType<IGenericBuiltInSerializerFactory>(
					typeof( NullableInstanceFactory<> ).MakeGenericType( underlyingType )
				).Create( context, schema );
		}
#else
		private static MessagePackSerializer CreateNullableSerializer( SerializationContext context, Type nullableType, Type underlyingType )
		{
			return new NullableMessagePackSerializer( context, nullableType, underlyingType );
		}
#endif // !UNITY

#if !UNITY
		private static MessagePackSerializer CreateListSerializer( SerializationContext context, Type itemType, PolymorphismSchema schema )
		{
#if DEBUG
			if ( SerializerDebugging.AvoidsGenericSerializer )
			{
				return null;
			}
#endif // DEBUG
			return
				ReflectionExtensions.CreateInstancePreservingExceptionType<IGenericBuiltInSerializerFactory>(
					typeof( ListInstanceFactory<> ).MakeGenericType( itemType )
				).Create( context, schema );
		}
#else
		private static MessagePackSerializer CreateListSerializer( SerializationContext context, Type targetType, CollectionTraits traits, PolymorphismSchema schema )
		{
			return new System_Collections_Generic_List_1MessagePackSerializer( context, targetType, traits, ( schema ?? PolymorphismSchema.Default ).ItemSchema );
		}
#endif // !UNITY

#if !UNITY
		private static MessagePackSerializer CreateDictionarySerializer( SerializationContext context, Type keyType, Type valueType, PolymorphismSchema schema )
		{
#if DEBUG
			if ( SerializerDebugging.AvoidsGenericSerializer )
			{
				return null;
			}
#endif // DEBUG
			return
				ReflectionExtensions.CreateInstancePreservingExceptionType<IGenericBuiltInSerializerFactory>(
					typeof( DictionaryInstanceFactory<,> ).MakeGenericType( keyType, valueType )
				).Create( context, schema );
		}
#else
		private static MessagePackSerializer CreateDictionarySerializer( SerializationContext context, Type targetType, CollectionTraits traits, Type keyType, Type valueType, PolymorphismSchema schema )
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
#if SILVERLIGHT || NET35 || NET40
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "context", Justification = "Used in other platform" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "targetType", Justification = "Used in other platform" )]
#endif // SILVERLIGHT
		// ReSharper disable UnusedParameter.Local
		private static MessagePackSerializer TryCreateImmutableCollectionSerializer( SerializationContext context, Type targetType, PolymorphismSchema schema )
		{
#if NET35 || NET40 || SILVERLIGHT
			// ImmutableCollections does not support above platforms.
			return null;
#else
			if ( targetType.Namespace != "System.Collections.Immutable"
				&& targetType.Namespace != "Microsoft.FSharp.Collections" )
			{
				return null;
			}

			if ( !targetType.GetIsGenericType() )
			{
				return null;
			}

			var itemSchema = ( schema ?? PolymorphismSchema.Default );
			switch ( DetermineImmutableCollectionType( targetType ) )
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
				case ImmutableCollectionType.FSharpList:
				{
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IGenericBuiltInSerializerFactory>(
							typeof( FSharpCollectionSerializerFactory<,> ).MakeGenericType( targetType, targetType.GetGenericArguments()[ 0 ] ),
							"ListModule"
						).Create( context, itemSchema );
				}
				case ImmutableCollectionType.FSharpSet:
				{
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IGenericBuiltInSerializerFactory>(
							typeof( FSharpCollectionSerializerFactory<,> ).MakeGenericType( targetType, targetType.GetGenericArguments()[ 0 ] ),
							"SetModule"
						).Create( context, itemSchema );
				}
				case ImmutableCollectionType.FSharpMap:
				{
					return
						ReflectionExtensions.CreateInstancePreservingExceptionType<IGenericBuiltInSerializerFactory>(
							typeof( FSharpMapSerializerFactory<,,> ).MakeGenericType( targetType, targetType.GetGenericArguments()[ 0 ], targetType.GetGenericArguments()[ 1 ] )
						).Create( context, itemSchema );
				}
				default:
				{
#if DEBUG
					Contract.Assert( false, "Unknown type:" + targetType );
#endif // DEBUG
					// ReSharper disable HeuristicUnreachableCode
					return null;
					// ReSharper restore HeuristicUnreachableCode
				}
			}
#endif // NET35 || NET40 || SILVERLIGHT
		}

#if !NET35 && !NET40 && !SILVERLIGHT
		private static ImmutableCollectionType DetermineImmutableCollectionType( Type targetType )
		{
			if ( targetType.Namespace != "System.Collections.Immutable" 
				&& targetType.Namespace != "Microsoft.FSharp.Collections" )
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
				case "FSharpList`1":
				{
					return ImmutableCollectionType.FSharpList;
				}
				case "FSharpSet`1":
				{
					return ImmutableCollectionType.FSharpSet;
				}
				case "FSharpMap`2":
				{
					return ImmutableCollectionType.FSharpMap;
				}
				default:
				{
#if DEBUG
					Contract.Assert( false, "Unknown type:" + targetType );
#endif // DEBUG
					// ReSharper disable HeuristicUnreachableCode
					return ImmutableCollectionType.Unknown;
					// ReSharper restore HeuristicUnreachableCode
				}
			}
		}
#endif // !NET35 && !NET40 && !SILVERLIGHT
#endif // !UNITY

		public static MessagePackSerializer TryCreateAbstractCollectionSerializer( SerializationContext context, Type abstractType, Type concreteType, PolymorphismSchema schema )
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
					abstractType.GetCollectionTraits( CollectionTraitOptions.None, context.CompatibilityOptions.AllowNonCollectionEnumerableTypes )
				);
		}

		internal static MessagePackSerializer TryCreateAbstractCollectionSerializer( SerializationContext context, Type abstractType, Type concreteType, PolymorphismSchema schema, CollectionTraits traits )
		{
			switch ( traits.DetailedCollectionType )
			{
				case CollectionDetailedKind.GenericList:
#if !NET35 && !UNITY
				case CollectionDetailedKind.GenericSet:
#endif // !NET35 && !UNITY
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
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
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
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
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
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
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
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
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

#if !UNITY && !NET35 && !NET40 && !SILVERLIGHT
			// ImmutableCollections does not support above platforms.
			if ( DetermineImmutableCollectionType( type ) != ImmutableCollectionType.Unknown )
			{
				return true;
			}
#endif // !UNITY && !NET35 && !NET40 && !SILVERLIGHT

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
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		interface IGenericBuiltInSerializerFactory
		{
			MessagePackSerializer Create( SerializationContext context, PolymorphismSchema schema );
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class NullableInstanceFactory<T> : IGenericBuiltInSerializerFactory
			where T : struct
		{
			public NullableInstanceFactory() { }

			public MessagePackSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				return new NullableMessagePackSerializer<T>( context );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class ListInstanceFactory<T> : IGenericBuiltInSerializerFactory
		{
			public ListInstanceFactory() { }

			public MessagePackSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new System_Collections_Generic_List_1MessagePackSerializer<T>( context, itemSchema.ItemSchema );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class DictionaryInstanceFactory<TKey, TValue> : IGenericBuiltInSerializerFactory
		{
			public DictionaryInstanceFactory() { }

			public MessagePackSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new System_Collections_Generic_Dictionary_2MessagePackSerializer<TKey, TValue>( context, itemSchema.KeySchema, itemSchema.ItemSchema );
			}
		}

#if !NET35 && !NET40 && !SILVERLIGHT

		[Preserve( AllMembers = true )]
		private sealed class ImmutableCollectionSerializerFactory<T, TItem> : IGenericBuiltInSerializerFactory
			where T : IEnumerable<TItem>
		{
			public ImmutableCollectionSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new ImmutableCollectionSerializer<T, TItem>( context, itemSchema.ItemSchema );
			}
		}

		[Preserve( AllMembers = true )]
		private sealed class ImmutableStackSerializerFactory<T, TItem> : IGenericBuiltInSerializerFactory
			where T : IEnumerable<TItem>
		{
			public ImmutableStackSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new ImmutableStackSerializer<T, TItem>( context, itemSchema.ItemSchema );
			}
		}

		[Preserve( AllMembers = true )]
		private sealed class ImmutableDictionarySerializerFactory<T, TKey, TValue> : IGenericBuiltInSerializerFactory
			where T : IDictionary<TKey, TValue>
		{
			public ImmutableDictionarySerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new ImmutableDictionarySerializer<T, TKey, TValue>( context, itemSchema.KeySchema, itemSchema.ItemSchema );
			}
		}


		[Preserve( AllMembers = true )]
		private sealed class FSharpCollectionSerializerFactory<T, TItem> : IGenericBuiltInSerializerFactory
			where T : IEnumerable<TItem>
		{
			private readonly string _factoryTypeName;

			public FSharpCollectionSerializerFactory( string factoryTypeName )
			{
				this._factoryTypeName = factoryTypeName;
			}

			public MessagePackSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new FSharpCollectionSerializer<T, TItem>( context, itemSchema.ItemSchema, this._factoryTypeName );
			}
		}

		[Preserve( AllMembers = true )]
		private sealed class FSharpMapSerializerFactory<T, TKey, TValue> : IGenericBuiltInSerializerFactory
			where T : IDictionary<TKey, TValue>
		{
			public FSharpMapSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new FSharpMapSerializer<T, TKey, TValue>( context, itemSchema.KeySchema, itemSchema.ItemSchema );
			}
		}
#endif

		/// <summary>
		///		Defines non-generic factory method for 'universal' serializers which use general collection features.
		/// </summary>
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		interface IVariantSerializerFactory
		{
			MessagePackSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema );
		}

		// ReSharper disable MemberHidesStaticFromOuterClass
		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class NonGenericEnumerableSerializerFactory<T> : IVariantSerializerFactory
			where T : IEnumerable
		{
			public NonGenericEnumerableSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractNonGenericEnumerableMessagePackSerializer<T>( context, targetType, schema );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class NonGenericCollectionSerializerFactory<T> : IVariantSerializerFactory
			where T : ICollection
		{
			public NonGenericCollectionSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractNonGenericCollectionMessagePackSerializer<T>( context, targetType, schema );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class NonGenericListSerializerFactory<T> : IVariantSerializerFactory
			where T : IList
		{
			public NonGenericListSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractNonGenericListMessagePackSerializer<T>( context, targetType, schema );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class NonGenericDictionarySerializerFactory<T> : IVariantSerializerFactory
			where T : IDictionary
		{
			public NonGenericDictionarySerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractNonGenericDictionaryMessagePackSerializer<T>( context, targetType, schema );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class EnumerableSerializerFactory<TCollection, TItem> : IVariantSerializerFactory
			where TCollection : IEnumerable<TItem>
		{
			public EnumerableSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractEnumerableMessagePackSerializer<TCollection, TItem>( context, targetType, schema );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class CollectionSerializerFactory<TCollection, TItem> : IVariantSerializerFactory
			where TCollection : ICollection<TItem>
		{
			public CollectionSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractCollectionMessagePackSerializer<TCollection, TItem>( context, targetType, schema );
			}
		}

#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
		[Preserve( AllMembers = true )]
		private sealed class ReadOnlyCollectionSerializerFactory<TCollection, TItem> : IVariantSerializerFactory
			where TCollection : IReadOnlyCollection<TItem>
		{
			public ReadOnlyCollectionSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractReadOnlyCollectionMessagePackSerializer<TCollection, TItem>( context, targetType, schema );
			}
		}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class DictionarySerializerFactory<TDictionary, TKey, TValue> : IVariantSerializerFactory
			where TDictionary : IDictionary<TKey, TValue>
		{
			public DictionarySerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractDictionaryMessagePackSerializer<TDictionary, TKey, TValue>( context, targetType, schema );
			}
		}

#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
		[Preserve( AllMembers = true )]
		private sealed class ReadOnlyDictionarySerializerFactory<TDictionary, TKey, TValue> : IVariantSerializerFactory
			where TDictionary : IReadOnlyDictionary<TKey, TValue>
		{
			public ReadOnlyDictionarySerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new AbstractReadOnlyDictionaryMessagePackSerializer<TDictionary, TKey, TValue>( context, targetType, schema );
			}
		}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
		// ReSharper restore MemberHidesStaticFromOuterClass
#endif // !UNITY

#if !NET35 && !NET40 && !SILVERLIGHT && !UNITY
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
			FSharpList,
			FSharpMap,
			FSharpSet,
		}
#endif // !NET35 && !NET40 && !SILVERLIGHT && !UNITY
	}
}
