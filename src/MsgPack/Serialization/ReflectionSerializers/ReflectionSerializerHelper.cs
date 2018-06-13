#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2018 FUJIWARA, Yusuke and contributors
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
// Contributors:
//    Samuel Cragg
//
#endregion -- License Terms --

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#define AOT
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Reflection;
using System.Runtime.Serialization;

using MsgPack.Serialization.DefaultSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Helper static methods for reflection serializers.
	/// </summary>
#if UNITY && DEBUG
	public
#else
	internal
#endif
	static class ReflectionSerializerHelper
	{
		internal static readonly PropertyInfo DictionaryEntryKeyProperty = typeof( DictionaryEntry ).GetProperty( "Key" );
		internal static readonly PropertyInfo DictionaryEntryValueProperty = typeof( DictionaryEntry ).GetProperty( "Value" );

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
		public static MessagePackSerializer CreateCollectionSerializer<T>(
#endif // !UNITY
			SerializationContext context,
			Type targetType,
			CollectionTraits traits,
			PolymorphismSchema schema
		)
		{
			var targetInfo = UnpackHelpers.DetermineCollectionSerializationStrategy( targetType, context.CompatibilityOptions.AllowAsymmetricSerializer );

			switch ( traits.DetailedCollectionType )
			{
				case CollectionDetailedKind.Array:
				{
					return ArraySerializer.Create<T>( context, schema );
				}
				case CollectionDetailedKind.GenericList:
#if !NET35 && !UNITY
				case CollectionDetailedKind.GenericSet:
#endif // !NET35 && !UNITY
				case CollectionDetailedKind.GenericCollection:
				{
					return
#if !UNITY
						( MessagePackSerializer<T> )
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantReflectionSerializerFactory>(
							typeof( CollectionSerializerFactory<,> ).MakeGenericType( typeof( T ), traits.ElementType )
						).Create( context, targetType, traits, schema, targetInfo );
#else
						new ReflectionCollectionMessagePackSerializer( context, typeof( T ), targetType, traits, schema, targetInfo );
#endif // !UNITY
				}
				case CollectionDetailedKind.GenericEnumerable:
				{
					return
#if !UNITY
						( MessagePackSerializer<T> )
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantReflectionSerializerFactory>(
							typeof( EnumerableSerializerFactory<,> ).MakeGenericType( typeof( T ), traits.ElementType )
						).Create( context, targetType, traits, schema, targetInfo );
#else
						new ReflectionEnumerableMessagePackSerializer( context, typeof( T ), targetType, traits, schema, targetInfo );
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
						).Create( context, targetType, traits, schema, targetInfo );
#else
						new ReflectionDictionaryMessagePackSerializer(
							context,
							typeof( T ),
							targetType,
							genericArgumentOfKeyValuePair[ 0 ],
							genericArgumentOfKeyValuePair[ 1 ],
							traits,
							schema,
							targetInfo
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
						).Create( context, targetType, traits, schema, targetInfo );
#else
						new ReflectionNonGenericListMessagePackSerializer( context, typeof( T ), targetType, traits, schema, targetInfo );
#endif // !UNITY
				}
				case CollectionDetailedKind.NonGenericCollection:
				{
					return
#if !UNITY
						( MessagePackSerializer<T> )
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantReflectionSerializerFactory>(
							typeof( NonGenericCollectionSerializerFactory<> ).MakeGenericType( typeof( T ) )
						).Create( context, targetType, traits, schema, targetInfo );
#else
						new ReflectionNonGenericCollectionMessagePackSerializer( context, typeof( T ), targetType, traits, schema, targetInfo );
#endif // !UNITY
				}
				case CollectionDetailedKind.NonGenericEnumerable:
				{
					return
#if !UNITY
						( MessagePackSerializer<T> )
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantReflectionSerializerFactory>(
							typeof( NonGenericEnumerableSerializerFactory<> ).MakeGenericType( typeof( T ) )
						).Create( context, targetType, traits, schema, targetInfo );
#else
						new ReflectionNonGenericEnumerableMessagePackSerializer( context, typeof( T ), targetType, traits, schema, targetInfo );
#endif // !UNITY
				}
				case CollectionDetailedKind.NonGenericDictionary:
				{
					return
#if !UNITY
						( MessagePackSerializer<T> )
						ReflectionExtensions.CreateInstancePreservingExceptionType<IVariantReflectionSerializerFactory>(
							typeof( NonGenericDictionarySerializerFactory<> ).MakeGenericType( typeof( T ) )
						).Create( context, targetType, traits, schema, targetInfo );
#else
						new ReflectionNonGenericDictionaryMessagePackSerializer( context, typeof( T ), targetType, traits, schema, targetInfo );
#endif // !UNITY
				}
				default:
				{
					return null;
				}
			}
		}

