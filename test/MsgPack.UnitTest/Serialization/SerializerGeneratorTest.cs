#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Linq;
using System.Reflection;
using System.Text;

using NUnit.Framework;

namespace MsgPack.Serialization
{
	[TestFixture]
	public class SerializerGeneratorTest
	{
		[Test]
		public void TestWithDefault_DllIsGeneratedOnAppBase()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var target = new SerializerGenerator( typeof( GeneratorTestObject ), name );
			var filePath = Path.GetFullPath( ".\\" + name.Name + ".dll" );
			target.GenerateAssemblyFile();
			try
			{
				TestOnWorkerAppDomain(
					filePath,
					PackerCompatibilityOptions.Classic, 
					new byte[] { ( byte )'A' },
					new byte[] { MessagePackCode.MinimumFixedArray + 1, MessagePackCode.MinimumFixedRaw + 1, ( byte )'A' }
					);
			}
			finally
			{
				File.Delete( filePath );
			}
		}

		[Test]
		public void TestWithDirectory_DllIsGeneratedOnSpecifiedDirectory()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var target = new SerializerGenerator( typeof( GeneratorTestObject ), name );
			var directory = Path.Combine( Path.GetTempPath(), Guid.NewGuid().ToString() );
			try
			{
				target.GenerateAssemblyFile( directory );
				TestOnWorkerAppDomain(
					Path.Combine( directory, ".\\" + name.Name + ".dll" ),
					PackerCompatibilityOptions.Classic, 
					new byte[] { ( byte )'A' },
					new byte[] { MessagePackCode.MinimumFixedArray + 1, MessagePackCode.MinimumFixedRaw + 1, ( byte )'A' }
					);
			}
			finally
			{
				Directory.Delete( directory, true );
			}
		}

		[Test]
		public void TestWithWithMethod_OptionsAreAsSpecified()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var target = new SerializerGenerator( typeof( GeneratorTestObject ), name );
			target.Method = SerializationMethod.Map;
			var filePath = Path.GetFullPath( ".\\" + name.Name + ".dll" );
			target.GenerateAssemblyFile();
			try
			{
				TestOnWorkerAppDomain(
					filePath,
					PackerCompatibilityOptions.Classic, 
					new byte[] { ( byte )'A' },
					new byte[] { MessagePackCode.MinimumFixedMap + 1, MessagePackCode.MinimumFixedRaw + 3, ( byte )'V', ( byte )'a', ( byte )'l',
						MessagePackCode.MinimumFixedRaw + 1, ( byte )'A' }
					);
			}
			finally
			{
				File.Delete( filePath );
			}
		}

		[Test]
		public void TestWithWithPackerOption_OptionsAreAsSpecified()
		{
			var name = new AssemblyName( MethodBase.GetCurrentMethod().Name );
			var target = new SerializerGenerator( typeof( GeneratorTestObject ), name );
			var filePath = Path.GetFullPath( ".\\" + name.Name + ".dll" );
			target.GenerateAssemblyFile();
			try
			{
				TestOnWorkerAppDomain(
					filePath,
					PackerCompatibilityOptions.None, 
					new byte[] { ( byte )'A' },
					new byte[] { MessagePackCode.MinimumFixedArray + 1, MessagePackCode.Bin8, 1, ( byte )'A' }
					);
			}
			finally
			{
				File.Delete( filePath );
			}
		}

		private static void TestOnWorkerAppDomain( string geneartedAssemblyFilePath, PackerCompatibilityOptions packerCompatibilityOptions, byte[] bytesValue, byte[] expectedPackedValue )
		{
			var appDomainSetUp = new AppDomainSetup() { ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase };
			var workerDomain = AppDomain.CreateDomain( "Worker", null, appDomainSetUp );
			try
			{
				var testerProxy =
					workerDomain.CreateInstanceAndUnwrap( typeof( Tester ).Assembly.FullName, typeof( Tester ).FullName ) as Tester;
				testerProxy.DoTest( geneartedAssemblyFilePath, ( int )packerCompatibilityOptions, bytesValue, expectedPackedValue );
			}
			finally
			{
				AppDomain.Unload( workerDomain );
			}
		}

		public sealed class Tester : MarshalByRefObject
		{
			public void DoTest( string testAssemblyFile, int packerCompatiblityOptions, byte[] bytesValue, byte[] expectedPackedValue )
			{
				var assembly = Assembly.LoadFrom( testAssemblyFile );
				var types = assembly.GetTypes().Where( t => typeof( IMessagePackSerializer ).IsAssignableFrom( t ) ).ToList();
				Assert.That( types.Count, Is.EqualTo( 1 ), String.Join( ", ", types ) );

				var context = new SerializationContext( ( PackerCompatibilityOptions )packerCompatiblityOptions );

				var serializer = Activator.CreateInstance( types[ 0 ], context ) as MessagePackSerializer<GeneratorTestObject>;
				var binary = serializer.PackSingleObject( new GeneratorTestObject() { Val = bytesValue } );
				Assert.That( binary, Is.EqualTo( expectedPackedValue ) );
			}
		}
	}

	public sealed class GeneratorTestObject
	{
		public byte[] Val { get; set; }
	}

}
