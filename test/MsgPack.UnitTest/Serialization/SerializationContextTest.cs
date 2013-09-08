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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack.Serialization
{
	[TestFixture]
	[Timeout( 15000 )]
	public class SerializationContextTest
	{
		[Test]
		public void TestGetSerializer()
		{
			var target = new SerializationContext();
			Assert.That( target.GetSerializer<int>(), Is.Not.Null );
			Assert.That( target.Serializers.Get<ComplexType>( target ), Is.Null );
			Assert.That( target.GetSerializer<int>(), Is.Not.Null );
		}

		[Test]
		public void TestGetSerializer_Null()
		{
			var target = new SerializationContext();
			Assert.Throws<ArgumentNullException>( () => target.GetSerializer( null ) );
		}

		[Test]
		public void TestGetSerializer_Concurrent()
		{
			var target = new SerializationContext();
			new ConcurrentHelper<MessagePackSerializer<ComplexType>>(
				target.GetSerializer<ComplexType>
			).Test();
		}

		[Test]
		public void TestGetSerializer_Type()
		{
			var target = new SerializationContext();
			Assert.That( target.GetSerializer( typeof( int ) ), Is.Not.Null );
			Assert.That( target.GetSerializer( typeof( ComplexType ) ), Is.Not.Null );
		}

		[Test]
		public void TestGetSerializer_TypeConcurrent()
		{
			var target = new SerializationContext();
			new ConcurrentHelper<MessagePackSerializer<ComplexType>>(
				() => ( MessagePackSerializer<ComplexType> )target.GetSerializer( typeof( ComplexType ) )
			).Test();
		}

		[Test]
		public void TestDefaultCollectionTypes_Default_Check()
		{
			var context = new SerializationContext();
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IList<> ) ), Is.EqualTo( typeof( List<> ) ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( ISet<> ) ), Is.EqualTo( typeof( HashSet<> ) ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( ICollection<> ) ), Is.EqualTo( typeof( List<> ) ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IEnumerable<> ) ), Is.EqualTo( typeof( List<> ) ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IDictionary<,> ) ), Is.EqualTo( typeof( Dictionary<,> ) ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IList ) ), Is.EqualTo( typeof( List<MessagePackObject> ) ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( ICollection ) ), Is.EqualTo( typeof( List<MessagePackObject> ) ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IEnumerable ) ), Is.EqualTo( typeof( List<MessagePackObject> ) ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IDictionary ) ), Is.EqualTo( typeof( MessagePackObjectDictionary ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_Collection_New_Ok()
		{
			var context = new SerializationContext();
			context.DefaultCollectionTypes.Register( typeof( NewAbstractCollection<> ), typeof( NewConcreteCollection<> ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( NewAbstractCollection<> ) ), Is.EqualTo( typeof( NewConcreteCollection<> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_Collection_Overwrite_Ok()
		{
			var context = new SerializationContext();
			context.DefaultCollectionTypes.Register( typeof( IList<> ), typeof( Collection<> ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IList<> ) ), Is.EqualTo( typeof( Collection<> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_Collection_Closed_Ok()
		{
			var context = new SerializationContext();
			context.DefaultCollectionTypes.Register( typeof( IList<string> ), typeof( Collection<string> ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IList<> ) ), Is.EqualTo( typeof( List<> ) ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IList<string> ) ), Is.EqualTo( typeof( Collection<string> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_NonCollection_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( Stream ), typeof( MemoryStream ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_ArityIsTooMany_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( IEnumerable<> ), typeof( Dictionary<,> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_ArityIsTooFew_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( IDictionary<,> ), typeof( StringKeyDictionary<> ) ) );
		}

#if !NETFX_CORE
		[Test]
		public void TestDefaultCollectionTypes_Register_Incompatible_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( IList<> ), typeof( ArrayList ) ) );
		}
#endif

		[Test]
		public void TestDefaultCollectionTypes_Register_NonGenericGenericMpoOk()
		{
			var context = new SerializationContext();
			context.DefaultCollectionTypes.Register( typeof( IList ), typeof( Collection<MessagePackObject> ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IList ) ), Is.EqualTo( typeof( Collection<MessagePackObject> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_AbstractType_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( IList ), typeof( IList<MessagePackObject> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_OpenClose_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( IList<> ), typeof( List<string> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_CloseOpen_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( IList<string> ), typeof( List<> ) ) );
		}

		private sealed class ConcurrentHelper<T> : IDisposable
			where T : class
		{
			private T _gotten;
			private readonly Func<T> _getter;
			private readonly Random _random;
			private readonly Barrier _barrier;

			public ConcurrentHelper( Func<T> getter )
			{
				this._getter = getter;
				this._random = new Random();
				this._barrier = new Barrier( 1 );
			}

			public void Dispose()
			{
				this._barrier.Dispose();
			}

			public void Test()
			{
				var tasks = new List<Task>();
				for ( int i = 0; i < Environment.ProcessorCount * 2; i++ )
				{
					this._barrier.AddParticipant();
					tasks.Add(
						Task.Factory.StartNew(
							@this => ( @this as ConcurrentHelper<T> ).TestCore(),
							this
						)
					);
				}

				this._barrier.SignalAndWait();
				// Wait join
				Task.WaitAll( tasks.ToArray() );
			}

			private void TestCore()
			{
				var iteration = 50;

				this._barrier.SignalAndWait();

				for ( int i = 0; i < iteration; i++ )
				{
					this.TestValue();
				}
			}

			private void TestValue()
			{
				var value = this._getter();
				if ( value != null )
				{
					var old = Interlocked.Exchange( ref this._gotten, value );
#if !NETFX_CORE
					Assert.That( old, Is.Null.Or.SameAs( value ).Or.InstanceOf( typeof( LazyDelegatingMessagePackSerializer<> ).MakeGenericType( typeof( T ).GetGenericArguments()[ 0 ] ) ) );
#else
					Assert.That( old, Is.Null.Or.SameAs( value ).Or.InstanceOf( typeof( LazyDelegatingMessagePackSerializer<> ).MakeGenericType( typeof( T ).GenericTypeArguments[ 0 ] ) ) );
#endif
				}
			}
		}

		public abstract class NewAbstractCollection<T> : Collection<T>
		{
			
		}

		public sealed class NewConcreteCollection<T> : NewAbstractCollection<T>
		{
		}

		private sealed class StringKeyDictionary<T> : Dictionary<String, T>
		{
		}
	}
}
