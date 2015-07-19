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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		Non generic part of <see cref="ExpressionTreeSerializerBuilder{TObject}"/>.
	/// </summary>
	internal static class ExpressionTreeSerializerBuilderHelpers
	{
		public static Type GetSerializerClass( Type targetType, CollectionTraits traits )
		{
			switch ( traits.DetailedCollectionType )
			{
				case CollectionDetailedKind.GenericEnumerable:
				{
					return typeof( ExpressionCallbackEnumerableMessagePackSerializer<,> ).MakeGenericType( targetType, traits.ElementType );
				}
				case CollectionDetailedKind.GenericCollection:
				case CollectionDetailedKind.GenericSet:
				case CollectionDetailedKind.GenericList:
				{
					return typeof( ExpressionCallbackCollectionMessagePackSerializer<,> ).MakeGenericType( targetType, traits.ElementType );
				}
				case CollectionDetailedKind.GenericDictionary:
				{
					var keyValuePairGenericArguments = traits.ElementType.GetGenericArguments();
					return
						typeof( ExpressionCallbackDictionaryMessagePackSerializer<,,> ).MakeGenericType(
							targetType,
							keyValuePairGenericArguments[ 0 ],
							keyValuePairGenericArguments[ 1 ]
						);
				}
#if !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyCollection:
				case CollectionDetailedKind.GenericReadOnlyList:
				{
					return typeof( ExpressionCallbackReadOnlyCollectionMessagePackSerializer<,> ).MakeGenericType( targetType, traits.ElementType );
				}
				case CollectionDetailedKind.GenericReadOnlyDictionary:
				{
					var keyValuePairGenericArguments = traits.ElementType.GetGenericArguments();
					return
						typeof( ExpressionCallbackReadOnlyDictionaryMessagePackSerializer<,,> ).MakeGenericType(
							targetType,
							keyValuePairGenericArguments[ 0 ],
							keyValuePairGenericArguments[ 1 ]
						);
				}
#endif // !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.NonGenericEnumerable:
				{
					return typeof( ExpressionCallbackNonGenericEnumerableMessagePackSerializer<> ).MakeGenericType( targetType );
				}
				case CollectionDetailedKind.NonGenericCollection:
				{
					return typeof( ExpressionCallbackNonGenericCollectionMessagePackSerializer<> ).MakeGenericType( targetType );
				}
				case CollectionDetailedKind.NonGenericList:
				{
					return typeof( ExpressionCallbackNonGenericListMessagePackSerializer<> ).MakeGenericType( targetType );
				}
				case CollectionDetailedKind.NonGenericDictionary:
				{
					return typeof( ExpressionCallbackNonGenericDictionaryMessagePackSerializer<> ).MakeGenericType( targetType );
				}
				default:
				{
					return
						targetType.GetIsEnum()
						? typeof( ExpressionCallbackEnumMessagePackSerializer<> ).MakeGenericType( targetType )
						: typeof( ExpressionCallbackMessagePackSerializer<> ).MakeGenericType( targetType );
				}
			}
		}

		// Ok this is generic, but private dependencies are non-generic.
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Well patterned" )]
		public static Func<SerializationContext, MessagePackSerializer<TObject>> CreateFactory<TObject>( ExpressionTreeContext codeGenerationContext, CollectionTraits traits, PolymorphismSchema schema )
		{
			// Get at this point to prevent unexpected context change.
			var packToCore = codeGenerationContext.GetPackToCore();
			var unpackFromCore = codeGenerationContext.GetUnpackFromCore();
			var unpackToCore = codeGenerationContext.GetUnpackToCore();
			var createInstance = codeGenerationContext.GetCreateInstance();
			var addItem = codeGenerationContext.GetAddItem();

			switch ( traits.DetailedCollectionType )
			{
				case CollectionDetailedKind.NonGenericEnumerable:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<IEnumerableCallbackSerializerFactory>(
							typeof( NonGenericEnumerableCallbackSerializerFactory<> ).MakeGenericType( typeof( TObject ) )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						context =>
							factory.Create( context, schema, createInstance, unpackFromCore, addItem ) as MessagePackSerializer<TObject>;
				}
				case CollectionDetailedKind.NonGenericCollection:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<IEnumerableCallbackSerializerFactory>(
							typeof( NonGenericCollectionCallbackSerializerFactory<> ).MakeGenericType( typeof( TObject ) )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						context =>
							factory.Create( context, schema, createInstance, unpackFromCore, addItem ) as MessagePackSerializer<TObject>;
				}
				case CollectionDetailedKind.NonGenericList:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICollectionCallbackSerializerFactory>(
							typeof( NonGenericListCallbackSerializerFactory<> ).MakeGenericType( typeof( TObject ) )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						context =>
							factory.Create( context, schema, createInstance, addItem ) as MessagePackSerializer<TObject>;
				}
				case CollectionDetailedKind.NonGenericDictionary:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICollectionCallbackSerializerFactory>(
							typeof( NonGenericDictionaryCallbackSerializerFactory<> ).MakeGenericType( typeof( TObject ) )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						context =>
							factory.Create( context, schema, createInstance, addItem ) as MessagePackSerializer<TObject>;
				}
				case CollectionDetailedKind.GenericEnumerable:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<IEnumerableCallbackSerializerFactory>(
							typeof( EnumerableCallbackSerializerFactory<,> ).MakeGenericType( typeof( TObject ), traits.ElementType )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						context =>
							factory.Create( context, schema, createInstance, unpackFromCore, addItem ) as MessagePackSerializer<TObject>;
				}
				case CollectionDetailedKind.GenericCollection:
				case CollectionDetailedKind.GenericSet:
				case CollectionDetailedKind.GenericList:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICollectionCallbackSerializerFactory>(
							typeof( CollectionCallbackSerializerFactory<,> ).MakeGenericType( typeof( TObject ), traits.ElementType )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						context =>
							factory.Create( context, schema, createInstance, addItem ) as MessagePackSerializer<TObject>;
				}
#if !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyCollection:
				case CollectionDetailedKind.GenericReadOnlyList:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICollectionCallbackSerializerFactory>(
							typeof( ReadOnlyCollectionCallbackSerializerFactory<,> ).MakeGenericType( typeof( TObject ), traits.ElementType )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						context =>
							factory.Create( context, schema, createInstance, addItem ) as MessagePackSerializer<TObject>;
				}
#endif // !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericDictionary:
				{
					var keyValuePairGenericArguments = traits.ElementType.GetGenericArguments();
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICollectionCallbackSerializerFactory>(
							typeof( DictionaryCallbackSerializerFactory<,,> ).MakeGenericType(
								typeof( TObject ),
								keyValuePairGenericArguments[ 0 ],
								keyValuePairGenericArguments[ 1 ]
							)
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						context =>
							factory.Create( context, schema, createInstance, addItem ) as MessagePackSerializer<TObject>;
				}
#if !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyDictionary:
				{
					var keyValuePairGenericArguments = traits.ElementType.GetGenericArguments();
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICollectionCallbackSerializerFactory>(
							typeof( ReadOnlyDictionaryCallbackSerializerFactory<,,> ).MakeGenericType(
								typeof( TObject ),
								keyValuePairGenericArguments[ 0 ],
								keyValuePairGenericArguments[ 1 ]
							)
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						context =>
							factory.Create( context, schema, createInstance, addItem ) as MessagePackSerializer<TObject>;
				}
#endif // !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				default:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICallbackSerializerFactory>(
							typeof( CallbackSerializerFactory<> ).MakeGenericType(
								typeof( TObject )
							)
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG
					return
						context =>
							factory.Create( context, packToCore, unpackFromCore, unpackToCore ) as MessagePackSerializer<TObject>;
				}
			}
		}

		private interface ICallbackSerializerFactory
		{
			object Create(
				SerializationContext context,
				Delegate packTo,
				Delegate unpackFrom,
				Delegate unpackTo
			);
		}

		private sealed class CallbackSerializerFactory<T> : ICallbackSerializerFactory
		{
			public CallbackSerializerFactory() { }

			private static object Create(
				SerializationContext context,
				Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> packTo,
				Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> unpackFrom,
				Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> unpackTo
			)
			{
				return
					new ExpressionCallbackMessagePackSerializer<T>(
						context,
						packTo,
						unpackFrom,
						unpackTo
					);
			}

			public object Create(
				SerializationContext context,
				Delegate packTo,
				Delegate unpackFrom,
				Delegate unpackTo
			)
			{
				return
					Create(
						context,
						( Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> )packTo,
						( Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> )unpackFrom,
						( Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> )unpackTo
					);
			}
		}

		private interface IEnumerableCallbackSerializerFactory
		{
			object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Delegate createInstance,
				Delegate unpackFrom,
				Delegate addItem
			);
		}

		private abstract class EnumerableCallbackSerializerFactoryBase<TCollection, TItem, TSerializer> :
			IEnumerableCallbackSerializerFactory
		{
			protected abstract object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<TSerializer, SerializationContext, int, TCollection> createInstance,
				Func<TSerializer, SerializationContext, Unpacker, TCollection> unpackFrom,
				Action<TSerializer, SerializationContext, TCollection, TItem> addItem
			);

			public object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Delegate createInstance,
				Delegate unpackFrom,
				Delegate addItem
			)
			{
				return
					this.Create(
						context,
						schema,
						( Func<TSerializer, SerializationContext, int, TCollection> )createInstance,
						( Func<TSerializer, SerializationContext, Unpacker, TCollection> )unpackFrom,
						( Action<TSerializer, SerializationContext, TCollection, TItem> )addItem
					);

			}
		}

		private sealed class EnumerableCallbackSerializerFactory<TCollection, TItem> : EnumerableCallbackSerializerFactoryBase<TCollection, TItem, ExpressionCallbackEnumerableMessagePackSerializer<TCollection, TItem>>
			where TCollection : IEnumerable<TItem>
		{
			public EnumerableCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<ExpressionCallbackEnumerableMessagePackSerializer<TCollection, TItem>, SerializationContext, int, TCollection> createInstance,
				Func<ExpressionCallbackEnumerableMessagePackSerializer<TCollection, TItem>, SerializationContext, Unpacker, TCollection> unpackFrom,
				Action<ExpressionCallbackEnumerableMessagePackSerializer<TCollection, TItem>, SerializationContext, TCollection, TItem> addItem
			)
			{
				return
					new ExpressionCallbackEnumerableMessagePackSerializer<TCollection, TItem>(
						context,
						schema,
						createInstance,
						unpackFrom,
						addItem
					);
			}
		}

		private sealed class NonGenericEnumerableCallbackSerializerFactory<TCollection> : EnumerableCallbackSerializerFactoryBase<TCollection, object, ExpressionCallbackNonGenericEnumerableMessagePackSerializer<TCollection>>
			where TCollection : IEnumerable
		{
			public NonGenericEnumerableCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<ExpressionCallbackNonGenericEnumerableMessagePackSerializer<TCollection>, SerializationContext, int, TCollection> createInstance,
				Func<ExpressionCallbackNonGenericEnumerableMessagePackSerializer<TCollection>, SerializationContext, Unpacker, TCollection> unpackFrom,
				Action<ExpressionCallbackNonGenericEnumerableMessagePackSerializer<TCollection>, SerializationContext, TCollection, object> addItem
			)
			{
				return
					new ExpressionCallbackNonGenericEnumerableMessagePackSerializer<TCollection>(
						context,
						schema,
						createInstance,
						unpackFrom,
						addItem
					);
			}
		}

		private sealed class NonGenericCollectionCallbackSerializerFactory<TCollection> : EnumerableCallbackSerializerFactoryBase<TCollection, object, ExpressionCallbackNonGenericCollectionMessagePackSerializer<TCollection>>
			where TCollection : ICollection
		{
			public NonGenericCollectionCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<ExpressionCallbackNonGenericCollectionMessagePackSerializer<TCollection>, SerializationContext, int, TCollection> createInstance,
				Func<ExpressionCallbackNonGenericCollectionMessagePackSerializer<TCollection>, SerializationContext, Unpacker, TCollection> unpackFrom,
				Action<ExpressionCallbackNonGenericCollectionMessagePackSerializer<TCollection>, SerializationContext, TCollection, object> addItem
			)
			{
				return
					new ExpressionCallbackNonGenericCollectionMessagePackSerializer<TCollection>(
						context,
						schema,
						createInstance,
						unpackFrom,
						addItem
					);
			}
		}

		private interface ICollectionCallbackSerializerFactory
		{
			object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Delegate createInstance,
				Delegate addItem
			);
		}

		private abstract class CollectionCallbackSerializerFactoryBase<TCollection, TItem, TSerializer> : ICollectionCallbackSerializerFactory
		{
			protected abstract object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<TSerializer, SerializationContext, int, TCollection> createInstance,
				Action<TSerializer, SerializationContext, TCollection, TItem> addItem
			);

			public object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Delegate createInstance,
				Delegate addItem
			)
			{
				return
					this.Create(
						context,
						schema,
						( Func<TSerializer, SerializationContext, int, TCollection> )createInstance,
						( Action<TSerializer, SerializationContext, TCollection, TItem> )addItem
					);

			}
		}

		private sealed class CollectionCallbackSerializerFactory<TCollection, TItem>
			: CollectionCallbackSerializerFactoryBase<TCollection, TItem, ExpressionCallbackCollectionMessagePackSerializer<TCollection, TItem>>
			where TCollection : ICollection<TItem>
		{
			public CollectionCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<ExpressionCallbackCollectionMessagePackSerializer<TCollection, TItem>, SerializationContext, int, TCollection> createInstance,
				Action<ExpressionCallbackCollectionMessagePackSerializer<TCollection, TItem>, SerializationContext, TCollection, TItem> addItem
			)
			{
				return
					new ExpressionCallbackCollectionMessagePackSerializer<TCollection, TItem>(
						context,
						schema,
						createInstance
					);
			}
		}

