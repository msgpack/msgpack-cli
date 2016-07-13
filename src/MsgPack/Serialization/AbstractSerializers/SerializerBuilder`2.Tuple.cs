#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
using System.Collections.Generic;
#if CORE_CLR
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR
using System.Linq;
using System.Reflection;
#if FEATURE_TAP
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct>
	{
		private void BuildTupleSerializer( TContext context, IList<PolymorphismSchema> itemSchemaList, out SerializationTarget targetInfo )
		{
			var itemTypes = TupleItems.GetTupleItemTypes( this.TargetType );
			targetInfo = SerializationTarget.CreateForTuple( itemTypes );

			this.BuildTuplePackTo( context, itemTypes, itemSchemaList, false );

#if FEATURE_TAP
			if ( this.WithAsync( context ) )
			{
				this.BuildTuplePackTo( context, itemTypes, itemSchemaList, true );
			}
#endif // FEATURE_TAP

			this.BuildTupleUnpackFrom( context, itemTypes, itemSchemaList, false );

#if FEATURE_TAP
			if ( this.WithAsync( context ) )
			{
				this.BuildTupleUnpackFrom( context, itemTypes, itemSchemaList, true );
			}
#endif // FEATURE_TAP
		}

		#region -- PackTo --

		private void BuildTuplePackTo( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList, bool isAsync )
		{
			/*
				 packer.PackArrayHeader( cardinarity );
				 _serializer0.PackTo( packer, tuple.Item1 );
					:
				 _serializer6.PackTo( packer, tuple.item7 );
				 _serializer7.PackTo( packer, tuple.Rest.Item1 );
			*/
			var methodName = 
#if FEATURE_TAP
				isAsync ? MethodName.PackToAsyncCore : 
#endif // FEATURE_TAP
				MethodName.PackToCore;

			context.BeginMethodOverride( methodName );
			context.EndMethodOverride(
				methodName,
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					this.BuildTuplePackToCore( context, itemTypes, itemSchemaList, isAsync )
				)
			);
		}

		private IEnumerable<TConstruct> BuildTuplePackToCore( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList, bool isAsync )
		{
			// Note: cardinality is put as array length by PackHelper.
			var depth = -1;
			var tupleTypeList = TupleItems.CreateTupleTypeList( itemTypes );
			var propertyInvocationChain = new List<PropertyInfo>( itemTypes.Count % 7 + 1 );
			var packValueArguments =
				new[] { context.Packer, context.PackToTarget }
#if FEATURE_TAP
				.Concat( isAsync ? new[] { this.ReferCancellationToken( context, 3 ) } : NoConstructs ).ToArray()
#endif // FEATURE_TAP
				;

			for ( int i = 0; i < itemTypes.Count; i++ )
			{
				if ( i % 7 == 0 )
				{
					depth++;
				}

				for ( int j = 0; j < depth; j++ )
				{
					// .TRest.TRest ...
					var restProperty = tupleTypeList[ j ].GetProperty( "Rest" );
#if DEBUG
					Contract.Assert( restProperty != null );
#endif
					propertyInvocationChain.Add( restProperty );
				}

				var itemNProperty = tupleTypeList[ depth ].GetProperty( "Item" + ( ( i % 7 ) + 1 ) );
				propertyInvocationChain.Add( itemNProperty );
#if DEBUG
				Contract.Assert(
					itemNProperty != null,
					tupleTypeList[ depth ].GetFullName() + "::Item" + ( ( i % 7 ) + 1 ) + " [ " + depth + " ] @ " + i
				);
#endif
				var count = i;
				this.ExtractPrivateMethod(
					context,
					AdjustName( MethodNamePrefix.PackValue + SerializationTarget.GetTupleItemNameFromIndex( i ), isAsync ),
#if FEATURE_TAP
					isAsync ? typeof( Task ) :
#endif // FEATURE_TAP
					typeof( void ),
					() => this.EmitSequentialStatements(
						context,
						typeof( void ),
						this.EmitPackTupleItemStatements(
							context,
							itemTypes[ count ],
							context.Packer,
							context.PackToTarget,
							propertyInvocationChain,
							itemSchemaList.Count == 0 ? null : itemSchemaList[ count ],
							isAsync
						)
					),
					packValueArguments
				);

				propertyInvocationChain.Clear();
			}

			var packHelperArguments =
				new[]
				{
					context.Packer,
					context.PackToTarget,
					this.EmitGetActionsExpression( context, ActionType.PackToArray, isAsync )
				}
#if FEATURE_TAP
				.Concat( isAsync ? new[] { this.ReferCancellationToken( context, 3 ) } : NoConstructs ).ToArray()
#endif // FEATURE_TAP
				;

			var packToArray =
				this.EmitInvokeMethodExpression(
					context,
					null,
					new MethodDefinition(
						AdjustName( MethodName.PackToArray, isAsync ),
						new [] { TypeDefinition.Object( this.TargetType ) },
						typeof( PackHelpers ),
#if FEATURE_TAP
						isAsync ? typeof( Task ) :
#endif // FEATURE_TAP
						typeof( void ),
						packHelperArguments.Select( a => a.ContextType ).ToArray()
					),
					packHelperArguments
				);

			if ( isAsync )
			{
				// Wrap with return to return Task
				packToArray = this.EmitRetrunStatement( context, packToArray );
			}

			yield return packToArray;
		}

		private IEnumerable<TConstruct> EmitPackTupleItemStatements(
			TContext context,
			Type itemType,
			TConstruct currentPacker,
			TConstruct tuple,
			IEnumerable<PropertyInfo> propertyInvocationChain,
			PolymorphismSchema itemsSchema,
			bool isAsync
		)
		{
			return
				this.EmitPackItemStatements(
					context,
					currentPacker,
					itemType,
					NilImplication.Null,
					null,
					propertyInvocationChain.Aggregate(
						tuple, ( propertySource, property ) => this.EmitGetPropertyExpression( context, propertySource, property )
					),
					null,
					itemsSchema,
					isAsync
				);
		}

		#endregion -- PackTo --

		#region -- UnpackFrom --

		private void BuildTupleUnpackFrom( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList, bool isAsync )
		{
			/*
			 * 	checked
			 * 	{
			 * 		if (!unpacker.IsArrayHeader)
			 * 		{
			 * 			throw SerializationExceptions.NewIsNotArrayHeader();
			 * 		}
			 * 		
			 * 		if ((int)unpacker.ItemsCount != n)
			 * 		{
			 * 			throw SerializationExceptions.NewTupleCardinarityIsNotMatch(n, (int)unpacker.ItemsCount);
			 * 		}
			 * 		
			 *		return 
			 *			new Tuple<...>( 
			 *				GET_VALUE_OR_DEFAULT( DESERIALIZE_VALUE( unpacker, typeof( T1? ) ) ),
			 *					:
			 *				GET_VALUE_OR_DEFAULT( DESERIALIZE_VALUE( unpacker, typeof( T7? ) ) ),
			 *				new Tuple<...>(
			 *					GET_VALUE_OR_DEFAULT( DESERIALIZE_VALUE( unpacker, typeof( T8? ) ) ),
			 *						:
			 *				)
			 *			);
			 *	}
			 */
			var methodName = 
#if FEATURE_TAP
				isAsync ? MethodName.UnpackFromAsyncCore : 
#endif // FEATURE_TAP
				MethodName.UnpackFromCore;
			context.BeginMethodOverride( methodName );
			context.EndMethodOverride(
				methodName,
				this.EmitSequentialStatements(
					context,
					this.TargetType,
					this.BuildTupleUnpackFromCore( context, itemTypes, itemSchemaList, isAsync )
				)
			);
		}

		private IEnumerable<TConstruct> BuildTupleUnpackFromCore( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList, bool isAsync )
		{
			var tupleTypeList = TupleItems.CreateTupleTypeList( itemTypes );
			yield return
				this.EmitCheckIsArrayHeaderExpression( context, context.Unpacker );

			yield return
				this.EmitCheckTupleCardinarityExpression(
					context,
					context.Unpacker,
					itemTypes.Count
				);

			var unpackingContext = this.GetTupleUnpackingContextInfo( context, itemTypes );

			foreach ( var statement in unpackingContext.Statements )
			{
				yield return statement;
			}

			var unpackValueArguments =
				new[] { context.Unpacker, context.UnpackingContextInUnpackValueMethods, context.IndexOfItem, context.ItemsCount }
#if FEATURE_TAP
				.Concat( isAsync ? new[] { this.ReferCancellationToken( context, 2 ) } : NoConstructs ).ToArray()
#endif // FEATURE_TAP
				;
			for ( var i = 0; i < itemTypes.Count; i++ )
			{
				var propertyName = SerializationTarget.GetTupleItemNameFromIndex( i );
				var unpackedItem = context.DefineUnpackedItemParameterInSetValueMethods( itemTypes[ i ] );
				var setUnpackValueOfMethodName = MethodNamePrefix.SetUnpackedValueOf + propertyName;

				var index = i;
				this.ExtractPrivateMethod(
					context,
					AdjustName( MethodNamePrefix.UnpackValue + propertyName, isAsync ),
#if FEATURE_TAP
					isAsync ? typeof( Task ) :
#endif // FEATURE_TAP
					typeof( void ),
					() => this.EmitUnpackItemValueStatement(
						context,
						itemTypes[ index ],
						this.MakeStringLiteral( context, propertyName ),
						context.TupleItemNilImplication,
						null, // memberInfo
						itemSchemaList.Count == 0 ? null : itemSchemaList[ index ],
						context.Unpacker,
						context.UnpackingContextInUnpackValueMethods,
						context.IndexOfItem,
						context.ItemsCount,
						context.IsDeclaredMethod( setUnpackValueOfMethodName )
							? this.EmitGetPrivateMethodDelegateExpression( 
								context, 
								context.GetDeclaredMethod( setUnpackValueOfMethodName )
							)
							: this.ExtractPrivateMethod(
								context,
								setUnpackValueOfMethodName,
								typeof( void ),
								() => unpackingContext.VariableType.TryGetRuntimeType() == typeof( DynamicUnpackingContext )
									? this.EmitInvokeVoidMethod(
										context,
										context.UnpackingContextInSetValueMethods,
										Metadata._DynamicUnpackingContext.Set,
										this.MakeStringLiteral( context, propertyName ),
										this.EmitBoxExpression( 
											context,
											itemTypes[ index ],
											unpackedItem
										)
									) : this.EmitSetField(
										context,
										context.UnpackingContextInSetValueMethods,
										unpackingContext.VariableType,
										propertyName,
										unpackedItem
									),
								context.UnpackingContextInSetValueMethods,
								unpackedItem
							),
						isAsync
					),
					unpackValueArguments
				);
			}

			TConstruct currentTuple = null;
			for ( int nest = tupleTypeList.Count - 1; nest >= 0; nest-- )
			{
				var gets =
					Enumerable.Range( nest * 7, Math.Min( itemTypes.Count - nest * 7, 7 ) )
						.Select( i =>
							unpackingContext.VariableType.TryGetRuntimeType() == typeof( DynamicUnpackingContext )
								? this.EmitUnboxAnyExpression(
									context,
									itemTypes[ i ],
									this.EmitInvokeMethodExpression(
										context,
										context.UnpackingContextInCreateObjectFromContext,
										Metadata._DynamicUnpackingContext.Get,
										this.MakeStringLiteral( context, SerializationTarget.GetTupleItemNameFromIndex( i ) )
									)
								) : this.EmitGetFieldExpression(
									context,
									context.UnpackingContextInCreateObjectFromContext,
									new FieldDefinition( 
										unpackingContext.VariableType,
										SerializationTarget.GetTupleItemNameFromIndex( i ),
										itemTypes[ i ]
									)
								)
						);
				if ( currentTuple != null )
				{
					gets = gets.Concat( new[] { currentTuple } );
				}

				currentTuple =
					this.EmitCreateNewObjectExpression(
						context,
						null, // Tuple is reference contextType.
						tupleTypeList[ nest ].GetConstructors().Single(),
						gets.ToArray()
					);
			}

#if DEBUG
			Contract.Assert( currentTuple != null );
#endif
			unpackingContext.Factory =
				this.EmitNewPrivateMethodDelegateExpressionWithCreation(
					context,
					new MethodDefinition(
						MethodName.CreateObjectFromContext,
						null,
						null,
						this.TargetType,
						unpackingContext.Type
					),
					() => this.EmitRetrunStatement(
						context,
						currentTuple
					),
					context.UnpackingContextInCreateObjectFromContext
				);


			var unpackHelperArguments =
				new[]
				{
					context.Unpacker,
					unpackingContext.Variable,
					unpackingContext.Factory,
					this.EmitGetMemberNamesExpression( context ),
					this.EmitGetActionsExpression( context, ActionType.UnpackFromArray, isAsync )
				}
#if FEATURE_TAP
				.Concat( isAsync ? new[] { this.ReferCancellationToken( context, 2 ) } : NoConstructs ).ToArray()
#endif // FEATURE_TAP
				;

			yield return
				this.EmitRetrunStatement(
					context,
					this.EmitInvokeMethodExpression(
						context,
						null,
						new MethodDefinition(
							AdjustName( MethodName.UnpackFromArray, isAsync ),
							new [] { unpackingContext.Type, this.TargetType },
							typeof( UnpackHelpers ),
#if FEATURE_TAP
							isAsync ? typeof( Task<> ).MakeGenericType( this.TargetType ) :
#endif // FEATURE_TAP
							this.TargetType,
							unpackHelperArguments.Select( a => a.ContextType ).ToArray()
						),
						unpackHelperArguments
					)
				);
		}

		private UnpackingContextInfo GetTupleUnpackingContextInfo( TContext context, IList<Type> itemTypes )
		{
			TypeDefinition type;
			ConstructorDefinition constructor;
			context.DefineUnpackingContext(
				itemTypes.Select( ( t, i ) =>
					new KeyValuePair<string, TypeDefinition>( SerializationTarget.GetTupleItemNameFromIndex( i ), t )
				).ToArray(),
				out type,
				out constructor
			);

			var unpackingContext = UnpackingContextInfo.Create( type, constructor, new HashSet<string>() );

			unpackingContext.Variable = this.DeclareLocal( context, unpackingContext.VariableType, "unpackingContext" );
			unpackingContext.Statements.Add( unpackingContext.Variable );
			unpackingContext.Statements.Add(
				this.EmitStoreVariableStatement(
					context,
					unpackingContext.Variable,
					this.EmitCreateNewObjectExpression(
						context,
						unpackingContext.Variable,
						unpackingContext.Constructor,
						unpackingContext.VariableType.TryGetRuntimeType() == typeof( DynamicUnpackingContext )
							? new[] { this.MakeInt32Literal( context, itemTypes.Count ) }
							: itemTypes.Select( t => this.MakeDefaultLiteral( context, t ) ).ToArray()
					)
				)
			);

			return unpackingContext;
		}

		private TConstruct EmitCheckTupleCardinarityExpression( TContext context, TConstruct unpacker, int cardinarity )
		{
			return
				this.EmitConditionalExpression(
					context,
					this.EmitNotEqualsExpression(
						context,
						this.EmitGetPropertyExpression( context, unpacker, Metadata._Unpacker.ItemsCount ),
						this.MakeInt64Literal( context, cardinarity ) // as i8 for compile time optimization
					),
					this.EmitInvokeVoidMethod(
						context,
						null,
						SerializationExceptions.ThrowTupleCardinarityIsNotMatchMethod,
						this.MakeInt32Literal( context, cardinarity ), // as i4 for valid API call
						this.EmitGetPropertyExpression( context, unpacker, Metadata._Unpacker.ItemsCount ),
						context.Unpacker
					),
					null
				);
		}

		#endregion -- UnpackFrom --
	}
}
