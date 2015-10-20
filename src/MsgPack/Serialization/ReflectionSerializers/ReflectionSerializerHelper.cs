#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2015 FUJIWARA, Yusuke
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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Reflection;

using MsgPack.Serialization.DefaultSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Helper static methods for reflection serializers.
	/// </summary>
	internal static class ReflectionSerializerHelper
	{
		public static MessagePackSerializer<T> CreateReflectionEnumMessagePackSerializer<T>( SerializationContext context )
		{
#if !UNITY
			return
				ReflectionExtensions.CreateInstancePreservingExceptionType<MessagePackSerializer<T>>(
					typeof( ReflectionEnumMessagePackSerializer<> ).MakeGenericType( typeof( T ) ),
					context 
				);
#else
			return MessagePackSerializer.Wrap<T>( context, new ReflectionEnumMessagePackSerializer( context, typeof( T ) ) );
#endif // !UNITY
		}

#if !UNITY
		public static MessagePackSerializer<T> CreateCollectionSerializer<T>(
#else
		public static IMessagePackSingleObjectSerializer CreateCollectionSerializer<T>(
#endif // !UNITY
			SerializationContext context,
			Type targetType,
			CollectionTraits traits,
			PolymorphismSchema schema
		)
		{
			switch ( traits.DetailedCollectionType )
			{
				case CollectionDetailedKind.Array:
				{
					return ArraySerializer.Create<T>( context, schema );
				}
				case CollectionDetailedKind.GenericList:
#if !NETFX_35 && !UNITY
				case CollectionDetailedKind.GenericSet:
#endif // !NETFX_35 && !UNITY
				case CollectionDetailedKind.GenericCollection:
				{
					return
#if !UNITY
						( MessagePackSerializer<T> )
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantReflectionSerializerFactory>(
							typeof( CollectionSerializerFactory<,> ).MakeGenericType( typeof( T ), traits.ElementType )
						).Create( context, targetType, schema );
#else
						new ReflectionCollectionMessagePackSerializer( context, typeof( T ), targetType, traits, schema );
#endif // !UNITY
				}
				case CollectionDetailedKind.GenericEnumerable:
				{
					return
#if !UNITY
						( MessagePackSerializer<T> )
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantReflectionSerializerFactory>(
							typeof( EnumerableSerializerFactory<,> ).MakeGenericType( typeof( T ), traits.ElementType )
						).Create( context, targetType, schema );
#else
						new ReflectionEnumerableMessagePackSerializer( context, typeof( T ), targetType, traits, schema );
#endif // !Enumerable
				}
				case CollectionDetailedKind.GenericDictionary:
				{
					var genericArgumentOfKeyValuePair = traits.ElementType.GetGenericArguments();
					return
#if !UNITY
						( MessagePackSerializer<T> )
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantReflectionSerializerFactory>(
							typeof( DictionarySerializerFactory<,,> ).MakeGenericType(
								typeof( T ),
								genericArgumentOfKeyValuePair[ 0 ],
								genericArgumentOfKeyValuePair[ 1 ]
							)
						).Create( context, targetType, schema );
#else
						new ReflectionDictionaryMessagePackSerializer(
							context,
							typeof( T ),
							targetType,
							genericArgumentOfKeyValuePair[ 0 ],
							genericArgumentOfKeyValuePair[ 1 ],
							traits,
							schema 
						);
#endif // !UNITY
				}
				case CollectionDetailedKind.NonGenericList:
				{
					return
#if !UNITY
						( MessagePackSerializer<T> )
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantReflectionSerializerFactory>(
							typeof( NonGenericListSerializerFactory<> ).MakeGenericType( typeof( T ) )
						).Create( context, targetType, schema );
#else
						new ReflectionNonGenericListMessagePackSerializer( context, typeof( T ), targetType, schema );
#endif // !UNITY
				}
				case CollectionDetailedKind.NonGenericCollection:
				{
					return
#if !UNITY
						( MessagePackSerializer<T> )
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantReflectionSerializerFactory>(
							typeof( NonGenericCollectionSerializerFactory<> ).MakeGenericType( typeof( T ), traits.ElementType )
						).Create( context, targetType, schema );
#else
						new ReflectionNonGenericCollectionMessagePackSerializer( context, typeof( T ), targetType, schema );
#endif // !UNITY
				}
				case CollectionDetailedKind.NonGenericEnumerable:
				{
					return
#if !UNITY
						( MessagePackSerializer<T> )
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantReflectionSerializerFactory>(
							typeof( NonGenericEnumerableSerializerFactory<> ).MakeGenericType( typeof( T ), traits.ElementType )
						).Create( context, targetType, schema );
#else
						new ReflectionNonGenericEnumerableMessagePackSerializer( context, typeof( T ), targetType, schema );
#endif // !UNITY
				}
				case CollectionDetailedKind.NonGenericDictionary:
				{
					return
#if !UNITY
						( MessagePackSerializer<T> )
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantReflectionSerializerFactory>(
							typeof( NonGenericDictionarySerializerFactory<> ).MakeGenericType( typeof( T ) )
						).Create( context, targetType, schema );
#else
						new ReflectionNonGenericDictionaryMessagePackSerializer( context, typeof( T ), targetType, schema );
#endif // !UNITY
				}
				default:
				{
					return null;
				}
			}
		}

#if !UNITY
		public static Action<TCollection, TItem> GetAddItem<TCollection, TItem>( Type targetType )
#else
		public static Action<object, object> GetAddItem( Type targetType )
#endif // !UNITY
		{
			var addMethod = targetType.GetCollectionTraits().AddMethod;
			if ( addMethod == null )
			{
				throw new NotSupportedException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Reflection based serializer only supports collection types which implement interface to add new item such as '{0}' and '{1}'",
						typeof( ICollection<> ).GetFullName(),
						typeof( IList )
					)
				);
			}

			// CreateDelegate causes AOT error.
			// So use reflection in AOT environment.
#if ( !UNITY && !XAMIOS ) || AOT_CHECK
			try
			{
				return addMethod.CreateDelegate( typeof( Action<TCollection, TItem> ) ) as Action<TCollection, TItem>;
			}
			catch ( ArgumentException )
			{
#endif // ( !UNITY && !XAMIOS ) || AOT_CHECK
			return ( collection, item ) => addMethod.InvokePreservingExceptionType( collection, item );
#if ( !UNITY && !XAMIOS ) || AOT_CHECK
			}
#endif // ( !UNITY && !XAMIOS ) || AOT_CHECK
		}

		public static void GetMetadata(
			IList<SerializingMember> members,
			SerializationContext context,
			out Func<object, object>[] getters,
			out Action<object, object>[] setters,
			out MemberInfo[] memberInfos,
			out DataMemberContract[] contracts,
			out IMessagePackSerializer[] serializers )
		{
			getters = new Func<object, object>[ members.Count ];
			setters = new Action<object, object>[ members.Count ];
			memberInfos = new MemberInfo[ members.Count ];
			contracts = new DataMemberContract[ members.Count ];
			serializers = new IMessagePackSerializer[ members.Count ];

			for ( var i = 0; i < members.Count; i++ )
			{
				var member = members[ i ];
				if ( member.Member == null )
				{
#if UNITY
					contracts[ i ] = DataMemberContract.Null;
#endif // UNITY
					continue;
				}

				FieldInfo asField;
				if ( ( asField = member.Member as FieldInfo ) != null )
				{
					getters[ i ] = asField.GetValue;
					setters[ i ] = asField.SetValue;
				}
				else
				{
					var property = member.Member as PropertyInfo;
#if DEBUG && !UNITY
					Contract.Assert( property != null, "member.Member is PropertyInfo" );
#endif // DEBUG && !UNITY
					getters[ i ] = target => property.GetGetMethod( true ).InvokePreservingExceptionType( target, null );
					var setter = property.GetSetMethod( true );
					if ( setter != null )
					{
						setters[ i ] = ( target, value ) => setter.InvokePreservingExceptionType( target, new[] { value } );
					}
				}

				memberInfos[ i ] = member.Member;
#if !UNITY
				contracts[ i ] = member.Contract;
#else
				contracts[ i ] = member.Contract ?? DataMemberContract.Null;
#endif // !UNITY
				var memberType = member.Member.GetMemberValueType();
				if ( memberType.GetIsEnum() )
				{
					serializers[ i ] =
						context.GetSerializer(
							memberType,
							EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod(
								context,
								memberType,
								member.GetEnumMemberSerializationMethod()
							)
						);
				}
				else if ( DateTimeMessagePackSerializerHelpers.IsDateTime( memberType ) )
				{
					serializers[ i ] =
						context.GetSerializer(
							memberType,
							DateTimeMessagePackSerializerHelpers.DetermineDateTimeConversionMethod(
								context,
								member.GetDateTimeMemberConversionMethod()
							)
						);
				}
				else
				{
					serializers[ i ] = context.GetSerializer( memberType, PolymorphismSchema.Create( memberType, member ) );
				}
			}
		}

