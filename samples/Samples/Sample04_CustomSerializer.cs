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
	///		A sample code to describe SerializationContext usage.
	/// </summary>
	[TestFixture]
	public class CustomSerializerSample
	{
		[Test]
		public void RegisterAndUseCustomSerializer()
		{
			var stream = new MemoryStream();

			// 1. To take advantage of SerializationContext, you should create own context to isolate others.
			//    Note that SerializationContext is thread safe.
			//    As you imagine, you can change 'default' settings by modifying properties of SerializationContext.Default.
			var context = new SerializationContext();

			// 2. Register custom serializers.
			context.Serializers.RegisterOverride( new NetUtcDateTimeSerializer( context ) );

			// 3. Get a serializer instance with customized settings.
			var serializer = MessagePackSerializer.Get<DateTime>( context );

			// Test it.
			var dateTime = DateTime.Now;
			serializer.Pack( stream, dateTime );
			stream.Position = 0;
			var deserialized = serializer.Unpack( stream );

			Debug.WriteLine( "Ticks are same? {0}", dateTime.Ticks == deserialized.Ticks );
		}
	}

	/// <summary>
	///		A custom serializer sample: Serialize <see cref="System.DateTime"/> as UTC.
	/// </summary>
	public class NetUtcDateTimeSerializer : MessagePackSerializer<DateTime>
	{
		// CAUTION: You MUST implement your serializer thread safe (usually, you can and you should implement serializer as immutable.)

		// Note: If your type has complex type fields, you want to add read only fields in the custom serializer for each fields complex types.
		// Note: It is good start point to use mpgen.exe utility to get Code-DOM generated serializer to understand custom serializer.
		//       Notice that the generated serializer is bit complex because of poor expressiveness of CodeDOM tree and built-in nil-handling (described in sample 06.)

		// ownerContext should be match the context to be registered.
		public NetUtcDateTimeSerializer( SerializationContext ownerContext ) : base( ownerContext ) { }

		protected override void PackToCore( Packer packer, DateTime objectTree )
		{
			packer.Pack( objectTree.Ticks );
		}

		protected override DateTime UnpackFromCore( Unpacker unpacker )
		{
			// Note that unapcker should be in head of the complex type.
			return new DateTime( unpacker.LastReadData.AsInt64() );

			// Note: if you deserialize following members, call Read() as following:
			// if ( unpacker.Read() )
			// {
			//     throw SerializationExceptions.NewUnexpectedEndOfStream();
			// }
			// ...Read next field...
		}
	}
}
