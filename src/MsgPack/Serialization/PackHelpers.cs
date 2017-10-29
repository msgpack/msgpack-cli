#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015-2016 FUJIWARA, Yusuke
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

#if DEBUG
#define ASSERT
#endif // DEBUG


using System;
using System.Collections.Generic;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif //!UNITY || MSGPACK_UNITY_FULL
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

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
#if DEBUG
		[Obsolete( "Use overload with PackToArrayParameters." )]
#endif // DEBUG
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
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

			var parameter =
				new PackToArrayParameters<TObject>
				{
					Packer = packer,
					Target = target,
					Operations = operations
				};
			PackToArray( ref parameter );
		}

		/// <summary>
		///		Packs object to msgpack array.
		/// </summary>
		/// <typeparam name="TObject">The type of the packing object.</typeparam>
		/// <param name="parameter">The reference of <see cref="PackToArrayParameters{T}"/> object which represents parameters of this method.</param>
		/// <returns>The unpacked object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<see cref="PackToArrayParameters{T}.Packer"/> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="PackToArrayParameters{T}.Operations"/> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		public static void PackToArray<TObject>(
			ref PackToArrayParameters<TObject> parameter
		)
		{
			if ( parameter.Packer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Packer" );
			}

			if ( parameter.Operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Operations" );
			}

#if ASSERT
			Contract.Assert( parameter.Packer != null );
			Contract.Assert( parameter.Operations != null );
#endif // ASSERT

			parameter.Packer.PackArrayHeader( parameter.Operations.Count );
			foreach ( var operation in parameter.Operations )
			{
				operation( parameter.Packer, parameter.Target );
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Packs object to msgpack array asynchronously.
		/// </summary>
		/// <typeparam name="TObject">The type of the packing object.</typeparam>
		/// <param name="packer">The packer.</param>
		/// <param name="target">The object to be packed.</param>
		/// <param name="operations">
		///		Delegates each ones unpack single member in order.
		///		The 1st argument will be <paramref name="packer"/> and 2nd argument will be <paramref name="target"/>.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
		/// <returns>The unpacked object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="packer"/> is <c>null</c>.
		///		Or, <paramref name="operations"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
#if DEBUG
		[Obsolete( "Use overload with PackToArrayAsyncParameters." )]
#endif // DEBUG
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		public static Task PackToArrayAsync<TObject>(
			Packer packer,
			TObject target,
			IList<Func<Packer, TObject, CancellationToken, Task>> operations,
			CancellationToken cancellationToken
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

			var parameter =
				new PackToArrayAsyncParameters<TObject>
				{
					Packer = packer,
					Target = target,
					Operations = operations,
					CancellationToken = cancellationToken
				};
			return PackToArrayAsync( ref parameter );
		}

		/// <summary>
		///		Packs object to msgpack array asynchronously.
		/// </summary>
		/// <typeparam name="TObject">The type of the packing object.</typeparam>
		/// <param name="parameter">The reference of <see cref="PackToArrayAsyncParameters{T}"/> object which represents parameters of this method.</param>
		/// <returns>The unpacked object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<see cref="PackToArrayAsyncParameters{T}.Packer"/> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="PackToArrayAsyncParameters{T}.Operations"/> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static Task PackToArrayAsync<TObject>(
			ref PackToArrayAsyncParameters<TObject> parameter
		)
		{
			if ( parameter.Packer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Packer" );
			}

			if ( parameter.Operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Operations" );
			}

			return PackToArrayAsyncCore( parameter.Packer, parameter.Target, parameter.Operations, parameter.CancellationToken );
		}

		private static async Task PackToArrayAsyncCore<TObject>(
			Packer packer,
			TObject target,
			IList<Func<Packer, TObject, CancellationToken, Task>> operations,
			CancellationToken cancellationToken
		)
		{
#if ASSERT
			Contract.Assert( packer != null );
			Contract.Assert( operations != null );
#endif // ASSERT

			await packer.PackArrayHeaderAsync( operations.Count, cancellationToken ).ConfigureAwait( false );
			foreach ( var operation in operations )
			{
				await operation( packer, target, cancellationToken ).ConfigureAwait( false );
			}
		}

#endif // FEATURE_TAP

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
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="operations"/> is <c>null</c>.
		/// </exception>
#if DEBUG
		[Obsolete( "Use overload with keyTransformer and nullDetectors." )]
#endif // DEBUG
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design" )]
		public static void PackToMap<TObject>(
			Packer packer,
			TObject target,
			IDictionary<string, Action<Packer, TObject>> operations
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

			var parameter =
				new PackToMapParameters<TObject>
				{
					Packer = packer,
					Target = target,
					Operations = operations
				};
			PackToMap( ref parameter );
		}

#pragma warning disable 618
		/// <summary>
		///		Packs object to msgpack map.
		/// </summary>
		/// <typeparam name="TObject">The type of the packing object.</typeparam>
		/// <param name="parameter">The reference of <see cref="PackToMapParameters{T}"/> object which represents parameters of this method.</param>
		/// <exception cref="ArgumentNullException">
		///		<see cref="PackToMapParameters{T}.Packer"/> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<c>PackToMapAsyncParameters{T}.Operations</c> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated correctly.")]
		public static void PackToMap<TObject>(
			ref PackToMapParameters<TObject> parameter
		)
		{
			if ( parameter.Packer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Packer" );
			}

			if ( parameter.Operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Operations" );
			}

#if ASSERT
			Contract.Assert( parameter.Packer != null );
			Contract.Assert( parameter.Operations != null );
#endif // ASSERT

			if ( parameter.NullCheckers != null 
				&& parameter.SerializationContext != null && parameter.SerializationContext.DictionarySerlaizationOptions.OmitNullEntry )
			{
#if ASSERT
				Contract.Assert( !SerializerDebugging.UseLegacyNullMapEntryHandling );
#endif // ASSERT

				// Skipping causes the entries count header reducing, so count up null entries first.
				var nullCount = 0;
				foreach ( var nullChecker in parameter.NullCheckers )
				{
					if ( nullChecker.Value( parameter.Target ) )
					{
						nullCount++;
					}
				}

				parameter.Packer.PackMapHeader( parameter.Operations.Count - nullCount );
				foreach ( var operation in parameter.Operations )
				{
					Func<TObject, bool> nullChecker;
					if ( parameter.NullCheckers.TryGetValue( operation.Key, out nullChecker ) )
					{
						if ( nullChecker( parameter.Target ) )
						{
							continue;
						}
					}

					parameter.Packer.PackString( operation.Key );
					operation.Value( parameter.Packer, parameter.Target );
				}
			}
			else
			{
				parameter.Packer.PackMapHeader( parameter.Operations.Count );
				// Compatible path
				foreach ( var operation in parameter.Operations )
				{
					parameter.Packer.PackString( operation.Key );
					operation.Value( parameter.Packer, parameter.Target );
				}
			}
#pragma warning restore 618
		}

#if FEATURE_TAP

		/// <summary>
		///		Packs object to msgpack map asynchronously.
		/// </summary>
		/// <typeparam name="TObject">The type of the packing object.</typeparam>
		/// <param name="packer">The packer.</param>
		/// <param name="target">The object to be packed.</param>
		/// <param name="operations">
		///		Delegates table each ones unpack single member and their keys correspond to unpacking membmer names.
		///		The 1st argument will be <paramref name="packer"/> and 2nd argument will be <paramref name="target"/>.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="packer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="operations"/> is <c>null</c>.
		/// </exception>
#if DEBUG
		[Obsolete( "Use overload with PackToMapParameters." )]
#endif // DEBUG
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design" )]
		public static Task PackToMapAsync<TObject>(
			Packer packer,
			TObject target,
			IDictionary<string, Func<Packer, TObject, CancellationToken, Task>> operations,
			CancellationToken cancellationToken
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

			var parameter =
				new PackToMapAsyncParameters<TObject>
				{
					Packer = packer,
					Target = target,
					Operations = operations,
					CancellationToken = cancellationToken
				};
			return PackToMapAsync( ref parameter );
		}

#pragma warning disable 618
		/// <summary>
		///		Packs object to msgpack map asynchronously.
		/// </summary>
		/// <typeparam name="TObject">The type of the packing object.</typeparam>
		/// <param name="parameter">The reference of <see cref="PackToMapAsyncParameters{T}"/> object which represents parameters of this method.</param>
		/// <exception cref="ArgumentNullException">
		///		<see cref="PackToMapAsyncParameters{T}.Packer"/> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<see cref="PackToMapAsyncParameters{T}.Operations"/> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static Task PackToMapAsync<TObject>(
			ref PackToMapAsyncParameters<TObject> parameter
		)
		{
			if ( parameter.Packer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Packer" );
			}

			if ( parameter.Operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Operations" );
			}

			return PackToMapAsyncCore( parameter.SerializationContext, parameter.Packer, parameter.Target, parameter.Operations, parameter.NullCheckers, parameter.CancellationToken );
		}
