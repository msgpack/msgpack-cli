#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		An implementation of <see cref="SerializerBuilder{TContext,TConstruct,TObject}"/> with <see cref="AssemblyBuilder"/>.
	/// </summary>
	/// <typeparam name="TObject">The type of the serializing object.</typeparam>
	internal class AssemblyBuilderSerializerBuilder<TObject> : ILEmittingSerializerBuilder<AssemblyBuilderEmittingContext, TObject>
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="AssemblyBuilderSerializerBuilder{TObject}"/> class for instance creation.
		/// </summary>
		public AssemblyBuilderSerializerBuilder() { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override ILConstruct EmitGetSerializerExpression( AssemblyBuilderEmittingContext context, Type targetType, SerializingMember? memberInfo, PolymorphismSchema itemsSchema )
		{
			var realSchema = itemsSchema ?? PolymorphismSchema.Create( targetType, memberInfo );
			var instructions =
				context.Emitter.RegisterSerializer(
					targetType,
					memberInfo == null
						? EnumMemberSerializationMethod.Default
						: memberInfo.Value.GetEnumMemberSerializationMethod(),
					memberInfo == null
						? DateTimeMemberConversionMethod.Default
						: memberInfo.Value.GetDateTimeMemberConversionMethod(),
					realSchema,
					() => this.EmitConstructPolymorphismSchema( context, realSchema ) 
				);

			return
				ILConstruct.Instruction(
					"getserializer",
					typeof( MessagePackSerializer<> ).MakeGenericType( targetType ),
					false,
					// Both of this pointer for FieldBasedSerializerEmitter and context argument of methods for ContextBasedSerializerEmitter are 0.
					il => instructions( il, 0 )
				);
		}

		private IEnumerable<ILConstruct> EmitConstructPolymorphismSchema(
			AssemblyBuilderEmittingContext context,
			PolymorphismSchema currentSchema 
		)
		{
			var schema = this.DeclareLocal( context, typeof( PolymorphismSchema ), "schema" );
			
			yield return schema;
			
			foreach ( var construct in this.EmitConstructPolymorphismSchema( context, schema, currentSchema ) )
			{
				yield return construct;
			}

			yield return this.EmitLoadVariableExpression( context, schema );
		}
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override ILConstruct EmitFieldOfExpression( AssemblyBuilderEmittingContext context, FieldInfo field )
		{
			var instructions =
				context.Emitter.RegisterField(
					field
				);

			return
				ILConstruct.Instruction(
					"getfield",
					typeof( FieldInfo ),
					false,
					// Both of this pointer for FieldBasedSerializerEmitter and context argument of methods for ContextBasedSerializerEmitter are 0.
					il => instructions( il, 0 )
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override ILConstruct EmitMethodOfExpression( AssemblyBuilderEmittingContext context, MethodBase method )
		{
			var instructions =
				context.Emitter.RegisterMethod(
					method
				);

			return
				ILConstruct.Instruction(
					"getsetter",
					typeof( MethodBase ),
					false,
				// Both of this pointer for FieldBasedSerializerEmitter and context argument of methods for ContextBasedSerializerEmitter are 0.
					il => instructions( il, 0 )
				);
		}

		protected override AssemblyBuilderEmittingContext CreateCodeGenerationContextForSerializerCreation( SerializationContext context )
		{
			string serializerTypeName, serializerTypeNamespace;
			DefaultSerializerNameResolver.ResolveTypeName(
				true,
				typeof( TObject ),
				this.GetType().Namespace,
				out serializerTypeName,
				out serializerTypeNamespace 
			);
			var spec =
				new SerializerSpecification(
					typeof( TObject ),
					CollectionTraitsOfThis,
					serializerTypeName,
					serializerTypeNamespace
				);

			return
				new AssemblyBuilderEmittingContext(
					context,
					typeof( TObject ),
					() => SerializationMethodGeneratorManager.Get().CreateEmitter( spec, BaseClass, EmitterFlavor.FieldBased ),
					() => SerializationMethodGeneratorManager.Get().CreateEnumEmitter( context, spec, EmitterFlavor.FieldBased )
				);
		}

#if !SILVERLIGHT
		protected override void BuildSerializerCodeCore( ISerializerCodeGenerationContext context, Type concreteType, PolymorphismSchema itemSchema )
		{
			var asAssemblyBuilderCodeGenerationContext = context as AssemblyBuilderCodeGenerationContext;
			if ( asAssemblyBuilderCodeGenerationContext == null )
			{
				throw new ArgumentException(
					"'context' was not created with CreateGenerationContextForCodeGeneration method.",
					"context"
				);
			}

			var emittingContext =
				asAssemblyBuilderCodeGenerationContext.CreateEmittingContext(
					typeof( TObject ),
					CollectionTraitsOfThis,
					BaseClass
				);

			if ( !typeof( TObject ).GetIsEnum() )
			{
				this.BuildSerializer( emittingContext, concreteType, itemSchema );
				// Finish type creation, and discard returned ctor.
				emittingContext.Emitter.CreateConstructor<TObject>();
			}
			else
			{
				this.BuildEnumSerializer( emittingContext );
				// Finish type creation, and discard returned ctor.
				emittingContext.EnumEmitter.CreateConstructor<TObject>();
			}
		}
#endif
	}
}