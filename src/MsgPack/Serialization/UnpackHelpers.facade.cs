 
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
#endif // !UNITY || MSGPACK_UNITY_FULL
#if DEBUG && !UNITY
#if CORE_CLR
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR
#endif // DEBUG && !UNITY
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization
{
	// This file is generated from UnpackHelpers.facade.tt file with T4.
	// Do not modify this cs file directly.

	partial class UnpackHelpers
	{
		/// <summary>
		///		Unpacks the complex object from specified <see cref="Unpacker"/> with specified <see cref="MessagePackSerializer{T}"/>L
		/// </summary>
		/// <typeparam name="T">The type of unpacking value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="serializer">The serializer to deserialize complex object.</param>
		/// <param name="unpacked">The current unpacked count for debugging.</param>
		/// <returns>
		///		A value read from current stream.
		/// </returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static T UnpackComplexObject<T>( Unpacker unpacker, MessagePackSerializer<T> serializer, int unpacked )
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
			Contract.Assert( unpacked >= 0 );
#endif // DEBUG && !UNITY
			if ( !unpacker.Read() )
			{
				SerializationExceptions.ThrowMissingItem( unpacked, unpacker );
			}

			if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
			{
				return serializer.UnpackFrom( unpacker );
			}
			else
			{
				using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
				{
					return  serializer.UnpackFrom( subtreeUnpacker );
				}
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks the complex object from specified <see cref="Unpacker"/> with specified <see cref="MessagePackSerializer{T}"/> asyncronouslyL
		/// </summary>
		/// <typeparam name="T">The type of unpacking value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="serializer">The serializer to deserialize complex object.</param>
		/// <param name="unpacked">The current unpacked count for debugging.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		a <typeparamref name="T" /> value read from current stream.
		/// </returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static async Task<T> UnpackComplexObjectAsync<T>( Unpacker unpacker, MessagePackSerializer<T> serializer, int unpacked, CancellationToken cancellationToken )
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
			Contract.Assert( unpacked >= 0 );
#endif // DEBUG && !UNITY
			if ( !( await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) ) )
			{
				SerializationExceptions.ThrowMissingItem( unpacked, unpacker );
			}

			if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
			{
				return await serializer.UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
				{
					return  await serializer.UnpackFromAsync( subtreeUnpacker, cancellationToken ).ConfigureAwait( false );
				}
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Unpacks the complex object from specified <see cref="Unpacker"/> with specified <see cref="MessagePackSerializer{T}"/>L
		/// </summary>
		/// <typeparam name="T">The type of unpacking value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="serializer">The serializer to deserialize complex object.</param>
		/// <param name="unpacked">The current unpacked count for debugging.</param>
		/// <param name="currentKey">The deserialized key of the map which should be name of the current item.</param>
		/// <returns>
		///		A value read from current stream.
		/// </returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static T UnpackComplexObject<T>( Unpacker unpacker, MessagePackSerializer<T> serializer, int unpacked, string currentKey )
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
			Contract.Assert( unpacked >= 0 );
#endif // DEBUG && !UNITY
			if ( !unpacker.Read() )
			{
				SerializationExceptions.ThrowMissingItem( unpacked, currentKey, unpacker );
			}

			if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
			{
				return serializer.UnpackFrom( unpacker );
			}
			else
			{
				using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
				{
					return  serializer.UnpackFrom( subtreeUnpacker );
				}
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks the complex object from specified <see cref="Unpacker"/> with specified <see cref="MessagePackSerializer{T}"/> asyncronouslyL
		/// </summary>
		/// <typeparam name="T">The type of unpacking value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="serializer">The serializer to deserialize complex object.</param>
		/// <param name="unpacked">The current unpacked count for debugging.</param>
		/// <param name="currentKey">The deserialized key of the map which should be name of the current item.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		a <typeparamref name="T" /> value read from current stream.
		/// </returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static async Task<T> UnpackComplexObjectAsync<T>( Unpacker unpacker, MessagePackSerializer<T> serializer, int unpacked, string currentKey, CancellationToken cancellationToken )
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
			Contract.Assert( unpacked >= 0 );
#endif // DEBUG && !UNITY
			if ( !( await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) ) )
			{
				SerializationExceptions.ThrowMissingItem( unpacked, currentKey, unpacker );
			}

			if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
			{
				return await serializer.UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
				{
					return  await serializer.UnpackFromAsync( subtreeUnpacker, cancellationToken ).ConfigureAwait( false );
				}
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Unpacks the value type value from MessagePack stream.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which will store deserialized value.</param>
		/// <param name="serializer">The serializer to deserialize complex object. This parameter should be <c>null</c> when <paramref name="directRead" /> is specified.</param>
		/// <param name="itemsCount">The items count to be unpacked.</param>
		/// <param name="unpacked">The unpacked items count.</param>
		/// <param name="targetObjectType">Type of the target object for debugging message.</param>
		/// <param name="memberName">Name of the member for debugging message.</param>
		/// <param name="nilImplication">The nil implication of current item.</param>
		/// <param name="directRead">The delegate which referes direct reading. This parameter should be <c>null</c> when <paramref name="serializer" /> is specified.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static void UnpackValueTypeValue<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName, NilImplication nilImplication,
			Func<Unpacker, Type, string, TValue> directRead, Action<TContext, TValue> setter
		)
			where TValue : struct
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( targetObjectType != null );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
			Contract.Assert( serializer != null || directRead != null );
#endif // DEBUG && !UNITY

			TValue? nullable;
			if ( unpacked < itemsCount )
			{
				nullable =
					( directRead != null
						? directRead( unpacker, targetObjectType, memberName )
						: UnpackComplexObject( unpacker, serializer, unpacked ) );
			}
			else
			{
				nullable = null;
			}

			if ( nullable == null )
			{
				SerializationExceptions.ThrowValueTypeCannotBeNull( memberName, typeof( TValue ), targetObjectType );
			}

			setter( context, nullable.GetValueOrDefault() );
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks the value type value from MessagePack stream asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which will store deserialized value.</param>
		/// <param name="serializer">The serializer to deserialize complex object. This parameter should be <c>null</c> when <paramref name="directRead" /> is specified.</param>
		/// <param name="itemsCount">The items count to be unpacked.</param>
		/// <param name="unpacked">The unpacked items count.</param>
		/// <param name="targetObjectType">Type of the target object for debugging message.</param>
		/// <param name="memberName">Name of the member for debugging message.</param>
		/// <param name="nilImplication">The nil implication of current item.</param>
		/// <param name="directRead">The delegate which referes direct reading. This parameter should be <c>null</c> when <paramref name="serializer" /> is specified.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		///	<returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static async Task UnpackValueTypeValueAsync<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName, NilImplication nilImplication,
			Func<Unpacker, Type, string, CancellationToken, Task<TValue>> directRead, Action<TContext, TValue> setter, CancellationToken cancellationToken
		)
			where TValue : struct
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( targetObjectType != null );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
			Contract.Assert( serializer != null || directRead != null );
#endif // DEBUG && !UNITY

			TValue? nullable;
			if ( unpacked < itemsCount )
			{
				nullable =
					await ( directRead != null
						? directRead( unpacker, targetObjectType, memberName, cancellationToken )
						: UnpackComplexObjectAsync( unpacker, serializer, unpacked, cancellationToken ) );
			}
			else
			{
				nullable = null;
			}

			if ( nullable == null )
			{
				SerializationExceptions.ThrowValueTypeCannotBeNull( memberName, typeof( TValue ), targetObjectType );
			}

			setter( context, nullable.GetValueOrDefault() );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Unpacks the reference type value from MessagePack stream.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which will store deserialized value.</param>
		/// <param name="serializer">The serializer to deserialize complex object. This parameter should be <c>null</c> when <paramref name="directRead" /> is specified.</param>
		/// <param name="itemsCount">The items count to be unpacked.</param>
		/// <param name="unpacked">The unpacked items count.</param>
		/// <param name="targetObjectType">Type of the target object for debugging message.</param>
		/// <param name="memberName">Name of the member for debugging message.</param>
		/// <param name="nilImplication">The nil implication of current item.</param>
		/// <param name="directRead">The delegate which referes direct reading. This parameter should be <c>null</c> when <paramref name="serializer" /> is specified.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static void UnpackReferenceTypeValue<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName, NilImplication nilImplication,
			Func<Unpacker, Type, string, TValue> directRead, Action<TContext, TValue> setter
		)
			where TValue : class
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( targetObjectType != null );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
			Contract.Assert( serializer != null || directRead != null );
#endif // DEBUG && !UNITY

			TValue nullable;
			if ( unpacked < itemsCount )
			{
				nullable =
					( directRead != null
						? directRead( unpacker, targetObjectType, memberName )
						: UnpackComplexObject( unpacker, serializer, unpacked ) );
			}
			else
			{
				nullable = null;
			}

			if ( nullable == null )
			{
				switch ( nilImplication )
				{
					case NilImplication.Prohibit:
					{
						SerializationExceptions.ThrowNullIsProhibited( memberName );
						break;
					}
					case NilImplication.MemberDefault:
					{
						return;
					}
				}
			}

			setter( context, nullable );
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks the reference type value from MessagePack stream asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which will store deserialized value.</param>
		/// <param name="serializer">The serializer to deserialize complex object. This parameter should be <c>null</c> when <paramref name="directRead" /> is specified.</param>
		/// <param name="itemsCount">The items count to be unpacked.</param>
		/// <param name="unpacked">The unpacked items count.</param>
		/// <param name="targetObjectType">Type of the target object for debugging message.</param>
		/// <param name="memberName">Name of the member for debugging message.</param>
		/// <param name="nilImplication">The nil implication of current item.</param>
		/// <param name="directRead">The delegate which referes direct reading. This parameter should be <c>null</c> when <paramref name="serializer" /> is specified.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		///	<returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static async Task UnpackReferenceTypeValueAsync<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName, NilImplication nilImplication,
			Func<Unpacker, Type, string, CancellationToken, Task<TValue>> directRead, Action<TContext, TValue> setter, CancellationToken cancellationToken
		)
			where TValue : class
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( targetObjectType != null );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
			Contract.Assert( serializer != null || directRead != null );
