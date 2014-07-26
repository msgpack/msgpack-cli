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
using System.Linq;
using System.Reflection;

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
				Activator.CreateInstance( typeof( ReflectionEnumMessagePackSerializer<> ).MakeGenericType( typeof( T ) ), context ) as
					MessagePackSerializer<T>;
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