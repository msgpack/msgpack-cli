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
using System.Reflection;
using System.Reflection.Emit;
using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		<see cref="SerializerEmitter"/> using <see cref="SerializationContext"/> to hold serializers for target members.
	/// </summary>
	internal sealed class ContextBasedSerializerEmitter : SerializerEmitter
	{
		private static readonly Type[] UnpackFromCoreParameterTypes = { typeof( SerializationContext ), typeof( Unpacker ) };
		private static readonly Type[] CreateInstanceParameterTypes = { typeof( SerializationContext ), typeof( int ) };

		private readonly Type _targetType;
		private readonly CollectionTraits _traits;
		private DynamicMethod _packToMethod;
		private DynamicMethod _unpackFromMethod;
		private DynamicMethod _unpackToMethod;
		private DynamicMethod _createInstanceMethod;
		private DynamicMethod _addItemMethod;
		private DynamicMethod _restoreSchemaMethod;

		/// <summary>
		///		Initializes a new instance of the <see cref="ContextBasedSerializerEmitter"/> class.
		/// </summary>
		/// <param name="specification">The specification of the serializer.</param>
		public ContextBasedSerializerEmitter( SerializerSpecification specification )
		{
			Contract.Requires( specification != null );

			this._targetType = specification.TargetType;
			this._traits = specification.TargetCollectionTraits;
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="MessagePackSerializer{T}.PackToCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="MessagePackSerializer{T}.PackToCore"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override TracingILGenerator GetPackToMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}::{1}", MethodBase.GetCurrentMethod(), "PackToCore" );
			}

			if ( this._packToMethod == null )
			{
				this._packToMethod =
					new DynamicMethod(
						"PackToCore",
						null,
						new[] { typeof( SerializationContext ), typeof( Packer ), this._targetType }
					);
			}

			return new TracingILGenerator( this._packToMethod, SerializerDebugging.ILTraceWriter );
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="MessagePackSerializer{T}.UnpackFromCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="MessagePackSerializer{T}.UnpackFromCore"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override TracingILGenerator GetUnpackFromMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}::{1}", MethodBase.GetCurrentMethod(), "UnpackFromCore" );
			}

			if ( this._unpackFromMethod == null )
			{
				this._unpackFromMethod =
					new DynamicMethod(
						"UnpackFromCore",
						this._targetType,
						UnpackFromCoreParameterTypes
					);
			}

			return new TracingILGenerator( this._unpackFromMethod, SerializerDebugging.ILTraceWriter );
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="MessagePackSerializer{T}.UnpackToCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="MessagePackSerializer{T}.UnpackToCore"/> overrides.
		/// </returns>
		public override TracingILGenerator GetUnpackToMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}::{1}", MethodBase.GetCurrentMethod(), "UnpackToCore" );
			}

			if ( this._unpackToMethod == null )
			{
				this._unpackToMethod = new DynamicMethod(
					"UnpackToCore",
					null,
					new[] { typeof( SerializationContext ), typeof( Unpacker ), this._targetType } );
			}

			return new TracingILGenerator( this._unpackToMethod, SerializerDebugging.ILTraceWriter );
		}

		/// <summary>
		///		Gets the IL generator to implement AddItem(TCollection, TItem) or AddItem(TCollection, object) overrides.
		/// </summary>
		/// <param name="declaration">The virtual method declaration to be overriden.</param>
		/// <returns>
		///		The IL generator to implement AddItem(TCollection, TItem) or AddItem(TCollection, object) overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override TracingILGenerator GetAddItemMethodILGenerator( MethodInfo declaration )
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}::{1}", MethodBase.GetCurrentMethod(), "AddItem" );
			}

			if ( this._addItemMethod == null )
			{
				this._addItemMethod =
					new DynamicMethod(
						"AddItem",
						null,
						( this._traits.DetailedCollectionType == CollectionDetailedKind.GenericDictionary
#if !NETFX_40 && !NETFX_35 && !SILVERLIGHT
							|| this._traits.DetailedCollectionType == CollectionDetailedKind.GenericReadOnlyDictionary
#endif // !NETFX_40 && !NETFX_35 && !SILVERLIGHT
						)
						? new[] { typeof( SerializationContext ), this._targetType, this._traits.ElementType.GetGenericArguments()[ 0 ], this._traits.ElementType.GetGenericArguments()[ 1 ] }
						: new[] { typeof( SerializationContext ), this._targetType, this._traits.ElementType }
					);
			}

			return new TracingILGenerator( this._addItemMethod, SerializerDebugging.ILTraceWriter );
		}

		/// <summary>
		///		Gets the IL generator to implement CreateInstance(int) overrides.
		/// </summary>
		/// <param name="declaration">The virtual method declaration to be overriden.</param>
		/// <returns>
		///		The IL generator to implement CreateInstance(int) overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override TracingILGenerator GetCreateInstanceMethodILGenerator( MethodInfo declaration )
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}::{1}", MethodBase.GetCurrentMethod(), "CreateInstance" );
			}

			if ( this._createInstanceMethod == null )
			{
				this._createInstanceMethod =
					new DynamicMethod(
						"CreateInstance",
						this._targetType,
						CreateInstanceParameterTypes
					);
			}

			return new TracingILGenerator( this._createInstanceMethod, SerializerDebugging.ILTraceWriter );
		}

		/// <summary>
		///		Gets the IL generator to implement private static RestoreSchema() method.
		/// </summary>
		/// <returns>
		///		The IL generator to implement RestoreSchema() static method.
		///		This value will not be <c>null</c>.
		/// </returns>
		public override TracingILGenerator GetRestoreSchemaMethodILGenerator()
		{
			if ( SerializerDebugging.TraceEnabled )
			{
				SerializerDebugging.TraceEvent( "{0}::{1}", MethodBase.GetCurrentMethod(), "RestoreSchema" );
			}

			if ( this._restoreSchemaMethod == null )
			{
				this._restoreSchemaMethod =
					new DynamicMethod(
						"RestoreSchema",
						typeof( PolymorphismSchema ),
						ReflectionAbstractions.EmptyTypes
					);
			}

			return new TracingILGenerator( this._restoreSchemaMethod, SerializerDebugging.ILTraceWriter );
		}

		/// <summary>
		///		Creates the serializer type built now and returns its constructor.
		/// </summary>
		/// <typeparam name="T">The type of serialization target.</typeparam>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer{T}"/> type constructor.
		///		This value will not be <c>null</c>.
		///	</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Well patterned" )]
		public override Func<SerializationContext, PolymorphismSchema, MessagePackSerializer<T>> CreateConstructor<T>()
		{
			var traits = typeof( T ).GetCollectionTraits();
			// Avoids capture of this pointer.
			var createInstanceMethod = this._createInstanceMethod;
			var unpackFromMethod = this._unpackFromMethod;
			var addItemMethod = this._addItemMethod;
			switch ( traits.DetailedCollectionType )
			{
				case CollectionDetailedKind.NonGenericEnumerable:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<IEnumerableCallbackSerializerFactory>(
							typeof( NonGenericEnumerableCallbackSerializerFactory<> ).MakeGenericType( typeof( T ) )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						( context, schema ) =>
							factory.Create( context, schema, createInstanceMethod, unpackFromMethod, addItemMethod ) as MessagePackSerializer<T>;
				}
				case CollectionDetailedKind.NonGenericCollection:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<IEnumerableCallbackSerializerFactory>(
							typeof( NonGenericCollectionCallbackSerializerFactory<> ).MakeGenericType( typeof( T ) )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						( context, schema ) =>
							factory.Create( context, schema, createInstanceMethod, unpackFromMethod, addItemMethod ) as MessagePackSerializer<T>;
				}
				case CollectionDetailedKind.NonGenericList:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICollectionCallbackSerializerFactory>(
							typeof( NonGenericListCallbackSerializerFactory<> ).MakeGenericType( typeof( T ) )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						( context, schema ) =>
							factory.Create( context, schema, createInstanceMethod, addItemMethod ) as MessagePackSerializer<T>;
				}
				case CollectionDetailedKind.NonGenericDictionary:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICollectionCallbackSerializerFactory>(
							typeof( NonGenericDictionaryCallbackSerializerFactory<> ).MakeGenericType( typeof( T ) )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						( context, schema ) =>
							factory.Create( context, schema, createInstanceMethod, addItemMethod ) as MessagePackSerializer<T>;
				}
				case CollectionDetailedKind.GenericEnumerable:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<IEnumerableCallbackSerializerFactory>(
							typeof( EnumerableCallbackSerializerFactory<,> ).MakeGenericType( typeof( T ), traits.ElementType )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						( context, schema ) =>
							factory.Create( context, schema, createInstanceMethod, unpackFromMethod, addItemMethod ) as MessagePackSerializer<T>;
				}
				case CollectionDetailedKind.GenericCollection:
#if !NETFX_35 && !UNITY
				case CollectionDetailedKind.GenericSet:
#endif // !NETFX_35 && !UNITY
				case CollectionDetailedKind.GenericList:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICollectionCallbackSerializerFactory>(
							typeof( CollectionCallbackSerializerFactory<,> ).MakeGenericType( typeof( T ), traits.ElementType )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						( context, schema ) =>
							factory.Create( context, schema, createInstanceMethod, addItemMethod ) as MessagePackSerializer<T>;
				}
				case CollectionDetailedKind.GenericDictionary:
				{
					var keyValuePairGenericArguments = traits.ElementType.GetGenericArguments();
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICollectionCallbackSerializerFactory>(
							typeof( DictionaryCallbackSerializerFactory<,,> ).MakeGenericType(
								typeof( T ),
								keyValuePairGenericArguments[ 0 ],
								keyValuePairGenericArguments[ 1 ]
							)
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						( context, schema ) =>
							factory.Create( context, schema, createInstanceMethod, addItemMethod ) as MessagePackSerializer<T>;
				}
#if !NETFX_35 && !UNITY && !NETFX_40 && !SILVERLIGHT
				case CollectionDetailedKind.GenericReadOnlyCollection:
				case CollectionDetailedKind.GenericReadOnlyList:
				{
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICollectionCallbackSerializerFactory>(
							typeof( ReadOnlyCollectionCallbackSerializerFactory<,> ).MakeGenericType( typeof( T ), traits.ElementType )
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						( context, schema ) =>
							factory.Create( context, schema, createInstanceMethod, addItemMethod ) as MessagePackSerializer<T>;
				}
				case CollectionDetailedKind.GenericReadOnlyDictionary:
				{
					var keyValuePairGenericArguments = traits.ElementType.GetGenericArguments();
					var factory =
						ReflectionExtensions.CreateInstancePreservingExceptionType<ICollectionCallbackSerializerFactory>(
							typeof( ReadOnlyDictionaryCallbackSerializerFactory<,,> ).MakeGenericType(
								typeof( T ),
								keyValuePairGenericArguments[ 0 ],
								keyValuePairGenericArguments[ 1 ]
							)
						);
#if DEBUG
					Contract.Assert( factory != null );
#endif // DEBUG

					return
						( context, schema ) =>
							factory.Create( context, schema, createInstanceMethod, addItemMethod ) as MessagePackSerializer<T>;
				}
#endif // !NETFX_35 && !UNITY && !NETFX_40 && !SILVERLIGHT
				default:
				{
					var packTo =
						this._packToMethod.CreateDelegate( typeof( Action<SerializationContext, Packer, T> ) ) as
							Action<SerializationContext, Packer, T>;
					var unpackFrom =
						unpackFromMethod.CreateDelegate( typeof( Func<SerializationContext, Unpacker, T> ) ) as
							Func<SerializationContext, Unpacker, T>;
					var unpackTo = default( Action<SerializationContext, Unpacker, T> );

					if ( this._unpackToMethod != null )
					{
						unpackTo =
							this._unpackToMethod.CreateDelegate( typeof( Action<SerializationContext, Unpacker, T> ) ) as
								Action<SerializationContext, Unpacker, T>;
					}

					return ( context, schema ) => new CallbackMessagePackSerializer<T>( context, packTo, unpackFrom, unpackTo );
				}
			}
		}

		/// <summary>
		///		Regisgters <see cref="MessagePackSerializer{T}"/> of target type usage to the current emitting session.
		/// </summary>
		/// <param name="targetType">The type of the member to be serialized/deserialized.</param>
		/// <param name="enumMemberSerializationMethod">The enum serialization method of the member to be serialized/deserialized.</param>
		/// <param name="dateTimeConversionMethod">The date time conversion method of the member to be serialized/deserialized.</param>
		/// <param name="polymorphismSchema">The schema for polymorphism support.</param>
		/// <param name="schemaRegenerationCodeProvider">The delegate to provide constructs to emit schema regeneration codes.</param>
		/// <returns>
		///		<see cref=" Action{T1,T2}"/> to emit serializer retrieval instructions.
		///		The 1st argument should be <see cref="TracingILGenerator"/> to emit instructions.
		///		The 2nd argument should be argument index of the serializer holder, normally 0 (this pointer).
		///		This value will not be <c>null</c>.
		/// </returns>
		public override Action<TracingILGenerator, int> RegisterSerializer(
			Type targetType,
			EnumMemberSerializationMethod enumMemberSerializationMethod,
			DateTimeMemberConversionMethod dateTimeConversionMethod,
			PolymorphismSchema polymorphismSchema,
			Func<IEnumerable<ILConstruct>> schemaRegenerationCodeProvider
		)
		{
			// This return value should not be used.
			return ( g, i ) => { throw new NotImplementedException(); };
		}

		private interface IEnumerableCallbackSerializerFactory
		{
			object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				DynamicMethod createInstance,
				DynamicMethod unpackFrom,
				DynamicMethod addItem
			);
		}

		private abstract class EnumerableCallbackSerializerFactoryBase<TCollection, TItem> :
			IEnumerableCallbackSerializerFactory
		{
			protected abstract object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<SerializationContext, int, TCollection> createInstance,
				Func<SerializationContext, Unpacker, TCollection> unpackFrom,
				Action<SerializationContext, TCollection, TItem> addItem
			);

			public object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				DynamicMethod createInstance,
				DynamicMethod unpackFrom,
				DynamicMethod addItem
			)
			{
				return
					this.Create(
						context,
						schema,
						( Func<SerializationContext, int, TCollection> )createInstance.CreateDelegate(
							typeof( Func<,,> ).MakeGenericType( typeof( SerializationContext ), typeof( int ), typeof( TCollection ) )
						),
						( Func<SerializationContext, Unpacker, TCollection> )unpackFrom.CreateDelegate(
							typeof( Func<SerializationContext, Unpacker, TCollection> )
						),
						addItem == null
						? null
						: ( Action<SerializationContext, TCollection, TItem> )addItem.CreateDelegate(
							typeof( Action<,,> ).MakeGenericType( typeof( SerializationContext ), typeof( TCollection ), typeof( TItem ) )
						)
					);

			}
		}

		private sealed class EnumerableCallbackSerializerFactory<TCollection, TItem> : EnumerableCallbackSerializerFactoryBase<TCollection, TItem>
			where TCollection : IEnumerable<TItem>
		{
			public EnumerableCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<SerializationContext, int, TCollection> createInstance,
				Func<SerializationContext, Unpacker, TCollection> unpackFrom,
				Action<SerializationContext, TCollection, TItem> addItem
			)
			{
				return
					new CallbackEnumerableMessagePackSerializer<TCollection, TItem>(
						context,
						schema,
						createInstance,
						unpackFrom,
						addItem
					);
			}
		}

		private sealed class NonGenericEnumerableCallbackSerializerFactory<TCollection> : EnumerableCallbackSerializerFactoryBase<TCollection, object>
			where TCollection : IEnumerable
		{
			public NonGenericEnumerableCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<SerializationContext, int, TCollection> createInstance,
				Func<SerializationContext, Unpacker, TCollection> unpackFrom,
				Action<SerializationContext, TCollection, object> addItem
			)
			{
				return
					new CallbackNonGenericEnumerableMessagePackSerializer<TCollection>(
						context,
						schema,
						createInstance,
						unpackFrom,
						addItem
					);
			}
		}

		private sealed class NonGenericCollectionCallbackSerializerFactory<TCollection> : EnumerableCallbackSerializerFactoryBase<TCollection, object>
			where TCollection : ICollection
		{
			public NonGenericCollectionCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<SerializationContext, int, TCollection> createInstance,
				Func<SerializationContext, Unpacker, TCollection> unpackFrom,
				Action<SerializationContext, TCollection, object> addItem
			)
			{
				return
					new CallbackNonGenericCollectionMessagePackSerializer<TCollection>(
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
				DynamicMethod createInstance,
				DynamicMethod addItem
			);
		}

		private abstract class CollectionCallbackSerializerFactoryBase<TCollection, TItem> : ICollectionCallbackSerializerFactory
		{
			protected abstract object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<SerializationContext, int, TCollection> createInstance,
				Action<SerializationContext, TCollection, TItem> addItem
			);

			public object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				DynamicMethod createInstance,
				DynamicMethod addItem
			)
			{
				return
					this.Create(
						context,
						schema,
						( Func<SerializationContext, int, TCollection> )createInstance.CreateDelegate(
							typeof( Func<,,> ).MakeGenericType( typeof( SerializationContext ), typeof( int ), typeof( TCollection ) )
						),
						addItem == null
						? null
						: ( Action<SerializationContext, TCollection, TItem> )addItem.CreateDelegate(
							typeof( Action<,,> ).MakeGenericType( typeof( SerializationContext ), typeof( TCollection ), typeof( TItem ) )
						)
					);

			}
		}

		private sealed class CollectionCallbackSerializerFactory<TCollection, TItem> : CollectionCallbackSerializerFactoryBase<TCollection, TItem>
			where TCollection : ICollection<TItem>
		{
			public CollectionCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<SerializationContext, int, TCollection> createInstance,
				Action<SerializationContext, TCollection, TItem> addItem
			)
			{
				return
					new CallbackCollectionMessagePackSerializer<TCollection, TItem>(
						context,
						schema,
						createInstance
					);
			}
		}

