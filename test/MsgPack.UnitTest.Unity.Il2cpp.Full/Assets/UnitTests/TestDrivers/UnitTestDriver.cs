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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
	/// <summary>
	///		Implements unit test driver for Unity IL2CPP.
	/// </summary>
	public class UnityTestDriver : TestDriver
	{
		#region -- NUnit specific --

		/// <summary>
		///		Determines whether the exception represents test failure instead of exceptional case (that is, error).
		/// </summary>
		/// <param name="ex">The exxception to be determined.</param>
		/// <returns><c>true</c> if the exception represents test failure; <c>false</c>, otherwise.</returns>
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

		/// <summary>
		///		Determines whether the exception represents test skipping instead of exceptional case (that is, error).
		/// </summary>
		/// <param name="ex">The exxception to be determined.</param>
		/// <returns><c>true</c> if the exception represents test skipping; <c>false</c>, otherwise.</returns>
		private static bool IsTestSkipping( Exception ex )
		{
			for ( var current = ex; current != null; current = current.InnerException )
			{
				if ( current.GetType().FullName == "NUnit.Framework.InconclusiveException"
					|| current.GetType().FullName == "NUnit.Framework.IgnoreException"
				)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		///		Initializes the underlying test engine.
		/// </summary>
		private static void InitializeTestEngine()
		{
			TestExecutionContext context = new TestExecutionContext();
			context.WorkDirectory = Environment.CurrentDirectory;
			CallContext.SetData( "NUnit.Framework.TestContext", context );
			//UnityTestHelper.AddBindingTraceLogger( m => UnityEngine.Debug.Log( m ) );
		}

		/// <summary>
		///		Clean-ups the underlying test engine.
		/// </summary>
		private static void CleanUpTestEngine()
		{
			CallContext.FreeNamedDataSlot( "NUnit.Framework.TestContext" );
		}

		#endregion -- NUnit specific --

		// TODO: View manipulation should be only in Presenter.		
		/// <summary>
		///		Runs this test driver.
		/// </summary>
		/// <param name="buttonPrefab">The prefab for test starting buttons.</param>
		/// <param name="buttonVertical">The vertical area which test starting buttons to be belonging.</param>
		/// <param name="resultPrefab">The prefab for test result indicators.</param>
		/// <param name="resultVertical">The vertical area which test result indicators to be belonging.</param>
		public void Run( Button buttonPrefab, GameObject buttonVertical, Result resultPrefab, GameObject resultVertical )
		{
			// For all run.
			{
				var allButton = GameObject.Instantiate( buttonPrefab );
				allButton.GetComponentInChildren<Text>().text = "All(" + this.TestClasses.Sum( c => c.MethodCount ).ToString( "#,0", CultureInfo.CurrentCulture ) + ")";
				allButton.OnClickAsObservable().Subscribe( _ =>
				{
					Clear( resultVertical );
					MainThreadDispatcher.StartCoroutine( RunAllTestCoroutine( this.TestClasses, resultPrefab, resultVertical ) );
				}
				);
				allButton.transform.SetParent( buttonVertical.transform, true );
			}

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

		/// <summary>
		///		Clears the test results..
		/// </summary>
		/// <param name="resultVertical">The vertical area which holds prevous test results.</param>
		private static void Clear( GameObject resultVertical )
		{
			foreach ( Transform child in resultVertical.transform )
			{
				GameObject.Destroy( child.gameObject );
			}
		}

		/// <summary>
		///		Creates new test result.
		/// </summary>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="resultPrefab">The prefab for test result indicators.</param>
		/// <param name="resultVertical">The vertical area which test result indicators to be belonging.</param>
		/// <returns>The result object represents current test method result.</returns>
		private static Result CreateResult( string methodName, Result resultPrefab, GameObject resultVertical )
		{
			var r = GameObject.Instantiate( resultPrefab );
			r.ForceInitialize();
			r.gameObject.transform.SetParent( resultVertical.transform, true );
			r.Message.Value = methodName;
			r.Color.Value = UnityEngine.Color.gray;
			return r;
		}

		/// <summary>
		///		Runs all test as coroutine.
		/// </summary>
		/// <param name="testClasses">The list of test classes.</param>
		/// <param name="resultPrefab">The prefab for test result indicators.</param>
		/// <param name="resultVertical">The vertical area which test result indicators to be belonging.</param>
		/// <returns><see cref="IEnumerator"/> for coroutine.</returns>
		private static IEnumerator RunAllTestCoroutine( IEnumerable<TestClass> testClasses, Result resultPrefab, GameObject resultVertical )
		{
			var sumaryReporter = new TestSummaryReporter( "All tests", true, resultPrefab, resultVertical );
			foreach ( var testClass in testClasses )
			{
				sumaryReporter.SetCurrentTestClassName( testClass.Name );
				var enumerator = RuntTestCoroutineCore( testClass, sumaryReporter, resultPrefab, resultVertical );
				try
				{
					while ( enumerator.MoveNext() )
					{
						yield return enumerator.Current;
					}
				}
				finally
				{
					var asDisposable = enumerator as IDisposable;
					if ( asDisposable != null )
					{
						asDisposable.Dispose();
					}
				}

				yield return null;
			}
		}

		/// <summary>
		///		Runs specified test class as coroutine.
		/// </summary>
		/// <param name="testClass">The test class.</param>
		/// <param name="resultPrefab">The prefab for test result indicators.</param>
		/// <param name="resultVertical">The vertical area which test result indicators to be belonging.</param>
		/// <returns><see cref="IEnumerator"/> for coroutine.</returns>
		private static IEnumerator RunTestCoroutine( TestClass testClass, Result resultPrefab, GameObject resultVertical )
		{
			return RuntTestCoroutineCore( testClass, new TestSummaryReporter( testClass.Name, false, resultPrefab, resultVertical ), resultPrefab, resultVertical );
		}

		/// <summary>
		///		Runs specified test class as coroutine.
		/// </summary>
		/// <param name="testClass">The test class.</param>
		/// <param name="summaryReporter">The reporter object to report test class execution summary.</param>
		/// <param name="resultPrefab">The prefab for test result indicators.</param>
		/// <param name="resultVertical">The vertical area which test result indicators to be belonging.</param>
		/// <returns><see cref="IEnumerator"/> for coroutine.</returns>
		private static IEnumerator RuntTestCoroutineCore( TestClass testClass, TestSummaryReporter summaryReporter, Result resultPrefab, GameObject resultVertical )
		{ 
			bool isCrashed = false;

			try
			{
				InitializeTestEngine();
			}
			catch ( Exception ex )
			{
				summaryReporter.HandleFatalException( "InitializeTestEngine", ex, resultPrefab, resultVertical );
				summaryReporter.RecordError( testClass.MethodCount );
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
				summaryReporter.HandleFatalException( "FixtureSetup", ex, resultPrefab, resultVertical );
				summaryReporter.RecordError( testClass.MethodCount );
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
				summaryReporter.HandleFatalException( "Instantiation", ex, resultPrefab, resultVertical );
				summaryReporter.RecordError( testClass.MethodCount );
				isCrashed = true;
			}

			if ( isCrashed )
			{
				yield break;
			}

			yield return null;

			int remains = testClass.MethodCount;
			foreach ( var method in instance.TestMethods )
			{
				try
				{
					instance.TestSetup();
				}
				catch ( Exception ex )
				{
					summaryReporter.HandleFatalException( "TestSetup", ex, resultPrefab, resultVertical );
					summaryReporter.RecordError( remains );
					isCrashed = true;
				}

				if ( isCrashed )
				{
					yield break;
				}

				yield return null;

				var fullMethodName = testClass.Name + "." + method.Name;
				
				try
				{
					method.Method();
					summaryReporter.RecordSuccess();
					// Omit test result to reduce memory usage and avoid cluttered screen.
				}
				catch ( Exception ex )
				{
					if ( IsTestSkipping( ex ) )
					{
						summaryReporter.RecordSkip();
					}
					else
					{
						bool isFailure = IsTestFailure( ex );
						var messageHeader = summaryReporter.FormatMethodName( method.Name ) + ( isFailure ? " NG" : " Error" ) + Environment.NewLine;
						UnityEngine.Debug.LogError( messageHeader + ex );
						var r = CreateResult( fullMethodName, resultPrefab, resultVertical );
						var baseException = ex.GetBaseException();
						if ( isFailure || baseException == ex )
						{
							r.Message.Value = messageHeader + ex.Message;
						}
						else
						{
							// Record BaseException to help investigation.
							r.Message.Value = messageHeader + ex.Message + "-->" + Environment.NewLine + baseException.Message;
						}

						r.Color.Value = UnityEngine.Color.red;

						if ( isFailure )
						{
							summaryReporter.RecordFailure();
						}
						else
						{
							summaryReporter.RecordError();
						}
					}
				}

				remains--;

				yield return null;

				try
				{
					instance.TestCleanup();
				}
				catch ( Exception ex )
				{
					summaryReporter.HandleFatalException( "TestCleanup", ex, resultPrefab, resultVertical );
					summaryReporter.RecordError( remains );
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
				summaryReporter.HandleFatalException( "FixtureCleanup", ex, resultPrefab, resultVertical );
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
				summaryReporter.HandleFatalException( "CleanupTestEngine", ex, resultPrefab, resultVertical );
			}

			yield return null;
		}
	}
}

#endif // !UNITY_METRO && !UNITY_4_5