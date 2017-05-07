#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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

namespace MsgPack
{
	public abstract partial class ByteArrayUnpackerTest : UnpackerTest
	{
		protected override bool ShouldCheckStreamPosition
		{
			get { return false; }
		}

		protected override bool CanReadFromEmptySource
		{
			get { return false; }
		}

		protected sealed override Unpacker CreateUnpacker( MemoryStream stream )
		{
			return this.CreateUnpacker( stream.ToArray() );
		}

		protected ByteArrayUnpacker CreateUnpacker( byte[] source )
		{
			return this.CreateUnpacker( new ArraySegment<byte>( source ) );
		}

		protected abstract ByteArrayUnpacker CreateUnpacker( ArraySegment<byte> source );

		protected abstract ByteArrayUnpacker CreateUnpacker( IList<ArraySegment<byte>> sources );

		private static ArraySegment<byte> PrependAppendExtra( byte[] data )
		{
			var buffer = new byte[ data.Length + 2 ];
			data.CopyTo( buffer, 1 );
			buffer[ 0 ] = 0xC1;
			buffer[ buffer.Length - 1 ] = 0xC1;
			return new ArraySegment<byte>( buffer, 1, data.Length + 1 );
		}

		private static ArraySegment<byte> Limit( byte[] data )
		{
			return new ArraySegment<byte>( data, 0, data.Length - 1 );
		}

		private static IList<ArraySegment<byte>> Split( byte[] data )
		{
			switch ( data.Length )
			{
				case 0:
				case 1:
				{
					Assert.Fail( "Invalid test. data.Length must be greator than 1." );
					return null;
				}
				case 2:
				{
					return
						new[]
						{
							new ArraySegment<byte>( data, 0, 1 ),
							new ArraySegment<byte>( data, 1, 1 ),
							new ArraySegment<byte>( new byte[] { 0xC1 }, 0, 1 )
						};
				}
				case 3:
				{
					return
						new[]
						{
							new ArraySegment<byte>( data, 0, 1 ),
							new ArraySegment<byte>( data, 1, 1 ),
							new ArraySegment<byte>( data, 2, 1 ),
							new ArraySegment<byte>( new byte[] { 0xC1 }, 0, 1 )
						};
				}
				default:
				{
					return
						new[]
						{
							new ArraySegment<byte>( data, 0, 1 ),
							new ArraySegment<byte>( data, 1, 1 ),
							new ArraySegment<byte>( data, 2, data.Length - 3 ),
							new ArraySegment<byte>( data, data.Length - 1, 1 ),
							new ArraySegment<byte>( new byte[] { 0xC1 }, 0, 1 )
						};
				}
			}
		}
	}
}
