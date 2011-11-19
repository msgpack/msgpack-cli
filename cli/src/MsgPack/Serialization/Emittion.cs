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
using System.Linq;
using System.Text;
using NLiblet.Reflection;
using System.Reflection.Emit;
using System.Reflection;
using System.Diagnostics.Contracts;

namespace MsgPack.Serialization
{
	internal static class Emittion
	{
		private static readonly MethodInfo _packerPackStringMethod = FromExpression.ToMethod( ( Packer packer, String value ) => packer.PackString( value ) );
		private static readonly MethodInfo _idisposableDisposeMethod = FromExpression.ToMethod( ( IDisposable disposable ) => disposable.Dispose() );
		private static readonly PropertyInfo _unpackerIsMapHeaderProperty = FromExpression.ToProperty( ( Unpacker unpacker ) => unpacker.IsMapHeader );
		private static readonly PropertyInfo _unpackerIsArrayHeaderProperty = FromExpression.ToProperty( ( Unpacker unpacker ) => unpacker.IsArrayHeader );
		private static readonly MethodInfo _unpackerReadMethod = FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.Read() );

		/// <summary>
		///		Builds the name of the generating method.
		/// </summary>
		/// <param name="operation">The operation.</param>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="targetMemberName">Name of the target member.</param>
		/// <returns>Name of the method.</returns>
		public static string BuildMethodName( string operation, Type targetType, string targetMemberName )
		{
			return String.Join( "_", operation, targetType.FullName.Replace( Type.Delimiter, '_' ), targetMemberName );
		}

		/// <summary>
		///		Emits 'for' statement on current IL stream.
		/// </summary>
		/// <param name="il">IL generator to be emitted to.</param>
		/// <param name="count">'count' local variable which is <see cref="Int32"/> type and holds maximum loop count.</param>
		/// <param name="bodyEmitter">Delegate to emit for statement body.</param>
		public static void EmitFor( TracingILGenerator il, LocalBuilder count, Action<TracingILGenerator, LocalBuilder> bodyEmitter )
		{
			var i = il.DeclareLocal( typeof( int ), "i" );
			var forCond = il.DefineLabel( "FOR_COND" );
			il.MarkLabel( forCond );

			// cond
			il.EmitAnyLdloc( i );
			il.EmitAnyLdloc( count );
			var endFor = il.DefineLabel( "END_FOR" );
			il.EmitBeq_S( endFor );

			bodyEmitter( il, i );
			// increment
			il.EmitAnyLdloc( i );
			il.EmitLdc_I4_1();
			il.EmitAdd();
			il.EmitAnyStloc( i );
			il.EmitBr_S( forCond );
			il.MarkLabel( endFor );
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
			var enumerator = il.DeclareLocal( traits.GetEnumeratorMethod.ReturnType, "enumerator" );

			// gets enumerator
			il.EmitAnyLdloc( collection );
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
			var moveNextMethod = traits.GetEnumeratorMethod.ReturnType.GetMethod( "MoveNext", Type.EmptyTypes );
			if ( moveNextMethod.ReturnType != typeof( bool ) )
			{
				moveNextMethod = typeof( IEnumerator<> ).MakeGenericType( traits.ElementType ).GetMethod( "MoveNext", Type.EmptyTypes );
			}

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
			il.EmitBrfalse_S( endLoop );

			bodyEmitter(
				il,
				() =>
				{
					var currentProperty = traits.GetEnumeratorMethod.ReturnType.GetProperty( "Current" );
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

			il.EmitBr_S( startLoop );
			il.MarkLabel( endLoop );

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
						il.EmitAnyCall( _idisposableDisposeMethod );
					}
				}
				else
				{
					il.EmitAnyLdloc( enumerator );
					il.EmitAnyCall( _idisposableDisposeMethod );
				}

				il.EndExceptionBlock();
			}
		}

		/// <summary>
		///		Emits applopriate loading member instructions.
		/// </summary>
		/// <param name="il">IL generator to be emitted to.</param>
		/// <param name="member"><see cref="MemberInfo"/> to be loaded.</param>
		public static void EmitLoadValue( TracingILGenerator il, MemberInfo member )
		{
			Contract.Assert( member != null );

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
		///		Emits applopriate storing member instructions.
		/// </summary>
		/// <param name="il">IL generator to be emitted to.</param>
		/// <param name="member"><see cref="MemberInfo"/> to be stored.</param>
		public static void EmitStoreValue( TracingILGenerator il, MemberInfo member )
		{
			Contract.Assert( member != null );

			var asProperty = member as PropertyInfo;
			if ( asProperty != null )
			{
				il.EmitSetProperty( asProperty );
			}
			else
			{
				Contract.Assert( member is FieldInfo, member.ToString() + ":" + member.MemberType );
				il.EmitStfld( member as FieldInfo );
			}
		}

		/// <summary>
		///		Emits <see cref="M:Packer.PackString(String)"/> method.
		/// </summary>
		/// <param name="il">IL generator to be emitted to.</param>
		/// <param name="packerArgumentIndex">Index to packer argument.</param>
		/// <param name="literal">String literal to be emmitted.</param>
		public static void EmitPackLiteralString( TracingILGenerator il, int packerArgumentIndex, string literal )
		{
			// packer.PackString( "..." );
			il.EmitAnyLdarg( packerArgumentIndex );
			il.EmitLdstr( literal );
			il.EmitAnyCall( _packerPackStringMethod );
			il.EmitPop();
		}

		public static void EmitMarshalValue( TracingILGenerator il, int packerArgumentIndex, int contextArgumentIndex, Type valueType, Action<TracingILGenerator> loadValueEmitter )
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

		public static void EmitUnmarshalValue( TracingILGenerator il, int unpackerArgumentIndex, int contextArgumentIndex, Type valueType, Action<TracingILGenerator,int> unpackerReading )
		{
			var fastUnmarshal = MarshalerRepository.GetFastUnmarshalMethod( valueType );
			if ( fastUnmarshal != null )
			{
				il.EmitAnyLdarg( unpackerArgumentIndex );
				il.EmitAnyCall( fastUnmarshal );
			}
			else
			{		//  context.Marshalers.Get<T>().UnmarshalFrom( packer, ... ) )
				if ( unpackerReading != null )
				{
					unpackerReading( il, unpackerArgumentIndex );
				}

				il.EmitAnyLdarg( contextArgumentIndex );
				il.EmitAnyLdarg( unpackerArgumentIndex );
				il.EmitAnyCall( SerializationContext.UnmarshalFrom1Method.MakeGenericMethod( valueType ) );
			}
		}

		public static void EmitReadUnpackerIfNotInHeader( TracingILGenerator il, int unpackerArgumentIndex )
		{
			/*
			 *	if ( !unpacker.IsMapHeader && !unpacker.IsArrayHeader )
			 *	{
			 *		if ( !unpacker.Read() )
			 *		{
			 *			throw SerializationExceptions.NewCannotReadCollectionHeader();
			 *		}
			 *	}
			 */

			var endIf = il.DefineLabel( "END_IF" );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( _unpackerIsMapHeaderProperty );
			il.EmitBrtrue_S( endIf );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( _unpackerIsArrayHeaderProperty );
			il.EmitBrtrue_S( endIf );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitAnyCall( _unpackerReadMethod );
			il.EmitBrtrue_S( endIf );
			il.EmitAnyCall( SerializationExceptions.NewCannotReadCollectionHeaderMethod );
			il.EmitThrow();
			il.MarkLabel( endIf );
		}
	}
}
