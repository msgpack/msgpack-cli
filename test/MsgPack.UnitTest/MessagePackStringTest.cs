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
using System.Security;
#if !NETFX_CORE && !WINDOWS_PHONE
using System.Security.Permissions;
using System.Security.Policy;
#endif
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif
using System.Globalization;

namespace MsgPack
{
	[TestFixture]
	public class MessagePackStringTest
	{
#if MSTEST
		public Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestContext TestContext
		{
			get;
			set;
		}

		private System.IO.TextWriter Console
		{
			get
			{
#if !SILVERLIGHT && !NETFX_CORE
				return System.Console.Out;
#else
				return System.IO.TextWriter.Null;
#endif
			}
		}
#endif

		[Test]
		public void TestGetHashCode_Binary()
		{
			var target = new MessagePackString( new byte[] { 0xFF, 0xED, 0xCB }, false );
			// OK, returned value is implementation detail.
			target.GetHashCode();
		}

		[Test]
		public void TestGetHashCode_String()
		{
			var target = new MessagePackString( "ABC" );
			Assert.AreEqual( "ABC".GetHashCode(), target.GetHashCode() );
		}

		[Test]
		public void TestGetHashCode_StringifiableBinary()
		{
			var target = new MessagePackString( new byte[] { ( byte )'A', ( byte )'B', ( byte )'C' }, false );
			Assert.AreEqual( "ABC".GetHashCode(), target.GetHashCode() );
		}

		[Test]
		public void TestGetHashCode_EmptyBinary()
		{
			var target = new MessagePackString( new byte[ 0 ], false );
			// OK, returned value is implementation detail.
			target.GetHashCode();
		}

		[Test]
		public void TestGetHashCode_EmptyString()
		{
			var target = new MessagePackString( String.Empty );
			Assert.AreEqual( String.Empty.GetHashCode(), target.GetHashCode() );
		}

		[Test]
		public void TestToString_Binary()
		{
			var target = new MessagePackString( new byte[] { 0xFF, 0xED, 0xCB }, false );
			Assert.AreEqual( "0xFFEDCB", target.ToString() );
		}

		[Test]
		public void TestToString_String()
		{
			var target = new MessagePackString( "ABC" );
			Assert.AreEqual( "ABC", target.ToString() );
		}

		[Test]
		public void TestToString_StringifiableBinary()
		{
			var target = new MessagePackString( new byte[] { ( byte )'A', ( byte )'B', ( byte )'C' }, false );
			Assert.AreEqual( String.Format( CultureInfo.InvariantCulture, "0x{0:x}{1:x}{2:x}", ( byte )'A', ( byte )'B', ( byte )'C' ), target.ToString() );
			// Encode
			target.GetString();
			Assert.AreEqual( "ABC", target.ToString() );
		}

		[Test]
		public void TestToString_EmptyBinary()
		{
			var target = new MessagePackString( new byte[ 0 ], false );
			Assert.AreEqual( String.Empty, target.ToString() );
		}

		[Test]
		public void TestToString_EmptyString()
		{
			var target = new MessagePackString( String.Empty );
			Assert.AreEqual( String.Empty, target.ToString() );
		}

#if !UNITY && !WINDOWS_PHONE && !NETFX_CORE
		[Test]
		public void TestEqualsFullTrust()
		{
			var result = TestEqualsCore();
			Assert.That( MessagePackString.IsFastEqualsDisabled, Is.False );
			Console.WriteLine( "TestEqualsFullTrust" );
			ShowResult( result );
		}

		private void ShowResult( Tuple<double, double, double, double> result )
		{
			Console.WriteLine( "Tiny(few bytes)      : {0:#,0.0} usec", result.Item1 );
			Console.WriteLine( "Small(16 chars)      : {0:#,0.0} usec", result.Item2 );
			Console.WriteLine( "Medium(1,000 chars)  : {0:#,0.0} usec", result.Item3 );
			Console.WriteLine( "Large(100,000 chars) : {0:#,0.0} usec", result.Item4 );
		}

#endif // !UNITY && !WINDOWS_PHONE && !NETFX_CORE

#if !NETFX_CORE && !WINDOWS_PHONE && !XAMIOS && !XAMDROID && !UNITY
		private static StrongName GetStrongName( Type type )
		{
			var assemblyName = type.Assembly.GetName();
			return new StrongName( new StrongNamePublicKeyBlob( assemblyName.GetPublicKey() ), assemblyName.Name, assemblyName.Version );
		}

