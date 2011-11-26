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
	// FIXME: null handling
	// TODO: Rename to EmittingSerializerBuilder
	/// <summary>
	///		<see cref="SerializerBuilder{T}"/> implementation using Reflection.Emit.
	/// </summary>
	/// <typeparam name="TObject">Object to be serialized/deserialized.</typeparam>
	internal sealed class EmittingMemberBinder<TObject> : SerializerBuilder<TObject>
	{
		public EmittingMemberBinder( SerializationContext context ) : base( context ) { }

		protected sealed override MessagePackSerializer<TObject> CreateSerializer( SerlializingMember[] entries )
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
				var locals = entries.Select( item => item.Member.CanSetValue() ? unpackerIL.DeclareLocal( item.Member.GetMemberValueType(), item.Contract.Name ) : null ).ToArray();

				var result = unpackerIL.DeclareLocal( typeof( TObject ), "result" );
				Emittion.EmitConstruction( unpackerIL, result.LocalType );
				unpackerIL.EmitAnyStloc( result );

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

		protected sealed override ConstructorInfo CreateArraySerializer( MemberInfo member, Type memberType )
		{
			// FIXME: Use Contract
			// FIXME: Use MessagePackArraySerializer.Create<T> directly
			var emitter = SerializationMethodGeneratorManager.Get().CreateEmitter( memberType );
			CreatePackArrayProceduresCore( emitter, member, memberType );
			CreateUnpackArrayProceduresCore( emitter, member, memberType );
			return emitter.Create();
		}

		private static void CreatePackArrayProceduresCore( SerializerEmitter emitter, MemberInfo member, Type memberType )
		{
			/*
			 * context.MarshalArrayTo<T>( packer, target.... );
			 */
			var il = emitter.GetPackToMethodILGenerator();
			try
			{
				var collection = il.DeclareLocal( memberType, "collection" );
				il.EmitAnyLdarg( 2 );
				Emittion.EmitLoadValue( il, member );
				il.EmitAnyStloc( collection );
				Emittion.EmitMarshalValue(
					emitter,
					il,
					1,
					memberType,
					il0 =>
					{
						il0.EmitAnyLdarg( 2 );
					}
				);
				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
			}
		}

		private static void CreateUnpackArrayProceduresCore( SerializerEmitter emitter, MemberInfo member, Type memberType )
		{
			/*
			 * context.UnmarshalArrayTo<T>( packer, target.... );
			 */
			var il = emitter.GetPackToMethodILGenerator();
			try
			{
				var collection = il.DeclareLocal( memberType, "collection" );
				Emittion.EmitLoadValue( il, member );
				il.EmitAnyStloc( collection );
				Emittion.EmitUnmarshalValue(
					emitter,
					il,
					1,
					collection,
					null
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
			return CreateMapSerializerCore( typeof( TObject ), null ).CreateInstance<TObject>( this.Context );
		}

		protected sealed override ConstructorInfo CreateMapSerializer( MemberInfo member, Type memberType )
		{
			return CreateMapSerializerCore( memberType, member ).Create();
		}

		private static SerializerEmitter CreateMapSerializerCore( Type collectionType, MemberInfo memberOfNull )
		{
			var emitter = SerializationMethodGeneratorManager.Get().CreateEmitter( collectionType );
			var traits = collectionType.GetCollectionTraits();
			CreatePackMapProceduresCore(
				emitter,
				collectionType,
				traits,
				( il, collection ) =>
				{
					il.EmitAnyLdarg( 2 );
					if ( memberOfNull != null )
					{
						Emittion.EmitLoadValue( il, memberOfNull );
					}

					il.EmitAnyStloc( collection );
				}
			);
			CreateUnpackMapProceduresCore(
				emitter,
				collectionType,
				traits,
				( il, collection ) =>
				{
					if ( memberOfNull == null )
					{
						Emittion.EmitConstruction( il, collectionType );
					}
					else
					{
						il.EmitAnyLdarg( 2 );
						Emittion.EmitLoadValue( il, memberOfNull );
					}

					il.EmitAnyStloc( collection );
				}
			);

			return emitter;
		}

		private static void CreatePackMapProceduresCore( SerializerEmitter emiter, Type collectionType, CollectionTraits traits, Action<TracingILGenerator, LocalBuilder> loadCollectionEmitter )
		{
			var il = emiter.GetPackToMethodILGenerator();
			try
			{

				/*
				 * 
				 * // Enumerable
				 * foreach( var item in map )
				 * {
				 * 		Context.MarshalTo( packer, array[ i ] );
				 * }
				 */
				var collection = il.DeclareLocal( collectionType, "collection" );
				var item = il.DeclareLocal( traits.ElementType, "item" );
				var keyProperty = traits.ElementType.GetProperty( "Key" );
				var valueProperty = traits.ElementType.GetProperty( "Value" );
				loadCollectionEmitter( il, collection );
				var count = il.DeclareLocal( typeof( int ), "count" );
				il.EmitAnyLdloc( collection );
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

		private static void CreateUnpackMapProceduresCore( SerializerEmitter emitter, Type collectionType, CollectionTraits traits, Action<TracingILGenerator, LocalBuilder> loadCollectionEmitter )
		{
			/*
			 * int itemCount = unpacker.ItemCount;
			 * var collection = target....;
			 * for( int i = 0; i < array.Length; i++ )
			 * {
			 *		if( !unpacker.MoveToNextEntry() )
			 *		{
			 *			throw new SerializationException();
			 *		}
			 *		collection.Add( Context.Serializers.Get<T>().Deserialize( unpacker ) )
			 * }
			 */
			var unpackFromIL = emitter.GetUnpackFromMethodILGenerator();
			try
			{
				var collection = unpackFromIL.DeclareLocal( collectionType, "collection" );
				loadCollectionEmitter( unpackFromIL, collection );
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

			var unpackToIL = emitter.GetUnpackToMethodILGenerator();
			try
			{
				var itemsCount = unpackToIL.DeclareLocal( typeof( int ), "itemsCount" );
#if DEBUG
				Contract.Assert( traits.ElementType.IsGenericType && traits.ElementType.GetGenericTypeDefinition() == typeof( KeyValuePair<,> )
					|| traits.ElementType == typeof( DictionaryEntry ) );
#endif

				var key = unpackToIL.DeclareLocal( traits.ElementType.IsGenericType ? traits.ElementType.GetGenericArguments()[ 0 ] : typeof( MessagePackObject ), "key" );
				var value = unpackToIL.DeclareLocal( traits.ElementType.IsGenericType ? traits.ElementType.GetGenericArguments()[ 1 ] : typeof( MessagePackObject ), "value" );

				unpackToIL.EmitAnyLdarg( 1 );
				unpackToIL.EmitGetProperty( Metadata._Unpacker.ItemsCount );
				unpackToIL.EmitConv_Ovf_I4();
				unpackToIL.EmitAnyStloc( itemsCount );
				Emittion.EmitFor(
					unpackToIL,
					itemsCount,
					( il0, i ) =>
					{
						Action<TracingILGenerator, int> unpackerReading =
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
							};

						// Key
						Emittion.EmitUnmarshalValue( emitter, il0, 1, key, unpackerReading );

						// Value
						Emittion.EmitUnmarshalValue( emitter, il0, 1, value, unpackerReading );

						il0.EmitAnyLdarg( 2 );

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
				unpackToIL.EmitRet();
			}
			finally
			{
				unpackToIL.FlushTrace();
			}
		}

		protected sealed override ConstructorInfo CreateObjectSerializer( MemberInfo member, Type memberType )
		{
			var emitter = SerializationMethodGeneratorManager.Get().CreateEmitter( memberType );
			CreatePackObjectProceduresCore( emitter, member, memberType );
			CreateUnpackObjectProceduresCore( emitter, member, memberType );
			return emitter.Create();
		}

		private static void CreatePackObjectProceduresCore( SerializerEmitter emitter, MemberInfo member, Type memberType )
		{
			/*
			 * Context.Serializers.Get<T>().Serialize( packer, value.... );
			 */
			var il = emitter.GetPackToMethodILGenerator();
			try
			{
				Emittion.EmitMarshalValue(
					emitter,
					il,
					1,
					memberType,
					il0 =>
					{
						il0.EmitAnyLdarg( 2 );
						Emittion.EmitLoadValue( il0, member );
					}
				);
				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
			}
		}

		private static void CreateUnpackObjectProceduresCore( SerializerEmitter emitter, MemberInfo member, Type memberType )
		{
			/*
			 * Context.Serializers.Get<T>().Deserialize( packer, value.... );
			 */
			var il = emitter.GetUnpackFromMethodILGenerator();
			try
			{
				var itemsCount = il.DeclareLocal( typeof( int ), "itemsCount" );
				var collection = il.DeclareLocal( memberType, "collection" );
				Emittion.EmitUnmarshalValue(
					emitter,
					il,
					1,
					collection,
					null // Dispatching closure shall adjust position.
				);
				il.EmitAnyLdarg( 1 );
				il.EmitAnyLdloc( collection );
				Emittion.EmitStoreValue( il, member );
				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
			}
		}
	}
}
