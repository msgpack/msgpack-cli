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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using NLiblet.Reflection;

namespace MsgPack.Serialization
{
	// TODO: Move fully-non-generic members to non-generic helpers to reduce JIT
	/// <summary>
	///		<see cref="SerializerBuilder{T}"/> implementation using Reflection.Emit.
	/// </summary>
	/// <typeparam name="TObject">Object to be serialized/deserialized.</typeparam>
	internal sealed class EmittingSerializerBuilder<TObject> : SerializerBuilder<TObject>
	{
		public EmittingSerializerBuilder( SerializationContext context ) : base( context ) { }

		protected sealed override MessagePackSerializer<TObject> CreateSerializer( SerializingMember[] entries )
		{
			var emitter = SerializationMethodGeneratorManager.Get().CreateEmitter( typeof( TObject ) );

			var packerIL = emitter.GetPackToMethodILGenerator();
			try
			{
				Emittion.EmitPackMambers(
					emitter,
					packerIL,
					1,
					typeof( TObject ),
					2,
					entries.Select( item => Tuple.Create( item.Member, item.Member.GetMemberValueType() ) ).ToArray()
					);
				packerIL.EmitRet();
			}
			finally
			{
				packerIL.FlushTrace();
			}

			var unpackerIL = emitter.GetUnpackFromMethodILGenerator();
			try
			{
				// TODO: Array for ordered.
				// TODO: For big struct, use Dictionary<String,SM>
				// TODO: Required
				var locals = entries.Select( item => !IsReadOnlyAppendableCollectionMember( item.Member ) ? unpackerIL.DeclareLocal( item.Member.GetMemberValueType(), item.Contract.Name ) : null ).ToArray();

				var result = unpackerIL.DeclareLocal( typeof( TObject ), "result" );
				Emittion.EmitConstruction( unpackerIL, result, null );

				Emittion.EmitUnpackMembers(
					emitter,
					unpackerIL,
					1,
					result,
					entries.Zip(
						locals,
						( entry, local ) => new Tuple<MemberInfo, string, LocalBuilder, LocalBuilder>( entry.Member, entry.Contract.Name, local, default( LocalBuilder ) )
					).ToArray()
				);

				foreach ( var item in entries.Zip( locals, ( Entry, Local ) => new { Entry, Local } ) )
				{
					if ( item.Local == null )
					{
						continue;
					}

					if ( result.LocalType.IsValueType )
					{
						unpackerIL.EmitAnyLdloca( result );
					}
					else
					{
						unpackerIL.EmitAnyLdloc( result );
					}

					unpackerIL.EmitAnyLdloc( item.Local );
					Emittion.EmitStoreValue( unpackerIL, item.Entry.Member );
				}

				unpackerIL.EmitAnyLdloc( result );
				unpackerIL.EmitRet();
			}
			finally
			{
				unpackerIL.FlushTrace();
			}

			return emitter.CreateInstance<TObject>( this.Context );
		}

		private static bool IsReadOnlyAppendableCollectionMember( MemberInfo memberInfo )
		{
			if ( memberInfo.CanSetValue() )
			{
				// Not read only
				return false;
			}

			Type memberValueType = memberInfo.GetMemberValueType();
			if ( memberValueType.IsArray )
			{
				// Not appendable
				return false;
			}

			CollectionTraits traits = memberValueType.GetCollectionTraits();
			return traits.CollectionType != CollectionKind.NotCollection && traits.AddMethod != null;
		}

		public sealed override MessagePackSerializer<TObject> CreateArraySerializer()
		{
			return CreateArraySerializerCore( typeof( TObject ) ).CreateInstance<TObject>( this.Context );
		}

		private static SerializerEmitter CreateArraySerializerCore( Type collectionType )
		{
			var emitter = SerializationMethodGeneratorManager.Get().CreateEmitter( collectionType );
			var traits = collectionType.GetCollectionTraits();
			CreatePackArrayProceduresCore( emitter, collectionType, traits );
			CreateUnpackArrayProceduresCore( emitter, collectionType, traits );
			return emitter;
		}

