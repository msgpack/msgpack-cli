#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

#if AOT
#define NUNITLITE
#endif // AOT

using System;
using System.Linq;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
using Does = NUnit.Framework.Does;
#endif

namespace MsgPack
{
	[TestFixture]
	public class MessagePackObjectTest_Miscs
	{
		[Test]
		public void TestGetHashCode_AllPossibleTypes_Success()
		{
			TestGetHashCodeCore(
				MessagePackObject.Nil,
				true,
				false,
				1,
				-1,
				SByte.MinValue,
				Byte.MaxValue,
				Int16.MinValue,
				UInt16.MaxValue,
				Int32.MinValue,
				UInt32.MaxValue,
				Int64.MinValue,
				UInt64.MaxValue,
				Single.MaxValue,
				Single.Epsilon,
				Single.MinValue,
				Single.NaN,
				Single.PositiveInfinity,
				Single.NegativeInfinity,
				Double.MaxValue,
				Double.Epsilon,
				Double.MinValue,
				Double.NaN,
				Double.PositiveInfinity,
				Double.NegativeInfinity,
				String.Empty,
				new String( 'A', 1 ),
				new String( 'A', 31 ),
				new String( 'A', 32 ),
				new String( 'A', 0xFFFF ),
				new String( 'A', 0x10000 ),
				new byte[ 0 ],
				new byte[] { 1 },
				Enumerable.Repeat( ( byte )1, 31 ).ToArray(),
				Enumerable.Repeat( ( byte )1, 32 ).ToArray(),
				Enumerable.Repeat( ( byte )1, 0xFFFF ).ToArray(),
				Enumerable.Repeat( ( byte )1, 0x10000 ).ToArray(),
				new MessagePackObject[ 0 ],
				new MessagePackObject[] { 1 },
				Enumerable.Repeat( ( MessagePackObject )1, 31 ).ToArray(),
				Enumerable.Repeat( ( MessagePackObject )1, 32 ).ToArray(),
				Enumerable.Repeat( ( MessagePackObject )1, 0xFFFF ).ToArray(),
				Enumerable.Repeat( ( MessagePackObject )1, 0x10000 ).ToArray(),
				new MessagePackObject( new MessagePackObjectDictionary() ),
				new MessagePackObject( new MessagePackObjectDictionary() { { 1, 1 } } ),
				new MessagePackObject( new MessagePackObjectDictionary( Enumerable.Range( 1, 15 ).ToDictionary( i => new MessagePackObject( i ), i => new MessagePackObject( i ), MessagePackObjectEqualityComparer.Instance ) ) ),
				new MessagePackObject( new MessagePackObjectDictionary( Enumerable.Range( 1, 16 ).ToDictionary( i => new MessagePackObject( i ), i => new MessagePackObject( i ), MessagePackObjectEqualityComparer.Instance ) ) ),
				new MessagePackObject( new MessagePackObjectDictionary( Enumerable.Range( 1, 0xFFFF ).ToDictionary( i => new MessagePackObject( i ), i => new MessagePackObject( i ), MessagePackObjectEqualityComparer.Instance ) ) ),
				new MessagePackObject( new MessagePackObjectDictionary( Enumerable.Range( 1, 0x10000 ).ToDictionary( i => new MessagePackObject( i ), i => new MessagePackObject( i ), MessagePackObjectEqualityComparer.Instance ) ) )
			);
		}

		private static void TestGetHashCodeCore( params MessagePackObject[] targets )
		{
			foreach ( var target in targets )
			{
				target.GetHashCode();
				// OK. The value is implementation specific.
			}
		}

		[Test]
		public void TestToString_Binary_Hex()
		{
			Assert.AreEqual(
				"0xFFEDCBA98765432100",
				new MessagePackObject( new byte[] { 0xFF, 0xED, 0xCB, 0xA9, 0x87, 0x65, 0x43, 0x21, 0x00 } ).ToString()
			);
		}

		[Test]
		public void TestToString_ExtendedTypeObject_AsIs()
		{
			var mpeto = new MessagePackExtendedTypeObject( 123, new byte[] {1, 2, 3} );
			Assert.AreEqual(
				mpeto.ToString(),
				new MessagePackObject( mpeto ).ToString()
			);
		}

