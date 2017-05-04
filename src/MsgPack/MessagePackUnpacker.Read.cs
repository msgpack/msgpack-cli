
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
using System.Collections.Generic;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file was generated from MessagePackUnpacker.tt and MessagePackUnpacker.Read.ttinclude T4Template.
	// Do not modify this file. Edit MessagePackUnpacker.tt and MessagePackUnpacker.Read.ttinclude instead.

	partial class MessagePackUnpacker
	{
		public abstract bool ReadBoolean( out Boolean result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Boolean>> ReadBooleanAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadNullableBoolean( out Boolean? result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Boolean?>> ReadNullableBooleanAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadByte( out Byte result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Byte>> ReadByteAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadNullableByte( out Byte? result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Byte?>> ReadNullableByteAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadSByte( out SByte result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<SByte>> ReadSByteAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadNullableSByte( out SByte? result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<SByte?>> ReadNullableSByteAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadInt16( out Int16 result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Int16>> ReadInt16Async( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadNullableInt16( out Int16? result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Int16?>> ReadNullableInt16Async( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadUInt16( out UInt16 result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<UInt16>> ReadUInt16Async( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadNullableUInt16( out UInt16? result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<UInt16?>> ReadNullableUInt16Async( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadInt32( out Int32 result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Int32>> ReadInt32Async( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadNullableInt32( out Int32? result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Int32?>> ReadNullableInt32Async( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadUInt32( out UInt32 result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<UInt32>> ReadUInt32Async( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadNullableUInt32( out UInt32? result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<UInt32?>> ReadNullableUInt32Async( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadInt64( out Int64 result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Int64>> ReadInt64Async( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadNullableInt64( out Int64? result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Int64?>> ReadNullableInt64Async( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadUInt64( out UInt64 result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<UInt64>> ReadUInt64Async( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadNullableUInt64( out UInt64? result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<UInt64?>> ReadNullableUInt64Async( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadSingle( out Single result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Single>> ReadSingleAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadNullableSingle( out Single? result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Single?>> ReadNullableSingleAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadDouble( out Double result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Double>> ReadDoubleAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadNullableDouble( out Double? result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Double?>> ReadNullableDoubleAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadBinary( out Byte[] result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Byte[]>> ReadBinaryAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadString( out String result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<String>> ReadStringAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadObject( bool isDeep, out MessagePackObject result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<MessagePackObject>> ReadObjectAsync( bool isDeep, CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadArrayLength( out Int64 result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Int64>> ReadArrayLengthAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadMapLength( out Int64 result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<Int64>> ReadMapLengthAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<MessagePackExtendedTypeObject>> ReadMessagePackExtendedTypeObjectAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

		public abstract bool ReadNullableMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject? result );

#if FEATURE_TAP

		public abstract Task<AsyncReadResult<MessagePackExtendedTypeObject?>> ReadNullableMessagePackExtendedTypeObjectAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP

	}
}

