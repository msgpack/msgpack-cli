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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using MsgPack.Serialization.ReflectionSerializers;

using NUnit.Framework;

namespace MsgPack.Serialization
{
	[TestFixture]
	public class AotTest
	{
		[TestFixtureSetUp]
		public static void SetupFixture()
		{
			MessagePackSerializer.PrepareType<Timestamp>();
			MessagePackSerializer.PrepareCollectionType<byte>();
			MessagePackSerializer.PrepareCollectionType<char>();
			MessagePackSerializer.PrepareCollectionType<int>();
			MessagePackSerializer.PrepareCollectionType<float>();
			MessagePackSerializer.PrepareCollectionType<double>();
			MessagePackSerializer.PrepareCollectionType<short>();
			MessagePackSerializer.PrepareCollectionType<uint>();
			MessagePackSerializer.PrepareCollectionType<ulong>();
			MessagePackSerializer.PrepareCollectionType<sbyte>();
			MessagePackSerializer.PrepareDictionaryType<string, int>();
			new ArraySegmentEqualityComparer<byte>().Equals( default( ArraySegment<byte> ), default( ArraySegment<byte> ) );
			new ArraySegmentEqualityComparer<char>().Equals( default( ArraySegment<char> ), default( ArraySegment<char> ) );
			new ArraySegmentEqualityComparer<int>().Equals( default( ArraySegment<int> ), default( ArraySegment<int> ) );
		}

		[Test]
		public void TestGenericDefaultSerializer_ArraySegmentOfByte()
		{
			TestGenericDefaultSerializerCore( new ArraySegment<byte>( new byte[] { 1 } ), new ArraySegmentEqualityComparer<byte>().Equals );
		}

		[Test]
		public void TestGenericDefaultSerializer_ArraySegmentOfChar()
		{
			TestGenericDefaultSerializerCore( new ArraySegment<char>( new[] { 'a' } ), new ArraySegmentEqualityComparer<char>().Equals );
		}

		[Test]
		public void TestGenericDefaultSerializer_ArraySegmentOfInt32()
		{
			TestGenericDefaultSerializerCore( new ArraySegment<int>( new[] { 1 } ), new ArraySegmentEqualityComparer<int>().Equals );
		}

		[Test]
		public void TestGenericDefaultSerializer_KeyValuePair()
		{
			TestGenericDefaultSerializerCore( new KeyValuePair<string, int>( "A", 1 ), ( x, y ) => x.Key == y.Key && x.Value == y.Value );
		}

		[Test]
		public void TestGenericDefaultSerializer_Stack()
		{
			var stack = new Stack<int>(2);
			stack.Push( 1 );
			stack.Push( 2 );
			TestGenericDefaultSerializerCore( stack, Enumerable.SequenceEqual );
		}

		[Test]
		public void TestGenericDefaultSerializer_Queue()
		{
			var queue = new Queue<int>(2);
			queue.Enqueue( 1 );
			queue.Enqueue( 2 );
			TestGenericDefaultSerializerCore( queue, Enumerable.SequenceEqual );
		}

		[Test]
		public void TestGenericDefaultSerializer_List()
		{
			TestGenericDefaultSerializerCore( new List<int>( 2 ) { 1, 2 }, Enumerable.SequenceEqual );
		}

		[Test]
		public void TestGenericDefaultSerializer_ListOfMessagePackObject()
		{
			TestGenericDefaultSerializerCore( new List<MessagePackObject>( 2 ) { 1, 2 }, Enumerable.SequenceEqual );
		}

		[Test]
		public void TestGenericDefaultSerializer_Dictionary()
		{
			TestGenericDefaultSerializerCore( new Dictionary<string, int>( 2 ) { { "A", 1 }, { "B", 2 } }, Enumerable.SequenceEqual );
		}

