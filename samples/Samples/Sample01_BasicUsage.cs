#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Diagnostics;
using System.IO;

using MsgPack.Serialization;

using NUnit.Framework; // For running checking

namespace Samples
{
	/// <summary>
	///		A simple sample code for basic serialization/deserialization.
	/// </summary>
	[TestFixture]
	public class BasicUsageSample
	{
		[Test]
		public void SerializeThenDeserialize()
		{
			// They are object for just description. 
			var targetObject =
				new PhotoEntry
				{
					Id = 123,
					Title = "My photo",
					Date = DateTime.Now,
					Image = new byte[] { 1, 2, 3, 4 },
					Comment = "This is test object to be serialize/deserialize using MsgPack."
				};
			targetObject.Tags.Add( "Sample" );
			targetObject.Tags.Add( "Excellent" );
			var stream = new MemoryStream();

			// 1. Create serializer instance.
			var serializer = MessagePackSerializer.Get<PhotoEntry>();

			// 2. Serialize object to the specified stream.
			serializer.Pack( stream, targetObject );

			// Set position to head of the stream to demonstrate deserialization.
			stream.Position = 0;

			// 3. Deserialize object from the specified stream.
			var deserializedObject = serializer.Unpack( stream );

			// Test deserialized value.
			Debug.WriteLine( "Same object? {0}", Object.ReferenceEquals( targetObject, deserializedObject ) );
			Debug.WriteLine( "Same Id? {0}", targetObject.Id == deserializedObject.Id );
			Debug.WriteLine( "Same Title? {0}", targetObject.Title == deserializedObject.Title );
			// Note that MsgPack defacto-standard is Unix epoc in milliseconds precision, so micro- and nano- seconds will be lost. See sample 04 for workaround.
			Debug.WriteLine( "Same Date? {0}", targetObject.Date.ToString( "YYYY-MM-DD HH:mm:ss.fff" ) == deserializedObject.Date.ToString( "YYYY-MM-DD HH:mm:ss.fff" ) );
			// Image and Comment tests are ommitted here.
			// Collection elements are deserialzed.
			Debug.WriteLine( "Items count: {0}", deserializedObject.Tags.Count );
		}
	}

	// Note: If you want to interop with other platform using SerializationMethod.Array (default), you should use [MessagePackMember]. See Sample06 for details.
	public class PhotoEntry
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public DateTime Date { get; set; }
		public string Comment { get; set; }
		public byte[] Image { get; set; }
		private readonly List<string> _tags = new List<string>();
		// Note that non-null read-only collection members are OK (of course, collections themselves must not be readonly.)
		public IList<string> Tags { get { return this._tags; } }
	}
}
