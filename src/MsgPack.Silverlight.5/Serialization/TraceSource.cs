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

#if SILVERLIGHT || XAMDROID
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For API compatibility" )]
		public SourceSwitch Switch
		{
			get { return this._switch; }
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Debugging code only" )]
		private readonly string _name;

		public TraceSource( string name )
		{
			this._name = name;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "For API compatibility" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "eventType", Justification = "Debugging code only" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "eventId", Justification = "Debugging code only" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "data", Justification = "Debugging code only" )]
		public void TraceData( TraceEventType eventType, int eventId, object data )
		{
			Debug.WriteLine( _template, this._name, eventType, eventId, data );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "For API compatibility" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "eventType", Justification = "Debugging code only" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "eventId", Justification = "Debugging code only" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "format", Justification = "Debugging code only" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args", Justification = "Debugging code only" )]
		public void TraceEvent( TraceEventType eventType, int eventId, string format, params object[] args )
		{
			Debug.WriteLine( _template, this._name, eventType, eventId, String.Format( CultureInfo.CurrentCulture, format, args ) );
		}
	}
}
#endif // if SILVERLIGHT || XAMDROID