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

#if !UNITY_METRO && !UNITY_4_5

using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Messaging;

using NUnit.Framework.Internal;

using UniRx;

using UnityEngine;
using UnityEngine.UI;

// This file is borrowed from UniRX (https://github.com/neuecc/UniRx/blob/master/Assets/UnitTests/UnitTests.tt)

// ReSharper disable AccessToStaticMemberViaDerivedType
// ReSharper disable RedundantNameQualifier
// ReSharper disable once CheckNamespace
namespace MsgPack
{
	public class UnityTestDriver : TestDriver
	{
		#region -- NUnit specific --

		private static bool IsTestFailure( Exception ex )
		{
			for ( var current = ex; current != null; current = current.InnerException )
			{
				if ( current.GetType().FullName == "NUnit.Framework.AssertionException" )
				{
					return true;
				}
			}

			return false;
		}

		private static void InitializeTestEngine()
		{
			TestExecutionContext context = new TestExecutionContext();
			context.WorkDirectory = Environment.CurrentDirectory;
			CallContext.SetData( "NUnit.Framework.TestContext", context );
		}

		private static void CleanUpTestEngine()
		{
			CallContext.FreeNamedDataSlot( "NUnit.Framework.TestContext" );
		}

		#endregion -- NUnit specific --

		public void Run( Button buttonPrefab, GameObject buttonVertical, Result resultPrefab, GameObject resultVertical )
		{
			foreach ( var item in this.TestClasses )
			{
				var testClass = item;
				var button = GameObject.Instantiate( buttonPrefab );
				button.GetComponentInChildren<Text>().text = testClass.Name + "(" + testClass.MethodCount.ToString( "#,0", CultureInfo.CurrentCulture ) + ")";
				button.OnClickAsObservable().Subscribe( _ =>
					{
						Clear( resultVertical );
						MainThreadDispatcher.StartCoroutine( RunTestCoroutine( testClass, resultPrefab, resultVertical ) );
					} 
				);
				button.transform.SetParent( buttonVertical.transform, true );
			}
		}


		private static void Clear( GameObject resultVertical )
		{
			foreach ( Transform child in resultVertical.transform )
			{
				GameObject.Destroy( child.gameObject );
			}
		}

		private static Result CreateResult( string methodName, Result resultPrefab, GameObject resultVertical )
		{
			var r = GameObject.Instantiate( resultPrefab );
			r.ForceInitialize();
			r.gameObject.transform.SetParent( resultVertical.transform, true );
			r.Message.Value = methodName;
			r.Color.Value = UnityEngine.Color.gray;
			return r;
		}

		private static void HandleFatalException( string testClassName, string stage, Exception exception, Result resultPrefab, GameObject resultVertical )
		{
			var r = CreateResult( testClassName + "." + stage, resultPrefab, resultVertical );
			r.Message.Value = testClassName + "." + stage + " FATAL " + Environment.NewLine + exception;
			r.Color.Value = UnityEngine.Color.red;
		}

		private static IEnumerator RunTestCoroutine( TestClass testClass, Result resultPrefab, GameObject resultVertical )
		{
			bool isCrashed = false;

			try
			{
				InitializeTestEngine();
			}
			catch ( Exception ex )
			{
				HandleFatalException( testClass.Name, "InitializeTestEngine", ex, resultPrefab, resultVertical );
				isCrashed = true;
			}

			if ( isCrashed )
			{
				yield break;
			}

			yield return null;

			try
			{
				testClass.FixtureSetup();
			}
			catch ( Exception ex )
			{
				HandleFatalException( testClass.Name, "FixtureSetup", ex, resultPrefab, resultVertical );
				isCrashed = true;
			}

			if ( isCrashed )
			{
				yield break;
			}

			yield return null;

			TestClassInstance instance = null;
			try
			{
				instance = testClass.NewTest();
			}
			catch ( Exception ex )
			{
				HandleFatalException( testClass.Name, "Instantiation", ex, resultPrefab, resultVertical );
				isCrashed = true;
			}

			if ( isCrashed )
			{
				yield break;
			}

			yield return null;

			foreach ( var method in instance.TestMethods )
			{
				try
				{
					instance.TestSetup();
				}
				catch ( Exception ex )
				{
					HandleFatalException( testClass.Name, "TestSetup", ex, resultPrefab, resultVertical );
					isCrashed = true;
				}

				if ( isCrashed )
				{
					yield break;
				}

				yield return null;

				var fullMethodName = testClass.Name + "." + method.Name;
				var r = CreateResult( fullMethodName, resultPrefab, resultVertical );
				var sw = System.Diagnostics.Stopwatch.StartNew();
				try
				{
					method.Method();
					r.Message.Value = method.Name + " OK " + sw.Elapsed.TotalMilliseconds + "ms";
					r.Color.Value = UnityEngine.Color.green;
				}
				catch ( Exception ex )
				{
					r.Message.Value = method.Name + " NG" + Environment.NewLine + ( IsTestFailure( ex ) ? ex.Message : ex.ToString() );
					r.Color.Value = UnityEngine.Color.red;
				}

				yield return null;

				try
				{
					instance.TestCleanup();
				}
				catch ( Exception ex )
				{
					HandleFatalException( testClass.Name, "TestCleanup", ex, resultPrefab, resultVertical );
					isCrashed = true;
				}

				if ( isCrashed )
				{
					yield break;
				}

				yield return null;
			} // foreach method

			try
			{
				testClass.FixtureCleanup();
			}
			catch ( Exception ex )
			{
				HandleFatalException( testClass.Name, "FixtureCleanup", ex, resultPrefab, resultVertical );
				isCrashed = true;
			}

			if ( isCrashed )
			{
				yield break;
			}

			yield return null;

			try
			{
				CleanUpTestEngine();
			}
			catch ( Exception ex )
			{
				HandleFatalException( testClass.Name, "InitializeTestEngine", ex, resultPrefab, resultVertical );
			}

			yield return null;
		}
	}
}

#endif // !UNITY_METRO && !UNITY_4_5