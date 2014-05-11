#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private void BuildArraySerializer( TContext context, CollectionTraits traits )
		{
			this.BuildCollectionPackTo( context, traits );
			this.BuildCollectionUnpackFrom( context, traits );
			if ( traits.AddMethod != null )
			{
				this.BuildCollectionUnpackTo( context, traits );
			}
		}

		private void BuildCollectionPackTo( TContext context, CollectionTraits traits )
		{
#if DEBUG
			Contract.Assert( !typeof( TObject ).IsArray );
#endif
			this.EmitMethodPrologue( context, SerializerMethod.PackToCore );
			TConstruct construct = null;
			try
			{
				if ( traits.CountProperty == null )
				{
					// IEnumerable but not ICollection
					var arrayType = traits.ElementType.MakeArrayType();
					construct =
						this.EmitInvokeMethodExpression(
							context,
							this.EmitGetSerializerExpression( context, arrayType, null ),
							typeof( MessagePackSerializer<> ).MakeGenericType( arrayType ).GetMethod( "PackTo" ),
							context.Packer,
							this.EmitInvokeEnumerableToArrayExpression( context, context.PackToTarget, traits.ElementType )
						);
				}
				else
				{
					// ICollection
					construct =
						this.EmitSequentialStatements(
							context,
							typeof( void ),
							this.EmitPutArrayHeaderExpression(
								context,
								this.EmitGetCollectionCountExpression( context, context.PackToTarget, traits )
							),
							this.EmitForEachLoop(
								context,
								traits,
								context.PackToTarget,
								item =>
									this.EmitSequentialStatements(
										context,
										typeof( void ),
										this.EmitPackItemStatements( context, context.Packer, traits.ElementType, NilImplication.Null, null, item, null )
									)
							)
						);
				}
			}
			finally
			{
				this.EmitMethodEpilogue( context, SerializerMethod.PackToCore, construct );
			}
		}

		private TConstruct EmitPutArrayHeaderExpression( TContext context, TConstruct length )
		{
			return
				this.EmitInvokeVoidMethod( context, context.Packer, Metadata._Packer.PackArrayHeader, length );
		}

		private TConstruct EmitInvokeEnumerableToArrayExpression( TContext context, TConstruct enumerable, Type elementType )
		{
			return this.EmitInvokeMethodExpression( context, null, Metadata._Enumerable.ToArray1Method.MakeGenericMethod( elementType ), enumerable );
		}

		private void BuildCollectionUnpackFrom( TContext context, CollectionTraits traits )
		{
			this.EmitMethodPrologue( context, SerializerMethod.UnpackFromCore );

			TConstruct construct = null;
			try
			{
				Type instanceType;
				if ( typeof( TObject ).GetIsInterface() || typeof( TObject ).GetIsAbstract() )
				{
					instanceType = context.SerializationContext.DefaultCollectionTypes.GetConcreteType( typeof( TObject ) );
					if ( instanceType == null )
					{
						throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( typeof( TObject ) );
					}
				}
				else
				{
					instanceType = typeof( TObject );
				}

				/*
				 *	if (!unpacker.IsArrayHeader)
				 *	{
				 *		throw SerializationExceptions.NewIsNotArrayHeader();
				 *	}
				 *	int capacity = ITEMS_COUNT(unpacker);
				 *	TCollection collection = new ...;
				 *	this.UnpackToCore(unpacker, array);
				 *	return collection;
				 */
				var collection =
					this.DeclareLocal(
						context,
						instanceType,
						"collection"
					);
				var ctor = GetCollectionConstructor( instanceType );
				var collectionCapacity = this.EmitGetItemsCountExpression( context, context.Unpacker );

				construct =
					this.EmitSequentialStatements(
						context,
						collection.ContextType,
						this.EmitCheckIsArrayHeaderExpression( context, context.Unpacker ),
						collection,
						traits.AddMethod == null
						? this.EmitSequentialStatements(
							context,
							typeof( void ),
							this.EmitStoreVariableStatement(
								context,
								collection,
								this.EmitCreateNewObjectExpression(
									context,
									collection,
									ctor,
									ctor.GetParameters().Length == 0
									? NoConstructs
									: new[] { collectionCapacity }
								)
							),
							this.EmitUnpackToSpecifiedCollection( context, instanceType.GetCollectionTraits(), context.Unpacker, collection )
						) : this.EmitUnpackCollectionWithUnpackToExpression(
							context,
							ctor,
							collectionCapacity,
							context.Unpacker,
							collection
						),
						this.EmitRetrunStatement(
							context,
							this.EmitLoadVariableExpression( context, collection )
						)
					);
			}
			finally
			{
				this.EmitMethodEpilogue( context, SerializerMethod.UnpackFromCore, construct );
			}
		}

		private void BuildCollectionUnpackTo( TContext context, CollectionTraits traits )
		{
			/*
				int count = GetItemsCount( unpacker );
				for ( int i = 0; i < count; i++ )
				{
					...
				}
			 */

			this.EmitMethodPrologue( context, SerializerMethod.UnpackToCore );
			TConstruct construct = null;
			try
			{
				construct = this.EmitUnpackToSpecifiedCollection( context, traits, context.Unpacker, context.UnpackToTarget );
			}
			finally
			{
				this.EmitMethodEpilogue( context, SerializerMethod.UnpackToCore, construct );
			}
		}

		private TConstruct EmitUnpackToSpecifiedCollection(
			TContext context,
			CollectionTraits traitsOfTheCollection,
			TConstruct unpacker,
			TConstruct collection
		)
		{
			var count =
				this.DeclareLocal(
					context,
					typeof( int ),
					"count"
				);
			return
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					this.EmitCheckIsArrayHeaderExpression( context, context.Unpacker ),
					count,
					this.EmitStoreVariableStatement(
						context,
						count,
						this.EmitGetItemsCountExpression( context, context.Unpacker )
					),
					this.EmitForLoop(
						context,
						count,
						flc => this.EmitUnpackToCollectionLoopBody( context, flc, traitsOfTheCollection, unpacker, collection )
					)
				);
		}

		private TConstruct EmitUnpackToCollectionLoopBody( TContext context, ForLoopContext forLoopContext, CollectionTraits traitsOfTheCollection, TConstruct unpacker, TConstruct collection )
		{
			/*
			    if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				T item;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					item = serializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						item = serializer.UnpackFrom( subtreeUnpacker );
					}
				}

				addition( item );
			 */

			return
				this.EmitUnpackItemValueExpression(
					context,
					traitsOfTheCollection.ElementType,
					context.CollectionItemNilImplication,
					unpacker,
					forLoopContext.Counter,
					this.EmitInvariantStringFormat( context, "item{0}", forLoopContext.Counter ),
					null,
					null,
					null,
					unpackedItem =>
						this.EmitAppendCollectionItem(
							context,
							null,
							traitsOfTheCollection,
							collection,
							unpackedItem
						)
				);
		}

		private TConstruct EmitCheckIsArrayHeaderExpression( TContext context, TConstruct unpacker )
		{
			return
				this.EmitConditionalExpression(
					context,
					this.EmitNotExpression(
						context,
						this.EmitGetPropretyExpression( context, unpacker, Metadata._Unpacker.IsArrayHeader )
					),
					this.EmitThrowExpression(
						context,
						typeof( Unpacker ),
						SerializationExceptions.NewIsNotArrayHeaderMethod
					),
					null
				);
		}
	}
}
