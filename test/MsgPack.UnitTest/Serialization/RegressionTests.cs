#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015 FUJIWARA, Yusuke
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

using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using MsgPack.Serialization.AbstractSerializers;
#if !NETFX_CORE && !WINDOWS_PHONE
using MsgPack.Serialization.CodeDomSerializers;
using MsgPack.Serialization.EmittingSerializers;
#endif // if !NETFX_CORE && !WINDOWS_PHONE
#if !NETFX_35
using MsgPack.Serialization.ExpressionSerializers;
#endif // if !NETFX_35
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack.Serialization
{
	[TestFixture]
	public class RegressionTests
	{
		[Test]
		public void TestIssue73()
		{
			var value =
				new Dictionary<string,object> { { "1", new object() }, { "2", new object() } };
			var serializer = MessagePackSerializer.Get<Dictionary<string, object>>( new SerializationContext() );
			using ( var buffer = new MemoryStream() )
			{
				Assert.Throws<SerializationException>( () => serializer.Pack( buffer, value ) );
			}
		}
	}
}
