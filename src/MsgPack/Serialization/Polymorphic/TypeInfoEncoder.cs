#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015-2018 FUJIWARA, Yusuke
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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.Polymorphic
{
	/// <summary>
	/// 	Implements type info encoding for type embedding.
	/// </summary>
#if UNITY && DEBUG
	public
#else
	internal
#endif
	static class TypeInfoEncoder
	{
		private const string Elipsis = ".";

		public static void Encode( Packer packer, string typeCode )
		{
			packer.PackArrayHeader( 2 );
			packer.PackString( typeCode );
		}

#if FEATURE_TAP

		public static async Task EncodeAsync( Packer packer, string typeCode, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( 2, cancellationToken ).ConfigureAwait( false );
			await packer.PackStringAsync( typeCode, cancellationToken ).ConfigureAwait( false );
		}

#endif // FEATURE_TAP

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
				.PackString( assemblyName.GetCultureName() )
				.PackBinary( assemblyName.GetPublicKeyToken() );
		}

#if FEATURE_TAP

		public static async Task EncodeAsync( Packer packer, Type type, CancellationToken cancellationToken )
		{
			var assemblyName = type.GetAssembly().GetName();

			await packer.PackArrayHeaderAsync( 2, cancellationToken ).ConfigureAwait( false );
			await packer.PackArrayHeaderAsync( 6, cancellationToken ).ConfigureAwait( false );

			await packer.PackAsync( ( byte )TypeInfoEncoding.RawCompressed, cancellationToken ).ConfigureAwait( false );

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

			await packer.PackStringAsync( compressedTypeName, cancellationToken ).ConfigureAwait( false );
			await packer.PackStringAsync( assemblyName.Name, cancellationToken ).ConfigureAwait( false );
			await packer.PackBinaryAsync( version, cancellationToken ).ConfigureAwait( false );
			await packer.PackStringAsync( assemblyName.GetCultureName(), cancellationToken ).ConfigureAwait( false );
			await packer.PackBinaryAsync( assemblyName.GetPublicKeyToken(), cancellationToken ).ConfigureAwait( false );
		}
#endif // FEATURE_TAP

		public static T Decode<T>( Unpacker unpacker, Func<string, Type> typeFinder, Func<Type, Unpacker, T> unpacking )
		{
			if ( !unpacker.IsArrayHeader || UnpackHelpers.GetItemsCount( unpacker ) != 2 )
			{
				throw SerializationExceptions.NewUnknownTypeEmbedding();
			}

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				string typeCode;
				if ( !subTreeUnpacker.ReadString( out typeCode ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( subTreeUnpacker );
				}

				var type = typeFinder( typeCode );

				if ( !subTreeUnpacker.Read() )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( subTreeUnpacker );
				}

				return unpacking( type, subTreeUnpacker );
			}
		}

		public static T Decode<T>( Unpacker unpacker, Func<Unpacker, Type> typeDecoder, Func<Type, Unpacker, T> unpacking )
		{
			if ( !unpacker.IsArrayHeader || UnpackHelpers.GetItemsCount( unpacker ) != 2 )
			{
				throw SerializationExceptions.NewUnknownTypeEmbedding();
			}

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				if ( !subTreeUnpacker.Read() )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( subTreeUnpacker );
				}

				var type = typeDecoder( subTreeUnpacker );

				if ( !subTreeUnpacker.Read() )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( subTreeUnpacker );
				}

				return unpacking( type, subTreeUnpacker );
			}
		}

