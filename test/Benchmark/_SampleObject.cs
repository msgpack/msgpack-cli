// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

//#define USE_ARRAY
#define INT32
#define BOOL
#define STRING
#define COLLECTION
#define NO_OPT
//#define NULL
//#define INLINE_TRIE
#define JSON

extern alias newmpcli;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using newmpcli::MsgPack.Serialization;
using newmpcli::MsgPack.Internal;
using newmpcli::MsgPack.Json;

namespace MsgPack.Samples
{
	public delegate ulong TrieKeyFactory(ref ReadOnlySpan<byte> rawPropertyKey);

	public sealed class SerializationFormat
	{
		public Type DecoderType { get; set; } // if null, no cast will be used
		public Type EncoderType { get; set; } // if null, no cast will be used
		public FormatFeatures Features { get; set; } // if null, no optimization will be applied
		public Func<string, byte[]> PropertyKeyEncoder { get; set; } // if null, use EncodeString
		public TrieKeyFactory TrieKeyHeadFactory { get; set; } // if null, use Dictionary<string, int>
		public TrieKeyFactory TrieKeyRestFactory { get; set; } // if null, use Dictionary<string, int>
	}

	public sealed class SerializationFormatFactory
	{
		public static SerializationFormatFactory Instance { get; } = new SerializationFormatFactory();
		private SerializationFormatFactory() { }
	}

	public static class MessagePackSerializationFormatFactoryExtensions
	{
		private static readonly SerializationFormat CurrentMessagePackSerializationFormat =
			new SerializationFormat
			{
				DecoderType = typeof(MessagePackDecoder),
				EncoderType = typeof(MessagePackEncoder), // typeof(CurrentMessagePackEncoder)
				Features = null, // TODO
				PropertyKeyEncoder = null, // CurrentMessagePackEncoder.InternalEncodeString,
				TrieKeyHeadFactory = MsgPackStringTrieKey.GetRaw64,
				TrieKeyRestFactory = MsgPackStringTrieKey.GetRaw64
			};
		private static readonly SerializationFormat LegacyMessagePackSerializationFormat =
			new SerializationFormat
			{
				DecoderType = typeof(MessagePackDecoder),
				EncoderType = typeof(MessagePackEncoder), // typeof(LegacyMessagePackEncoder)
				Features = null, // TODO
				PropertyKeyEncoder = null, // LegacyMessagePackEncoder.InternalEncodeString,
				TrieKeyHeadFactory = MsgPackStringTrieKey.GetRaw64,
				TrieKeyRestFactory = MsgPackStringTrieKey.GetRaw64
			};

		public static SerializationFormat GetMessagePack(this SerializationFormatFactory factory, bool isCurrent = true)
			=> isCurrent ? CurrentMessagePackSerializationFormat : LegacyMessagePackSerializationFormat;
	}


	public static class JsonSerializationFormatFactoryExtensions
	{
		private static readonly SerializationFormat SimpleJsonSerializationFormat =
			new SerializationFormat
			{
				DecoderType = typeof(JsonDecoder), // typeof(SimpleJsonDecoder)
				EncoderType = typeof(JsonEncoder),
				Features = null, // TODO
				PropertyKeyEncoder = null, // JsonEncoder.InternalEncodeString,
				TrieKeyHeadFactory = MsgPackStringTrieKey.GetAsMsgPackString,
				TrieKeyRestFactory = MsgPackStringTrieKey.GetRaw64
			};
		private static readonly SerializationFormat FlexibleJsonSerializationFormat =
			new SerializationFormat
			{
				DecoderType = typeof(JsonDecoder), // typeof(FlexibleJsonDecoder)
				EncoderType = typeof(JsonEncoder),
				Features = null, // TODO
				PropertyKeyEncoder = null, // JsonEncoder.InternalEncodeString,
				TrieKeyHeadFactory = MsgPackStringTrieKey.GetAsMsgPackString,
				TrieKeyRestFactory = MsgPackStringTrieKey.GetRaw64
			};

		public static SerializationFormat GetJson(this SerializationFormatFactory factory, JsonParseOptions parserOptions)
			=> parserOptions == JsonParseOptions.None ? SimpleJsonSerializationFormat : FlexibleJsonSerializationFormat;
	}

#if USE_ARRAY
	[MessagePack.MessagePackObject(keyAsPropertyName: false)]
#else
	[MessagePack.MessagePackObject(keyAsPropertyName: true)]
#endif
	public class SampleObject
	{
		public static readonly SampleObject TestValue =
#if NULL
			null;
#else
			new SampleObject()
			{
#if INT32
				Age = 38,
#endif
#if STRING
				Name = "yfakariya",
#endif
#if BOOL
				IsActive = true,
#endif
#if COLLECTION
				Attributes = { ["Busy"] = "Sometimes" },
				Roles = { "Developer" }
#endif
			};
#endif // NULL

#if INT32
#if USE_ARRAY
		[MessagePack.Key(0)]
#endif
		public int Age { get; set; }
#endif

#if STRING
#if USE_ARRAY
		[MessagePack.Key(1)]
#endif
		public string Name { get; set; } = null!;
#endif

#if BOOL
#if USE_ARRAY
		[MessagePack.Key(2)]
#endif
		public bool IsActive { get; set; }
#endif

#if COLLECTION
#if USE_ARRAY
		[MessagePack.Key(3)]
#endif
		public IDictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

#if USE_ARRAY
		[MessagePack.Key(4)]
#endif
		public IList<string> Roles { get; } = new List<string>();
#endif
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
	public sealed class SampleSerializer : ObjectSerializer<SampleObject>
	{
		public static readonly MsgPack.Serialization.SerializationContext SerializationContext =
#if USE_ARRAY
			new Serialization.SerializationContext() { SerializationMethod = Serialization.SerializationMethod.Array };
#else
			new Serialization.SerializationContext() { SerializationMethod = Serialization.SerializationMethod.Map };
#endif
		public SampleSerializer(ObjectSerializationContext ownedContext)
			: base(ownedContext, SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize | SerializerCapabilities.DeserializeTo) { }


		private bool UseArray { get; set; }
#if USE_ARRAY
			= true;
#endif
		private FormatFeatures FormatFeatures { get; set; } =
			new FormatFeaturesBuilder
			{
#if MSGPACK
				CanCountCollectionItems = true,
				CanSpecifyStringEncoding = true,
				IsContextful = false,
				SupportsExtensionTypes = true
#else
				CanCountCollectionItems = false,
				CanSpecifyStringEncoding = false,
				IsContextful = false,
				SupportsExtensionTypes = false
#endif
			}.Build();

