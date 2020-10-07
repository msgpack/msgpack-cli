// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.ComponentModel;

namespace MsgPack.Serialization
{
	/// <summary>
	///		A builder object for options which affect serializer generation and generated serializer behavior.
	/// </summary>
	public sealed class SerializerGenerationOptionsBuilder
	{
		// Following fields are initialized lazily to avoid unnecessary memory allocation
		// because there are no configurations needed in usual cases.

		private BindingOptionsBuilder? _bindingOptionsBuilder;

		private SerializationCompatibilityOptionsBuilder? _compatibilityOptionsBuilder;

		private DefaultConcreteTypeRepositoryBuilder? _defaultCollectionTypesBuilder;

		private DictionarySerializationOptionsBuilder? _dictionaryOptionsBuilder;

		private EnumSerializationOptionsBuilder? _enumOptionsBuilder;

		private DateTimeSerializationOptionsBuilder? _dateTimeOptionsBuilder;

		private SerializationOptionsBuilder? _serializationOptionsBuilder;

		private DeserializationOptionsBuilder? _deserializationOptionsBuilder;

		/// <summary>
		///		Gets the value indicating whether the serializer runtime allows serialization even if feature complete serializer cannot be generated due to lack of some requirement.
		/// </summary>
		/// <value>
		///		<c>true</c> if the serializer runtime allows serialization even if feature complete serializer cannot be generated due to lack of some requirement; otherwise, <c>false</c>. The default is <c>false</c>.
		/// </value>
		/// <remarks>
		///		Currently, the lack of constructor (default or parameterized) or lack of settable members are considerd as "cannot generate feature complete serializer".
		///		Therefore, you can get serialization only serializer if this property is set to <c>true</c>.
		///		This is useful for logging, telemetry injestion, or so.
		///		You can investigate serializer capability via <see cref="Serializer.Capabilities"/> property.
		/// </remarks>
		public bool AllowsAsymmetricSerializer { get; private set; }

		/// <summary>
		///		Gets the value which indicates whether on-the-fly runtime code generation is disabled.
		/// </summary>
		/// <value>
		///		<c>true</c> if runtime code generation (via <c>System.Reflection.Emit)</c>) is disabled; <c>false</c>, otherwise.
		/// </value>
		/// <remarks>
		///		If this value set to <c>false</c>, reflection based serializers will be used if they are activated.
		///		If it was not activated, only pre-registered serializers will be used.
		///		Typically, this value will be used for debugging purposes.
		/// </remarks>
		public bool IsRuntimeCodeGenerationDisabled { get; private set; }

		private const string DefaultRuntimeCodeGenerationAssemblyName = "MsgPack.Serialization.EmittingSerializers.GeneratedSerealizers";

		/// <summary>
		///		Gets the name of the assembly which holds generated serializer types.
		/// </summary>
		/// <value>
		///		The name of the assembly which holds generated serializer types.
		///		Default value is <c>MsgPack.Serialization.EmittingSerializers.GeneratedSerealizers</c>.
		/// </value>
		public string RuntimeCodeGenerationAssemblyName { get; private set; } = DefaultRuntimeCodeGenerationAssemblyName;

		/// <summary>
		///		This is an advanced option.
		///		Gets the value which indicates runtime code generation flavor.
		/// </summary>
		/// <value>
		///		The value which indicates runtime code generation flavor.
		/// </value>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public SerializationMethodGeneratorOption RuntimeGeneratedCodeOption { get; private set; } = SerializationMethodGeneratorOption.Fast;

		/// <summary>
		///		Indicates serialization runtime should allow serialization even if feature complete serializer cannot be generated due to lack of some requirement.
		/// </summary>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		You can investigate serializer capability via <see cref="Serializer.Capabilities"/> property.
		/// </remarks>
		public SerializerGenerationOptionsBuilder AllowAsymmetricSerializer()
		{
			this.AllowsAsymmetricSerializer = true;
			return this;
		}

		/// <summary>
		///		Indicates serialization runtime should NOT allow serialization even if feature complete serializer cannot be generated due to lack of some requirement.
		/// </summary>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="AllowsAsymmetricSerializer"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public SerializerGenerationOptionsBuilder DisallowAsymmetricSerializer()
		{
			this.AllowsAsymmetricSerializer = false;
			return this;
		}

		/// <summary>
		///		Indicates serialization runtime can use runtime code generation for serializers.
		/// </summary>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="IsRuntimeCodeGenerationDisabled"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public SerializerGenerationOptionsBuilder EnableRuntimeCodeGeneration()
		{
			this.IsRuntimeCodeGenerationDisabled = false;
			return this;
		}

		/// <summary>
		///		Indicates serialization runtime cannot use runtime code generation for serializers.
		/// </summary>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		In some platform such as iOS prohibit JIT (Just In Time compilation), it also disable runtime code generation.
		///		If your solution have to run on such environment, disabling runtime code generation help code consistency and stability.
		///		That means when you forget to register pre-generated serializers, you will get an exception instead of on-the-fly serializer generation.
		/// </remarks>
		public SerializerGenerationOptionsBuilder DisableRuntimeCodeGeneration()
		{
			this.IsRuntimeCodeGenerationDisabled = true;
			return this;
		}

