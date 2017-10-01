#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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
#if !NET35 && !UNITY
#if !WINDOWS_PHONE
#if !UNITY || MSGPACK_UNITY_FULL
using System.Numerics;
#endif // !NET35 && !UNITY
#endif // !WINDOWS_PHONE
#endif // !UNITY || MSGPACK_UNITY_FULL

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
	partial class TimestampTest
	{
		[Test]
		public void TestAdd_TimeSpan_Same()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.Zero;
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdditionOperator_TimeSpan_Same()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.Zero;
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdd_TimeSpan_1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.FromSeconds( 1 );
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdditionOperator_TimeSpan_1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.FromSeconds( 1 );
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdd_TimeSpan_1Tick()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.FromTicks( 1 );
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 101 ) );
		}

		[Test]
		public void TestAdditionOperator_TimeSpan_1Tick()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.FromTicks( 1 );
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 101 ) );
		}

		[Test]
		public void TestAdd_TimeSpan_Minus1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.FromSeconds( -1 );
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdditionOperator_TimeSpan_Minus1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.FromSeconds( -1 );
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdd_TimeSpan_Minus1Tick()
		{
			var @base = new Timestamp( 1L, 101 );
			var operand = TimeSpan.FromTicks( -1 );
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdditionOperator_TimeSpan_Minus1Tick()
		{
			var @base = new Timestamp( 1L, 101 );
			var operand = TimeSpan.FromTicks( -1 );
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdd_TimeSpan_MoveUp()
		{
			var @base = new Timestamp( 1L, 999999900 );
			var operand = TimeSpan.FromTicks( 1 );
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestAdditionOperator_TimeSpan_MoveUp()
		{
			var @base = new Timestamp( 1L, 999999900 );
			var operand = TimeSpan.FromTicks( 1 );
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestAdd_TimeSpan_MoveDown()
		{
			var @base = new Timestamp( 1L, 99 );
			var operand = TimeSpan.FromTicks( -1 );
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 999999999 ) );
		}

		[Test]
		public void TestAdditionOperator_TimeSpan_MoveDown()
		{
			var @base = new Timestamp( 1L, 99 );
			var operand = TimeSpan.FromTicks( -1 );
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 999999999 ) );
		}

		[Test]
		public void TestAdd_TimeSpan_MaxPlus1Sec_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 0 );
			var operand = TimeSpan.FromSeconds( 1 );
			Assert.Throws<OverflowException>( () => @base.Add( operand ) );
	}

		[Test]
		public void TestAdditionOperator_TimeSpan_MaxPlus1Sec_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 0 );
			var operand = TimeSpan.FromSeconds( 1 );
			Assert.Throws<OverflowException>( () => { var x = @base + operand; } );
		}

		[Test]
		public void TestAdd_TimeSpan_MinMinus1Sec_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 999999999 );
			var operand = TimeSpan.FromSeconds( -1 );
			Assert.Throws<OverflowException>( () => @base.Add( operand ) );
	}

		[Test]
		public void TestAdditionOperator_TimeSpan_MinMinus1Sec_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 999999999 );
			var operand = TimeSpan.FromSeconds( -1 );
			Assert.Throws<OverflowException>( () => { var x = @base + operand; } );
		}

		[Test]
		public void TestAdd_TimeSpan_MaxPlus1Tick_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 999999900 );
			var operand = TimeSpan.FromTicks( 1 );
			Assert.Throws<OverflowException>( () => @base.Add( operand ) );
	}

		[Test]
		public void TestAdditionOperator_TimeSpan_MaxPlus1Tick_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 999999900 );
			var operand = TimeSpan.FromTicks( 1 );
			Assert.Throws<OverflowException>( () => { var x = @base + operand; } );
		}

		[Test]
		public void TestAdd_TimeSpan_MinMinus1ick_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 99 );
			var operand = TimeSpan.FromTicks( -1 );
			Assert.Throws<OverflowException>( () => @base.Add( operand ) );
	}

		[Test]
		public void TestAdditionOperator_TimeSpan_MinMinus1ick_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 99 );
			var operand = TimeSpan.FromTicks( -1 );
			Assert.Throws<OverflowException>( () => { var x = @base + operand; } );
		}

		[Test]
		public void TestSubtract_TimeSpan_Same()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.Zero;
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtractionOperator_TimeSpan_Same()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.Zero;
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtract_TimeSpan_1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.FromSeconds( 1 );
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtractionOperator_TimeSpan_1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.FromSeconds( 1 );
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtract_TimeSpan_1Tick()
		{
			var @base = new Timestamp( 1L, 101 );
			var operand = TimeSpan.FromTicks( 1 );
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtractionOperator_TimeSpan_1Tick()
		{
			var @base = new Timestamp( 1L, 101 );
			var operand = TimeSpan.FromTicks( 1 );
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtract_TimeSpan_Minus1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.FromSeconds( -1 );
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtractionOperator_TimeSpan_Minus1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.FromSeconds( -1 );
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtract_TimeSpan_Minus1Tick()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.FromTicks( -1 );
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 101 ) );
		}

		[Test]
		public void TestSubtractionOperator_TimeSpan_Minus1Tick()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = TimeSpan.FromTicks( -1 );
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 101 ) );
		}

		[Test]
		public void TestSubtract_TimeSpan_MoveUp()
		{
			var @base = new Timestamp( 1L, 999999900 );
			var operand = TimeSpan.FromTicks( -1 );
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestSubtractionOperator_TimeSpan_MoveUp()
		{
			var @base = new Timestamp( 1L, 999999900 );
			var operand = TimeSpan.FromTicks( -1 );
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestSubtract_TimeSpan_MoveDown()
		{
			var @base = new Timestamp( 1L, 99 );
			var operand = TimeSpan.FromTicks( 1 );
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 999999999 ) );
		}

		[Test]
		public void TestSubtractionOperator_TimeSpan_MoveDown()
		{
			var @base = new Timestamp( 1L, 99 );
			var operand = TimeSpan.FromTicks( 1 );
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 999999999 ) );
		}

		[Test]
		public void TestSubtract_TimeSpan_MaxPlus1Sec_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 0 );
			var operand = TimeSpan.FromSeconds( -1 );
			Assert.Throws<OverflowException>( () => @base.Subtract( operand ) );
		}

		[Test]
		public void TestSubtractionOperator_TimeSpan_MaxPlus1Sec_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 0 );
			var operand = TimeSpan.FromSeconds( -1 );
			Assert.Throws<OverflowException>( () => { var x = @base - operand; } );
		}

		[Test]
		public void TestSubtract_TimeSpan_MinMinus1Sec_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 999999999 );
			var operand = TimeSpan.FromSeconds( 1 );
			Assert.Throws<OverflowException>( () => @base.Subtract( operand ) );
		}

		[Test]
		public void TestSubtractionOperator_TimeSpan_MinMinus1Sec_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 999999999 );
			var operand = TimeSpan.FromSeconds( 1 );
			Assert.Throws<OverflowException>( () => { var x = @base - operand; } );
		}

		[Test]
		public void TestSubtract_TimeSpan_MaxPlus1Tick_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 999999900 );
			var operand = TimeSpan.FromTicks( -1 );
			Assert.Throws<OverflowException>( () => @base.Subtract( operand ) );
		}

		[Test]
		public void TestSubtractionOperator_TimeSpan_MaxPlus1Tick_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 999999900 );
			var operand = TimeSpan.FromTicks( -1 );
			Assert.Throws<OverflowException>( () => { var x = @base - operand; } );
		}

		[Test]
		public void TestSubtract_TimeSpan_MinMinus1Tick_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 99 );
			var operand = TimeSpan.FromTicks( 1 );
			Assert.Throws<OverflowException>( () => @base.Subtract( operand ) );
		}

		[Test]
		public void TestSubtractionOperator_TimeSpan_MinMinus1Tick_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 99 );
			var operand = TimeSpan.FromTicks( 1 );
			Assert.Throws<OverflowException>( () => { var x = @base - operand; } );
		}

