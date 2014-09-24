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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if !UNITY
using System.Diagnostics.Contracts;
#endif // !UNITY
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
#if UNITY_IPHONE
			throw new PlatformNotSupportedException( "On-the-fly enum serializer generation is not supported in Unity iOS. Use pre-generated serializer instead." );
#else
			return
				Activator.CreateInstance( typeof( ReflectionEnumMessagePackSerializer<> ).MakeGenericType( typeof( T ) ), context )
					as
					MessagePackSerializer<T>;
#endif
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
#if !NETFX_35 && !UNITY
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
#endif // !NETFX_35 && !UNITY
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
#if DEBUG && !UNITY
					Contract.Assert( traits.DetailedCollectionType == CollectionDetailedKind.NonGenericEnumerable );
#endif // DEBUG && !UNITY
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
#if DEBUG && !UNITY
				Contract.Assert( traits.DetailedCollectionType == CollectionDetailedKind.NonGenericDictionary );
#endif // DEBUG && !UNITY
				return
					new ReflectionCollectionSerializer<T>(
						context,
						new NonGenericDictionarySerializer( context, targetType )
						);
			}
		}

		public static void GetMetadata(
			SerializationContext context,
			Type targetType,
			out Func<object, object>[] getters,
			out Action<object, object>[] setters,
			out MemberInfo[] memberInfos,
			out DataMemberContract[] contracts,
			out IMessagePackSerializer[] serializers )
		{
			SerializationTarget.VerifyType( targetType );
			var members = SerializationTarget.Prepare( context, targetType );

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
					Contract.Assert( property != null );
#endif // DEBUG && !UNITY
					getters[ i ] = target => property.GetGetMethod( true ).Invoke( target, null );
					var setter = property.GetSetMethod( true );
					if ( setter != null )
					{
						setters[ i ] = ( target, value ) => setter.Invoke( target, new[] { value } );
					}
				}

				memberInfos[ i ] = member.Member;
				contracts[ i ] = member.Contract;
				var memberType = member.Member.GetMemberValueType();
				if ( !memberType.GetIsEnum() )
				{
					serializers[ i ] = context.GetSerializer( memberType );

				}
				else
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
			}
		}
	}
}