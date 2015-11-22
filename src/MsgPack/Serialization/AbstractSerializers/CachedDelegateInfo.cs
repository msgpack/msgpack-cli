#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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
	/// <summary>
	///		Represents information of the cached delegate instance which should be stored in readonly instance field.
	/// </summary>
	internal struct CachedDelegateInfo
	{
		public readonly bool IsThisInstance;
		public readonly MethodDefinition TargetMethod;
		public readonly FieldDefinition BackingField;

		public CachedDelegateInfo( bool isThisInstance, MethodDefinition targetMethod, FieldDefinition backingField )
		{
			this.IsThisInstance = isThisInstance;
			this.TargetMethod = targetMethod;
			this.BackingField = backingField;
		}
	}
}