#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2018 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Diagnostics;
#if NETSTANDARD1_1 || NETSTANDARD1_3 || ( UNITY && !MSGPACK_UNITY_FULL )
using System.Globalization;
#endif // NETSTANDARD1_1 || NETSTANDARD1_3 || ( UNITY && !MSGPACK_UNITY_FULL )

namespace MsgPack.Serialization
{
#if UNITY && DEBUG
	public
#else
	internal
#endif
	static class Tracer
	{
		public static readonly TraceSource Emit = new TraceSource( "MsgPack.Serialization.Emit" );
		public static readonly TraceSource Binding = new TraceSource( "MsgPack.Serialization.Binding" );
		public static readonly TraceSource Tracing = new TraceSource( "MsgPack.Serialization.Tracing" );

		public static class EventId
		{
			public const int Trace = 0;
			public const int ILTrace = 101;
			public const int DefineType = 102;
			public const int PolimorphicSchema = 103;
			public const int NoAccessorFound = 901;
			public const int MultipleAccessorFound = 902;
			public const int ReadOnlyValueTypeMember = 903;
			public const int UnsupportedType = 10901;
		}

		public static class EventType
		{
			public const TraceEventType Trace = TraceEventType.Verbose;
			public const TraceEventType ILTrace = TraceEventType.Verbose;
			public const TraceEventType DefineType = TraceEventType.Verbose;
			public const TraceEventType PolimorphicSchema = TraceEventType.Verbose;
			public const TraceEventType NoAccessorFound = TraceEventType.Verbose;
			public const TraceEventType MultipleAccessorFound = TraceEventType.Verbose;
			public const TraceEventType ReadOnlyValueTypeMember = TraceEventType.Verbose;
			public const TraceEventType UnsupportedType = TraceEventType.Information;
		}
	}

#if NETSTANDARD1_1 || NETSTANDARD1_3 || ( UNITY && !MSGPACK_UNITY_FULL )
#if UNITY && DEBUG
	public
#else
	internal
#endif
	enum TraceEventType
	{
		Critical = 1,
		Error = 2,
		Information = 8,
		Verbose = 16,
		Warning = 4,
	}

#if UNITY && DEBUG
	public
#else
	internal
#endif
	class TraceSource
	{
		private readonly string _name;

		public TraceSource( string name )
		{
			this._name = name;
		}

		[Conditional( "TRACE" )]
		public void TraceEvent( TraceEventType eventType, int id, string format, params object[] args )
		{
#if !UNITY
			Debug.WriteLine( String.Format( CultureInfo.InvariantCulture, "{0} {1}: {2} : {3}", this._name, eventType, id, String.Format( CultureInfo.InvariantCulture, format, args ) ) );
#endif // !UNITY
		}

		[Conditional( "TRACE" )]
		public void TraceData( TraceEventType eventType, int id, object data )
		{
#if !UNITY
			Debug.WriteLine( String.Format( CultureInfo.InvariantCulture, "{0} {1}: {2} : {3}", this._name, eventType, id, data ) );
#endif // !UNITY
		}
	}
#endif // NETSTANDARD1_1 || NETSTANDARD1_3 || ( UNITY && !MSGPACK_UNITY_FULL )
}