#endif // DEBUG && !UNITY

			TValue nullable;
			if ( unpacked < itemsCount )
			{
				nullable =
					await ( directRead != null
						? directRead( unpacker, targetObjectType, memberName, cancellationToken )
						: UnpackComplexObjectAsync( unpacker, serializer, unpacked, cancellationToken ) );
			}
			else
			{
				nullable = null;
			}

			if ( nullable == null )
			{
				switch ( nilImplication )
				{
					case NilImplication.Prohibit:
					{
						SerializationExceptions.ThrowNullIsProhibited( memberName );
						break;
					}
					case NilImplication.MemberDefault:
					{
						return;
					}
				}
			}

			setter( context, nullable );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Unpacks the nullable type value from MessagePack stream.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which will store deserialized value.</param>
		/// <param name="serializer">The serializer to deserialize complex object. This parameter should be <c>null</c> when <paramref name="directRead" /> is specified.</param>
		/// <param name="itemsCount">The items count to be unpacked.</param>
		/// <param name="unpacked">The unpacked items count.</param>
		/// <param name="targetObjectType">Type of the target object for debugging message.</param>
		/// <param name="memberName">Name of the member for debugging message.</param>
		/// <param name="nilImplication">The nil implication of current item.</param>
		/// <param name="directRead">The delegate which referes direct reading. This parameter should be <c>null</c> when <paramref name="serializer" /> is specified.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static void UnpackNullableTypeValue<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue?> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName, NilImplication nilImplication,
			Func<Unpacker, Type, string, TValue?> directRead, Action<TContext, TValue?> setter
		)
			where TValue : struct
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( targetObjectType != null );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
			Contract.Assert( serializer != null || directRead != null );
