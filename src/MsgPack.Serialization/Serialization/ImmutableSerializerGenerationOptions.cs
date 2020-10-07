// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Runtime.CompilerServices;

namespace MsgPack.Serialization
{
	/// <summary>
	///		An internal immutable implementation of <see cref="ISerializerGenerationOptions"/>.
	/// </summary>
	internal sealed class ImmutableSerializerGenerationOptions : ISerializerGenerationOptions
	{
		/// <summary>
		///		Indicates whether runtime code generation with <c>System.Reflection.Emit</c> can be used in current environment.
		/// </summary>
		internal static readonly bool CanEmit = DetermineCanEmit();

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool DetermineCanEmit()
		{
#if FEATURE_RUNTIME_FEATURES
			return RuntimeFeature.IsDynamicCodeCompiled;
#else
#if (NETSTANDARD1_3 || NETSTANDARD2_0) && !WINDOWS_UWP
			try
			{
				return DetermineCanEmitCore();
			}
			catch
			{
				return false;
			}
#elif NETFX_CORE || UNITY
			return false;
#else
			// Desktop etc.
			return true;
#endif
#endif
		}

#if (NETSTANDARD1_3 || NETSTANDARD2_0) && !WINDOWS_UWP

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool DetermineCanEmitCore()
		{
			return SerializationMethodGeneratorManager.Fast != null;
		}

#endif // (NETSTANDARD1_3 || NETSTANDARD2_0) && !WINDOWS_UWP

		/// <inheritdoc />
		public bool AllowsAsymmetricSerializer { get; }

		/// <inheritdoc />
		public bool IsRuntimeCodeGenerationDisabled { get; }

		/// <inheritdoc />
		public string RuntimeCodeGenerationAssemblyName { get; }

		/// <inheritdoc />
		public IBindingOptions BindingOptions { get; }

		/// <inheritdoc />
		public ISerializationCompatibilityOptions CompatibilityOptions { get; }

		/// <inheritdoc />
		public IDictionarySerializationOptions DictionaryOptions { get; }

		/// <inheritdoc />
		public IEnumSerializationOptions EnumOptions { get; }

		/// <inheritdoc />
		public IDateTimeSerializationOptions DateTimeOptions { get; }

		/// <inheritdoc />
		public IDefaultConcreteTypeRepository DefaultCollectionTypes { get; }

		/// <inheritdoc />
		public SerializationOptions SerializationOptions { get; }

		/// <inheritdoc />
		public DeserializationOptions DeserializationOptions { get; }

		internal ImmutableSerializerGenerationOptions(SerializerGenerationOptionsBuilder builder)
		{
			this.AllowsAsymmetricSerializer = builder.AllowsAsymmetricSerializer;
			this.IsRuntimeCodeGenerationDisabled = builder.IsRuntimeCodeGenerationDisabled;
			this.RuntimeCodeGenerationAssemblyName = builder.RuntimeCodeGenerationAssemblyName;
			this.BindingOptions = builder.BuildBindngOptions();
			this.CompatibilityOptions = builder.BuildCompatibilityOptions();
			this.DictionaryOptions = builder.BuildDictionaryOptions();
			this.EnumOptions = builder.BuildEnumOptions();
			this.DateTimeOptions = builder.BuildDateTimeOptions();
			this.DefaultCollectionTypes = builder.BuildDefaultCollectionTypes();
			this.SerializationOptions = builder.BuildSerializationOptions();
			this.DeserializationOptions = builder.BuildDeserializationOptions();
		}
	}
}
