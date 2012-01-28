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
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using NLiblet.Reflection;

namespace MsgPack.Serialization
{
	// FIXME : comment
	internal static class Emittion
	{
		private static readonly Type[] _ctor_Int32_ParameterTypes = new[] { typeof( int ) };

		/// <summary>
		///		Emits 'for' statement on current IL stream.
		/// </summary>
		/// <param name="il">IL generator to be emitted to.</param>
		/// <param name="count">'count' local variable which is <see cref="Int32"/> type and holds maximum loop count.</param>
		/// <param name="bodyEmitter">Delegate to emit for statement body.</param>
		public static void EmitFor( TracingILGenerator il, LocalBuilder count, Action<TracingILGenerator, LocalBuilder> bodyEmitter )
		{
			Contract.Requires( il != null );
			Contract.Requires( count != null );
			Contract.Requires( bodyEmitter != null );

			var i = il.DeclareLocal( typeof( int ), "i" );
			il.EmitLdc_I4_0();
			il.EmitAnyStloc( i );
			var forCond = il.DefineLabel( "FOR_COND" );
			il.EmitBr( forCond );
			var body = il.DefineLabel( "BODY" );
			il.MarkLabel( body );
			bodyEmitter( il, i );
			// increment
			il.EmitAnyLdloc( i );
			il.EmitLdc_I4_1();
			il.EmitAdd();
			il.EmitAnyStloc( i );
			// cond
			il.MarkLabel( forCond );
			il.EmitAnyLdloc( i );
			il.EmitAnyLdloc( count );
			il.EmitBlt( body );
		}

		/// <summary>
		///		Emits 'foreach' statement on the IL stream.
		/// </summary>
		/// <param name="il">IL generator to be emitted to.</param>
		/// <param name="traits"><see cref="CollectionTraits"/> which contains traits of the iterating collection.</param>
		/// <param name="collection">'collection' argument index.</param>
		/// <param name="bodyEmitter">Delegate to emit body statement.</param>
		public static void EmitForEach( TracingILGenerator il, CollectionTraits traits, LocalBuilder collection, Action<TracingILGenerator, Action> bodyEmitter )
		{
			Contract.Requires( il != null );
			Contract.Requires( collection != null );
			Contract.Requires( bodyEmitter != null );

			var enumerator = il.DeclareLocal( traits.GetEnumeratorMethod.ReturnType, "enumerator" );

			// gets enumerator
			if ( collection.LocalType.IsValueType )
			{
				il.EmitAnyLdloca( collection );
			}
			else
			{
				il.EmitAnyLdloc( collection );
			}

			il.EmitAnyCall( traits.GetEnumeratorMethod );
			il.EmitAnyStloc( enumerator );

			if ( typeof( IDisposable ).IsAssignableFrom( traits.GetEnumeratorMethod.ReturnType ) )
			{
				il.BeginExceptionBlock();
			}

			var hasNext = il.DeclareLocal( typeof( bool ), "hasNext" );
			var startLoop = il.DefineLabel( "START_LOOP" );
			il.MarkLabel( startLoop );
			var endLoop = il.DefineLabel( "END_LOOP" );
			var enumeratorType = traits.GetEnumeratorMethod.ReturnType;
			MethodInfo moveNextMethod = enumeratorType.GetMethod( "MoveNext", Type.EmptyTypes );
			PropertyInfo currentProperty = traits.GetEnumeratorMethod.ReturnType.GetProperty( "Current" );

			if ( moveNextMethod == null )
			{
				moveNextMethod = Metadata._IEnumerator.MoveNext;
			}

			if ( currentProperty == null )
			{
				if ( enumeratorType == typeof( IDictionaryEnumerator ) )
				{
					currentProperty = Metadata._IDictionaryEnumerator.Current;
				}
				else if ( enumeratorType.IsInterface )
				{
					if ( enumeratorType.IsGenericType && enumeratorType.GetGenericTypeDefinition() == typeof( IEnumerator<> ) )
					{
						currentProperty = typeof( IEnumerator<> ).MakeGenericType( traits.ElementType ).GetProperty( "Current" );
					}
					else
					{
						currentProperty = Metadata._IEnumerator.Current;
					}
				}
			}

			Contract.Assert( currentProperty != null, enumeratorType.ToString() );

			// iterates
			if ( traits.GetEnumeratorMethod.ReturnType.IsValueType )
			{
				il.EmitAnyLdloca( enumerator );
			}
			else
			{
				il.EmitAnyLdloc( enumerator );
			}

			il.EmitAnyCall( moveNextMethod );
			il.EmitBrfalse( endLoop );

			bodyEmitter(
				il,
				() =>
				{
					if ( traits.GetEnumeratorMethod.ReturnType.IsValueType )
					{
						il.EmitAnyLdloca( enumerator );
					}
					else
					{
						il.EmitAnyLdloc( enumerator );
					}
					il.EmitGetProperty( currentProperty );
				}
			);

			il.EmitBr( startLoop );
			il.MarkLabel( endLoop );

			// Dispose
			if ( typeof( IDisposable ).IsAssignableFrom( traits.GetEnumeratorMethod.ReturnType ) )
			{
				il.BeginFinallyBlock();

				if ( traits.GetEnumeratorMethod.ReturnType.IsValueType )
				{
					var disposeMethod = traits.GetEnumeratorMethod.ReturnType.GetMethod( "Dispose" );
					if ( disposeMethod != null && disposeMethod.GetParameters().Length == 0 && disposeMethod.ReturnType == typeof( void ) )
					{
						il.EmitAnyLdloca( enumerator );
						il.EmitAnyCall( disposeMethod );
					}
					else
					{
						il.EmitAnyLdloc( enumerator );
						il.EmitBox( traits.GetEnumeratorMethod.ReturnType );
						il.EmitAnyCall( Metadata._IDisposable.Dispose );
					}
				}
				else
				{
					il.EmitAnyLdloc( enumerator );
					il.EmitAnyCall( Metadata._IDisposable.Dispose );
				}

				il.EndExceptionBlock();
			}
		}

