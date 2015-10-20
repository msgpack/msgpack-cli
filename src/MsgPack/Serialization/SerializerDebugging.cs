#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
#if !NETFX_35 && !UNITY && !WINDOWS_PHONE
using System.Collections.Concurrent;
#endif // !NETFX_35 && !UNITY && !WINDOWS_PHONE
using System.Collections.Generic;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Holds debugging support information.
	/// </summary>
	internal static class SerializerDebugging
	{
#if !XAMIOS && !XAMDROID && !UNITY
		[ThreadStatic]
		private static bool _traceEnabled;

		/// <summary>
		///		Gets or sets a value indicating whether instruction/expression tracing is enabled or not.
		/// </summary>
		/// <value>
		///		<c>true</c> if instruction/expression tracing is enabled; otherwise, <c>false</c>.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static bool TraceEnabled
		{
			get { return _traceEnabled; }
			set { _traceEnabled = value; }
		}

		[ThreadStatic]
		private static bool _dumpEnabled;

		/// <summary>
		///		Gets or sets a value indicating whether IL dump is enabled or not.
		/// </summary>
		/// <value>
		///   <c>true</c> if IL dump is enabled; otherwise, <c>false</c>.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static bool DumpEnabled
		{
			get { return _dumpEnabled; }
			set { _dumpEnabled = value; }
		}
#endif // !XAMIOS && !XAMDROID && !UNITY

		[ThreadStatic]
		private static bool _avoidsGenericSerializer;

		/// <summary>
		///		Gets or sets a value indicating whether generic serializer for array, <see cref="List{T}"/>, <see cref="Dictionary{TKey,TValue}"/>, 
		///		or <see cref="Nullable{T}"/> is not used.
		/// </summary>
		/// <value>
		///   <c>true</c> if generic serializer is not used; otherwise, <c>false</c>.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static bool AvoidsGenericSerializer
		{
			get { return _avoidsGenericSerializer; }
			set { _avoidsGenericSerializer = value; }
		}

#if !XAMIOS && !XAMDROID && !UNITY
#if !NETFX_CORE
		[ThreadStatic]
		private static StringWriter _ilTraceWriter;
#endif // !NETFX_CORE

		/// <summary>
		///		Gets the <see cref="TextWriter"/> for IL tracing.
		/// </summary>
		/// <value>
		///		The <see cref="TextWriter"/> for IL tracing.
		///		This value will not be <c>null</c>.
		/// </value>
		public static TextWriter ILTraceWriter
		{
			get
			{
#if !NETFX_CORE
				if ( !_traceEnabled )
				{
					return TextWriter.Null;
				}

				if ( _ilTraceWriter == null )
				{
					_ilTraceWriter = new StringWriter( CultureInfo.InvariantCulture );
				}

				return _ilTraceWriter;
#else
				return TextWriter.Null;
#endif // !NETFX_CORE
			}
		}

		/// <summary>
		///		Traces the specific event.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">The args for formatting.</param>
#if NETFX_CORE || WINDOWS_PHONE
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "format", Justification = "Used in other platforms" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args", Justification = "Used in other platforms" )]
#endif // NETFX_CORE || WINDOWS_PHONE
		public static void TraceEvent( string format, params object[] args )
		{
#if !NETFX_CORE && !WINDOWS_PHONE
			if ( !_traceEnabled )
			{
				return;
			}

			Tracer.Emit.TraceEvent( Tracer.EventType.DefineType, Tracer.EventId.DefineType, format, args );
#endif // !NETFX_CORE && !WINDOWS_PHONE
		}

		/// <summary>
		///		Flushes the trace data.
		/// </summary>
		public static void FlushTraceData()
		{
#if !NETFX_CORE && !WINDOWS_PHONE
			if ( !_traceEnabled )
			{
				return;
			}

			Tracer.Emit.TraceData( Tracer.EventType.DefineType, Tracer.EventId.DefineType, _ilTraceWriter.ToString() );
#endif // !NETFX_CORE && !WINDOWS_PHONE
		}

#if !NETFX_CORE && !SILVERLIGHT && !NETFX_35
		[ThreadStatic]
		private static AssemblyBuilder _assemblyBuilder;

		[ThreadStatic]
		private static ModuleBuilder _moduleBuilder;

		/// <summary>
		///		Prepares instruction dump with specified <see cref="AssemblyBuilder"/>.
		/// </summary>
		/// <param name="assemblyBuilder">The assembly builder to hold instructions.</param>
		public static void PrepareDump( AssemblyBuilder assemblyBuilder )
		{
			if ( _dumpEnabled )
			{
#if DEBUG
				Contract.Assert( assemblyBuilder != null );
#endif // DEBUG
				_assemblyBuilder = assemblyBuilder;
			}
		}

		/// <summary>
		///		Prepares the dump with dedicated internal <see cref="AssemblyBuilder"/>.
		/// </summary>
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
#endif // !NETFX_CORE && !SILVERLIGHT && !NETFX_35
		// TODO: Cleanup %Temp% to delete temp assemblies generated for on the fly code DOM.

#if !NETFX_CORE && !SILVERLIGHT
		[ThreadStatic]
		private static IList<string> _runtimeAssemblies;

		[ThreadStatic]
		private static IList<string> _compiledCodeDomSerializerAssemblies;

		public static IEnumerable<string> CodeDomSerializerDependentAssemblies
		{
			get
			{
				EnsureDependentAssembliesListsInitialized();
#if DEBUG
				Contract.Assert( _compiledCodeDomSerializerAssemblies != null );
#endif // DEBUG
				// FCL dependencies and msgpack core libs
				foreach ( var runtimeAssembly in _runtimeAssemblies )
				{
					yield return runtimeAssembly;
				}

				// dependents
				foreach ( var compiledAssembly in _compiledCodeDomSerializerAssemblies )
				{
					yield return compiledAssembly;
				}
			}
		}
#endif // !NETFX_CORE && !SILVERLIGHT
#if NETFX_CORE || SILVERLIGHT
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "pathToAssembly", Justification = "For API compatibility" )]
#endif // NETFX_CORE || SILVERLIGHT
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void AddRuntimeAssembly( string pathToAssembly )
		{
#if !NETFX_CORE && !SILVERLIGHT
			EnsureDependentAssembliesListsInitialized();
			_runtimeAssemblies.Add( pathToAssembly );
#endif // !NETFX_CORE && !SILVERLIGHT
		}

