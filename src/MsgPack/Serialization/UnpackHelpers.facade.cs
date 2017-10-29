 
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
#endif // !UNITY || MSGPACK_UNITY_FULL
#if ASSERT
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
#endif // ASSERT
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
		///		Unpacks the complex object from specified <see cref="Unpacker"/> with specified <see cref="MessagePackSerializer{T}"/>/
		/// </summary>
		/// <typeparam name="T">The type of unpacking value.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="serializer">The serializer to deserialize complex object.</param>
		/// <param name="unpacked">The current unpacked count for debugging.</param>
		/// <returns>
		///		A value read from current stream.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="serializer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="unpacked"/> is negative number.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		public static T UnpackComplexObject<T>( 
			Unpacker unpacker, MessagePackSerializer<T> serializer, int unpacked 
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( serializer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "serializer" );
			}

			if ( unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "unpacked" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
			Contract.Assert( unpacked >= 0 );
#endif // ASSERT
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
				using ( var subtreeUnpacker = unpacker.ReadSubtree() )
				{
					return  serializer.UnpackFrom( subtreeUnpacker );
				}
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks the complex object from specified <see cref="Unpacker"/> with specified <see cref="MessagePackSerializer{T}"/> asyncronously/
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
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="serializer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="unpacked"/> is negative number.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		public static async Task<T> UnpackComplexObjectAsync<T>( 
			Unpacker unpacker, MessagePackSerializer<T> serializer, int unpacked, CancellationToken cancellationToken 
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( serializer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "serializer" );
			}

			if ( unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "unpacked" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
			Contract.Assert( unpacked >= 0 );
#endif // ASSERT
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
				using ( var subtreeUnpacker = unpacker.ReadSubtree() )
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
		/// <param name="directRead">The delegate which refers direct reading. This parameter should be <c>null</c> when <paramref name="serializer" /> is specified.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		///		Or, <paramref name="memberName"/> is <c>null</c>.
		///		Or, <paramref name="targetObjectType"/> is <c>null</c>.
		///		Or, <paramref name="setter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="itemsCount"/> is negative number.
		///		Or, <paramref name="unpacked"/> is negative number.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Both of <paramref name="directRead"/> and <paramref name="serializer" /> are <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "8", Justification = "False positive because never reached." )]
		public static void UnpackValueTypeValue<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName,
			Func<Unpacker, Type, string, TValue> directRead, Action<TContext, TValue> setter
		)
			where TValue : struct
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( context == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "context" );
			}

			if ( itemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "itemsCount" );
			}

			if ( unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "unpacked" );
			}

			if ( targetObjectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "targetObjectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

			if ( setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "setter" );
			}

			if ( serializer == null && directRead == null )
			{
				SerializationExceptions.ThrowArgumentException( "directRead", "directRead cannot be null if serializer argument is null." );
			}

			var parameter =
				new UnpackValueTypeValueParameters<TContext, TValue>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					Serializer = serializer,
					ItemsCount = itemsCount,
					Unpacked = unpacked,
					TargetObjectType = targetObjectType,
					MemberName = memberName,
					DirectRead = directRead,
					Setter = setter,
				};
			UnpackValueTypeValue( ref parameter );
		}

		/// <summary>
		///		Unpacks the value type value from MessagePack stream.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackValueTypeValueParameters{TContext, TValue}" /> object.</param>
		/// <exception cref="ArgumentNullException">
		///		<see cref="UnpackValueTypeValueParameters{TContext, TValue}.Unpacker" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackValueTypeValueParameters{TContext, TValue}.UnpackingContext" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackValueTypeValueParameters{TContext, TValue}.MemberName" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackValueTypeValueParameters{TContext, TValue}.TargetObjectType" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackValueTypeValueParameters{TContext, TValue}.Setter" /> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<see cref="UnpackValueTypeValueParameters{TContext, TValue}.ItemsCount" /> of <paramref name="parameter"/> is negative number.
		///		Or, <see cref="UnpackValueTypeValueParameters{TContext, TValue}.Unpacked" /> of <paramref name="parameter"/> is negative number.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Both of <see cref="UnpackValueTypeValueParameters{TContext, TValue}.DirectRead" /> 
		///		and <see cref="UnpackValueTypeValueParameters{TContext, TValue}.Serializer" /> of <paramref name="parameter"/> are <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static void UnpackValueTypeValue<TContext, TValue>(
			ref UnpackValueTypeValueParameters<TContext, TValue> parameter
		)
			where TValue : struct
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.UnpackingContext == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "UnpackingContext" );
			}

			if ( parameter.ItemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "ItemsCount" );
			}

			if ( parameter.Unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "Unpacked" );
			}

			if ( parameter.TargetObjectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "TargetObjectType" );
			}

			if ( parameter.MemberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "MemberName" );
			}

			if ( parameter.Setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Setter" );
			}

			if ( parameter.Serializer == null && parameter.DirectRead == null )
			{
				SerializationExceptions.ThrowArgumentException( "parameter", "DirectRead cannot be null if Serializer field is null." );
			}
			
			UnpackValueTypeValueCore(
				parameter.Unpacker,
				parameter.UnpackingContext,
				parameter.Serializer,
				parameter.ItemsCount,
				parameter.Unpacked,
				parameter.TargetObjectType,
				parameter.MemberName,
				parameter.DirectRead,
				parameter.Setter
			);
		}

		private static void UnpackValueTypeValueCore<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName,
			Func<Unpacker, Type, string, TValue> directRead, Action<TContext, TValue> setter
		)
			where TValue : struct
		{

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( targetObjectType != null );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
			Contract.Assert( serializer != null || directRead != null );
#endif // ASSERT

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
		/// <param name="directRead">The delegate which refers direct reading. This parameter should be <c>null</c> when <paramref name="serializer" /> is specified.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		///	<returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		///		Or, <paramref name="memberName"/> is <c>null</c>.
		///		Or, <paramref name="targetObjectType"/> is <c>null</c>.
		///		Or, <paramref name="setter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="itemsCount"/> is negative number.
		///		Or, <paramref name="unpacked"/> is negative number.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Both of <paramref name="directRead"/> and <paramref name="serializer" /> are <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "8", Justification = "False positive because never reached." )]
		public static Task UnpackValueTypeValueAsync<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName,
			Func<Unpacker, Type, string, CancellationToken, Task<TValue>> directRead, Action<TContext, TValue> setter, CancellationToken cancellationToken
		)
			where TValue : struct
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( context == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "context" );
			}

			if ( itemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "itemsCount" );
			}

			if ( unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "unpacked" );
			}

			if ( targetObjectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "targetObjectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

			if ( setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "setter" );
			}

			if ( serializer == null && directRead == null )
			{
				SerializationExceptions.ThrowArgumentException( "directRead", "directRead cannot be null if serializer argument is null." );
			}

			var parameter =
				new UnpackValueTypeValueAsyncParameters<TContext, TValue>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					Serializer = serializer,
					ItemsCount = itemsCount,
					Unpacked = unpacked,
					TargetObjectType = targetObjectType,
					MemberName = memberName,
					DirectRead = directRead,
					Setter = setter,
					CancellationToken = cancellationToken
				};
			return UnpackValueTypeValueAsync( ref parameter );
		}

		/// <summary>
		///		Unpacks the value type value from MessagePack stream asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackValueTypeValueAsyncParameters{TContext, TValue}" /> object.</param>
		///	<returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<see cref="UnpackValueTypeValueAsyncParameters{TContext, TValue}.Unpacker" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackValueTypeValueAsyncParameters{TContext, TValue}.UnpackingContext" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackValueTypeValueAsyncParameters{TContext, TValue}.MemberName" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackValueTypeValueAsyncParameters{TContext, TValue}.TargetObjectType" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackValueTypeValueAsyncParameters{TContext, TValue}.Setter" /> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<see cref="UnpackValueTypeValueAsyncParameters{TContext, TValue}.ItemsCount" /> of <paramref name="parameter"/> is negative number.
		///		Or, <see cref="UnpackValueTypeValueAsyncParameters{TContext, TValue}.Unpacked" /> of <paramref name="parameter"/> is negative number.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Both of <see cref="UnpackValueTypeValueAsyncParameters{TContext, TValue}.DirectRead" /> 
		///		and <see cref="UnpackValueTypeValueAsyncParameters{TContext, TValue}.Serializer" /> of <paramref name="parameter"/> are <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static Task UnpackValueTypeValueAsync<TContext, TValue>(
			ref UnpackValueTypeValueAsyncParameters<TContext, TValue> parameter
		)
			where TValue : struct
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.UnpackingContext == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "UnpackingContext" );
			}

			if ( parameter.ItemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "ItemsCount" );
			}

			if ( parameter.Unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "Unpacked" );
			}

			if ( parameter.TargetObjectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "TargetObjectType" );
			}

			if ( parameter.MemberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "MemberName" );
			}

			if ( parameter.Setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Setter" );
			}

			if ( parameter.Serializer == null && parameter.DirectRead == null )
			{
				SerializationExceptions.ThrowArgumentException( "parameter", "DirectRead cannot be null if Serializer field is null." );
			}
			
			return UnpackValueTypeValueAsyncCore(
				parameter.Unpacker,
				parameter.UnpackingContext,
				parameter.Serializer,
				parameter.ItemsCount,
				parameter.Unpacked,
				parameter.TargetObjectType,
				parameter.MemberName,
				parameter.DirectRead,
				parameter.Setter
				, parameter.CancellationToken
			);
		}

		private static async Task UnpackValueTypeValueAsyncCore<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName,
			Func<Unpacker, Type, string, CancellationToken, Task<TValue>> directRead, Action<TContext, TValue> setter, CancellationToken cancellationToken
		)
			where TValue : struct
		{

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( targetObjectType != null );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
			Contract.Assert( serializer != null || directRead != null );
#endif // ASSERT

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
		/// <param name="directRead">The delegate which refers direct reading. This parameter should be <c>null</c> when <paramref name="serializer" /> is specified.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		///		Or, <paramref name="memberName"/> is <c>null</c>.
		///		Or, <paramref name="targetObjectType"/> is <c>null</c>.
		///		Or, <paramref name="setter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="itemsCount"/> is negative number.
		///		Or, <paramref name="unpacked"/> is negative number.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Both of <paramref name="directRead"/> and <paramref name="serializer" /> are <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "9", Justification = "False positive because never reached." )]
		public static void UnpackReferenceTypeValue<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName,
			NilImplication nilImplication,
			Func<Unpacker, Type, string, TValue> directRead, Action<TContext, TValue> setter
		)
			where TValue : class
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( context == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "context" );
			}

			if ( itemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "itemsCount" );
			}

			if ( unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "unpacked" );
			}

			if ( targetObjectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "targetObjectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

			if ( setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "setter" );
			}

			if ( serializer == null && directRead == null )
			{
				SerializationExceptions.ThrowArgumentException( "directRead", "directRead cannot be null if serializer argument is null." );
			}

			var parameter =
				new UnpackReferenceTypeValueParameters<TContext, TValue>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					Serializer = serializer,
					ItemsCount = itemsCount,
					Unpacked = unpacked,
					TargetObjectType = targetObjectType,
					MemberName = memberName,
					NilImplication = nilImplication,
					DirectRead = directRead,
					Setter = setter,
				};
			UnpackReferenceTypeValue( ref parameter );
		}

		/// <summary>
		///		Unpacks the reference type value from MessagePack stream.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackReferenceTypeValueParameters{TContext, TValue}" /> object.</param>
		/// <exception cref="ArgumentNullException">
		///		<see cref="UnpackReferenceTypeValueParameters{TContext, TValue}.Unpacker" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackReferenceTypeValueParameters{TContext, TValue}.UnpackingContext" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackReferenceTypeValueParameters{TContext, TValue}.MemberName" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackReferenceTypeValueParameters{TContext, TValue}.TargetObjectType" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackReferenceTypeValueParameters{TContext, TValue}.Setter" /> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<see cref="UnpackReferenceTypeValueParameters{TContext, TValue}.ItemsCount" /> of <paramref name="parameter"/> is negative number.
		///		Or, <see cref="UnpackReferenceTypeValueParameters{TContext, TValue}.Unpacked" /> of <paramref name="parameter"/> is negative number.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Both of <see cref="UnpackReferenceTypeValueParameters{TContext, TValue}.DirectRead" /> 
		///		and <see cref="UnpackReferenceTypeValueParameters{TContext, TValue}.Serializer" /> of <paramref name="parameter"/> are <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static void UnpackReferenceTypeValue<TContext, TValue>(
			ref UnpackReferenceTypeValueParameters<TContext, TValue> parameter
		)
			where TValue : class
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.UnpackingContext == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "UnpackingContext" );
			}

			if ( parameter.ItemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "ItemsCount" );
			}

			if ( parameter.Unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "Unpacked" );
			}

			if ( parameter.TargetObjectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "TargetObjectType" );
			}

			if ( parameter.MemberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "MemberName" );
			}

			if ( parameter.Setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Setter" );
			}

			if ( parameter.Serializer == null && parameter.DirectRead == null )
			{
				SerializationExceptions.ThrowArgumentException( "parameter", "DirectRead cannot be null if Serializer field is null." );
			}
			
			UnpackReferenceTypeValueCore(
				parameter.Unpacker,
				parameter.UnpackingContext,
				parameter.Serializer,
				parameter.ItemsCount,
				parameter.Unpacked,
				parameter.TargetObjectType,
				parameter.MemberName,
				parameter.NilImplication,
				parameter.DirectRead,
				parameter.Setter
			);
		}

		private static void UnpackReferenceTypeValueCore<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName,
			NilImplication nilImplication,
			Func<Unpacker, Type, string, TValue> directRead, Action<TContext, TValue> setter
		)
			where TValue : class
		{

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( targetObjectType != null );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
			Contract.Assert( serializer != null || directRead != null );
#endif // ASSERT

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
		/// <param name="directRead">The delegate which refers direct reading. This parameter should be <c>null</c> when <paramref name="serializer" /> is specified.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		///	<returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		///		Or, <paramref name="memberName"/> is <c>null</c>.
		///		Or, <paramref name="targetObjectType"/> is <c>null</c>.
		///		Or, <paramref name="setter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="itemsCount"/> is negative number.
		///		Or, <paramref name="unpacked"/> is negative number.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Both of <paramref name="directRead"/> and <paramref name="serializer" /> are <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "9", Justification = "False positive because never reached." )]
		public static Task UnpackReferenceTypeValueAsync<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName,
			NilImplication nilImplication,
			Func<Unpacker, Type, string, CancellationToken, Task<TValue>> directRead, Action<TContext, TValue> setter, CancellationToken cancellationToken
		)
			where TValue : class
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( context == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "context" );
			}

			if ( itemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "itemsCount" );
			}

			if ( unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "unpacked" );
			}

			if ( targetObjectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "targetObjectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

			if ( setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "setter" );
			}

			if ( serializer == null && directRead == null )
			{
				SerializationExceptions.ThrowArgumentException( "directRead", "directRead cannot be null if serializer argument is null." );
			}

			var parameter =
				new UnpackReferenceTypeValueAsyncParameters<TContext, TValue>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					Serializer = serializer,
					ItemsCount = itemsCount,
					Unpacked = unpacked,
					TargetObjectType = targetObjectType,
					MemberName = memberName,
					NilImplication = nilImplication,
					DirectRead = directRead,
					Setter = setter,
					CancellationToken = cancellationToken
				};
			return UnpackReferenceTypeValueAsync( ref parameter );
		}

		/// <summary>
		///		Unpacks the reference type value from MessagePack stream asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackReferenceTypeValueAsyncParameters{TContext, TValue}" /> object.</param>
		///	<returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<see cref="UnpackReferenceTypeValueAsyncParameters{TContext, TValue}.Unpacker" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackReferenceTypeValueAsyncParameters{TContext, TValue}.UnpackingContext" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackReferenceTypeValueAsyncParameters{TContext, TValue}.MemberName" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackReferenceTypeValueAsyncParameters{TContext, TValue}.TargetObjectType" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackReferenceTypeValueAsyncParameters{TContext, TValue}.Setter" /> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<see cref="UnpackReferenceTypeValueAsyncParameters{TContext, TValue}.ItemsCount" /> of <paramref name="parameter"/> is negative number.
		///		Or, <see cref="UnpackReferenceTypeValueAsyncParameters{TContext, TValue}.Unpacked" /> of <paramref name="parameter"/> is negative number.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Both of <see cref="UnpackReferenceTypeValueAsyncParameters{TContext, TValue}.DirectRead" /> 
		///		and <see cref="UnpackReferenceTypeValueAsyncParameters{TContext, TValue}.Serializer" /> of <paramref name="parameter"/> are <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static Task UnpackReferenceTypeValueAsync<TContext, TValue>(
			ref UnpackReferenceTypeValueAsyncParameters<TContext, TValue> parameter
		)
			where TValue : class
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.UnpackingContext == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "UnpackingContext" );
			}

			if ( parameter.ItemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "ItemsCount" );
			}

			if ( parameter.Unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "Unpacked" );
			}

			if ( parameter.TargetObjectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "TargetObjectType" );
			}

			if ( parameter.MemberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "MemberName" );
			}

			if ( parameter.Setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Setter" );
			}

			if ( parameter.Serializer == null && parameter.DirectRead == null )
			{
				SerializationExceptions.ThrowArgumentException( "parameter", "DirectRead cannot be null if Serializer field is null." );
			}
			
			return UnpackReferenceTypeValueAsyncCore(
				parameter.Unpacker,
				parameter.UnpackingContext,
				parameter.Serializer,
				parameter.ItemsCount,
				parameter.Unpacked,
				parameter.TargetObjectType,
				parameter.MemberName,
				parameter.NilImplication,
				parameter.DirectRead,
				parameter.Setter
				, parameter.CancellationToken
			);
		}

		private static async Task UnpackReferenceTypeValueAsyncCore<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName,
			NilImplication nilImplication,
			Func<Unpacker, Type, string, CancellationToken, Task<TValue>> directRead, Action<TContext, TValue> setter, CancellationToken cancellationToken
		)
			where TValue : class
		{

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( targetObjectType != null );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
			Contract.Assert( serializer != null || directRead != null );
#endif // ASSERT

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
		/// <param name="directRead">The delegate which refers direct reading. This parameter should be <c>null</c> when <paramref name="serializer" /> is specified.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		///		Or, <paramref name="memberName"/> is <c>null</c>.
		///		Or, <paramref name="targetObjectType"/> is <c>null</c>.
		///		Or, <paramref name="setter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="itemsCount"/> is negative number.
		///		Or, <paramref name="unpacked"/> is negative number.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Both of <paramref name="directRead"/> and <paramref name="serializer" /> are <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "9", Justification = "False positive because never reached." )]
		public static void UnpackNullableTypeValue<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue?> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName,
			NilImplication nilImplication,
			Func<Unpacker, Type, string, TValue?> directRead, Action<TContext, TValue?> setter
		)
			where TValue : struct
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( context == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "context" );
			}

			if ( itemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "itemsCount" );
			}

			if ( unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "unpacked" );
			}

			if ( targetObjectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "targetObjectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

			if ( setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "setter" );
			}

			if ( serializer == null && directRead == null )
			{
				SerializationExceptions.ThrowArgumentException( "directRead", "directRead cannot be null if serializer argument is null." );
			}

			var parameter =
				new UnpackNullableTypeValueParameters<TContext, TValue>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					Serializer = serializer,
					ItemsCount = itemsCount,
					Unpacked = unpacked,
					TargetObjectType = targetObjectType,
					MemberName = memberName,
					NilImplication = nilImplication,
					DirectRead = directRead,
					Setter = setter,
				};
			UnpackNullableTypeValue( ref parameter );
		}

		/// <summary>
		///		Unpacks the nullable type value from MessagePack stream.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackNullableTypeValueParameters{TContext, TValue}" /> object.</param>
		/// <exception cref="ArgumentNullException">
		///		<see cref="UnpackNullableTypeValueParameters{TContext, TValue}.Unpacker" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackNullableTypeValueParameters{TContext, TValue}.UnpackingContext" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackNullableTypeValueParameters{TContext, TValue}.MemberName" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackNullableTypeValueParameters{TContext, TValue}.TargetObjectType" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackNullableTypeValueParameters{TContext, TValue}.Setter" /> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<see cref="UnpackNullableTypeValueParameters{TContext, TValue}.ItemsCount" /> of <paramref name="parameter"/> is negative number.
		///		Or, <see cref="UnpackNullableTypeValueParameters{TContext, TValue}.Unpacked" /> of <paramref name="parameter"/> is negative number.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Both of <see cref="UnpackNullableTypeValueParameters{TContext, TValue}.DirectRead" /> 
		///		and <see cref="UnpackNullableTypeValueParameters{TContext, TValue}.Serializer" /> of <paramref name="parameter"/> are <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static void UnpackNullableTypeValue<TContext, TValue>(
			ref UnpackNullableTypeValueParameters<TContext, TValue> parameter
		)
			where TValue : struct
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.UnpackingContext == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "UnpackingContext" );
			}

			if ( parameter.ItemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "ItemsCount" );
			}

			if ( parameter.Unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "Unpacked" );
			}

			if ( parameter.TargetObjectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "TargetObjectType" );
			}

			if ( parameter.MemberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "MemberName" );
			}

			if ( parameter.Setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Setter" );
			}

			if ( parameter.Serializer == null && parameter.DirectRead == null )
			{
				SerializationExceptions.ThrowArgumentException( "parameter", "DirectRead cannot be null if Serializer field is null." );
			}
			
			UnpackNullableTypeValueCore(
				parameter.Unpacker,
				parameter.UnpackingContext,
				parameter.Serializer,
				parameter.ItemsCount,
				parameter.Unpacked,
				parameter.TargetObjectType,
				parameter.MemberName,
				parameter.NilImplication,
				parameter.DirectRead,
				parameter.Setter
			);
		}

		private static void UnpackNullableTypeValueCore<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue?> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName,
			NilImplication nilImplication,
			Func<Unpacker, Type, string, TValue?> directRead, Action<TContext, TValue?> setter
		)
			where TValue : struct
		{

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( targetObjectType != null );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
			Contract.Assert( serializer != null || directRead != null );
#endif // ASSERT

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
		/// <param name="directRead">The delegate which refers direct reading. This parameter should be <c>null</c> when <paramref name="serializer" /> is specified.</param>
		/// <param name="setter">The delegate which takes <paramref name="context" /> and unpacked value, and then set the value to the context.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		///	<returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		///		Or, <paramref name="memberName"/> is <c>null</c>.
		///		Or, <paramref name="targetObjectType"/> is <c>null</c>.
		///		Or, <paramref name="setter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="itemsCount"/> is negative number.
		///		Or, <paramref name="unpacked"/> is negative number.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Both of <paramref name="directRead"/> and <paramref name="serializer" /> are <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "9", Justification = "False positive because never reached." )]
		public static Task UnpackNullableTypeValueAsync<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue?> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName,
			NilImplication nilImplication,
			Func<Unpacker, Type, string, CancellationToken, Task<TValue?>> directRead, Action<TContext, TValue?> setter, CancellationToken cancellationToken
		)
			where TValue : struct
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( context == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "context" );
			}

			if ( itemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "itemsCount" );
			}

			if ( unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "unpacked" );
			}

			if ( targetObjectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "targetObjectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

			if ( setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "setter" );
			}

			if ( serializer == null && directRead == null )
			{
				SerializationExceptions.ThrowArgumentException( "directRead", "directRead cannot be null if serializer argument is null." );
			}

			var parameter =
				new UnpackNullableTypeValueAsyncParameters<TContext, TValue>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					Serializer = serializer,
					ItemsCount = itemsCount,
					Unpacked = unpacked,
					TargetObjectType = targetObjectType,
					MemberName = memberName,
					NilImplication = nilImplication,
					DirectRead = directRead,
					Setter = setter,
					CancellationToken = cancellationToken
				};
			return UnpackNullableTypeValueAsync( ref parameter );
		}

		/// <summary>
		///		Unpacks the nullable type value from MessagePack stream asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackNullableTypeValueAsyncParameters{TContext, TValue}" /> object.</param>
		///	<returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<see cref="UnpackNullableTypeValueAsyncParameters{TContext, TValue}.Unpacker" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackNullableTypeValueAsyncParameters{TContext, TValue}.UnpackingContext" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackNullableTypeValueAsyncParameters{TContext, TValue}.MemberName" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackNullableTypeValueAsyncParameters{TContext, TValue}.TargetObjectType" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackNullableTypeValueAsyncParameters{TContext, TValue}.Setter" /> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<see cref="UnpackNullableTypeValueAsyncParameters{TContext, TValue}.ItemsCount" /> of <paramref name="parameter"/> is negative number.
		///		Or, <see cref="UnpackNullableTypeValueAsyncParameters{TContext, TValue}.Unpacked" /> of <paramref name="parameter"/> is negative number.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Both of <see cref="UnpackNullableTypeValueAsyncParameters{TContext, TValue}.DirectRead" /> 
		///		and <see cref="UnpackNullableTypeValueAsyncParameters{TContext, TValue}.Serializer" /> of <paramref name="parameter"/> are <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static Task UnpackNullableTypeValueAsync<TContext, TValue>(
			ref UnpackNullableTypeValueAsyncParameters<TContext, TValue> parameter
		)
			where TValue : struct
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.UnpackingContext == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "UnpackingContext" );
			}

			if ( parameter.ItemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "ItemsCount" );
			}

			if ( parameter.Unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "Unpacked" );
			}

			if ( parameter.TargetObjectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "TargetObjectType" );
			}

			if ( parameter.MemberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "MemberName" );
			}

			if ( parameter.Setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Setter" );
			}

			if ( parameter.Serializer == null && parameter.DirectRead == null )
			{
				SerializationExceptions.ThrowArgumentException( "parameter", "DirectRead cannot be null if Serializer field is null." );
			}
			
			return UnpackNullableTypeValueAsyncCore(
				parameter.Unpacker,
				parameter.UnpackingContext,
				parameter.Serializer,
				parameter.ItemsCount,
				parameter.Unpacked,
				parameter.TargetObjectType,
				parameter.MemberName,
				parameter.NilImplication,
				parameter.DirectRead,
				parameter.Setter
				, parameter.CancellationToken
			);
		}

		private static async Task UnpackNullableTypeValueAsyncCore<TContext, TValue>(
			Unpacker unpacker, TContext context, MessagePackSerializer<TValue?> serializer,
			int itemsCount, int unpacked,
			Type targetObjectType, string memberName,
			NilImplication nilImplication,
			Func<Unpacker, Type, string, CancellationToken, Task<TValue?>> directRead, Action<TContext, TValue?> setter, CancellationToken cancellationToken
		)
			where TValue : struct
		{

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( context != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( targetObjectType != null );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
			Contract.Assert( serializer != null || directRead != null );
#endif // ASSERT

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
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		///		Or, <paramref name="memberName"/> is <c>null</c>.
		///		Or, <paramref name="setter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="itemsCount"/> is negative number.
		///		Or, <paramref name="unpacked"/> is negative number.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "6", Justification = "False positive because never reached." )]
		public static void UnpackMessagePackObjectValueFromArray<TContext>(
			Unpacker unpacker, TContext context,
			int itemsCount, int unpacked,
			string memberName, NilImplication nilImplication,
			Action<TContext, MessagePackObject> setter
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( context == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "context" );
			}

			if ( itemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "itemsCount" );
			}

			if ( unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "unpacked" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

			if ( setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "setter" );
			}

			var parameter =
				new UnpackMessagePackObjectValueParameters<TContext>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					ItemsCount = itemsCount,
					Unpacked = unpacked,
					MemberName = memberName,
					Setter = setter,
					NilImplication = nilImplication,
				};
			UnpackMessagePackObjectValue( ref parameter );
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
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		///		Or, <paramref name="memberName"/> is <c>null</c>.
		///		Or, <paramref name="setter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="itemsCount"/> is negative number.
		///		Or, <paramref name="unpacked"/> is negative number.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "6", Justification = "False positive because never reached." )]
		public static Task UnpackMessagePackObjectValueFromArrayAsync<TContext>(
			Unpacker unpacker, TContext context,
			int itemsCount, int unpacked,
			string memberName, NilImplication nilImplication,
			Action<TContext, MessagePackObject> setter, CancellationToken cancellationToken
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( context == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "context" );
			}

			if ( itemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "itemsCount" );
			}

			if ( unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "unpacked" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

			if ( setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "setter" );
			}

			var parameter =
				new UnpackMessagePackObjectValueAsyncParameters<TContext>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					ItemsCount = itemsCount,
					Unpacked = unpacked,
					MemberName = memberName,
					Setter = setter,
					NilImplication = nilImplication,
					CancellationToken = cancellationToken
				};
			return UnpackMessagePackObjectValueAsync( ref parameter );
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
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		///		Or, <paramref name="memberName"/> is <c>null</c>.
		///		Or, <paramref name="setter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="itemsCount"/> is negative number.
		///		Or, <paramref name="unpacked"/> is negative number.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "6", Justification = "False positive because never reached." )]
		public static void UnpackMessagePackObjectValueFromMap<TContext>(
			Unpacker unpacker, TContext context,
			int itemsCount, int unpacked,
			string memberName, NilImplication nilImplication,
			Action<TContext, MessagePackObject> setter
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( context == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "context" );
			}

			if ( itemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "itemsCount" );
			}

			if ( unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "unpacked" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

			if ( setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "setter" );
			}

			var parameter =
				new UnpackMessagePackObjectValueParameters<TContext>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					ItemsCount = itemsCount,
					Unpacked = unpacked,
					MemberName = memberName,
					Setter = setter,
					NilImplication = nilImplication,
				};
			UnpackMessagePackObjectValue( ref parameter );
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
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		///		Or, <paramref name="memberName"/> is <c>null</c>.
		///		Or, <paramref name="setter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="itemsCount"/> is negative number.
		///		Or, <paramref name="unpacked"/> is negative number.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "6", Justification = "False positive because never reached." )]
		public static Task UnpackMessagePackObjectValueFromMapAsync<TContext>(
			Unpacker unpacker, TContext context,
			int itemsCount, int unpacked,
			string memberName, NilImplication nilImplication,
			Action<TContext, MessagePackObject> setter, CancellationToken cancellationToken
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( context == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "context" );
			}

			if ( itemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "itemsCount" );
			}

			if ( unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "unpacked" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

			if ( setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "setter" );
			}

			var parameter =
				new UnpackMessagePackObjectValueAsyncParameters<TContext>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					ItemsCount = itemsCount,
					Unpacked = unpacked,
					MemberName = memberName,
					Setter = setter,
					NilImplication = nilImplication,
					CancellationToken = cancellationToken
				};
			return UnpackMessagePackObjectValueAsync( ref parameter );
		}
