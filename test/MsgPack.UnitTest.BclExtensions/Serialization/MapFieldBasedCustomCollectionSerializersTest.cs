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
#if !SILVERLIGHT && !AOT && !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
using MsgPack.Serialization.CodeDomSerializers;
#endif // !SILVERLIGHT && !AOT && !NETSTANDARD1_1 && !NETSTANDARD1_3 && !XAMARIN
#if !SILVERLIGHT && !AOT && !NETSTANDARD1_1
using MsgPack.Serialization.EmittingSerializers;
#endif // !SILVERLIGHT && !AOT && !NETSTANDARD1_1
#if !NETFX_CORE
using Microsoft.FSharp.Collections;
#endif // !NETFX_CORE
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using SetUpAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestInitializeAttribute;
using TearDownAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestCleanupAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack.Serialization
{
	[TestFixture]
	public class MapFieldBasedImmutableCollectionsTest
	{
		private MessagePackSerializer<T> CreateTarget<T>()
		{
			var context = new SerializationContext { SerializationMethod = SerializationMethod.Map };
			context.SerializerOptions.EmitterFlavor = EmitterFlavor.FieldBased;
			return context.GetSerializer<T>( PolymorphismSchema.Default );
		}
		
		private bool CanDump
		{
			get { return true; }
		}

#if !NETFX_CORE && !SILVERLIGHT && !AOT && !XAMARIN
		[SetUp]
		public void SetUp()
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !NETSTANDARD1_6
			//SerializerDebugging.TraceEnabled = true;
			//SerializerDebugging.DumpEnabled = true;
			if ( SerializerDebugging.TraceEnabled )
			{
				Tracer.Emit.Listeners.Clear();
				Tracer.Emit.Switch.Level = SourceLevels.All;
#if NETSTANDARD2_0
				Tracer.Emit.Listeners.Add( new TextWriterTraceListener( Console.Out ) );
#else // NETSTANDRD2_0
				Tracer.Emit.Listeners.Add( new ConsoleTraceListener() );
#endif // NETSTANDRD2_0
			}

			SerializerDebugging.DependentAssemblyManager = new TempFileDependentAssemblyManager( TestContext.CurrentContext.TestDirectory );
			SerializerDebugging.DeletePastTemporaries();
			SerializerDebugging.OnTheFlyCodeGenerationEnabled = true;

#if NET35
			SerializerDebugging.SetCodeCompiler( CodeDomCodeGeneration.Compile );
#else
			SerializerDebugging.SetCodeCompiler( RoslynCodeGeneration.Compile );
#endif // NET35

			SerializerDebugging.DumpDirectory = TestContext.CurrentContext.TestDirectory;
			SerializerDebugging.AddRuntimeAssembly( typeof( ImmutableList ).Assembly.Location );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !NETSTANDARD1_6
		}

		[TearDown]
		public void TearDown()
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3 && !NETSTANDARD1_6
			if ( SerializerDebugging.DumpEnabled && this.CanDump )
			{
#if !NETSTANDARD2_0
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
					SerializationMethodGeneratorManager.Refresh();
				}
#else // !NETSTANDARD2_0
				SerializationMethodGeneratorManager.Refresh();
#endif // !NETSTANDARD2_0
			}

			SerializerDebugging.Reset();
			SerializerDebugging.OnTheFlyCodeGenerationEnabled = false;
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3 && !NETSTANDARD1_6
		}
#endif // !NETFX_CORE && !SDILVERLIGHT && !AOT && !XAMARIN

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

#if !NETFX_CORE


		[Test]
		public void FSharpListTest_0_Success()
		{
			var collection = FSharpList<int>.Empty;
			var target = this.CreateTarget<FSharpList<int>>();
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
		public void FSharpListTest_1_Success()
		{
			var collection = FSharpList<int>.Empty;
			collection = new FSharpList<int>( 0, collection );
			var target = this.CreateTarget<FSharpList<int>>();
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
		public void FSharpListTest_2_Success()
		{
			var collection = FSharpList<int>.Empty;
			collection = new FSharpList<int>( 0, collection );
			collection = new FSharpList<int>( 1, collection );
			var target = this.CreateTarget<FSharpList<int>>();
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
		public void FSharpSetTest_0_Success()
		{
			var collection = new FSharpSet<int>( Enumerable.Empty<int>() );
			var target = this.CreateTarget<FSharpSet<int>>();
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
		public void FSharpSetTest_1_Success()
		{
			var collection = new FSharpSet<int>( Enumerable.Empty<int>() );
			collection = collection.Add( 0 );
			var target = this.CreateTarget<FSharpSet<int>>();
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
		public void FSharpSetTest_2_Success()
		{
			var collection = new FSharpSet<int>( Enumerable.Empty<int>() );
			collection = collection.Add( 0 );
			collection = collection.Add( 1 );
			var target = this.CreateTarget<FSharpSet<int>>();
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
		public void FSharpMapTest_0_Success()
		{
			var collection = new FSharpMap<int, int>( Enumerable.Empty<Tuple<int, int>>() );
			var target = this.CreateTarget<FSharpMap<int, int>>();
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
		public void FSharpMapTest_1_Success()
		{
			var collection = new FSharpMap<int, int>( Enumerable.Empty<Tuple<int, int>>() );
			collection = collection.Add( 0, 0 );
			var target = this.CreateTarget<FSharpMap<int, int>>();
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
		public void FSharpMapTest_2_Success()
		{
			var collection = new FSharpMap<int, int>( Enumerable.Empty<Tuple<int, int>>() );
			collection = collection.Add( 0, 0 );
			collection = collection.Add( 1, 1 );
			var target = this.CreateTarget<FSharpMap<int, int>>();
			using ( var buffer = new MemoryStream() )
			{
				target.Pack( buffer, collection );
				buffer.Position = 0;
				var unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				Assert.That( unpacked.ToArray(), Is.EqualTo( collection.ToArray() ) );
			}
		}

#endif // !NETFX_CORE

	}
}
