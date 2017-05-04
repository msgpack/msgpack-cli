
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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
using System.IO;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file was generated from UnpackerReader.TypedRead.tt and UnpackerReader.TypedRead.ttinclude T4Template.
	// Do not modify this file. Edit UnpackerReader.TypedRead.tt and UnpackerReader.TypedRead.ttinclude instead.

	partial class UnpackerReader
	{

		public abstract byte ReadByte();

		public abstract int TryReadByte();

		public abstract sbyte ReadSByte();

		public abstract short ReadInt16();

		public abstract ushort ReadUInt16();

		public abstract int TryReadUInt16();

		public abstract int ReadInt32();

		public abstract uint ReadUInt32();

		public abstract long TryReadUInt32();

		public abstract long ReadInt64();

		public abstract ulong ReadUInt64();

		public abstract float ReadSingle();

		public abstract double ReadDouble();

#if FEATURE_TAP

		public abstract Task<byte> ReadByteAsync( CancellationToken cancellationToken );

		public abstract Task<int> TryReadByteAsync( CancellationToken cancellationToken );

		public abstract Task<sbyte> ReadSByteAsync( CancellationToken cancellationToken );

		public abstract Task<short> ReadInt16Async( CancellationToken cancellationToken );

		public abstract Task<ushort> ReadUInt16Async( CancellationToken cancellationToken );

		public abstract Task<int> TryReadUInt16Async( CancellationToken cancellationToken );

		public abstract Task<int> ReadInt32Async( CancellationToken cancellationToken );

		public abstract Task<uint> ReadUInt32Async( CancellationToken cancellationToken );

		public abstract Task<long> TryReadUInt32Async( CancellationToken cancellationToken );

		public abstract Task<long> ReadInt64Async( CancellationToken cancellationToken );

		public abstract Task<ulong> ReadUInt64Async( CancellationToken cancellationToken );

		public abstract Task<float> ReadSingleAsync( CancellationToken cancellationToken );

		public abstract Task<double> ReadDoubleAsync( CancellationToken cancellationToken );

#endif // FEATURE_TAP
	}
}