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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if !SILVERLIGHT && !NETFX_35 && !UNITY
using System.Collections.Concurrent;
#else // !SILVERLIGHT && !NETFX_35 && !UNITY
using System.Collections.Generic;
#endif // !SILVERLIGHT && !NETFX_35 && !UNITY
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
#if UNITY || NETFX_CORE
using System.Linq;
#endif // UNITY || NETFX_CORE
#if NETFX_CORE
using System.Linq.Expressions;
#endif // NETFX_CORE
#if UNITY || XAMIOS || XAMDROID || NETFX_CORE
using System.Reflection;
#endif // UNITY || XAMIOS || XAMDROID || NETFX_CORE
using System.Threading;

using MsgPack.Serialization.DefaultSerializers;
using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Represents serialization context information for internal serialization logic.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Rerely instanticated and backward compatibility" )]
	public sealed partial class SerializationContext
	{
#if UNITY
		private static readonly object DefaultContextSyncRoot = new object();
#endif // UNITY

#if UNITY || XAMIOS || XAMDROID
		private static readonly MethodInfo GetSerializer1Method =
			typeof( SerializationContext ).GetMethod( "GetSerializer", new[] { typeof( object ) } );
#endif // UNITY || XAMIOS || XAMDROID


		// Set SerializerRepository null because it requires SerializationContext, so re-init in constructor.
		private static SerializationContext _default = new SerializationContext( PackerCompatibilityOptions.None );

		/// <summary>
		///		Gets or sets the default instance.
		/// </summary>
		/// <value>
		///		The default <see cref="SerializationContext"/> instance.
		/// </value>
		/// <exception cref="ArgumentNullException">The setting value is <c>null</c>.</exception>
		public static SerializationContext Default
		{
			get
			{
#if !UNITY
				return Interlocked.CompareExchange( ref _default, null, null );
#else
				lock( DefaultContextSyncRoot )
				{
					return _default;
				}
#endif // !UNITY
			}
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException( "value" );
				}

#if !UNITY
				Interlocked.Exchange( ref _default, value );
#else
				lock( DefaultContextSyncRoot )
				{
					_default = value;
				}
#endif // !UNITY
			}
		}

		private readonly SerializerRepository _serializers;
#if SILVERLIGHT || NETFX_35 || UNITY
		private readonly Dictionary<Type, object> _typeLock;
#else
		private readonly ConcurrentDictionary<Type, object> _typeLock;
#endif // SILVERLIGHT || NETFX_35 || UNITY

		private readonly object _generationLock;

		/// <summary>
		///		Gets the current <see cref="SerializerRepository"/>.
		/// </summary>
		/// <value>
		///		The  current <see cref="SerializerRepository"/>.
		/// </value>
		public SerializerRepository Serializers
		{
			get
			{
#if !UNITY
				Contract.Ensures( Contract.Result<SerializerRepository>() != null );
#endif // !UNITY

				return this._serializers;
			}
		}

#if XAMIOS || XAMDROID || UNITY_IPHONE || UNITY_ANDROID
		private EmitterFlavor _emitterFlavor = EmitterFlavor.ReflectionBased;
#elif !NETFX_CORE
		private EmitterFlavor _emitterFlavor = EmitterFlavor.FieldBased;
#else
		private EmitterFlavor _emitterFlavor = EmitterFlavor.ExpressionBased;
