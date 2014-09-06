#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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

//    This file is copy of MessagePack for Java.

#endregion -- License Terms --

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack
{
#if !NETFX_CORE && !WINDOWS_PHONE
	[TestFixture]
	public class TestSuite
	{
		private static void FeedFile( Stream stream, String path )
		{
			long original = stream.Position;
			using ( var input = new FileStream( path, FileMode.Open ) )
			{
				byte[] buffer = new byte[ 64 * 1024 ];
				while ( true )
				{
					int count = input.Read( buffer, 0, buffer.Length );
					if ( count <= 0 )
					{
						break;
					}
					stream.Write( buffer, 0, count );
				}
			}

			stream.Position = original;
		}

		[Test]
		public void Run()
		{
			using ( var pacStream = new MemoryStream() )
			using ( var pacCompactStream = new MemoryStream() )
			{
				Unpacker pac = Unpacker.Create( pacStream );
				Unpacker pac_compact = Unpacker.Create( pacCompactStream );

				FeedFile( pacStream, "." + Path.DirectorySeparatorChar + "cases.mpac" );
				FeedFile( pacCompactStream, "." + Path.DirectorySeparatorChar + "cases_compact.mpac" );

				pac.SequenceEqual( pac_compact, EqualityComparer<MessagePackObject>.Default );
			}
		}
	}
#endif
}