		[Test]
		public void TestEqualsPartialTrust()
		{
			var appDomainSetUp = new AppDomainSetup() { ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase };
			var evidence = new Evidence();
#if MONO || NETFX_35
#pragma warning disable 0612
			// TODO: patching
			// currently, Mono does not declare AddHostEvidence
			evidence.AddHost( new Zone( SecurityZone.Internet ) );
#pragma warning restore 0612
			var permisions = GetDefaultInternetZoneSandbox();
#else
			evidence.AddHostEvidence( new Zone( SecurityZone.Internet ) );
			var permisions = SecurityManager.GetStandardSandbox( evidence );
#endif // if MONO || NETFX_35
			AppDomain workerDomain = AppDomain.CreateDomain( "PartialTrust", evidence, appDomainSetUp, permisions, GetStrongName( this.GetType() ) );
			try
			{
				workerDomain.DoCallBack( TestEqualsWorker );
				
#if !MONO
				// P/Invoke is not disabled on Mono even if in the InternetZone.
				Assert.That( ( bool )workerDomain.GetData( "MessagePackString.IsFastEqualsDisabled" ), Is.True );
#endif // if !MONO
				Console.WriteLine( "TestEqualsPartialTrust" );
				ShowResult( workerDomain.GetData( "TestEqualsWorker.Performance" ) as Tuple<double, double, double, double> );
			}
			finally
			{
				AppDomain.Unload( workerDomain );
			}
		}
		
#if MONO || NETFX_35
		private static PermissionSet GetDefaultInternetZoneSandbox()
		{
			var permissions = new PermissionSet( PermissionState.None );
			permissions.AddPermission(
				new FileDialogPermission(
					FileDialogPermissionAccess.Open
				)
			);
			permissions.AddPermission(
				new IsolatedStorageFilePermission( PermissionState.None )
				{
					UsageAllowed = IsolatedStorageContainment.ApplicationIsolationByUser,
					UserQuota = 1024000
				}			
			);
			permissions.AddPermission(
				new SecurityPermission(
					SecurityPermissionFlag.Execution
#if NETFX_35
					| SecurityPermissionFlag.SkipVerification
#endif // if NETFX_35
				)
			);
			permissions.AddPermission(
				new UIPermission(
					UIPermissionWindow.SafeTopLevelWindows,
					UIPermissionClipboard.OwnClipboard
				)
			);
			
			return permissions;
		}
#endif // if MONO || NETFX_35

		public static void TestEqualsWorker()
		{
			var result = TestEqualsCore();
			AppDomain.CurrentDomain.SetData( "TestEqualsWorker.Performance", result );
			AppDomain.CurrentDomain.SetData( "MessagePackString.IsFastEqualsDisabled", MessagePackString.IsFastEqualsDisabled );
		}
#endif // if !NETFX_CORE && !XAMIOS && !XAMDROID && !UNITY