		/// <summary>
		///		Indicates serialization runtime uses default assembly name for the assembly which will contain all generated serializer types.
		/// </summary>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="RuntimeCodeGenerationAssemblyName"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public SerializerGenerationOptionsBuilder UseDefaultRuntimeCodeGenerationAssemblyName()
		{
			this.RuntimeCodeGenerationAssemblyName = DefaultRuntimeCodeGenerationAssemblyName;
			return this;
		}

		/// <summary>
		///		Sets the name of the assembly which holds generated serializer types.
		/// </summary>
		/// <param name="value">
		///		The name of the assembly which holds generated serializer types.
		///		Default value is <c>MsgPack.Serialization.EmittingSerializers.GeneratedSerealizers</c>.
		/// </param>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="value"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="value"/> is empty string, or contains white spaces only.
		/// </exception>
		/// <remarks>
		///		You should use this method when you depends on <see cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/> in 
		///		the assembly which defines your non public target types.
		///		In such case, you must mark that assembly with <see cref="System.Runtime.CompilerServices.InternalsVisibleToAttribute"/> which accepts the assembly name specified to this property.
		/// </remarks>
		public SerializerGenerationOptionsBuilder UseCustomRuntimeCodeGenerationAssemblyName(string value)
		{
			this.RuntimeCodeGenerationAssemblyName = Ensure.NotBlank(value);
			return this;
		}

		/// <summary>
		///		This is an advanced option.
		///		Sets the value which indicates runtime code generation flavor.
		/// </summary>
		/// <param name="value">
		///		The value which indicates runtime code generation flavor.
		/// </param>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="value"/> is not known <see cref="SerializationMethodGeneratorOption"/> value.
		/// </exception>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public SerializerGenerationOptionsBuilder TweakRuntimeGeneratedCodeOption(SerializationMethodGeneratorOption value)
		{
			switch (value)
			{
				case SerializationMethodGeneratorOption.CanCollect:
				case SerializationMethodGeneratorOption.CanDump:
				case SerializationMethodGeneratorOption.Fast:
				{
					this.RuntimeGeneratedCodeOption = value;
					break;
				}
				default:
				{
					Throw.UndefinedEnumMember(value);
					break; // never
				}
			}

			return this;
		}

		/// <summary>
		///		Initializes a new instance of <see cref="SerializerGenerationOptionsBuilder"/> object.
		/// </summary>
		public SerializerGenerationOptionsBuilder() { }

		/// <summary>
		///		Configures binding options of target members.
		/// </summary>
		/// <param name="configure">A delegate which configure the options through passed <see cref="BindingOptionsBuilder"/>.</param>
		/// <returns>This <see cref="SerializerGenerationOptionsBuilder"/> instance.</returns>
		public SerializerGenerationOptionsBuilder ConfigureBindingOptions(Action<BindingOptionsBuilder> configure)
		{
			Ensure.NotNull(configure);
			var builder = this._bindingOptionsBuilder ??= new BindingOptionsBuilder();
			configure(builder);
			return this;
		}

		internal IBindingOptions BuildBindngOptions()
			=> new ImmutableBindingOptions(this._bindingOptionsBuilder ?? BindingOptionsBuilder.Default);

		/// <summary>
		///		Configures compatibility options of serialization runtime.
		/// </summary>
		/// <param name="configure">A delegate which configure the options through passed <see cref="SerializationCompatibilityOptionsBuilder"/>.</param>
		/// <returns>This <see cref="SerializerGenerationOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		A term 'compatibility' has 2 meanings:
		///		<list type="number">
		///			<item>Backward compatibility for old design.</item>
		///			<item>User code compatibility for other wellknown libraries.</item>
		///		</list>
		/// </remarks>
		public SerializerGenerationOptionsBuilder ConfigureCompatibilityOptions(Action<SerializationCompatibilityOptionsBuilder> configure)
		{
			Ensure.NotNull(configure);
			var builder = this._compatibilityOptionsBuilder ??= new SerializationCompatibilityOptionsBuilder();
			configure(builder);
			return this;
		}

		internal ISerializationCompatibilityOptions BuildCompatibilityOptions()
			=> this._compatibilityOptionsBuilder == null ? ImmutableSerializationCompatibilityOptions.Default : new ImmutableSerializationCompatibilityOptions(this._compatibilityOptionsBuilder);

		/// <summary>
		///		Configures the repository of known concrete collection types for abstract collection types on deserialization. 
		/// </summary>
		/// <param name="configure">A delegate which configure the options through passed <see cref="DefaultConcreteTypeRepositoryBuilder"/>.</param>
		/// <returns>This <see cref="SerializerGenerationOptionsBuilder"/> instance.</returns>
		public SerializerGenerationOptionsBuilder ConfigureDefaultCollectionType(Action<DefaultConcreteTypeRepositoryBuilder> configure)
		{
			Ensure.NotNull(configure);
			var builder = this._defaultCollectionTypesBuilder ??= new DefaultConcreteTypeRepositoryBuilder();
			configure(builder);
			return this;
		}

