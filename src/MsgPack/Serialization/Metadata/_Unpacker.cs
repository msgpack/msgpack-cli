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
using System.Reflection;
#if FEATURE_TAP
using System.Threading;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.Metadata
{
	internal static partial class _Unpacker
	{
		public static readonly MethodInfo Read = typeof( Unpacker ).GetMethod( nameof( Unpacker.Read ), ReflectionAbstractions.EmptyTypes );
#if !NET35
		public static readonly PropertyInfo ItemsCount = typeof( Unpacker ).GetProperty( nameof( Unpacker.ItemsCount ) );
#endif // !NET35
		public static readonly PropertyInfo IsArrayHeader = typeof( Unpacker ).GetProperty( nameof( Unpacker.IsArrayHeader ) );
		public static readonly PropertyInfo IsMapHeader = typeof( Unpacker ).GetProperty( nameof( Unpacker.IsMapHeader ) );
#if FEATURE_TAP
		public static readonly MethodInfo ReadAsync = typeof( Unpacker ).GetMethod( nameof( Unpacker.ReadAsync ), new[] { typeof( CancellationToken ) } );
#endif // FEATURE_TAP
	}
}
