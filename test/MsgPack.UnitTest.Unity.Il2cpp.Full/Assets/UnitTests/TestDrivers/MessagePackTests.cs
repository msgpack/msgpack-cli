

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

// This file is borrowed from UniRX (https://github.com/neuecc/UniRx/blob/master/Assets/UnitTests/UnitTests.tt)
#if !UNITY_METRO && !UNITY_4_5
using System;
using System.Collections.Generic;
using System.Linq;

using MsgPack;
using MsgPack.Serialization;

namespace MsgPack
{
	public partial class TestClass
	{
		private static readonly Action Nop = () => {};
		private static readonly Action<TestClassInstance, object> NoInitialization = ( c, i ) => {};
		public string Name { get; private set; }
		public Action FixtureSetup { get; set; }
		public Action FixtureCleanup { get; set; }
		public int MethodCount { get; private set; }
		private readonly Func<object> _instanceFactory;
		private readonly Action<TestClassInstance, object> _testClassInstanceInitializer;

		public TestClass( string name, Func<object> instanceFactory, int methodCount, Action<TestClassInstance, object> testClassInstanceInitializer )
		{
			if ( String.IsNullOrEmpty( name ) )
			{
				throw new ArgumentException( "name cannot be null nor empty.", "name" );
			}

			if ( instanceFactory == null )
			{
				throw new ArgumentNullException( "instanceFactory" );
			}

			this.Name = name;
			this._instanceFactory = instanceFactory;
			this._testClassInstanceInitializer = testClassInstanceInitializer ?? NoInitialization;
			this.MethodCount = methodCount;
			this.FixtureSetup = Nop;
			this.FixtureCleanup = Nop;
		}

		public TestClassInstance NewTest()
		{
			var instance = this._instanceFactory();
			var result = new TestClassInstance( this.MethodCount );
			this._testClassInstanceInitializer( result, instance );
			return result;
		}
	}

	public partial class TestClassInstance
	{
		private static readonly Action Nop = () => {};
		public Action TestSetup { get; set; }
		public Action TestCleanup { get; set; }
		public IList<TestMethod> TestMethods { get; private set; }

		public TestClassInstance( int methodCount )
		{
			this.TestMethods = new List<TestMethod>( methodCount );
			this.TestSetup = Nop;
			this.TestCleanup = Nop;
		}
	}

	public partial class TestMethod
	{
		public string Name { get; private set; }
		public Action Method { get; private set; }

		public TestMethod( string name, Action method )
		{
			this.Name = name;
			this.Method = method;
		}
	}

	public partial class TestDriver
	{
		protected IList<TestClass> TestClasses { get; private set; }

		protected TestDriver()
		{
			this.TestClasses = NewTestClasses();
			InitializeTestClasses( this.TestClasses );
		}
	}

	partial class TestDriver
	{
		private static void InitializeTestClasses( IList<TestClass> testClasses )
		{
			var testClass = 
				new TestClass( 
					"PackerTest", 
					() => new PackerTest(), 
					3,
					( testClassInstance, testFixtureInstance ) =>
					{
						var instance = ( ( PackerTest )testFixtureInstance );
						testClassInstance.TestMethods.Add( new TestMethod( "TestCreate_OwnsStreamIsFalse_StreamIsNotClosed", new Action( instance.TestCreate_OwnsStreamIsFalse_StreamIsNotClosed ) ) );
						testClassInstance.TestMethods.Add( new TestMethod( "TestCreate_OwnsStreamIsTrue_StreamIsClosed", new Action( instance.TestCreate_OwnsStreamIsTrue_StreamIsClosed ) ) );
						testClassInstance.TestMethods.Add( new TestMethod( "TestCreate_StreamIsNull", new Action( instance.TestCreate_StreamIsNull ) ) );
					}
				 );
				testClasses.Add( testClass );

		} // void InitializeTestClasses

		private static IList<TestClass> NewTestClasses()
		{
			return new List<TestClass>( 1 );
		}

	} // partial class TestDriver
}
#endif // !UNITY_METRO && !UNITY_4_5
