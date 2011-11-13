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

namespace MsgPack.Serialization
{
	/// <summary>
	///		<see cref="SerializerBuilder{T}"/> implementation using Reflection.Emit.
	/// </summary>
	/// <typeparam name="TObject">Object to be serialized/deserialized.</typeparam>
	internal sealed class EmittingMemberBinder<TObject> : SerializerBuilder<TObject>
	{
		public TextWriter Trace { get; set; }

		private static readonly MethodInfo _packerPackStringMethod = FromExpression.ToMethod( ( Packer packer, String value ) => packer.PackString( value ) );
		private static readonly MethodInfo _packerPackMessagePackObject = FromExpression.ToMethod( ( Packer packer, MessagePackObject value ) => packer.Pack( value ) );
		private static readonly PropertyInfo _unpackerDataProperty = FromExpression.ToProperty( ( Unpacker unpacker ) => unpacker.Data );
		private static readonly PropertyInfo _messagePackObjectIsNilProperty = FromExpression.ToProperty( ( MessagePackObject value ) => value.IsNil );
		private static readonly PropertyInfo _unpackerItemsCountProperty = FromExpression.ToProperty( ( Unpacker unpadker ) => unpadker.ItemsCount );
		private static readonly MethodInfo _unpackerMoveToNextEntryMethod = FromExpression.ToInstanceMethod( ( Unpacker unpacker ) => unpacker.MoveToNextEntry() );
		private static readonly MethodInfo _packerPackArrayHeaderMethod = FromExpression.ToMethod( ( Packer packer, int count ) => packer.PackArrayHeader( count ) );
		private static readonly MethodInfo _packerPackMapHeaderMethod = FromExpression.ToMethod( ( Packer packer, int count ) => packer.PackMapHeader( count ) );
		private static readonly Type[] _packingMethodParameters = new[] { typeof( Packer ), typeof( TObject ), typeof( SerializationContext ) };
		private static readonly Type[] _unpackingMethodParameters = new[] { typeof( Unpacker ), typeof( TObject ), typeof( SerializationContext ) };

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

			packing = CreatePacking( packings );
			unpacking = CreateUnpacking( unpackings );

			return true;
		}

		private Action<Packer, TObject, SerializationContext> CreatePacking( Action<Packer, TObject, SerializationContext>[] packings )
		{
			return
				( packer, target, context ) =>
				{
					foreach ( var packing in packings )
					{
						packing( packer, target, context );
					}
				};
		}

		private Func<Unpacker, SerializationContext, TObject> CreateUnpacking( Action<Unpacker, TObject, SerializationContext>[] unpackings )
		{
			var ctor = CreateObjectInitializer();
			return
				( unpacker, context ) =>
				{
					TObject target = ctor();

					foreach ( var unpacking in unpackings )
					{
						unpacking( unpacker, target, context );
					}

					return target;
				};
		}

		private static Func<TObject> CreateObjectInitializer()
		{
			if ( typeof( TObject ).IsValueType )
			{
				return () => default( TObject );
			}

			var ctor = typeof( TObject ).GetConstructor( Type.EmptyTypes );
			if ( ctor == null )
			{
				throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructor( typeof( TObject ) );
			}

			return Expression.Lambda<Func<TObject>>( Expression.New( ctor ) ).Compile();
		}

		protected sealed override Action<Packer, TObject, SerializationContext> CreatePacking( MemberInfo member, Type memberType, DataMemberContract contract )
		{
			var dynamicMethod = new DynamicMethod( BuildMethodName( "Pack", member.DeclaringType, contract.Name ), null, GetPackingMethodParameters() );
			this.Trace.WriteLine();
			this.Trace.WriteLine( "CreatePacking::" );
			this.Trace.WriteLine( dynamicMethod );
			var il = new TracingILGenerator( dynamicMethod, this.Trace );
			var value = il.DeclareLocal( memberType );
			EmitPackString( il, 0, contract.Name );
			EmitMarshalValue(
				il, 0, 2, value.LocalType,
				il0 =>
				{
					il0.EmitLdarg_1();
					GetLoadValueEmitter( member )( il0 );
				}
			);
			il.EmitRet();
			return dynamicMethod.CreateDelegate<Action<Packer, TObject, SerializationContext>>();
		}

