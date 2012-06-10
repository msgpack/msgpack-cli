
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
using System.Reflection;
using System.Reflection.Emit;
#if FALSE
using MsgPack.Serialization.EmittingSerializers;
#endif

#warning Remove FALSE

namespace MsgPack.Serialization.DefaultSerializers
{
	internal sealed class System_Collections_Generic_KeyValuePair_2MessagePackSerializer<TKey, TValue> : MessagePackSerializer<KeyValuePair<TKey, TValue>>
	{
#if FALSE
		private readonly MessagePackSerializer<KeyValuePair<TKey, TValue>> _underlying;
#endif
		private readonly MessagePackSerializer<TKey> _keySerializer;
		private readonly MessagePackSerializer<TValue> _valueSerializer;

		public System_Collections_Generic_KeyValuePair_2MessagePackSerializer( SerializationContext context )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			this._keySerializer = context.GetSerializer<TKey>();
			this._valueSerializer = context.GetSerializer<TValue>();
		}

		private static Action<Packer, KeyValuePair<TKey, TValue>> CreatePackToCore()
		{
			throw new NotImplementedException();
		}

		private static Func<Unpacker, KeyValuePair<TKey, TValue>> CreateUnpackFromCore()
		{
			throw new NotImplementedException();
		}

#if FALSE
		public System_Collections_Generic_KeyValuePair_2MessagePackSerializer( SerializationContext context )
#if !WINDOWS_PHONE
			: this( context, EmitterFlavor.FieldBased )
#else
			: this( context, EmitterFlavor.ContextBased )
#endif
		{ }

		public System_Collections_Generic_KeyValuePair_2MessagePackSerializer( SerializationContext context, EmitterFlavor emitterFlavor )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			var emitter = SerializationMethodGeneratorManager.Get().CreateEmitter( typeof( KeyValuePair<TKey, TValue> ), emitterFlavor );
			CreatePacker( emitter );
			CreateUnpacker( emitter );
			this._underlying = emitter.CreateInstance<KeyValuePair<TKey, TValue>>( context );
		}

		private static void CreatePacker( SerializerEmitter emitter )
		{
			var il = emitter.GetPackToMethodILGenerator();
			try
			{
				il.EmitAnyLdarg( 1 );
				il.EmitAnyLdc_I4( 2 );
				il.EmitAnyCall( Metadata._Packer.PackArrayHeader );
				il.EmitPop();
				Emittion.EmitSerializeValue(
					emitter,
					il,
					1,
					typeof( TKey ),
					null,
					NilImplication.MemberDefault,
					il0 =>
					{
						il0.EmitAnyLdarga( 2 );
						il0.EmitGetProperty( Metadata._KeyValuePair<TKey, TValue>.Key );
					}
				);
				Emittion.EmitSerializeValue(
					emitter,
					il,
					1,
					typeof( TValue ),
					null,
					NilImplication.MemberDefault,
					il0 =>
					{
						il0.EmitAnyLdarga( 2 );
						il0.EmitGetProperty( Metadata._KeyValuePair<TKey, TValue>.Value );
					}
				);
				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
			}
		}

		private static void CreateUnpacker( SerializerEmitter emitter )
		{
			var il = emitter.GetUnpackFromMethodILGenerator();
			try
			{
				var key = il.DeclareLocal( typeof( TKey ), "key" );
				var value = il.DeclareLocal( typeof( TValue ), "value" );
				il.EmitAnyLdarg( 1 );
				il.EmitAnyCall( Metadata._Unpacker.Read );
				var endIf0 = il.DefineLabel( "END_IF" );
				il.EmitBrtrue_S( endIf0 );
				il.EmitAnyCall( SerializationExceptions.NewUnexpectedEndOfStreamMethod );
				il.EmitThrow();
				il.MarkLabel( endIf0 );
				Emittion.EmitDeserializeValue( emitter, il, 1, key, null, null, NilImplication.MemberDefault, null );
				il.EmitAnyLdarg( 1 );
				il.EmitAnyCall( Metadata._Unpacker.Read );
				var endIf1 = il.DefineLabel( "END_IF" );
				il.EmitBrtrue_S( endIf1 );
				il.EmitAnyCall( SerializationExceptions.NewUnexpectedEndOfStreamMethod );
				il.EmitThrow();
				il.MarkLabel( endIf1 );
				Emittion.EmitDeserializeValue( emitter, il, 1, value, null, null, NilImplication.MemberDefault, null );

				var result = il.DeclareLocal( typeof( KeyValuePair<TKey, TValue> ), "result" );
				il.EmitAnyLdloca( result );
				il.EmitAnyLdloc( key );
				il.EmitAnyLdloc( value );
				il.EmitCallConstructor( Metadata._KeyValuePair<TKey, TValue>.Ctor );
				il.EmitAnyLdloc( result );
				il.EmitRet();
			}
			finally
			{
				il.FlushTrace();
			}
		}
#endif // NETFX_CORE else

		protected internal sealed override void PackToCore( Packer packer, KeyValuePair<TKey, TValue> objectTree )
		{
#if FALSE
			this._underlying.PackTo( packer, objectTree );
#else
			packer.PackArrayHeader( 2 );
			this._keySerializer.PackTo( packer, objectTree.Key );
			this._valueSerializer.PackTo( packer, objectTree.Value );
#endif
		}

		protected internal sealed override KeyValuePair<TKey, TValue> UnpackFromCore( Unpacker unpacker )
		{
#if FALSE
			return this._underlying.UnpackFrom( unpacker );
#else
			if ( !unpacker.Read() )
			{
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}

			TKey key = unpacker.Data.Value.IsNil ? default( TKey ) : this._keySerializer.UnpackFrom( unpacker );

			if ( !unpacker.Read() )
			{
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}

			TValue value = unpacker.Data.Value.IsNil ? default( TValue ) : this._valueSerializer.UnpackFrom( unpacker );

			return new KeyValuePair<TKey, TValue>( key, value );
#endif
		}
	}
}
