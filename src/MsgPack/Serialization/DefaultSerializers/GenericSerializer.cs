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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections;
using System.Collections.Generic;
#if !UNITY
using System.Diagnostics.Contracts;
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
			return Create( context, typeof( T ), schema ) as MessagePackSerializer<T>;
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
				return CreateNullableSerializer( context, nullableUnderlyingType, schema );
			}

			if ( targetType.GetIsGenericType() )
			{
				var typeDefinition = targetType.GetGenericTypeDefinition();
				if ( typeDefinition == typeof( List<> ) )
				{
					return CreateListSerializer( context, targetType.GetGenericArguments()[ 0 ], schema );
				}

				if ( typeDefinition == typeof( Dictionary<,> ) )
				{
					var genericTypeArguments = targetType.GetGenericArguments();
					return CreateDictionarySerializer( context, genericTypeArguments[ 0 ], genericTypeArguments[ 1 ], schema );
				}
			}

			return TryCreateImmutableCollectionSerializer( context, targetType, schema );
		}

		private static IMessagePackSingleObjectSerializer CreateArraySerializer( SerializationContext context, Type targetType, PolymorphismSchema itemsSchema )
		{
#if DEBUG && !UNITY
			Contract.Assert( targetType.IsArray );
#endif // DEBUG && !UNITY
			return ArraySerializer.Create( context, targetType, itemsSchema );
		}

		private static IMessagePackSingleObjectSerializer CreateNullableSerializer( SerializationContext context, Type underlyingType, PolymorphismSchema schema )
		{
			return
				( ( IGenericBuiltInSerializerFactory )Activator.CreateInstance(
					typeof( NullableInstanceFactory<> ).MakeGenericType( underlyingType )
				) ).Create( context, schema );
		}

		private static IMessagePackSingleObjectSerializer CreateListSerializer( SerializationContext context, Type itemType, PolymorphismSchema schema )
		{
#if DEBUG && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
			if ( SerializerDebugging.AvoidsGenericSerializer )
			{
				return null;
			}
#endif // DEBUG && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
			return
				( ( IGenericBuiltInSerializerFactory )Activator.CreateInstance(
					typeof( ListInstanceFactory<> ).MakeGenericType( itemType )
				) ).Create( context, schema );
		}

		private static IMessagePackSingleObjectSerializer CreateDictionarySerializer( SerializationContext context, Type keyType, Type valueType, PolymorphismSchema schema )
		{
#if DEBUG && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
			if ( SerializerDebugging.AvoidsGenericSerializer )
			{
				return null;
			}
#endif // DEBUG && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
			return
				( ( IGenericBuiltInSerializerFactory )Activator.CreateInstance(
					typeof( DictionaryInstanceFactory<,> ).MakeGenericType( keyType, valueType )
				) ).Create( context, schema );
		}

