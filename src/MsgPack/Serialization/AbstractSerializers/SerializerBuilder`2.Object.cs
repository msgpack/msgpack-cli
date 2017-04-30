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
#if CORE_CLR || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || NETSTANDARD1_1
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP


namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct>
	{
		private SerializationTarget BuildObjectSerializer( TContext context )
		{
			SerializationTarget.VerifyType( this.TargetType );
			var targetInfo = SerializationTarget.Prepare( context.SerializationContext, this.TargetType );

			if ( typeof( IPackable ).IsAssignableFrom( this.TargetType ) )
			{
				this.BuildIPackablePackTo( context );
			}
			else
			{
				this.BuildObjectPackTo( context, targetInfo, false );
			}

#if FEATURE_TAP

			if ( this.WithAsync( context ) )
			{
				if ( typeof( IAsyncPackable ).IsAssignableFrom( this.TargetType ) )
				{
					this.BuildIAsyncPackablePackTo( context );
				}
				else
				{
					this.BuildObjectPackTo( context, targetInfo, true );
				}
			}

#endif // FEATURE_TAP

			if ( typeof( IUnpackable ).IsAssignableFrom( this.TargetType ) )
			{
				this.BuildIUnpackableUnpackFrom( context, this.GetUnpackableObjectInstantiation( context ), targetInfo.CanDeserialize );
			}
			else
			{
				this.BuildObjectUnpackFrom( context, targetInfo, false );
			}

#if FEATURE_TAP

			if ( this.WithAsync( context ) )
			{
				if ( typeof( IAsyncUnpackable ).IsAssignableFrom( this.TargetType ) )
				{
					this.BuildIAsyncUnpackableUnpackFrom( context, this.GetUnpackableObjectInstantiation( context ), targetInfo.CanDeserialize );
				}
				else
				{
					this.BuildObjectUnpackFrom( context, targetInfo, true );
				}
			}

#endif // FEATURE_TAP

			return targetInfo;
		}

		#region -- IPackable --

		private void BuildIPackablePackTo( TContext context )
		{
			context.BeginMethodOverride( MethodName.PackToCore );
			context.EndMethodOverride(
				MethodName.PackToCore,
				this.BuildIPackablePackToCore( context, typeof( IPackable ) )
			);
		}

		#endregion -- IPackable --

#if FEATURE_TAP

		#region -- IAsyncPackable --

		private void BuildIAsyncPackablePackTo( TContext context )
		{
			context.BeginMethodOverride( MethodName.PackToAsyncCore );
			context.EndMethodOverride(
				MethodName.PackToAsyncCore,
				this.BuildIPackablePackToCore( context, typeof( IAsyncPackable ) )
			);
		}

		#endregion -- IAsyncPackable --

#endif // FEATURE_TAP

		private TConstruct BuildIPackablePackToCore( TContext context, Type @interface )
		{
			var packTo = this.TargetType.GetInterfaceMap( @interface ).TargetMethods.Single();

			if ( packTo.ReturnType == typeof( void ) )
			{
				return
					this.EmitInvokeVoidMethod(
						context,
						context.PackToTarget,
						new MethodDefinition( packTo, @interface ),
						context.Packer,
						this.MakeNullLiteral( context, TypeDefinition.PackingOptionsType )
					);
			}
			else
			{
#if FEATURE_TAP
				Contract.Assert( context.SerializationContext.SerializerOptions.WithAsync );
				return
					this.EmitRetrunStatement(
						context,
						this.EmitInvokeMethodExpression(
							context,
							context.PackToTarget,
							new MethodDefinition( packTo, @interface ),
							context.Packer,
							this.MakeNullLiteral( context, TypeDefinition.PackingOptionsType ),
							this.ReferCancellationToken( context, 3 )
						)
					);
#else
				ThrowAsyncNotSupportedException();
				return default ( TConstruct ); // never reaches
#endif // FEATURE_TAP
			}
		}

		#region -- PackTo --

		private void BuildObjectPackTo( TContext context, SerializationTarget targetInfo, bool isAsync )
		{
			if ( targetInfo.Members.Count == 0 )
			{
				throw new SerializationException(
					String.Format(
						CultureInfo.CurrentCulture,
						isAsync
						? "At least one serializable member is required because type '{0}' does not implement IAsyncPackable interface."
						: "At least one serializable member is required because type '{0}' does not implement IPackable interface.",
						this.TargetType
					)
				);
			}

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
					this.BuildObjectPackToCore( context, targetInfo.Members, isAsync )
				)
			);
		}

		private IEnumerable<TConstruct> BuildObjectPackToCore( TContext context, IList<SerializingMember> entries, bool isAsync )
		{
			var parameters =
#if FEATURE_TAP
				isAsync ? new[] { context.Packer, context.PackToTarget, this.ReferCancellationToken( context, 3 ) } :
#endif // FEATURE_TAP
				new[] { context.Packer, context.PackToTarget };
			var argumentsForNull =
#if FEATURE_TAP
				isAsync ? new[] { this.ReferCancellationToken( context, 3 ) } :
#endif // FEATURE_TAP
				NoConstructs;
			var nullCheckParameters = new[] { context.NullCheckTarget };
			var methodForNull =
#if FEATURE_TAP
				isAsync ? Metadata._Packer.PackNullAsync :
#endif // FEATURE_TAP
				Metadata._Packer.PackNull;

			TConstruct forArray = null;
			TConstruct forMap = null;

			foreach ( var method in new[] { SerializationMethod.Array, SerializationMethod.Map } )
			{
				for ( int i = 0; i < entries.Count; i++ )
				{
					var count = i;
					if ( entries[ i ].Member == null )
					{
						if ( method == SerializationMethod.Map )
						{
							// skip
							continue;
						}

						// missing member, always nil
						DefinePrivateMethod(
							context,
							AdjustName( MethodName.PackMemberPlaceHolder, isAsync ),
							false, // isStatic
#if FEATURE_TAP
							isAsync ? TypeDefinition.TaskType :
#endif // FEATURE_TAP
							TypeDefinition.VoidType,
							() => isAsync
								? this.EmitRetrunStatement( context, this.EmitInvokeMethodExpression( context, context.Packer, methodForNull, argumentsForNull ) )
								: this.EmitInvokeVoidMethod( context, context.Packer, methodForNull, argumentsForNull ),
							parameters
						);
					}
					else
					{
						var itemType = entries[ count ].Member.GetMemberValueType();
						// PackValue
						DefinePrivateMethod(
							context,
							GetPackValueMethodName( entries[ i ], isAsync ),
							false, // isStatic
#if FEATURE_TAP
							isAsync ? TypeDefinition.TaskType :
#endif // FEATURE_TAP
							TypeDefinition.VoidType,
							() => this.EmitSequentialStatements(
								context,
								TypeDefinition.VoidType,
								this.EmitPackItemStatements(
									context,
									context.Packer,
									itemType,
									entries[ count ].Contract.NilImplication,
									entries[ count ].Member.ToString(),
									this.EmitGetMemberValueExpression( context, context.PackToTarget, entries[ count ].Member ),
									entries[ count ],
									null,
									isAsync
								)
							),
							parameters
						);

						if ( ( !itemType.GetIsValueType() || Nullable.GetUnderlyingType( itemType ) != null )
#if DEBUG
							&& !SerializerDebugging.UseLegacyNullMapEntryHandling
#endif // DEBUG
						)
						{
							var nullCheckTarget = this.EmitGetMemberValueExpression( context, context.NullCheckTarget, entries[ count ].Member );
							// Trying uses nullCheckTarget.ContextType because it may be Object when using reflection.
							var nullCheckTargetType = nullCheckTarget.ContextType.TryGetRuntimeType() ?? itemType;
							// CheckNull
							DefinePrivateMethod(
								context,
								GetCheckNullMethodName( entries[ i ] ),
								false, // isStatic
								TypeDefinition.BooleanType,
								() =>
									nullCheckTargetType.GetIsValueType() // Is Nullable<T> ?
									? this.EmitSequentialStatements(
										context,
										TypeDefinition.BooleanType,
										this.EmitHasValueCore( context, nullCheckTarget, nullCheckTargetType )
									) : this.EmitRetrunStatement(
										context,
										this.EmitEqualsExpression( context, nullCheckTarget, this.MakeNullLiteral( context, nullCheckTargetType ) )
									),
								nullCheckParameters
							);
						}
					}
				}

				var packHelperArguments =
					new Dictionary<string, TConstruct>
					{
						{ "Packer", context.Packer },
						{ "Target", context.PackToTarget },
						{
							"Operations",
							this.EmitGetActionsExpression(
								context,
								method == SerializationMethod.Array
									? ActionType.PackToArray
									: ActionType.PackToMap,
								isAsync
							)
						}
					};

				if ( method == SerializationMethod.Map
#if DEBUG
					&& !SerializerDebugging.UseLegacyNullMapEntryHandling
#endif // DEBUG
				)
				{
					packHelperArguments.Add(
						"SerializationContext",
						this.EmitGetPropertyExpression( context, this.EmitThisReferenceExpression( context ), Metadata._MessagePackSerializer.OwnerContext )
					);
					// isAsync is always false to prevent _nullCheckersTableAsync creation.
					packHelperArguments.Add( "NullCheckers", this.EmitGetActionsExpression( context, ActionType.IsNull, isAsync: false ) );
				}

#if FEATURE_TAP
				if ( isAsync )
				{
					packHelperArguments.Add( "CancellationToken", this.ReferCancellationToken( context, 3 ) );
				}
#endif // FEATURE_TAP

				var packHelperParameterTypeDefinition =
					( method == SerializationMethod.Array
						? (
#if FEATURE_TAP
							isAsync ? typeof( PackToArrayAsyncParameters<> ) :
#endif // FEATURE_TAP
							typeof( PackToArrayParameters<> )
						) : (
#if FEATURE_TAP
							isAsync ? typeof( PackToMapAsyncParameters<> ) :
#endif // FEATURE_TAP
							typeof( PackToMapParameters<> )
						)
					);

				var packHelperMethodName = AdjustName( "PackTo" + method, isAsync );
				var packHelperParameterType =
					TypeDefinition.GenericValueType( packHelperParameterTypeDefinition, this.TargetType );
				var packHelperMethod =
					new MethodDefinition(
						packHelperMethodName,
						new TypeDefinition[] { this.TargetType },
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
					// Wrap with return to return Task.
					methodInvocation = this.EmitRetrunStatement( context, methodInvocation );
				}

				if ( method == SerializationMethod.Array )
				{
					forArray = methodInvocation;
				}
				else
				{
					forMap = methodInvocation;
				}
			} // foreach (method)

			yield return
				this.EmitConditionalExpression(
					context,
					this.EmitEqualsExpression(
						context,
						this.EmitGetPropertyExpression(
							context,
							this.EmitGetPropertyExpression(
								context,
								this.EmitThisReferenceExpression( context ),
								Metadata._MessagePackSerializer.OwnerContext
							),
							Metadata._SerializationContext.SerializationMethod
						),
						this.MakeEnumLiteral( context, TypeDefinition.SerializationMethodType, SerializationMethod.Array )
					),
					forArray,
					forMap
				);
		}

		private IEnumerable<TConstruct> EmitHasValueCore( TContext context, TConstruct nullCheckTarget, Type itemType )
		{
			var nullable = this.DeclareLocal( context, itemType, "nullable" );
			yield return nullable;
			yield return this.EmitStoreVariableStatement( context, nullable, nullCheckTarget );
			yield return
				this.EmitRetrunStatement(
					context,
					this.EmitEqualsExpression(
						context,
						this.EmitGetPropertyExpression( context, nullable, itemType.GetProperty( "HasValue" ) ),
						this.MakeBooleanLiteral( context, false )
					)
				);
		}

#endregion -- IPackable --

#region -- Pack Operation Initialization --

		protected internal TConstruct EmitPackOperationListInitialization( TContext context, SerializationTarget targetInfo, bool isAsync )
		{
			var actionType = this.GetPackOperationType( context, isAsync );
			var listType = TypeDefinition.Array( actionType );
			return
				this.EmitSequentialStatements(
					context,
					listType,
					this.EmitPackActionCollectionCore(
						context,
						targetInfo,
						actionType,
						this.DeclareLocal( context, listType, AdjustName( "packOperationList", isAsync ) ),
						SerializationMethod.Array,
						isAsync
					)
				);
		}

		protected internal TConstruct EmitPackOperationTableInitialization( TContext context, SerializationTarget targetInfo, bool isAsync )
		{
			var actionType = this.GetPackOperationType( context, isAsync );
			var listType = TypeDefinition.GenericReferenceType( typeof( Dictionary<,> ), TypeDefinition.StringType, actionType );
			return
				this.EmitSequentialStatements(
					context,
					listType,
					this.EmitPackActionCollectionCore(
						context,
						targetInfo,
						actionType,
						this.DeclareLocal( context, listType, AdjustName( "packOperationTable", isAsync ) ),
						SerializationMethod.Map,
						isAsync
					)
				);
		}

		protected virtual TypeDefinition GetPackOperationType( TContext context, bool isAsync )
		{
			return
#if FEATURE_TAP
				isAsync
					? typeof( Func<,,,> ).MakeGenericType( typeof( Packer ), this.TargetType, typeof( CancellationToken ), typeof( Task ) ) :
#endif // FEATURE_TAP
					typeof( Action<,> ).MakeGenericType( typeof( Packer ), this.TargetType );
		}

		private IEnumerable<TConstruct> EmitPackActionCollectionCore( TContext context, SerializationTarget targetInfo, TypeDefinition actionType, TConstruct actionCollection, SerializationMethod method, bool isAsync )
		{
			yield return actionCollection;

#if DEBUG
			Contract.Assert(
				targetInfo.Members.Where( m => m.Member != null ).All( m => context.IsDeclaredMethod( GetPackValueMethodName( m, isAsync ) ) ),
				"Some of PackValueOfX methods are not found:" +
					String.Join(
						", ",
						targetInfo.Members
						.Where( m => m.Member != null && !context.IsDeclaredMethod( GetPackValueMethodName( m, isAsync ) ) )
						.Select( m => m.Contract.Name )
						.ToArray()
					)
			);
			Contract.Assert(
				method == SerializationMethod.Map
				|| targetInfo.Members.All( m => m.MemberName != null )
				|| context.IsDeclaredMethod( MethodName.PackMemberPlaceHolder ),
				"No PackMemberSpaceHolder."
			);
#endif // DEBUG
			var knownActions = GetKnownActions( context, targetInfo, method, GetPackValueMethodName, isAsync );

			yield return
				this.EmitStoreVariableStatement(
					context,
					actionCollection,
					method == SerializationMethod.Array
					? this.EmitCreateNewArrayExpression( context, actionType, knownActions.Length )
					: this.EmitCreateNewObjectExpression(
						context,
						actionCollection,
						new ConstructorDefinition( actionCollection.ContextType, TypeDefinition.Int32Type ),
						this.MakeInt32Literal( context, knownActions.Length )
					)
				);

			for ( int i = 0; i < knownActions.Length; i++ )
			{
				if ( method == SerializationMethod.Map
					&& knownActions[ i ].Key == null )
				{
					// For map, just slip missing item.
					continue;
				}

				var packItemBody =
					this.EmitNewPrivateMethodDelegateExpression(
						context,
						knownActions[ i ].Value
					);

				if ( method == SerializationMethod.Array )
				{
					yield return
						this.EmitSetArrayElementStatement(
							context,
							actionCollection,
							this.MakeInt32Literal( context, i ),
							packItemBody
						);
				}
				else
				{
					// Use indexer to support multiple UnpackSpaceHolder apperance.
					yield return
						this.EmitSetIndexedProperty(
							context,
							actionCollection,
							actionCollection.ContextType,
							"Item",
							// Set key as transformed.
							this.MakeStringLiteral( context, context.SerializationContext.DictionarySerlaizationOptions.SafeKeyTransformer( knownActions[ i ].Key ) ),
							packItemBody
						);
				}
			}

			yield return
				this.EmitFinishFieldInitializationStatement(
					context,
					AdjustName(
						method == SerializationMethod.Array
							? FieldName.PackOperationList
							: FieldName.PackOperationTable,
						isAsync
					),
					actionCollection
				);
		}

		protected internal TConstruct EmitPackNullCheckerTableInitialization( TContext context, SerializationTarget targetInfo )
		{
			var listType = typeof( Dictionary<,> ).MakeGenericType( typeof( string ), typeof( Func<,> ).MakeGenericType( this.TargetType, typeof( bool ) ) );
			return
				this.EmitSequentialStatements(
					context,
					listType,
					this.EmitPackNullCheckerTableInitializationCore(
						context,
						targetInfo,
						this.DeclareLocal( context, listType, "nullCheckerTable" )
					)
				);
		}

		private IEnumerable<TConstruct> EmitPackNullCheckerTableInitializationCore( TContext context, SerializationTarget targetInfo, TConstruct actionCollection )
		{
			yield return actionCollection;

			var knownActions = GetDeclaredKnownActions( context, targetInfo, m => GetCheckNullMethodName( m ) );

			yield return
				this.EmitStoreVariableStatement(
					context,
					actionCollection,
					this.EmitCreateNewObjectExpression(
						context,
						actionCollection,
						new ConstructorDefinition( actionCollection.ContextType, TypeDefinition.Int32Type ),
						this.MakeInt32Literal( context, knownActions.Length )
					)
				);

			for ( int i = 0; i < knownActions.Length; i++ )
			{
				yield return
					this.EmitSetIndexedProperty(
						context,
						actionCollection,
						actionCollection.ContextType,
						"Item",
						// Set key as transformed.
						this.MakeStringLiteral( context, context.SerializationContext.DictionarySerlaizationOptions.SafeKeyTransformer( knownActions[ i ].Key ) ),
						this.EmitNewPrivateMethodDelegateExpression(
							context,
							knownActions[ i ].Value
						)
					);
			}

			yield return
				this.EmitFinishFieldInitializationStatement(
					context,
					FieldName.NullCheckersTable,
					actionCollection
				);
		}

		private static string GetPackValueMethodName( SerializingMember member, bool isAsync )
		{
			return
				AdjustName(
					member.MemberName == null
						? MethodName.PackMemberPlaceHolder
						: MethodNamePrefix.PackValue + member.MemberName,
					isAsync
				);
		}

		private static string GetCheckNullMethodName( SerializingMember member )
		{
			return "Is" + member.MemberName + "Null";
		}

#endregion -- Pack Operation Initialization --

#region -- IUnpackable --

		private void BuildIUnpackableUnpackFrom( TContext context, TConstruct objectCreation, bool canDeserialize )
		{

			context.BeginMethodOverride( MethodName.UnpackFromCore );
			context.EndMethodOverride( MethodName.UnpackFromCore,
				canDeserialize
				? this.EmitSequentialStatements(
					context,
					this.TargetType,
					this.BuildIUnpackableUnpackFromCore( context, typeof( IUnpackable ), objectCreation )
				) : this.EmitThrowCannotUnpackFrom( context )
			);
		}

		private TConstruct GetUnpackableObjectInstantiation( TContext context )
		{
			return
				this.EmitCreateNewObjectExpression(
					context,
					null,
					// reference contextType.
					this.GetDefaultConstructor( this.TargetType )
				);
		}

#endregion -- IUnpackable --

#if FEATURE_TAP

#region -- IAsyncUnpackable --

		private void BuildIAsyncUnpackableUnpackFrom( TContext context, TConstruct objectCreation, bool canDeserialize )
		{
			context.BeginMethodOverride( MethodName.UnpackFromAsyncCore );
			context.EndMethodOverride(
				MethodName.UnpackFromAsyncCore,
				canDeserialize
				? this.EmitSequentialStatements(
					context,
					this.TargetType,
					this.BuildIUnpackableUnpackFromCore( context, typeof( IAsyncUnpackable ), objectCreation )
				) : this.EmitThrowCannotUnpackFrom( context )
			);
		}

#endregion -- IAsyncUnpackable --

#endif // FEATURE_TAP

#region -- UnpackFrom --

		private IEnumerable<TConstruct> BuildIUnpackableUnpackFromCore( TContext context, Type @interface, TConstruct objectCreation )
		{
			var result =
				this.DeclareLocal(
					context,
					this.TargetType,
					"result"
				);

			yield return result;

			if ( !this.TargetType.GetIsValueType() )
			{
				yield return
					this.EmitStoreVariableStatement(
						context,
						result,
						objectCreation
					);
			}

			var unpackFrom = this.TargetType.GetInterfaceMap( @interface ).TargetMethods.Single();

			if ( unpackFrom.ReturnType == typeof( void ) )
			{
				yield return this.EmitInvokeVoidMethod( context, result, new MethodDefinition( unpackFrom, @interface ), context.Unpacker );
				yield return this.EmitRetrunStatement( context, this.EmitLoadVariableExpression( context, result ) );
			}
			else
			{
#if FEATURE_TAP
				Contract.Assert( context.SerializationContext.SerializerOptions.WithAsync );
				yield return
					this.EmitRetrunStatement(
						context,
						this.EmitInvokeMethodExpression(
							context,
							null,
							Metadata._UnpackHelpers.UnpackFromMessageAsyncMethod.MakeGenericMethod( this.TargetType ),
							result,
							context.Unpacker,
							this.ReferCancellationToken( context, 2 )
						)
					);
#else
				ThrowAsyncNotSupportedException();
#endif // FEATURE_TAP
			}
		}

		private void BuildObjectUnpackFrom( TContext context, SerializationTarget targetInfo, bool isAsync )
		{
			if ( targetInfo.Members.Count == 0 )
			{
				throw new SerializationException(
					String.Format(
						CultureInfo.CurrentCulture,
						isAsync
						? "At least one serializable member is required because type '{0}' does not implement IAsyncUnpackable interface."
						: "At least one serializable member is required because type '{0}' does not implement IUnpackable interface.",
						this.TargetType
					)
				);
			}

			/*
			 *	#if T is IUnpackable
			 *  result.UnpackFromMessage( unpacker );
			 *	#else
			 *	if( unpacker.IsArrayHeader )
			 *	{
			 *		...
			 *	}
			 *	else
			 *	{
			 *		...
			 *	}
			 *	#endif
			 */
			var methodName = 
#if FEATURE_TAP
				isAsync ? MethodName.UnpackFromAsyncCore : 
#endif // FEATURE_TAP
				MethodName.UnpackFromCore;

			context.BeginMethodOverride( methodName );
			context.EndMethodOverride(
				methodName,
				targetInfo.CanDeserialize
				? this.EmitSequentialStatements(
					context,
					this.TargetType,
					this.EmitObjectUnpackFromCore( context, targetInfo, isAsync )
				) : this.EmitThrowCannotUnpackFrom( context )
			);
		}

		private IEnumerable<TConstruct> EmitObjectUnpackFromCore( TContext context, SerializationTarget targetInfo, bool isAsync )
		{
			var unpackingContext = this.EmitObjectUnpackingContextInitialization( context, targetInfo );

			foreach ( var statement in unpackingContext.Statements )
			{
				yield return statement;
			}

			yield return
				this.EmitConditionalExpression(
					context,
					this.EmitGetPropertyExpression( context, context.Unpacker, Metadata._Unpacker.IsArrayHeader ),
					this.EmitObjectUnpackFromCore( context, targetInfo, unpackingContext, SerializationMethod.Array, isAsync ),
					this.EmitObjectUnpackFromCore( context, targetInfo, unpackingContext, SerializationMethod.Map, isAsync )
				);

		}

		private TConstruct EmitObjectUnpackFromCore( TContext context, SerializationTarget targetInfo, UnpackingContextInfo unpackingContext, SerializationMethod method, bool isAsync )
		{
			var unpackOperationParameters =
				new[] { context.Unpacker, context.UnpackingContextInUnpackValueMethods, context.IndexOfItem, context.ItemsCount }
#if FEATURE_TAP
				.Concat( isAsync ? new [] { this.ReferCancellationToken( context, 2 ) } : NoConstructs ).ToArray()
#endif // FEATURE_TAP
				;

			for ( int i = 0; i < targetInfo.Members.Count; i++ )
			{
				var count = i;
				TConstruct privateMethodBody;
				if ( targetInfo.Members[ i ].Member == null
					// ReSharper disable once PossibleNullReferenceException
					|| ( targetInfo.IsConstructorDeserialization && !unpackingContext.MappableConstructorArguments.Contains( targetInfo.Members[ i ].Contract.Name ) ) )
				{
					// just pop
					privateMethodBody =
#if FEATURE_TAP
						isAsync
						? this.EmitRetrunStatement(
							context,
							this.EmitInvokeMethodExpression(
								context,
								context.Unpacker,
								Metadata._Unpacker.ReadAsync,
								this.ReferCancellationToken( context, 5 )
							)
						) :
#endif // FEATURE_TAP
						this.EmitInvokeVoidMethod(
							context,
							context.Unpacker,
							Metadata._Unpacker.Read
						);
				}
				else
				{
					var name = targetInfo.Members[ count ].MemberName;
					Contract.Assert( !String.IsNullOrEmpty( name ), targetInfo.Members[ count ] + "@" + i + " does not have member name.");
					var unpackedItem =
						context.DefineUnpackedItemParameterInSetValueMethods( targetInfo.Members[ count ].Member.GetMemberValueType() );
					Func<TConstruct> storeValueStatementEmitter;
					if ( targetInfo.IsConstructorDeserialization || this.TargetType.GetIsValueType() )
					{
						storeValueStatementEmitter =
							() =>
								this.EmitSetField( context, context.UnpackingContextInSetValueMethods, unpackingContext.Type, name, unpackedItem );
					}
					else
					{
						storeValueStatementEmitter =
							() =>
								this.EmitSetMemberValueStatement( context, context.UnpackingContextInSetValueMethods, targetInfo.Members[ count ].Member, unpackedItem );
					}

					privateMethodBody =
						this.EmitUnpackItemValueStatement(
							context,
							targetInfo.Members[ count ].Member.GetMemberValueType(),
							this.MakeStringLiteral( context, targetInfo.Members[ count ].MemberName ),
							targetInfo.Members[ count ].Contract.NilImplication,
							targetInfo.Members[ count ],
							null, // schema
							context.Unpacker,
							context.UnpackingContextInUnpackValueMethods,
							context.IndexOfItem,
							context.ItemsCount,
							this.EmitNewPrivateMethodDelegateExpressionWithCreation(
								context,
								new MethodDefinition(
									MethodNamePrefix.SetUnpackedValueOf + targetInfo.Members[ count ].Member.Name,
									null,
									null,
									false, // isStatic
									TypeDefinition.VoidType,
									unpackingContext.VariableType,
									targetInfo.Members[ count ].Member.GetMemberValueType()
								),
								storeValueStatementEmitter,
								context.UnpackingContextInSetValueMethods,
								unpackedItem
							),
							isAsync
						);
				}

				DefinePrivateMethod(
					context,
					GetUnpackValueMethodName( targetInfo.Members[ i ], isAsync ),
					false, // isStatic
#if FEATURE_TAP
					isAsync ? TypeDefinition.TaskType :
#endif // FEATURE_TAP
					TypeDefinition.VoidType,
					() => privateMethodBody,
					unpackOperationParameters
				);
			}

			var unpackHelperArguments =
				new TConstruct[ ( ( method == SerializationMethod.Array ) ? 5 : 4 ) + ( isAsync ? 1 : 0 ) ];

			unpackHelperArguments[ 0 ] = context.Unpacker;
			unpackHelperArguments[ 1 ] = unpackingContext.Variable;
			unpackHelperArguments[ 2 ] = unpackingContext.Factory;

			if ( method == SerializationMethod.Array )
			{
				unpackHelperArguments[ 3 ] = this.EmitGetMemberNamesExpression( context );
				unpackHelperArguments[ 4 ] = this.EmitGetActionsExpression( context, ActionType.UnpackFromArray, isAsync );
			}
			else
			{
				unpackHelperArguments[ 3 ] = this.EmitGetActionsExpression( context, ActionType.UnpackFromMap, isAsync );
			}

#if FEATURE_TAP
			if ( isAsync )
			{
				unpackHelperArguments[ unpackHelperArguments.Length - 1 ] = this.ReferCancellationToken( context, 2 );
			}
#endif // FEATURE_TAP

			return
				this.EmitRetrunStatement(
					context,
					this.EmitInvokeMethodExpression(
						context,
						null,
						new MethodDefinition(
							AdjustName( MethodNamePrefix.UnpackFrom + method, isAsync ),
							new[] { unpackingContext.Type, this.TargetType },
							TypeDefinition.UnpackHelpersType,
							true, // isStatic
							this.TargetType,
							unpackHelperArguments.Select( a => a.ContextType ).ToArray()
						),
						unpackHelperArguments
					)
				);
		}

#endregion -- UnpackFrom --

#region -- UnpackingContext Initialization --

		private UnpackingContextInfo EmitObjectUnpackingContextInitialization( TContext context, SerializationTarget targetInfo )
		{
			if ( targetInfo.IsConstructorDeserialization )
			{
				var constructorParameters = targetInfo.DeserializationConstructor.GetParameters();
				var contextFields =
					constructorParameters.Select( ( p, i ) => new KeyValuePair<string, TypeDefinition>( targetInfo.GetCorrespondingMemberName( i ) ?? ( "__OrphanParameter" + i.ToString( CultureInfo.InvariantCulture ) ), p.ParameterType ) ).ToArray();
				var constructorArguments = new List<TConstruct>( constructorParameters.Length );
				var mappableConstructorArguments = new HashSet<string>();
				var initializationStatements =
					this.InitializeConstructorArgumentInitializationStatements(
						context,
						targetInfo,
						constructorParameters,
						constructorArguments,
						mappableConstructorArguments
					).ToArray();

				var unpackingContext =
					this.EmitObjectUnpackingContextInitialization(
						context,
						contextFields,
						constructorArguments,
						mappableConstructorArguments,
						initializationStatements
					);

				unpackingContext.Factory =
					this.EmitNewPrivateMethodDelegateExpressionWithCreation(
						context,
						this.GetCreateObjectFromContextMethod( unpackingContext ),
						() => this.EmitInvokeDeserializationConstructorStatement(
							context,
							targetInfo.DeserializationConstructor,
							context.UnpackingContextInCreateObjectFromContext,
							contextFields
						),
						context.UnpackingContextInCreateObjectFromContext
					);

				return unpackingContext;
			}
			else if ( this.TargetType.GetIsValueType() )
			{
				var contextFields =
					targetInfo.Members.Where( m => m.MemberName != null )
						.Select( m => new KeyValuePair<string, TypeDefinition>( m.MemberName, m.Member.GetMemberValueType() ) )
						.ToArray();
				var constructorArguments = new List<TConstruct>( contextFields.Length );
				var mappableConstructorArguments = new HashSet<string>();
				var initializationStatements =
					this.InitializeConstructorArgumentInitializationStatements( context, contextFields, constructorArguments ).ToArray();

				var unpackingContext =
					this.EmitObjectUnpackingContextInitialization(
						context,
						contextFields,
						constructorArguments,
						mappableConstructorArguments,
						initializationStatements
					);

				unpackingContext.Factory =
					this.EmitNewPrivateMethodDelegateExpressionWithCreation(
						context,
						this.GetCreateObjectFromContextMethod( unpackingContext ),
						() => this.EmitSequentialStatements(
							context,
							this.TargetType,
							this.EmitCreateObjectFromContextCore( context, targetInfo, unpackingContext, contextFields )
						),
						context.UnpackingContextInCreateObjectFromContext
					);

				return unpackingContext;
			}
			else
			{
				// Reference type without constructor deserialization.
				var parameterType = context.DefineUnpackingContextWithResultObject();
				var unpackingContext =
					UnpackingContextInfo.Create(
						parameterType,
						this.GetDefaultConstructor( this.TargetType ),
						new HashSet<string>()
					);

				var result = this.DeclareLocal( context, this.TargetType, "result" );
				unpackingContext.Variable = result;

				unpackingContext.Statements.Add( result );

				unpackingContext.Statements.Add(
					this.EmitStoreVariableStatement(
						context,
						result,
						this.EmitCreateNewObjectExpression(
							context,
							null,
							unpackingContext.Constructor
						)
					)
				);

				unpackingContext.Factory =
					this.EmitInvokeMethodExpression(
						context,
						null,
						unpackingContext.Type.TryGetRuntimeType() == typeof( object )
						? Metadata._UnpackHelpers.Unbox_1Method.MakeGenericMethod( this.TargetType )
						: Metadata._UnpackHelpers.GetIdentity_1Method.MakeGenericMethod( unpackingContext.VariableType.ResolveRuntimeType() )
					);

				return unpackingContext;
			}
		}

		private UnpackingContextInfo EmitObjectUnpackingContextInitialization(
			TContext context,
			KeyValuePair<string, TypeDefinition>[] contextFields,
			IList<TConstruct> constructorArguments,
			HashSet<string> mappableConstructorArguments,
			IEnumerable<TConstruct> argumentInitializers
		)
		{
#if DEBUG
			Contract.Assert(
				contextFields.Length == constructorArguments.Count,
				"contextFields.Length(" + contextFields.Length + ") == constructorArguments.Count(" + constructorArguments.Count + ")"
			);
#endif // DEBUG
			TypeDefinition unpackingContextType;
			ConstructorDefinition unpackingContextConstructor;
			context.DefineUnpackingContext( contextFields, out unpackingContextType, out unpackingContextConstructor );
			var unpackingContext = UnpackingContextInfo.Create( unpackingContextType, unpackingContextConstructor, mappableConstructorArguments );
			unpackingContext.Variable = this.DeclareLocal( context, unpackingContext.VariableType, "unpackingContext" );
			unpackingContext.Statements.Add( unpackingContext.Variable );

			unpackingContext.Statements.AddRange( argumentInitializers );

			unpackingContext.Statements.Add(
				this.EmitStoreVariableStatement(
					context,
					unpackingContext.Variable,
					this.EmitCreateNewObjectExpression(
						context,
						unpackingContext.Variable,
						unpackingContext.Constructor,
						constructorArguments.ToArray()
					)
				)
			);

			return unpackingContext;
		}

#endregion -- UnpackingContext Initialization --

#region -- CreateObjectFromContext --

		private IEnumerable<TConstruct> EmitCreateObjectFromContextCore(
			TContext context, SerializationTarget targetInfo, UnpackingContextInfo unpackingContext, KeyValuePair<string, TypeDefinition>[] fields
		)
		{
#if DEBUG
			Contract.Assert( this.TargetType.GetIsValueType() );
#endif // DEBUG
			var result = this.DeclareLocal( context, this.TargetType, "result" );
			yield return result;

			var members = targetInfo.Members.Where( m => m.Member != null ).ToDictionary( m => m.Member.Name, m => m.Member );

			foreach ( var field in fields )
			{
				MemberInfo member;
				if ( !members.TryGetValue( field.Key, out member ) )
				{
					continue;
				}

				yield return
					this.EmitSetMemberValueStatement(
						context,
						result,
						member,
						this.EmitGetFieldExpression(
							context,
							context.UnpackingContextInCreateObjectFromContext,
							new FieldDefinition(
								unpackingContext.Type,
								field.Key,
								field.Value
							)
						)
					);
			}

			yield return
				this.EmitRetrunStatement(
					context,
					this.EmitLoadVariableExpression( context, result )
				);
		}

		private MethodDefinition GetCreateObjectFromContextMethod( UnpackingContextInfo unpackingContext )
		{
			return
				new MethodDefinition(
					MethodName.CreateObjectFromContext,
					null,
					null,
					true, // isStatic
					this.TargetType,
					unpackingContext.Type
				);
		}

#endregion -- CreateObjectFromContext --

#region -- Unpack Operation Initialization --

		protected internal TConstruct EmitUnpackOperationListInitialization( TContext context, SerializationTarget targetInfo, bool isAsync )
		{
			var actionType = this.GetUnpackOperationType( context, isAsync );
			var listType = TypeDefinition.Array( actionType );
			return
				this.EmitSequentialStatements(
					context,
					listType,
					this.EmitUnpackActionCollectionInitializationCore(
						context,
						targetInfo,
						actionType,
						this.DeclareLocal( context, TypeDefinition.Array( actionType ), AdjustName( "unpackOperationList", isAsync ) ),
						SerializationMethod.Array,
						isAsync
					)
				);
		}

		protected internal TConstruct EmitUnpackOperationTableInitialization( TContext context, SerializationTarget targetInfo, bool isAsync )
		{
			var actionType = this.GetUnpackOperationType( context, isAsync );
			var dictionaryType = TypeDefinition.GenericReferenceType( typeof( Dictionary<,> ), TypeDefinition.StringType, actionType );
			return
				this.EmitSequentialStatements(
					context,
					dictionaryType,
					this.EmitUnpackActionCollectionInitializationCore(
						context,
						targetInfo,
						actionType,
						this.DeclareLocal( context, dictionaryType, AdjustName( "unpackOperationTable", isAsync ) ),
						SerializationMethod.Map,
						isAsync
					)
				);
		}

		protected virtual TypeDefinition GetUnpackOperationType( TContext context, bool isAsync )
		{
			return
#if FEATURE_TAP
				isAsync
				? TypeDefinition.GenericReferenceType(
					typeof( Func<,,,,,> ),
					TypeDefinition.UnpackerType,
					context.UnpackingContextType ?? this.TargetType,
					TypeDefinition.Int32Type,
					TypeDefinition.Int32Type,
					TypeDefinition.CancellationTokenType,
					TypeDefinition.TaskType
				) :
#endif // FEATURE_TAP
				TypeDefinition.GenericReferenceType(
					typeof( Action<,,,> ),
					TypeDefinition.UnpackerType,
					context.UnpackingContextType ?? this.TargetType,
					TypeDefinition.Int32Type,
					TypeDefinition.Int32Type
				);
		}

		private static string GetUnpackValueMethodName( SerializingMember member, bool isAsync )
		{
			return
				AdjustName(
					member.MemberName == null
						? MethodName.UnpackMemberPlaceHolder
						: ( MethodNamePrefix.UnpackValue + member.MemberName ),
					isAsync
				);
		}

		private IEnumerable<TConstruct> EmitUnpackActionCollectionInitializationCore(
			TContext context,
			SerializationTarget targetInfo,
			TypeDefinition actionType,
			TConstruct actionCollection,
			SerializationMethod method,
			bool isAsync
		)
		{
			if ( !targetInfo.CanDeserialize )
			{
				yield break;
			}

			yield return actionCollection;

#if DEBUG
			Contract.Assert(
				targetInfo.Members.All( m => context.IsDeclaredMethod( GetUnpackValueMethodName( m, isAsync ) ) ),
				"Some of UnpackValueOfX methods are not found."
			);
#endif // DEBUG
			var knownActions = GetKnownActions( context, targetInfo, method, GetUnpackValueMethodName, isAsync );

			yield return
				this.EmitStoreVariableStatement(
					context,
					actionCollection,
					method == SerializationMethod.Array
					? this.EmitCreateNewArrayExpression( context, actionType, knownActions.Length )
					: this.EmitCreateNewObjectExpression(
						context,
						actionCollection,
						new ConstructorDefinition( actionCollection.ContextType, TypeDefinition.Int32Type ),
						this.MakeInt32Literal( context, knownActions.Length )
					)
				);

			for ( int i = 0; i < knownActions.Length; i++ )
			{
				if ( method == SerializationMethod.Map
					&& knownActions[ i ].Key == null )
				{
					// For map, just slip missing item.
					continue;
				}

				var unpackItemBody =
					this.EmitNewPrivateMethodDelegateExpression(
						context,
						knownActions[ i ].Value
					);

				if ( method == SerializationMethod.Array )
				{
					yield return
						this.EmitSetArrayElementStatement(
							context,
							actionCollection,
							this.MakeInt32Literal( context, i ),
							unpackItemBody
						);
				}
				else
				{
					// Use indexer to support multiple UnpackSpaceHolder apperance.
					yield return
						this.EmitSetIndexedProperty(
							context,
							actionCollection,
							actionCollection.ContextType,
							"Item",
							// Set key as transformed.
							this.MakeStringLiteral( context, context.SerializationContext.DictionarySerlaizationOptions.SafeKeyTransformer( knownActions[ i ].Key ) ),
							unpackItemBody
						);
				}
			}

			yield return
				this.EmitFinishFieldInitializationStatement(
					context,
					AdjustName(
						method == SerializationMethod.Array
							? FieldName.UnpackOperationList
							: FieldName.UnpackOperationTable,
						isAsync
					),
					actionCollection
				);
		}

		private IEnumerable<TConstruct> InitializeConstructorArgumentInitializationStatements( TContext context, SerializationTarget target, ParameterInfo[] constructorParameters, List<TConstruct> constructorArguments, HashSet<string> mappableConstructorArguments )
		{
#if DEBUG
			Contract.Assert( target.IsConstructorDeserialization, "target.IsConstructorDeserialization" );
#endif // DEBUG
			for ( var i = 0; i < constructorParameters.Length; i++ )
			{
				var argument =
					this.DeclareLocal(
						context,
						constructorParameters[ i ].ParameterType,
						"ctorArg" + i.ToString( CultureInfo.InvariantCulture )
					);

				yield return argument;

				constructorArguments.Add( argument );
				var correspondingMemberName = target.GetCorrespondingMemberName( i );
				if ( correspondingMemberName != null )
				{
					mappableConstructorArguments.Add( correspondingMemberName );
				}

				yield return
					this.EmitStoreVariableStatement(
						context,
						argument,
						this.MakeDefaultParameterValueLiteral(
							context,
							argument,
							constructorParameters[ i ].ParameterType,
							constructorParameters[ i ].DefaultValue,
							constructorParameters[ i ].GetHasDefaultValue()
						)
					);
			}
		}

		private IEnumerable<TConstruct> InitializeConstructorArgumentInitializationStatements( TContext context, KeyValuePair<string, TypeDefinition>[] contextFields, List<TConstruct> constructorArguments )
		{
			for ( var i = 0; i < contextFields.Length; i++ )
			{
				var argument =
					this.DeclareLocal(
						context,
						contextFields[ i ].Value,
						"ctorArg" + i.ToString( CultureInfo.InvariantCulture )
					);

				yield return argument;

				constructorArguments.Add( argument );
				yield return
					this.EmitStoreVariableStatement(
						context,
						argument,
						this.MakeDefaultLiteral( context, contextFields[ i ].Value )
					);
			}
		}

		private TConstruct EmitInvokeDeserializationConstructorStatement( TContext context, ConstructorInfo constructor, TConstruct unpackingContext, IList<KeyValuePair<string, TypeDefinition>> fields )
		{
			return
				this.EmitSequentialStatements(
					context,
					constructor.DeclaringType,
					this.EmitInvokeDeserializationConstructorStatementsCore( context, constructor, unpackingContext, fields ).ToArray()
				);
		}

		private IEnumerable<TConstruct> EmitInvokeDeserializationConstructorStatementsCore( TContext context, ConstructorInfo constructor, TConstruct unpackingContext, IList<KeyValuePair<string, TypeDefinition>> fields )
		{
			var resultVariable =
				this.DeclareLocal( context, constructor.DeclaringType, "result" );

			yield return resultVariable;
			yield return
				this.EmitStoreVariableStatement(
					context,
					resultVariable,
					this.EmitCreateNewObjectExpression(
						context,
						resultVariable,
						constructor,
						fields.Select(
							f =>
								this.EmitGetFieldExpression( context, unpackingContext, new FieldDefinition( unpackingContext.ContextType, f.Key, f.Value ) )
						).ToArray()
					)
				);
			yield return
				this.EmitRetrunStatement(
					context,
					this.EmitLoadVariableExpression( context, resultVariable )
				);
		}

		protected internal TConstruct EmitMemberListInitialization( TContext context, SerializationTarget targetInfo )
		{
			return
				this.EmitFinishFieldInitializationStatement(
					context,
					FieldName.MemberNames,
					this.EmitCreateNewArrayExpression(
						context,
						TypeDefinition.StringType,
						targetInfo.Members.Count,
						targetInfo.Members.Select(
							m =>
								m.MemberName == null
								? this.MakeNullLiteral( context, TypeDefinition.StringType )
								: this.MakeStringLiteral( context, m.MemberName )
						).ToArray()
					)
				);
		}

		protected abstract TConstruct EmitGetMemberNamesExpression( TContext context );

#endregion -- Unpack Operation Initialization --

#region -- Operation Helpers --

		protected abstract TConstruct EmitGetActionsExpression( TContext context, ActionType actionType, bool isAsync );

		protected abstract TConstruct EmitFinishFieldInitializationStatement( TContext context, string name, TConstruct value );

		// For factory for UnpakcHelpers.
		private TConstruct EmitNewPrivateMethodDelegateExpressionWithCreation( TContext context, MethodDefinition method, Func<TConstruct> bodyFactory, params TConstruct[] privateMethodParameters )
		{
			return
				this.ExtractPrivateMethod(
					context,
					method.MethodName,
					false, // isStatic
					method.ReturnType,
					bodyFactory,
					privateMethodParameters
				);
		}

		private static KeyValuePair<string, MethodDefinition>[] GetKnownActions( TContext context, SerializationTarget targetInfo, SerializationMethod method, Func<SerializingMember, bool, string> nameFactory, bool isAsync )
		{
			var filter = method == SerializationMethod.Array ? _ => true : new Func<SerializingMember, bool>( m => m.MemberName != null );

			return
				targetInfo.Members
				.Where( filter )
				.Select( m =>
					new KeyValuePair<string, MethodDefinition>(
						m.MemberName,
						context.GetDeclaredMethod( nameFactory( m, isAsync ) )
					)
				).ToArray();

		}

		private static KeyValuePair<string, MethodDefinition>[] GetDeclaredKnownActions( TContext context, SerializationTarget targetInfo, Func<SerializingMember, string> nameFactory )
		{
			return
				targetInfo.Members
				.Where( m => m.MemberName != null )
				.Select( m =>
					new KeyValuePair<string, MethodDefinition>(
						m.MemberName,
						context.TryGetDeclaredMethod( nameFactory( m ) )
					)
				).Where( kv => kv.Value != null )
				.ToArray();

		}

#endregion -- Operation Helpers --
	}
}

