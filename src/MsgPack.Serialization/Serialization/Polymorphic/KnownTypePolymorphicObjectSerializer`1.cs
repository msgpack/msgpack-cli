// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
#if FEATURE_TAP
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Internal;

namespace MsgPack.Serialization.Polymorphic
{
	/// <summary>
	///		Implements polymorphic serializer which uses closed known types and interoperable ext-type feature.
	/// </summary>
	/// <typeparam name="T">The base type of the polymorhic member.</typeparam>
	internal sealed class KnownTypePolymorphicObjectSerializer<T> : ObjectSerializer<T>
	{
		private readonly PolymorphismSchema _schema;
		private readonly IDictionary<string, RuntimeTypeHandle> _typeHandleMap;
		private readonly IDictionary<RuntimeTypeHandle, string> _typeCodeMap;

		public KnownTypePolymorphicObjectSerializer(SerializerProvider ownerProvider, PolymorphismSchema schema)
			: base(ownerProvider, SerializerCapabilities.Serialize | SerializerCapabilities.Deserialize | SerializerCapabilities.DeserializeTo)
		{
			if (typeof(T).GetIsValueType())
			{
				Throw.ValueTypeCannotBePolymorphic(typeof(T));
			}

			this._schema = schema.FilterSelf();
			this._typeHandleMap = BuildTypeCodeTypeHandleMap(schema.CodeTypeMapping);
			this._typeCodeMap = BuildTypeHandleTypeCodeMap(schema.CodeTypeMapping);
		}

		private static IDictionary<string, RuntimeTypeHandle> BuildTypeCodeTypeHandleMap(IDictionary<string, Type> typeMap)
		{
			return typeMap.ToDictionary(kv => kv.Key, kv => kv.Value.TypeHandle);
		}

		private static IDictionary<RuntimeTypeHandle, string> BuildTypeHandleTypeCodeMap(IDictionary<string, Type> typeMap)
		{
			var result = new Dictionary<RuntimeTypeHandle, string>(typeMap.Count);
			foreach (var typeHandleTypeCodeMapping in typeMap.GroupBy(kv => kv.Value))
			{
				if (typeHandleTypeCodeMapping.Count() > 1)
				{
					Throw.TypeIsMappedToMultipleExtensionTypes(
						typeHandleTypeCodeMapping.Key,
						typeHandleTypeCodeMapping.Select(kv => kv.Key).ToArray()
					);
				}

				result.Add(typeHandleTypeCodeMapping.Key.TypeHandle, typeHandleTypeCodeMapping.First().Key);
			}

			return result;
		}

		private ObjectSerializer GetActualTypeSerializer(Type actualType)
		{
			var result = this.ObjectSerializerProvider.GetPolymorphicSerializer(actualType, this._schema);
			if (result == null)
			{
				Throw.CannotGetActualTypeSerializer(actualType);
			}

			return result!;
		}

		public sealed override void Serialize(ref SerializationOperationContext context, [AllowNull] T obj, IBufferWriter<byte> sink)
		{
			if (obj is null)
			{
				context.Encoder.EncodeNull(sink);
				return;
			}

			TypeInfoEncoder.EncodeStart(ref context, sink, this.GetTypeCode(obj));
			// T is ref type here, so boxing will not be occurred.
			this.GetActualTypeSerializer(obj.GetType()).SerializeObject(ref context, obj, sink);
			TypeInfoEncoder.EncodeEnd(ref context, sink);
		}

		private string GetTypeCode(T obj)
		{
			if (!this._typeCodeMap.TryGetValue(obj!.GetType().TypeHandle, out var typeCode))
			{
				Throw.TypeIsNotDefinedAsKnownType(obj.GetType());
				// Never reaches
			}

			return typeCode!;
		}


		[return: MaybeNull]
		public sealed override T Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
			=> TypeInfoEncoder.Decode(
					ref context,
					ref source,
					(c, p) => this.GetTypeFromCode(c, p),
					(Type t, ref DeserializationOperationContext x, ref SequenceReader<byte> s) => (T)this.GetActualTypeSerializer(t).DeserializeObject(ref x, ref s)!
				);

		private Type GetTypeFromCode(string typeCode, long position)
		{
			RuntimeTypeHandle typeHandle;
			if (!this._typeHandleMap.TryGetValue(typeCode, out typeHandle))
			{
				Throw.UnknownTypeCode(StringEscape.ForDisplay(typeCode), position);
			}

			return Type.GetTypeFromHandle(typeHandle);
		}

		public sealed override bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, T obj)
			=> this.GetActualTypeSerializer(Ensure.NotNull(obj).GetType()).DeserializeObjectTo(ref context, ref source, obj);

#if FEATURE_TAP
		public sealed override ValueTask<T> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
			=> TypeInfoEncoder.DecodeAsync(
					context,
					source,
					(c, p) => this.GetTypeFromCode(c, p),
					async (t, x, s) => ((T)await this.GetActualTypeSerializer(t).DeserializeObjectAsync(x, s).ConfigureAwait(false))!
				);

		public sealed override ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, T obj)
			=> this.GetActualTypeSerializer(Ensure.NotNull(obj).GetType()).DeserializeObjectToAsync(context, source, obj);
#endif // FEATURE_TAP
	}
}
