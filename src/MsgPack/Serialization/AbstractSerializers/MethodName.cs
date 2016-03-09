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

using System;

namespace MsgPack.Serialization.AbstractSerializers
{
	internal static class MethodName
	{
		// Normal Serializer abstract methods
		public const string PackToCore = "PackToCore";
		public const string UnpackFromCore = "UnpackFromCore";
		public const string UnpackToCore = "UnpackToCore"; // also collection serializer abstract.
#if FEATURE_TAP
		public const string PackToAsyncCore = "PackToAsyncCore";
		public const string UnpackFromAsyncCore = "UnpackFromAsyncCore";
		public const string UnpackToAsyncCore = "UnpackToAsyncCore";
#endif // FEATURE_TAP

		// Enum Serializer abstract methods
		public const string PackUnderlyingValueTo = "PackUnderlyingValueTo";
		public const string UnpackFromUnderlyingValue = "UnpackFromUnderlyingValue";
#if FEATURE_TAP
		public const string PackUnderlyingValueToAsync = "PackUnderlyingValueToAsync";
#endif // FEATURE_TAP

		// Collection Serializer abstract methods
		public const string CreateInstance = "CreateInstance";
		public const string AddItem = "AddItem";

		// private static methods for collection serializer
		public const string RestoreSchema = "RestoreSchema";

		// private methods for object/tuple

		public const string PackMemberPlaceHolder = "PackMemberPlaceHolder";

		public const string UnpackMemberPlaceHolder = "UnpackMemberPlaceHolder";

		public const string CreateObjectFromContext = "CreateInstanceFromContext";

		// private methods for collection
		public const string UnpackCollectionItem = "UnpackCollectionItem";

		public const string AppendUnpackedItem = "AppendUnpackedItem";

		// For tuple (prefix + Array)
		public const string PackToArray = "PackToArray";

		public const string UnpackFromArray = "UnpackFromArray";
	}
}
