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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Reflection;
using NLiblet.Reflection;
using System.Reflection.Emit;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<see cref="SerializerBuilder{T}"/> implementation using Reflection.Emit.
	/// </summary>
	/// <typeparam name="TObject">Object to be serialized/deserialized.</typeparam>
	internal sealed class MapEmittingSerializerBuilder<TObject> : EmittingSerializerBuilder<TObject>
	{
		public MapEmittingSerializerBuilder( SerializationContext context )
			: base( context ) { }

		protected sealed override void EmitPackMembers( SerializerEmitter emitter, TracingILGenerator packerIL, SerializingMember[] entries )
		{
			Emittion.EmitPackMambers(
				emitter,
				packerIL,
				1,
				typeof( TObject ),
				2,
				entries.Select( item => Tuple.Create( item.Member, item.Member.GetMemberValueType() ) ).ToArray()
			);
			packerIL.EmitRet();
		}
	}


	internal sealed class ArrayEmittingSerializerBuilder<TObject> : EmittingSerializerBuilder<TObject>
	{
		public ArrayEmittingSerializerBuilder( SerializationContext context )
			: base( context ) { }

		protected override void EmitPackMembers( SerializerEmitter emitter, TracingILGenerator packerIL, SerializingMember[] entries )
		{
			throw new NotImplementedException();
		}
	}
}
