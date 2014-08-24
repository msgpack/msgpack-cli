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
using System.Diagnostics;
using System.IO;

using MsgPack;
using MsgPack.Serialization;
using NUnit.Framework; // For running checking

namespace Samples
{
	/// <summary>
	///		A sample code for explore MessagePackObject.
	/// </summary>
	[TestFixture]
	public class HandlingDynamicObjectSample
	{
		[Test]
		public void SerializeThenDeserialize()
		{
			// They are object for just description. 
			var targetObject =
				new PhotoEntry // See Sample01_BasicUsage.cs
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

			// Set using Map instead of Array to serialize complex object. See Sample03 for details.
			var context = new SerializationContext();
			context.SerializationMethod = SerializationMethod.Map;
			// You can use default context if you want to use map in all serializations which use default context.
			// SerializationContext.Default.SerializationMethod = SerializationMethod.Map;

			// 1. Create serializer instance.
			var serializer = MessagePackSerializer.Get<PhotoEntry>( context );

			// 2. Serialize object to the specified stream.
			serializer.Pack( stream, targetObject );

			// Set position to head of the stream to demonstrate deserialization.
			stream.Position = 0;

			// 3. Unpack MessagePackObject to get raw representation.
			var rawObject = Unpacking.UnpackObject( stream );
			// You can read MPO tree via Unpacker
			// var unpacker = Unpacker.Create( stream );

			// Check its type
			Debug.WriteLine( "Is array? {0}", rawObject.IsArray ); // IsList is alias
			Debug.WriteLine( "Is map? {0}", rawObject.IsMap ); // IsDictionary is alias
			Debug.WriteLine( "Type: {0}", rawObject.UnderlyingType );

			// Gets serialized fields.
			// Note: When the object was serialized as array instead of map, use index instead.
			var asDictionary = rawObject.AsDictionary();
			Debug.WriteLine( "Id : {0}({1})", asDictionary[ "Id" ], asDictionary[ "Id" ].UnderlyingType );
			// String is encoded as utf-8 by default.
			Debug.WriteLine( "Title : {0}({1})", asDictionary[ "Title" ], asDictionary[ "Title" ].UnderlyingType );
			// Non-primitive is serialized as complex type or encoded primitive type.
			// DateTimeOffset is encoded as array[2]{ticks,offset}
			Debug.WriteLine( "Date : {0}({1})", asDictionary[ "Date" ], asDictionary[ "Date" ].UnderlyingType );
			// byte[] is byte[], as you know.
			Debug.WriteLine( "Image : {0}({1})", asDictionary[ "Image" ], asDictionary[ "Image" ].UnderlyingType );
		}
	}
}
