#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace MsgPack.Serialization
{
	[TestFixture]
	public class SerializerGeneratorTest
	{
		#region -- Compat --
#pragma warning disable 0618
		[Test]
		public void TestGenerateAssemblyFile_WithDefault_DllIsGeneratedOnAppBase()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var target = new SerializerGenerator( typeof( GeneratorTestObject ), name );
			var filePath = Path.GetFullPath( "." + Path.DirectorySeparatorChar + name.Name + ".dll" );
			target.GenerateAssemblyFile();
			// Assert is not polluted.
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );

			try
			{
				TestOnWorkerAppDomain(
					filePath,
					PackerCompatibilityOptions.Classic,
					new byte[] { ( byte )'A' },
					new byte[] { MessagePackCode.MinimumFixedArray + 1, MessagePackCode.MinimumFixedRaw + 1, ( byte )'A' },
					TestType.GeneratorTestObject
				);
			}
			finally
			{
				File.Delete( filePath );
			}
		}

		[Test]
		public void TestGenerateAssemblyFile_WithDirectory_DllIsGeneratedOnSpecifiedDirectory()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var target = new SerializerGenerator( typeof( GeneratorTestObject ), name );
			var directory = Path.Combine( Path.GetTempPath(), Guid.NewGuid().ToString() );
			target.GenerateAssemblyFile( directory );
			// Assert is not polluted.
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );

			try
			{
				TestOnWorkerAppDomain(
					Path.Combine( directory, name.Name + ".dll" ),
					PackerCompatibilityOptions.Classic,
					new byte[] { ( byte )'A' },
					new byte[] { MessagePackCode.MinimumFixedArray + 1, MessagePackCode.MinimumFixedRaw + 1, ( byte )'A' },
					TestType.GeneratorTestObject
				);
			}
			finally
			{
				Directory.Delete( directory, true );
			}
		}

		[Test]
		public void TestGenerateAssemblyFile_WithMethod_OptionsAreAsSpecified()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var target = new SerializerGenerator( typeof( GeneratorTestObject ), name );
			target.Method = SerializationMethod.Map;
			var filePath = Path.GetFullPath( "." + Path.DirectorySeparatorChar + name.Name + ".dll" );
			target.GenerateAssemblyFile();
			// Assert is not polluted.
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );

			try
			{
				TestOnWorkerAppDomain(
					filePath,
					PackerCompatibilityOptions.Classic,
					new byte[] { ( byte )'A' },
					new byte[] { MessagePackCode.MinimumFixedMap + 1, MessagePackCode.MinimumFixedRaw + 3, ( byte )'V', ( byte )'a', ( byte )'l',
						MessagePackCode.MinimumFixedRaw + 1, ( byte )'A' },
					TestType.GeneratorTestObject
				);
			}
			finally
			{
				File.Delete( filePath );
			}
		}

		[Test]
		public void TestGenerateAssemblyFile_WithPackerOption_OptionsAreAsSpecified()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var target = new SerializerGenerator( typeof( GeneratorTestObject ), name );
			var filePath = Path.GetFullPath( "." + Path.DirectorySeparatorChar + name.Name + ".dll" );
			target.GenerateAssemblyFile();
			// Assert is not polluted.
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );

			try
			{
				TestOnWorkerAppDomain(
					filePath,
					PackerCompatibilityOptions.None,
					new byte[] { ( byte )'A' },
					new byte[] { MessagePackCode.MinimumFixedArray + 1, MessagePackCode.Bin8, 1, ( byte )'A' },
					TestType.GeneratorTestObject
				);
			}
			finally
			{
				File.Delete( filePath );
			}
		}

		[Test]
		public void TestGenerateAssemblyFile_ComplexType_ChildGeneratorsAreContainedAutomatically()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var target = new SerializerGenerator( typeof( RootGeneratorTestObject ), name );
			var directory = Path.Combine( Path.GetTempPath(), Guid.NewGuid().ToString() );
			target.GenerateAssemblyFile( directory );
			// Assert is not polluted.
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( RootGeneratorTestObject ) ), Is.False );

			try
			{
				TestOnWorkerAppDomain(
					Path.Combine( directory, "." + Path.DirectorySeparatorChar + name.Name + ".dll" ),
					PackerCompatibilityOptions.Classic,
					new byte[] { ( byte )'A' },
					new byte[]
					{
						MessagePackCode.MinimumFixedArray + 2,
						MessagePackCode.MinimumFixedArray + 1, MessagePackCode.MinimumFixedRaw + 1, ( byte ) 'A',
						MessagePackCode.NilValue
					},
					TestType.RootGeneratorTestObject
				);
			}
			finally
			{
				Directory.Delete( directory, true );
			}
		}

		[Test]
		public void TestGenerateAssemblyFile_ComplexType_MultipleTypes()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var target = new SerializerGenerator( name );
			target.TargetTypes.Add( typeof( GeneratorTestObject ) );
			target.TargetTypes.Add( typeof( AnotherGeneratorTestObject ) );
			var directory = Path.Combine( Path.GetTempPath(), Guid.NewGuid().ToString() );
			target.GenerateAssemblyFile( directory );
			// Assert is not polluted.
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( AnotherGeneratorTestObject ) ), Is.False );

			try
			{
				TestOnWorkerAppDomainForMultiple(
					Path.Combine( directory, "." + Path.DirectorySeparatorChar + name.Name + ".dll" ),
					PackerCompatibilityOptions.Classic,
					new byte[] { ( byte )'A' },
					new byte[]
					{
						MessagePackCode.MinimumFixedArray + 1, MessagePackCode.MinimumFixedRaw + 1, ( byte ) 'A',
					},
					new byte[] { ( byte )'B' },
					new byte[]
					{
						MessagePackCode.MinimumFixedArray + 1, MessagePackCode.MinimumFixedRaw + 1, ( byte ) 'B',
					}
				);
			}
			finally
			{
				Directory.Delete( directory, true );
			}
		}
