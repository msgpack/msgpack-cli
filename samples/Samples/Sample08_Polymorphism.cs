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
using System.Collections.Generic;
using System.IO;

using MsgPack.Serialization;
using NUnit.Framework; // For running checking

namespace Samples
{
	/// <summary>
	///	A sample to describe polymorphism.
	/// </summary>
	[TestFixture]
	public class PolymorphismSample
	{
		/// <summary>
		///	Demonstrates basic polymorphism.
		/// </summary>
		[Test]
		public void Polymorphism()
		{
			// As of 0.7, polymorphism is implemented.

			// Setup the context to use map for pretty printing.
			var context = new SerializationContext { SerializationMethod = SerializationMethod.Map };

			var serializer = context.GetSerializer<PolymorphicHolder>();

			var rootObject =
				new PolymorphicHolder
				{
					WithRuntimeType = new FileObject { Path = "/path/to/file" },
					WithKnownType = new DirectoryObject { Path = "/path/to/dir/" }
				};

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack(
					buffer, new PolymorphicHolder
					{
						WithRuntimeType = new FileObject { Path = "/path/to/file" },
						WithKnownType = new DirectoryObject { Path = "/path/to/dir/" }
					}
				);

				buffer.Position = 0;

				Print( buffer );

				buffer.Position = 0;
				var deserialized = serializer.Unpack( buffer );

				Assert.That( deserialized.WithRuntimeType, Is.TypeOf<FileObject>() );
				Assert.That( deserialized.WithRuntimeType.Path, Is.EqualTo( "/path/to/file" ) );
				Assert.That( deserialized.WithKnownType, Is.TypeOf<DirectoryObject>() );
				Assert.That( deserialized.WithKnownType.Path, Is.EqualTo( "/path/to/dir/" ) );
			}
		}

		/// <summary>
		///	Demonstrates polymorphism without enclosing type and its attributes.
		/// </summary>
		[Test]
		public void DirectPolymorphism()
		{
			// As of 0.9, you can serialize polymorphic directly.

			// Setup the context to use map for pretty printing.
			var context = new SerializationContext { SerializationMethod = SerializationMethod.Map };

			// With type information embedding (runtime type)
			// The argument is "decoded" info from MessagePackRuntimeTypeAttribute or its family.
			var serializerWithTypeInfoEmbedding =
				context.GetSerializer<IFileSystemObject>( PolymorphismSchema.ForPolymorphicObject( typeof( IFileSystemObject ), SampleTypeVerifier.Verify ) );

			using ( var buffer = new MemoryStream() )
			{
				serializerWithTypeInfoEmbedding.Pack( buffer, new FileObject { Path = "/path/to/file" } );
				buffer.Position = 0;

				Print( buffer );

				buffer.Position = 0;
				var deserialized = serializerWithTypeInfoEmbedding.Unpack( buffer );
				Assert.That( deserialized, Is.TypeOf<FileObject>() );
				Assert.That( deserialized.Path, Is.EqualTo( "/path/to/file" ) );
			}

			// With type code mapping (known type)
			// The argument is "decoded" info from MessagePackKnownTypeAttribute or its family.
			var serializerWithTypeCodeMapping =
				context.GetSerializer<IFileSystemObject>(
					PolymorphismSchema.ForPolymorphicObject(
						typeof( IFileSystemObject ),
						new Dictionary<string, Type>
						{
							{ "f", typeof( FileObject ) },
							{ "d", typeof( DirectoryObject ) }
						}
					)
				);

			using ( var buffer = new MemoryStream() )
			{
				serializerWithTypeCodeMapping.Pack( buffer, new DirectoryObject { Path = "/path/to/dir/" } );
				buffer.Position = 0;

				Print( buffer );

				buffer.Position = 0;
				var deserialized = serializerWithTypeCodeMapping.Unpack( buffer );
				Assert.That( deserialized, Is.TypeOf<DirectoryObject>() );
				Assert.That( deserialized.Path, Is.EqualTo( "/path/to/dir/" ) );
			}
		}

		private static void Print( Stream serialized )
		{
			// Print serialized binary as JSON.
			// You can see that the object serialized as [<type info>, {<map>}] structure.
			Console.WriteLine( MessagePackSerializer.UnpackMessagePackObject( serialized ) );
		}
	}

	public class PolymorphicHolder
	{
		[MessagePackKnownType( "f", typeof( FileObject ) )]
		[MessagePackKnownType( "d", typeof( DirectoryObject ) )]
		public IFileSystemObject WithKnownType { get; set; }

		[MessagePackRuntimeType(VerifierType = typeof(SampleTypeVerifier), VerifierMethodName = "Verify")]
		public IFileSystemObject WithRuntimeType { get; set; }
	}

	/// <summary>
	///	Sample base type.
	/// </summary>
	public interface IFileSystemObject
	{
		string Path { get; set; }
	}

	/// <summary>
	///	Sample derived type 1.
	/// </summary>
	public class FileObject : IFileSystemObject
	{
		public string Path { get; set; }
	}

	/// <summary>
	///	Sample derived type 2.
	/// </summary>
	public class DirectoryObject : IFileSystemObject
	{
		public string Path { get; set; }
	}

	/// <summary>
	///	Sample type verifier.
	/// </summary>
	public static class SampleTypeVerifier
	{
        /// <summary>
        ///	Sample type verifier.
        /// </summary>
        /// <remarks>The signature of this method is important.</remarks>
        /// <param name="context">The context which has information of deserializing type.</param>
        /// <returns>True for accepting; otherwise, false.</returns>
        public static bool Verify( PolymorphicTypeVerificationContext context )
		{
			// You should put type verification logic to prevent unexpected code execution via specified type.
			// 1. Check context.LoadingAssemblyName here to verify the assembly is known for you.
			// 2. Check context.LoadingTypeFullName here to verify the type name is known for you.

			// Note: You should not get Assembly, Type, or related reflection related objects here
			//       to prevent potentially malicous code execution in its type initializer etc.

			// True for accepting; otherwise, false.
			return true;
		}
	}
}