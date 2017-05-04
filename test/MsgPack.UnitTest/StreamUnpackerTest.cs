#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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
	public abstract partial class StreamUnpackerTest : UnpackerTest
	{
		protected sealed override Unpacker CreateUnpacker( MemoryStream stream )
		{
			return this.CreateUnpacker( stream as Stream );
		}

		protected abstract Unpacker CreateUnpacker( Stream stream );

		[Test]
		public void TestCreate_StreamIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => { using ( this.CreateUnpacker( null ) ) { } } );
		}

		[Test]
		public void TestCreate_OwnsStreamisFalse_NotDisposeStream()
		{
			using ( var stream = new MemoryStream() )
			{
				using ( Unpacker.Create( stream, false ) ) { }

				// Should not throw ObjectDisposedException.
				stream.WriteByte( 1 );
			}
		}
	}
}