		private static Tuple<double, double, double, double> TestEqualsCore()
		{
			Assert.IsTrue(
				new MessagePackString( new byte[] { ( byte )'A', ( byte )'B', ( byte )'C' }, false ).Equals(
					new MessagePackString( new byte[] { ( byte )'A', ( byte )'B', ( byte )'C' }, false )
				),
				"Binary-Binary-True"
			);

			Assert.IsTrue(
				new MessagePackString( new byte[] { ( byte )'A', ( byte )'B', ( byte )'C' }, false ).Equals(
					new MessagePackString( "ABC" )
				),
				"Binary-String-True"
			);

			Assert.IsTrue(
				new MessagePackString( "ABC" ).Equals(
					new MessagePackString( new byte[] { ( byte )'A', ( byte )'B', ( byte )'C' }, false )
				),
				"String-Binary-True"
			);

			Assert.IsTrue(
				new MessagePackString( "ABC" ).Equals(
					new MessagePackString( "ABC" )
				),
				"String-String-True"
			);

			Assert.IsFalse(
				new MessagePackString( new byte[] { ( byte )'A', ( byte )'B', ( byte )'C' }, false ).Equals(
					new MessagePackString( new byte[] { ( byte )'A', ( byte )'B', ( byte )'D' }, false )
				),
				"Binary-Binary-False"
			);

			Assert.IsFalse(
				new MessagePackString( new byte[] { ( byte )'A', ( byte )'B', ( byte )'C' }, false ).Equals(
					new MessagePackString( "ABD" )
				),
				"Binary-String-False"
			);

			Assert.IsFalse(
				new MessagePackString( "ABD" ).Equals(
					new MessagePackString( new byte[] { ( byte )'A', ( byte )'B', ( byte )'C' }, false )
				),
				"String-Binary-False"
			);

			Assert.IsFalse(
				new MessagePackString( "ABC" ).Equals(
					new MessagePackString( "ABD" )
				),
				"String-String-False"
			);

			var values =
				new[] 
				{ 
					new MessagePackString( new byte[ 0 ], false ), 
					new MessagePackString( new byte[] { 0x20 }, false ), 
					new MessagePackString( new byte[] { 0xff }, false ),
					new MessagePackString( new byte[] { 1, 2, 3 }, false ), 
					new MessagePackString( new byte[] { 3, 2, 1 }, false ) 
			};

			const int iteration = 10;
			double tinyAvg = Double.MaxValue;
			double smallAvg = Double.MaxValue;
			double mediumAvg = Double.MaxValue;
			double largeAvg = Double.MaxValue;

			var sw = new Stopwatch();
			for ( int i = 0; i < iteration; i++ )
			{
				sw.Restart();
				for ( int x = 0; x < values.Length; x++ )
				{
					Assert.That( values[ x ].Equals( null ), Is.False );

					for ( int y = 0; y < values.Length; y++ )
					{
						Assert.That( values[ x ].Equals( values[ y ] ), Is.EqualTo( x == y ) );
					}
				}
				sw.Stop();
				tinyAvg = Math.Min( tinyAvg, sw.Elapsed.Ticks * 10.0 / ( values.Length * values.Length ) );
			}

			var smallX = new MessagePackString( new String( 'A', 16 ) );
			var smallY = new MessagePackString( MessagePackConvert.EncodeString( new String( 'A', 16 ) ), false );

			for ( int i = 0; i < iteration; i++ )
			{
				sw.Restart();
				Assert.That( smallX.Equals( smallY ), Is.True );
				sw.Stop();
				smallAvg = Math.Min( smallAvg, sw.Elapsed.Ticks * 10.0 );
			}

			var mediumX = new MessagePackString( new String( 'A', 1000 ) );
			var mediumY = new MessagePackString( MessagePackConvert.EncodeString( new String( 'A', 1000 ) ), false );

			for ( int i = 0; i < iteration; i++ )
			{
				sw.Restart();
				Assert.That( mediumX.Equals( mediumY ), Is.True );
				sw.Stop();
				mediumAvg = Math.Min( mediumAvg, sw.Elapsed.Ticks * 10.0 );
			}

			var largeX = new MessagePackString( new String( 'A', 100000 ) );
			var largeY = new MessagePackString( MessagePackConvert.EncodeString( new String( 'A', 100000 ) ), false );

			for ( int i = 0; i < iteration; i++ )
			{
				sw.Restart();
				Assert.That( largeX.Equals( largeY ), Is.True );
				sw.Stop();
				largeAvg = Math.Min( largeAvg, sw.Elapsed.Ticks * 10.0 );
			}

			return Tuple.Create( tinyAvg, smallAvg, mediumAvg, largeAvg );
		}
	}
}
