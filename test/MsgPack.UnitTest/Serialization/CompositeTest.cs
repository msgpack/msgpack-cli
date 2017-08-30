#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke and contributors
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
//    Samuel Cragg
//
#endregion -- License Terms --

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using MsgPack.Serialization.AbstractSerializers;
#if !NETSTANDARD1_3
using MsgPack.Serialization.CodeDomSerializers;
#endif // !NETSTANDARD1_3
using MsgPack.Serialization.EmittingSerializers;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack.Serialization
{
	[TestFixture]
	public class CompositeTest
	{
#if !NETSTANDARD1_3
		[SetUp]
		public void SetUp()
		{
			SerializerDebugging.DeletePastTemporaries();
			//SerializerDebugging.TraceEnabled = true;
			//SerializerDebugging.DumpEnabled = true;
			if ( SerializerDebugging.TraceEnabled )
			{
				Tracer.Emit.Listeners.Clear();
				Tracer.Emit.Switch.Level = SourceLevels.All;
#if NETSTANDARD2_0
				Tracer.Emit.Listeners.Add( new TextWriterTraceListener( Console.Out ) );
#else // NETSTANDRD2_0
				Tracer.Emit.Listeners.Add( new ConsoleTraceListener() );
#endif // NETSTANDRD2_0
			}

			SerializerDebugging.DependentAssemblyManager = new TempFileDependentAssemblyManager( TestContext.CurrentContext.TestDirectory );
			SerializerDebugging.OnTheFlyCodeGenerationEnabled = true;
#if NET35
			SerializerDebugging.SetCodeCompiler( CodeDomCodeGeneration.Compile );
#else
			SerializerDebugging.SetCodeCompiler( RoslynCodeGeneration.Compile );
#endif // NET35
			SerializerDebugging.AddRuntimeAssembly( this.GetType().Assembly.Location );
		}

		[TearDown]
		public void TearDown()
		{
			if ( SerializerDebugging.DumpEnabled )
			{
#if !NETSTANDARD2_0
				try
				{
					SerializerDebugging.Dump();
				}
				catch ( NotSupportedException ex )
				{
					Console.Error.WriteLine( ex );
				}
				finally
				{
					SerializationMethodGeneratorManager.Refresh();
				}
#else // !NETSTANDARD2_0
				SerializationMethodGeneratorManager.Refresh();
#endif // !NETSTANDARD2_0
			}

			SerializerDebugging.Reset();
			SerializerDebugging.OnTheFlyCodeGenerationEnabled = false;
		}
#endif // !NETSTANDARD1_3

		[Test]
		public void TestArrayFieldBased()
		{
			TestCore( EmitterFlavor.FieldBased, SerializationMethod.Array, new AssemblyBuilderSerializerBuilder( typeof( DirectoryItem ), typeof( DirectoryItem ).GetCollectionTraits( CollectionTraitOptions.Full, allowNonCollectionEnumerableTypes: false ) ) );
		}

		[Test]
		public void TestMapFieldBased()
		{
			TestCore( EmitterFlavor.FieldBased, SerializationMethod.Map, new AssemblyBuilderSerializerBuilder( typeof( DirectoryItem ), typeof( DirectoryItem ).GetCollectionTraits( CollectionTraitOptions.Full, allowNonCollectionEnumerableTypes: false ) ) );
		}

#if !NETSTANDARD1_3
		[Test]
		public void TestArrayCodeDomBased()
		{
			TestCore( EmitterFlavor.CodeDomBased, SerializationMethod.Array, new CodeDomSerializerBuilder( typeof( DirectoryItem ), typeof( DirectoryItem ).GetCollectionTraits( CollectionTraitOptions.Full, allowNonCollectionEnumerableTypes: false ) ) );
		}

		[Test]
		public void TestMapCodeDomBased()
		{
			TestCore( EmitterFlavor.CodeDomBased, SerializationMethod.Map, new CodeDomSerializerBuilder( typeof( DirectoryItem ), typeof( DirectoryItem ).GetCollectionTraits( CollectionTraitOptions.Full, allowNonCollectionEnumerableTypes: false ) ) );
		}
#endif // !NETSTANDARD1_3

		private static void TestCore( EmitterFlavor emittingFlavor, SerializationMethod serializationMethod, ISerializerBuilder generator )
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

			var context =
				new SerializationContext
				{
					SerializationMethod = serializationMethod,
					SerializerOptions =
					{
#if !NETSTANDARD1_3
						GeneratorOption = SerializationMethodGeneratorOption.CanDump,
#endif // !NETSTANDARD1_3
						EmitterFlavor = emittingFlavor
					}
				};

			var serializer =
				( MessagePackSerializer<DirectoryItem> ) generator.BuildSerializerInstance(
					context,
					typeof( DirectoryItem ),
					PolymorphismSchema.Default
				);
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

	public sealed class FileItem : FileSystemItem
	{
		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "FileItem {{ Path = \"{0}\" }}", this.Name ?? "(null)" );
		}
	}

	public sealed class DirectoryItem : FileSystemItem
	{
		public FileItem[] Files { get; set; }
		public DirectoryItem[] Directories { get; set; }

		public override string ToString()
		{
			return
				String.Format(
					CultureInfo.InvariantCulture,
					"DirecgtoryItem {{ Path = \"{0}\", Directories = {1}, Files = {2} }}",
					this.Name ?? "(null)",
					this.Directories == null ? "(null)" : this.Directories.Length.ToString( CultureInfo.InvariantCulture ),
					this.Files == null ? "(null)" : this.Files.Length.ToString( CultureInfo.InvariantCulture )
				);
		}
	}
}
