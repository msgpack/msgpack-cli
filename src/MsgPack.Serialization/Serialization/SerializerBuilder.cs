// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;

namespace MsgPack.Serialization
{
	internal abstract class SerializerBuilder
	{
		public abstract ObjectSerializer BuildObjectSerializer(Type targetType, IObjectSerializerProvider owningProvider, in SerializationTarget target, ISerializerGenerationOptions options);

		public abstract ObjectSerializer BuildTupleSerializer(Type targetType, IObjectSerializerProvider owningProvider, in SerializationTarget target, ISerializerGenerationOptions options);
	}

	internal sealed class SerializerBuilderRegistry
	{
		public static SerializerBuilderRegistry Instance
		{
			get;
#if DEBUG
			private set;
#endif
		} = new SerializerBuilderRegistry();

#if DEBUG
		public static void DebugResetInstance()
			=> Instance = new SerializerBuilderRegistry();
#endif // DEBUG

		private SerializerBuilder? _forSourceCodeGeneration;
		private SerializerBuilder? _forRuntimeCodeGeneration;
		private SerializerBuilder? _forReflection;

		private SerializerBuilderRegistry() { }

		public void RegisterForSourceCodeGeneration(SerializerBuilder builder)
			=> this._forSourceCodeGeneration = builder;

		public void RegisterForRuntimeCodeGeneration(SerializerBuilder builder)
			=> this._forRuntimeCodeGeneration = builder;

		public void RegisterForReflection(SerializerBuilder builder)
			=> this._forReflection = builder;

		public SerializerBuilder GetForSourceCodeGeneration()
			=> this._forSourceCodeGeneration ?? Throw.ForSourceCodeGenerationNotRegistered();

		public SerializerBuilder GetForRuntimeCodeGeneration()
			=> this._forRuntimeCodeGeneration ?? Throw.ForRuntimeCodeGenerationNotRegistered();

		public SerializerBuilder GetForReflection()
			=> this._forReflection ?? Throw.ForReflectionNotRegistered();
	}
}
