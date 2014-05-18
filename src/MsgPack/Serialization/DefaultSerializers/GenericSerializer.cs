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

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		Defines serializer factory for well known structured types.
	/// </summary>
	internal static class GenericSerializer
	{
		public static MessagePackSerializer<T> Create<T>( SerializationContext context )
		{
			if ( typeof( T ).IsArray )
			{
				return CreateArraySerializer<T>( context );
			}

			Type nullableUnderlyingType;
			if ( ( nullableUnderlyingType = Nullable.GetUnderlyingType( typeof( T ) ) ) != null )
			{
				return CreateNullableSerializer<T>( context, nullableUnderlyingType );
			}

			if ( typeof( T ).GetIsGenericType() )
			{
				var typeDefinition = typeof( T ).GetGenericTypeDefinition();
				if ( typeDefinition == typeof( List<> ) )
				{
					return CreateListSerializer<T>( context, typeof( T ).GetGenericArguments()[ 0 ] );
				}

				if ( typeDefinition == typeof( Dictionary<,> ) )
				{
					var genericTypeArguments = typeof( T ).GetGenericArguments();
					return CreateDictionarySerializer<T>( context, genericTypeArguments[ 0 ], genericTypeArguments[ 1 ] );
				}
			}

			return TryCreateImmutableCollectionSerializer<T>( context );
		}

		private static MessagePackSerializer<T> CreateArraySerializer<T>( SerializationContext context )
		{
#if DEBUG
			Contract.Assert( typeof( T ).IsArray );
#endif // if DEBUG
			return ArraySerializer.Create<T>( context );
		}

		private static MessagePackSerializer<T> CreateNullableSerializer<T>( SerializationContext context, Type underlyingType )
		{
			var factoryType = typeof( NullableInstanceFactory<> ).MakeGenericType( underlyingType );
			var instanceFactory = Activator.CreateInstance( factoryType ) as IInstanceFactory;
#if DEBUG
			Contract.Assert( instanceFactory != null );
#endif
			return instanceFactory.Create( context ) as MessagePackSerializer<T>;
		}

		private static MessagePackSerializer<T> CreateListSerializer<T>( SerializationContext context, Type itemType )
		{
			var factoryType = typeof( ListInstanceFactory<> ).MakeGenericType( itemType );
			var instanceFactory = Activator.CreateInstance( factoryType ) as IInstanceFactory;
#if DEBUG && !XAMIOS && !UNITY_IPHONE && !UNITY_ANDROID
			Contract.Assert( instanceFactory != null );
			if ( SerializerDebugging.AvoidsGenericSerializer )
			{
				return null;
			}
#endif // DEBUG && !XAMIOS && !UNITY_IPHONE && !UNITY_ANDROID
			return instanceFactory.Create( context ) as MessagePackSerializer<T>;
		}

		private static MessagePackSerializer<T> CreateDictionarySerializer<T>( SerializationContext context, Type keyType, Type valueType )
		{
			var factoryType = typeof( DictionaryInstanceFactory<,> ).MakeGenericType( keyType, valueType );
			var instanceFactory = Activator.CreateInstance( factoryType ) as IInstanceFactory;
#if DEBUG && !XAMIOS && !UNITY_IPHONE && !UNITY_ANDROID
			Contract.Assert( instanceFactory != null );
			if ( SerializerDebugging.AvoidsGenericSerializer )
			{
				return null;
			}
#endif // DEBUG && !XAMIOS && !UNITY_IPHONE && !UNITY_ANDROID
			return instanceFactory.Create( context ) as MessagePackSerializer<T>;
		}

		private static MessagePackSerializer<T> TryCreateImmutableCollectionSerializer<T>( SerializationContext context )
		{
#if NETFX_35 || NETFX_40 || SILVERLIGHT
			// ImmutableCollections does not support above platforms.
			return null;
#else
			if ( typeof( T ).Namespace != "System.Collections.Immutable" )
			{
				return null;
			}

			if ( !typeof( T ).GetIsGenericType() )
			{
				return null;
			}

			switch ( typeof( T ).GetGenericTypeDefinition().Name )
			{
				case "ImmutableList`1":
				case "ImmutableHashSet`1":
				case "ImmutableSortedSet`1":
				case "ImmutableQueue`1":
				{
					return
						Activator.CreateInstance(
							typeof( ImmutableCollectionSerializer<,> )
							.MakeGenericType( typeof( T ), typeof( T ).GetGenericArguments()[ 0 ] ),
							context
						) as MessagePackSerializer<T>;
				}
				case "ImmutableStack`1":
				{
					return
						Activator.CreateInstance(
							typeof( ImmutableStackSerializer<,> )
							.MakeGenericType( typeof( T ), typeof( T ).GetGenericArguments()[ 0 ] ),
							context
						) as MessagePackSerializer<T>;
				}
				case "ImmutableDictionary`2":
				case "ImmutableSortedDictionary`2":
				{
					return
						Activator.CreateInstance(
							typeof( ImmutableDictionarySerializer<,,> )
							.MakeGenericType( typeof( T ), typeof( T ).GetGenericArguments()[ 0 ], typeof( T ).GetGenericArguments()[ 1 ] ),
							context
						) as MessagePackSerializer<T>;
				}
				default:
				{
#if DEBUG
					Contract.Assert( false, "Unknown type:" + typeof( T ) );
#endif
					// ReSharper disable HeuristicUnreachableCode
					return null;
					// ReSharper restore HeuristicUnreachableCode
				}
			}
#endif
		}

		public static IMessagePackSerializer CreateCollectionInterfaceSerializer( SerializationContext context, Type abstractType, Type concreteType )
		{
			Type serializerType;

			if ( abstractType.GetGenericTypeDefinition() == typeof( IList<> ) )
			{
				serializerType = typeof( ListSerializer<> ).MakeGenericType( abstractType.GetGenericArguments()[ 0 ] );
			}
#if !NETFX_35
			else if ( abstractType.GetGenericTypeDefinition() == typeof( ISet<> ) )
			{
				serializerType = typeof( SetSerializer<> ).MakeGenericType( abstractType.GetGenericArguments()[ 0 ] );
			}
#endif // !NETFX_35
			else if ( abstractType.GetGenericTypeDefinition() == typeof( ICollection<> ) )
			{
				serializerType = typeof( CollectionSerializer<> ).MakeGenericType( abstractType.GetGenericArguments()[ 0 ] );
			}
			else if ( abstractType.GetGenericTypeDefinition() == typeof( IEnumerable<> ) )
			{
				serializerType = typeof( EnumerableSerializer<> ).MakeGenericType( abstractType.GetGenericArguments()[ 0 ] );

			}
			else if ( abstractType.GetGenericTypeDefinition() == typeof( IDictionary<,> ) )
			{
				serializerType =
					typeof( DictionarySerializer<,> ).MakeGenericType(
					abstractType.GetGenericArguments()[ 0 ],
					abstractType.GetGenericArguments()[ 1 ]
					);
			}
			else
			{
				throw new NotSupportedException(
					String.Format( CultureInfo.CurrentCulture, "Abstract type '{0}' is not supported.", abstractType )
				);
			}

			return Activator.CreateInstance( serializerType, context, concreteType ) as IMessagePackSerializer;
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