#pragma warning restore 0618
		#endregion -- Compat --

		[Test]
		public void TestGenerateAssembly_WithDefault_DllIsGeneratedOnAppBase()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var filePath = Path.GetFullPath( "." + Path.DirectorySeparatorChar + name.Name + ".dll" );
			var result =
				SerializerGenerator.GenerateAssembly(
					new SerializerAssemblyGenerationConfiguration { AssemblyName = name }, typeof( GeneratorTestObject )
				);
			// Assert is not polluted.
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );
			Assert.That( result, Is.EqualTo( filePath ) );

			try
			{
				TestOnWorkerAppDomain(
					filePath,
					PackerCompatibilityOptions.Classic,
					new byte[] { ( byte )'A' },
					new byte[] { MessagePackCode.MinimumFixedArray + 1, MessagePackCode.MinimumFixedRaw + 1, ( byte )'A' },
					TestType.GeneratorTestObject
				);
			}
			finally
			{
				File.Delete( filePath );
			}
		}

		[Test]
		public void TestGenerateAssembly_WithDirectory_DllIsGeneratedOnSpecifiedDirectory()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var directory = Path.Combine( Path.GetTempPath(), Guid.NewGuid().ToString() );
			var filePath = Path.Combine( directory, name.Name + ".dll" );
			var result =
				SerializerGenerator.GenerateAssembly(
					new SerializerAssemblyGenerationConfiguration { AssemblyName = name, OutputDirectory = directory },
					typeof( GeneratorTestObject )
				);
			// Assert is not polluted.
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );
			Assert.That( result, Is.EqualTo( filePath ) );

			try
			{
				TestOnWorkerAppDomain(
					filePath,
					PackerCompatibilityOptions.Classic,
					new byte[] { ( byte )'A' },
					new byte[] { MessagePackCode.MinimumFixedArray + 1, MessagePackCode.MinimumFixedRaw + 1, ( byte )'A' },
					TestType.GeneratorTestObject
				);
			}
			finally
			{
				Directory.Delete( directory, true );
			}
		}

		[Test]
		public void TestGenerateAssembly_WithMethod_OptionsAreAsSpecified()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var filePath = Path.GetFullPath( "." + Path.DirectorySeparatorChar + name.Name + ".dll" );
			var result =
				SerializerGenerator.GenerateAssembly(
					new SerializerAssemblyGenerationConfiguration { AssemblyName = name, SerializationMethod = SerializationMethod.Map },
					typeof( GeneratorTestObject )
				);
			// Assert is not polluted.
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );
			Assert.That( result, Is.EqualTo( filePath ) );

			try
			{
				TestOnWorkerAppDomain(
					filePath,
					PackerCompatibilityOptions.Classic,
					new byte[] { ( byte )'A' },
					new byte[] { MessagePackCode.MinimumFixedMap + 1, MessagePackCode.MinimumFixedRaw + 3, ( byte )'V', ( byte )'a', ( byte )'l',
						MessagePackCode.MinimumFixedRaw + 1, ( byte )'A' },
					TestType.GeneratorTestObject
					);
			}
			finally
			{
				File.Delete( filePath );
			}
		}

		[Test]
		public void TestGenerateAssembly_WithPackerOption_OptionsAreAsSpecified()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var filePath = Path.GetFullPath( "." + Path.DirectorySeparatorChar + name.Name + ".dll" );
			var result =
				SerializerGenerator.GenerateAssembly(
					new SerializerAssemblyGenerationConfiguration { AssemblyName = name },
					typeof( GeneratorTestObject )
				);
			// Assert is not polluted.
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );
			Assert.That( result, Is.EqualTo( filePath ) );

			try
			{
				TestOnWorkerAppDomain(
					filePath,
					PackerCompatibilityOptions.None,
					new byte[] { ( byte )'A' },
					new byte[] { MessagePackCode.MinimumFixedArray + 1, MessagePackCode.Bin8, 1, ( byte )'A' },
					TestType.GeneratorTestObject
				);
			}
			finally
			{
				File.Delete( filePath );
			}
		}

		[Test]
		public void TestGenerateAssembly_ComplexType_ChildGeneratorsAreContainedAutomatically()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var directory = Path.Combine( Path.GetTempPath(), Guid.NewGuid().ToString() );
			var filePath = Path.Combine( directory, name.Name + ".dll" );
			var result =
				SerializerGenerator.GenerateAssembly(
					new SerializerAssemblyGenerationConfiguration { AssemblyName = name, OutputDirectory = directory },
					typeof( RootGeneratorTestObject )
				);
			// Assert is not polluted.
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( RootGeneratorTestObject ) ), Is.False );
			Assert.That( result, Is.EqualTo( filePath ) );

			try
			{
				TestOnWorkerAppDomain(
					filePath,
					PackerCompatibilityOptions.Classic,
					new byte[] { ( byte )'A' },
					new byte[]
					{
						MessagePackCode.MinimumFixedArray + 2,
						MessagePackCode.MinimumFixedArray + 1, MessagePackCode.MinimumFixedRaw + 1, ( byte ) 'A',
						MessagePackCode.NilValue
					},
					TestType.RootGeneratorTestObject
				);
			}
			finally
			{
				Directory.Delete( directory, true );
			}
		}

		[Test]
		public void TestGenerateAssembly_ComplexType_MultipleTypes()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var directory = Path.Combine( Path.GetTempPath(), Guid.NewGuid().ToString() );
			var filePath = Path.Combine( directory, name.Name + ".dll" );
			var result =
				SerializerGenerator.GenerateAssembly(
					new SerializerAssemblyGenerationConfiguration { AssemblyName = name, OutputDirectory = directory },
					typeof( GeneratorTestObject ),
					typeof( AnotherGeneratorTestObject )
				);
			// Assert is not polluted.
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );
			Assert.That( SerializationContext.Default.ContainsSerializer( typeof( AnotherGeneratorTestObject ) ), Is.False );
			Assert.That( result, Is.EqualTo( filePath ) );

			try
			{
				TestOnWorkerAppDomainForMultiple(
					filePath,
					PackerCompatibilityOptions.Classic,
					new byte[] { ( byte )'A' },
					new byte[]
					{
						MessagePackCode.MinimumFixedArray + 1, MessagePackCode.MinimumFixedRaw + 1, ( byte ) 'A',
					},
					new byte[] { ( byte )'B' },
					new byte[]
					{
						MessagePackCode.MinimumFixedArray + 1, MessagePackCode.MinimumFixedRaw + 1, ( byte ) 'B',
					}
				);
			}
			finally
			{
				Directory.Delete( directory, true );
			}
		}

		#region -- Issue102 --

		[Test]
		public void TestGenerateAssembly_DefaultEnumSerializationMethod_IsReflected()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var filePath = Path.GetFullPath( "." + Path.DirectorySeparatorChar + name.Name + ".dll" );
			var result =
				SerializerGenerator.GenerateAssembly(
					new SerializerAssemblyGenerationConfiguration { AssemblyName = name, EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue }, typeof( TestEnumType )
				);
			try
			{
				// Assert is not polluted.
				Assert.That( SerializationContext.Default.ContainsSerializer( typeof( TestEnumType ) ), Is.False );
				Assert.That( result, Is.EqualTo( filePath ) );

				TestOnWorkerAppDomain(
					filePath,
					PackerCompatibilityOptions.Classic,
					EnumSerializationMethod.ByUnderlyingValue,
					TestEnumType.One,
					new byte[] { ( byte )TestEnumType.One }
				);
			}
			finally
			{
				File.Delete( filePath );
			}
		}

		#endregion -- Issue102--

		#region -- Issue107 --

		[Test]
		public void TestGenerateSerializerCodeAssembly_WithDefaultNamespace()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var filePath = Path.GetFullPath( "." + Path.DirectorySeparatorChar + name.Name + ".dll" );
			var result =
				SerializerGenerator.GenerateSerializerCodeAssembly(
					new SerializerAssemblyGenerationConfiguration { AssemblyName = name, IsRecursive = false },
					typeof( GeneratorTestObject ),
					typeof( AnotherGeneratorTestObject )
				).ToArray();
			try
			{
				// Assert is not polluted.
				Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );
				Assert.That( SerializationContext.Default.ContainsSerializer( typeof( AnotherGeneratorTestObject ) ), Is.False );

				Assert.That( result.Length, Is.EqualTo( 2 ) );
				// Same path
				Assert.That( result.Select( r => r.FilePath ), Is.All.EqualTo( filePath ) );

				var one = result.Single( r => r.TargetType == typeof( GeneratorTestObject ) );
				Assert.That(
					one.SerializerTypeName,
					Is.EqualTo( "MsgPack_Serialization_GeneratorTestObjectSerializer" ) 
				);
				Assert.That(
					one.SerializerTypeNamespace,
					Is.EqualTo( "MsgPack.Serialization.EmittingSerializers.Generated" )
				);
				Assert.That(
					one.SerializerTypeFullName,
					Is.EqualTo( "MsgPack.Serialization.EmittingSerializers.Generated.MsgPack_Serialization_GeneratorTestObjectSerializer" )
				);

				var another = result.Single( r => r.TargetType == typeof( AnotherGeneratorTestObject ) );
				Assert.That(
					another.SerializerTypeName,
					Is.EqualTo( "MsgPack_Serialization_AnotherGeneratorTestObjectSerializer" )
				);
				Assert.That(
					another.SerializerTypeNamespace,
					Is.EqualTo( "MsgPack.Serialization.EmittingSerializers.Generated" )
				);
				Assert.That(
					another.SerializerTypeFullName,
					Is.EqualTo( "MsgPack.Serialization.EmittingSerializers.Generated.MsgPack_Serialization_AnotherGeneratorTestObjectSerializer" )
				);
			}
			finally
			{
				File.Delete( filePath );
			}
		}

		#endregion -- Issue107 --


		#region -- Issue105 --

		[Test]
		public void TestGenerateSerializerCodeAssembly_WithBuiltInSupportedTypes_Ignored()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var filePath = Path.GetFullPath( "." + Path.DirectorySeparatorChar + name.Name + ".dll" );
			var result =
				SerializerGenerator.GenerateSerializerCodeAssembly(
					new SerializerAssemblyGenerationConfiguration { AssemblyName = name, IsRecursive = true },
					typeof( int ),
					typeof( string ),
					typeof( DateTime ),
					typeof( List<int> ),
					typeof( int[] )
				).ToArray();
			try
			{
				Assert.That( result.Length, Is.EqualTo( 0 ) );
			}
			finally
			{
				File.Delete( filePath );
			}
		}

		#endregion -- Issue105 --

		[Test]
		public void TestGenerateCode_WithDefault_CSFileGeneratedOnAppBase()
		{
			var filePathCS =
				Path.GetFullPath(
					String.Join(
						Path.DirectorySeparatorChar.ToString(),
						new[]
						{
							".",
							"MsgPack",
							"Serialization",
							"GeneratedSerializers",
							IdentifierUtility.EscapeTypeName( typeof( GeneratorTestObject ) ) + "Serializer.cs"
						}
					)
				);
			var resultCS =
				SerializerGenerator.GenerateCode(
					typeof( GeneratorTestObject )
				).ToArray();
			try
			{
				// Assert is not polluted.
				Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );
				Assert.That( resultCS.Single(), Is.EqualTo( filePathCS ) );
				var linesCS = File.ReadAllLines( filePathCS );
				// BracingStyle, IndentString
				Assert.That( !linesCS.Any( l => Regex.IsMatch( l, @"^\t+if.+\{\s*$" ) ) );
				// Nemespace
				Assert.That(
					linesCS.Any( l => Regex.IsMatch( l, @"^\s*namespace\s+MsgPack\.Serialization\.GeneratedSerializers\s+" ) ) );
				// Array
				Assert.That( linesCS.Any( l => Regex.IsMatch( l, @"\.PackArrayHeader" ) ) );
			}
			finally
			{
				foreach ( var file in resultCS )
				{
					File.Delete( file );
				}
			}
		}

		[Test]
		public void TestGeneratCode_WithOptions_OptionsAreValid()
		{
			var directory = Path.Combine( Path.GetTempPath(), Guid.NewGuid().ToString() );
			var filePathCS =
				String.Join(
					Path.DirectorySeparatorChar.ToString(),
					new[]
					{
						directory,
						"Test",
						IdentifierUtility.EscapeTypeName( typeof( GeneratorTestObject ) ) + "Serializer.cs"
					}
				);
			var resultCS =
				SerializerGenerator.GenerateCode(
					new SerializerCodeGenerationConfiguration
					{
						CodeIndentString = "\t",
						Namespace = "Test",
						SerializationMethod = SerializationMethod.Map,
						OutputDirectory = directory,
					},
					typeof( GeneratorTestObject )
				).ToArray();
			try 
			{ 
				// Assert is not polluted.
				Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );
				Assert.That( resultCS.Single(), Is.EqualTo( filePathCS ) );
				//Console.WriteLine( File.ReadAllText( filePathCS ) );
				var linesCS = File.ReadAllLines( filePathCS );
				// BracingStyle, IndentString
				Assert.That( linesCS.Any( l => Regex.IsMatch( l, @"^\t+[^\{\s]+.+\{\s*$" ) ) );
				// Nemespace
				Assert.That( linesCS.Any( l => Regex.IsMatch( l, @"^\s*namespace\s+Test\s+" ) ) );
				// Map
				Assert.That( linesCS.Any( l => Regex.IsMatch( l, @"\.PackMapHeader" ) ) );

				// Language
				var filePathVB =
					String.Join(
						Path.DirectorySeparatorChar.ToString(),
						new[]
						{
							directory,
							"MsgPack",
							"Serialization",
							"GeneratedSerializers",
							IdentifierUtility.EscapeTypeName( typeof( GeneratorTestObject ) ) + "Serializer.vb"
						}
					);
				var resultVB =
					SerializerGenerator.GenerateCode(
						new SerializerCodeGenerationConfiguration
						{
							Language = "VB",
							OutputDirectory = directory,
						},
						typeof( GeneratorTestObject )
					).ToArray();
				try
				{
					// Assert is not polluted.
					Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );
					Assert.That( resultVB.Single(), Is.EqualTo( filePathVB ) );
					var linesVB = File.ReadAllLines( filePathVB );
					// CheckVB
					Assert.That( linesVB.Any( l => Regex.IsMatch( l, @"^\s*End Sub\s*$" ) ) );
				}
				finally
				{
					foreach ( var file in resultVB )
					{
						File.Delete( file );
					}
				}
			}
			finally
			{
				foreach ( var file in resultCS )
				{
					File.Delete( file );
				}
			}
		}

		[Test]
		public void TestGenerateCode_ComplexType_ChildGeneratorsAreNotContainedAutomatically()
		{
			var filePathCS =
				Path.GetFullPath(
					String.Join(
						Path.DirectorySeparatorChar.ToString(),
						new[]
						{
							".",
							"MsgPack",
							"Serialization",
							"GeneratedSerializers",
							IdentifierUtility.EscapeTypeName( typeof( RootGeneratorTestObject ) ) + "Serializer.cs"
						}
					)
				);
			var resultCS =
				SerializerGenerator.GenerateCode(
					typeof( RootGeneratorTestObject )
					).ToArray();
			try
			{
				// Assert is not polluted.
				Assert.That( SerializationContext.Default.ContainsSerializer( typeof( RootGeneratorTestObject ) ), Is.False );
				Assert.That( resultCS.Length, Is.EqualTo( 1 ) );
				Assert.That( resultCS[ 0 ], Is.EqualTo( filePathCS ) );
			}
			finally
			{
				foreach ( var file in resultCS )
				{
					File.Delete( file );
				}
			}
		}

		[Test]
		public void TestGenerateCode_ComplexType_MultipleTypes()
		{
			var filePathCS1 =
				Path.GetFullPath(
					String.Join(
						Path.DirectorySeparatorChar.ToString(),
						new[]
						{
							".",
							"MsgPack",
							"Serialization",
							"GeneratedSerializers",
							IdentifierUtility.EscapeTypeName( typeof( GeneratorTestObject ) ) + "Serializer.cs"
						}
					)
				);
			var filePathCS2 =
				Path.GetFullPath(
					String.Join(
						Path.DirectorySeparatorChar.ToString(),
						new[]
						{
							".",
							"MsgPack",
							"Serialization",
							"GeneratedSerializers",
							IdentifierUtility.EscapeTypeName( typeof( AnotherGeneratorTestObject ) ) + "Serializer.cs"
						}
					)
				);
			var resultCS =
				SerializerGenerator.GenerateCode(
					typeof( GeneratorTestObject ),
					typeof( AnotherGeneratorTestObject )
					).ToArray();
			try
			{
					// Assert is not polluted.
				Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );
				Assert.That( SerializationContext.Default.ContainsSerializer( typeof( AnotherGeneratorTestObject ) ), Is.False );
				Assert.That( SerializationContext.Default.ContainsSerializer( typeof( RootGeneratorTestObject ) ), Is.False );
				Assert.That( resultCS.Length, Is.EqualTo( 2 ) );
				Assert.That( resultCS, Contains.Item( filePathCS1 ).And.Contains( filePathCS2 ) );
			}
			finally
			{
				File.Delete( resultCS[ 0 ] );
			}
		}

		#region -- Issue102 --

		[Test]
		public void TestGenerateCode_DefaultEnumSerializationMethod_IsReflected()
		{
			var resultCS =
				SerializerGenerator.GenerateCode(
					new SerializerCodeGenerationConfiguration { EnumSerializationMethod = EnumSerializationMethod.ByUnderlyingValue },
					typeof( TestEnumType )
				).ToArray();
			try
			{
				// Assert is not polluted.
				Assert.That( SerializationContext.Default.ContainsSerializer( typeof( TestEnumType ) ), Is.False );
				Assert.That( resultCS.Length, Is.EqualTo( 1 ) );

				TestOnWorkerAppDomainWithCompile(
					resultCS[ 0 ],
					PackerCompatibilityOptions.Classic,
					EnumSerializationMethod.ByUnderlyingValue,
					TestEnumType.One,
					new byte[] { ( byte )TestEnumType.One }
				);
			}
			finally
			{
				foreach ( var file in resultCS )
				{
					File.Delete( file );
				}
			}
		}

		#endregion -- Issue102 --

		#region -- Issue107 --

		[Test]
		public void TestGenerateSerializerSourceCodes_WithoutNamespace_Default()
		{
			TestGenerateSerializerSourceCodesCore( null );
		}

		[Test]
		public void TestGenerateSerializerSourceCodes_WithNamespace_Used()
		{
			TestGenerateSerializerSourceCodesCore( "TestNamespace" );
		}

		[Test]
		public void TestGenerateSerializerSourceCodes_WithGlobalNameSpace_Used()
		{
			TestGenerateSerializerSourceCodesCore( String.Empty );
		}

		private static void TestGenerateSerializerSourceCodesCore( string @namespace )
		{
			var configuration = new SerializerCodeGenerationConfiguration { IsRecursive = false, Namespace = @namespace };
			var resultCS =
				SerializerGenerator.GenerateSerializerSourceCodes(
					configuration,
					typeof( GeneratorTestObject ),
					typeof( AnotherGeneratorTestObject )
				).ToArray();
			try
			{
				// Assert is not polluted.
				Assert.That( SerializationContext.Default.ContainsSerializer( typeof( GeneratorTestObject ) ), Is.False );
				Assert.That( SerializationContext.Default.ContainsSerializer( typeof( AnotherGeneratorTestObject ) ), Is.False );

				Assert.That( resultCS.Length, Is.EqualTo( 2 ) );

				var one = resultCS.SingleOrDefault( r => r.TargetType == typeof( GeneratorTestObject ) );
				Assert.That( one, Is.Not.Null, String.Join( ", ", resultCS.Select( r => r.TargetType.FullName ).ToArray() ) );
				Assert.That(
					one.FilePath,
					Is.EqualTo(
						Path.GetFullPath(
							String.Join(
								Path.DirectorySeparatorChar.ToString(),
								new[] { "." }
								.Concat( configuration.Namespace.Split( Type.Delimiter ) )
								.Concat(
									new[] { "MsgPack_Serialization_GeneratorTestObjectSerializer.cs" }
								).ToArray()
							)
						)
					)
				);
				Assert.That(
					one.SerializerTypeName,
					Is.EqualTo( "MsgPack_Serialization_GeneratorTestObjectSerializer" )
				);
				Assert.That(
					one.SerializerTypeNamespace,
					Is.EqualTo( configuration.Namespace )
				);
				Assert.That(
					one.SerializerTypeFullName,
					Is.EqualTo(
						( configuration.Namespace.Length > 0 ? configuration.Namespace + "." : String.Empty ) + 
						"MsgPack_Serialization_GeneratorTestObjectSerializer"
					)
				);

				var another = resultCS.SingleOrDefault( r => r.TargetType == typeof( AnotherGeneratorTestObject ) );
				Assert.That( another, Is.Not.Null, String.Join( ", ", resultCS.Select( r => r.TargetType.FullName ).ToArray() ) );
				Assert.That(
					another.FilePath,
					Is.EqualTo(
						Path.GetFullPath(
							String.Join(
								Path.DirectorySeparatorChar.ToString(),
								new[] { "." }
								.Concat( configuration.Namespace.Split( Type.Delimiter ) )
								.Concat(
									new[] { "MsgPack_Serialization_AnotherGeneratorTestObjectSerializer.cs" }
								).ToArray()
							)
						)
					)
				);
				Assert.That(
					another.SerializerTypeName,
					Is.EqualTo( "MsgPack_Serialization_AnotherGeneratorTestObjectSerializer" )
				);
				Assert.That(
					another.SerializerTypeNamespace,
					Is.EqualTo( configuration.Namespace )
				);
				Assert.That(
					another.SerializerTypeFullName,
					Is.EqualTo(
						( configuration.Namespace.Length > 0 ? configuration.Namespace + "." : String.Empty ) +
						"MsgPack_Serialization_AnotherGeneratorTestObjectSerializer"
					)
				);
			}
			finally
			{
				foreach ( var result in resultCS )
				{
					File.Delete( result.FilePath );
				}
			}
		}

		#endregion -- Issue107 --

		#region -- Issue105 --

		[Test]
		public void TestGenerateSerializerSourceCodes_WithBuiltInSupportedTypes_Ignored()
		{
			var configuration = new SerializerCodeGenerationConfiguration { IsRecursive = true, PreferReflectionBasedSerializer = false };
			var resultCS =
				SerializerGenerator.GenerateSerializerSourceCodes(
					configuration,
					typeof( int ),
					typeof( string ),
					typeof( DateTime ),
					typeof( List<int> ),
					typeof( int[] )
				).ToArray();
			try
			{
				Assert.That( resultCS.Length, Is.EqualTo( 0 ) );
			}
			finally
			{
				foreach ( var result in resultCS )
				{
					File.Delete( result.FilePath );
				}
			}
		}

		#endregion -- Issue105 --


		private static void TestOnWorkerAppDomain( string geneartedAssemblyFilePath, PackerCompatibilityOptions packerCompatibilityOptions, byte[] bytesValue, byte[] expectedPackedValue, TestType testType )
		{
			var appDomainSetUp = new AppDomainSetup() { ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase };
			var workerDomain = AppDomain.CreateDomain( "Worker", null, appDomainSetUp );
			try
			{
				var testerProxy =
					workerDomain.CreateInstanceAndUnwrap( typeof( Tester ).Assembly.FullName, typeof( Tester ).FullName ) as Tester;
				testerProxy.DoTest( geneartedAssemblyFilePath, ( int )packerCompatibilityOptions, bytesValue, expectedPackedValue, 1, testType );
			}
			finally
			{
				AppDomain.Unload( workerDomain );
			}
		}

		private static void TestOnWorkerAppDomain( string geneartedAssemblyFilePath, PackerCompatibilityOptions packerCompatibilityOptions, EnumSerializationMethod enumSerializationMethod, TestEnumType enumValue, byte[] expectedPackedValue )
		{
			var appDomainSetUp = new AppDomainSetup() { ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase };
			var workerDomain = AppDomain.CreateDomain( "Worker", null, appDomainSetUp );
			try
			{
				var testerProxy =
					workerDomain.CreateInstanceAndUnwrap( typeof( Tester ).Assembly.FullName, typeof( Tester ).FullName ) as Tester;
				testerProxy.DoTest( geneartedAssemblyFilePath, ( int )packerCompatibilityOptions, ( int )enumSerializationMethod, enumValue, expectedPackedValue, 1 );
			}
			finally
			{
				AppDomain.Unload( workerDomain );
			}
		}

		private static void TestOnWorkerAppDomainWithCompile( string geneartedSourceFilePath, PackerCompatibilityOptions packerCompatibilityOptions, EnumSerializationMethod enumSerializationMethod, TestEnumType enumValue, byte[] expectedPackedValue )
		{
			var parameters = new CompilerParameters();
			parameters.ReferencedAssemblies.Add( typeof( GeneratedCodeAttribute ).Assembly.Location );
			parameters.ReferencedAssemblies.Add( typeof( MessagePackObject ).Assembly.Location );
			parameters.ReferencedAssemblies.Add( Assembly.GetExecutingAssembly().Location );
			var result =
				CodeDomProvider.CreateProvider( "C#" ).CompileAssemblyFromFile( parameters, geneartedSourceFilePath );

			Assert.That( result.Errors.Count, Is.EqualTo( 0 ), String.Join( Environment.NewLine, result.Output.OfType<string>().ToArray() ) );

			try
			{
				var appDomainSetUp =
					new AppDomainSetup
					{
						ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
					};
				var workerDomain = AppDomain.CreateDomain( "Worker", null, appDomainSetUp );
				try
				{
					var testerProxy =
						workerDomain.CreateInstanceAndUnwrap( typeof( Tester ).Assembly.FullName, typeof( Tester ).FullName ) as Tester;
					testerProxy.DoTest(
						result.PathToAssembly,
						( int ) packerCompatibilityOptions,
						( int ) enumSerializationMethod,
						enumValue,
						expectedPackedValue,
						1 
					);
				}
				finally
				{
					AppDomain.Unload( workerDomain );
				}
			}
			finally
			{
				File.Delete( result.PathToAssembly );
			}
		}

		private static void TestOnWorkerAppDomainForMultiple( string geneartedAssemblyFilePath, PackerCompatibilityOptions packerCompatibilityOptions, byte[] bytesValue1, byte[] expectedPackedValue1, byte[] bytesValue2, byte[] expectedPackedValue2 )
		{
			var appDomainSetUp = new AppDomainSetup() { ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase };
			var workerDomain = AppDomain.CreateDomain( "Worker", null, appDomainSetUp );
			try
			{
				var testerProxy =
					workerDomain.CreateInstanceAndUnwrap( typeof( Tester ).Assembly.FullName, typeof( Tester ).FullName ) as Tester;
				testerProxy.DoTest( geneartedAssemblyFilePath, ( int )packerCompatibilityOptions, bytesValue1, expectedPackedValue1, 2, TestType.GeneratorTestObject );
				testerProxy.DoTest( geneartedAssemblyFilePath, ( int )packerCompatibilityOptions, bytesValue2, expectedPackedValue2, 2, TestType.AnotherGeneratorTestObject );
			}
			finally
			{
				AppDomain.Unload( workerDomain );
			}
		}

		public sealed class Tester : MarshalByRefObject
		{
			public void DoTest( string testAssemblyFile, int packerCompatiblityOptions, byte[] bytesValue, byte[] expectedPackedValue, int expectedSerializerTypeCounts, TestType testType )
			{
				var assembly = Assembly.LoadFrom( testAssemblyFile );
				var types = assembly.GetTypes().Where( t => typeof( IMessagePackSerializer ).IsAssignableFrom( t ) ).ToList();
				Assert.That( types.Count, Is.EqualTo( expectedSerializerTypeCounts ), String.Join( ", ", types.Select( t => t.ToString() ).ToArray() ) );

				var context = new SerializationContext( ( PackerCompatibilityOptions )packerCompatiblityOptions );

				byte[] binary;
				switch ( testType )
				{
					case TestType.GeneratorTestObject:
					{
						var serializer = Activator.CreateInstance( types.Single( t => typeof( MessagePackSerializer<GeneratorTestObject> ).IsAssignableFrom( t ) ), context ) as MessagePackSerializer<GeneratorTestObject>;
						binary = serializer.PackSingleObject( new GeneratorTestObject() { Val = bytesValue } );
						break;
					}
					case TestType.RootGeneratorTestObject:
					{
						var serializer = Activator.CreateInstance( types.Single( t => typeof( MessagePackSerializer<RootGeneratorTestObject> ).IsAssignableFrom( t ) ), context ) as MessagePackSerializer<RootGeneratorTestObject>;
						binary = serializer.PackSingleObject( new RootGeneratorTestObject() { Val = null, Child = new GeneratorTestObject() { Val = bytesValue } } );
						break;
					}
					default:
					{
						var serializer = Activator.CreateInstance( types.Single( t => typeof( MessagePackSerializer<AnotherGeneratorTestObject> ).IsAssignableFrom( t ) ), context ) as MessagePackSerializer<AnotherGeneratorTestObject>;
						binary = serializer.PackSingleObject( new AnotherGeneratorTestObject() { Val = bytesValue } );
						break;
					}
				}
				Assert.That(
					binary,
					Is.EqualTo( expectedPackedValue ),
					"{0} != {1}",
					Binary.ToHexString( binary ),
					Binary.ToHexString( expectedPackedValue )
				);
			}

			public void DoTest( string testAssemblyFile, int packerCompatiblityOptions, int enumSerializationMethod, TestEnumType enumValue, byte[] expectedPackedValue, int expectedSerializerTypeCounts )
			{
				var assembly = Assembly.LoadFrom( testAssemblyFile );
				var types = assembly.GetTypes().Where( t => typeof( IMessagePackSerializer ).IsAssignableFrom( t ) ).ToList();
				Assert.That( types.Count, Is.EqualTo( expectedSerializerTypeCounts ), String.Join( ", ", types.Select( t => t.ToString() ).ToArray() ) );

				var context = new SerializationContext( ( PackerCompatibilityOptions )packerCompatiblityOptions );

				byte[] binary;
				var serializer = Activator.CreateInstance( types.Single( t => typeof( MessagePackSerializer<TestEnumType> ).IsAssignableFrom( t ) ), context ) as MessagePackSerializer<TestEnumType>;
				binary = serializer.PackSingleObject( enumValue );
				Assert.That(
					binary,
					Is.EqualTo( expectedPackedValue ),
					"{0} != {1}",
					Binary.ToHexString( binary ),
					Binary.ToHexString( expectedPackedValue ) 
				);
			}
		}
	}

	[Serializable]
	public enum TestType
	{
		GeneratorTestObject,
		RootGeneratorTestObject,
		AnotherGeneratorTestObject,
	}

	public sealed class GeneratorTestObject
	{
		public byte[] Val { get; set; }
	}

	public sealed class RootGeneratorTestObject
	{
		public string Val { get; set; }
		public GeneratorTestObject Child { get; set; }
	}

	public sealed class AnotherGeneratorTestObject
	{
		public byte[] Val { get; set; }
	}

	[Serializable]
	public enum TestEnumType
	{
		Zero = 0,
		One = 1
	}
}
