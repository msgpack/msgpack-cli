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
// Contributors:
//    Shrenik Jhaveri (ShrenikOne)
//
#endregion -- License Terms --

using System;
using System.Collections.Generic;
using System.IO;

using MsgPack.Serialization;

#if !MSTEST
using NUnit.Framework; // For running checking
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack
{
	[TestFixture]
	public class MessagePackMemberSkipTest
	{
		[Test]
		public void SerializeThenDeserialize_Array()
		{
			// They are object for just description. 
			var targetObject = new PhotoEntry
			{
				Id = 123,
				Title = "My photo",
				Date = DateTime.Now,
				Image = new byte[] { 1, 2, 3, 4 },
				Comment = "This is test object to be serialize/deserialize using MsgPack."
			};

			targetObject.Tags.Add( new PhotoTag { Name = "Sample", Id = 123 } );
			targetObject.Tags.Add( new PhotoTag { Name = "Excellent", Id = 456 } );
			var stream = new MemoryStream();

			// 1. Create serializer instance.
			SerializationContext context = new SerializationContext();
			context.SerializationMethod = SerializationMethod.Array;
			context.DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native;
			context.BindingOptions.SetIgnoringMembers( typeof( PhotoEntry ), new[] { nameof( PhotoEntry.Image ) } );
			context.BindingOptions.SetIgnoringMembers( typeof( PhotoTag ), new[] { nameof( PhotoTag.Name ) } );
			var serializer = MessagePackSerializer.Get<PhotoEntry>( context );

			// 2. Serialize object to the specified stream.
			serializer.Pack( stream, targetObject );

			// Set position to head of the stream to demonstrate deserialization.
			stream.Position = 0;

			// 3. Deserialize object from the specified stream.
			var deserializedObject = serializer.Unpack( stream );

			Assert.AreEqual( targetObject.Comment, deserializedObject.Comment );
			Assert.AreEqual( targetObject.Id, deserializedObject.Id );
			Assert.AreEqual( targetObject.Date, deserializedObject.Date );
			Assert.AreEqual( targetObject.Title, deserializedObject.Title );
			Assert.Null( deserializedObject.Image );
			Assert.AreEqual( targetObject.Tags.Count, deserializedObject.Tags.Count );
			for ( int i = 0; i < deserializedObject.Tags.Count; i++ )
			{
				Assert.AreEqual( targetObject.Tags[ i ].Id, deserializedObject.Tags[ i ].Id );
				Assert.Null( deserializedObject.Tags[ i ].Name );
			}

			//// TODO: @yfakariya, Need help, how i can achieve below....
			//// How i can inject Nil or Null for Skipped/Ignored member, to support interoperability...


			//// SerializationContext newContext = new SerializationContext();
			//// newContext.SerializationMethod = SerializationMethod.Array;
			//// newContext.DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native;
			//// serializer = MessagePackSerializer.Get<PhotoEntry>( newContext );

			//// // Set position to head of the stream to demonstrate deserialization.
			//// stream.Position = 0;

			//// // 3. Deserialize object from the specified stream.
			//// deserializedObject = serializer.Unpack( stream );

			//// Assert.AreEqual( targetObject.Comment, deserializedObject.Comment );
			//// Assert.AreEqual( targetObject.Id, deserializedObject.Id );
			//// Assert.AreEqual( targetObject.Date, deserializedObject.Date );
			//// Assert.AreEqual( targetObject.Title, deserializedObject.Title );
			//// Assert.Null( deserializedObject.Image );
			//// Assert.AreEqual( targetObject.Tags.Count, deserializedObject.Tags.Count );
			//// for ( int i = 0; i < deserializedObject.Tags.Count; i++ )
			//// {
			//// 	Assert.AreEqual( targetObject.Tags[ i ].Id, deserializedObject.Tags[ i ].Id );
			//// 	Assert.Null( deserializedObject.Tags[ i ].Name );
			//// }
		}

		[Test]
		public void SerializeThenDeserialize_Map()
		{
			// They are object for just description. 
			var targetObject = new PhotoEntry
			{
				Id = 123,
				Title = "My photo",
				Date = DateTime.Now,
				Image = new byte[] { 1, 2, 3, 4 },
				Comment = "This is test object to be serialize/deserialize using MsgPack."
			};

			targetObject.Tags.Add( new PhotoTag { Name = "Sample", Id = 123 } );
			targetObject.Tags.Add( new PhotoTag { Name = "Excellent", Id = 456 } );
			var stream = new MemoryStream();

			// 1. Create serializer instance.
			SerializationContext context = new SerializationContext();
			context.SerializationMethod = SerializationMethod.Map;
			context.DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native;
			context.BindingOptions.SetIgnoringMembers( typeof( PhotoEntry ), new[] { nameof( PhotoEntry.Image ) } );
			context.BindingOptions.SetIgnoringMembers( typeof( PhotoTag ), new[] { nameof( PhotoTag.Name ) } );
			var serializer = MessagePackSerializer.Get<PhotoEntry>( context );

			// 2. Serialize object to the specified stream.
			serializer.Pack( stream, targetObject );

			// Set position to head of the stream to demonstrate deserialization.
			stream.Position = 0;

			// 3. Deserialize object from the specified stream.
			var deserializedObject = serializer.Unpack( stream );

			Assert.AreEqual( targetObject.Comment, deserializedObject.Comment );
			Assert.AreEqual( targetObject.Id, deserializedObject.Id );
			Assert.AreEqual( targetObject.Date, deserializedObject.Date );
			Assert.AreEqual( targetObject.Title, deserializedObject.Title );
			Assert.Null( deserializedObject.Image );
			Assert.AreEqual( targetObject.Tags.Count, deserializedObject.Tags.Count );
			for ( int i = 0; i < deserializedObject.Tags.Count; i++ )
			{
				Assert.AreEqual( targetObject.Tags[ i ].Id, deserializedObject.Tags[ i ].Id );
				Assert.Null( deserializedObject.Tags[ i ].Name );
			}

			SerializationContext newContext = new SerializationContext();
			newContext.SerializationMethod = SerializationMethod.Map;
			newContext.DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native;
			serializer = MessagePackSerializer.Get<PhotoEntry>( context );

			// Set position to head of the stream to demonstrate deserialization.
			stream.Position = 0;

			// 3. Deserialize object from the specified stream.
			deserializedObject = serializer.Unpack( stream );

			Assert.AreEqual( targetObject.Comment, deserializedObject.Comment );
			Assert.AreEqual( targetObject.Id, deserializedObject.Id );
			Assert.AreEqual( targetObject.Date, deserializedObject.Date );
			Assert.AreEqual( targetObject.Title, deserializedObject.Title );
			Assert.Null( deserializedObject.Image );
			Assert.AreEqual( targetObject.Tags.Count, deserializedObject.Tags.Count );
			for ( int i = 0; i < deserializedObject.Tags.Count; i++ )
			{
				Assert.AreEqual( targetObject.Tags[ i ].Id, deserializedObject.Tags[ i ].Id );
				Assert.Null( deserializedObject.Tags[ i ].Name );
			}
		}
	}

	/// <summary>
	///	Simple class that will be used for serialization/deserialization.
	/// </summary>
	/// <remarks>
	/// If you want to interop with other platform using SerializationMethod.Array (default), you should use [MessagePackMember]. See Sample06 for details.
	/// </remarks>
	public class PhotoEntry
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public DateTime Date { get; set; }
		public string Comment { get; set; }
		public byte[] Image { get; set; }
		private readonly List<PhotoTag> _tags = new List<PhotoTag>();
		// Note that non-null read-only collection members are OK (of course, collections themselves must not be readonly.)
		public IList<PhotoTag> Tags { get { return this._tags; } }
	}

	public class PhotoTag
	{
		public long Id { get; set; }

		public string Name { get; set; }
	}
}
