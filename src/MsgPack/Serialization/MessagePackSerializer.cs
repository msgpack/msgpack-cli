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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Linq;

using MsgPack.Serialization.ReflectionSerializers;
#if SILVERLIGHT || NETFX_35 || UNITY
using System.Collections.Generic;
#else
using System.Collections.Concurrent;
#endif // SILVERLIGHT || NETFX_35 || UNITY
#if !UNITY
using System.Diagnostics.Contracts;
#endif // !UNITY
#if !WINDOWS_PHONE && !NETFX_35 && !XAMIOS && !XAMDROID && !UNITY
using System.Globalization;
#endif // !WINDOWS_PHONE && !NETFX_35 && !XAMIOS && !XAMDROID && !UNITY
#if NETFX_CORE || WINDOWS_PHONE
using System.Linq.Expressions;
#endif
#if !XAMIOS && !XAMDROID && !UNITY
using MsgPack.Serialization.AbstractSerializers;
#if !NETFX_CORE && !WINDOWS_PHONE
#if !SILVERLIGHT
using MsgPack.Serialization.CodeDomSerializers;
#endif // !SILVERLIGHT
using MsgPack.Serialization.EmittingSerializers;
#endif // NETFX_CORE && !WINDOWS_PHONE
#endif // !!XAMIOS && !XAMDROID && !UNITY
#if !NETFX_35 && !XAMIOS && !XAMDROID && !UNITY
using MsgPack.Serialization.ExpressionSerializers;
#endif // !NETFX_35 && !XAMIOS && !XAMDROID && !UNITY

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
		[Obsolete( "Use Get<T>() instead." )]
		public static MessagePackSerializer<T> Create<T>()
		{
#if !UNITY
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );
#endif // !UNITY

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
		[Obsolete( "Use Get<T>(SerializationContext) instead." )]
		public static MessagePackSerializer<T> Create<T>( SerializationContext context )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			return CreateInternal<T>( context );
		}

		/// <summary>
		///		Gets existing or new <see cref="MessagePackSerializer{T}"/> instance with default context (<see cref="SerializationContext.Default"/>).
		/// </summary>
		/// <typeparam name="T">Target type.</typeparam>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <remarks>
		///		This method simply invokes <see cref="Get{T}(SerializationContext)"/> with <see cref="SerializationContext.Default"/> for the <c>context</c>.
		/// </remarks>
		public static MessagePackSerializer<T> Get<T>()
		{
			return Get<T>( SerializationContext.Default );
		}

		/// <summary>
		///		Gets existing or new <see cref="MessagePackSerializer{T}"/> instance with default context (<see cref="SerializationContext.Default"/>).
		/// </summary>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="providerParameter">A provider specific parameter. See remarks section for details.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <remarks>
		///		This method simply invokes <see cref="Get{T}(SerializationContext,Object)"/> with <see cref="SerializationContext.Default"/> for the <c>context</c>.
		/// </remarks>
		public static MessagePackSerializer<T> Get<T>( object providerParameter )
		{
			return Get<T>( SerializationContext.Default, providerParameter );
		}

		/// <summary>
		///		Gets existing or new <see cref="MessagePackSerializer{T}"/> instance with specified <see cref="SerializationContext"/>.
		/// </summary>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="context">
		///		<see cref="SerializationContext"/> to store known/created serializers.
		/// </param>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		This method simply invokes <see cref="Get{T}(SerializationContext,Object)"/> with <c>null</c> for the <c>providerParameter</c>.
		/// </remarks>
		public static MessagePackSerializer<T> Get<T>( SerializationContext context )
		{
			return Get<T>( context, null );
		}

		/// <summary>
		///		Gets existing or new <see cref="MessagePackSerializer{T}"/> instance with specified <see cref="SerializationContext"/>.
		/// </summary>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="context">
		///		<see cref="SerializationContext"/> to store known/created serializers.
		/// </param>
		/// <param name="providerParameter">A provider specific parameter. See remarks section for details.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		<para>
		///			This method simply invokes <see cref="SerializationContext.GetSerializer{T}(Object)"/>, so see the method description for details.
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
		public static MessagePackSerializer<T> Get<T>( SerializationContext context, object providerParameter )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			return context.GetSerializer<T>( providerParameter );
		}

		internal static MessagePackSerializer<T> CreateInternal<T>( SerializationContext context )
		{
#if !XAMIOS && !XAMDROID && !UNITY
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );
#endif // !XAMIOS && !XAMDROID && !UNITY
#if XAMIOS || XAMDROID || UNITY
			return CreateReflectionInternal<T>( context );
#else
			ISerializerBuilder<T> builder;
#if NETFX_CORE || WINDOWS_PHONE
			builder = new ExpressionTreeSerializerBuilder<T>();
#elif SILVERLIGHT
			builder = new DynamicMethodSerializerBuilder<T>();
#else
			switch ( context.EmitterFlavor )
			{
				case EmitterFlavor.ReflectionBased:
				{
					return CreateReflectionInternal<T>( context );
				}
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
#endif // XAMIOS || XAMDROID || UNITY else
		}

#if !XAMIOS && !XAMDROID && !UNITY
#if !SILVERLIGHT && !NETFX_35
		private static readonly ConcurrentDictionary<Type, Func<SerializationContext, IMessagePackSingleObjectSerializer>> _creatorCache = new ConcurrentDictionary<Type, Func<SerializationContext, IMessagePackSingleObjectSerializer>>();
#else
		private static readonly object _syncRoot = new object();
		private static readonly Dictionary<Type, Func<SerializationContext, IMessagePackSingleObjectSerializer>> _creatorCache = new Dictionary<Type, Func<SerializationContext, IMessagePackSingleObjectSerializer>>();
