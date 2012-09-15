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

//    This file is copy of Message Pack for Java.

#endregion -- License Terms --

using System;
using System.IO;
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
	[TestFixture]
	[Timeout( 1000 )]
	public class MessageUnpackableTest
	{
		[Test]
		public void TestImage()
		{
			Image src = new Image();
			src.title = "msgpack";
			src.uri = "http://msgpack.org/";
			src.width = 2560;
			src.height = 1600;
			src.size = 4096000;

			var buffer = new MemoryStream();
			src.PackToMessage( Packer.Create( buffer ), null );

			Image dst = new Image();
			buffer.Seek( 0L, SeekOrigin.Begin );
			using ( var unpacker = Unpacker.Create( buffer ) )
			{
				Assert.That( unpacker.Read() );
				dst.UnpackFromMessage( unpacker );
			}

			Assert.AreEqual( src, dst );
		}
	}
}