		public sealed override void Serialize(ref SerializationOperationContext context, SampleObject obj, IBufferWriter<byte> writer)
		{
#if JSON
			var encoder = new JsonEncoder(context.Encoder.Options as JsonEncoderOptions);
#elif NO_OPT
			var encoder = context.Encoder;
#else
			var encoder = context.Encoder;
#endif
			if (obj == null)
			{
				encoder.EncodeNull(writer);
				return;
			}

			const int propertyCount =
					0
#if INT32
					+ 1
#endif
#if STRING
					+ 1
#endif
#if BOOL
					+ 1
#endif
#if COLLECTION
					+ 1
					+ 1
#endif
					;
			//if (this.UseArray)
#if USE_ARRAY
			{
				encoder.EncodeArrayStart(propertyCount, writer, context.CollectionContext); // OMITTABLE
#if STRING
#if NO_OPT
				encoder.EncodeArrayItemStart(0, writer, context.CollectionContext); // OMITTABLE
#endif
				encoder.EncodeString(obj.Name, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding

#if NO_OPT
				encoder.EncodeArrayItemEnd(0, writer, context.CollectionContext); // OMITTABLE
#endif
#endif
#if INT32
#if NO_OPT
				encoder.EncodeArrayItemStart(1, writer, context.CollectionContext); // OMITTABLE
#endif
				encoder.EncodeInt32(obj.Age, writer);
#if NO_OPT
				encoder.EncodeArrayItemEnd(1, writer, context.CollectionContext); // OMITTABLE
#endif
#endif
#if BOOL
#if NO_OPT
				encoder.EncodeArrayItemStart(2, writer, context.CollectionContext); // OMITTABLE
#endif
				encoder.EncodeBoolean(obj.IsActive, writer);
#if NO_OPT
				encoder.EncodeArrayItemEnd(2, writer, context.CollectionContext); // OMITTABLE
#endif
#endif // BOOL
#if COLLECTION
#if NO_OPT
				encoder.EncodeArrayItemStart(3, writer, context.CollectionContext); // OMITTABLE
#endif
				if (obj.Roles == null)
				{
					encoder.EncodeNull(writer);
				}
				else
				{
					encoder.EncodeArrayStart(obj.Roles.Count, writer, context.CollectionContext); // OMITTABLE
					context.IncrementDepth();
					var roles = obj.Roles;
					var count = roles.Count;
					for (var i = 0; i < count; i++)
					{
#if NO_OPT
						encoder.EncodeArrayItemStart(i, writer, context.CollectionContext); // OMITTABLE
#endif
						encoder.EncodeString(roles[i], writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
#if NO_OPT
						encoder.EncodeArrayItemEnd(i, writer, context.CollectionContext); // OMITTABLE
#endif
					}
					context.DecrementDepth();
#if NO_OPT
					encoder.EncodeArrayEnd(obj.Roles.Count, writer, context.CollectionContext);
#endif
				}
#if NO_OPT
				encoder.EncodeArrayItemEnd(3, writer, context.CollectionContext); // OMITTABLE

				encoder.EncodeArrayItemStart(4, writer, context.CollectionContext); // OMITTABLE
#endif
				if (obj.Attributes == null)
				{
					encoder.EncodeNull(writer);
				}
				else
				{
					encoder.EncodeMapStart(obj.Attributes.Count, writer, context.CollectionContext); // OMITTABLE
					context.IncrementDepth();
#if NO_OPT
					var i = 0; // OMITTABLE
#endif
					foreach (var entry in obj.Attributes)
					{
#if NO_OPT
						encoder.EncodeMapKeyStart(i, writer, context.CollectionContext); // OMITTABLE
#endif
						encoder.EncodeString(entry.Key, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
#if NO_OPT
						encoder.EncodeMapKeyEnd(i, writer, context.CollectionContext); // OMITTABLE
						encoder.EncodeMapValueStart(i, writer, context.CollectionContext); // OMITTABLE
#endif
						encoder.EncodeString(entry.Value, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
#if NO_OPT
						encoder.EncodeMapValueEnd(i, writer, context.CollectionContext); // OMITTABLE
#endif
					}
					context.DecrementDepth();
#if NO_OPT
					encoder.EncodeMapEnd(obj.Attributes.Count, writer, context.CollectionContext); // OMITTABLE
#endif
				}
#if NO_OPT
				encoder.EncodeArrayItemEnd(4, writer, context.CollectionContext); // OMITTABLE
#endif
#endif // COLLECTION
#if NO_OPT
				encoder.EncodeArrayEnd(propertyCount, writer, context.CollectionContext);
#endif
					}
#else // USEARRAY
			//else
			{
				encoder.EncodeMapStart(propertyCount, writer, context.CollectionContext); // OMITTABLE

#if STRING
#if JSON
				ReadOnlySpan<byte> key0 = new[] { (byte)'"', (byte)'N', (byte)'a', (byte)'m', (byte)'e', (byte)'"' }; // static readonly
#else
				ReadOnlySpan<byte> key0 = new[] { 0xA4, (byte)'N', (byte)'a', (byte)'m', (byte)'e' }; // static readonly
#endif
#warning TODO: Use DirectWrite
#if NO_OPT
				encoder.EncodeMapKeyStart(0, writer, context.CollectionContext); // OMITTABLE
#endif
				encoder.EncodeRawString(key0, 4, writer, context.CancellationToken);
#if NO_OPT
				encoder.EncodeMapKeyEnd(0, writer, context.CollectionContext); // OMITTABLE
				encoder.EncodeMapValueStart(0, writer, context.CollectionContext); // OMITTABLE
#endif
				encoder.EncodeString(obj.Name, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
#if NO_OPT
				encoder.EncodeMapValueEnd(0, writer, context.CollectionContext); // OMITTABLE
#endif
#endif // STRING
#if INT32
#if JSON
				ReadOnlySpan<byte> key1 = new[] { (byte)'"', (byte)'A', (byte)'g', (byte)'e', (byte)'"' }; // static readonly
#else
				ReadOnlySpan<byte> key1 = new[] { 0xA3, (byte)'A', (byte)'g', (byte)'e' }; // static readonly
#endif
#if NO_OPT
				encoder.EncodeMapKeyStart(1, writer, context.CollectionContext); // OMITTABLE
#endif
				encoder.EncodeRawString(key1, 3, writer, context.CancellationToken);
#if NO_OPT
				encoder.EncodeMapKeyEnd(1, writer, context.CollectionContext); // OMITTABLE
				encoder.EncodeMapValueStart(1, writer, context.CollectionContext); // OMITTABLE
#endif
				encoder.EncodeInt32(obj.Age, writer);
#if NO_OPT
				encoder.EncodeMapValueEnd(1, writer, context.CollectionContext); // OMITTABLE
#endif
#endif // INT32
#if BOOL
#if JSON
				ReadOnlySpan<byte> key2 = new[] { (byte)'"', (byte)'I', (byte)'s', (byte)'A', (byte)'c', (byte)'t', (byte)'i', (byte)'v', (byte)'e', (byte)'"' }; // static readonly
#else
				ReadOnlySpan<byte> key2 = new[] { 0xA8, (byte)'I', (byte)'s', (byte)'A', (byte)'c', (byte)'t', (byte)'i', (byte)'v', (byte)'e' }; // static readonly
#endif
#if NO_OPT
				encoder.EncodeMapKeyStart(2, writer, context.CollectionContext); // OMITTABLE
#endif
				encoder.EncodeRawString(key2, 8, writer, context.CancellationToken);
#if NO_OPT
				encoder.EncodeMapKeyEnd(2, writer, context.CollectionContext); // OMITTABLE
				encoder.EncodeMapValueStart(2, writer, context.CollectionContext); // OMITTABLE
#endif
				encoder.EncodeBoolean(obj.IsActive, writer);
#if NO_OPT
				encoder.EncodeMapValueEnd(2, writer, context.CollectionContext); // OMITTABLE
#endif
#endif // BOOL
#if COLLECTION
#if JSON
				ReadOnlySpan<byte> key3 = new[] { (byte)'"', (byte)'R', (byte)'o', (byte)'l', (byte)'e', (byte)'s', (byte)'"' }; // static readonly
#else
				ReadOnlySpan<byte> key3 = new[] { 0xA5, (byte)'R', (byte)'o', (byte)'l', (byte)'e', (byte)'s' }; // static readonly
#endif
#if NO_OPT
				encoder.EncodeMapKeyStart(3, writer, context.CollectionContext); // OMITTABLE
#endif
				encoder.EncodeRawString(key3, 5, writer, context.CancellationToken);
#if NO_OPT
				encoder.EncodeMapKeyEnd(3, writer, context.CollectionContext); // OMITTABLE
				encoder.EncodeMapValueStart(3, writer, context.CollectionContext); // OMITTABLE
#endif
				if (obj.Roles == null)
				{
					encoder.EncodeNull(writer);
				}
				else
				{
					encoder.EncodeArrayStart(obj.Roles.Count, writer, context.CollectionContext); // OMITTABLE
					context.IncrementDepth();
#if NO_OPT
					var i = 0; // OMITTABLE
#endif
					foreach (var item in obj.Roles)
					{
#if NO_OPT
						encoder.EncodeArrayItemStart(i, writer, context.CollectionContext); // OMITTABLE
#endif
						encoder.EncodeString(item, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
#if NO_OPT
						encoder.EncodeArrayItemEnd(i, writer, context.CollectionContext); // OMITTABLE
#endif
					}
					context.DecrementDepth();
#if NO_OPT
					encoder.EncodeArrayEnd(obj.Roles.Count, writer, context.CollectionContext); // OPTIMIZABLE
#endif
				}
#if NO_OPT
				encoder.EncodeMapValueEnd(3, writer, context.CollectionContext); // OMITTABLE
#endif

#if JSON
				ReadOnlySpan<byte> key4 = new[] { (byte)'"', (byte)'A', (byte)'t', (byte)'t', (byte)'r', (byte)'i', (byte)'b', (byte)'u', (byte)'t', (byte)'e', (byte)'s', (byte)'"' }; // static readonly
#else
				ReadOnlySpan<byte> key4 = new[] { 0xAA, (byte)'A', (byte)'t', (byte)'t', (byte)'r', (byte)'i', (byte)'b', (byte)'u', (byte)'t', (byte)'e', (byte)'s' }; // static readonly
#endif
#if NO_OPT
				encoder.EncodeMapKeyStart(4, writer, context.CollectionContext); // OMITTABLE
#endif
				encoder.EncodeRawString(key4, 10, writer, context.CancellationToken);
#if NO_OPT
				encoder.EncodeMapKeyEnd(4, writer, context.CollectionContext); // OMITTABLE
				encoder.EncodeMapValueStart(4, writer, context.CollectionContext); // OMITTABLE
#endif
				if (obj.Attributes == null)
				{
					encoder.EncodeNull(writer);
				}
				else
				{
					encoder.EncodeMapStart(obj.Attributes.Count, writer, context.CollectionContext); // OMITTABLE
					context.IncrementDepth();
					var i = 0; // OMITTABLE
					foreach (var entry in obj.Attributes)
					{
#if NO_OPT
						encoder.EncodeMapKeyStart(i, writer, context.CollectionContext); // OMITTABLE
#endif
						encoder.EncodeString(entry.Key, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
#if NO_OPT
						encoder.EncodeMapKeyEnd(i, writer, context.CollectionContext); // OMITTABLE
						encoder.EncodeMapValueStart(i, writer, context.CollectionContext); // OMITTABLE
#endif
						encoder.EncodeString(entry.Value, writer, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
#if NO_OPT
						encoder.EncodeMapValueEnd(i, writer, context.CollectionContext); // OMITTABLE
#endif
					}
					context.DecrementDepth();
					encoder.EncodeMapEnd(obj.Attributes.Count, writer, context.CollectionContext);
				}
#if NO_OPT
				encoder.EncodeMapValueEnd(4, writer, context.CollectionContext); // OMITTABLE
#endif
#endif // COLLECTION
#if NO_OPT
				encoder.EncodeMapEnd(propertyCount, writer, context.CollectionContext);
#endif
			}
#endif // !USE_ARRAY
		}

		private static readonly MsgPackStringTrie<uint> DeserializationTrie = InitializeDeserializationTrie();
		private static MsgPackStringTrie<uint> InitializeDeserializationTrie()
		{
			ReadOnlySpan<byte> __name = new byte[] { (byte)'N', (byte)'a', (byte)'m', (byte)'e' };
			ReadOnlySpan<byte> __age = new byte[] { (byte)'A', (byte)'g', (byte)'e' };
			ReadOnlySpan<byte> __isActive = new byte[] { (byte)'I', (byte)'s', (byte)'A', (byte)'c', (byte)'t', (byte)'i', (byte)'v', (byte)'e' };
			ReadOnlySpan<byte> __roles = new byte[] { (byte)'R', (byte)'o', (byte)'l', (byte)'e', (byte)'s' };
			ReadOnlySpan<byte> __attributes = new byte[] { (byte)'A', (byte)'t', (byte)'t', (byte)'r', (byte)'i', (byte)'b', (byte)'u', (byte)'t', (byte)'e', (byte)'s' };
			var trie = new MsgPackStringTrie<uint>(5);
#if !JSON
#if STRING
			trie.TryAdd(__name, 0);
#endif
#if INT32
			trie.TryAdd(__age, 1);
#endif
#if BOOL
			trie.TryAdd(__isActive, 2);
#endif
#if COLLECTION
			trie.TryAdd(__roles, 3);
			trie.TryAdd(__attributes, 4);
#endif
#else
#if STRING
			trie.TryAddRaw(__name, 0);
#endif
#if INT32
			trie.TryAddRaw(__age, 1);
#endif
#if BOOL
			trie.TryAddRaw(__isActive, 2);
#endif
#if COLLECTION
			trie.TryAddRaw(__roles, 3);
			trie.TryAddRaw(__attributes, 4);
#endif
#endif
			//			var trie32 = new MsgPackStringTrie32<uint>(5);
			//#if STRING
			//			trie32.TryAdd(__name, 0);
			//#endif
			//#if INT32
			//			trie32.TryAdd(__age, 1);
			//#endif
			//#if BOOL
			//			trie32.TryAdd(__isActive, 2);
			//#endif
			//#if COLLECTION
			//			trie32.TryAdd(__roles, 3);
			//			trie32.TryAdd(__attributes, 4);
			//#endif
			Console.WriteLine(String.Join(Environment.NewLine, trie.GetDebugView().Select(x => $"{String.Join('-', x.Keys.Select(l => l.ToString("X8")))} = {x.Value}")));

			return trie;
		}

		public sealed override SampleObject Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> reader)
		{
#warning TODO: Inlining to avoid allocation when null.
			// WHEN T is reference type and it has a default contractor -> DeserializeToAsync
			var obj = new SampleObject();
			if (this.DeserializeTo(ref context, ref reader, obj))
			{
				return obj;
			}
			else
			{
				// WHEN T is not nullable, then throw.
				return null;
			}

			// WHEN T does not have default constructor -> Inline implementation which is same as  DeserializeToAsync
			// ....
			// return new SampleObject(name, age, isActive, roles, attributes);

			// WHEN T is mutable value type
			// ...
			// var obj = default;
			// obj.Name = name;
			// obj.Age = age;
			// obj.IsActive = isActive;
			// obj.Roles = roles; // Roles must be null
			// obj.Attributes = attributes; // Attributes must be null
			// return obj;
		}

		public sealed override async ValueTask<SampleObject> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence streamSource)
		{
			var obj = new SampleObject();
			// WHEN T is reference type and it has a default contractor -> DeserializeToAsync
			if (await this.DeserializeToAsync(context, streamSource, obj).ConfigureAwait(false))
			{
				return obj;
			}
			else
			{
				// WHEN T is not nullable, then throw.
				return null;
			}

			// WHEN T does not have default constructor -> Inline implementation which is same as  DeserializeToAsync
			// ....
			// return new SampleObject(name, age, isActive, roles, attributes);

			// WHEN T is mutable value type
			// ...
			// var obj = default;
			// obj.Name = name;
			// obj.Age = age;
			// obj.IsActive = isActive;
			// obj.Roles = roles; // Roles must be null
			// obj.Attributes = attributes; // Attributes must be null
			// return obj;
		}

		private static void ThrowNotEnoughItems(long actual, int expected)
			=> throw new MessageTypeException();

		public sealed override bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> reader, in SampleObject obj)
		{
			const int propertyCount =
					0
#if INT32
					+ 1
#endif
#if STRING
					+ 1
#endif
#if BOOL
					+ 1
#endif
#if USE_ARRAY
					+ 1
					+ 1
#endif
					;
#if STRING
			string name = default!;
#endif
#if INT32
			int age = default;
#endif
#if BOOL
			bool isActive = default;
#endif
#if COLLECTION
			var roles = obj.Roles;
			var attributes = obj.Attributes;
#endif

#if JSON
			var decoder = (JsonDecoder)(object)context.Decoder;
#elif NO_OPT
			var decoder = context.Decoder;
#else
			var decoder = new MessagePackDecoder(context.Decoder.Options as MessagePackDecoderOptions);
#endif
			CollectionType arrayOrMap;
			int itemsCount;
			CollectionItemIterator propertyIterator;

			if (this.FormatFeatures.CanCountCollectionItems) // OPTIMIZABLE
			{
				arrayOrMap = decoder.DecodeArrayOrMapHeader(ref reader, out itemsCount);
				if (arrayOrMap.IsNull)
				{
#warning TODO: result = null;
					return false;
				}

				if (itemsCount < propertyCount)
				{
					ThrowNotEnoughItems(itemsCount, propertyCount);
				}

				propertyIterator = default;
			}
			else
			{
				arrayOrMap = decoder.DecodeArrayOrMap(ref reader, out propertyIterator);
				if (arrayOrMap.IsNull)
				{
#warning TODO: result = null;
					return false;
				}
				itemsCount = -1;
			}

			// Instantiate object for Deserialize

			context.IncrementDepth();

			// If !SerializerGenerationOptions.InfersObjectSerialization && SerializerGenerationOptions.UseArray || typeof(T).IsDefined([SerializeAs(Array)])
			if (arrayOrMap.IsArray)
			{
#if STRING
#if NO_OPT
				if (!decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					CheckNextItemExists(ref reader, ref propertyIterator);
				}
#endif

				name = decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
#endif

#if INT32
#if NO_OPT
				if (!decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					CheckNextItemExists(ref reader, ref propertyIterator);
				}
#endif

				age = decoder.DecodeInt32(ref reader);
#endif

#if BOOL
#if NO_OPT
				if (!decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					CheckNextItemExists(ref reader, ref propertyIterator);
				}
#endif

				isActive = decoder.DecodeBoolean(ref reader);
#endif

#if COLLECTION
#if NO_OPT
				if (!decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					CheckNextItemExists(ref reader, ref propertyIterator);
				}
#endif

				context.IncrementDepth();
#if NO_OPT
				if (decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
#endif
					var arrayLength = decoder.DecodeArrayHeader(ref reader);
					// If settable
					//if (roles == null)
					//{
					//	roles = new List<string>(arrayLength); // or 0
					//}

					for (var i = 0; i < arrayLength; i++)
					{
						roles.Add(decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken)); // <>NameEncoding ?? context.StringEncoding
					}
#if NO_OPT
				}
				else
				{
					var iterator = decoder.DecodeArray(ref reader);
					while (!iterator.CollectionEnds(ref reader))
					{
						roles.Add(decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken)); // <>NameEncoding ?? context.StringEncoding
					}
					iterator.Drain(ref reader);
				}
#endif
				context.DecrementDepth();

#if NO_OPT
				if (!decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					CheckNextItemExists(ref reader, ref propertyIterator);
				}
#endif
				context.IncrementDepth();
#if NO_OPT
				if (decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
#endif
					var mapCount = decoder.DecodeMapHeader(ref reader);
					// If settable
					//if (attributes == null)
					//{
					//	attributes = new Dictionary<string, string>(mapCount); // or 0
					//}
					for (var i = 0; i < mapCount; i++)
					{
						attributes.Add(
							decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken), // <>NameEncoding ?? context.StringEncoding
							decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken) // <>NameEncoding ?? context.StringEncoding
						);
					}
#if NO_OPT
				}
				else
				{
					var iterator = decoder.DecodeMap(ref reader);
					while (!iterator.CollectionEnds(ref reader))
					{
						attributes.Add(
							decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken), // <>NameEncoding ?? context.StringEncoding
							decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken) // <>NameEncoding ?? context.StringEncoding
						);
					}
					iterator.Drain(ref reader);
				}
#endif
				context.DecrementDepth();
#endif
			}
			else
			{
				// Map

				if (decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					for (var i = 0; i < itemsCount; i++)
					{
#warning For msgpack, it should contain header, For JSON, it should contain ...? -> For Collection
						decoder.GetRawString(ref reader, out ReadOnlySpan<byte> key, context.CancellationToken);

						context.ValidatePropertyKeyLength(reader.Consumed, key.Length);

						// Use inlined trie, prefixed by the count as MP uint32.
						// char is UTF-8, big endian.
						// 1st node: [length(1-5)][chars(7-3)]

#if INLINE_TRIE
						// TODO: 64bit, binary search
						//417349A8-76697463-00000065 = 2
						//656741A3 = 1
						//6C6F52A5-00007365 = 3
						//6D614EA4-00000065 = 0
						//747441AA-75626972-00736574 = 4					
						var intKey = MsgPackStringTrieKey.GetRaw64(ref key);
						switch (intKey)
#else
						switch (DeserializationTrie.GetOrDefault(key))
#endif // INLINE_TRIE
						{
#if STRING
#if INLINE_TRIE
							case 0x6D614EA4:
							{
								intKey = MsgPackStringTrieKey.GetRaw64(ref key);
								if (intKey != 0x00000065)
								{
									goto default;
								}
#else
							case 0:
							{
#endif

								name = decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
								break;
							}
#endif
#if INT32
#if INLINE_TRIE
							case 0x656741A3:
#else
							case 1:
#endif
							{
								age = decoder.DecodeInt32(ref reader);
								break;
							}
#endif
#if BOOL
#if INLINE_TRIE
							case 0x417349A8:
							{
								intKey = MsgPackStringTrieKey.GetRaw64(ref key);
								if (intKey != 0x76697463)
								{
									goto default;
								}

								intKey = MsgPackStringTrieKey.GetRaw64(ref key);
								if (intKey != 0x00000065)
								{
									goto default;
								}
#else
							case 2:
							{
#endif
								isActive = decoder.DecodeBoolean(ref reader);
								break;
							}
#endif
#if COLLECTION
#if INLINE_TRIE
							case 0x6C6F52A5:
							{
								intKey = MsgPackStringTrieKey.GetRaw64(ref key);
								if (intKey != 0x00007365)
								{
									goto default;
								}

#else
							case 3:
							{
#endif
#warning TODO: No Collection items deserialized!
								context.IncrementDepth();
								if (decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
								{
									var arrayLength = decoder.DecodeArrayHeader(ref reader);
									// If settable
									//if (roles == null)
									//{
									//	roles = new List<string>(arrayLength); // or 0
									//}
									for (var j = 0; j < arrayLength; j++)
									{
										roles.Add(decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken)); // <>NameEncoding ?? context.StringEncoding
									}
								}
								else
								{
									var iterator = decoder.DecodeArray(ref reader);
									while (!iterator.CollectionEnds(ref reader))
									{
										roles.Add(decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken)); // <>NameEncoding ?? context.StringEncoding
									}
									iterator.Drain(ref reader);
								}
								context.DecrementDepth();
								break;
							}
#if INLINE_TRIE
							case 0x747441AA:
							{
								intKey = MsgPackStringTrieKey.GetRaw64(ref key);
								if (intKey != 0x75626972)
								{
									goto default;
								}

								intKey = MsgPackStringTrieKey.GetRaw64(ref key);
								if (intKey != 0x00736574)
								{
									goto default;
								}
#else
							case 4:
							{
#endif
								context.IncrementDepth();
								if (decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
								{
									var mapCount = decoder.DecodeMapHeader(ref reader);
									// If settable
									//if (attributes == null)
									//{
									//	attributes = new Dictionary<string, string>(mapCount); // or 0
									//}
									for (var j = 0; j < mapCount; j++)
									{
										attributes.Add(
											decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken), // <>NameEncoding ?? context.StringEncoding
											decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken) // <>NameEncoding ?? context.StringEncoding
										);
									}
								}
								else
								{
									var iterator = decoder.DecodeMap(ref reader);
									while (!iterator.CollectionEnds(ref reader))
									{
										attributes.Add(
											decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken), // <>NameEncoding ?? context.StringEncoding
											decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken) // <>NameEncoding ?? context.StringEncoding
										);
									}
									iterator.Drain(ref reader);
								}
								context.DecrementDepth();
								break;
							}
#endif
							default:
							{
								context.Decoder.Skip(ref reader, context.CollectionContext, context.CancellationToken);
								break;
							}
						}
					}
				}
				else
				{
					while (!propertyIterator.CollectionEnds(ref reader))
					{
#warning For msgpack, it should contain header, For JSON, it should contain ...? -> For Collection
						decoder.GetRawString(ref reader, out ReadOnlySpan<byte> key, context.CancellationToken);

						context.ValidatePropertyKeyLength(reader.Consumed, key.Length);

						CheckNextItemExists(ref reader, ref propertyIterator);

						// Use inlined trie, prefixed by the count as MP uint32.
						// char is UTF-8, big endian.
						// 1st node: [length(1-5)][chars(7-3)]

#if INLINE_TRIE
						// TODO: 64bit, binary search
						//417349A8-76697463-00000065 = 2
						//656741A3 = 1
						//6C6F52A5-00007365 = 3
						//6D614EA4-00000065 = 0
						//747441AA-75626972-00736574 = 4					
						var intKey = MsgPackStringTrieKey.GetRaw64(ref key);
						switch (intKey)
#else
						switch (DeserializationTrie.GetOrDefault(key))
#endif // INLINE_TRIE
						{
#if STRING
#if INLINE_TRIE
							case 0x6D614EA4:
							{
								intKey = MsgPackStringTrieKey.GetRaw64(ref key);
								if (intKey != 0x00000065)
								{
									goto default;
								}
#else
							case 0:
							{
#endif

								name = decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
								break;
							}
#endif
#if INT32
#if INLINE_TRIE
							case 0x656741A3:
#else
							case 1:
#endif
							{
								age = decoder.DecodeInt32(ref reader);
								break;
							}
#endif
#if BOOL
#if INLINE_TRIE
							case 0x417349A8:
							{
								intKey = MsgPackStringTrieKey.GetRaw64(ref key);
								if (intKey != 0x76697463)
								{
									goto default;
								}

								intKey = MsgPackStringTrieKey.GetRaw64(ref key);
								if (intKey != 0x00000065)
								{
									goto default;
								}
#else
							case 2:
							{
#endif
								isActive = decoder.DecodeBoolean(ref reader);
								break;
							}
#endif
#if COLLECTION
#if INLINE_TRIE
							case 0x6C6F52A5:
							{
								intKey = MsgPackStringTrieKey.GetRaw64(ref key);
								if (intKey != 0x00007365)
								{
									goto default;
								}

#else
							case 3:
							{
#endif
#warning TODO: No Collection items deserialized!
								context.IncrementDepth();
								if (decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
								{
									var arrayLength = decoder.DecodeArrayHeader(ref reader);
									// If settable
									//if (roles == null)
									//{
									//	roles = new List<string>(arrayLength); // or 0
									//}
									for (var j = 0; j < arrayLength; j++)
									{
										roles.Add(decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken)); // <>NameEncoding ?? context.StringEncoding
									}
								}
								else
								{
									var iterator = decoder.DecodeArray(ref reader);
									while (!iterator.CollectionEnds(ref reader))
									{
										roles.Add(decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken)); // <>NameEncoding ?? context.StringEncoding
									}
									iterator.Drain(ref reader);
								}
								context.DecrementDepth();
								break;
							}
#if INLINE_TRIE
							case 0x747441AA:
							{
								intKey = MsgPackStringTrieKey.GetRaw64(ref key);
								if (intKey != 0x75626972)
								{
									goto default;
								}

								intKey = MsgPackStringTrieKey.GetRaw64(ref key);
								if (intKey != 0x00736574)
								{
									goto default;
								}
#else
							case 4:
							{
#endif
								context.IncrementDepth();
								if (decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
								{
									var mapCount = decoder.DecodeMapHeader(ref reader);
									// If settable
									//if (attributes == null)
									//{
									//	attributes = new Dictionary<string, string>(mapCount); // or 0
									//}
									for (var j = 0; j < mapCount; j++)
									{
										attributes.Add(
											decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken), // <>NameEncoding ?? context.StringEncoding
											decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken) // <>NameEncoding ?? context.StringEncoding
										);
									}
								}
								else
								{
									var iterator = decoder.DecodeMap(ref reader);
									while (!iterator.CollectionEnds(ref reader))
									{
										var attributesKey = decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken); // <>NameEncoding ?? context.StringEncoding
										CheckNextItemExists(ref reader, ref iterator);
										attributes.Add(
											attributesKey,
											decoder.DecodeString(ref reader, context.StringEncoding, context.CancellationToken) // <>NameEncoding ?? context.StringEncoding
										);
									}
									iterator.Drain(ref reader);
								}
								context.DecrementDepth();
								break;
							}
#endif
							default:
							{
								context.Decoder.Skip(ref reader, context.CollectionContext, context.CancellationToken);
								break;
							}
						}
					}

				}
			}

#if NO_OPT
			if (decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
			{
#endif
				decoder.Drain(ref reader, context.CollectionContext, itemsCount - 5, context.CancellationToken);
#if NO_OPT
			}
			else
			{
				propertyIterator.Drain(ref reader);
			}
#endif

			context.DecrementDepth();

			// They are verbose for Deserialize
#if STRING
			obj.Name = name;
#endif
#if INT32
			obj.Age = age;
#endif
#if BOOL
			obj.IsActive = isActive;
#endif
			// If settable
			// obj.Roles = roles;
			// If settable
			// obj.Attributes = attributes;
			return true;
		}

		private static void CheckNextItemExists(ref SequenceReader<byte> reader, ref CollectionItemIterator propertyIterator)
		{
			if (propertyIterator.CollectionEnds(ref reader))
			{
				throw new MessageTypeException(); // Use Throws
			}
		}

		private static bool TryDecodeArrayOrMapHeader(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, out int itemsCount, out CollectionType arrayOrMap, out CollectionItemIterator propertyIterator, out int requestHint)
		{
			context.IncrementDepth();
			var reader = new SequenceReader<byte>(sequence);

			arrayOrMap = context.Decoder.DecodeArrayOrMapHeader(ref reader, out itemsCount, out requestHint);
			if (arrayOrMap.IsNone)
			{
				propertyIterator = default;
				return false;
			}

			if (context.Decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
			{
				arrayOrMap = context.Decoder.DecodeArrayOrMapHeader(ref reader, out itemsCount);
				if (itemsCount < 5)
				{
					throw new MessageTypeException(); // Use Throws
				}

				propertyIterator = default;
			}
			else
			{
				arrayOrMap = context.Decoder.DecodeArrayOrMap(ref reader, out propertyIterator);
				itemsCount = -1;
			}

			sequence = sequence.Slice(reader.Consumed);

			return true;
		}

		private static bool TryDecodeArrayHeader(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, out long arrayLength, out int requestHint)
		{
			var reader = new SequenceReader<byte>(sequence);

			arrayLength = context.Decoder.DecodeArrayHeader(ref reader, out requestHint);
			if (requestHint == 0)
			{
				sequence = sequence.Slice(reader.Consumed);
			}
			return requestHint == 0;
		}

		private static bool TryDecodeMapHeader(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, out long mapCount, out int requestHint)
		{
			var reader = new SequenceReader<byte>(sequence);

			mapCount = context.Decoder.DecodeMapHeader(ref reader, out requestHint);
			if (requestHint == 0)
			{
				sequence = sequence.Slice(reader.Consumed);
			}
			return requestHint == 0;
		}

		private static bool TryDecodeValueOfName(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, out string name, out int requestHint)
		{
			var reader = new SequenceReader<byte>(sequence);

			name = context.Decoder.DecodeString(ref reader, out requestHint, context.StringEncoding, context.CancellationToken)!; // <>NameEncoding ?? context.StringEncoding
			if (requestHint == 0)
			{
				sequence = sequence.Slice(reader.Consumed);
			}
			return requestHint == 0;
		}

		private static bool TryDecodeValueOfAge(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, out int age, out int requestHint)
		{
			var reader = new SequenceReader<byte>(sequence);

			age = context.Decoder.DecodeInt32(ref reader, out requestHint);
			if (requestHint == 0)
			{
				sequence = sequence.Slice(reader.Consumed);
			}
			return requestHint == 0;
		}

		private static bool TryDecodeValueOfIsActive(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, out bool isActive, out int requestHint)
		{
			var reader = new SequenceReader<byte>(sequence);

			isActive = context.Decoder.DecodeBoolean(ref reader, out requestHint);
			if (requestHint == 0)
			{
				sequence = sequence.Slice(reader.Consumed);
			}
			return requestHint == 0;
		}

		private static bool TryDecodeItemOfRoles(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, out string item, out int requestHint)
		{
			var reader = new SequenceReader<byte>(sequence);

			item = context.Decoder.DecodeString(ref reader, out requestHint, context.StringEncoding, context.CancellationToken)!; // <>NameEncoding ?? context.StringEncoding
			if (requestHint == 0)
			{
				sequence = sequence.Slice(reader.Consumed);
			}
			return requestHint == 0;
		}

		private static bool TryDecodeKeyOfAttributes(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, out string key, out int requestHint)
		{
			var reader = new SequenceReader<byte>(sequence);

			key = context.Decoder.DecodeString(ref reader, out requestHint, context.StringEncoding, context.CancellationToken)!; // <>NameEncoding ?? context.StringEncoding
			if (requestHint == 0)
			{
				sequence = sequence.Slice(reader.Consumed);
			}
			return requestHint == 0;
		}

		private static bool TryDecodeValueOfAttributes(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, out string value, out int requestHint)
		{
			var reader = new SequenceReader<byte>(sequence);

			value = context.Decoder.DecodeString(ref reader, out requestHint, context.StringEncoding, context.CancellationToken)!; // <>NameEncoding ?? context.StringEncoding
			if (requestHint == 0)
			{
				sequence = sequence.Slice(reader.Consumed);
			}
			return requestHint == 0;
		}

		private static bool TryDecodeArray(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, out CollectionItemIterator iterator, out int requestHint)
		{
			var reader = new SequenceReader<byte>(sequence);

			iterator = context.Decoder.DecodeArray(ref reader, out requestHint);
			if (requestHint == 0)
			{
				sequence = sequence.Slice(reader.Consumed);
			}
			return requestHint == 0;
		}

		private static bool TryDecodeMap(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, out CollectionItemIterator iterator, out int requestHint)
		{
			var reader = new SequenceReader<byte>(sequence);

			iterator = context.Decoder.DecodeMap(ref reader, out requestHint);
			if (requestHint == 0)
			{
				sequence = sequence.Slice(reader.Consumed);
			}
			return requestHint == 0;
		}

		private static bool TryGetRawString(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, [NotNullWhen(true)] out byte[] keyArray, out ReadOnlyMemory<byte> key, out int requestHint)
		{
			var reader = new SequenceReader<byte>(sequence);

			ReadOnlySpan<byte> span;
			if (!context.Decoder.GetRawString(ref reader, out span, out requestHint, context.CancellationToken))
			{
				keyArray = default!;
				key = default;
				return false;
			}

			keyArray = context.ByteBufferPool.Rent(span.Length);
			span.CopyTo(keyArray);
			key = keyArray.AsMemory(0, span.Length);
			return true;
		}

		private static bool TryDrain(AsyncDeserializationOperationContext context, ref ReadOnlySequence<byte> sequence, long remaining, out int requestHint)
		{
			var reader = new SequenceReader<byte>(sequence);
			context.Decoder.Drain(ref reader, context.CollectionContext, remaining, out requestHint);
			if (requestHint == 0)
			{
				sequence = sequence.Slice(reader.Consumed);
			}
			return requestHint == 0;
		}

		private bool TryCheckNextItemExists(ref CollectionItemIterator propertyIterator, ReadOnlySequence<byte> sequence, out int requestHint)
		{
			if (propertyIterator.CollectionEnds(sequence, out requestHint))
			{
				throw new MessageTypeException(); // use throws
			}

			return requestHint == 0;
		}

		public sealed override async ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, SampleObject obj)
		{
			// T が値�
			// throw new NotSupportedException();

			await source.FetchAsync(context.CancellationToken).ConfigureAwait(false);
			var sequence = source.Sequence;
			int requestHint;

#if STRING
			string name = default!;
#endif
#if INT32
			int age = default;
#endif
#if BOOL
			bool isActive = default;
#endif
#if COLLECTION
			var roles = obj.Roles;
			var attributes = obj.Attributes;
#endif

			var decoder = context.Decoder;
			context.IncrementDepth();
			int itemsCount;
			CollectionType arrayOrMap;
			CollectionItemIterator propertyIterator;
			while (!TryDecodeArrayOrMapHeader(context, ref sequence, out itemsCount, out arrayOrMap, out propertyIterator, out requestHint))
			{
				await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
			}

			if (arrayOrMap.IsNull)
			{
				return false;
			}

			// Instantiate when Deserialize

			// If !SerializerGenerationOptions.InfersObjectSerialization && SerializerGenerationOptions.UseArray || typeof(T).IsDefined([SerializeAs(Array)])
			if (arrayOrMap.IsArray)
			{
#if STRING
				if (!context.Decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					while (!this.TryCheckNextItemExists(ref propertyIterator, sequence, out requestHint))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}
				}

				while (!TryDecodeValueOfName(context, ref sequence, out name, out requestHint))
				{
					await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
				}
#endif

#if INT32
				if (!context.Decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					while (!this.TryCheckNextItemExists(ref propertyIterator, sequence, out requestHint))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}
				}

				while (!TryDecodeValueOfAge(context, ref sequence, out age, out requestHint))
				{
					await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
				}
#endif

#if BOOL
				if (!context.Decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					while (!this.TryCheckNextItemExists(ref propertyIterator, sequence, out requestHint))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}
				}

