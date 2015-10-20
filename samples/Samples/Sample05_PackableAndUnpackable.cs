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
using System.IO;

using MsgPack;
using MsgPack.Serialization;
using NUnit.Framework; // For running checking

namespace Samples
{
	/// <summary>
	///		A sample code to describe SerializationContext usage.
	/// </summary>
	[TestFixture]
	public class PackageAndUnpackableSample
	{
		[Test]
		public void RegisterAndUseCustomSerializer()
		{
			var targetObject = new PackableUnpackableObject();

			var stream = new MemoryStream();

			// You can serialize/deserialize objects which implement IPackable and/or IUnpackable as usual.

			var serializer = MessagePackSerializer.Get<PackableUnpackableObject>();
			serializer.Pack( stream, targetObject );
			stream.Position = 0;
			var deserializedObject = serializer.Unpack( stream );
		}
	}

	/// <summary>
	///		A custom serializer sample: Serialize <see cref="System.DateTime"/> as UTC.
	/// </summary>
	public class PackableUnpackableObject : IPackable, IUnpackable
	{
		// Imagine whien you cannot use auto-generated serializer so you have to implement custom serializer for own type easily.
		public long Id { get; set; }
		public string Name { get; set; }

		public void PackToMessage( Packer packer, PackingOptions options )
		{
			// Pack fields are here:
			// First, record total fields size.
			packer.PackArrayHeader( 2 );
			packer.Pack( this.Id );
			packer.PackString( this.Name );

			// ...Instead, you can pack as map as follows:
			// packer.PackMapHeader( 2 );
			// packer.Pack( "Id" );
			// packer.Pack( this.Id );
			// packer.Pack( "Name" );
			// packer.Pack( this.Name );
		}

		public void UnpackFromMessage( Unpacker unpacker )
		{
			// Unpack fields are here:

			// temp variables
			long id;
			string name;

			// It should be packed as array because we use hand-made packing implementation above.
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			// Check items count.
			if ( UnpackHelpers.GetItemsCount( unpacker ) != 2 )
			{
				throw SerializationExceptions.NewUnexpectedArrayLength( 2, UnpackHelpers.GetItemsCount( unpacker ) );
			}

			// Unpack fields here:
			if ( !unpacker.ReadInt64( out id ) )
			{
				throw SerializationExceptions.NewMissingProperty( "Id" );
			}

			this.Id = id;

			if ( !unpacker.ReadString( out name ) )
			{
				throw SerializationExceptions.NewMissingProperty( "Name" );
			}

			this.Name = name;

			// ...Instead, you can unpack from map as follows:
			//if ( !unpacker.IsMapHeader )
			//{
			//	throw SerializationExceptions.NewIsNotMapHeader();
			//}

			//// Check items count.
			//if ( UnpackHelpers.GetItemsCount( unpacker ) != 2 )
			//{
			//	throw SerializationExceptions.NewUnexpectedArrayLength( 2, UnpackHelpers.GetItemsCount( unpacker ) );
			//}

			//// Unpack fields here:
			//for ( int i = 0; i < 2 /* known count of fields */; i++ )
			//{
			//	// Unpack and verify key of entry in map.
			//	string key;
			//	if ( !unpacker.ReadString( out key ) )
			//	{
			//		// Missing key, incorrect.
			//		throw SerializationExceptions.NewUnexpectedEndOfStream();
			//	}

			//	switch ( key )
			//	{
			//		case "Id":
			//		{
			//			if ( !unpacker.ReadInt64( out id ) )
			//			{
			//				throw SerializationExceptions.NewMissingProperty( "Id" );
			//			}
			//
			//          this.Id = id;
			//			break;
			//		}
			//		case "Name":
			//		{
			//			if ( !unpacker.ReadString( out name ) )
			//			{
			//				throw SerializationExceptions.NewMissingProperty( "Name" );
			//			}
			//
			//          this.Name = name;
			//			break;
			//		}

			//		// Note: You should ignore unknown fields for forward compatibility.
			//	}
			//}
		}
	}
}
