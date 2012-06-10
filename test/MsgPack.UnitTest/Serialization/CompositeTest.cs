#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using MsgPack.Serialization.EmittingSerializers;
using MsgPack.Serialization.ExpressionSerializers;
using NUnit.Framework;

namespace MsgPack.Serialization
{
	[TestFixture]
	public class CompositeTest
	{
		[Test]
		public void TestArrayFieldBased()
		{
			TestCore( EmitterFlavor.FieldBased, c => new ArrayEmittingSerializerBuilder<DirectoryItem>( c ) );
		}

		[Test]
		public void TestMapFieldBased()
		{
			TestCore( EmitterFlavor.FieldBased, c => new MapEmittingSerializerBuilder<DirectoryItem>( c ) );
		}

		[Test]
		public void TestArrayContextBased()
		{
			TestCore( EmitterFlavor.ContextBased, c => new ArrayEmittingSerializerBuilder<DirectoryItem>( c ) );
		}

		[Test]
		public void TestMapContextBased()
		{
			TestCore( EmitterFlavor.ContextBased, c => new MapEmittingSerializerBuilder<DirectoryItem>( c ) );
		}

		[Test]
		public void TestArrayExpressionBased()
		{
			TestCore( EmitterFlavor.ExpressionBased, c => new ExpressionSerializerBuilder<DirectoryItem>( c ) );
		}

		[Test]
		public void TestMapExpressionBased()
		{
			TestCore( EmitterFlavor.ExpressionBased, c => new ExpressionSerializerBuilder<DirectoryItem>( c ) );
		}

		private static void TestCore( EmitterFlavor emittingFlavor, Func<SerializationContext, SerializerBuilder<DirectoryItem>> builderProvider )
		{
			var root = new DirectoryItem() { Name = "/" };
			root.Directories =
				new[]
				{
					new DirectoryItem() { Name = "tmp/" },
					new DirectoryItem() 
					{ 
						Name = "var/", 
						Directories = new DirectoryItem[ 0 ], 
						Files = new []{ new FileItem(){ Name = "system.log" } }
					}
				};
			root.Files = new FileItem[ 0 ];

			var serializer = new AutoMessagePackSerializer<DirectoryItem>( new SerializationContext() { EmitterFlavor = emittingFlavor, GeneratorOption = SerializationMethodGeneratorOption.CanDump }, builderProvider );
			using ( var memoryStream = new MemoryStream() )
			{
				serializer.Pack( memoryStream, root );

				memoryStream.Position = 0;
				var result = serializer.Unpack( memoryStream );
				Assert.That( result.Name, Is.EqualTo( "/" ) );
				Assert.That( result.Files, Is.Not.Null.And.Empty );
				Assert.That( result.Directories, Is.Not.Null.And.Length.EqualTo( 2 ) );
				Assert.That( result.Directories[ 0 ], Is.Not.Null );
				Assert.That( result.Directories[ 0 ].Name, Is.EqualTo( "tmp/" ) );
				Assert.That( result.Directories[ 0 ].Files, Is.Null );
				Assert.That( result.Directories[ 0 ].Directories, Is.Null );
				Assert.That( result.Directories[ 1 ], Is.Not.Null );
				Assert.That( result.Directories[ 1 ].Name, Is.EqualTo( "var/" ) );
				Assert.That( result.Directories[ 1 ].Files, Is.Not.Null.And.Length.EqualTo( 1 ) );
				Assert.That( result.Directories[ 1 ].Files[ 0 ], Is.Not.Null );
				Assert.That( result.Directories[ 1 ].Files[ 0 ].Name, Is.EqualTo( "system.log" ) );
				Assert.That( result.Directories[ 1 ].Directories, Is.Not.Null.And.Empty );
			}
		}
	}

	public abstract class FileSystemItem
	{
		public string Name { get; set; }
	}

	public sealed class FileItem : FileSystemItem { }

	public sealed class DirectoryItem : FileSystemItem
	{
		public FileItem[] Files { get; set; }
		public DirectoryItem[] Directories { get; set; }
	}
}
