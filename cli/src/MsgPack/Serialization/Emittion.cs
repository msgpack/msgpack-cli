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
		// TODO: -> Metadata
		private static readonly MethodInfo _ienumeratorMoveNextMethod = FromExpression.ToMethod( ( IEnumerator enumerator ) => enumerator.MoveNext() );
		private static readonly PropertyInfo _ienumeratorCurrentProperty = FromExpression.ToProperty( ( IEnumerator enumerator ) => enumerator.Current );
		private static readonly PropertyInfo _idictionaryEnumeratorCurrentProperty = FromExpression.ToProperty( ( IDictionaryEnumerator enumerator ) => enumerator.Entry );
		private static readonly Type[] _ctor_Int32_ParameterTypes = new[] { typeof( int ) };

		/// <summary>
		///		Emits 'for' statement on current IL stream.
		/// </summary>
		/// <param name="il">IL generator to be emitted to.</param>
		/// <param name="count">'count' local variable which is <see cref="Int32"/> type and holds maximum loop count.</param>
		/// <param name="bodyEmitter">Delegate to emit for statement body.</param>
		public static void EmitFor( TracingILGenerator il, LocalBuilder count, Action<TracingILGenerator, LocalBuilder> bodyEmitter )
		{
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
			il.EmitGetProperty( Metadata._Unpacker.IsArrayHeader );
			il.EmitBrtrue_S( then );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( Metadata._Unpacker.IsMapHeader );
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
			il.EmitGetProperty( Metadata._Unpacker.IsArrayHeader );
			il.EmitBrtrue_S( then );
			il.EmitAnyLdarg( unpackerArgumentIndex );
			il.EmitGetProperty( Metadata._Unpacker.IsMapHeader );
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
			il.EmitAnyCall( Metadata._Unpacker.ReadSubtree );
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
			il.EmitAnyCall( Metadata._IDisposable.Dispose );
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

		public static void EmitConstruction( TracingILGenerator il, LocalBuilder target, Action<TracingILGenerator> initialCountLoadingEmitter )
		{
			Contract.Assert( il != null );
			Contract.Assert( target != null );

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
					il.EmitAnyLdloca( target );
					initialCountLoadingEmitter( il );
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
		///		Emit member unpacking boiler plates.
		/// </summary>
		/// <param name="emitter"><see cref="SerializerEmitter"/> to register the filed which holds descendant serializers.</param>
		/// <param name="il"><see cref="TracingILGenerator"/> to emit instructions.</param>
		/// <param name="target">Local variable which holds deserializing target instance.</param>
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
