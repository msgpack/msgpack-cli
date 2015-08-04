 
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
#if !NETFX_CORE
using System.Collections;
#endif
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
#if !NETFX_CORE
using MsgPack.Serialization.CodeDomSerializers;
using MsgPack.Serialization.EmittingSerializers;
#endif
using MsgPack.Serialization.ExpressionSerializers;
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
	public class ArrayContextBasedImmutableCollectionsTest
	{
		private MessagePackSerializer<T> CreateTarget<T>()
		{
			var context = new SerializationContext { SerializationMethod = SerializationMethod.Array, EmitterFlavor = EmitterFlavor.ContextBased };
			return context.GetSerializer<T>( PolymorphismSchema.Default );
		}
		
		private bool CanDump
		{
			get { return false; }
		}

#if !NETFX_CORE
		[SetUp]
		public void SetUp()
		{
			SerializerDebugging.DeletePastTemporaries();
			//SerializerDebugging.TraceEnabled = true;
			//SerializerDebugging.DumpEnabled = true;
			if ( SerializerDebugging.TraceEnabled )
			{
				Tracer.Emit.Listeners.Clear();
				Tracer.Emit.Switch.Level = SourceLevels.All;
				Tracer.Emit.Listeners.Add( new ConsoleTraceListener() );
			}

			SerializerDebugging.OnTheFlyCodeDomEnabled = true;
			SerializerDebugging.AddRuntimeAssembly( typeof( ImmutableList ).Assembly.Location );
		}

		[TearDown]
		public void TearDown()
		{
			if ( SerializerDebugging.DumpEnabled && this.CanDump )
			{
				try
				{
					SerializerDebugging.Dump();
				}
				catch ( NotSupportedException ex )
				{
					Console.Error.WriteLine( ex );
				}
				finally
				{
					DefaultSerializationMethodGeneratorManager.Refresh();
				}
			}

			SerializerDebugging.Reset();
			SerializerDebugging.OnTheFlyCodeDomEnabled = false;
		}
#endif

		[Test]
		public void QueueSerializationTest()
		{
			var serializer = SerializationContext.Default.GetSerializer<Queue<int>>();
			var value = new Queue<int>();
			value.Enqueue( 1 );
			value.Enqueue( 2 );

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, value );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.EqualTo( value ) );
			}
		}

		[Test]
		public void StackSerializationTest()
		{
			var serializer = SerializationContext.Default.GetSerializer<Stack<int>>();
			var value = new Stack<int>();
			value.Push( 1 );
			value.Push( 2 );

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, value );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.EqualTo( value ) );
			}
		}

#if !NETFX_CORE
		[Test]
		public void NonGenericQueueSerializationTest()
		{
			var serializer = SerializationContext.Default.GetSerializer<Queue>();
			var value = new Queue();
			value.Enqueue( ( MessagePackObject )1 );
			value.Enqueue( ( MessagePackObject )2 );

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, value );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.EqualTo( value ) );
			}
		}

		[Test]
		public void NonGenericStackSerializationTest()
		{
			var serializer = SerializationContext.Default.GetSerializer<Stack>();
			var value = new Stack();
			value.Push( ( MessagePackObject )1 );
			value.Push( ( MessagePackObject )2 );

			using ( var buffer = new MemoryStream() )
			{
				serializer.Pack( buffer, value );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.EqualTo( value ) );
			}
		}
