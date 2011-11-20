#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using NUnit.Framework;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Specialized;

namespace MsgPack.Serialization
{
	[TestFixture]
	[Timeout( 1000 )]
	public partial class AutoMessagePackSerializerTest
	{
		private static bool _traceOn = true;

		[SetUp]
		public void SetUp()
		{
			if ( _traceOn )
			{
				DumpableSerializationMethodGeneratorManager.DefaultSerializationMethodGeneratorOption = SerializationMethodGeneratorOption.CanDump;
				//Tracer.Emit.Listeners.Clear();
				//Tracer.Emit.Switch.Level = SourceLevels.All;
				//Tracer.Emit.Listeners.Add( new ConsoleTraceListener() );
			}
		}

		[TearDown]
		public void TearDown()
		{
			if ( _traceOn )
			{
				try
				{
					DumpableSerializationMethodGeneratorManager.DumpTo();
					Console.WriteLine( "DumpAssemblyTo: {0}", Path.GetFullPath( "." ) );
				}
				finally
				{
					DumpableSerializationMethodGeneratorManager.Refresh();
				}
			}
		}

		[Test]
		public void TestInt32()
		{
			TestCore( 1, stream => Unpacking.UnpackInt32( stream ), null );
		}

		[Test]
		public void TestInt64()
		{
			TestCore( Int32.MaxValue + 1L, stream => Unpacking.UnpackInt64( stream ), null );
		}

		[Test]
		public void TestString()
		{
			TestCore( "abc", stream => Unpacking.UnpackString( stream ), null );
		}

		[Test]
		public void TestDateTime()
		{
			TestCore(
				DateTime.UtcNow,
				stream => MessagePackConvert.ToDateTime( Unpacking.UnpackInt64( stream ) ),
				CompareDateTime
			);
		}

		[Test]
		public void TestDateTimeOffset()
		{
			TestCore(
				DateTimeOffset.UtcNow,
				stream => MessagePackConvert.ToDateTimeOffset( Unpacking.UnpackInt64( stream ) ),
				( x, y ) => CompareDateTime( x.DateTime.ToUniversalTime(), y.DateTime.ToUniversalTime() )
			);
		}

		private static bool CompareDateTime( DateTime x, DateTime y )
		{
			return x.Date == y.Date && x.Hour == y.Hour && x.Minute == y.Minute && x.Second == y.Second && x.Millisecond == y.Millisecond;
		}

		[Test]
		public void TestUri()
		{
			TestCore( new Uri( "http://www.example.com" ), stream => new Uri( Unpacking.UnpackString( stream ) ), null );
		}

		[Test]
		public void TextComplexObject()
		{
			var target = new ComplexType() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
			TestCoreWithVerify( target );
		}

		[Test]
		public void TextComplexObjectTypeWithDataContract()
		{
			var target = new ComplexTypeWithDataContract() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
			target.NonSerialized = new DefaultTraceListener();
			TestCoreWithVerify( target );
		}

		[Test]
		public void TextComplexObjectTypeWithNonSerialized()
		{
			var target = new ComplexTypeWithNonSerialized() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
			target.NonSerialized = new DefaultTraceListener();
			TestCoreWithVerify( target );
		}

		private static void TestCore<T>( T value, Func<Stream, T> unpacking, Func<T, T, bool> comparer )
		{
			var safeComparer = comparer ?? EqualityComparer<T>.Default.Equals;
			var target = new AutoMessagePackSerializer<T>();
			using ( var buffer = new MemoryStream() )
			{
				new AutoMessagePackSerializer<T>().Pack( value, buffer );
				Console.WriteLine( "Length:{0}", buffer.Length );
				Console.WriteLine( BitConverter.ToString( buffer.ToArray() ) );
				buffer.Position = 0;
				T intermediate = unpacking( buffer );
				Assert.That( safeComparer( intermediate, value ), "Expected:{1}{0}Actual :{2}", Environment.NewLine, value, intermediate );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				Assert.That( safeComparer( unpacked, value ), "Expected:{1}{0}Actual :{2}", Environment.NewLine, value, unpacked );
			}
		}

		private static void TestCoreWithVerify<T>( T value )
			where T : IVerifiable
		{
			var target = new AutoMessagePackSerializer<T>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( value, buffer );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				unpacked.Verify( buffer );
			}
		}