#if !NET35 && !UNITY
#if !WINDOWS_PHONE
#if !UNITY || MSGPACK_UNITY_FULL

		[Test]
		public void TestAdd_BigInteger_Same()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = BigInteger.Zero;
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdditionOperator_BigInteger_Same()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = BigInteger.Zero;
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdd_BigInteger_1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = 1000000000;
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdditionOperator_BigInteger_1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = 1000000000;
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdd_BigInteger_1Nsec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = 1;
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 2 ) );
		}

		[Test]
		public void TestAdditionOperator_BigInteger_1Nsec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = 1;
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 2 ) );
		}

		[Test]
		public void TestAdd_BigInteger_Minus1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = -1000000000;
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdditionOperator_BigInteger_Minus1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = -1000000000;
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdd_BigInteger_Minus1Nsec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = -1;
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestAdditionOperator_BigInteger_Minus1Nsec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = -1;
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestAdd_BigInteger_MoveUp()
		{
			var @base = new Timestamp( 1L, 999999999 );
			var operand = 2;
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdditionOperator_BigInteger_MoveUp()
		{
			var @base = new Timestamp( 1L, 999999999 );
			var operand = 2;
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestAdd_BigInteger_MoveDown()
		{
			var @base = new Timestamp( 1L, 0 );
			var operand = -2;
			var result = @base.Add( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 999999998 ) );
		}

		[Test]
		public void TestAdditionOperator_BigInteger_MoveDown()
		{
			var @base = new Timestamp( 1L, 0 );
			var operand = -2;
			var result = @base + operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 999999998 ) );
		}

		[Test]
		public void TestAdd_BigInteger_MaxPlus1Sec_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 0 );
			var operand = 1000000000;
			Assert.Throws<OverflowException>( () => @base.Add( operand ) );
	}

		[Test]
		public void TestAdditionOperator_BigInteger_MaxPlus1Sec_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 0 );
			var operand = 1000000000;
			Assert.Throws<OverflowException>( () => { var x = @base + operand; } );
		}

		[Test]
		public void TestAdd_BigInteger_MinMinus1Sec_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 999999999 );
			var operand = -1000000000;
			Assert.Throws<OverflowException>( () => @base.Add( operand ) );
	}

		[Test]
		public void TestAdditionOperator_BigInteger_MinMinus1Sec_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 999999999 );
			var operand = -1000000000;
			Assert.Throws<OverflowException>( () => { var x = @base + operand; } );
		}

		[Test]
		public void TestAdd_BigInteger_MaxPlus1Nsec_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 999999999 );
			var operand = 1;
			Assert.Throws<OverflowException>( () => @base.Add( operand ) );
	}

		[Test]
		public void TestAdditionOperator_BigInteger_MaxPlus1Nsec_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 999999999 );
			var operand = 1;
			Assert.Throws<OverflowException>( () => { var x = @base + operand; } );
		}

		[Test]
		public void TestAdd_BigInteger_MinMinus1Nsec_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 0 );
			var operand = -1;
			Assert.Throws<OverflowException>( () => @base.Add( operand ) );
	}

		[Test]
		public void TestAdditionOperator_BigInteger_MinMinus1Nsec_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 0 );
			var operand = -1;
			Assert.Throws<OverflowException>( () => { var x = @base + operand; } );
		}

		[Test]
		public void TestSubtract_BigInteger_Same()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = BigInteger.Zero;
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtractionOperator_BigInteger_Same()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = BigInteger.Zero;
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtract_BigInteger_1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = 1000000000;
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtractionOperator_BigInteger_1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = 1000000000;
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtract_BigInteger_1Nsec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = 1;
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestSubtractionOperator_BigInteger_1Nsec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = 1;
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestSubtract_BigInteger_Minus1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = -1000000000;
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtractionOperator_BigInteger_Minus1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = -1000000000;
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtract_BigInteger_Minus1Nsec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = -1;
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 2 ) );
		}

		[Test]
		public void TestSubtractionOperator_BigInteger_Minus1Nsec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = -1;
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 1 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 2 ) );
		}

		[Test]
		public void TestSubtract_BigInteger_MoveUp()
		{
			var @base = new Timestamp( 1L, 999999999 );
			var operand = -2;
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtractionOperator_BigInteger_MoveUp()
		{
			var @base = new Timestamp( 1L, 999999999 );
			var operand = -2;
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 2 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestSubtract_BigInteger_MoveDown()
		{
			var @base = new Timestamp( 1L, 0 );
			var operand = 2;
			var result = @base.Subtract( operand );
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 999999998 ) );
		}

		[Test]
		public void TestSubtractionOperator_BigInteger_MoveDown()
		{
			var @base = new Timestamp( 1L, 0 );
			var operand = 2;
			var result = @base - operand;
			Assert.That( result.UnixEpochSecondsPart, Is.EqualTo( 0 ) );
			Assert.That( result.NanosecondsPart, Is.EqualTo( 999999998 ) );
		}

		[Test]
		public void TestSubtract_BigInteger_MaxPlus1Sec_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 0 );
			var operand = -1000000000;
			Assert.Throws<OverflowException>( () => @base.Subtract( operand ) );
		}

		[Test]
		public void TestSubtractionOperator_BigInteger_MaxPlus1Sec_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 0 );
			var operand = -1000000000;
			Assert.Throws<OverflowException>( () => { var x = @base - operand; } );
		}

		[Test]
		public void TestSubtract_BigInteger_MinMinus1Sec_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 999999999 );
			var operand = 1000000000;
			Assert.Throws<OverflowException>( () => @base.Subtract( operand ) );
		}

		[Test]
		public void TestSubtractionOperator_BigInteger_MinMinus1Sec_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 999999999 );
			var operand = 1000000000;
			Assert.Throws<OverflowException>( () => { var x = @base - operand; } );
		}

		[Test]
		public void TestSubtract_BigInteger_MaxPlus1Nsec_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 999999999 );
			var operand = -1;
			Assert.Throws<OverflowException>( () => @base.Subtract( operand ) );
		}

		[Test]
		public void TestSubtractionOperator_BigInteger_MaxPlus1Nsec_Overflow()
		{
			var @base = new Timestamp( 9223372036854775807L, 999999999 );
			var operand = -1;
			Assert.Throws<OverflowException>( () => { var x = @base - operand; } );
		}

		[Test]
		public void TestSubtract_BigInteger_MinMinus1Nsec_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 0 );
			var operand = 1;
			Assert.Throws<OverflowException>( () => @base.Subtract( operand ) );
		}

		[Test]
		public void TestSubtractionOperator_BigInteger_MinMinus1Nsec_Overflow()
		{
			var @base = new Timestamp( -9223372036854775808L, 0 );
			var operand = 1;
			Assert.Throws<OverflowException>( () => { var x = @base - operand; } );
		}

		[Test]
		public void TestSubtract_Timestamp_Same()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = new Timestamp( 1L, 1 );
			var result = @base.Subtract( operand );
			Assert.That( result, Is.EqualTo( new BigInteger( 0 ) ) );
		}

		[Test]
		public void TestSubtractionOperator_Timstamp_Same()
		{
			var @base = new Timestamp( 1, 1 );
			var operand = new Timestamp( 1, 1 );
			var result = @base - operand;
			Assert.That( result, Is.EqualTo( new BigInteger( 0 ) ) );
		}

		[Test]
		public void TestSubtract_Timestamp_1Sec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = new Timestamp( 1L, 0 );
			var result = @base.Subtract( operand );
			Assert.That( result, Is.EqualTo( new BigInteger( 1 ) ) );
		}

		[Test]
		public void TestSubtractionOperator_Timstamp_1Sec()
		{
			var @base = new Timestamp( 1, 1 );
			var operand = new Timestamp( 1, 0 );
			var result = @base - operand;
			Assert.That( result, Is.EqualTo( new BigInteger( 1 ) ) );
		}

		[Test]
		public void TestSubtract_Timestamp_1Nsec()
		{
			var @base = new Timestamp( 1L, 1 );
			var operand = new Timestamp( 0L, 1 );
			var result = @base.Subtract( operand );
			Assert.That( result, Is.EqualTo( new BigInteger( 1000000000 ) ) );
		}

		[Test]
		public void TestSubtractionOperator_Timstamp_1Nsec()
		{
			var @base = new Timestamp( 1, 1 );
			var operand = new Timestamp( 0, 1 );
			var result = @base - operand;
			Assert.That( result, Is.EqualTo( new BigInteger( 1000000000 ) ) );
		}

		[Test]
		public void TestSubtract_Timestamp_MoveDown()
		{
			var @base = new Timestamp( 2L, 1 );
			var operand = new Timestamp( 1L, 2 );
			var result = @base.Subtract( operand );
			Assert.That( result, Is.EqualTo( new BigInteger( 999999999 ) ) );
		}

		[Test]
		public void TestSubtractionOperator_Timstamp_MoveDown()
		{
			var @base = new Timestamp( 2, 1 );
			var operand = new Timestamp( 1, 2 );
			var result = @base - operand;
			Assert.That( result, Is.EqualTo( new BigInteger( 999999999 ) ) );
		}

		[Test]
		public void TestSubtract_Timestamp_PositiveNegative()
		{
			var @base = new Timestamp( 1L, 2 );
			var operand = new Timestamp( -1L, 1 );
			var result = @base.Subtract( operand );
			Assert.That( result, Is.EqualTo( new BigInteger( 2000000001 ) ) );
		}

		[Test]
		public void TestSubtractionOperator_Timstamp_PositiveNegative()
		{
			var @base = new Timestamp( 1, 2 );
			var operand = new Timestamp( -1, 1 );
			var result = @base - operand;
			Assert.That( result, Is.EqualTo( new BigInteger( 2000000001 ) ) );
		}

		[Test]
		public void TestSubtract_Timestamp_MaxMin()
		{
			var @base = new Timestamp( 9223372036854775807L, 999999999 );
			var operand = new Timestamp( -9223372036854775808L, 0 );
			var result = @base.Subtract( operand );
			Assert.That( result, Is.EqualTo( new BigInteger( Int64.MaxValue ) * 1000000000 + 999999999 - new BigInteger( Int64.MinValue ) * 1000000000 ) );
		}

		[Test]
		public void TestSubtractionOperator_Timstamp_MaxMin()
		{
			var @base = new Timestamp( 9223372036854775807, 999999999 );
			var operand = new Timestamp( -9223372036854775808, 0 );
			var result = @base - operand;
			Assert.That( result, Is.EqualTo( new BigInteger( Int64.MaxValue ) * 1000000000 + 999999999 - new BigInteger( Int64.MinValue ) * 1000000000 ) );
		}

		[Test]
		public void TestSubtract_Timestamp_MinMax()
		{
			var @base = new Timestamp( -9223372036854775808L, 0 );
			var operand = new Timestamp( 9223372036854775807L, 999999999 );
			var result = @base.Subtract( operand );
			Assert.That( result, Is.EqualTo( new BigInteger( Int64.MinValue ) * 1000000000 - new BigInteger( Int64.MaxValue ) * 1000000000 - 999999999 ) );
		}

		[Test]
		public void TestSubtractionOperator_Timstamp_MinMax()
		{
			var @base = new Timestamp( -9223372036854775808, 0 );
			var operand = new Timestamp( 9223372036854775807, 999999999 );
			var result = @base - operand;
			Assert.That( result, Is.EqualTo( new BigInteger( Int64.MinValue ) * 1000000000 - new BigInteger( Int64.MaxValue ) * 1000000000 - 999999999 ) );
		}


#endif // !NET35 && !UNITY
#endif // !WINDOWS_PHONE
#endif // !UNITY || MSGPACK_UNITY_FULL
	}
}
