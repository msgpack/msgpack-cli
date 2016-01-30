 
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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
	// This file was generated from UnpackerTest.Object.tt T4Template.
	// Do not modify this file. Edit UnpackerTest.Object.tt instead.

	[TestFixture]
	public partial class UnpackerTest_Object
	{
		[Test]
		public void TestReadObjectFromPositiveFixNum()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = 1;
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<byte>().GetValueOrDefault(), "Type: PositiveFixNum" );
					Assert.That( result.Equals( value ), "Value: " + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromNegativeFixNum()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = -1;
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<sbyte>().GetValueOrDefault(), "Type: NegativeFixNum" );
					Assert.That( result.Equals( value ), "Value: " + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromInt8()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = -32;
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<sbyte>().GetValueOrDefault(), "Type: Int8" );
					Assert.That( result.Equals( value ), "Value: " + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromUInt8()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = 0x80;
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<byte>().GetValueOrDefault(), "Type: UInt8" );
					Assert.That( result.Equals( value ), "Value: " + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromInt16()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Int16.MinValue;
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<short>().GetValueOrDefault(), "Type: Int16" );
					Assert.That( result.Equals( value ), "Value: " + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromUInt16()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = UInt16.MaxValue;
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<ushort>().GetValueOrDefault(), "Type: UInt16" );
					Assert.That( result.Equals( value ), "Value: " + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromInt32()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Int32.MinValue;
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<int>().GetValueOrDefault(), "Type: Int32" );
					Assert.That( result.Equals( value ), "Value: " + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromUInt32()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = UInt32.MaxValue;
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<uint>().GetValueOrDefault(), "Type: UInt32" );
					Assert.That( result.Equals( value ), "Value: " + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromInt64()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Int64.MinValue;
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<long>().GetValueOrDefault(), "Type: Int64" );
					Assert.That( result.Equals( value ), "Value: " + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromUInt64()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = UInt64.MaxValue;
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<ulong>().GetValueOrDefault(), "Type: UInt64" );
					Assert.That( result.Equals( value ), "Value: " + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromReal32()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Single.Epsilon;
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<float>().GetValueOrDefault(), "Type: Real32" );
					Assert.That( result.Equals( value ), "Value: " + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromReal64()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Double.Epsilon;
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<double>().GetValueOrDefault(), "Type: Real64" );
					Assert.That( result.Equals( value ), "Value: " + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromFixedArray()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Enumerable.Repeat( 1, 16 ).ToArray();
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsList, "Type: FixedArray" );
					Assert.That( result.AsList().SequenceEqual( value.Select( i => ( MessagePackObject )i ) ), "Value: " + result );
					Assert.That( result.AsList().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromArray16()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Enumerable.Repeat( 1, UInt16.MaxValue ).ToArray();
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsList, "Type: Array16" );
					Assert.That( result.AsList().SequenceEqual( value.Select( i => ( MessagePackObject )i ) ), "Value: " + result );
					Assert.That( result.AsList().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromArray32()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Enumerable.Repeat( 1, UInt16.MaxValue + 1 ).ToArray();
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsList, "Type: Array32" );
					Assert.That( result.AsList().SequenceEqual( value.Select( i => ( MessagePackObject )i ) ), "Value: " + result );
					Assert.That( result.AsList().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromFixedMap()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Enumerable.Range( 1, 16 ).ToDictionary( i => i, i => ~i );
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsDictionary, "Type: FixedMap" );
					DictionaryEquals( result.AsDictionary(), value );
					Assert.That( result.AsDictionary().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromMap16()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Enumerable.Range( 1, UInt16.MaxValue ).ToDictionary( i => i, i => ~i );
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsDictionary, "Type: Map16" );
					DictionaryEquals( result.AsDictionary(), value );
					Assert.That( result.AsDictionary().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromMap32()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Enumerable.Range( 1, UInt16.MaxValue + 1 ).ToDictionary( i => i, i => ~i );
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsDictionary, "Type: Map32" );
					DictionaryEquals( result.AsDictionary(), value );
					Assert.That( result.AsDictionary().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromFixedRaw()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = new String( Enumerable.Repeat( 'a', 16 ).ToArray() );
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<string>().GetValueOrDefault(), "Type: FixedRaw" );
					Assert.That( result.AsString().Equals( value ), "Value: " + result );
					Assert.That( result.AsString().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromStr8()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = new String( Enumerable.Repeat( 'a', Byte.MaxValue ).ToArray() );
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<string>().GetValueOrDefault(), "Type: Str8" );
					Assert.That( result.AsString().Equals( value ), "Value: " + result );
					Assert.That( result.AsString().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromStr16()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = new String( Enumerable.Repeat( 'a', UInt16.MaxValue + 1 ).ToArray() );
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<string>().GetValueOrDefault(), "Type: Str16" );
					Assert.That( result.AsString().Equals( value ), "Value: " + result );
					Assert.That( result.AsString().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromStr32()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = new String( Enumerable.Repeat( 'a', UInt16.MaxValue + 1 ).ToArray() );
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<string>().GetValueOrDefault(), "Type: Str32" );
					Assert.That( result.AsString().Equals( value ), "Value: " + result );
					Assert.That( result.AsString().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromBin8()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Enumerable.Repeat( ( byte )1, 16 ).ToArray();
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<byte[]>().GetValueOrDefault(), "Type: Bin8" );
					Assert.That( result.AsBinary().SequenceEqual( value ), "Value: " + result );
					Assert.That( result.AsBinary().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromBin16()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Enumerable.Repeat( ( byte )1, UInt16.MaxValue ).ToArray();
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<byte[]>().GetValueOrDefault(), "Type: Bin16" );
					Assert.That( result.AsBinary().SequenceEqual( value ), "Value: " + result );
					Assert.That( result.AsBinary().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		[Test]
		public void TestReadObjectFromBin32()
		{
			using ( var buffer = new MemoryStream() )
			using ( var packer = Packer.Create( buffer ) )
			{
				var value = Enumerable.Repeat( ( byte )1, UInt16.MaxValue + 1 ).ToArray();
				packer.Pack( value );
				buffer.Position = 0;
				using ( var unpacker = Unpacker.Create( buffer ) )
				{
					MessagePackObject result;
					Assert.That( unpacker.ReadObject( out result ) );
					Assert.That( result.IsTypeOf<byte[]>().GetValueOrDefault(), "Type: Bin32" );
					Assert.That( result.AsBinary().SequenceEqual( value ), "Value: " + result );
					Assert.That( result.AsBinary().Count(), Is.EqualTo( value.Count() ), "Count:" + result );
				}
			}
		}

		private static void DictionaryEquals( MessagePackObjectDictionary actual, IDictionary<int, int> expected )
		{
			foreach( var entry in actual )
			{
				int value;
				Assert.That( expected.TryGetValue( entry.Key.AsInt32(), out value ), "Key: " + entry.Key );
				Assert.That( entry.Value, Is.EqualTo( ( MessagePackObject )value ), "Value: " + entry.Key );
			}
		}
	}
}
