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
#if !NETFX_CORE
using MsgPack.Serialization.EmittingSerializers;
#endif
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
#if !NETFX_CORE
	[TestFixture]
	public class MapContextBasedAutoMessagePackSerializerTest : AutoMessagePackSerializerTest
	{
		protected override SerializationContext GetSerializationContext()
		{
			return new SerializationContext() { SerializationMethod = SerializationMethod.Map, EmitterFlavor = EmitterFlavor.ContextBased };
		}

		protected override MessagePackSerializer<T> CreateTarget<T>( SerializationContext context )
		{
			return new AutoMessagePackSerializer<T>( context, c => new MapEmittingSerializerBuilder<T>( c ) );
		}
	}
#endif
}