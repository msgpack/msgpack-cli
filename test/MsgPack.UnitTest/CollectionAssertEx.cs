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
using System.Linq;
using System.Text;
using System.Collections;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif
using System.Globalization;

namespace MsgPack
{
	public static class CollectionAssertEx
	{
		public static void StartsWith( IEnumerable expected, IEnumerable actual)
		{
			StartsWith( expected, actual, null );
		}

		public static void StartsWith( IEnumerable expected, IEnumerable actual, String format, params object[] args )
		{
			var expectedIterator = expected.GetEnumerator();
			try
			{
				var actualIterator = actual.GetEnumerator();
				try
				{
					long index = -1L;
					while ( expectedIterator.MoveNext() )
					{
						index++;

						if ( !actualIterator.MoveNext() )
						{
							Assert.Fail( "{2}{0}'actual' too short.{0}Index: {1}", Environment.NewLine, index, format == null ? null : String.Format( CultureInfo.CurrentCulture, format, args ) );
						}

						var expectedValue = expectedIterator.Current;
						var actualValue = actualIterator.Current;

						if ( expectedValue == null )
						{
							if ( actualValue != null )
							{
								Assert.Fail( "{3}{0}'actual' at {1} is not null.{0}Expcted : null{0}Actual : {2}(0x{2:x})", Environment.NewLine, index, actualValue, format == null ? null : String.Format( CultureInfo.CurrentCulture, format, args ) );
							}
						}
						else
						{
							if ( expectedValue == null )
							{
								Assert.Fail( "{3}{0}'actual' at {1} is not null.{0}Expcted : {2}(0x{2:x}){0}Actual : null", Environment.NewLine, index, expectedValue, format == null ? null : String.Format( CultureInfo.CurrentCulture, format, args ) );
							}
							else
							{
								if ( !expectedValue.Equals( actualValue ) )
								{
									Assert.Fail( "{4}{0}Items at {1} is not equal.{0}Expcted : {2}(0x{2:x}){0}Actual : {3}(0x{3:x})", Environment.NewLine, index, expectedValue, actualValue, format == null ? null : String.Format( CultureInfo.CurrentCulture, format, args ) );
								}
							}
						}
					}
				}
				finally
				{
					TryDispose( actualIterator );
				}
			}
			finally
			{
				TryDispose( expectedIterator );
			}
		}

		private static void TryDispose( object mightBeDisposable )
		{
			var disposable = mightBeDisposable as IDisposable;
			if ( disposable != null )
			{
				disposable.Dispose();
			}
		}
	}
}
