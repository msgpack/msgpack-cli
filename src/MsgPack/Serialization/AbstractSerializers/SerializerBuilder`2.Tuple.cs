#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
#if CORE_CLR || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || NETSTANDARD1_1
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
			var isValueTuple = this.TargetType.GetIsValueType();

			this.BuildTuplePackTo( context, itemTypes, itemSchemaList, isValueTuple, false );

#if FEATURE_TAP
			if ( this.WithAsync( context ) )
			{
				this.BuildTuplePackTo( context, itemTypes, itemSchemaList, isValueTuple, true );
			}
#endif // FEATURE_TAP

			this.BuildTupleUnpackFrom( context, itemTypes, itemSchemaList, isValueTuple, false );

#if FEATURE_TAP
			if ( this.WithAsync( context ) )
			{
				this.BuildTupleUnpackFrom( context, itemTypes, itemSchemaList, isValueTuple, true );
			}
#endif // FEATURE_TAP
		}

		#region -- PackTo --

		private void BuildTuplePackTo( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList, bool isValueTuple, bool isAsync )
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
					TypeDefinition.VoidType,
					this.BuildTuplePackToCore( context, itemTypes, itemSchemaList, isValueTuple, isAsync )
				)
			);
		}

		private IEnumerable<TConstruct> BuildTuplePackToCore( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList, bool isValueTuple, bool isAsync )
		{
			return
				isValueTuple
					? BuildTuplePackToCore( context, itemTypes, itemSchemaList, ( t, n ) => t.GetField( n ), ( c, s, m ) => this.EmitGetFieldExpression( c, s, m ), isAsync )
					: BuildTuplePackToCore( context, itemTypes, itemSchemaList, ( t, n ) => t.GetProperty( n ), ( c, s, m )=> this.EmitGetPropertyExpression( c, s, m ), isAsync );
		}

		private IEnumerable<TConstruct> BuildTuplePackToCore<TInfo>( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList, Func<Type, string, TInfo> memberFactory, Func<TContext, TConstruct, TInfo, TConstruct> chainConstructFactory, bool isAsync )
		{
			// Note: cardinality is put as array length by PackHelper.
			var depth = -1;
			var tupleTypeList = TupleItems.CreateTupleTypeList( this.TargetType );
			var memberInvocationChain = new List<TInfo>( itemTypes.Count % 7 + 1 );
			var packValueArguments =
				new[] { context.Packer, context.PackToTarget }
#if FEATURE_TAP
				.Concat( isAsync ? new[] { this.ReferCancellationToken( context, 3 ) } : NoConstructs ).ToArray()
#endif // FEATURE_TAP
				;

			for ( var i = 0; i < itemTypes.Count; i++ )
			{
				if ( i % 7 == 0 )
				{
					depth++;
				}

				for ( var j = 0; j < depth; j++ )
				{
					// .TRest.TRest ...
					var restMember = memberFactory( tupleTypeList[ j ], "Rest" );
#if DEBUG
					Contract.Assert( restMember != null, tupleTypeList[ j ].GetFullName() + ".Rest is not defined" );
#endif
					memberInvocationChain.Add( restMember );
				}

				var itemNMember = memberFactory( tupleTypeList[ depth ], "Item" + ( ( i % 7 ) + 1 ) );
				memberInvocationChain.Add( itemNMember );
#if DEBUG
				Contract.Assert(
					itemNMember != null,
					tupleTypeList[ depth ].GetFullName() + "::Item" + ( ( i % 7 ) + 1 ) + " [ " + depth + " ] @ " + i
				);
#endif
				var count = i;
				DefinePrivateMethod(
					context,
					AdjustName( MethodNamePrefix.PackValue + SerializationTarget.GetTupleItemNameFromIndex( i ), isAsync ),
					false, // isStatic
#if FEATURE_TAP
					isAsync ? TypeDefinition.TaskType :
#endif // FEATURE_TAP
					TypeDefinition.VoidType,
					() => this.EmitSequentialStatements(
						context,
						TypeDefinition.VoidType,
						this.EmitPackTupleItemStatements(
							context,
							itemTypes[ count ],
							context.Packer,
							context.PackToTarget,
							memberInvocationChain,
							itemSchemaList.Count == 0 ? null : itemSchemaList[ count ],
							chainConstructFactory,
							isAsync
						)
					),
					packValueArguments
				);

				memberInvocationChain.Clear();
			}

			var packHelperArguments =
					new Dictionary<string, TConstruct>
					{
						{ "Packer", context.Packer },
						{ "Target", context.PackToTarget },
						{ "Operations", this.EmitGetActionsExpression( context, ActionType.PackToArray, isAsync ) }
					};

#if FEATURE_TAP
			if ( isAsync )
			{
				packHelperArguments.Add( "CancellationToken", this.ReferCancellationToken( context, 3 ) );
			}
#endif // FEATURE_TAP

			var packHelperParameterTypeDefinition =
#if FEATURE_TAP
				isAsync ? typeof( PackToArrayAsyncParameters<> ) :
#endif // FEATURE_TAP
				typeof( PackToArrayParameters<> );

			var packHelperParameterType =
					TypeDefinition.GenericValueType( packHelperParameterTypeDefinition, this.TargetType );
			var packHelperMethod =
					new MethodDefinition(
						AdjustName( MethodName.PackToArray, isAsync ),
						new [] { TypeDefinition.Object( this.TargetType ) },
						TypeDefinition.PackHelpersType,
						true, // isStatic
#if FEATURE_TAP
						isAsync ? TypeDefinition.TaskType :
#endif // FEATURE_TAP
						TypeDefinition.VoidType,
						packHelperParameterType
					);

			var packHelperParameters = this.DeclareLocal( context, packHelperParameterType, "packHelperParameters" );
			yield return packHelperParameters;
			foreach ( var construct in this.CreatePackUnpackHelperArgumentInitialization( context, packHelperParameters, packHelperArguments ) )
			{
				yield return construct;
			}

			var methodInvocation =
					this.EmitInvokeMethodExpression(
						context,
						null,
						packHelperMethod,
						this.EmitMakeRef( context, packHelperParameters )
					);

			if ( isAsync )
			{
				// Wrap with return to return Task
				methodInvocation = this.EmitRetrunStatement( context, methodInvocation );
			}

			yield return methodInvocation;
		}

		private IEnumerable<TConstruct> EmitPackTupleItemStatements<TInfo>(
			TContext context,
			Type itemType,
			TConstruct currentPacker,
			TConstruct tuple,
			IEnumerable<TInfo> memberInvocationChain,
			PolymorphismSchema itemsSchema,
			Func<TContext, TConstruct, TInfo, TConstruct> chainConstructFactory,
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
					memberInvocationChain.Aggregate(
						tuple, ( memberSource, member ) => chainConstructFactory( context, memberSource, member )
					),
					null,
					itemsSchema,
					isAsync
				);
		}

		#endregion -- PackTo --

		#region -- UnpackFrom --

		private void BuildTupleUnpackFrom( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList, bool isValueTuple, bool isAsync )
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
					this.BuildTupleUnpackFromCore( context, itemTypes, itemSchemaList, isValueTuple, isAsync )
				)
			);
		}

		private IEnumerable<TConstruct> BuildTupleUnpackFromCore( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList, bool isValueTuple, bool isAsync )
		{
			return
				isValueTuple
					? BuildTupleUnpackFromCore( context, itemTypes, itemSchemaList, ( t, n ) => t.GetField( n ), isAsync )
					: BuildTupleUnpackFromCore( context, itemTypes, itemSchemaList, ( t, n ) => t.GetProperty( n ), isAsync );
		}

		private IEnumerable<TConstruct> BuildTupleUnpackFromCore<TInfo>( TContext context, IList<Type> itemTypes, IList<PolymorphismSchema> itemSchemaList, Func<Type, string, TInfo> memberFactory, bool isAsync )
		{
			var tupleTypeList = TupleItems.CreateTupleTypeList( this.TargetType );
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
				var memberName = SerializationTarget.GetTupleItemNameFromIndex( i );
				var unpackedItem = context.DefineUnpackedItemParameterInSetValueMethods( itemTypes[ i ] );
				var setUnpackValueOfMethodName = MethodNamePrefix.SetUnpackedValueOf + memberName;

				var index = i;
				this.ExtractPrivateMethod(
					context,
					AdjustName( MethodNamePrefix.UnpackValue + memberName, isAsync ),
					false, // isStatic
#if FEATURE_TAP
					isAsync ? TypeDefinition.TaskType :
#endif // FEATURE_TAP
					TypeDefinition.VoidType,
					() => this.EmitUnpackItemValueStatement(
						context,
						itemTypes[ index ],
						this.MakeStringLiteral( context, memberName ),
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
								false, // isStatic
								TypeDefinition.VoidType,
								() =>
									this.EmitSetField(
										context,
										context.UnpackingContextInSetValueMethods,
										unpackingContext.VariableType,
										memberName,
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
			for ( var nest = tupleTypeList.Count - 1; nest >= 0; nest-- )
			{
				var gets =
					Enumerable.Range( nest * 7, Math.Min( itemTypes.Count - nest * 7, 7 ) )
						.Select( i =>
							this.EmitGetFieldExpression(
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

				var constructor = tupleTypeList[ nest ].GetConstructors().SingleOrDefault();
				if ( constructor == null )
				{
					// arity 0 value tuple
#if DEBUG
					Contract.Assert( tupleTypeList[ nest ].GetFullName() == "System.ValueTuple", tupleTypeList[ nest ].GetFullName() + " == System.ValueTuple");
#endif
					currentTuple = this.MakeDefaultLiteral( context, tupleTypeList[ nest ] );
				}
				else
				{
					var tempVariable = default( TConstruct );

					if ( tupleTypeList[ nest ].GetIsValueType() )
					{
						// Temp var is required for value type (that is, ValueTuple)
						tempVariable = this.DeclareLocal( context, tupleTypeList[ nest ], context.GetUniqueVariableName( "tuple" ) );
					}

					currentTuple =
						this.EmitCreateNewObjectExpression(
							context,
							tempVariable, // Tuple is reference contextType.
							constructor,
							gets.ToArray()
						);
				}
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
						true, // isStatic
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
							TypeDefinition.UnpackHelpersType,
							true, // isStatic
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
