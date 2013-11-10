using System;
using System.Diagnostics.Contracts;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal class DynamicMethodEmittingContext : ILEmittingContext
	{
		public ILConstruct Context { get; private set; }

		public DynamicMethodEmittingContext( SerializationContext context, Type targetType, SerializerEmitter emitter )
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
			this.Context = ILConstruct.Argument( 0, typeof( SerializationContext ), "context" );
		}
	}

	internal class DynamicMethodSerializerBuilder<TObject> : ILEmittingSerializerBuilder<DynamicMethodEmittingContext, TObject>
	{
		public DynamicMethodSerializerBuilder()
			: base() { }

		protected override DynamicMethodEmittingContext CreateGenerationContextForSerializerCreation( SerializationContext context )
		{
			return
				new DynamicMethodEmittingContext(
					context,
					typeof( TObject ),
					SerializationMethodGeneratorManager.Get().CreateEmitter( typeof( TObject ), EmitterFlavor.ContextBased )
				);
		}

		protected override ILConstruct EmitThisReferenceExpression( DynamicMethodEmittingContext context )
		{
			throw new NotSupportedException();
		}

		protected override ILConstruct EmitInvokeUnpackTo( DynamicMethodEmittingContext context, ILConstruct unpacker, ILConstruct collection )
		{
			return
				this.EmitInvokeVoidMethod(
					context,
					this.EmitInvokeMethodExpression(
						context,
						context.Context,
						Metadata._SerializationContext.GetSerializer1_Method.MakeGenericMethod( typeof( TObject ) )
					),
					typeof( MessagePackSerializer<TObject> ).GetMethod( "UnpackTo" ),
					unpacker,
					collection
				);
		}

		protected override ILConstruct EmitGetSerializerExpression( DynamicMethodEmittingContext context, Type targetType )
		{
			return
				this.EmitInvokeMethodExpression(
					context,
					context.Context,
					Metadata._SerializationContext.GetSerializer1_Method.MakeGenericMethod( targetType )
				);
		}
	}
}