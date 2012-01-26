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
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using NLiblet.Reflection;

namespace MsgPack.Serialization
{
	public enum SerializationMethod
	{
		Array = 0,
		Map
	}

	internal enum EmitterFlavor
	{
		ContextBased,
		FieldBased,
	}

	/// <summary>
	///		Implements common features code generation based serializer builders.
	/// </summary>
	/// <typeparam name="TObject">The type of the serialization target.</typeparam>
	internal abstract class EmittingSerializerBuilder<TObject> : SerializerBuilder<TObject>
	{
		private readonly SerializationMethodGeneratorOption _generatorOption;
		private readonly EmitterFlavor _emitterFlavor;

		protected EmittingSerializerBuilder( SerializationContext context )
			: base( context )
		{
			this._generatorOption = context.GeneratorOption;
			this._emitterFlavor = context.EmitterFlavor;
		}

		protected sealed override MessagePackSerializer<TObject> CreateSerializer( SerializingMember[] entries )
		{
			var emitter = SerializationMethodGeneratorManager.Get( this._generatorOption ).CreateEmitter( typeof( TObject ), this._emitterFlavor );

			var packerIL = emitter.GetPackToMethodILGenerator();
			try
			{
				this.EmitPackMembers( emitter, packerIL, entries );
			}
			finally
			{
				packerIL.FlushTrace();
			}

			var unpackerIL = emitter.GetUnpackFromMethodILGenerator();
			try
			{
				// TODO: Array for ordered.
				// TODO: For big struct, use Dictionary<String,SM>
				// TODO: Required
				var result = unpackerIL.DeclareLocal( typeof( TObject ), "result" );
				Emittion.EmitConstruction( unpackerIL, result, null );

				this.EmitUnpackMembers( emitter, unpackerIL, entries, result );

				unpackerIL.EmitAnyLdloc( result );
				unpackerIL.EmitRet();
			}
			finally
			{
				unpackerIL.FlushTrace();
			}

			return emitter.CreateInstance<TObject>( this.Context );
		}

		protected abstract void EmitPackMembers( SerializerEmitter emitter, TracingILGenerator packerIL, SerializingMember[] entries );

		private void EmitUnpackMembers( SerializerEmitter emitter, TracingILGenerator unpackerIL, SerializingMember[] entries, LocalBuilder result )
		{
			/*
			 *	if( unpacker.IsArrayHeader )
			 *	{
			 *		...
			 *	}
			 *	else
			 *	{
			 *		...
			 *	}
			 */
			unpackerIL.EmitAnyLdarg( 1 );
			unpackerIL.EmitGetProperty( Metadata._Unpacker.IsArrayHeader );
			var @else = unpackerIL.DefineLabel( "ELSE" );
			var endif = unpackerIL.DefineLabel( "END_IF" );
			unpackerIL.EmitBrfalse( @else );
			EmitUnpackMembersFromArray( emitter, unpackerIL, entries, result );
			unpackerIL.EmitBr( endif );
			unpackerIL.MarkLabel( @else );
			EmitUnpackMembersFromMap( emitter, unpackerIL, entries, result );
			unpackerIL.MarkLabel( endif );
		}

		private static void EmitUnpackMembersFromArray( SerializerEmitter emitter, TracingILGenerator unpackerIL, SerializingMember[] entries, LocalBuilder result )
		{
			/*
			 *	if( !unpacker.Read() )
			 *	{
			 *		throw SerializationExceptions.NewUnexpectedEndOfStreamMethod();
			 *	}
			 * 
			 *	local1 = this._serializer1.Unpack
			 */
			var items =
				entries.Select(
					item =>
						!EmittingSerializerBuilderLogics.IsReadOnlyAppendableCollectionMember( item.Member )
						? unpackerIL.DeclareLocal( item.Member.GetMemberValueType(), item.Contract.Name )
						: null
				).Zip( entries, ( Local, Entry ) => new { Entry, Local } )
				.ToArray();

			for ( int i = 0; i < items.Length; i++ )
			{
				unpackerIL.EmitAnyLdarg( 1 );
				unpackerIL.EmitAnyCall( Metadata._Unpacker.Read );
				var endIf = unpackerIL.DefineLabel( "END_IF" );
				unpackerIL.EmitBrtrue_S( endIf );
				unpackerIL.EmitAnyCall( SerializationExceptions.NewUnexpectedEndOfStreamMethod );
				unpackerIL.EmitThrow();
				unpackerIL.MarkLabel( endIf );
				if ( items[ i ].Local == null )
				{
					Emittion.EmitDeserializeCollectionValue( emitter, unpackerIL, 1, result, items[ i ].Entry.Member, items[ i ].Entry.Member.GetMemberValueType() );
				}
				else
				{
					Emittion.EmitDeserializeValue( emitter, unpackerIL, 1, items[ i ].Local, null );
				}
			}

			foreach ( var item in items )
			{
				if ( item.Local == null )
				{
					continue;
				}

				if ( result.LocalType.IsValueType )
				{
					unpackerIL.EmitAnyLdloca( result );
				}
				else
				{
					unpackerIL.EmitAnyLdloc( result );
				}

				unpackerIL.EmitAnyLdloc( item.Local );
				Emittion.EmitStoreValue( unpackerIL, item.Entry.Member );
			}
		}

