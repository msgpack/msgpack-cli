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
using System.Collections.Generic;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif //!UNITY || MSGPACK_UNITY_FULL
#if !UNITY
#if CORE_CLR
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR
#endif // !UNITY

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Defines serialization helper APIs.
	/// </summary>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	public static class PackHelpers
	{
		/// <summary>
		///		Packs object to msgpack array.
		/// </summary>
		/// <typeparam name="TObject">The type of the packing object.</typeparam>
		/// <param name="packer">The packer.</param>
		/// <param name="target">The object to be packed.</param>
		/// <param name="operations">
		///		Delegates each ones unpack single member in order.
		///		The 1st argument will be <paramref name="packer"/> and 2nd argument will be <paramref name="target"/>.
		/// </param>
		/// <returns>The unpacked object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="packer"/> is <c>null</c>.
		///		Or, <paramref name="operations"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static void PackToArray<TObject>(
			Packer packer,
			TObject target,
			IList<Action<Packer, TObject>> operations
			)
		{
			if ( packer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "packer" );
			}

			if ( operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "operations" );
			}

#if DEBUG && !UNITY
			Contract.Assert( packer != null );
			Contract.Assert( operations != null );
#endif // DEBUG && !UNITY

			packer.PackArrayHeader( operations.Count );
			foreach ( var operation in operations )
			{
				operation( packer, target );
			}
		}

		/// <summary>
		///		Packs object to msgpack map.
		/// </summary>
		/// <typeparam name="TObject">The type of the packing object.</typeparam>
		/// <param name="packer">The packer.</param>
		/// <param name="target">The object to be packed.</param>
		/// <param name="operations">
		///		Delegates table each ones unpack single member and their keys correspond to unpacking membmer names.
		///		The 1st argument will be <paramref name="packer"/> and 2nd argument will be <paramref name="target"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="packer"/> is <c>null</c>.
		///		Or, <paramref name="operations"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static void PackToMap<TObject>(
			Packer packer,
			TObject target,
			IDictionary<string, Action<Packer, TObject>> operations
		)
		{
			if ( packer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "operations" );
			}

#if DEBUG && !UNITY
			Contract.Assert( packer != null );
			Contract.Assert( operations != null );
#endif // DEBUG && !UNITY

			packer.PackMapHeader( operations.Count );
			foreach ( var operation in operations )
			{
				packer.PackString( operation.Key );
				operation.Value( packer, target );
			}
		}
	}
}