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

#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if FEATURE_CONCURRENT
using System.Collections.Concurrent;
using System.Threading;
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

		private readonly ConcurrentDictionary<string, byte[]> _runtimeAssemblies;

		private readonly ConcurrentDictionary<string, byte[]> _compiledCodeDomSerializerAssemblies;

#else

		private static volatile DependentAssemblyManager _default = new NullDependentAssemblyManager();

		public static DependentAssemblyManager Default
		{
			get { return _default; }
			set { _default = value; }
		}

		private readonly object _syncRoot;

		private readonly Dictionary<string, byte[]> _runtimeAssemblies;

		private readonly Dictionary<string, byte[]> _compiledCodeDomSerializerAssemblies;

#endif // FEATURE_CONCURRENT

		public IEnumerable<object> CodeSerializerDependentAssemblies
		{
			get
			{
				// ReSharper disable JoinDeclarationAndInitializer
				IEnumerable<object> runtimeAssemblies;
				IEnumerable<object> compiledAssemblies;
				// ReSharper restore JoinDeclarationAndInitializer
#if !FEATURE_CONCURRENT
				lock ( this._syncRoot )
				{
					runtimeAssemblies = this._runtimeAssemblies.Keys.ToArray();
					compiledAssemblies = this._compiledCodeDomSerializerAssemblies.Select( kv => kv.Value as object ?? kv.Key ).ToArray();
				}
#else
				runtimeAssemblies = this._runtimeAssemblies.Keys;
				compiledAssemblies = this._compiledCodeDomSerializerAssemblies.Select( kv => kv.Value as object ?? kv.Key );
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "It is by design for internal utilities." )]
		protected DependentAssemblyManager()
		{
#if FEATURE_CONCURRENT
			this._runtimeAssemblies = new ConcurrentDictionary<string, byte[]>( StringComparer.OrdinalIgnoreCase );
#else
			this._syncRoot = new object();
			this._runtimeAssemblies = new Dictionary<string, byte[]>( StringComparer.OrdinalIgnoreCase );
#endif // FEATURE_CONCURRENT

			this.ResetRuntimeAssemblies();

#if FEATURE_CONCURRENT
			this._compiledCodeDomSerializerAssemblies = new ConcurrentDictionary<string, byte[]>( StringComparer.OrdinalIgnoreCase );
#else
			this._compiledCodeDomSerializerAssemblies = new Dictionary<string, byte[]>( StringComparer.OrdinalIgnoreCase );
#endif // FEATURE_CONCURRENT
		}

		protected abstract IEnumerable<string> GetRuntimeAssemblies();

		private void ResetRuntimeAssemblies()
		{
			var assemblies = this.GetRuntimeAssemblies();
#if !FEATURE_CONCURRENT
			lock ( this._syncRoot )
			{
#endif // !FEATURE_CONCURRENT
			this._runtimeAssemblies.Clear();
			foreach ( var assembly in assemblies )
			{
				this._runtimeAssemblies[ assembly ] = null;
			}
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

		public void AddCompiledCodeAssembly( string pathToAssembly )
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

		public void AddCompiledCodeAssembly( string name, byte[] image )
		{
#if !FEATURE_CONCURRENT
			lock ( this._syncRoot )
			{
#endif // !FEATURE_CONCURRENT
			this._compiledCodeDomSerializerAssemblies[ name ] = image;
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
			this.Record( this._compiledCodeDomSerializerAssemblies.Where( kv => kv.Value == null ).Select( kv => kv.Key  ) );
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

			protected override IEnumerable<string> GetRuntimeAssemblies()
			{
				yield break;
			}
		}
	}
}
#endif // DEBUG
