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
using System.Globalization;

using UnityEngine;

namespace MsgPack
{
	/// <summary>
	///		Implements test reporting via dedicated <see cref="Result"/> object.
	/// </summary>
	internal class TestSummaryReporter
	{
		/// <summary>
		///		Current test class name. This field will be changed via <see cref="SetCurrentTestClassName"/> when all-run mode.
		/// </summary>
		private string _currentTestClassName;

		/// <summary>
		///		<c>true</c> if the summary reporting is in context of all-run mode.
		/// </summary>
		private readonly bool _isAllRunMode;

		/// <summary>
		///		The <see cref="Result"/> object which represents test class or all test result summary.
		/// </summary>
		private readonly Result _globalResult;

		/// <summary>
		///		The number of succeeded tests.
		/// </summary>
		private int _succeeded;

		/// <summary>
		///		The number of skipped tests.
		/// </summary>
		private int _skipped;

		/// <summary>
		///		The number of failed tests.
		/// </summary>
		private int _failed;

		/// <summary>
		///		The number of test errors.
		/// </summary>
		private int _errors;

		/// <summary>
		///		Initializes a new instance of the <see cref="TestSummaryReporter"/> class.
		/// </summary>
		/// <param name="testClassName">Name of the test class.</param>
		/// <param name="isAllRunMode"><c>true</c> if the summary reporting is in context of all-run mode..</param>
		/// <param name="resultPrefab">The prefab for test result indicators.</param>
		/// <param name="resultVertical">The vertical area which test result indicators to be belonging.</param>
		public TestSummaryReporter( string testClassName, bool isAllRunMode, Result resultPrefab, GameObject resultVertical )
		{
			this._currentTestClassName = testClassName;
			this._isAllRunMode = isAllRunMode;
			this._globalResult = CreateNewResult( resultPrefab, resultVertical );
			this._globalResult.Message.Value = testClassName;
			this._globalResult.Color.Value = UnityEngine.Color.gray;
		}

		/// <summary>
		///		Creates the new <see cref="Result"/> object.
		/// </summary>
		/// <param name="resultPrefab">The prefab for test result indicators.</param>
		/// <param name="resultVertical">The vertical area which test result indicators to be belonging.</param>
		/// <returns>The new <see cref="Result"/> object with finishing common initialization.</returns>
		private static Result CreateNewResult( Result resultPrefab, GameObject resultVertical )
		{
			var r = GameObject.Instantiate( resultPrefab );
			r.ForceInitialize();
			r.gameObject.transform.SetParent( resultVertical.transform, true );
			return r;
		}

		/// <summary>
		///		Sets the name of the current test class.
		/// </summary>
		/// <param name="testClassName">Name of the test class.</param>
		public void SetCurrentTestClassName( string testClassName )
		{
			this._currentTestClassName = testClassName;
		}

		/// <summary>
		///		Formats the name of the method for reporting.
		/// </summary>
		/// <param name="methodName">Name of the method.</param>
		/// <returns>The formatted method name.</returns>
		public string FormatMethodName( string methodName )
		{
			if ( this._isAllRunMode )
			{
				return this._currentTestClassName + "." + methodName;
			}
			else
			{
				return methodName;
			}
		}

		/// <summary>
		///		Records that last test has been succeeded.
		/// </summary>
		public void RecordSuccess()
		{
			this._succeeded++;
			this.UpdateResult();
		}

		/// <summary>
		///		Records that last test has been skipped.
		/// </summary>
		public void RecordSkip()
		{
			this._skipped++;
			this.UpdateResult();
		}

		/// <summary>
		///		Records that last test has been failed.
		/// </summary>
		public void RecordFailure()
		{
			this._failed++;
			this.UpdateResult();
		}

		/// <summary>
		///		Records that an error has been ocurred.
		/// </summary>
		public void RecordError()
		{
			this.RecordError( 1 );
		}

		/// <summary>
		///		Records that a critical error has been ocurred and specified tests will be skipped as error.
		/// </summary>
		/// <param name="errorCount">The count of tests will be skipped as error</param>
		public void RecordError( int errorCount )
		{
			if ( errorCount <= 0 )
			{
				UnityEngine.Debug.LogWarning( "Invalid error count:" + errorCount );
				return;
			}

			this._errors += errorCount;
			this.UpdateResult();
		}

		/// <summary>
		///		Handles the fatal exception.
		/// </summary>
		/// <param name="stage">The stage of test execution for reporting.</param>
		/// <param name="exception">The fatal exception.</param>
		/// <param name="resultPrefab">The prefab for test result indicators.</param>
		/// <param name="resultVertical">The vertical area which test result indicators to be belonging.</param>
		public void HandleFatalException( string stage, Exception exception, Result resultPrefab, GameObject resultVertical )
		{
			var message = this._currentTestClassName + "." + stage + " FATAL " + Environment.NewLine + exception;
			UnityEngine.Debug.LogError( message );
			var result = this._isAllRunMode ? this._globalResult : CreateNewResult( resultPrefab, resultVertical );
			result.Message.Value = message;
			result.Color.Value = UnityEngine.Color.red;
		}

		/// <summary>
		///		Updates the summary <see cref="Result"/>.
		/// </summary>
		private void UpdateResult()
		{
			this._globalResult.Message.Value =
				String.Format(
					CultureInfo.CurrentCulture,
					"{0}{1}Success:{2:#,0} Skipped:{3:#,0}, Failure:{4:#,0} Error:{5:#,0}",
					this._currentTestClassName,
					Environment.NewLine,
					this._succeeded,
					this._skipped,
					this._failed,
					this._errors
				);
			if ( this._failed > 0 || this._errors > 0 )
			{
				this._globalResult.Color.Value = UnityEngine.Color.red;
			}
			else if( this._succeeded > 0 )
			{
				this._globalResult.Color.Value = UnityEngine.Color.green;
			}
		}
	}
}

#endif //  !UNITY_METRO && !UNITY_4_5
