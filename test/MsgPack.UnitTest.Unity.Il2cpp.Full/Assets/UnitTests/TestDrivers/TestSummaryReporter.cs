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
	internal class TestSummaryReporter
	{
		private readonly string _testClassName;
		private readonly Result _globalResult;
		private int _succeeded;
		private int _failued;
		private int _errors;

		public TestSummaryReporter( string testClassName, Result resultPrefab, GameObject resultVertical )
		{
			this._testClassName = testClassName;
			this._globalResult = GameObject.Instantiate( resultPrefab );
			this._globalResult.ForceInitialize();
			this._globalResult.gameObject.transform.SetParent( resultVertical.transform, true );
			this._globalResult.Message.Value = testClassName;
			this._globalResult.Color.Value = UnityEngine.Color.gray;
		}

			public void RecordSuccess()
		{
			this._succeeded++;
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

#warning TODO: Inconclusive

		public void HandleFatalException( string stage, Exception exception, Result resultPrefab, GameObject resultVertical )
		{
			var message = this._testClassName + "." + stage + " FATAL " + Environment.NewLine + exception;
			UnityEngine.Debug.LogError( message );
			this._globalResult.Message.Value = message;
			this._globalResult.Color.Value = UnityEngine.Color.red;
		}


		private void UpdateResult()
		{
			this._globalResult.Message.Value =
				String.Format(
					CultureInfo.CurrentCulture,
					"{0}{1}Success:{2:#,0} Failure:{3:#,0} Error:{4:#,0}",
					this._testClassName,
					Environment.NewLine,
					this._succeeded,
					this._failued,
					this._errors
				);
			if ( this._failued > 0 || this._errors > 0 )
			{
				this._globalResult.Color.Value = UnityEngine.Color.red;
			}
			else
			{
				this._globalResult.Color.Value = UnityEngine.Color.green;
			}
		}
	}
}

#endif //  !UNITY_METRO && !UNITY_4_5
