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
using NUnit.Framework;

namespace MsgPack.Serialization
{
	[TestFixture]
	public class MapFieldBasedAutoMessagePackSerializerTest : AutoMessagePackSerializerTest
	{
		private static readonly SerializationContext _defaultContext = CreateContext();

		private static SerializationContext CreateContext()
		{
			return new SerializationContext() { SerializationMethod = SerializationMethod.Map, EmitterFlavor = EmitterFlavor.FieldBased };
		}

		protected override SerializationContext GetSerializationContext()
		{
			return ReuseContext ? _defaultContext : CreateContext();
		}

		protected override MessagePackSerializer<T> CreateTarget<T>( SerializationContext context )
		{
			return new AutoMessagePackSerializer<T>( context, c => new MapEmittingSerializerBuilder<T>( c ) );
		}
	}

	[TestFixture]
	public class ArrayFieldBasedAutoMessagePackSerializerTest : AutoMessagePackSerializerTest
	{
		private static readonly SerializationContext _defaultContext = CreateContext();

		private static SerializationContext CreateContext()
		{
			return new SerializationContext() { SerializationMethod = SerializationMethod.Array, EmitterFlavor = EmitterFlavor.FieldBased };
		}

		protected override SerializationContext GetSerializationContext()
		{
			return ReuseContext ? _defaultContext : CreateContext();
		}
		protected override MessagePackSerializer<T> CreateTarget<T>( SerializationContext context )
		{
			return new AutoMessagePackSerializer<T>( context, c => new ArrayEmittingSerializerBuilder<T>( c ) );
		}
	}

	[TestFixture]
	public class MapContextBasedAutoMessagePackSerializerTest : AutoMessagePackSerializerTest
	{
		private static readonly SerializationContext _defaultContext = CreateContext();

		private static SerializationContext CreateContext()
		{
			return new SerializationContext() { SerializationMethod = SerializationMethod.Map, EmitterFlavor = EmitterFlavor.ContextBased };
		}

		protected override SerializationContext GetSerializationContext()
		{
			return ReuseContext ? _defaultContext : CreateContext();
		}

		protected override MessagePackSerializer<T> CreateTarget<T>( SerializationContext context )
		{
			return new AutoMessagePackSerializer<T>( context, c => new MapEmittingSerializerBuilder<T>( c ) );
		}
	}

	[TestFixture]
	public class ArrayContextBasedAutoMessagePackSerializerTest : AutoMessagePackSerializerTest
	{
		private static readonly SerializationContext _defaultContext = CreateContext();

		private static SerializationContext CreateContext()
		{
			return new SerializationContext() { SerializationMethod = SerializationMethod.Array, EmitterFlavor = EmitterFlavor.ContextBased };
		}

		protected override SerializationContext GetSerializationContext()
		{
			return ReuseContext ? _defaultContext : CreateContext();
		}

		protected override MessagePackSerializer<T> CreateTarget<T>( SerializationContext context )
		{
			return new AutoMessagePackSerializer<T>( context, c => new ArrayEmittingSerializerBuilder<T>( c ) );
		}
	}
}
