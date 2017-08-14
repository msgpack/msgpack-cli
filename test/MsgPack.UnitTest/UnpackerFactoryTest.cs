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
using TestCaseAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.DataRowAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack
{
	[TestFixture]
	[Timeout( 1000 )]
	public class UnpackerFactoryTest
	{
		[Test]
		public void TestCreate_Stream_StreamIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => { using ( Unpacker.Create( default( Stream ) ) ) { } } );
		}

		[Test]
		public void TestCreate_Stream_Boolean_StreamIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => { using ( Unpacker.Create( default( Stream ), true ) ) { } } );
		}

		[Test]
		public void TestCreate_OwnsStreamisFalse_NotDisposeStream()
		{
			using ( var stream = new MemoryStream() )
			{
				using ( var unpacker = Unpacker.Create( stream, false ) )
				{
					Assert.That( unpacker, Is.InstanceOf<MessagePackStreamUnpacker>() );
					Assert.That( ( unpacker as MessagePackStreamUnpacker ).DebugOwnsStream, Is.False );
				}

				// Should not throw ObjectDisposedException.
				stream.WriteByte( 1 );
			}
		}

		[Test]
		public void TestCreate_Stream_PackerUnpackerStreamOptions_StreamIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => { using ( Unpacker.Create( default( Stream ), default( PackerUnpackerStreamOptions ) ) ) { } } );
		}

		[Test]
		public void TestCreate_Stream_PackerUnpackerStreamOptions_UnpackerOptions_StreamIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => { using ( Unpacker.Create( default( Stream ), default( PackerUnpackerStreamOptions ), default( UnpackerOptions ) ) ) { } } );
		}

		[Test]
		public void TestCreate_StreamOptionIsNull()
		{
			PackerUnpackerStreamOptions.AlwaysWrap = true;
			try
			{
				using ( var stream = new MemoryStream() )
				using ( var unpacker = Unpacker.Create( stream, default( PackerUnpackerStreamOptions ), default( UnpackerOptions ) ) )
				{
					Assert.That( unpacker, Is.InstanceOf<MessagePackStreamUnpacker>() );
					var streamUnpacker = unpacker as MessagePackStreamUnpacker;
					Assert.That( streamUnpacker.DebugOwnsStream, Is.False );
					Assert.That( streamUnpacker.DebugSource, Is.SameAs( stream ) );
				}
			}
			finally
			{
				PackerUnpackerStreamOptions.AlwaysWrap = false;
			}
		}

		[Test]
		public void TestCreate_WithBuffering()
		{
			PackerUnpackerStreamOptions.AlwaysWrap = true;
			try
			{
				using ( var stream = new MemoryStream() )
				using ( var unpacker = Unpacker.Create( stream, new PackerUnpackerStreamOptions { OwnsStream = false, WithBuffering = true, BufferSize = 123 }, default( UnpackerOptions ) ) )
				{
					Assert.That( unpacker, Is.InstanceOf<MessagePackStreamUnpacker>() );
					var streamUnpacker = unpacker as MessagePackStreamUnpacker;
					Assert.That( streamUnpacker.DebugOwnsStream, Is.False );
#if !SILVERLIGHT
					Assert.That( streamUnpacker.DebugSource, Is.Not.SameAs( stream ) );
#if NETSTANDARD1_1 || NETSTANDARD1_3
					// Avoid type name conflicts between netcoreapp and msgpack
					Assert.That( streamUnpacker.DebugSource.GetType().FullName, Is.EqualTo( "System.IO.BufferedStream" ) );
					Assert.That( streamUnpacker.DebugSource.GetType().GetAssembly().FullName, Is.EqualTo( typeof( MessagePackObject ).GetAssembly().FullName ) );
#else // NETSTANDARD1_1 || NETSTANDARD1_3
					Assert.That( streamUnpacker.DebugSource, Is.InstanceOf<BufferedStream>() );
#endif // // NETSTANDARD1_1 || NETSTANDARD1_3
#else
					Assert.That( streamUnpacker.DebugSource, Is.SameAs( stream ) );
#endif // !SILVERLIGHT
				}
			}
			finally
			{
				PackerUnpackerStreamOptions.AlwaysWrap = false;
			}
		}

		[Test]
		public void TestCreate_Stream_DefaultValidationLevel()
		{
			using ( var stream = new MemoryStream() )
			using ( var unpacker = Unpacker.Create( stream, PackerUnpackerStreamOptions.None, default( UnpackerOptions ) ) )
			{
				Assert.That( unpacker, Is.InstanceOf<CollectionValidatingStreamUnpacker>() );
			}
		}

		[Test]
		public void TestCreate_Stream_CollectionValidationLevel()
		{
			using ( var stream = new MemoryStream() )
			using ( var unpacker = Unpacker.Create( stream, PackerUnpackerStreamOptions.None, new UnpackerOptions { ValidationLevel = UnpackerValidationLevel.Collection } ) )
			{
				Assert.That( unpacker, Is.InstanceOf<CollectionValidatingStreamUnpacker>() );
			}
		}

		[Test]
		public void TestCreate_Stream_NoneValidationLevel()
		{
			using ( var stream = new MemoryStream() )
			using ( var unpacker = Unpacker.Create( stream, PackerUnpackerStreamOptions.None, new UnpackerOptions { ValidationLevel = UnpackerValidationLevel.None } ) )
			{
				Assert.That( unpacker, Is.InstanceOf<FastStreamUnpacker>() );
			}
		}

		private static void AssertSource( ByteArrayUnpacker unpacker, byte[] array, int expectedOffset )
		{
			Assert.That( unpacker, Is.InstanceOf<MessagePackByteArrayUnpacker>() );
			var byteArrayUnpacker = unpacker as MessagePackByteArrayUnpacker;
			Assert.That( byteArrayUnpacker.DebugSource, Is.SameAs( array ) );
			Assert.That( byteArrayUnpacker.Offset, Is.EqualTo( expectedOffset ) );
		}

		[Test]
		public void TestCreate_ByteArray_EntireArray()
		{
			var array = Guid.NewGuid().ToByteArray();

			using ( var unpacker = Unpacker.Create( array ) )
			{
				AssertSource( unpacker, array, 0 );
			}
		}

		[Test]
		public void TestCreate_ByteArray_ArrayIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => { using ( Unpacker.Create( default( byte[] ) ) ) { } } );
		}

		[Test]
		public void TestCreate_ByteArray_ArrayIsEmpty()
		{
			Assert.Throws<ArgumentException>( () => { using ( Unpacker.Create( new byte[ 0 ] ) ) { } } );
		}

		[Test]
		public void TestCreate_ByteArray_Int32_Offset()
		{
			var array = Guid.NewGuid().ToByteArray();
			var offset = 1;

			using ( var unpacker = Unpacker.Create( array, offset ) )
			{
				AssertSource( unpacker, array, offset );
			}
		}

		[Test]
		public void TestCreate_ByteArray_Int32_Empty()
		{
			Assert.Throws<ArgumentException>( () => { using ( Unpacker.Create( new byte[ 0 ], 0 ) ) { } } );
		}

		[Test]
		public void TestCreate_ByteArray_Int32_ArrayIsNull()
		{
			Assert.Throws<ArgumentNullException>( () => { using ( Unpacker.Create( default( byte[] ), 0 ) ) { } } );
		}

		[Test]
		public void TestCreate_ByteArray_Int32_NegativeOffset()
		{
			Assert.Throws<ArgumentOutOfRangeException>( () => { using ( Unpacker.Create( new byte[ 1 ], -1 ) ) { } } );
		}

		[Test]
		public void TestCreate_ByteArray_Int32_TooLargeOffset()
		{
			Assert.Throws<ArgumentException>( () => { using ( Unpacker.Create( new byte[ 1 ], 2 ) ) { } } );
		}

		[Test]
		public void TestCreate_ByteArray_Int32_DefaultValidationLevel()
		{
			var array = Guid.NewGuid().ToByteArray();

			using ( var unpacker = Unpacker.Create( array, 0, default( UnpackerOptions ) ) )
			{
				Assert.That( unpacker, Is.InstanceOf<CollectionValidatingByteArrayUnpacker>() );
			}
		}

		[Test]
		public void TestCreate_ByteArray_Int32_CollectionValidationLevel()
		{
			var array = Guid.NewGuid().ToByteArray();

			using ( var unpacker = Unpacker.Create( array, 0, new UnpackerOptions { ValidationLevel = UnpackerValidationLevel.Collection } ) )
			{
				Assert.That( unpacker, Is.InstanceOf<CollectionValidatingByteArrayUnpacker>() );
			}
		}

		[Test]
		public void TestCreate_ByteArray_Int32_NoneValidationLevel()
		{
			var array = Guid.NewGuid().ToByteArray();

			using ( var unpacker = Unpacker.Create( array, 0, new UnpackerOptions { ValidationLevel = UnpackerValidationLevel.None } ) )
			{
				Assert.That( unpacker, Is.InstanceOf<FastByteArrayUnpacker>() );
			}
		}
	}
}
