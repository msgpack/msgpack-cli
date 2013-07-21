#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Linq;
using System.Linq.Expressions;
#if NETFX_CORE
using System.Reflection;
#endif
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.ExpressionSerializers
{
	internal class TupleExpressionMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly IMessagePackSerializer[] _itemSerializers;

		private readonly Action<Packer, T, IMessagePackSerializer[]> _packToCore;
		private readonly Func<Unpacker, IMessagePackSerializer[], T> _unpackFromCore;

		public TupleExpressionMessagePackSerializer( SerializationContext context )
			: base( ( context ?? SerializationContext.Default ).CompatibilityOptions.PackerCompatibilityOptions )
		{
			Contract.Assert( typeof( T ).FullName.StartsWith( "System.Tuple`", StringComparison.Ordinal ), typeof( T ) + " is not Tuple<...>" );
			var flattenTypes = TupleItems.GetTupleItemTypes( typeof( T ) );
			this._itemSerializers = flattenTypes.Select( t => context.GetSerializer( t ) ).ToArray();

			var packerParameter = Expression.Parameter( typeof( Packer ), "packer" );
			var objectTreeParameter = Expression.Parameter( typeof( T ), "objectTree" );
			var itemSerializersParameter = Expression.Parameter( typeof( IMessagePackSerializer[] ), "itemSerializers" );
			var itemSerializerTypes = flattenTypes.Select( t => typeof( MessagePackSerializer<> ).MakeGenericType( t ) ).ToArray();

			/*
			 *	packer.PackArrayHeader( arity );
			 *	serializer[ 0 ].PackTo( pack, tuple.Item1 );
			 *		:
			 *	serializer[ N ].PackTo( pack, tuple.Rest....ItemM );
			 */
			this._packToCore =
				Expression.Lambda<Action<Packer, T, IMessagePackSerializer[]>>(
					Expression.Block(
						new[] {
							Expression.Call(
								packerParameter,
								Metadata._Packer.PackArrayHeader,
								Expression.Constant( flattenTypes.Count )
							)
						}.Concat(
							CreatePackExpressions( packerParameter, objectTreeParameter, itemSerializersParameter, itemSerializerTypes, flattenTypes )
						).ToArray()
					),
					packerParameter, objectTreeParameter, itemSerializersParameter
				).Compile();

			var unpackerParameter = Expression.Parameter( typeof( Unpacker ), "unpacker" );

			/*
			 * 	checked
			 * 	{
			 *		if (!unpacker.Read())
			 *		{
			 *			throw SerializationExceptions.NewMissingItem(0);
			 *		}
			 *		
			 *		T1 item1;
			 *		if (!unpacker.IsArrayHeader && !unpacker.IsMapHeader)
			 *		{
			 *			item1 = this._serializer0.UnpackFrom(unpacker);
			 *		}
			 *		else
			 *		{
			 *			using (Unpacker subtreeUnpacker = unpacker.ReadSubtree())
			 *			{
			 *				item1 = this._serializer0.UnpackFrom(subtreeUnpacker);
			 *			}
			 *		}
			 *		
			 *		if (!unpacker.Read())
			 *			:
			 *		
			 *		return new Tuple<...>( item1, ... , new Tuple<...>(...)...);
			 *	}
			 */

			this._unpackFromCore =
				Expression.Lambda<Func<Unpacker, IMessagePackSerializer[], T>>(
					CreateNestedTupleCreationExpression(
						flattenTypes,
						CreateUnpackExpressions( unpackerParameter, itemSerializersParameter, itemSerializerTypes, flattenTypes ).ToArray()
					),
					unpackerParameter, itemSerializersParameter
				).Compile();
		}

		private Expression CreateNestedTupleCreationExpression( IList<Type> itemTypes, IEnumerable<Expression> itemExpressions )
		{
			var tupleTypeList = TupleItems.CreateTupleTypeList( itemTypes );

			Expression right = null;
			for ( int depth = tupleTypeList.Count - 1; 0 <= depth; depth-- )
			{
				var constructor = tupleTypeList[ depth ].GetConstructors().Single();
				var args = itemExpressions.Skip( depth * 7 ).Take( Math.Min( constructor.GetParameters().Length, 7 ) );
				if ( right != null )
				{
					args = args.Concat( new[] { right } );
				}

				right = Expression.New( constructor, args );
			}

			return right;
		}

		private IEnumerable<Expression> CreatePackExpressions( ParameterExpression packerParameter, ParameterExpression objectTreeParameter, ParameterExpression itemSerializersParameter, Type[] itemSerializerTypes, IList<Type> flattenTypes )
		{
			int number = 0;
			foreach ( var right in CreateTupleItemExpressions( objectTreeParameter, flattenTypes ) )
			{
				// serializer[ i ].PackTo( pack, <right> );
				yield return
					Expression.Call(
						Expression.TypeAs(
							Expression.ArrayIndex(
								itemSerializersParameter,
								Expression.Constant( number )
							),
							itemSerializerTypes[ number ]
						),
						itemSerializerTypes[ number ].GetMethod( "PackTo" ),
						packerParameter,
						right
					);
				number++;
			}
		}

		private IEnumerable<Expression> CreateUnpackExpressions( ParameterExpression unpackerParameter, ParameterExpression itemSerializersParameter, Type[] itemSerializerTypes, IList<Type> flattenTypes )
		{
			for ( int number = 0; number < flattenTypes.Count; number++ )
			{
				/*
				 * 	checked
				 * 	{
				 *		if (!unpacker.Read())
				 *		{
				 *			throw SerializationExceptions.NewMissingItem(0);
				 *		}
				 *		
				 *		T1 item1;
				 *		if (!unpacker.IsArrayHeader && !unpacker.IsMapHeader)
				 *		{
				 *			item1 = this._serializer0.UnpackFrom(unpacker);
				 *		}
				 *		else
				 *		{
				 *			using (Unpacker subtreeUnpacker = unpacker.ReadSubtree())
				 *			{
				 *				item1 = this._serializer0.UnpackFrom(subtreeUnpacker);
				 *			}
				 *		}
				 *		
				 *		if (!unpacker.Read())
				 *			:
				 *		
				 *		return new Tuple<...>( item1, ... , new Tuple<...>(...)...);
				 *	}
				 */
				// Express above statement as expressions like functional.
				yield return
					Expression.Block(
						flattenTypes[ number ],
						Expression.IfThen(
							Expression.IsFalse(
								Expression.Call(
									unpackerParameter,
									Metadata._Unpacker.Read
								)
							),
							Expression.Call(
								SerializationExceptions.NewMissingItemMethod,
								Expression.Constant( number )
							)
						),
						ExpressionSerializerLogics.CreateUnpackItem(
							unpackerParameter,
							Metadata._UnpackHelpers.InvokeUnpackFrom_1Method.MakeGenericMethod( flattenTypes[ number ] ),
							Expression.ArrayIndex(
								itemSerializersParameter,
								Expression.Constant( number )
							),
							itemSerializerTypes[ number ]
						)
					);
			}
		}
		
		private IEnumerable<Expression> CreateTupleItemExpressions( ParameterExpression objectTreeParameter, IList<Type> flattenTypes )
		{
			var tupleTypeList = TupleItems.CreateTupleTypeList( flattenTypes );

			int depth = -1;
			for ( int i = 0; i < flattenTypes.Count; i++ )
			{
				if ( i % 7 == 0 )
				{
					depth++;
				}

				Expression current = objectTreeParameter;
				for ( int j = 0; j < depth; j++ )
				{
					// .TRest.TRest ...
					var rest = tupleTypeList[ j ].GetProperty( "Rest" );
					current = Expression.Property( current, rest );
				}

				var itemn = tupleTypeList[ depth ].GetProperty( "Item" + ( ( i % 7 ) + 1 ) );
#if DEBUG
				Contract.Assert( itemn != null, tupleTypeList[ depth ].GetFullName() + "::Item" + ( ( i % 7 ) + 1 ) + " [ " + depth + " ] @ " + i );
#endif
				yield return Expression.Property( current, itemn );
			}
		}

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			this._packToCore( packer, objectTree, this._itemSerializers );
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			if ( ( int )unpacker.ItemsCount != this._itemSerializers.Length )
			{
				throw SerializationExceptions.NewTupleCardinarityIsNotMatch( this._itemSerializers.Length, ( int )unpacker.ItemsCount );
			}

			return this._unpackFromCore( unpacker, this._itemSerializers );
		}
	}
}
