#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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
#if !SILVERLIGHT && !NETFX_35 && !UNITY_ANDROID && !UNITY_IPHONE
using System.Collections.Concurrent;
#endif // !SILVERLIGHT && !NETFX_35 && !UNITY_ANDROID && !UNITY_IPHONE
using System.Collections.Generic;
#if !UNITY_ANDROID && !UNITY_IPHONE
using System.Diagnostics.Contracts;
#endif // !UNITY_ANDROID && !UNITY_IPHONE
using System.Globalization;
#if NETFX_CORE
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
#endif // NETFX_CORE
using System.Threading;

using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Represents serialization context information for internal serialization logic.
	/// </summary>
	public sealed class SerializationContext
	{
		// Set SerializerRepository null because it requires SerializationContext, so re-init in constructor.
		private static SerializationContext _default = new SerializationContext( default( SerializerRepository ) );

		/// <summary>
		///		Gets or sets the default instance.
		/// </summary>
		/// <value>
		///		The default <see cref="SerializationContext"/> instance.
		/// </value>
		/// <exception cref="ArgumentNullException">The setting value is <c>null</c>.</exception>
		public static SerializationContext Default
		{
			get { return Interlocked.CompareExchange( ref  _default, null, null ); }
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException( "value" );
				}

				Interlocked.Exchange( ref _default, value );
			}
		}

		private readonly SerializerRepository _serializers;
#if !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
#if SILVERLIGHT || NETFX_35
		private readonly Dictionary<Type, object> _typeLock;
#else
		private readonly ConcurrentDictionary<Type, object> _typeLock;
#endif // SILVERLIGHT || NETFX_35
#endif // XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE

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
#if !UNITY_ANDROID && !UNITY_IPHONE
				Contract.Ensures( Contract.Result<SerializerRepository>() != null );
#endif // !UNITY_ANDROID && !UNITY_IPHONE

				return this._serializers;
			}
		}

#if !XAMIOS && !UNITY_IPHONE
#if !NETFX_CORE
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
#endif // !XAMIOS && !UNITY_IPHONE

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
#if !UNITY_ANDROID && !UNITY_IPHONE
				Contract.Ensures( Contract.Result<SerializationCompatibilityOptions>() != null );
#endif // !UNITY_ANDROID && !UNITY_IPHONE

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
#if !UNITY_ANDROID && !UNITY_IPHONE
				Contract.Ensures( Enum.IsDefined( typeof( SerializationMethod ), Contract.Result<SerializationMethod>() ) );
#endif // !UNITY_ANDROID && !UNITY_IPHONE

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

#if !UNITY_ANDROID && !UNITY_IPHONE
				Contract.EndContractBlock();
#endif // !UNITY_ANDROID && !UNITY_IPHONE


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
#if !UNITY_ANDROID && !UNITY_IPHONE
				Contract.Ensures( Enum.IsDefined( typeof( EnumSerializationMethod ), Contract.Result<EnumSerializationMethod>() ) );
#endif // !UNITY_ANDROID && !UNITY_IPHONE

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

#if !UNITY_ANDROID && !UNITY_IPHONE
				Contract.EndContractBlock();
#endif // !UNITY_ANDROID && !UNITY_IPHONE


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
#if !UNITY_ANDROID && !UNITY_IPHONE
				Contract.Ensures( Enum.IsDefined( typeof( SerializationMethod ), Contract.Result<SerializationMethodGeneratorOption>() ) );
#endif // !UNITY_ANDROID && !UNITY_IPHONE

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

#if !UNITY_ANDROID && !UNITY_IPHONE
				Contract.EndContractBlock();
