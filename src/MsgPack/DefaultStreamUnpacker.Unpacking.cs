
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file was generated from DefaultStreamUnpacker.Unpacking.tt and Unpacker.Unpacking.ttinclude T4Template.
	// Do not modify this file. Edit DefaultStreamUnpacker.Unpacking.tt and Unpacker.Unpacking.ttinclude instead.

	partial class DefaultStreamUnpacker
	{
#if DEBUG
		internal
#endif // DEBUG
		protected MessagePackUnpacker<StreamUnpackerReader> Core { get; }

		[Obsolete( "Consumer should not use this property. Query LastReadData instead." )]
		public override MessagePackObject? Data
		{
			get { return this.Core.Data; }
			protected set { this.Core.Data = value.GetValueOrDefault(); }
		}

		public override bool IsArrayHeader
		{
			get { return this.Core.CollectionType == CollectionType.Array; }
		}

		public override bool IsMapHeader
		{
			get { return this.Core.CollectionType == CollectionType.Map; }
		}


		public override long ItemsCount
		{
			get { return this.Core.CollectionType == CollectionType.None ? 0 : this.Core.ItemsCount; }
		}

		protected override bool ReadCore()
		{
			return this.Core.Read();
		}

		protected override long? SkipCore()
		{
			return this.Core.Skip();
		}

#if FEATURE_TAP

		protected override Task<long?> SkipAsyncCore( CancellationToken cancellationToken )
		{
			return this.Core.SkipAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadBoolean( out Boolean result )
		{
			return this.Core.ReadBoolean( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<Boolean>> ReadBooleanAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadBooleanAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public override bool ReadNullableBoolean( out Boolean? result )
		{
			return this.Core.ReadNullableBoolean( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullables essentially must be nested generic." )]
		public override Task<AsyncReadResult<Boolean?>> ReadNullableBooleanAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadNullableBooleanAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadByte( out Byte result )
		{
			return this.Core.ReadByte( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<Byte>> ReadByteAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadByteAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public override bool ReadNullableByte( out Byte? result )
		{
			return this.Core.ReadNullableByte( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullables essentially must be nested generic." )]
		public override Task<AsyncReadResult<Byte?>> ReadNullableByteAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadNullableByteAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadSByte( out SByte result )
		{
			return this.Core.ReadSByte( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<SByte>> ReadSByteAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadSByteAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public override bool ReadNullableSByte( out SByte? result )
		{
			return this.Core.ReadNullableSByte( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullables essentially must be nested generic." )]
		public override Task<AsyncReadResult<SByte?>> ReadNullableSByteAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadNullableSByteAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadInt16( out Int16 result )
		{
			return this.Core.ReadInt16( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<Int16>> ReadInt16Async( CancellationToken cancellationToken )
		{
			return this.Core.ReadInt16Async( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public override bool ReadNullableInt16( out Int16? result )
		{
			return this.Core.ReadNullableInt16( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullables essentially must be nested generic." )]
		public override Task<AsyncReadResult<Int16?>> ReadNullableInt16Async( CancellationToken cancellationToken )
		{
			return this.Core.ReadNullableInt16Async( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadUInt16( out UInt16 result )
		{
			return this.Core.ReadUInt16( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<UInt16>> ReadUInt16Async( CancellationToken cancellationToken )
		{
			return this.Core.ReadUInt16Async( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public override bool ReadNullableUInt16( out UInt16? result )
		{
			return this.Core.ReadNullableUInt16( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullables essentially must be nested generic." )]
		public override Task<AsyncReadResult<UInt16?>> ReadNullableUInt16Async( CancellationToken cancellationToken )
		{
			return this.Core.ReadNullableUInt16Async( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadInt32( out Int32 result )
		{
			return this.Core.ReadInt32( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<Int32>> ReadInt32Async( CancellationToken cancellationToken )
		{
			return this.Core.ReadInt32Async( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public override bool ReadNullableInt32( out Int32? result )
		{
			return this.Core.ReadNullableInt32( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullables essentially must be nested generic." )]
		public override Task<AsyncReadResult<Int32?>> ReadNullableInt32Async( CancellationToken cancellationToken )
		{
			return this.Core.ReadNullableInt32Async( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadUInt32( out UInt32 result )
		{
			return this.Core.ReadUInt32( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<UInt32>> ReadUInt32Async( CancellationToken cancellationToken )
		{
			return this.Core.ReadUInt32Async( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public override bool ReadNullableUInt32( out UInt32? result )
		{
			return this.Core.ReadNullableUInt32( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullables essentially must be nested generic." )]
		public override Task<AsyncReadResult<UInt32?>> ReadNullableUInt32Async( CancellationToken cancellationToken )
		{
			return this.Core.ReadNullableUInt32Async( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadInt64( out Int64 result )
		{
			return this.Core.ReadInt64( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<Int64>> ReadInt64Async( CancellationToken cancellationToken )
		{
			return this.Core.ReadInt64Async( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public override bool ReadNullableInt64( out Int64? result )
		{
			return this.Core.ReadNullableInt64( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullables essentially must be nested generic." )]
		public override Task<AsyncReadResult<Int64?>> ReadNullableInt64Async( CancellationToken cancellationToken )
		{
			return this.Core.ReadNullableInt64Async( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadUInt64( out UInt64 result )
		{
			return this.Core.ReadUInt64( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<UInt64>> ReadUInt64Async( CancellationToken cancellationToken )
		{
			return this.Core.ReadUInt64Async( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public override bool ReadNullableUInt64( out UInt64? result )
		{
			return this.Core.ReadNullableUInt64( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullables essentially must be nested generic." )]
		public override Task<AsyncReadResult<UInt64?>> ReadNullableUInt64Async( CancellationToken cancellationToken )
		{
			return this.Core.ReadNullableUInt64Async( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadSingle( out Single result )
		{
			return this.Core.ReadSingle( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<Single>> ReadSingleAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadSingleAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public override bool ReadNullableSingle( out Single? result )
		{
			return this.Core.ReadNullableSingle( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullables essentially must be nested generic." )]
		public override Task<AsyncReadResult<Single?>> ReadNullableSingleAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadNullableSingleAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadDouble( out Double result )
		{
			return this.Core.ReadDouble( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<Double>> ReadDoubleAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadDoubleAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public override bool ReadNullableDouble( out Double? result )
		{
			return this.Core.ReadNullableDouble( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullables essentially must be nested generic." )]
		public override Task<AsyncReadResult<Double?>> ReadNullableDoubleAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadNullableDoubleAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject result )
		{
			return this.Core.ReadMessagePackExtendedTypeObject( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<MessagePackExtendedTypeObject>> ReadMessagePackExtendedTypeObjectAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadMessagePackExtendedTypeObjectAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public override bool ReadNullableMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject? result )
		{
			return this.Core.ReadNullableMessagePackExtendedTypeObject( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullables essentially must be nested generic." )]
		public override Task<AsyncReadResult<MessagePackExtendedTypeObject?>> ReadNullableMessagePackExtendedTypeObjectAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadNullableMessagePackExtendedTypeObjectAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadArrayLength( out long result )
		{
			return this.Core.ReadArrayLength( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<long>> ReadArrayLengthAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadArrayLengthAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadMapLength( out long result )
		{
			return this.Core.ReadMapLength( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<long>> ReadMapLengthAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadMapLengthAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadBinary( out byte[] result )
		{
			return this.Core.ReadBinary( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<byte[]>> ReadBinaryAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadBinaryAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadString( out string result )
		{
			return this.Core.ReadString( out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		public override Task<AsyncReadResult<string>> ReadStringAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadStringAsync( cancellationToken );
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public override bool ReadObject( out MessagePackObject result )
		{
			return this.Core.ReadObject( /* isDeep*/true, out result );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design" )]
		public override Task<AsyncReadResult<MessagePackObject>> ReadObjectAsync( CancellationToken cancellationToken )
		{
			return this.Core.ReadObjectAsync( /* isDeep*/true,  cancellationToken );
		}

#endif // FEATURE_TAP

	}
}