		protected sealed override Action<Unpacker, TObject, SerializationContext> CreateUnpacking( MemberInfo member, Type memberType, DataMemberContract contract )
		{
			var dynamicMethod = new DynamicMethod( BuildMethodName( "Unpack", member.DeclaringType, contract.Name ), null, GetUnpackingMethodParameters() );
			this.Trace.WriteLine();
			this.Trace.WriteLine( "CreateUnpacking::" );
			this.Trace.WriteLine( dynamicMethod );
			var il = new TracingILGenerator( dynamicMethod, this.Trace );
			var value = il.DeclareLocal( memberType );
			var data = il.DeclareLocal( typeof( MessagePackObject ) );
			// var data = unpacker.Data.Value;
			il.EmitAnyLdarg( 0 );
			il.EmitGetProperty( _unpackerDataProperty );
			il.EmitGetProperty( typeof( Nullable<MessagePackObject> ).GetProperty( "Value" ) );
			il.EmitAnyStloc( data.LocalIndex );

			var endIf = il.DefineLabel( "END_IF" );
			il.EmitAnyLdloc( data.LocalIndex );
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
				GetStoreValueEmitter( member )( il );
				il.EmitRet();
			}

			il.MarkLabel( endIf );
			// target.... = context.UnmarshalFrom<T>( unpacker );
			EmitUnmarshalValue( il, 0, 2, memberType );
			GetStoreValueEmitter( member )( il );
			il.EmitRet();
			return dynamicMethod.CreateDelegate<Action<Unpacker, TObject, SerializationContext>>();
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

		private Action<Packer, TObject, SerializationContext> CreatePackArrayProceduresCore( MemberInfo member, Type memberType, DataMemberContract contract, CollectionTraits traits )
		{
			/*
			 * var collection = value...
			 * var count = collection.Count;
			 * packer.PackArrayHeader( count );
			 */
			var dynamicMethod = new DynamicMethod( BuildMethodName( "Pack", member.DeclaringType, contract.Name ), null, GetPackingMethodParameters() );
			this.Trace.WriteLine();
			this.Trace.WriteLine( "CreatePackArrayProceduresCore::" );
			this.Trace.WriteLine( dynamicMethod );
			var il = new TracingILGenerator( dynamicMethod, this.Trace );
			var collection = il.DeclareLocal( memberType );
			var data = il.DeclareLocal( typeof( MessagePackObject ) );
			il.EmitAnyLdarg( 1 );
			GetLoadValueEmitter( member )( il );
			il.EmitAnyStloc( collection.LocalIndex );
			var count = il.DeclareLocal( typeof( int ) );
			il.EmitAnyLdloc( collection.LocalIndex );
			il.EmitGetProperty( traits.CountProperty );
			il.EmitAnyStloc( count.LocalIndex );
			il.EmitAnyLdarg( 0 );
			il.EmitAnyCall( _packerPackArrayHeaderMethod );
			// Array
			if ( memberType.IsArray )
			{
				/*
				 * // array
				 * for( int i = 0; i < length; i++ )
				 * {
				 * 		Context.Serializers.Get<T>().Serialize( packer, array[ i ] );
				 * }
				 */
				EmitFor(
					il,
					count,
					( il0, i ) =>
					{
						EmitMarshalValue(
							il0,
							0,
							2,
							traits.ElementType,
							il1 =>
							{
								il1.EmitAnyLdloc( collection.LocalIndex );
								il1.EmitAnyLdloc( i.LocalIndex );
								il1.EmitLdelem( traits.ElementType );
							}
						);
					}
				);
				il.EmitRet();
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
				var currentItem = il.DeclareLocal( traits.ElementType );
				EmitForEach(
					il,
					traits,
					currentItem,
					( il0, getCurrentEmitter ) =>
					{
						EmitMarshalValue(
							il0,
							0,
							2,
							traits.ElementType,
							_ => getCurrentEmitter()
						);
					}
				);
				il.EmitRet();
			}

			return dynamicMethod.CreateDelegate<Action<Packer, TObject, SerializationContext>>();
		}

