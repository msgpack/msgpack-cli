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
#endregion -- License Terms --

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif
using System.IO;

namespace MsgPack.Serialization
{
	[TestFixture]
	public partial class PackerTest
	{
		[Test]
		public void TestCreate_OwnsStreamIsTrue_StreamIsClosed()
		{
			using ( var stream = new MemoryStream() )
			{
				using ( var packer = Packer.Create( stream, true ) ) { }

				try
				{
					stream.ReadByte();
					Assert.Fail();
				}
				catch ( ObjectDisposedException ) { }
			}
		}

		[Test]
		public void TestCreate_OwnsStreamIsFalse_StreamIsNotClosed()
		{
			using ( var stream = new MemoryStream() )
			{
				using ( var packer = Packer.Create( stream, false ) ) { }

				Assert.That( stream.ReadByte(), Is.EqualTo( -1 ) );
			}
		}

		[Test]
		public void TestCreate_StreamIsNull()
		{
			using ( var stream = new MemoryStream() )
			{
				Assert.Throws<ArgumentNullException>( () => { using ( var packer = Packer.Create( null, true ) ) { } } );
			}
		}
	}
}
