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
using System.IO;
#if NET35
using System.Linq;
#endif // NET35
using System.Reflection;
using System.Threading;

namespace MsgPack.Serialization
{
	internal sealed class TempFileDependentAssemblyManager : DependentAssemblyManager
	{
		private static int _wasDeleted;
		private const string HistoryFile = "MsgPack.Serialization.SerializationGenerationDebugging.CodeDOM.History.txt";

		private readonly string _baseDirectory;

		public TempFileDependentAssemblyManager( string baseDirectory )
		{
			this._baseDirectory = baseDirectory;
			this.ResetDependentAssemblies();
		}

		protected override void Record( IEnumerable<string> assemblies )
		{
#if !NET35
			File.AppendAllLines( GetHistoryFilePath(), assemblies );
#else
			File.AppendAllText( GetHistoryFilePath(), String.Join( Environment.NewLine, assemblies.ToArray() ) + Environment.NewLine );
#endif // !NET35
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For unit testing" )]
		private static string GetHistoryFilePath()
		{
			return Path.Combine( Path.GetTempPath(), HistoryFile );
		}

		public override void DeletePastTemporaries()
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

		public override Assembly LoadAssembly( string path )
		{
			return Assembly.LoadFrom( path );
		}

		protected override IEnumerable<string> GetRuntimeAssemblies()
		{
#if NETSTANDARD1_3
			if ( this._baseDirectory == null )
			{
				yield break;
			}

			// TODO: X-Plat
			var referenceAssemblyDirectory = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ProgramFilesX86 ), @"Reference Assemblies\Microsoft\Framework\.NETCore\v4.5.1" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Runtime.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Collections.dll" );
			yield return Path.Combine( this._baseDirectory, "System.Collections.NonGeneric.dll" );
			yield return Path.Combine( this._baseDirectory, "System.Collections.Specialized.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Diagnostics.Debug.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Diagnostics.Tools.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Globalization.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Linq.dll" );
			yield return Path.Combine( this._baseDirectory, "System.Numerics.Vectors.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Reflection.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Reflection.Extensions.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Reflection.Primitives.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Runtime.Extensions.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Runtime.Numerics.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Runtime.Serialization.Primitives.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Threading.dll" );
			yield return Path.Combine( referenceAssemblyDirectory, "System.Threading.Tasks.dll" );
			yield return Path.Combine( this._baseDirectory, "MsgPack.Core.dll" );
			yield return Path.Combine( this._baseDirectory, "MsgPack.Serialization.dll" );
#else
			yield return typeof( object ).Assembly.Location; // System.dll
			yield return typeof( Stack<> ).Assembly.Location; // System.dll
#if NET35
			yield return typeof( Enumerable ).Assembly.Location; // System.Core.dll
#else
			yield return typeof( Action<,,,,,,,,,,> ).Assembly.Location; // System.Core.dll
			yield return typeof( System.Numerics.BigInteger ).Assembly.Location; // System.Numerics.dll
#endif // NET35
			yield return typeof( MessagePackObject ).Assembly.Location;
			yield return typeof( SerializationContext ).Assembly.Location;
#endif // NETSTANDARD1_1
		}
	}
}