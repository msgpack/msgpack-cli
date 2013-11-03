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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private void BuildTupleSerializer( TContext context )
		{
			var itemTypes = TupleItems.GetTupleItemTypes( typeof( TObject ) );

			this.BuildTuplePackTo( context, itemTypes );
			this.BuildTupleUnpackFrom( context, itemTypes );
		}

		private void BuildTuplePackTo( TContext context, IList<Type> itemTypes )
		{
			/*
				 packer.PackArrayHeader( cardinarity );
				 _serializer0.PackTo( packer, tuple.Item1 );
					:
				 _serializer6.PackTo( packer, tuple.item7 );
				 _serializer7.PackTo( packer, tuple.Rest.Item1 );
			*/

			this.BeginPackToMethod( context );
			TConstruct construct = null;
			try
			{
				construct =
					this.EmitSequentialStatements(
						context,
						this.BuildTuplePackToCore( context, itemTypes )
					);
			}
			finally
			{
				this.EndPackToMethod( context, construct );
			}
		}

		private IEnumerable<TConstruct> BuildTuplePackToCore( TContext context, IList<Type> itemTypes )
		{
			// Put cardinality as array length.
			yield return this.EmitPutArrayHeaderExpression( context, this.MakeInt32Literal( context, itemTypes.Count ) );
			var depth = -1;
			var tupleTypeList = TupleItems.CreateTupleTypeList( itemTypes );
			var propertyInvocationChain = new List<PropertyInfo>( itemTypes.Count % 7 + 1 );
			for ( int i = 0; i < itemTypes.Count; i++ )
			{
				if ( i % 7 == 0 )
				{
					depth++;
				}

				for ( int j = 0; j < depth; j++ )
				{
					// .TRest.TRest ...
					var restProperty = tupleTypeList[ j ].GetProperty( "Rest" );
#if DEBUG
					Contract.Assert( restProperty != null );
#endif
					propertyInvocationChain.Add( restProperty );
				}

				var itemNProperty = tupleTypeList[ depth ].GetProperty( "Item" + ( ( i % 7 ) + 1 ) );
				propertyInvocationChain.Add( itemNProperty );
#if DEBUG
				Contract.Assert(
					itemNProperty != null,
					tupleTypeList[ depth ].GetFullName() + "::Item" + ( ( i % 7 ) + 1 ) + " [ " + depth + " ] @ " + i );
#endif
				yield return
					this.EmitPackTupleItemExpression(
						context,
						itemTypes[ i ],
						context.Packer,
						context.PackingTarget,
						propertyInvocationChain
					);

				propertyInvocationChain.Clear();
			}
		}

		private TConstruct EmitPackTupleItemExpression(
			TContext context,
			Type itemType,
			TConstruct currentPacker,
			TConstruct tuple,
			IEnumerable<PropertyInfo> propertyInvocationChain )
		{
			return
				this.EmitPackItemExpression(
					context,
					currentPacker,
					itemType,
					NilImplication.Null,
					null,
					propertyInvocationChain.Aggregate(
						tuple, ( propertySource, property ) => this.EmitGetPropretyExpression( context, propertySource, property )
					)
				);
		}


		private void BuildTupleUnpackFrom( TContext context, IList<Type> itemTypes )
		{
			/*
			 * 	checked
			 * 	{
			 * 		if (!unpacker.IsArrayHeader)
			 * 		{
			 * 			throw SerializationExceptions.NewIsNotArrayHeader();
			 * 		}
			 * 		
			 * 		if ((int)unpacker.ItemsCount != n)
			 * 		{
			 * 			throw SerializationExceptions.NewTupleCardinarityIsNotMatch(n, (int)unpacker.ItemsCount);
			 * 		}
			 * 		
			 *		return 
			 *			new Tuple<...>( 
			 *				GET_VALUE_OR_DEFAULT( DESERIALIZE_VALUE( unpacker, typeof( T1? ) ) ),
			 *					:
			 *				GET_VALUE_OR_DEFAULT( DESERIALIZE_VALUE( unpacker, typeof( T7? ) ) ),
			 *				new Tuple<...>(
			 *					GET_VALUE_OR_DEFAULT( DESERIALIZE_VALUE( unpacker, typeof( T8? ) ) ),
			 *						:
			 *				)
			 *			);
			 *	}
			 */
			this.BeginUnpackFromMethod( context );
			var tupleTypeList = TupleItems.CreateTupleTypeList( itemTypes );
			TConstruct construct = null;
			try
			{
				var checkCardinarity =
					this.EmitCheckTupleCardinarityExpression(
						context,
						this.EmitCheckIsArrayHeaderExpression( context, context.Unpacker ),
						itemTypes.Count
					);

				var unpackedNullableItemTypes =
					itemTypes.Select(
						itemType =>
						( itemType != typeof( MessagePackObject ) &&
						  itemType.GetIsValueType() &&
						  Nullable.GetUnderlyingType( itemType ) == null )
							// Use temporary nullable value for nil implication.
							? typeof( Nullable<> ).MakeGenericType( itemType )
							: itemType
					).ToArray();

				var unpackedItems =
					itemTypes.Select(
						( type, i ) =>
						this.DeclareLocal(
							context,
							type,
							"item" + i,
							null
						)
					).ToArray();

				var unpackItems =
					unpackedNullableItemTypes.Select(
						( unpackedNullableItemType, i ) =>
							this.EmitUnpackItemValueExpression(
								context,
								unpackedNullableItemType,
								context.TupleItemNilImplication,
								context.Unpacker,
								null,
								this.MakeInt32Literal( context, i ),
								this.MakeStringLiteral( context, "Item" + i.ToString( CultureInfo.InvariantCulture ) ),
								null,
								null,
								( ctx, ni, val ) =>
									this.EmitSetVariable(
										context,
										unpackedItems[ i ],
										val
									)
							)
					);

				TConstruct currentTuple = null;
				for ( int nest = tupleTypeList.Count - 1; nest >= 0; nest-- )
				{
					var items = unpackedItems.Skip( nest * 7 ).Take( unpackedItems.Length - ( nest * 7 ) );
					currentTuple =
						this.EmitCreateNewObjectExpression(
							context,
							tupleTypeList[ nest ].GetConstructors().Single(),
							currentTuple == null
								? items.ToArray()
								: items.ToArray().Concat( new[] { currentTuple } ).ToArray()
							);
				}

#if DEBUG
				Contract.Assert( currentTuple != null );
#endif
				construct =
					this.EmitSequentialStatements(
						context,
						new[] { checkCardinarity }
						.Concat( unpackedItems )
						.Concat( unpackItems )
						.Concat( new[] { currentTuple } )
						.ToArray()
					);
			}
			finally
			{
				this.EndUnpackFromMethod( context, construct );
			}
		}

		private TConstruct EmitCheckTupleCardinarityExpression( TContext context, TConstruct unpacker, int cardinarity )
		{
			return
				this.EmitConditionalExpression(
					context,
					this.EmitNotEqualsExpression(
						context,
						this.EmitGetPropretyExpression( context, unpacker, Metadata._Unpacker.ItemsCount ),
						this.MakeInt64Literal( context, cardinarity )
					),
					this.EmitThrow(
						context,
						typeof( Unpacker ),
						SerializationExceptions.NewIsNotArrayHeaderMethod
					),
					unpacker
				);
		}
	}
}
