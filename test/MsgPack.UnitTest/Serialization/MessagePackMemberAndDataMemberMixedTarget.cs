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
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	[DataContract]
	public class MessagePackMemberAndDataMemberMixedTarget
	{
		[MessagePackMember( 0 )]
		public int ShouldSerialized1;

		[DataMember( Order = 1 )]
		public int ShouldNotSerialized1;

		[MessagePackMember( 1 )]
		[DataMember( Order = 2 )]
		public int ShouldSerialized2;

		public int ShouldNotSerialized2;

		[MessagePackMember( 2 )]
#if !NETFX_CORE && !SILVERLIGHT
		[NonSerialized]
#endif
		public int ShouldSerialized3;
	}
}
