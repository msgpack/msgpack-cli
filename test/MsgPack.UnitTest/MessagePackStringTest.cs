#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
using System.Globalization;
#if NETFX_35
using Debug = System.Console; // For missing Debug.WriteLine(String, params Object[])
#endif // NETFX_35
#if !MSTEST
using NUnit.Framework;
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
	}
}