#endif

		/// <summary>
		///		Gets or sets the <see cref="EmitterFlavor"/>.
		/// </summary>
		/// <value>
		///		The <see cref="EmitterFlavor"/>
		/// </value>
		/// <remarks>
		///		For testing purposes.
		/// </remarks>
		internal EmitterFlavor EmitterFlavor
		{
			get { return this._emitterFlavor; }
			set { this._emitterFlavor = value; }
		}

		private readonly SerializationCompatibilityOptions _compatibilityOptions;

		/// <summary>
		///		Gets the compatibility options.
		/// </summary>
		/// <value>
		///		The <see cref="SerializationCompatibilityOptions"/> which stores compatibility options. This value will not be <c>null</c>.
		/// </value>
		public SerializationCompatibilityOptions CompatibilityOptions
		{
			get
			{
#if !UNITY
				Contract.Ensures( Contract.Result<SerializationCompatibilityOptions>() != null );
#endif // !UNITY

				return this._compatibilityOptions;
			}
		}

		private SerializationMethod _serializationMethod;

		/// <summary>
		///		Gets or sets the <see cref="SerializationMethod"/> to determine serialization strategy.
		/// </summary>
		/// <value>
		///		The <see cref="SerializationMethod"/> to determine serialization strategy.
		/// </value>
		public SerializationMethod SerializationMethod
		{
			get
			{
#if !UNITY
				Contract.Ensures( Enum.IsDefined( typeof( SerializationMethod ), Contract.Result<SerializationMethod>() ) );
#endif // !UNITY

				return this._serializationMethod;
			}
			set
			{
				switch ( value )
				{
					case SerializationMethod.Array:
					case SerializationMethod.Map:
					{
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException( "value" );
					}
				}

#if !UNITY
				Contract.EndContractBlock();
#endif // !UNITY


				this._serializationMethod = value;
			}
		}

		private EnumSerializationMethod _enumSerializationMethod;

		/// <summary>
		///		Gets or sets the <see cref="EnumSerializationMethod"/> to determine default serialization strategy of enum types.
		/// </summary>
		/// <value>
		///		The <see cref="EnumSerializationMethod"/> to determine default serialization strategy of enum types.
		/// </value>
		/// <remarks>
		///		A serialization strategy for specific <strong>member</strong> is determined as following:
		///		<list type="numeric">
		///			<item>If the member is marked with <see cref="MessagePackEnumMemberAttribute"/> and its value is not <see cref="EnumMemberSerializationMethod.Default"/>, then it will be used.</item>
		///			<item>Otherwise, if the enum type itself is marked with <see cref="MessagePackEnumAttribute"/>, then it will be used.</item>
		///			<item>Otherwise, the value of this property will be used.</item>
		/// 	</list>
		///		Note that the default value of this property is <see cref="T:EnumSerializationMethod.ByName"/>, it is not size efficient but tolerant to unexpected enum definition change.
		/// </remarks>
		public EnumSerializationMethod EnumSerializationMethod
		{
			get
			{
#if !UNITY
				Contract.Ensures( Enum.IsDefined( typeof( EnumSerializationMethod ), Contract.Result<EnumSerializationMethod>() ) );
#endif // !UNITY

				return this._enumSerializationMethod;
			}
			set
			{
				switch ( value )
				{
					case EnumSerializationMethod.ByName:
					case EnumSerializationMethod.ByUnderlyingValue:
					{
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException( "value" );
					}
				}

#if !UNITY
				Contract.EndContractBlock();
#endif // !UNITY


				this._enumSerializationMethod = value;
			}
		}

#if !XAMIOS && !UNITY_IPHONE
		private SerializationMethodGeneratorOption _generatorOption;

		/// <summary>
		///		Gets or sets the <see cref="SerializationMethodGeneratorOption"/> to control code generation.
		/// </summary>
		/// <value>
		///		The <see cref="SerializationMethodGeneratorOption"/>.
		/// </value>
		public SerializationMethodGeneratorOption GeneratorOption
		{
			get
			{
#if !UNITY
				Contract.Ensures( Enum.IsDefined( typeof( SerializationMethod ), Contract.Result<SerializationMethodGeneratorOption>() ) );
#endif // !UNITY

				return this._generatorOption;
			}
			set
			{
				switch ( value )
				{
					case SerializationMethodGeneratorOption.Fast:
#if !SILVERLIGHT
					case SerializationMethodGeneratorOption.CanCollect:
					case SerializationMethodGeneratorOption.CanDump:
#endif
					{
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException( "value" );
					}
				}

#if !UNITY
				Contract.EndContractBlock();
#endif // !UNITY


				this._generatorOption = value;
			}
		}
#endif // !XAMIOS && !UNITY_IPHONE

		private readonly DefaultConcreteTypeRepository _defaultCollectionTypes;

		/// <summary>
		///		Gets the default collection types.
		/// </summary>
		/// <value>
		///		The default collection types. This value will not be <c>null</c>.
		/// </value>
		public DefaultConcreteTypeRepository DefaultCollectionTypes
		{
			get { return this._defaultCollectionTypes; }
		}

#if !XAMIOS && !UNITY_IPHONE

		/// <summary>
		///		Gets or sets a value indicating whether runtime generation is disabled or not.
		/// </summary>
		/// <value>
		///		<c>true</c> if runtime generation is disabled; otherwise, <c>false</c>.
		/// </value>
		internal bool IsRuntimeGenerationDisabled { get; set; }
