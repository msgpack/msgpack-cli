#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents serializing member information.
	/// </summary>
#if !UNITY
	internal struct SerializingMember
#else
	internal sealed class SerializingMember
#endif // !UNITY
	{
		public readonly MemberInfo Member;
		public readonly DataMemberContract Contract;

#if UNITY
		public SerializingMember()
		{
			this.Member = null;
			this.Contract = new DataMemberContract ();
		}
#endif // UNITY

		public SerializingMember( MemberInfo member, DataMemberContract contract )
		{
			this.Member = member;
			this.Contract = contract;
		}

		public EnumMemberSerializationMethod GetEnumMemberSerializationMethod()
		{
#if NETFX_CORE
			var messagePackEnumMemberAttribute = 
				this.Member.GetCustomAttribute<MessagePackEnumMemberAttribute>();
			if ( messagePackEnumMemberAttribute != null)
			{
				var serializationMethod = messagePackEnumMemberAttribute.SerializationMethod;
#else
			var messagePackEnumMemberAttributes =
				this.Member.GetCustomAttributes( typeof( MessagePackEnumMemberAttribute ), true );
			if ( messagePackEnumMemberAttributes.Length > 0 )
			{
				var serializationMethod =
					// ReSharper disable once PossibleNullReferenceException
					( messagePackEnumMemberAttributes[ 0 ] as MessagePackEnumMemberAttribute ).SerializationMethod;
#endif // NETFX_CORE 
				switch ( serializationMethod )
				{
					case EnumMemberSerializationMethod.ByName:
					{
						return EnumMemberSerializationMethod.ByName;
					}
					case EnumMemberSerializationMethod.ByUnderlyingValue:
					{
						return EnumMemberSerializationMethod.ByUnderlyingValue;
					}
				}
			}

			return EnumMemberSerializationMethod.Default;
		}
	}
}
