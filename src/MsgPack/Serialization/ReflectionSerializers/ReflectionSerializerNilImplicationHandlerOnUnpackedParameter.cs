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

namespace MsgPack.Serialization.ReflectionSerializers
{
	internal struct ReflectionSerializerNilImplicationHandlerOnUnpackedParameter : INilImplicationHandlerOnUnpackedParameter<Action<object>>
	{
		private readonly Type _itemType;
		public Type ItemType { get { return this._itemType; } }
		private readonly Action<object> _store;
		public Action<object> Store { get { return this._store; } }
		public readonly string MemberName;
		public readonly Type DeclaringType;

		public ReflectionSerializerNilImplicationHandlerOnUnpackedParameter( Type itemType, Action<object> store, string memberName, Type declaringType )
		{
			this._itemType = itemType;
			this._store = store;
			this.MemberName = memberName;
			this.DeclaringType = declaringType;
		}
	}
}