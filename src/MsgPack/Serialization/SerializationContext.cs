#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif // !UNITY || MSGPACK_UNITY_FULL
#if FEATURE_CONCURRENT
using System.Collections.Concurrent;
#else // !FEATURE_CONCURRENT
using System.Collections.Generic;
#endif // !SILVERLIGHT && !NET35 && !UNITY
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
#if UNITY || NETSTANDARD1_1 || NETSTANDARD1_3
using System.Linq;
#endif // UNITY || NETSTANDARD1_1 || NETSTANDARD1_3
#if UNITY || WINDOWS_PHONE || WINDOWS_UWP
using System.Reflection;
#endif // UNITY || WINDOWS_PHONE || WINDOWS_UWP 
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
#warning WORKAROUND
		private static readonly object DefaultContextSyncRoot = new object();
#endif // UNITY

#if UNITY || WINDOWS_PHONE || WINDOWS_UWP
		private static readonly MethodInfo GetSerializer1Method =
			typeof( SerializationContext ).GetRuntimeMethod( "GetSerializer", new[] { typeof( object ) } );
#endif // UNITY || WINDOWS_PHONE || WINDOWS_UWP


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
#if !FEATURE_CONCURRENT
		private readonly Dictionary<Type, object> _typeLock;
#else
		private readonly ConcurrentDictionary<Type, object> _typeLock;
#endif // !FEATURE_CONCURRENT

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
#if DEBUG
				Contract.Ensures( Contract.Result<SerializerRepository>() != null );
#endif // DEBUG

				return this._serializers;
			}
		}

		private readonly SerializerOptions _serializerGeneratorOptions;

		/// <summary>
		///		Gets the option settings for serializer generation.
		/// </summary>
		/// <value>
		///		The option settings for serializer generation.
		///		This value will not be <c>null</c>.
		/// </value>
		public SerializerOptions SerializerOptions
		{
			get
			{
#if DEBUG
				Contract.Ensures( Contract.Result<SerializerOptions>() != null );
#endif // DEBUG

				return this._serializerGeneratorOptions;
			}
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
#if DEBUG
				Contract.Ensures( Contract.Result<SerializationCompatibilityOptions>() != null );
#endif // DEBUG

				return this._compatibilityOptions;
			}
		}

		private readonly DictionarySerializationOptions _dictionarySerializationOptions;

		/// <summary>
		///		Gets the dictionary(map) based serialization options.
		/// </summary>
		/// <value>
		///		The <see cref="DictionarySerializationOptions"/> which stores dictionary(map) based serialization options. This value will not be <c>null</c>.
		/// </value>
		public DictionarySerializationOptions DictionarySerializationOptions
		{
			get
			{
#if DEBUG
				Contract.Ensures( Contract.Result<DictionarySerializationOptions>() != null );
#endif // DEBUG

				return this._dictionarySerializationOptions;
			}
		}

		/// <summary>
		///		Gets the dictionary(map) based serialization options.
		/// </summary>
		/// <value>
		///		The <see cref="DictionarySerializationOptions"/> which stores dictionary(map) based serialization options. This value will not be <c>null</c>.
		/// </value>
		[Obsolete("Use DictionarySerializationOption instead.")]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable(EditorBrowsableState.Never)]