#endif // FEATURE_TAP


		/// <summary>
		///		Unpacks the <see cref="MessagePackObject" /> value from MessagePack stream.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackMessagePackObjectValueParameters{T}" /> object.</param>
		/// <exception cref="ArgumentNullException">
		///		<see cref="UnpackMessagePackObjectValueParameters{TContext}.Unpacker" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackMessagePackObjectValueParameters{TContext}.UnpackingContext" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackMessagePackObjectValueParameters{TContext}.MemberName" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackMessagePackObjectValueParameters{TContext}.Setter" /> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<see cref="UnpackMessagePackObjectValueParameters{TContext}.ItemsCount" /> of <paramref name="parameter"/> is negative number.
		///		Or, <see cref="UnpackMessagePackObjectValueParameters{TContext}.Unpacked" /> of <paramref name="parameter"/> is negative number.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static void UnpackMessagePackObjectValue<TContext>(
			ref UnpackMessagePackObjectValueParameters<TContext> parameter
		)
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.UnpackingContext == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "UnpackingContext" );
			}

			if ( parameter.ItemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "ItemsCount" );
			}

			if ( parameter.Unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "Unpacked" );
			}

			if ( parameter.MemberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "MemberName" );
			}

			if ( parameter.Setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Setter" );
			}

			UnpackMessagePackObjectValueCore(
					parameter.Unpacker,
					parameter.UnpackingContext,
					parameter.ItemsCount,
					parameter.Unpacked,
					parameter.MemberName,
					parameter.NilImplication,
					parameter.Setter
				);
		}

		private static void UnpackMessagePackObjectValueCore<TContext>(
			Unpacker unpacker, TContext unpackingContext,
			int itemsCount, int unpacked,
			string memberName, NilImplication nilImplication,
			Action<TContext, MessagePackObject> setter
		)
		{
#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( unpackingContext != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
#endif // ASSERT

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

			setter( unpackingContext, nullable );
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks the <see cref="MessagePackObject" /> value from MessagePack stream asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context object which will store deserialized value.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackMessagePackObjectValueAsyncParameters{T}" /> object.</param>
		///	<returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<see cref="UnpackMessagePackObjectValueAsyncParameters{TContext}.Unpacker" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackMessagePackObjectValueAsyncParameters{TContext}.UnpackingContext" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackMessagePackObjectValueAsyncParameters{TContext}.MemberName" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackMessagePackObjectValueAsyncParameters{TContext}.Setter" /> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<see cref="UnpackMessagePackObjectValueAsyncParameters{TContext}.ItemsCount" /> of <paramref name="parameter"/> is negative number.
		///		Or, <see cref="UnpackMessagePackObjectValueAsyncParameters{TContext}.Unpacked" /> of <paramref name="parameter"/> is negative number.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static Task UnpackMessagePackObjectValueAsync<TContext>(
			ref UnpackMessagePackObjectValueAsyncParameters<TContext> parameter
		)
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.UnpackingContext == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "UnpackingContext" );
			}

			if ( parameter.ItemsCount < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "ItemsCount" );
			}

			if ( parameter.Unpacked < 0 )
			{
				SerializationExceptions.ThrowArgumentCannotBeNegativeException( "parameter", "Unpacked" );
			}

			if ( parameter.MemberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "MemberName" );
			}

			if ( parameter.Setter == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Setter" );
			}

			return UnpackMessagePackObjectValueAsyncCore(
					parameter.Unpacker,
					parameter.UnpackingContext,
					parameter.ItemsCount,
					parameter.Unpacked,
					parameter.MemberName,
					parameter.NilImplication,
					parameter.Setter
					, parameter.CancellationToken
				);
		}

		private static async Task UnpackMessagePackObjectValueAsyncCore<TContext>(
			Unpacker unpacker, TContext unpackingContext,
			int itemsCount, int unpacked,
			string memberName, NilImplication nilImplication,
			Action<TContext, MessagePackObject> setter, CancellationToken cancellationToken
		)
		{
#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( unpackingContext != null );
			Contract.Assert( itemsCount >= 0 );
			Contract.Assert( unpacked >= 0 );
			Contract.Assert( memberName != null );
			Contract.Assert( setter != null );
#endif // ASSERT

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

			setter( unpackingContext, nullable );
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
		/// <param name="itemNames">The names of the members for pretty exception message.</param>
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "itemNames", Justification = "For tracing." )]
		public static TResult UnpackFromArray<TContext, TResult>(
			Unpacker unpacker, TContext context,
			Func<TContext, TResult> factory, 
			IList<string> itemNames,
			IList<Action<Unpacker, TContext, int, int>> operations
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( factory == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "factory" );
			}

			if ( operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "operations" );
			}

			var parameter =
				new UnpackFromArrayParameters<TContext, TResult>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					Factory = factory,
					ItemNames = itemNames,
					Operations = operations,
				};
			return UnpackFromArray( ref parameter );
		}

		/// <summary>
		///		Unpacks object from msgpack array.
		/// </summary>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackFromArrayParameters{TContext, TResult}" /> object.</param>
		/// <returns>
		///		An unpacked object.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<see cref="UnpackFromArrayParameters{TContext, TResult}.Unpacker" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackFromArrayParameters{TContext, TResult}.Factory" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackFromArrayParameters{TContext, TResult}.Operations" /> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static TResult UnpackFromArray<TContext, TResult>(
			ref UnpackFromArrayParameters<TContext, TResult> parameter
		)
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.Factory == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Factory" );
			}

			if ( parameter.Operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Operations" );
			}

			return 
				UnpackFromArrayCore(
					parameter.Unpacker,
					parameter.UnpackingContext,
					parameter.Factory,
					parameter.ItemNames,
					parameter.Operations
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "itemNames", Justification = "For DEBUG build." )]
		private static TResult UnpackFromArrayCore<TContext, TResult>(
			Unpacker unpacker, TContext unpackingContext,
			Func<TContext, TResult> factory, 
			IList<string> itemNames,
			IList<Action<Unpacker, TContext, int, int>> operations
		)
		{
#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( factory != null );
			Contract.Assert( operations != null );
#endif // ASSERT

			var count = GetItemsCount( unpacker );

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			var limit = Math.Min( count, operations.Count );
			for ( var i = 0; i < limit; i++ )
			{
				operations[ i ]( unpacker, unpackingContext, i, count );
				Trace( ctx, "ReadItem", unpacker, i, itemNames );
			}

			if ( count > limit )
			{
				for ( var i = limit; i < count; i++ )
				{
					unpacker.Read();
				}
			}

			return factory( unpackingContext );
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
		/// <param name="itemNames">The names of the members for pretty exception message.</param>
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "itemNames", Justification = "For tracing." )]
		public static Task<TResult> UnpackFromArrayAsync<TContext, TResult>(
			Unpacker unpacker, TContext context,
			Func<TContext, TResult> factory, 
			IList<string> itemNames,
			IList<Func<Unpacker, TContext, int, int, CancellationToken, Task>> operations, CancellationToken cancellationToken
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( factory == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "factory" );
			}

			if ( operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "operations" );
			}

			var parameter =
				new UnpackFromArrayAsyncParameters<TContext, TResult>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					Factory = factory,
					ItemNames = itemNames,
					Operations = operations,
					CancellationToken = cancellationToken
				};
			return UnpackFromArrayAsync( ref parameter );
		}

		/// <summary>
		///		Unpacks object from msgpack array asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackFromArrayAsyncParameters{TContext, TResult}" /> object.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked object.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<see cref="UnpackFromArrayAsyncParameters{TContext, TResult}.Unpacker" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackFromArrayAsyncParameters{TContext, TResult}.Factory" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackFromArrayAsyncParameters{TContext, TResult}.Operations" /> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static Task<TResult> UnpackFromArrayAsync<TContext, TResult>(
			ref UnpackFromArrayAsyncParameters<TContext, TResult> parameter
		)
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.Factory == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Factory" );
			}

			if ( parameter.Operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Operations" );
			}

			return 
				UnpackFromArrayAsyncCore(
					parameter.Unpacker,
					parameter.UnpackingContext,
					parameter.Factory,
					parameter.ItemNames,
					parameter.Operations
					, parameter.CancellationToken
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "itemNames", Justification = "For DEBUG build." )]
		private static async Task<TResult> UnpackFromArrayAsyncCore<TContext, TResult>(
			Unpacker unpacker, TContext unpackingContext,
			Func<TContext, TResult> factory, 
			IList<string> itemNames,
			IList<Func<Unpacker, TContext, int, int, CancellationToken, Task>> operations, CancellationToken cancellationToken
		)
		{
#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( factory != null );
			Contract.Assert( operations != null );
#endif // ASSERT

			var count = GetItemsCount( unpacker );

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			var limit = Math.Min( count, operations.Count );
			for ( var i = 0; i < limit; i++ )
			{
				await operations[ i ]( unpacker, unpackingContext, i, count, cancellationToken ).ConfigureAwait( false );
				Trace( ctx, "ReadItem", unpacker, i, itemNames );
			}

			if ( count > limit )
			{
				for ( var i = limit; i < count; i++ )
				{
					await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false );
				}
			}

			return factory( unpackingContext );
		}