#if !NETFX_35 && !UNITY && !NETFX_40 && !SILVERLIGHT
		private sealed class ReadOnlyCollectionCallbackSerializerFactory<TCollection, TItem> : CollectionCallbackSerializerFactoryBase<TCollection, TItem>
			where TCollection : IReadOnlyCollection<TItem>
		{
			public ReadOnlyCollectionCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<SerializationContext, int, TCollection> createInstance,
				Action<SerializationContext, TCollection, TItem> addItem
			)
			{
				return
					new CallbackReadOnlyCollectionMessagePackSerializer<TCollection, TItem>(
						context,
						schema,
						createInstance,
						addItem
					);
			}
		}
#endif // !NETFX_35 && !UNITY && !NETFX_40 && !SILVERLIGHT


		private sealed class NonGenericListCallbackSerializerFactory<TCollection> : CollectionCallbackSerializerFactoryBase<TCollection, object>
			where TCollection : IList
		{
			public NonGenericListCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<SerializationContext, int, TCollection> createInstance,
				Action<SerializationContext, TCollection, object> addItem
			)
			{
				return
					new CallbackNonGenericListMessagePackSerializer<TCollection>(
						context,
						schema,
						createInstance
					);
			}
		}

		private sealed class NonGenericDictionaryCallbackSerializerFactory<TDictionary> : CollectionCallbackSerializerFactoryBase<TDictionary, DictionaryEntry>
			where TDictionary : IDictionary
		{
			public NonGenericDictionaryCallbackSerializerFactory() { }

			protected override object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<SerializationContext, int, TDictionary> createInstance,
				Action<SerializationContext, TDictionary, DictionaryEntry> addItem
			)
			{
				return
					new CallbackNonGenericDictionaryMessagePackSerializer<TDictionary>(
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
				Func<SerializationContext, int, TDictionary> createInstance
			)
			{
				return
					new CallbackDictionaryMessagePackSerializer<TDictionary, TKey, TValue>(
						context,
						schema,
						createInstance
					);
			}

			public object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				DynamicMethod createInstance,
				DynamicMethod addItem
			)
			{
				return
					Create(
						context,
						schema,
						( Func<SerializationContext, int, TDictionary> )createInstance.CreateDelegate(
							typeof( Func<,,> ).MakeGenericType( typeof( SerializationContext ), typeof( int ), typeof( TDictionary ) )
						)
					);
			}
		}