#if !UNITY
		public static Func<int, T> CreateCollectionInstanceFactory<T, TKey>( Type targetType )
#else
		public static Func<int, object> CreateCollectionInstanceFactory( Type abstractType, Type targetType, Type comparisonType )
#endif // !UNITY
		{
			var constructor = UnpackHelpers.GetCollectionConstructor( targetType );
			var parameters = constructor.GetParameters();

			switch ( parameters.Length )
			{
				case 0:
				{
					return _ => 
#if !UNITY
						( T )
#endif // !UNITY
						constructor.InvokePreservingExceptionType();
				}
				case 1:
				{
					if ( parameters[ 0 ].ParameterType == typeof( int ) )
					{
						return capacity =>
#if !UNITY
							( T )
#endif // !UNITY
							constructor.InvokePreservingExceptionType( capacity );
					}
					else if ( UnpackHelpers.IsIEqualityComparer( parameters[ 0 ].ParameterType ) )
					{
						var comparer = 
#if !UNITY
							EqualityComparer<TKey>.Default;
#else
							UnpackHelpers.GetEqualityComparer( comparisonType );
#endif // !UNITY
						return _ =>
#if !UNITY
							( T )
#endif // !UNITY
							constructor.InvokePreservingExceptionType( comparer );
					}

					break;
				}
				case 2:
				{
					var comparer =
#if !UNITY
						EqualityComparer<TKey>.Default;
#else
						UnpackHelpers.GetEqualityComparer( comparisonType );
#endif // !UNITY
					if ( parameters[ 0 ].ParameterType == typeof( int )
						&& UnpackHelpers.IsIEqualityComparer( parameters[ 1 ].ParameterType ) )
					{
						return capacity =>
#if !UNITY
							( T )
#endif // !UNITY
							constructor.InvokePreservingExceptionType( capacity, comparer );
					}
					else if ( UnpackHelpers.IsIEqualityComparer( parameters[ 0 ].ParameterType ) &&
							parameters[ 0 ].ParameterType == typeof( int ) )
					{
						return capacity =>
#if !UNITY
							( T )
#endif // !UNITY
							constructor.InvokePreservingExceptionType( comparer, capacity );
					}

					break;
				}
			}
			
			throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity(
#if !UNITY
				typeof( T )
#else
				abstractType
#endif // !UNITY
			);
		}

