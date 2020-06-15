// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using MsgPack.Internal;
using MsgPack.Serialization.Internal;

namespace MsgPack.Samples
{
	public class SampleObject
	{
		public int Age { get; set; }
		public string Name { get; set; } = null!;
		public bool IsActive { get; set; }
		public IDictionary<string, string> Attributes { get; } = new Dictionary<string, string>();
		public IList<string> Roles { get; } = new List<string>();
	}

	// MP.Core.Abstraction
	// MP.Core / MP.Json.Core / MP.Yaml.Core / MP.Coff.Core
	// MP.Serialization.Core -> Interfaces, ....
	// MP.Serialization -> Basic serializers, context, utilities, Stream adapter, byte[] adapter, etc.
	// MP.Serialization.Serializers-> Extended serializers implementation.
	// MP.Serialization.Reflection -> Reflection
	// MP.Serialization.ILGeneration -> Reflection
	// MP.Serialization.SourceGeneration -> Reflection
	// MP.Extensions  / MP.Json.Extensions / MP.Yaml.Extensions / MP.Coff.Extensions -> MPO, DOM, etc
	// MP.Compatibility1 -> v1 compatibility shims
	// MP.Cli -> v1 compatibility shims

	// Packer/Unpacker compatibility note:
	// Packer -> Packer.Create(new MemoryStream())
	// Unpacker -> Unpacker.Create(new MemoryStream())???

	// For PoC of MVP and reference for emit.
	/// <summary>
	///		Sample hand made serializer.
	/// </summary>
	public sealed class SampleSerializer<TExtensionType> : IObjectSerializer<SampleObject, TExtensionType>
	{
		private bool UseArray { get; set; }

