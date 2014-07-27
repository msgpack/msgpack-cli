#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;
using System.Reflection;

using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Helper static methods for reflection serializers.
	/// </summary>
	internal static class ReflectionSerializerHelper
	{
		public static MessagePackSerializer<T> CreateReflectionEnuMessagePackSerializer<T>( SerializationContext context )
		{
			return
				Activator.CreateInstance( typeof( ReflectionEnumMessagePackSerializer<> ).MakeGenericType( typeof( T ) ), context )
					as
					MessagePackSerializer<T>;
		}

		public static MessagePackSerializer<T> CreateArraySerializer<T>(
			SerializationContext context,
			Type targetType,
			CollectionTraits traits )
		{
			switch ( traits.DetailedCollectionType )
			{
				case CollectionDetailedKind.Array:
				{
					return ArraySerializer.Create<T>( context );
				}
				case CollectionDetailedKind.GenericList:
				{
					return
						new ReflectionCollectionSerializer<T>(
							context,
							Activator.CreateInstance(
								typeof( ListSerializer<> ).MakeGenericType( traits.ElementType ),
								context,
								targetType
								) as IMessagePackSerializer
							);
				}
#if !NETFX_35 && !UNITY_ANDROID && !UNITY_IPHONE
				case CollectionDetailedKind.GenericSet:
				{
					return
						new ReflectionCollectionSerializer<T>(
							context,
							Activator.CreateInstance(
								typeof( SetSerializer<> ).MakeGenericType( traits.ElementType ),
								context,
								targetType
								) as IMessagePackSerializer
							);
				}
#endif // !NETFX_35 && !UNITY_ANDROID && !UNITY_IPHONE
				case CollectionDetailedKind.GenericCollection:
				{
					return
						new ReflectionCollectionSerializer<T>(
							context,
							Activator.CreateInstance(
								typeof( CollectionSerializer<> ).MakeGenericType( traits.ElementType ),
								context,
								targetType
								) as IMessagePackSerializer
							);
				}
				case CollectionDetailedKind.GenericEnumerable:
				{
					return
						new ReflectionCollectionSerializer<T>(
							context,
							Activator.CreateInstance(
								typeof( EnumerableSerializer<> ).MakeGenericType( traits.ElementType ),
								context,
								targetType
								) as IMessagePackSerializer
							);
				}
				case CollectionDetailedKind.NonGenericList:
				{
					return
						new ReflectionCollectionSerializer<T>(
							context,
							new NonGenericListSerializer( context, targetType )
							);
				}
				case CollectionDetailedKind.NonGenericCollection:
				{
					return
						new ReflectionCollectionSerializer<T>(
							context,
							new NonGenericCollectionSerializer( context, targetType )
							);
				}
				default:
				{
#if DEBUG && !UNITY_ANDROID && !UNITY_IPHONE
					Contract.Assert( traits.DetailedCollectionType == CollectionDetailedKind.NonGenericEnumerable );
#endif // DEBUG && !UNITY_ANDROID && !UNITY_IPHONE
					return
						new ReflectionCollectionSerializer<T>(
							context,
							new NonGenericEnumerableSerializer( context, targetType )
							);
				}
			}
		}

		public static MessagePackSerializer<T> CreateMapSerializer<T>(
			SerializationContext context,
			Type targetType,
			CollectionTraits traits )
		{
			// ReSharper disable once RedundantIfElseBlock
			if ( traits.DetailedCollectionType == CollectionDetailedKind.GenericDictionary )
			{
				return
					new ReflectionCollectionSerializer<T>(
						context,
						Activator.CreateInstance(
							typeof( DictionarySerializer<,> ).MakeGenericType( traits.ElementType.GetGenericArguments() ),
							context,
							targetType
							) as IMessagePackSerializer
						);
			}
			else
			{
#if DEBUG && !UNITY_ANDROID && !UNITY_IPHONE
				Contract.Assert( traits.DetailedCollectionType == CollectionDetailedKind.NonGenericDictionary );
#endif // DEBUG && !UNITY_ANDROID && !UNITY_IPHONE
				return
					new ReflectionCollectionSerializer<T>(
						context,
						new NonGenericDictionarySerializer( context, targetType )
						);
			}
		}

		public static void GetMetadata(
			SerializationContext context,
			Type targeType,
			out MemberInfo[] getters,
			out MemberInfo[] setters,
			out DataMemberContract[] contracts,
			out IMessagePackSerializer[] serializers )
		{
			var members = SerializationTarget.Prepare( context, targeType ).ToDictionary( member => member.Contract.Id );
			var membersCount = members.Keys.Max();
			if ( context.CompatibilityOptions.OneBoundDataMemberOrder )
			{
				membersCount--;
			}

			getters = new MemberInfo[ membersCount ];
			setters = new MemberInfo[ membersCount ];
			contracts = new DataMemberContract[ membersCount ];
			serializers = new IMessagePackSerializer[ membersCount ];

			// TODO: CollecitonProperty, ReadOnly property
			for ( var i = 0; i < membersCount; i++ )
			{
				SerializingMember member;
				if ( !members.TryGetValue( i, out member ) )
				{
					continue;
				}

				if ( member.Member is FieldInfo )
				{
					getters[ i ] = member.Member;
					setters[ i ] = member.Member;
				}
				else
				{
					var property = member.Member as PropertyInfo;
#if DEBUG
					Contract.Assert( property != null );
#endif
					getters[ i ] = property.GetGetMethod( true );
					setters[ i ] = property.GetSetMethod( true );
				}

				contracts[ i ] = member.Contract;
				serializers[ i ] = context.GetSerializer( member.Member.GetMemberValueType() );
			}
		}
	}
}