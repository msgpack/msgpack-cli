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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#define AOT
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

using MsgPack;

namespace MsgPack.Serialization
{
	partial class SerializerRepository 
	{
		private static readonly string AssemblyPublicKeyToken = Binary.ToHexString(typeof( SerializationContext ).GetAssembly().GetName().GetPublicKeyToken()) ?? "null";
		private static readonly string AssemblyVersion = typeof(SerializationContext).GetAssembly().GetName().Version.ToString();

		private static readonly Func<SerializationContext, IEnumerable<KeyValuePair<RuntimeTypeHandle, Object>>>[] BuiltInSerializerProviders =
			new []
			{
				"Extended",
				"LegacyCollection",
				"DbType",
				"Numeric",
				"Vector",
				"SecurityPrimitive"
			}.Select( name =>
				String.Format( CultureInfo.InvariantCulture, "MsgPack.{0}Serializers, Version={1}, Culture=neutral, PublicKeyToken={2}", name, AssemblyVersion, AssemblyPublicKeyToken )
			).Select( fullName => TryLoadAssembly( fullName ) )
			.Where( assembly => assembly != null )
			.SelectMany( assembly => assembly.DefinedTypes )
			.Select( typeInfo => typeInfo.AsType() )
			.Where( type => typeof( BuiltInSerializerProvider ).IsAssignableFrom( type ) && !type.GetIsAbstract() )
			.Select( type =>
				type.GetConstructor( ReflectionAbstractions.EmptyTypes )
#if !AOT
				.CreateConstructorDelegate<Func<BuiltInSerializerProvider>>()()
#else
				.InvokePreservingExceptionType() as BuiltInSerializerProvider
#endif // !AOT
			).Select( x => new Func<SerializationContext, IEnumerable<KeyValuePair<RuntimeTypeHandle, Object>>>( x.GetSerializers ) )
			.ToArray();

		private static Assembly TryLoadAssembly( string assemblyName )
		{
			try
			{
				return Assembly.Load( new AssemblyName( assemblyName ) );
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex);
				return null;
			}
		}

		// Should be 54.

	#if DEBUG
		internal
	#else
		private
	#endif // DEBUG
		static Dictionary<RuntimeTypeHandle, Object> InitializeDefaultTable( SerializationContext ownerContext )
		{
			return BuiltInSerializerProviders.SelectMany( factory => factory( ownerContext ) ).ToDictionary( kv => kv.Key, kv => kv.Value );
		}
	}
}
