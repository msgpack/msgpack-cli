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
using System.Reflection.Emit;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal class AssemblyBuilderEmittingContext : ILEmittingContext
	{
		public AssemblyBuilderEmittingContext( SerializationContext context, Type targetType, SerializerEmitter emitter )
			: base(
				context,
				targetType,
				emitter,
				ILConstruct.Argument( 1, typeof( Packer ), "packer" ),
				ILConstruct.Argument( 2, targetType, "objectTree" ),
				ILConstruct.Argument( 1, typeof( Unpacker ), "unpacker" ),
				ILConstruct.Argument( 2, targetType, "collection" )
			)
		{
		}

	}

	internal class AssemblyBuilderSerializerBuilder<TObject> : ILEmittingSerializerBuilder<AssemblyBuilderEmittingContext, TObject>
	{
		private readonly AssemblyBuilder _predefinedAssemblyBuilder;

		public AssemblyBuilderSerializerBuilder()
			: base()
		{
			this._predefinedAssemblyBuilder = null;
		}

		public AssemblyBuilderSerializerBuilder( AssemblyBuilder predefinedAssemblyBuilder )
			: base( predefinedAssemblyBuilder.GetName().Name, predefinedAssemblyBuilder.GetName().Version )
		{
			this._predefinedAssemblyBuilder = predefinedAssemblyBuilder;
		}


		protected override ILConstruct EmitGetSerializerExpression( AssemblyBuilderEmittingContext context, Type targetType )
		{
			var instructions = context.Emitter.RegisterSerializer( targetType );
			return
				ILConstruct.Instruction(
					"getserializer",
					typeof( MessagePackSerializer<> ).MakeGenericType( targetType ),
					false,
				// Both of this pointer for FieldBasedSerializerEmitter and context argument of methods for ContextBasedSerializerEmitter are 0.
					il => instructions( il, 0 )
				);
		}

		protected override AssemblyBuilderEmittingContext CreateGenerationContextForSerializerCreation( SerializationContext context )
		{
			return
				new AssemblyBuilderEmittingContext(
					context,
					typeof( TObject ),
					SerializationMethodGeneratorManager.Get().CreateEmitter( typeof( TObject ), EmitterFlavor.FieldBased )
				);
		}

		protected override AssemblyBuilderEmittingContext CreateGenerationContextForCodeGeneration( SerializationContext context )
		{
			if ( this._predefinedAssemblyBuilder == null )
			{
				throw new InvalidOperationException( "predefinedAssemblyBuilder was not specified." );
			}

			DefaultSerializationMethodGeneratorManager.SetUpAssemblyBuilderAttributes( this._predefinedAssemblyBuilder, false );

			var generatorManager = SerializationMethodGeneratorManager.Get( this._predefinedAssemblyBuilder );

			return
				new AssemblyBuilderEmittingContext(
					context,
					typeof( TObject ),
					generatorManager.CreateEmitter( typeof( TObject ), EmitterFlavor.FieldBased )
				);
		}

		protected override void BuildSerializerCodeCore( AssemblyBuilderEmittingContext context )
		{
			this.BuildSerializer( context );
		}
	}
}