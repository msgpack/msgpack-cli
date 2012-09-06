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
using System.Globalization;
using System.IO;
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

namespace MsgPack.Serialization
{
	[TestFixture]
	public class FromExpressionTest
	{
		[Test]
		public void TestToProperty_Static()
		{
			FromExpression.ToProperty( () => DateTime.Now );
		}

		[Test]
		public void TestToProperty_Instance()
		{
			FromExpression.ToProperty( ( DateTime value ) => value.Ticks );
		}

		[Test]
		public void TestToMethod_Static()
		{
			FromExpression.ToMethod( () => Guid.NewGuid() );
			FromExpression.ToMethod( ( String value ) => Int32.Parse( value ) );
			FromExpression.ToMethod( ( String value, NumberStyles style ) => Int32.Parse( value, style ) );
			FromExpression.ToMethod( ( String value, NumberStyles style, IFormatProvider provider ) => Int32.Parse( value, style, provider ) );
#if !NETFX_CORE
			FromExpression.ToMethod( () => Console.WriteLine() );
			FromExpression.ToMethod( ( String value ) => Console.WriteLine( value ) );
			FromExpression.ToMethod( ( String value, object arg0 ) => Console.WriteLine( value, arg0 ) );
			FromExpression.ToMethod( ( String value, object arg0, String arg1 ) => Console.WriteLine( value, arg0, arg1 ) );
#endif
		}

		[Test]
		public void TestToMethod_Instance()
		{
			FromExpression.ToMethod( ( StringBuilder instance ) => instance.AppendLine() );
			FromExpression.ToMethod( ( StringBuilder instance, String value ) => instance.Append( value ) );
			FromExpression.ToMethod( ( StringBuilder instance, String value, object arg0 ) => instance.AppendFormat( value, arg0 ) );
			FromExpression.ToMethod( ( StringBuilder instance, String value, object arg0, object arg1 ) => instance.AppendFormat( value, arg0, arg1 ) );
			FromExpression.ToMethod( ( TextWriter instance ) => instance.WriteLine() );
			FromExpression.ToMethod( ( TextWriter instance, String value ) => instance.WriteLine( value ) );
			FromExpression.ToMethod( ( TextWriter instance, String value, object arg0 ) => instance.WriteLine( value, arg0 ) );
			FromExpression.ToMethod( ( TextWriter instance, String value, object arg0, String arg1 ) => instance.WriteLine( value, arg0, arg1 ) );
		}
	}
}
