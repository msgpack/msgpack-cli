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

using System;
using System.Collections.Generic;
using System.Reflection;
#if FEATURE_CONCURRENT
using System.Collections.Concurrent;
using System.Threading;
#else
using System.Linq;
#endif // FEATURE_CONCURRENT

namespace MsgPack.Serialization
{
	internal abstract class DependentAssemblyManager
	{
#if FEATURE_CONCURRENT

		private static DependentAssemblyManager _default = new NullDependentAssemblyManager();

		public static DependentAssemblyManager Default
		{
			get { return Volatile.Read( ref _default ); }
			set { Volatile.Write( ref _default, value ); }
		}

		private readonly ConcurrentDictionary<string, object> _runtimeAssemblies;

		private readonly ConcurrentDictionary<string, object> _compiledCodeDomSerializerAssemblies;

#else

		private static volatile DependentAssemblyManager _default = new NullDependentAssemblyManager();

		public static DependentAssemblyManager Default
		{
			get { return _default; }
			set { _default = value; }
		}

		private readonly object _syncRoot;

		private readonly Dictionary<string, object> _runtimeAssemblies;

		private readonly Dictionary<string, object> _compiledCodeDomSerializerAssemblies;

#endif // FEATURE_CONCURRENT

		public IEnumerable<string> CodeSerializerDependentAssemblies
		{
			get
			{
				// ReSharper disable JoinDeclarationAndInitializer
				ICollection<string> runtimeAssemblies;
				ICollection<string> compiledAssemblies;
				// ReSharper restore JoinDeclarationAndInitializer
#if !FEATURE_CONCURRENT
				lock ( this._syncRoot )
				{
					runtimeAssemblies = this._runtimeAssemblies.Keys.ToArray();
					compiledAssemblies = this._compiledCodeDomSerializerAssemblies.Keys.ToArray();
				}
#else
				runtimeAssemblies = this._runtimeAssemblies.Keys;
				compiledAssemblies = this._compiledCodeDomSerializerAssemblies.Keys;
#endif // !FEATURE_CONCURRENT

				// FCL dependencies and msgpack core libs
				foreach ( var runtimeAssembly in runtimeAssemblies )
				{
					yield return runtimeAssembly;
				}

				// dependents
				foreach ( var compiledAssembly in compiledAssemblies )
				{
					yield return compiledAssembly;
				}
			}
		}

#if FEATURE_CONCURRENT

		private string _dumpDirectory;

		public string DumpDirectory
		{
			get { return Volatile.Read( ref this._dumpDirectory ); }
			set { Volatile.Write( ref this._dumpDirectory, value ); }
		}

#else

		private volatile string _dumpDirectory;

		public string DumpDirectory
		{
			get { return this._dumpDirectory; }
			set { this._dumpDirectory = value; }
		}

#endif // FEATURE_CONCURRENT

		protected DependentAssemblyManager()
		{
#if FEATURE_CONCURRENT
			this._runtimeAssemblies = new ConcurrentDictionary<string, object>( StringComparer.OrdinalIgnoreCase );
#else
			this._runtimeAssemblies = new Dictionary<string, object>( StringComparer.OrdinalIgnoreCase );
#endif // FEATURE_CONCURRENT

			this.ResetRuntimeAssemblies();

#if FEATURE_CONCURRENT
			this._compiledCodeDomSerializerAssemblies = new ConcurrentDictionary<string, object>( StringComparer.OrdinalIgnoreCase );
#else
			this._compiledCodeDomSerializerAssemblies = new Dictionary<string, object>( StringComparer.OrdinalIgnoreCase );
			this._syncRoot = new object();
#endif // FEATURE_CONCURRENT
		}

