#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;
using System.Globalization;
#if NETFX_CORE
using System.Linq;
using System.Linq.Expressions;
#endif
using System.Reflection;
#if !NETFX_CORE
using MsgPack.Serialization.EmittingSerializers;
#endif

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
			if ( typeof( T ).GetIsGenericType() && typeof( T ).GetGenericTypeDefinition() == typeof( Nullable<> ) )
			{
				return value;
			}
			else
			{
				return default( TValue );
			}
		}

#if !NETFX_CORE
		private readonly MessagePackSerializer<T> _underlying;
#else
		private readonly Action<Packer, T, IMessagePackSerializer> _packToCore;
		private readonly Func<Unpacker, IMessagePackSerializer, T> _unpackFromCore;
		private readonly IMessagePackSerializer _underlyingTypeSerializer;
#endif
		public NullableMessagePackSerializer( SerializationContext context )
#if WINDOWS_PHONE
			: this( context, EmitterFlavor.ContextBased )
#elif NETFX_CORE
			: this( context, EmitterFlavor.ExpressionBased )
#else
			: this( context, context.EmitterFlavor )
#endif
		{ }

		internal NullableMessagePackSerializer( SerializationContext context, EmitterFlavor emitterFlavor )
			: base( ( context ?? SerializationContext.Default ).CompatibilityOptions.PackerCompatibilityOptions )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			Contract.EndContractBlock();

			if ( _nullableTImplicitOperator == null )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not nullable type.", typeof( T ) ) );
			}

#if !NETFX_CORE
			var emitter = SerializationMethodGeneratorManager.Get().CreateEmitter( typeof( T ), emitterFlavor );
			CreatePacking( emitter );
			CreateUnpacking( emitter );
			this._underlying = emitter.CreateInstance<T>( context );
#else
			var underlyingType = Nullable.GetUnderlyingType( typeof( T ) );
			this._packToCore = CreatePacking( underlyingType );
			this._unpackFromCore = CreateUnpacking( underlyingType );
			this._underlyingTypeSerializer = context.GetSerializer( underlyingType );
#endif
		}

#if !NETFX_CORE
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
				Emittion.EmitSerializeValue(
					emitter,
					il,
					1,
					_nullableTValueProperty.PropertyType,
					null,
					NilImplication.MemberDefault,
					il0 =>
					{
						il0.EmitAnyLdarga( 2 );
						il.EmitGetProperty( _nullableTValueProperty );
					},
					new LocalVariableHolder( il )
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
#else
		private static Action<Packer, T, IMessagePackSerializer> CreatePacking( Type underlyingType )
		{
			var packerParameter = Expression.Parameter( typeof( Packer ), "packer" );
			var targetParameter = Expression.Parameter( typeof( T ), "target" );
			var serializerParameter = Expression.Parameter( typeof( IMessagePackSerializer ), "serializer" );
			var serializerType = typeof( MessagePackSerializer<> ).MakeGenericType( underlyingType );
			var endOfMethod = Expression.Label( "END_OF_METHOD" );

			return 
				Expression.Lambda<Action<Packer,T, IMessagePackSerializer>>(
					Expression.Block(
						Expression.IfThen(
							Expression.IsFalse(
								Expression.Property(
									targetParameter,
									_nullableTHasValueProperty
								)
							),
							Expression.Block(
								Expression.Call(
									packerParameter,
									Metadata._Packer.PackNull
								),
								Expression.Return( endOfMethod )
							)
						),
						Expression.Call(
							Expression.TypeAs( 
								serializerParameter,
								serializerType
							),
							serializerType.GetRuntimeMethods().Single( m => m.Name == "PackTo" ),
							packerParameter,
							Expression.Property(
								targetParameter,
								_nullableTValueProperty
							)
						),
						Expression.Label( endOfMethod )
					),
					packerParameter, targetParameter, serializerParameter
				).Compile();
		}
#endif

#if !NETFX_CORE

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
				 * return context.Get<T>().Unpack( unpacker );
				 */
				var data = il.DeclareLocal( typeof( MessagePackObject ), "data" );
				var result = il.DeclareLocal( typeof( T ), "result" );
				var value = il.DeclareLocal( _nullableTValueProperty.PropertyType, "value" );
				var endIf = il.DefineLabel( "END_IF" );
				var endMethod = il.DefineLabel( "END_METHOD" );
				il.EmitAnyLdarg( 1 );
				il.EmitGetProperty( NullableMessagePackSerializer.UnpackerLastReadDataProperty );
				il.EmitAnyStloc( data );
				il.EmitAnyLdloca( data );
				il.EmitGetProperty( NullableMessagePackSerializer.MessagePackObject_IsNilProperty );
				il.EmitBrfalse_S( endIf );
				il.EmitAnyLdloca( result );
				il.EmitInitobj( result.LocalType );
				il.EmitBr_S( endMethod );

				il.MarkLabel( endIf );

				// Just invoke Unpack without direct unpacking nor subtree reading...
				// Unpacker must locate in first value of non-null value-type object.
				Emittion.EmitUnpackFrom( emitter, il, value, 1 );
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
#else
		private static Func<Unpacker, IMessagePackSerializer, T> CreateUnpacking( Type underlyingType )
		{
			var unpackerParameter = Expression.Parameter( typeof( Unpacker ), "unpacker" );
			var serializerParameter = Expression.Parameter( typeof( IMessagePackSerializer ), "serializer" );
			var serializerType = typeof( MessagePackSerializer<> ).MakeGenericType( underlyingType );
			var endOfMethod = Expression.Label( "END_OF_METHOD" );

			return 
				Expression.Lambda<Func<Unpacker, IMessagePackSerializer, T>>(
					Expression.Condition(
						Expression.IsTrue(
							Expression.Property(
								Expression.Property(
									unpackerParameter,
									Metadata._Unpacker.LastReadData
								),
								Metadata._MessagePackObject.IsNil
							)
						),
						Expression.Default( typeof( T ) ),
						Expression.Convert(
							Expression.Call(
								null,
								Metadata._UnpackHelpers.InvokeUnpackFrom_1Method.MakeGenericMethod( underlyingType ),
								Expression.TypeAs( 
									serializerParameter,
									serializerType
								),
								unpackerParameter
							),
							typeof( T ),
							_nullableTImplicitOperator
						)
					),
					unpackerParameter, serializerParameter
				).Compile();
		}
#endif

		protected internal sealed override void PackToCore( Packer packer, T value )
		{
#if !NETFX_CORE
			this._underlying.PackTo( packer, value );
#else
			this._packToCore( packer, value, this._underlyingTypeSerializer );
#endif
		}

		protected internal sealed override T UnpackFromCore( Unpacker unpacker )
		{
#if !NETFX_CORE
			return this._underlying.UnpackFrom( unpacker );
#else
			return this._unpackFromCore( unpacker, this._underlyingTypeSerializer );
#endif
		}
	}
}
