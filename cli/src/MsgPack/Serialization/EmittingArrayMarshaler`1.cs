#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.Globalization;
using System.Reflection;
using System.Linq;

namespace MsgPack.Serialization
{
	internal sealed class EmittingArrayMarshaler<TCollection> : ArrayMarshaler<TCollection>
	{
		private static readonly Type[] _marshalingMethodParameters = new[] { typeof( Packer ), typeof( TCollection ), typeof( SerializationContext ) };
		private static readonly Type[] _unmarshalingMethodParameters = new[] { typeof( Unpacker ), typeof( TCollection ), typeof( SerializationContext ) };
		private static readonly PropertyInfo _unpackerItemsCountProperty = FromExpression.ToProperty( ( Unpacker unpadker ) => unpadker.ItemsCount );
		private static readonly MethodInfo _unpackerMoveToNextEntryMethod = FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.MoveToNextEntry() );
		private static readonly MethodInfo _packerPackArrayHeader = FromExpression.ToMethod( ( Packer packer, int length ) => packer.PackArrayHeader( length ) );
		private static readonly MethodInfo _enumerableToArray1Method = typeof( Enumerable ).GetMethod( "ToArray" );
		private readonly Action<Packer, TCollection, SerializationContext> _marshaling;
		private readonly Action<Unpacker, TCollection, SerializationContext> _unmarshaling;

		public EmittingArrayMarshaler()
		{
			CreateArrayProcedures( out this._marshaling, out this._unmarshaling );
		}

		protected sealed override void MarshalCore( Packer packer, TCollection collection, SerializationContext context )
		{
			this._marshaling( packer, collection, context );
		}

		protected sealed override void UnmarshalCore( Unpacker unpacker, TCollection collection, SerializationContext context )
		{
			this._unmarshaling( unpacker, collection, context );
		}

		private static void CreateArrayProcedures( out Action<Packer, TCollection, SerializationContext> marshaling, out Action<Unpacker, TCollection, SerializationContext> unmarshaling )
		{
			var traits = typeof( TCollection ).GetCollectionTraits();
			if ( traits.CollectionType != CollectionKind.Array )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not list.", typeof( TCollection ) ) );
			}

			if ( traits.AddMethod == null )
			{
				throw SerializationExceptions.NewMissingAddMethod( typeof( TCollection ) );
			}

			marshaling = CreatePackMarshalProcedure( traits );
			unmarshaling = CreateUnpackArrayProceduresCore( traits );
		}

		private static Action<Packer, TCollection, SerializationContext> CreatePackMarshalProcedure( CollectionTraits traits )
		{
			/*
			 * context.Marshalers.Get<T[]>().Marshal( packer, collection );
			 */
			var dynamicMethod = SerializationMethodGeneratorManager.Get().CreateGenerator( "Marshal", typeof( TCollection ), "Items", null, _marshalingMethodParameters );
			var il = dynamicMethod.GetILGenerator();
			// Array
			if ( typeof( TCollection ).IsArray )
			{
				/*
				 * // array
				 *  packer.PackArrayHeader( length );
				 * for( int i = 0; i < length; i++ )
				 * {
				 * 		Context.Serializers.Get<T>().Serialize( packer, collection[ i ] );
				 * }
				 */
				var length = il.DeclareLocal( typeof( int ), "length" );
				il.EmitLdarg( 1 );
				il.EmitLdlen();
				il.EmitAnyStloc( length );
				il.EmitAnyLdarg( 0 );
				il.EmitAnyLdloc( length );
				il.EmitAnyCall( _packerPackArrayHeader );
				il.EmitPop();
				Emittion.EmitFor(
					il,
					length,
					( il0, i ) =>
					{
						Emittion.EmitMarshalValue(
							il0,
							0,
							2,
							traits.ElementType,
							il1 =>
							{
								il1.EmitAnyLdarg( 1 );
								il1.EmitAnyLdloc( i );
								il1.EmitLdelem( traits.ElementType );
							}
						);
					}
				);
			}
			else if ( traits.CountProperty == null )
			{
				/*
				 *  array = collection.ToArray();
				 *  packer.PackArrayHeader( length );
				 * for( int i = 0; i < length; i++ )
				 * {
				 * 		Context.Serializers.Get<T>().Serialize( packer, array[ i ] );
				 * }
				 */
				var array = il.DeclareLocal( traits.ElementType.MakeArrayType(), "array" );
				il.EmitLdarg( 1 );
				il.EmitAnyCall( _enumerableToArray1Method.MakeGenericMethod( traits.ElementType ) );
				il.EmitAnyStloc( array );
				var length = il.DeclareLocal( typeof( int ), "length" );
				il.EmitAnyLdloc( array );
				il.EmitLdlen();
				il.EmitAnyStloc( length );
				il.EmitAnyLdarg( 0 );
				il.EmitAnyLdloc( length );
				il.EmitAnyCall( _packerPackArrayHeader );
				il.EmitPop();
				Emittion.EmitFor(
					il,
					length,
					( il0, i ) =>
					{
						Emittion.EmitMarshalValue(
							il0,
							0,
							2,
							traits.ElementType,
							il1 =>
							{
								il1.EmitAnyLdloc( array );
								il1.EmitAnyLdloc( i );
								il1.EmitLdelem( traits.ElementType );
							}
						);
					}
				);
			}
			else
			{
				/*
				 * // Enumerable
				 *  packer.PackArrayHeader( collection.Count );
				 * foreach( var item in list )
				 * {
				 * 		Context.MarshalTo( packer, array[ i ] );
				 * }
				 */
				var collection = il.DeclareLocal( typeof( TCollection ), "collection" );
				il.EmitAnyLdarg( 1 );
				il.EmitAnyStloc( collection );
				var count = il.DeclareLocal( typeof( int ), "count" );
				il.EmitLdarg( 1 );
				il.EmitGetProperty( traits.CountProperty );
				il.EmitAnyStloc( count );
				il.EmitAnyLdarg( 0 );
				il.EmitAnyLdloc( count );
				il.EmitAnyCall( _packerPackArrayHeader );
				il.EmitPop();
				Emittion.EmitForEach(
					il,
					traits,
					collection,
					( il0, getCurrentEmitter ) =>
					{
						Emittion.EmitMarshalValue(
							il0,
							0,
							2,
							traits.ElementType,
							_ => getCurrentEmitter()
						);
					}
				);
			}
			il.EmitRet();

			return dynamicMethod.CreateDelegate<Action<Packer, TCollection, SerializationContext>>();
		}

		private static Action<Unpacker, TCollection, SerializationContext> CreateUnpackArrayProceduresCore( CollectionTraits traits )
		{
			/*
			 * int itemCount = unpacker.ItemCount;
			 * for( int i = 0; i < array.Length; i++ )
			 * {
			 *		if( !unpacker.MoveToNextEntry() )
			 *		{
			 *			throw new SerializationException();
			 *		}
			 *		collection.Add( Context.Serializers.Get<T>().Deserialize( unpacker ) )
			 * }
			 */
			var dynamicMethod = SerializationMethodGeneratorManager.Get().CreateGenerator( "Unmarshal", typeof( TCollection ), "Items", null, _unmarshalingMethodParameters );
			var il = dynamicMethod.GetILGenerator();
			var itemsCount = il.DeclareLocal( typeof( int ), "itemsCount" );

			Emittion.EmitReadUnpackerIfNotInHeader( il, 0 );
			il.EmitAnyLdarg( 0 );
			il.EmitGetProperty( _unpackerItemsCountProperty );
			il.EmitConv_Ovf_I4();
			il.EmitAnyStloc( itemsCount );
			Emittion.EmitFor(
				il,
				itemsCount,
				( il0, i ) =>
				{
					il0.EmitAnyLdarg( 1 );
					if ( typeof( TCollection ).IsArray )
					{
						il0.EmitAnyLdloc( i );
					}

					Emittion.EmitUnmarshalValue(
						il0,
						0,
						2,
						traits.ElementType,
						( il1, unpackerIndex ) =>
						{
							il1.EmitAnyLdarg( unpackerIndex );
							il1.EmitAnyCall( _unpackerMoveToNextEntryMethod );
							var endIf = il1.DefineLabel( "END_IF" );
							il1.EmitBrtrue_S( endIf );
							il1.EmitAnyLdloc( i );
							il1.EmitAnyCall( SerializationExceptions.NewMissingItemMethod );
							il1.EmitThrow();
							il1.MarkLabel( endIf );
						}
					);
					if ( typeof( TCollection ).IsArray )
					{
						il0.EmitStelem( traits.ElementType );
					}
					else
					{
						il0.EmitAnyCall( traits.AddMethod );
						if ( traits.AddMethod.ReturnType != typeof( void ) )
						{
							il0.EmitPop();
						}
					}
				}
			);
			il.EmitRet();

			return dynamicMethod.CreateDelegate<Action<Unpacker, TCollection, SerializationContext>>();
		}

	}
}
