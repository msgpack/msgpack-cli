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
		public void TestIEquatableEquals_Identical_True()
		{
			Assert.That(
				new Timestamp( 1, 1 ).Equals( new Timestamp( 1, 1 ) ),
				Is.True
			);
		}

		[Test]
		public void TestEquals_Identical_True()
		{
			Assert.That(
				new Timestamp( 1, 1 ).Equals( ( object )new Timestamp( 1, 1 ) ),
				Is.True
			);
		}

		[Test]
		public void TestCompareTo_Identical_0()
		{
			Assert.That(
				new Timestamp( 1, 1 ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( 0 )
			);
		}

		[Test]
		public void TestIComparableCompareTo_Identical_0()
		{
			Assert.That(
				( ( IComparable )new Timestamp( 1, 1 ) ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( 0 )
			);
		}

		[Test]
		public void TestCompare_Identical_0()
		{
			Assert.That(
				Timestamp.Compare( new Timestamp( 1, 1 ), new Timestamp( 1, 1 ) ),
				Is.EqualTo( 0 )
			);
		}

		[Test]
		public void TestEqualityOperator_Identical_True()
		{
			Assert.That(
				new Timestamp( 1, 1 ) == new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestInequalityOperator_Identical_False()
		{
			Assert.That(
				new Timestamp( 1, 1 ) != new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestGreaterThanOperator_Identical_False()
		{
			Assert.That(
				new Timestamp( 1, 1 ) > new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestGreaterThanOrEqualOperator_Identical_True()
		{
			Assert.That(
				new Timestamp( 1, 1 ) >= new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestLessThanOperator_Identical_False()
		{
			Assert.That(
				new Timestamp( 1, 1 ) < new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestLessThanOrEqualOperator_Identical_True()
		{
			Assert.That(
				new Timestamp( 1, 1 ) <= new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestIEquatableEquals_LargeBySecond_False()
		{
			Assert.That(
				new Timestamp( 2, 1 ).Equals( new Timestamp( 1, 1 ) ),
				Is.False
			);
		}

		[Test]
		public void TestEquals_LargeBySecond_False()
		{
			Assert.That(
				new Timestamp( 2, 1 ).Equals( ( object )new Timestamp( 1, 1 ) ),
				Is.False
			);
		}

		[Test]
		public void TestCompareTo_LargeBySecond_1()
		{
			Assert.That(
				new Timestamp( 2, 1 ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( 1 )
			);
		}

		[Test]
		public void TestIComparableCompareTo_LargeBySecond_1()
		{
			Assert.That(
				( ( IComparable )new Timestamp( 2, 1 ) ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( 1 )
			);
		}

		[Test]
		public void TestCompare_LargeBySecond_1()
		{
			Assert.That(
				Timestamp.Compare( new Timestamp( 2, 1 ), new Timestamp( 1, 1 ) ),
				Is.EqualTo( 1 )
			);
		}

		[Test]
		public void TestEqualityOperator_LargeBySecond_False()
		{
			Assert.That(
				new Timestamp( 2, 1 ) == new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestInequalityOperator_LargeBySecond_True()
		{
			Assert.That(
				new Timestamp( 2, 1 ) != new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestGreaterThanOperator_LargeBySecond_True()
		{
			Assert.That(
				new Timestamp( 2, 1 ) > new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestGreaterThanOrEqualOperator_LargeBySecond_True()
		{
			Assert.That(
				new Timestamp( 2, 1 ) >= new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestLessThanOperator_LargeBySecond_False()
		{
			Assert.That(
				new Timestamp( 2, 1 ) < new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestLessThanOrEqualOperator_LargeBySecond_False()
		{
			Assert.That(
				new Timestamp( 2, 1 ) <= new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestIEquatableEquals_LargeByNanosecond_False()
		{
			Assert.That(
				new Timestamp( 1, 2 ).Equals( new Timestamp( 1, 1 ) ),
				Is.False
			);
		}

		[Test]
		public void TestEquals_LargeByNanosecond_False()
		{
			Assert.That(
				new Timestamp( 1, 2 ).Equals( ( object )new Timestamp( 1, 1 ) ),
				Is.False
			);
		}

		[Test]
		public void TestCompareTo_LargeByNanosecond_1()
		{
			Assert.That(
				new Timestamp( 1, 2 ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( 1 )
			);
		}

		[Test]
		public void TestIComparableCompareTo_LargeByNanosecond_1()
		{
			Assert.That(
				( ( IComparable )new Timestamp( 1, 2 ) ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( 1 )
			);
		}

		[Test]
		public void TestCompare_LargeByNanosecond_1()
		{
			Assert.That(
				Timestamp.Compare( new Timestamp( 1, 2 ), new Timestamp( 1, 1 ) ),
				Is.EqualTo( 1 )
			);
		}

		[Test]
		public void TestEqualityOperator_LargeByNanosecond_False()
		{
			Assert.That(
				new Timestamp( 1, 2 ) == new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestInequalityOperator_LargeByNanosecond_True()
		{
			Assert.That(
				new Timestamp( 1, 2 ) != new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestGreaterThanOperator_LargeByNanosecond_True()
		{
			Assert.That(
				new Timestamp( 1, 2 ) > new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestGreaterThanOrEqualOperator_LargeByNanosecond_True()
		{
			Assert.That(
				new Timestamp( 1, 2 ) >= new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestLessThanOperator_LargeByNanosecond_False()
		{
			Assert.That(
				new Timestamp( 1, 2 ) < new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestLessThanOrEqualOperator_LargeByNanosecond_False()
		{
			Assert.That(
				new Timestamp( 1, 2 ) <= new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestIEquatableEquals_LargeBySecondEvenIfNanosecondIsSmall_False()
		{
			Assert.That(
				new Timestamp( 2, 0 ).Equals( new Timestamp( 1, 1 ) ),
				Is.False
			);
		}

		[Test]
		public void TestEquals_LargeBySecondEvenIfNanosecondIsSmall_False()
		{
			Assert.That(
				new Timestamp( 2, 0 ).Equals( ( object )new Timestamp( 1, 1 ) ),
				Is.False
			);
		}

		[Test]
		public void TestCompareTo_LargeBySecondEvenIfNanosecondIsSmall_1()
		{
			Assert.That(
				new Timestamp( 2, 0 ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( 1 )
			);
		}

		[Test]
		public void TestIComparableCompareTo_LargeBySecondEvenIfNanosecondIsSmall_1()
		{
			Assert.That(
				( ( IComparable )new Timestamp( 2, 0 ) ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( 1 )
			);
		}

		[Test]
		public void TestCompare_LargeBySecondEvenIfNanosecondIsSmall_1()
		{
			Assert.That(
				Timestamp.Compare( new Timestamp( 2, 0 ), new Timestamp( 1, 1 ) ),
				Is.EqualTo( 1 )
			);
		}

		[Test]
		public void TestEqualityOperator_LargeBySecondEvenIfNanosecondIsSmall_False()
		{
			Assert.That(
				new Timestamp( 2, 0 ) == new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestInequalityOperator_LargeBySecondEvenIfNanosecondIsSmall_True()
		{
			Assert.That(
				new Timestamp( 2, 0 ) != new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestGreaterThanOperator_LargeBySecondEvenIfNanosecondIsSmall_True()
		{
			Assert.That(
				new Timestamp( 2, 0 ) > new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestGreaterThanOrEqualOperator_LargeBySecondEvenIfNanosecondIsSmall_True()
		{
			Assert.That(
				new Timestamp( 2, 0 ) >= new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestLessThanOperator_LargeBySecondEvenIfNanosecondIsSmall_False()
		{
			Assert.That(
				new Timestamp( 2, 0 ) < new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestLessThanOrEqualOperator_LargeBySecondEvenIfNanosecondIsSmall_False()
		{
			Assert.That(
				new Timestamp( 2, 0 ) <= new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestIEquatableEquals_SmallBySecond_False()
		{
			Assert.That(
				new Timestamp( 0, 1 ).Equals( new Timestamp( 1, 1 ) ),
				Is.False
			);
		}

		[Test]
		public void TestEquals_SmallBySecond_False()
		{
			Assert.That(
				new Timestamp( 0, 1 ).Equals( ( object )new Timestamp( 1, 1 ) ),
				Is.False
			);
		}

		[Test]
		public void TestCompareTo_SmallBySecond_Minus1()
		{
			Assert.That(
				new Timestamp( 0, 1 ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( -1 )
			);
		}

		[Test]
		public void TestIComparableCompareTo_SmallBySecond_Minus1()
		{
			Assert.That(
				( ( IComparable )new Timestamp( 0, 1 ) ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( -1 )
			);
		}

		[Test]
		public void TestCompare_SmallBySecond_Minus1()
		{
			Assert.That(
				Timestamp.Compare( new Timestamp( 0, 1 ), new Timestamp( 1, 1 ) ),
				Is.EqualTo( -1 )
			);
		}

		[Test]
		public void TestEqualityOperator_SmallBySecond_False()
		{
			Assert.That(
				new Timestamp( 0, 1 ) == new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestInequalityOperator_SmallBySecond_True()
		{
			Assert.That(
				new Timestamp( 0, 1 ) != new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestGreaterThanOperator_SmallBySecond_False()
		{
			Assert.That(
				new Timestamp( 0, 1 ) > new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestGreaterThanOrEqualOperator_SmallBySecond_False()
		{
			Assert.That(
				new Timestamp( 0, 1 ) >= new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestLessThanOperator_SmallBySecond_True()
		{
			Assert.That(
				new Timestamp( 0, 1 ) < new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestLessThanOrEqualOperator_SmallBySecond_True()
		{
			Assert.That(
				new Timestamp( 0, 1 ) <= new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestIEquatableEquals_SmallByNanosecond_False()
		{
			Assert.That(
				new Timestamp( 1, 0 ).Equals( new Timestamp( 1, 1 ) ),
				Is.False
			);
		}

		[Test]
		public void TestEquals_SmallByNanosecond_False()
		{
			Assert.That(
				new Timestamp( 1, 0 ).Equals( ( object )new Timestamp( 1, 1 ) ),
				Is.False
			);
		}

		[Test]
		public void TestCompareTo_SmallByNanosecond_Minus1()
		{
			Assert.That(
				new Timestamp( 1, 0 ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( -1 )
			);
		}

		[Test]
		public void TestIComparableCompareTo_SmallByNanosecond_Minus1()
		{
			Assert.That(
				( ( IComparable )new Timestamp( 1, 0 ) ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( -1 )
			);
		}

		[Test]
		public void TestCompare_SmallByNanosecond_Minus1()
		{
			Assert.That(
				Timestamp.Compare( new Timestamp( 1, 0 ), new Timestamp( 1, 1 ) ),
				Is.EqualTo( -1 )
			);
		}

		[Test]
		public void TestEqualityOperator_SmallByNanosecond_False()
		{
			Assert.That(
				new Timestamp( 1, 0 ) == new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestInequalityOperator_SmallByNanosecond_True()
		{
			Assert.That(
				new Timestamp( 1, 0 ) != new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestGreaterThanOperator_SmallByNanosecond_False()
		{
			Assert.That(
				new Timestamp( 1, 0 ) > new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestGreaterThanOrEqualOperator_SmallByNanosecond_False()
		{
			Assert.That(
				new Timestamp( 1, 0 ) >= new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestLessThanOperator_SmallByNanosecond_True()
		{
			Assert.That(
				new Timestamp( 1, 0 ) < new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestLessThanOrEqualOperator_SmallByNanosecond_True()
		{
			Assert.That(
				new Timestamp( 1, 0 ) <= new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestIEquatableEquals_SmallBySecondEvenIfNanosecondIsLarge_False()
		{
			Assert.That(
				new Timestamp( 0, 2 ).Equals( new Timestamp( 1, 1 ) ),
				Is.False
			);
		}

		[Test]
		public void TestEquals_SmallBySecondEvenIfNanosecondIsLarge_False()
		{
			Assert.That(
				new Timestamp( 0, 2 ).Equals( ( object )new Timestamp( 1, 1 ) ),
				Is.False
			);
		}

		[Test]
		public void TestCompareTo_SmallBySecondEvenIfNanosecondIsLarge_Minus1()
		{
			Assert.That(
				new Timestamp( 0, 2 ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( -1 )
			);
		}

		[Test]
		public void TestIComparableCompareTo_SmallBySecondEvenIfNanosecondIsLarge_Minus1()
		{
			Assert.That(
				( ( IComparable )new Timestamp( 0, 2 ) ).CompareTo( new Timestamp( 1, 1 ) ),
				Is.EqualTo( -1 )
			);
		}

		[Test]
		public void TestCompare_SmallBySecondEvenIfNanosecondIsLarge_Minus1()
		{
			Assert.That(
				Timestamp.Compare( new Timestamp( 0, 2 ), new Timestamp( 1, 1 ) ),
				Is.EqualTo( -1 )
			);
		}

		[Test]
		public void TestEqualityOperator_SmallBySecondEvenIfNanosecondIsLarge_False()
		{
			Assert.That(
				new Timestamp( 0, 2 ) == new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestInequalityOperator_SmallBySecondEvenIfNanosecondIsLarge_True()
		{
			Assert.That(
				new Timestamp( 0, 2 ) != new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestGreaterThanOperator_SmallBySecondEvenIfNanosecondIsLarge_False()
		{
			Assert.That(
				new Timestamp( 0, 2 ) > new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestGreaterThanOrEqualOperator_SmallBySecondEvenIfNanosecondIsLarge_False()
		{
			Assert.That(
				new Timestamp( 0, 2 ) >= new Timestamp( 1, 1 ),
				Is.False
			);
		}

		[Test]
		public void TestLessThanOperator_SmallBySecondEvenIfNanosecondIsLarge_True()
		{
			Assert.That(
				new Timestamp( 0, 2 ) < new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestLessThanOrEqualOperator_SmallBySecondEvenIfNanosecondIsLarge_True()
		{
			Assert.That(
				new Timestamp( 0, 2 ) <= new Timestamp( 1, 1 ),
				Is.True
			);
		}

		[Test]
		public void TestEquals_null_False()
		{
			Assert.That( new Timestamp( 1, 1 ).Equals( null ), Is.False );
		}

		[Test]
		public void TestIComparableCompareTo_null_AlwaysLarge()
		{
			Assert.That( ( ( IComparable )Timestamp.MinValue ).CompareTo( null ), Is.EqualTo( 1 ) );
		}
	}
}
