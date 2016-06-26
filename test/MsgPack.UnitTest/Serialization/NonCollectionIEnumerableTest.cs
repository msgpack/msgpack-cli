using System;
using System.Collections;
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
	public sealed class NonCollectionIEnumerableTest
	{
		[Test]
		public void ShouldSerializeATypeImplementingIEnumerableWithoutAnAddMethodAsANormalType()
		{
			var context = new SerializationContext();
			context.SerializationMethod = SerializationMethod.Array;
			var serializer = context.GetSerializer<NonCollectionType>();
			using ( var buffer = new MemoryStream() )
			{
				var obj = new NonCollectionType { Property = 123 };
				serializer.Pack(buffer, obj);

				buffer.Position = 0;

				var clone = serializer.Unpack( buffer );
				Assert.That( clone.Property, Is.EqualTo( 123 ) );
			}
		}

		[Serializable]
		public class NonCollectionType : IEnumerable<int>
		{
			public int Property { get; set; }

			public IEnumerator<int> GetEnumerator()
			{
				yield break;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
	}
}