#if FEATURE_TAP

		public static async Task<T> DecodeAsync<T>( Unpacker unpacker, Func<string, Type> typeFinder, Func<Type, Unpacker, CancellationToken, Task<object>> unpackingAsync, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader || UnpackHelpers.GetItemsCount( unpacker ) != 2 )
			{
				throw SerializationExceptions.NewUnknownTypeEmbedding();
			}

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				var typeCode = await subTreeUnpacker.ReadStringAsync( cancellationToken ).ConfigureAwait( false );
				if ( !typeCode.Success )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( subTreeUnpacker );
				}

				var type = typeFinder( typeCode.Value );

				if ( !await subTreeUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( subTreeUnpacker );
				}

				return ( T )await unpackingAsync( type, subTreeUnpacker, cancellationToken ).ConfigureAwait( false );
			}
		}

		public static async Task<T> DecodeAsync<T>( Unpacker unpacker, Func<Unpacker, CancellationToken, Task<Type>> asyncTypeDecoder, Func<Type, Unpacker, CancellationToken, Task<object>> unpackingAsync, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader || UnpackHelpers.GetItemsCount( unpacker ) != 2 )
			{
				throw SerializationExceptions.NewUnknownTypeEmbedding();
			}

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				if ( !await subTreeUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( subTreeUnpacker );
				}

				var type = await asyncTypeDecoder( subTreeUnpacker, cancellationToken ).ConfigureAwait( false );

				if ( !await subTreeUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( subTreeUnpacker );
				}

				return ( T )await unpackingAsync( type, subTreeUnpacker, cancellationToken ).ConfigureAwait( false );
			}
		}

#endif // FEATURE_TAP

		public static Type DecodeRuntimeTypeInfo( Unpacker unpacker, Func<PolymorphicTypeVerificationContext, bool> typeVerifier )
		{
			CheckUnpackerForRuntimeTypeInfoDecoding( unpacker );

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				byte encodeType;
				if ( !subTreeUnpacker.ReadByte( out encodeType ) )
				{
					ThrowFailedToDecodeEncodingType();
				}

				if ( encodeType != ( byte )TypeInfoEncoding.RawCompressed )
				{
					ThrowUnknownEncodingType( encodeType );
				}

				string compressedTypeName;
				if ( !subTreeUnpacker.ReadString( out compressedTypeName ) )
				{
					ThrowFailedToDecodeCompressedTypeName();
				}

				string assemblySimpleName;
				if ( !subTreeUnpacker.ReadString( out assemblySimpleName ) )
				{
					ThrowFailedToDecodeAssemblySimpleName();
				}

				byte[] version;
				if ( !subTreeUnpacker.ReadBinary( out version ) )
				{
					ThrowFailedToDecodeAssemblyVersion();
				}

				string culture;
				if ( !subTreeUnpacker.ReadString( out culture ) )
				{
					ThrowFailedToDecodeAssemblyCulture();
				}

				byte[] publicKeyToken;
				if ( !subTreeUnpacker.ReadBinary( out publicKeyToken ) )
				{
					ThrowFailedToDecodeAssemblyKeyToken();
				}

				var assemblyName = BuildAssemblyName( assemblySimpleName, version, culture, publicKeyToken );
				var typeFullName = DecompressTypeName( assemblyName.Name, compressedTypeName );
				RuntimeTypeVerifier.Verify( assemblyName, typeFullName, typeVerifier );

				return LoadDecodedType( assemblyName, typeFullName );
			}
		}

		private static void ThrowFailedToDecodeEncodingType()
		{
			throw new SerializationException( "Failed to decode encode type component." );
		}

		private static void ThrowUnknownEncodingType( byte encodeType )
		{
			throw new SerializationException( String.Format( CultureInfo.InvariantCulture, "Unknown encoded type : {0}", encodeType ) );
		}

		private static void ThrowFailedToDecodeCompressedTypeName()
		{
			throw new SerializationException( "Failed to decode type name component." );
		}

		private static void ThrowFailedToDecodeAssemblySimpleName()
		{
			throw new SerializationException( "Failed to decode assembly name component." );
		}

		private static void ThrowFailedToDecodeAssemblyVersion()
		{
			throw new SerializationException( "Failed to decode version component." );
		}

		private static void ThrowFailedToDecodeAssemblyCulture()
		{
			throw new SerializationException( "Failed to decode culture component." );
		}

		private static void ThrowFailedToDecodeAssemblyKeyToken()
		{
			throw new SerializationException( "Failed to decode public key token component." );
		}

		private static void CheckUnpackerForRuntimeTypeInfoDecoding( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				ThrowEncodedTypeIsNotInNonNillArray();
			}

			if ( unpacker.ItemsCount != 6 )
			{
				ThrowEncodedTypeDoesNotHaveValidArrayItems();
			}
		}

		private static void ThrowEncodedTypeIsNotInNonNillArray()
		{
			throw new SerializationException( "Type info must be non-nil array." );
		}

		private static void ThrowEncodedTypeDoesNotHaveValidArrayItems()
		{
			throw new SerializationException( "Components count of type info is not valid." );
		}

		private static AssemblyName BuildAssemblyName( string assemblySimpleName, byte[] version, string culture, byte[] publicKeyToken )
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
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
#if !SILVERLIGHT
							: CultureInfo.GetCultureInfo( culture ),