		[Test]
		public void TestToString_AllPossibleTypes_Success()
		{
#if MONO || ENABLE_MONO || ( XAMARIN && !AOT ) || UNITY
			Assert.Inconclusive( "Mono Regex causes StackOverflow... ");
#endif
			TestToStringCore(
				MessagePackObject.Nil,
				true,
				false,
				1,
				-1,
				SByte.MinValue,
				Byte.MaxValue,
				Int16.MinValue,
				UInt16.MaxValue,
				Int32.MinValue,
				UInt32.MaxValue,
				Int64.MinValue,
				UInt64.MaxValue,
				Single.MaxValue,
#if UNITY_WORKAROUND // Epsilon is 0 in Unity
				Single.Epsilon,
#endif // UNITY_WORKAROUND
				Single.MinValue,
				Single.NaN,
				Single.PositiveInfinity,
				Single.NegativeInfinity,
				Double.MaxValue,
#if UNITY_WORKAROUND // Epsilon is 0 in Unity
				Double.Epsilon,
#endif // UNITY_WORKAROUND
				Double.MinValue,
				Double.NaN,
				Double.PositiveInfinity,
				Double.NegativeInfinity,
				String.Empty,
				new String( 'A', 1 ),
				new String( 'A', 31 ),
				new String( 'A', 32 ),
				new String( 'A', 0xFFFF ),
				new String( 'A', 0x10000 ),
				new byte[ 0 ],
				new byte[] { 0xFF },
				Enumerable.Repeat( ( byte )0xFF, 31 ).ToArray(),
				Enumerable.Repeat( ( byte )0xFF, 32 ).ToArray(),
				Enumerable.Repeat( ( byte )0xFF, 0xFFFF ).ToArray(),
				Enumerable.Repeat( ( byte )0xFF, 0x10000 ).ToArray(),
				new MessagePackObject[ 0 ],
				new MessagePackObject[] { 1 },
				Enumerable.Repeat( ( MessagePackObject )1, 31 ).ToArray(),
				Enumerable.Repeat( ( MessagePackObject )1, 32 ).ToArray(),
				Enumerable.Repeat( ( MessagePackObject )1, 0xFFFF ).ToArray(),
				Enumerable.Repeat( ( MessagePackObject )1, 0x10000 ).ToArray(),
				new MessagePackObject( new MessagePackObjectDictionary() ),
				new MessagePackObject( new MessagePackObjectDictionary() { { 1, 1 } } ),
				new MessagePackObject( new MessagePackObjectDictionary( Enumerable.Range( 1, 15 ).ToDictionary( i => new MessagePackObject( i ), i => new MessagePackObject( i ), MessagePackObjectEqualityComparer.Instance ) ) ),
				new MessagePackObject( new MessagePackObjectDictionary( Enumerable.Range( 1, 16 ).ToDictionary( i => new MessagePackObject( i ), i => new MessagePackObject( i ), MessagePackObjectEqualityComparer.Instance ) ) ),
				new MessagePackObject( new MessagePackObjectDictionary( Enumerable.Range( 1, 0xFFFF ).ToDictionary( i => new MessagePackObject( i ), i => new MessagePackObject( i ), MessagePackObjectEqualityComparer.Instance ) ) ),
				new MessagePackObject( new MessagePackObjectDictionary( Enumerable.Range( 1, 0x10000 ).ToDictionary( i => new MessagePackObject( i ), i => new MessagePackObject( i ), MessagePackObjectEqualityComparer.Instance ) ) )
			);
		}

		private static void TestToStringCore( params MessagePackObject[] targets )
		{
			string previous = null;
			int index = -1;
			foreach ( var target in targets )
			{
				index++;
				var result = target.ToString();
				var indicator = String.Format( "Index:{0}, Next to \"{1}\", '{2}'", index, previous, target.DebugDump() );
				previous = result;
				if ( target.IsNil )
				{
					Assert.That( result, Is.Empty );
				}
				else if ( target.IsRaw )
				{
					try
					{
						Assert.AreEqual( target.AsString(), result, indicator );
						continue;
					}
					catch ( InvalidOperationException ) { }

					Assert.That( result, Does.Match( "^(0x[0-9A-F]+)?$" ), indicator );
				}
				else if ( target.IsArray )
				{
					Assert.That( result, Does.Match( @"^\[\s*([0-9]+(,\s*[0-9]+)*)?\s*\]$" ), indicator );
				}
				else if ( target.IsDictionary )
				{
					Assert.That( result, Does.Match( @"^\{\s*([0-9]+\s*:\s*[0-9]+(,\s*[0-9]+\s*:\s*[0-9]+)*)?\s*\}$" ), indicator );
				}
				else if ( target.IsTypeOf<float>().GetValueOrDefault() || target.IsTypeOf<double>().GetValueOrDefault() )
				{
					Assert.That( result, Does.Match( "^(NaN|Infinity|-Infinity|(-?[0-9]+\\.[0-9]+(E(\\+|-)[0-9]+)?))$" ), indicator );
				}
				else if ( target.IsTypeOf<bool>().GetValueOrDefault() )
				{
					Assert.That( result, Does.Match( "^(True|False)$" ), indicator );
				}
				else
				{
					Assert.That( result, Does.Match( "^-?[0-9]+$" ), indicator );
				}
			}
		}

#if NUNITLITE
		private static class Does
		{
			public static NUnit.Framework.Constraints.Constraint Match( string regex )
			{
				return Is.StringMatching( regex );
			}
		}
#endif // NUNITLITE && !NETFX_CORE
    }
}
