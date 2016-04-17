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
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;

using MsgPack.Serialization.ReflectionSerializers;
#if !SILVERLIGHT && !NETFX_35 && !UNITY
using System.Collections.Concurrent;
#else // !SILVERLIGHT && !NETFX_35 && !UNITY
using System.Collections.Generic;
#endif // !SILVERLIGHT && !NETFX_35 && !UNITY
#if !UNITY
using System.Diagnostics.Contracts;
#endif // !UNITY
#if NETFX_CORE || WINDOWS_PHONE
using System.Linq.Expressions;
#endif
#if !AOT
using MsgPack.Serialization.AbstractSerializers;
#if !SILVERLIGHT
#if !NETSTD_11 && !NETSTD_13
using MsgPack.Serialization.CodeDomSerializers;
#endif // !NETSTD_11 && !NETSTD_13
using MsgPack.Serialization.EmittingSerializers;
#endif // !SILVERLIGHT
#endif // !AOT

namespace MsgPack.Serialization
{
	partial class MessagePackSerializer
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

			// Old Create behavior was effectively Get() because the Builder internally register genreated serializer and returned existent one if it had been already registered. 
			// It was just aweful resource consumption.
			return Get<T>( context, null );
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

		internal static MessagePackSerializer<T> CreateInternal<T>( SerializationContext context, PolymorphismSchema schema )
		{

#if !AOT
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );
#endif // !AOT

#if DEBUG && !AOT && !SILVERLIGHT
			SerializerDebugging.TraceEvent(
				"SerializationContext::CreateInternal<{0}>(@{1}, {2})",
				typeof( T ),
				context.GetHashCode(),
				schema == null ? "null" : schema.DebugString
			);

#endif // DEBUG && !AOT && !SILVERLIGHT
			Type concreteType = null;
			CollectionTraits collectionTraits = typeof( T ).GetCollectionTraits();

			if ( typeof( T ).GetIsAbstract() || typeof( T ).GetIsInterface() )
			{
				// Abstract collection types will be handled correctly.
				if ( collectionTraits.CollectionType != CollectionKind.NotCollection )
				{
					concreteType = context.DefaultCollectionTypes.GetConcreteType( typeof( T ) );
				}

				if ( concreteType == null )
				{
					// return null for polymoirphic provider.
					return null;
				}

				ValidateType( concreteType );
			}
			else
			{
				ValidateType( typeof( T ) );
			}

#if !AOT
#if !SILVERLIGHT
			ISerializerBuilder builder;
#endif // !SILVERLIGHT
			switch ( context.SerializerOptions.EmitterFlavor )
			{
#if !SILVERLIGHT
#if !NETSTD_11 && !NETSTD_13
				case EmitterFlavor.CodeDomBased:
				{
					if ( !SerializerDebugging.OnTheFlyCodeDomEnabled )
					{
						throw new NotSupportedException(
							String.Format(
								CultureInfo.CurrentCulture,
								"Flavor '{0:G}'({0:D}) is not supported for serializer instance creation.",
								context.SerializerOptions.EmitterFlavor
							)
						);
					}

					builder = new CodeDomSerializerBuilder( typeof( T ), collectionTraits );
					break;
				}
#endif // !!NETSTD_11 && !NETSTD_13
				case EmitterFlavor.FieldBased:
				{
					builder = new AssemblyBuilderSerializerBuilder( typeof( T ), collectionTraits );
					break;
				}
#endif // !SILVERLIGHT
				default: // EmitterFlavor.ReflectionBased
				{
#endif // !AOT
					return
						DefaultSerializers.GenericSerializer.TryCreateAbstractCollectionSerializer( context, typeof( T ), concreteType, schema ) as MessagePackSerializer<T>
						?? CreateReflectionInternal<T>( context, concreteType ?? typeof( T ), schema );
#if !AOT
				}
			}
#endif // !AOT

#if !AOT && !SILVERLIGHT
			return ( MessagePackSerializer<T> ) builder.BuildSerializerInstance( context, concreteType, schema == null ? null : schema.FilterSelf() );
#endif // !AOT
		}

#if !XAMIOS && !XAMDROID && !UNITY
#if !SILVERLIGHT && !NETFX_35
		private static readonly ConcurrentDictionary<Type, Func<SerializationContext, MessagePackSerializer>> _creatorCache = new ConcurrentDictionary<Type, Func<SerializationContext, MessagePackSerializer>>();
#else
		private static readonly object _syncRoot = new object();
		private static readonly Dictionary<Type, Func<SerializationContext, MessagePackSerializer>> _creatorCache = new Dictionary<Type, Func<SerializationContext, MessagePackSerializer>>();
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
		public static MessagePackSerializer Create( Type targetType, SerializationContext context )
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
			Contract.Ensures( Contract.Result<MessagePackSerializer>() != null );