#pragma warning restore 618

		private static async Task PackToMapAsyncCore<TObject>(
			SerializationContext serializationContext,
			Packer packer,
			TObject target,
			IDictionary<string, Func<Packer, TObject, CancellationToken, Task>> operations,
			IDictionary<string, Func<TObject, bool>> nullCheckers,
			CancellationToken cancellationToken
		)
		{
#if ASSERT
			Contract.Assert( packer != null );
			Contract.Assert( operations != null );
#endif // ASSERT

			if ( nullCheckers != null
				&& serializationContext != null && serializationContext.DictionarySerlaizationOptions.OmitNullEntry )
			{
#if ASSERT
				Contract.Assert( !SerializerDebugging.UseLegacyNullMapEntryHandling );
#endif // ASSERT
				// Skipping causes the entries count header reducing, so count up null entries first.
				var nullCount = 0;
				foreach ( var nullChecker in nullCheckers )
				{
					if ( nullChecker.Value( target ) )
					{
						nullCount++;
					}
				}

				await packer.PackMapHeaderAsync( operations.Count - nullCount, cancellationToken ).ConfigureAwait( false );
				foreach ( var operation in operations )
				{
					Func<TObject, bool> nullChecker;
					if ( nullCheckers.TryGetValue( operation.Key, out nullChecker ) )
					{
						if ( nullChecker( target ) )
						{
							continue;
						}
					}

					await packer.PackStringAsync( operation.Key, cancellationToken ).ConfigureAwait( false );
					await operation.Value( packer, target, cancellationToken ).ConfigureAwait( false );
				}
			}
			else
			{
				await packer.PackMapHeaderAsync( operations.Count, cancellationToken ).ConfigureAwait( false );
				foreach ( var operation in operations )
				{
					// Compat path
					await packer.PackStringAsync( operation.Key, cancellationToken ).ConfigureAwait( false );
					await operation.Value( packer, target, cancellationToken ).ConfigureAwait( false );
				}
			}
		}

#endif // FEATURE_TAP

	}
}
