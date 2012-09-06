#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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

#if SILVERLIGHT
using System;
using System.Diagnostics;
using System.Globalization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		System.Diagnostics.TraceSource alternative.
	/// </summary>
	internal sealed class TraceSource
	{
		private const string _template = "{0} {1}: {2} :{3}";

		private readonly SourceSwitch _switch = new SourceSwitch() { Level = SourceLevels.All };

		public SourceSwitch Switch
		{
			get { return this._switch; }
		}

		private readonly string _name;

		public TraceSource( string name )
		{
			this._name = name;
		}

		public void TraceData( TraceEventType eventType, int eventId, object data )
		{
			Debug.WriteLine( _template, this._name, eventType, eventId, data );
		}

		public void TraceEvent( TraceEventType eventType, int eventId, string format, params object[] args )
		{
			Debug.WriteLine( _template, this._name, eventType, eventId, String.Format( CultureInfo.CurrentCulture, format, args ) );
		}
	}
}
#endif