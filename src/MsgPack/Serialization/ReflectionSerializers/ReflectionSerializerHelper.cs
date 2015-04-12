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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
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
		public static MessagePackSerializer<T> CreateReflectionEnumMessagePackSerializer<T>( SerializationContext context )
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
			CollectionTraits traits,
			PolymorphismSchema itemsSchema
		)
		{
			switch ( traits.DetailedCollectionType )
			{
				case CollectionDetailedKind.Array:
				{
					return ArraySerializer.Create<T>( context, itemsSchema );
				}
				default:
				{
					return
						( MessagePackSerializer<T> )
							GenericSerializer.TryCreateAbstractCollectionSerializer( context, targetType, targetType, itemsSchema, traits );
				}

			}
		}

		public static MessagePackSerializer<T> CreateMapSerializer<T>(
			SerializationContext context,
			Type targetType,
			CollectionTraits traits,
			PolymorphismSchema itemsSchema
		)
		{
			return
			( MessagePackSerializer<T> )
				GenericSerializer.TryCreateAbstractCollectionSerializer( context, targetType, targetType, itemsSchema, traits );
		}

		public static void GetMetadata(
			IList<SerializingMember> members,
			SerializationContext context,
			Type targetType,
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
					serializers[ i ] = context.GetSerializer( memberType, PolymorphismSchema.Create( context, memberType, member ) );
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

		private static readonly Type[] ConstructorWithCapacityParameters = { typeof( int ) };

		public static Func<int,T> CreateCollectionInstanceFactory<T>()
		{
			var constructorWithCapacity =typeof(T).GetConstructor( ConstructorWithCapacityParameters );
			if ( constructorWithCapacity != null )
			{
				return capacity => ( T ) constructorWithCapacity.Invoke( new object[] { capacity } );
			}

			var constructorWithoutCapacity= typeof(T).GetConstructor( ReflectionAbstractions.EmptyTypes );
			if ( constructorWithoutCapacity == null )
			{
				throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity( typeof( T ) );
			}

			return _ => ( T ) constructorWithoutCapacity.Invoke( null );
		}
	}
}