#if !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
		private sealed class ReadOnlyCollectionCallbackSerializerFactory<TCollection, TItem>
			: CollectionCallbackSerializerFactoryBase<TCollection, TItem, ExpressionCallbackReadOnlyCollectionMessagePackSerializer<TCollection, TItem>>
			where TCollection : IReadOnlyCollection<TItem>
		{
			public ReadOnlyCollectionCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<ExpressionCallbackReadOnlyCollectionMessagePackSerializer<TCollection, TItem>, SerializationContext, int, TCollection> createInstance,
				Action<ExpressionCallbackReadOnlyCollectionMessagePackSerializer<TCollection, TItem>, SerializationContext, TCollection, TItem> addItem
			)
			{
				return
					new ExpressionCallbackReadOnlyCollectionMessagePackSerializer<TCollection, TItem>(
						context,
						schema,
						createInstance,
						addItem
					);
			}
		}
#endif // !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )

		private sealed class NonGenericListCallbackSerializerFactory<TCollection>
			: CollectionCallbackSerializerFactoryBase<TCollection, object, ExpressionCallbackNonGenericListMessagePackSerializer<TCollection>>
			where TCollection : IList
		{
			public NonGenericListCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<ExpressionCallbackNonGenericListMessagePackSerializer<TCollection>, SerializationContext, int, TCollection> createInstance,
				Action<ExpressionCallbackNonGenericListMessagePackSerializer<TCollection>, SerializationContext, TCollection, object> addItem
			)
			{
				return
					new ExpressionCallbackNonGenericListMessagePackSerializer<TCollection>(
						context,
						schema,
						createInstance
					);
			}
		}

		private sealed class NonGenericDictionaryCallbackSerializerFactory<TDictionary>
			: CollectionCallbackSerializerFactoryBase<TDictionary, DictionaryEntry, ExpressionCallbackNonGenericDictionaryMessagePackSerializer<TDictionary>>
			where TDictionary : IDictionary
		{
			public NonGenericDictionaryCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<ExpressionCallbackNonGenericDictionaryMessagePackSerializer<TDictionary>, SerializationContext, int, TDictionary> createInstance,
				Action<ExpressionCallbackNonGenericDictionaryMessagePackSerializer<TDictionary>, SerializationContext, TDictionary, DictionaryEntry> addItem
			)
			{
				return
					new ExpressionCallbackNonGenericDictionaryMessagePackSerializer<TDictionary>(
						context,
						schema,
						createInstance
					);
			}
		}

		private sealed class DictionaryCallbackSerializerFactory<TDictionary, TKey, TValue> : ICollectionCallbackSerializerFactory
			where TDictionary : IDictionary<TKey, TValue>
		{
			public DictionaryCallbackSerializerFactory() { }

			private static object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<ExpressionCallbackDictionaryMessagePackSerializer<TDictionary, TKey, TValue>, SerializationContext, int, TDictionary> createInstance
			)
			{
				return
					new ExpressionCallbackDictionaryMessagePackSerializer<TDictionary, TKey, TValue>(
						context,
						schema,
						createInstance
					);
			}

			public object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Delegate createInstance,
				Delegate addItem
			)
			{
				return
					Create(
						context,
						schema,
						( Func<ExpressionCallbackDictionaryMessagePackSerializer<TDictionary, TKey, TValue>, SerializationContext, int, TDictionary> )createInstance
					);
			}
		}