#endif // !SILVERLIGHT && !NETFX_35
#endif // !XAMIOS && !XAMDROID && !UNITY

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
		[Obsolete( "Use Get(Type) instead." )]
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
		[Obsolete( "Use Get(Type,SerializationContext) instead." )]
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

#if !UNITY
			Contract.Ensures( Contract.Result<IMessagePackSerializer>() != null );
#endif // !UNITY

#if XAMIOS || XAMDROID || UNITY
			return CreateReflectionInternal( context, targetType );
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
#endif // XAMIOS || XAMDROID || UNITY else
		}

		/// <summary>
		///		Gets existing or new <see cref="IMessagePackSerializer"/> instance with default context (<see cref="SerializationContext.Default"/>).
		/// </summary>
		/// <param name="targetType">Target type.</param>
		/// <returns>
		///		<see cref="IMessagePackSingleObjectSerializer"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		<para>
		///			This method simply invokes <see cref="SerializationContext.GetSerializer(Type)"/>, so see the method description for details.
		///		</para>
		///		<para>
		///		Although <see cref="Get{T}()"/> is preferred,
		///		this method can be used from non-generic type or methods.
		///		</para>
		/// </remarks>
		public static IMessagePackSingleObjectSerializer Get(
			Type targetType )
		{
			return Get( targetType, SerializationContext.Default, null );
		}

		/// <summary>
		///		Gets existing or new <see cref="IMessagePackSerializer"/> instance with default context (<see cref="SerializationContext.Default"/>).
		/// </summary>
		/// <param name="targetType">Target type.</param>
		/// <param name="providerParameter">A provider specific parameter. See remarks section for details.</param>
		/// <returns>
		///		<see cref="IMessagePackSingleObjectSerializer"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		<para>
		///			This method simply invokes <see cref="SerializationContext.GetSerializer(Type,Object)"/>, so see the method description for details.
		///		</para>
		///		<para>
		///		Although <see cref="Get{T}(Object)"/> is preferred,
		///		this method can be used from non-generic type or methods.
		///		</para>
		/// </remarks>
		public static IMessagePackSingleObjectSerializer Get(
			Type targetType,
			object providerParameter )
		{
			return Get( targetType, SerializationContext.Default, providerParameter );
		}

		/// <summary>
		///		Gets existing or new <see cref="IMessagePackSerializer"/> instance with specified <see cref="SerializationContext"/>.
		/// </summary>
		/// <param name="targetType">Target type.</param>
		/// <param name="context">
		///		<see cref="SerializationContext"/> to store known/created serializers.
		/// </param>
		/// <returns>
		///		<see cref="IMessagePackSingleObjectSerializer"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		<para>
		///			This method simply invokes <see cref="SerializationContext.GetSerializer(Type)"/>, so see the method description for details.
		///		</para>
		///		<para>
		///		Although <see cref="Get{T}(SerializationContext)"/> is preferred,
		///		this method can be used from non-generic type or methods.
		///		</para>
		/// </remarks>
		public static IMessagePackSingleObjectSerializer Get(
			Type targetType,
			SerializationContext context )
		{
			return Get( targetType, context, null );
		}

		/// <summary>
		///		Gets existing or new <see cref="IMessagePackSerializer"/> instance with specified <see cref="SerializationContext"/>.
		/// </summary>
		/// <param name="targetType">Target type.</param>
		/// <param name="context">
		///		<see cref="SerializationContext"/> to store known/created serializers.
		/// </param>
		/// <param name="providerParameter">A provider specific parameter. See remarks section for details.</param>
		/// <returns>
		///		<see cref="IMessagePackSingleObjectSerializer"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		<para>
		///			This method simply invokes <see cref="SerializationContext.GetSerializer(Type,Object)"/>, so see the method description for details.
		///		</para>
		///		<para>
		///		Although <see cref="Get{T}(SerializationContext,Object)"/> is preferred,
		///		this method can be used from non-generic type or methods.
		///		</para>
		/// </remarks>
		public static IMessagePackSingleObjectSerializer Get(
			Type targetType, SerializationContext context, object providerParameter )
		{
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			return context.GetSerializer( targetType, providerParameter );
		}

#if XAMIOS || XAMDROID || UNITY
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
#if UNITY_ANDROID || UNITY
			return
				(
					Delegate.CreateDelegate( 
						typeof( Func<SerializationContext, object> ),
						CreateReflectionInternal_1.MakeGenericMethod( targetType )
					)
				as Func<SerializationContext, object> )( context ) as IMessagePackSingleObjectSerializer;
#else
			return 
				( CreateReflectionInternal_1.MakeGenericMethod( targetType ).CreateDelegate( typeof( Func<SerializationContext,object> ) ) 
				as Func<SerializationContext, object> )( context ) as IMessagePackSingleObjectSerializer;
#endif
		}
#endif // XAMIOS || XAMDROID || UNITY

		internal static MessagePackSerializer<T> CreateReflectionInternal<T>( SerializationContext context )
		{
			var serializer = context.Serializers.Get<T>( context );

			if ( serializer != null )
			{
				// For MessagePack.Create compatibility. 
				// Required for built-in types.
				return serializer;
			}

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
#if !WINDOWS_PHONE && !NETFX_35 && !UNITY
					if ( ( typeof( T ).GetAssembly().Equals( typeof( object ).GetAssembly() ) ||
								typeof( T ).GetAssembly().Equals( typeof( Enumerable ).GetAssembly() ) )
							  && typeof( T ).GetIsPublic() &&
							  typeof( T ).Name.StartsWith( "Tuple`", StringComparison.Ordinal ) )
					{
						return new ReflectionTupleMessagePackSerializer<T>( context );
					}
#endif // !WINDOWS_PHONE && !NETFX_35 && !UNITY

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
