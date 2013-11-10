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
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private void BuildArraySerializer( TContext context, CollectionTraits traits )
		{
			this.BuildCollectionPackTo( context, traits );
			this.BuildCollectionUnpackFrom( context );
			this.BuildCollectionUnpackTo( context, traits );
		}

		private void BuildCollectionPackTo( TContext context, CollectionTraits traits )
		{
#if DEBUG
			Contract.Assert( !typeof( TObject ).IsArray );
#endif
			this.BeginPackToMethod( context );
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
							this.EmitGetSerializerExpression( context, arrayType ),
							typeof( MessagePackSerializer<> ).MakeGenericType( arrayType ).GetMethod( "PackTo" ),
							context.Packer,
							this.EmitInvokeEnumerableToArrayExpression( context, context.PackingTarget, traits.ElementType )
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
								this.EmitGetCollectionCountExpression( context, context.PackingTarget, traits )
							),
							this.EmitForEachLoop(
								context,
								traits,
								context.PackingTarget,
								item =>
									this.EmitPackItemExpression( context, context.Packer, traits.ElementType, NilImplication.Null, null, item )
							)
						);
				}
			}
			finally
			{
				this.EndPackToMethod( context, construct );
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

		private void BuildCollectionUnpackFrom( TContext context )
		{
			this.BeginUnpackFromMethod( context );

			TConstruct construct = null;
			try
			{
				Type instanceType;
				if ( typeof( TObject ).IsInterface || typeof( TObject ).IsAbstract )
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
						typeof( TObject ),
						"collection"
					);

				construct =
					this.EmitSequentialStatements(
						context,
						collection.ContextType,
						this.EmitCheckIsArrayHeaderExpression( context, context.Unpacker ),
						collection,
						this.EmitUnpackCollectionWithUnpackToExpression(
							context,
							GetCollectionConstructor( instanceType ),
							this.EmitGetItemsCountExpression( context, context.Unpacker ),
							context.Unpacker,
							collection
						),
						this.EmitLoadVariableExpression( context, collection )
					);
			}
			finally
			{
				this.EndUnpackFromMethod( context, construct );
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

			this.BeginUnpackToMethod( context );
			TConstruct construct = null;
			try
			{
				var count =
					this.DeclareLocal(
						context,
						typeof( int ),
						"count"
					);

				construct =
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
							flc => this.EmitUnpackToCollectionLoopBody( context, flc, traits, context.Unpacker )
						)
					);
			}
			finally
			{
				this.EndUnpackToMethod( context, construct );
			}
		}

		private TConstruct EmitUnpackToCollectionLoopBody( TContext context, ForLoopContext forLoopContext, CollectionTraits traits, TConstruct unpacker )
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
					traits.ElementType,
					context.CollectionItemNilImplication,
					unpacker,
					forLoopContext.Counter,
					this.EmitInvariantStringFormat( context, "item{0}", forLoopContext.Counter ),
					null,
					null,
					unpackedItem =>
						this.EmitAppendCollectionItem(
							context,
							null,
							traits,
							context.UnpackToTarget,
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
					unpacker
				);
		}
	}
}
