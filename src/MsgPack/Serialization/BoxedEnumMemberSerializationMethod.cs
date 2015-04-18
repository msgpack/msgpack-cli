#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Helpers to reduce boxing for <see cref="EnumMemberSerializationMethod"/>.
	/// </summary>
	internal static class BoxedEnumMemberSerializationMethod
	{
		public static object Default = EnumMemberSerializationMethod.Default;
		public static object ByName = EnumMemberSerializationMethod.ByName;
		public static object ByUnderlyingValue = EnumMemberSerializationMethod.ByUnderlyingValue;

		public static object Box( EnumMemberSerializationMethod value )
		{
			switch ( value )
			{
				case EnumMemberSerializationMethod.ByName:
				{
					return ByName;
				}
				case EnumMemberSerializationMethod.ByUnderlyingValue:
				{
					return ByUnderlyingValue;
				}
				default:
				{
					return Default;
				}
			}
		}
	}
}