		/// <summary>
		///		Emits appropriate loading member instructions.
		/// </summary>
		/// <param name="il">IL generator to be emitted to.</param>
		/// <param name="member"><see cref="MemberInfo"/> to be loaded.</param>
		public static void EmitLoadValue( TracingILGenerator il, MemberInfo member )
		{
			Contract.Requires( il != null );
			Contract.Requires( member != null );

			var asProperty = member as PropertyInfo;
			if ( asProperty != null )
			{
				il.EmitGetProperty( asProperty );
			}
			else
			{
				Contract.Assert( member is FieldInfo, member.ToString() + ":" + member.MemberType );
				il.EmitLdfld( member as FieldInfo );
			}
		}

		/// <summary>
		///		Emits appropriate storing member instructions.
		/// </summary>
		/// <param name="il">IL generator to be emitted to.</param>
		/// <param name="member"><see cref="MemberInfo"/> to be stored.</param>
		public static void EmitStoreValue( TracingILGenerator il, MemberInfo member )
		{
			Contract.Requires( il != null );
			Contract.Requires( member != null );

			var asProperty = member as PropertyInfo;
			if ( asProperty != null )
			{
				if ( !asProperty.CanWrite )
				{
					throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot set value to '{0}.{1}' property.", asProperty.DeclaringType, asProperty.Name ) );
				}

				il.EmitSetProperty( asProperty );
			}
			else
			{
				Contract.Assert( member is FieldInfo, member.ToString() + ":" + member.MemberType );
				var asField = member as FieldInfo;
				if ( asField.IsInitOnly )
				{
					throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "Cannot set value to '{0}.{1}' field.", asField.DeclaringType, asField.Name ) );
				}

