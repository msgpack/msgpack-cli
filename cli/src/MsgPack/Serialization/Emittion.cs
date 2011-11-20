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
using System.Collections;

namespace MsgPack.Serialization
{
	internal static class Emittion
	{
		private static readonly MethodInfo _packerPackStringMethod = FromExpression.ToMethod( ( Packer packer, String value ) => packer.PackString( value ) );
		private static readonly MethodInfo _idisposableDisposeMethod = FromExpression.ToMethod( ( IDisposable disposable ) => disposable.Dispose() );
		private static readonly PropertyInfo _unpackerIsMapHeaderProperty = FromExpression.ToProperty( ( Unpacker unpacker ) => unpacker.IsMapHeader );
		private static readonly PropertyInfo _unpackerIsArrayHeaderProperty = FromExpression.ToProperty( ( Unpacker unpacker ) => unpacker.IsArrayHeader );
		private static readonly MethodInfo _unpackerReadMethod = FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.Read() );
		private static readonly PropertyInfo _unpackerDataProperty = FromExpression.ToProperty( ( Unpacker unpacker ) => unpacker.Data );
		private static readonly PropertyInfo _nullableMessagePackObjectValueProperty = FromExpression.ToProperty( ( MessagePackObject? value ) => value.Value );
		private static readonly PropertyInfo _messagePackObjectIsNilProperty = FromExpression.ToProperty( ( MessagePackObject value ) => value.IsNil );
		private static readonly MethodInfo _ienumeratorMoveNextMethod = FromExpression.ToMethod( ( IEnumerator enumerator ) => enumerator.MoveNext() );
		private static readonly PropertyInfo _ienumeratorCurrentProperty = FromExpression.ToProperty( ( IEnumerator enumerator ) => enumerator.Current );
		private static readonly PropertyInfo _idictionaryEnumeratorCurrentProperty = FromExpression.ToProperty( ( IDictionaryEnumerator enumerator ) => enumerator.Entry );

		/// <summary>
		///		Builds the name of the generating method.
		/// </summary>
		/// <param name="operation">The operation.</param>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="targetMemberName">Name of the target member.</param>
		/// <returns>Name of the method.</returns>
		public static string BuildMethodName( string operation, Type targetType, string targetMemberName )
		{
			return String.Join( "_", operation, targetType.GetFullName().Replace( "[]", "Array" ).Replace( Type.Delimiter, '_' ).Replace( '`', '_' ).Replace( '[', '_' ).Replace( ']', '_' ), targetMemberName );
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
			var enumeratorType = traits.GetEnumeratorMethod.ReturnType;
			MethodInfo moveNextMethod = enumeratorType.GetMethod( "MoveNext", Type.EmptyTypes );
			PropertyInfo currentProperty = traits.GetEnumeratorMethod.ReturnType.GetProperty( "Current" );

			if ( moveNextMethod == null )
			{
				moveNextMethod = _ienumeratorMoveNextMethod;
			}

			if ( currentProperty == null )
			{
				if ( enumeratorType == typeof( IDictionaryEnumerator ) )
				{
					currentProperty = _idictionaryEnumeratorCurrentProperty;
				}
				else if ( enumeratorType.IsInterface )
				{
					if ( enumeratorType.IsGenericType && enumeratorType.GetGenericTypeDefinition() == typeof( IEnumerator<> ) )
					{
						currentProperty = typeof( IEnumerator<> ).MakeGenericType( traits.ElementType ).GetProperty( "Current" );
					}
					else
					{
						currentProperty = _ienumeratorCurrentProperty;
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
			il.EmitBrfalse_S( endLoop );

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
			//  context.MarshalTo( packer, ... ) )
			il.EmitAnyLdarg( contextArgumentIndex );
			il.EmitAnyLdarg( packerArgumentIndex );
			loadValueEmitter( il );
			il.EmitAnyCall( SerializationContext.MarshalTo1Method.MakeGenericMethod( valueType ) );
		}

		public static void EmitUnmarshalValue( TracingILGenerator il, int unpackerArgumentIndex, int contextArgumentIndex, Type valueType, Action<TracingILGenerator, int> unpackerReading )
		{
			if ( unpackerReading != null )
			{
				unpackerReading( il, unpackerArgumentIndex );
			}

			//  context.Marshalers.Get<T>().UnmarshalFrom( packer, ... ) )
			il.EmitAnyLdarg( contextArgumentIndex );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitAnyCall( SerializationContext.UnmarshalFrom1Method.MakeGenericMethod( valueType ) );
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
