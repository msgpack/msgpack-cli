#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
#if !SILVERLIGHT && !NETFX_35
using System.Collections.Concurrent;
#endif
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
#if NETFX_CORE
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
#endif

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Represents serialization context information for internal serialization logic.
	/// </summary>
	public sealed class SerializationContext
	{
		private static SerializationContext _default = new SerializationContext();

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
#if SILVERLIGHT || NETFX_35
		private readonly Dictionary<Type, object> _typeLock;
#else
		private readonly ConcurrentDictionary<Type, object> _typeLock;
#endif

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
				Contract.Ensures( Contract.Result<SerializerRepository>() != null );

				return this._serializers;
			}
		}

		private EmitterFlavor _emitterFlavor = EmitterFlavor.FieldBased;

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
				Contract.Ensures( Contract.Result<SerializationCompatibilityOptions>() != null );

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
				Contract.Ensures( Enum.IsDefined( typeof( SerializationMethod ), Contract.Result<SerializationMethod>() ) );

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

				Contract.EndContractBlock();

				this._serializationMethod = value;
			}
		}

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
				Contract.Ensures( Enum.IsDefined( typeof( SerializationMethod ), Contract.Result<SerializationMethodGeneratorOption>() ) );

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

				Contract.EndContractBlock();

				this._generatorOption = value;
			}
		}

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
			Contract.Requires( serializers != null );

			this._compatibilityOptions =
				new SerializationCompatibilityOptions
				{
					PackerCompatibilityOptions =
						packerCompatibilityOptions
				};
			this._serializers = serializers;
#if SILVERLIGHT || NETFX_35
			this._typeLock = new Dictionary<Type, object>();
#else
			this._typeLock = new ConcurrentDictionary<Type, object>();
#endif
			this._defaultCollectionTypes = new DefaultConcreteTypeRepository();
		}

		internal bool ContainsSerializer( Type rootType )
		{
			return this._serializers.Contains( rootType );
		}

		/// <summary>
		///		Gets the <see cref="MessagePackSerializer{T}"/> with this instance.
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
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );

			MessagePackSerializer<T> serializer = null;
			while ( serializer == null )
			{
				serializer = this._serializers.Get<T>( this );
				if ( serializer == null )
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
#endif
							}
							finally
							{
#if SILVERLIGHT || NETFX_35
								if ( !lockTaken )
#else
								if ( !lockTaken && newLockTaken )
#endif
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
								serializer = MessagePackSerializer.Create<T>( this );
							}
							else
							{
								// This thread owns existing lock -- thus, constructing self-composite type.

								// Prevent release owned lock.
								aquiredLock = null;
								return new LazyDelegatingMessagePackSerializer<T>( this );
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
#endif
						}

						if ( aquiredLock != null )
						{
							// Release primary lock or waiting lock.
							Monitor.Exit( aquiredLock );
						}
					}
				}
			}

			if ( !this._serializers.Register( serializer ) )
			{
				serializer = this._serializers.Get<T>( this );
			}

			return serializer;
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
		///		Although <see cref="GetSerializer{T}"/> is preferred,
		///		this method can be used from non-generic type or methods.
		/// </remarks>
		public IMessagePackSingleObjectSerializer GetSerializer( Type targetType )
		{
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

			Contract.Ensures( Contract.Result<IMessagePackSerializer>() != null );

			return SerializerGetter.Instance.Get( this, targetType );
		}

		private sealed class SerializerGetter
		{
			public static readonly SerializerGetter Instance = new SerializerGetter();

			private readonly Dictionary<RuntimeTypeHandle, Func<SerializationContext, IMessagePackSingleObjectSerializer>> _cache =
				new Dictionary<RuntimeTypeHandle, Func<SerializationContext, IMessagePackSingleObjectSerializer>>();

			private SerializerGetter() { }

			public IMessagePackSingleObjectSerializer Get( SerializationContext context, Type targetType )
			{
				Func<SerializationContext, IMessagePackSingleObjectSerializer> func;
				if ( !this._cache.TryGetValue( targetType.TypeHandle, out func ) || func == null )
				{
#if !NETFX_CORE
					func =
						Delegate.CreateDelegate(
							typeof( Func<SerializationContext, IMessagePackSingleObjectSerializer> ),
							typeof( SerializerGetter<> ).MakeGenericType( targetType ).GetMethod( "Get" )
						) as Func<SerializationContext, IMessagePackSingleObjectSerializer>;
#else
					var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
					func =
						Expression.Lambda<Func<SerializationContext, IMessagePackSingleObjectSerializer>>(
							Expression.Call(
								null,
								typeof( SerializerGetter<> ).MakeGenericType( targetType ).GetRuntimeMethods().Single( m => m.Name == "Get" ),
								contextParameter
							),
							contextParameter
						).Compile();
#endif
#if DEBUG
					Contract.Assert( func != null );
#endif
					this._cache[ targetType.TypeHandle ] = func;
				}

				return func( context );
			}
		}

		private static class SerializerGetter<T>
		{
#if !NETFX_CORE
			private static readonly Func<SerializationContext, MessagePackSerializer<T>> _func =
				Delegate.CreateDelegate(
					typeof( Func<SerializationContext, MessagePackSerializer<T>> ),
					Metadata._SerializationContext.GetSerializer1_Method.MakeGenericMethod( typeof( T ) )
				) as Func<SerializationContext, MessagePackSerializer<T>>;
#else
			private static readonly Func<SerializationContext, MessagePackSerializer<T>> _func =
				CreateFunc();

			private static Func<SerializationContext, MessagePackSerializer<T>> CreateFunc()
			{
				var thisParameter = Expression.Parameter( typeof( SerializationContext ), "this" );
				return
					Expression.Lambda<Func<SerializationContext, MessagePackSerializer<T>>>(
						Expression.Call(
							thisParameter,
							Metadata._SerializationContext.GetSerializer1_Method.MakeGenericMethod( typeof( T ) )
						),
						thisParameter
					).Compile();
			}
#endif

// ReSharper disable UnusedMember.Local
			// This method is invoked via Reflection on SerializerGetter.Get().
			public static IMessagePackSingleObjectSerializer Get( SerializationContext context )
			{
				return _func( context );
			}
			// ReSharper restore UnusedMember.Local
		}
	}
}