#if !UNITY
		/// <summary>
		///		Defines non-generic factory method for 'universal' serializers which use general collection features.
		/// </summary>
		private interface IVariantReflectionSerializerFactory
		{
			IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema );
		}

		// ReSharper disable MemberHidesStaticFromOuterClass

		private sealed class NonGenericEnumerableSerializerFactory<T> : IVariantReflectionSerializerFactory
			where T : IEnumerable
		{
			public NonGenericEnumerableSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new ReflectionNonGenericEnumerableMessagePackSerializer<T>( context, targetType, schema );
			}
		}

		private sealed class NonGenericCollectionSerializerFactory<T> : IVariantReflectionSerializerFactory
			where T : ICollection
		{
			public NonGenericCollectionSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new ReflectionNonGenericCollectionMessagePackSerializer<T>( context, targetType, schema );
			}
		}

		private sealed class NonGenericListSerializerFactory<T> : IVariantReflectionSerializerFactory
			where T : IList
		{
			public NonGenericListSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new ReflectionNonGenericListMessagePackSerializer<T>( context, targetType, schema );
			}
		}

		private sealed class NonGenericDictionarySerializerFactory<T> : IVariantReflectionSerializerFactory
			where T : IDictionary
		{
			public NonGenericDictionarySerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new ReflectionNonGenericDictionaryMessagePackSerializer<T>( context, targetType, schema );
			}
		}

		private sealed class EnumerableSerializerFactory<TCollection, TItem> : IVariantReflectionSerializerFactory
			where TCollection : IEnumerable<TItem>
		{
			public EnumerableSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new ReflectionEnumerableMessagePackSerializer<TCollection, TItem>( context, targetType, itemSchema );
			}
		}

		private sealed class CollectionSerializerFactory<TCollection, TItem> : IVariantReflectionSerializerFactory
			where TCollection : ICollection<TItem>
		{
			public CollectionSerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new ReflectionCollectionMessagePackSerializer<TCollection, TItem>( context, targetType, itemSchema );
			}
		}

		private sealed class DictionarySerializerFactory<TDictionary, TKey, TValue> : IVariantReflectionSerializerFactory
			where TDictionary : IDictionary<TKey, TValue>
		{
			public DictionarySerializerFactory() { }

			public IMessagePackSingleObjectSerializer Create( SerializationContext context, Type targetType, PolymorphismSchema schema )
			{
				return new ReflectionDictionaryMessagePackSerializer<TDictionary, TKey, TValue>( context, targetType, schema );
			}
		}
		// ReSharper restore MemberHidesStaticFromOuterClass
#endif // !UNITY
	}
}