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
#if CORE_CLR
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		Non generic part of <see cref="ExpressionTreeSerializerBuilder"/>.
	/// </summary>
	internal static class ExpressionTreeSerializerBuilderHelpers
	{
		public static readonly PropertyInfo DelegatesIndexer =
			typeof( IDictionary<string, Delegate> ).GetProperty( "Item" );

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

		public static IDictionary<string, Delegate> SupplyPrivateMethodCommonArguments( object @this, IDictionary<string, LambdaExpression> delegates )
		{
			var thisExpression = Expression.Constant( @this, @this.GetType() );
			var commonParametersExpression = new Expression[] { thisExpression, Expression.Property( thisExpression, "OwnerContext" ) };
			return
#if DEBUG
				DebuggableDictionary.Wrap(
#endif // DEBUG
				delegates.ToDictionary(
					kv => kv.Key,
					kv =>
					{
						if ( kv.Value.Parameters.Count < 2 || kv.Value.Parameters[ 1 ].Type != typeof( SerializationContext ) )
						{
#if DEBUG
							Contract.Assert(
								kv.Value.Parameters.Count == 0 || kv.Value.Parameters[ 0 ].Type != thisExpression.Type
							);
#endif // DEBUG
							// may be static
							return kv.Value.Compile();
						}
#if DEBUG
						Contract.Assert( kv.Value.Parameters[ 0 ].Type == thisExpression.Type, kv.Value.Parameters[ 0 ].Type + " == " + thisExpression.Type + " :" + kv.Key );
#endif // DEBUG
						return
							Expression.Lambda(
								Expression.Invoke(
									kv.Value,
									commonParametersExpression.Concat( kv.Value.Parameters.Skip( 2 ) )
								),
								kv.Value.Parameters.Skip( 2 )
							).Compile();
					}
#if DEBUG
				)
#endif // DEBUG
				);
		}

		// Ok this is generic, but private dependencies are non-generic.
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Well patterned" )]
		public static Func<SerializationContext, MessagePackSerializer<TObject>> CreateFactory<TObject>(
			ExpressionTreeContext codeGenerationContext,
			CollectionTraits traits,
			PolymorphismSchema schema,
			IList<Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Packer, TObject>> packOperationList,
			IDictionary<string, Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Packer, TObject>> packOperationTable,
			IList<Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Unpacker, object, int, int>> unpackOperationList,
			IDictionary<string, Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Unpacker, object, int, int>> unpackOperationTable,
			IList<string> memberNames
		)
		{
			// Get at this point to prevent unexpected context change.
			var packToCore = codeGenerationContext.GetDelegate( MethodName.PackToCore );
			var unpackFromCore = codeGenerationContext.GetDelegate( MethodName.UnpackFromCore );
			var createInstance = codeGenerationContext.GetDelegate( MethodName.CreateInstance );
			var addItem = codeGenerationContext.GetDelegate( MethodName.AddItem );
			var delegates = codeGenerationContext.GetMethodLamdaTable();

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
					var serializerType =
						typeof( ExpressionCallbackNonGenericEnumerableMessagePackSerializer<> ).MakeGenericType( typeof( TObject ) );
					var unpackToCore =
						GetActionForInstanceMethod(
							codeGenerationContext,
							serializerType,
							MethodName.UnpackToCore,
							typeof( Unpacker ),
							typeof( TObject ),
							typeof( int )
						);

					return
						context =>
							factory.Create( context, schema, createInstance, unpackFromCore, addItem, unpackToCore, delegates ) as MessagePackSerializer<TObject>;
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
					var serializerType =
						typeof( ExpressionCallbackNonGenericCollectionMessagePackSerializer<> ).MakeGenericType( typeof( TObject ) );
					var unpackToCore =
						GetActionForInstanceMethod(
							codeGenerationContext,
							serializerType,
							MethodName.UnpackToCore,
							typeof( Unpacker ),
							typeof( TObject ),
							typeof( int )
						);

					return
						context =>
							factory.Create( context, schema, createInstance, unpackFromCore, addItem, unpackToCore, delegates ) as MessagePackSerializer<TObject>;
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
					var serializerType =
						typeof( ExpressionCallbackEnumerableMessagePackSerializer<,> ).MakeGenericType( typeof( TObject ), traits.ElementType );
					var unpackToCore =
						GetActionForInstanceMethod(
							codeGenerationContext,
							serializerType,
							MethodName.UnpackToCore,
							typeof( Unpacker ),
							typeof( TObject ),
							typeof( int )
						);

					return
						context =>
							factory.Create( context, schema, createInstance, unpackFromCore, addItem, unpackToCore, delegates ) as MessagePackSerializer<TObject>;
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
							factory.Create(
								context,
								packToCore,
								unpackFromCore,
								codeGenerationContext.GetDelegate( MethodName.UnpackToCore ),
								packOperationList,
								packOperationTable,
								unpackOperationList,
								unpackOperationTable,
								delegates,
								codeGenerationContext.GetDelegate( MethodName.CreateObjectFromContext ),
								memberNames
							) as MessagePackSerializer<TObject>;
				}
			}
		}

		private static Delegate GetActionForInstanceMethod( ExpressionTreeContext codeGenerationContext, Type serializerType, string name, params Type[] parameterTypes )
		{
			var result =
				codeGenerationContext.GetDelegate( name );
			if ( result == null )
			{
				var method =
					codeGenerationContext.This.ContextType.ResolveRuntimeType()
						.GetRuntimeMethod( name, parameterTypes );
				if ( method != null )
				{
					var genericArguments = new Type[ parameterTypes.Length + 1 ];
					genericArguments[ 0 ] = serializerType;
					Array.ConstrainedCopy( parameterTypes, 0, genericArguments, 1, parameterTypes.Length );
					result =
						method.CreateDelegate(
						// ReSharper disable once PossibleNullReferenceException
							Type.GetType(
								"System.Action`" +
								( parameterTypes.Length + 1 ).ToString( CultureInfo.InvariantCulture ) )
							.MakeGenericType( genericArguments ),
							null
						);
				}
			}

			return result;
		}

		private interface ICallbackSerializerFactory
		{
			object Create(
				SerializationContext context,
				Delegate packTo,
				Delegate unpackFrom,
				Delegate unpackTo,
				object packOperationList,
				object packOperationTable,
				object unpackOperationList,
				object unpackOperationTable,
				IDictionary<string, LambdaExpression> delegates,
				object createInstanceFromContext,
				IList<string> memberNames
			);
		}

		private sealed class CallbackSerializerFactory<T> : ICallbackSerializerFactory
		{
			public CallbackSerializerFactory() { }

			private static object Create(
				SerializationContext context,
				Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> packTo,
				Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> unpackFrom,
				Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> unpackTo,
				IList<Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T>> packOperationList,
				IDictionary<string, Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T>> packOperationTable,
				IList<Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, object, int, int>> unpackOperationList,
				IDictionary<string, Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, object, int, int>> unpackOperationTable,
				IDictionary<string, LambdaExpression> delegates,
				Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, object, T> createInstanceFromContext,
				IList<string> memberNames
			)
			{


				return
					new ExpressionCallbackMessagePackSerializer<T>(
						context,
						packTo,
						unpackFrom,
						unpackTo,
						packOperationList,
						packOperationTable,
						unpackOperationList,
						unpackOperationTable,
						delegates,
						createInstanceFromContext,
						memberNames
					);
			}

			public object Create(
				SerializationContext context,
				Delegate packTo,
				Delegate unpackFrom,
				Delegate unpackTo,
				object packOperationList,
				object packOperationTable,
				object unpackOperationList,
				object unpackOperationTable,
				IDictionary<string, LambdaExpression> delegates,
				object createInstanceFromContext,
				IList<string> memberNames
			)
			{
				return
					Create(
						context,
						( Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> )packTo,
						( Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> )unpackFrom,
						( Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> )unpackTo,
						( IList<Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T>> )packOperationList,
						( IDictionary<string, Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T>> )packOperationTable,
						( IList<Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, object, int, int>> )unpackOperationList,
						( IDictionary<string, Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, object, int, int>> )unpackOperationTable,
						delegates,
						( Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, object, T> )createInstanceFromContext,
						memberNames
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
				Delegate addItem,
				Delegate unpackToCore,
				IDictionary<string, LambdaExpression> privateMethods
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
				Action<TSerializer, SerializationContext, TCollection, TItem> addItem,
				Delegate unpackToCore,
				IDictionary<string, LambdaExpression> delegates
			);

			public object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Delegate createInstance,
				Delegate unpackFrom,
				Delegate addItem,
				Delegate unpackToCore,
				IDictionary<string, LambdaExpression> delegates
			)
			{
				return
					this.Create(
						context,
						schema,
						( Func<TSerializer, SerializationContext, int, TCollection> )createInstance,
						( Func<TSerializer, SerializationContext, Unpacker, TCollection> )unpackFrom,
						( Action<TSerializer, SerializationContext, TCollection, TItem> )addItem,
						unpackToCore,
						delegates
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
				Action<ExpressionCallbackEnumerableMessagePackSerializer<TCollection, TItem>, SerializationContext, TCollection, TItem> addItem,
				Delegate unpackToCore,
				IDictionary<string, LambdaExpression> delegates
			)
			{
				return
					new ExpressionCallbackEnumerableMessagePackSerializer<TCollection, TItem>(
						context,
						schema,
						createInstance,
						unpackFrom,
						addItem,
						unpackToCore,
						delegates
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
				Action<ExpressionCallbackNonGenericEnumerableMessagePackSerializer<TCollection>, SerializationContext, TCollection, object> addItem,
				Delegate unpackToCore,
				IDictionary<string, LambdaExpression> delegates
			)
			{
				return
					new ExpressionCallbackNonGenericEnumerableMessagePackSerializer<TCollection>(
						context,
						schema,
						createInstance,
						unpackFrom,
						addItem,
						unpackToCore,
						delegates
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
				Action<ExpressionCallbackNonGenericCollectionMessagePackSerializer<TCollection>, SerializationContext, TCollection, object> addItem,
				Delegate unpackToCore,
				IDictionary<string, LambdaExpression> delegates
			)
			{
				return
					new ExpressionCallbackNonGenericCollectionMessagePackSerializer<TCollection>(
						context,
						schema,
						createInstance,
						unpackFrom,
						addItem,
						unpackToCore,
						delegates
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

#if DEBUG
		private sealed class DebuggableDictionary
		{
			public static IDictionary<string, T> Wrap<T>( IDictionary<string, T> dictionary )
			{
				return new DebuggableDictionary<T>( dictionary );
			}
		}

		private sealed class DebuggableDictionary<T> : IDictionary<String, T>
		{
			private readonly IDictionary<string, T> _dictionary;

			public int Count
			{
				get { return this._dictionary.Count; }
			}

			public bool IsReadOnly
			{
				get { return this._dictionary.IsReadOnly; }
			}

			public ICollection<string> Keys
			{
				get { return this._dictionary.Keys; }
			}

			public ICollection<T> Values
			{
				get { return this._dictionary.Values; }
			}

			public T this[ string key ]
			{
				get
				{
					try
					{
						return this._dictionary[ key ];
					}
					catch ( KeyNotFoundException )
					{
						throw new KeyNotFoundException( String.Format( CultureInfo.CurrentCulture, "Key '{0}' is not found in the dictionary.", key ) );
					}
				}
				set { this._dictionary[ key ] = value; }
			}

			public DebuggableDictionary( IDictionary<string, T> dictionary )
			{
				this._dictionary = dictionary;
			}

			public void Add( string key, T value )
			{
				this._dictionary.Add( key, value );
			}

			public bool ContainsKey( string key )
			{
				return this._dictionary.ContainsKey( key );
			}

			public bool Remove( string key )
			{
				return this._dictionary.Remove( key );
			}

			public bool TryGetValue( string key, out T value )
			{
				return this._dictionary.TryGetValue( key, out value );
			}

			public void Add( KeyValuePair<string, T> item )
			{
				this._dictionary.Add( item );
			}

			public void Clear()
			{
				this._dictionary.Clear();
			}

			public bool Contains( KeyValuePair<string, T> item )
			{
				return this._dictionary.Contains( item );
			}

			public void CopyTo( KeyValuePair<string, T>[] array, int arrayIndex )
			{
				this._dictionary.CopyTo( array, arrayIndex );
			}

			public bool Remove( KeyValuePair<string, T> item )
			{
				return this._dictionary.Remove( item );
			}

			public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
			{
				return this._dictionary.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return ( this._dictionary as IEnumerable ).GetEnumerator();
			}
		}
#endif
	}
}