				while (!TryDecodeValueOfIsActive(context, ref sequence, out isActive, out requestHint))
				{
					await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
				}
#endif

#if COLLECTION
				if (!context.Decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					while (!this.TryCheckNextItemExists(ref propertyIterator, sequence, out requestHint))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}
				}

				context.IncrementDepth();

				if (decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					long arrayLength;
					while (!TryDecodeArrayHeader(context, ref sequence, out arrayLength, out requestHint))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}

					// If settable
					//if (roles == null)
					//{
					//	roles = new List<string>(arrayLength); // or 0
					//}

					for (var i = 0; i < arrayLength; i++)
					{
						string item;
						while (!TryDecodeItemOfRoles(context, ref sequence, out item, out requestHint))
						{
							await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
						}
						roles.Add(item);
					}
				}
				else
				{
					CollectionItemIterator iterator;
					while (!TryDecodeArray(context, ref sequence, out iterator, out requestHint))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}

					while (!iterator.CollectionEnds(sequence, out requestHint))
					{
						if (requestHint != 0)
						{
							await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
						}

						string item;
						while (!TryDecodeItemOfRoles(context, ref sequence, out item, out requestHint))
						{
							await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
						}
						roles.Add(item);
					}

					while (!iterator.Drain(ref sequence, out requestHint))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}
				}
				context.DecrementDepth();

				if (!context.Decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					while (!this.TryCheckNextItemExists(ref propertyIterator, sequence, out requestHint))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}
				}

				context.IncrementDepth();
				if (decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
				{
					long mapCount;
					while (!TryDecodeMapHeader(context, ref sequence, out mapCount, out requestHint))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}

					// If settable
					//if (attributes == null)
					//{
					//	attributes = new Dictionary<string, string>(mapCount); // or 0
					//}
					for (var i = 0; i < mapCount; i++)
					{
						string key;
						while (!TryDecodeKeyOfAttributes(context, ref sequence, out key, out requestHint))
						{
							await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
						}
						string value;
						while (!TryDecodeValueOfAttributes(context, ref sequence, out value, out requestHint))
						{
							await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
						}
						attributes.Add(key, value);
					}
				}
				else
				{
					CollectionItemIterator iterator;
					while (!TryDecodeMap(context, ref sequence, out iterator, out requestHint))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}

					while (!iterator.CollectionEnds(sequence, out requestHint))
					{
						if (requestHint != 0)
						{
							await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
						}

						string key;
						while (!TryDecodeKeyOfAttributes(context, ref sequence, out key, out requestHint))
						{
							await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
						}
						string value;
						while (!TryDecodeValueOfAttributes(context, ref sequence, out value, out requestHint))
						{
							await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
						}
						attributes.Add(key, value);
					}

					while (!iterator.Drain(ref sequence, out requestHint))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}
				}
				context.DecrementDepth();