		private static void TestGenericDefaultSerializerCore<T>( T value, Func<T, T, bool> comparer )
		{
			var context = new SerializationContext( PackerCompatibilityOptions.None );
			var serializer = context.GetSerializer<T>();
			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, value );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( comparer( value, result ), "  Expected: {1}{0}  Actual :{2}", Environment.NewLine, value, result );
			}
		}
		
		[Test]
		public void TestTypeMetadataExtraction()
		{
			VerifyType( typeof( WithMessagePackMember ), new[] { "B", "A" }, new string[ 0 ] );
			VerifyType( typeof( ComplexTypeWithDataContractWithOrder ), new[] { "Source", "Data", "TimeStamp", "History" }, new[] { "History" } );
			VerifyType( typeof( ComplexTypeWithOneBaseOrder ), new[] { null, "One", "Two" }, new string[ 0 ] );
			VerifyType( typeof( DataMemberAttributeNamedPropertyTestTarget ), new[] { "Alias" }, new string[ 0 ] );
		}

		private static void VerifyType( Type type, string[] expectedMemberNames, string[] readOnlyMembers )
		{
			var context = new SerializationContext();
			var target = SerializationTarget.Prepare( context, type );
			Assert.That(
				target.Members.Count,
				Is.EqualTo( expectedMemberNames.Length ),
				"Some members are lacked.{0}  Expected:[{1}]{0}  Actual  :[{2}]",
				Environment.NewLine,
				String.Join( ", ", expectedMemberNames ),
				String.Join( ", ", target.Members.Select( m => String.Format("{{Name: {0}, Contract: {{Name: {1}, Id: {2}, NilImplication: {3}}}, Member: '{4}'}}", m.MemberName, m.Contract.Name, m.Contract.Id, m.Contract.NilImplication, m.Member) ).ToArray() )
			);

			for ( var i = 0; i < expectedMemberNames.Length; i++ )
			{
				Assert.That(
					target.Members[ i ].MemberName,
					Is.EqualTo( expectedMemberNames[ i ] ),
					"Member at index {1} is differ.{0}  Expected:[{2}]{0}  Actual  :[{3}]",
					Environment.NewLine,
					i,
					String.Join( ", ", expectedMemberNames ),
					String.Join( ", ", target.Members.Select( m => m.MemberName + "@Id=" + m.Contract.Id ).ToArray() )
				);
			}

			Func<object, object>[] getters;
			Action<object, object>[] setters;
			MemberInfo[] memberInfos;
			DataMemberContract[] contracts;
			MessagePackSerializer[] serializers;
			ReflectionSerializerHelper.GetMetadata( type, target.Members, context, out getters, out setters, out memberInfos, out contracts, out serializers );

			Assert.That( getters.Length, Is.EqualTo( target.Members.Count ), "getters.Length" );
			Assert.That( setters.Length, Is.EqualTo( target.Members.Count ), "setters.Length" );
			Assert.That( memberInfos.Length, Is.EqualTo( target.Members.Count ), "memberInfos.Length" );
			Assert.That( contracts.Length, Is.EqualTo( target.Members.Count ), "contracts.Length" );
			Assert.That( serializers.Length, Is.EqualTo( target.Members.Count ), "serializers.Length" );

			for ( var i = 0; i < expectedMemberNames.Length; i++ )
			{
				if ( expectedMemberNames[ i ] == null )
				{
					Assert.That( getters[ i ], Is.Null, "getters[{0}]", i );
					Assert.That( setters[ i ], Is.Null, "setters[{0}]", i );
					Assert.That( memberInfos[ i ], Is.Null, "memberInfos[{0}]", i );
					Assert.That( contracts[ i ].Name, Is.Null, "contracts[{0}]", i );
					Assert.That( serializers[ i ], Is.Null, "serializers[{0}]", i );
				}
				else
				{
					Assert.That( getters[ i ], Is.Not.Null, "getters[{0}]", i );
					if ( readOnlyMembers.Contains( expectedMemberNames[ i ] ) )
					{
						Assert.That( setters[ i ], Is.Null, "setters[{0}]", i );
					}
					else
					{
						Assert.That( setters[ i ], Is.Not.Null, "setters[{0}]", i );
					}
					Assert.That( memberInfos[ i ], Is.Not.Null, "memberInfos[{0}]", i );
					Assert.That( contracts[ i ].Name, Is.Not.Null, "contracts[{0}]", i );
					Assert.That( serializers[ i ], Is.Not.Null, "serializers[{0}]", i );
				}
			}
		}

		public class WithMessagePackMember
		{
			[MessagePackMember( 0 )]
			public string B { get; set; }

			[MessagePackMember( 1 )]
			public string A { get; set; }
		}
	}
}
