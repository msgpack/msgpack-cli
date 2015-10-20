#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents options for custom or pre-generated serializer registration.
	/// </summary>
	[Flags]
	public enum SerializerRegistrationOptions
	{
		/// <summary>
		///		None of options are applied.
		/// </summary>
		None = 0,

		/// <summary>
		///		Overrides existing registration with specified serializer.
		/// </summary>
		AllowOverride = 0x1,

#if !UNITY
		// Unity causes AOT error for auto nullable registration...
		/// <summary>
		///		For non-nullable value type, registering nullable companion simulary.
		/// </summary>
		WithNullable = 0x2
#endif // !UNITY
	}
}