#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private void BuildTupleSerializer( TContext context, IList<PolymorphismSchema> itemSchemaList, out SerializationTarget targetInfo )
		{
			var itemTypes = TupleItems.GetTupleItemTypes( typeof( TObject ) );
			targetInfo = SerializationTarget.CreateForTuple( itemTypes );

			this.BuildTuplePackTo( context, itemTypes, itemSchemaList );
			this.BuildTupleUnpackFrom( context, itemTypes, itemSchemaList );
		}

		#region -- PackTo --

		private void BuildTuplePackTo( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList )
		{
			/*
				 packer.PackArrayHeader( cardinarity );
				 _serializer0.PackTo( packer, tuple.Item1 );
					:
				 _serializer6.PackTo( packer, tuple.item7 );
				 _serializer7.PackTo( packer, tuple.Rest.Item1 );
			*/

			context.BeginMethodOverride( MethodName.PackToCore );
			context.EndMethodOverride(
				MethodName.PackToCore,
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					this.BuildTuplePackToCore( context, itemTypes, itemSchemaList )
				)
			);
		}

		private IEnumerable<TConstruct> BuildTuplePackToCore( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList )
		{
			// Note: cardinality is put as array length by PackHelper.
			var depth = -1;
			var tupleTypeList = TupleItems.CreateTupleTypeList( itemTypes );
			var propertyInvocationChain = new List<PropertyInfo>( itemTypes.Count % 7 + 1 );
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
					tupleTypeList[ depth ].GetFullName() + "::Item" + ( ( i % 7 ) + 1 ) + " [ " + depth + " ] @ " + i );
#endif
				var count = i;
				this.EmitPrivateMethod(
					context,
					MethodNamePrefix.PackValue + SerializationTarget.GetTupleItemNameFromIndex( i ),
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
							itemSchemaList.Count == 0 ? null : itemSchemaList[ count ]
						)
					),
					context.Packer,
					context.PackToTarget
				);

				propertyInvocationChain.Clear();
			}

			var packHelperArguments =
				new[]
				{
					context.Packer,
					context.PackToTarget,
					this.EmitGetActionsExpression( context, ActionType.PackToArray )
				};

			yield return
				this.EmitInvokeMethodExpression(
					context,
					null,
					new MethodDefinition(
						MethodName.PackToArray,
						new [] { TypeDefinition.Object( typeof( TObject ) ) },
						typeof( PackHelpers ),
						typeof( void ),
						packHelperArguments.Select( a => a.ContextType ).ToArray()
					),
					packHelperArguments
				);
		}

		private IEnumerable<TConstruct> EmitPackTupleItemStatements(
			TContext context,
			Type itemType,
			TConstruct currentPacker,
			TConstruct tuple,
			IEnumerable<PropertyInfo> propertyInvocationChain,
			PolymorphismSchema itemsSchema
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
					itemsSchema
				);
		}

		#endregion -- PackTo --

		#region -- UnpackFrom --

		private void BuildTupleUnpackFrom( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList )
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
			context.BeginMethodOverride( MethodName.UnpackFromCore );
			context.EndMethodOverride(
				MethodName.UnpackFromCore,
				this.EmitSequentialStatements(
					context,
					typeof( TObject ),
					this.BuildTupleUnpackFromCore( context, itemTypes, itemSchemaList )
				)
			);
		}

		private IEnumerable<TConstruct> BuildTupleUnpackFromCore( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList )
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

			for ( var i = 0; i < itemTypes.Count; i++ )
			{
				var propertyName = SerializationTarget.GetTupleItemNameFromIndex( i );
				var index = i;
				this.EmitPrivateMethod(
					context,
					MethodNamePrefix.UnpackValue + propertyName,
					typeof( void ),
					() => this.EmitUnpackItemValueExpression(
						context,
						itemTypes[ index ],
						context.TupleItemNilImplication,
						context.Unpacker,
						this.MakeInt32Literal( context, index ),
						this.MakeStringLiteral( context, propertyName ),
						null,
						itemSchemaList.Count == 0 ? null : itemSchemaList[ index ],
						unpackedItem =>
							unpackingContext.VariableType.TryGetRuntimeType() == typeof( DynamicUnpackingContext )
								? this.EmitInvokeVoidMethod(
									context,
									context.UnpackingContextInUnpackValueMethods,
									Metadata._DynamicUnpackingContext.Set,
									this.MakeStringLiteral( context, propertyName ),
									this.EmitBoxExpression( 
										context,
										itemTypes[ index ],
										unpackedItem
									)
								) : this.EmitSetField(
									context,
									context.UnpackingContextInUnpackValueMethods,
									unpackingContext.VariableType,
									propertyName,
									unpackedItem
								)
					),
					context.Unpacker,
					context.UnpackingContextInUnpackValueMethods,
					context.IndexOfItem
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
						typeof( TObject ),
						unpackingContext.Type
					),
					() => 
						this.EmitRetrunStatement(
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
					this.EmitGetActionsExpression( context, ActionType.UnpackFromArray )
				};

			yield return
				this.EmitRetrunStatement(
					context,
					this.EmitInvokeMethodExpression(
						context,
						null,
						new MethodDefinition(
							MethodName.UnpackFromArray,
							new [] { unpackingContext.Type, typeof( TObject ) },
							typeof( UnpackHelpers ),
							typeof( TObject ),
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
				unpackingContext.VariableType.TryGetRuntimeType() == typeof( DynamicUnpackingContext )
					? this.EmitStoreVariableStatement(
						context,
						unpackingContext.Variable,
						this.EmitCreateNewObjectExpression(
							context,
							unpackingContext.Variable,
							unpackingContext.Constructor,
							this.MakeInt32Literal( context, itemTypes.Count )
						)
					)
					: this.EmitStoreVariableStatement(
						context,
						unpackingContext.Variable,
						this.EmitCreateNewObjectExpression(
							context,
							unpackingContext.Variable,
							unpackingContext.Constructor,
							itemTypes.Select( t => this.MakeDefaultLiteral( context, t ) ).ToArray()
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