#endif // !XAMIOS && !UNITY_IPHONE

		/// <summary>
		///		Gets or sets the default <see cref="DateTime"/> conversion methods of built-in serializers.
		/// </summary>
		/// <value>
		///		The default <see cref="DateTime"/> conversion methods of built-in serializers. The default is <see cref="F:DateTimeConversionMethod.Native"/>.
		/// </value>
		/// <remarks>
		///		As of 0.6, <see cref="DateTime"/> value is serialized as its native representation instead of interoperable UTC milliseconds Unix epoc.
		///		This behavior solves some debugging problem and interop issues, but breaks compability.
		///		If you want to change this behavior, set this value to <see cref="F:DateTimeConversionMethod.UnixEpoc"/>.
		/// </remarks>
		public DateTimeConversionMethod DefaultDateTimeConversionMethod
		{
			get;
			set;
		}

#if UNITY
		private readonly object _resolveSerializerSyncRoot = new object();
#endif // UNITY

		private EventHandler<ResolveSerializerEventArgs> _resolveSerializer;

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
		///			You can use <see cref="SerializationContext" /> to get dependent serializers as you like, 
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
		public event EventHandler<ResolveSerializerEventArgs> ResolveSerializer
		{
			add
			{
#if UNITY
				lock( this._resolveSerializerSyncRoot )
				{
					this._resolveSerializer += value;
				}
#else

				EventHandler<ResolveSerializerEventArgs> expectedHandler;
				var actualHandler = this._resolveSerializer;
				do
				{
					expectedHandler = actualHandler;
					var combined = ( EventHandler<ResolveSerializerEventArgs> )Delegate.Combine( expectedHandler, value );
					actualHandler = Interlocked.CompareExchange( ref this._resolveSerializer, combined, expectedHandler );
				}
				while ( expectedHandler != actualHandler );
#endif
			}
			remove
			{
#if UNITY
				lock( this._resolveSerializerSyncRoot )
				{
					// ReSharper disable once DelegateSubtraction
					this._resolveSerializer -= value;
				}
#else
				EventHandler<ResolveSerializerEventArgs> expectedHandler;
				var actualHandler = this._resolveSerializer;
				do
				{
					expectedHandler = actualHandler;
					var removed = ( EventHandler<ResolveSerializerEventArgs> )Delegate.Remove( expectedHandler, value );
					actualHandler = Interlocked.CompareExchange( ref this._resolveSerializer, removed, expectedHandler );
				}
				while ( expectedHandler != actualHandler );
#endif
			}
		}

		private MessagePackSerializer<T> OnResolveSerializer<T>( PolymorphismSchema schema )
		{
#if UNITY
			lock( this._resolveSerializerSyncRoot )
			{
			var handler = this._resolveSerializer;
#else
			var handler = Interlocked.CompareExchange( ref this._resolveSerializer, null, null );
#endif
			if ( handler == null )
			{
				return null;
			}

			// Lazily allocate event args memory.
			var e = new ResolveSerializerEventArgs( this, typeof( T ), schema );
			handler( this, e );
			return e.GetFoundSerializer<T>();
#if UNITY
			}
#endif
		}

		/// <summary>
		///		Configures <see cref="Default"/> as new classic <see cref="SerializationContext"/> instance.
		/// </summary>
		/// <returns>The previously set context as <see cref="Default"/>.</returns>
		/// <seealso cref="CreateClassicContext()"/>
		public static SerializationContext ConfigureClassic()
		{
#if !UNITY
			return Interlocked.Exchange( ref _default, CreateClassicContext() );
#else
			lock ( DefaultContextSyncRoot )
			{
				var old = _default;
				_default = CreateClassicContext();
				return old;
			}
#endif // !UNITY
		}

		/// <summary>
		///		Creates a new <see cref="SerializationContext"/> which is configured as same as 0.5.
		/// </summary>
		/// <returns>
		///		A new <see cref="SerializationContext"/> which is configured as same as 0.5.
		/// </returns>
		/// <remarks>
		///		There are breaking changes of <see cref="SerializationContext"/> properties to improve API usability and to prevent accidental failure.
		///		This method returns a <see cref="SerializationContext"/> which configured as classic style settings as follows:
		///		<list type="table">
		///			<listheader>
		///				<term></term>
		///				<description>Default (as of 0.6)</description>
		///				<description>Classic (before 0.6)</description>
		///			</listheader>
		///			<item>
		///				<term>Packed object members order (if members are not marked with <see cref="MessagePackMemberAttribute"/>  nor <c>System.Runtime.Serialization.DataMemberAttribute</c> and serializer uses <see cref="F:SerializationMethod.Array"/>)</term>
		///				<description>As declared (metadata table order)</description>
		///				<description>As lexicographical</description>
		///			</item>
		///			<item>
		///				<term><see cref="DateTime"/> value</term>
		///				<description>Native representation (100-nano ticks, preserving <see cref="DateTimeKind"/>.)</description>
		///				<description>UTC, milliseconds Unix epoc.</description>
		///			</item>
		///		</list>
		/// </remarks>
		public static SerializationContext CreateClassicContext()
		{
			return
				new SerializationContext( PackerCompatibilityOptions.Classic )
				{
					DefaultDateTimeConversionMethod = DateTimeConversionMethod.UnixEpoc
				};
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializationContext"/> class with copy of <see cref="SerializerRepository.GetDefault()"/>.
		/// </summary>
		public SerializationContext()
			: this( PackerCompatibilityOptions.None ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializationContext"/> class with copy of <see cref="SerializerRepository.GetDefault()"/> for specified <see cref="PackerCompatibilityOptions"/>.
		/// </summary>
		/// <param name="packerCompatibilityOptions"><see cref="PackerCompatibilityOptions"/> which will be used on built-in serializers.</param>
		public SerializationContext( PackerCompatibilityOptions packerCompatibilityOptions )
		{
			this._compatibilityOptions =
				new SerializationCompatibilityOptions
				{
					PackerCompatibilityOptions =
						packerCompatibilityOptions
				};

			this._serializers = new SerializerRepository( SerializerRepository.GetDefault( this ) );

#if SILVERLIGHT || NETFX_35 || UNITY
			this._typeLock = new Dictionary<Type, object>();
#else
			this._typeLock = new ConcurrentDictionary<Type, object>();
#endif // SILVERLIGHT || NETFX_35 || UNITY
			this._generationLock = new object();
			this._defaultCollectionTypes = new DefaultConcreteTypeRepository();
#if !XAMIOS &&!UNITY
			this._generatorOption = SerializationMethodGeneratorOption.Fast;
#endif // !XAMIOS && !UNITY
		}

		internal bool ContainsSerializer( Type rootType )
		{
			return
				this._serializers.Contains( rootType )
				|| ( rootType.GetIsGenericType() && this._serializers.Contains( rootType.GetGenericTypeDefinition() ) );
		}

		/// <summary>
		///		Gets the <see cref="MessagePackSerializer{T}"/> with this instance without provider parameter.
		/// </summary>
		/// <typeparam name="T">Type of serialization/deserialization target.</typeparam>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <remarks>
		///		This method automatically register new instance via <see cref="SerializerRepository.Register{T}(MessagePackSerializer{T})"/>.
		/// </remarks>
		public MessagePackSerializer<T> GetSerializer<T>()
		{
			return this.GetSerializer<T>( null );
		}

		/// <summary>
		///		Gets the <see cref="MessagePackSerializer{T}"/> with this instance.
		/// </summary>
		/// <typeparam name="T">Type of serialization/deserialization target.</typeparam>
		/// <param name="providerParameter">A provider specific parameter. See remarks section for details.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <remarks>
		///		<para>
		///			This method automatically register new instance via <see cref="SerializerRepository.Register{T}(MessagePackSerializer{T})"/>.
		///		</para>
		///		<para>
		///			Currently, only following provider parameters are supported.
		///			<list type="table">
		///				<listheader>
		///					<term>Target type</term>
		///					<description>Provider parameter</description>
		///				</listheader>
		///				<item>
		///					<term><see cref="EnumMessagePackSerializer{TEnum}"/> or its descendants.</term>
		///					<description><see cref="EnumSerializationMethod"/>. The returning instance corresponds to this value for serialization.</description>
		///				</item>
		///			</list>
		///			<note><c>null</c> is valid value for <paramref name="providerParameter"/> and it indeicates default behavior of parameter.</note>
		///		</para>
		/// </remarks>
		public MessagePackSerializer<T> GetSerializer<T>( object providerParameter )
		{
#if !UNITY
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );
#endif // !UNITY

			var schema = providerParameter as PolymorphismSchema;
			// Explicitly generated serializer should always used, so get it first.
			MessagePackSerializer<T> serializer = this._serializers.Get<T>( this, providerParameter );

			if ( serializer != null )
			{
				return serializer;
			}

			bool lockTaken = false;
			lock ( this._generationLock )
			{
				// Re-get to check because other thread might create the serializer when I wait the lock.
				serializer = this._serializers.Get<T>( this, providerParameter );

				if ( serializer != null )
				{
					return serializer;
				}

				try
				{
					try {}
					finally
					{
#if SILVERLIGHT || NETFX_35 || UNITY
						lock ( this._typeLock )
						{
							var typeLock = new object();
							object aquiredLock;
							lockTaken = !this._typeLock.TryGetValue( typeof( T ), out aquiredLock );
							if ( lockTaken )
							{
								this._typeLock.Add( typeof( T ), typeLock );
							}
						}
#else
						var typeLock = new object();
						var aquiredTypeLock = this._typeLock.GetOrAdd( typeof( T ), _ => typeLock );
						lockTaken = typeLock == aquiredTypeLock;
#endif // if  SILVERLIGHT || NETFX_35 || UNITY
					}

					if ( lockTaken )
					{
						// First try to create generic serializer w/o code generation.
						serializer = GenericSerializer.Create<T>( this, schema );

						if ( serializer == null )
						{
#if !XAMIOS && !XAMDROID && !UNITY
							if ( this.IsRuntimeGenerationDisabled )
							{
#endif // !XAMIOS && !XAMDROID && !UNITY
								// On debugging, or AOT only envs, use reflection based aproach.
								serializer =
									this.GetSerializerWithoutGeneration<T>( schema )
									?? this.OnResolveSerializer<T>( schema )
									?? MessagePackSerializer.CreateReflectionInternal<T>( this, this.EnsureConcreteTypeRegistered( typeof( T ) ), schema );
#if !XAMIOS && !XAMDROID && !UNITY
							}
							else
							{
								// This thread creating new type serializer.
								serializer = this.OnResolveSerializer<T>( schema ) ?? MessagePackSerializer.CreateInternal<T>( this, schema );
							}
#endif // !XAMIOS && !XAMDROID && !UNITY
						}
					}
					else
					{
						// This thread owns existing lock -- thus, constructing self-composite type.
						return new LazyDelegatingMessagePackSerializer<T>( this, providerParameter );
					}


					// Some types always have to use provider. 
					MessagePackSerializerProvider provider;
					var asEnumSerializer = serializer as ICustomizableEnumSerializer;
					if ( asEnumSerializer != null )
					{
#if DEBUG && !UNITY
						Contract.Assert( typeof( T ).GetIsEnum(), typeof( T ) + " is not enum but generated serializer is ICustomizableEnumSerializer" );
#endif // DEBUG && !UNITY

						provider = new EnumMessagePackSerializerProvider( typeof( T ), asEnumSerializer );
					}
					else
					{
#if DEBUG && !UNITY
						Contract.Assert( !typeof( T ).GetIsEnum(), typeof( T ) + " is enum but generated serializer is not ICustomizableEnumSerializer : " + ( serializer == null ? "null" : serializer.GetType().FullName ) );
#endif // DEBUG && !UNITY

						// Creates provider even if no schema -- the schema might be specified future for the type.
						// It is OK to use polymorphic provider for value type.
#if !UNITY
						provider = new PolymorphicSerializerProvider<T>( serializer );
#else
						provider = new PolymorphicSerializerProvider<T>( this, serializer );
#endif // !UNITY
					}

#if !UNITY
					Type nullableType;
					MessagePackSerializerProvider nullableSerializerProvider;
					SerializerRepository.GetNullableCompanion(
						typeof( T ),
						this,
						serializer,
						out nullableType,
						out nullableSerializerProvider
					);

					this._serializers.Register(
						typeof( T ),
						provider,
						nullableType,
						nullableSerializerProvider,
						SerializerRegistrationOptions.WithNullable
					);
#else
					this._serializers.Register(
						typeof( T ),
						provider,
						null,
						null,
						SerializerRegistrationOptions.None
					);
#endif // !UNITY

					// Re-get to avoid duplicated registration and handle provider parameter or get the one created by prececing thread.
					// If T is null and schema is not provided or default schema is provided, then exception will be thrown here from the new provider.
					return this._serializers.Get<T>( this, providerParameter );
				}
				finally
				{
					if ( lockTaken )
					{
#if SILVERLIGHT || NETFX_35 || UNITY
					lock ( this._typeLock )
					{
						this._typeLock.Remove( typeof( T ) );
					}
#else
						object dummy;
						this._typeLock.TryRemove( typeof( T ), out dummy );
#endif // if SILVERLIGHT || NETFX_35 || UNITY
					}
				}
			}
		}

		private Type EnsureConcreteTypeRegistered( Type mayBeAbstractType )
		{
			if ( !mayBeAbstractType.GetIsAbstract() && !mayBeAbstractType.GetIsInterface() )
			{
				return mayBeAbstractType;
			}

			var concreteType = this.DefaultCollectionTypes.GetConcreteType( mayBeAbstractType );
			if ( concreteType == null )
			{
				throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( mayBeAbstractType );
			}

			return concreteType;
		}


		private MessagePackSerializer<T> GetSerializerWithoutGeneration<T>( PolymorphismSchema schema )
		{
			PolymorphicSerializerProvider<T> provider;
			if ( typeof( T ).GetIsInterface() || typeof( T ).GetIsAbstract() )
			{
				var concreteCollectionType = this._defaultCollectionTypes.GetConcreteType( typeof( T ) );
				if ( concreteCollectionType != null )
				{
					var serializer =
						GenericSerializer.TryCreateAbstractCollectionSerializer( this, typeof( T ), concreteCollectionType, schema );

#if !UNITY
					if ( serializer != null )
					{
						var typedSerializer = serializer as MessagePackSerializer<T>;

#if DEBUG && !UNITY
						Contract.Assert(
							typedSerializer != null,
							serializer.GetType() + " : " + serializer.GetType().GetBaseType() + " is " + typeof( MessagePackSerializer<T> )
						);
#endif // DEBUG && !UNITY

						provider = new PolymorphicSerializerProvider<T>( typedSerializer );
					}
					else
					{
						provider = new PolymorphicSerializerProvider<T>( null );
					}
#else
					provider = new PolymorphicSerializerProvider<T>( this, serializer );
#endif
				}
				else
				{
#if !UNITY
					provider = new PolymorphicSerializerProvider<T>( null );
#else
					provider = new PolymorphicSerializerProvider<T>( this, null );
#endif // !UNITY
				}
			}
			else
			{
				// Go to reflection mode.
				return null;
			}

			// Fail when already registered manually.
			this.Serializers.Register( typeof( T ), provider, null, null, SerializerRegistrationOptions.None );

			return ( MessagePackSerializer<T> ) provider.Get( this, schema );
		}

		/// <summary>
		///		Gets the serializer for the specified <see cref="Type"/>.
		/// </summary>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <returns>
		///		<see cref="IMessagePackSingleObjectSerializer"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		Although <see cref="GetSerializer{T}()"/> is preferred,
		///		this method can be used from non-generic type or methods.
		/// </remarks>
		public IMessagePackSingleObjectSerializer GetSerializer( Type targetType )
		{
			return this.GetSerializer( targetType, null );
		}

		/// <summary>
		///		Gets the serializer for the specified <see cref="Type"/>.
		/// </summary>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <param name="providerParameter">A provider specific parameter. See remarks section of <see cref="GetSerializer{T}(Object)"/> for details.</param>
		/// <returns>
		///		<see cref="IMessagePackSingleObjectSerializer"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		Although <see cref="GetSerializer{T}(Object)"/> is preferred,
		///		this method can be used from non-generic type or methods.
		/// </remarks>
		public IMessagePackSingleObjectSerializer GetSerializer( Type targetType, object providerParameter )
		{
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

#if !UNITY
			Contract.Ensures( Contract.Result<IMessagePackSerializer>() != null );
#endif // !UNITY

			return SerializerGetter.Instance.Get( this, targetType, providerParameter );
		}

		private sealed class SerializerGetter
		{
			public static readonly SerializerGetter Instance = new SerializerGetter();

#if !SILVERLIGHT && !NETFX_35 && !UNITY
			private readonly ConcurrentDictionary<RuntimeTypeHandle, Func<SerializationContext, object, IMessagePackSingleObjectSerializer>> _cache =
				new ConcurrentDictionary<RuntimeTypeHandle, Func<SerializationContext, object, IMessagePackSingleObjectSerializer>>();
#elif UNITY
			private readonly Dictionary<RuntimeTypeHandle, MethodInfo> _cache =
				new Dictionary<RuntimeTypeHandle, MethodInfo>();
#else
			private readonly Dictionary<RuntimeTypeHandle, Func<SerializationContext, object, IMessagePackSingleObjectSerializer>> _cache =
				new Dictionary<RuntimeTypeHandle, Func<SerializationContext, object, IMessagePackSingleObjectSerializer>>();
#endif // !SILVERLIGHT && !NETFX_35 && !UNITY

			private SerializerGetter() { }

			public IMessagePackSingleObjectSerializer Get( SerializationContext context, Type targetType, object providerParameter )
			{
#if UNITY
				MethodInfo method;
				if ( !this._cache.TryGetValue( targetType.TypeHandle, out method ) || method == null )
				{
					method = GetSerializer1Method.MakeGenericMethod( targetType );
					this._cache[ targetType.TypeHandle ] = method;
				}

				return ( IMessagePackSingleObjectSerializer )method.InvokePreservingExceptionType( context, providerParameter );
#else
				Func<SerializationContext, object, IMessagePackSingleObjectSerializer> func;
#if SILVERLIGHT || NETFX_35 || UNITY
				lock ( this._cache )
				{
#endif // SILVERLIGHT || NETFX_35 || UNITY
				if ( !this._cache.TryGetValue( targetType.TypeHandle, out func ) || func == null )
				{
#if !NETFX_CORE
					func =
						Delegate.CreateDelegate(
							typeof( Func<SerializationContext, object, IMessagePackSingleObjectSerializer> ),
							typeof( SerializerGetter<> ).MakeGenericType( targetType ).GetMethod( "Get" )
						) as Func<SerializationContext, object, IMessagePackSingleObjectSerializer>;
#else
					var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
					var providerParameterParameter = Expression.Parameter( typeof( Object ), "providerParameter" );
					func =
						Expression.Lambda<Func<SerializationContext, object, IMessagePackSingleObjectSerializer>>(
							Expression.Call(
								null,
								typeof( SerializerGetter<> ).MakeGenericType( targetType ).GetRuntimeMethods().Single( m => m.Name == "Get" ),
								contextParameter,
								providerParameterParameter
							),
							contextParameter,
							providerParameterParameter
						).Compile();
#endif // !NETFX_CORE
#if DEBUG && !UNITY
					Contract.Assert( func != null, "func != null" );
#endif // if DEBUG && !UNITY
					this._cache[ targetType.TypeHandle ] = func;
				}
#if SILVERLIGHT || NETFX_35 || UNITY
				}
#endif // SILVERLIGHT || NETFX_35 || UNITY
				return func( context, providerParameter );
#endif // UNITY
			}
		}

#if !UNITY
		private static class SerializerGetter<T>
		{
#if !NETFX_CORE
			private static readonly Func<SerializationContext, object, MessagePackSerializer<T>> _func =
				Delegate.CreateDelegate(
					typeof( Func<SerializationContext, object, MessagePackSerializer<T>> ),
#if XAMIOS || XAMDROID
					GetSerializer1Method.MakeGenericMethod( typeof( T ) )
#else
					Metadata._SerializationContext.GetSerializer1_Parameter_Method.MakeGenericMethod( typeof( T ) )
#endif // XAMIOS || XAMDROID
				) as Func<SerializationContext, object, MessagePackSerializer<T>>;
#else
			private static readonly Func<SerializationContext, object, MessagePackSerializer<T>> _func =
				CreateFunc();

			private static Func<SerializationContext, object, MessagePackSerializer<T>> CreateFunc()
			{
				var thisParameter = Expression.Parameter( typeof( SerializationContext ), "this" );
				var providerParameterParameter = Expression.Parameter( typeof( Object ), "providerParameter" );
				return
					Expression.Lambda<Func<SerializationContext, object, MessagePackSerializer<T>>>(
						Expression.Call(
							thisParameter,
							Metadata._SerializationContext.GetSerializer1_Parameter_Method.MakeGenericMethod( typeof( T ) ),
							providerParameterParameter
						),
						thisParameter,
						providerParameterParameter
					).Compile();
			}
#endif // if !NETFX_CORE

			// ReSharper disable UnusedMember.Local
			// This method is invoked via Reflection on SerializerGetter.Get().
			public static IMessagePackSingleObjectSerializer Get( SerializationContext context, object providerParameter )
			{
				return _func( context, providerParameter );
			}
			// ReSharper restore UnusedMember.Local
		}
#endif // !UNITY
	}
}