		public void Serialize(in SerializationOperationContext<TExtensionType> context, SampleObject obj, IBufferWriter<byte> writer)
		{
			var encoder = context.Encoder;
			if (obj == null)
			{
				encoder.EncodeNull(writer);
				return;
			}

			if (this.UseArray)
			{
				encoder.EncodeArrayStart(5, writer, context.CollectionContext); // OMITTABLE
				encoder.EncodeString(obj.Name, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
				encoder.EncodeInt32(obj.Age, writer);
				encoder.EncodeBoolean(obj.IsActive, writer);
				if (obj.Attributes == null)
				{
					encoder.EncodeNull(writer);
				}
				else
				{
					encoder.EncodeMapStart(obj.Attributes.Count, writer, context.CollectionContext); // OMITTABLE
					var i = 0; // OMITTABLE
					foreach (var entry in obj.Attributes)
					{
						encoder.EncodeMapKeyStart(i, writer, context.CollectionContext); // OMITTABLE
						encoder.EncodeString(entry.Key, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
						encoder.EncodeMapKeyEnd(i, writer, context.CollectionContext); // OMITTABLE
						encoder.EncodeMapValueStart(i, writer, context.CollectionContext); // OMITTABLE
						encoder.EncodeString(entry.Value, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
						encoder.EncodeMapValueEnd(i, writer, context.CollectionContext); // OMITTABLE
					}
					encoder.EncodeMapEnd(obj.Attributes.Count, writer, context.CollectionContext);
				}
				if (obj.Roles == null)
				{
					encoder.EncodeNull(writer);
				}
				else
				{
					encoder.EncodeArrayStart(obj.Roles.Count, writer, context.CollectionContext); // OMITTABLE
					var i = 0; // OMITTABLE
					foreach (var item in obj.Roles)
					{
						encoder.EncodeArrayItemStart(i, writer, context.CollectionContext); // OMITTABLE
						encoder.EncodeString(item, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
						encoder.EncodeArrayItemEnd(i, writer, context.CollectionContext); // OMITTABLE
					}
					encoder.EncodeArrayEnd(obj.Roles.Count, writer, context.CollectionContext);
				}
				encoder.EncodeArrayEnd(5, writer, context.CollectionContext); // OMITTABLE
			}
			else
			{
				encoder.EncodeMapStart(5, writer, context.CollectionContext); // OMITTABLE
				ReadOnlySpan<byte> key0 = new[] { (byte)'N', (byte)'a', (byte)'m', (byte)'e' }; // static readonly
				encoder.EncodeString(key0, 4, writer, context.CancellationToken);
				encoder.EncodeString(obj.Name, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
				ReadOnlySpan<byte> key1 = new[] { (byte)'A', (byte)'g', (byte)'e' }; // static readonly
				encoder.EncodeString(key1, 3, writer, context.CancellationToken);
				encoder.EncodeInt32(obj.Age, writer);
				ReadOnlySpan<byte> key2 = new[] { (byte)'I', (byte)'s', (byte)'a', (byte)'c', (byte)'t', (byte)'i', (byte)'v', (byte)'e' }; // static readonly
				encoder.EncodeString(key2, 8, writer, context.CancellationToken);
				encoder.EncodeBoolean(obj.IsActive, writer);
				ReadOnlySpan<byte> key3 = new[] { (byte)'A', (byte)'t', (byte)'t', (byte)'r', (byte)'i', (byte)'b', (byte)'u', (byte)'t', (byte)'e', (byte)'s' }; // static readonly
				encoder.EncodeString(key3, 10, writer, context.CancellationToken);
				if (obj.Attributes == null)
				{
					encoder.EncodeNull(writer);
				}
				else
				{
					encoder.EncodeMapStart(obj.Attributes.Count, writer, context.CollectionContext); // OMITTABLE
					var i = 0; // OMITTABLE
					foreach (var entry in obj.Attributes)
					{
						encoder.EncodeMapKeyStart(i, writer, context.CollectionContext); // OMITTABLE
						encoder.EncodeString(entry.Key, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
						encoder.EncodeMapKeyEnd(i, writer, context.CollectionContext); // OMITTABLE
						encoder.EncodeMapValueStart(i, writer, context.CollectionContext); // OMITTABLE
						encoder.EncodeString(entry.Value, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
						encoder.EncodeMapValueEnd(i, writer, context.CollectionContext); // OMITTABLE
					}
					encoder.EncodeMapEnd(obj.Attributes.Count, writer, context.CollectionContext);
				}
				ReadOnlySpan<byte> key4 = new[] { (byte)'R', (byte)'o', (byte)'l', (byte)'e' }; // static readonly
				encoder.EncodeString(key4, 4, writer, context.CancellationToken);
				if (obj.Roles == null)
				{
					encoder.EncodeNull(writer);
				}
				else
				{
					encoder.EncodeArrayStart(obj.Roles.Count, writer, context.CollectionContext); // OMITTABLE
					var i = 0; // OMITTABLE
					foreach (var item in obj.Roles)
					{
						encoder.EncodeArrayItemStart(i, writer, context.CollectionContext); // OMITTABLE
						encoder.EncodeString(item, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
						encoder.EncodeArrayItemEnd(i, writer, context.CollectionContext); // OMITTABLE
					}
					encoder.EncodeArrayEnd(obj.Roles.Count, writer, context.CollectionContext);
				}
				encoder.EncodeMapEnd(5, writer, context.CollectionContext); // OMITTABLE
			}
		}

		public async ValueTask SerializeAsync(SerializationOperationContext<TExtensionType> context, SampleObject obj, Stream streamSink)
		{
			await using (var writer = new StreamBufferWriter(streamSink, ownsStream: false, ArrayPool<byte>.Shared, cleansBuffer: true))
			{
				this.Serialize(context, obj, writer);
			}
		}

		private static readonly MsgPackStringTrie<uint> DeserializationTrie = InitializeDeserializationTrie();
		private static MsgPackStringTrie<uint> InitializeDeserializationTrie()
		{
			ReadOnlySpan<byte> __name = new byte[] { 0xA4, (byte)'N', (byte)'a', (byte)'m', (byte)'e' };
			ReadOnlySpan<byte> __age = new byte[] { 0xA3, (byte)'A', (byte)'g', (byte)'e' };
			ReadOnlySpan<byte> __isActive = new byte[] { 0xAA, (byte)'i', (byte)'s', (byte)'A', (byte)'c', (byte)'t', (byte)'i', (byte)'v', (byte)'e' };
			ReadOnlySpan<byte> __roles = new byte[] { 0xA4, (byte)'N', (byte)'a', (byte)'m', (byte)'e', };
			ReadOnlySpan<byte> __attributes = new byte[] { 0xAC, (byte)'A', (byte)'t', (byte)'t', (byte)'r', (byte)'i', (byte)'b', (byte)'u', (byte)'t', (byte)'e', (byte)'s' };
			var trie = new MsgPackStringTrie<uint>(5);
			trie.TryAdd(__name, 0);
			trie.TryAdd(__age, 1);
			trie.TryAdd(__isActive, 2);
			trie.TryAdd(__roles, 3);
			trie.TryAdd(__attributes, 4);
			return trie;
		}

		public SampleObject Deserialize(in DeserializationOperationContext<TExtensionType> context, in SequenceReader<byte> reader)
		{
			// T が参照型でデフォルトコンストラクターがある -> DeserializeTo
			var obj = new SampleObject();
			this.DeserializeTo(context, reader, obj);
			return obj;

			// T にデフォルトコンストラクターがない -> from DeserializeTo と同じ処理をインライン実装
			// ....
			// return new SampleObject(name, age, isActive, roles, attributes);

			// T がミュータブルな値型
			// ...
			// var obj = default;
			// obj.Name = name;
			// obj.Age = age;
			// obj.IsActive = isActive;
			// obj.Roles = roles; // Roles は絶対に null のはず。
			// obj.Attributes = attributes; // Attributes は絶対に null のはず。
			// return obj;
		}

		public async ValueTask<SampleObject> DeserializeAsync(DeserializationOperationContext<TExtensionType> context, Stream streamSource)
		{
			// T が参照型でデフォルトコンストラクターがある -> DeserializeToAsync
			var obj = new SampleObject();
			await this.DeserializeToAsync(context, streamSource, obj).ConfigureAwait(false);
			return obj;

			// T にデフォルトコンストラクターがない -> DeserializeToAsync と同じ処理をインライン実装
			// ....
			// return new SampleObject(name, age, isActive, roles, attributes);

			// T がミュータブルな値型
			// ...
			// var obj = default;
			// obj.Name = name;
			// obj.Age = age;
			// obj.IsActive = isActive;
			// obj.Roles = roles; // Roles は絶対に null のはず。
			// obj.Attributes = attributes; // Attributes は絶対に null のはず。
			// return obj;
		}

		public void DeserializeTo(in DeserializationOperationContext<TExtensionType> context, in SequenceReader<byte> reader, in SampleObject obj)
		{
			string name = default!;
			int age = default;
			bool isActive = default;
			var roles = obj.Roles;
			var attributes = obj.Attributes;

			var decoder = context.Decoder;
			CollectionType arrayOrMap;
			long itemsCount;
			CollectionItemIterator propertyIterator;

			if (decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
			{
				arrayOrMap = decoder.DecodeArrayOrMapHeader(reader, out itemsCount);
				if (itemsCount < 5)
				{
					throw new MessageTypeException(); // Use Throws
				}

				propertyIterator = default;
			}
			else
			{
				arrayOrMap = decoder.DecodeArrayOrMap(reader, out propertyIterator);
				itemsCount = -1;
			}

			context.IncrementDepth();

			// If !SerializerGenerationOptions.InfersObjectSerialization && SerializerGenerationOptions.UseArray || typeof(T).IsDefined([SerializeAs(Array)])
			if (arrayOrMap.IsArray)
			{
				if (!decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
				{
					CheckNextItemExists(reader, ref propertyIterator);
				}

				name = decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding

				if (!decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
				{
					CheckNextItemExists(reader, ref propertyIterator);
				}

				age = decoder.DecodeInt32(reader);

				if (!decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
				{
					CheckNextItemExists(reader, ref propertyIterator);
				}

				isActive = decoder.DecodeBoolean(reader);

				if (!decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
				{
					CheckNextItemExists(reader, ref propertyIterator);
				}

				context.IncrementDepth();
				if (decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
				{
					var arrayLength = decoder.DecodeArrayHeader(reader);
					// If settable
					//if (roles == null)
					//{
					//	roles = new List<string>(arrayLength); // or 0
					//}

					for (var i = 0; i < arrayLength; i++)
					{
						roles.Add(decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken)); // <>NameEncoding ?? context.StringEncoding
					}
				}
				else
				{
					var iterator = decoder.DecodeArray(reader);
					while (!iterator.CollectionEnds(reader))
					{
						roles.Add(decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken)); // <>NameEncoding ?? context.StringEncoding
					}
					iterator.Drain(reader); 
				}
				context.DecrementDepth();

				if (!decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
				{
					CheckNextItemExists(reader, ref propertyIterator);
				}

				context.IncrementDepth();
				if (decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
				{
					var mapCount = decoder.DecodeMapHeader(reader);
					// If settable
					//if (attributes == null)
					//{
					//	attributes = new Dictionary<string, string>(mapCount); // or 0
					//}
					for (var i = 0; i < mapCount; i++)
					{
						attributes.Add(
							decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken), // <>NameEncoding ?? context.StringEncoding
							decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken) // <>NameEncoding ?? context.StringEncoding
						);
					}
				}
				else
				{
					var iterator = decoder.DecodeMap(reader);
					while (!iterator.CollectionEnds(reader))
					{
						attributes.Add(
							decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken), // <>NameEncoding ?? context.StringEncoding
							decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken) // <>NameEncoding ?? context.StringEncoding
						);
					}
					iterator.Drain(reader);
				}
				context.DecrementDepth();
			}
			else
			{
				// Map

				// if !decoder.FormatFeatures.CanCountCollectionItems // OPTIMIZABLE
				// while(!propertyIterator.CollectionEnds(reader))
				for (var i = 0; i < itemsCount; i++)
				{
					decoder.GetRawString(reader, out ReadOnlySpan<byte> key, context.CancellationToken);

					context.ValidatePropertyKeyLength(reader.Consumed, key.Length);

					// Use inlined trie, prefixed by the count as MP uint32.
					// char is UTF-8, big endian.
					// 1st node: [length(1-5)][chars(7-3)]
					switch (DeserializationTrie.GetOrDefault(key))
					{
						case 0:
						{
							name = decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
							break;
						}
						case 1:
						{
							age = decoder.DecodeInt32(reader);
							break;
						}
						case 2:
						{
							isActive = decoder.DecodeBoolean(reader);
							break;
						}
						case 3:
						{
							context.IncrementDepth();
							if (decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
							{
								var arrayLength = decoder.DecodeArrayHeader(reader);
								// If settable
								//if (roles == null)
								//{
								//	roles = new List<string>(arrayLength); // or 0
								//}
								for (var j = 0; j < arrayLength; j++)
								{
									roles.Add(decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken)); // <>NameEncoding ?? context.StringEncoding
								}
							}
							else
							{
								var iterator = decoder.DecodeArray(reader);
								while (!iterator.CollectionEnds(reader))
								{
									roles.Add(decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken)); // <>NameEncoding ?? context.StringEncoding
								}
								iterator.Drain(reader);
							}
							context.DecrementDepth();
							break;
						}
						case 4:
						{
							context.IncrementDepth();
							if (decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
							{
								var mapCount = decoder.DecodeMapHeader(reader);
								// If settable
								//if (attributes == null)
								//{
								//	attributes = new Dictionary<string, string>(mapCount); // or 0
								//}
								for (var j = 0; j < mapCount; j++)
								{
									attributes.Add(
										decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken), // <>NameEncoding ?? context.StringEncoding
										decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken) // <>NameEncoding ?? context.StringEncoding
									);
								}
							}
							else
							{
								var iterator = decoder.DecodeMap(reader);
								while (!iterator.CollectionEnds(reader))
								{
									attributes.Add(
										decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken), // <>NameEncoding ?? context.StringEncoding
										decoder.DecodeString(reader, context.StringEncoding, context.CancellationToken) // <>NameEncoding ?? context.StringEncoding
									);
								}
								iterator.Drain(reader);
							}
							context.DecrementDepth();
							break;
						}
					}
				}
			}

			if (decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
			{
				decoder.Drain(reader, context.CollectionContext, itemsCount - 5, context.CancellationToken);
			}
			else
			{
				propertyIterator.Drain(reader);
			}

			context.DecrementDepth();

			obj.Name = name;
			obj.Age = age;
			obj.IsActive = isActive;
			// If settable
			// obj.Roles = roles;
			// If settable
			// obj.Attributes = attributes;
		}

		private static void CheckNextItemExists(SequenceReader<byte> reader, ref CollectionItemIterator propertyIterator)
		{
			if (!propertyIterator.CollectionEnds(reader))
			{
				throw new MessageTypeException(); // Use Throws
			}
		}

		private static bool TryDecodeArrayOrMapHeader(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, out long itemsCount, out CollectionType arrayOrMap, out CollectionItemIterator propertyIterator, out int requestHint)
		{
			context.IncrementDepth();
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));

			arrayOrMap = context.Decoder.DecodeArrayOrMapHeader(reader, out itemsCount, out requestHint);
			if (arrayOrMap.IsNone)
			{
				propertyIterator = default;
				return false;
			}

			if (context.Decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
			{
				arrayOrMap = context.Decoder.DecodeArrayOrMapHeader(reader, out itemsCount);
				if (itemsCount < 5)
				{
					throw new MessageTypeException(); // Use Throws
				}

				propertyIterator = default;
			}
			else
			{
				arrayOrMap = context.Decoder.DecodeArrayOrMap(reader, out propertyIterator);
				itemsCount = -1;
			}

			memory = memory.Slice(unchecked((int)reader.Consumed));

			return true;
		}

		private static bool TryDecodeArrayHeader(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, out long arrayLength, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));

			arrayLength = context.Decoder.DecodeArrayHeader(reader, out requestHint);
			if (requestHint == 0)
			{
				memory = memory.Slice(unchecked((int)reader.Consumed));
			}
			return requestHint == 0;
		}

		private static bool TryDecodeMapHeader(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, out long mapCount, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));

			mapCount = context.Decoder.DecodeMapHeader(reader, out requestHint);
			if (requestHint == 0)
			{
				memory = memory.Slice(unchecked((int)reader.Consumed));
			}
			return requestHint == 0;
		}

		private static bool TryDecodeValueOfName(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, out string name, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));

			name = context.Decoder.DecodeString(reader, out requestHint, context.StringEncoding, context.CancellationToken)!; // <>NameEncoding ?? context.StringEncoding
			if (requestHint == 0)
			{
				memory = memory.Slice(unchecked((int)reader.Consumed));
			}
			return requestHint == 0;
		}

		private static bool TryDecodeValueOfAge(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, out int age, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));