		internal static void Verify<T>( T expected, T actual )
		{
			if ( expected is IEnumerable )
			{
				var expecteds = ( ( IEnumerable )expected ).Cast<Object>().ToArray();
				var actuals = ( ( IEnumerable )actual ).Cast<Object>().ToArray();
				Assert.That( expecteds.Length, Is.EqualTo( actuals.Length ) );
				for ( int i = 0; i < expecteds.Length; i++ )
				{
					if ( expecteds[ i ] is DateTime )
					{
						Assert.That(
							MillisecondsDateTimeComparer.Instance.Equals( ( DateTime )expecteds[ i ], ( DateTime )actuals[ i ] ),
							"Expected:[{2}]({3})[{1}]->{4}({5}){0}Actual :[{6}]({7})[{1}]->87}({9})",
							Environment.NewLine,
							i,
							String.Join( ", ", ( ( IEnumerable )expected ).Cast<Object>() ),
							expected == null ? "(null)" : expected.GetType().FullName,
							expecteds[ i ],
							expecteds[ i ] == null ? "(null)" : expecteds[ i ].GetType().FullName,
							String.Join( ", ", ( ( IEnumerable )actual ).Cast<Object>() ),
							actual == null ? "(null)" : actual.GetType().FullName,
							actuals[ i ],
							actuals[ i ] == null ? "(null)" : actuals[ i ].GetType().FullName
						);
					}
					else if ( expecteds[ i ] is DateTimeOffset )
					{
						Assert.That(
							MillisecondsDateTimeOffsetComparer.Instance.Equals( ( DateTimeOffset )expecteds[ i ], ( DateTimeOffset )actuals[ i ] ),
							"Expected:[{2}]({3})[{1}]->{4}({5}){0}Actual :[{6}]({7})[{1}]->{8}({9})",
							Environment.NewLine,
							i,
							String.Join( ", ", ( ( IEnumerable )expected ).Cast<Object>() ),
							expected == null ? "(null)" : expected.GetType().FullName,
							expecteds[ i ],
							expecteds[ i ] == null ? "(null)" : expecteds[ i ].GetType().FullName,
							String.Join( ", ", ( ( IEnumerable )actual ).Cast<Object>() ),
							actual == null ? "(null)" : actual.GetType().FullName,
							actuals[ i ],
							actuals[ i ] == null ? "(null)" : actuals[ i ].GetType().FullName
						);
					}
					else if ( expecteds[ i ] is IEnumerable )
					{
						Verify( expecteds[ i ], actuals[ i ] );
					}
					else
					{
						Assert.That(
							expecteds[ i ],
							Is.EqualTo( actuals[ i ] ),
							"Expected:[{2}]({3})[{1}]->{4}({5}){0}Actual :[{6}]({7})[{1}]->{8}({9})",
							Environment.NewLine,
							i,
							String.Join( ", ", ( ( IEnumerable )expected ).Cast<Object>() ),
							expected == null ? "(null)" : expected.GetType().FullName,
							expecteds[ i ],
							expecteds[ i ] == null ? "(null)" : expecteds[ i ].GetType().FullName,
							String.Join( ", ", ( ( IEnumerable )actual ).Cast<Object>() ),
							actual == null ? "(null)" : actual.GetType().FullName,
							actuals[ i ],
							actuals[ i ] == null ? "(null)" : actuals[ i ].GetType().FullName
						);
					}
				}
				return;
			}

			if ( expected is IStructuralEquatable )
			{
				Assert.That(
					( ( IStructuralEquatable )expected ).Equals( actual, EqualityComparer<T>.Default ),
					"Expected:{1}({2}){0}Actual :{3}({4})",
					Environment.NewLine,
					expected,
					expected == null ? "(null)" : expected.GetType().FullName,
					actual,
					actual == null ? "(null)" : actual.GetType().FullName
				);
				return;
			}

			Assert.That(
				EqualityComparer<T>.Default.Equals( expected, actual ),
					"Expected:{1}({2}){0}Actual :{3}({4})",
				Environment.NewLine,
				expected,
				expected == null ? "(null)" : expected.GetType().FullName,
				actual,
				actual == null ? "(null)" : actual.GetType().FullName
			);
		}

		[Test]
		public void TestNameValueCollection()
		{
			var target = new NameValueCollection();
			target.Add( null, "null-1" );
			target.Add( null, "null-2" );
			target.Add( String.Empty, "Empty-1" );
			target.Add( String.Empty, "Empty-1" );
			target.Add( "1", "1-1" );
			target.Add( "1", "1-2" );
			target.Add( "1", "1-3" );
			target.Add( "null", null );
			target.Add( "Empty", String.Empty );
			target.Add( "2", "2" );
			var serializer = new AutoMessagePackSerializer<NameValueCollection>();
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( target, stream );
				stream.Position = 0;
				NameValueCollection result = serializer.Unpack( stream );
				Assert.That( result.GetValues( null ), Is.EquivalentTo( new[] { "null1-", "null-2" } ) );
				Assert.That( result.GetValues( String.Empty ), Is.EquivalentTo( new[] { "Empty-1", "Empty-2" } ) );
				Assert.That( result.GetValues( "1" ), Is.EquivalentTo( new[] { "1-1", "1-2", "1-3" } ) );
				Assert.That( result.GetValues( "null" ), Is.EquivalentTo( new string[] { null } ) );
				Assert.That( result.GetValues( "Empty" ), Is.EquivalentTo( new string[] { String.Empty } ) );
				Assert.That( result.GetValues( "2" ), Is.EquivalentTo( new string[] { "2" } ) );
				Assert.That( result.Count, Is.EqualTo( target.Count ) );
			}
		}

		// TODO: テストケースを網羅的にする。少なくとも以下の内容入れる。
		// TODO: IEnumerable...?
		// TOOD: IDictionary
		// TODO: ICollection
		// TODO: int[]
		// TOOD: List<T>
		// TODO: Collection<T>
		// TODO: ReadOnlyCollection<T>
		// TODO: ConcurrentDictionary<T>
		//   more
		// それぞれ、トップ、別オブジェクト、Array、Map　のキーと値でやる。
		// 既定のマーシャラもテストする
		// 明示的なリポジトリもテスト。
		// そのあとは　RPC。
		// まずは MPO でやる。基本は　Test First。
		// TCP send -> TCP notify -> UDP send -> UDP notify
		// 0.1 Async TCP Comm
		// 0.2 Error
		// 0.3 IDL
		// 0.4 UDP
		// 0.5 Silverlight
		// 0.6 Extensibility
		// 0.7 Improve error handling

		// FIXME: NameValueCollection
	}
}