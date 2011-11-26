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
		private static readonly MethodInfo _packerPackMessagePackObject = FromExpression.ToMethod( ( Packer packer, MessagePackObject value ) => packer.Pack( value ) );
		private static readonly PropertyInfo _unpackerDataProperty = FromExpression.ToProperty( ( Unpacker unpacker ) => unpacker.Data );
		private static readonly PropertyInfo _messagePackObjectIsNilProperty = FromExpression.ToProperty( ( MessagePackObject value ) => value.IsNil );
		private static readonly PropertyInfo _unpackerItemsCountProperty = FromExpression.ToProperty( ( Unpacker unpadker ) => unpadker.ItemsCount );
		private static readonly MethodInfo _packerPackArrayHeaderMethod = FromExpression.ToMethod( ( Packer packer, int count ) => packer.PackArrayHeader( count ) );
		private static readonly MethodInfo _packerPackMapHeaderMethod = FromExpression.ToMethod( ( Packer packer, int count ) => packer.PackMapHeader( count ) );
		private static readonly PropertyInfo _unpackerIsInStartProperty = FromExpression.ToProperty( ( Unpacker unpacker ) => unpacker.IsInStart );
		private static readonly MethodInfo _unpackerReadMethod = FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.Read() );
		private static readonly Type[] _packingMethodParameters = new[] { typeof( Packer ), typeof( TObject ), typeof( SerializationContext ) };
		private static readonly Type[] _unpackingMethodParameters = new[] { typeof( Unpacker ), typeof( TObject ), typeof( SerializationContext ) };
		private static readonly PropertyInfo _dictionaryEntryKeyProperty = FromExpression.ToProperty( ( DictionaryEntry entry ) => entry.Key );
		private static readonly PropertyInfo _dictionaryEntryValueProperty = FromExpression.ToProperty( ( DictionaryEntry entry ) => entry.Value );

		public EmittingMemberBinder( SerializationContext context ) : base( context ) { }

		[Obsolete]
		protected sealed override bool CreateProcedures( SerlializingMember[] entries, out Action<Packer, TObject, SerializationContext> packing, out Func<Unpacker, SerializationContext, TObject> unpacking )
		{
			Action<Packer, TObject, SerializationContext>[] packings = new Action<Packer, TObject, SerializationContext>[ entries.Length ];
			Action<Unpacker, TObject, SerializationContext>[] unpackings = new Action<Unpacker, TObject, SerializationContext>[ entries.Length ];
			for ( int i = 0; i < entries.Length; i++ )
			{
				PropertyInfo asProperty;
				if ( ( asProperty = entries[ i ].Member as PropertyInfo ) != null )
				{
					if ( !this.CreateProcedures( asProperty, entries[ i ].Contract, out packings[ i ], out unpackings[ i ] ) )
					{
						packing = null;
						unpacking = null;
						return false;
					}
				}
				else
				{
					if ( !this.CreateProcedures( entries[ i ].Member as FieldInfo, entries[ i ].Contract, out packings[ i ], out unpackings[ i ] ) )
					{
						packing = null;
						unpacking = null;
						return false;
					}
				}
			}

			packing = Closures.PackObject( entries, packings );
			unpacking = Closures.UnpackObject( entries, unpackings );

			return true;
		}

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

		private void CreatePackMembers( SerializerEmitter emitter, SerlializingMember[] entries, ISet<Type> usedSerializerTypes )
		{
			//var il = emitter.GetPackToMethodILGenerator();
			//try
			//{
			//    il.EmitAnyLdarg( 1 );
			//    il.EmitAnyLdc_I4( entries.Length );
			//    il.EmitAnyCall( Metadata._Packer.PackMapHeader );
			//    il.EmitPop();
			//    foreach ( var entry in entries )
			//    {
			//        PropertyInfo asProperty = entry.Member as PropertyInfo;
			//        FieldInfo asField = entry.Member as FieldInfo;
			//        Type memberType = asProperty != null ? asProperty.PropertyType : asField.FieldType;
			//        var serializerType = this.CreateSerialierIfNecessary( entry, memberType );
			//        usedSerializerTypes.Add( serializerType );
			//        var serializer = emitter.RegisterSerializer( serializerType );
			//        Emittion.EmitMarshalValue(
			//            emitter,
			//            il,
			//            1,
			//            memberType,
			//            il0 =>
			//            {
			//                il0.EmitAnyLdarg( 2 );
			//                Emittion.EmitLoadValue( il0, entry.Member );
			//            }
			//        );
			//    }

			//    il.EmitRet();
			//}
			//finally
			//{
			//    il.FlushTrace();
			//}
		}

		private void CreateUnpackMembers( SerializerEmitter emitter, SerlializingMember[] entries, ISet<Type> usedSerializerTypes )
		{
			var il = emitter.GetPackToMethodILGenerator();
			try
			{
				Emittion.EmitConstruction( il, typeof( TObject ) );

				il.EmitAnyLdarg( 1 );
				var subtreeUnpacker = il.DeclareLocal( typeof( Unpacker ), "subtreeUnpacker" );
				il.EmitAnyCall( Metadata._Unpacker.ReadSubtree );
				il.EmitAnyStloc( subtreeUnpacker );
				il.BeginExceptionBlock();

				//Emittion.EmitUnpackMembers()
				// Get count
				var count = il.DeclareLocal( typeof( long ), "count" );
				il.EmitAnyLdarg( 1 );
				il.EmitGetProperty( Metadata._Unpacker.ItemsCount );
				il.EmitAnyStloc( count );



				il.EmitAnyLdc_I4( entries.Length );
				il.EmitAnyCall( Metadata._Packer.PackMapHeader );
				il.EmitPop();
				foreach ( var entry in entries )
				{
					PropertyInfo asProperty = entry.Member as PropertyInfo;
					FieldInfo asField = entry.Member as FieldInfo;
					Type memberType = asProperty != null ? asProperty.PropertyType : asField.FieldType;
					var serializerType = this.CreateSerialierIfNecessary( entry, memberType );
					usedSerializerTypes.Add( serializerType );
					var serializer = emitter.RegisterSerializer( serializerType );
					Emittion.EmitMarshalValue(
						emitter,
						il,
						1,
						memberType,
						il0 =>
						{
							il0.EmitAnyLdarg( 2 );
							Emittion.EmitLoadValue( il0, entry.Member );
						}
					);
				}

				il.BeginFinallyBlock();
				il.EmitAnyLdloc( subtreeUnpacker );
				il.EmitAnyCall( Metadata._IDisposable.Dispose );
				il.EndExceptionBlock();

				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
			}
		}

		private Type CreateSerialierIfNecessary( SerlializingMember entry, Type memberType )
		{
			var serializer = SerializerRepository.Get1Method.MakeGenericMethod( memberType ).Invoke( this.Context, null );
			if ( serializer != null )
			{
				return serializer.GetType();
			}

			var ctor = this.CreateSerializer( entry.Member, entry.Contract );
			serializer = ctor.Invoke( new object[] { this.Context } );
			if ( ( bool )SerializerRepository.Register1Method.MakeGenericMethod( memberType ).Invoke( this.Context.Serializers, new object[] { serializer } ) )
			{
				return serializer.GetType();
			}

			return SerializerRepository.Get1Method.MakeGenericMethod( memberType ).Invoke( this.Context, null ).GetType();
		}


		[Obsolete]
		protected sealed override bool CreateArrayProcedures( MemberInfo member, Type memberType, DataMemberContract contract, CollectionTraits traits, out Action<Packer, TObject, SerializationContext> packing, out Action<Unpacker, TObject, SerializationContext> unpacking )
		{
			packing = CreatePackArrayProceduresCore( member, memberType, contract, traits );
			if ( packing == null )
			{
				unpacking = null;
				return false;
			}

			unpacking = CreateUnpackArrayProceduresCore( member, memberType, contract, traits );
			return unpacking != null;
		}

		protected sealed override ConstructorInfo CreateArraySerializer( MemberInfo member, Type memberType )
		{
			var emitter = SerializationMethodGeneratorManager.Get().CreateEmitter( memberType );
			CreatePackArrayProceduresCore( emitter, member, memberType );
			CreateUnpackArrayProceduresCore( emitter, member, memberType );
			return emitter.Create();
		}

		[Obsolete]
		private static Action<Packer, TObject, SerializationContext> CreatePackArrayProceduresCore( MemberInfo member, Type memberType, DataMemberContract contract, CollectionTraits traits )
		{
			/*
			 * context.MarshalArrayTo<T>( packer, target.... );
			 */
			var dynamicMethod = SerializationMethodGeneratorManager.Get().CreateGenerator( "Pack", member.DeclaringType, contract.Name, null, _packingMethodParameters );
			var il = dynamicMethod.GetILGenerator();
			try
			{
				il.EmitAnyLdarg( 2 );
				il.EmitAnyLdarg( 0 );
				il.EmitAnyLdarg( 1 );
				Emittion.EmitLoadValue( il, member );
				il.EmitAnyCall( SerializationContext.MarshalArrayTo1Method.MakeGenericMethod( memberType ) );
				il.EmitRet();

				return dynamicMethod.CreateDelegate<Action<Packer, TObject, SerializationContext>>();
			}
			finally
			{
				il.FlushTrace();
				dynamicMethod.FlushTrace();
			}
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

		[Obsolete]
		private static Action<Unpacker, TObject, SerializationContext> CreateUnpackArrayProceduresCore( MemberInfo member, Type memberType, DataMemberContract contract, CollectionTraits traits )
		{
			/*
			 * context.UnmarshalArrayTo<T>( packer, target.... );
			 */
			var dynamicMethod = SerializationMethodGeneratorManager.Get().CreateGenerator( "Unpack", member.DeclaringType, contract.Name, null, _unpackingMethodParameters );
			var il = dynamicMethod.GetILGenerator();
			try
			{
				il.EmitAnyLdarg( 2 );
				il.EmitAnyLdarg( 0 );
				il.EmitAnyLdarg( 1 );
				Emittion.EmitLoadValue( il, member );
				il.EmitAnyCall( SerializationContext.UnmarshalArrayTo1Method.MakeGenericMethod( memberType ) );
				il.EmitRet();

				return dynamicMethod.CreateDelegate<Action<Unpacker, TObject, SerializationContext>>();
			}
			finally
			{
				il.FlushTrace();
				dynamicMethod.FlushTrace();
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

		[Obsolete]
		protected sealed override bool CreateMapProcedures( MemberInfo member, Type memberType, DataMemberContract contract, CollectionTraits traits, out Action<Packer, TObject, SerializationContext> packing, out Action<Unpacker, TObject, SerializationContext> unpacking )
		{
			packing =
				CreatePackMapProceduresCore(
					member.DeclaringType,
					contract.Name,
					memberType,
					traits,
					( il, collection ) =>
					{
						il.EmitAnyLdarg( 1 );
						Emittion.EmitLoadValue( il, member );
						il.EmitAnyStloc( collection );
					}
				);
			if ( packing == null )
			{
				unpacking = null;
				return false;
			}

			unpacking =
				CreateUnpackMapProceduresCore(
					member.DeclaringType,
					contract.Name,
					memberType,
					traits,
					( il, collection ) =>
					{
						il.EmitAnyLdarg( 1 );
						Emittion.EmitLoadValue( il, member );
						il.EmitAnyStloc( collection );
					}
				);
			return unpacking != null;
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

		[Obsolete]
		public sealed override bool CreateMapProcedures( CollectionTraits traits, out Action<Packer, TObject, SerializationContext> packing, out Func<Unpacker, SerializationContext, TObject> unpacking )
		{
			packing =
				CreatePackMapProceduresCore(
					typeof( TObject ),
					"Instance",
					typeof( TObject ),
					traits,
					( il, collection ) =>
					{
						il.EmitAnyLdarg( 1 );
						il.EmitAnyStloc( collection );
					}
				);
			if ( packing == null )
			{
				unpacking = null;
				return false;
			}

			unpacking =
				Closures.UnpackWithForwarding<TObject>(
					CreateUnpackMapProceduresCore(
						typeof( TObject ),
						"Instance",
						typeof( TObject ),
						traits,
						( il, collection ) =>
						{
							il.EmitAnyLdarg( 1 );
							il.EmitAnyStloc( collection );
						}
					)
				);
			return unpacking != null;
		}

		[Obsolete]
		private Action<Packer, TObject, SerializationContext> CreatePackMapProceduresCore( Type targetType, string memberName, Type collectionType, CollectionTraits traits, Action<TracingILGenerator, LocalBuilder> loadCollectionEmitter )
		{
			var dynamicMethod = SerializationMethodGeneratorManager.Get().CreateGenerator( "Pack", targetType, memberName, null, _packingMethodParameters );
			var il = dynamicMethod.GetILGenerator();
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
				il.EmitAnyLdarg( 0 );
				il.EmitAnyLdloc( count );
				il.EmitAnyCall( _packerPackMapHeaderMethod );
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
								il0,
								0,
								2,
								traits.ElementType.GetGenericArguments()[ 0 ],
								il1 =>
								{
									il1.EmitAnyLdloca( item );
									il1.EmitGetProperty( keyProperty );
								}
							);

							Emittion.EmitMarshalValue(
								il0,
								0,
								2,
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
								il0,
								0,
								2,
								typeof( MessagePackObject ),
								il1 =>
								{
									il0.EmitAnyLdloca( item );
									il0.EmitGetProperty( _dictionaryEntryKeyProperty );
									il0.EmitUnbox_Any( typeof( MessagePackObject ) );
								}
							);

							Emittion.EmitMarshalValue(
								il0,
								0,
								2,
								typeof( MessagePackObject ),
								il1 =>
								{
									il0.EmitAnyLdloca( item );
									il0.EmitGetProperty( _dictionaryEntryValueProperty );
									il0.EmitUnbox_Any( typeof( MessagePackObject ) );
								}
							);
						}
					}
				);
				il.EmitRet();

				return dynamicMethod.CreateDelegate<Action<Packer, TObject, SerializationContext>>();
			}
			finally
			{
				il.FlushTrace();
				dynamicMethod.FlushTrace();
			}
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
				il.EmitAnyCall( _packerPackMapHeaderMethod );
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
									il0.EmitGetProperty( _dictionaryEntryKeyProperty );
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
									il0.EmitGetProperty( _dictionaryEntryValueProperty );
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

		[Obsolete]
		private Action<Unpacker, TObject, SerializationContext> CreateUnpackMapProceduresCore( Type targetType, string memberName, Type collectionType, CollectionTraits traits, Action<TracingILGenerator, LocalBuilder> loadCollectionEmitter )
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
			var dynamicMethod = SerializationMethodGeneratorManager.Get().CreateGenerator( "Unpack", targetType, memberName, null, _unpackingMethodParameters );
			var il = dynamicMethod.GetILGenerator();
			try
			{
				var itemsCount = il.DeclareLocal( typeof( int ), "itemsCount" );
				var collection = il.DeclareLocal( collectionType, "collection" );
#if DEBUG
				Contract.Assert( traits.ElementType.IsGenericType && traits.ElementType.GetGenericTypeDefinition() == typeof( KeyValuePair<,> )
					|| traits.ElementType == typeof( DictionaryEntry ) );
#endif
				var key = il.DeclareLocal( traits.ElementType.IsGenericType ? traits.ElementType.GetGenericArguments()[ 0 ] : typeof( MessagePackObject ), "key" );
				var value = il.DeclareLocal( traits.ElementType.IsGenericType ? traits.ElementType.GetGenericArguments()[ 1 ] : typeof( MessagePackObject ), "value" );

				il.EmitAnyLdarg( 0 );
				il.EmitGetProperty( _unpackerItemsCountProperty );
				il.EmitConv_Ovf_I4();
				il.EmitAnyStloc( itemsCount );
				loadCollectionEmitter( il, collection );
				Emittion.EmitFor(
					il,
					itemsCount,
					( il0, i ) =>
					{
						Action<TracingILGenerator, int> unpackerReading =
							( il1, unpacker ) =>
							{
								il1.EmitAnyLdarg( unpacker );
								il1.EmitAnyCall( _unpackerReadMethod );
								var endIf = il1.DefineLabel( "END_IF" );
								il1.EmitBrtrue_S( endIf );
								il1.EmitAnyLdloc( i );
								il1.EmitAnyCall( SerializationExceptions.NewMissingItemMethod );
								il1.EmitThrow();
								il1.MarkLabel( endIf );
							};

						// Key
						Emittion.EmitUnmarshalValue( il0, 0, 2, key, unpackerReading );

						// Value
						Emittion.EmitUnmarshalValue( il0, 0, 2, value, unpackerReading );

						il0.EmitAnyLdloc( collection );

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

				return dynamicMethod.CreateDelegate<Action<Unpacker, TObject, SerializationContext>>();
			}
			finally
			{
				il.FlushTrace();
				dynamicMethod.FlushTrace();
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
				unpackToIL.EmitGetProperty( _unpackerItemsCountProperty );
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
								il1.EmitAnyCall( _unpackerReadMethod );
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

		[Obsolete]
		protected sealed override bool CreateObjectProcedures( MemberInfo member, Type memberType, DataMemberContract contract, out Action<Packer, TObject, SerializationContext> packing, out Action<Unpacker, TObject, SerializationContext> unpacking )
		{
			packing = CreatePackObjectProceduresCore( member, memberType, contract );
			if ( packing == null )
			{
				unpacking = null;
				return false;
			}

			unpacking = CreateUnpackObjectProceduresCore( member, memberType, contract );
			return unpacking != null;
		}

		protected sealed override ConstructorInfo CreateObjectSerializer( MemberInfo member, Type memberType )
		{
			var emitter = SerializationMethodGeneratorManager.Get().CreateEmitter( memberType );
			CreatePackObjectProceduresCore( emitter, member, memberType );
			CreateUnpackObjectProceduresCore( emitter, member, memberType );
			return emitter.Create();
		}

		[Obsolete]
		private Action<Packer, TObject, SerializationContext> CreatePackObjectProceduresCore( MemberInfo member, Type memberType, DataMemberContract contract )
		{
			/*
			 * Context.Serializers.Get<T>().Serialize( packer, value.... );
			 */
			var dynamicMethod = SerializationMethodGeneratorManager.Get().CreateGenerator( "Pack", member.DeclaringType, contract.Name, null, _packingMethodParameters );
			var il = dynamicMethod.GetILGenerator();
			try
			{
				Emittion.EmitMarshalValue(
					il,
					0,
					2,
					memberType,
					il0 =>
					{
						il0.EmitAnyLdarg( 1 );
						Emittion.EmitLoadValue( il0, member );
					}
				);
				il.EmitRet();

				return dynamicMethod.CreateDelegate<Action<Packer, TObject, SerializationContext>>();
			}
			finally
			{
				il.FlushTrace();
				dynamicMethod.FlushTrace();
			}
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

		[Obsolete]
		private Action<Unpacker, TObject, SerializationContext> CreateUnpackObjectProceduresCore( MemberInfo member, Type memberType, DataMemberContract contract )
		{
			/*
			 * Context.Serializers.Get<T>().Deserialize( packer, value.... );
			 */
			var dynamicMethod = SerializationMethodGeneratorManager.Get().CreateGenerator( "Unpack", member.DeclaringType, contract.Name, null, _unpackingMethodParameters );
			var il = dynamicMethod.GetILGenerator();
			try
			{
				var itemsCount = il.DeclareLocal( typeof( int ), "itemsCount" );
				var collection = il.DeclareLocal( memberType, "collection" );
				Emittion.EmitUnmarshalValue(
					il,
					0,
					2,
					collection,
					null // Dispatching closure shall adjust position.
				);
				il.EmitAnyLdarg( 1 );
				il.EmitAnyLdloc( collection );
				Emittion.EmitStoreValue( il, member );
				il.EmitRet();

				return dynamicMethod.CreateDelegate<Action<Unpacker, TObject, SerializationContext>>();
			}
			finally
			{
				il.FlushTrace();
				dynamicMethod.FlushTrace();
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

		private static string BuildMethodName( string action )
		{
			return action + "_" + typeof( TObject ).FullName.Replace( Type.Delimiter, '_' );
		}
	}
}
