// PR #162
using System;
using System.Collections.Generic;
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

namespace MsgPack.Serialization
{
	[TestFixture]
	public class InheritanceTest
	{
		[Test]
		public void Issue147_161_162_HidingMembersWillNotBeDuplicated()
		{
			var context = new SerializationContext();
			context.SerializationMethod = SerializationMethod.Array;
			var serializer = context.GetSerializer<Child>();
			using ( var buffer = new MemoryStream() )
			{
				var obj = new Child { Field = new List<string> { "A", "B" }, Property = new List<string> { "C", "D" } };
				serializer.Pack( buffer, obj );
				
				buffer.Position = 0;

				using ( var unpacker = Unpacker.Create( buffer, ownsStream: false ) )
				{
					Assert.That( unpacker.Read() );
					// If the hiding is not respected, this value will be 4.
					Assert.That( unpacker.LastReadData.AsInt32(), Is.EqualTo( 2 ) );
				}
			}
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