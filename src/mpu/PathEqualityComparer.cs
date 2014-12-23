#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
using System.Globalization;
using System.IO;

namespace mpu
{
	internal abstract class PathEqualityComparer : IEqualityComparer<string>
	{
		private static readonly PathEqualityComparer _instance = DetermineInstance();

		private static PathEqualityComparer DetermineInstance()
		{
			switch ( Environment.OSVersion.Platform )
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
				case PlatformID.Xbox:
				{
					return OrdinalPathEqualityComparer.CaseInsensitive;
				}
				case PlatformID.MacOSX: // Mac's NFD problem may not affect in assembly probling...
				case PlatformID.Unix:
				{
					return OrdinalPathEqualityComparer.CaseSensitive;
				}
				default:
				{
					throw new PlatformNotSupportedException(String.Format( CultureInfo.CurrentCulture, "Unknown platform '{0:G}({0:D})'.", Environment.OSVersion.Platform ));
				}
			}
		}

		public static PathEqualityComparer Instance
		{
			get { return _instance; }
		}


		public bool Equals( string x, string y )
		{
			return EqualsCore( SafeGetFullPath( x ), SafeGetFullPath( y ) );
		}

		protected abstract bool EqualsCore( string x, string y );

		public int GetHashCode( string obj )
		{
			return SafeGetFullPath( obj ).GetHashCode();
		}

		private static string SafeGetFullPath( string mayBePath )
		{
			if ( String.IsNullOrEmpty( mayBePath ) )
			{
				return String.Empty;
			}

			return Path.GetFullPath( mayBePath.Trim() ).Replace( Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar );
		}

		private sealed class OrdinalPathEqualityComparer : PathEqualityComparer
		{
			public static readonly OrdinalPathEqualityComparer CaseSensitive =
				new OrdinalPathEqualityComparer( StringComparer.Ordinal );

			public static readonly OrdinalPathEqualityComparer CaseInsensitive =
				new OrdinalPathEqualityComparer( StringComparer.OrdinalIgnoreCase );

			private readonly IEqualityComparer<string> _pathComparer;

			private OrdinalPathEqualityComparer( IEqualityComparer<string> pathComparer )
			{
				this._pathComparer = pathComparer;
			}

			protected override bool EqualsCore( string x, string y )
			{
				return this._pathComparer.Equals( x, y );
			}
		}
	}
}
