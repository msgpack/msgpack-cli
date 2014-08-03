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
using System.Linq;

using MsgPack.Serialization.ReflectionSerializers;
#if SILVERLIGHT || NETFX_35 || UNITY_ANDROID || UNITY_IPHONE
using System.Collections.Generic;
#else
using System.Collections.Concurrent;
#endif // SILVERLIGHT || NETFX_35 || UNITY_ANDROID || UNITY_IPHONE
#if !UNITY_ANDROID && !UNITY_IPHONE
using System.Diagnostics.Contracts;
#endif // !UNITY_ANDROID && !UNITY_IPHONE
#if !WINDOWS_PHONE && !NETFX_35 && !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
using System.Globalization;
#endif // !WINDOWS_PHONE && !NETFX_35 && !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
#if NETFX_CORE
using System.Linq.Expressions;
#endif
#if !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
using MsgPack.Serialization.AbstractSerializers;
#if !NETFX_CORE
#if !SILVERLIGHT
using MsgPack.Serialization.CodeDomSerializers;
#endif // !SILVERLIGHT
using MsgPack.Serialization.EmittingSerializers;
#endif // NETFX_CORE
#endif // !!XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
#if !WINDOWS_PHONE && !NETFX_35 && !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
using MsgPack.Serialization.ExpressionSerializers;
#endif // !WINDOWS_PHONE && !NETFX_35 && !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines entry points for <see cref="MessagePackSerializer{T}"/> usage.
	/// </summary>
	public static class MessagePackSerializer
	{
		/// <summary>
		///		Creates new <see cref="MessagePackSerializer{T}"/> instance with <see cref="SerializationContext.Default"/>.
		/// </summary>
		/// <typeparam name="T">Target type.</typeparam>
		/// <returns>
		///		New <see cref="MessagePackSerializer{T}"/> instance to serialize/deserialize the object tree which the top is <typeparamref name="T"/>.
		/// </returns>
		[Obsolete( "Use SerializationContext.Default.GetSerializer<T>() instead." )]
		public static MessagePackSerializer<T> Create<T>()
		{
#if !UNITY_ANDROID && !UNITY_IPHONE
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );
#endif // !UNITY_ANDROID && !UNITY_IPHONE

			return Create<T>( SerializationContext.Default );
		}

		/// <summary>
		///		Creates new <see cref="MessagePackSerializer{T}"/> instance with specified <see cref="SerializationContext"/>.
		/// </summary>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="context">
		///		<see cref="SerializationContext"/> to store known/created serializers.
		/// </param>
		/// <returns>
		///		New <see cref="MessagePackSerializer{T}"/> instance to serialize/deserialize the object tree which the top is <typeparamref name="T"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="context"/> is <c>null</c>.
		/// </exception>
		[Obsolete( "Use SerializationContext.GetSerializer<T>() instead." )]
		public static MessagePackSerializer<T> Create<T>( SerializationContext context )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			return CreateInternal<T>( context );
		}

		internal static MessagePackSerializer<T> CreateInternal<T>( SerializationContext context )
		{
#if XAMIOS || XAMDROID || UNITY_IPHONE || UNITY_ANDROID
			return context.GetSerializer<T>();
#else

			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );

			//Func<SerializationContext, SerializerBuilder<T>> builderProvider;
			ISerializerBuilder<T> builder;
#if NETFX_CORE
			builder = new ExpressionTreeSerializerBuilder<T>();
#elif SILVERLIGHT
			builder = new DynamicMethodSerializerBuilder<T>();
