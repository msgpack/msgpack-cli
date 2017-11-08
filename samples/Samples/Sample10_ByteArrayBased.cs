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
    /// <summary>
    /// A sample code to describe byte array based behavior.
    /// </summary>
	[TestFixture]
	public class Sample10_ByteArrayBased
	{
        /// <summary>
        /// Uses byte array for serialization and deserialization.
        /// </summary>
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
			// Note that the packer automatically increse buffer.
			using ( var bytePacker = Packer.Create( buffer, true, PackerCompatibilityOptions.None ) )
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
	}

	public class MyArrayBufferManager
	{
		public IList<ArraySegment<byte>> Buffers { get; }
	}
}
