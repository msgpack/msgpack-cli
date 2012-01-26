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
			var locals =
				entries.Select(
					item =>
					!EmittingSerializerBuilderLogics.IsReadOnlyAppendableCollectionMember( item.Member )
					? unpackerIL.DeclareLocal( item.Member.GetMemberValueType(), item.Contract.Name )
					: null
				).ToArray();

			Emittion.EmitUnpackMembers(
				emitter,
				unpackerIL,
				1,
				result,
				entries.Zip(
					locals,
					( entry, local ) => new Tuple<MemberInfo, string, LocalBuilder, LocalBuilder>( entry.Member, entry.Contract.Name, local, default( LocalBuilder ) )
				).ToArray()
			);

			foreach ( var item in entries.Zip( locals, ( Entry, Local ) => new { Entry, Local } ) )
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