#if !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "targetType", Justification = "For Unity compatibility." )]
		public static Action<TCollection, TItem> GetAddItem<TCollection, TItem>( Type targetType, CollectionTraits collectionTraits )
#else
		public static Action<object, object> GetAddItem( Type targetType, CollectionTraits collectionTraits )
#endif // !UNITY
		{
			if ( collectionTraits.AddMethod == null )
			{
				throw new SerializationException(
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
#if !UNITY
			if ( SerializerOptions.CanEmit )
			{
				try
				{
					return collectionTraits.AddMethod.CreateDelegate( typeof( Action<TCollection, TItem> ) ) as Action<TCollection, TItem>;
				}
				catch ( ArgumentException ) { }
			}
#endif //! UNITY

			return ( collection, item ) => collectionTraits.AddMethod.InvokePreservingExceptionType( collection, item );
		}

		public static void GetMetadata(
			Type targetType,
			IList<SerializingMember> members,
			SerializationContext context,
			out Func<object, object>[] getters,
			out Action<object, object>[] setters,
			out MemberInfo[] memberInfos,
			out DataMemberContract[] contracts,
			out MessagePackSerializer[] serializers
		)
		{
			SerializationTarget.VerifyCanSerializeTargetType( context, targetType );

			if ( members.Count == 0 )
			{
				if ( !typeof( IPackable ).IsAssignableFrom( targetType ) )
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"At least one serializable member is required because type '{0}' does not implement IPackable interface.",
							targetType
						)
					);
				}

				if ( !typeof( IUnpackable ).IsAssignableFrom( targetType ) )
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"At least one serializable member is required because type '{0}' does not implement IUnpackable interface.",
							targetType
						)
					);
				}

#if FEATURE_TAP
				if ( context.SerializerOptions.WithAsync )
				{
					if ( !typeof( IAsyncPackable ).IsAssignableFrom( targetType ) )
					{
						throw new SerializationException(
							String.Format(
								CultureInfo.CurrentCulture,
								"At least one serializable member is required because type '{0}' does not implement IAsyncPackable interface.",
								targetType
							)
						);
					}

					if ( !typeof( IAsyncUnpackable ).IsAssignableFrom( targetType ) )
					{
						throw new SerializationException(
							String.Format(
								CultureInfo.CurrentCulture,
								"At least one serializable member is required because type '{0}' does not implement IAsyncUnpackable interface.",
								targetType
							)
						);
					}
				}

