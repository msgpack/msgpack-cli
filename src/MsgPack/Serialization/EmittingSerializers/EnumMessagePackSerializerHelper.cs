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

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Helper methods for enum message pack serializer.
	/// </summary>
	internal static class EnumMessagePackSerializerHelper
	{
		/// <summary>
		///		Determines <see cref="EnumSerializationMethod"/> for the target.
		/// </summary>
		/// <param name="context">Context information.</param>
		/// <param name="enumType">The target enum type.</param>
		/// <param name="enumMemberSerializationMethod">The method argued by the member.</param>
		/// <returns>Determined <see cref="EnumSerializationMethod"/> for the target.</returns>
		public static EnumSerializationMethod DetermineEnumSerializationMethod(
			SerializationContext context,
			Type enumType,
			EnumMemberSerializationMethod enumMemberSerializationMethod )
		{
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
					var attributesOnType = enumType.GetCustomAttributes( typeof( MessagePackEnumAttribute ), false );
					if ( attributesOnType.Length > 0 )
					{
						// ReSharper disable once PossibleNullReferenceException
						method = ( attributesOnType[ 0 ] as MessagePackEnumAttribute ).SerializationMethod;
					}

					break;
				}
			}

			return method;
		}
	}
}