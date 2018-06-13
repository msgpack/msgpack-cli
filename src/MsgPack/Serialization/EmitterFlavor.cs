#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2018 FUJIWARA, Yusuke
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
	///		Determines emitter strategy.
	/// </summary>
#if UNITY && DEBUG
	public
#else
	internal
#endif
	enum EmitterFlavor
	{
#if !SILVERLIGHT
		/// <summary>
		///		Caches serializers for the members (de)serialization.
		///		It is default.
		/// </summary>
		FieldBased,
#endif // SILVERLIGHT

#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !SILVERLIGHT
		/// <summary>
		///		Uses code DOM code generation to (de)serialization.
		///		It requires a long time but prevents runtime code generation at all.
		/// </summary>
		CodeDomBased,
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !SILVERLIGHT

		/// <summary>
		///		Uses reflection to (de)serialization.
		///		It requires additional resources but may work on most environment.
		/// </summary>
		ReflectionBased
	}
}
