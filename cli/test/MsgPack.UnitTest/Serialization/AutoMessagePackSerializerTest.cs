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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using NUnit.Framework;

namespace MsgPack.Serialization
{
	[Timeout( 3000 )]
	public abstract partial class AutoMessagePackSerializerTest
	{
		private static bool _traceOn = false;
		protected static bool ReuseContext = true;

		protected abstract  SerializationContext GetSerializationContext();

		[SetUp]
		public void SetUp()
		{
			if ( _traceOn )
			{
				Tracer.Emit.Listeners.Clear();
				Tracer.Emit.Switch.Level = SourceLevels.All;
				Tracer.Emit.Listeners.Add( new ConsoleTraceListener() );
			}

			/*
			 * Core2 Duo 6300 1.83GHz Windows 7 x64:
			 * CanCollect : 4.35 sec
			 * CanDump    : 4.18 sec
			 * Fast       : 3.43 sec
			 * 
			 */
			SerializationMethodGeneratorManager.DefaultSerializationMethodGeneratorOption = SerializationMethodGeneratorOption.CanDump;
		}

		[TearDown]
		public void TearDown()
		{
			if ( _traceOn )
			{
				try
				{
					DefaultSerializationMethodGeneratorManager.DumpTo();
				}
				finally
				{
					DefaultSerializationMethodGeneratorManager.Refresh();
				}
			}
		}

		protected abstract MessagePackSerializer<T> CreateTarget<T>( SerializationContext context );

		[Test]
		public void TestUnpackTo()
		{
			var target = this.CreateTarget<Int32[]>( GetSerializationContext() );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, new[] { 1, 2 } );
				buffer.Position = 0;
				int[] result = new int[ 2 ];
				using ( var unpacker = Unpacker.Create( buffer, false ) )
				{
					unpacker.Read();
					target.UnpackTo( unpacker, result );
					Assert.That( result, Is.EqualTo( new[] { 1, 2 } ) );
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
		public void TestComplexObject()
		{
			var target = new ComplexType() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
			TestCoreWithVerify( target );
		}

		[Test]
		public void TestComplexObjectTypeWithDataContract()
		{
			var target = new ComplexTypeWithDataContract() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
			target.NonSerialized = new DefaultTraceListener();
			TestCoreWithVerify( target );
		}

		[Test]
		public void TestComplexObjectTypeWithNonSerialized()
		{
			var target = new ComplexTypeWithNonSerialized() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
			target.NonSerialized = new DefaultTraceListener();
			TestCoreWithVerify( target );
		}

		[Test]
		public void TestEnum()
		{
			TestCore( DayOfWeek.Sunday, stream => ( DayOfWeek )Enum.Parse( typeof( DayOfWeek ), Unpacking.UnpackString( stream ) ), ( x, y ) => x == y );
		}

		[Test]
		public void TestNameValueCollection()
		{
			var target = new NameValueCollection();
			target.Add( String.Empty, "Empty-1" );
			target.Add( String.Empty, "Empty-2" );
			target.Add( "1", "1-1" );
			target.Add( "1", "1-2" );
			target.Add( "1", "1-3" );
			target.Add( "null", null ); // This value will not be packed.
			target.Add( "Empty", String.Empty );
			target.Add( "2", "2" );
			var serializer = this.CreateTarget<NameValueCollection>( this.GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, target );
				stream.Position = 0;
				NameValueCollection result = serializer.Unpack( stream );
				Assert.That( result.GetValues( String.Empty ), Is.EquivalentTo( new[] { "Empty-1", "Empty-2" } ) );
				Assert.That( result.GetValues( "1" ), Is.EquivalentTo( new[] { "1-1", "1-2", "1-3" } ) );
				Assert.That( result.GetValues( "null" ), Is.Null );
				Assert.That( result.GetValues( "Empty" ), Is.EquivalentTo( new string[] { String.Empty } ) );
				Assert.That( result.GetValues( "2" ), Is.EquivalentTo( new string[] { "2" } ) );
				// null only value is not packed.
				Assert.That( result.Count, Is.EqualTo( target.Count - 1 ) );
			}
		}

		[Test]
		[ExpectedException( typeof( NotSupportedException ) )]
		public void TestNameValueCollection_NullKey()
		{
			var target = new NameValueCollection();
			target.Add( null, "null" );
			var serializer = this.CreateTarget<NameValueCollection>( this.GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, target );
			}
		}

