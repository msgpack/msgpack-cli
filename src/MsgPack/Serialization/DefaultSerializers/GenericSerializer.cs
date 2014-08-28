#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2012-2014 FUJIWARA, Yusuke
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
using System.Collections;
using System.Collections.Generic;
#if !UNITY
using System.Diagnostics.Contracts;
#endif // !UNITY
using System.Globalization;

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		Defines serializer factory for well known structured types.
	/// </summary>
	internal static class GenericSerializer
	{
		public static MessagePackSerializer<T> Create<T>( SerializationContext context )
		{
			return Create( context, typeof( T ) ) as MessagePackSerializer<T>;
		}

		public static IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType )
		{
			if ( targetType.IsArray )
			{
				return CreateArraySerializer( context, targetType );
			}

			Type nullableUnderlyingType;
			if ( ( nullableUnderlyingType = Nullable.GetUnderlyingType( targetType ) ) != null )
			{
				return CreateNullableSerializer( context, nullableUnderlyingType );
			}

			if ( targetType.GetIsGenericType() )
			{
				var typeDefinition = targetType.GetGenericTypeDefinition();
				if ( typeDefinition == typeof( List<> ) )
				{
					return CreateListSerializer( context, targetType.GetGenericArguments()[ 0 ] );
				}

				if ( typeDefinition == typeof( Dictionary<,> ) )
				{
					var genericTypeArguments = targetType.GetGenericArguments();
					return CreateDictionarySerializer( context, genericTypeArguments[ 0 ], genericTypeArguments[ 1 ] );
				}
			}

			return TryCreateImmutableCollectionSerializer( context, targetType );
		}

		private static IMessagePackSingleObjectSerializer CreateArraySerializer( SerializationContext context, Type targetType )
		{
#if DEBUG && !UNITY
			Contract.Assert( targetType.IsArray );
#endif // DEBUG && !UNITY
			return ArraySerializer.Create( context, targetType );
		}

		private static IMessagePackSingleObjectSerializer CreateNullableSerializer( SerializationContext context, Type underlyingType )
		{
			var factoryType = typeof( NullableInstanceFactory<> ).MakeGenericType( underlyingType );
			var instanceFactory = Activator.CreateInstance( factoryType ) as IInstanceFactory;
#if DEBUG && !UNITY
			Contract.Assert( instanceFactory != null );
#endif // DEBUG && !UNITY
			return instanceFactory.Create( context ) as IMessagePackSingleObjectSerializer;
		}

		private static IMessagePackSingleObjectSerializer CreateListSerializer( SerializationContext context, Type itemType )
		{
			var factoryType = typeof( ListInstanceFactory<> ).MakeGenericType( itemType );
			var instanceFactory = Activator.CreateInstance( factoryType ) as IInstanceFactory;
#if DEBUG && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
			Contract.Assert( instanceFactory != null );
			if ( SerializerDebugging.AvoidsGenericSerializer )
			{
				return null;
			}
#endif // DEBUG && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
			return instanceFactory.Create( context ) as IMessagePackSingleObjectSerializer;
		}

		private static IMessagePackSingleObjectSerializer CreateDictionarySerializer( SerializationContext context, Type keyType, Type valueType )
		{
			var factoryType = typeof( DictionaryInstanceFactory<,> ).MakeGenericType( keyType, valueType );
			var instanceFactory = Activator.CreateInstance( factoryType ) as IInstanceFactory;
#if DEBUG && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
			Contract.Assert( instanceFactory != null );
			if ( SerializerDebugging.AvoidsGenericSerializer )
			{
				return null;
			}
#endif // DEBUG && !XAMIOS && !XAMDROID && !UNITY_IPHONE && !UNITY_ANDROID
			return instanceFactory.Create( context ) as IMessagePackSingleObjectSerializer;
		}

