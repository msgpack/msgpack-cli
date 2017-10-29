#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
	// This file is generated from UnpackHelpers.direct.tt file with T4.
	// Do not modify this cs file directly.

	partial class UnpackHelpers
	{
		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableBoolean" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Boolean type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Boolean UnpackBooleanValue( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Boolean result;
				if ( !unpacker.ReadBoolean( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Boolean ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableBoolean" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Boolean type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Boolean? UnpackNullableBooleanValue( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Boolean? result;
				if ( !unpacker.ReadNullableBoolean( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Boolean? ); // never reaches.
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableBoolean" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Boolean type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Boolean> UnpackBooleanValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Boolean> result =
					await unpacker.ReadBooleanAsync( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Boolean ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableBoolean" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Boolean type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Boolean?> UnpackNullableBooleanValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Boolean?> result =
					await unpacker.ReadNullableBooleanAsync( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Boolean? ); // never reaches.
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableByte" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Byte type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Byte UnpackByteValue( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Byte result;
				if ( !unpacker.ReadByte( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Byte ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableByte" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Byte type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Byte? UnpackNullableByteValue( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Byte? result;
				if ( !unpacker.ReadNullableByte( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Byte? ); // never reaches.
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableByte" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Byte type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Byte> UnpackByteValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Byte> result =
					await unpacker.ReadByteAsync( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Byte ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableByte" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Byte type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Byte?> UnpackNullableByteValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Byte?> result =
					await unpacker.ReadNullableByteAsync( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Byte? ); // never reaches.
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt16" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int16 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Int16 UnpackInt16Value( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Int16 result;
				if ( !unpacker.ReadInt16( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Int16 ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt16" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int16 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Int16? UnpackNullableInt16Value( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Int16? result;
				if ( !unpacker.ReadNullableInt16( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Int16? ); // never reaches.
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt16" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int16 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Int16> UnpackInt16ValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Int16> result =
					await unpacker.ReadInt16Async( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Int16 ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt16" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int16 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Int16?> UnpackNullableInt16ValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Int16?> result =
					await unpacker.ReadNullableInt16Async( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Int16? ); // never reaches.
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt32" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int32 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Int32 UnpackInt32Value( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Int32 result;
				if ( !unpacker.ReadInt32( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Int32 ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt32" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int32 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Int32? UnpackNullableInt32Value( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Int32? result;
				if ( !unpacker.ReadNullableInt32( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Int32? ); // never reaches.
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt32" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int32 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Int32> UnpackInt32ValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Int32> result =
					await unpacker.ReadInt32Async( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Int32 ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt32" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int32 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Int32?> UnpackNullableInt32ValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Int32?> result =
					await unpacker.ReadNullableInt32Async( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Int32? ); // never reaches.
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt64" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int64 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Int64 UnpackInt64Value( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Int64 result;
				if ( !unpacker.ReadInt64( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Int64 ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt64" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int64 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Int64? UnpackNullableInt64Value( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Int64? result;
				if ( !unpacker.ReadNullableInt64( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Int64? ); // never reaches.
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt64" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int64 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Int64> UnpackInt64ValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Int64> result =
					await unpacker.ReadInt64Async( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Int64 ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt64" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int64 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Int64?> UnpackNullableInt64ValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Int64?> result =
					await unpacker.ReadNullableInt64Async( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Int64? ); // never reaches.
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableSByte" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack SByte type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static SByte UnpackSByteValue( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				SByte result;
				if ( !unpacker.ReadSByte( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( SByte ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableSByte" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack SByte type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static SByte? UnpackNullableSByteValue( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				SByte? result;
				if ( !unpacker.ReadNullableSByte( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( SByte? ); // never reaches.
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableSByte" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack SByte type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< SByte> UnpackSByteValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<SByte> result =
					await unpacker.ReadSByteAsync( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( SByte ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableSByte" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack SByte type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< SByte?> UnpackNullableSByteValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<SByte?> result =
					await unpacker.ReadNullableSByteAsync( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( SByte? ); // never reaches.
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt16" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt16 type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static UInt16 UnpackUInt16Value( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				UInt16 result;
				if ( !unpacker.ReadUInt16( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( UInt16 ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt16" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt16 type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static UInt16? UnpackNullableUInt16Value( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				UInt16? result;
				if ( !unpacker.ReadNullableUInt16( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( UInt16? ); // never reaches.
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt16" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt16 type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< UInt16> UnpackUInt16ValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<UInt16> result =
					await unpacker.ReadUInt16Async( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( UInt16 ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt16" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt16 type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< UInt16?> UnpackNullableUInt16ValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<UInt16?> result =
					await unpacker.ReadNullableUInt16Async( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( UInt16? ); // never reaches.
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt32" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt32 type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static UInt32 UnpackUInt32Value( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				UInt32 result;
				if ( !unpacker.ReadUInt32( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( UInt32 ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt32" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt32 type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static UInt32? UnpackNullableUInt32Value( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				UInt32? result;
				if ( !unpacker.ReadNullableUInt32( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( UInt32? ); // never reaches.
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt32" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt32 type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< UInt32> UnpackUInt32ValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<UInt32> result =
					await unpacker.ReadUInt32Async( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( UInt32 ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt32" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt32 type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< UInt32?> UnpackNullableUInt32ValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<UInt32?> result =
					await unpacker.ReadNullableUInt32Async( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( UInt32? ); // never reaches.
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt64" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt64 type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static UInt64 UnpackUInt64Value( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				UInt64 result;
				if ( !unpacker.ReadUInt64( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( UInt64 ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt64" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt64 type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static UInt64? UnpackNullableUInt64Value( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				UInt64? result;
				if ( !unpacker.ReadNullableUInt64( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( UInt64? ); // never reaches.
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt64" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt64 type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< UInt64> UnpackUInt64ValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<UInt64> result =
					await unpacker.ReadUInt64Async( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( UInt64 ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt64" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt64 type value from underlying stream.
		/// </exception>
		[CLSCompliant( false )]
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< UInt64?> UnpackNullableUInt64ValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<UInt64?> result =
					await unpacker.ReadNullableUInt64Async( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( UInt64? ); // never reaches.
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableSingle" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Single type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Single UnpackSingleValue( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Single result;
				if ( !unpacker.ReadSingle( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Single ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableSingle" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Single type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Single? UnpackNullableSingleValue( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Single? result;
				if ( !unpacker.ReadNullableSingle( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Single? ); // never reaches.
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableSingle" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Single type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Single> UnpackSingleValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Single> result =
					await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Single ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableSingle" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Single type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Single?> UnpackNullableSingleValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Single?> result =
					await unpacker.ReadNullableSingleAsync( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Single? ); // never reaches.
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableDouble" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Double type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Double UnpackDoubleValue( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Double result;
				if ( !unpacker.ReadDouble( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Double ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableDouble" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Double type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static Double? UnpackNullableDoubleValue( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				Double? result;
				if ( !unpacker.ReadNullableDouble( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Double? ); // never reaches.
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableDouble" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Double type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Double> UnpackDoubleValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Double> result =
					await unpacker.ReadDoubleAsync( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Double ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableDouble" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Double type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< Double?> UnpackNullableDoubleValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<Double?> result =
					await unpacker.ReadNullableDoubleAsync( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( Double? ); // never reaches.
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadString" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack string type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static string UnpackStringValue( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				string result;
				if ( !unpacker.ReadString( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( string ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadBinary" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>
		///		An unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack byte array type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static byte[] UnpackBinaryValue( Unpacker unpacker, Type objectType, String memberName )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				byte[] result;
				if ( !unpacker.ReadBinary( out result ) )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( byte[] ); // never reaches.
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadString" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack string type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< string> UnpackStringValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<string> result =
					await unpacker.ReadStringAsync( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( string ); // never reaches.
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadBinary" /> and returns its result asyncronously.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains a value whether the operation was succeeded and
		///		an unpacked value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack byte array type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static async Task< byte[]> UnpackBinaryValueAsync( Unpacker unpacker, Type objectType, String memberName, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( objectType == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "objectType" );
			}

			if ( memberName == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "memberName" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( objectType != null );
			Contract.Assert( memberName != null );
#endif // ASSERT

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			try
			{
				AsyncReadResult<byte[]> result =
					await unpacker.ReadBinaryAsync( cancellationToken ).ConfigureAwait( false );
				if ( !result.Success )
				{
					SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, null );
				}

				Trace( ctx, "ReadDirect", unpacker, memberName );

				return result.Value;
			}
			catch ( MessageTypeException ex )
			{
				SerializationExceptions.ThrowFailedToDeserializeMember( objectType, memberName, ex );
				return default( byte[] ); // never reaches.
			}
		}

#endif // FEATURE_TAP

	}
}