		[Test]
		public void TestByteArrayContent()
		{
			var serializer = this.CreateTarget<byte[]>( this.GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new byte[] { 1, 2, 3, 4 } );
				stream.Position = 0;
				Assert.That( Unpacking.UnpackBinary( stream ).ToArray(), Is.EqualTo( new byte[] { 1, 2, 3, 4 } ) );
			}
		}

		[Test]
		public void TestCharArrayContent()
		{
			var serializer = this.CreateTarget<char[]>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new char[] { 'a', 'b', 'c', 'd' } );
				stream.Position = 0;
				Assert.That( Unpacking.UnpackString( stream ), Is.EqualTo( "abcd" ) );
			}
		}

		[Test]
		public void TestTuple1()
		{
			TestTupleCore( new Tuple<int>( 1 ) );
		}

		[Test]
		public void TestTuple7()
		{
			TestTupleCore( new Tuple<int, string, int, string, int, string, int>( 1, "2", 3, "4", 5, "6", 7 ) );
		}

		[Test]
		public void TestTuple8()
		{
			TestTupleCore(
				new Tuple<
				int, string, int, string, int, string, int,
				Tuple<string>>(
					1, "2", 3, "4", 5, "6", 7,
					new Tuple<string>( "8" )
				)
			);
		}

		[Test]
		public void TestTuple14()
		{
			TestTupleCore(
				new Tuple<
				int, string, int, string, int, string, int,
				Tuple<
				string, int, string, int, string, int, string
				>
				>(
					1, "2", 3, "4", 5, "6", 7,
					new Tuple<string, int, string, int, string, int, string>(
						"8", 9, "10", 11, "12", 13, "14"
					)
				)
			);
		}

		[Test]
		public void TestTuple15()
		{
			TestTupleCore(
				new Tuple<
				int, string, int, string, int, string, int,
				Tuple<
				string, int, string, int, string, int, string,
				Tuple<int>
				>
				>(
					1, "2", 3, "4", 5, "6", 7,
					new Tuple<string, int, string, int, string, int, string, Tuple<int>>(
						"8", 9, "10", 11, "12", 13, "14",
						new Tuple<int>( 15 )
					)
				)
			);
		}

		private void TestTupleCore<T>( T expected )
			where T : IStructuralEquatable
		{
			var serializer = this.CreateTarget<T>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, expected );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( expected ) );
			}
		}

		[Test]
		public void TestEmptyBytes()
		{
			var serializer = this.CreateTarget<byte[]>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new byte[ 0 ] );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( new byte[ 0 ] ) );
			}
		}

		[Test]
		public void TestEmptyString()
		{
			var serializer = this.CreateTarget<string>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, String.Empty );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( String.Empty ) );
			}
		}

		[Test]
		public void TestEmptyIntArray()
		{
			var serializer = this.CreateTarget<int[]>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new int[ 0 ] );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( new int[ 0 ] ) );
			}
		}

		[Test]
		public void TestEmptyKeyValuePairArray()
		{
			var serializer = this.CreateTarget<KeyValuePair<string, int>[]>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new KeyValuePair<string, int>[ 0 ] );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( new KeyValuePair<string, int>[ 0 ] ) );
			}
		}

		[Test]
		public void TestEmptyMap()
		{
			var serializer = this.CreateTarget<Dictionary<string, int>>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, new Dictionary<string, int>() );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( new Dictionary<string, int>() ) );
			}
		}

		[Test]
		public void TestNullable()
		{
			var serializer = this.CreateTarget<int?>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				serializer.Pack( stream, 1 );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( 1 ) );

				stream.Position = 0;
				serializer.Pack( stream, null );
				Assert.That( stream.Length, Is.EqualTo( 1 ) );
				stream.Position = 0;
				Assert.That( serializer.Unpack( stream ), Is.EqualTo( null ) );
			}
		}

		[Test]
		public void TestValueType_Success()
		{
			var serializer = this.CreateTarget<TestValueType>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new TestValueType() { StringField = "ABC", Int32ArrayField = new int[] { 1, 2, 3 }, DictionaryField = new Dictionary<int, int>() { { 1, 1 } } };
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.StringField, Is.EqualTo( value.StringField ) );
				Assert.That( result.Int32ArrayField, Is.EqualTo( value.Int32ArrayField ) );
				Assert.That( result.DictionaryField, Is.EqualTo( value.DictionaryField ) );
			}
		}

		[Test]
		[ExpectedException( typeof( NotSupportedException ) )]
		public void TestAbstractList_Fail()
		{
			var serializer = this.CreateTarget<IList<int>>( GetSerializationContext() );
			serializer.Unpack( new MemoryStream( new byte[] { 0x90 } ) );
		}

		[Test]
		[ExpectedException( typeof( NotSupportedException ) )]
		public void TestAbstractDictionary_Fail()
		{
			var serializer = this.CreateTarget<IDictionary<int, int>>( GetSerializationContext() );
			serializer.Unpack( new MemoryStream( new byte[] { 0x90 } ) );
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestHasInitOnlyField_Fail()
		{
			this.CreateTarget<HasInitOnlyField>( GetSerializationContext() );
		}

		[Test]
		[ExpectedException( typeof( SerializationException ) )]
		public void TestHasGetOnlyProperty_Fail()
		{
			this.CreateTarget<HasGetOnlyProperty>( GetSerializationContext() );
		}

		[Test]
		public void TestCollection_Success()
		{
			var serializer = this.CreateTarget<Collection<int>>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new Collection<int>() { 1, 2, 3 };
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.ToArray(), Is.EqualTo( new int[] { 1, 2, 3 } ) );
			}
		}

		[Test]
		public void TestIListValueType_Success()
		{
			var serializer = this.CreateTarget<ListValueType<int>>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new ListValueType<int>( 3 ) { 1, 2, 3 };
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.ToArray(), Is.EqualTo( new int[] { 1, 2, 3 } ) );
			}
		}

		[Test]
		public void TestIDictionaryValueType_Success()
		{
			var serializer = this.CreateTarget<DictionaryValueType<int, int>>( GetSerializationContext() );
			using ( var stream = new MemoryStream() )
			{
				var value = new DictionaryValueType<int, int>( 3 ) { { 1, 1 }, { 2, 2 }, { 3, 3 } };
				serializer.Pack( stream, value );
				stream.Position = 0;
				var result = serializer.Unpack( stream );
				Assert.That( result.ToArray(), Is.EquivalentTo( Enumerable.Range( 1, 3 ).Select( i => new KeyValuePair<int, int>( i, i ) ).ToArray() ) );
			}
		}

		// FIXME: init-only field, get-only property, Value type which implements IList<T> and has .ctor(int), Enumerator class which explicitly implements IEnumerator

		private void TestCore<T>( T value, Func<Stream, T> unpacking, Func<T, T, bool> comparer )
		{
			var safeComparer = comparer ?? EqualityComparer<T>.Default.Equals;
			var target = this.CreateTarget<T>( GetSerializationContext() );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				T intermediate = unpacking( buffer );
				Assert.That( safeComparer( intermediate, value ), "Expected:{1}{0}Actual :{2}", Environment.NewLine, value, intermediate );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				Assert.That( safeComparer( unpacked, value ), "Expected:{1}{0}Actual :{2}", Environment.NewLine, value, unpacked );
			}
		}

		private void TestCoreWithVerify<T>( T value )
			where T : IVerifiable
		{
			var target = this.CreateTarget<T>( GetSerializationContext() );
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, value );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				unpacked.Verify( buffer );
			}
		}

		internal static void Verify<T>( T expected, T actual )
		{
			if ( expected == null )
			{
				Assert.That( actual, Is.Null );
				return;
			}

			if ( expected.GetType().IsGenericType && expected.GetType().GetGenericTypeDefinition() == typeof( ArraySegment<> ) )
			{
				AssertArraySegmentEquals( expected, actual );
				return;
			}

			if ( expected is DateTime )
			{
				Assert.That(
					MillisecondsDateTimeComparer.Instance.Equals( ( DateTime )( object )expected, ( DateTime )( object )actual ),
					"Expected:{1}({2},{3}){0}Actual :{4}({5},{6})",
					Environment.NewLine,
					expected,
					expected == null ? "(null)" : expected.GetType().FullName,
					( ( DateTime )( object )expected ).Kind,
					actual,
					actual == null ? "(null)" : actual.GetType().FullName,
					( ( DateTime )( object )actual ).Kind
					);
				return;
			}
			else if ( expected is DateTimeOffset )
			{
				Assert.That(
					MillisecondsDateTimeOffsetComparer.Instance.Equals( ( DateTimeOffset )( object )expected, ( DateTimeOffset )( object )actual ),
				"Expected:{1}({2}){0}Actual :{3}({4})",
				Environment.NewLine,
				expected,
				expected == null ? "(null)" : expected.GetType().FullName,
				actual,
				actual == null ? "(null)" : actual.GetType().FullName
				);
				return;
			}

			if ( expected is IDictionary )
			{
				var actuals = ( IDictionary )actual;
				foreach ( DictionaryEntry entry in ( ( IDictionary )expected ) )
				{
					Assert.That( actuals.Contains( entry.Key ), "'{0}' is not in '[{1}]'", entry.Key, String.Join( ", ", actuals.Keys.OfType<object>() ) );
					Verify( entry.Value, actuals[ entry.Key ] );
				}
				return;
			}

			if ( expected is IEnumerable )
			{
				var expecteds = ( ( IEnumerable )expected ).Cast<Object>().ToArray();
				var actuals = ( ( IEnumerable )actual ).Cast<Object>().ToArray();
				Assert.That( expecteds.Length, Is.EqualTo( actuals.Length ) );
				for ( int i = 0; i < expecteds.Length; i++ )
				{
					Verify( expecteds[ i ], actuals[ i ] );
				}
				return;
			}

			if ( expected is IStructuralEquatable )
			{
				Assert.That(
					( ( IStructuralEquatable )expected ).Equals( actual, EqualityComparer<object>.Default ),
					"Expected:{1}({2}){0}Actual :{3}({4})",
					Environment.NewLine,
					expected,
					expected == null ? "(null)" : expected.GetType().FullName,
					actual,
					actual == null ? "(null)" : actual.GetType().FullName
				);
				return;
			}

			if ( expected is FILETIME )
			{
				var expectedFileTime = ( FILETIME )( object )expected;
				var actualFileTime = ( FILETIME )( object )actual;
				Verify(
					DateTime.FromFileTimeUtc( ( ( ( long )expectedFileTime.dwHighDateTime ) << 32 ) | ( expectedFileTime.dwLowDateTime & 0xffffffff ) ),
					DateTime.FromFileTimeUtc( ( ( ( long )actualFileTime.dwHighDateTime ) << 32 ) | ( actualFileTime.dwLowDateTime & 0xffffffff ) )
				);
				return;
			}

			if ( expected.GetType().IsGenericType && expected.GetType().GetGenericTypeDefinition() == typeof( KeyValuePair<,> ) )
			{
				Verify( ( ( dynamic )expected ).Key, ( ( dynamic )actual ).Key );
				Verify( ( ( dynamic )expected ).Value, ( ( dynamic )actual ).Value );
				return;
			}

			if ( expected is DictionaryEntry )
			{
				var expectedEntry = ( DictionaryEntry )( object )expected;
				var actualEntry = ( DictionaryEntry )( object )actual;

				if ( expectedEntry.Key == null )
				{
					Assert.That( ( ( MessagePackObject )actualEntry.Key ).IsNil );
				}
				else
				{
					Verify( ( MessagePackObject )expectedEntry.Key, ( MessagePackObject )actualEntry.Key );
				}


				if ( expectedEntry.Value == null )
				{
					Assert.That( ( ( MessagePackObject )actualEntry.Value ).IsNil );
				}
				else
				{
					Verify( ( MessagePackObject )expectedEntry.Value, ( MessagePackObject )actualEntry.Value );
				}

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

		private static void AssertArraySegmentEquals( object x, object y )
		{
			var type = typeof( ArraySegmentEqualityComparer<> ).MakeGenericType( x.GetType().GetGenericArguments()[ 0 ] );
			Assert.That(
				( bool )type.InvokeMember( "Equals", BindingFlags.InvokeMethod, null, Activator.CreateInstance( type ), new[] { x, y } ),
				"Expected:{1}{0}Actual :{2}",
				Environment.NewLine,
				x,
				y
			);
		}

		public struct TestValueType
		{
			public string StringField;
			public int[] Int32ArrayField;
			public Dictionary<int, int> DictionaryField;
		}

		public class HasInitOnlyField
		{
			public readonly string Field = "ABC";
		}

		public class HasGetOnlyProperty
		{
			public string Property { get { return "ABC"; } }
		}

		public struct ListValueType<T> : IList<T>
		{
			private readonly List<T> _underlying;

			public T this[ int index ]
			{
				get
				{
					if ( this._underlying == null )
					{
						throw new ArgumentOutOfRangeException( "index" );
					}

					return this._underlying[ index ];
				}
				set
				{
					if ( this._underlying == null )
					{
						throw new ArgumentOutOfRangeException( "index" );
					}

					this._underlying[ index ] = value;
				}
			}

			public int Count
			{
				get { return this._underlying == null ? 0 : this._underlying.Count; }
			}

			public ListValueType( int capacity )
			{
				this._underlying = new List<T>( capacity );
			}

			public void Add( T item )
			{
				this._underlying.Add( item );
			}

			public void CopyTo( T[] array, int arrayIndex )
			{
				this._underlying.CopyTo( array, arrayIndex );
			}

			public IEnumerator<T> GetEnumerator()
			{
				if ( this._underlying == null )
				{
					yield break;
				}

				foreach ( var item in this._underlying )
				{
					yield return item;
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			public int IndexOf( T item )
			{
				throw new NotImplementedException();
			}

			public void Insert( int index, T item )
			{
				throw new NotImplementedException();
			}

			public void RemoveAt( int index )
			{
				throw new NotImplementedException();
			}

			public void Clear()
			{
				throw new NotImplementedException();
			}

			public bool Contains( T item )
			{
				throw new NotImplementedException();
			}

			public bool IsReadOnly
			{
				get { throw new NotImplementedException(); }
			}

			public bool Remove( T item )
			{
				throw new NotImplementedException();
			}
		}

		public struct DictionaryValueType<TKey, TValue> : IDictionary<TKey, TValue>
		{
			private readonly Dictionary<TKey, TValue> _underlying;

			public int Count
			{
				get { return this._underlying == null ? 0 : this._underlying.Count; }
			}

			public TValue this[ TKey key ]
			{
				get
				{
					if ( this._underlying == null )
					{
						throw new NotSupportedException();
					}

					return this._underlying[ key ];
				}
				set
				{
					if ( this._underlying == null )
					{
						throw new NotSupportedException();
					}

					this._underlying[ key ] = value;
				}
			}

			public DictionaryValueType( int capacity )
			{
				this._underlying = new Dictionary<TKey, TValue>( capacity );
			}

			public void Add( TKey key, TValue value )
			{
				if ( this._underlying == null )
				{
					throw new NotSupportedException();
				}

				this._underlying.Add( key, value );
			}

			public void CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
			{
				if ( this._underlying == null )
				{
					return;
				}

				( this._underlying as ICollection<KeyValuePair<TKey, TValue>> ).CopyTo( array, arrayIndex );
			}

			public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
			{
				if ( this._underlying == null )
				{
					yield break;
				}

				foreach ( var entry in this._underlying )
				{
					yield return entry;
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			public bool ContainsKey( TKey key )
			{
				throw new NotImplementedException();
			}

			public ICollection<TKey> Keys
			{
				get { throw new NotImplementedException(); }
			}

			public bool Remove( TKey key )
			{
				throw new NotImplementedException();
			}

			public bool TryGetValue( TKey key, out TValue value )
			{
				throw new NotImplementedException();
			}

			public ICollection<TValue> Values
			{
				get { throw new NotImplementedException(); }
			}

			public void Add( KeyValuePair<TKey, TValue> item )
			{
				throw new NotImplementedException();
			}

			public void Clear()
			{
				throw new NotImplementedException();
			}

			public bool Contains( KeyValuePair<TKey, TValue> item )
			{
				throw new NotImplementedException();
			}


			public bool IsReadOnly
			{
				get { throw new NotImplementedException(); }
			}

			public bool Remove( KeyValuePair<TKey, TValue> item )
			{
				throw new NotImplementedException();
			}
		}

		// TODO: RPC
		// TCP send -> TCP notify -> UDP send -> UDP notify
		// 0.1 Async TCP Comm
		// 0.2 Error
		// 0.3 IDL
		// 0.4 UDP
		// 0.5 Silverlight
		// 0.6 Extensibility
		// 0.7 Improve error handling
	}
}