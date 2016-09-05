#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
#define AOT
#endif

using System;
#if !NETFX_35 && !UNITY && !WINDOWS_PHONE
using System.Collections.Concurrent;
#endif // !NETFX_35 && !UNITY && !WINDOWS_PHONE
using System.Collections.Generic;
#if CORE_CLR || UNITY
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || UNITY
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Holds debugging support information.
	/// </summary>
	internal static class SerializerDebugging
	{
#if !AOT
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

#if !NETSTANDARD1_1 && !NETSTANDARD1_3
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
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
#endif // !AOT

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

#if !AOT && !SILVERLIGHT
		[ThreadStatic]
		private static StringWriter _ilTraceWriter;

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
				if ( !_traceEnabled )
				{
					return NullTextWriter.Instance;
				}

				if ( _ilTraceWriter == null )
				{
					_ilTraceWriter = new StringWriter( CultureInfo.InvariantCulture );
				}

				return _ilTraceWriter;
			}
		}

		/// <summary>
		///		Traces the emitting event.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">The args for formatting.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "format", Justification = "Used in other platforms" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args", Justification = "Used in other platforms" )]
		public static void TraceEmitEvent( string format, params object[] args )
		{
			if ( !_traceEnabled )
			{
				return;
			}

			Tracer.Emit.TraceEvent( Tracer.EventType.DefineType, Tracer.EventId.DefineType, format, args );
		}

		/// <summary>
		///		Traces the polymorphic schema event.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="memberInfo">The target of schema.</param>
		/// <param name="schema">The schema.</param>

		public static void TracePolimorphicSchemaEvent( string format, MemberInfo memberInfo, PolymorphismSchema schema )
		{
			if ( !_traceEnabled )
			{
				return;
			}

			Tracer.Emit.TraceEvent( Tracer.EventType.PolimorphicSchema, Tracer.EventId.PolimorphicSchema, format, memberInfo, schema == null ? "(null)" : schema.DebugString );
		}

		/// <summary>
		///		Flushes the trace data.
		/// </summary>
		public static void FlushTraceData()
		{
			if ( !_traceEnabled )
			{
				return;
			}

			_ilTraceWriter.WriteLine();
			Tracer.Emit.TraceData( Tracer.EventType.DefineType, Tracer.EventId.DefineType, _ilTraceWriter.ToString() );
			_ilTraceWriter.GetStringBuilder().Length = 0;
		}

#if !NETSTANDARD1_1 && !NETSTANDARD1_3
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
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
#endif // !SILVERLIGHT
#if NETSTANDARD1_1 || NETSTANDARD1_3
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "pathToAssembly", Justification = "For API compatibility" )]
#endif // NETSTANDARD1_1 || NETSTANDARD1_3
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void AddRuntimeAssembly( string pathToAssembly )
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			EnsureDependentAssembliesListsInitialized();
			_runtimeAssemblies.Add( pathToAssembly );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
		}

#if NETSTANDARD1_1 || NETSTANDARD1_3
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "pathToAssembly", Justification = "For API compatibility" )]
#endif // NETSTANDARD1_1 || NETSTANDARD1_3
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void AddCompiledCodeDomAssembly( string pathToAssembly )
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			EnsureDependentAssembliesListsInitialized();
			_compiledCodeDomSerializerAssemblies.Add( pathToAssembly );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void ResetDependentAssemblies()
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			EnsureDependentAssembliesListsInitialized();

#if !NETFX_35
			File.AppendAllLines( GetHistoryFilePath(), _compiledCodeDomSerializerAssemblies );
#else
			File.AppendAllText( GetHistoryFilePath(), String.Join( Environment.NewLine, _compiledCodeDomSerializerAssemblies.ToArray() ) + Environment.NewLine );
#endif // !NETFX_35
			_compiledCodeDomSerializerAssemblies.Clear();
			ResetRuntimeAssemblies();
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
		}

		private static string _dumpDirectory;

		public static string DumpDirectory
		{
			get { return Interlocked.CompareExchange( ref _dumpDirectory, null, null ); }
			set { Interlocked.Exchange( ref _dumpDirectory, value ); }
		}

#if !NETSTANDARD1_1 && !NETSTANDARD1_3
		private static int _wasDeleted;
		private const string HistoryFile = "MsgPack.Serialization.SerializationGenerationDebugging.CodeDOM.History.txt";

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
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
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3

		[ThreadStatic]
		private static bool _onTheFlyCodeDomEnabled;

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static bool OnTheFlyCodeDomEnabled
		{
			get { return _onTheFlyCodeDomEnabled; }
			set { _onTheFlyCodeDomEnabled = value; }
		}

#if !NETSTANDARD1_1 && !NETSTANDARD1_3
		/// <summary>
		///		Creates the new type builder for the serializer.
		/// </summary>
		/// <param name="targetType">The serialization target type.</param>
		/// <returns></returns>
		/// <exception cref="System.InvalidOperationException">PrepareDump() was not called.</exception>
		[Obsolete( "", true )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static TypeBuilder NewTypeBuilder( Type targetType )
		{
			if ( _moduleBuilder == null )
			{
				throw new InvalidOperationException( "PrepareDump() was not called." );
			}

			return
				_moduleBuilder.DefineType( IdentifierUtility.EscapeTypeName( targetType ) + "SerializerLogics" );
		}

		/// <summary>
		///		Takes dump of instructions.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void Dump()
		{
#if !NETFX_35
			if ( _assemblyBuilder != null )
			{
				_assemblyBuilder.Save( _assemblyBuilder.GetName().Name + ".dll" );
			}
#endif // !NETFX_35
		}
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3

		/// <summary>
		///		Resets debugging states.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void Reset()
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			_assemblyBuilder = null;
			_moduleBuilder = null;
			_dumpEnabled = false;
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3

			if ( _ilTraceWriter != null )
			{
				_ilTraceWriter.Dispose();
				_ilTraceWriter = null;
			}

			_traceEnabled = false;
			ResetDependentAssemblies();
		}
#endif // !AOT && !SILVERLIGHT

#if NETFX_35 || UNITY || SILVERLIGHT
		private static int _useLegacyNullMapEntryHandling;
#else
		private static bool _useLegacyNullMapEntryHandling;
#endif // NETFX_35 || UNITY || SILVERLIGHT

		internal static bool UseLegacyNullMapEntryHandling
		{
			get
			{
#if NETFX_35 || UNITY || SILVERLIGHT
				return Volatile.Read( ref _useLegacyNullMapEntryHandling ) == 1;
#else
				return Volatile.Read( ref _useLegacyNullMapEntryHandling );
#endif
			}
			set
			{
#if NETFX_35 || UNITY || SILVERLIGHT
				Volatile.Write( ref _useLegacyNullMapEntryHandling, value ? 1 : 0 );
#else
				Volatile.Write( ref _useLegacyNullMapEntryHandling, value );
#endif
			}
		}


#if DEBUG && FEATURE_TAP

		private static bool _isNaiveAsyncAllowed;

		internal static bool IsNaiveAsyncAllowed
		{
			get { return Volatile.Read( ref _isNaiveAsyncAllowed ); }
			set { Volatile.Write( ref _isNaiveAsyncAllowed, value ); }
		}

		internal static void EnsureNaiveAsyncAllowed( object source, [CallerMemberName]string method = null )
		{
			if ( !Volatile.Read( ref _isNaiveAsyncAllowed ) )
			{
				throw new NotImplementedException( "This method is not implemented as generated method. " + source + "." + method );
			}
		}

#endif // DEBUG && FEATURE_TAP
	}
}