#endif // DEBUG && !UNITY

			TValue? nullable;
			if ( unpacked < itemsCount )
			{
				nullable =
					( directRead != null
						? directRead( unpacker, targetObjectType, memberName )
						: UnpackComplexObject( unpacker, serializer, unpacked ) );
			}
			else
			{
				nullable = null;
			}

			if ( nullable == null )
			{
				switch ( nilImplication )
				{
					case NilImplication.Prohibit:
					{
						SerializationExceptions.ThrowNullIsProhibited( memberName );
						break;
					}
					case NilImplication.MemberDefault:
					{
						return;
					}
				}
			}

			setter( context, nullable );
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks the nullable type value from MessagePack stream asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which will store deserialized value.</param>
		/// <param name="serializer">The serializer to deserialize complex object. This parameter should be <c>null</c> when <paramref name="directRead" /> is specified.</param>
		/// <param name="itemsCount">The items count to be unpacked.</param>
		/// <param name="unpacked">The unpacked items count.</param>
		/// <param name="targetObjectType">Type of the target object for debugging message.</param>
		/// <param name="memberName">Name of the member for debugging message.</param>
		/// <param name="nilImplication">The nil implication of current item.</param>
		/// <param name="directRead">The delegate which referes direct reading. This parameter should be <c>null</c> when <paramref name="serializer" /> is specified.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		///	<returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static async Task UnpackNullableTypeValueAsync<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue?> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName, NilImplication nilImplication,
			Func<Unpacker, Type, string, CancellationToken, Task<TValue?>> directRead, Action<TContext, TValue?> setter, CancellationToken cancellationToken
		)
			where TValue : struct
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( targetObjectType != null );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
			Contract.Assert( serializer != null || directRead != null );