#endif // FEATURE_TAP

			}

			getters = new Func<object, object>[ members.Count ];
			setters = new Action<object, object>[ members.Count ];
			memberInfos = new MemberInfo[ members.Count ];
			contracts = new DataMemberContract[ members.Count ];
			serializers = new MessagePackSerializer[ members.Count ];

			for ( var i = 0; i < members.Count; i++ )
			{
				var member = members[ i ];

				if ( member.Member == null )
				{
					// Missing member exist because of unconbinous Id of MessagePackMember or Order of DataMember. 
#if UNITY
					contracts[ i ] = DataMemberContract.Null;
#endif // UNITY
					continue;
				}

				FieldInfo asField;
				if ( ( asField = member.Member as FieldInfo ) != null )
				{
					if ( context.SerializerOptions.DisablePrivilegedAccess && !asField.GetIsPublic() )
					{
						continue;
					}

					getters[ i ] = asField.GetValue;
					if ( !asField.IsInitOnly )
					{
						setters[ i ] = asField.SetValue;
					}
				}
				else
				{
					var property = member.Member as PropertyInfo;
#if DEBUG
					Contract.Assert( property != null, "member.Member is PropertyInfo" );
#endif // DEBUG
					if ( context.SerializerOptions.DisablePrivilegedAccess && !property.GetIsPublic() )
					{
						continue;
					}

					var getter = property.GetGetMethod( true );
					if ( getter == null )
					{
						ThrowMissingGetterException( targetType, i, property );
					}

					getters[ i ] = target => getter.InvokePreservingExceptionType( target, null );
					var setter = property.GetSetMethod( true );
					if ( setter != null && ( !context.SerializerOptions.DisablePrivilegedAccess || setter.GetIsPublic() ) )
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

		private static void ThrowMissingGetterException( Type targetType, int number, PropertyInfo property )
		{
			throw new SerializationException(
				String.Format(
					CultureInfo.CurrentCulture,
					"The {0}th getter metadata of '{1}' ('{2}' property) is missing."
#if UNITY
					+ " Ensure link.xml or [Preserve] attribute for the target type, or use pre-generated serializer."
#endif // UNITY
					,
					number,
					targetType,
					property
				)
			);
		}

#if !UNITY
		public static Func<int, T> CreateCollectionInstanceFactory<T, TKey>( ConstructorInfo constructor )
#else
		public static Func<int, object> CreateCollectionInstanceFactory( Type abstractType, Type targetType, Type comparisonType, ConstructorInfo constructor )
#endif // !UNITY
		{
			// ReSharper disable once PossibleNullReferenceException
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

			SerializationExceptions.ThrowTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity(
#if !UNITY
				typeof( T )
#else
				abstractType
#endif // !UNITY
			);
			return null; // Never reach
		}

#if !UNITY
		/// <summary>
		///		Defines non-generic factory method for 'universal' serializers which use general collection features.
		/// </summary>
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		interface IVariantReflectionSerializerFactory
		{
			MessagePackSerializer Create( SerializationContext context, Type targetType, CollectionTraits collectionTraits, PolymorphismSchema schema, SerializationTarget targetInfo );
		}

		// ReSharper disable MemberHidesStaticFromOuterClass

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class NonGenericEnumerableSerializerFactory<T> : IVariantReflectionSerializerFactory
			where T : IEnumerable
		{
			public NonGenericEnumerableSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, CollectionTraits collectionTraits, PolymorphismSchema schema, SerializationTarget targetInfo )
			{
				return new ReflectionNonGenericEnumerableMessagePackSerializer<T>( context, targetType, collectionTraits, schema, targetInfo );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class NonGenericCollectionSerializerFactory<T> : IVariantReflectionSerializerFactory
			where T : ICollection
		{
			public NonGenericCollectionSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, CollectionTraits collectionTraits, PolymorphismSchema schema, SerializationTarget targetInfo )
			{
				return new ReflectionNonGenericCollectionMessagePackSerializer<T>( context, targetType, collectionTraits, schema, targetInfo );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class NonGenericListSerializerFactory<T> : IVariantReflectionSerializerFactory
			where T : IList
		{
			public NonGenericListSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, CollectionTraits collectionTraits, PolymorphismSchema schema, SerializationTarget targetInfo )
			{
				return new ReflectionNonGenericListMessagePackSerializer<T>( context, targetType, collectionTraits, schema, targetInfo );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class NonGenericDictionarySerializerFactory<T> : IVariantReflectionSerializerFactory
			where T : IDictionary
		{
			public NonGenericDictionarySerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, CollectionTraits collectionTraits, PolymorphismSchema schema, SerializationTarget targetInfo )
			{
				return new ReflectionNonGenericDictionaryMessagePackSerializer<T>( context, targetType, collectionTraits, schema, targetInfo );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class EnumerableSerializerFactory<TCollection, TItem> : IVariantReflectionSerializerFactory
			where TCollection : IEnumerable<TItem>
		{
			public EnumerableSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, CollectionTraits collectionTraits, PolymorphismSchema schema, SerializationTarget targetInfo )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new ReflectionEnumerableMessagePackSerializer<TCollection, TItem>( context, targetType, collectionTraits, itemSchema, targetInfo );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class CollectionSerializerFactory<TCollection, TItem> : IVariantReflectionSerializerFactory
			where TCollection : ICollection<TItem>
		{
			public CollectionSerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, CollectionTraits collectionTraits, PolymorphismSchema schema, SerializationTarget targetInfo )
			{
				var itemSchema = schema ?? PolymorphismSchema.Default;
				return new ReflectionCollectionMessagePackSerializer<TCollection, TItem>( context, targetType, collectionTraits, itemSchema, targetInfo );
			}
		}

		[Preserve( AllMembers = true )]
#if SILVERLIGHT
		internal
#else
		private
#endif // SILVERLIGHT
		sealed class DictionarySerializerFactory<TDictionary, TKey, TValue> : IVariantReflectionSerializerFactory
			where TDictionary : IDictionary<TKey, TValue>
		{
			public DictionarySerializerFactory() { }

			public MessagePackSerializer Create( SerializationContext context, Type targetType, CollectionTraits collectionTraits, PolymorphismSchema schema, SerializationTarget targetInfo )
			{
				return new ReflectionDictionaryMessagePackSerializer<TDictionary, TKey, TValue>( context, targetType, collectionTraits, schema, targetInfo );
			}
		}
		// ReSharper restore MemberHidesStaticFromOuterClass
#endif // !UNITY
	}
}
