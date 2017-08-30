#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2013 FUJIWARA, Yusuke
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Threading.Tasks
{
	/// <summary>
	///		Dummy Task for compability.
	/// </summary>
	internal class Task : IDisposable
	{
		private static readonly TaskFactory _factory = new TaskFactory();

		public static TaskFactory Factory
		{
			get { return _factory; }
		}

		private readonly ManualResetEventSlim _event;
		private readonly Action<object> _action;
		private Exception _exception;

		internal Task( Action<object> action, object state )
		{
			this._action = action;
			this._event = new ManualResetEventSlim( false );
			ThreadPool.QueueUserWorkItem( this.Run, state );
		}

		public void Dispose()
		{
			this._event.Dispose();
		}

		private void Run( object state )
		{
			try
			{
				this._action( state );
			}
			catch ( Exception ex )
			{
				Interlocked.Exchange( ref this._exception, ex );
			}
			finally
			{
				try
				{
					this._event.Set();
				}
				catch ( ObjectDisposedException ) { }
			}
		}

		public static bool WaitAll( params Task[] tasks )
		{
			return WaitHandle.WaitAll( tasks.Select( t => t._event.WaitHandle ).ToArray() );
		}
	}
}