#endif // FEATURE_TAP


		/// <summary>
		///		Unpacks object from msgpack map.
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "False positive because never reached." )]
		public static TResult UnpackFromMap<TContext, TResult>(
			Unpacker unpacker, TContext context,
			Func<TContext, TResult> factory, 
			IDictionary<string, Action<Unpacker, TContext, int, int>> operations
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( factory == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "factory" );
			}

			if ( operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "operations" );
			}

			var parameter =
				new UnpackFromMapParameters<TContext, TResult>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					Factory = factory,
					Operations = operations,
				};
			return UnpackFromMap( ref parameter );
		}

		/// <summary>
		///		Unpacks object from msgpack map.
		/// </summary>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackFromMapParameters{TContext, TResult}" /> object.</param>
		/// <returns>
		///		An unpacked object.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<see cref="UnpackFromMapParameters{TContext, TResult}.Unpacker" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackFromMapParameters{TContext, TResult}.Factory" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackFromMapParameters{TContext, TResult}.Operations" /> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static TResult UnpackFromMap<TContext, TResult>(
			ref UnpackFromMapParameters<TContext, TResult> parameter
		)
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.Factory == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Factory" );
			}

			if ( parameter.Operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Operations" );
			}

			return 
				UnpackFromMapCore(
					parameter.Unpacker,
					parameter.UnpackingContext,
					parameter.Factory,
					parameter.Operations
				);
		}

		private static TResult UnpackFromMapCore<TContext, TResult>(
			Unpacker unpacker, TContext unpackingContext,
			Func<TContext, TResult> factory, 
			IDictionary<string, Action<Unpacker, TContext, int, int>> operations
		)
		{
#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( factory != null );
			Contract.Assert( operations != null );
#endif // ASSERT

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
					operation( unpacker, unpackingContext, i, count );
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

			return factory( unpackingContext );
		}