#endif
		public DictionarySerializationOptions DictionarySerlaizationOptions
		{
			get
			{
#if DEBUG
				Contract.Ensures( Contract.Result<DictionarySerializationOptions>() != null );
#endif // DEBUG

				return this._dictionarySerializationOptions;
			}
		}

		private int _serializationMethod;

		/// <summary>
		///		Gets or sets the <see cref="SerializationMethod"/> to determine serialization strategy.
		/// </summary>
		/// <value>
		///		The <see cref="SerializationMethod"/> to determine serialization strategy.
		/// </value>
		/// <exception cref="ArgumentOutOfRangeException">The setting value is invalid as <see cref="SerializationMethod"/> enum.</exception>
		public SerializationMethod SerializationMethod
		{
			get
			{
#if DEBUG
				Contract.Ensures( Enum.IsDefined( typeof( SerializationMethod ), Contract.Result<SerializationMethod>() ) );
#endif // DEBUG

				return ( SerializationMethod )Volatile.Read( ref this._serializationMethod );
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

				Contract.EndContractBlock();

				Volatile.Write( ref this._serializationMethod, ( int )value );
			}
		}

		private readonly EnumSerializationOptions _enumSerializationOptions;

		/// <summary>
		///		Gets the enum serialization options.
		/// </summary>
		/// <value>
		///		The <see cref="Serialization.EnumSerializationOptions"/> which stores enum serialization options. This value will not be <c>null</c>.
		/// </value>
		public EnumSerializationOptions EnumSerializationOptions
		{
			get
			{
#if DEBUG
				Contract.Ensures( Contract.Result<EnumSerializationOptions>() != null );
#endif // DEBUG

				return this._enumSerializationOptions;
			}
		}

		/// <summary>
		///		Gets or sets the <see cref="EnumSerializationMethod"/> to determine default serialization strategy of enum types.
		/// </summary>
		/// <value>
		///		The <see cref="EnumSerializationMethod"/> to determine default serialization strategy of enum types.
		/// </value>
		/// <exception cref="ArgumentOutOfRangeException">The setting value is invalid as <see cref="EnumSerializationMethod"/> enum.</exception>
		/// <remarks>
		///		<note>This property is wrapper for <see cref="Serialization.EnumSerializationOptions.SerializationMethod"/> property.</note>
		///		A serialization strategy for specific <strong>member</strong> is determined as following:
		///		<list type="numeric">
		///			<item>If the member is marked with <see cref="MessagePackEnumMemberAttribute"/> and its value is not <see cref="EnumMemberSerializationMethod.Default"/>, then it will be used.</item>
		///			<item>Otherwise, if the enum type itself is marked with <see cref="MessagePackEnumAttribute"/>, then it will be used.</item>
		///			<item>Otherwise, the value of this property will be used.</item>
		/// 	</list>
		///		Note that the default value of this property is <see cref="T:EnumSerializationMethod.ByName"/>, it is not size efficient but tolerant to unexpected enum definition change.
		/// </remarks>
		[Obsolete( "Use EnumSerializationOptions.SerializationMethod instead." )]
		public EnumSerializationMethod EnumSerializationMethod
		{
			get { return this._enumSerializationOptions.SerializationMethod; }
			set { this._enumSerializationOptions.SerializationMethod = value; }
		}

#if !UNITY

		/// <summary>
		///		Gets or sets the <see cref="SerializationMethodGeneratorOption"/> to control code generation.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">The setting value is invalid as <see cref="SerializationMethodGeneratorOption"/> enum.</exception>
		/// <value>
		///		The <see cref="SerializationMethodGeneratorOption"/>.
		/// </value>
		[Obsolete( "Use SerializerOptions.GeneratorOption instead." )]
		public SerializationMethodGeneratorOption GeneratorOption
		{

			get { return this._serializerGeneratorOptions.GeneratorOption; }
			set { this._serializerGeneratorOptions.GeneratorOption = value; }
		}

#endif // !UNITY

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

		private int _defaultDateTimeConversionMethod = ( int )DateTimeConversionMethod.Timestamp;

		/// <summary>
		///		Gets or sets the default <see cref="DateTime"/> conversion methods of built-in serializers.
		/// </summary>
		/// <value>
		///		The default <see cref="DateTime"/> conversion methods of built-in serializers. The default is <see cref="F:DateTimeConversionMethod.Native"/>.
		/// </value>
		/// <exception cref="ArgumentOutOfRangeException">The setting value is invalid as <see cref="SerializationMethod"/> enum.</exception>
		/// <remarks>
		///		As of 0.6, <see cref="DateTime"/> value is serialized as its native representation instead of interoperable UTC milliseconds Unix epoc.
		///		This behavior solves some debugging problem and interop issues, but breaks compability.
		///		If you want to change this behavior, set this value to <see cref="F:DateTimeConversionMethod.UnixEpoc"/>.
		/// </remarks>
		public DateTimeConversionMethod DefaultDateTimeConversionMethod
		{
			get { return ( DateTimeConversionMethod )Volatile.Read( ref this._defaultDateTimeConversionMethod ); }
			set
			{
				switch ( value )
				{
					case DateTimeConversionMethod.Native:
					case DateTimeConversionMethod.UnixEpoc:
					case DateTimeConversionMethod.Timestamp:
					case DateTimeConversionMethod.Iso8601ExtendedFormat:
					{
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException( "value" );
					}
				}

				Contract.EndContractBlock();

				Volatile.Write( ref this._defaultDateTimeConversionMethod, ( int )value );
			}
		}

