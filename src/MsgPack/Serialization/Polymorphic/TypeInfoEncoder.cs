#region -- License Terms --
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
#endregion -- License Terms --

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
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

		public static void Encode( Packer packer, Type type )
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
				type.Namespace.StartsWith( assemblyName.Name, StringComparison.Ordinal )
				? Elipsis + type.FullName.Substring( assemblyName.Name.Length )
				: type.FullName;
			var version = new byte[ 16 ];
			Buffer.BlockCopy( BitConverter.GetBytes( assemblyName.Version.Major ), 0, version, 0, 4 );
			Buffer.BlockCopy( BitConverter.GetBytes( assemblyName.Version.Minor ), 0, version, 4, 4 );
			Buffer.BlockCopy( BitConverter.GetBytes( assemblyName.Version.Build ), 0, version, 8, 4 );
			Buffer.BlockCopy( BitConverter.GetBytes( assemblyName.Version.Revision ), 0, version, 12, 4 );

			packer.PackArrayHeader( 5 )
				.Pack( compressedTypeName )
				.Pack( assemblyName.Name )
				.Pack( version )
				.Pack( assemblyName.GetCultureName() )
				.Pack( assemblyName.GetPublicKeyToken() );
		}

		public static Type Decode( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw new SerializationException( "Type info must be non-nil array." );
			}

			if ( unpacker.ItemsCount != 5 )
			{
				throw new SerializationException( "Components count of type info is not valid." );
			}

			string compressedTypeName;
			if ( !unpacker.ReadString( out compressedTypeName ) )
			{
				throw new SerializationException( "Failed to decode type name component." );
			}

			string assemblySimpleName;
			if ( !unpacker.ReadString( out assemblySimpleName ) )
			{
				throw new SerializationException( "Failed to decode assembly name component." );
			}

			byte[] version;
			if ( !unpacker.ReadBinary( out version ) )
			{
				throw new SerializationException( "Failed to decode version component." );
			}

			string culture;
			if ( !unpacker.ReadString( out culture ) )
			{
				throw new SerializationException( "Failed to decode culture component." );
			}

			byte[] publicKeyToken;
			if ( !unpacker.ReadBinary( out publicKeyToken ) )
			{
				throw new SerializationException( "Failed to decode public key token component." );
			}

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
						String.IsNullOrEmpty( culture ) ? "neutral" : culture,
						( publicKeyToken == null || publicKeyToken.Length == 0 ) ? "null" : Binary.ToHexString( publicKeyToken, false )
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
					compressedTypeName.StartsWith( Elipsis, StringComparison.Ordinal )
					? assemblySimpleName + compressedTypeName
					: compressedTypeName
#if !NETFX_CORE
					, throwOnError: true
#endif // !NETFX_CORE
				);
		}
	}
}