			age = context.Decoder.DecodeInt32(reader, out requestHint);
			if (requestHint == 0)
			{
				memory = memory.Slice(unchecked((int)reader.Consumed));
			}
			return requestHint == 0;
		}

		private static bool TryDecodeValueOfIsActive(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, out bool isActive, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));

			isActive = context.Decoder.DecodeBoolean(reader, out requestHint);
			if (requestHint == 0)
			{
				memory = memory.Slice(unchecked((int)reader.Consumed));
			}
			return requestHint == 0;
		}

		private static bool TryDecodeItemOfRoles(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, out string item, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));

			item = context.Decoder.DecodeString(reader, out requestHint, context.StringEncoding, context.CancellationToken)!; // <>NameEncoding ?? context.StringEncoding
			if (requestHint == 0)
			{
				memory = memory.Slice(unchecked((int)reader.Consumed));
			}
			return requestHint == 0;
		}

		private static bool TryDecodeKeyOfAttributes(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, out string key, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));

			key = context.Decoder.DecodeString(reader, out requestHint, context.StringEncoding, context.CancellationToken)!; // <>NameEncoding ?? context.StringEncoding
			if (requestHint == 0)
			{
				memory = memory.Slice(unchecked((int)reader.Consumed));
			}
			return requestHint == 0;
		}

		private static bool TryDecodeValueOfAttributes(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, out string value, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));

			value = context.Decoder.DecodeString(reader, out requestHint, context.StringEncoding, context.CancellationToken)!; // <>NameEncoding ?? context.StringEncoding
			if (requestHint == 0)
			{
				memory = memory.Slice(unchecked((int)reader.Consumed));
			}
			return requestHint == 0;
		}

		private static bool TryDecodeArray(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, out CollectionItemIterator iterator, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));

			iterator = context.Decoder.DecodeArray(reader, out requestHint);
			if (requestHint == 0)
			{
				memory = memory.Slice(unchecked((int)reader.Consumed));
			}
			return requestHint == 0;
		}

		private static bool TryDecodeMap(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, out CollectionItemIterator iterator, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));

			iterator = context.Decoder.DecodeMap(reader, out requestHint);
			if (requestHint == 0)
			{
				memory = memory.Slice(unchecked((int)reader.Consumed));
			}
			return requestHint == 0;
		}

		private static bool TryGetRawString(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, [NotNullWhen(true)]out byte[] key, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));

			ReadOnlySpan<byte> span;
			if (!context.Decoder.GetRawString(reader, out span, out requestHint, context.CancellationToken))
			{
				key = default!;
				return false;
			}

			key = context.ArrayPool.Rent(span.Length);
			span.CopyTo(key);
			return true;
		}

		private static bool TryDrain(in DeserializationOperationContext<TExtensionType> context, ref ReadOnlyMemory<byte> memory, long remaining, out int requestHint)
		{
			var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>(memory));
			context.Decoder.Drain(reader, context.CollectionContext, remaining, out requestHint);
			if (requestHint == 0)
			{
				memory = memory.Slice(unchecked((int)reader.Consumed));
			}
			return requestHint == 0;
		}

		private bool TryCheckNextItemExists(ref CollectionItemIterator propertyIterator, ReadOnlyMemory<byte> memory, out int requestHint)
		{
			if (propertyIterator.CollectionEnds(memory, out requestHint))
			{
				throw new MessageTypeException(); // use throws
			}

			return requestHint == 0;
		}

		public async ValueTask DeserializeToAsync(DeserializationOperationContext<TExtensionType> context, Stream streamSource, SampleObject obj)
		{
			// T が値型
			// throw new NotSupportedException();

			var buffer = context.ArrayPool.Rent(2 * 1024 * 1024);
			try
			{
				var provider = new StreamReadOnlyMemoryProvider(streamSource, buffer);
				var memory = await provider.GetNextAsync(default, 0, context.CancellationToken).ConfigureAwait(false);
				int requestHint;

				string name = default!;
				int age = default;
				bool isActive = default;
				var roles = obj.Roles;
				var attributes = obj.Attributes;

				var decoder = context.Decoder;
				context.IncrementDepth();

				long itemsCount;
				CollectionType arrayOrMap;
				CollectionItemIterator propertyIterator;
				while (!TryDecodeArrayOrMapHeader(context, ref memory, out itemsCount, out arrayOrMap, out propertyIterator, out requestHint))
				{
					memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
				}

				// If !SerializerGenerationOptions.InfersObjectSerialization && SerializerGenerationOptions.UseArray || typeof(T).IsDefined([SerializeAs(Array)])
				if (arrayOrMap.IsArray)
				{
					if (!context.Decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
					{
						while (!this.TryCheckNextItemExists(ref propertyIterator, memory, out requestHint))
						{
							memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
						}
					}

					while (!TryDecodeValueOfName(context, ref memory, out name, out requestHint))
					{
						memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
					}

					if (!context.Decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
					{
						while (!this.TryCheckNextItemExists(ref propertyIterator, memory, out requestHint))
						{
							memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
						}
					}

					while (!TryDecodeValueOfAge(context, ref memory, out age, out requestHint))
					{
						memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
					}

					if (!context.Decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
					{
						while (!this.TryCheckNextItemExists(ref propertyIterator, memory, out requestHint))
						{
							memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
						}
					}

					while (!TryDecodeValueOfIsActive(context, ref memory, out isActive, out requestHint))
					{
						memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
					}

					if (!context.Decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
					{
						while (!this.TryCheckNextItemExists(ref propertyIterator, memory, out requestHint))
						{
							memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
						}
					}

					context.IncrementDepth();

					if (decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
					{
						long arrayLength;
						while (!TryDecodeArrayHeader(context, ref memory, out arrayLength, out requestHint))
						{
							memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
						}

						// If settable
						//if (roles == null)
						//{
						//	roles = new List<string>(arrayLength); // or 0
						//}

						for (var i = 0; i < arrayLength; i++)
						{
							string item;
							while (!TryDecodeItemOfRoles(context, ref memory, out item, out requestHint))
							{
								memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
							}
							roles.Add(item);
						}
					}
					else
					{
						CollectionItemIterator iterator;
						while (!TryDecodeArray(context, ref memory, out iterator, out requestHint))
						{
							memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
						}

						while (!iterator.CollectionEnds(memory, out requestHint))
						{
							if (requestHint != 0)
							{
								memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
							}

							string item;
							while (!TryDecodeItemOfRoles(context, ref memory, out item, out requestHint))
							{
								memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
							}
							roles.Add(item);
						}
						
						while(!iterator.Drain(ref memory, out requestHint))
						{
							memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
						}
					}
					context.DecrementDepth();

					if (!context.Decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
					{
						while (!this.TryCheckNextItemExists(ref propertyIterator, memory, out requestHint))
						{
							memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
						}
					}

					context.IncrementDepth();
					if (decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
					{
						long mapCount;
						while (!TryDecodeMapHeader(context, ref memory, out mapCount, out requestHint))
						{
							memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
						}

						// If settable
						//if (attributes == null)
						//{
						//	attributes = new Dictionary<string, string>(mapCount); // or 0
						//}
						for (var i = 0; i < mapCount; i++)
						{
							string key;
							while (!TryDecodeKeyOfAttributes(context, ref memory, out key, out requestHint))
							{
								memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
							}
							string value;
							while (!TryDecodeValueOfAttributes(context, ref memory, out value, out requestHint))
							{
								memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
							}
							attributes.Add(key, value);
						}
					}
					else
					{
						CollectionItemIterator iterator;
						while (!TryDecodeMap(context, ref memory, out iterator, out requestHint))
						{
							memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
						}

						while (!iterator.CollectionEnds(memory, out requestHint))
						{
							if (requestHint != 0)
							{
								memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
							}

							string key;
							while (!TryDecodeKeyOfAttributes(context, ref memory, out key, out requestHint))
							{
								memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
							}
							string value;
							while (!TryDecodeValueOfAttributes(context, ref memory, out value, out requestHint))
							{
								memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
							}
							attributes.Add(key, value);
						}

						while (!iterator.Drain(ref memory, out requestHint))
						{
							memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
						}
					}
					context.DecrementDepth();
				}
				else
				{
					// Map
					// #if !context.Decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
					// while (!propertyIterator.CollectionEnds(memory, out requestHint))
					// {
					//  	if (requestHint != 0)
					//  	{
					//  		memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
					//			continue;
					//  	}
					for (var i = 0; i < itemsCount; i++)
					{
						byte[] propertyKey = null!;
						try
						{
							while (!TryGetRawString(context, ref memory, out propertyKey, out requestHint))
							{
								memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
							}

							// Use inlined trie, prefixed by the count as MP uint32.
							// char is UTF-8, big endian.
							// 1st node: [length(1-5)][chars(7-3)]
							switch (DeserializationTrie.GetOrDefault(propertyKey))
							{
								case 0:
								{
									while (!TryDecodeValueOfName(context, ref memory, out name, out requestHint))
									{
										memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
									}

									break;
								}
								case 1:
								{
									while (!TryDecodeValueOfAge(context, ref memory, out age, out requestHint))
									{
										memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
									}

									break;
								}
								case 2:
								{
									while (!TryDecodeValueOfIsActive(context, ref memory, out isActive, out requestHint))
									{
										memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
									}

									break;
								}
								case 3:
								{
									context.IncrementDepth();
									if (decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
									{
										long arrayLength;
										while (!TryDecodeArrayHeader(context, ref memory, out arrayLength, out requestHint))
										{
											memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
										}

										// If settable
										//if (roles == null)
										//{
										//	roles = new List<string>(arrayLength); // or 0
										//}
										for (var j = 0; j < arrayLength; j++)
										{
											string item;
											while (!TryDecodeItemOfRoles(context, ref memory, out item, out requestHint))
											{
												memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
											}

											roles.Add(item);
										}
									}
									else
									{
										CollectionItemIterator iterator;
										while (!TryDecodeArray(context, ref memory, out iterator, out requestHint))
										{
											memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
										}

										while (!iterator.CollectionEnds(memory, out requestHint))
										{
											if (requestHint != 0)
											{
												memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
											}

											string item;
											while (!TryDecodeItemOfRoles(context, ref memory, out item, out requestHint))
											{
												memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
											}

											roles.Add(item);
										}

										while (!iterator.Drain(ref memory, out requestHint))
										{
											memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
										}
									}
									context.DecrementDepth();
									break;
								}
								case 4:
								{
									context.IncrementDepth();
									if (decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
									{
										long mapCount;
										while (!TryDecodeMapHeader(context, ref memory, out mapCount, out requestHint))
										{
											memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
										}

										// If settable
										//if (attributes == null)
										//{
										//	attributes = new Dictionary<string, string>(mapCount); // or 0
										//}
										for (var j = 0; j < mapCount; j++)
										{
											string key;
											while (!TryDecodeKeyOfAttributes(context, ref memory, out key, out requestHint))
											{
												memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
											}
											string value;
											while (!TryDecodeValueOfAttributes(context, ref memory, out value, out requestHint))
											{
												memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
											}

											attributes.Add(key, value);
										}
									}
									else
									{
										CollectionItemIterator iterator;
										while (!TryDecodeMap(context, ref memory, out iterator, out requestHint))
										{
											memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
										}

										while (!iterator.CollectionEnds(memory, out requestHint))
										{
											if (requestHint != 0)
											{
												memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
											}

											string key;
											while (!TryDecodeKeyOfAttributes(context, ref memory, out key, out requestHint))
											{
												memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
											}
											string value;
											while (!TryDecodeValueOfAttributes(context, ref memory, out value, out requestHint))
											{
												memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
											}

											attributes.Add(key, value);
										}

										while (!iterator.Drain(ref memory, out requestHint))
										{
											memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
										}
									}
									context.DecrementDepth();
									break;
								}
							}
						}
						finally
						{
							if (propertyKey != null)
							{
								context.ArrayPool.Return(propertyKey);
							}
						}
					}
				}

				if (context.Decoder.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
				{
					itemsCount -= 5;
					while (!TryDrain(context, ref memory, itemsCount, out requestHint))
					{
						memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
					}
				}
				else
				{
					while(!propertyIterator.Drain(ref memory, out requestHint))
					{
						memory = await provider.GetNextAsync(memory, requestHint, context.CancellationToken).ConfigureAwait(false);
					}
				}

				context.DecrementDepth();

				obj.Name = name;
				obj.Age = age;
				obj.IsActive = isActive;
				// If settable
				// obj.Roles = roles;
				// If settable
				// obj.Attributes = attributes;
			}
			finally
			{
				context.ArrayPool.Return(buffer, clearArray: true);
			}
		}
	}

	public sealed class SampleInt32ArraySerializer<TExtensionType> : IObjectSerializer<int[], TExtensionType>
	{
		public void Serialize(in SerializationOperationContext<TExtensionType> context, int[] obj, IBufferWriter<byte> sink)
		{
			if (obj is null)
			{
				context.Encoder.EncodeNull(sink);
				return;
			}

			context.Encoder.EncodeArrayStart(obj.Length, sink, context.CollectionContext);
			for(var i=0; i <obj.Length;i++)
			{
				context.Encoder.EncodeArrayItemStart(i, sink, context.CollectionContext);
				context.Encoder.EncodeInt32(obj[i], sink);
				context.Encoder.EncodeArrayItemEnd(i, sink, context.CollectionContext);
			}
			context.Encoder.EncodeArrayEnd(obj.Length, sink, context.CollectionContext);
		}

		public ValueTask SerializeAsync(SerializationOperationContext<TExtensionType> context, int[] obj, Stream streamSink)
		{
			throw new NotImplementedException();
		}

		public int[] Deserialize(in DeserializationOperationContext<TExtensionType> context, in SequenceReader<byte> source)
		{
			if (context.Decoder.FormatFeatures.CanCountCollectionItems)
			{
				var length = context.Decoder.DecodeArrayHeader(source);
				var result = new int[length];
				for (var i = 0; i < result.Length; i++)
				{
					result[i] = context.Decoder.DecodeInt32(source);
				}
				context.Decoder.Drain(source, context.CollectionContext, 0);
				return result;
			}
			else
			{
				var result = new List<int>();
				var iterator = context.Decoder.DecodeArray(source);
				while (!iterator.CollectionEnds(source))
				{
					result.Add(context.Decoder.DecodeInt32(source));
				}
				iterator.Drain(source);
				return result.ToArray();
			}
		}

		public ValueTask<int[]> DeserializeAsync(DeserializationOperationContext<TExtensionType> context, Stream streamSource)
		{
			throw new NotImplementedException();
		}

		public void DeserializeTo(in DeserializationOperationContext<TExtensionType> context, in SequenceReader<byte> source, in int[] obj)
		{
			throw new NotImplementedException();
		}

		public ValueTask DeserializeToAsync(DeserializationOperationContext<TExtensionType> context, Stream streamSource, int[] obj)
		{
			throw new NotImplementedException();
		}
	}

	public sealed class SampleInt32Serializer<TExtensionType> : IObjectSerializer<int, TExtensionType>
	{
		public void Serialize(in SerializationOperationContext<TExtensionType> context, int obj, IBufferWriter<byte> sink)
		{
			context.Encoder.EncodeInt32(obj, sink);
		}

		public ValueTask SerializeAsync(SerializationOperationContext<TExtensionType> context, int obj, Stream streamSink)
		{
			throw new NotImplementedException();
		}

		public int Deserialize(in DeserializationOperationContext<TExtensionType> context, in SequenceReader<byte> source)
		{
			return context.Decoder.DecodeInt32(source);
		}

		public ValueTask<int> DeserializeAsync(DeserializationOperationContext<TExtensionType> context, Stream streamSource)
		{
			throw new NotImplementedException();
		}

		public void DeserializeTo(in DeserializationOperationContext<TExtensionType> context, in SequenceReader<byte> source, in int obj)
		{
			throw new NotImplementedException();
		}

		public ValueTask DeserializeToAsync(DeserializationOperationContext<TExtensionType> context, Stream streamSource, int obj)
		{
			throw new NotImplementedException();
		}
	}
}
