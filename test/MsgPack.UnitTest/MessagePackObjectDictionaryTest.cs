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
using System.Collections;
using System.Collections.Generic;
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
using System.Text;
using System.IO;
#if !NETFX_CORE && !SILVERLIGHT
using System.Runtime.Serialization.Formatters.Binary;
#endif // !NETFX_CORE && !SILVERLIGHT

namespace MsgPack
{
	[TestFixture]
	[Timeout( 1000 )]
	public class MessagePackObjectDictionaryTest
	{
		[Test]
		public void TestAddForEachRemoveClearContains()
		{
			var target = new MessagePackObjectDictionary();
			// 0
			AssertCount( target, 0 );

			// Add
			foreach ( var i in Enumerable.Range( 0, 11 ) )
			{
				target.Add( i, i );
				Assert.That( target.ContainsKey( i ) );
				Assert.That( target.ContainsValue( i ) );
				Assert.That( target.Contains( new KeyValuePair<MessagePackObject, MessagePackObject>( i, i ) ) );
				Assert.That( target[ i ], Is.EqualTo( new MessagePackObject( i ) ) );
				MessagePackObject value;
				Assert.That( target.TryGetValue( i, out value ) );
				Assert.That( value, Is.EqualTo( new MessagePackObject( i ) ) );
				AssertCount( target, i + 1 );
			}

			// Remove
			foreach ( var i in Enumerable.Range( 0, 11 ) )
			{
				Assert.That( target.Remove( i ) );
				Assert.That( target.ContainsKey( i ), Is.False );
				Assert.That( target.ContainsValue( i ), Is.False );
				Assert.That( target.Contains( new KeyValuePair<MessagePackObject, MessagePackObject>( i, i ) ), Is.False );
				try
				{
					var v = target[ i ];
					Assert.Fail( "Exception is not thrown for removed key '{0}', returned '{1}'", i, v );
				}
				catch ( KeyNotFoundException ) { }

				MessagePackObject value;
				Assert.That( target.TryGetValue( i, out value ), Is.False );
				AssertCount( target, 10 - i );
			}

			// Add again
			foreach ( var i in Enumerable.Range( 0, 11 ) )
			{
				target.Add( i, i );
				Assert.That( target.ContainsKey( i ) );
				Assert.That( target.ContainsValue( i ) );
				Assert.That( target.Contains( new KeyValuePair<MessagePackObject, MessagePackObject>( i, i ) ) );
				Assert.That( target[ i ], Is.EqualTo( new MessagePackObject( i ) ) );
				AssertCount( target, i + 1 );
			}

			// Clear
			target.Clear();
			AssertCount( target, 0 );

			// Remove empty
			foreach ( var i in Enumerable.Range( 0, 11 ) )
			{
				Assert.That( target.Remove( i ), Is.False );
			}
		}

		[Test]
		public void TestIndexer()
		{
			var target = new MessagePackObjectDictionary();
			// 0
			AssertCount( target, 0 );

			foreach ( var count in new[] { 9, 10, 11 } )
			{
				target.Clear();

				// Add
				foreach ( var i in Enumerable.Range( 0, count ) )
				{
					target[ i ] = i;
					Assert.That( target.ContainsKey( i ) );
					Assert.That( target.ContainsValue( i ) );
					Assert.That( target.Contains( new KeyValuePair<MessagePackObject, MessagePackObject>( i, i ) ) );
					Assert.That( target[ i ], Is.EqualTo( new MessagePackObject( i ) ) );
					AssertCount( target, i + 1 );
				}

				// Overwrite with same
				foreach ( var i in Enumerable.Range( 0, count ) )
				{
					target[ i ] = i;
					Assert.That( target.ContainsKey( i ) );
					Assert.That( target.ContainsValue( i ) );
					Assert.That( target.Contains( new KeyValuePair<MessagePackObject, MessagePackObject>( i, i ) ) );
					Assert.That( target[ i ], Is.EqualTo( new MessagePackObject( i ) ) );
					AssertCount( target, count );
				}

				// Overwrite 
				foreach ( var i in Enumerable.Range( 0, count ) )
				{
					target[ i ] = i.ToString();
					Assert.That( target.ContainsKey( i ) );
					Assert.That( target.ContainsValue( i.ToString() ) );
					Assert.That( target.Contains( new KeyValuePair<MessagePackObject, MessagePackObject>( i, i.ToString() ) ) );
					Assert.That( target[ i ], Is.EqualTo( new MessagePackObject( i.ToString() ) ) );
					AssertCount( target, count );
				}
			}
		}

		private static void AssertCount( ICollection target, int expected )
		{
			Assert.That( target.Count, Is.EqualTo( expected ) );
			int count = 0;
			foreach ( var item in target )
			{
				count++;
			}

			Assert.That( count, Is.EqualTo( expected ), "GetEnumeraotr does not return expected count items." );
			Assert.That( target.OfType<KeyValuePair<MessagePackObject, MessagePackObject>>().Count(), Is.EqualTo( expected ) );
		}

		private static readonly object[] _nonGenericKeys =
			new object[]
				{
					true,
					false,
					0,
					( sbyte )-1,
					SByte.MinValue,
					( short )SByte.MinValue -1 ,
					Int16.MinValue,
					Int16.MinValue - 1,
					Int32.MinValue,
					( long )Int32.MinValue - 1,
					Int64.MinValue,
					( byte )1,
					Byte.MaxValue,
					( ushort )Byte.MaxValue + 1,
					UInt16.MaxValue,
					( uint )UInt16.MaxValue + 1,
					UInt32.MaxValue ,
					( ulong )UInt32.MaxValue + 1,
					UInt64.MaxValue,
					Single.MinValue,
					Single.MaxValue,
					Double.MinValue,
					Double.MaxValue,
					DateTime.UtcNow,
					new byte[] { 1 },
					String.Empty,
					" ",
					new MessagePackString( "a" )
				};

