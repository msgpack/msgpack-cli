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
		private string _currentTestClassName;
		private readonly bool _shouldUseQualifiedMethodName;
		private readonly Result _globalResult;
		private int _succeeded;
		private int _skipped;
		private int _failued;
		private int _errors;

		public TestSummaryReporter( string testClassName, bool shouldUseQualifiedMethodName, Result resultPrefab, GameObject resultVertical )
		{
			this._currentTestClassName = testClassName;
			this._shouldUseQualifiedMethodName = shouldUseQualifiedMethodName;
			this._globalResult = GameObject.Instantiate( resultPrefab );
			this._globalResult.ForceInitialize();
			this._globalResult.gameObject.transform.SetParent( resultVertical.transform, true );
			this._globalResult.Message.Value = testClassName;
			this._globalResult.Color.Value = UnityEngine.Color.gray;
		}

		public void SetCurrentTestClassName( string testClassName )
		{
			this._currentTestClassName = testClassName;
		}

		public string FormatMethodName( string methodName )
		{
			if ( this._shouldUseQualifiedMethodName )
			{
				return this._currentTestClassName + "." + methodName;
			}
			else
			{
				return methodName;
			}
		}

		public void RecordSuccess()
		{
			this._succeeded++;
			this.UpdateResult();
		}

		public void RecordSkip()
		{
			this._skipped++;
			this.UpdateResult();
		}

		public void RecordFailure()
		{
			this._failued++;
			this.UpdateResult();
		}

		public void RecordError()
		{
			this._errors++;
			this.UpdateResult();
		}

		public void HandleFatalException( string stage, Exception exception, Result resultPrefab, GameObject resultVertical )
		{
			var message = this._currentTestClassName + "." + stage + " FATAL " + Environment.NewLine + exception;
			UnityEngine.Debug.LogError( message );
			this._globalResult.Message.Value = message;
			this._globalResult.Color.Value = UnityEngine.Color.red;
		}


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
					this._failued,
					this._errors
				);
			if ( this._failued > 0 || this._errors > 0 )
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
