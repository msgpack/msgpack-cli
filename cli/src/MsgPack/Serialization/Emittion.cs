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
using System.Globalization;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	// FIXME : restructuring
	internal static class Emittion
	{
		private static readonly MethodInfo _packerPackStringMethod = FromExpression.ToMethod( ( Packer packer, String value ) => packer.PackString( value ) );
		private static readonly MethodInfo _idisposableDisposeMethod = FromExpression.ToMethod( ( IDisposable disposable ) => disposable.Dispose() );
		private static readonly PropertyInfo _unpackerIsMapHeaderProperty = FromExpression.ToProperty( ( Unpacker unpacker ) => unpacker.IsMapHeader );
		private static readonly PropertyInfo _unpackerIsArrayHeaderProperty = FromExpression.ToProperty( ( Unpacker unpacker ) => unpacker.IsArrayHeader );
		private static readonly MethodInfo _unpackerReadMethod = FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.Read() );
		private static readonly MethodInfo _unpackerReadSubTreeMethod = FromExpression.ToMethod( ( Unpacker unpacker ) => unpacker.ReadSubtree() );
		private static readonly PropertyInfo _unpackerDataProperty = FromExpression.ToProperty( ( Unpacker unpacker ) => unpacker.Data );
		private static readonly PropertyInfo _nullableMessagePackObjectValueProperty = FromExpression.ToProperty( ( MessagePackObject? value ) => value.Value );
		private static readonly PropertyInfo _messagePackObjectIsNilProperty = FromExpression.ToProperty( ( MessagePackObject value ) => value.IsNil );
		private static readonly MethodInfo _ienumeratorMoveNextMethod = FromExpression.ToMethod( ( IEnumerator enumerator ) => enumerator.MoveNext() );
		private static readonly PropertyInfo _ienumeratorCurrentProperty = FromExpression.ToProperty( ( IEnumerator enumerator ) => enumerator.Current );
		private static readonly PropertyInfo _idictionaryEnumeratorCurrentProperty = FromExpression.ToProperty( ( IDictionaryEnumerator enumerator ) => enumerator.Entry );

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
			il.EmitBeq( endFor );

			bodyEmitter( il, i );
			// increment
			il.EmitAnyLdloc( i );
			il.EmitLdc_I4_1();
			il.EmitAdd();
			il.EmitAnyStloc( i );
			il.EmitBr( forCond );
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

		// TODO: Caching
		[Obsolete]
		public static void EmitMarshalValue( TracingILGenerator il, int packerArgumentIndex, int contextArgumentIndex, Type valueType, Action<TracingILGenerator> loadValueEmitter )
		{
			//  context.MarshalTo( packer, ... ) )
			il.EmitAnyLdarg( contextArgumentIndex );
			il.EmitAnyLdarg( packerArgumentIndex );
			loadValueEmitter( il );
			il.EmitAnyCall( SerializationContext.MarshalTo1Method.MakeGenericMethod( valueType ) );
		}

		public static void EmitMarshalValue( SerializerEmitter emitter, TracingILGenerator il, int packerArgumentIndex, Type valueType, Action<TracingILGenerator> loadValueEmitter )
		{
			var serializerField = emitter.RegisterSerializer( valueType );
			//  context.MarshalTo( packer, ... ) )
			il.EmitLdarg_0();
			il.EmitLdfld( serializerField );
			il.EmitAnyLdarg( packerArgumentIndex );
			loadValueEmitter( il );
			il.EmitAnyCall( serializerField.FieldType.GetMethod( "PackTo" ) );
		}

		// TODO: Caching
		[Obsolete]
		public static void EmitUnmarshalValue( TracingILGenerator il, int unpackerArgumentIndex, int contextArgumentIndex, LocalBuilder value, Action<TracingILGenerator, int> unpackerReading )
		{
			if ( unpackerReading != null )
			{
				unpackerReading( il, unpackerArgumentIndex );
			}

			/*
			 * if( unpacker.IsArrayHeader || unpacker.IsMapHeader )
			 * {
			 *		
			 *		context..UnmarshalFrom<T>( unpacker, ... ) )
			 * }
			 * else
			 * {
			 *		context..UnmarshalFrom<T>( unpacker, ... ) )
			 * }
			 */

			var then = il.DefineLabel( "THEN" );
			var endIf = il.DefineLabel( "END_IF" );

			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( _unpackerIsArrayHeaderProperty );
			il.EmitBrtrue_S( then );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( _unpackerIsMapHeaderProperty );
			il.EmitBrtrue_S( then );
			// else
			il.EmitAnyLdarg( contextArgumentIndex );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitAnyCall( SerializationContext.UnmarshalFrom1Method.MakeGenericMethod( value.LocalType ) );
			il.EmitAnyStloc( value );
			il.EmitBr_S( endIf );
			// then
			var subTreeUnpacker = il.DeclareLocal( typeof( Unpacker ), "subTreeUnpacker" );
			il.MarkLabel( then );
			EmitUnpackerBeginReadSubtree( il, unpackerArgumentIndex, subTreeUnpacker );
			il.EmitAnyLdarg( contextArgumentIndex );
			il.EmitAnyLdloc( subTreeUnpacker );
			il.EmitAnyCall( SerializationContext.UnmarshalFrom1Method.MakeGenericMethod( value.LocalType ) );
			il.EmitAnyStloc( value );
			EmitUnpackerEndReadSubtree( il, subTreeUnpacker );
			il.MarkLabel( endIf );
		}

		[Obsolete( "", true )]
		public static void EmitUnmarshalValue( TracingILGenerator il, LocalBuilder unpacker, int contextArgumentIndex, Type valueType, Action<TracingILGenerator, LocalBuilder> unpackerReading )
		{
			if ( unpackerReading != null )
			{
				unpackerReading( il, unpacker );
			}

			//  context.Marshalers.Get<T>().UnmarshalFrom( packer, ... ) )
			il.EmitAnyLdarg( contextArgumentIndex );
			il.EmitAnyLdloc( unpacker );
			il.EmitAnyCall( SerializationContext.UnmarshalFrom1Method.MakeGenericMethod( valueType ) );
		}

		public static void EmitUnmarshalValue( SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder value, Action<TracingILGenerator, int> unpackerReading )
		{
			if ( unpackerReading != null )
			{
				unpackerReading( il, unpackerArgumentIndex );
			}

			/*
			 * if( unpacker.IsArrayHeader || unpacker.IsMapHeader )
			 * {
			 *		
			 *		context..UnmarshalFrom<T>( unpacker, ... ) )
			 * }
			 * else
			 * {
			 *		context..UnmarshalFrom<T>( unpacker, ... ) )
			 * }
			 */

			var then = il.DefineLabel( "THEN" );
			var endIf = il.DefineLabel( "END_IF" );
			var serializerField = emitter.RegisterSerializer( value.LocalType );

			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( _unpackerIsArrayHeaderProperty );
			il.EmitBrtrue_S( then );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( _unpackerIsMapHeaderProperty );
			il.EmitBrtrue_S( then );
			// else
			il.EmitLdarg_0();
			il.EmitLdfld( serializerField );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitAnyCall( serializerField.FieldType.GetMethod( "UnpackFrom" ) );
			il.EmitAnyStloc( value );
			il.EmitBr_S( endIf );
			// then
			var subTreeUnpacker = il.DeclareLocal( typeof( Unpacker ), "subTreeUnpacker" );
			il.MarkLabel( then );
			EmitUnpackerBeginReadSubtree( il, unpackerArgumentIndex, subTreeUnpacker );
			il.EmitLdarg_0();
			il.EmitLdfld( serializerField );
			il.EmitAnyLdloc( subTreeUnpacker );
			il.EmitAnyCall( serializerField.FieldType.GetMethod( "UnpackFrom" ) );
			il.EmitAnyStloc( value );
			EmitUnpackerEndReadSubtree( il, subTreeUnpacker );
			il.MarkLabel( endIf );
		}

		public static void EmitUnmarshalCollectionValue( SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder target, MemberInfo member, Type memberType, Action<TracingILGenerator, int> unpackerReading )
		{
			if ( unpackerReading != null )
			{
				unpackerReading( il, unpackerArgumentIndex );
			}

			/*
			 * if( unpacker.IsArrayHeader || unpacker.IsMapHeader )
			 * {
			 *		
			 *		context..UnmarshalFrom<T>( unpacker, ... ) )
			 * }
			 * else
			 * {
			 *		context..UnmarshalFrom<T>( unpacker, ... ) )
			 * }
			 */

			var then = il.DefineLabel( "THEN" );
			var endIf = il.DefineLabel( "END_IF" );
			var serializerField = emitter.RegisterSerializer( memberType );

			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( _unpackerIsArrayHeaderProperty );
			il.EmitBrtrue_S( then );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( _unpackerIsMapHeaderProperty );
			il.EmitBrtrue_S( then );
			// else
			il.EmitTypeOf( memberType );
			il.EmitAnyCall( SerializationExceptions.NewTypeCannotDeserializeMethod );
			il.EmitThrow();
			// then
			var subTreeUnpacker = il.DeclareLocal( typeof( Unpacker ), "subTreeUnpacker" );
			il.MarkLabel( then );
			EmitUnpackerBeginReadSubtree( il, unpackerArgumentIndex, subTreeUnpacker );
			il.EmitLdarg_0();
			il.EmitLdfld( serializerField );
			il.EmitAnyLdloc( subTreeUnpacker );
			il.EmitAnyLdloc( target );
			Emittion.EmitLoadValue( il, member );
			var unpackTo = serializerField.FieldType.GetMethod( "UnpackTo", new[] { typeof( Unpacker ), memberType } );
			Contract.Assert( unpackTo != null, serializerField.FieldType + " does not declare UnpackTo(Unpacker," + memberType + ")" );
			il.EmitAnyCall( unpackTo );
			EmitUnpackerEndReadSubtree( il, subTreeUnpacker );
			il.MarkLabel( endIf );
		}

		public static void EmitUnpackerBeginReadSubtree( TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder subTreeUnpacker )
		{
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitAnyCall( _unpackerReadSubTreeMethod );
			il.EmitAnyStloc( subTreeUnpacker );
			il.BeginExceptionBlock();
		}

		public static void EmitUnpackerEndReadSubtree( TracingILGenerator il, LocalBuilder subTreeUnpacker )
		{
			il.BeginFinallyBlock();
			il.EmitAnyLdloc( subTreeUnpacker );
			var endIf = il.DefineLabel( "END_IF" );
			il.EmitBrfalse_S( endIf );
			il.EmitAnyLdloc( subTreeUnpacker );
			il.EmitAnyCall( _idisposableDisposeMethod );
			il.MarkLabel( endIf );
			il.EndExceptionBlock();
		}

		/// <summary>
		///		Emit member unpacking boiler plates.
		/// </summary>
		/// <param name="emitter"><see cref="SerializerEmitter"/> to register the filed which holds descendant serializers.</param>
		/// <param name="il"><see cref="TracingILGenerator"/> to emit instructions.</param>
		/// <param name="packerArgumentIndex">Packer argyument index. Note that the index of the first argument of instance method is 1, not 0.</param>
		/// <param name="targetType">Type of packing target.</param>
		/// <param name="targetArgumentIndex">Packing target argument index. Note that the index of the first argument of instance method is 1, not 0.</param>
		/// <param name="members">Member informations of the target. The 1st item is metadata of the member, and the 2nd item is type of the member value.</param>
		public static void EmitPackMambers( SerializerEmitter emitter, TracingILGenerator il, int packerArgumentIndex, Type targetType, int targetArgumentIndex, params Tuple<MemberInfo, Type>[] members )
		{
			il.EmitAnyLdarg( packerArgumentIndex );
			il.EmitAnyLdc_I4( members.Length );
			il.EmitAnyCall( Metadata._Packer.PackMapHeader );
			il.EmitPop();

			foreach ( var member in members )
			{
				il.EmitAnyLdarg( packerArgumentIndex );
				il.EmitLdstr( member.Item1.Name );
				il.EmitAnyCall( Metadata._Packer.PackString );
				il.EmitPop();
				Emittion.EmitMarshalValue(
					emitter,
					il,
					packerArgumentIndex,
					member.Item2,
					il0 =>
					{
						if ( targetType.IsValueType )
						{
							il0.EmitAnyLdarga( targetArgumentIndex );
						}
						else
						{
							il0.EmitAnyLdarg( targetArgumentIndex );
						}

						Emittion.EmitLoadValue( il0, member.Item1 );
					}
				);
			}
		}

		public static void EmitConstruction( TracingILGenerator il, Type type )
		{
			Contract.Assert( il != null );
			Contract.Assert( type != null );

			if ( type.IsValueType )
			{
				return;
			}

			// TODO: For collection, supports .ctor(IEnumerable<> other)

			var ctor = type.GetConstructor( Type.EmptyTypes );
			if ( ctor == null )
			{
				throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructor( type );
			}

			il.EmitNewobj( ctor );
		}

		/// <summary>
		///		Emit member unpacking boiler plates.
		/// </summary>
		/// <param name="emitter"><see cref="SerializerEmitter"/> to register the filed which holds descendant serializers.</param>
		/// <param name="il"><see cref="TracingILGenerator"/> to emit instructions.</param>
		/// <param name="unpackerArgumentIndex">Index of unpacker argument. Note that the index of the first argument of instance method is 1, not 0.</param>
		/// <param name="members">Tuple of member information that to be unpacked. The 1st item is metadata of the member, the 2nd the name of the member, the 3rd item is the local to be stored, and the 4th item is optional 'found' marker local.</param>
		public static void EmitUnpackMembers( SerializerEmitter emitter, TracingILGenerator il, int unpackerArgumentIndex, LocalBuilder target, params Tuple<MemberInfo, string, LocalBuilder, LocalBuilder>[] members )
		{
			// TODO: Compare mmember name as Byte[], not string to avoid string decoding (and its allocation).
			/*
			 *	// Assume subtree unpacker
			 *	while( unpacker.Read() )
			 *	{
			 *		var memberName = unpacker.Data.AsString();
			 *		if( memberName == "A" )
			 *		{
			 *			if( !unpacker.Read() )
			 *			{
			 *				throw SerializationExceptions.NewUnexpectedStreamEndsException();
			 *			}
			 *			
			 *			isAFound = true;
			 *		}
			 *		:
			 *	}
			 */

			var whileCond = il.DefineLabel( "WHILE_COND" );
			var endWhile = il.DefineLabel( "END_WHILE" );
			il.MarkLabel( whileCond );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitAnyCall( Metadata._Unpacker.Read );
			il.EmitBrfalse( endWhile );

			var data = il.DeclareLocal( typeof( MessagePackObject? ), "data" );
			var dataValue = il.DeclareLocal( typeof( MessagePackObject ), "dataValue" );
			var memberName = il.DeclareLocal( typeof( string ), "memberName" );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( Metadata._Unpacker.Data );
			il.EmitAnyStloc( data );
			il.EmitAnyLdloca( data );
			il.EmitGetProperty( Metadata._Nullable<MessagePackObject>.Value );
			il.EmitAnyStloc( dataValue );
			il.EmitAnyLdloca( dataValue );
			il.EmitAnyCall( Metadata._MessagePackObject.AsString );
			il.EmitAnyStloc( memberName );
			for ( int i = 0; i < members.Length; i++ )
			{
				// TODO: binary comparison
				il.EmitAnyLdloc( memberName );
				il.EmitLdstr( members[ i ].Item2 );
				il.EmitAnyCall( Metadata._String.op_Inequality );
				var endIf0 = il.DefineLabel( "END_IF_0_" + i );
				il.EmitBrtrue_S( endIf0 );
				il.EmitAnyLdarg( unpackerArgumentIndex );
				il.EmitAnyCall( Metadata._Unpacker.Read );
				var endIf1 = il.DefineLabel( "END_IF_1_" + i );
				il.EmitBrtrue_S( endIf1 );
				il.EmitAnyCall( SerializationExceptions.NewUnexpectedEndOfStreamMethod );
				il.EmitThrow();
				il.MarkLabel( endIf1 );

				if ( members[ i ].Item3 == null )
				{
					Emittion.EmitUnmarshalCollectionValue( emitter, il, unpackerArgumentIndex, target, members[ i ].Item1, members[ i ].Item1.GetMemberValueType(), null );
				}
				else
				{
					Emittion.EmitUnmarshalValue( emitter, il, unpackerArgumentIndex, members[ i ].Item3, null );
				}

				if ( members[ i ].Item4 != null )
				{
					il.EmitLdc_I4_1();
					il.EmitAnyStloc( members[ i ].Item4 );
				}

				il.EmitBr( whileCond );
				il.MarkLabel( endIf0 );
			}

			il.MarkLabel( endWhile );

			for ( int i = 0; i < members.Length; i++ )
			{
				if ( members[ i ].Item4 != null )
				{
					var endIf = il.DefineLabel( "END_IF_2_" + i );
					il.EmitAnyLdloc( members[ i ].Item4 );
					il.EmitBrtrue_S( endIf );
					il.EmitLdstr( members[ i ].Item2 );
					il.EmitAnyCall( SerializationExceptions.NewMissingPropertyMethod );
					il.EmitThrow();
					il.MarkLabel( endIf );
				}
			}
		}
	}
}
