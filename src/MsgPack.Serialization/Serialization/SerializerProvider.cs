// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MsgPack.Codecs;
using MsgPack.Serialization.BuiltinSerializers;
using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization
{
	/// <summary>
	///		A single entry point to get or create <see cref="Serializer"/> which is entry point of serialization.
	/// </summary>
	public sealed class SerializerProvider : IObjectSerializerProvider
	{
#warning TODO: Revise
		private static SerializerProvider s_default = new SerializerProvider();

		public static SerializerProvider Default
		{
			get => s_default;
			set => s_default = Ensure.NotNull(value);
		}

		private readonly SerializerBuilderRegistry _builderRegistry;
		private readonly ConcurrentDictionary<RuntimeTypeHandle, IObjectSerializerProvider> _innerProviders;
		private readonly ConcurrentDictionary<RuntimeTypeHandle, Serializer> _serializers;

		internal ISerializerGenerationOptions SerializerGenerationOptions { get; }

		private ResolveObjectSerializerEventHandler? _resolveSerializer;

		// Unity cannot use Interlocked.CompareExchange<T>, so use explicit implementation here.
		/// <summary>
		///		Occurs when the context have not find appropriate registered nor built-in serializer for specified type.
		/// </summary>
		/// <remarks>
		///		This event will be occured when the context could not found known serializer from:
		///		<list type="bullet">
		///			<item>
		///				Known built-in serializers for arrays, nullables, and collections, etc.
		///			</item>
		///			<item>
		///				Known default serializers for some known various FCL value types and some reference types.
		///			</item>
		///			<item>
		///				Previously registered or generated serializer.
		///			</item>
		///		</list>
		///		You can instantiate your custom serializer using various <see cref="ResolveSerializerEventArgs" /> properties,
		///		and then call <see cref="ResolveSerializerEventArgs.SetSerializer{T}"/> to provide toward the context.
		///		<note>
		///			You can use <see cref="SerializerProvider" /> to get dependent serializers as you like, 
		///			but you should never explicitly register new serializer(s) explicitly via the context from the event handler and its dependents.
		///			Instead, you specify the instanciated serializer with <see cref="ResolveSerializerEventArgs.SetSerializer{T}"/> once at a time. 
		///			Dependent serializer(s) should be registered via next (nested) raise of this event.
		///		</note>
		///		<note>
		///			The context implicitly holds 'lock' for the target type for the requested serializer in the current thread.
		///			So you should not use any synchronization primitives in the event handler and its dependents,
		///			or you may face complex dead lock.
		///			That is, the event handler should be as simple as possible like just instanciate the serializer and set it to the event argument.
		///		</note>
		/// </remarks>
		public event Func<ResolveObjectSerializerEventArgs, ObjectSerializer> ResolveObjectSerializer
		{
			add
			{
#if UNITY
				lock(this._resolveSerializerSyncRoot)
				{
					this._resolveSerializer += value;
				}
#else

				ResolveObjectSerializerEventHandler? expectedHandler;
				var actualHandler = this._resolveSerializer;
				do
				{
					expectedHandler = actualHandler;
					var combined = (ResolveObjectSerializerEventHandler)Delegate.Combine(expectedHandler, value);
					actualHandler = Interlocked.CompareExchange(ref this._resolveSerializer, combined, expectedHandler);
				}
				while (expectedHandler != actualHandler);
#endif
			}
			remove
			{
#if UNITY
				lock(this._resolveSerializerSyncRoot)
				{
					// ReSharper disable once DelegateSubtraction
					this._resolveSerializer -= value;
				}
#else
				ResolveObjectSerializerEventHandler? expectedHandler;
				var actualHandler = this._resolveSerializer;
				do
				{
					expectedHandler = actualHandler;
					var removed = (ResolveObjectSerializerEventHandler?)Delegate.Remove(expectedHandler, value);
					actualHandler = Interlocked.CompareExchange(ref this._resolveSerializer, removed, expectedHandler);
				}
				while (expectedHandler != actualHandler);
#endif
			}
		}

		private ObjectSerializer? OnResolveSerializer(Type targetType, PolymorphismSchema schema)
		{
#if UNITY
			lock(this._resolveSerializerSyncRoot)
			{
			var handler = this._resolveSerializer;
#else
			var handler = Interlocked.CompareExchange(ref this._resolveSerializer, null, null);
#endif
			if (handler == null)
			{
				return null;
			}

			// Lazily allocate event args memory.
			var e = new ResolveObjectSerializerEventArgs(this, targetType, schema);
			handler(ref e);
			return e.GetSerializer();
#if UNITY
			}
#endif
		}

		public SerializerProvider()
			: this(new SerializerGenerationOptionsBuilder()) { }

		public SerializerProvider(
			SerializerGenerationOptionsBuilder optionsBuilder
		)
			: this(Ensure.NotNull(optionsBuilder), SerializerBuilderRegistry.Instance) { }

		internal SerializerProvider(
			SerializerGenerationOptionsBuilder optionsBuilder,
			SerializerBuilderRegistry builderRegistry
		)
		{
			this._innerProviders = new ConcurrentDictionary<RuntimeTypeHandle, IObjectSerializerProvider>();
			this._serializers = new ConcurrentDictionary<RuntimeTypeHandle, Serializer>();
			this.SerializerGenerationOptions = optionsBuilder.Build();
			this._builderRegistry = builderRegistry;
		}

		internal Type EnsureDateTimeLikeType(Type targetType)
		{
			Ensure.NotNull(targetType);
			var knownDateTimeLikeTypes = this.SerializerGenerationOptions.DateTimeOptions.KnownDateTimeLikeTypes;

			if (!knownDateTimeLikeTypes.Contains(targetType))
			{
				var nullableUnderlyingType = Nullable.GetUnderlyingType(targetType);
				if (nullableUnderlyingType == null || !knownDateTimeLikeTypes.Contains(nullableUnderlyingType))
				{
					Throw.TargetTypeIsNotDateTimeLikeType(targetType, knownDateTimeLikeTypes);
				}
			}

			return targetType;
		}

		public Serializer GetSerializer(Type targetType, object? providerParameter, CodecProvider codecProvider)
		{
			var context = (This: this, Type: targetType, CodecProvider: codecProvider, ProviderParameter: providerParameter);
			return this._serializers.GetOrAdd(targetType.TypeHandle, (_, x) => x.This.CreateSerializer(x.Type, x.CodecProvider, x.ProviderParameter), context);
		}

		private Serializer CreateSerializer(Type targetType, CodecProvider codecProvider, object? providerParameter)
			=> SerializerFactory.CreateSerializer(
				targetType,
				codecProvider,
				this.GetObjectSerializer(targetType, providerParameter),
				this.SerializerGenerationOptions.SerializationOptions,
				this.SerializerGenerationOptions.DeserializationOptions
			);

		ObjectSerializer IObjectSerializerProvider.GetSerializer(Type targetType, object? providerParameter)
			=> this.GetObjectSerializer(targetType, providerParameter);

		private ObjectSerializer GetObjectSerializer(Type targetType, object? providerParameter)
		{
			var context = (This: this, Type: targetType, ProviderParameter: providerParameter);
			return this._innerProviders.GetOrAdd(targetType.TypeHandle, (_, x) => x.This.CreateObjectSerializerProvider(x.Type, x.ProviderParameter), context).GetSerializer(targetType, providerParameter);
		}

		private readonly object _generationLock = new object();
		private readonly Dictionary<Type, IObjectSerializerProvider> _typeLock = new Dictionary<Type, IObjectSerializerProvider>();

		private IObjectSerializerProvider CreateObjectSerializerProvider(Type targetType, object? providerParameter)
		{
			// Explicitly registered serializer should always used, so get it first.
			if (this._innerProviders.TryGetValue(targetType.TypeHandle, out var provider))
			{
				return provider;
			}

			lock (this._generationLock)
			{
				// Re-get to check because other thread might create the serializer when I wait the lock.
				if (this._innerProviders.TryGetValue(targetType.TypeHandle, out provider))
				{
					return provider;
				}

				try
				{
					if (this._typeLock.TryGetValue(targetType, out var lazyProvider))
					{
						return lazyProvider;
					}

#warning TODO: Revise option
					var isTuple = TupleItems.IsTuple(targetType);
					var metadata =
						isTuple ?
							SerializationMetadataFactory.GetTupleMetadata(targetType) :
							SerializationMetadataFactory.GetObjectMetadata(targetType, this.SerializerGenerationOptions);
					var capabilities = metadata.GetCapabilities();

					this._typeLock[targetType] =
						new StaticObjectSerializerProvider(
							SerializerFactory.CreateObjectSerializer(
								typeof(LazyDelegatingObjectSerializer<>),
								targetType,
								this,
								capabilities
							)
						);

					return this.CreateRealObjectSerializerProvider(targetType, providerParameter, provider, metadata, isTuple);
				}
				finally
				{
					this._typeLock.Remove(targetType);
				}
			}
		}

		private IObjectSerializerProvider CreateRealObjectSerializerProvider(
			Type targetType,
			object? providerParameter,
			IObjectSerializerProvider? provider,
			in SerializationTarget metadata,
			bool isTuple
		)
		{
			var schema = (providerParameter ?? PolymorphismSchema.Create(targetType, null)) as PolymorphismSchema ?? PolymorphismSchema.Default;

#warning TODO: GenericSerializer.Create<T> here! (Mainly, collections)
			//serializer = GenericSerializer.Create<T>(this, schema);

			var builder =
				this.SerializerGenerationOptions.IsRuntimeCodeGenerationDisabled ?
					this._builderRegistry.GetForReflection() :
					this._builderRegistry.GetForRuntimeCodeGeneration();

			var registrationOptions = SerializerRegistrationOptions.WithNullable;
			var serializer = this.OnResolveSerializer(targetType, schema);
			if (serializer == null)
			{
				var underlyingType = Nullable.GetUnderlyingType(targetType);
				if (underlyingType != null)
				{
					provider = this.CreateObjectSerializerProvider(underlyingType, providerParameter);
					registrationOptions &= ~SerializerRegistrationOptions.WithNullable;
				}
				else
				{
					if (targetType.GetIsEnum())
					{
						provider = new EnumSerializerProvider(targetType, this);
					}
					else if (this.SerializerGenerationOptions.DateTimeOptions.KnownDateTimeLikeTypes.Contains(targetType))
					{
						provider = this.CreateDateTimeSerializerProvider(targetType);
					}
					else if (isTuple)
					{
						serializer = builder.BuildTupleSerializer(targetType, this, metadata, this.SerializerGenerationOptions);
					}
					else
					{
						serializer = builder.BuildObjectSerializer(targetType, this, metadata, this.SerializerGenerationOptions);
					}
				}
			}

			provider = this.RegisterSerializerAndProvider(targetType, registrationOptions, provider, serializer);

			return provider;
		}

		private IObjectSerializerProvider CreateDateTimeSerializerProvider(Type targetType)
		{
			if (targetType == typeof(DateTimeOffset))
			{
				return DateTimeSerializerProvider.CreateDateTimeOffset(this);
			}
			else if (targetType == typeof(DateTime))
			{
				return DateTimeSerializerProvider.CreateDateTime(this);
			}
			else if (targetType == typeof(Timestamp))
			{
				return DateTimeSerializerProvider.CreateTimestamp(this);
			}
			else
			{
				Throw.DateTimeSerializerProviderIsNotRegistered(targetType);
				return default; // Never reaches
			}
		}

		public void Register(Type targetType, ObjectSerializer serializer, SerializerRegistrationOptions registrationOptions)
		{
			Ensure.NotNull(targetType);
			Ensure.NotNull(serializer);

			lock (this._generationLock)
			{
				this.RegisterSerializerAndProvider(targetType, registrationOptions, provider: null, serializer);
			}
		}

		public void RegisterDateTimeSerializerProvider(Type targetType, IObjectSerializerProvider provider, SerializerRegistrationOptions registrationOptions)
		{
			this.EnsureDateTimeLikeType(targetType);
			Ensure.NotNull(provider);

			lock (this._generationLock)
			{
				this.RegisterSerializerAndProvider(targetType, registrationOptions, provider, serializer: null);
			}
		}

		private IObjectSerializerProvider RegisterSerializerAndProvider(
			Type targetType,
			SerializerRegistrationOptions registrationOptions,
			IObjectSerializerProvider? provider,
			ObjectSerializer? serializer
		)
		{
			var overwrites = (registrationOptions & SerializerRegistrationOptions.AllowOverride) != 0;

			if (provider == null)
			{
				Debug.Assert(serializer != null);

				if (targetType.GetIsValueType())
				{
					provider = new StaticObjectSerializerProvider(serializer);

					if ((registrationOptions & SerializerRegistrationOptions.WithNullable) != 0)
					{
						// Create nullable companion
						var nullableType = typeof(Nullable<>).MakeGenericType(targetType);
						var nullableProvider =
							new StaticObjectSerializerProvider(
								SerializerFactory.CreateNullableObjectSerializer(
									nullableType,
									targetType,
									this,
									serializer
								)
							);
						if (overwrites)
						{
							this._innerProviders[nullableType.TypeHandle] = nullableProvider;
						}
						else
						{
							this._innerProviders.TryAdd(nullableType.TypeHandle, nullableProvider);
						}
					}
				}
				else
				{
					provider = new PolymorphicSerializerProvider(targetType, serializer!);
				}
			}

			if (overwrites)
			{
				this._innerProviders[targetType.TypeHandle] = provider;
			}
			else
			{
				this._innerProviders.TryAdd(targetType.TypeHandle, provider);
			}

			return provider;
		}
	}
}