#else
			switch ( context.EmitterFlavor )
			{
#if !NETFX_35 && !NETFX_40 && !NETFX_CORE && !WINDOWS_PHONE && !SILVERLIGHT
				case EmitterFlavor.ReflectionBased:
				{
					return CreateReflectionInternal<T>( context );
				}
#endif // !NETFX_35 && !NETFX_40 && !NETFX_CORE && !WINDOWS_PHONE && !SILVERLIGHT
#if !WINDOWS_PHONE && !NETFX_35
				case EmitterFlavor.ExpressionBased:
				{
					builder = new ExpressionTreeSerializerBuilder<T>();
					break;
				}
#endif // if !WINDOWS_PHONE && !NETFX_35
				case EmitterFlavor.FieldBased:
				{
					builder = new AssemblyBuilderSerializerBuilder<T>();
					break;
				}
				case EmitterFlavor.ContextBased:
				{
					builder = new DynamicMethodSerializerBuilder<T>();
					break;
				}
				default:
				{
#if !NETFX_35
					if ( !SerializerDebugging.OnTheFlyCodeDomEnabled )
					{
						throw new NotSupportedException(
							String.Format(
								CultureInfo.CurrentCulture,
								"Flavor '{0:G}'({0:D}) is not supported for serializer instance creation.",
								context.EmitterFlavor
							)
						);
					}
#endif // if !NETFX_35
					builder = new CodeDomSerializerBuilder<T>();
					break;
				}
			}
#endif // NETFX_CORE else

			return new AutoMessagePackSerializer<T>( context, builder );
#endif // XAMIOS || XAMDROID || UNITY_IPHONE || UNITY_ANDROID else
		}

#if !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE
#if !SILVERLIGHT && !NETFX_35
		private static readonly ConcurrentDictionary<Type, Func<SerializationContext, IMessagePackSingleObjectSerializer>> _creatorCache = new ConcurrentDictionary<Type, Func<SerializationContext, IMessagePackSingleObjectSerializer>>();
#else
		private static readonly object _syncRoot = new object();
		private static readonly Dictionary<Type, Func<SerializationContext, IMessagePackSingleObjectSerializer>> _creatorCache = new Dictionary<Type, Func<SerializationContext, IMessagePackSingleObjectSerializer>>();
#endif // !SILVERLIGHT && !NETFX_35
#endif // !XAMIOS && !XAMDROID && !UNITY_ANDROID && !UNITY_IPHONE

		/// <summary>
		///		Creates new <see cref="IMessagePackSerializer"/> instance with <see cref="SerializationContext.Default"/>.
		/// </summary>
		/// <param name="targetType">Target type.</param>
		/// <returns>
		///		New <see cref="IMessagePackSingleObjectSerializer"/> instance to serialize/deserialize the object tree which the top is <paramref name="targetType"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		To avoid boxing and strongly typed API is prefered, use <see cref="Create{T}()"/> instead when possible.
		/// </remarks>
		[Obsolete( "Use SerializationContext.Default.GetSerializer(Type) instead." )]
		public static IMessagePackSingleObjectSerializer Create( Type targetType )
		{
			return Create( targetType, SerializationContext.Default );
		}

		/// <summary>
		///		Creates new <see cref="IMessagePackSerializer"/> instance with specified <see cref="SerializationContext"/>.
		/// </summary>
		/// <param name="targetType">Target type.</param>
		/// <param name="context">
		///		<see cref="SerializationContext"/> to store known/created serializers.
		/// </param>
		/// <returns>
		///		New <see cref="IMessagePackSingleObjectSerializer"/> instance to serialize/deserialize the object tree which the top is <paramref name="targetType"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		To avoid boxing and strongly typed API is prefered, use <see cref="Create{T}(SerializationContext)"/> instead when possible.
		/// </remarks>
		[Obsolete( "Use SerializationContext.GetSerializer(Type) instead." )]
		public static IMessagePackSingleObjectSerializer Create( Type targetType, SerializationContext context )
		{
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

#if !UNITY_ANDROID && !UNITY_IPHONE
			Contract.Ensures( Contract.Result<IMessagePackSerializer>() != null );
#endif // !UNITY_ANDROID && !UNITY_IPHONE

#if XAMIOS || XAMDROID || UNITY_ANDROID || UNITY_IPHONE
			return context.GetSerializer( targetType );
#else
			// MPS.Create should always return new instance, and creator delegate should be cached for performance.
#if NETFX_CORE
			var factory =
				_creatorCache.GetOrAdd(
					targetType,
					type =>
					{
						var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
						// Utilize covariance of delegate.
						return
							Expression.Lambda<Func<SerializationContext, IMessagePackSingleObjectSerializer>>(
								Expression.Call(
									null,
									Metadata._MessagePackSerializer.Create1_Method.MakeGenericMethod( type ),
									contextParameter
								),
								contextParameter
							).Compile();
					}
				);
#elif SILVERLIGHT || NETFX_35
			Func<SerializationContext, IMessagePackSingleObjectSerializer> factory;

			lock ( _syncRoot )
			{
				_creatorCache.TryGetValue( targetType, out factory );
			}

			if ( factory == null )
			{
				// Utilize covariance of delegate.
				factory =
					Delegate.CreateDelegate(
						typeof( Func<SerializationContext, IMessagePackSingleObjectSerializer> ),
						Metadata._MessagePackSerializer.Create1_Method.MakeGenericMethod( targetType )
						) as Func<SerializationContext, IMessagePackSingleObjectSerializer>;

				Contract.Assert( factory != null );

				lock ( _syncRoot )
				{
					_creatorCache[ targetType ] = factory;
				}
			}
#else
			var factory =
				_creatorCache.GetOrAdd(
					targetType,
					type =>
						// Utilize covariance of delegate.
						Delegate.CreateDelegate(
							typeof( Func<SerializationContext, IMessagePackSingleObjectSerializer> ),
							Metadata._MessagePackSerializer.Create1_Method.MakeGenericMethod( type )
						) as Func<SerializationContext, IMessagePackSingleObjectSerializer>
				);
#endif // NETFX_CORE
			return factory( context );
#endif // XAMIOS || XAMDROID || UNITY_ANDROID || UNITY_IPHONE else
		}