#endif // DEBUG && !UNITY

			TValue? nullable;
			if ( unpacked < itemsCount )
			{
				nullable =
					await ( directRead != null
						? directRead( unpacker, targetObjectType, memberName, cancellationToken )
						: UnpackComplexObjectAsync( unpacker, serializer, unpacked, cancellationToken ) );
			}
			else
			{
				nullable = null;
			}

			if ( nullable == null )
			{
				switch ( nilImplication )
				{
					case NilImplication.Prohibit:
					{
						SerializationExceptions.ThrowNullIsProhibited( memberName );
						break;
					}
					case NilImplication.MemberDefault:
					{
						return;
					}
				}
			}

			setter( context, nullable );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Unpacks the <see cref="MessagePackObject" /> value from MessagePack array.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which will store deserialized value.</param>
		/// <param name="itemsCount">The items count to be unpacked.</param>
		/// <param name="unpacked">The unpacked items count.</param>
		/// <param name="memberName">Name of the member for debugging message.</param>
		/// <param name="nilImplication">The nil implication of current item.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static void UnpackMessagePackObjectValueFromArray<TContext>(
			Unpacker unpacker, TContext context,
			int itemsCount, int unpacked,
			string memberName, NilImplication nilImplication,
			Action<TContext, MessagePackObject> setter
		)
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
#endif // DEBUG && !UNITY

			MessagePackObject nullable;
			if ( unpacked < itemsCount )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( unpacked, unpacker );
				}

				nullable = unpacker.LastReadData;
			}
			else
			{
				nullable = MessagePackObject.Nil;
			}

			if ( nullable.IsNil )
			{
				switch ( nilImplication )
				{
					case NilImplication.Prohibit:
					{
						SerializationExceptions.ThrowNullIsProhibited( memberName );
						break;
					}
					case NilImplication.MemberDefault:
					{
						return;
					}
				}
			}

			setter( context, nullable );
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks the <see cref="MessagePackObject" /> value from MessagePack array asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which will store deserialized value.</param>
		/// <param name="itemsCount">The items count to be unpacked.</param>
		/// <param name="unpacked">The unpacked items count.</param>
		/// <param name="memberName">Name of the member for debugging message.</param>
		/// <param name="nilImplication">The nil implication of current item.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		///	<returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static async Task UnpackMessagePackObjectValueFromArrayAsync<TContext>(
			Unpacker unpacker, TContext context,
			int itemsCount, int unpacked,
			string memberName, NilImplication nilImplication,
			Action<TContext, MessagePackObject> setter, CancellationToken cancellationToken
		)
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
#endif // DEBUG && !UNITY

			MessagePackObject nullable;
			if ( unpacked < itemsCount )
			{
				if ( !( await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) ) )
				{
					SerializationExceptions.ThrowMissingItem( unpacked, unpacker );
				}

				nullable = unpacker.LastReadData;
			}
			else
			{
				nullable = MessagePackObject.Nil;
			}

			if ( nullable.IsNil )
			{
				switch ( nilImplication )
				{
					case NilImplication.Prohibit:
					{
						SerializationExceptions.ThrowNullIsProhibited( memberName );
						break;
					}
					case NilImplication.MemberDefault:
					{
						return;
					}
				}
			}

			setter( context, nullable );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Unpacks the <see cref="MessagePackObject" /> value from MessagePack map.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which will store deserialized value.</param>
		/// <param name="itemsCount">The items count to be unpacked.</param>
		/// <param name="unpacked">The unpacked items count.</param>
		/// <param name="memberName">Name of the member for debugging message.</param>
		/// <param name="nilImplication">The nil implication of current item.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static void UnpackMessagePackObjectValueFromMap<TContext>(
			Unpacker unpacker, TContext context,
			int itemsCount, int unpacked,
			string memberName, NilImplication nilImplication,
			Action<TContext, MessagePackObject> setter
		)
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
#endif // DEBUG && !UNITY

			MessagePackObject nullable;
			if ( unpacked < itemsCount )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( unpacked, memberName, unpacker );
				}

				nullable = unpacker.LastReadData;
			}
			else
			{
				nullable = MessagePackObject.Nil;
			}

			if ( nullable.IsNil )
			{
				switch ( nilImplication )
				{
					case NilImplication.Prohibit:
					{
						SerializationExceptions.ThrowNullIsProhibited( memberName );
						break;
					}
					case NilImplication.MemberDefault:
					{
						return;
					}
				}
			}

			setter( context, nullable );
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks the <see cref="MessagePackObject" /> value from MessagePack map asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which will store deserialized value.</param>
		/// <param name="itemsCount">The items count to be unpacked.</param>
		/// <param name="unpacked">The unpacked items count.</param>
		/// <param name="memberName">Name of the member for debugging message.</param>
		/// <param name="nilImplication">The nil implication of current item.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		///	<returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static async Task UnpackMessagePackObjectValueFromMapAsync<TContext>(
			Unpacker unpacker, TContext context,
			int itemsCount, int unpacked,
			string memberName, NilImplication nilImplication,
			Action<TContext, MessagePackObject> setter, CancellationToken cancellationToken
		)
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
#endif // DEBUG && !UNITY

			MessagePackObject nullable;
			if ( unpacked < itemsCount )
			{
				if ( !( await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) ) )
				{
					SerializationExceptions.ThrowMissingItem( unpacked, memberName, unpacker );
				}

				nullable = unpacker.LastReadData;
			}
			else
			{
				nullable = MessagePackObject.Nil;
			}

			if ( nullable.IsNil )
			{
				switch ( nilImplication )
				{
					case NilImplication.Prohibit:
					{
						SerializationExceptions.ThrowNullIsProhibited( memberName );
						break;
					}
					case NilImplication.MemberDefault:
					{
						return;
					}
				}
			}

			setter( context, nullable );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Unpacks object from msgpack array.
		/// </summary>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which holds intermediate states. This value may be <c>null</c> when the caller implementation allows it.</param>
		/// <param name="factory">A delegate to the factory method which creates the result from the context.</param>
		/// <param name="itemNames">The names of the membesr for pretty exception.</param>
		/// <param name="operations">
		///		Delegates each ones unpack single member in order.
		///		The 1st argument will be <paramref name="unpacker"/>, 2nd argument will be <paramref name="context"/>,
		///		and 3rd argument is index of current item.
		/// </param>
		/// <returns>
		///		An unpacked object.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="factory"/> is <c>null</c>.
		///		Or, <paramref name="operations"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static TResult UnpackFromArray<TContext, TResult>(
			Unpacker unpacker, TContext context,
			Func<TContext, TResult> factory, 
			IList<string> itemNames,
			IList<Action<Unpacker, TContext, int, int>> operations
		)
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( factory != null );
			Contract.Assert( operations != null );
