#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using NUnit.Framework;
using System.IO;
using System.Diagnostics;

namespace MsgPack.Serialization
{
	[TestFixture]
	public class AutoMessagePackSerializerTest
	{
		private static bool _traceOn = true;

		[SetUp]
		public void SetUp()
		{
			if ( _traceOn )
			{
				Tracer.AutoMessagePackSerializer.Listeners.Clear();
				Tracer.AutoMessagePackSerializer.Switch.Level = SourceLevels.All;
				Tracer.AutoMessagePackSerializer.Listeners.Add( new ConsoleTraceListener() );
			}
		}

		[Test]
		public void TestInt32()
		{
			TestCore( 1, stream => Unpacking.UnpackInt32( stream ), null );
		}

		[Test]
		public void TestInt64()
		{
			TestCore( Int32.MaxValue + 1L, stream => Unpacking.UnpackInt64( stream ), null );
		}

		[Test]
		public void TestString()
		{
			TestCore( "abc", stream => Unpacking.UnpackString( stream ), null );
		}

		[Test]
		public void TestDateTime()
		{
			TestCore(
				DateTime.UtcNow,
				stream => MessagePackConvert.ToDateTime( Unpacking.UnpackInt64( stream ) ),
				CompareDateTime
			);
		}

		[Test]
		public void TestDateTimeOffset()
		{
			TestCore(
				DateTimeOffset.UtcNow,
				stream => MessagePackConvert.ToDateTimeOffset( Unpacking.UnpackInt64( stream ) ),
				( x, y ) => CompareDateTime( x.DateTime.ToUniversalTime(), y.DateTime.ToUniversalTime() )
			);
		}

		private static bool CompareDateTime( DateTime x, DateTime y )
		{
			return x.Date == y.Date && x.Hour == y.Hour && x.Minute == y.Minute && x.Second == y.Second && x.Millisecond == y.Millisecond;
		}

		[Test]
		public void TestUri()
		{
			TestCore( new Uri( "http://www.example.com" ), stream => new Uri( Unpacking.UnpackString( stream ) ), null );
		}

		[Test]
		public void TextComplexObject()
		{
			var target = new ComplexType() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
			TestCoreWithVerify( target );
		}

		[Test]
		public void TextComplexObjectTypeWithDataContract()
		{
			var target = new ComplexTypeWithDataContract() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
			target.NonSerialized = new DefaultTraceListener();
			TestCoreWithVerify( target );
		}

		[Test]
		public void TextComplexObjectTypeWithNonSerialized()
		{
			var target = new ComplexTypeWithNonSerialized() { Source = new Uri( "http://www.exambple.com" ), TimeStamp = DateTime.Now, Data = new byte[] { 0x1, 0x2, 0x3, 0x4 } };
			target.History.Add( DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ), "Create New" );
			target.NonSerialized = new DefaultTraceListener();
			TestCoreWithVerify( target );
		}

		private static void TestCore<T>( T value, Func<Stream, T> unpacking, Func<T, T, bool> comparer )
		{
			var safeComparer = comparer ?? EqualityComparer<T>.Default.Equals;
			var target = new AutoMessagePackSerializer<T>();
			using ( var buffer = new MemoryStream() )
			{
				new AutoMessagePackSerializer<T>().Pack( value, buffer );
				Console.WriteLine( "Length:{0}", buffer.Length );
				Console.WriteLine( BitConverter.ToString( buffer.ToArray() ) );
				buffer.Position = 0;
				T intermediate = unpacking( buffer );
				Assert.That( safeComparer( intermediate, value ), "Expected:{1}{0}Actual :{2}", Environment.NewLine, value, intermediate );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				Assert.That( safeComparer( unpacked, value ), "Expected:{1}{0}Actual :{2}", Environment.NewLine, value, unpacked );
			}
		}

		private static void TestCoreWithVerify<T>( T value )
			where T : IVerifiable
		{
			var target = new AutoMessagePackSerializer<T>();
			using ( var buffer = new MemoryStream() )
			{
				new AutoMessagePackSerializer<T>().Pack( value, buffer );
				buffer.Position = 0;
				T unpacked = target.Unpack( buffer );
				buffer.Position = 0;
				unpacked.Assert( buffer );
			}
		}
	}
}