		[Test]
		public void TestIDictionaryAddForEachRemoveClearContains()
		{
			IDictionary target = new MessagePackObjectDictionary();
			// 0
			AssertCount( target, 0 );

			// Check late binding
			int previous = 0;

			foreach ( var key in _nonGenericKeys )
			{
				target.Add( key, previous );
				AssertCount( target, previous + 1 );
				Assert.That( target.Contains( key ) );
				Assert.That( ( MessagePackObject )target[ key ], Is.EqualTo( new MessagePackObject( previous ) ) );
				previous++;
			}

			// removes
			foreach ( var key in _nonGenericKeys )
			{
				target.Remove( key );
				AssertCount( target, previous - 1 );
				Assert.That( target.Contains( key ), Is.False );
				Assert.That( target[ key ], Is.Null );
				previous--;
			}

			foreach ( var key in _nonGenericKeys )
			{
				// No exceptions is thrown.
				target.Remove( key );
			}
		}

		[Test]
		public void TestIDictionaryIndexer()
		{
			IDictionary target = new MessagePackObjectDictionary();
			// 0
			AssertCount( target, 0 );

			// Check late binding
			int previous = 0;

			foreach ( var key in _nonGenericKeys )
			{
				target[ key ] = previous;
				AssertCount( target, previous + 1 );
				Assert.That( target.Contains( key ) );
				Assert.That( ( MessagePackObject )target[ key ], Is.EqualTo( new MessagePackObject( previous ) ) );
				previous++;
			}

			// Overrites same
			int i = 0;
			foreach ( var key in _nonGenericKeys )
			{
				target[ key ] = i;
				AssertCount( target, previous );
				Assert.That( target.Contains( key ) );
				Assert.That( ( MessagePackObject )target[ key ], Is.EqualTo( new MessagePackObject( i ) ) );
				i++;
			}

			// Overwrites
			i = 0;
			foreach ( var key in _nonGenericKeys )
			{
				target[ key ] = i.ToString();
				AssertCount( target, previous );
				Assert.That( target.Contains( key ) );
				Assert.That( ( MessagePackObject )target[ key ], Is.EqualTo( new MessagePackObject( i.ToString() ) ) );
				i++;
			}
		}

		[Test]
		public void TestAddNull()
		{
			// The spec is ambigious, but many languages do not allow null key, so this implementation should not allow null key.
			Assert.Throws<ArgumentNullException>( () => new MessagePackObjectDictionary().Add( MessagePackObject.Nil, 0 ) );
		}

		[Test]
		public void TestICollectionTAddNull()
		{
			// The spec is ambigious, but many languages do not allow null key, so this implementation should not allow null key.
			Assert.Throws<ArgumentNullException>( () =>
				( new MessagePackObjectDictionary() as ICollection<KeyValuePair<MessagePackObject, MessagePackObject>> )
					.Add( new KeyValuePair<MessagePackObject, MessagePackObject>( MessagePackObject.Nil, MessagePackObject.Nil ) )
			);
		}

		[Test]
		public void TestIDictionaryAddObjectNull()
		{
			// The spec is ambigious, but many languages do not allow null key, so this implementation should not allow null key.
			Assert.Throws<ArgumentNullException>( () => ( new MessagePackObjectDictionary() as IDictionary ).Add( null, 0 ) );
		}

		[Test]
		public void TestIDictionaryAddMessagePackObjectNull()
		{
			// The spec is ambigious, but many languages do not allow null key, so this implementation should not allow null key.
			Assert.Throws<ArgumentNullException>( () => ( new MessagePackObjectDictionary() as IDictionary ).Add( MessagePackObject.Nil, 0 ) );
		}

		[Test]
		public void TestGetNull()
		{
			// The spec is ambigious, but many languages do not allow null key, so this implementation should not allow null key.
			var target = new MessagePackObjectDictionary();
			target.Add( 0, 0 );
			Assert.Throws<ArgumentNullException>( () => { var v = target[ MessagePackObject.Nil ]; } );
		}

		[Test]
		public void TestIDictionaryGetObjectNull()
		{
			// The spec is ambigious, but many languages do not allow null key, so this implementation should not allow null key.
			IDictionary target = new MessagePackObjectDictionary();
			target.Add( 0, 0 );
			Assert.Throws<ArgumentNullException>( () => { var v = target[ null ]; } );
		}

		[Test]
		public void TestIDictionaryGetMessagePackObjectNull()
		{
			// The spec is ambigious, but many languages do not allow null key, so this implementation should not allow null key.
			IDictionary target = new MessagePackObjectDictionary();
			target.Add( 0, 0 );
			Assert.Throws<ArgumentNullException>( () => { var v = target[ MessagePackObject.Nil ]; } );
		}

		[Test]
		public void TestTryGetValueNull()
		{
			// The spec is ambigious, but many languages do not allow null key, so this implementation should not allow null key.
			var target = new MessagePackObjectDictionary();
			target.Add( 0, 0 );
			MessagePackObject value;
			Assert.Throws<ArgumentNullException>( () => target.TryGetValue( MessagePackObject.Nil, out value ) );
		}

		[Test]
		public void TestSetNull()
		{
			// The spec is ambigious, but many languages do not allow null key, so this implementation should not allow null key.
			Assert.Throws<ArgumentNullException>( () => new MessagePackObjectDictionary()[ MessagePackObject.Nil ] = 0 );
		}

		[Test]
		public void TestIDictionarySetObjectNull()
		{
			// The spec is ambigious, but many languages do not allow null key, so this implementation should not allow null key.
			Assert.Throws<ArgumentNullException>( () => ( new MessagePackObjectDictionary() as IDictionary )[ null ] = 0 );
		}

		[Test]
		public void TestIDictionarySetMessagePackObjectNull()
		{
			// The spec is ambigious, but many languages do not allow null key, so this implementation should not allow null key.
			Assert.Throws<ArgumentNullException>( () => ( new MessagePackObjectDictionary() as IDictionary )[ MessagePackObject.Nil ] = 0 );
		}

		[Test]
		public void TestContainsKeyNull()
		{
			var target = new MessagePackObjectDictionary();
			target.Add( 0, 0 );
			Assert.Throws<ArgumentNullException>( () => target.ContainsKey( MessagePackObject.Nil ) );
		}

		[Test]
		public void TestRemoveNull()
		{
			var target = new MessagePackObjectDictionary();
			target.Add( 0, 0 );
			Assert.Throws<ArgumentNullException>( () => target.Remove( MessagePackObject.Nil ) );
		}