#if NETFX_CORE || SILVERLIGHT
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "pathToAssembly", Justification = "For API compatibility" )]
#endif // NETFX_CORE || SILVERLIGHT
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void AddCompiledCodeDomAssembly( string pathToAssembly )
		{
#if !NETFX_CORE && !SILVERLIGHT
			EnsureDependentAssembliesListsInitialized();
			_compiledCodeDomSerializerAssemblies.Add( pathToAssembly );
#endif // !NETFX_CORE && !SILVERLIGHT
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void ResetDependentAssemblies()
		{
#if !NETFX_CORE && !SILVERLIGHT
			EnsureDependentAssembliesListsInitialized();

#if !NETFX_35
			File.AppendAllLines( GetHistoryFilePath(), _compiledCodeDomSerializerAssemblies );
#else
			File.AppendAllText( GetHistoryFilePath(), String.Join( Environment.NewLine, _compiledCodeDomSerializerAssemblies.ToArray() ) + Environment.NewLine );
#endif // !NETFX_35
			_compiledCodeDomSerializerAssemblies.Clear();
			ResetRuntimeAssemblies();
#endif // !NETFX_CORE && !SILVERLIGHT
		}

#if !NETFX_CORE && !SILVERLIGHT
		private static int _wasDeleted;
		private const string HistoryFile = "MsgPack.Serialization.SerializationGenerationDebugging.CodeDOM.History.txt";

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" , Justification = "For unit testing")]
		public static void DeletePastTemporaries()
		{
			if ( Interlocked.CompareExchange( ref _wasDeleted, 1, 0 ) != 0 )
			{
				return;
			}

			try
			{
				var historyFilePath = GetHistoryFilePath();
				if ( !File.Exists( historyFilePath ) )
				{
					return;
				}

				foreach ( var pastAssembly in File.ReadAllLines( historyFilePath ) )
				{
					if ( !String.IsNullOrEmpty( pastAssembly ) )
					{
						File.Delete( pastAssembly );
					}
				}

				new FileStream( historyFilePath, FileMode.Truncate ).Close();
			}
			catch ( IOException ) { }
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" , Justification = "For unit testing")]
		private static string GetHistoryFilePath()
		{
			return Path.Combine( Path.GetTempPath(), HistoryFile );
		}

		private static void EnsureDependentAssembliesListsInitialized()
		{
			if ( _runtimeAssemblies == null )
			{
				_runtimeAssemblies = new List<string>();
				ResetRuntimeAssemblies();
			}

			if ( _compiledCodeDomSerializerAssemblies == null )
			{
				_compiledCodeDomSerializerAssemblies = new List<string>();
			}
		}

		private static void ResetRuntimeAssemblies()
		{
			_runtimeAssemblies.Add( "System.dll" );
#if NETFX_35
			_runtimeAssemblies.Add( typeof( Enumerable ).Assembly.Location );
#else
			_runtimeAssemblies.Add( "System.Core.dll" );
			_runtimeAssemblies.Add( "System.Numerics.dll" );
#endif // NETFX_35
			_runtimeAssemblies.Add( typeof( SerializerDebugging ).Assembly.Location );
		}
#endif // !NETFX_CORE && !SILVERLIGHT

		[ThreadStatic]
		private static bool _onTheFlyCodeDomEnabled;

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static bool OnTheFlyCodeDomEnabled
		{
			get { return _onTheFlyCodeDomEnabled; }
			set { _onTheFlyCodeDomEnabled = value; }
		}

#if !NETFX_CORE && !SILVERLIGHT && !NETFX_35
		/// <summary>
		///		Creates the new type builder for the serializer.
		/// </summary>
		/// <param name="targetType">The serialization target type.</param>
		/// <returns></returns>
		/// <exception cref="System.InvalidOperationException">PrepareDump() was not called.</exception>
		public static TypeBuilder NewTypeBuilder( Type targetType )
		{
			if ( _moduleBuilder == null )
			{
				throw new InvalidOperationException( "PrepareDump() was not called." );
			}

			return
				_moduleBuilder.DefineType( IdentifierUtility.EscapeTypeName( targetType ) + "SerializerLogics" );
		}
#endif // !NETFX_CORE && !SILVERLIGHT && !NETFX_35

#if !NETFX_CORE && !SILVERLIGHT
		/// <summary>
		///		Takes dump of instructions.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" , Justification = "For unit testing")]
		public static void Dump()
		{
#if !NETFX_35
			if ( _assemblyBuilder != null )
			{
				_assemblyBuilder.Save( _assemblyBuilder.GetName().Name + ".dll" );
			}
#endif // !NETFX_35
		}
#endif // !NETFX_CORE && !SILVERLIGHT

		/// <summary>
		///		Resets debugging states.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void Reset()
		{
#if !NETFX_CORE && !SILVERLIGHT && !NETFX_35
			_assemblyBuilder = null;
			_moduleBuilder = null;

			if ( _ilTraceWriter != null )
			{
				_ilTraceWriter.Dispose();
				_ilTraceWriter = null;
			}
#endif // !NETFX_CORE && !SILVERLIGHT && !NETFX_35

			_dumpEnabled = false;
			_traceEnabled = false;
			ResetDependentAssemblies();
		}
#endif // !XAMIOS && !XAMDROID && !UNITY
	}
}