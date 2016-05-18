using MsgPack.Serialization;
using NUnit.Framework;
using System.Collections.Generic;

namespace MsgPack
{
	[TestFixture]
	public class InheritanceTest
	{
		[Test]
		public void SerializatorCanBeCreated()
		{
			var serializer = MessagePackSerializer.Get<Child>();
		}

		public abstract class Base
		{
			public List<int> Field;
			public virtual List<int> Property { get; set; }
		}

		public class Child : Base
		{
			public new List<string> Field;
			public new virtual List<string> Property { get; set; }
		}
	}
}