#else
							: new CultureInfo( culture ),
#endif //  !SILVERLIGHT
				};
			assemblyName.SetPublicKeyToken( publicKeyToken );
			return assemblyName;
#else
			return
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
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
		}

		private static string DecompressTypeName( string assemblySimpleName, string compressedTypeName )
		{
			return
				compressedTypeName.StartsWith( Elipsis, StringComparison.Ordinal )
					? assemblySimpleName + compressedTypeName
					: compressedTypeName;
		}

		private static Type LoadDecodedType( AssemblyName assemblyName, string typeFullName )
		{
			return
				Assembly.Load(
					assemblyName
#if SILVERLIGHT
					.ToString()
#endif // SILVERLIGHT
				).GetType(
					typeFullName
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
					, throwOnError: true
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
				);
		}

#if FEATURE_TAP

		public static async Task<Type> DecodeRuntimeTypeInfoAsync( Unpacker unpacker, Func<PolymorphicTypeVerificationContext, bool> typeVerifier, CancellationToken cancellationToken )
		{
			CheckUnpackerForRuntimeTypeInfoDecoding( unpacker );

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				var encodeType = await subTreeUnpacker.ReadByteAsync( cancellationToken ).ConfigureAwait( false );
				if ( !encodeType.Success )
				{
					ThrowFailedToDecodeEncodingType();
				}

				if ( encodeType.Value != ( byte )TypeInfoEncoding.RawCompressed )
				{
					ThrowUnknownEncodingType( encodeType.Value );
				}

				var compressedTypeName = await subTreeUnpacker.ReadStringAsync( cancellationToken ).ConfigureAwait( false );
				if ( !compressedTypeName.Success )
				{
					ThrowFailedToDecodeCompressedTypeName();
				}

				var assemblySimpleName = await subTreeUnpacker.ReadStringAsync( cancellationToken ).ConfigureAwait( false );
				if ( !assemblySimpleName.Success )
				{
					ThrowFailedToDecodeAssemblySimpleName();
				}

				var version = await subTreeUnpacker.ReadBinaryAsync( cancellationToken ).ConfigureAwait( false );
				if ( !version.Success )
				{
					ThrowFailedToDecodeAssemblyVersion();
				}

				var culture = await subTreeUnpacker.ReadStringAsync( cancellationToken ).ConfigureAwait( false );
				if ( !culture.Success )
				{
					ThrowFailedToDecodeAssemblyCulture();
				}

				var publicKeyToken = await subTreeUnpacker.ReadBinaryAsync( cancellationToken ).ConfigureAwait( false );
				if ( !publicKeyToken.Success )
				{
					ThrowFailedToDecodeAssemblyKeyToken();
				}

				var assemblyName = BuildAssemblyName( assemblySimpleName.Value, version.Value, culture.Value, publicKeyToken.Value );
				var typeFullName = DecompressTypeName( assemblyName.Name, compressedTypeName.Value );
				RuntimeTypeVerifier.Verify( assemblyName, typeFullName, typeVerifier );
				return LoadDecodedType( assemblyName, typeFullName );
			}
		}

#endif // FEATURE_TAP
	}
}
