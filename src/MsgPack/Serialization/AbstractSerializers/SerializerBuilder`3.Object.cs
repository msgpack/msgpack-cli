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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private void BuildObjectSerializer( TContext context )
		{
			if ( typeof( TObject ).GetIsInterface() || typeof( TObject ).GetIsAbstract() )
			{
				throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( typeof( TObject ) );
			}

			var entries = SerializationTarget.GetTargetMembers( typeof( TObject ) ).OrderBy( item => item.Contract.Id ).ToArray();

			if ( entries.Length == 0 )
			{
				throw SerializationExceptions.NewNoSerializableFieldsException( typeof( TObject ) );
			}

			if ( entries.All( item => item.Contract.Id == DataMemberContract.UnspecifiedId ) )
			{
				// Alphabetical order.
				this.BuildObjectSerializer( context, entries.OrderBy( item => item.Contract.Name ).ToArray() );
				return;
			}

			// ID order.

			Contract.Assert( entries[ 0 ].Contract.Id >= 0 );

			if ( context.SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder && entries[ 0 ].Contract.Id == 0 )
			{
				throw new NotSupportedException( "Cannot specify order value 0 on DataMemberAttribute when SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder is set to true." );
			}

			var maxId = entries.Max( item => item.Contract.Id );
			var result = new List<SerializingMember>( maxId + 1 );
			for ( int source = 0, destination = context.SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder ? 1 : 0; source < entries.Length; source++, destination++ )
			{
				Contract.Assert( entries[ source ].Contract.Id >= 0 );

				if ( entries[ source ].Contract.Id < destination )
				{
					throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The member ID '{0}' is duplicated in the '{1}' elementType.", entries[ source ].Contract.Id, typeof( TObject ) ) );
				}

				while ( entries[ source ].Contract.Id > destination )
				{
					result.Add( new SerializingMember() );
					destination++;
				}

				result.Add( entries[ source ] );
			}

			this.BuildObjectSerializer( context, result.ToArray() );
		}

		private void BuildObjectSerializer( TContext context, SerializingMember[] entries )
		{
			VerifyNilImplication( entries );

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

		private void VerifyNilImplication( SerializingMember[] entries )
		{
			foreach ( var serializingMember in entries )
			{
				if ( serializingMember.Contract.NilImplication == NilImplication.Null )
				{
					var itemType = serializingMember.Member.GetMemberValueType();

					if ( itemType != typeof( MessagePackObject )
						&& itemType.GetIsValueType()
						&& Nullable.GetUnderlyingType( itemType ) == null )
					{
						throw SerializationExceptions.NewValueTypeCannotBeNull( serializingMember.Member.ToString(), itemType, typeof( TObject ) );
					}

					bool isReadOnly;
					FieldInfo asField;
					PropertyInfo asProperty;
					if ( ( asField = serializingMember.Member as FieldInfo ) != null )
					{
						isReadOnly = asField.IsInitOnly;
					}
					else
					{
						asProperty = serializingMember.Member as PropertyInfo;
#if DEBUG
						Contract.Assert( asProperty != null, serializingMember.Member.ToString() );
#endif
						isReadOnly = asProperty.GetSetMethod() == null;
					}

					if ( isReadOnly )
					{
						throw SerializationExceptions.NewNullIsProhibited( serializingMember.Member.ToString() );
					}
				}
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

		private void BuildObjectPackTo( TContext context, SerializingMember[] entries )
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

		private void BuildObjectUnpackFrom( TContext context, SerializingMember[] entries )
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
