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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
#if !MSTEST
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif
#endif

[CLSCompliant( false )]
#if !MSTEST
[SetUpFixture]
#else
[Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClass]
#endif
public sealed class _SetUpFixture
{
#if MSTEST
	[Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssemblyInitialize]
	public static void InitializeAssembly(Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestContext c)
	{
		new _SetUpFixture().SetupCurrentNamespaceTests();
		MsgPack.NUnitPortable.TestingPlatform.Current = new MSTest12TestingPlatform();
	}
#endif

#if !MSTEST
	[SetUp]
#endif
	public void SetupCurrentNamespaceTests()
	{
		Contract.ContractFailed += ( sender, e ) => e.SetUnwind();
	}
}

#if MSTEST
internal class MSTest12TestingPlatform : MsgPack.NUnitPortable.TestingPlatform
{
	public override void Success( string message )
	{
		throw new Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssertInconclusiveException( String.Format( System.Globalization.CultureInfo.CurrentCulture, "Success is not supported on MSTest v12. Original message:{0}{1}", Environment.NewLine, message ) );
	}

	public override void Fail( string message )
	{
		throw new Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssertFailedException( message );
	}

	public override void Inconclusive( string message )
	{
		throw new Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssertInconclusiveException( message );
	}

	public override void Ignore( string message )
	{
		throw new Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AssertInconclusiveException( String.Format( System.Globalization.CultureInfo.CurrentCulture, "Ignore is not supported on MSTest v12. Original message:{0}{1}", Environment.NewLine, message ) );
	}
}
#endif
