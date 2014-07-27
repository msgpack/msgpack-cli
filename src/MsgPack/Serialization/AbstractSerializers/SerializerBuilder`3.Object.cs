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
// Contributors:
//    Takeshi KIRIYA
//
#endregion -- License Terms --

using System;
using System.Collections.Generic;
using System.Linq;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private void BuildObjectSerializer( TContext context )
		{
			SerializationTarget.VerifyType( typeof( TObject ) );
			var entries = SerializationTarget.Prepare( context.SerializationContext, typeof( TObject ) );

			if ( typeof( IPackable ).IsAssignableFrom( typeof( TObject ) ) )
			{
				this.BuildIPackablePackTo( context );
			}
			else
			{
				this.BuildObjectPackTo( context, entries );
			}

			if ( typeof( IUnpackable ).IsAssignableFrom( typeof( TObject ) ) )
			{
				this.BuildIUnpackableUnpackFrom( context );
			}
			else
			{
				this.BuildObjectUnpackFrom( context, entries );
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

		private void BuildObjectPackTo( TContext context, IList<SerializingMember> entries )
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
						? this.BuildObjectPackToWithArray( context, entries )
						: this.BuildObjectPackToWithMap( context, entries )
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
							entries[ i ]
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
						entries[ i ]
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
						entries[ i ]
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

		private void BuildObjectUnpackFrom( TContext context, IList<SerializingMember> entries )
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
						this.BuildObjectUnpackFromCore( context, entries )
					);
			}
			finally
			{
				this.EmitMethodEpilogue( context, SerializerMethod.UnpackFromCore, construct );
			}
		}

		private IEnumerable<TConstruct> BuildObjectUnpackFromCore( TContext context, IList<SerializingMember> entries )
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
							null, // reference contextType
							GetDefaultConstructor( typeof( TObject ) )
						)
					);
			}

			yield return
				this.EmitConditionalExpression(
					context,
					this.EmitGetPropretyExpression( context, context.Unpacker, Metadata._Unpacker.IsArrayHeader ),
					this.EmitObjectUnpackFromArray( context, result, entries ),
					this.EmitObjectUnpackFromMap( context, result, entries )
				);

			yield return this.EmitRetrunStatement( context, this.EmitLoadVariableExpression( context, result ) );
		}

		private TConstruct EmitObjectUnpackFromArray( TContext context, TConstruct result, IList<SerializingMember> entries )
		{
			// TODO: Supports ExtensionObject like round-tripping.
			return
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					this.EmitObjectUnpackFromArrayCore( context, result, entries )
				);
		}

		private IEnumerable<TConstruct> EmitObjectUnpackFromArrayCore( TContext context, TConstruct result, IList<SerializingMember> entries )
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

			for ( int i = 0; i < entries.Count; i++ )
			{
				var count = i;

				if ( entries[ i ].Member == null )
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
					yield return
						this.EmitUnpackItemValueExpression(
							context,
							entries[ count ].Member.GetMemberValueType(),
							entries[ count ].Contract.NilImplication,
							context.Unpacker,
							this.MakeInt32Literal( context, count ),
							this.MakeStringLiteral( context, entries[ count ].Member.ToString() ),
							itemsCount,
							unpacked,
							entries[ i ],
							unpackedItem =>
								this.EmitSetMemberValueStatement( context, result, entries[ count ].Member, unpackedItem )
						);
				}
			}
		}

		private TConstruct EmitObjectUnpackFromMap( TContext context, TConstruct result, IList<SerializingMember> entries )
		{
			// TODO: Supports ExtensionObject like round-tripping.

			return
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					this.EmitObjectUnpackFromMapCore( context, result, entries )
				);
		}

		private IEnumerable<TConstruct> EmitObjectUnpackFromMapCore( TContext context, TConstruct result, IList<SerializingMember> entries )
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
								unpackedKey =>
									this.EmitStoreVariableStatement( context, key, unpackedKey )
							);
						var assigns =
							this.EmitStringSwitchStatement(
								context,
								key,
								entries.Where( e => e.Member != null ).ToDictionary(
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
											unpackedValue =>
												this.EmitSetMemberValueStatement( context, result, entry.Member, unpackedValue )
										)
								)
							);

						return this.EmitSequentialStatements( context, typeof( void ), key, unpackKey, assigns );
					}
				);
		}
	}
}