		private static void CreatePackArrayProceduresCore( SerializerEmitter emitter, Type collectionType, CollectionTraits traits )
		{
			var il = emitter.GetPackToMethodILGenerator();
			try
			{
				// Array
				if ( typeof( TObject ).IsArray )
				{
					/*
					 * // array
					 *  packer.PackArrayHeader( length );
					 * for( int i = 0; i < length; i++ )
					 * {
					 * 		this._serializer.PackTo( packer, collection[ i ] );
					 * }
					 */
					var length = il.DeclareLocal( typeof( int ), "length" );
					il.EmitAnyLdarg( 2 );
					il.EmitLdlen();
					il.EmitAnyStloc( length );
					il.EmitAnyLdarg( 1 );
					il.EmitAnyLdloc( length );
					il.EmitAnyCall( Metadata._Packer.PackArrayHeader );
					il.EmitPop();
					Emittion.EmitFor(
						il,
						length,
						( il0, i ) =>
						{
							Emittion.EmitMarshalValue(
								emitter,
								il0,
								1,
								traits.ElementType,
								il1 =>
								{
									il1.EmitAnyLdarg( 2 );
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
					 * 		this._serializer.PackTo( packer, array[ i ] );
					 * }
					 */
					var array = il.DeclareLocal( traits.ElementType.MakeArrayType(), "array" );
					EmitLoadTarget( il, 2 );
					il.EmitAnyCall( Metadata._Enumerable.ToArray1Method.MakeGenericMethod( traits.ElementType ) );
					il.EmitAnyStloc( array );
					var length = il.DeclareLocal( typeof( int ), "length" );
					il.EmitAnyLdloc( array );
					il.EmitLdlen();
					il.EmitAnyStloc( length );
					il.EmitAnyLdarg( 1 );
					il.EmitAnyLdloc( length );
					il.EmitAnyCall( Metadata._Packer.PackArrayHeader );
					il.EmitPop();
					Emittion.EmitFor(
						il,
						length,
						( il0, i ) =>
						{
							Emittion.EmitMarshalValue(
								emitter,
								il0,
								1,
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
					 * 		this._serializer.PackTo( packer, array[ i ] );
					 * }
					 */
					var collection = il.DeclareLocal( collectionType, "collection" );
					// This instruction always ldarg, not to be ldarga
					il.EmitAnyLdarg( 2 );
					il.EmitAnyStloc( collection );
					var count = il.DeclareLocal( typeof( int ), "count" );
					EmitLoadTarget( il, 2 );
					il.EmitGetProperty( traits.CountProperty );
					il.EmitAnyStloc( count );
					il.EmitAnyLdarg( 1 );
					il.EmitAnyLdloc( count );
					il.EmitAnyCall( Metadata._Packer.PackArrayHeader );
					il.EmitPop();
					Emittion.EmitForEach(
						il,
						traits,
						collection,
						( il0, getCurrentEmitter ) =>
						{
							Emittion.EmitMarshalValue(
								emitter,
								il0,
								1,
								traits.ElementType,
								_ => getCurrentEmitter()
							);
						}
					);
				}
				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
			}
		}

		private static void CreateUnpackArrayProceduresCore( SerializerEmitter emitter, Type collectionType, CollectionTraits traits )
		{
			CreateArrayUnpackFrom( emitter, collectionType );
			CreateArrayUnpackTo( emitter, collectionType, traits );
		}

		private static void CreateArrayUnpackFrom( SerializerEmitter emitter, Type collectionType )
		{
			var unpackFromIL = emitter.GetUnpackFromMethodILGenerator();
			try
			{
				if ( collectionType.IsInterface || collectionType.IsAbstract )
				{
					unpackFromIL.EmitTypeOf( collectionType );
					unpackFromIL.EmitAnyCall( SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractTypeMethod );
					unpackFromIL.EmitThrow();
					return;
				}

				/*
				 *	if (!unpacker.IsArrayHeader)
				 *	{
				 *		throw SerializationExceptions.NewIsNotArrayHeader();
				 *	}
				 *	
				 *	TCollection collection = new ...;
				 *	this.UnpackToCore(unpacker, array);
				 *	return collection;
				 */

				unpackFromIL.EmitAnyLdarg( 1 );
				unpackFromIL.EmitGetProperty( Metadata._Unpacker.IsArrayHeader );
				var endIf = unpackFromIL.DefineLabel( "END_IF" );
				unpackFromIL.EmitBrtrue_S( endIf );
				unpackFromIL.EmitAnyCall( SerializationExceptions.NewIsNotArrayHeaderMethod );
				unpackFromIL.EmitThrow();
				unpackFromIL.MarkLabel( endIf );
				var collection = unpackFromIL.DeclareLocal( collectionType, "collection" );
				// Emit newobj, newarr, or call ValueType..ctor()
				Emittion.EmitConstruction(
					unpackFromIL,
					collection,
					il => Emittion.EmitGetUnpackerItemsCountAsInt32( il, 1 )
				);

				unpackFromIL.EmitAnyLdarg( 0 );
				unpackFromIL.EmitAnyLdarg( 1 );
				unpackFromIL.EmitAnyLdloc( collection );
				unpackFromIL.EmitAnyCall( MessagePackSerializer<TObject>.UnpackToCoreMethod );
				unpackFromIL.EmitAnyLdloc( collection );
				unpackFromIL.EmitRet();
			}
			finally
			{
				unpackFromIL.FlushTrace();
			}
		}

		private static void CreateArrayUnpackTo( SerializerEmitter emitter, Type collectionType, CollectionTraits traits )
		{
			/*
			 *	int count = checked((int)unpacker.ItemsCount);
			 *	for (int i = 0; i < count; i++)
			 *	{
			 *		if (!unpacker.Read())
			 *		{
			 *			throw SerializationExceptions.NewMissingItem(i);
			 *		}
			 *		
			 *		T item;
			 *		if (!unpacker.IsArrayHeader && !unpacker.IsMapHeader)
			 *		{
			 *			item = this._serializer.UnpackFrom(unpacker);
			 *		}
			 *		else
			 *		{
			 *			using (Unpacker subtreeUnpacker = unpacker.ReadSubtree())
			 *			{
			 *				item = this._serializer.UnpackFrom(subtreeUnpacker);
			 *			}
			 *		}
			 *		
			 *		collection[i] = guid;
			 *	}
	}		 */

			var il = emitter.GetUnpackToMethodILGenerator();
			try
			{
				var itemsCount = il.DeclareLocal( typeof( int ), "itemsCount" );

				Emittion.EmitGetUnpackerItemsCountAsInt32( il, 1 );
				il.EmitAnyStloc( itemsCount );
				Emittion.EmitFor(
					il,
					itemsCount,
					( il0, i ) =>
					{
						var value = il0.DeclareLocal( traits.ElementType, "value" );
						Emittion.EmitUnmarshalValue(
							emitter,
							il0,
							1,
							value,
							( il1, unpacker ) =>
							{
								il1.EmitAnyLdarg( unpacker );
								il1.EmitAnyCall( Metadata._Unpacker.Read );
								var endIf = il1.DefineLabel( "END_IF" );
								il1.EmitBrtrue_S( endIf );
								il1.EmitAnyLdloc( i );
								il1.EmitAnyCall( SerializationExceptions.NewMissingItemMethod );
								il1.EmitThrow();
								il1.MarkLabel( endIf );
							}
						);

						EmitLoadTarget( il0, 2 );

						if ( collectionType.IsArray )
						{
							il0.EmitAnyLdloc( i );
						}

						il0.EmitAnyLdloc( value );

						if ( collectionType.IsArray )
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
			}
			finally
			{
				il.FlushTrace();
			}
		}

		public sealed override MessagePackSerializer<TObject> CreateMapSerializer()
		{
			return CreateMapSerializerCore( typeof( TObject ) ).CreateInstance<TObject>( this.Context );
		}

		private static SerializerEmitter CreateMapSerializerCore( Type collectionType )
		{
			var emitter = SerializationMethodGeneratorManager.Get().CreateEmitter( collectionType );
			var traits = collectionType.GetCollectionTraits();
			CreateMapPack(
				emitter,
				collectionType,
				traits
			);
			CreateMapUnpack(
				emitter,
				collectionType,
				traits
			);

			return emitter;
		}

		private static void CreateMapPack( SerializerEmitter emiter, Type collectionType, CollectionTraits traits )
		{
			var il = emiter.GetPackToMethodILGenerator();
			try
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

				var collection = il.DeclareLocal( collectionType, "collection" );
				var item = il.DeclareLocal( traits.ElementType, "item" );
				var keyProperty = traits.ElementType.GetProperty( "Key" );
				var valueProperty = traits.ElementType.GetProperty( "Value" );
				// This instruction is always ldarg, not to be ldarga.
				il.EmitAnyLdarg( 2 );
				il.EmitAnyStloc( collection );
				var count = il.DeclareLocal( typeof( int ), "count" );
				EmitLoadTarget( il, collection );
				il.EmitGetProperty( traits.CountProperty );
				il.EmitAnyStloc( count );
				il.EmitAnyLdarg( 1 );
				il.EmitAnyLdloc( count );
				il.EmitAnyCall( Metadata._Packer.PackMapHeader );
				il.EmitPop();

				Emittion.EmitForEach(
					il,
					traits,
					collection,
					( il0, getCurrentEmitter ) =>
					{
						if ( traits.ElementType.IsGenericType )
						{
							Contract.Assert( traits.ElementType.GetGenericTypeDefinition() == typeof( KeyValuePair<,> ) );
							getCurrentEmitter();
							il0.EmitAnyStloc( item );
							Emittion.EmitMarshalValue(
								emiter,
								il0,
								1,
								traits.ElementType.GetGenericArguments()[ 0 ],
								il1 =>
								{
									il1.EmitAnyLdloca( item );
									il1.EmitGetProperty( keyProperty );
								}
							);

							Emittion.EmitMarshalValue(
								emiter,
								il0,
								1,
								traits.ElementType.GetGenericArguments()[ 1 ],
								il1 =>
								{
									il1.EmitAnyLdloca( item );
									il1.EmitGetProperty( valueProperty );
								}
							);
						}
						else
						{
							Contract.Assert( traits.ElementType == typeof( DictionaryEntry ) );
							getCurrentEmitter();
							il0.EmitAnyStloc( item );
							Emittion.EmitMarshalValue(
								emiter,
								il0,
								1,
								typeof( MessagePackObject ),
								il1 =>
								{
									il0.EmitAnyLdloca( item );
									il0.EmitGetProperty( Metadata._DictionaryEntry.Key );
									il0.EmitUnbox_Any( typeof( MessagePackObject ) );
								}
							);

							Emittion.EmitMarshalValue(
								emiter,
								il0,
								1,
								typeof( MessagePackObject ),
								il1 =>
								{
									il0.EmitAnyLdloca( item );
									il0.EmitGetProperty( Metadata._DictionaryEntry.Value );
									il0.EmitUnbox_Any( typeof( MessagePackObject ) );
								}
							);
						}
					}
				);
				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
			}
		}

		private static void CreateMapUnpack( SerializerEmitter emitter, Type collectionType, CollectionTraits traits )
		{
			CreateMapUnpackFrom( emitter, collectionType );
			CreateMapUnpackTo( emitter, traits );
		}

		private static void CreateMapUnpackFrom( SerializerEmitter emitter, Type collectionType )
		{
			var il = emitter.GetUnpackFromMethodILGenerator();
			try
			{
				/*
				 *	if (!unpacker.IsMapHeader)
				 *	{
				 *		throw SerializationExceptions.NewIsNotMapHeader();
				 *	}
				 *	
				 *	TDictionary<TKey, TValue> dictionary = new ...;
				 *	this.UnpackToCore(unpacker, dictionary);
				 *	return dictionary;
				 */

				if ( collectionType.IsInterface || collectionType.IsAbstract )
				{
					il.EmitTypeOf( collectionType );
					il.EmitAnyCall( SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractTypeMethod );
					il.EmitThrow();
					return;
				}

				il.EmitAnyLdarg( 1 );
				il.EmitGetProperty( Metadata._Unpacker.IsMapHeader );
				var endIf = il.DefineLabel( "END_IF" );
				il.EmitBrtrue_S( endIf );
				il.EmitAnyCall( SerializationExceptions.NewIsNotMapHeaderMethod );
				il.EmitThrow();
				il.MarkLabel( endIf );

				var collection = il.DeclareLocal( collectionType, "collection" );
				Emittion.EmitConstruction(
					il,
					collection,
					il0 => Emittion.EmitGetUnpackerItemsCountAsInt32( il0, 1 )
				);

				il.EmitAnyLdarg( 0 );
				il.EmitAnyLdarg( 1 );
				il.EmitAnyLdloc( collection );
				il.EmitAnyCall( MessagePackSerializer<TObject>.UnpackToCoreMethod );
				il.EmitAnyLdloc( collection );
				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
			}
		}

		private static void CreateMapUnpackTo( SerializerEmitter emitter, CollectionTraits traits )
		{
			var il = emitter.GetUnpackToMethodILGenerator();
			try
			{
				/*
				 *	if (!unpacker.IsMapHeader)
				 *	{
				 *		throw SerializationExceptions.NewIsNotMapHeader();
				 *	}
				 *	
				 *	int count = checked((int)unpacker.ItemsCount);
				 *	for (int i = 0; i < count; i++)
				 *	{
				 *		if (!unpacker.Read())
				 *		{
				 *			throw SerializationExceptions.NewMissingItem(i);
				 *		}
				 *		
				 *		TKey key;
				 *		if (!unpacker.IsArrayHeader && !unpacker.IsMapHeader)
				 *		{
				 *			key = this._serializer0.UnpackFrom(unpacker);
				 *		}
				 *		else
				 *		{
				 *			using (Unpacker subtreeUnpacker = unpacker.ReadSubtree())
				 *			{
				 *				key = this._serializer0.UnpackFrom(subtreeUnpacker);
				 *			}
				 *		}
				 *		
				 *		if (!unpacker.Read())
				 *		{
				 *			throw SerializationExceptions.NewMissingItem(i);
				 *		}
				 *		
				 *		TValue value;
				 *		if (!unpacker.IsArrayHeader && !unpacker.IsMapHeader)
				 *		{
				 *			value = this._serializer1.UnpackFrom(unpacker);
				 *		}
				 *		else
				 *		{
				 *			using (Unpacker subtreeUnpacker = unpacker.ReadSubtree())
				 *			{
				 *				value = this._serializer1.UnpackFrom(subtreeUnpacker);
				 *			}
				 *		}
				 *		
				 *		dictionary.Add(key, value);
				 *	}
				 */

				il.EmitAnyLdarg( 1 );
				il.EmitGetProperty( Metadata._Unpacker.IsMapHeader );
				var endIf = il.DefineLabel( "END_IF" );
				il.EmitBrtrue_S( endIf );
				il.EmitAnyCall( SerializationExceptions.NewIsNotMapHeaderMethod );
				il.EmitThrow();
				il.MarkLabel( endIf );

				var itemsCount = il.DeclareLocal( typeof( int ), "itemsCount" );
#if DEBUG
				Contract.Assert( traits.ElementType.IsGenericType && traits.ElementType.GetGenericTypeDefinition() == typeof( KeyValuePair<,> )
					|| traits.ElementType == typeof( DictionaryEntry ) );
#endif

				var key = il.DeclareLocal( traits.ElementType.IsGenericType ? traits.ElementType.GetGenericArguments()[ 0 ] : typeof( MessagePackObject ), "key" );
				var value = il.DeclareLocal( traits.ElementType.IsGenericType ? traits.ElementType.GetGenericArguments()[ 1 ] : typeof( MessagePackObject ), "value" );

				Emittion.EmitGetUnpackerItemsCountAsInt32( il, 1 );
				il.EmitAnyStloc( itemsCount );
				Emittion.EmitFor(
					il,
					itemsCount,
					( il0, i ) =>
					{
						Action<TracingILGenerator, int> unpackerReading =
							( il1, unpacker ) =>
							{
								il1.EmitAnyLdarg( unpacker );
								il1.EmitAnyCall( Metadata._Unpacker.Read );
								var endIf0 = il1.DefineLabel( "END_IF0" );
								il1.EmitBrtrue_S( endIf0 );
								il1.EmitAnyLdloc( i );
								il1.EmitAnyCall( SerializationExceptions.NewMissingItemMethod );
								il1.EmitThrow();
								il1.MarkLabel( endIf0 );
							};

						// Key
						Emittion.EmitUnmarshalValue( emitter, il0, 1, key, unpackerReading );

						// Value
						Emittion.EmitUnmarshalValue( emitter, il0, 1, value, unpackerReading );

						EmitLoadTarget( il0, 2 );

						il0.EmitAnyLdloc( key );
						if ( !traits.ElementType.IsGenericType )
						{
							il0.EmitBox( key.LocalType );
						}

						il0.EmitAnyLdloc( value );
						if ( !traits.ElementType.IsGenericType )
						{
							il0.EmitBox( value.LocalType );
						}

						il0.EmitAnyCall( traits.AddMethod );
						if ( traits.AddMethod.ReturnType != typeof( void ) )
						{
							il0.EmitPop();
						}
					}
				);
				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
			}
		}

		public sealed override MessagePackSerializer<TObject> CreateTupleSerializer()
		{
			return CreateTupleSerializerCore( typeof( TObject ) ).CreateInstance<TObject>( this.Context );
		}

		private static SerializerEmitter CreateTupleSerializerCore( Type tupleType )
		{
			var emitter = SerializationMethodGeneratorManager.Get().CreateEmitter( tupleType );
			var itemTypes = GetTupleItemTypes( tupleType );
			CreateTuplePack(
				emitter,
				tupleType,
				itemTypes,
				( il, collection ) =>
				{
					il.EmitAnyLdarg( 2 );
					il.EmitAnyStloc( collection );
				}
			);
			CreateTupleUnpack(
				emitter,
				itemTypes
			);

			return emitter;
		}

		private static IList<Type> GetTupleItemTypes( Type tupleType )
		{
			Contract.Assert( tupleType.Name.StartsWith( "Tuple`" ) && tupleType.Assembly == typeof( Tuple ).Assembly );
			var arguments = tupleType.GetGenericArguments();
			List<Type> itemTypes = new List<Type>( tupleType.GetGenericArguments().Length );
			GetTupleItemTypes( arguments, itemTypes );
			return itemTypes;
		}

		private static void GetTupleItemTypes( IList<Type> itemTypes, IList<Type> result )
		{
			int count = itemTypes.Count == 8 ? 7 : itemTypes.Count;
			for ( int i = 0; i < count; i++ )
			{
				result.Add( itemTypes[ i ] );
			}

			if ( itemTypes.Count == 8 )
			{
				var trest = itemTypes[ 7 ];
				Contract.Assert( trest.Name.StartsWith( "Tuple`" ) && trest.Assembly == typeof( Tuple ).Assembly );
				GetTupleItemTypes( trest.GetGenericArguments(), result );
			}
		}

		private static void CreateTuplePack( SerializerEmitter emiter, Type tupleType, IList<Type> itemTypes, Action<TracingILGenerator, LocalBuilder> loadTupleEmitter )
		{
			var il = emiter.GetPackToMethodILGenerator();
			try
			{
				/*
				 * packer.PackArrayHeader( cardinarity );
				 * _serializer0.PackTo( packer, tuple.Item1 );
				 *	:
				 * _serializer6.PackTo( packer, tuple.item7 );
				 * _serializer7.PackTo( packer, tuple.Rest.Item1 );
				 */

				var tuple = il.DeclareLocal( tupleType, "tuple" );
				loadTupleEmitter( il, tuple );
				il.EmitAnyLdarg( 1 );
				il.EmitAnyLdc_I4( itemTypes.Count );
				il.EmitAnyCall( Metadata._Packer.PackArrayHeader );
				il.EmitPop();

				var tupleTypeList = TupleItems.CreateTupleTypeList( itemTypes );

				int depth = -1;
				for ( int i = 0; i < itemTypes.Count; i++ )
				{
					if ( i % 7 == 0 )
					{
						depth++;
					}

					Emittion.EmitMarshalValue(
						emiter,
						il,
						1,
						itemTypes[ i ],
						il0 =>
						{
							il0.EmitAnyLdloc( tuple );

							for ( int j = 0; j < depth; j++ )
							{
								// .TRest.TRest ...
								var rest = tupleTypeList[ j ].GetProperty( "Rest" );
								il0.EmitGetProperty( rest );
							}

							var itemn = tupleTypeList[ depth ].GetProperty( "Item" + ( ( i % 7 ) + 1 ) );
#if DEBUG
							Contract.Assert( itemn != null, tupleTypeList[ depth ].GetFullName() + "::Item" + ( ( i % 7 ) + 1 ) + " [ " + depth + " ] @ " + i );
#endif
							il0.EmitGetProperty( itemn );
						}
					);
				}
				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
			}
		}

		private static void CreateTupleUnpack( SerializerEmitter emitter, IList<Type> itemTypes )
		{
			CreateTupleUnpackFrom( emitter, itemTypes );
		}

		private static void CreateTupleUnpackFrom( SerializerEmitter emitter, IList<Type> itemTypes )
		{
			var il = emitter.GetUnpackFromMethodILGenerator();
			try
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

				il.EmitAnyLdarg( 1 );
				il.EmitGetProperty( Metadata._Unpacker.IsArrayHeader );
				var endIf = il.DefineLabel( "END_IF" );
				il.EmitBrtrue_S( endIf );
				il.EmitAnyCall( SerializationExceptions.NewIsNotArrayHeaderMethod );
				il.EmitThrow();
				il.MarkLabel( endIf );

				var itemsCount = il.DeclareLocal( typeof( int ), "itemsCount" );
				Emittion.EmitGetUnpackerItemsCountAsInt32( il, 1 );
				il.EmitAnyLdc_I4( itemTypes.Count );
				il.EmitAnyStloc( itemsCount );
				il.EmitAnyLdloc( itemsCount );
				var endIf1 = il.DefineLabel( "END_IF1" );
				il.EmitBeq_S( endIf1 );
				il.EmitAnyLdc_I4( itemTypes.Count );
				il.EmitAnyLdloc( itemsCount );
				il.EmitAnyCall( SerializationExceptions.NewTupleCardinarityIsNotMatchMethod );
				il.EmitThrow();
				il.MarkLabel( endIf1 );

				LocalBuilder[] itemLocals = new LocalBuilder[ itemTypes.Count ];
				int i = 0;
				Action<TracingILGenerator, int> unpackerReading =
					( il1, unpacker ) =>
					{
						il1.EmitAnyLdarg( unpacker );
						il1.EmitAnyCall( Metadata._Unpacker.Read );
						var endIf0 = il1.DefineLabel( "END_IF0" );
						il1.EmitBrtrue_S( endIf0 );
						il1.EmitAnyLdc_I4( i );
						il1.EmitAnyCall( SerializationExceptions.NewMissingItemMethod );
						il1.EmitThrow();
						il1.MarkLabel( endIf0 );
					};

				for ( ; i < itemTypes.Count; i++ )
				{
					itemLocals[ i ] = il.DeclareLocal( itemTypes[ i ], "item" + i );
					Emittion.EmitUnmarshalValue( emitter, il, 1, itemLocals[ i ], unpackerReading );
				}

				foreach ( var item in itemLocals )
				{
					il.EmitAnyLdloc( item );
				}

				var tupleTypeList = TupleItems.CreateTupleTypeList( itemTypes );

				for ( int depth = tupleTypeList.Count - 1; 0 <= depth; depth-- )
				{
					il.EmitNewobj( tupleTypeList[ depth ].GetConstructors().Single() );
				}

				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
			}
		}

		private static void EmitLoadTarget( TracingILGenerator il, int parameterIndex )
		{
			if ( typeof( TObject ).IsValueType )
			{
				il.EmitAnyLdarga( parameterIndex );
			}
			else
			{
				il.EmitAnyLdarg( parameterIndex );
			}
		}

		private static void EmitLoadTarget( TracingILGenerator il, LocalBuilder local )
		{
			if ( typeof( TObject ).IsValueType )
			{
				il.EmitAnyLdloca( local );
			}
			else
			{
				il.EmitAnyLdloc( local );
			}
		}
	}
}
