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

namespace MsgPack.Serialization.ReflectionSerializers
{
#if !UNITY
	internal struct ReflectionSerializerNilImplicationHandlerParameter : INilImplicationHandlerParameter
#else
	internal sealed class ReflectionSerializerNilImplicationHandlerParameter : INilImplicationHandlerParameter
#endif
	{
		private readonly Type _itemType;

		public Type ItemType { get { return this._itemType; } }
		
		private readonly string _memberName;
		
		public string MemberName { get { return this._memberName; } }

		public ReflectionSerializerNilImplicationHandlerParameter( Type itemType, string memberName )
		{
			this._itemType = itemType;
			this._memberName = memberName;
		}
	}
}