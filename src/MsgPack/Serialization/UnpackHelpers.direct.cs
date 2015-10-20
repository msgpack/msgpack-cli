 
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

using System;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif // !UNITY || MSGPACK_UNITY_FULL

namespace MsgPack.Serialization
{
	partial class UnpackHelpers
	{
		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableBoolean" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Boolean type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Boolean UnpackBooleanValue( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Boolean result;
				if ( !unpacker.ReadBoolean( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableBoolean" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Boolean type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Boolean? UnpackNullableBooleanValue( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Boolean? result;
				if ( !unpacker.ReadNullableBoolean( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableByte" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Byte type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Byte UnpackByteValue( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Byte result;
				if ( !unpacker.ReadByte( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableByte" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Byte type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Byte? UnpackNullableByteValue( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Byte? result;
				if ( !unpacker.ReadNullableByte( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt16" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int16 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Int16 UnpackInt16Value( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Int16 result;
				if ( !unpacker.ReadInt16( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt16" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int16 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Int16? UnpackNullableInt16Value( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Int16? result;
				if ( !unpacker.ReadNullableInt16( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt32" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int32 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Int32 UnpackInt32Value( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Int32 result;
				if ( !unpacker.ReadInt32( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt32" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int32 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Int32? UnpackNullableInt32Value( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Int32? result;
				if ( !unpacker.ReadNullableInt32( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt64" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int64 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Int64 UnpackInt64Value( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Int64 result;
				if ( !unpacker.ReadInt64( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableInt64" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Int64 type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Int64? UnpackNullableInt64Value( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Int64? result;
				if ( !unpacker.ReadNullableInt64( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableSByte" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack SByte type value from underlying stream.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static SByte UnpackSByteValue( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				SByte result;
				if ( !unpacker.ReadSByte( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableSByte" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack SByte type value from underlying stream.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static SByte? UnpackNullableSByteValue( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				SByte? result;
				if ( !unpacker.ReadNullableSByte( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt16" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt16 type value from underlying stream.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static UInt16 UnpackUInt16Value( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				UInt16 result;
				if ( !unpacker.ReadUInt16( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt16" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt16 type value from underlying stream.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static UInt16? UnpackNullableUInt16Value( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				UInt16? result;
				if ( !unpacker.ReadNullableUInt16( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt32" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt32 type value from underlying stream.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static UInt32 UnpackUInt32Value( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				UInt32 result;
				if ( !unpacker.ReadUInt32( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt32" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt32 type value from underlying stream.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static UInt32? UnpackNullableUInt32Value( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				UInt32? result;
				if ( !unpacker.ReadNullableUInt32( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt64" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt64 type value from underlying stream.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static UInt64 UnpackUInt64Value( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				UInt64 result;
				if ( !unpacker.ReadUInt64( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableUInt64" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack UInt64 type value from underlying stream.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static UInt64? UnpackNullableUInt64Value( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				UInt64? result;
				if ( !unpacker.ReadNullableUInt64( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableSingle" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Single type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Single UnpackSingleValue( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Single result;
				if ( !unpacker.ReadSingle( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableSingle" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Single type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Single? UnpackNullableSingleValue( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Single? result;
				if ( !unpacker.ReadNullableSingle( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableDouble" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Double type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Double UnpackDoubleValue( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Double result;
				if ( !unpacker.ReadDouble( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadNullableDouble" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack Double type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static Double? UnpackNullableDoubleValue( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				Double? result;
				if ( !unpacker.ReadNullableDouble( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}


		/// <summary>
		///		Invokes <see cref="Unpacker.ReadString" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack string type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static string UnpackStringValue( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				string result;
				if ( !unpacker.ReadString( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadBinary" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack byte array type value from underlying stream.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		public static byte[] UnpackBinaryValue( Unpacker unpacker, Type objectType, String memberName )
		{
			try
			{
				byte[] result;
				if ( !unpacker.ReadBinary( out result ) )
				{
					throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
				}

				return result;
			}
			catch ( MessageTypeException ex )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, ex );
			}
		}
	}
}
