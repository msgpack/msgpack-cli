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
using System.Reflection;
using System.Threading;

namespace MsgPack.Serialization
{
	internal sealed class TempFileDependentAssemblyManager : DependentAssemblyManager
	{
		private static int _wasDeleted;
		private const string HistoryFile = "MsgPack.Serialization.SerializationGenerationDebugging.CodeDOM.History.txt";

		protected override void Record( IEnumerable<string> assemblies )
		{
#if !NETFX_35 && !UNITY
			File.AppendAllLines( GetHistoryFilePath(), assemblies );
#else
			File.AppendAllText( GetHistoryFilePath(), String.Join( Environment.NewLine, assemblies.ToArray() ) + Environment.NewLine );
#endif // !NETFX_35 && !UNITY
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
	}
}