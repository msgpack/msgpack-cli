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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using MsgPack;
using MsgPack.Serialization;
using NUnit.Framework; // For running checking

namespace Samples
{
	/// <summary>
	///		A sample code to describe SerializationContext usage.
	/// </summary>
	[TestFixture]
	public class SerializationContextAndOptionsSample
	{
		[Test]
		public void CustomizeSerializeBehavior()
		{
			// 1. To take advantage of SerializationContext, you should create own context to isolate others.
			//    Note that SerializationContext is thread safe.
			//    As you imagine, you can change 'default' settings by modifying properties of SerializationContext.Default.
			var context = new SerializationContext();

			// 2. Set options.

			// 2-1. SerializationMethod: it changes comple type serialization method as array or map.
			//     Array(default): Space and time efficient, but depends on member declaration order so less version torrelant.
			//     Map : Less effitient, but more version torrelant (and easy to traverse as MesasgePackObject).
			context.SerializationMethod = SerializationMethod.Map;

			// 2-2. EnumSerializationMethod: it changes enum serialization as their name or underlying value.
			//    ByName(default): More version torrelant and interoperable, and backward compatible prior to 0.5 of MsgPack for CLI.
			//    ByUnderlyingValue: More efficient, but you should manage their underlying value and specify precise data contract between counterpart systems.
			context.EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue;

			// 2-3. If CompatibilityOptions.OneBoundDataMemberOrder is set, the origin DataMemberAttribute.Order becomes 1.
			//      It is compatibility options 1 base library like Proto-buf.NET.
			context.CompatibilityOptions.OneBoundDataMemberOrder = true;

			// 2-4. The CompatibilityOptions.PackerCompatibilityOptions control packer compatibility level.
			//      If you want to communicate with the library which only supports legacy message pack format spec, use PackerCompatibilityOptions.Classic flag set (default).
			//      If you want to utilize full feature including tiny string type, binary type, extended type, specify PackerCompatibilityOptions.None explicitly.
			context.CompatibilityOptions.PackerCompatibilityOptions = PackerCompatibilityOptions.None;

			// 2-5. You can tweak default concrete collection types for collection interfaces including IEnumerable<T>, IList, etc.
			context.DefaultCollectionTypes.Unregister( typeof( IList<> ) );
			context.DefaultCollectionTypes.Register( typeof( IList<> ), typeof( Collection<> ) );

			// 3. Get a serializer instance with customized settings.
			var serializer = MessagePackSerializer.Get<PhotoEntry>( context );

			// Following instructions are omitted... see sample 01.
		}
	}
}
