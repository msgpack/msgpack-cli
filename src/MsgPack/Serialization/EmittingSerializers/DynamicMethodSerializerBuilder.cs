using System;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal class DynamicMethodSerializerBuilder<TObject> : ILEmittingSerializerBuilder<TObject>
	{
		public DynamicMethodSerializerBuilder()
			: base() { }

		protected override ILEmittingContext CreateGenerationContextForSerializerCreation( SerializationContext context )
		{
			return
				new ILEmittingContext(
					context,
					typeof( TObject ),
					SerializationMethodGeneratorManager.Get().CreateEmitter( typeof( TObject ), EmitterFlavor.ContextBased )
				);
		}
	}
}