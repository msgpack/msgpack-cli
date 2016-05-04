
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
#if DEBUG && !NETFX_CORE
#define TRACING
#endif // DEBUG && !NETFX_CORE

using System;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif //!UNITY || MSGPACK_UNITY_FULL
using System.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Defines serialization helper reflection APIs.
	/// </summary>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	public static class ReflectionHelpers
	{
		/// <summary>
		///		Gets a specified <see cref="MethodInfo"/> even if the method is not publicly exposed.
		/// </summary>
		/// <param name="type">The type to be introspected.</param>
		/// <param name="name">The name of the method.</param>
		/// <param name="parameterTypes">The parameter types of the method.</param>
		/// <returns>A <see cref="MethodInfo"/> object.</returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static MethodInfo GetMethod( Type type, string name, Type[] parameterTypes )
		{
			return type.GetRuntimeMethod( name, parameterTypes );
		}

		/// <summary>
		///		Gets a specified <see cref="FieldInfo"/> even if the field is not publicly exposed.
		/// </summary>
		/// <param name="type">The type to be introspected.</param>
		/// <param name="name">The name of the method.</param>
		/// <returns>A <see cref="FieldInfo"/> object.</returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static FieldInfo GetField( Type type, string name )
		{
			return type.GetRuntimeField( name );
		}
	}
}