#endif // !UNITY_ANDROID && !UNITY_IPHONE


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
		///		Initializes a new instance of the <see cref="SerializationContext"/> class with copy of <see cref="SerializerRepository.Default"/>.
		/// </summary>
		public SerializationContext()
			: this( new SerializerRepository( SerializerRepository.Default ), PackerCompatibilityOptions.Classic ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializationContext"/> class with copy of <see cref="SerializerRepository.GetDefault(PackerCompatibilityOptions)"/> for specified <see cref="PackerCompatibilityOptions"/>.
		/// </summary>
		/// <param name="packerCompatibilityOptions"><see cref="PackerCompatibilityOptions"/> which will be used on built-in serializers.</param>
		public SerializationContext( PackerCompatibilityOptions packerCompatibilityOptions )
			: this( new SerializerRepository( SerializerRepository.GetDefault( packerCompatibilityOptions ) ), packerCompatibilityOptions ) { }

		internal SerializationContext(
			SerializerRepository serializers, PackerCompatibilityOptions packerCompatibilityOptions )
		{
			this._compatibilityOptions =
				new SerializationCompatibilityOptions
				{
					PackerCompatibilityOptions =
						packerCompatibilityOptions
				};
			this._serializers = serializers;
#if !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
#if SILVERLIGHT || NETFX_35
			this._typeLock = new Dictionary<Type, object>();
#else
			this._typeLock = new ConcurrentDictionary<Type, object>();
#endif // SILVERLIGHT || NETFX_35
#endif // !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
			this._defaultCollectionTypes = new DefaultConcreteTypeRepository();
		}

		// For default init.
		private SerializationContext( SerializerRepository allwaysNull )
			: this( allwaysNull, PackerCompatibilityOptions.Classic ) // TODO: configurable
		{
			this._serializers = new SerializerRepository( SerializerRepository.GetDefault( this ) );
		}

		internal bool ContainsSerializer( Type rootType )
		{
			return this._serializers.Contains( rootType );
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
			return GetSerializer<T>( null );
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
#if !UNITY_ANDROID && !UNITY_IPHONE
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );
#endif // !UNITY_ANDROID && !UNITY_IPHONE

			MessagePackSerializer<T> serializer = null;
			while ( serializer == null )
			{
				serializer = this._serializers.Get<T>( this, providerParameter ) ?? GenericSerializer.Create<T>( this );
				if ( serializer == null )
				{
#if !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
					if ( this.IsRuntimeGenerationDisabled )
					{
#endif // !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
						return this.GetSerializerWithoutGeneration( typeof( T ) ) as MessagePackSerializer<T>;
#if !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
					}
					// ReSharper disable once RedundantIfElseBlock
					else
					{
						object aquiredLock = null;
						bool lockTaken = false;
						try
						{
							try { }
							finally
							{
								var newLock = new object();
#if SILVERLIGHT || NETFX_35
								Monitor.Enter( newLock );
								try
								{
									lock( this._typeLock )
									{
										lockTaken = !this._typeLock.TryGetValue( typeof( T ), out aquiredLock );
										if ( lockTaken )
										{
											aquiredLock = newLock;
											this._typeLock.Add( typeof( T ), newLock );
										}
									}
#else
								bool newLockTaken = false;
								try
								{
									Monitor.Enter( newLock, ref newLockTaken );
									aquiredLock = this._typeLock.GetOrAdd( typeof( T ), _ => newLock );
									lockTaken = newLock == aquiredLock;
#endif // if  SILVERLIGHT || NETFX_35
								}
								finally
								{
#if SILVERLIGHT || NETFX_35
									if ( !lockTaken )
#else
									if ( !lockTaken && newLockTaken )
#endif // if SILVERLIGHT || NETFX_35
									{
										// Release the lock which failed to become 'primary' lock.
										Monitor.Exit( newLock );
									}
								}
							}

							if ( Monitor.TryEnter( aquiredLock ) )
							{
								// Decrement monitor counter.
								Monitor.Exit( aquiredLock );

								if ( lockTaken )
								{
									// This thread creating new type serializer.
									serializer = MessagePackSerializer.CreateInternal<T>( this );
								}
								else
								{
									// This thread owns existing lock -- thus, constructing self-composite type.

									// Prevent release owned lock.
									aquiredLock = null;
									return new LazyDelegatingMessagePackSerializer<T>( this, providerParameter );
								}
							}
							else
							{
								// Wait creation by other thread.
								// Acquire as 'waiting' lock.
								Monitor.Enter( aquiredLock );
							}
						}
						finally
						{
							if ( lockTaken )
							{
#if SILVERLIGHT || NETFX_35
								lock( this._typeLock )
								{
									this._typeLock.Remove( typeof( T ) );
								}
#else
								object dummy;
								this._typeLock.TryRemove( typeof( T ), out dummy );
#endif // if SILVERLIGHT || NETFX_35
							}

							if ( aquiredLock != null )
							{
								// Release primary lock or waiting lock.
								Monitor.Exit( aquiredLock );
							}
						}
					}
#endif // !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
				}
			}

#if !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
			if ( !this._serializers.Register( serializer ) )
			{
				serializer = this._serializers.Get<T>( this, providerParameter );
			}
#endif // !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE

			return serializer;
		}

		private IMessagePackSingleObjectSerializer GetSerializerWithoutGeneration( Type targetType )
		{
			if ( targetType.GetIsInterface() || targetType.GetIsAbstract() )
			{
				var concreteCollectionType = this._defaultCollectionTypes.GetConcreteType( targetType );
				if ( concreteCollectionType != null )
				{
					var serializer =
						GenericSerializer.CreateCollectionInterfaceSerializer( this, targetType, concreteCollectionType );

					if ( serializer != null )
					{
						this.Serializers.Register( targetType, serializer );
						return serializer;
					}
				}
			}

			throw new InvalidOperationException(
				String.Format(
					CultureInfo.CurrentCulture,
					"The serializer for type '{0}' is not registered yet. On-the-fly generation is not supported in this platform.",
					targetType
				)
			);
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

#if !UNITY_ANDROID && !UNITY_IPHONE
			Contract.Ensures( Contract.Result<IMessagePackSerializer>() != null );
#endif // !UNITY_ANDROID && !UNITY_IPHONE

#if !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
			return SerializerGetter.Instance.Get( this, targetType, providerParameter );
#else
			var serializer = this._serializers.Get( this, targetType, providerParameter ) ?? GenericSerializer.Create( this, targetType );
			if ( serializer != null )
			{
				return serializer;
			}
			
			return this.GetSerializerWithoutGeneration( targetType );
#endif // !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
		}

#if !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
		private sealed class SerializerGetter
		{
			public static readonly SerializerGetter Instance = new SerializerGetter();

#if !SILVERLIGHT && !NETFX_35
			private readonly ConcurrentDictionary<RuntimeTypeHandle, Func<SerializationContext, object, IMessagePackSingleObjectSerializer>> _cache =
				new ConcurrentDictionary<RuntimeTypeHandle, Func<SerializationContext, object, IMessagePackSingleObjectSerializer>>();
#else
			private readonly Dictionary<RuntimeTypeHandle, Func<SerializationContext, object, IMessagePackSingleObjectSerializer>> _cache =
				new Dictionary<RuntimeTypeHandle, Func<SerializationContext, object, IMessagePackSingleObjectSerializer>>();
#endif

			private SerializerGetter() { }

			public IMessagePackSingleObjectSerializer Get( SerializationContext context, Type targetType, object providerParameter )
			{
				Func<SerializationContext, object, IMessagePackSingleObjectSerializer> func;
#if SILVERLIGHT || NETFX_35
				lock ( this._cache)
				{
#endif
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
#endif // if !NETFX_CORE
#if DEBUG
					Contract.Assert( func != null );
#endif // if DEBUG
					this._cache[ targetType.TypeHandle ] = func;
				}
#if SILVERLIGHT || NETFX_35
				}
#endif
				return func( context, providerParameter );
			}
		}

		private static class SerializerGetter<T>
		{
#if !NETFX_CORE
			private static readonly Func<SerializationContext, object, MessagePackSerializer<T>> _func =
				Delegate.CreateDelegate(
					typeof( Func<SerializationContext, object, MessagePackSerializer<T>> ),
					Metadata._SerializationContext.GetSerializer1_Parameter_Method.MakeGenericMethod( typeof( T ) )
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
#endif // !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
	}
}