		[Test]
		public void TestCollectionTContainsNull()
		{
			var target = new MessagePackObjectDictionary();
			target.Add( 0, 0 );
			var asCollectionT = target as ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>;
			Assert.Throws<ArgumentNullException>( () => asCollectionT.Contains( new KeyValuePair<MessagePackObject, MessagePackObject>( MessagePackObject.Nil, MessagePackObject.Nil ) ) );
		}

		[Test]
		public void TestCollectionTRemoveNull()
		{
			var target = new MessagePackObjectDictionary();
			target.Add( 0, 0 );
			var asCollectionT = target as ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>;
			Assert.Throws<ArgumentNullException>( () => asCollectionT.Remove( new KeyValuePair<MessagePackObject, MessagePackObject>( MessagePackObject.Nil, MessagePackObject.Nil ) ) );
		}		

		[Test]
		public void TestIDictionaryContainsNull()
		{
			var target = new MessagePackObjectDictionary();
			target.Add( 0, 0 );
			var asIDictionary = target as IDictionary;
			Assert.That( asIDictionary.Contains( null ), Is.False );
			Assert.That( asIDictionary.Contains( MessagePackObject.Nil ), Is.False );
		}

		[Test]
		public void TestIDictionaryRemoveObjectNull()
		{
			var target = new MessagePackObjectDictionary();
			target.Add( 0, 0 );
			var asIDictionary = target as IDictionary;
			Assert.Throws<ArgumentNullException>( () => asIDictionary.Remove( null ) );
		}

		[Test]
		public void TestIDictionaryRemoveMessagePackObjectNull()
		{
			var target = new MessagePackObjectDictionary();
			target.Add( 0, 0 );
			var asIDictionary = target as IDictionary;
			Assert.Throws<ArgumentNullException>( () => asIDictionary.Remove( MessagePackObject.Nil ) );
		}

		[Test]
		public void TestCopyTo()
		{
			IDictionary<MessagePackObject, MessagePackObject> target = new MessagePackObjectDictionary();
			var array = new KeyValuePair<MessagePackObject, MessagePackObject>[ 5 ];

			// empty
			//target.CopyTo( array );
			//VerifyArray( target, array, 0, 0 );
			target.CopyTo( array, 1 );
			VerifyArray( target, array, 0, 0 );
			//target.CopyTo( 1, array, 1, 2 );
			//VerifyArray( target, array, 0, 0 );

			target.Add( "A", 1 );
			target.Add( "B", 2 );
			target.Add( "C", 3 );

			//target.CopyTo( array );
			//VerifyArray( target, array, 0, 3 );
			//Array.Clear( array, 0, array.Length );

			target.CopyTo( array, 1 );
			VerifyArray( target, array, 1, 3 );
			Array.Clear( array, 0, array.Length );

			//target.CopyTo( 1, array, 1, 2 );
			//Assert.That( array[ 0 ], Is.EqualTo( default( KeyValuePair<MessagePackObject, MessagePackObject> ) ) );
			//Assert.That( array[ 1 ], Is.EqualTo( new KeyValuePair<MessagePackObject, MessagePackObject>( "B", 2 ) ) );
			//Assert.That( array[ 2 ], Is.EqualTo( new KeyValuePair<MessagePackObject, MessagePackObject>( "C", 3 ) ) );
			//Assert.That( array[ 3 ], Is.EqualTo( default( KeyValuePair<MessagePackObject, MessagePackObject> ) ) );
			//Assert.That( array[ 4 ], Is.EqualTo( default( KeyValuePair<MessagePackObject, MessagePackObject> ) ) );
		}