#endif
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
					byte[] propertyKeyArray = null!;
					try
					{
						ReadOnlyMemory<byte> propertyKey;
						while (!TryGetRawString(context, ref sequence, out propertyKeyArray, out propertyKey, out requestHint))
						{
							await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
						}

						// Use inlined trie, prefixed by the count as MP uint32.
						// char is UTF-8, big endian.
						// 1st node: [length(1-5)][chars(7-3)]
						switch (DeserializationTrie.GetOrDefault(propertyKey.Span))
						{
#if STRING
							case 0:
							{
								while (!TryDecodeValueOfName(context, ref sequence, out name, out requestHint))
								{
									await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
								}

								break;
							}
#endif
#if INT32
							case 1:
							{
								while (!TryDecodeValueOfAge(context, ref sequence, out age, out requestHint))
								{
									await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
								}

								break;
							}
#endif
#if BOOL
							case 2:
							{
								while (!TryDecodeValueOfIsActive(context, ref sequence, out isActive, out requestHint))
								{
									await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
								}

								break;
							}
#endif
#if COLLECTION
							case 3:
							{
								context.IncrementDepth();
								if (decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
								{
									long arrayLength;
									while (!TryDecodeArrayHeader(context, ref sequence, out arrayLength, out requestHint))
									{
										await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
									}

									// If settable
									//if (roles == null)
									//{
									//	roles = new List<string>(arrayLength); // or 0
									//}
									for (var j = 0; j < arrayLength; j++)
									{
										string item;
										while (!TryDecodeItemOfRoles(context, ref sequence, out item, out requestHint))
										{
											await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
										}

										roles.Add(item);
									}
								}
								else
								{
									CollectionItemIterator iterator;
									while (!TryDecodeArray(context, ref sequence, out iterator, out requestHint))
									{
										await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
									}

									while (!iterator.CollectionEnds(sequence, out requestHint))
									{
										if (requestHint != 0)
										{
											await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
										}

										string item;
										while (!TryDecodeItemOfRoles(context, ref sequence, out item, out requestHint))
										{
											await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
										}

										roles.Add(item);
									}

									while (!iterator.Drain(ref sequence, out requestHint))
									{
										await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
									}
								}
								context.DecrementDepth();
								break;
							}
							case 4:
							{
								context.IncrementDepth();
								if (decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
								{
									long mapCount;
									while (!TryDecodeMapHeader(context, ref sequence, out mapCount, out requestHint))
									{
										await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
									}

									// If settable
									//if (attributes == null)
									//{
									//	attributes = new Dictionary<string, string>(mapCount); // or 0
									//}
									for (var j = 0; j < mapCount; j++)
									{
										string key;
										while (!TryDecodeKeyOfAttributes(context, ref sequence, out key, out requestHint))
										{
											await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
										}
										string value;
										while (!TryDecodeValueOfAttributes(context, ref sequence, out value, out requestHint))
										{
											await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
										}

										attributes.Add(key, value);
									}
								}
								else
								{
									CollectionItemIterator iterator;
									while (!TryDecodeMap(context, ref sequence, out iterator, out requestHint))
									{
										await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
									}

									while (!iterator.CollectionEnds(sequence, out requestHint))
									{
										if (requestHint != 0)
										{
											await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
										}

										string key;
										while (!TryDecodeKeyOfAttributes(context, ref sequence, out key, out requestHint))
										{
											await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
										}
										string value;
										while (!TryDecodeValueOfAttributes(context, ref sequence, out value, out requestHint))
										{
											await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
										}

										attributes.Add(key, value);
									}

									while (!iterator.Drain(ref sequence, out requestHint))
									{
										await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
									}
								}
								context.DecrementDepth();
								break;
							}
#endif
							default:
							{
#warning TODO: TrySkip loop
								break;
							}
						}
					}
					finally
					{
						if (propertyKeyArray != null)
						{
							context.ByteBufferPool.Return(propertyKeyArray);
						}
					}
				}
			}

			if (context.Decoder.Options.Features.CanCountCollectionItems) // OPTIMIZABLE
			{
				itemsCount -= 5;
				while (!TryDrain(context, ref sequence, itemsCount, out requestHint))
				{
					await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
				}
			}
			else
			{
				while (!propertyIterator.Drain(ref sequence, out requestHint))
				{
					await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
				}
			}

			context.DecrementDepth();

			// Verbose for Deserialize...
