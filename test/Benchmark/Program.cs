// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//#define MSGPACK
#define JSON

extern alias newmpcli;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Benchmark.Fixture;
using Benchmark.Models;
using Benchmark.Serializers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MessagePack;
using MsgPack.Samples;

namespace Benchmark
{
	internal static class Program
	{
		static void Main(string[] args)
		{
			//JsonTest();
			BenchmarkRunner.Run<SampleBenchmark>();
		}

		static void Test(SerializerBase serializer, SampleObject obj)
		{
			Console.WriteLine($"---- {serializer.GetType().Name} ----");
			var array = serializer.Serialize<SampleObject>(obj);
			Console.WriteLine((array as byte[])?.Length);
#if MSGPACK
			Console.WriteLine(BitConverter.ToString(array as byte[]));
#else
			Console.WriteLine(Encoding.UTF8.GetString(array as byte[]));
#endif
			var result = serializer.Deserialize<SampleObject>(array);
			//Console.WriteLine($"Name       : {result?.Name}");
			//Console.WriteLine($"Age        : {result?.Age}");
			//Console.WriteLine($"IsActive   : {result?.IsActive}");
			//Console.WriteLine($"Roles      : [{String.Join(", ", result?.Roles ?? Array.Empty<string>())}]");
			//Console.WriteLine($"Attributes : {{{String.Join(", ", (result?.Attributes ?? Enumerable.Empty<KeyValuePair<string, string>>()).Select(x => $"{x.Key}: {x.Value}"))}}}");
		}

		static void JsonTest()
		{
			var obj = SampleObject.TestValue;
				//new SampleObject { Name = "A\tあいうえお\u007F　\\u3042" };
			//Test(new JsonNet(), obj);
			Test(new MsgPackCliJson(), obj);
			//Test(new SpanJson_(), obj);
			//Test(new SystemTextJson(), obj);
			//Test(new Utf8Json_(), obj);
		}
	}

	[Config(typeof(BenchmarkConfig))]
	public class SampleBenchmark
	{
		[ParamsSource(nameof(Serializers))]
		public SerializerBase Serializer;

		private bool isContractless;

		// Currently BenchmarkdDotNet does not detect inherited ParamsSource so use copy and paste:)
		public IEnumerable<SerializerBase> Serializers => new SerializerBase[]
		{
#if MSGPACK
			new MessagePack_v2(),
#endif
			//new MsgPack_v2_opt(),
			//new MessagePackLz4_v2(),
			//new MsgPack_v2_string(),
			//new MsgPack_v2_str_lz4(),
			//new ProtobufNet(),
#if JSON
			new JsonNet(),
#endif
			//new BinaryFormatter_(),
			//new DataContract_(),
			//new Hyperion_(),
			//new Jil_(),
#if JSON
			new SpanJson_(),
			new Utf8Json_(),
			new SystemTextJson(),
			new MsgPackCliJson(),
#endif
#if MSGPACK

			new MsgPackCli(),
			new MsgPackCli_v2(),
#endif
			//new FsPickler_(),
			//new Ceras_(),
		};

		protected static readonly ExpressionTreeFixture ExpressionTreeFixture = new ExpressionTreeFixture();

		// primitives
		protected static readonly int IntInput = ExpressionTreeFixture.Create<int>();

		// models
		protected static readonly SampleObject AnswerInput = SampleObject.TestValue;//  ExpressionTreeFixture.Create<SampleObject>();
																					// not same data so does not gurantee correctly.
		protected static readonly SampleObject Answer2Input = SampleObject.TestValue;//ExpressionTreeFixture.Create<SampleObject>();

		private object IntOutput;
		private object AnswerOutput;

		[GlobalSetup]
		public void Setup()
		{
			this.isContractless = (Serializer is MsgPack_v2_string) || (Serializer is MsgPack_v2_str_lz4);

			// primitives
			this.IntOutput = this.Serializer.Serialize(IntInput);

			// models
			if (isContractless)
			{
				this.AnswerOutput = this.Serializer.Serialize(Answer2Input);
			}
			else
			{
				this.AnswerOutput = this.Serializer.Serialize(AnswerInput);
			}

			var o = this.AnswerOutput as byte[];
			Console.WriteLine("-------------------------------------------------------------------");
			Console.WriteLine($"{this.Serializer.GetType().Name} -> {o.Length} bytes");
			Console.WriteLine(BitConverter.ToString(o));
			Console.WriteLine("-------------------------------------------------------------------");
		}

		// Serialize
		/* [Benchmark] public object _PrimitiveIntSerialize() => this.Serializer.Serialize(IntInput); */

	[Benchmark]
		public object AnswerSerialize()
		{
			if (isContractless)
			{
				return this.Serializer.Serialize(Answer2Input);
			}
			else
			{
				return this.Serializer.Serialize(AnswerInput);
			}
		}

		// Deserialize
		/* [Benchmark] public Int32 _PrimitiveIntDeserialize() => this.Serializer.Deserialize<Int32>(this.IntOutput); */

		[Benchmark]
		public object AnswerDeserialize()
		{
			if (isContractless)
			{
				return this.Serializer.Deserialize<SampleObject>(this.AnswerOutput);
			}
			else
			{
				return this.Serializer.Deserialize<SampleObject>(this.AnswerOutput);
			}
		}
	}
}