		private void ResetRuntimeAssemblies()
		{
#if !FEATURE_CONCURRENT
			lock ( this._syncRoot )
			{
#endif // !FEATURE_CONCURRENT

#if NETSTANDARD1_1
			this._runtimeAssemblies[ typeof( object ).GetAssembly().ManifestModule.FullyQualifiedName ] = null; // System.Runtime
			this._runtimeAssemblies[ typeof( Math ).GetAssembly().ManifestModule.FullyQualifiedName ] = null; // System.Runtime
			this._runtimeAssemblies[ typeof( System.Linq.Enumerable ).GetAssembly().ManifestModule.FullyQualifiedName ] = null; // System.Linq.dll
			this._runtimeAssemblies[ typeof( System.Globalization.CultureInfo ).GetAssembly().ManifestModule.FullyQualifiedName ] = null; // System.Globalization.dll
			this._runtimeAssemblies[ typeof( IEnumerable<> ).GetAssembly().ManifestModule.FullyQualifiedName ] = null; // System.Collections.dll
			this._runtimeAssemblies[ typeof( MessagePackObject ).GetAssembly().ManifestModule.FullyQualifiedName ] = null; // MsgPack.Core.dll
			this._runtimeAssemblies[ typeof( SerializationContext ).GetAssembly().ManifestModule.FullyQualifiedName ] = null; // MsgPack.Serialization.dll
			// They should be registered with extensions or test assembly:
			// System.Runtime.Numerics
			// System.Collections.NonGeneric
			// System.Collections.Specialized
			// System.Numeric.Vectors
#else
			this._runtimeAssemblies[ "System.dll" ] = null; // GAC
#if NETFX_35
			this._runtimeAssemblies[ typeof( Enumerable ).Assembly.Location ] = null;
#else
			this._runtimeAssemblies[ "System.Core.dll" ] = null; // GAC
			this._runtimeAssemblies[ "System.Numerics.dll" ] = null; // GAC
#endif // NETFX_35
			this._runtimeAssemblies[ typeof( SerializerDebugging ).Assembly.Location ] = null;
#endif // NETSTANDARD1_1
			this._runtimeAssemblies[ typeof( SerializerDebugging ).GetAssembly().ManifestModule.FullyQualifiedName ] = null;

#if !FEATURE_CONCURRENT
			}
#endif // !FEATURE_CONCURRENT
		}

		public void AddRuntimeAssembly( string pathToAssembly )
		{
#if !FEATURE_CONCURRENT
			lock ( this._syncRoot )
			{
#endif // !FEATURE_CONCURRENT
			this._runtimeAssemblies[ pathToAssembly ] = null;
#if !FEATURE_CONCURRENT
			}
#endif // !FEATURE_CONCURRENT
		}

		public void AddCompiledCodeDomAssembly( string pathToAssembly )
		{
#if !FEATURE_CONCURRENT
			lock ( this._syncRoot )
			{
#endif // !FEATURE_CONCURRENT
			this._compiledCodeDomSerializerAssemblies[ pathToAssembly ] = null;
#if !FEATURE_CONCURRENT
			}
#endif // !FEATURE_CONCURRENT
		}

		public void ResetDependentAssemblies()
		{
#if !FEATURE_CONCURRENT
			lock ( this._syncRoot )
			{
#endif // !FEATURE_CONCURRENT
			this.Record( this._compiledCodeDomSerializerAssemblies.Keys );
			this._compiledCodeDomSerializerAssemblies.Clear();
			this.ResetRuntimeAssemblies();
#if !FEATURE_CONCURRENT
			}
#endif // !FEATURE_CONCURRENT
		}

		protected abstract void Record( IEnumerable<string> assemblies );

		public abstract void DeletePastTemporaries();

		public virtual Assembly LoadAssembly( string path )
		{
			throw new NotSupportedException();
		}

		private sealed class NullDependentAssemblyManager : DependentAssemblyManager
		{
			public NullDependentAssemblyManager() : base() { }

			protected override void Record( IEnumerable<string> assemblies ) { }

			public override void DeletePastTemporaries() { }
		}
	}
}