#if STRING
			obj.Name = name;
#endif
#if INT32
			obj.Age = age;
#endif
#if BOOL
			obj.IsActive = isActive;
#endif
			// If settable
			// obj.Roles = roles;
			// If settable
			// obj.Attributes = attributes;
			return true;
		}
	}

	public sealed class SampleInt32ArraySerializer : ObjectSerializer<int[]>
	{
		public SampleInt32ArraySerializer(ObjectSerializationContext ownedContext)
			: base(ownedContext, SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize) { }

		public sealed override void Serialize(ref SerializationOperationContext context, int[] obj, IBufferWriter<byte> sink)
		{
			if (obj is null)
			{
				context.Encoder.EncodeNull(sink);
				return;
			}

			context.Encoder.EncodeArrayStart(obj.Length, sink, context.CollectionContext);
			for (var i = 0; i < obj.Length; i++)
			{
				context.Encoder.EncodeArrayItemStart(i, sink, context.CollectionContext);
				context.Encoder.EncodeInt32(obj[i], sink);
				context.Encoder.EncodeArrayItemEnd(i, sink, context.CollectionContext);
			}
			context.Encoder.EncodeArrayEnd(obj.Length, sink, context.CollectionContext);
		}

		public sealed override int[] Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			if (context.Decoder.Options.Features.CanCountCollectionItems)
			{
				var length = context.Decoder.DecodeArrayHeader(ref source);
				var result = new int[length];
				for (var i = 0; i < result.Length; i++)
				{
					result[i] = context.Decoder.DecodeInt32(ref source);
				}
				context.Decoder.Drain(ref source, context.CollectionContext, 0);
				return result;
			}
			else
			{
				var result = new List<int>();
				var iterator = context.Decoder.DecodeArray(ref source);
				while (!iterator.CollectionEnds(ref source))
				{
					result.Add(context.Decoder.DecodeInt32(ref source));
				}
				iterator.Drain(ref source);
				return result.ToArray();
			}
		}

		public sealed override ValueTask<int[]> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence streamSource)
		{
			throw new NotImplementedException();
		}

		public sealed override bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, in int[] obj)
		{
			throw new NotImplementedException();
		}

		public sealed override ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence streamSource, int[] obj)
		{
			throw new NotImplementedException();
		}
	}

	public sealed class SampleInt32Serializer : ObjectSerializer<int>
	{
		public SampleInt32Serializer(ObjectSerializationContext ownedContext)
			: base(ownedContext, SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize) { }

		public sealed override void Serialize(ref SerializationOperationContext context, int obj, IBufferWriter<byte> sink)
		{
			context.Encoder.EncodeInt32(obj, sink);
		}

		public sealed override int Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			return context.Decoder.DecodeInt32(ref source);
		}

		public sealed override ValueTask<int> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence streamSource)
		{
			throw new NotImplementedException();
		}

		public sealed override bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, in int obj)
		{
			throw new NotImplementedException();
		}

		public sealed override ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence streamSource, int obj)
		{
			throw new NotImplementedException();
		}
	}
}
