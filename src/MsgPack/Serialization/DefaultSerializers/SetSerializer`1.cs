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
using System.Collections.Generic;
using System.Linq;

namespace MsgPack.Serialization.DefaultSerializers
{
#if !NETFX_35 && !UNITY_ANDROID && !UNITY_IPHONE
	/// <summary>
	///		Set interface serializer.
	/// </summary>
	/// <typeparam name="T">The type of the item of collection.</typeparam>
	internal sealed class SetSerializer<T> : EnumerableSerializerBase<ISet<T>, T>
	{
		public SetSerializer( SerializationContext ownerContext, Type targetType )
			: base( ownerContext, targetType ) { }

		protected override void PackArrayHeader( Packer packer, ISet<T> objectTree )
		{
			packer.PackArrayHeader( objectTree.Count );
		}
	}
#endif // !NETFX_35 && !UNITY_ANDROID && !UNITY_IPHONE
}