#if XAMIOS || XAMDROID || UNITY_ANDROID || UNITY_IPHONE
		private static readonly System.Reflection.MethodInfo CreateReflectionInternal_1 = 
			typeof( MessagePackSerializer ).GetMethod( 
				"CreateReflectionInternal", 
				System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic,
				null,
				new []{ typeof( SerializationContext ) },
				null
			);

		internal static IMessagePackSingleObjectSerializer CreateReflectionInternal( SerializationContext context, Type targetType )
		{
			return CreateReflectionInternal_1.MakeGenericMethod( targetType ).Invoke( null, new object[]{ context } ) as IMessagePackSingleObjectSerializer;
		}
#endif // XAMIOS || XAMDROID || UNITY_ANDROID || UNITY_IPHONE

		internal static MessagePackSerializer<T> CreateReflectionInternal<T>( SerializationContext context )
		{
			var traits = typeof( T ).GetCollectionTraits();
			switch ( traits.CollectionType )
			{
				case CollectionKind.Array:
				{
					return ReflectionSerializerHelper.CreateArraySerializer<T>( context, EnsureConcreteTypeRegistered( context, typeof( T ) ), traits );
				}
				case CollectionKind.Map:
				{
					return ReflectionSerializerHelper.CreateMapSerializer<T>( context, EnsureConcreteTypeRegistered( context, typeof( T ) ), traits );
				}
				default:
				{
					if ( typeof( T ).GetIsEnum() )
					{
						return ReflectionSerializerHelper.CreateReflectionEnuMessagePackSerializer<T>( context );
					}
#if !WINDOWS_PHONE && !NETFX_35
					if ( ( typeof( T ).GetAssembly().Equals( typeof( object ).GetAssembly() ) ||
								typeof( T ).GetAssembly().Equals( typeof( Enumerable ).GetAssembly() ) )
							  && typeof( T ).GetIsPublic() &&
							  typeof( T ).Name.StartsWith( "Tuple`", StringComparison.Ordinal ) )
					{
						return new ReflectionTupleMessagePackSerializer<T>( context );
					}
#endif // !WINDOWS_PHONE && !NETFX_35

					return new ReflectionObjectMessagePackSerializer<T>( context );
				}
			}
		}

		private static Type EnsureConcreteTypeRegistered( SerializationContext context, Type mayBeAbstractType )
		{
			if ( !mayBeAbstractType.GetIsAbstract() && !mayBeAbstractType.GetIsInterface() )
			{
				return mayBeAbstractType;
			}

			var concreteType = context.DefaultCollectionTypes.GetConcreteType( mayBeAbstractType );
			if ( concreteType == null )
			{
				throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( mayBeAbstractType );
			}

			return concreteType;
		}
	}
}