#endif // !NETFX_CORE


		[Test]
		public void ImmutableArrayTest_0_Success()
		{
			var collection = ImmutableArray.Create<int>();
			var target = this.CreateTarget<ImmutableArray<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableArrayTest_1_Success()
		{
			var collection = ImmutableArray.Create<int>();
			collection = collection.Add( 0 );
			var target = this.CreateTarget<ImmutableArray<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableArrayTest_2_Success()
		{
			var collection = ImmutableArray.Create<int>();
			collection = collection.Add( 0 );
			collection = collection.Add( 1 );
			var target = this.CreateTarget<ImmutableArray<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableListTest_0_Success()
		{
			var collection = ImmutableList.Create<int>();
			var target = this.CreateTarget<ImmutableList<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableListTest_1_Success()
		{
			var collection = ImmutableList.Create<int>();
			collection = collection.Add( 0 );
			var target = this.CreateTarget<ImmutableList<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableListTest_2_Success()
		{
			var collection = ImmutableList.Create<int>();
			collection = collection.Add( 0 );
			collection = collection.Add( 1 );
			var target = this.CreateTarget<ImmutableList<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableStackTest_0_Success()
		{
			var collection = ImmutableStack.Create<int>();
			var target = this.CreateTarget<ImmutableStack<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableStackTest_1_Success()
		{
			var collection = ImmutableStack.Create<int>();
			collection = collection.Push( 0 );
			var target = this.CreateTarget<ImmutableStack<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableStackTest_2_Success()
		{
			var collection = ImmutableStack.Create<int>();
			collection = collection.Push( 0 );
			collection = collection.Push( 1 );
			var target = this.CreateTarget<ImmutableStack<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableQueueTest_0_Success()
		{
			var collection = ImmutableQueue.Create<int>();
			var target = this.CreateTarget<ImmutableQueue<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableQueueTest_1_Success()
		{
			var collection = ImmutableQueue.Create<int>();
			collection = collection.Enqueue( 0 );
			var target = this.CreateTarget<ImmutableQueue<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableQueueTest_2_Success()
		{
			var collection = ImmutableQueue.Create<int>();
			collection = collection.Enqueue( 0 );
			collection = collection.Enqueue( 1 );
			var target = this.CreateTarget<ImmutableQueue<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableHashSetTest_0_Success()
		{
			var collection = ImmutableHashSet.Create<int>();
			var target = this.CreateTarget<ImmutableHashSet<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableHashSetTest_1_Success()
		{
			var collection = ImmutableHashSet.Create<int>();
			collection = collection.Add( 0 );
			var target = this.CreateTarget<ImmutableHashSet<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableHashSetTest_2_Success()
		{
			var collection = ImmutableHashSet.Create<int>();
			collection = collection.Add( 0 );
			collection = collection.Add( 1 );
			var target = this.CreateTarget<ImmutableHashSet<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableSortedSetTest_0_Success()
		{
			var collection = ImmutableSortedSet.Create<int>();
			var target = this.CreateTarget<ImmutableSortedSet<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableSortedSetTest_1_Success()
		{
			var collection = ImmutableSortedSet.Create<int>();
			collection = collection.Add( 0 );
			var target = this.CreateTarget<ImmutableSortedSet<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableSortedSetTest_2_Success()
		{
			var collection = ImmutableSortedSet.Create<int>();
			collection = collection.Add( 0 );
			collection = collection.Add( 1 );
			var target = this.CreateTarget<ImmutableSortedSet<int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableDictionaryTest_0_Success()
		{
			var collection = ImmutableDictionary.Create<int, int>();
			var target = this.CreateTarget<ImmutableDictionary<int, int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableDictionaryTest_1_Success()
		{
			var collection = ImmutableDictionary.Create<int, int>();
			collection = collection.Add( 0, 0 );
			var target = this.CreateTarget<ImmutableDictionary<int, int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableDictionaryTest_2_Success()
		{
			var collection = ImmutableDictionary.Create<int, int>();
			collection = collection.Add( 0, 0 );
			collection = collection.Add( 1, 1 );
			var target = this.CreateTarget<ImmutableDictionary<int, int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableSortedDictionaryTest_0_Success()
		{
			var collection = ImmutableSortedDictionary.Create<int, int>();
			var target = this.CreateTarget<ImmutableSortedDictionary<int, int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableSortedDictionaryTest_1_Success()
		{
			var collection = ImmutableSortedDictionary.Create<int, int>();
			collection = collection.Add( 0, 0 );
			var target = this.CreateTarget<ImmutableSortedDictionary<int, int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

		[Test]
		public void ImmutableSortedDictionaryTest_2_Success()
		{
			var collection = ImmutableSortedDictionary.Create<int, int>();
			collection = collection.Add( 0, 0 );
			collection = collection.Add( 1, 1 );
			var target = this.CreateTarget<ImmutableSortedDictionary<int, int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}
	}
}