#endif // DEBUG && !UNITY

			var count = GetItemsCount( unpacker );

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			var limit = Math.Min( count, operations.Count );
			for ( var i = 0; i < limit; i++ )
			{
				operations[ i ]( unpacker, context, i, count );
				Trace( ctx, "ReadItem", unpacker, i, itemNames );
			}

			if ( count > limit )
			{
				for ( var i = limit; i < count; i++ )
				{
					unpacker.Read();
				}
			}

			return factory( context );
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks object from msgpack array asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which holds intermediate states. This value may be <c>null</c> when the caller implementation allows it.</param>
		/// <param name="factory">A delegate to the factory method which creates the result from the context.</param>
		/// <param name="itemNames">The names of the membesr for pretty exception.</param>
		/// <param name="operations">
		///		Delegates each ones unpack single member in order.
		///		The 1st argument will be <paramref name="unpacker"/>, 2nd argument will be <paramref name="context"/>,
		///		and 3rd argument is index of current item.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked object.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="factory"/> is <c>null</c>.
		///		Or, <paramref name="operations"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static async Task<TResult> UnpackFromArrayAsync<TContext, TResult>(
			Unpacker unpacker, TContext context,
			Func<TContext, TResult> factory, 
			IList<string> itemNames,
			IList<Func<Unpacker, TContext, int, int, CancellationToken, Task>> operations, CancellationToken cancellationToken
		)
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( factory != null );
			Contract.Assert( operations != null );