#endif // !UNITY

#if AOT
			return CreateInternal( context, targetType, null );
#else
			// MPS.Create should always return new instance, and creator delegate should be cached for performance.
#if NETSTD_11 || NETSTD_13
			var factory =
				_creatorCache.GetOrAdd(
					targetType,
					type =>
						// Utilize covariance of delegate.
						Metadata._MessagePackSerializer.Create1_Method.MakeGenericMethod( type ).CreateDelegate(
							typeof( Func<SerializationContext, MessagePackSerializer> )
						) as Func<SerializationContext, MessagePackSerializer>
				);
#elif SILVERLIGHT || NETFX_35
			Func<SerializationContext, MessagePackSerializer> factory;

			lock ( _syncRoot )
			{
				_creatorCache.TryGetValue( targetType, out factory );
			}

			if ( factory == null )
			{
				// Utilize covariance of delegate.
				factory =
					Delegate.CreateDelegate(
						typeof( Func<SerializationContext, MessagePackSerializer> ),
						Metadata._MessagePackSerializer.Create1_Method.MakeGenericMethod( targetType )
						) as Func<SerializationContext, MessagePackSerializer>;

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
							typeof( Func<SerializationContext, MessagePackSerializer> ),
							Metadata._MessagePackSerializer.Create1_Method.MakeGenericMethod( type )
						) as Func<SerializationContext, MessagePackSerializer>
				);
#endif // NETSTD_11 || NETSTD_13
			return factory( context );
#endif // AOT
		}

		/// <summary>
		///		Gets existing or new <see cref="MessagePackSerializer"/> instance with default context (<see cref="SerializationContext.Default"/>).
		/// </summary>
		/// <param name="targetType">Target type.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer"/>.
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
		public static MessagePackSerializer Get( Type targetType )
		{
			return Get( targetType, SerializationContext.Default, null );
		}

		/// <summary>
		///		Gets existing or new <see cref="MessagePackSerializer"/> instance with default context (<see cref="SerializationContext.Default"/>).
		/// </summary>
		/// <param name="targetType">Target type.</param>
		/// <param name="providerParameter">A provider specific parameter. See remarks section for details.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer"/>.
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
		public static MessagePackSerializer Get( Type targetType, object providerParameter )
		{
			return Get( targetType, SerializationContext.Default, providerParameter );
		}

		/// <summary>
		///		Gets existing or new <see cref="MessagePackSerializer"/> instance with specified <see cref="SerializationContext"/>.
		/// </summary>
		/// <param name="targetType">Target type.</param>
		/// <param name="context">
		///		<see cref="SerializationContext"/> to store known/created serializers.
		/// </param>
		/// <returns>
		///		<see cref="MessagePackSerializer"/>.
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
		public static MessagePackSerializer Get( Type targetType, SerializationContext context )
		{
			return Get( targetType, context, null );
		}

		/// <summary>
		///		Gets existing or new <see cref="MessagePackSerializer"/> instance with specified <see cref="SerializationContext"/>.
		/// </summary>
		/// <param name="targetType">Target type.</param>
		/// <param name="context">
		///		<see cref="SerializationContext"/> to store known/created serializers.
		/// </param>
		/// <param name="providerParameter">A provider specific parameter. See remarks section for details.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer"/>.
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
		public static MessagePackSerializer Get( Type targetType, SerializationContext context, object providerParameter )
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

#if AOT
		private static readonly System.Reflection.MethodInfo CreateInternal_2 = 
			typeof( MessagePackSerializer ).GetRuntimeMethod( 
				"CreateInternal", 
				new []{ typeof( SerializationContext ), typeof( PolymorphismSchema ) }
			);

		internal static MessagePackSerializer CreateInternal( SerializationContext context, Type targetType, PolymorphismSchema schema )
		{
#if UNITY
			return
				(
					Delegate.CreateDelegate( 
						typeof( Func<SerializationContext, PolymorphismSchema, object> ),
						CreateInternal_2.MakeGenericMethod( targetType )
					)
				as Func<SerializationContext, PolymorphismSchema, object> )( context, schema ) as MessagePackSerializer;
#else
			return 
				( CreateInternal_2.MakeGenericMethod( targetType ).CreateDelegate( typeof( Func<SerializationContext, PolymorphismSchema, object> ) ) 
				as Func<SerializationContext, PolymorphismSchema, object> )( context, schema ) as MessagePackSerializer;
#endif // UNITY
		}
