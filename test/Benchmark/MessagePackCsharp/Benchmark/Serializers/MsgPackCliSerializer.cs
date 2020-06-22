// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

extern alias newmpcli;

using System;
using System.Buffers;
using System.Threading;
using Benchmark.Serializers;
using MsgPack.Samples;

#pragma warning disable SA1649 // File name should match first type name

public class MsgPackCli : SerializerBase
{
	public override T Deserialize<T>(object input)
	{
		return MsgPackCliSerializerRepository<T>.V1.UnpackSingleObject((byte[])input);
	}

	public override object Serialize<T>(T input)
	{
		return MsgPackCliSerializerRepository<T>.V1.PackSingleObject(input);
	}
}
public class MsgPackCli_with_Get : SerializerBase
{
	public override T Deserialize<T>(object input)
	{
		return MsgPack.Serialization.MessagePackSerializer.Get<T>().UnpackSingleObject((byte[])input);
	}

	public override object Serialize<T>(T input)
	{
		return MsgPack.Serialization.MessagePackSerializer.Get<T>().PackSingleObject(input);
	}
}

internal static class MsgPackCliSerializerRepository<T>
{
	public static readonly MsgPack.Serialization.MessagePackSerializer<T> V1 = SampleSerializer.SerializationContext.GetSerializer<T>();

	public static readonly newmpcli::MsgPack.Serialization.ObjectSerializer<T> V2 = InitV2();

	public static readonly newmpcli::MsgPack.Serialization.ObjectSerializer<T> Json = InitJson();

	private static newmpcli::MsgPack.Serialization.ObjectSerializer<T> InitV2()
	{
		if (typeof(MsgPack.Samples.SampleObject).IsAssignableFrom(typeof(T)))
		{
			return (newmpcli::MsgPack.Serialization.ObjectSerializer<T>)(object)new MsgPack.Samples.SampleSerializer(new newmpcli::MsgPack.Serialization.ObjectSerializationContext());
		}
		else if (typeof(T) == typeof(int[]))
		{
			return (newmpcli::MsgPack.Serialization.ObjectSerializer<T>)(object)new MsgPack.Samples.SampleInt32ArraySerializer(new newmpcli::MsgPack.Serialization.ObjectSerializationContext());
		}
		else if (typeof(T) == typeof(int))
		{
			return (newmpcli::MsgPack.Serialization.ObjectSerializer<T>)(object)new MsgPack.Samples.SampleInt32Serializer(new newmpcli::MsgPack.Serialization.ObjectSerializationContext());
		}

		throw new NotSupportedException($"No {typeof(T)} serializer.");
	}

	private static newmpcli::MsgPack.Serialization.ObjectSerializer<T> InitJson()
	{
		if (typeof(MsgPack.Samples.SampleObject).IsAssignableFrom(typeof(T)))
		{
			return (newmpcli::MsgPack.Serialization.ObjectSerializer<T>)(object)new MsgPack.Samples.SampleSerializer(new newmpcli::MsgPack.Serialization.ObjectSerializationContext());
		}
		else if (typeof(T) == typeof(int[]))
		{
			return (newmpcli::MsgPack.Serialization.ObjectSerializer<T>)(object)new MsgPack.Samples.SampleInt32ArraySerializer(new newmpcli::MsgPack.Serialization.ObjectSerializationContext());
		}
		else if (typeof(T) == typeof(int))
		{
			return (newmpcli::MsgPack.Serialization.ObjectSerializer<T>)(object)new MsgPack.Samples.SampleInt32Serializer(new newmpcli::MsgPack.Serialization.ObjectSerializationContext());
		}

		throw new NotSupportedException($"No {typeof(T)} serializer.");
	}

}

public class MsgPackCli_v2 : SerializerBase
{
	private static readonly newmpcli::MsgPack.Internal.MessagePackEncoder Encoder = newmpcli::MsgPack.Internal.MessagePackEncoder.CreateCurrent(newmpcli::MsgPack.Internal.MessagePackEncoderOptions.Default);
	private static readonly newmpcli::MsgPack.Internal.MessagePackDecoder Decoder = new newmpcli::MsgPack.Internal.MessagePackDecoder(newmpcli::MsgPack.Internal.MessagePackDecoderOptions.Default);

	public override T Deserialize<T>(object input)
	{
		var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>((byte[])input));
		var context =
			new newmpcli::MsgPack.Serialization.DeserializationOperationContext(
				Decoder,
				null,
				CancellationToken.None
			);
		return MsgPackCliSerializerRepository<T>.V2.Deserialize(ref context, ref reader);
	}

	[ThreadStatic]
	private static ArrayBufferWriter<byte> t_writer;

	public override object Serialize<T>(T input)
	{
		if (t_writer == null)
		{
			t_writer = new ArrayBufferWriter<byte>(128);
		}
		else
		{
			t_writer.Clear();
		}

		var writer = t_writer;
		var context =
			new newmpcli::MsgPack.Serialization.SerializationOperationContext(
				Encoder,
				null,
				CancellationToken.None
			);
		MsgPackCliSerializerRepository<T>.V2.Serialize(ref context, input, writer);
		return writer.WrittenMemory.ToArray();
	}
}



public class MsgPackCliJson : SerializerBase
{
	private static readonly newmpcli::MsgPack.Json.JsonEncoder Encoder = new newmpcli::MsgPack.Json.JsonEncoder(newmpcli::MsgPack.Json.JsonEncoderOptions.Default);
	private static readonly newmpcli::MsgPack.Json.JsonDecoder Decoder = newmpcli::MsgPack.Json.JsonDecoder.Create(newmpcli::MsgPack.Json.JsonDecoderOptions.Default);

	public override T Deserialize<T>(object input)
	{
		var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>((byte[])input));
		var context =
			new newmpcli::MsgPack.Serialization.DeserializationOperationContext(
				Decoder,
				null,
				CancellationToken.None
			);
		return MsgPackCliSerializerRepository<T>.Json.Deserialize(ref context, ref reader);
	}

	[ThreadStatic]
	private static ArrayBufferWriter<byte> t_writer;

	public override object Serialize<T>(T input)
	{
		if (t_writer == null)
		{
			t_writer = new ArrayBufferWriter<byte>(128);
		}
		else
		{
			t_writer.Clear();
		}

		var writer = t_writer;
		var context =
			new newmpcli::MsgPack.Serialization.SerializationOperationContext(
				Encoder,
				null,
				CancellationToken.None
			);
		MsgPackCliSerializerRepository<T>.Json.Serialize(ref context, input, writer);
		return writer.WrittenMemory.ToArray();
	}
}