		private static void VerifyArray<TKey, TValue>( MessagePackObjectDictionary target, KeyValuePair<MessagePackObject, MessagePackObject>[] array, int startAt, int count )
		{
			var enumerator = target.GetEnumerator();
			try
			{
				for ( int i = 0; i < array.Length; i++ )
				{
					if ( i < startAt || startAt + count <= i )
					{
						Assert.That( array[ i ], Is.EqualTo( default( KeyValuePair<MessagePackObject, MessagePackObject> ) ) );
					}
					else
					{
						var hasNext = enumerator.MoveNext();
						Assert.That( hasNext, Is.True );
						Assert.That( enumerator.Current, Is.EqualTo( array[ i ] ) );
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		private static void VerifyArray<T>( ICollection<T> target, T[] array, int startAt, int count )
		{
			var enumerator = target.GetEnumerator();
			try
			{
				for ( int i = 0; i < array.Length; i++ )
				{
					if ( i < startAt || startAt + count <= i )
					{
						Assert.That( array[ i ], Is.EqualTo( default( T ) ) );
					}
					else
					{
						var hasNext = enumerator.MoveNext();
						Assert.That( hasNext, Is.True );
						Assert.That( enumerator.Current, Is.EqualTo( array[ i ] ) );
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}


		#region -- Ported from Mono's Dictionary`2 unit tests --

		[Test] // bug 432441
		public void Clear_Iterators()
		{
			var d = new MessagePackObjectDictionary();

			d[ 1 ] = 1;
			d.Clear();
			int hash = 0;
			foreach ( object o in d )
			{
				hash += o.GetHashCode();
			}
		}

		[Test]
		public void IEnumeratorTest()
		{
			var _dictionary = new MessagePackObjectDictionary();
			_dictionary.Add( "key1", "value1" );
			_dictionary.Add( "key2", "value2" );
			_dictionary.Add( "key3", "value3" );
			_dictionary.Add( "key4", "value4" );
			IEnumerator itr = ( ( IEnumerable )_dictionary ).GetEnumerator();
			while ( itr.MoveNext() )
			{
				object o = itr.Current;
				Assert.AreEqual( typeof( KeyValuePair<MessagePackObject, MessagePackObject> ), o.GetType(), "Current should return a type of KeyValuePair" );
				KeyValuePair<MessagePackObject, MessagePackObject> entry = ( KeyValuePair<MessagePackObject, MessagePackObject> )itr.Current;
			}
			Assert.AreEqual( "value4", _dictionary[ "key4" ].ToString(), "" );
		}


		[Test]
		public void IEnumeratorGenericTest()
		{
			var _dictionary = new MessagePackObjectDictionary();
			_dictionary.Add( "key1", "value1" );
			_dictionary.Add( "key2", "value2" );
			_dictionary.Add( "key3", "value3" );
			_dictionary.Add( "key4", "value4" );
			IEnumerator<KeyValuePair<MessagePackObject, MessagePackObject>> itr = ( ( IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>> )_dictionary ).GetEnumerator();
			while ( itr.MoveNext() )
			{
				object o = itr.Current;
				Assert.AreEqual( typeof( KeyValuePair<MessagePackObject, MessagePackObject> ), o.GetType(), "Current should return a type of KeyValuePair<MessagePackObject, MessagePackObject>" );
				KeyValuePair<MessagePackObject, MessagePackObject> entry = ( KeyValuePair<MessagePackObject, MessagePackObject> )itr.Current;
			}
			Assert.AreEqual( "value4", _dictionary[ "key4" ].ToString(), "" );
		}

		[Test]
		public void IDictionaryEnumeratorTest()
		{
			var _dictionary = new MessagePackObjectDictionary();
			_dictionary.Add( "key1", "value1" );
			_dictionary.Add( "key2", "value2" );
			_dictionary.Add( "key3", "value3" );
			_dictionary.Add( "key4", "value4" );
			IDictionaryEnumerator itr = ( ( IDictionary )_dictionary ).GetEnumerator();
			while ( itr.MoveNext() )
			{
				object o = itr.Current;
				Assert.AreEqual( typeof( DictionaryEntry ), o.GetType(), "Current should return a type of DictionaryEntry" );
				DictionaryEntry entry = ( DictionaryEntry )itr.Current;
			}
			Assert.AreEqual( "value4", _dictionary[ "key4" ].ToString(), "" );
		}

		[Test]
		public void ForEachTest()
		{
			var _dictionary = new MessagePackObjectDictionary();
			_dictionary.Add( "key1", "value1" );
			_dictionary.Add( "key2", "value2" );
			_dictionary.Add( "key3", "value3" );
			_dictionary.Add( "key4", "value4" );

			int i = 0;
			foreach ( KeyValuePair<MessagePackObject, MessagePackObject> entry in _dictionary )
				i++;
			Assert.AreEqual( 4, i, "fail1: foreach entry failed!" );

			i = 0;
			foreach ( KeyValuePair<MessagePackObject, MessagePackObject> entry in ( ( IEnumerable )_dictionary ) )
				i++;
			Assert.AreEqual( 4, i, "fail2: foreach entry failed!" );

			i = 0;
			foreach ( DictionaryEntry entry in ( ( IDictionary )_dictionary ) )
				i++;
			Assert.AreEqual( 4, i, "fail3: foreach entry failed!" );
		}

		[Test] // bug 75073
		public void SliceCollectionsEnumeratorTest()
		{
			var values = new MessagePackObjectDictionary();

			IEnumerator<MessagePackObject> ke = values.Keys.GetEnumerator();
			IEnumerator<MessagePackObject> ve = values.Values.GetEnumerator();

#if !UNITY
			Assert.IsTrue( ke is MessagePackObjectDictionary.KeySet.Enumerator );
#else
			Assert.IsTrue( ke is MessagePackObjectDictionary.KeyCollection.Enumerator );
#endif // !UNITY
			Assert.IsTrue( ve is MessagePackObjectDictionary.ValueCollection.Enumerator );
		}

		[Test]
		public void PlainEnumeratorReturnTest()
		{
			var _dictionary = new MessagePackObjectDictionary();
			// Test that we return a KeyValuePair even for non-generic dictionary iteration
			_dictionary[ "foo" ] = "bar";
			IEnumerator<KeyValuePair<MessagePackObject, MessagePackObject>> enumerator = _dictionary.GetEnumerator();
			Assert.IsTrue( enumerator.MoveNext(), "#1" );
			Assert.AreEqual( typeof( KeyValuePair<MessagePackObject, MessagePackObject> ), ( ( IEnumerator )enumerator ).Current.GetType(), "#2" );
			Assert.AreEqual( typeof( DictionaryEntry ), ( ( IDictionaryEnumerator )enumerator ).Entry.GetType(), "#3" );
			Assert.AreEqual( typeof( KeyValuePair<MessagePackObject, MessagePackObject> ), ( ( IDictionaryEnumerator )enumerator ).Current.GetType(), "#4" );
			Assert.AreEqual( typeof( KeyValuePair<MessagePackObject, MessagePackObject> ), ( ( object )enumerator.Current ).GetType(), "#5" );
		}

		[Test]
		public void FailFastTest1()
		{
			var d = new MessagePackObjectDictionary();
			d[ 1 ] = 1;
			int count = 0;
			Assert.Throws<InvalidOperationException>( () =>
				{
					foreach ( KeyValuePair<MessagePackObject, MessagePackObject> kv in d )
					{
						d[ kv.Key.AsInt32() + 1 ] = kv.Value.AsInt32() + 1;
						if ( count++ != 0 )
							Assert.Fail( "Should not be reached" );
					}
				}
			);
		}

		[Test]
		public void FailFastTest2()
		{
			var d = new MessagePackObjectDictionary();
			d[ 1 ] = 1;
			int count = 0;
			Assert.Throws<InvalidOperationException>( () =>
				{
					foreach ( int i in d.Keys )
					{
						d[ i + 1 ] = i + 1;
						if ( count++ != 0 )
							Assert.Fail( "Should not be reached" );
					}
				}
			);
		}

		[Test]
		public void FailFastTest3()
		{
			var d = new MessagePackObjectDictionary();
			d[ 1 ] = 1;
			int count = 0;
			Assert.Throws<InvalidOperationException>( () =>
				{
					foreach ( int i in d.Keys )
					{
						d[ i ] = i;
						if ( count++ != 0 )
							Assert.Fail( "Should not be reached" );
					}
				}
			);
		}

		[Test]
		public void Empty_KeysValues_CopyTo()
		{
			var d = new MessagePackObjectDictionary();
			MessagePackObject[] array = new MessagePackObject[ 1 ];
			d.Keys.CopyTo( array, array.Length );
			d.Values.CopyTo( array, array.Length );
		}

		[Test]
		public void IDictionary_Contains()
		{
			IDictionary d = new MessagePackObjectDictionary();
			d.Add( 1, 2 );
			Assert.IsTrue( d.Contains( 1 ) );
			Assert.IsFalse( d.Contains( 2 ) );
			Assert.IsFalse( d.Contains( "x" ) );
		}

		[Test]
		public void IDictionary_Add_Null()
		{
			IDictionary d = new MessagePackObjectDictionary();
			d.Add( 1, null );
			d[ 2 ] = null;

			Assert.That( d[ 1 ], Is.EqualTo( MessagePackObject.Nil ) );
			Assert.That( d[ 2 ], Is.EqualTo( MessagePackObject.Nil ) );
		}

		[Test]
		public void IDictionary_Remove1()
		{
			IDictionary d = new MessagePackObjectDictionary();
			d.Add( 1, 2 );
			d.Remove( 1 );
			d.Remove( 5 );
			d.Remove( "foo" );
		}

		[Test]
		public void IDictionary_IndexerGetNonExistingTest()
		{
			IDictionary d = new MessagePackObjectDictionary();
			d.Add( 1, 2 );
			Assert.IsNull( d[ 2 ] );
			Assert.IsNull( d[ "foo" ] );
		}

		[Test] // bug #332534
		public void Dictionary_MoveNext()
		{
			var a = new MessagePackObjectDictionary();
			a.Add( 3, 1 );
			a.Add( 4, 1 );

			IEnumerator en = a.GetEnumerator();
			for ( int i = 1; i < 10; i++ )
				en.MoveNext();
		}

		[Test]
		public void ResetKeysEnumerator()
		{
			var test = new MessagePackObjectDictionary();
			test.Add( "monkey", "singe" );
			test.Add( "singe", "mono" );
			test.Add( "mono", "monkey" );

			IEnumerator enumerator = test.Keys.GetEnumerator();

			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );

			enumerator.Reset();

			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsFalse( enumerator.MoveNext() );
		}

		[Test]
		public void ResetValuesEnumerator()
		{
			var test = new MessagePackObjectDictionary();
			test.Add( "monkey", "singe" );
			test.Add( "singe", "mono" );
			test.Add( "mono", "monkey" );

			IEnumerator enumerator = test.Values.GetEnumerator();

			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );

			enumerator.Reset();

			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsFalse( enumerator.MoveNext() );
		}

		[Test]
		public void ResetShimEnumerator()
		{
			IDictionary test = new MessagePackObjectDictionary();
			test.Add( "monkey", "singe" );
			test.Add( "singe", "mono" );
			test.Add( "mono", "monkey" );

			IEnumerator enumerator = test.GetEnumerator();

			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );

			enumerator.Reset();

			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsFalse( enumerator.MoveNext() );
		}

		[Test]
		public void ICollectionOfKeyValuePairContains()
		{
			var dictionary = new MessagePackObjectDictionary();
			dictionary.Add( "foo", 42 );
			dictionary.Add( "bar", 12 );

			var collection = dictionary as ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>;

			Assert.AreEqual( 2, collection.Count );

			Assert.IsFalse( collection.Contains( new KeyValuePair<MessagePackObject, MessagePackObject>( "baz", 13 ) ) );
			Assert.IsFalse( collection.Contains( new KeyValuePair<MessagePackObject, MessagePackObject>( "foo", 13 ) ) );
			Assert.IsTrue( collection.Contains( new KeyValuePair<MessagePackObject, MessagePackObject>( "foo", 42 ) ) );
		}

		[Test]
		public void ICollectionOfKeyValuePairRemove()
		{
			var dictionary = new MessagePackObjectDictionary();
			dictionary.Add( "foo", 42 );
			dictionary.Add( "bar", 12 );

			var collection = dictionary as ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>;

			Assert.AreEqual( 2, collection.Count );

			Assert.IsFalse( collection.Remove( new KeyValuePair<MessagePackObject, MessagePackObject>( "baz", 13 ) ) );
			Assert.IsFalse( collection.Remove( new KeyValuePair<MessagePackObject, MessagePackObject>( "foo", 13 ) ) );
			Assert.IsTrue( collection.Remove( new KeyValuePair<MessagePackObject, MessagePackObject>( "foo", 42 ) ) );

			Assert.AreEqual( new MessagePackObject( 12 ), dictionary[ "bar" ] );
			Assert.IsFalse( dictionary.ContainsKey( "foo" ) );
		}

		[Test]
		public void ICollectionCopyToKeyValuePairArray()
		{
			var dictionary = new MessagePackObjectDictionary();
			dictionary.Add( "foo", 42 );

			var collection = dictionary as ICollection;

			Assert.AreEqual( 1, collection.Count );

			var pairs = new KeyValuePair<MessagePackObject, MessagePackObject>[ 1 ];

			collection.CopyTo( pairs, 0 );

			Assert.AreEqual( new MessagePackObject( "foo" ), pairs[ 0 ].Key );
			Assert.AreEqual( new MessagePackObject( 42 ), pairs[ 0 ].Value );
		}

		[Test]
		public void ICollectionCopyToDictionaryEntryArray()
		{
			var dictionary = new MessagePackObjectDictionary();
			dictionary.Add( "foo", 42 );

			var collection = dictionary as ICollection;

			Assert.AreEqual( 1, collection.Count );

			var entries = new DictionaryEntry[ 1 ];

			collection.CopyTo( entries, 0 );

			Assert.AreEqual( new MessagePackObject( "foo" ), ( MessagePackObject )entries[ 0 ].Key );
			Assert.AreEqual( new MessagePackObject( 42 ), ( MessagePackObject )entries[ 0 ].Value );
		}

		[Test]
		public void ICollectionCopyToObjectArray()
		{
			var dictionary = new MessagePackObjectDictionary();
			dictionary.Add( "foo", 42 );

			var collection = dictionary as ICollection;

			Assert.AreEqual( 1, collection.Count );

			var array = new object[ 1 ];

			collection.CopyTo( array, 0 );

			var pair = ( KeyValuePair<MessagePackObject, MessagePackObject> )array[ 0 ];

			Assert.AreEqual( new MessagePackObject( "foo" ), pair.Key );
			Assert.AreEqual( new MessagePackObject( 42 ), pair.Value );
		}

		[Test]
		public void ICollectionCopyToInvalidArray()
		{
			var dictionary = new MessagePackObjectDictionary();
			dictionary.Add( "foo", 42 );

			var collection = dictionary as ICollection;

			Assert.AreEqual( 1, collection.Count );

			var array = new int[ 1 ];

			Assert.Throws<ArgumentException>( () => collection.CopyTo( array, 0 ) );
		}

		[Test]
		public void ValuesCopyToObjectArray()
		{
			var dictionary = new MessagePackObjectDictionary { { "foo", "bar" } };

			var values = dictionary.Values as ICollection;

			var array = new object[ values.Count ];

			values.CopyTo( array, 0 );

			Assert.AreEqual( new MessagePackObject( "bar" ), array[ 0 ] );
		}

		delegate void D();
		bool Throws( D d )
		{
			try
			{
				d();
				return false;
			}
			catch
			{
				return true;
			}
		}

		[Test]
		// based on #491858, #517415
		public void Enumerator_Current()
		{
			var e1 = new MessagePackObjectDictionary.Enumerator();
			Assert.IsFalse( Throws( delegate { var x = e1.Current; } ) );

			var d = new MessagePackObjectDictionary();
			var e2 = d.GetEnumerator();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );
			e2.MoveNext();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );
			e2.Dispose();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );

			var e3 = ( ( IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>> )d ).GetEnumerator();
			Assert.IsFalse( Throws( delegate { var x = e3.Current; } ) );
			e3.MoveNext();
			Assert.IsFalse( Throws( delegate { var x = e3.Current; } ) );
			e3.Dispose();
			Assert.IsFalse( Throws( delegate { var x = e3.Current; } ) );

			var e4 = ( ( IEnumerable )d ).GetEnumerator();
			Assert.IsTrue( Throws( delegate { var x = e4.Current; } ) );
			e4.MoveNext();
			Assert.IsTrue( Throws( delegate { var x = e4.Current; } ) );
			( ( IDisposable )e4 ).Dispose();
			Assert.IsTrue( Throws( delegate { var x = e4.Current; } ) );
		}

		[Test]
		// based on #491858, #517415
		public void KeyEnumerator_Current()
		{
#if !UNITY
			var e1 = new MessagePackObjectDictionary.KeySet.Enumerator();
			Assert.IsFalse( Throws( delegate { var x = e1.Current; } ) );
#else
			var e1 = new MessagePackObjectDictionary.KeyCollection.Enumerator();
			Assert.IsFalse( Throws( delegate { var x = e1.Current; } ) );
#endif // !UNITY

			var d = new MessagePackObjectDictionary().Keys;
			var e2 = d.GetEnumerator();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );
			e2.MoveNext();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );
			e2.Dispose();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );

			var e3 = ( ( IEnumerable<MessagePackObject> )d ).GetEnumerator();
			Assert.IsFalse( Throws( delegate { var x = e3.Current; } ) );
			e3.MoveNext();
			Assert.IsFalse( Throws( delegate { var x = e3.Current; } ) );
			e3.Dispose();
			Assert.IsFalse( Throws( delegate { var x = e3.Current; } ) );

			var e4 = ( ( IEnumerable )d ).GetEnumerator();
			Assert.IsTrue( Throws( delegate { var x = e4.Current; } ) );
			e4.MoveNext();
			Assert.IsTrue( Throws( delegate { var x = e4.Current; } ) );
			( ( IDisposable )e4 ).Dispose();
			Assert.IsTrue( Throws( delegate { var x = e4.Current; } ) );
		}

		[Test]
		// based on #491858, #517415
		public void ValueEnumerator_Current()
		{
			var e1 = new MessagePackObjectDictionary.ValueCollection.Enumerator();
			Assert.IsFalse( Throws( delegate { var x = e1.Current; } ) );

			var d = new MessagePackObjectDictionary().Values;
			var e2 = d.GetEnumerator();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );
			e2.MoveNext();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );
			e2.Dispose();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );

			var e3 = ( ( IEnumerable<MessagePackObject> )d ).GetEnumerator();
			Assert.IsFalse( Throws( delegate { var x = e3.Current; } ) );
			e3.MoveNext();
			Assert.IsFalse( Throws( delegate { var x = e3.Current; } ) );
			e3.Dispose();
			Assert.IsFalse( Throws( delegate { var x = e3.Current; } ) );

			var e4 = ( ( IEnumerable )d ).GetEnumerator();
			Assert.IsTrue( Throws( delegate { var x = e4.Current; } ) );
			e4.MoveNext();
			Assert.IsTrue( Throws( delegate { var x = e4.Current; } ) );
			( ( IDisposable )e4 ).Dispose();
			Assert.IsTrue( Throws( delegate { var x = e4.Current; } ) );
		}

		[Test]
		public void ICollectionCopyTo()
		{
			var d = new MessagePackObjectDictionary();

			ICollection c = d;
			c.CopyTo( new object[ 0 ], 0 );
			c.CopyTo( new string[ 0 ], 0 );
			c.CopyTo( new MyClass[ 0 ], 0 );

			c = d.Keys;
			c.CopyTo( new object[ 0 ], 0 );
			c.CopyTo( new ValueType[ 0 ], 0 );

			c = d.Values;
			c.CopyTo( new object[ 0 ], 0 );
			c.CopyTo( new MyClass[ 0 ], 0 );

			d[ 3 ] = MessagePackObject.Nil;

			c = d.Keys;
			c.CopyTo( new object[ 1 ], 0 );
			c.CopyTo( new ValueType[ 1 ], 0 );

			c = d.Values;
			c.CopyTo( new object[ 1 ], 0 );
		}

		[Test]
		public void ICollectionCopyTo_ex3()
		{
			var d = new MessagePackObjectDictionary();
			d[ 3 ] = "5";

			ICollection c = d.Keys;
			Assert.Throws<ArgumentException>( () => c.CopyTo( new MyClass[ 1 ], 0 ) );
		}

		[Test]
		public void ICollectionCopyTo_ex4()
		{
			var d = new MessagePackObjectDictionary();
			d[ 3 ] = "5";

			ICollection c = d.Values;
			Assert.Throws<ArgumentException>( () => c.CopyTo( new MyClass[ 1 ], 0 ) );
		}

		#endregion -- Ported from Mono's Dictionary`2 unit tests --

		[Test]
		public void TestKeys()
		{
			var dictionary = new MessagePackObjectDictionary() { { "A", 1 }, { "B", 2 } };
			var target = dictionary.Keys;
			var result = new List<MessagePackObject>();
			foreach ( var item in target )
			{
				result.Add( item );
			}
			Assert.That( result.Count, Is.EqualTo( 2 ) );
			Assert.That( result[ 0 ], Is.EqualTo( new MessagePackObject( "A" ) ) );
			Assert.That( result[ 1 ], Is.EqualTo( new MessagePackObject( "B" ) ) );
		}

		[Test]
		public void TestKeysCopyTo()
		{
			var dictionary = new MessagePackObjectDictionary();
			var target = dictionary.Keys;
			var array = new MessagePackObject[ 5 ];

			// empty
			target.CopyTo( array );
			VerifyArray( target, array, 0, 0 );
			target.CopyTo( array, 1 );
			VerifyArray( target, array, 0, 0 );
			target.CopyTo( 1, array, 1, 2 );
			VerifyArray( target, array, 0, 0 );

			dictionary.Add( "A", 1 );
			dictionary.Add( "B", 2 );
			dictionary.Add( "C", 3 );

			target.CopyTo( array );
			VerifyArray( target, array, 0, 3 );
			Array.Clear( array, 0, array.Length );

			target.CopyTo( array, 1 );
			VerifyArray( target, array, 1, 3 );
			Array.Clear( array, 0, array.Length );

			target.CopyTo( 1, array, 1, 2 );
			Assert.That( array[ 0 ], Is.EqualTo( MessagePackObject.Nil ) );
			Assert.That( array[ 1 ], Is.EqualTo( new MessagePackObject( "B" ) ) );
			Assert.That( array[ 2 ], Is.EqualTo( new MessagePackObject( "C" ) ) );
			Assert.That( array[ 3 ], Is.EqualTo( MessagePackObject.Nil ) );
			Assert.That( array[ 4 ], Is.EqualTo( MessagePackObject.Nil ) );
		}

