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
using System.Diagnostics.Contracts;

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
			this.BeginPackToMethod( context );
			TConstruct construct = null;
			try
			{
				construct =
					this.EmitSequentialStatements(
						context,
						this.EmitPutMapHeaderExpression(
							context, this.EmitGetCollectionCountExpression( context, context.PackingTarget, traits )
						),
						this.EmitForEachLoop(
							context,
							traits,
							context.PackingTarget,
							context.Packer,
							( kvp, packer ) =>
								this.EmitPackKeyValuePair(
									context,
									traits,
									packer,
									kvp
								)
						)
					);
			}
			finally
			{
				this.EndPackToMethod( context, construct );
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
					this.EmitPackItemExpression(
						context,
						packer,
						keyType,
						NilImplication.Null,
						null,
						this.EmitGetPropretyExpression( context, keyValuePair, traits.ElementType.GetProperty( "Key" ) )
					),
					this.EmitPackItemExpression(
						context,
						packer,
						valueType,
						NilImplication.Null,
						null,
						this.EmitGetPropretyExpression( context, keyValuePair, traits.ElementType.GetProperty( "Value" ) )
					)
				);
		}

		private void BuildMapUnpackFrom( TContext context )
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

				if ( construct == null )
				{
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

					construct =
						this.EmitSequentialStatements(
							context,
							this.EmitCheckIsMapHeaderExpression( context, context.Unpacker ),
							this.EmitUnpackCollectionWithUnpackToExpression(
								context,
								GetCollectionConstructor( instanceType ),
								this.EmitGetItemsCountExpression( context, context.Unpacker ),
								context.Unpacker
							)
						);
				}
			}
			finally
			{
				this.EndUnpackFromMethod( context, construct );
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
			this.BeginUnpackToMethod( context );
			TConstruct construct = null;
			try
			{
				construct =
					this.EmitForLoop(
						context,
						this.EmitGetItemsCountExpression( context, context.Unpacker ),
						context.Unpacker,
						flc => this.EmitUnpackToMapLoopBody( context, flc, traits, context.Unpacker )
					);
			}
			finally
			{
				this.EndUnpackToMethod( context, construct );
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

			var key = this.DeclareLocal( context, keyType, "key", null );
			var value = this.DeclareLocal( context, valueType, "value", null );
			return
				this.EmitSequentialStatements(
					context,
					key,
					value,
					this.EmitUnpackItemValueExpression(
						context,
						keyType,
						context.DictionaryKeyNilImplication,
						unpacker,
						context.UnpackToTarget,
						forLoopContext.Counter,
						this.EmitInvariantStringFormat( context, "key{0}", forLoopContext.Counter ),
						null,
						null,
						( ctx, ni, val ) => 
							this.EmitSetVariable( 
								context, 
								key,
								val
							)
					),
					this.EmitUnpackItemValueExpression(
						context,
						valueType,
						context.CollectionItemNilImplication,
						unpacker,
						context.UnpackToTarget,
						forLoopContext.Counter,
						this.EmitInvariantStringFormat( context, "value{0}", forLoopContext.Counter ),
						null,
						null,
						( ctx, ni, val ) => 
							this.EmitSetVariable( 
								context, 
								value, 
								val
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
					this.EmitThrow(
						context,
						typeof( Unpacker ),
						SerializationExceptions.NewIsNotMapHeaderMethod
					),
					unpacker
				);
		}

		private TConstruct EmitAppendDictionaryItem( TContext context, CollectionTraits traits, TConstruct dictionary, Type keyType, TConstruct key, Type valueType, TConstruct value, bool withBoxing )
		{
			return
				this.EmitInvokeVoidMethod(
					context,
					dictionary,
					traits.AddMethod,
					withBoxing
					? this.EmitBoxExpression( context, keyType, key )
					: key,
					withBoxing
					? this.EmitBoxExpression( context, valueType, value )
					: value
				);
		}

		private static void GetDictionaryKeyValueType( Type elementType, out Type keyType , out Type valueType )
		{
			if ( elementType == typeof( DictionaryEntry ) )
			{
				keyType = typeof( object );
				valueType = typeof( object );
			}
			else
			{
				keyType = elementType.GetGenericArguments()[ 0 ];
				valueType = elementType.GetGenericArguments()[ 1 ];
			}
		}
	}
}
