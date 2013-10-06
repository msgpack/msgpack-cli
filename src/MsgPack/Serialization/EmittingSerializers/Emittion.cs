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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	// FIXME : comment
	// FIXME : local-variable sharing
	internal static partial class Emittion
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
				if ( !asProperty.CanSetValue() )
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
		/// <param name="localHolder">The <see cref="LocalVariableHolder"/> which holds shared local variable information.</param>
		public static void EmitGetUnpackerItemsCountAsInt32( TracingILGenerator il, int unpackerArgumentIndex, LocalVariableHolder localHolder )
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
			il.EmitAnyLdloca( localHolder.RawItemsCount );
			il.EmitInitobj( typeof( long ) );
			il.BeginExceptionBlock();
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( Metadata._Unpacker.ItemsCount );
			il.EmitAnyStloc( localHolder.RawItemsCount );
			il.BeginCatchBlock( typeof( InvalidOperationException ) );
			il.EmitAnyCall( SerializationExceptions.NewIsIncorrectStreamMethod );
			il.EmitThrow();
			il.EndExceptionBlock();

			il.EmitAnyLdloc( localHolder.RawItemsCount );
			il.EmitLdc_I8( Int32.MaxValue );
			var endIf = il.DefineLabel();
			il.EmitBle_S( endIf );
			il.EmitAnyCall( SerializationExceptions.NewIsTooLargeCollectionMethod );
			il.EmitThrow();
			il.MarkLabel( endIf );
			il.EmitAnyLdloc( localHolder.RawItemsCount );
			il.EmitConv_I4();
		}

		/// <summary>
		///		Emits the serializing value instructions.
		/// </summary>
		/// <param name="emitter">The emitter.</param>
		/// <param name="il">The il generator.</param>
		/// <param name="packerArgumentIndex">Index of the packer argument.</param>
		/// <param name="valueType">Type of the current member value.</param>
		/// <param name="memberName">Name of the current member.</param>
		/// <param name="nilImplication">The nil implication of the current member.</param>
		/// <param name="loadValueEmitter">The delegate which emits case specific value loading instructions.</param>
		/// <param name="localHolder">The <see cref="LocalVariableHolder"/> which holds shared local variable information.</param>
		public static void EmitSerializeValue( SerializerEmitter emitter, TracingILGenerator il, int packerArgumentIndex, Type valueType, string memberName, NilImplication nilImplication, Action<TracingILGenerator> loadValueEmitter, LocalVariableHolder localHolder )
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
			var value = localHolder.GetSerializingValue( valueType );
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

		/// <summary>
		/// Emits the deserialize value.
		/// </summary>
		/// <param name="emitter">The emitter.</param>
		/// <param name="il">The il generator.</param>
		/// <param name="unpackerArgumentIndex">Index of the unpacker argument.</param>
		/// <param name="result">The result local variable which represents the result of deserialization.</param>
		/// <param name="member">The metadata for nil implication. Specify <c>null</c> if nil implication is not needed.</param>
		/// <param name="localHolder">The <see cref="LocalVariableHolder"/> which holds shared local variable information.</param>
		public static void EmitDeserializeValue( SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder result, SerializingMember member, LocalVariableHolder localHolder )
		{
			Contract.Requires( emitter != null );
			Contract.Requires( il != null );
			Contract.Requires( unpackerArgumentIndex >= 0 );
			Contract.Requires( result != null );

			var endOfDeserialization = il.DefineLabel( "END_OF_DESERIALIZATION" );

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
			LocalBuilder value;
			bool useDummyNullableValue = false;
			if ( member.Member.GetMemberValueType().GetIsValueType() &&
				Nullable.GetUnderlyingType( member.Member.GetMemberValueType() ) == null )
			{
				// Use temporary nullable value for nil implication.
				value = localHolder.GetDeserializedValue(
					typeof( Nullable<> ).MakeGenericType( member.Member.GetMemberValueType() ) );
				useDummyNullableValue = true;
			}
			else
			{
				value = localHolder.GetDeserializedValue( member.Member.GetMemberValueType() );
			}

			if ( value.LocalType.GetIsValueType() )
			{
				il.EmitAnyLdloca( value );
				il.EmitInitobj( value.LocalType );
			}
			else
			{
				il.EmitLdnull();
				il.EmitAnyStloc( value );
			}

			EmitDeserializeValueCore( emitter, il, unpackerArgumentIndex, value, result.LocalType, member, member.Contract.Name, endOfDeserialization, localHolder );

			if ( result.LocalType.IsValueType )
			{
				il.EmitAnyLdloca( result );
			}
			else
			{
				il.EmitAnyLdloc( result );
			}

			if ( useDummyNullableValue )
			{
				il.EmitAnyLdloca( value );
				il.EmitGetProperty( typeof( Nullable<> ).MakeGenericType( member.Member.GetMemberValueType() ).GetProperty( "Value" ) );
			}
			else
			{
				il.EmitAnyLdloc( value );
			}

			EmitStoreValue( il, member.Member );

			il.MarkLabel( endOfDeserialization );
		}

		/// <summary>
		///		Emits deserializing value instructions.
		/// </summary>
		/// <param name="emitter">The emitter.</param>
		/// <param name="il">The il generator.</param>
		/// <param name="unpackerArgumentIndex">Index of the unpacker argument.</param>
		/// <param name="value">The value local variable which stores unpacked value.</param>
		/// <param name="targetType">The type of deserialzing type.</param>
		/// <param name="memberName">The name of the member.</param>
		/// <param name="localHolder">The <see cref="LocalVariableHolder"/> which holds shared local variable information.</param>
		public static void EmitDeserializeValueWithoutNilImplication( SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder value, Type targetType, string memberName, LocalVariableHolder localHolder )
		{
			Contract.Requires( emitter != null );
			Contract.Requires( il != null );
			Contract.Requires( unpackerArgumentIndex >= 0 );
			Contract.Requires( value != null );

			var endOfDeserialization = il.DefineLabel( "END_OF_DESERIALIZATION" );

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

			// Nil implication is not needed.
			EmitDeserializeValueCore( emitter, il, unpackerArgumentIndex, value, targetType, null, memberName, endOfDeserialization, localHolder );

			il.MarkLabel( endOfDeserialization );
		}

		/// <summary>
		/// Emits the deserialize value.
		/// </summary>
		/// <param name="emitter">The emitter.</param>
		/// <param name="il">The il generator.</param>
		/// <param name="unpackerArgumentIndex">Index of the unpacker argument.</param>
		/// <param name="value">The value local variable which stores unpacked value.</param>
		/// <param name="targetType">The type of deserialzing type.</param>
		/// <param name="member">The metadata for nil implication. Specify <c>null</c> if nil implication is not needed.</param>
		/// <param name="memberName">The name of the member.</param>
		/// <param name="endOfDeserialization">The end of deserialization label for nil implication.</param>
		/// <param name="localHolder">The <see cref="LocalVariableHolder"/> which holds shared local variable information.</param>
		private static void EmitDeserializeValueCore( SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder value, Type targetType, SerializingMember? member, string memberName, Label endOfDeserialization, LocalVariableHolder localHolder )
		{
			var directUnpacking = Metadata._Unpacker.GetDirectReadMethod( value.LocalType );
			if ( directUnpacking != null && ( member == null || !UnpackHelpers.IsReadOnlyAppendableCollectionMember( member.Value.Member ) ) )
			{
				var isSuccess = localHolder.IsDeserializationSucceeded;
				il.EmitLdc_I4_0();
				il.EmitAnyStloc( isSuccess );

				il.BeginExceptionBlock();
				il.EmitAnyLdarg( unpackerArgumentIndex );
				il.EmitAnyLdloca( value );
				il.EmitAnyCall( directUnpacking );
				il.EmitAnyStloc( isSuccess );
				il.BeginCatchBlock( typeof( MessageTypeException ) );
				var ex = localHolder.GetCatchedException( typeof( MessageTypeException ) );
				il.EmitAnyStloc( ex );
				il.EmitTypeOf( targetType );
				il.EmitLdstr( memberName );
				il.EmitAnyLdloc( ex );
				il.EmitAnyCall( SerializationExceptions.NewFailedToDeserializeMemberMethod );
				il.EmitThrow();
				il.EndExceptionBlock();
				var endIf0 = il.DefineLabel( "END_IF" );
				il.EmitAnyLdloc( isSuccess );
				il.EmitBrtrue_S( endIf0 );
				il.EmitAnyCall( SerializationExceptions.NewUnexpectedEndOfStreamMethod );
				il.EmitThrow();
				il.MarkLabel( endIf0 );
				if ( member != null )
				{
					// If null, nil implication is NOT needed.
					EmitNilImplicationForPrimitive( il, member.Value, value, endOfDeserialization );
				}
			}
			else
			{
				EmitGeneralRead( il, unpackerArgumentIndex );
				if ( member != null )
				{
					// If null, nil implication is NOT needed.
					EmitNilImplication(
						il,
						unpackerArgumentIndex,
						member.Value,
						endOfDeserialization,
						localHolder
					);
				}

				var thenIffCollection = il.DefineLabel( "THEN_IF_COLLECTION" );
				var endIfCollection = il.DefineLabel( "END_IF_COLLECTION" );

				/*
				 *	if( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				 *	{
				 *		value = GET_SERIALIZER().UnpackFrom( unpacker );
				 *	}
				 *	else
				 *	{
				 *		using( var subtreeUnpacker = unpacker.ReadSubtree() )
				 *		{
				 *			value = GET_SERIALIZER().UnpackFrom( subtreeUnpacker );
				 *		}
				 *	}
				 */

				il.EmitAnyLdarg( unpackerArgumentIndex );
				il.EmitGetProperty( Metadata._Unpacker.IsArrayHeader );
				il.EmitAnyLdarg( unpackerArgumentIndex );
				il.EmitGetProperty( Metadata._Unpacker.IsMapHeader );
				il.EmitOr();
				il.EmitBrtrue_S( thenIffCollection );
				EmitUnpackFrom( emitter, il, value, unpackerArgumentIndex );
				il.EmitBr_S( endIfCollection );
				var subtreeUnpacker = localHolder.SubtreeUnpacker;
				il.MarkLabel( thenIffCollection );
				EmitUnpackerBeginReadSubtree( il, unpackerArgumentIndex, subtreeUnpacker );
				EmitUnpackFrom( emitter, il, value, subtreeUnpacker );
				EmitUnpackerEndReadSubtree( il, subtreeUnpacker );
				il.MarkLabel( endIfCollection );
			}

		}

		/// <summary>
		///		Emits unpacking method with flavor specific getter.
		/// </summary>
		/// <param name="emitter">SerializerEmitter which knows the emittion flavor.</param>
		/// <param name="il">The il generator.</param>
		/// <param name="result">A variable wich stores unpacked result.</param>
		/// <param name="unpackerIndex">The argument index which stores current Unpacker.</param>
		public static void EmitUnpackFrom( SerializerEmitter emitter, TracingILGenerator il, LocalBuilder result, int unpackerIndex )
		{
			var serializerGetter = emitter.RegisterSerializer( result.LocalType );
			serializerGetter( il, 0 );
			il.EmitAnyLdarg( unpackerIndex );
			il.EmitAnyCall( Metadata._UnpackHelpers.InvokeUnpackFrom_1Method.MakeGenericMethod( result.LocalType ) );
			il.EmitAnyStloc( result );
		}

		/// <summary>
		///		Emits unpacking method with flavor specific getter.
		/// </summary>
		/// <param name="emitter">SerializerEmitter which knows the emittion flavor.</param>
		/// <param name="il">The il generator.</param>
		/// <param name="result">A variable wich stores unpacked result.</param>
		/// <param name="unpacker">The local variable which stores current Unpacker.</param>
		public static void EmitUnpackFrom( SerializerEmitter emitter, TracingILGenerator il, LocalBuilder result, LocalBuilder unpacker )
		{
			var serializerGetter = emitter.RegisterSerializer( result.LocalType );
			serializerGetter( il, 0 );
			il.EmitAnyLdloc( unpacker );
			il.EmitAnyCall( Metadata._UnpackHelpers.InvokeUnpackFrom_1Method.MakeGenericMethod( result.LocalType ) );
			il.EmitAnyStloc( result );
		}

		/// <summary>
		///		Emits the nil implication.
		/// </summary>
		/// <param name="il">The il generator.</param>
		/// <param name="unpackerArgumentIndex">Index of the unpacker argument.</param>
		/// <param name="member">Metadata of the serializing member.</param>
		/// <param name="endOfDeserialization">The label to the end of deserialization.</param>
		/// <param name="localHolder">The <see cref="LocalVariableHolder"/> which holds shared local variable information.</param>
		public static void EmitNilImplication(
			TracingILGenerator il,
			int unpackerArgumentIndex,
			SerializingMember member,
			Label endOfDeserialization,
			LocalVariableHolder localHolder
		)
		{
			switch ( member.Contract.NilImplication )
			{
				case NilImplication.MemberDefault:
				{
					// TODO: This should be empty for extra items.
					/*
						 * if( unpacker.Data.Value.IsNil )
						 * {
						 *		// Skip current.
						 *		goto END_OF_DESERIALIZATION;
						 * }
						 */
					il.EmitAnyLdarg( unpackerArgumentIndex );
					il.EmitGetProperty( Metadata._Unpacker.LastReadData );
					var data = localHolder.UnpackedData;
					il.EmitAnyStloc( data );
					il.EmitAnyLdloca( data );
					il.EmitGetProperty( Metadata._MessagePackObject.IsNil );
					il.EmitBrtrue( endOfDeserialization );

					break;
				}
				case NilImplication.Null:
				{
					if ( member.Member.GetMemberValueType().GetIsValueType() && Nullable.GetUnderlyingType( member.Member.GetMemberValueType() ) == null )
					{
						throw SerializationExceptions.NewValueTypeCannotBeNull(
							member.Contract.Name, member.Member.GetMemberValueType(), member.Member.DeclaringType 
						);
					}

					if ( !member.Member.CanSetValue() )
					{
						throw SerializationExceptions.NewReadOnlyMemberItemsMustNotBeNull(
							 member.Contract.Name
						);
					}

					break;
				}
				case NilImplication.Prohibit:
				{
					if ( !member.Member.CanSetValue() )
					{
						throw SerializationExceptions.NewReadOnlyMemberItemsMustNotBeNull(
							member.Contract.Name
						);
					}
					/*
						 * if( unpacker.Data.Value.IsNil )
						 * {
						 *		throw SerializationEceptions.NewProhibitNullException( "..." );
						 * }
						 */
					il.EmitAnyLdarg( unpackerArgumentIndex );
					il.EmitGetProperty( Metadata._Unpacker.LastReadData );
					var data = localHolder.UnpackedData;
					il.EmitAnyStloc( data );
					il.EmitAnyLdloca( data );
					il.EmitGetProperty( Metadata._MessagePackObject.IsNil );
					var endIf0 = il.DefineLabel( "END_IF0" );
					il.EmitBrfalse_S( endIf0 );
					il.EmitLdstr( member.Contract.Name );
					il.EmitAnyCall( SerializationExceptions.NewNullIsProhibitedMethod );
					il.EmitThrow();
					il.MarkLabel( endIf0 );

					break;
				}
			}
		}

		private static void EmitNilImplicationForPrimitive( TracingILGenerator il, SerializingMember member, LocalBuilder value, Label endOfDeserialization )
		{
			var endIf = il.DefineLabel( "END_IF_NULL" );
			EmitCompareNull( il, value, endIf );

			switch ( member.Contract.NilImplication )
			{
				case NilImplication.MemberDefault:
				{
					/*
					 * if( value == null )
					 * {
					 *		// Skip current.
					 *		goto END_OF_DESERIALIZATION;
					 * }
					 */
					il.EmitBr( endOfDeserialization );
					break;
				}
				case NilImplication.Null:
				{
					if ( member.Member.GetMemberValueType().GetIsValueType() && Nullable.GetUnderlyingType( member.Member.GetMemberValueType() ) == null)
					{
						throw SerializationExceptions.NewValueTypeCannotBeNull(
							member.Contract.Name, member.Member.GetMemberValueType(), member.Member.DeclaringType
						);
					}

					// TODO: Refactoring
					// Throw exception for non-nullable value type.
					// Nop for nullables.
					if ( member.Member.GetMemberValueType().GetIsValueType()
						&& Nullable.GetUnderlyingType( member.Member.GetMemberValueType() ) == null )
					{
						/*
						 * if( value == null )
						 * {
						 *		throw SerializationEceptions.NewValueTypeCannotBeNull( "...", typeof( MEMBER ), typeof( TYPE ) );
						 * }
						 */
						il.EmitLdstr( member.Contract.Name );
						il.EmitLdtoken( member.Member.GetMemberValueType() );
						il.EmitAnyCall( Metadata._Type.GetTypeFromHandle );
						il.EmitLdtoken( member.Member.DeclaringType );
						il.EmitAnyCall( Metadata._Type.GetTypeFromHandle );
						il.EmitAnyCall( SerializationExceptions.NewValueTypeCannotBeNull3Method );
						il.EmitThrow();
					}

					break;
				}
				case NilImplication.Prohibit:
				{
					/*
					 * if( value == null )
					 * {
					 *		throw SerializationEceptions.NewProhibitNullException( "..." );
					 * }
					 */
					il.EmitLdstr( member.Contract.Name );
					il.EmitAnyCall( SerializationExceptions.NewNullIsProhibitedMethod );
					il.EmitThrow();
					break;
				}
			}

			il.MarkLabel( endIf );
		}

		private static void EmitCompareNull( TracingILGenerator il, LocalBuilder value, Label targetIfNotNull )
		{
			if ( value.LocalType == typeof( MessagePackObject ) )
			{
				il.EmitAnyLdloca( value );
				il.EmitGetProperty( Metadata._MessagePackObject.IsNil );
				il.EmitBrfalse_S( targetIfNotNull );
			}
			else if ( value.LocalType.GetIsValueType() )
			{
				Contract.Assert( Nullable.GetUnderlyingType( value.LocalType ) != null, value.LocalType.FullName );
				il.EmitAnyLdloca( value );
				il.EmitGetProperty( value.LocalType.GetProperty( "HasValue" ) );
				il.EmitBrtrue_S( targetIfNotNull );
			}
			else // ref type
			{
				il.EmitAnyLdloc( value );
				il.EmitLdnull();
				il.EmitBne_Un_S( targetIfNotNull );
			}
		}

		/// <summary>
		/// Emits the deserialize collection value.
		/// </summary>
		/// <param name="emitter">The emitter.</param>
		/// <param name="il">The il generator.</param>
		/// <param name="unpackerArgumentIndex">Index of the unpacker argument.</param>
		/// <param name="target">The target collection variable.</param>
		/// <param name="member">The deserializing member metadata which holds the collection.</param>
		/// <param name="memberType">Type of the deserializing member.</param>
		/// <param name="nilImplication">The nil implication.</param>
		/// <param name="localHolder">The <see cref="LocalVariableHolder"/> which holds shared local variable information.</param>
		public static void EmitDeserializeCollectionValue( SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder target, MemberInfo member, Type memberType, NilImplication nilImplication, LocalVariableHolder localHolder )
		{
			Contract.Requires( emitter != null );
			Contract.Requires( il != null );
			Contract.Requires( unpackerArgumentIndex >= 0 );
			Contract.Requires( target != null );
			Contract.Requires( member != null );
			Contract.Requires( memberType != null );
			Contract.Requires( localHolder != null );

			var endOfDeserialization = il.DefineLabel( "END_OF_DESERIALIZATION" );

			EmitGeneralRead( il, unpackerArgumentIndex );

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
					il.EmitGetProperty( Metadata._Unpacker.LastReadData );
					var data = localHolder.UnpackedData;
					il.EmitAnyStloc( data );
					il.EmitAnyLdloca( data );
					il.EmitGetProperty( Metadata._MessagePackObject.IsNil );
					il.EmitBrtrue( endOfDeserialization );

					break;
				}
				case NilImplication.Null:
				case NilImplication.Prohibit:
				{
					/*
					 * // for Prohibit
					 * if( unpacker.Data.Value.IsNil )
					 * {
					 *		throw SerializationEceptions.NewProhibitNullException( "..." );
					 * }
					 * 
					 * // for Null, and 
					 * if( unpacker.Data.Value.IsNil )
					 * {
					 *		throw SerializationEceptions.NewReadOnlyMemberItemsMustNotBeNullMethod( "..." );
					 * }
					 */
					il.EmitAnyLdarg( unpackerArgumentIndex );
					il.EmitGetProperty( Metadata._Unpacker.LastReadData );
					var data = localHolder.UnpackedData;
					il.EmitAnyStloc( data );
					il.EmitAnyLdloca( data );
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
						// Because result member is readonly collection, so the member will not be null if packed value was nil.
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
			 *		GET_SERIALIZER.UnpackTo( unpacker, result ) )
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
			var subtreeUnpacker = localHolder.SubtreeUnpacker;
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

			// TODO: For collection, supports .ctor(IEnumerable<> other)

			if ( target.LocalType.IsAbstract || target.LocalType.IsInterface )
			{
				throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( target.LocalType );
			}

			if ( target.LocalType.IsArray )
			{
				Contract.Assert( initialCountLoadingEmitter != null );
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

		/// <summary>
		///		Emits reading next item operation from the unpacker.
		/// </summary>
		/// <param name="il">Writng IL generator.</param>
		/// <param name="unpackerArgumentIndex">The index of the Unpacker argument.</param>
		public static void EmitGeneralRead( TracingILGenerator il, int unpackerArgumentIndex )
		{
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitAnyCall( Metadata._Unpacker.Read );
			var endIf = il.DefineLabel( "END_IF" );
			il.EmitBrtrue_S( endIf );
			il.EmitAnyCall( SerializationExceptions.NewUnexpectedEndOfStreamMethod );
			il.EmitThrow();
			il.MarkLabel( endIf );
		}
	}
}
