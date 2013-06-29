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
using System.Linq;
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
	public class MessagePackExtendedTypeObjectTest
	{
		[Test]
		public void TestIsValid()
		{
			Assert.That( new MessagePackExtendedTypeObject( 0, new byte[ 0 ] ).IsValid, Is.True );
			Assert.That( default( MessagePackExtendedTypeObject ).IsValid, Is.False );
		}

		[Test]
		public void TestConstructor()
		{
			Assert.DoesNotThrow( () => new MessagePackExtendedTypeObject( 0, new byte[ 1 ] ) );
			Assert.DoesNotThrow( () => new MessagePackExtendedTypeObject( 127, new byte[ 1 ] ) );
			Assert.Throws<ArgumentException>( () => new MessagePackExtendedTypeObject( 128, new byte[ 1 ] ) );
			Assert.Throws<ArgumentException>( () => new MessagePackExtendedTypeObject( 255, new byte[ 1 ] ) );
			Assert.DoesNotThrow( () => new MessagePackExtendedTypeObject( 0, new byte[ 0 ] ) );
			Assert.Throws<ArgumentNullException>( () => new MessagePackExtendedTypeObject( 0, null ) );
		}

		[Test]
		public void TestProperties()
		{
			byte typeCode = 0;
			var body = new byte[] { 1 };
			var actual = new MessagePackExtendedTypeObject( typeCode, body );

			Assert.That( actual.TypeCode, Is.EqualTo( typeCode ) );
			Assert.That( actual.Body, Is.SameAs( body ) );
			Assert.That( actual.GetBody(), Is.Not.SameAs( body ).And.EqualTo( body ) );
		}

		[Test]
		public void TestEquality_ValueEqual()
		{
			foreach ( var testCase in
				new[]
				{
					Tuple.Create( 0, 0, new byte[] {1}, new byte[] {1}, true ),
					Tuple.Create( 0, 1, new byte[] {1}, new byte[] {1}, false ),
					Tuple.Create( 0, 0, new byte[] {1}, new byte[] {1, 2}, false ),
					Tuple.Create( 0, 0, new byte[] {1}, new byte[] {2}, false ),
				} )
			{
				checked
				{
					var left = new MessagePackExtendedTypeObject( ( byte )testCase.Item1, testCase.Item3 );
					var right = new MessagePackExtendedTypeObject( ( byte )testCase.Item2, testCase.Item4 );

					Assert.That( left.Equals( right ), Is.EqualTo( testCase.Item5 ), "IEquatable.Equals" );
					Assert.That( left.Equals( ( object )right ), Is.EqualTo( testCase.Item5 ), "Equals" );
					Assert.That( left == right, Is.EqualTo( testCase.Item5 ), "==" );
					Assert.That( left != right, Is.EqualTo( !testCase.Item5 ), "!=" );
				}
			}
		}

		[Test]
		public void TestEquality_Self()
		{
			var left = new MessagePackExtendedTypeObject( 0, new byte[] { 1 } );
			var right = left;

			Assert.That( left.Equals( right ), Is.True, "IEquatable.Equals" );
			Assert.That( left.Equals( ( object )right ), Is.True, "Equals" );
			Assert.That( left == right, Is.True, "==" );
			Assert.That( left != right, Is.False, "!=" );
		}

		[Test]
		public void TestEquality_SameBody()
		{
			var left = new MessagePackExtendedTypeObject( 0, new byte[] { 1 } );
			var right = new MessagePackExtendedTypeObject( 0, left.Body );

			Assert.That( left.Equals( right ), Is.True, "IEquatable.Equals" );
			Assert.That( left.Equals( ( object )right ), Is.True, "Equals" );
			Assert.That( left == right, Is.True, "==" );
			Assert.That( left != right, Is.False, "!=" );
		}

		[Test]
		public void TestGetHashCode()
		{
			var testCases =
				new[]
				{
					// Select thinking code path and considerable values
					new byte[0],
					new byte[] {1},
					new byte[] {2},
					new byte[] {1, 2},
					new byte[] {1, 3},
					new byte[] {1, 2, 3, 4},
					new byte[] {1, 2, 3, 5},
					Enumerable.Range( 1, 8 ).Select( i => ( byte ) i ).ToArray(),
					Enumerable.Range( 1, 8 ).Select( i => ( byte ) i ).Reverse().ToArray(),
					Enumerable.Range( 1, 16 ).Select( i => ( byte ) i ).ToArray(),
					Enumerable.Range( 1, 16 ).Select( i => ( byte ) i ).Reverse().ToArray(),
					Enumerable.Range( 1, 32 ).Select( i => ( byte ) i ).ToArray(),
					Enumerable.Range( 1, 32 ).Select( i => ( byte ) i ).Reverse().ToArray(),
				};
			var result = new HashSet<int>();

			foreach ( var testCase in testCases )
			{
				foreach ( var typeCode in new byte[] { 0, 1 } )
				{
					checked
					{
						result.Add( new MessagePackExtendedTypeObject( typeCode, testCase ).GetHashCode() );
					}
				}
			}

			// Check variance
			Assert.That( result.Count, Is.GreaterThan( testCases.Length * 2 * 0.6 ) );
		}

		[Test]
		public void TestGethashCode_Null()
		{
			Assert.That( default( MessagePackExtendedTypeObject ).GetHashCode(), Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestToString()
		{
			Assert.That( new MessagePackExtendedTypeObject( 1, new byte[] {1} ).ToString(), Is.EqualTo( "[1]0x01" ) );
		}

		[Test]
		public void TestToString_Null()
		{
			Assert.That( default( MessagePackExtendedTypeObject ).ToString(), Is.Not.Null.And.Empty );
		}


		[Test]
		public void TestToStringJson()
		{
			var buffer = new StringBuilder();
			new MessagePackExtendedTypeObject( 1, new byte[] {1} ).ToString( buffer, true );
			Assert.That( buffer.ToString(), Is.EqualTo( "{\"TypeCode\":1, \"Body\":\"0x01\"}" ) );
		}

		[Test]
		public void TestToStringJson_Null()
		{
			var buffer = new StringBuilder();
			default (MessagePackExtendedTypeObject ).ToString( buffer, true );
			Assert.That( buffer.ToString(), Is.EqualTo( "null" ) );
		}
	}
}
