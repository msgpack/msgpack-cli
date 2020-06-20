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

	public static readonly newmpcli::MsgPack.Serialization.Internal.IObjectSerializer<T, newmpcli::MsgPack.Internal.MessagePackExtensionType> V2 = InitV2();

	public static readonly newmpcli::MsgPack.Serialization.Internal.IObjectSerializer<T, newmpcli::MsgPack.Internal.NullExtensionType> Json = InitJson();

	private static newmpcli::MsgPack.Serialization.Internal.IObjectSerializer<T, newmpcli::MsgPack.Internal.MessagePackExtensionType> InitV2()
	{
		if (typeof(MsgPack.Samples.SampleObject).IsAssignableFrom(typeof(T)))
		{
			return (newmpcli::MsgPack.Serialization.Internal.IObjectSerializer<T, newmpcli::MsgPack.Internal.MessagePackExtensionType>)(object)new MsgPack.Samples.SampleSerializer<newmpcli::MsgPack.Internal.MessagePackExtensionType>();
		}
		else if (typeof(T) == typeof(int[]))
		{
			return (newmpcli::MsgPack.Serialization.Internal.IObjectSerializer<T, newmpcli::MsgPack.Internal.MessagePackExtensionType>)(object)new MsgPack.Samples.SampleInt32ArraySerializer<newmpcli::MsgPack.Internal.MessagePackExtensionType>();
		}
		else if (typeof(T) == typeof(int))
		{
			return (newmpcli::MsgPack.Serialization.Internal.IObjectSerializer<T, newmpcli::MsgPack.Internal.MessagePackExtensionType>)(object)new MsgPack.Samples.SampleInt32Serializer<newmpcli::MsgPack.Internal.MessagePackExtensionType>();
		}

		throw new NotSupportedException($"No {typeof(T)} serializer.");
	}

	private static newmpcli::MsgPack.Serialization.Internal.IObjectSerializer<T, newmpcli::MsgPack.Internal.NullExtensionType> InitJson()
	{
		if (typeof(MsgPack.Samples.SampleObject).IsAssignableFrom(typeof(T)))
		{
			return (newmpcli::MsgPack.Serialization.Internal.IObjectSerializer<T, newmpcli::MsgPack.Internal.NullExtensionType>)(object)new MsgPack.Samples.SampleSerializer<newmpcli::MsgPack.Internal.NullExtensionType>();
		}
		else if (typeof(T) == typeof(int[]))
		{
			return (newmpcli::MsgPack.Serialization.Internal.IObjectSerializer<T, newmpcli::MsgPack.Internal.NullExtensionType>)(object)new MsgPack.Samples.SampleInt32ArraySerializer<newmpcli::MsgPack.Internal.NullExtensionType>();
		}
		else if (typeof(T) == typeof(int))
		{
			return (newmpcli::MsgPack.Serialization.Internal.IObjectSerializer<T, newmpcli::MsgPack.Internal.NullExtensionType>)(object)new MsgPack.Samples.SampleInt32Serializer<newmpcli::MsgPack.Internal.NullExtensionType>();
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
			new newmpcli::MsgPack.Serialization.Internal.DeserializationOperationContext<newmpcli::MsgPack.Internal.MessagePackExtensionType>(
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
			new newmpcli::MsgPack.Serialization.Internal.SerializationOperationContext<newmpcli::MsgPack.Internal.MessagePackExtensionType>(
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
			new newmpcli::MsgPack.Serialization.Internal.DeserializationOperationContext<newmpcli::MsgPack.Internal.NullExtensionType>(
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
			new newmpcli::MsgPack.Serialization.Internal.SerializationOperationContext<newmpcli::MsgPack.Internal.NullExtensionType>(
				Encoder,
				null,
				CancellationToken.None
			);
		MsgPackCliSerializerRepository<T>.Json.Serialize(ref context, input, writer);
		return writer.WrittenMemory.ToArray();
	}
}

