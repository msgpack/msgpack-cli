#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Holds debugging support information.
	/// </summary>
	internal static class SerializerDebugging
	{
		[ThreadStatic]
		private static bool _traceEnabled;

		public static bool TraceEnabled
		{
			get { return _traceEnabled; }
			set { _traceEnabled = value; }
		}

		[ThreadStatic]
		private static bool _dumpEnabled;

		public static bool DumpEnabled
		{
			get { return _dumpEnabled; }
			set { _dumpEnabled = value; }
		}

		[ThreadStatic]
		private static StringWriter _ilTraceWriter;

		public static TextWriter ILTraceWriter
		{
			get
			{
				if ( !_traceEnabled )
				{
					return TextWriter.Null;
				}

				if ( _ilTraceWriter == null )
				{
					_ilTraceWriter = new StringWriter( CultureInfo.InvariantCulture );
				}

				return _ilTraceWriter;
			}
		}

		public static void TraceInstruction( string format, params object[] args )
		{
			if ( !_traceEnabled )
			{
				return;
			}

			_ilTraceWriter.WriteLine( format, args );
		}

		public static void TraceEvent( string format, params object[] args )
		{
			if ( !_traceEnabled )
			{
				return;
			}

			Tracer.Emit.TraceEvent( Tracer.EventType.DefineType, Tracer.EventId.DefineType, format, args );
		}

		public static void FlushTraceData()
		{
			if ( !_traceEnabled )
			{
				return;
			}

			Tracer.Emit.TraceData( Tracer.EventType.DefineType, Tracer.EventId.DefineType, _ilTraceWriter.ToString() );
		}

		[ThreadStatic]
		private static AssemblyBuilder _assemblyBuilder;

		[ThreadStatic]
		private static ModuleBuilder _moduleBuilder;

		public static void PrepareDump( AssemblyBuilder assemblyBuilder )
		{
			if ( _dumpEnabled )
			{
#if DEBUG
				Contract.Assert( assemblyBuilder != null );
#endif
				_assemblyBuilder = assemblyBuilder;
			}
		}

		public static void PrepareDump()
		{
			_assemblyBuilder =
				AppDomain.CurrentDomain.DefineDynamicAssembly(
					new AssemblyName( "ExpressionTreeSerializerLogics" ),
					AssemblyBuilderAccess.Save,
					default( IEnumerable<CustomAttributeBuilder> )
				);
			_moduleBuilder =
				_assemblyBuilder.DefineDynamicModule( "ExpressionTreeSerializerLogics", "ExpressionTreeSerializerLogics.dll", true );
		}

		public static TypeBuilder NewTypeBuilder( Type type )
		{
			if ( _moduleBuilder == null )
			{
				throw new InvalidOperationException( "PrepareDump() was not called." );
			}

			return
				_moduleBuilder.DefineType( IdentifierUtility.EscapeTypeName( type ) + "SerializerLogics" );
		}

		public static void Dump()
		{
			if ( _assemblyBuilder != null )
			{
				_assemblyBuilder.Save( _assemblyBuilder.GetName().Name + ".dll" );
			}
		}

		public static void Reset()
		{
			_assemblyBuilder = null;
			_moduleBuilder = null;

			if ( _ilTraceWriter != null )
			{
				_ilTraceWriter.Dispose();
				_ilTraceWriter = null;
			}

			_dumpEnabled = false;
			_traceEnabled = false;
		}
	}
}