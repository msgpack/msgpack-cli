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
using System.Collections;
using System.Linq;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private void BuildMapSerializer( TContext context, CollectionTraits traits )
		{
			this.BuildMapPackTo( context, traits );
			this.BuildMapUnpackFrom( context );
			this.BuildMapUnpackTo( context, traits );
		}

		private void BuildMapPackTo( TContext context, CollectionTraits traits )
		{
			/*
			 * 	int count = ((ICollection<KeyValuePair<string, DateTime>>)dictionary).Count;
			 * 	packer.PackMapHeader(count);
			 * 	foreach (KeyValuePair<string, DateTime> current in dictionary)
			 * 	{
			 * 		this._serializer0.PackTo(packer, current.Key);
			 * 		this._serializer1.PackTo(packer, current.Value);
			 * 	}
			 */
			this.EmitMethodPrologue( context, SerializerMethod.PackToCore );
			TConstruct construct = null;
			try
			{
				construct =
					this.EmitSequentialStatements(
						context,
						typeof( void ),
						this.EmitPutMapHeaderExpression(
							context, this.EmitGetCollectionCountExpression( context, context.PackToTarget, traits )
						),
						this.EmitForEachLoop(
							context,
							traits,
							context.PackToTarget,
							keyValuePair =>
								this.EmitPackKeyValuePair(
									context,
									traits,
									context.Packer,
									keyValuePair
								)
						)
					);
			}
			finally
			{
				this.EmitMethodEpilogue( context, SerializerMethod.PackToCore, construct );
			}
		}

		private TConstruct EmitPutMapHeaderExpression( TContext context, TConstruct collectionCount )
		{
			return
				this.EmitInvokeVoidMethod( context, context.Packer, Metadata._Packer.PackMapHeader, collectionCount );
		}

		private TConstruct EmitPackKeyValuePair( TContext context, CollectionTraits traits, TConstruct packer, TConstruct keyValuePair )
		{
			/*
			 *	this._serializer0.PackTo(packer, current.Key);
			 * 	this._serializer1.PackTo(packer, current.Value);
			 */

			Type keyType, valueType;
			GetDictionaryKeyValueType( traits.ElementType, out keyType, out valueType );

			return
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					this.EmitPackItemStatements(
						context,
						packer,
						keyType,
						NilImplication.Null,
						null,
						this.EmitGetPropretyExpression( context, keyValuePair, traits.ElementType.GetProperty( "Key" ) ),
						null
					).Concat(
						this.EmitPackItemStatements(
							context,
							packer,
							valueType,
							NilImplication.Null,
							null,
							this.EmitGetPropretyExpression( context, keyValuePair, traits.ElementType.GetProperty( "Value" ) ),
							null
						)
					)
				);
		}

		private void BuildMapUnpackFrom( TContext context )
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
				 *	if (!unpacker.IsMapHeader)
				 *	{
				 *		throw SerializationExceptions.NewIsNotMapHeader();
				 *	}
				 *	int capacity = ITEMS_COUNT(unpacker);
				 *	
				 *	TDictionary<TKey, TValue> dictionary = new ...;
				 *	this.UnpackToCore(unpacker, dictionary);
				 *	return dictionary;
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
						typeof( TObject ),
						this.EmitCheckIsMapHeaderExpression( context, context.Unpacker ),
						collection,
						this.EmitUnpackCollectionWithUnpackToExpression(
							context,
							GetCollectionConstructor( instanceType ),
							this.EmitGetItemsCountExpression( context, context.Unpacker ),
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

		private void BuildMapUnpackTo( TContext context, CollectionTraits traits )
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
				var itemsCount =
					this.DeclareLocal(
						context,
						typeof( int ),
						"itemsCount"
					);

				construct =
					this.EmitSequentialStatements(
						context,
						typeof( void ),
						this.EmitCheckIsMapHeaderExpression( context, context.Unpacker ),
						itemsCount,
						this.EmitStoreVariableStatement(
							context,
							itemsCount,
							this.EmitGetItemsCountExpression( context, context.Unpacker )
						),
						this.EmitForLoop(
							context,
							itemsCount,
							flc => this.EmitUnpackToMapLoopBody( context, flc, traits, context.Unpacker )
						)
					);
			}
			finally
			{
				this.EmitMethodEpilogue( context, SerializerMethod.UnpackToCore, construct );
			}
		}

		private TConstruct EmitUnpackToMapLoopBody( TContext context, ForLoopContext forLoopContext, CollectionTraits traits, TConstruct unpacker )
		{
			/*
					if ( !unpacker.Read() )
					{
						throw SerializationExceptions.NewMissingItem( i );
					}

					TKey key;
					if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
					{
						key = keySerializer.UnpackFrom( unpacker );
					}
					else
					{
						using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
						{
							key = keySerializer.UnpackFrom( subtreeUnpacker );
						}
					}


					if ( !unpacker.Read() )
					{
						throw SerializationExceptions.NewMissingItem( i );
					}

					TValue value;
					if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
					{
						value = valueSerializer.UnpackFrom( unpacker );
					}
					else
					{
						using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
						{
							value = valueSerializer.UnpackFrom( subtreeUnpacker );
						}
					}

					dictionary.Add( key, value );
			 */

			Type keyType, valueType;
			GetDictionaryKeyValueType( traits.ElementType, out keyType, out valueType );

			var key = this.DeclareLocal( context, keyType, "key" );
			var value = this.DeclareLocal( context, valueType, "value" );
			// ReSharper disable ImplicitlyCapturedClosure
			return
				this.EmitSequentialStatements(
					context,
					typeof( void ),
					key,
					value,
					this.EmitUnpackItemValueExpression(
						context,
						keyType,
						context.DictionaryKeyNilImplication,
						unpacker,
						forLoopContext.Counter,
						this.EmitInvariantStringFormat( context, "key{0}", forLoopContext.Counter ),
						null,
						null,
						null,
						unpackedKey =>
							this.EmitStoreVariableStatement(
								context,
								key,
								unpackedKey
							)
					),
					this.EmitUnpackItemValueExpression(
						context,
						valueType,
						context.CollectionItemNilImplication,
						unpacker,
						forLoopContext.Counter,
						this.EmitInvariantStringFormat( context, "value{0}", forLoopContext.Counter ),
						null,
						null,
						null,
						unpackedValue =>
							this.EmitStoreVariableStatement(
								context,
								value,
								unpackedValue
							)
					),
					this.EmitAppendDictionaryItem(
						context,
						traits,
						context.UnpackToTarget,
						keyType,
						key,
						valueType,
						value,
						traits.ElementType == typeof( DictionaryEntry )
					)
				);
			// ReSharper restore ImplicitlyCapturedClosure
		}

		private TConstruct EmitCheckIsMapHeaderExpression( TContext context, TConstruct unpacker )
		{
			return
				this.EmitConditionalExpression(
					context,
					this.EmitNotExpression(
						context,
						this.EmitGetPropretyExpression( context, unpacker, Metadata._Unpacker.IsMapHeader )
					),
					this.EmitThrowExpression(
						context,
						typeof( Unpacker ),
						SerializationExceptions.NewIsNotMapHeaderMethod
					),
					null
				);
		}
	}
}
