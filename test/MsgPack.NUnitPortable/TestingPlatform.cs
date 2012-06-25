using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MsgPack.NUnitPortable
{
	public abstract class TestingPlatform
	{
		private static readonly object _syncRoot = new object();

		private static TestingPlatform _current;

		public static TestingPlatform Current
		{
			get
			{
				lock ( _syncRoot )
				{
					return _current;
				}
			}
			set
			{
				lock ( _syncRoot )
				{
					_current = value;
				}
			}
		}

		public abstract void Success( string message );
		public abstract void Fail( string message );
		public abstract void Inconclusive( string message );
		public abstract void Ignore( string message );

		internal static TestingPlatform EnsureInitialized()
		{
			var platform = Current;
			if ( platform == null )
			{
				throw new InvalidOperationException( "TestingPlatform.Current must be set via assembly level initialization ([AssemblyInitialize], [SetUpFixture], or so)." );
			}

			return platform;
		}
	}
}
