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
#if FEATURE_CONCURRENT
#endif // FEATURE_CONCURRENT
using System.Collections.Generic;
#if CORE_CLR || UNITY || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || UNITY || NETSTANDARD1_1
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

#if !AOT && !SILVERLIGHT
using MsgPack.Serialization.AbstractSerializers;
#if !NETSTANDARD1_1
using MsgPack.Serialization.CodeDomSerializers;
#endif // !NETSTANDARD1_1
#endif // !AOT && !SILVERLIGHT

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
#endif // !AOT && !SILVERLIGHT

		/// <summary>
		///		Traces the polymorphic schema event.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="memberInfo">The target of schema.</param>
		/// <param name="schema">The schema.</param>

		public static void TracePolimorphicSchemaEvent( string format, MemberInfo memberInfo, PolymorphismSchema schema )
		{
#if !AOT && !SILVERLIGHT
			if ( !_traceEnabled )
			{
				return;
			}

			Tracer.Emit.TraceEvent( Tracer.EventType.PolimorphicSchema, Tracer.EventId.PolimorphicSchema, format, memberInfo, schema == null ? "(null)" : schema.DebugString );
#endif // !AOT && !SILVERLIGHT
		}

#if !AOT && !SILVERLIGHT
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
#endif // !AOT && !SILVERLIGHT

#if !NETSTANDARD1_1 && !SILVERLIGHT && !AOT
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
#endif // !NETSTANDARD1_1 && !SILVERLIGHT && !AOT

#if !AOT && !SILVERLIGHT
#if FEATURE_CONCURRENT

		private static DependentAssemblyManager _dependentAssemblyManager = DependentAssemblyManager.Default;

		public static DependentAssemblyManager DependentAssemblyManager
		{
			get { return Volatile.Read( ref _dependentAssemblyManager ); }
			set { Volatile.Write( ref _dependentAssemblyManager, value ); }
		}

#else

		private static volatile DependentAssemblyManager _dependentAssemblyManager = DependentAssemblyManager.Default;

		public static DependentAssemblyManager DependentAssemblyManager
		{
			get { return _dependentAssemblyManager; }
			set { _dependentAssemblyManager = value; }
		}

#endif // FEATURE_CONCURRENT

		public static IEnumerable<string> CodeSerializerDependentAssemblies
		{
			get { return _dependentAssemblyManager.CodeSerializerDependentAssemblies; }
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void AddRuntimeAssembly( string pathToAssembly )
		{
			_dependentAssemblyManager.AddRuntimeAssembly( pathToAssembly );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "pathToAssembly", Justification = "For API compatibility" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void AddCompiledCodeDomAssembly( string pathToAssembly )
		{
			_dependentAssemblyManager.AddCompiledCodeDomAssembly( pathToAssembly );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void ResetDependentAssemblies()
		{
			_dependentAssemblyManager.ResetDependentAssemblies();
		}

		public static string DumpDirectory
		{
			get { return _dependentAssemblyManager.DumpDirectory; }
			set { _dependentAssemblyManager.DumpDirectory = value; }
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
		private static Func<Type, CollectionTraits, ISerializerBuilder> _onTheFlyCodeGenerationSerializerBuilderFactory;

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static bool OnTheFlyCodeGenerationEnabled
		{
			get { return _onTheFlyCodeGenerationSerializerBuilderFactory != null; }
		}

		public static ISerializerBuilder CreateOnTheFlyCodeGenerationSerializerBuilder( Type targetType, CollectionTraits collectionTraits, EmitterFlavor emitterFlavor )
		{
			var factory = _onTheFlyCodeGenerationSerializerBuilderFactory;

			if ( factory == null )
			{
				throw new NotSupportedException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Flavor '{0:G}'({0:D}) is not supported for serializer instance creation.",
						emitterFlavor
					)
				);
			}

			return factory( targetType, collectionTraits );
		}

		public static void SetOnTheFlyCodeGenerationBuilderFactory( Func<Type, CollectionTraits, ISerializerBuilder> factory )
		{
			_onTheFlyCodeGenerationSerializerBuilderFactory = factory;
		}

#if !NETSTANDARD1_1
		
		/// <summary>
		///		Takes dump of instructions.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void Dump()
		{
			if ( _assemblyBuilder != null )
			{
				_assemblyBuilder.Save( _assemblyBuilder.GetName().Name + ".dll" );
			}
		}

#endif // !NETSTANDARD1_1

		/// <summary>
		///		Resets debugging states.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		public static void Reset()
		{
#if !NETSTANDARD1_1
			_assemblyBuilder = null;
			_moduleBuilder = null;
			_dumpEnabled = false;
#endif // !NETSTANDARD1_1

			if ( _ilTraceWriter != null )
			{
				_ilTraceWriter.Dispose();
				_ilTraceWriter = null;
			}

			_traceEnabled = false;
			ResetDependentAssemblies();
		}
#endif // !AOT && !SILVERLIGHT

#if !FEATURE_CONCURRENT
		private static int _useLegacyNullMapEntryHandling;
#else
		private static bool _useLegacyNullMapEntryHandling;
#endif // !FEATURE_CONCURRENT

		internal static bool UseLegacyNullMapEntryHandling
		{
			get
			{
#if !FEATURE_CONCURRENT
				return Volatile.Read( ref _useLegacyNullMapEntryHandling ) == 1;
#else
				return Volatile.Read( ref _useLegacyNullMapEntryHandling );
#endif // !FEATURE_CONCURRENT
			}
			set
			{
#if !FEATURE_CONCURRENT
				Volatile.Write( ref _useLegacyNullMapEntryHandling, value ? 1 : 0 );
#else
				Volatile.Write( ref _useLegacyNullMapEntryHandling, value );
#endif // !FEATURE_CONCURRENT
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