#if SILVERLIGHT || NETFX_35 || NETFX_40
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "context", Justification = "Used in other platform" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "targetType", Justification = "Used in other platform" )]
#endif // SILVERLIGHT
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
			switch ( targetType.GetGenericTypeDefinition().Name )
			{
				case "ImmutableList`1":
				case "ImmutableHashSet`1":
				case "ImmutableSortedSet`1":
				case "ImmutableQueue`1":
				{
					return
						( ( IGenericBuiltInSerializerFactory )
							Activator.CreateInstance(
								typeof( ImmutableCollectionSerializerFactory<,> )
									.MakeGenericType( targetType, targetType.GetGenericArguments()[ 0 ] )
						) ).Create( context, itemSchema );
				}
				case "ImmutableStack`1":
				{
					return
						( ( IGenericBuiltInSerializerFactory )
							Activator.CreateInstance(
								typeof( ImmutableStackSerializerFactory<,> )
									.MakeGenericType( targetType, targetType.GetGenericArguments()[ 0 ] )
						) ).Create( context, itemSchema );
				}
				case "ImmutableDictionary`2":
				case "ImmutableSortedDictionary`2":
				{
					return
						( ( IGenericBuiltInSerializerFactory )
							Activator.CreateInstance(
								typeof( ImmutableDictionarySerializerFactory<,,> )
									.MakeGenericType( targetType, targetType.GetGenericArguments()[ 0 ], targetType.GetGenericArguments()[ 1 ] )
						) ).Create( context, itemSchema );
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

		public static IMessagePackSingleObjectSerializer TryCreateCollectionInterfaceSerializer( SerializationContext context, Type abstractType, Type concreteType, PolymorphismSchema schema )
		{
			var itemSchema = ( schema ?? PolymorphismSchema.Default );

			if ( abstractType.GetIsGenericType() && abstractType.GetGenericTypeDefinition() == typeof( IList<> ) )
			{
				return
					( ( IUniversalGenericSerializerFactory )Activator.CreateInstance(
						typeof( ListSerializerFactory<> ).MakeGenericType( abstractType.GetGenericArguments()[ 0 ] )
					) ).Create( context, concreteType, itemSchema );
			}
#if !NETFX_35 && !UNITY
			else if ( abstractType.GetIsGenericType() && abstractType.GetGenericTypeDefinition() == typeof( ISet<> ) )
			{
				return
					( ( IUniversalGenericSerializerFactory )Activator.CreateInstance(
						typeof( SetSerializerFactory<> ).MakeGenericType( abstractType.GetGenericArguments()[ 0 ] )
					) ).Create( context, concreteType, itemSchema );
			}
#endif // !NETFX_35 && !UNITY
			else if ( abstractType.GetIsGenericType() && abstractType.GetGenericTypeDefinition() == typeof( ICollection<> ) )
			{
				return
					( ( IUniversalGenericSerializerFactory )Activator.CreateInstance(
						typeof( CollectionSerializerFactory<> ).MakeGenericType( abstractType.GetGenericArguments()[ 0 ] )
					) ).Create( context, concreteType, itemSchema );
			}
			else if ( abstractType.GetIsGenericType() && abstractType.GetGenericTypeDefinition() == typeof( IEnumerable<> ) )
			{
				return
					( ( IUniversalGenericSerializerFactory )Activator.CreateInstance(
						typeof( EnumerableSerializerFactory<> ).MakeGenericType( abstractType.GetGenericArguments()[ 0 ] )
					) ).Create( context, concreteType, itemSchema );
			}
			else if ( abstractType.GetIsGenericType() && abstractType.GetGenericTypeDefinition() == typeof( IDictionary<,> ) )
			{
				return
					( ( IUniversalGenericSerializerFactory )Activator.CreateInstance(
						typeof( DictionarySerializerFactory<,> ).MakeGenericType(
							abstractType.GetGenericArguments()[ 0 ],
							abstractType.GetGenericArguments()[ 1 ]
						)
					) ).Create( context, concreteType, itemSchema );
			}
			else if ( abstractType == typeof( IList ) )
			{
				return new NonGenericListSerializer( context, concreteType, itemSchema.ItemSchema );
			}
			else if ( abstractType == typeof( ICollection ) )
			{
				return new NonGenericCollectionSerializer( context, concreteType, itemSchema.ItemSchema );
			}
			else if ( abstractType == typeof( IEnumerable ) )
			{
				return new NonGenericEnumerableSerializer( context, concreteType, itemSchema.ItemSchema );
			}
			else if ( abstractType == typeof( IDictionary ) )
			{
				return new NonGenericDictionarySerializer( context, concreteType, itemSchema.KeySchema, itemSchema.ItemSchema );
			}
			else
			{
				return null;
			}
		}

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
		///		Defines non-generic factory method for 'universal' serializers which use general collection features and require generic type arguments.
		/// </summary>
		private interface IUniversalGenericSerializerFactory
		{
			IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema );
		}

		// ReSharper disable MemberHidesStaticFromOuterClass
		private sealed class EnumerableSerializerFactory<T> : IUniversalGenericSerializerFactory
		{
			public EnumerableSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new EnumerableSerializer<T>( context, targetType, itemSchema.ItemSchema );
			}
		}

		private sealed class CollectionSerializerFactory<T> : IUniversalGenericSerializerFactory
		{
			public CollectionSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new CollectionSerializer<T>( context, targetType, itemSchema.ItemSchema );
			}
		}

#if !NETFX_35 && !UNITY
		private sealed class SetSerializerFactory<T> : IUniversalGenericSerializerFactory
		{
			public SetSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new SetSerializer<T>( context, targetType, itemSchema.ItemSchema );
			}
		}
#endif // !NETFX_35 && !UNITY

		private sealed class ListSerializerFactory<T> : IUniversalGenericSerializerFactory
		{
			public ListSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new ListSerializer<T>( context, targetType, itemSchema.ItemSchema );
			}
		}

		private sealed class DictionarySerializerFactory<TKey, TValue> : IUniversalGenericSerializerFactory
		{
			public DictionarySerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new DictionarySerializer<TKey, TValue>( context, targetType, itemSchema.KeySchema, itemSchema.ItemSchema );
			}
		}
		// ReSharper restore MemberHidesStaticFromOuterClass
	}
}
