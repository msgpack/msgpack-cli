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
using System.IO;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
using ExplicitAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.IgnoreAttribute;
#endif

namespace MsgPack
{
	[TestFixture]
	public class PackerTest
	{
#if !NETFX_CORE && !SILVERLIGHT && !XAMIOS && !UNITY_IPHONE
		[Test]
		public void DefaultCompatibilityOptions_Classic()
		{
			var worker = AppDomain.CreateDomain( "worker", null, AppDomain.CurrentDomain.SetupInformation );

			try
			{
				worker.DoCallBack( DefaultCompatibilityOptions_ClassicCore );
			}
			finally
			{
				AppDomain.Unload( worker );
			}
		}

		private static void DefaultCompatibilityOptions_ClassicCore()
		{
			Assert.That( Packer.DefaultCompatibilityOptions, Is.EqualTo( PackerCompatibilityOptions.Classic ) );
		}
#endif // !NETFX_CORE && !SILVERLIGHT && !XAMIOS && !UNITY_IPHONE

		[Test]
		public void TestCreate_DefaultOptions()
		{
			using ( var target = Packer.Create( Stream.Null ) )
			{
				Assert.That( target, Is.Not.Null );
				Assert.That( target.CompatibilityOptions, Is.EqualTo( Packer.DefaultCompatibilityOptions ) );
			}

			using ( var target = Packer.Create( Stream.Null, ~Packer.DefaultCompatibilityOptions ) )
			{
				Assert.That( target, Is.Not.Null );
				Assert.That( target.CompatibilityOptions, Is.EqualTo( ~Packer.DefaultCompatibilityOptions ) );
			}

			using ( var target = Packer.Create( Stream.Null, false ) )
			{
				Assert.That( target, Is.Not.Null );
				Assert.That( target.CompatibilityOptions, Is.EqualTo( Packer.DefaultCompatibilityOptions ) );
			}

			using ( var target = Packer.Create( Stream.Null, ~Packer.DefaultCompatibilityOptions, false ) )
			{
				Assert.That( target, Is.Not.Null );
				Assert.That( target.CompatibilityOptions, Is.EqualTo( ~Packer.DefaultCompatibilityOptions ) );
			}
		}

		[Test]
		public void TestCreate_OwnsStreamIsFalse_StreamIsNotClosedWhenPackerIsDisposed()
		{
			var stream = new CloseAwareStream();
			Packer.Create( stream ).Dispose();
			Assert.That( stream.IsClosed, Is.True );

			stream = new CloseAwareStream();
			Packer.Create( stream, ~Packer.DefaultCompatibilityOptions ).Dispose();
			Assert.That( stream.IsClosed, Is.True );

			stream = new CloseAwareStream();
			Packer.Create( stream, false ).Dispose();
			Assert.That( stream.IsClosed, Is.False );
			stream.Dispose();

			stream = new CloseAwareStream();
			Packer.Create( stream, ~Packer.DefaultCompatibilityOptions, false ).Dispose();
			Assert.That( stream.IsClosed, Is.False );
			stream.Dispose();
		}

		[Test]
		public void TestCreate_NullStream_ArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>( () => Packer.Create( null ) );
			Assert.Throws<ArgumentNullException>( () => Packer.Create( null, ~Packer.DefaultCompatibilityOptions ) );
			Assert.Throws<ArgumentNullException>( () => Packer.Create( null, false ) );
			Assert.Throws<ArgumentNullException>( () => Packer.Create( null, ~Packer.DefaultCompatibilityOptions, false ) );
		}

		private sealed class CloseAwareStream : Stream
		{
			public bool IsClosed
			{
				get;
				private set;
			}

			public override bool CanRead
			{
				get { return true; }
			}

			public override bool CanSeek
			{
				get { return true; }
			}

			public override bool CanWrite
			{
				get { return true; }
			}

			public override long Length
			{
				get { return 0; }
			}

			public override long Position
			{
				get { return 0; }
				set { }
			}

			public CloseAwareStream() { }

			protected override void Dispose( bool disposing )
			{
				this.IsClosed = true;
				base.Dispose( disposing );
			}

			public override void Flush() { }

			public override int Read( byte[] buffer, int offset, int count )
			{
				return 0;
			}

			public override long Seek( long offset, SeekOrigin origin )
			{
				return 0;
			}

			public override void SetLength( long value ) { }

			public override void Write( byte[] buffer, int offset, int count ) { }
		}

	}
}
