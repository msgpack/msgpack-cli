#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.Metadata
{
	// ReSharper disable once InconsistentNaming
	internal static class _MessagePackSerializer
	{
		// ReSharper disable InconsistentNaming
		public static readonly MethodInfo Create1_Method = typeof( MessagePackSerializer ).GetMethod( nameof( MessagePackSerializer.Create ), new[] { typeof( SerializationContext ) } );
		// ReSharper restore InconsistentNaming
		public static readonly PropertyInfo OwnerContext =
			typeof( MessagePackSerializer )
			// Use LINQ to get non public property in netstandard 1.x
			.GetRuntimeProperties()
			.Single( p => p.Name == nameof( MessagePackSerializer.OwnerContext ) );
	}
}
