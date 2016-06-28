#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015-2016 FUJIWARA, Yusuke and contributors
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

using System;
using System.Collections.Generic;
#if CORE_CLR || UNITY
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || UNITY
using System.Linq;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.AbstractSerializers
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Well patterned" )]
	partial class SerializerBuilder<TContext, TConstruct>
	{
		private void BuildCollectionSerializer(
			TContext context,
			Type concreteType,
			PolymorphismSchema schema
		)
		{
#if DEBUG
			Contract.Assert( this.CollectionTraits.DetailedCollectionType != CollectionDetailedKind.Array );
#endif // DEBUG
			bool isUnpackFromRequired;
			bool isAddItemRequired;
			this.DetermineSerializationStrategy( out isUnpackFromRequired, out isAddItemRequired );

			if ( typeof( IPackable ).IsAssignableFrom( this.TargetType ) )
			{
				this.BuildIPackablePackTo( context );
			}
#if FEATURE_TAP

			if ( this.WithAsync( context ) )
			{
				if ( typeof( IAsyncPackable ).IsAssignableFrom( this.TargetType ) )
				{
					this.BuildIAsyncPackablePackTo( context );
				}
			}

#endif // FEATURE_TAP

			this.BuildCollectionCreateInstance( context, concreteType );

			var useUnpackable = false;

			if ( typeof( IUnpackable ).IsAssignableFrom( concreteType ?? this.TargetType ) )
			{
				this.BuildIUnpackableUnpackFrom( context, this.GetUnpackableCollectionInstantiation( context ) );
				useUnpackable = true;
			}

#if FEATURE_TAP

			if ( this.WithAsync( context ) )
			{
				if ( typeof( IAsyncUnpackable ).IsAssignableFrom( concreteType ?? this.TargetType ) )
				{
					this.BuildIAsyncUnpackableUnpackFrom( context, this.GetUnpackableCollectionInstantiation( context ) );
					useUnpackable = true;
				}
			}

#endif // FEATURE_TAP

			if ( isAddItemRequired )
			{
				if ( useUnpackable )
				{
					// AddItem should never called because UnpackFromCore calls IUnpackable/IAsyncUnpackable
					this.BuildCollectionAddItemNotImplemented( context );
				}
				else
				{
					// For IEnumerable implements and IReadOnlyXXX implements
					this.BuildCollectionAddItem( 
						context, 
						this.CollectionTraits.AddMethod != null
						? this.CollectionTraits // For declared collection.
						: ( concreteType ?? this.TargetType ).GetCollectionTraits( CollectionTraitOptions.Full, context.SerializationContext.CompatibilityOptions.AllowNonCollectionEnumerableTypes ) // For concrete collection.
					);
				}
			}

			if ( isUnpackFromRequired && !useUnpackable )
			{
				this.BuildCollectionUnpackFromCore( context, concreteType, schema, false );
#if FEATURE_TAP
				if ( this.WithAsync( context ) )
				{
					this.BuildCollectionUnpackFromCore( context, concreteType, schema, true );
				}
#endif // FEATURE_TAP
			}

			this.BuildRestoreSchema( context, schema );
		}

		private void DetermineSerializationStrategy( out bool isUnpackFromRequired, out bool isAddItemRequired )
		{
			switch ( this.CollectionTraits.DetailedCollectionType )
			{
				case CollectionDetailedKind.NonGenericEnumerable:
				case CollectionDetailedKind.NonGenericCollection:
				{
					isUnpackFromRequired = true;
					isAddItemRequired = true;
					break;
				}
				case CollectionDetailedKind.NonGenericList:
				{
					isUnpackFromRequired = false;
					isAddItemRequired = false;
					break;
				}
				case CollectionDetailedKind.NonGenericDictionary:
				{
					isUnpackFromRequired = false;
					isAddItemRequired = false;
					break;
				}
				case CollectionDetailedKind.GenericEnumerable:
				{
					isUnpackFromRequired = true;
					isAddItemRequired = true;
					break;
				}
				case CollectionDetailedKind.GenericDictionary:
				{
					isUnpackFromRequired = false;
					isAddItemRequired = false;
					break;
				}
#if !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				case CollectionDetailedKind.GenericReadOnlyDictionary:
				{
					isUnpackFromRequired = false;
					isAddItemRequired = true;
					break;
				}
				case CollectionDetailedKind.GenericReadOnlyList:
				case CollectionDetailedKind.GenericReadOnlyCollection:
				{
					isUnpackFromRequired = false;
					isAddItemRequired = true;
					break;
				}
#endif // !NETFX_35 && !UNITY && !NETFX_40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				default:
				{
					isUnpackFromRequired = false;
					isAddItemRequired = false;
					break;
				}
			} // switch
		}

		private TConstruct GetUnpackableCollectionInstantiation( TContext context )
		{
			return
				this.EmitInvokeMethodExpression(
					context,
					this.EmitThisReferenceExpression( context ),
					context.GetDeclaredMethod( MethodName.CreateInstance ),
					this.MakeInt32Literal( context, 0 ) // Always 0
				);
		}

		#region -- AddItem --

		private void BuildCollectionAddItem( TContext context, CollectionTraits traits )
		{
			var addItem = this.BaseClass.GetRuntimeMethod( MethodName.AddItem );
			context.BeginMethodOverride( MethodName.AddItem );
			context.EndMethodOverride(
				MethodName.AddItem,
				traits.CollectionType == CollectionKind.Map
				? this.EmitAppendDictionaryItem(
					context,
					traits,
					context.CollectionToBeAdded,
					addItem.GetParameters()[ 0 ].ParameterType,
					context.KeyToAdd,
					addItem.GetParameters()[ 1 ].ParameterType,
					context.ValueToAdd,
					false
				)
				: this.EmitAppendCollectionItem(
					context,
					null,
					traits,
					context.CollectionToBeAdded,
					context.ItemToAdd
				)
			);
		}

		private void BuildCollectionAddItemNotImplemented( TContext context )
		{
			context.BeginMethodOverride( MethodName.AddItem );
			context.EndMethodOverride(
				MethodName.AddItem,
				this.EmitSequentialStatements( context, typeof( void ) ) // nop
			);
		}

		#endregion -- AddItem --

		#region -- UnpackFromCore --

		private void BuildCollectionUnpackFromCore( TContext context, Type concreteType, PolymorphismSchema schema, bool isAsync )
		{
			var methodName =
#if FEATURE_TAP
				 isAsync ? MethodName.UnpackFromAsyncCore :
#endif // FEATURE_TAP
				 MethodName.UnpackFromCore;

			context.BeginMethodOverride( methodName );

			var instanceType = concreteType ?? this.TargetType;

			context.EndMethodOverride(
				methodName,
				this.EmitSequentialStatements(
					context,
					this.TargetType,
					this.EmitCollectionUnpackFromStatements( context, instanceType, schema, isAsync )
				)
			);
		}

		private IEnumerable<TConstruct> EmitCollectionUnpackFromStatements( TContext context, Type instanceType, PolymorphismSchema schema, bool isAsync )
		{
			// Header check
			yield return
				this.CollectionTraits.CollectionType == CollectionKind.Array
					? this.EmitCheckIsArrayHeaderExpression( context, context.Unpacker )
					: this.EmitCheckIsMapHeaderExpression( context, context.Unpacker );

			var itemsCount = this.DeclareLocal( context, typeof( int ), "itemsCount" );

			// Unpack items count and store it
			yield return itemsCount;
			yield return
				this.EmitStoreVariableStatement(
					context,
					itemsCount,
					this.EmitGetItemsCountExpression( context, context.Unpacker )
				);

			var createInstance =
				this.EmitInvokeMethodExpression(
					context,
					this.EmitThisReferenceExpression( context ),
					context.GetDeclaredMethod( MethodName.CreateInstance ),
					itemsCount
				);
			var collection =
				instanceType == this.TargetType
					? createInstance
					: this.EmitUnboxAnyExpression( context, instanceType, createInstance );

			// Get delegates to UnpackHelpers
			TConstruct iterative;
			TConstruct bulk;
			if ( this.CollectionTraits.CollectionType == CollectionKind.Array && this.CollectionTraits.AddMethod == null )
			{
				// Try to use concrete collection's Add.
				var traitsOfTheCollection = instanceType.GetCollectionTraits( CollectionTraitOptions.Full, context.SerializationContext.CompatibilityOptions.AllowNonCollectionEnumerableTypes);

				bulk =
					this.MakeNullLiteral(
						context,
#if FEATURE_TAP
						isAsync ? TypeDefinition.GenericReferenceType( typeof( Func<,,,,> ), typeof( Unpacker ), this.TargetType, typeof( int ), typeof( CancellationToken ), typeof( Task ) ) :
#endif // FEATURE_TAP
						TypeDefinition.GenericReferenceType( typeof( Action<,,> ), typeof( Unpacker ), this.TargetType, typeof( int ) )
					);

				var indexOfItemParameter = context.IndexOfItem;
				var itemsCountParameter = context.ItemsCount;
				var appendToTargetParameter = context.CollectionToBeAdded;
				var unpackedItemParameter = context.DefineUnpackedItemParameterInSetValueMethods( this.CollectionTraits.ElementType );
				var unpackItemValueArguments = 
					new[] { context.Unpacker, context.UnpackToTarget, indexOfItemParameter, itemsCountParameter }
#if FEATURE_TAP
					.Concat( isAsync ? new[] { this.ReferCancellationToken( context, 2 ) } : NoConstructs ).ToArray()
#endif // FEATURE_TAP
					;

				iterative =
					this.ExtractPrivateMethod(
						context,
						AdjustName( MethodName.UnpackCollectionItem, isAsync ),
#if FEATURE_TAP
						isAsync ? typeof( Task ) :
#endif // FEATURE_TAP
						typeof( void ),
						() => this.EmitUnpackItemValueStatement(	
							context,
							traitsOfTheCollection.ElementType,
							this.EmitInvariantStringFormat( context, "item{0}", indexOfItemParameter ),
							context.CollectionItemNilImplication,
							null,
							( schema ?? PolymorphismSchema.Default ).ItemSchema,
							context.Unpacker,
							context.UnpackToTarget,
							indexOfItemParameter,
							itemsCountParameter,
							this.ExtractPrivateMethod(
								context,
								MethodName.AppendUnpackedItem,
								typeof( void ),
								() => this.EmitAppendCollectionItem(
									context,
									null,
									traitsOfTheCollection,
									appendToTargetParameter,
									unpackedItemParameter
								),
								appendToTargetParameter,
								unpackedItemParameter
							),
							true, // forMap, this method should not be called IDictionary[<,>]
							isAsync
						),
						unpackItemValueArguments
					);
			}
			else
			{
				// Invoke UnpackToCore because AddItem override/inheritance is available.
				bulk = this.EmitGetActionsExpression( context, ActionType.UnpackTo, isAsync );
				context.IsUnpackToUsed = true;
				iterative =
					this.MakeNullLiteral(
						context,
#if FEATURE_TAP
						isAsync ? TypeDefinition.GenericReferenceType( typeof( Func<,,,,,> ), typeof( Unpacker ), this.TargetType, typeof( int ), typeof( int ), typeof( CancellationToken ), typeof( Task ) ) :
#endif // FEATURE_TAP
						TypeDefinition.GenericReferenceType( typeof( Action<,,,> ), typeof( Unpacker ), this.TargetType, typeof( int ), typeof( int ) )
					);
			}

			var arguments =
				new[] { context.Unpacker, itemsCount, collection, bulk, iterative }
#if FEATURE_TAP
				.Concat( isAsync ? new[] { this.ReferCancellationToken( context, 2 ) } : NoConstructs ).ToArray()
#endif // FEATURE_TAP
				;

			// Call UnpackHelpers
			yield return
				this.EmitRetrunStatement(
					context,
					this.EmitInvokeMethodExpression(
						context,
						null,
#if FEATURE_TAP
						isAsync ? Metadata._UnpackHelpers.UnpackCollectionAsync_1Method.MakeGenericMethod( this.TargetType ) :
#endif // FEATURE_TAP
						Metadata._UnpackHelpers.UnpackCollection_1Method.MakeGenericMethod( this.TargetType ),
						arguments
					)
				);
		}

		private TConstruct EmitGetItemsCountExpression( TContext context, TConstruct unpacker )
		{
			return
				this.EmitInvokeMethodExpression(
					context,
					null,
					Metadata._UnpackHelpers.GetItemsCount,
					unpacker
				);
		}

		#endregion -- UnpackFromCore --
		
		#region -- CreateInstance --

		private void BuildCollectionCreateInstance( TContext context, Type concreteType )
		{
			context.BeginMethodOverride( MethodName.CreateInstance );

			var instanceType = concreteType ?? this.TargetType;
			var collection =
				this.DeclareLocal(
					context,
					this.TargetType,
					"collection"
				);
			var ctor = UnpackHelpers.GetCollectionConstructor( instanceType );
			var ctorArguments = this.DetermineCollectionConstructorArguments( context, ctor );
			context.EndMethodOverride(
				MethodName.CreateInstance,
				this.EmitSequentialStatements(
					context,
					this.TargetType,
					collection,
					this.EmitStoreVariableStatement(
						context,
						collection,
						this.EmitCreateNewObjectExpression(
							context,
							collection,
							ctor,
							ctorArguments
						)
					),
					this.EmitRetrunStatement(
						context,
						this.EmitLoadVariableExpression( context, collection )
					)
				)
			);
		}

		#endregion -- CreateInstance --

		#region -- RestoreSchema --

		private void BuildRestoreSchema( TContext context, PolymorphismSchema schema )
		{
			context.BeginPrivateMethod( MethodName.RestoreSchema, true, typeof( PolymorphismSchema ) );

			var storage =
				this.DeclareLocal(
					context,
					typeof( PolymorphismSchema ),
					"schema"
				);
			context.EndPrivateMethod(
				MethodName.RestoreSchema,
				this.EmitSequentialStatements(
					context,
					typeof( PolymorphismSchema ),
					new[] { storage }
					.Concat( this.EmitConstructPolymorphismSchema( context, storage, schema ) )
					.Concat( new[] { this.EmitRetrunStatement( context, this.EmitLoadVariableExpression( context, storage ) ) } )
				)
			);
		}

		#endregion -- RestoreSchema --

		#region -- Constructor Helpers --

		protected internal TConstruct EmitUnpackToInitialization( TContext context )
		{
			// This method should be called at most once, so caching following array should be wasting.
			var parameterTypes = new[] { typeof( Unpacker ), this.TargetType, typeof( int ) };
			var initUnpackTo =
				this.EmitSetField(
					context,
					this.EmitThisReferenceExpression( context ),
					context.GetDeclaredField( FieldName.UnpackTo ),
					this.EmitNewPrivateMethodDelegateExpression(
						context,
						this.BaseClass.GetRuntimeMethod( MethodName.UnpackToCore, parameterTypes )
					)
				);

#if FEATURE_TAP
			if ( context.SerializationContext.SerializerOptions.WithAsync )
			{
				// This method should be called at most once, so caching following array should be wasting.
				var asyncParameterTypes = new[] { typeof( Unpacker ), this.TargetType, typeof( int ), typeof( CancellationToken ) };
				var initAsyncUnpackTo =
					this.EmitSetField(
						context,
						this.EmitThisReferenceExpression( context ),
						context.GetDeclaredField( FieldName.UnpackTo + "Async" ),
						this.EmitNewPrivateMethodDelegateExpression(
							context,
							this.BaseClass.GetRuntimeMethod( MethodName.UnpackToAsyncCore, asyncParameterTypes )
						)
					);

				return this.EmitSequentialStatements( context, typeof( void ), initUnpackTo, initAsyncUnpackTo );
			}
#endif // FEATURE_TAP

			return initUnpackTo;
		}
		
		#endregion -- Constructor Helpers --
	}
}
