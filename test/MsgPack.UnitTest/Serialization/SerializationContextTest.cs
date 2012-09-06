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
using System.Collections.Generic;
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
	}
}