		private static void EmitUnpackMembersFromMap( SerializerEmitter emitter, TracingILGenerator unpackerIL, SerializingMember[] entries, LocalBuilder result )
		{
			var items =
				entries.Select(
					item =>
					!EmittingSerializerBuilderLogics.IsReadOnlyAppendableCollectionMember( item.Member )
					? unpackerIL.DeclareLocal( item.Member.GetMemberValueType(), item.Contract.Name )
					: null
				).Zip( entries, ( Local, Entry ) => new { Entry, Local } ).ToArray();

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

			var whileCond = unpackerIL.DefineLabel( "WHILE_COND" );
			var endWhile = unpackerIL.DefineLabel( "END_WHILE" );
			unpackerIL.MarkLabel( whileCond );
			unpackerIL.EmitAnyLdarg( 1 );
			unpackerIL.EmitAnyCall( Metadata._Unpacker.Read );
			unpackerIL.EmitBrfalse( endWhile );

			var data = unpackerIL.DeclareLocal( typeof( MessagePackObject? ), "data" );
			var dataValue = unpackerIL.DeclareLocal( typeof( MessagePackObject ), "dataValue" );
			var memberName = unpackerIL.DeclareLocal( typeof( string ), "memberName" );
			unpackerIL.EmitAnyLdarg( 1 );
			unpackerIL.EmitGetProperty( Metadata._Unpacker.Data );
			unpackerIL.EmitAnyStloc( data );
			unpackerIL.EmitAnyLdloca( data );
			unpackerIL.EmitGetProperty( Metadata._Nullable<MessagePackObject>.Value );
			unpackerIL.EmitAnyStloc( dataValue );
			unpackerIL.EmitAnyLdloca( dataValue );
			unpackerIL.EmitAnyCall( Metadata._MessagePackObject.AsString );
			unpackerIL.EmitAnyStloc( memberName );
			for ( int i = 0; i < items.Length; i++ )
			{
				// TODO: binary comparison
				unpackerIL.EmitAnyLdloc( memberName );
				unpackerIL.EmitLdstr( items[ i ].Entry.Member.Name );
				unpackerIL.EmitAnyCall( Metadata._String.op_Inequality );
				var endIf0 = unpackerIL.DefineLabel( "END_IF_0_" + i );
				unpackerIL.EmitBrtrue_S( endIf0 );
				unpackerIL.EmitAnyLdarg( 1 );
				unpackerIL.EmitAnyCall( Metadata._Unpacker.Read );
				var endIf1 = unpackerIL.DefineLabel( "END_IF_1_" + i );
				unpackerIL.EmitBrtrue_S( endIf1 );
				unpackerIL.EmitAnyCall( SerializationExceptions.NewUnexpectedEndOfStreamMethod );
				unpackerIL.EmitThrow();
				unpackerIL.MarkLabel( endIf1 );

				if ( items[ i ].Local == null )
				{
					Emittion.EmitDeserializeCollectionValue( emitter, unpackerIL, 1, result, items[ i ].Entry.Member, items[ i ].Entry.Member.GetMemberValueType() );
				}
				else
				{
					Emittion.EmitDeserializeValue( emitter, unpackerIL, 1, items[ i ].Local, null );
				}

				// TOOD: Record for missing check

				unpackerIL.EmitBr( whileCond );
				unpackerIL.MarkLabel( endIf0 );
			}

			unpackerIL.MarkLabel( endWhile );
			
			// TOOD: Check missing

			foreach ( var item in items )
			{
				if ( item.Local == null )
				{
					continue;
				}

				if ( result.LocalType.IsValueType )
				{
					unpackerIL.EmitAnyLdloca( result );
				}
				else
				{
					unpackerIL.EmitAnyLdloc( result );
				}

				unpackerIL.EmitAnyLdloc( item.Local );
				Emittion.EmitStoreValue( unpackerIL, item.Entry.Member );
			}
		}

		public sealed override MessagePackSerializer<TObject> CreateArraySerializer()
		{
			return EmittingSerializerBuilderLogics.CreateArraySerializerCore( typeof( TObject ), this._emitterFlavor ).CreateInstance<TObject>( this.Context );
		}

		public sealed override MessagePackSerializer<TObject> CreateMapSerializer()
		{
			return EmittingSerializerBuilderLogics.CreateMapSerializerCore( typeof( TObject ), this._emitterFlavor ).CreateInstance<TObject>( this.Context );
		}

		public sealed override MessagePackSerializer<TObject> CreateTupleSerializer()
		{
			return EmittingSerializerBuilderLogics.CreateTupleSerializerCore( typeof( TObject ), this._emitterFlavor ).CreateInstance<TObject>( this.Context );
		}
	}

}