#if !UNITY
		[Test]
		public void TestKeySetIsProperSupersetOf()
		{
			var dictionary = new MessagePackObjectDictionary();
			var target = dictionary.Keys;

			// empty
			Assert.That( target.IsProperSupersetOf( new MessagePackObject[ 0 ] ), Is.False );
			Assert.That( target.IsProperSupersetOf( new MessagePackObject[] { 1, 2, 3 } ), Is.False );

			dictionary.Add( 1, 1 );
			dictionary.Add( 2, 2 );
			dictionary.Add( 3, 3 );

			Assert.That( target.IsProperSupersetOf( new MessagePackObject[] { 1, 2, 3 } ), Is.False );
			Assert.That( target.IsProperSupersetOf( new MessagePackObject[] { 1, 2 } ), Is.True );
			Assert.That( target.IsProperSupersetOf( new MessagePackObject[] { 1, 2, 3, 4 } ), Is.False );
			Assert.That( target.IsProperSupersetOf( new MessagePackObject[] { 1, 2, 3, 1 } ), Is.False );
			Assert.That( target.IsProperSupersetOf( new MessagePackObject[] { 1, 2, 1 } ), Is.True );
			Assert.That( target.IsProperSupersetOf( new MessagePackObject[] { 1, 2, 3, 4, 1 } ), Is.False );
			Assert.That( target.IsProperSupersetOf( new MessagePackObject[ 0 ] ), Is.True );
		}

		[Test]
		public void TestKeySetIsSupersetOf()
		{
			var dictionary = new MessagePackObjectDictionary();
			var target = dictionary.Keys;

			// empty
			Assert.That( target.IsSupersetOf( new MessagePackObject[ 0 ] ), Is.True );
			Assert.That( target.IsSupersetOf( new MessagePackObject[] { 1, 2, 3 } ), Is.False );

			dictionary.Add( 1, 1 );
			dictionary.Add( 2, 2 );
			dictionary.Add( 3, 3 );

			Assert.That( target.IsSupersetOf( new MessagePackObject[] { 1, 2, 3 } ), Is.True );
			Assert.That( target.IsSupersetOf( new MessagePackObject[] { 1, 2 } ), Is.True );
			Assert.That( target.IsSupersetOf( new MessagePackObject[] { 1, 2, 3, 4 } ), Is.False );
			Assert.That( target.IsSupersetOf( new MessagePackObject[] { 1, 2, 3, 1 } ), Is.True );
			Assert.That( target.IsSupersetOf( new MessagePackObject[] { 1, 2, 1 } ), Is.True );
			Assert.That( target.IsSupersetOf( new MessagePackObject[] { 1, 2, 3, 4, 1 } ), Is.False );
			Assert.That( target.IsSupersetOf( new MessagePackObject[ 0 ] ), Is.True );
		}

		[Test]
		public void TestKeySetIsProperSubsetOf()
		{
			var dictionary = new MessagePackObjectDictionary();
			var target = dictionary.Keys;

			// empty
			Assert.That( target.IsProperSubsetOf( new MessagePackObject[ 0 ] ), Is.False );
			Assert.That( target.IsProperSubsetOf( new MessagePackObject[] { 1, 2, 3 } ), Is.True );

			dictionary.Add( 1, 1 );
			dictionary.Add( 2, 2 );
			dictionary.Add( 3, 3 );

			Assert.That( target.IsProperSubsetOf( new MessagePackObject[] { 1, 2, 3 } ), Is.False );
			Assert.That( target.IsProperSubsetOf( new MessagePackObject[] { 1, 2 } ), Is.False );
			Assert.That( target.IsProperSubsetOf( new MessagePackObject[] { 1, 2, 3, 4 } ), Is.True );
			Assert.That( target.IsProperSubsetOf( new MessagePackObject[] { 1, 2, 3, 1 } ), Is.False );
			Assert.That( target.IsProperSubsetOf( new MessagePackObject[] { 1, 2, 1 } ), Is.False );
			Assert.That( target.IsProperSubsetOf( new MessagePackObject[] { 1, 2, 3, 4, 1 } ), Is.True );
			Assert.That( target.IsProperSubsetOf( new MessagePackObject[ 0 ] ), Is.False );
		}

		[Test]
		public void TestKeySetIsSubsetOf()
		{
			var dictionary = new MessagePackObjectDictionary();
			var target = dictionary.Keys;

			// empty
			Assert.That( target.IsSubsetOf( new MessagePackObject[ 0 ] ), Is.True );
			Assert.That( target.IsSubsetOf( new MessagePackObject[] { 1, 2, 3 } ), Is.True );

			dictionary.Add( 1, 1 );
			dictionary.Add( 2, 2 );
			dictionary.Add( 3, 3 );

			Assert.That( target.IsSubsetOf( new MessagePackObject[] { 1, 2, 3 } ), Is.True );
			Assert.That( target.IsSubsetOf( new MessagePackObject[] { 1, 2 } ), Is.False );
			Assert.That( target.IsSubsetOf( new MessagePackObject[] { 1, 2, 3, 4 } ), Is.True );
			Assert.That( target.IsSubsetOf( new MessagePackObject[] { 1, 2, 3, 1 } ), Is.True );
			Assert.That( target.IsSubsetOf( new MessagePackObject[] { 1, 2, 1 } ), Is.False );
			Assert.That( target.IsSubsetOf( new MessagePackObject[] { 1, 2, 3, 4, 1 } ), Is.True );
			Assert.That( target.IsSubsetOf( new MessagePackObject[ 0 ] ), Is.False );
		}

		[Test]
		public void TestKeySetOverlaps()
		{
			var dictionary = new MessagePackObjectDictionary();
			var target = dictionary.Keys;

			// empty
			Assert.That( target.Overlaps( new MessagePackObject[ 0 ] ), Is.False );
			Assert.That( target.Overlaps( new MessagePackObject[] { 1, 2, 3 } ), Is.False );

			dictionary.Add( 1, 1 );
			dictionary.Add( 2, 2 );
			dictionary.Add( 3, 3 );

			Assert.That( target.Overlaps( new MessagePackObject[] { 1, 2, 3 } ), Is.True );
			Assert.That( target.Overlaps( new MessagePackObject[] { 1, 2 } ), Is.True );
			Assert.That( target.Overlaps( new MessagePackObject[] { 1, 2, 3, 4 } ), Is.True );
			Assert.That( target.Overlaps( new MessagePackObject[] { 1, 2, 3, 1 } ), Is.True );
			Assert.That( target.Overlaps( new MessagePackObject[] { 1, 2, 1 } ), Is.True );
			Assert.That( target.Overlaps( new MessagePackObject[] { 1, 2, 3, 4, 1 } ), Is.True );
			Assert.That( target.Overlaps( new MessagePackObject[ 0 ] ), Is.False );
		}

		[Test]
		public void TestKeySetSetEquals()
		{
			var dictionary = new MessagePackObjectDictionary();
			var target = dictionary.Keys;

			// empty
			Assert.That( target.SetEquals( new MessagePackObject[ 0 ] ), Is.True );
			Assert.That( target.SetEquals( new MessagePackObject[] { 1, 2, 3 } ), Is.False );

			dictionary.Add( 1, 1 );
			dictionary.Add( 2, 2 );
			dictionary.Add( 3, 3 );

			Assert.That( target.SetEquals( new MessagePackObject[] { 1, 2, 3 } ), Is.True );
			Assert.That( target.SetEquals( new MessagePackObject[] { 1, 2 } ), Is.False );
			Assert.That( target.SetEquals( new MessagePackObject[] { 1, 2, 3, 4 } ), Is.False );
			Assert.That( target.SetEquals( new MessagePackObject[] { 1, 2, 3, 1 } ), Is.True );
			Assert.That( target.SetEquals( new MessagePackObject[] { 1, 2, 1 } ), Is.False );
			Assert.That( target.SetEquals( new MessagePackObject[] { 1, 2, 3, 4, 1 } ), Is.False );
			Assert.That( target.SetEquals( new MessagePackObject[ 0 ] ), Is.False );
		}
