#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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

#if PERFORMANCE_TEST
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
	public class PerformanceTest
	{
		private const int Iteration = 500;

		private static TestResult[] Test(
			EmitterFlavor flavor,
			SerializationMethod method,
			EnumSerializationMethod enumMethod )
		{
			var stopWatch = new Stopwatch();
			using ( var buffer = new MemoryStream() )
			{
				// JIT
				TestCreateSerializer(
					stopWatch,
					new SerializationContext
					{
						EmitterFlavor = flavor,
						SerializationMethod = method,
						EnumSerializationMethod = enumMethod
					}
					);
				TestSerialize(
					stopWatch,
					buffer,
					new SerializationContext
					{
						EmitterFlavor = flavor,
						SerializationMethod = method,
						EnumSerializationMethod = enumMethod
					}
					);
				buffer.Position = 0;
				TestDeserialize(
					stopWatch,
					buffer,
					new SerializationContext
					{
						EmitterFlavor = flavor,
						SerializationMethod = method,
						EnumSerializationMethod = enumMethod
					}
					);

				var samples = new long[ flavor == EmitterFlavor.CodeDomBased ? 10 : Iteration ];

				// 1st-time only
				for ( int i = 0; i < samples.Length; i++ )
				{
					TestCreateSerializer(
						stopWatch,
						new SerializationContext
						{
							EmitterFlavor = flavor,
							SerializationMethod = method,
							EnumSerializationMethod = enumMethod
						}
						);
					samples[ i ] = stopWatch.Elapsed.Ticks;
				}

				var creationResult = Calculate( samples );

				samples = new long[ Iteration ];

				// Exept-1st time
				var context =
					new SerializationContext
					{
						EmitterFlavor = flavor,
						SerializationMethod = method,
						EnumSerializationMethod = enumMethod
					};

				var samples2 = new long[ samples.Length ];

				// Dry-run
				buffer.SetLength( 0 );
				TestSerialize( stopWatch, buffer, context );
				buffer.Position = 0;
				TestDeserialize( stopWatch, buffer, context );

				for ( int i = 0; i < samples.Length; i++ )
				{
					buffer.SetLength( 0 );
					TestSerialize( stopWatch, buffer, context );
					samples[ i ] = stopWatch.Elapsed.Ticks;
					buffer.Position = 0;
					TestDeserialize( stopWatch, buffer, context );
					samples2[ i ] = stopWatch.Elapsed.Ticks;
				}

				var serializationResult = Calculate( samples );
				var deserializationResult = Calculate( samples2 );

				return new TestResult[] { creationResult, serializationResult, deserializationResult };
			}
		}

		private static byte[] Image = GenerateImage();

		private static byte[] GenerateImage()
		{
			var random = new Random();
			var bytes = new byte[ 256 * 1024 ];
			random.NextBytes( bytes );
			return bytes;
		}

		private static void TestCreateSerializer( Stopwatch stopWatch, SerializationContext serializationContext )
		{
			stopWatch.Restart();
			serializationContext.GetSerializer<PhotoData>();
			stopWatch.Stop();
		}

		private static void TestSerialize( Stopwatch stopWatch, Stream buffer, SerializationContext serializationContext )
		{
			var target =
				new PhotoData
				{
					Id = 123,
					Image = Image,
					Rating = Rating.Good,
					TakenDateTime = DateTimeOffset.Now,
					Title = "Favorite"
				};
			target.Tags.Add( "Example" );
			target.Tags.Add( "Foo" );
			target.Tags.Add( "Bar" );

			var serializer = serializationContext.GetSerializer<PhotoData>();
			stopWatch.Restart();
			serializer.Pack( buffer, target );
			stopWatch.Stop();
		}


		private static void TestDeserialize( Stopwatch stopWatch, Stream buffer, SerializationContext serializationContext )
		{
			var serializer = serializationContext.GetSerializer<PhotoData>();
			stopWatch.Restart();
			serializer.Unpack( buffer );
			stopWatch.Stop();
		}

		private static TestResult Calculate( long[] samples )
		{
			var max = 0L;
			var min = Int64.MaxValue;
			var sum = 0L;
			var unbiasedVariance = 0.0;
			for ( int i = 0; i < samples.Length; i++ )
			{
				max = Math.Max( max, samples[ i ] );
				min = Math.Min( min, samples[ i ] );
				sum += samples[ i ];
			}

			var average = sum * 1.0 / samples.Length;

			for ( int i = 0; i < samples.Length; i++ )
			{
				unbiasedVariance += Math.Pow( samples[ i ] - average, 2 );
			}

			unbiasedVariance /= ( samples.Length - 1 );
			var unbiasedStdDev = Math.Pow( unbiasedVariance, 0.5 );
			var normalSamples = new List<long>( samples.Length );
			var normalLower = average - unbiasedStdDev;
			var normalUpper = average + unbiasedStdDev;
			// Filters iregular values
			for ( int i = 0; i < samples.Length; i++ )
			{
				if ( normalLower < samples[ i ] && samples[ i ] < normalUpper )
				{
					normalSamples.Add( samples[ i ] );
				}
			}
			
			return
				new TestResult
				{
					AverageElapsedTicks = normalSamples.Sum() / normalSamples.Count,
					MaxElapsedTicks = max,
					MinElapsedTicks = min,
					StandardDeviation = unbiasedStdDev
				};
		}

		[Test]
		public void TestPerformance()
		{
			SerializerDebugging.OnTheFlyCodeDomEnabled = true;
			SerializerDebugging.AddRuntimeAssembly(Assembly.GetExecutingAssembly().Location);
			Console.WriteLine(
				"Flavor\tMethod\tEnumMethod\tMin(usec,gen)\tMax(usec,gen)\tAvg(usec,gen)\tStdDiv(gen)\tMin(usec,pack)\tMax(usec,pack)\tAvg(usec,pack)\tStdDiv(usec,pack)\tMin(usec,unpack)\tMax(usec,unpack)\tAvg(usec,unpack)\tStdDiv(usec,unpack)"
			);
			foreach ( var flavor in
				new[] { EmitterFlavor.FieldBased, EmitterFlavor.ContextBased, EmitterFlavor.ExpressionBased, EmitterFlavor.CodeDomBased, EmitterFlavor.ReflectionBased } )
			{
				foreach ( var serializationMethod in new[] { SerializationMethod.Array, SerializationMethod.Map } )
				{
					foreach ( var enumSerializationMethod in new[] { EnumSerializationMethod.ByName, EnumSerializationMethod.ByUnderlyingValue } )
					{
						var result = Test( flavor, serializationMethod, enumSerializationMethod );
						Console.WriteLine(
							"{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}",
							flavor,
							serializationMethod,
							enumSerializationMethod,
							result[ 0 ].MinElapsedTicks / 10.0,
							result[ 0 ].MaxElapsedTicks / 10.0,
							result[ 0 ].AverageElapsedTicks / 10.0,
							result[ 0 ].StandardDeviation / 10.0,
							result[ 1 ].MinElapsedTicks / 10.0,
							result[ 1 ].MaxElapsedTicks / 10.0,
							result[ 1 ].AverageElapsedTicks / 10.0,
							result[ 1 ].StandardDeviation / 10.0,
							result[ 2 ].MinElapsedTicks / 10.0,
							result[ 2 ].MaxElapsedTicks / 10.0,
							result[ 2 ].AverageElapsedTicks / 10.0,
							result[ 2 ].StandardDeviation / 10.0
							);
					}
				}
			}

		}

		private class TestResult
		{
			public long MaxElapsedTicks;
			public long MinElapsedTicks;
			public long AverageElapsedTicks;
			public double StandardDeviation;
		}
	}

	public class PhotoData
	{
		private long _id;

		public long Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		private string _title;

		public string Title
		{
			get { return this._title; }
			set { this._title = value; }
		}

		private DateTimeOffset _takenDateTime;

		public DateTimeOffset TakenDateTime
		{
			get
			{
				return this._takenDateTime;
			}
			set
			{
				this._takenDateTime = value;
			}
		}

		private Rating _rating;

		public Rating Rating
		{
			get { return this._rating; }
			set { this._rating = value; }
		}

		private byte[] _image;

		public byte[] Image
		{
			get { return this._image; }
			set { this._image = value; }
		}

		private readonly List<string> _tags = new List<string>();

		public IList<string> Tags
		{
			get { return this._tags; }
		}

		public PhotoData() { }
	}

	public enum Rating
	{
		Unspecified = 0,
		Poor = 1,
		Ok = 2,
		Good = 3,
		Great = 4,
		Excellent = 5
	}
}
#endif