#if !NETFX_35 && !UNITY && !NETFX_40 && !SILVERLIGHT
		private sealed class ReadOnlyDictionaryCallbackSerializerFactory<TDictionary, TKey, TValue> : ICollectionCallbackSerializerFactory
			where TDictionary : IReadOnlyDictionary<TKey, TValue>
		{
			public ReadOnlyDictionaryCallbackSerializerFactory() { }

			private static object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				Func<SerializationContext, int, TDictionary> createInstance,
				Action<SerializationContext, TDictionary, TKey, TValue> addItem
			)
			{
				return
					new CallbackReadOnlyDictionaryMessagePackSerializer<TDictionary, TKey, TValue>(
						context,
						schema,
						createInstance,
						addItem
					);
			}

			public object Create(
				SerializationContext context,
				PolymorphismSchema schema,
				DynamicMethod createInstance,
				DynamicMethod addItem
			)
			{
				return
					Create(
						context,
						schema,
						( Func<SerializationContext, int, TDictionary> )createInstance.CreateDelegate(
							typeof( Func<,,> ).MakeGenericType( typeof( SerializationContext ), typeof( int ), typeof( TDictionary ) )
						),
						addItem == null
						? null
						: ( Action<SerializationContext, TDictionary, TKey, TValue> )addItem.CreateDelegate(
							typeof( Action<,,,> ).MakeGenericType( typeof( SerializationContext ), typeof( TDictionary ), typeof( TKey ), typeof( TValue ) )
						)
					);
			}
		}
#endif // !NETFX_35 && !UNITY && !NETFX_40 && !SILVERLIGHT
	}
}