#endif // DEBUG && !UNITY

			var count = GetItemsCount( unpacker );

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			var limit = Math.Min( count, operations.Count );
			for ( var i = 0; i < limit; i++ )
			{
				await operations[ i ]( unpacker, context, i, count, cancellationToken ).ConfigureAwait( false );
				Trace( ctx, "ReadItem", unpacker, i, itemNames );
			}

			if ( count > limit )
			{
				for ( var i = limit; i < count; i++ )
				{
					await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false );
				}
			}

			return factory( context );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Unpacks object from msgpack array.
		/// </summary>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which holds intermediate states. This value may be <c>null</c> when the caller implementation allows it.</param>
		/// <param name="factory">A delegate to the factory method which creates the result from the context.</param>
		/// <param name="operations">
		///		Delegates each ones unpack single member in order.
		///		The key of this dictionary must be member name.
		///		The 1st argument will be <paramref name="unpacker"/>, 2nd argument will be <paramref name="context"/>,
		///		and 3rd argument is index of current item.
		/// </param>
		/// <returns>
		///		An unpacked object.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="factory"/> is <c>null</c>.
		///		Or, <paramref name="operations"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static TResult UnpackFromMap<TContext, TResult>(
			Unpacker unpacker, TContext context,
			Func<TContext, TResult> factory, 
			IDictionary<string, Action<Unpacker, TContext, int, int>> operations
		)
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( factory != null );
			Contract.Assert( operations != null );