		private Action<Unpacker, TObject, SerializationContext> CreateUnpackArrayProceduresCore( MemberInfo member, Type memberType, DataMemberContract contract, CollectionTraits traits )
		{
			/*
			 * int itemCount = unpacker.ItemCount;
			 * var collection = target....;
			 * for( int i = 0; i < array.Length; i++ )
			 * {
			 *		if( !unpacker.Read() )
			 *		{
			 *			throw new SerializationException();
			 *		}
			 *		collection.Add( Context.Serializers.Get<T>().Deserialize( unpacker ) )
			 * }
			 */
			var dynamicMethod = new DynamicMethod( BuildMethodName( "Unpack", member.DeclaringType, contract.Name ), null, GetUnpackingMethodParameters() );
			this.Trace.WriteLine();
			this.Trace.WriteLine( "CreateUnpackArrayProceduresCore::" );
			this.Trace.WriteLine( dynamicMethod );
			var il = new TracingILGenerator( dynamicMethod, this.Trace );
			var itemsCount = il.DeclareLocal( typeof( int ) );
			var collection = il.DeclareLocal( memberType );

			il.EmitAnyLdarg( 0 );
			il.EmitGetProperty( _unpackerItemsCountProperty );
			il.EmitAnyStloc( itemsCount.LocalIndex );
			il.EmitAnyLdarg( 1 );
			GetLoadValueEmitter( member )( il );
			il.EmitAnyStloc( collection.LocalIndex );
			EmitFor(
				il,
				itemsCount,
				( il0, i ) =>
				{
					il0.EmitAnyLdarg( 0 );
					il0.EmitAnyCall( _unpackerMoveToNextEntryMethod );
					var endIf = il0.DefineLabel( "END_IF" );
					il0.EmitBrfalse_S( endIf );
					il0.EmitAnyLdloc( i.LocalIndex );
					il0.EmitAnyCall( SerializationExceptions.NewMissingItemMethod );
					il0.EmitThrow();
					il0.MarkLabel( endIf );
					il0.EmitAnyLdloc( collection.LocalIndex );
					EmitUnmarshalValue( il0, 0, 2, memberType );
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

		protected sealed override bool CreateMapProcedures( MemberInfo member, Type memberType, DataMemberContract contract, CollectionTraits traits, out Action<Packer, TObject, SerializationContext> packing, out Action<Unpacker, TObject, SerializationContext> unpacking )
		{
			packing = CreatePackMapProceduresCore( member, memberType, contract, traits );
			if ( packing == null )
			{
				unpacking = null;
				return false;
			}

			unpacking = CreateUnpackMapProceduresCore( member, memberType, contract, traits );
			return unpacking != null;
		}

		private Action<Packer, TObject, SerializationContext> CreatePackMapProceduresCore( MemberInfo member, Type memberType, DataMemberContract contract, CollectionTraits traits )
		{
			var dynamicMethod = new DynamicMethod( BuildMethodName( "Pack", member.DeclaringType, contract.Name ), null, GetPackingMethodParameters() );
			this.Trace.WriteLine();
			this.Trace.WriteLine( "CreatePackMapProceduresCore::" );
			this.Trace.WriteLine( dynamicMethod );
			var il = new TracingILGenerator( dynamicMethod, this.Trace );
			/*
			 * 
			 * // Enumerable
			 * foreach( var item in map )
			 * {
			 * 		Context.MarshalTo( packer, array[ i ] );
			 * }
			 */
			var collection = il.DeclareLocal( memberType );
			var data = il.DeclareLocal( typeof( MessagePackObject ) );
			il.EmitAnyLdarg( 1 );
			GetLoadValueEmitter( member )( il );
			il.EmitAnyStloc( collection.LocalIndex );
			var count = il.DeclareLocal( typeof( int ) );
			il.EmitAnyLdloc( collection.LocalIndex );
			il.EmitGetProperty( traits.CountProperty );
			il.EmitAnyStloc( count.LocalIndex );
			il.EmitAnyLdarg( 0 );
			il.EmitAnyCall( _packerPackMapHeaderMethod );
			var currentItem = il.DeclareLocal( traits.ElementType );
			EmitForEach(
				il,
				traits,
				currentItem,
				( il0, getCurrentEmitter ) =>
				{
					EmitMarshalValue(
						il0,
						0,
						2,
						traits.ElementType,
						_ => getCurrentEmitter()
					);
				}
			);
			il.EmitRet();

			return dynamicMethod.CreateDelegate<Action<Packer, TObject, SerializationContext>>();
		}

		private Action<Unpacker, TObject, SerializationContext> CreateUnpackMapProceduresCore( MemberInfo member, Type memberType, DataMemberContract contract, CollectionTraits traits )
		{
			/*
			 * int itemCount = unpacker.ItemCount;
			 * var collection = target....;
			 * for( int i = 0; i < array.Length; i++ )
			 * {
			 *		if( !unpacker.Read() )
			 *		{
			 *			throw new SerializationException();
			 *		}
			 *		collection.Add( Context.Serializers.Get<T>().Deserialize( unpacker ) )
			 * }
			 */
			var dynamicMethod = new DynamicMethod( BuildMethodName( "Unpack", member.DeclaringType, contract.Name ), null, GetUnpackingMethodParameters() );
			this.Trace.WriteLine();
			this.Trace.WriteLine( "CreateUnpackMapProceduresCore::" );
			this.Trace.WriteLine( dynamicMethod );
			var il = new TracingILGenerator( dynamicMethod, this.Trace );
			var itemsCount = il.DeclareLocal( typeof( int ) );
			var collection = il.DeclareLocal( memberType );
#if DEBUG
			Contract.Assert( traits.ElementType.IsGenericType && traits.ElementType.GetGenericTypeDefinition() == typeof( KeyValuePair<,> ), traits.ElementType.FullName );
#endif
			var key = il.DeclareLocal( traits.ElementType.GetGenericArguments()[ 0 ] );
			var value = il.DeclareLocal( traits.ElementType.GetGenericArguments()[ 1 ] );

			il.EmitAnyLdarg( 0 );
			il.EmitGetProperty( _unpackerItemsCountProperty );
			il.EmitAnyStloc( itemsCount.LocalIndex );
			il.EmitAnyLdarg( 1 );
			GetLoadValueEmitter( member )( il );
			il.EmitAnyStloc( collection.LocalIndex );
			EmitFor(
				il,
				itemsCount,
				( il0, i ) =>
				{
					// Key
					il0.EmitAnyLdarg( 0 );
					il0.EmitAnyCall( _unpackerMoveToNextEntryMethod );
					var endIf0 = il0.DefineLabel( "END_IF0" );
					il0.EmitBrfalse_S( endIf0 );
					il0.EmitAnyLdloc( i.LocalIndex );
					il0.EmitAnyCall( SerializationExceptions.NewMissingItemMethod );
					il0.EmitThrow();
					il0.MarkLabel( endIf0 );
					EmitUnmarshalValue( il0, 0, 2, memberType );
					il0.EmitAnyStloc( key.LocalIndex );

					// Value
					il0.EmitAnyLdarg( 0 );
					il0.EmitAnyCall( _unpackerMoveToNextEntryMethod );
					var endIf1 = il0.DefineLabel( "END_IF1" );
					il0.EmitBrfalse_S( endIf1 );
					il0.EmitAnyLdloc( i.LocalIndex );
					il0.EmitAnyCall( SerializationExceptions.NewMissingItemMethod );
					il0.EmitThrow();
					il0.MarkLabel( endIf1 );
					EmitUnmarshalValue( il0, 0, 2, memberType );
					il0.EmitAnyStloc( value.LocalIndex );

					il0.EmitAnyLdloc( collection.LocalIndex );
					il0.EmitAnyLdloc( key.LocalIndex );
					il0.EmitAnyLdloc( value.LocalIndex );
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
			var dynamicMethod = new DynamicMethod( BuildMethodName( "Pack", member.DeclaringType, contract.Name ), null, GetPackingMethodParameters() );
			this.Trace.WriteLine();
			this.Trace.WriteLine( "CreatePackObjectProceduresCore::" );
			this.Trace.WriteLine( dynamicMethod );
			var il = new TracingILGenerator( dynamicMethod, this.Trace );
			EmitMarshalValue(
				il,
				0,
				2,
				memberType,
				il0 =>
				{
					il0.EmitAnyLdarg( 1 );
					GetLoadValueEmitter( member )( il0 );
				}
			);
			il.EmitRet();

			return dynamicMethod.CreateDelegate<Action<Packer, TObject, SerializationContext>>();
		}

		private Action<Unpacker, TObject, SerializationContext> CreateUnpackObjectProceduresCore( MemberInfo member, Type memberType, DataMemberContract contract )
		{
			/*
			 * Context.Serializers.Get<T>().Deserialize( packer, value.... );
			 */
			var dynamicMethod = new DynamicMethod( BuildMethodName( "Unpack", member.DeclaringType, contract.Name ), null, GetUnpackingMethodParameters() );
			this.Trace.WriteLine();
			this.Trace.WriteLine( "CreateUnpackObjectProceduresCore::" );
			this.Trace.WriteLine( dynamicMethod );
			var il = new TracingILGenerator( dynamicMethod, this.Trace );
			var itemsCount = il.DeclareLocal( typeof( int ) );
			var collection = il.DeclareLocal( memberType );
			il.EmitAnyLdarg( 1 );
			EmitUnmarshalValue( il, 0, 2, memberType );
			GetStoreValueEmitter( member )( il );
			il.EmitRet();

			return dynamicMethod.CreateDelegate<Action<Unpacker, TObject, SerializationContext>>();
		}

		private static string BuildMethodName( string action )
		{
			return action + "_" + typeof( TObject ).FullName.Replace( Type.Delimiter, '_' );
		}

		private static string BuildMethodName( string action, Type targetType, string targetMember )
		{
			return String.Join( "_", action, targetType.FullName.Replace( Type.Delimiter, '_' ), targetMember );
		}

		private static Type[] GetPackingMethodParameters()
		{
			return _packingMethodParameters;
		}

		private static Type[] GetUnpackingMethodParameters()
		{
			return _unpackingMethodParameters;
		}

		private static void EmitFor( TracingILGenerator il, LocalBuilder count, Action<TracingILGenerator, LocalBuilder> bodyEmitter )
		{
			var i = il.DeclareLocal( typeof( int ) );
			var forCond = il.DefineLabel( "FOR_COND" );
			il.MarkLabel( forCond );
			il.EmitAnyLdloc( count.LocalIndex );
			var endFor = il.DefineLabel( "END_FOR" );
			il.EmitBeq_S( endFor );

			bodyEmitter( il, i );
			// incr
			il.EmitAnyLdloc( i.LocalIndex );
			il.EmitLdc_I4_1();
			il.EmitAdd();
			il.EmitAnyStloc( i.LocalIndex );
			il.EmitBr_S( forCond );
			il.MarkLabel( endFor );
		}

		private static void EmitForEach( TracingILGenerator il, CollectionTraits traits, LocalBuilder currentItem, Action<TracingILGenerator, Action> bodyEmitter )
		{
			var enumerator = il.DeclareLocal( traits.GetEnumeratorMethod.ReturnType );
			il.EmitAnyCall( traits.GetEnumeratorMethod );
			var hasNext = il.DeclareLocal( typeof( bool ) );
			var startLoop = il.DefineLabel( "START_LOOP" );
			il.MarkLabel( startLoop );
			var endLoop = il.DefineLabel( "END_LOOP" );
			var moveNextMethod = traits.GetEnumeratorMethod.ReturnType.GetMethod( "MoveNext", Type.EmptyTypes );
			if ( moveNextMethod.ReturnType != typeof( bool ) )
			{
				moveNextMethod = typeof( IEnumerator<> ).MakeGenericType( traits.ElementType ).GetMethod( "MoveNext", Type.EmptyTypes );
			}

			if ( traits.GetEnumeratorMethod.ReturnType.IsValueType )
			{
				il.EmitLdloca_S( ( byte )enumerator.LocalIndex );
			}
			else
			{
				il.EmitAnyLdloc( enumerator.LocalIndex );
			}

			il.EmitAnyCall( moveNextMethod );
			il.EmitBrfalse_S( endLoop );

			bodyEmitter( il, () =>
			{
				var currentProperty = traits.GetEnumeratorMethod.ReturnType.GetProperty( "Current" );
				il.EmitGetProperty( currentProperty );
			} );

			il.EmitBr_S( startLoop );
			il.MarkLabel( endLoop );
		}

		private static Action<TracingILGenerator> GetLoadValueEmitter( MemberInfo member )
		{
			Contract.Assert( member != null );

			var asProperty = member as PropertyInfo;
			if ( asProperty != null )
			{
				return il => il.EmitGetProperty( asProperty );
			}
			else
			{
				Contract.Assert( member is FieldInfo, member.ToString() + ":" + member.MemberType );
				return il => il.EmitLdfld( member as FieldInfo );
			}
		}

		private static Action<TracingILGenerator> GetStoreValueEmitter( MemberInfo member )
		{
			Contract.Assert( member != null );

			var asProperty = member as PropertyInfo;
			if ( asProperty != null )
			{
				return il => il.EmitSetProperty( asProperty );
			}
			else
			{
				Contract.Assert( member is FieldInfo, member.ToString() + ":" + member.MemberType );
				return il => il.EmitStfld( member as FieldInfo );
			}
		}

		private static void EmitPackString( TracingILGenerator il, int packerArgumentIndex, string value )
		{
			// packer.PackString( "..." );
			il.EmitAnyLdarg( packerArgumentIndex );
			il.EmitLdstr( value );
			il.EmitAnyCall( _packerPackStringMethod );
			il.EmitPop();
		}

		private static void EmitMarshalValue( TracingILGenerator il, int packerArgumentIndex, int contextArgumentIndex, Type valueType, Action<TracingILGenerator> loadValueEmitter )
		{
			var fastMarshal = MarshalerRepository.GetFastMarshalMethod( valueType );
			if ( fastMarshal != null )
			{
				il.EmitAnyLdarg( packerArgumentIndex );
				loadValueEmitter( il );
				il.EmitAnyCall( fastMarshal );
				if ( fastMarshal.ReturnType != typeof( void ) )
				{
					il.EmitPop();
				}
			}
			else
			{
				//  context.MarshalTo( packer, ... ) )
				il.EmitAnyLdarg( contextArgumentIndex );
				il.EmitAnyLdarg( packerArgumentIndex );
				loadValueEmitter( il );
				il.EmitAnyCall( SerializationContext.MarshalTo1Method.MakeGenericMethod( valueType ) );
			}
		}

		private static void EmitUnmarshalValue( TracingILGenerator il, int unpackerArgumentIndex, int contextArgumentIndex, Type valueType )
		{
			var fastUnmarshal = MarshalerRepository.GetFastUnmarshalMethod( valueType );
			if ( fastUnmarshal != null )
			{
				il.EmitAnyLdarg( unpackerArgumentIndex );
				il.EmitAnyCall( fastUnmarshal );
			}
			else
			{		//  context.Marshalers.Get<T>().UnmarshalFrom( packer, ... ) )
				il.EmitAnyLdarg( contextArgumentIndex );
				il.EmitAnyLdarg( unpackerArgumentIndex );
				il.EmitAnyCall( SerializationContext.UnmarshalFrom1Method.MakeGenericMethod( valueType ) );
			}
		}
	}
}