#if UNITY
#warning WORKAROUND
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
		///		Configures <see cref="Default"/> as new classic <see cref="SerializationContext"/> instance as compatible for ver 0.5.
		/// </summary>
		/// <returns>The previously set context as <see cref="Default"/>.</returns>
		/// <seealso cref="CreateClassicContext()"/>
		[Obsolete( "Use ConfigureClassic(SerializationCompatibilityLevel) instead." )]
		public static SerializationContext ConfigureClassic()
		{
			return ConfigureClassic( SerializationCompatibilityLevel.Version0_5 );
		}

		/// <summary>
		///		Configures <see cref="Default"/> as new classic <see cref="SerializationContext"/> instance as compatible for sepcified version.
		/// </summary>
		/// <param name="compatibilityLevel">A <see cref="SerializationCompatibilityLevel"/> to specify compatibility level.</param>
		/// <returns>The previously set context as <see cref="Default"/>.</returns>
		/// <seealso cref="CreateClassicContext(SerializationCompatibilityLevel)"/>
		public static SerializationContext ConfigureClassic(SerializationCompatibilityLevel compatibilityLevel)
		{
#if !UNITY
			return Interlocked.Exchange( ref _default, CreateClassicContext( compatibilityLevel) );
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
		[Obsolete( "Use CreateClassicContext(SerializationCompatibilityLevel) instead." )]
		public static SerializationContext CreateClassicContext()
		{
			return CreateClassicContext( SerializationCompatibilityLevel.Version0_5 );
		}

		/// <summary>
		///		Creates a new <see cref="SerializationContext"/> which is configured as compatible for the specified version.
		/// </summary>
		/// <param name="compatibilityLevel">A <see cref="SerializationCompatibilityLevel"/> to specify compatibility level.</param>
		/// <returns>
		///		A new <see cref="SerializationContext"/> which is configured as compatible for the specified version.
		/// </returns>
		/// <remarks>
		///		<para>
		///			There are breaking changes of <see cref="SerializationContext"/> properties to improve API usability and to prevent accidental failure.
		///			This method returns a <see cref="SerializationContext"/> which configured as classic style settings as follows:
		///		</para>
		///		<list type="table">
		///			<listheader>
		///				<term></term>
		///				<description>Latest (as of 0.6)</description>
		///				<description>Version0_9 (as of 0.6)</description>
		///				<description>Version0_5 (formally, "classic", before 0.6)</description>
		///			</listheader>
		///			<item>
		///				<term><see cref="DateTime"/> and <see cref="DateTimeOffset"/> value</term>
		///				<description><see cref="Timestamp"/> value.</description>
		///				<description>Native representation (100-nano ticks, preserving <see cref="DateTimeKind"/>.)</description>
		///				<description>UTC, milliseconds Unix epoc.</description>
		///			</item>
		///			<item>
		///				<term>Usage of <c>ext</c> types</term>
		///				<description>Allowed</description>
		///				<description>Allowed</description>
		///				<description>Prohibited</description>
		///			</item>
		///			<item>
		///				<term>Binary (such as <c>Byte[]</c>) representation</term>
		///				<description>Bin types</description>
		///				<description>Bin types</description>
		///				<description>Raw types</description>
		///			</item>
		///			<item>
		///				<term>Strings which lengthes are between 17 to 255</term>
		///				<description>Str8 type</description>
		///				<description>Str8 types</description>
		///				<description>Raw16 type</description>
		///			</item>
		///		</list>
		///		<para>
		///			In short, <see cref="SerializationCompatibilityLevel.Version0_5"/> prohibits deserialization error in legacy implementation
		///			which do not recognize ext types, str8 type, and/or bin types.
		///			<see cref="SerializationCompatibilityLevel.Version0_9"/> prohibits only <see cref="Timestamp"/> serialization for datetime
		///			to keep compatibility for 0.9.x instead of maximize datetime serialization for modern implementations which uses msgpack timestamp type,
		///			which is composite ext type, nano-second precision Unix epoc time.
		///		</para>
		/// </remarks>
		public static SerializationContext CreateClassicContext( SerializationCompatibilityLevel compatibilityLevel )
		{
			switch ( compatibilityLevel )
			{
				case SerializationCompatibilityLevel.Version0_5:
				{
					return
						new SerializationContext( PackerCompatibilityOptions.Classic )
						{
							DefaultDateTimeConversionMethod = DateTimeConversionMethod.UnixEpoc
						};
				}
				case SerializationCompatibilityLevel.Version0_9:
				{
					return
						new SerializationContext( PackerCompatibilityOptions.None )
						{
							DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native
						};
				}
				case SerializationCompatibilityLevel.Latest:
				{
					return new SerializationContext( PackerCompatibilityOptions.None );
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "Unknown SerializationCompatibilityLevel value." );
				}
			}
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

#if !FEATURE_CONCURRENT
			this._typeLock = new Dictionary<Type, object>();
#else
			this._typeLock = new ConcurrentDictionary<Type, object>();
#endif // !FEATURE_CONCURRENT
			this._generationLock = new object();
			this._defaultCollectionTypes = new DefaultConcreteTypeRepository();
			this._serializerGeneratorOptions = new SerializerOptions();
			this._dictionarySerializationOptions = new DictionarySerializationOptions();
			this._enumSerializationOptions = new EnumSerializationOptions();
			this._bindingOptions = new BindingOptions();
		}

		internal bool ContainsSerializer( Type rootType )
		{
			return
				this._serializers.ContainsFor( rootType )
				|| ( rootType.GetIsGenericType() && this._serializers.ContainsFor( rootType.GetGenericTypeDefinition() ) );
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
#if DEBUG
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );
#endif // DEBUG

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
					try { }
					finally
					{
#if !FEATURE_CONCURRENT
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
#endif // !FEATURE_CONCURRENT
					}

					if ( lockTaken )
					{
						// First try to create generic serializer w/o code generation.
						var schema = ( providerParameter ?? PolymorphismSchema.Create( typeof( T ), null ) ) as PolymorphismSchema;
						serializer = GenericSerializer.Create<T>( this, schema );

						if ( serializer == null )
						{
#if !UNITY
							if ( !this._serializerGeneratorOptions.CanRuntimeCodeGeneration )
							{
#endif // !UNITY
								// On debugging, or AOT only envs, use reflection based aproach.
								serializer =
									this.GetSerializerWithoutGeneration<T>( schema )
									?? this.OnResolveSerializer<T>( schema )
									?? MessagePackSerializer.CreateReflectionInternal<T>( this, this.EnsureConcreteTypeRegistered( typeof( T ) ), schema );
#if !UNITY
							}
							else
							{
								// This thread creating new type serializer.
								serializer = this.OnResolveSerializer<T>( schema ) ?? MessagePackSerializer.CreateInternal<T>( this, schema );
							}
#endif // !UNITY
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
#if DEBUG
						Contract.Assert( typeof( T ).GetIsEnum(), typeof( T ) + " is not enum but generated serializer is ICustomizableEnumSerializer" );
#endif // DEBUG

						provider = new EnumMessagePackSerializerProvider( typeof( T ), asEnumSerializer );
					}
					else
					{
#if DEBUG
						Contract.Assert( !typeof( T ).GetIsEnum(), typeof( T ) + " is enum but generated serializer is not ICustomizableEnumSerializer : " + ( serializer == null ? "null" : serializer.GetType().FullName ) );
#endif // DEBUG

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
#if !FEATURE_CONCURRENT
					lock ( this._typeLock )
					{
						this._typeLock.Remove( typeof( T ) );
					}
#else
						object dummy;
						this._typeLock.TryRemove( typeof( T ), out dummy );
#endif // !FEATURE_CONCURRENT
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

#if DEBUG
						Contract.Assert(
							typedSerializer != null,
							serializer.GetType() + " : " + serializer.GetType().GetBaseType() + " is " + typeof( MessagePackSerializer<T> )
						);
#endif // DEBUG

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

			return ( MessagePackSerializer<T> )provider.Get( this, schema );
		}

		/// <summary>
		///		Gets the serializer for the specified <see cref="Type"/>.
		/// </summary>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		///		If the platform supports async/await programming model, return type is <c>IAsyncMessagePackSingleObjectSerializer</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		Although <see cref="GetSerializer{T}()"/> is preferred,
		///		this method can be used from non-generic type or methods.
		/// </remarks>
		public MessagePackSerializer GetSerializer( Type targetType )
		{
			return this.GetSerializer( targetType, null );
		}

		/// <summary>
		///		Gets the serializer for the specified <see cref="Type"/>.
		/// </summary>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <param name="providerParameter">A provider specific parameter. See remarks section of <see cref="GetSerializer{T}(Object)"/> for details.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		///		If the platform supports async/await programming model, return type is <c>IAsyncMessagePackSingleObjectSerializer</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		Although <see cref="GetSerializer{T}(Object)"/> is preferred,
		///		this method can be used from non-generic type or methods.
		/// </remarks>
		public MessagePackSerializer GetSerializer( Type targetType, object providerParameter )
		{
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

#if DEBUG
			Contract.Ensures( Contract.Result<MessagePackSerializer>() != null );
#endif // DEBUG

#if UNITY
			try
			{
#endif // UNITY
			return SerializerGetter.Instance.Get( this, targetType, providerParameter );
#if UNITY
			}
			catch ( Exception ex )
			{
				AotHelper.HandleAotError( targetType, ex );
				throw;
			}
#endif // UNITY
		}

		private sealed class SerializerGetter
		{
			public static readonly SerializerGetter Instance = new SerializerGetter();

#if FEATURE_CONCURRENT
			private readonly ConcurrentDictionary<RuntimeTypeHandle, Func<SerializationContext, object, MessagePackSerializer>> _cache =
				new ConcurrentDictionary<RuntimeTypeHandle, Func<SerializationContext, object, MessagePackSerializer>>();
#elif UNITY
			private readonly Dictionary<RuntimeTypeHandle, MethodInfo> _cache =
				new Dictionary<RuntimeTypeHandle, MethodInfo>();
#else
			private readonly Dictionary<RuntimeTypeHandle, Func<SerializationContext, object, MessagePackSerializer>> _cache =
				new Dictionary<RuntimeTypeHandle, Func<SerializationContext, object, MessagePackSerializer>>();
#endif // FEATURE_CONCURRENT

			private SerializerGetter() { }

			public MessagePackSerializer Get( SerializationContext context, Type targetType, object providerParameter )
			{
#if UNITY
				MethodInfo method;
				if ( !this._cache.TryGetValue( targetType.TypeHandle, out method ) || method == null )
				{
					method = GetSerializer1Method.MakeGenericMethod( targetType );
					this._cache[ targetType.TypeHandle ] = method;
				}

				return ( MessagePackSerializer )method.InvokePreservingExceptionType( context, providerParameter );
#else
				Func<SerializationContext, object, MessagePackSerializer> func;
#if !FEATURE_CONCURRENT
				lock ( this._cache )
				{
#endif // !FEATURE_CONCURRENT
				if ( !this._cache.TryGetValue( targetType.TypeHandle, out func ) || func == null )
				{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
					func =
						Delegate.CreateDelegate(
							typeof( Func<SerializationContext, object, MessagePackSerializer> ),
							typeof( SerializerGetter<> ).MakeGenericType( targetType ).GetMethod( "Get" )
						) as Func<SerializationContext, object, MessagePackSerializer>;
#else
					func =
						typeof( SerializerGetter<> ).MakeGenericType( targetType ).GetMethod( "Get" ).CreateDelegate(
							typeof( Func<SerializationContext, object, MessagePackSerializer> )
						) as Func<SerializationContext, object, MessagePackSerializer>;
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3

					Contract.Assert( func != null, "func != null" );

					this._cache[ targetType.TypeHandle ] = func;
				}
#if !FEATURE_CONCURRENT
				}
#endif // !FEATURE_CONCURRENT
				return func( context, providerParameter );
#endif // UNITY
			}
		}

#if !UNITY
		[Preserve( AllMembers = true )]
		private static class SerializerGetter<T>
		{
			private static readonly Func<SerializationContext, object, MessagePackSerializer<T>> _func =
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !WINDOWS_PHONE && !UNITY
				Delegate.CreateDelegate(
					typeof( Func<SerializationContext, object, MessagePackSerializer<T>> ),
					Metadata._SerializationContext.GetSerializer1_Parameter_Method.MakeGenericMethod( typeof( T ) )
				) as Func<SerializationContext, object, MessagePackSerializer<T>>;
#else
#if !UNITY && !WINDOWS_PHONE && !WINDOWS_UWP
				Metadata._SerializationContext.GetSerializer1_Parameter_Method
#else // !UNITY && !WINDOWS_PHONE && !WINDOWS_UWP
				GetSerializer1Method
#endif // !UNITY && !WINDOWS_PHONE && !WINDOWS_UWP
				.MakeGenericMethod( typeof( T ) ).CreateDelegate(
					typeof( Func<SerializationContext, object, MessagePackSerializer<T>> )
				) as Func<SerializationContext, object, MessagePackSerializer<T>>;
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !WINDOWS_PHONE && !UNITY

			// ReSharper disable UnusedMember.Local
			// This method is invoked via Reflection on SerializerGetter.Get().
			public static MessagePackSerializer Get( SerializationContext context, object providerParameter )
			{
				return _func( context, providerParameter );
			}
			// ReSharper restore UnusedMember.Local
		}
#endif // !UNITY && !UNITY
	}
}
