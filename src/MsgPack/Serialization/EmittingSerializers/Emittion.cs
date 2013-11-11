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
	}
}
