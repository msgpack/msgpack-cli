#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2018 FUJIWARA, Yusuke and contributors
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
// Contributors:
//    Samuel Cragg
//
#endregion -- License Terms --

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if FEATURE_CONCURRENT
using System.Collections.Concurrent;
#endif // FEATURE_CONCURRENT
using System.Collections.Generic;
using System.Diagnostics;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Holds debugging support information.
	/// </summary>
#if UNITY && DEBUG
	public
#else
	internal
#endif
	static class SerializerDebugging
	{
#if !UNITY
#if DEBUG
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
#endif // DEBUG
#endif // !UNITY

#if DEBUG
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
#endif // DEBUG

#if FEATURE_EMIT
#if DEBUG
		[ThreadStatic]
		private static StringWriter _ilTraceWriter;
#endif // DEBUG

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
#if DEBUG
				if ( !_traceEnabled )
				{
					return NullTextWriter.Instance;
				}

				if ( _ilTraceWriter == null )
				{
					_ilTraceWriter = new StringWriter( CultureInfo.InvariantCulture );
				}

				return _ilTraceWriter;
#else
				return NullTextWriter.Instance;
#endif // DEBUG
			}
		}

#if DEBUG
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
#endif // DEBUG
#endif // FEATURE_EMIT

		/// <summary>
		///		Traces the polymorphic schema event.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="memberInfo">The target of schema.</param>
		/// <param name="schema">The schema.</param>
		[Conditional( "DEBUG" )]
		public static void TracePolimorphicSchemaEvent( string format, MemberInfo memberInfo, PolymorphismSchema schema )
		{
#if DEBUG
#if FEATURE_EMIT
			if ( !_traceEnabled )
			{
				return;
			}

			Tracer.Emit.TraceEvent( Tracer.EventType.PolimorphicSchema, Tracer.EventId.PolimorphicSchema, format, memberInfo, schema == null ? "(null)" : schema.DebugString );
#endif // FEATURE_EMIT
#endif
		}

#if DEBUG

#if FEATURE_EMIT
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

#if !NETSTANDARD1_3
#if !NETSTANDARD2_0
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
#endif // !NETSTANDARD2_0

#if !FERATURE_CONCURRENT
		private static volatile DependentAssemblyManager _dependentAssemblyManager = DependentAssemblyManager.Default;

		public static DependentAssemblyManager DependentAssemblyManager
		{
			get { return _dependentAssemblyManager; }
			set { _dependentAssemblyManager = value; }
		}
#else
		private static DependentAssemblyManager _dependentAssemblyManager = DependentAssemblyManager.Default;

		public static DependentAssemblyManager DependentAssemblyManager
		{
			get { return Volatile.Read( ref _dependentAssemblyManager ); }
			set { Volatile.Write( ref _dependentAssemblyManager, value ); }
		}
#endif // FERATURE_CONCURRENT

		public static IEnumerable<object> CodeSerializerDependentAssemblies
		{
			get { return _dependentAssemblyManager.CodeSerializerDependentAssemblies; }
		}

		public static void AddRuntimeAssembly( string pathToAssembly )
		{
			_dependentAssemblyManager.AddRuntimeAssembly( pathToAssembly );
		}

		public static void AddCompiledCodeAssembly( string pathToAssembly )
		{
			_dependentAssemblyManager.AddCompiledCodeAssembly( pathToAssembly );
		}

		public static void AddCompiledCodeAssembly( string name, byte[] image )
		{
			_dependentAssemblyManager.AddCompiledCodeAssembly( name, image );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void ResetDependentAssemblies()
		{
			_dependentAssemblyManager.ResetDependentAssemblies();
		}

		public static string DumpDirectory
		{
			get { return _dependentAssemblyManager.DumpDirectory; }
#if DEBUG
			set { _dependentAssemblyManager.DumpDirectory = value; }
#endif // DEBUG
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void DeletePastTemporaries()
		{
			_dependentAssemblyManager.DeletePastTemporaries();
		}

		public static Assembly LoadAssembly( string path )
		{
			return _dependentAssemblyManager.LoadAssembly( path );
		}

		[ThreadStatic]
		private static bool _onTheFlyCodeDomEnabled;

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static bool OnTheFlyCodeGenerationEnabled
		{
			get { return _onTheFlyCodeDomEnabled; }
			set { _onTheFlyCodeDomEnabled = value; }
		}

#if !NETSTANDARD2_0
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
#if !NET35
			if ( _assemblyBuilder != null )
			{
				_assemblyBuilder.Save( _assemblyBuilder.GetName().Name + ".dll" );
			}
#endif // !NET35
		}
#endif // !NETSTANDARD2_0

		/// <summary>
		///		Resets debugging states.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void Reset()
		{
#if !NETSTANDARD2_0
			_assemblyBuilder = null;
			_moduleBuilder = null;
#endif // !NETSTANDARD2_0
			_dumpEnabled = false;

			if ( _ilTraceWriter != null )
			{
				_ilTraceWriter.Dispose();
				_ilTraceWriter = null;
			}

			_traceEnabled = false;
			_codeWriter = null;
			ResetDependentAssemblies();
		}
#endif // !NETSTANDARD1_3
#endif // FEATURE_EMIT

#if NET35 || UNITY || SILVERLIGHT
		private static int _useLegacyNullMapEntryHandling;
#else
		private static bool _useLegacyNullMapEntryHandling;
#endif // NET35 || UNITY || SILVERLIGHT

		public static bool UseLegacyNullMapEntryHandling
		{
			get
			{
#if NET35 || UNITY || SILVERLIGHT
				return Volatile.Read( ref _useLegacyNullMapEntryHandling ) == 1;
#else
				return Volatile.Read( ref _useLegacyNullMapEntryHandling );
#endif
			}
			set
			{
#if NET35 || UNITY || SILVERLIGHT
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

#if !NETSTANDARD1_1 && !NETSTANDARD1_3
		[ThreadStatic]
		private static StringWriter _codeWriter;

		public static StringWriter CodeWriter
		{
			get
			{
				if ( _codeWriter == null )
				{
					_codeWriter = new StringWriter( CultureInfo.InvariantCulture );
				}

				return _codeWriter;
			}
		}

		public static void CompileAssembly( bool isDebug, out Assembly compiledAssembly, out IList<string> errors, out IList<string> warnings )
		{
			_codeCompiler( CodeWriter.ToString(), isDebug, out compiledAssembly, out errors, out warnings );
		}

		public static void ClearCodeBuffer()
		{
			// Clears buffer and enable reopen.
			_codeWriter = null;
		}

#if FEATURE_CONCURRENT
		private static CodeCompiler _codeCompiler;
#else
		private static volatile CodeCompiler _codeCompiler;
#endif

		public static void SetCodeCompiler( CodeCompiler codeCompiler )
		{
#if FEATURE_CONCURRENT
			Volatile.Write( ref _codeCompiler, codeCompiler );
#else
			_codeCompiler = codeCompiler;
#endif
		}

		public delegate void CodeCompiler( string code, bool isDebug, out Assembly compiledAssembly, out IList<string> errors, out IList<string> warnings );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
#endif // DEBUG
	}
}