#endif // DEBUG && !UNITY

			var count = GetItemsCount( unpacker );

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			var limit = count;
			for ( var i = 0; i < limit; i++ )
			{
				var key = UnpackStringValue( unpacker, typeof( TResult ), "MemberName" );
				Trace( ctx, "ReadKey", unpacker, i, key );

				Action<Unpacker, TContext, int, int> operation;
				if ( key != null && operations.TryGetValue( key, out operation ) )
				{
					operation( unpacker, context, i, count );
					Trace( ctx, "ReadValue", unpacker, i, key );
				}
				else
				{
					// skip unknown item.
					unpacker.Skip();
					Trace( ctx, "Skip", unpacker, i, key ?? "(null)" );
				}
			}

			if ( count > limit )
			{
				for ( var i = limit; i < count; i++ )
				{
					unpacker.Read();
				}
			}

			return factory( context );
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks object from msgpack array asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which holds intermediate states. This value may be <c>null</c> when the caller implementation allows it.</param>
		/// <param name="factory">A delegate to the factory method which creates the result from the context.</param>
		/// <param name="operations">
		///		Delegates each ones unpack single member in order.
		///		The key of this dictionary must be member name.
		///		The 1st argument will be <paramref name="unpacker"/>, 2nd argument will be <paramref name="context"/>,
		///		and 3rd argument is index of current item.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked object.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="factory"/> is <c>null</c>.
		///		Or, <paramref name="operations"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static async Task<TResult> UnpackFromMapAsync<TContext, TResult>(
			Unpacker unpacker, TContext context,
			Func<TContext, TResult> factory, 
			IDictionary<string, Func<Unpacker, TContext, int, int, CancellationToken, Task>> operations, CancellationToken cancellationToken
		)
		{
#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( factory != null );
			Contract.Assert( operations != null );
#endif // DEBUG && !UNITY

			var count = GetItemsCount( unpacker );

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			var limit = count;
			for ( var i = 0; i < limit; i++ )
			{
				var key = await UnpackStringValueAsync( unpacker, typeof( TResult ), "MemberName", cancellationToken ).ConfigureAwait( false );
				Trace( ctx, "ReadKey", unpacker, i, key );

				Func<Unpacker, TContext, int, int, CancellationToken, Task> operation;
				if ( key != null && operations.TryGetValue( key, out operation ) )
				{
					await operation( unpacker, context, i, count, cancellationToken ).ConfigureAwait( false );
					Trace( ctx, "ReadValue", unpacker, i, key );
				}
				else
				{
					// skip unknown item.
					await unpacker.SkipAsync( cancellationToken ).ConfigureAwait( false );
					Trace( ctx, "Skip", unpacker, i, key ?? "(null)" );
				}
			}

			if ( count > limit )
			{
				for ( var i = limit; i < count; i++ )
				{
					await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false );
				}
			}

			return factory( context );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Unpacks the collection from MessagePack stream.
		/// </summary>
		/// <typeparam name="T">The type of the collection to be unpacked.</typeparam>
		/// <param name="unpacker">The unpacker where position is located at array or map header.</param>
		/// <param name="itemsCount">The collection count gotten from the <paramref name="unpacker"/>.</param>
		/// <param name="collection">The collection instance to be added unpacked items.</param>
		/// <param name="bulkOperation">
		///		A delegate to the bulk operation (typically UnpackToCore call). 
		///		The 1st argument will be <paramref name="unpacker"/>, 2nd argument will be <paramref name="collection"/>,
		///		and 3rd argument will be <paramref name="itemsCount"/>.
		///		If this parameter is <c>null</c>, <paramref name="eachOperation"/> will be used.
		/// </param>
		/// <param name="eachOperation">
		///		A delegate to the operation for each items, which typically unpack value and append it to the <paramref name="collection"/>.
		///		The 1st argument will be <paramref name="unpacker"/>, 2nd argument will be <paramref name="collection"/>,
		///		and 3rd argument will be index of the current item.
		///		If <paramref name="bulkOperation"/> parameter is not <c>null</c>, this parameter will be ignored.
		/// </param>
		/// <returns>
		///		An unpacked collection.
		/// </returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static T UnpackCollection<T>( Unpacker unpacker, int itemsCount, T collection, Action<Unpacker, T, int> bulkOperation, Action<Unpacker, T, int, int> eachOperation )
		{
			if ( collection == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "collection" );
			}

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			if ( bulkOperation != null )
			{
				bulkOperation( unpacker, collection, itemsCount );

				Trace( ctx, "UnpackTo", unpacker );
			}
			else
			{
				if ( eachOperation == null )
				{
					SerializationExceptions.ThrowArgumentException( "bulkOperation or eachOperation must not be null." );
				}

#if DEBUG && !UNITY
				Contract.Assert( eachOperation != null );
#endif // DEBUG && !UNITY

				for ( var i = 0; i < itemsCount; i++ )
				{
					eachOperation( unpacker, collection, i, itemsCount );
					Trace( ctx, "ReadItem", unpacker, i );
				}
			}

			return collection;
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks the collection from MessagePack stream.
		/// </summary>
		/// <typeparam name="T">The type of the collection to be unpacked.</typeparam>
		/// <param name="unpacker">The unpacker where position is located at array or map header.</param>
		/// <param name="itemsCount">The collection count gotten from the <paramref name="unpacker"/>.</param>
		/// <param name="collection">The collection instance to be added unpacked items.</param>
		/// <param name="bulkOperation">
		///		A delegate to the bulk operation (typically UnpackToCore call). 
		///		The 1st argument will be <paramref name="unpacker"/>, 2nd argument will be <paramref name="collection"/>,
		///		and 3rd argument will be <paramref name="itemsCount"/>.
		///		If this parameter is <c>null</c>, <paramref name="eachOperation"/> will be used.
		/// </param>
		/// <param name="eachOperation">
		///		A delegate to the operation for each items, which typically unpack value and append it to the <paramref name="collection"/>.
		///		The 1st argument will be <paramref name="unpacker"/>, 2nd argument will be <paramref name="collection"/>,
		///		and 3rd argument will be index of the current item.
		///		If <paramref name="bulkOperation"/> parameter is not <c>null</c>, this parameter will be ignored.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked collection.
		/// </returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static async Task<T> UnpackCollectionAsync<T>( Unpacker unpacker, int itemsCount, T collection, Func<Unpacker, T, int, CancellationToken, Task> bulkOperation, Func<Unpacker, T, int, int, CancellationToken, Task> eachOperation, CancellationToken cancellationToken )
		{
			if ( collection == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "collection" );
			}

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			if ( bulkOperation != null )
			{
				await bulkOperation( unpacker, collection, itemsCount, cancellationToken ).ConfigureAwait( false );

				Trace( ctx, "UnpackTo", unpacker );
			}
			else
			{
				if ( eachOperation == null )
				{
					SerializationExceptions.ThrowArgumentException( "bulkOperation or eachOperation must not be null." );
				}

#if DEBUG && !UNITY
				Contract.Assert( eachOperation != null );
#endif // DEBUG && !UNITY

				for ( var i = 0; i < itemsCount; i++ )
				{
					await eachOperation( unpacker, collection, i, itemsCount, cancellationToken ).ConfigureAwait( false );
					Trace( ctx, "ReadItem", unpacker, i );
				}
			}

			return collection;
		}

#endif // FEATURE_TAP

	}
}
