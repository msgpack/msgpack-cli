// #region -- License Terms --
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
// #endregion -- License Terms --

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if !UNITY
using System.Diagnostics.Contracts;
#endif // !UNITY
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.Polymorphic
{
	/// <summary>
	/// 	Implements type info encoding for type embedding.
	/// </summary>
	internal static class TypeInfoEncoder
	{
		private const string Elipsis = ".";

		public static MessagePackObject[] Encode( Type type )
		{
			var assemblyName =
#if !SILVERLIGHT
				type.GetAssembly().GetName();
#else
				new AssemblyName( type.GetAssembly().FullName );
#endif // !SILVERLIGHT

			// Omit namespace prefix when it equals to declaring assembly simple name.
#if DEBUG && !UNITY
			Contract.Assert( type.Namespace != null, "type.Namespace != null" );
#endif // DEBUG && !UNITY

			var compressedTypeName =
				type.Namespace.StartsWith( assemblyName.Name )
				? Elipsis + type.FullName.Substring( assemblyName.Name.Length )
				: type.FullName;
			var version = new byte[ 16 ];
			Buffer.BlockCopy( BitConverter.GetBytes( assemblyName.Version.Major ), 0, version, 0, 4 );
			Buffer.BlockCopy( BitConverter.GetBytes( assemblyName.Version.Minor ), 0, version, 4, 4 );
			Buffer.BlockCopy( BitConverter.GetBytes( assemblyName.Version.Build ), 0, version, 8, 4 );
			Buffer.BlockCopy( BitConverter.GetBytes( assemblyName.Version.Revision ), 0, version, 12, 4 );
			return
				new MessagePackObject[]
				{
					compressedTypeName,
					assemblyName.Name,
					version,
					assemblyName.GetCultureName(),
					assemblyName.GetPublicKeyToken()
				};
		}

		public static Type Decode( MessagePackObject typeInfo )
		{
			if ( typeInfo.IsNil || !typeInfo.IsArray )
			{
				throw new SerializationException( "Type info must be non-nil array." );
			}

			IList<MessagePackObject> typeInfoComponents = typeInfo.AsList();
			if ( typeInfoComponents.Count != 5 )
			{
				throw new SerializationException( "Components count of type info is not valid." );
			}

			var compressedTypeName = DecodeTypeInfoComponent( typeInfoComponents[ 0 ], component => component.AsString(), "Failed to decode type name component." );
			var assemblySimpleName = DecodeTypeInfoComponent( typeInfoComponents[ 1 ], component => component.AsString(), "Failed to decode assembly name component." );
			var version = DecodeTypeInfoComponent( typeInfoComponents[ 2 ], component => component.AsBinary(), "Failed to decode version component." );
			var culture = DecodeTypeInfoComponent( typeInfoComponents[ 3 ], component => component.AsString(), "Failed to decode culture component." );
			var publicKeyToken = DecodeTypeInfoComponent( typeInfoComponents[ 4 ], component => component.AsBinary(), "Failed to decode type name component." );

#if !NETFX_CORE
			var assemblyName =
				new AssemblyName
				{
					Name = assemblySimpleName,
					Version =
						new Version(
							BitConverter.ToInt32( version, 0 ),
							BitConverter.ToInt32( version, 4 ),
							BitConverter.ToInt32( version, 8 ),
							BitConverter.ToInt32( version, 12 )
						),
					CultureInfo = 
						String.IsNullOrEmpty( culture ) 
						? null 
#if !WINDOWS_PHONE && !SILVERLIGHT
						: CultureInfo.GetCultureInfo( culture ),
#else
						: new CultureInfo( culture ),
#endif //  !WINDOWS_PHONE
				};
			assemblyName.SetPublicKeyToken( publicKeyToken );
#else
			var assemblyName = 
				new AssemblyName( 
					String.Format( 
						CultureInfo.InvariantCulture, 
						"{0},Version={1},Culture={2},PublicKeyToken={3}",
						assemblySimpleName,
						new Version(
							BitConverter.ToInt32( version, 0 ),
							BitConverter.ToInt32( version, 4 ),
							BitConverter.ToInt32( version, 8 ),
							BitConverter.ToInt32( version, 12 )
						),
						culture,
						Binary.ToHexString( publicKeyToken )
					)
				);
#endif // !NETFX_CORE

			return
				Assembly.Load(
					assemblyName
#if SILVERLIGHT
					.ToString()
#endif // SILVERLIGHT
				).GetType(
					compressedTypeName.StartsWith( Elipsis )
					? assemblySimpleName + compressedTypeName
					: compressedTypeName
#if !NETFX_CORE
					, throwOnError: true
#endif // !NETFX_CORE
				);
		}

		private static T DecodeTypeInfoComponent<T>( MessagePackObject component, Func<MessagePackObject, T> getter, string message )
		{
			try
			{
				return getter( component );
			}
			catch ( MessageTypeException ex )
			{
				throw new SerializationException( message, ex );
			}
		}
	}
}