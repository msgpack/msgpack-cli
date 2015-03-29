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
// Contributors:
//    Takeshi KIRIYA
//
#endregion -- License Terms --

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private void BuildObjectSerializer( TContext context )
		{
			SerializationTarget.VerifyType( typeof( TObject ) );
			var target = SerializationTarget.Prepare( context.SerializationContext, typeof( TObject ) );

			if ( typeof( IPackable ).IsAssignableFrom( typeof( TObject ) ) )
			{
				this.BuildIPackablePackTo( context );
			}
			else
			{
				this.BuildObjectPackTo( context, target );
			}

			if ( typeof( IUnpackable ).IsAssignableFrom( typeof( TObject ) ) )
			{
				this.BuildIUnpackableUnpackFrom( context );
			}
			else
			{
				this.BuildObjectUnpackFrom( context, target );
			}
		}

		private void BuildIPackablePackTo( TContext context )
		{
			this.EmitMethodPrologue( context, SerializerMethod.PackToCore );
			TConstruct construct = null;
			try
			{
				construct =
					this.EmitInvokeVoidMethod(
						context,
						context.PackToTarget,
						typeof( TObject ).GetInterfaceMap( typeof( IPackable ) ).TargetMethods.Single(),
						context.Packer,
						this.MakeNullLiteral( context, typeof( PackingOptions ) )
					);
			}
			finally
			{
				this.EmitMethodEpilogue( context, SerializerMethod.PackToCore, construct );
			}
		}

		private void BuildObjectPackTo( TContext context, SerializationTarget target )
		{
			this.EmitMethodPrologue( context, SerializerMethod.PackToCore );
			TConstruct construct = null;
			try
			{
				construct =
					this.EmitSequentialStatements(
						context,
						typeof( void ),
						context.SerializationContext.SerializationMethod == SerializationMethod.Array
						? this.BuildObjectPackToWithArray( context, target.Members )
						: this.BuildObjectPackToWithMap( context, target.Members )
					);
			}
			finally
			{
				this.EmitMethodEpilogue( context, SerializerMethod.PackToCore, construct );
			}
		}

		private IEnumerable<TConstruct> BuildObjectPackToWithArray( TContext context, IList<SerializingMember> entries )
		{
			yield return
				this.EmitPutArrayHeaderExpression(
					context, this.MakeInt32Literal( context, entries.Count )
				);

			for ( int i = 0; i < entries.Count; i++ )
			{
				if ( entries[ i ].Member == null )
				{
					// missing member, always nil
					yield return
						this.EmitInvokeVoidMethod( context, context.Packer, Metadata._Packer.PackNull );
				}
				else
				{
					foreach ( var packItem in
						this.EmitPackItemStatements(
							context,
							context.Packer,
							entries[ i ].Member.GetMemberValueType(),
							entries[ i ].Contract.NilImplication,
							entries[ i ].Member.ToString(),
							this.EmitGetMemberValueExpression( context, context.PackToTarget, entries[ i ].Member ),
							entries[ i ],
							null
						)
					)
					{
						yield return packItem;
					}
				}
			}
		}

		private IEnumerable<TConstruct> BuildObjectPackToWithMap( TContext context, IList<SerializingMember> entries )
		{
			yield return
				this.EmitPutMapHeaderExpression( context, this.MakeInt32Literal( context, entries.Count( e => e.Member != null ) ) );

			for ( int i = 0; i < entries.Count; i++ )
			{
				if ( entries[ i ].Member == null )
				{
					// skip
					continue;
				}

				foreach ( var packKey in
					this.EmitPackItemStatements(
						context,
						context.Packer,
						typeof( string ),
						NilImplication.Null,
						"MemberName",
						this.MakeStringLiteral( context, entries[ i ].Contract.Name ),
						entries[ i ],
						null
					)
				)
				{
					yield return packKey;
				}

				foreach ( var packValue in
					this.EmitPackItemStatements(
						context,
						context.Packer,
						entries[ i ].Member.GetMemberValueType(),
						entries[ i ].Contract.NilImplication,
						entries[ i ].Member.ToString(),
						this.EmitGetMemberValueExpression( context, context.PackToTarget, entries[ i ].Member ),
						entries[ i ],
						null
					)
				)
				{
					yield return packValue;
				}
			}
		}

		private void BuildIUnpackableUnpackFrom( TContext context )
		{

			this.EmitMethodPrologue( context, SerializerMethod.UnpackFromCore );
			TConstruct construct = null;
			try
			{
				construct =
					this.EmitSequentialStatements(
						context,
						typeof( TObject ),
						this.BuildIUnpackableUnpackFromCore( context )
					);
			}
			finally
			{
				this.EmitMethodEpilogue( context, SerializerMethod.UnpackFromCore, construct );
			}
		}

		private IEnumerable<TConstruct> BuildIUnpackableUnpackFromCore( TContext context )
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

			yield return
				this.EmitInvokeVoidMethod(
					context,
					result,
					typeof( TObject ).GetInterfaceMap( typeof( IUnpackable ) ).TargetMethods.Single(),
					context.Unpacker
				);

			yield return this.EmitRetrunStatement( context, this.EmitLoadVariableExpression( context, result ) );
		}

		private void BuildObjectUnpackFrom( TContext context, SerializationTarget target )
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

			this.EmitMethodPrologue( context, SerializerMethod.UnpackFromCore );
			TConstruct construct = null;
			try
			{
				construct =
					this.EmitSequentialStatements(
						context,
						typeof( TObject ),
						this.BuildObjectUnpackFromCore( context, target )
					);
			}
			finally
			{
				this.EmitMethodEpilogue( context, SerializerMethod.UnpackFromCore, construct );
			}
		}

		private IEnumerable<TConstruct> BuildObjectUnpackFromCore( TContext context, SerializationTarget target )
		{
			var result =
				this.DeclareLocal(
					context,
					typeof( TObject ),
					"result"
				);
			yield return result;
			if ( !typeof( TObject ).GetIsValueType() && !target.IsConstructorDeserialization )
			{
				yield return
					this.EmitStoreVariableStatement(
						context,
						result,
						this.EmitCreateNewObjectExpression(
							context,
							null, // reference contextType
							GetDefaultConstructor( typeof( TObject ) )
						)
					);
			}

			yield return
				this.EmitConditionalExpression(
					context,
					this.EmitGetPropertyExpression( context, context.Unpacker, Metadata._Unpacker.IsArrayHeader ),
					this.EmitObjectUnpackFromArray( context, result, target ),
					this.EmitObjectUnpackFromMap( context, result, target )
				);

			yield return this.EmitRetrunStatement( context, this.EmitLoadVariableExpression( context, result ) );
		}

		private TConstruct EmitObjectUnpackFromArray( TContext context, TConstruct result, SerializationTarget target )
		{
			// TODO: Supports ExtensionObject like round-tripping.
			return
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					this.EmitObjectUnpackFromArrayCore( context, result, target )
				);
		}

		private IEnumerable<TConstruct> EmitObjectUnpackFromArrayCore( TContext context, TConstruct result, SerializationTarget target )
		{
			/*
			 *	int unpacked = 0;
			 *	int itemsCount = ITEMS_COUNT( unpacker );
			 *		:
			 *	T? item;
			 *	if ( unpacked >= itemsCount )
			 *	{
			 *		item = null;
			 *	}
			 */

			var unpacked =
				this.DeclareLocal(
					context,
					typeof( int ),
					"unpacked"
				);

			yield return unpacked;

			var itemsCount =
				this.DeclareLocal(
					context,
					typeof( int ),
					"itemsCount"
				);
			yield return itemsCount;

			yield return
				this.EmitStoreVariableStatement(
					context,
					itemsCount,
					this.EmitGetItemsCountExpression( context, context.Unpacker )
				);

			var constructorParameters =
				target.IsConstructorDeserialization
					? target.DeserializationConstructor.GetParameters()
					: null;
			var constructorArguments =
				target.IsConstructorDeserialization
				// ReSharper disable once PossibleNullReferenceException
					? new List<TConstruct>( constructorParameters.Length )
					: null;
			var constructorArgumentsIndex =
				target.IsConstructorDeserialization
				// ReSharper disable once PossibleNullReferenceException
					? new Dictionary<string, TConstruct>( constructorParameters.Length )
					: null;
			foreach (
				var construct in
					this.InitializeConstructorArgumentInitializationStatements(
						context,
						target,
						constructorParameters,
						constructorArguments,
						constructorArgumentsIndex ) )
			{
				yield return construct;
			}


			for ( int i = 0; i < target.Members.Count; i++ )
			{
				var count = i;

				if ( target.Members[ i ].Member == null
					// ReSharper disable once PossibleNullReferenceException
					|| ( target.IsConstructorDeserialization && !constructorArgumentsIndex.ContainsKey( target.Members[ i ].Contract.Name ) ) )
				{
					// just pop
					yield return
						this.EmitInvokeVoidMethod(
							context,
							context.Unpacker,
							Metadata._Unpacker.Read
						);
				}
				else
				{
					Func<TConstruct, TConstruct> storeValueStatementEmitter;
					if ( target.IsConstructorDeserialization )
					{
#if DEBUG && !UNITY
						Contract.Assert( constructorArgumentsIndex != null );
#endif // DEBUG && !UNITY
						var member = target.Members[ i ];
						storeValueStatementEmitter =
							unpackedItem =>
								this.EmitStoreVariableStatement( context, constructorArgumentsIndex[ member.Contract.Name ], unpackedItem );
					}
					else
					{
						storeValueStatementEmitter =
							unpackedItem =>
								this.EmitSetMemberValueStatement( context, result, target.Members[ count ].Member, unpackedItem );

					}

					yield return
						this.EmitUnpackItemValueExpression(
							context,
							target.Members[ count ].Member.GetMemberValueType(),
							target.Members[ count ].Contract.NilImplication,
							context.Unpacker,
							this.MakeInt32Literal( context, count ),
							this.MakeStringLiteral( context, target.Members[ count ].Member.ToString() ),
							itemsCount,
							unpacked,
							target.Members[ i ],
							null,
							storeValueStatementEmitter
						);
				}
			}

			if ( target.IsConstructorDeserialization )
			{
				yield return
					this.EmitInvokeDeserializationConstructorStatement( context, target.DeserializationConstructor, constructorArguments, result );
			}
		}

		private TConstruct EmitObjectUnpackFromMap( TContext context, TConstruct result, SerializationTarget target )
		{
			// TODO: Supports ExtensionObject like round-tripping.

			return
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					this.EmitObjectUnpackFromMapCore( context, result, target )
				);
		}

		private IEnumerable<TConstruct> EmitObjectUnpackFromMapCore( TContext context, TConstruct result, SerializationTarget target )
		{
			var itemsCount =
				this.DeclareLocal(
					context,
					typeof( int ),
					"itemsCount"
				);
			yield return itemsCount;

			yield return
				this.EmitStoreVariableStatement(
					context,
					itemsCount,
					this.EmitGetItemsCountExpression( context, context.Unpacker )
				);

			var constructorParameters =
				target.IsConstructorDeserialization
					? target.DeserializationConstructor.GetParameters()
					: null;
			var constructorArguments =
				target.IsConstructorDeserialization
				// ReSharper disable once PossibleNullReferenceException
					? new List<TConstruct>( constructorParameters.Length )
					: null;
			var constructorArgumentsIndex =
				target.IsConstructorDeserialization
				// ReSharper disable once PossibleNullReferenceException
					? new Dictionary<string, TConstruct>( constructorParameters.Length )
					: null;

			foreach (
				var construct in
					this.InitializeConstructorArgumentInitializationStatements(
						context,
						target,
						constructorParameters,
						constructorArguments,
						constructorArgumentsIndex ) )
			{
				yield return construct;
			}

			Func<TConstruct, SerializingMember, TConstruct> storeValueStatementEmitter;
			if ( target.IsConstructorDeserialization )
			{
#if DEBUG && !UNITY
				Contract.Assert( constructorArgumentsIndex != null );
#endif // DEBUG && !UNITY
				storeValueStatementEmitter =
					( unpackedValue, entry ) =>
						this.EmitStoreVariableStatement( context, constructorArgumentsIndex[ entry.Contract.Name ], unpackedValue );
			}
			else
			{
				storeValueStatementEmitter =
					( unpackedValue, entry ) =>
						this.EmitSetMemberValueStatement( context, result, entry.Member, unpackedValue );
			}

			yield return
				this.EmitForLoop(
					context,
					itemsCount,
					loopContext =>
					{
						var key =
							this.DeclareLocal(
								context,
								typeof( string ),
								"key"
							);
						var unpackKey =
							this.EmitUnpackItemValueExpression(
								context,
								typeof( string ),
								context.DictionaryKeyNilImplication,
								context.Unpacker,
								loopContext.Counter,
								this.MakeStringLiteral( context, "MemberName" ),
								null,
								null,
								null,
								null,
								unpackedKey =>
									this.EmitStoreVariableStatement( context, key, unpackedKey )
							);
						var assigns =
							this.EmitStringSwitchStatement(
								context,
								key,
								target.Members.Where( e => 
									e.Member != null
									// ReSharper disable once PossibleNullReferenceException
									&& ( !target.IsConstructorDeserialization || constructorArgumentsIndex.ContainsKey( e.Contract.Name ) )
								).ToDictionary(
									entry => entry.Contract.Name,
									entry =>
										this.EmitUnpackItemValueExpression(
											context,
											entry.Member.GetMemberValueType(),
											entry.Contract.NilImplication,
											context.Unpacker,
											loopContext.Counter,
											this.MakeStringLiteral( context, entry.Member.ToString() ),
											null,
											null,
											entry,
											null,
											unpackedValue => storeValueStatementEmitter( unpackedValue, entry )
										)
									),
									this.EmitInvokeVoidMethod(
										context,
										context.Unpacker,
										typeof( Unpacker ).GetMethod( "Skip" )
									)
								);

						return this.EmitSequentialStatements( context, typeof( void ), key, unpackKey, assigns );
					}
				);

			if ( target.IsConstructorDeserialization )
			{
				yield return
					this.EmitInvokeDeserializationConstructorStatement( context, target.DeserializationConstructor, constructorArguments, result );
			}
		}

		private IEnumerable<TConstruct> InitializeConstructorArgumentInitializationStatements( TContext context, SerializationTarget target, ParameterInfo[] constructorParameters, List<TConstruct> constructorArguments, Dictionary<string, TConstruct> constructorArgumentsIndex )
		{
			if ( !target.IsConstructorDeserialization )
			{
				// not constructor deserialization
				yield break;
			}

			for ( var i = 0; i < constructorParameters.Length; i++ )
			{
				var argument =
					this.DeclareLocal(
						context,
						constructorParameters[ i ].ParameterType,
						"ctorArg" + i
						);

				yield return argument;

				constructorArguments.Add( argument );
				var correspondingMemberName = target.FindCorrespondingMemberName( constructorParameters[ i ] );
				if ( correspondingMemberName != null )
				{
					constructorArgumentsIndex.Add( correspondingMemberName, argument );
				}

				yield return this.EmitStoreVariableStatement( context, argument, this.MakeDefaultParameterValueLiteral( context, argument, constructorParameters[ i ].ParameterType, constructorParameters[ i ].DefaultValue, constructorParameters[ i ].GetHasDefaultValue() ) );
			}
		}

		private TConstruct EmitInvokeDeserializationConstructorStatement( TContext context, ConstructorInfo constructor, IList<TConstruct> constructorArguments, TConstruct resultVariable )
		{
			return
				this.EmitStoreVariableStatement(
					context,
					resultVariable,
					this.EmitCreateNewObjectExpression(
						context,
						resultVariable,
						constructor,
						constructorArguments.ToArray()
					)
				);
		}
	}
}
