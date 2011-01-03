#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using NUnit.Framework;
using System.IO;

namespace MsgPack.Linq
{
	[TestFixture]
	public sealed class StreamEnumerableTest
	{
		[Test]
		public void TestRead()
		{
			using ( var stream = new MemoryStream( new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 } ) )
			{
				Assert.IsTrue( Enumerable.Range( 1, 8 ).Select( i => ( byte )i ).SequenceEqual( stream.AsEnumerable() ) );
				Assert.AreEqual( 0, stream.AsEnumerable().Count() );
			}
		}
	}
}
