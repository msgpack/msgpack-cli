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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private void BuildObjectSerializer( TContext context, out SerializationTarget targetInfo )
		{
			SerializationTarget.VerifyType( typeof( TObject ) );
			targetInfo = SerializationTarget.Prepare( context.SerializationContext, typeof( TObject ) );

			if ( typeof( IPackable ).IsAssignableFrom( typeof( TObject ) ) )
			{
				this.BuildIPackablePackTo( context );
			}
			else
			{
				this.BuildObjectPackTo( context, targetInfo );
			}

			if ( typeof( IUnpackable ).IsAssignableFrom( typeof( TObject ) ) )
			{
				this.BuildIUnpackableUnpackFrom( context );
			}
			else
			{
				this.BuildObjectUnpackFrom( context, targetInfo );
			}
		}

		#region -- IPackable --

		private void BuildIPackablePackTo( TContext context )
		{
			context.BeginMethodOverride( MethodName.PackToCore );
			context.EndMethodOverride(
				MethodName.PackToCore,
				this.EmitInvokeVoidMethod(
					context,
					context.PackToTarget,
					typeof( TObject ).GetInterfaceMap( typeof( IPackable ) ).TargetMethods.Single(),
					context.Packer,
					this.MakeNullLiteral( context, typeof( PackingOptions ) )
				)
			);
		}


		#endregion -- IPackable --

		#region -- PackTo --
		
		private void BuildObjectPackTo( TContext context, SerializationTarget targetInfo )
		{
			context.BeginMethodOverride( MethodName.PackToCore );
			context.EndMethodOverride(
				MethodName.PackToCore,
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					this.BuildObjectPackToCore( context, targetInfo.Members, context.SerializationContext.SerializationMethod )
				)
			);
		}

		private IEnumerable<TConstruct> BuildObjectPackToCore( TContext context, IList<SerializingMember> entries, SerializationMethod method )
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
					this.EmitPrivateMethod(
						context,
						MethodName.PackMemberPlaceHolder,
						typeof( void ),
						() => this.EmitInvokeVoidMethod( context, context.Packer, Metadata._Packer.PackNull ),
						context.Packer,
						context.PackToTarget
					);
				}
				else
				{
					this.EmitPrivateMethod(
						context,
						GetPackValueMethodName( entries[ i ] ),
						typeof( void ),
						() =>
							this.EmitSequentialStatements(
								context,
								typeof( void ),
								this.EmitPackItemStatements(
									context,
									context.Packer,
									entries[ count ].Member.GetMemberValueType(),
									entries[ count ].Contract.NilImplication,
									entries[ count ].Member.ToString(),
									this.EmitGetMemberValueExpression( context, context.PackToTarget, entries[ count ].Member ),
									entries[ count ],
									null
								)
							),
						context.Packer, 
						context.PackToTarget
					);
				}
			}

			var packHelperArguments =
				new[]
				{
					context.Packer,
					context.PackToTarget,
					this.EmitGetActionsExpression(
						context,
						method == SerializationMethod.Array
						? ActionType.PackToArray
						: ActionType.PackToMap
					)
				};

			yield return
				this.EmitInvokeMethodExpression(
					context,
					null,
					new MethodDefinition(
						"PackTo" + method,
						new [] { TypeDefinition.Object( typeof( TObject ) ),},
						typeof( PackHelpers ),
						typeof( void ),
						packHelperArguments.Select( a => a.ContextType ).ToArray()
					),
					packHelperArguments
				);
		}

		#endregion -- IPackable --

		#region -- Pack Operation Initialization --

		protected internal TConstruct EmitPackOperationListInitialization( TContext context, SerializationTarget targetInfo )
		{
			var actionType = this.GetPackOperationType( context );
			var listType = TypeDefinition.Array( actionType );
			return
				this.EmitSequentialStatements(
					context,
					listType,
					this.EmitPackActionCollectionCore(
						context,
						targetInfo,
						actionType,
						this.DeclareLocal( context, listType, "packOperationList" ),
						SerializationMethod.Array
					)
				);
		}

		protected internal TConstruct EmitPackOperationTableInitialization( TContext context, SerializationTarget targetInfo )
		{
			var actionType = this.GetPackOperationType( context );
			var listType = TypeDefinition.GenericReferenceType( typeof( Dictionary<,> ), typeof( string ), actionType );
			return
				this.EmitSequentialStatements(
					context,
					listType,
					this.EmitPackActionCollectionCore(
						context,
						targetInfo,
						actionType,
						this.DeclareLocal( context, listType, "packOperationTable" ),
						SerializationMethod.Map
					)
				);
		}

		protected virtual TypeDefinition GetPackOperationType( TContext context )
		{
			return typeof( Action<Packer, TObject> );
		}

		private IEnumerable<TConstruct> EmitPackActionCollectionCore( TContext context, SerializationTarget targetInfo, TypeDefinition actionType, TConstruct actionCollection, SerializationMethod method )
		{
			yield return actionCollection;

#if DEBUG
			Contract.Assert(
				targetInfo.Members.Where( m => m.Member != null ).All( m => context.IsDeclaredMethod( GetPackValueMethodName( m ) ) ),
				"Some of PackValueOfX methods are not found:" +
					String.Join(
						", ",
						targetInfo.Members
						.Where( m => m.Member != null && !context.IsDeclaredMethod( GetPackValueMethodName( m ) ) )
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
			var knownActions = GetKnownActions( context, targetInfo, method, GetPackValueMethodName );

			yield return
				this.EmitStoreVariableStatement(
					context,
					actionCollection,
					method == SerializationMethod.Array
					? this.EmitCreateNewArrayExpression( context, actionType, knownActions.Length )
					: this.EmitCreateNewObjectExpression(
						context,
						actionCollection,
						new ConstructorDefinition( actionCollection.ContextType, typeof( int ) ),
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
							this.MakeStringLiteral( context, knownActions[ i ].Key ),
							packItemBody
						);
				}
			}

			yield return
				this.EmitFinishFieldInitializationStatement(
					context,
					method == SerializationMethod.Array
					? FieldName.PackOperationList
					: FieldName.PackOperationTable,
					actionCollection
				);
		}

		private static string GetPackValueMethodName( SerializingMember member )
		{
			return
				member.MemberName == null
				? MethodName.PackMemberPlaceHolder
				: MethodNamePrefix.PackValue + member.MemberName;
		}

		#endregion -- Pack Operation Initialization --

		#region -- IUnpackable --

		private void BuildIUnpackableUnpackFrom( TContext context )
		{

			context.BeginMethodOverride( MethodName.UnpackFromCore );
			context.EndMethodOverride( MethodName.UnpackFromCore,
				this.EmitSequentialStatements(
					context,
					typeof( TObject ),
					this.BuildIUnpackableUnpackFromCore( context, typeof( IUnpackable ) )
				)
			);
		}


		#endregion -- IUnpackable --
	
		#region -- UnpackFrom --
		
		private IEnumerable<TConstruct> BuildIUnpackableUnpackFromCore( TContext context, Type @interface )
		{
			var result =
				this.DeclareLocal(
					context,
					typeof( TObject ),
					"result"
				);

			yield return result;

			if ( !typeof( TObject ).GetIsValueType() )
			{
				yield return
					this.EmitStoreVariableStatement(
						context,
						result,
						this.EmitCreateNewObjectExpression(
							context,
							null, // reference contextType.
							GetDefaultConstructor( typeof( TObject ) )
						)
					);
			}

			var unpackFrom = typeof( TObject ).GetInterfaceMap( @interface ).TargetMethods.Single();

			if ( unpackFrom.ReturnType == typeof( void ) )
			{
				yield return this.EmitInvokeVoidMethod( context, result, unpackFrom, context.Unpacker );
				yield return this.EmitRetrunStatement( context, this.EmitLoadVariableExpression( context, result ) );
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		private void BuildObjectUnpackFrom( TContext context, SerializationTarget targetInfo )
		{
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

			context.BeginMethodOverride( MethodName.UnpackFromCore );
			context.EndMethodOverride(
				MethodName.UnpackFromCore,
				this.EmitSequentialStatements(
					context,
					typeof( TObject ),
					this.EmitObjectUnpackFromCore( context, targetInfo )
				)
			);
		}

		private IEnumerable<TConstruct> EmitObjectUnpackFromCore( TContext context, SerializationTarget targetInfo )
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
					this.EmitObjectUnpackFromCore( context, targetInfo, unpackingContext, SerializationMethod.Array ),
					this.EmitObjectUnpackFromCore( context, targetInfo, unpackingContext, SerializationMethod.Map )
				);

		}

		private TConstruct EmitObjectUnpackFromCore( TContext context, SerializationTarget targetInfo, UnpackingContextInfo unpackingContext, SerializationMethod method )
		{
			var unpackOperationParameters = new[] { context.Unpacker, context.UnpackingContextInUnpackValueMethods, context.IndexOfItem };

			int constructorParameterIndex = 0;
			var fieldNames =
				targetInfo.IsConstructorDeserialization
					? targetInfo.DeserializationConstructor.GetParameters().Select( p => p.Name ).ToArray()
					: targetInfo.Members.Where( m => m.MemberName != null ).Select( m => m.MemberName ).ToArray();

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
						this.EmitInvokeVoidMethod(
							context,
							context.Unpacker,
							Metadata._Unpacker.Read
						);
				}
				else
				{
					Func<TConstruct, TConstruct> storeValueStatementEmitter;
					if ( unpackingContext.VariableType.TryGetRuntimeType() == typeof( DynamicUnpackingContext ) )
					{
						storeValueStatementEmitter =
							unpackedItem =>
								this.EmitInvokeVoidMethod(
									context,
									context.UnpackingContextInUnpackValueMethods,
									Metadata._DynamicUnpackingContext.Set,
									this.MakeStringLiteral( context, fieldNames[ count ] ),
									targetInfo.Members[ count ].Member.GetMemberValueType().GetIsValueType()
									? this.EmitBoxExpression(
										context,
										unpackedItem.ContextType,
										unpackedItem
									) : unpackedItem
								);
					}
					else if ( targetInfo.IsConstructorDeserialization || typeof( TObject ).GetIsValueType() )
					{
						var name = fieldNames[ constructorParameterIndex ];
						storeValueStatementEmitter =
							unpackedItem =>
								this.EmitSetField( context, context.UnpackingContextInUnpackValueMethods, unpackingContext.Type, name, unpackedItem );
						constructorParameterIndex++;
					}
					else
					{
						storeValueStatementEmitter =
							unpackedItem =>
								this.EmitSetMemberValueStatement( context, context.UnpackingContextInUnpackValueMethods, targetInfo.Members[ count ].Member, unpackedItem );
					}

					privateMethodBody =
						this.EmitUnpackItemValueExpression(
							context,
							targetInfo.Members[ count ].Member.GetMemberValueType(),
							targetInfo.Members[ count ].Contract.NilImplication,
							context.Unpacker,
							context.IndexOfItem,
							this.MakeStringLiteral( context, targetInfo.Members[ count ].Member.Name ),
							targetInfo.Members[ i ],
							null,
							storeValueStatementEmitter
						);
				}

				this.EmitPrivateMethod(
					context,
					GetUnpackValueMethodName( targetInfo.Members[ i ] ),
					typeof( void ),
					() => privateMethodBody,
					unpackOperationParameters
				);
			}

			var unpackHelperArguments =
				new TConstruct[ method == SerializationMethod.Array ? 5 : 4 ];

			unpackHelperArguments[ 0 ] = context.Unpacker;
			unpackHelperArguments[ 1 ] = unpackingContext.Variable;
			unpackHelperArguments[ 2 ] = unpackingContext.Factory;

			if ( method == SerializationMethod.Array )
			{
				unpackHelperArguments[ 3 ] = this.EmitGetMemberNamesExpression( context );
				unpackHelperArguments[ 4 ] = this.EmitGetActionsExpression( context, ActionType.UnpackFromArray );
			}
			else
			{
				unpackHelperArguments[ 3 ] = this.EmitGetActionsExpression( context, ActionType.UnpackFromMap );
			}

			return
				this.EmitRetrunStatement(
					context,
					this.EmitInvokeMethodExpression(
						context,
						null,
						new MethodDefinition(
							MethodNamePrefix.UnpackFrom + method,
							new[] { unpackingContext.Type, typeof( TObject ) },
							typeof( UnpackHelpers ),
							typeof( TObject ),
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
					constructorParameters.Select( p => new KeyValuePair<string, TypeDefinition>( p.Name, p.ParameterType ) ).ToArray();
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
						GetCreateObjectFromContextMethod( unpackingContext ),
						() =>
							this.EmitInvokeDeserializationConstructorStatement(
								context,
								targetInfo.DeserializationConstructor,
								context.UnpackingContextInCreateObjectFromContext,
								contextFields
							),
						context.UnpackingContextInCreateObjectFromContext
					);

				return unpackingContext;
			}
			else if ( typeof( TObject ).GetIsValueType() )
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
						GetCreateObjectFromContextMethod( unpackingContext ),
						() =>
							this.EmitSequentialStatements(
								context,
								typeof( TObject ),
								this.EmitCreateObjectFromContextCore( context, targetInfo, unpackingContext, contextFields )
							),
						context.UnpackingContextInCreateObjectFromContext
					);

				return unpackingContext;
			}
			else
			{
				// Reference tyoe without constructor deserialization.
				var parameterType = context.DefineUnpackingContextWithResultObject();
				var unpackingContext =
					UnpackingContextInfo.Create(
						parameterType,
						GetDefaultConstructor( typeof( TObject ) ),
						new HashSet<string>()
					);

				var result = this.DeclareLocal( context, typeof( TObject ), "result" );
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
						? Metadata._UnpackHelpers.Unbox_1Method.MakeGenericMethod( typeof( TObject ) )
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
				unpackingContext.VariableType.TryGetRuntimeType() == typeof( DynamicUnpackingContext )
				? this.EmitSequentialStatements(
					context,
					typeof ( void ),
					new[]
					{
						this.EmitStoreVariableStatement(
							context,
							unpackingContext.Variable,
							this.EmitCreateNewObjectExpression(
								context,
								unpackingContext.Variable,
								unpackingContext.Constructor,
								this.MakeInt32Literal( context, constructorArguments.Count )
							)
						)
					}.Concat(
						constructorArguments.Select( ( a, i ) =>
							this.EmitInvokeVoidMethod(
								context,
								unpackingContext.Variable,
								Metadata._DynamicUnpackingContext.Set,
								this.MakeStringLiteral( context, contextFields[ i ].Key ),
								a.ContextType.ResolveRuntimeType().GetIsValueType()
									? this.EmitBoxExpression(
										context,
										a.ContextType,
										a
									) : a
							)
						)
					)
				) : this.EmitStoreVariableStatement(
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
			Contract.Assert( typeof( TObject ).GetIsValueType() );
#endif // DEBUG
			var result = this.DeclareLocal( context, typeof( TObject ), "result" );
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
						unpackingContext.VariableType.TryGetRuntimeType() == typeof( DynamicUnpackingContext )
							? this.EmitUnboxAnyExpression(
								context,
								field.Value,
								this.EmitInvokeMethodExpression(
									context,
									context.UnpackingContextInCreateObjectFromContext,
									Metadata._DynamicUnpackingContext.Get,
									this.MakeStringLiteral( context, field.Key )
									)
								)
							: this.EmitGetFieldExpression(
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

		private static MethodDefinition GetCreateObjectFromContextMethod( UnpackingContextInfo unpackingContext )
		{
			return
				new MethodDefinition(
					MethodName.CreateObjectFromContext,
					null,
					null,
					typeof( TObject ),
					unpackingContext.Type
				);
		}

		#endregion -- CreateObjectFromContext --

		#region -- Unpack Operation Initialization --

		protected internal TConstruct EmitUnpackOperationListInitialization( TContext context, SerializationTarget targetInfo )
		{
			var actionType = this.GetUnpackOperationType( context );
			var listType = TypeDefinition.Array( actionType );
			return
				this.EmitSequentialStatements(
					context,
					listType,
					this.EmitUnpackActionCollectionInitializationCore(
						context,
						targetInfo,
						actionType,
						this.DeclareLocal( context, TypeDefinition.Array( actionType ), "unpackOperationList" ),
						SerializationMethod.Array
					)
				);
		}

		protected internal TConstruct EmitUnpackOperationTableInitialization( TContext context, SerializationTarget targetInfo )
		{
			var actionType = this.GetUnpackOperationType( context );
			var dictionaryType = TypeDefinition.GenericReferenceType( typeof( Dictionary<,> ), typeof( string ), actionType );
			return
				this.EmitSequentialStatements(
					context,
					dictionaryType,
					this.EmitUnpackActionCollectionInitializationCore(
						context,
						targetInfo,
						actionType,
						this.DeclareLocal( context, dictionaryType, "unpackOperationTable" ),
						SerializationMethod.Map
					)
				);
		}

		protected virtual TypeDefinition GetUnpackOperationType( TContext context )
		{
			return
				TypeDefinition.GenericReferenceType(
					typeof( Action<,,> ),
					typeof( Unpacker ),
					context.UnpackingContextType ?? typeof( TObject ),
					typeof( int )
				);
		}

		private static string GetUnpackValueMethodName( SerializingMember member )
		{
			return member.MemberName == null
				? MethodName.UnpackMemberPlaceHolder
				: ( MethodNamePrefix.UnpackValue + member.MemberName );
		}

		private IEnumerable<TConstruct> EmitUnpackActionCollectionInitializationCore( TContext context, SerializationTarget targetInfo, TypeDefinition actionType, TConstruct actionCollection, SerializationMethod method )
		{
			yield return actionCollection;

#if DEBUG
			Contract.Assert(
				targetInfo.Members.All( m => context.IsDeclaredMethod( GetUnpackValueMethodName( m ) ) ),
				"Some of UnpackValueOfX methods are not found."
			);
#endif // DEBUG
			var knownActions = GetKnownActions( context, targetInfo, method, GetUnpackValueMethodName );

			yield return
				this.EmitStoreVariableStatement(
					context,
					actionCollection,
					method == SerializationMethod.Array
					? this.EmitCreateNewArrayExpression( context, actionType, knownActions.Length )
					: this.EmitCreateNewObjectExpression(
						context,
						actionCollection,
						new ConstructorDefinition( actionCollection.ContextType, typeof( int ) ),
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
							this.MakeStringLiteral( context, knownActions[ i ].Key ),
							unpackItemBody
						);
				}
			}

			yield return
				this.EmitFinishFieldInitializationStatement(
					context,
					method == SerializationMethod.Array
					? FieldName.UnpackOperationList
					: FieldName.UnpackOperationTable,
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
				var correspondingMemberName = target.FindCorrespondingMemberName( constructorParameters[ i ] );
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
								unpackingContext.ContextType.TryGetRuntimeType() == typeof( DynamicUnpackingContext )
								? this.EmitUnboxAnyExpression( 
									context,
									f.Value,
									this.EmitInvokeMethodExpression(
										context,
										unpackingContext,
										Metadata._DynamicUnpackingContext.Get,
										this.MakeStringLiteral( context, f.Key )
									)
								)
								: this.EmitGetFieldExpression( context, unpackingContext, new FieldDefinition( unpackingContext.ContextType, f.Key, f.Value ) )
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
						typeof( string ),
						targetInfo.Members.Count,
						targetInfo.Members.Select(
							m => 
								m.MemberName == null
								? this.MakeNullLiteral( context, typeof( string ) )
								: this.MakeStringLiteral( context, m.MemberName )
						).ToArray()
					)
				);
		}

		protected abstract TConstruct EmitGetMemberNamesExpression( TContext context );

		#endregion -- Unpack Operation Initialization --

		#region -- Operation Helpers --

		protected abstract TConstruct EmitGetActionsExpression( TContext context, ActionType actionType );

		protected abstract TConstruct EmitFinishFieldInitializationStatement( TContext context, string name, TConstruct value );

		// For PackCore and UnpackFromCore
		private void EmitPrivateMethod( TContext context, string name, TypeDefinition returnType, Func<TConstruct> bodyFactory, params TConstruct[] privateMethodParameters )
		{
			if ( !context.IsDeclaredMethod( name ) )
			{
				this.ExtractPrivateMethod(
					context,
					name,
					returnType,
					bodyFactory(),
					privateMethodParameters
				);
			}
		}

		// For factory for UnpakcHelpers.
		private TConstruct EmitNewPrivateMethodDelegateExpressionWithCreation( TContext context, MethodDefinition method, Func<TConstruct> bodyFactory, params TConstruct[] privateMethodParameters )
		{
			return
				context.IsDeclaredMethod( method.MethodName )
				? this.EmitNewPrivateMethodDelegateExpression( context, method )
				: this.ExtractPrivateMethod(
					context,
					method.MethodName,
					method.ReturnType,
					bodyFactory(),
					privateMethodParameters
				);
		}

		private static KeyValuePair<string, MethodDefinition>[] GetKnownActions( TContext context, SerializationTarget targetInfo, SerializationMethod method, Func<SerializingMember, string> nameFactory )
		{
			var filter = method == SerializationMethod.Array ? _ => true : new Func<SerializingMember, bool>( m => m.MemberName != null );

			return
				targetInfo.Members
				.Where( filter )
				.Select( m =>
					new KeyValuePair<string, MethodDefinition>(
						m.MemberName,
						context.GetDeclaredMethod( nameFactory( m ) )
					)
				).ToArray();

		}

		#endregion -- Operation Helpers --
	}
}
