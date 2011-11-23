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

namespace MsgPack.Serialization.DefaultMarshalers
{
	internal sealed class NullableMarshaler<T> : MessageMarshaler<T>
	{
		private static readonly PropertyInfo _nullableTHasValueProperty;
		private static readonly PropertyInfo _nullableTValueProperty;
		private static readonly MethodInfo _nullableTImplicitOperator;

		static NullableMarshaler()
		{
			if ( typeof( T ).IsGenericType && typeof( T ).GetGenericTypeDefinition() == typeof( Nullable<> ) )
			{
				_nullableTHasValueProperty = typeof( Nullable<> ).MakeGenericType( Nullable.GetUnderlyingType( typeof( T ) ) ).GetProperty( "HasValue" );
				_nullableTValueProperty = typeof( Nullable<> ).MakeGenericType( Nullable.GetUnderlyingType( typeof( T ) ) ).GetProperty( "Value" );
				_nullableTImplicitOperator = typeof( Nullable<> ).MakeGenericType( Nullable.GetUnderlyingType( typeof( T ) ) ).GetMethod( "op_Implicit", new Type[] { Nullable.GetUnderlyingType( typeof( T ) ) } );
			}
		}

		private readonly SerializationContext _context;
		private readonly Action<Packer, T> _packing;
		private readonly Func<Unpacker, T> _unpacking;

		public NullableMarshaler()
			: this( null, null ) { }

		public NullableMarshaler( MarshalerRepository marshalers, SerializerRepository serializers )
		{
			if ( _nullableTImplicitOperator == null )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not nullable type.", typeof( T ) ) );
			}

			this._context = new SerializationContext( marshalers ?? MarshalerRepository.Default, serializers ?? SerializerRepository.Default );
			var packing = CreatePacking();
			var unpacking = CreateUnpacking();
			this._packing = ( packer, value ) => packing( packer, value, this._context );
			this._unpacking = unpacker => unpacking( unpacker, this._context );
		}

		private Action<Packer, T, SerializationContext> CreatePacking()
		{
			var dynamicMethod = SerializationMethodGeneratorManager.Get().CreateGenerator( "Pack", typeof( T ), "Instance", typeof( void ), typeof( Packer ), typeof( T ), typeof( SerializationContext ) );
			var il = dynamicMethod.GetILGenerator();
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
				il.EmitAnyLdarga( 1 );
				il.EmitGetProperty( _nullableTHasValueProperty );
				il.EmitBrtrue_S( endIf );
				il.EmitAnyLdarg( 0 );
				il.EmitAnyCall( NullableMarshaler.PackerPackNull );
				il.EmitPop();
				il.EmitBr_S( endMethod );

				il.MarkLabel( endIf );
				Emittion.EmitMarshalValue(
					il,
					0,
					2,
					_nullableTValueProperty.PropertyType,
					il0 =>
					{
						il0.EmitAnyLdarga( 1 );
						il.EmitGetProperty( _nullableTValueProperty );
					}
				);

				il.MarkLabel( endMethod );
				il.EmitRet();

				return dynamicMethod.CreateDelegate<Action<Packer, T, SerializationContext>>();
			}
			finally
			{
				il.FlushTrace();
				dynamicMethod.FlushTrace();
			}
		}


		private Func<Unpacker, SerializationContext, T> CreateUnpacking()
		{
			var dynamicMethod = SerializationMethodGeneratorManager.Get().CreateGenerator( "Unpack", typeof( T ), "Instance", typeof( T ), typeof( Unpacker ), typeof( SerializationContext ) );
			var il = dynamicMethod.GetILGenerator();
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
				il.EmitAnyLdarg( 0 );
				il.EmitGetProperty( NullableMarshaler.UnpackerDataProperty );
				il.EmitAnyStloc( mayBeNullData );
				il.EmitAnyLdloca( mayBeNullData );
				il.EmitGetProperty( NullableMarshaler.Nullable_MessagePackObject_ValueProperty );
				il.EmitAnyStloc( data );
				il.EmitAnyLdloca( data );
				il.EmitGetProperty( NullableMarshaler.MessagePackObject_IsNilProperty );
				il.EmitBrfalse_S( endIf );
				il.EmitAnyLdloca( result );
				il.EmitInitobj( result.LocalType );
				il.EmitBr_S( endMethod );

				il.MarkLabel( endIf );
				Emittion.EmitUnmarshalValue( il, 0, 1, value, null );
				il.EmitAnyLdloc( value );
				il.EmitAnyCall( _nullableTImplicitOperator );
				il.EmitAnyStloc( result );
				il.MarkLabel( endMethod );
				il.EmitAnyLdloc( result );
				il.EmitRet();
				return dynamicMethod.CreateDelegate<Func<Unpacker, SerializationContext, T>>();
			}
			finally
			{
				il.FlushTrace();
				dynamicMethod.FlushTrace();
			}
		}

		protected sealed override void MarshalToCore( Packer packer, T value )
		{
			this._packing( packer, value );
		}

		protected sealed override T UnmarshalFromCore( Unpacker unpacker )
		{
			return this._unpacking( unpacker );
		}
	}
}
