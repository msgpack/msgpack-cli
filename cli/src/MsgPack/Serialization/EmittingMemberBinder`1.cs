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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using NLiblet.Reflection;
using System.Globalization;
using System.Collections;

namespace MsgPack.Serialization
{
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

		protected sealed override Action<Packer, TObject, SerializationContext> CreatePacking( MemberInfo member, Type memberType, DataMemberContract contract )
		{
			var dynamicMethod = SerializationMethodGeneratorManager.Get().CreateGenerator( "Pack", member.DeclaringType, contract.Name, null, _packingMethodParameters );
			var il = dynamicMethod.GetILGenerator();
			try
			{
				var value = il.DeclareLocal( memberType, "value" );
				Emittion.EmitPackLiteralString( il, 0, contract.Name );
				Emittion.EmitMarshalValue(
					il, 0, 2, value.LocalType,
					il0 =>
					{
						il0.EmitLdarg_1();
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

		protected sealed override Action<Unpacker, TObject, SerializationContext> CreateUnpacking( MemberInfo member, Type memberType, DataMemberContract contract )
		{
			var dynamicMethod = SerializationMethodGeneratorManager.Get().CreateGenerator( "Unpack", member.DeclaringType, contract.Name, null, _unpackingMethodParameters );
			var il = dynamicMethod.GetILGenerator();
			try
			{
				var value = il.DeclareLocal( memberType, "value" );
				var data = il.DeclareLocal( typeof( MessagePackObject ), "data" );
				il.EmitAnyLdarg( 0 );
				il.EmitGetProperty( _unpackerDataProperty );
				il.EmitGetProperty( typeof( Nullable<MessagePackObject> ).GetProperty( "Value" ) );
				il.EmitAnyStloc( data );

				var endIf = il.DefineLabel( "END_IF" );
				var endOfMethod = il.DefineLabel( "END_OF_METHOD" );
				il.EmitAnyLdloc( data );
				il.EmitGetProperty( _messagePackObjectIsNilProperty );
				il.EmitBrfalse_S( endIf );
				if ( memberType.IsValueType )
				{
					// throw SerializationExceptions.NewValueTypeCannotBeNull( "..."< typeof( ... ), typeof( ... ) );
					il.EmitLdstr( member.Name );
					il.EmitTypeOf( memberType );
					il.EmitTypeOf( member.DeclaringType );
					il.EmitAnyCall( SerializationExceptions.NewValueTypeCannotBeNullMethod );
					il.EmitThrow();
				}
				else
				{
					// target.... = null;
					il.EmitAnyLdarg( 1 );
					il.EmitLdnull();
					Emittion.EmitStoreValue( il, member );
					il.EmitBr_S( endOfMethod );
				}

				il.MarkLabel( endIf );
				// target.... = context.UnmarshalFrom<T>( unpacker );
				Emittion.EmitUnmarshalValue( il, 0, 2, memberType, null ); // Dispatching closure shall adjust position.
				Emittion.EmitStoreValue( il, member );
				il.MarkLabel( endOfMethod );
				il.EmitRet();
				return dynamicMethod.CreateDelegate<Action<Unpacker, TObject, SerializationContext>>();
			}
			finally
			{
				dynamicMethod.FlushTrace();
				il.FlushTrace();
			}
		}

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

				Emittion.EmitReadUnpackerIfNotInHeader( il, 0 );
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
						Emittion.EmitUnmarshalValue( il0, 0, 2, key.LocalType, unpackerReading );
						il0.EmitAnyStloc( key );

						// Value
						Emittion.EmitUnmarshalValue( il0, 0, 2, value.LocalType, unpackerReading );
						il0.EmitAnyStloc( value );

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
				il.EmitAnyLdarg( 1 );
				Emittion.EmitUnmarshalValue(
					il,
					0,
					2,
					memberType,
					null // Dispatching closure shall adjust position.
				);
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

		private static string BuildMethodName( string action )
		{
			return action + "_" + typeof( TObject ).FullName.Replace( Type.Delimiter, '_' );
		}
	}
}
