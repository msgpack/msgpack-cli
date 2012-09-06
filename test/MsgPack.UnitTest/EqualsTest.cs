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
using System.Text;
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
	public class EqualsTest
	{
		[Test]
		public void TestInt()
		{
			TestInt( 0 );
			TestInt( -1 );
			TestInt( 1 );
			TestInt( Int32.MinValue );
			TestInt( Int32.MaxValue );
			Random rand = new Random();
			for ( int i = 0; i < 1000; i++ )
			{
				TestInt( rand.Next() );
			}
		}

		private static void TestInt( int val )
		{
			MessagePackObject objInt = val;
			MessagePackObject objLong = ( long )val;
#pragma warning disable 1718
			Assert.IsTrue( objInt == objInt );
#pragma warning restore 1718
			Assert.IsTrue( objInt == objLong );
			Assert.IsTrue( objLong == objInt );
#pragma warning disable 1718
			Assert.IsTrue( objLong == objLong );
#pragma warning restore 1718
		}

		[Test]
		public void TestLong()
		{
			TestLong( 0 );
			TestLong( -1 );
			TestLong( 1 );
			TestLong( Int32.MinValue );
			TestLong( Int32.MaxValue );
			TestLong( Int64.MinValue );
			TestLong( Int64.MaxValue );
			Random rand = new Random();
			var buffer = new byte[ sizeof( long ) ];
			for ( int i = 0; i < 1000; i++ )
			{
				rand.NextBytes( buffer );
				TestLong( BitConverter.ToInt64( buffer, 0 ) );
			}
		}

		private static void TestLong( long val )
		{
			MessagePackObject objInt = unchecked( ( int )( val & 0xffffffff ) );
			MessagePackObject objLong = val;
			if ( val > ( long )Int32.MaxValue || val < ( long )Int32.MinValue )
			{
#pragma warning disable 1718
				Assert.IsTrue( objInt == objInt );
#pragma warning restore 1718
				Assert.IsFalse( objInt == objLong );
				Assert.IsFalse( objLong == objInt );
#pragma warning disable 1718
				Assert.IsTrue( objLong == objLong );
#pragma warning restore 1718
			}
			else
			{
#pragma warning disable 1718
				Assert.IsTrue( objInt == objInt );
#pragma warning restore 1718
				Assert.IsTrue( objInt == objLong );
				Assert.IsTrue( objLong == objInt );
#pragma warning disable 1718
				Assert.IsTrue( objLong == objLong );
#pragma warning restore 1718
			}
		}

		[Test]
		public void TestNil()
		{
#pragma warning disable 1718
			Assert.IsTrue( MessagePackObject.Nil == MessagePackObject.Nil );
#pragma warning restore 1718
			Assert.IsTrue( MessagePackObject.Nil == default( MessagePackObject ) );
			Assert.IsFalse( MessagePackObject.Nil == 0 );
			Assert.IsFalse( MessagePackObject.Nil == false );
		}

		[Test]
		public void TestString()
		{
			TestString( "" );
			TestString( "a" );
			TestString( "ab" );
			TestString( "abc" );
		}

		private static void TestString( String str )
		{
			Assert.IsTrue( new MessagePackObject( Encoding.UTF8.GetBytes( str ) ) == Encoding.UTF8.GetBytes( str ) );
		}
	}
}