				il.EmitStfld( asField );
			}
		}

		/// <summary>
		/// Emits gets <see cref="Unpacker.ItemsCount"/> with exception handling.
		/// Note that final state is the value is pushed top of the evaluation stack.
		/// </summary>
		/// <param name="il">IL generator.</param>
		/// <param name="unpackerArgumentIndex">Argument index of the unpacker.</param>
		public static void EmitGetUnpackerItemsCountAsInt32( TracingILGenerator il, int unpackerArgumentIndex )
		{
			Contract.Requires( il != null );
			Contract.Requires( unpackerArgumentIndex >= 0 );

			/*
			 *	long rawItemsCount;
			 *	try
			 *	{
			 *		rawItemsCount = unpacker.ItemsCount;
			 *	}
			 *	catch ( InvalidOperationException ex )
			 *	{
			 *		throw SerializationExceptions.NewIsIncorrectStream( ex );
			 *	}
			 * 
			 *	if( rawItemsCount > Int32.MaxValue )
			 *	{
			 *		throw SerializationException.NewIsTooLargeCollection(); 
			 *	}
			 * 
			 *	... unchecked( ( int )rawItemsCount );
			 */
			var rawItemsCount = il.DeclareLocal( typeof( long ), "rawItemsCount" );

			il.BeginExceptionBlock();
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( Metadata._Unpacker.ItemsCount );
			il.EmitAnyStloc( rawItemsCount );
			il.BeginCatchBlock( typeof( InvalidOperationException ) );
			var ex = il.DeclareLocal( typeof( InvalidOperationException ), "ex" );
			il.EmitAnyCall( SerializationExceptions.NewIsIncorrectStreamMethod );
			il.EmitThrow();
			il.EndExceptionBlock();

			il.EmitAnyLdloc( rawItemsCount );
			il.EmitLdc_I8( Int32.MaxValue );
			var endIf = il.DefineLabel();
			il.EmitBle_S( endIf );
			il.EmitAnyCall( SerializationExceptions.NewIsTooLargeCollectionMethod );
			il.EmitThrow();
			il.MarkLabel( endIf );
			il.EmitAnyLdloc( rawItemsCount );
			il.EmitConv_I4();
		}

		public static void EmitSerializeValue( SerializerEmitter emitter, TracingILGenerator il, int packerArgumentIndex, Type valueType, string memberName, NilImplication nilImplication, Action<TracingILGenerator> loadValueEmitter )
		{
			Contract.Requires( emitter != null );
			Contract.Requires( il != null );
			Contract.Requires( packerArgumentIndex >= 0 );
			Contract.Requires( valueType != null );
			Contract.Requires( loadValueEmitter != null );

			/*
			 * var serializingValue = LOAD_VALUE;
			 * NULL_PROHIBIT_HANDLING
			 * GET_SERIALIZER.PackTo( packer, serializingValue );
			 */
			var value = il.DeclareLocal( valueType, "serializingValue" );
			loadValueEmitter( il );
			il.EmitAnyStloc( value );
			if ( memberName != null && nilImplication == NilImplication.Prohibit )
			{
				/*
				 *	if( serializingValue == null )(
				 *	{
				 *		throw SerializationExceptions.NewNullIsProhibited();
				 *	}
				 */

				if ( !valueType.IsValueType )
				{
					il.EmitAnyLdloc( value );
					var endIf = il.DefineLabel( "END_IF" );
					il.EmitBrtrue_S( endIf );
					il.EmitLdstr( memberName );
					il.EmitAnyCall( SerializationExceptions.NewNullIsProhibitedMethod );
					il.EmitThrow();
					il.MarkLabel( endIf );
				}
				else if ( Nullable.GetUnderlyingType( valueType ) != null )
				{
					il.EmitAnyLdloca( value );
					il.EmitGetProperty( typeof( Nullable<> ).MakeGenericType( Nullable.GetUnderlyingType( valueType ) ).GetProperty( "HasValue" ) );
					var endIf = il.DefineLabel( "END_IF" );
					il.EmitBrtrue_S( endIf );
					il.EmitLdstr( memberName );
					il.EmitAnyCall( SerializationExceptions.NewNullIsProhibitedMethod );
					il.EmitThrow();
					il.MarkLabel( endIf );
				}
			}

			var serializerGetter = emitter.RegisterSerializer( valueType );
			serializerGetter( il, 0 );
			il.EmitAnyLdarg( packerArgumentIndex );
			il.EmitAnyLdloc( value );
			il.EmitAnyCall( typeof( MessagePackSerializer<> ).MakeGenericType( valueType ).GetMethod( "PackTo" ) );
		}

		public static void EmitDeserializeValue( SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder value, LocalBuilder isUnpacked, string memberName, NilImplication nilImplication, Action<TracingILGenerator, int> customUnpackerReading )
		{
			Contract.Requires( emitter != null );
			Contract.Requires( il != null );
			Contract.Requires( unpackerArgumentIndex >= 0 );
			Contract.Requires( value != null );

			if ( customUnpackerReading != null )
			{
				customUnpackerReading( il, unpackerArgumentIndex );
			}

			var endOfDeserialization = il.DefineLabel( "END_OF_DESERIALIZATION" );
			if ( memberName != null )
			{
				switch ( nilImplication )
				{
					case NilImplication.MemberDefault:
					{
						/*
						 * if( unpacker.Data.Value.IsNil )
						 * {
						 *		// Skip current.
						 *		goto END_OF_DESERIALIZATION;
						 * }
						 */
						il.EmitAnyLdarg( unpackerArgumentIndex );
						il.EmitGetProperty( Metadata._Unpacker.Data );
						var data = il.DeclareLocal( typeof( MessagePackObject? ), "data" );
						il.EmitAnyStloc( data );
						il.EmitAnyLdloca( data );
						il.EmitGetProperty( Metadata._Nullable<MessagePackObject>.Value );
						var dataValue = il.DeclareLocal( typeof( MessagePackObject ), "dataValue" );
						il.EmitAnyStloc( dataValue );
						il.EmitAnyLdloca( dataValue );
						il.EmitGetProperty( Metadata._MessagePackObject.IsNil );
						il.EmitBrtrue( endOfDeserialization );

						break;
					}
					case NilImplication.Prohibit:
					{
						/*
						 * if( unpacker.Data.Value.IsNil )
						 * {
						 *		throw SerializationEceptions.NewProhibitNullException( "..." );
						 * }
						 */
						il.EmitAnyLdarg( unpackerArgumentIndex );
						il.EmitGetProperty( Metadata._Unpacker.Data );
						var data = il.DeclareLocal( typeof( MessagePackObject? ), "data" );
						il.EmitAnyStloc( data );
						il.EmitAnyLdloca( data );
						il.EmitGetProperty( Metadata._Nullable<MessagePackObject>.Value );
						var dataValue = il.DeclareLocal( typeof( MessagePackObject ), "dataValue" );
						il.EmitAnyStloc( dataValue );
						il.EmitAnyLdloca( dataValue );
						il.EmitGetProperty( Metadata._MessagePackObject.IsNil );
						var endIf0 = il.DefineLabel( "END_IF0" );
						il.EmitBrfalse_S( endIf0 );
						il.EmitLdstr( memberName );
						il.EmitAnyCall( SerializationExceptions.NewNullIsProhibitedMethod );
						il.EmitThrow();
						il.MarkLabel( endIf0 );

						break;
					}
				}
			}

			/*
			 * 
			 * if( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
			 * {
			 *		valueN = GET_SERIALIZER.UnpackFrom( unpacker );
			 * }
			 * else
			 * {
			 *		using( var subtreeUnpacker = unpacker.ReadSubtree )
			 *		{
			 *			valueN = GET_SERIALIZER.UnpackFrom( unpacker );
			 *		}
			 * }
			 * 
			 * isValueNUnpacked = true;
			 * END_OF_DESERIALIZATION:
			 */

			var then = il.DefineLabel( "THEN" );
			var endIf = il.DefineLabel( "END_IF" );
			var serializerGetter = emitter.RegisterSerializer( value.LocalType );

			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( Metadata._Unpacker.IsArrayHeader );
			il.EmitBrtrue_S( then );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( Metadata._Unpacker.IsMapHeader );
			il.EmitBrtrue_S( then );
			// else
			serializerGetter( il, 0 );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitAnyCall( typeof( MessagePackSerializer<> ).MakeGenericType( value.LocalType ).GetMethod( "UnpackFrom" ) );
			il.EmitAnyStloc( value );
			il.EmitBr_S( endIf );
			// then
			var subtreeUnpacker = il.DeclareLocal( typeof( Unpacker ), "subtreeUnpacker" );
			il.MarkLabel( then );
			EmitUnpackerBeginReadSubtree( il, unpackerArgumentIndex, subtreeUnpacker );
			serializerGetter( il, 0 );
			il.EmitAnyLdloc( subtreeUnpacker );
			il.EmitAnyCall( typeof( MessagePackSerializer<> ).MakeGenericType( value.LocalType ).GetMethod( "UnpackFrom" ) );
			il.EmitAnyStloc( value );
			EmitUnpackerEndReadSubtree( il, subtreeUnpacker );
			il.MarkLabel( endIf );

			if ( isUnpacked != null )
			{
				il.EmitLdc_I4_1();
				il.EmitAnyStloc( isUnpacked );
			}

			il.MarkLabel( endOfDeserialization );
		}

		public static void EmitDeserializeCollectionValue( SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder target, MemberInfo member, Type memberType, NilImplication nilImplication )
		{
			Contract.Requires( emitter != null );
			Contract.Requires( il != null );
			Contract.Requires( unpackerArgumentIndex >= 0 );
			Contract.Requires( target != null );
			Contract.Requires( member != null );
			Contract.Requires( memberType != null );

			var endOfDeserialization = il.DefineLabel( "END_OF_DESERIALIZATION" );
			switch ( nilImplication )
			{
				case NilImplication.MemberDefault:
				{
					/*
					 * if( unpacker.Data.Value.IsNil )
					 * {
					 *		// Skip current.
					 *		goto END_OF_DESERIALIZATION;
					 * }
					 */
					il.EmitAnyLdarg( unpackerArgumentIndex );
					il.EmitGetProperty( Metadata._Unpacker.Data );
					var data = il.DeclareLocal( typeof( MessagePackObject? ), "data" );
					il.EmitAnyStloc( data );
					il.EmitAnyLdloca( data );
					il.EmitGetProperty( Metadata._Nullable<MessagePackObject>.Value );
					var dataValue = il.DeclareLocal( typeof( MessagePackObject ), "dataValue" );
					il.EmitAnyStloc( dataValue );
					il.EmitAnyLdloca( dataValue );
					il.EmitGetProperty( Metadata._MessagePackObject.IsNil );
					il.EmitBrtrue( endOfDeserialization );

					break;
				}
				case NilImplication.Null:
				case NilImplication.Prohibit:
				{
					/*
					 * if( unpacker.Data.Value.IsNil )
					 * {
					 *		throw SerializationEceptions.NewProhibitNullException( "..." );
					 * }
					 */
					il.EmitAnyLdarg( unpackerArgumentIndex );
					il.EmitGetProperty( Metadata._Unpacker.Data );
					var data = il.DeclareLocal( typeof( MessagePackObject? ), "data" );
					il.EmitAnyStloc( data );
					il.EmitAnyLdloca( data );
					il.EmitGetProperty( Metadata._Nullable<MessagePackObject>.Value );
					var dataValue = il.DeclareLocal( typeof( MessagePackObject ), "dataValue" );
					il.EmitAnyStloc( dataValue );
					il.EmitAnyLdloca( dataValue );
					il.EmitGetProperty( Metadata._MessagePackObject.IsNil );
					var endIf0 = il.DefineLabel( "END_IF0" );
					il.EmitBrfalse_S( endIf0 );
					il.EmitLdstr( member.Name );
					if ( nilImplication == NilImplication.Prohibit )
					{
						il.EmitAnyCall( SerializationExceptions.NewNullIsProhibitedMethod );
					}
					else
					{
						il.EmitAnyCall( SerializationExceptions.NewReadOnlyMemberItemsMustNotBeNullMethod );
					}
					il.EmitThrow();
					il.MarkLabel( endIf0 );

					break;
				}
			}

			/*
			 *	if( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
			 *	{
			 *		throw new SerializatonException( "Cannot deserialize..." );
			 *	}
			 * 
			 *	using( var subtreeUnpacker = unpacker.ReadSubtree() )
			 *	{
			 *		GET_SERIALIZER.UnpackTo( unpacker, target ) )
			 *	}
			 *	
			 *	END_OF_DESERIALIZATION:
			 */

			var endIf = il.DefineLabel( "THEN" );
			var serializerGetter = emitter.RegisterSerializer( memberType );

			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( Metadata._Unpacker.IsArrayHeader );
			il.EmitBrtrue_S( endIf );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( Metadata._Unpacker.IsMapHeader );
			il.EmitBrtrue_S( endIf );
			// else
			il.EmitLdstr( member.Name );
			il.EmitAnyCall( SerializationExceptions.NewStreamDoesNotContainCollectionForMemberMethod );
			il.EmitThrow();
			// then
			var subtreeUnpacker = il.DeclareLocal( typeof( Unpacker ), "subtreeUnpacker" );
			il.MarkLabel( endIf );
			EmitUnpackerBeginReadSubtree( il, unpackerArgumentIndex, subtreeUnpacker );
			serializerGetter( il, 0 );
			il.EmitAnyLdloc( subtreeUnpacker );
			il.EmitAnyLdloc( target );
			Emittion.EmitLoadValue( il, member );
			il.EmitAnyCall( typeof( MessagePackSerializer<> ).MakeGenericType( memberType ).GetMethod( "UnpackTo", new[] { typeof( Unpacker ), memberType } ) );
			EmitUnpackerEndReadSubtree( il, subtreeUnpacker );
			il.MarkLabel( endOfDeserialization );
		}

		public static void EmitUnpackerBeginReadSubtree( TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder subtreeUnpacker )
		{
			Contract.Requires( il != null );
			Contract.Requires( unpackerArgumentIndex >= 0 );
			Contract.Requires( subtreeUnpacker != null );

			/*
			 * subtreeUnpacker = unpacker.ReadSubtree()
			 */

			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitAnyCall( Metadata._Unpacker.ReadSubtree );
			il.EmitAnyStloc( subtreeUnpacker );
			il.BeginExceptionBlock();
		}

		public static void EmitUnpackerEndReadSubtree( TracingILGenerator il, LocalBuilder subtreeUnpacker )
		{
			Contract.Requires( il != null );
			Contract.Requires( subtreeUnpacker != null );

			/*
			 *	finally
			 *	{
			 *		if( subtreeUnpacker != null )
			 *		{
			 *			subtreeUnpacker.Dispose();
			 *		}
			 *	}
			 */

			il.BeginFinallyBlock();
			il.EmitAnyLdloc( subtreeUnpacker );
			var endIf = il.DefineLabel( "END_IF" );
			il.EmitBrfalse_S( endIf );
			il.EmitAnyLdloc( subtreeUnpacker );
			il.EmitAnyCall( Metadata._IDisposable.Dispose );
			il.MarkLabel( endIf );
			il.EndExceptionBlock();
		}

		public static void EmitConstruction( TracingILGenerator il, LocalBuilder target, Action<TracingILGenerator> initialCountLoadingEmitter )
		{
			Contract.Requires( il != null );
			Contract.Requires( target != null );
			Contract.Requires( initialCountLoadingEmitter != null );
			
			// TODO: For collection, supports .ctor(IEnumerable<> other)

			if ( target.LocalType.IsArray )
			{
				initialCountLoadingEmitter( il );
				il.EmitNewarr( target.LocalType.GetElementType() );
				il.EmitAnyStloc( target );
				return;
			}

			ConstructorInfo ctor = target.LocalType.GetConstructor( _ctor_Int32_ParameterTypes );
			if ( ctor != null && initialCountLoadingEmitter != null && typeof( IEnumerable ).IsAssignableFrom( target.LocalType ) )
			{
				if ( target.LocalType.IsValueType )
				{
					// Same as general method call
					var capacity = il.DeclareLocal( typeof( int ), "capacity" );
					initialCountLoadingEmitter( il );
					il.EmitAnyStloc( capacity );
					il.EmitAnyLdloca( target );
					il.EmitAnyLdloc( capacity );
					il.EmitCallConstructor( ctor );
				}
				else
				{
					initialCountLoadingEmitter( il );
					il.EmitNewobj( ctor );
					il.EmitAnyStloc( target );
				}
				return;
			}

			if ( target.LocalType.IsValueType )
			{
				// ValueType instance has been initialized by the runtime.
				return;
			}

			ctor = target.LocalType.GetConstructor( Type.EmptyTypes );
			if ( ctor == null )
			{
				throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity( target.LocalType );
			}

			il.EmitNewobj( ctor );
			il.EmitAnyStloc( target );
		}
	}
}