#if SILVERLIGHT || NETFX_35 || NETFX_40
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "context", Justification = "Used in other platform" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "targetType", Justification = "Used in other platform" )]
#endif // SILVERLIGHT
		private static IMessagePackSingleObjectSerializer TryCreateImmutableCollectionSerializer( SerializationContext context, Type targetType )
		{
#if NETFX_35 || NETFX_40 || SILVERLIGHT
			// ImmutableCollections does not support above platforms.
			return null;
#else
			if ( targetType.Namespace != "System.Collections.Immutable" )
			{
				return null;
			}

			if ( !targetType.GetIsGenericType() )
			{
				return null;
			}

			switch ( targetType.GetGenericTypeDefinition().Name )
			{
				case "ImmutableList`1":
				case "ImmutableHashSet`1":
				case "ImmutableSortedSet`1":
				case "ImmutableQueue`1":
				{
					return
						Activator.CreateInstance(
							typeof( ImmutableCollectionSerializer<,> )
								.MakeGenericType( targetType, targetType.GetGenericArguments()[ 0 ] ),
							context
						) as IMessagePackSingleObjectSerializer;
				}
				case "ImmutableStack`1":
				{
					return
						Activator.CreateInstance(
							typeof( ImmutableStackSerializer<,> )
								.MakeGenericType( targetType, targetType.GetGenericArguments()[ 0 ] ),
							context
						) as IMessagePackSingleObjectSerializer;
				}
				case "ImmutableDictionary`2":
				case "ImmutableSortedDictionary`2":
				{
					return
						Activator.CreateInstance(
							typeof( ImmutableDictionarySerializer<,,> )
								.MakeGenericType( targetType, targetType.GetGenericArguments()[ 0 ], targetType.GetGenericArguments()[ 1 ] ),
							context
						) as IMessagePackSingleObjectSerializer;
				}
				default:
				{
#if DEBUG && !UNITY
					Contract.Assert( false, "Unknown type:" + targetType );
#endif // DEBUG && !UNITY
					// ReSharper disable HeuristicUnreachableCode
					return null;
					// ReSharper restore HeuristicUnreachableCode
				}
			}
#endif // NETFX_35 || NETFX_40 || SILVERLIGHT
		}

		public static IMessagePackSingleObjectSerializer CreateCollectionInterfaceSerializer( SerializationContext context, Type abstractType, Type concreteType )
		{
			Type serializerType;

			if ( abstractType.GetIsGenericType() && abstractType.GetGenericTypeDefinition() == typeof( IList<> ) )
			{
				serializerType = typeof( ListSerializer<> ).MakeGenericType( abstractType.GetGenericArguments()[ 0 ] );
			}
#if !NETFX_35 && !UNITY
			else if ( abstractType.GetIsGenericType() && abstractType.GetGenericTypeDefinition() == typeof( ISet<> ) )
			{
				serializerType = typeof( SetSerializer<> ).MakeGenericType( abstractType.GetGenericArguments()[ 0 ] );
			}
#endif // !NETFX_35 && !UNITY
			else if ( abstractType.GetIsGenericType() && abstractType.GetGenericTypeDefinition() == typeof( ICollection<> ) )
			{
				serializerType = typeof( CollectionSerializer<> ).MakeGenericType( abstractType.GetGenericArguments()[ 0 ] );
			}
			else if ( abstractType.GetIsGenericType() && abstractType.GetGenericTypeDefinition() == typeof( IEnumerable<> ) )
			{
				serializerType = typeof( EnumerableSerializer<> ).MakeGenericType( abstractType.GetGenericArguments()[ 0 ] );

			}
			else if ( abstractType.GetIsGenericType() && abstractType.GetGenericTypeDefinition() == typeof( IDictionary<,> ) )
			{
				serializerType =
					typeof( DictionarySerializer<,> ).MakeGenericType(
					abstractType.GetGenericArguments()[ 0 ],
					abstractType.GetGenericArguments()[ 1 ]
					);
			}
			else if ( abstractType == typeof( IList ) )
			{
				serializerType = typeof( NonGenericListSerializer );
			}
			else if ( abstractType == typeof( ICollection ) )
			{
				serializerType = typeof( NonGenericCollectionSerializer );
			}
			else if ( abstractType == typeof( IEnumerable ) )
			{
				serializerType = typeof( NonGenericEnumerableSerializer );

			}
			else if ( abstractType == typeof( IDictionary ) )
			{
				serializerType = typeof( NonGenericDictionarySerializer );
			}
			else
			{
				throw new NotSupportedException(
					String.Format( CultureInfo.CurrentCulture, "Abstract type '{0}' is not supported.", abstractType )
				);
			}

			return Activator.CreateInstance( serializerType, context, concreteType ) as IMessagePackSingleObjectSerializer;
		}

		/// <summary>
		///		Defines non-generic factory method.
		/// </summary>
		private interface IInstanceFactory
		{
			object Create( SerializationContext context );
		}

		private sealed class NullableInstanceFactory<T> : IInstanceFactory
			where T : struct
		{
			public NullableInstanceFactory() { }

			public object Create( SerializationContext context )
			{
				return new NullableMessagePackSerializer<T>( context );
			}
		}

		private sealed class ListInstanceFactory<T> : IInstanceFactory
		{
			public ListInstanceFactory() { }

			public object Create( SerializationContext context )
			{
				return new System_Collections_Generic_List_1MessagePackSerializer<T>( context );
			}
		}

		private sealed class DictionaryInstanceFactory<TKey, TValue> : IInstanceFactory
		{
			public DictionaryInstanceFactory() { }

			public object Create( SerializationContext context )
			{
				return new System_Collections_Generic_Dictionary_2MessagePackSerializer<TKey, TValue>( context );
			}
		}
	}
}
