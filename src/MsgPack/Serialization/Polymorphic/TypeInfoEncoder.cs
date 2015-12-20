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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
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

		public static void Encode( Packer packer, string typeCode )
		{
			packer.PackArrayHeader( 2 );
			packer.PackString( typeCode );
		}

		public static void Encode( Packer packer, Type type )
		{
			var assemblyName =
#if !SILVERLIGHT
				type.GetAssembly().GetName();
#else
				new AssemblyName( type.GetAssembly().FullName );
#endif // !SILVERLIGHT

			packer.PackArrayHeader( 2 );
			packer.PackArrayHeader( 6 );

			packer.Pack( ( byte )TypeInfoEncoding.RawCompressed );

			// Omit namespace prefix when it equals to declaring assembly simple name.
			var compressedTypeName =
				( type.Namespace != null && type.Namespace.StartsWith( assemblyName.Name, StringComparison.Ordinal ) )
					? Elipsis + type.FullName.Substring( assemblyName.Name.Length + 1 )
					: type.FullName;
			var version = new byte[ 16 ];
			Buffer.BlockCopy( BitConverter.GetBytes( assemblyName.Version.Major ), 0, version, 0, 4 );
			Buffer.BlockCopy( BitConverter.GetBytes( assemblyName.Version.Minor ), 0, version, 4, 4 );
			Buffer.BlockCopy( BitConverter.GetBytes( assemblyName.Version.Build ), 0, version, 8, 4 );
			Buffer.BlockCopy( BitConverter.GetBytes( assemblyName.Version.Revision ), 0, version, 12, 4 );

			packer
				.PackString( compressedTypeName )
				.PackString( assemblyName.Name )
				.PackBinary( version )
#if !XAMIOS && !XAMDROID
				.PackString( assemblyName.GetCultureName() )
#else
				.PackString( assemblyName.GetCultureName() == "neutral" ? null : assemblyName.GetCultureName() )
#endif // !XAMIOS && !XAMDROID
				.PackBinary( assemblyName.GetPublicKeyToken() );
		}

		public static T Decode<T>( Unpacker unpacker, Func<Unpacker, Type> typeFinder, Func<Type, Unpacker, T> unpacking )
		{
			if ( !unpacker.IsArrayHeader || UnpackHelpers.GetItemsCount( unpacker ) != 2 )
			{
				throw SerializationExceptions.NewUnknownTypeEmbedding();
			}

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				if ( !subTreeUnpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				var type = typeFinder( subTreeUnpacker );

				if ( !subTreeUnpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				return unpacking( type, subTreeUnpacker );
			}
		}

		public static Type DecodeRuntimeTypeInfo( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw new SerializationException( "Type info must be non-nil array." );
			}

			if ( unpacker.ItemsCount != 6 )
			{
				throw new SerializationException( "Components count of type info is not valid." );
			}

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				byte encodeType;
				if ( !subTreeUnpacker.ReadByte( out encodeType ) )
				{
					throw new SerializationException( "Failed to decode encode type component." );
				}

				if ( encodeType != ( byte )TypeInfoEncoding.RawCompressed )
				{
					throw new SerializationException(
						String.Format( CultureInfo.InvariantCulture, "Unknown encoded type : {0}", encodeType )
					);
				}

				string compressedTypeName;
				if ( !subTreeUnpacker.ReadString( out compressedTypeName ) )
				{
					throw new SerializationException( "Failed to decode type name component." );
				}

				string assemblySimpleName;
				if ( !subTreeUnpacker.ReadString( out assemblySimpleName ) )
				{
					throw new SerializationException( "Failed to decode assembly name component." );
				}

				byte[] version;
				if ( !subTreeUnpacker.ReadBinary( out version ) )
				{
					throw new SerializationException( "Failed to decode version component." );
				}

				string culture;
				if ( !subTreeUnpacker.ReadString( out culture ) )
				{
					throw new SerializationException( "Failed to decode culture component." );
				}

				byte[] publicKeyToken;
				if ( !subTreeUnpacker.ReadBinary( out publicKeyToken ) )
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
}