#endif // !UNITY

		[Test]
		public void TestValues()
		{
			var dictionary = new MessagePackObjectDictionary() { { "A", 1 }, { "B", 2 } };
			var target = dictionary.Values;
			var result = new List<MessagePackObject>();
			foreach ( var item in target )
			{
				result.Add( item );
			}
			Assert.That( result.Count, Is.EqualTo( 2 ) );
			Assert.That( result[ 0 ], Is.EqualTo( new MessagePackObject( 1 ) ) );
			Assert.That( result[ 1 ], Is.EqualTo( new MessagePackObject( 2 ) ) );
		}

		[Test]
		public void TestValuesCopyTo()
		{
			var dictionary = new MessagePackObjectDictionary();
			var target = dictionary.Values;
			var array = new MessagePackObject[ 5 ];

			// empty
			target.CopyTo( array );
			VerifyArray( target, array, 0, 0 );
			target.CopyTo( array, 1 );
			VerifyArray( target, array, 0, 0 );
			target.CopyTo( 1, array, 1, 2 );
			VerifyArray( target, array, 0, 0 );

			dictionary.Add( "A", 1 );
			dictionary.Add( "B", 2 );
			dictionary.Add( "C", 3 );

			target.CopyTo( array );
			VerifyArray( target, array, 0, 3 );
			Array.Clear( array, 0, array.Length );

			target.CopyTo( array, 1 );
			VerifyArray( target, array, 1, 3 );
			Array.Clear( array, 0, array.Length );

			target.CopyTo( 1, array, 1, 2 );
			Assert.That( array[ 0 ], Is.EqualTo( MessagePackObject.Nil ) );
			Assert.That( array[ 1 ], Is.EqualTo( new MessagePackObject( 2 ) ) );
			Assert.That( array[ 2 ], Is.EqualTo( new MessagePackObject( 3 ) ) );
			Assert.That( array[ 3 ], Is.EqualTo( MessagePackObject.Nil ) );
			Assert.That( array[ 4 ], Is.EqualTo( MessagePackObject.Nil ) );
		}

#if !NETFX_CORE && !SILVERLIGHT && !UNITY
		[Test]
		public void TestRuntimeSerialization_NotEmpty_RoundTripped()
		{
			var target = new MessagePackObjectDictionary() { { "Double", 1.0 }, { "Integer", 1 }, { "Boolean", true }, { "Array", new MessagePackObject[] { 1, 2 } } };
			using ( var buffer = new MemoryStream() )
			{
				var serializer = new BinaryFormatter();
				serializer.Serialize( buffer, target );
				buffer.Position = 0;
				var deserialized = (MessagePackObjectDictionary) serializer.Deserialize( buffer );
				Assert.AreEqual( target, deserialized );
			}
		}
#endif // !NETFX_CORE && !SILVERLIGHT && !UNITY

		private sealed class MyClass { }
	}
}
