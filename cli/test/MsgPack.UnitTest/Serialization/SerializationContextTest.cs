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
using System.Threading;
using NUnit.Framework;

namespace MsgPack.Serialization
{
	[TestFixture]
	[Timeout( 3000 )]
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
				for ( int i = 0; i < Environment.ProcessorCount * 2; i++ )
				{
					this._barrier.AddParticipant();
					ThreadPool.QueueUserWorkItem( @this => ( @this as ConcurrentHelper<T> ).TestCore(), this );
				}

				this._barrier.SignalAndWait();
				// Wait join
				this._barrier.SignalAndWait();
			}

			private void TestCore()
			{
				var iteration = this._random.Next( 1000 );

				this._barrier.SignalAndWait();

				for ( int i = 0; i < iteration; i++ )
				{
					this.TestValue();
				}

				this._barrier.SignalAndWait();
			}

			private void TestValue()
			{
				var value = this._getter();
				if ( value != null )
				{
					var old = Interlocked.Exchange( ref this._gotten, value );
					Assert.That( old, Is.Null.Or.SameAs( value ) );
				}
			}
		}
	}
}
