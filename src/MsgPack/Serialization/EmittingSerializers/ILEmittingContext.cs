#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Reflection.Emit;

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal class ILEmittingContext : SerializerGenerationContext<ILConstruct>
	{
		internal static readonly Dictionary<Type, Action<TracingILGenerator>> ConversionInstructionMap =
			new Dictionary<Type, Action<TracingILGenerator>>
			{
				{typeof( sbyte ), il => il.EmitConv_I1()},
				{typeof( short ), il => il.EmitConv_I2()},
				{typeof( int ), il => il.EmitConv_I4()},
				{typeof( long ), il => il.EmitConv_I8()},
				{typeof( IntPtr ), il => il.EmitConv_I()},
				{typeof( byte ), il => il.EmitConv_U1()},
				{typeof( ushort ), il => il.EmitConv_U2()},
				{typeof( uint ), il => il.EmitConv_U4()},
				{typeof( ulong ), il => il.EmitConv_U8()},
				{typeof( UIntPtr ), il => il.EmitConv_U()},
				{typeof( float ), il => il.EmitConv_R4()},
				{typeof( double ), il => il.EmitConv_R8()}
			};

		internal TracingILGenerator IL { get; set; }
		internal Type SerializerType { get; private set; }
		internal SerializerEmitter Emitter { get; private set; }

		public ILEmittingContext( SerializationContext serializationContext, Type targetType, SerializerEmitter emitter )
			: base(
				serializationContext,
				ILConstruct.Argument( 1, typeof( Packer ), "packer" ),
				ILConstruct.Argument( 2, targetType, "objectTree" ),
				ILConstruct.Argument( 1, typeof( Unpacker ), "unpacker" ),
				ILConstruct.Argument( 2, targetType, "collection" )
			)
		{
			this.SerializerType = typeof( MessagePackSerializer<> ).MakeGenericType( targetType );
			this.Emitter = emitter;
		}
	}
}