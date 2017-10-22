#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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
using System.IO;
using System.Threading.Tasks;

using MsgPack.Serialization;
using NUnit.Framework; // For running checking

namespace Samples
{
    /// <summary>
    ///	A sample code to describe async methods.
    /// </summary>
    [TestFixture]
	public class AsyncSample
	{
        /// <summary>
        /// Has object serialized and deserialized asynchronously.
        /// </summary>
		[Test]
		public async Task RunAsync()
		{
			// Now supports async.
			var context = new SerializationContext();

			// Asynchronous is allowed by default.
			Assert.True( context.SerializerOptions.WithAsync );

			var serializer = context.GetSerializer<PhotoEntry>();
			var targetObject =
				new PhotoEntry // See Sample01_BasicUsage.cs
				{
					Id = 123,
					Title = "My photo",
					Date = DateTime.Now,
					Image = new byte[] { 1, 2, 3, 4 },
					Comment = "This is test object to be serialize/deserialize using MsgPack."
				};

			using ( var stream = new MemoryStream() )
			{
				// Note: PackAsync/UnpackAsync uses internal BufferedStream to avopid chatty asynchronous call for underlying I/O system.
				//       If you change this behavior, use PackToAsync/UnpackFromAsync with Packer/Unpacker factories to specify options.
				await serializer.PackAsync( stream, targetObject );
				stream.Position = 0;
				var roundTripped = await serializer.UnpackAsync( stream );
			}
		}
	}
}