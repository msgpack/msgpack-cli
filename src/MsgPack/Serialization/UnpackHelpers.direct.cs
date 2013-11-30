 
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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

using System;
using System.ComponentModel;

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
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static Boolean? UnpackBooleanValue( Unpacker unpacker, Type objectType, String memberName )
		{
			Boolean? result;
			if ( !unpacker.ReadNullableBoolean( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
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
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static Byte? UnpackByteValue( Unpacker unpacker, Type objectType, String memberName )
		{
			Byte? result;
			if ( !unpacker.ReadNullableByte( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
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
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static Int16? UnpackInt16Value( Unpacker unpacker, Type objectType, String memberName )
		{
			Int16? result;
			if ( !unpacker.ReadNullableInt16( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
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
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static Int32? UnpackInt32Value( Unpacker unpacker, Type objectType, String memberName )
		{
			Int32? result;
			if ( !unpacker.ReadNullableInt32( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
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
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static Int64? UnpackInt64Value( Unpacker unpacker, Type objectType, String memberName )
		{
			Int64? result;
			if ( !unpacker.ReadNullableInt64( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
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
		[CLSCompliant( false )]
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static SByte? UnpackSByteValue( Unpacker unpacker, Type objectType, String memberName )
		{
			SByte? result;
			if ( !unpacker.ReadNullableSByte( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
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
		[CLSCompliant( false )]
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static UInt16? UnpackUInt16Value( Unpacker unpacker, Type objectType, String memberName )
		{
			UInt16? result;
			if ( !unpacker.ReadNullableUInt16( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
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
		[CLSCompliant( false )]
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static UInt32? UnpackUInt32Value( Unpacker unpacker, Type objectType, String memberName )
		{
			UInt32? result;
			if ( !unpacker.ReadNullableUInt32( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
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
		[CLSCompliant( false )]
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static UInt64? UnpackUInt64Value( Unpacker unpacker, Type objectType, String memberName )
		{
			UInt64? result;
			if ( !unpacker.ReadNullableUInt64( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
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
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static Single? UnpackSingleValue( Unpacker unpacker, Type objectType, String memberName )
		{
			Single? result;
			if ( !unpacker.ReadNullableSingle( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
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
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static Double? UnpackDoubleValue( Unpacker unpacker, Type objectType, String memberName )
		{
			Double? result;
			if ( !unpacker.ReadNullableDouble( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
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
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static string UnpackStringValue( Unpacker unpacker, Type objectType, String memberName )
		{
			string result;
			if ( !unpacker.ReadString( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
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
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static byte[] UnpackBinaryValue( Unpacker unpacker, Type objectType, String memberName )
		{
			byte[] result;
			if ( !unpacker.ReadBinary( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
		}

		/// <summary>
		///		Invokes <see cref="Unpacker.ReadObject" /> and returns its result.
		/// </summary>
		/// <param name="unpacker">The unpacker to be used.</param>
		/// <param name="objectType">The type of the object which is deserializing now.</param>
		/// <param name="memberName">The name of the member which is deserializing now.</param>
		/// <returns>The unpacked value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to unpack object value from underlying stream.
		/// </exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static MessagePackObject UnpackObjectValue( Unpacker unpacker, Type objectType, String memberName )
		{
			MessagePackObject result;
			if ( !unpacker.ReadObject( out result ) )
			{
				throw SerializationExceptions.NewFailedToDeserializeMember( objectType, memberName, null );
			}

			return result;
		}
	}
}
