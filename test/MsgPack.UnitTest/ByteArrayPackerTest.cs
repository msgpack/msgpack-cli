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
using System.IO;
using System.Linq;
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
	[TestFixture]
	[Timeout( 1000 )]
	public partial class ByteArrayPackerTest : PackerTest
	{
		protected override Packer CreatePacker( MemoryStream stream )
		{
			Assert.That( stream.Position, Is.EqualTo( 0 ) );
			return Packer.Create( new byte[ 64 * 1024 ] );
		}

		protected override Packer CreatePacker( MemoryStream stream, PackerCompatibilityOptions compatibilityOptions )
		{
			Assert.That( stream.Position, Is.EqualTo( 0 ) );
			return Packer.Create( new byte[ 64 * 1024 ], true, compatibilityOptions );
		}

		private static ByteArrayPacker CreatePacker( byte[] buffer, int startOffset, bool allowsBufferExtension )
		{
			return Packer.Create( buffer, startOffset, allowsBufferExtension, PackerCompatibilityOptions.None );
		}

		private static ByteArrayPacker CreatePacker( byte[] buffer, int startOffset, Func<byte[], int, byte[]> allocator )
		{
			return Packer.Create( buffer, startOffset, allocator, PackerCompatibilityOptions.None );
		}

		protected override byte[] GetResult( Packer packer )
		{
			return ( ( ByteArrayPacker )packer ).GetResultBytes().ToArray();
		}

		private sealed class Allocator
		{
			public int LastAllocationSize { get; private set; }

			public bool IsAllocateCalled { get; set; }

			public bool IsReallocateCalled { get; set; }

			public bool IsOnlyAllocateCalled()
			{
				return this.IsAllocateCalled && !this.IsReallocateCalled;
			}

			public bool IsOnlyReallocateCalled()
			{
				return !this.IsAllocateCalled && this.IsReallocateCalled;
			}

			public ArraySegment<byte> Allocate( int size )
			{
				this.IsAllocateCalled = true;
				int actualSize = Math.Max( size, 16 );
				this.LastAllocationSize = actualSize;
				return new ArraySegment<byte>( new byte[ actualSize ] );
			}

			public byte[] Reallocate( byte[] old, int size )
			{
				this.IsReallocateCalled = true;
				int actualSize = old.Length + Math.Max( size, 16 );
				this.LastAllocationSize = actualSize;
				var result = new byte[ actualSize ];
				Buffer.BlockCopy( old, 0, result, 0, old.Length );
				return result;
			}
		}
	}
}