#if FEATURE_TAP

		/// <summary>
		///		Unpacks object from msgpack map asyncronously.
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "False positive because never reached." )]
		public static Task<TResult> UnpackFromMapAsync<TContext, TResult>(
			Unpacker unpacker, TContext context,
			Func<TContext, TResult> factory, 
			IDictionary<string, Func<Unpacker, TContext, int, int, CancellationToken, Task>> operations, CancellationToken cancellationToken
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( factory == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "factory" );
			}

			if ( operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "operations" );
			}

			var parameter =
				new UnpackFromMapAsyncParameters<TContext, TResult>
				{
					Unpacker = unpacker,
					UnpackingContext = context,
					Factory = factory,
					Operations = operations,
					CancellationToken = cancellationToken
				};
			return UnpackFromMapAsync( ref parameter );
		}

		/// <summary>
		///		Unpacks object from msgpack map asyncronously.
		/// </summary>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackFromMapAsyncParameters{TContext, TResult}" /> object.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked object.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<see cref="UnpackFromMapAsyncParameters{TContext, TResult}.Unpacker" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackFromMapAsyncParameters{TContext, TResult}.Factory" /> of <paramref name="parameter"/> is <c>null</c>.
		///		Or, <see cref="UnpackFromMapAsyncParameters{TContext, TResult}.Operations" /> of <paramref name="parameter"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static Task<TResult> UnpackFromMapAsync<TContext, TResult>(
			ref UnpackFromMapAsyncParameters<TContext, TResult> parameter
		)
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.Factory == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Factory" );
			}

			if ( parameter.Operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Operations" );
			}

			return 
				UnpackFromMapAsyncCore(
					parameter.Unpacker,
					parameter.UnpackingContext,
					parameter.Factory,
					parameter.Operations
					, parameter.CancellationToken
				);
		}

		private static async Task<TResult> UnpackFromMapAsyncCore<TContext, TResult>(
			Unpacker unpacker, TContext unpackingContext,
			Func<TContext, TResult> factory, 
			IDictionary<string, Func<Unpacker, TContext, int, int, CancellationToken, Task>> operations, CancellationToken cancellationToken
		)
		{
#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( factory != null );
			Contract.Assert( operations != null );
#endif // ASSERT

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
					await operation( unpacker, unpackingContext, i, count, cancellationToken ).ConfigureAwait( false );
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

			return factory( unpackingContext );
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "False positive because never reached." )]
		public static T UnpackCollection<T>(
			Unpacker unpacker, int itemsCount, T collection, Action<Unpacker, T, int> bulkOperation, Action<Unpacker, T, int, int> eachOperation
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "collection" );
			}

			var parameters =
				new UnpackCollectionParameters<T>
				{
					Unpacker = unpacker,
					ItemsCount = itemsCount,
					Collection = collection,
					BulkOperation = bulkOperation,
					EachOperation = eachOperation,
				};
			return UnpackCollection( ref parameters );
		}
		/// <summary>
		///		Unpacks the collection from MessagePack stream.
		/// </summary>
		/// <typeparam name="T">The type of the collection to be unpacked.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackCollectionParameters{T}" /> object.</param>
		/// <returns>
		///		An unpacked collection.
		/// </returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static T UnpackCollection<T>(
			ref UnpackCollectionParameters<T> parameter
		)
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.Collection == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Collection" );
			}

			return 
				UnpackCollectionCore(
					parameter.Unpacker,
					parameter.ItemsCount,
					parameter.Collection,
					parameter.BulkOperation,
					parameter.EachOperation
				);
		}

		private static T UnpackCollectionCore<T>(
			Unpacker unpacker, int itemsCount, T collection, Action<Unpacker, T, int> bulkOperation, Action<Unpacker, T, int, int> eachOperation
		)
		{
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
					SerializationExceptions.ThrowArgumentException( "eachOperation", "eachOperation cannot not be null when bulkOperation is null." );
				}