		internal IDefaultConcreteTypeRepository BuildDefaultCollectionTypes()
			=> this._defaultCollectionTypesBuilder == null ? ImmutableDefaultConcreteTypeRepository.Default : this._defaultCollectionTypesBuilder.Build();

		/// <summary>
		///		Configures enum serialization options. 
		/// </summary>
		/// <param name="configure">A delegate which configure the options through passed <see cref="EnumSerializationOptionsBuilder"/>.</param>
		/// <returns>This <see cref="SerializerGenerationOptionsBuilder"/> instance.</returns>
		public SerializerGenerationOptionsBuilder ConfigureEnumSerializationOptions(Action<EnumSerializationOptionsBuilder> configure)
		{
			Ensure.NotNull(configure);
			var builder = this._enumOptionsBuilder ??= new EnumSerializationOptionsBuilder();
			configure(builder);
			return this;
		}

		internal IEnumSerializationOptions BuildEnumOptions()
			=> new ImmutableEnumSerializationOptions(this._enumOptionsBuilder ?? EnumSerializationOptionsBuilder.Default);

		/// <summary>
		///		Configures <see cref="DateTime"/> and similar types serialization behavior
		/// </summary>
		/// <param name="configure">A delegate which configure the options through passed <see cref="DateTimeSerializationOptionsBuilder"/>.</param>
		/// <returns>This <see cref="SerializerGenerationOptionsBuilder"/> instance.</returns>
		public SerializerGenerationOptionsBuilder ConfigureDateTimeSerializationOptions(Action<DateTimeSerializationOptionsBuilder> configure)
		{
			Ensure.NotNull(configure);
			var builder = this._dateTimeOptionsBuilder ??= new DateTimeSerializationOptionsBuilder();
			configure(builder);
			return this;
		}

		internal IDateTimeSerializationOptions BuildDateTimeOptions()
			=> new ImmutableDateTimeSerializationOptions(this._dateTimeOptionsBuilder ?? DateTimeSerializationOptionsBuilder.Default);

		/// <summary>
		///		Configures <see cref="DateTime"/> and similar types serialization behavior.
		/// </summary>
		/// <param name="configure">A delegate which configure the options through passed <see cref="DictionarySerializationOptionsBuilder"/>.</param>
		/// <returns>This <see cref="SerializerGenerationOptionsBuilder"/> instance.</returns>
		public SerializerGenerationOptionsBuilder ConfigureDictionarySerializationOptions(Action<DictionarySerializationOptionsBuilder> configure)
		{
			Ensure.NotNull(configure);
			var builder = this._dictionaryOptionsBuilder ??= new DictionarySerializationOptionsBuilder();
			configure(builder);
			return this;
		}

		internal IDictionarySerializationOptions BuildDictionaryOptions()
			=> this._dictionaryOptionsBuilder == null ? ImmutableDictionarySerializationOptions.Default : new ImmutableDictionarySerializationOptions(this._dictionaryOptionsBuilder);

		/// <summary>
		///		Configures options for each serialization operations.
		/// </summary>
		/// <param name="configure">A delegate which configure the options through passed <see cref="SerializationOptionsBuilder"/>.</param>
		/// <returns>This <see cref="SerializerGenerationOptionsBuilder"/> instance.</returns>
		public SerializerGenerationOptionsBuilder ConfigureSerializationOptions(Action<SerializationOptionsBuilder> configure)
		{
			Ensure.NotNull(configure);
			var builder = this._serializationOptionsBuilder ??= new SerializationOptionsBuilder();
			configure(builder);
			return this;
		}

		internal SerializationOptions BuildSerializationOptions()
			=> this._serializationOptionsBuilder == null ? SerializationOptions.Default : new SerializationOptions(this._serializationOptionsBuilder);

		/// <summary>
		///		Configures options for each deserialization operations.
		/// </summary>
		/// <param name="configure">A delegate which configure the options through passed <see cref="DeserializationOptionsBuilder"/>.</param>
		/// <returns>This <see cref="SerializerGenerationOptionsBuilder"/> instance.</returns>
		public SerializerGenerationOptionsBuilder ConfigureDeserializationOptions(Action<DeserializationOptionsBuilder> configure)
		{
			Ensure.NotNull(configure);
			var builder = this._deserializationOptionsBuilder ??= new DeserializationOptionsBuilder();
			configure(builder);
			return this;
		}

		internal DeserializationOptions BuildDeserializationOptions()
			=> this._deserializationOptionsBuilder == null ? DeserializationOptions.Default : new DeserializationOptions(this._deserializationOptionsBuilder);

		internal ISerializerGenerationOptions Build() => new ImmutableSerializerGenerationOptions(this);
	}
}