#endif // AOT

		internal static MessagePackSerializer<T> CreateReflectionInternal<T>( SerializationContext context, Type concreteType, PolymorphismSchema schema )
		{
			if ( concreteType.GetIsAbstract() || concreteType.GetIsInterface() )
			{
				// return null for polymoirphic provider.
				return null;
			}

			var serializer = context.Serializers.Get<T>( context );

			if ( serializer != null )
			{
				// For MessagePack.Create compatibility. 
				// Required for built-in types.
				return serializer;
			}

			ValidateType( typeof( T ) );
			var traits = typeof( T ).GetCollectionTraits();
			switch ( traits.CollectionType )
			{
				case CollectionKind.Array:
				case CollectionKind.Map:
				{
					return 
#if !UNITY
						ReflectionSerializerHelper.CreateCollectionSerializer<T>( context, concreteType, traits, ( schema ?? PolymorphismSchema.Default ) );
#else
						Wrap<T>( 
							context,
							ReflectionSerializerHelper.CreateCollectionSerializer<T>( context, concreteType, traits, ( schema ?? PolymorphismSchema.Default ) )
						);
#endif // !UNITY
				}
				default:
				{
					if ( typeof( T ).GetIsEnum() )
					{
						return ReflectionSerializerHelper.CreateReflectionEnumMessagePackSerializer<T>( context );
					}
#if !WINDOWS_PHONE && !NETFX_35 && !UNITY
					if ( TupleItems.IsTuple( typeof( T ) ) )
					{
						return
							new ReflectionTupleMessagePackSerializer<T>(
								context,
								( schema ?? PolymorphismSchema.Default ).ChildSchemaList
							);
					}
#endif // !WINDOWS_PHONE && !NETFX_35 && !UNITY

					return new ReflectionObjectMessagePackSerializer<T>( context );
				}
			}
		}

		private static void ValidateType( Type type )
		{
			if ( !type.GetIsVisible() )
			{
				throw new SerializationException(
					String.Format( CultureInfo.CurrentCulture, "Non-public type '{0}' cannot be serialized.", type ) );
			}
		}

#if UNITY
		internal static MessagePackSerializer<T> Wrap<T>( SerializationContext context, MessagePackSerializer nonGeneric )
		{
			return
				nonGeneric == null
				? null
				: ( nonGeneric as TypedMessagePackSerializerWrapper<T> )
				?? ( 
					nonGeneric is ICustomizableEnumSerializer
					? new EnumTypedMessagePackSerializerWrapper<T>( context, nonGeneric )
					: new TypedMessagePackSerializerWrapper<T>( context, nonGeneric )
				);
		}
#endif // UNITY

		// For stable behavior, use singleton concrete deserializer and private context.
		private static readonly MessagePackSerializer<MessagePackObject> _singleTonMpoDeserializer =
			new DefaultSerializers.MsgPack_MessagePackObjectMessagePackSerializer( new SerializationContext() );

		/// <summary>
		///		Directly deserialize specified MessagePack <see cref="Stream"/> as <see cref="MessagePackObject"/> tree.
		/// </summary>
		/// <param name="stream">The stream which contains deserializing data.</param>
		/// <returns>A <see cref="MessagePackObject"/> which is root of the deserialized MessagePack object tree.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		This method is convinient wrapper for <see cref="MessagePackSerializer.Get{T}(SerializationContext)"/> for <see cref="MessagePackObject"/>.
		///		<note>
		///			You cannot override this method behavior because this method uses private <see cref="SerializationContext"/> instead of default context which is able to be accessed via <see cref="SerializationContext.Default"/>.
		///		</note>
		/// </remarks>
		public static MessagePackObject UnpackMessagePackObject( Stream stream )
		{
			return _singleTonMpoDeserializer.Unpack( stream );
		}

		/// <summary>
		///		Try to prepare specified type for some AOT(Ahead-Of-Time) compilation environment.
		/// </summary>
		/// <typeparam name="T">The type to be prepared. Normally, this should be value type.</typeparam>
		/// <remarks>
		///		<para>
		///			Currently, this method only works in Unity3D build.
		///			This method does not any work for other environments(and should be removed on JIT/AOT), but exists to simplify the application compilation.
		///			It is recommended to use this method on start up code to reduce probability of some AOT errors.
		///		</para>
		///		<para>
		///			Please note that this method do not ensure for full linkage for AOT.
		///			Manifest or attribute based linker options (e.g. for .NET Native or Xamarin.iOS) are still required.
		///		</para>
		/// </remarks>
		// ReSharper disable once UnusedTypeParameter
		public static void PrepareType<T>()
		{
#if UNITY
			// Ensure GetSerializer<T>( object ) is AOT-ed.
			SerializationContext.Default.GetSerializer<T>( null );
			// Ensure Dictionary<T, ?> is work.
			AotHelper.PrepareEqualityComparer<T>();
#endif // UNITY
		}
	}
}
