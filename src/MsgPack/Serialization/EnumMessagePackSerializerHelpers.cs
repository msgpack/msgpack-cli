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
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif // !UNITY || MSGPACK_UNITY_FULL
#if NETFX_CORE
using System.Reflection;
#endif

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Helper methods for enum message pack serializer.
	/// </summary>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	public static class EnumMessagePackSerializerHelpers
	{
		/// <summary>
		///		Determines <see cref="EnumSerializationMethod"/> for the target.
		/// </summary>
		/// <param name="context">Context information.</param>
		/// <param name="enumType">The target enum type.</param>
		/// <param name="enumMemberSerializationMethod">The method argued by the member.</param>
		/// <returns>Determined <see cref="EnumSerializationMethod"/> for the target.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="context"/> is <c>null</c>.
		///		Or <paramref name="enumType"/> is <c>null</c>.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "enumType should be Type of Enum" )]
		public static EnumSerializationMethod DetermineEnumSerializationMethod(
			SerializationContext context,
			Type enumType,
			EnumMemberSerializationMethod enumMemberSerializationMethod 
		)
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			if ( enumType == null )
			{
				throw new ArgumentNullException( "enumType" );
			}

			EnumSerializationMethod method = context.EnumSerializationMethod;
			switch ( enumMemberSerializationMethod )
			{
				case EnumMemberSerializationMethod.ByName:
				{
					method = EnumSerializationMethod.ByName;
					break;
				}
				case EnumMemberSerializationMethod.ByUnderlyingValue:
				{
					method = EnumSerializationMethod.ByUnderlyingValue;
					break;
				}
				default:
				{
#if NETFX_CORE
					var messagePackEnumAttribute = 
						enumType.GetTypeInfo().GetCustomAttribute<MessagePackEnumAttribute>();
					if ( messagePackEnumAttribute != null)
					{
						method = messagePackEnumAttribute.SerializationMethod;
#else
					var messagePackEnumAttributes =
						enumType.GetCustomAttributes( typeof( MessagePackEnumAttribute ), true );
					if ( messagePackEnumAttributes.Length > 0 )
					{
						// ReSharper disable once PossibleNullReferenceException
						method = ( messagePackEnumAttributes[ 0 ] as MessagePackEnumAttribute ).SerializationMethod;
#endif // NETFX_CORE
					}

					break;
				}
			}

			return method;
		}
	}
}