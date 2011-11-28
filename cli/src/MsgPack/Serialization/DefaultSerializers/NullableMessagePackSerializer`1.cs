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
using System.Globalization;
using System.Reflection;

namespace MsgPack.Serialization.DefaultSerializers
{
	internal sealed class NullableMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		// TODO: -> Metadata
		// TODO: comment
		private static readonly PropertyInfo _nullableTHasValueProperty = GetOnlyWhenNullable( typeof( Nullable<> ).MakeGenericType( Nullable.GetUnderlyingType( typeof( T ) ) ).GetProperty( "HasValue" ) );
		private static readonly PropertyInfo _nullableTValueProperty = GetOnlyWhenNullable( typeof( Nullable<> ).MakeGenericType( Nullable.GetUnderlyingType( typeof( T ) ) ).GetProperty( "Value" ) );
		private static readonly MethodInfo _nullableTImplicitOperator = GetOnlyWhenNullable( typeof( Nullable<> ).MakeGenericType( Nullable.GetUnderlyingType( typeof( T ) ) ).GetMethod( "op_Implicit", new Type[] { Nullable.GetUnderlyingType( typeof( T ) ) } ) );

		private static TValue GetOnlyWhenNullable<TValue>( TValue value )
		{
			if ( typeof( T ).IsGenericType && typeof( T ).GetGenericTypeDefinition() == typeof( Nullable<> ) )
			{
				return value;
			}
			else
			{
				return default( TValue );
			}
		}

		private readonly MessagePackSerializer<T> _underlying;

		public NullableMessagePackSerializer( SerializationContext context )
		{
			if ( _nullableTImplicitOperator == null )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not nullable type.", typeof( T ) ) );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			var emitter = SerializationMethodGeneratorManager.Get().CreateEmitter( typeof( T ) );
			CreatePacking( emitter );
			CreateUnpacking( emitter );
			this._underlying = emitter.CreateInstance<T>( context );
		}

		private static void CreatePacking( SerializerEmitter emitter )
		{
			var il = emitter.GetPackToMethodILGenerator();
			try
			{
				/*
				 * if( value == null )
				 * {
				 *		packer.PackNull();
				 *		return;
				 * }
				 * 
				 * return context.MarshalTo<T>( packer, value );
				 */
				var endIf = il.DefineLabel( "END_IF" );
				var endMethod = il.DefineLabel( "END_METHOD" );
				il.EmitAnyLdarga( 2 );
				il.EmitGetProperty( _nullableTHasValueProperty );
				il.EmitBrtrue_S( endIf );
				il.EmitAnyLdarg( 1 );
				il.EmitAnyCall( NullableMessagePackSerializer.PackerPackNull );
				il.EmitPop();
				il.EmitBr_S( endMethod );

				il.MarkLabel( endIf );
				Emittion.EmitMarshalValue(
					emitter,
					il,
					1,
					_nullableTValueProperty.PropertyType,
					il0 =>
					{
						il0.EmitAnyLdarga( 2 );
						il.EmitGetProperty( _nullableTValueProperty );
					}
				);

				il.MarkLabel( endMethod );
				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
				emitter.FlushTrace();
			}
		}

		private static void CreateUnpacking( SerializerEmitter emitter )
		{
			var il = emitter.GetUnpackFromMethodILGenerator();
			try
			{
				/*
				 * if( unpacker.Data.Value.IsNil )
				 * {
				 *		return default( T );
				 * }
				 * 
				 * return context.UnmarshalFrom<T>( packer, value );
				 */
				var mayBeNullData = il.DeclareLocal( typeof( MessagePackObject? ), "mayBeNullData" );
				var data = il.DeclareLocal( typeof( MessagePackObject ), "data" );
				var result = il.DeclareLocal( typeof( T ), "result" );
				var value = il.DeclareLocal( _nullableTValueProperty.PropertyType, "value" );
				var endIf = il.DefineLabel( "END_IF" );
				var endMethod = il.DefineLabel( "END_METHOD" );
				il.EmitAnyLdarg( 1 );
				il.EmitGetProperty( NullableMessagePackSerializer.UnpackerDataProperty );
				il.EmitAnyStloc( mayBeNullData );
				il.EmitAnyLdloca( mayBeNullData );
				il.EmitGetProperty( NullableMessagePackSerializer.Nullable_MessagePackObject_ValueProperty );
				il.EmitAnyStloc( data );
				il.EmitAnyLdloca( data );
				il.EmitGetProperty( NullableMessagePackSerializer.MessagePackObject_IsNilProperty );
				il.EmitBrfalse_S( endIf );
				il.EmitAnyLdloca( result );
				il.EmitInitobj( result.LocalType );
				il.EmitBr_S( endMethod );

				il.MarkLabel( endIf );
				Emittion.EmitUnmarshalValue( emitter, il, 1, value, null );
				il.EmitAnyLdloc( value );
				il.EmitAnyCall( _nullableTImplicitOperator );
				il.EmitAnyStloc( result );
				il.MarkLabel( endMethod );
				il.EmitAnyLdloc( result );
				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
				emitter.FlushTrace();
			}
		}

		protected sealed override void PackToCore( Packer packer, T value )
		{
			this._underlying.PackTo( packer, value );
		}

		protected sealed override T UnpackFromCore( Unpacker unpacker )
		{
			return this._underlying.UnpackFrom( unpacker );
		}
	}
}