#if ASSERT
				Contract.Assert( eachOperation != null );
#endif // ASSERT

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
		///		Unpacks the collection from MessagePack stream asyncronously.
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "4", Justification = "False positive because never reached." )]
		public static Task<T> UnpackCollectionAsync<T>(
			Unpacker unpacker, int itemsCount, T collection, Func<Unpacker, T, int, CancellationToken, Task> bulkOperation, Func<Unpacker, T, int, int, CancellationToken, Task> eachOperation, CancellationToken cancellationToken
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "collection" );
			}

			var parameters =
				new UnpackCollectionAsyncParameters<T>
				{
					Unpacker = unpacker,
					ItemsCount = itemsCount,
					Collection = collection,
					BulkOperation = bulkOperation,
					EachOperation = eachOperation,
					CancellationToken = cancellationToken
				};
			return UnpackCollectionAsync( ref parameters );
		}
		/// <summary>
		///		Unpacks the collection from MessagePack stream asyncronously.
		/// </summary>
		/// <typeparam name="T">The type of the collection to be unpacked.</typeparam>
		/// <param name="parameter">The reference to <see cref="UnpackCollectionAsyncParameters{T}" /> object.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked collection.
		/// </returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Avoiding memcpy is critical here." )]
		public static Task<T> UnpackCollectionAsync<T>(
			ref UnpackCollectionAsyncParameters<T> parameter
		)
		{
			if ( parameter.Unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Unpacker" );
			}

			if ( parameter.Collection == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "parameter", "Collection" );
			}

			return 
				UnpackCollectionAsyncCore(
					parameter.Unpacker,
					parameter.ItemsCount,
					parameter.Collection,
					parameter.BulkOperation,
					parameter.EachOperation
					, parameter.CancellationToken
				);
		}

		private static async Task<T> UnpackCollectionAsyncCore<T>(
			Unpacker unpacker, int itemsCount, T collection, Func<Unpacker, T, int, CancellationToken, Task> bulkOperation, Func<Unpacker, T, int, int, CancellationToken, Task> eachOperation, CancellationToken cancellationToken
		)
		{
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
					SerializationExceptions.ThrowArgumentException( "eachOperation", "eachOperation cannot not be null when bulkOperation is null." );
				}

#if ASSERT
				Contract.Assert( eachOperation != null );
#endif // ASSERT

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
