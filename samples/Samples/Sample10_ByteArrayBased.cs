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

using System;
using System.Collections.Generic;
using MsgPack;
using MsgPack.Serialization;
using NUnit.Framework; // For running checking

namespace Samples
{
	[TestFixture]
	public class Sample10_ByteArrayBased
	{
		[Test]
		public void SimpleBufferCase()
		{
			// Assumes that we know maximum serialized data size, so just use it!
			var buffer = new byte[ 1024 * 64 ];

			var context = new SerializationContext();
			var serializer = context.GetSerializer<PhotoEntry>();

			var obj =
				new PhotoEntry
				{
					Id = 123,
					Title = "My photo",
					Date = DateTime.Now,
					Image = new byte[] { 1, 2, 3, 4 },
					Comment = "This is test object to be serialize/deserialize using MsgPack."
				};
			// Note that the packer automatically increse buffer list.
			using ( var bytePacker = Packer.Create( buffer, allowsBufferExpansion: false ) )
			{
				serializer.PackTo( bytePacker, obj );
				// Note: You can get actual bytes with GetResultBytes(), but it causes array copy.
				//       You can avoid copying using original buffer (when you prohibits buffer allocation on Packer.Create) or GetFinalBuffers() instead.
				Console.WriteLine( "Serialized: {0}", BitConverter.ToString( buffer, 0, ( int )bytePacker.BytesUsed ) );

				using ( var byteUnpacker = Unpacker.Create( buffer ) )
				{
					var deserialized = serializer.UnpackFrom( byteUnpacker );
				}
			}
		}

		[Test]
		public void ComplexBufferListCase()
		{
			var bufferManager = new MyArrayBufferManager();

			var context = new SerializationContext();
			var serializer = context.GetSerializer<PhotoEntry>();

			var obj =
				new PhotoEntry
				{
					Id = 123,
					Title = "My photo",
					Date = DateTime.Now,
					Image = new byte[] { 1, 2, 3, 4 },
					Comment = "This is test object to be serialize/deserialize using MsgPack."
				};
			// Note that the packer automatically increse buffer list.
			using ( var bytePacker = Packer.Create( bufferManager.Buffers, startIndex: 0, startOffset: 0, allowsBufferExpansion: true ) )
			{
				serializer.PackTo( bytePacker, obj );
				// The ByteArrayPacker tracks buffer usage (used bytes, current index of the list and offset of the current array).
				// In general, you bookkeep CurrentBufferIndex and CurrentBufferOffset for next Packer.Create call's startIndex and startOffset respectively.
				Console.WriteLine( "Packed {0:#,0}bytes. Now buffer position is index:{1}, offset:{2}", bytePacker.BytesUsed, bytePacker.CurrentBufferIndex, bytePacker.CurrentBufferOffset );

				using ( var byteUnpacker = Unpacker.Create( bufferManager.Buffers, startIndex: 0, startOffset: 0 ) )
				{
					var deserialized = serializer.UnpackFrom( byteUnpacker );
					// The ByteArrayUnpacker tracks buffer usage (used bytes, current index of the list and offset of the current array).
					Console.WriteLine( "Unpacked {0:#,0}bytes. Now buffer position is index:{1}, offset:{2}", byteUnpacker.BytesUsed, byteUnpacker.CurrentSourceIndex, byteUnpacker.CurrentSourceOffset );
				}
			}
		}
	}

	public class MyArrayBufferManager
	{
		public IList<ArraySegment<byte>> Buffers { get; }
	}
}