#if !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
		private sealed class ReadOnlyDictionaryCallbackSerializerFactory<TDictionary, TKey, TValue> : ICollectionCallbackSerializerFactory
			where TDictionary : IReadOnlyDictionary<TKey, TValue>
		{
			public ReadOnlyDictionaryCallbackSerializerFactory() { }

			private static object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<ExpressionCallbackReadOnlyDictionaryMessagePackSerializer<TDictionary, TKey, TValue>, SerializationContext, int, TDictionary> createInstance,
				Action<ExpressionCallbackReadOnlyDictionaryMessagePackSerializer<TDictionary, TKey, TValue>, SerializationContext, TDictionary, TKey, TValue> addItem
			)
			{
				return
					new ExpressionCallbackReadOnlyDictionaryMessagePackSerializer<TDictionary, TKey, TValue>(
						context,
						schema,
						createInstance,
						addItem
					);
			}

			public object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Delegate createInstance,
				Delegate addItem
			)
			{
				return
					Create(
						context,
						schema,
						( Func<ExpressionCallbackReadOnlyDictionaryMessagePackSerializer<TDictionary, TKey, TValue>, SerializationContext, int, TDictionary> )createInstance,
						( Action<ExpressionCallbackReadOnlyDictionaryMessagePackSerializer<TDictionary, TKey, TValue>, SerializationContext, TDictionary, TKey, TValue> )addItem
					);
			}
		}
#endif // !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
	}
}