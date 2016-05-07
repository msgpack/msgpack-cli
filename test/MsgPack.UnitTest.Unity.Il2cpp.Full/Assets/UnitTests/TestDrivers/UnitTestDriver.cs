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
using System.Diagnostics;
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
	/// <summary>
	///		Implements unit test driver for Unity IL2CPP.
	/// </summary>
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

		// TODO: View manipulation should be only in Presenter.
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
		
		private static IEnumerator RunTestCoroutine( TestClass testClass, Result resultPrefab, GameObject resultVertical )
		{
			// Summary reporter.
			var summaryReporter = new TestSummaryReporter( testClass.Name, resultPrefab, resultVertical );

			bool isCrashed = false;

			try
			{
				InitializeTestEngine();
			}
			catch ( Exception ex )
			{
				summaryReporter.HandleFatalException( "InitializeTestEngine", ex, resultPrefab, resultVertical );
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
					summaryReporter.HandleFatalException( "TestSetup", ex, resultPrefab, resultVertical );
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
						var messageHeader = method.Name + ( isFailure ? " NG" : "Error" ) + Environment.NewLine;
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

				yield return null;

				try
				{
					instance.TestCleanup();
				}
				catch ( Exception ex )
				{
					summaryReporter.HandleFatalException( "TestCleanup", ex, resultPrefab, resultVertical );
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
				summaryReporter.HandleFatalException( "InitializeTestEngine", ex, resultPrefab, resultVertical );
			}

			yield return null;
		}
	}
}

#endif // !UNITY_METRO && !UNITY_4_5