// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MsgPack.Internal;
using MsgPack.Serialization.Internal;

using PrimitiveEncoder = System.Action<MsgPack.Internal.FormatEncoder, object?, System.Buffers.IBufferWriter<byte>>;
using StringEncoder = System.Action<MsgPack.Internal.FormatEncoder, object?, System.Buffers.IBufferWriter<byte>, System.Text.Encoding?, System.Threading.CancellationToken>;
using BinaryEncoder = System.Action<MsgPack.Internal.FormatEncoder, object?, System.Buffers.IBufferWriter<byte>, System.Threading.CancellationToken>;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Implements <see cref="IReflectionObjectSerializer"/> for non collection objects and tuples.
	/// </summary>
	internal sealed partial class ObjectDelegateSerializer : IReflectionObjectSerializer
	{
		private readonly IReadOnlyList<Delegate> _memberValueEncoders;
		private readonly IReadOnlyList<Delegate> _memberValueDecoders;
		private readonly IReadOnlyList<bool> _memberIsCollections;
		private readonly IReadOnlyList<Func<object, object?>> _memberValueGetters;
		private readonly IReadOnlyList<Action<object, object?>>? _memberValueSetters;
		private readonly IReadOnlyDictionary<string, int> _memberIndexes;
		private readonly SerializationTarget _target;
		private readonly IReadOnlyList<Type>? _tupleTypes;
#if FEATURE_TAP
		private readonly IReadOnlyList<Delegate> _asyncMemberValueDecoders;
#endif // FEATURE_TAP

		// object
		// tuple
		// value tuple
		public ObjectDelegateSerializer(ObjectSerializationContext context, SerializationTarget target)
		{
			this._target = target;
			var isTuple = TupleItems.IsTuple(target.Type);
			this._tupleTypes = isTuple ? null : TupleItems.CreateTupleTypeList(target.Type);
			this._memberValueEncoders = CreateMemberValueEncoders(context, target.Members).ToList();
			this._memberValueDecoders =
				CreateMemberValueDecoders(
					context,
					target.Members,
					hasDeserializationConstructor: target.DeserializationConstructor != null,
					hasPriviledgedAccess: !context.Options.DisablePrivilegedAccess
				).ToList();
			this._memberIsCollections = DetermineWhetherIndividualMembersAreCollection(target.Members).ToList();
			this._memberValueGetters = CreateMemberValueGetters(target.Members, this._tupleTypes).ToList();
			this._memberValueSetters = this._tupleTypes == null ? null : CreateMemberValueSetters(target.Members).ToList();
			this._memberIndexes = target.Members.Select((m, i) => (Name: m.MemberName, Index: i)).ToDictionary(x => x.Name ?? $"Item{x.Index}", x => x.Index);
#if FEATURE_TAP
			this._asyncMemberValueDecoders =
				CreateAsyncMemberValueDecoders(
					context,
					target.Members,
					hasDeserializationConstructor: target.DeserializationConstructor != null,
					hasPriviledgedAccess: !context.Options.DisablePrivilegedAccess
				).ToList();
#endif // FEATURE_TAP
		}

		private static IEnumerable<Delegate> CreateMemberValueEncoders(ObjectSerializationContext context, IEnumerable<SerializingMember> members)
		{
			foreach (var member in members)
			{
				var type = member.Member.GetMemberValueType();
				var memberInfo = member.Member;
				yield return
					type switch
					{
						var t when t == typeof(sbyte) || t == typeof(short) || t == typeof(int) => EncodeInt32NonNull,
						var t when t == typeof(sbyte?) || t == typeof(short?) || t == typeof(int?) => EncodeInt32Nullable,
						var t when t == typeof(long) => EncodeInt64NonNull,
						var t when t == typeof(long?) => EncodeInt64Nullable,
						var t when t == typeof(byte) || t == typeof(ushort) || t == typeof(uint) => EncodeUInt32NonNull,
						var t when t == typeof(byte?) || t == typeof(ushort?) || t == typeof(uint?) => EncodeUInt32Nullable,
						var t when t == typeof(ulong) => EncodeUInt64NonNull,
						var t when t == typeof(ulong?) => EncodeUInt64Nullable,
						var t when t == typeof(bool) => EncodeBooleanNonNull,
						var t when t == typeof(bool?) => EncodeBooleanNullable,
						var t when t == typeof(float) => EncodeSingleNonNull,
						var t when t == typeof(float?) => EncodeSingleNullable,
						var t when t == typeof(double) => EncodeDoubleNonNull,
						var t when t == typeof(double?) => EncodeDoubleNullable,
						var t
							when t == typeof(string) || t == typeof(StringBuilder) || t == typeof(char[])
							|| t == typeof(ReadOnlyMemory<char>) || t == typeof(Memory<char>) || t == typeof(ReadOnlySequence<char>) || t == typeof(IEnumerable<char>)
								=> EncodeString,
						var t when t == typeof(ReadOnlySpan<char>) => ReflectionSerializerThrow.ByRefLikeIsNotSupported(memberInfo),
						var t when t == typeof(Span<char>) => ReflectionSerializerThrow.ByRefLikeIsNotSupported(memberInfo),
						var t when t == typeof(byte[]) || t == typeof(ReadOnlyMemory<byte>) || t == typeof(Memory<byte>) || t == typeof(ReadOnlySequence<byte>) || t == typeof(IEnumerable<byte>)
							=> EncodeBinary,
						var t when t == typeof(ReadOnlySpan<byte>) => ReflectionSerializerThrow.ByRefLikeIsNotSupported(memberInfo),
						var t when t == typeof(Span<byte>) => ReflectionSerializerThrow.ByRefLikeIsNotSupported(memberInfo),
#warning TODO: PolymorphismSchema
						_ => new Serialization(context.GetSerializer(type, null).SerializeObject)
					};
			}
		}

		private static IEnumerable<Delegate> CreateMemberValueDecoders(ObjectSerializationContext context, IEnumerable<SerializingMember> members, bool hasDeserializationConstructor, bool hasPriviledgedAccess)
		{
			foreach (var member in members)
			{
				var type = member.Member.GetMemberValueType();
				var item = (Type: type, IsNullable: member.Member!.IsNullable());
				var memberInfo = member.Member;
				yield return
					item switch
					{
						var x when x.Type == typeof(sbyte) => DecodeSByteNonNull,
						var x when x.Type == typeof(short) => DecodeInt16NonNull,
						var x when x.Type == typeof(int) => DecodeInt32NonNull,
						var x when x.Type == typeof(sbyte?) => DecodeSByteNullable,
						var x when x.Type == typeof(short?) => DecodeInt16Nullable,
						var x when x.Type == typeof(int?) => DecodeInt32Nullable,
						var x when x.Type == typeof(long) => DecodeInt64NonNull,
						var x when x.Type == typeof(long?) => DecodeInt64Nullable,
						var x when x.Type == typeof(byte) => DecodeByteNonNull,
						var x when x.Type == typeof(ushort) => DecodeUInt16NonNull,
						var x when x.Type == typeof(uint) => DecodeUInt32NonNull,
						var x when x.Type == typeof(byte?) => DecodeByteNullable,
						var x when x.Type == typeof(ushort?) => DecodeUInt16Nullable,
						var x when x.Type == typeof(uint?) => DecodeUInt32Nullable,
						var x when x.Type == typeof(ulong) => DecodeUInt64NonNull,
						var x when x.Type == typeof(ulong?) => DecodeUInt64Nullable,
						var x when x.Type == typeof(bool) => DecodeBooleanNonNull,
						var x when x.Type == typeof(bool?) => DecodeBooleanNullable,
						var x when x.Type == typeof(float) => DecodeSingleNonNull,
						var x when x.Type == typeof(float?) => DecodeSingleNullable,
						var x when x.Type == typeof(double) => DecodeDoubleNonNull,
						var x when x.Type == typeof(double?) => DecodeDoubleNullable,
						var x when (x.Type == typeof(string) || x.Type == typeof(IEnumerable<char>)) && !x.IsNullable => DecodeStringNonNull,
						var x when (x.Type == typeof(string) || x.Type == typeof(IEnumerable<char>)) && x.IsNullable => DecodeStringNullable,
						var x when x.Type == typeof(StringBuilder) && !x.IsNullable => DecodeStringBuilderNonNull,
						var x when x.Type == typeof(StringBuilder) && x.IsNullable => DecodeStringBuilderNullable,
						var x when x.Type == typeof(char[]) && !x.IsNullable => DecodeCharArrayNonNull,
						var x when x.Type == typeof(char[]) && x.IsNullable => DecodeCharArrayNullable,
						var x when x.Type == typeof(ReadOnlyMemory<char>) && !x.IsNullable => DecodeReadOnlyMemoryOfCharNonNull,
						var x when x.Type == typeof(ReadOnlyMemory<char>) && x.IsNullable => DecodeReadOnlyMemoryOfCharNullable,
						var x when x.Type == typeof(Memory<char>) && !x.IsNullable => DecodeMemoryOfCharNonNull,
						var x when x.Type == typeof(Memory<char>) && x.IsNullable => DecodeMemoryOfCharNullable,
						var x when x.Type == typeof(ReadOnlySequence<char>) && !x.IsNullable => DecodeReadOnlySequenceOfCharNonNull,
						var x when x.Type == typeof(ReadOnlySequence<char>) && x.IsNullable => DecodeReadOnlySequenceOfCharNullable,
						var x when x.Type == typeof(ReadOnlySpan<char>) => ReflectionSerializerThrow.ByRefLikeIsNotSupported(memberInfo),
						var x when x.Type == typeof(Span<char>) => ReflectionSerializerThrow.ByRefLikeIsNotSupported(memberInfo),
						var x when (x.Type == typeof(byte[]) || x.Type == typeof(IEnumerable<byte>)) && !x.IsNullable => DecodeByteArrayNonNull,
						var x when (x.Type == typeof(byte[]) || x.Type == typeof(IEnumerable<byte>)) && x.IsNullable => DecodeByteArrayNullable,
						var x when x.Type == typeof(ReadOnlyMemory<byte>) && !x.IsNullable => DecodeReadOnlyMemoryOfByteNonNull,
						var x when x.Type == typeof(ReadOnlyMemory<byte>) && x.IsNullable => DecodeReadOnlyMemoryOfByteNullable,
						var x when x.Type == typeof(Memory<byte>) && !x.IsNullable => DecodeMemoryOfCharNonNull,
						var x when x.Type == typeof(Memory<byte>) && x.IsNullable => DecodeMemoryOfCharNullable,
						var x when x.Type == typeof(ReadOnlySequence<byte>) && !x.IsNullable => DecodeReadOnlySequenceOfByteNonNull,
						var x when x.Type == typeof(ReadOnlySequence<byte>) && x.IsNullable => DecodeReadOnlySequenceOfByteNullable,
						var x when x.Type == typeof(ReadOnlySpan<byte>) => ReflectionSerializerThrow.ByRefLikeIsNotSupported(memberInfo),
						var x when x.Type == typeof(Span<byte>) => ReflectionSerializerThrow.ByRefLikeIsNotSupported(memberInfo),
#warning TODO: PolymorphismSchema
						_ =>
							hasDeserializationConstructor ?
								new Deserialization(context.GetSerializer(type, null).DeserializeObject) :
								memberInfo switch
								{
									PropertyInfo p =>
										p switch
										{
											var pi when pi.GetSetMethod(nonPublic: hasPriviledgedAccess) != null => new Deserialization(context.GetSerializer(type, null).DeserializeObject),
											_ => new DeserializingFill(context.GetSerializer(type, null).DeserializeObjectTo),
										},
									FieldInfo f =>
										f switch
										{
											var fi when !fi.IsInitOnly => new Deserialization(context.GetSerializer(type, null).DeserializeObject),
											_ => new DeserializingFill(context.GetSerializer(type, null).DeserializeObjectTo),
										},
									_ => ReflectionSerializerThrow.UnexpectedMemberType(memberInfo)
								}
					};
			}
		}

#if FEATURE_TAP

		private static IEnumerable<Delegate> CreateAsyncMemberValueDecoders(ObjectSerializationContext context, IEnumerable<SerializingMember> members, bool hasDeserializationConstructor, bool hasPriviledgedAccess)
		{
			foreach (var member in members)
			{
				var type = member.Member.GetMemberValueType();
				var item = (Type: type, IsNullable: member.Member!.IsNullable());
				var memberInfo = member.Member;
				yield return
					item switch
					{
						var x when x.Type == typeof(sbyte) => DecodeSByteNonNullAsync,
						var x when x.Type == typeof(short) => DecodeInt16NonNullAsync,
						var x when x.Type == typeof(int) => DecodeInt32NonNullAsync,
						var x when x.Type == typeof(sbyte?) => DecodeSByteNullableAsync,
						var x when x.Type == typeof(short?) => DecodeInt16NullableAsync,
						var x when x.Type == typeof(int?) => DecodeInt32NullableAsync,
						var x when x.Type == typeof(long) => DecodeInt64NonNullAsync,
						var x when x.Type == typeof(long?) => DecodeInt64NullableAsync,
						var x when x.Type == typeof(byte) => DecodeByteNonNullAsync,
						var x when x.Type == typeof(ushort) => DecodeUInt16NonNullAsync,
						var x when x.Type == typeof(uint) => DecodeUInt32NonNullAsync,
						var x when x.Type == typeof(byte?) => DecodeByteNullableAsync,
						var x when x.Type == typeof(ushort?) => DecodeUInt16NullableAsync,
						var x when x.Type == typeof(uint?) => DecodeUInt32NullableAsync,
						var x when x.Type == typeof(ulong) => DecodeUInt64NonNullAsync,
						var x when x.Type == typeof(ulong?) => DecodeUInt64NullableAsync,
						var x when x.Type == typeof(bool) => DecodeBooleanNonNullAsync,
						var x when x.Type == typeof(bool?) => DecodeBooleanNullableAsync,
						var x when x.Type == typeof(float) => DecodeSingleNonNullAsync,
						var x when x.Type == typeof(float?) => DecodeSingleNullableAsync,
						var x when x.Type == typeof(double) => DecodeDoubleNonNullAsync,
						var x when x.Type == typeof(double?) => DecodeDoubleNullableAsync,
						var x when (x.Type == typeof(string) || x.Type == typeof(IEnumerable<char>)) && !x.IsNullable => DecodeStringNonNullAsync,
						var x when (x.Type == typeof(string) || x.Type == typeof(IEnumerable<char>)) && x.IsNullable => DecodeStringNullableAsync,
						var x when x.Type == typeof(StringBuilder) && !x.IsNullable => DecodeStringBuilderNonNullAsync,
						var x when x.Type == typeof(StringBuilder) && x.IsNullable => DecodeStringBuilderNullableAsync,
						var x when x.Type == typeof(char[]) && !x.IsNullable => DecodeCharArrayNonNullAsync,
						var x when x.Type == typeof(char[]) && x.IsNullable => DecodeCharArrayNullableAsync,
						var x when x.Type == typeof(ReadOnlyMemory<char>) && !x.IsNullable => DecodeReadOnlyMemoryOfCharNonNullAsync,
						var x when x.Type == typeof(ReadOnlyMemory<char>) && x.IsNullable => DecodeReadOnlyMemoryOfCharNullableAsync,
						var x when x.Type == typeof(Memory<char>) && !x.IsNullable => DecodeMemoryOfCharNonNullAsync,
						var x when x.Type == typeof(Memory<char>) && x.IsNullable => DecodeMemoryOfCharNullableAsync,
						var x when x.Type == typeof(ReadOnlySequence<char>) && !x.IsNullable => DecodeReadOnlySequenceOfCharNonNullAsync,
						var x when x.Type == typeof(ReadOnlySequence<char>) && x.IsNullable => DecodeReadOnlySequenceOfCharNullableAsync,
						var x when x.Type == typeof(ReadOnlySpan<char>) => ReflectionSerializerThrow.ByRefLikeIsNotSupported(memberInfo),
						var x when x.Type == typeof(Span<char>) => ReflectionSerializerThrow.ByRefLikeIsNotSupported(memberInfo),
						var x when (x.Type == typeof(byte[]) || x.Type == typeof(IEnumerable<byte>)) && !x.IsNullable => DecodeByteArrayNonNullAsync,
						var x when (x.Type == typeof(byte[]) || x.Type == typeof(IEnumerable<byte>)) && x.IsNullable => DecodeByteArrayNullableAsync,
						var x when x.Type == typeof(ReadOnlyMemory<byte>) && !x.IsNullable => DecodeReadOnlyMemoryOfByteNonNullAsync,
						var x when x.Type == typeof(ReadOnlyMemory<byte>) && x.IsNullable => DecodeReadOnlyMemoryOfByteNullableAsync,
						var x when x.Type == typeof(Memory<byte>) && !x.IsNullable => DecodeMemoryOfCharNonNullAsync,
						var x when x.Type == typeof(Memory<byte>) && x.IsNullable => DecodeMemoryOfCharNullableAsync,
						var x when x.Type == typeof(ReadOnlySequence<byte>) && !x.IsNullable => DecodeReadOnlySequenceOfByteNonNullAsync,
						var x when x.Type == typeof(ReadOnlySequence<byte>) && x.IsNullable => DecodeReadOnlySequenceOfByteNullableAsync,
						var x when x.Type == typeof(ReadOnlySpan<byte>) => ReflectionSerializerThrow.ByRefLikeIsNotSupported(memberInfo),
						var x when x.Type == typeof(Span<byte>) => ReflectionSerializerThrow.ByRefLikeIsNotSupported(memberInfo),
#warning TODO: PolymorphismSchema
						_ =>
							hasDeserializationConstructor ?
								new AsyncDeserialization(context.GetSerializer(type, null).DeserializeObjectAsync) :
								memberInfo switch
								{
									PropertyInfo p =>
										p switch
										{
											var pi when pi.GetSetMethod(nonPublic: hasPriviledgedAccess) != null => new Deserialization(context.GetSerializer(type, null).DeserializeObject),
											_ => new AsyncDeserializingFill(context.GetSerializer(type, null).DeserializeObjectToAsync),
										},
									FieldInfo f =>
										f switch
										{
											var fi when !fi.IsInitOnly => new Deserialization(context.GetSerializer(type, null).DeserializeObject),
											_ => new AsyncDeserializingFill(context.GetSerializer(type, null).DeserializeObjectToAsync),
										},
									_ => ReflectionSerializerThrow.UnexpectedMemberType(memberInfo)
								}
					};
			}
		}

#endif // FEATURE_TAP

		private static IEnumerable<bool> DetermineWhetherIndividualMembersAreCollection(IEnumerable<SerializingMember> members)
		{
			foreach (var member in members)
			{
				yield return member.Member?.GetMemberValueType()?.GetCollectionTraits(CollectionTraitOptions.None, allowNonCollectionEnumerableTypes: false).CollectionType != CollectionKind.NotCollection;
			}
		}

		private static IEnumerable<Func<object, object?>> CreateMemberValueGetters(IReadOnlyList<SerializingMember> members, IReadOnlyList<Type>? tupleTypes)
		{
			var nestAccessorList = new List<Func<object, object>>(tupleTypes?.Count ?? 0);
			// immutable array for closure.
			var nestAccessors = Array.Empty<Func<object, object>>();

			for (var i = 0; i < members.Count; i++)
			{
				Func<object, object?>? itemAccessor = null;

				if (tupleTypes != null)
				{
					var itemIndex = Math.DivRem(i, 7, out var depth) + 1;
					if (itemIndex == 1 && depth > 0)
					{
						var restProperty = tupleTypes[depth - 1].GetProperty("Rest")!;
						nestAccessorList.Add(t => restProperty.GetValue(t)!);
						nestAccessors = nestAccessorList.ToArray();
					}

					var itemProperty = tupleTypes[depth].GetProperty($"Item{itemIndex}")!;
					itemAccessor = t => itemProperty.GetValue(t)!;
				}

				var member = members[i];

				if (tupleTypes != null)
				{
					Debug.Assert(itemAccessor != null);

					yield return
						t =>
						{
							foreach (var nestAccessor in nestAccessors)
							{
								t = nestAccessor(t);
							}

							return itemAccessor!(t);
						};
				}
				else if (member.Member is PropertyInfo asProperty)
				{
					yield return o => asProperty.GetValue(o, BindingFlags.DoNotWrapExceptions, binder: null, index: null, culture: null);
				}
				else if (member.Member is FieldInfo asField)
				{
					yield return o => asField.GetValue(o);
				}
				else
				{
					ReflectionSerializerThrow.UnexpectedMemberType(member.Member);
				}
			}
		}

		private static IEnumerable<Action<object, object?>> CreateMemberValueSetters(IReadOnlyList<SerializingMember> members)
		{
			// Tuple is deserialized via constructor, so this method will not be called.

			for (var i = 0; i < members.Count; i++)
			{
				var member = members[i];

				if (member.Member is PropertyInfo asProperty)
				{
					yield return (o, v) => asProperty.SetValue(o, v, BindingFlags.DoNotWrapExceptions, binder: null, index: null, culture: null);
				}
				else if (member.Member is FieldInfo asField)
				{
					yield return (o, v) => asField.SetValue(o, v);
				}
				else
				{
					ReflectionSerializerThrow.UnexpectedMemberType(member.Member);
				}
			}
		}

		public void Serialize(ref SerializationOperationContext context, object? obj, IBufferWriter<byte> sink)
		{
			var encoder = context.Encoder;

			if (obj is null)
			{
				context.Encoder.EncodeNull(sink);
				return;
			}

			if (context.SerializationMethod == SerializationMethod.Array)
			{
				encoder.EncodeArrayStart(this._memberValueEncoders.Count, sink, context.CollectionContext);

				for (var i = 0; i < this._memberValueEncoders.Count; i++)
				{
					var value = this._memberValueGetters[i](obj);

					encoder.EncodeArrayItemStart(i, sink, context.CollectionContext);
					if (this._memberValueEncoders[i] is PrimitiveEncoder primitiveEncoder)
					{
						primitiveEncoder(encoder, value, sink);
					}
					else if (this._memberValueEncoders[i] is StringEncoder stringEncoder)
					{
						var encoding = this._target.Members[i].Member!.GetCustomAttribute<StringEncodingAttribute>()?.Encoding ?? context.StringEncoding;
						stringEncoder(encoder, value, sink, encoding, context.CancellationToken);
					}
					else if (this._memberValueEncoders[i] is BinaryEncoder binaryEncoder)
					{
						binaryEncoder(encoder, value, sink, context.CancellationToken);
					}
					else if (this._memberValueEncoders[i] is Serialization serializer)
					{
						if (this._memberIsCollections[i])
						{
							context.IncrementDepth();
						}

						serializer(ref context, value, sink);

						if (this._memberIsCollections[i])
						{
							context.DecrementDepth();
						}
					}
					else
					{
						ReflectionSerializerThrow.UnexpectedEncoderDelegate(this._memberValueEncoders[i]);
					}

					encoder.EncodeArrayItemEnd(i, sink, context.CollectionContext);
				}

				encoder.EncodeArrayEnd(this._memberValueEncoders.Count, sink, context.CollectionContext);
			}
			else
			{
				encoder.EncodeMapStart(this._memberValueEncoders.Count, sink, context.CollectionContext);

				for (var i = 0; i < this._memberValueEncoders.Count; i++)
				{
					var value = this._memberValueGetters[i](obj);

					encoder.EncodeMapKeyStart(i, sink, context.CollectionContext);
					encoder.EncodeRawString(this._target.Members[i].Utf8MemberName!.AsSpan().Bytes, this._target.Members[i].MemberName!.Length, sink, context.CancellationToken);
					encoder.EncodeMapKeyStart(i, sink, context.CollectionContext);

					encoder.EncodeMapValueStart(i, sink, context.CollectionContext);
					if (this._memberValueEncoders[i] is PrimitiveEncoder primitiveEncoder)
					{
						primitiveEncoder(encoder, value, sink);
					}
					else if (this._memberValueEncoders[i] is StringEncoder stringEncoder)
					{
						var encoding = this._target.Members[i].Member!.GetCustomAttribute<StringEncodingAttribute>()?.Encoding ?? context.StringEncoding;
						stringEncoder(encoder, value, sink, encoding, context.CancellationToken);
					}
					else if (this._memberValueEncoders[i] is BinaryEncoder binaryEncoder)
					{
						binaryEncoder(encoder, value, sink, context.CancellationToken);
					}
					else if (this._memberValueEncoders[i] is Serialization serializer)
					{
						serializer(ref context, value, sink);
					}
					else
					{
						ReflectionSerializerThrow.UnexpectedEncoderDelegate(this._memberValueEncoders[i]);
					}

					encoder.EncodeMapValueEnd(i, sink, context.CollectionContext);
				}

				encoder.EncodeMapEnd(this._memberValueEncoders.Count, sink, context.CollectionContext);
			}
		}

		public object? Deserialize(ref DeserializationOperationContext context, ref SequenceReader<byte> source)
		{
			if (context.Decoder.TryDecodeNull(ref source))
			{
				return null;
			}

			object? result = null;
			var constructorParameters = this._target.DeserializationConstructor?.GetParameters() ?? Array.Empty<ParameterInfo>();
			Dictionary<string, object?>? deserializedMembers = null;

			if (this._target.DeserializationConstructor == null)
			{
				result = Activator.CreateInstance(this._target.Type);
			}
			else
			{
				deserializedMembers = new Dictionary<string, object?>(constructorParameters.Length);
			}

			var decoder = context.Decoder;

			if (decoder.Options.Features.CanCountCollectionItems)
			{
				var collectionType = decoder.DecodeArrayOrMapHeader(ref source, out var itemsCount);
				for (var i = 0; i < itemsCount; i++)
				{
					this.DeserializeMember(ref context, ref source, result, deserializedMembers, i, collectionType);
				}
			}
			else
			{
				var collectionType = decoder.DecodeArrayOrMap(ref source, out var iterator);
				for (var i = 0; !iterator.CollectionEnds(ref source); i++)
				{
					this.DeserializeMember(ref context, ref source, result, deserializedMembers, i, collectionType);
					i++;
				}
			}

			if (result == null)
			{
				Debug.Assert(deserializedMembers != null);
				return this.CreateInstanceFromDeserializedMembers(constructorParameters, deserializedMembers);
			}
			else
			{
				return result;
			}
		}

		public async ValueTask<object?> DeserializeAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source)
		{
			await source.FetchAsync(context.CancellationToken).ConfigureAwait(false);
			
			if (await context.Decoder.TryDecodeNullAsync(source, context.CancellationToken).ConfigureAwait(false))
			{
				return null;
			}

			object? result = null;
			var constructorParameters = this._target.DeserializationConstructor?.GetParameters() ?? Array.Empty<ParameterInfo>();
			Dictionary<string, object?>? deserializedMembers = null;

			if (this._target.DeserializationConstructor == null)
			{
				result = Activator.CreateInstance(this._target.Type);
			}
			else
			{
				deserializedMembers = new Dictionary<string, object?>(constructorParameters.Length);
			}

			var decoder = context.Decoder;

			if (decoder.Options.Features.CanCountCollectionItems)
			{
				var collectionTypeAndCount = await decoder.DecodeArrayOrMapHeaderAsync(source, context.CancellationToken).ConfigureAwait(false);
				var collectionType = collectionTypeAndCount.CollectionType;
				var itemsCount = collectionTypeAndCount.ItemsCount;
				for (var i = 0; i < itemsCount; i++)
				{
					await this.DeserializeMemberAsync(context, source, result, deserializedMembers, i, collectionType).ConfigureAwait(false);
				}
			}
			else
			{
				var collectionTypeAndIterator = await decoder.DecodeArrayOrMapAsync(source, context.CancellationToken).ConfigureAwait(false);
				var collectionType = collectionTypeAndIterator.CollectionType;
				var iterator = collectionTypeAndIterator.Iterator;
				for (var i = 0; !(await iterator.CollectionEndsAsync(source, context.CancellationToken).ConfigureAwait(false)); i++)
				{
					await this.DeserializeMemberAsync(context, source, result, deserializedMembers, i, collectionType).ConfigureAwait(false);
					i++;
				}
			}

			if (result == null)
			{
				Debug.Assert(deserializedMembers != null);
				return this.CreateInstanceFromDeserializedMembers(constructorParameters, deserializedMembers);
			}
			else
			{
				return result;
			}
		}

		private void DeserializeMember(
			ref DeserializationOperationContext context,
			ref SequenceReader<byte> source,
			object? result,
			Dictionary<string, object?>? deserializedMembers,
			int i,
			CollectionType collectionType
		)
		{
			var decoder = context.Decoder;
			string property;
			int index;
			if (collectionType.IsMap)
			{
				property = decoder.DecodeString(ref source, encoding: null, context.CancellationToken);
				if (!this._memberIndexes.TryGetValue(property, out index))
				{
					// skip value of unknown member.
					decoder.Skip(ref source, context.CollectionContext, context.CancellationToken);
					return;
				}
			}
			else
			{
				if (i > this._target.Members.Count)
				{
					// skip extra member.
					decoder.Skip(ref source, context.CollectionContext, context.CancellationToken);
					return;
				}

				index = i;
				property = this._target.Members[i].MemberName ?? $"Item{i}";
			}

			object? value;
			if (this._memberValueDecoders[index] is PrimitiveDecoder primitiveDecoder)
			{
				value = primitiveDecoder(decoder, ref source);
			}
			else if (this._memberValueDecoders[index] is StringDecoder stringDecoder)
			{
				value = stringDecoder(decoder, ref source, context.StringEncoding, context.CancellationToken);
			}
			else if (this._memberValueDecoders[index] is BinaryDecoder binaryDecoder)
			{
				value = binaryDecoder(decoder, ref source, context.CancellationToken);
			}
			else if (this._memberValueDecoders[index] is Deserialization deserializer)
			{
				if (this._memberIsCollections[index])
				{
					context.IncrementDepth();
				}

				value = deserializer(ref context, ref source);

				if (this._memberIsCollections[index])
				{
					context.DecrementDepth();
				}
			}
			else if (this._memberValueDecoders[index] is DeserializingFill filler)
			{
				Debug.Assert(result != null);

				if (this._memberIsCollections[index])
				{
					context.IncrementDepth();
				}

				value = this._memberValueGetters[index](result);
				if (value == null)
				{
					Throw.UnsettableCollectionMemberMustNotBeNull(this._target.Members[i].Member);
				}

				filler(ref context, ref source, value!);


				if (this._memberIsCollections[index])
				{
					context.DecrementDepth();
				}

				// Assign is not required.
				return;
			}
			else
			{
				ReflectionSerializerThrow.UnexpectedDecoderDelegate(this._memberValueDecoders[i]);
				value = null;
			}

			if (result != null)
			{
				Debug.Assert(this._memberValueSetters != null);
				this._memberValueSetters[index](result, value);
			}
			else
			{
				Debug.Assert(deserializedMembers != null);
				deserializedMembers.Add(property, value);
			}
		}


		private async ValueTask DeserializeMemberAsync(
			AsyncDeserializationOperationContext context,
			ReadOnlyStreamSequence source,
			object? result,
			Dictionary<string, object?>? deserializedMembers,
			int i,
			CollectionType collectionType
		)
		{
			var decoder = context.Decoder;
			string property;
			int index;
			if (collectionType.IsMap)
			{
				property = await decoder.DecodeStringAsync(source, context.CancellationToken).ConfigureAwait(false);
				if (!this._memberIndexes.TryGetValue(property, out index))
				{
					// skip value of unknown member.
					await decoder.SkipAsync(source, context.CollectionContext, context.CancellationToken).ConfigureAwait(false);
					return;
				}
			}
			else
			{
				if (i > this._target.Members.Count)
				{
					// skip extra member.
					await decoder.SkipAsync(source, context.CollectionContext, context.CancellationToken).ConfigureAwait(false);
					return;
				}

				index = i;
				property = this._target.Members[i].MemberName ?? $"Item{i}";
			}

			object? value;
			if (this._asyncMemberValueDecoders[index] is AsyncPrimitiveDecoder asyncPrimitiveDecoder)
			{
				value = await asyncPrimitiveDecoder(decoder, source, context.CancellationToken).ConfigureAwait(false);
			}
			else if (this._asyncMemberValueDecoders[index] is AsyncStringDecoder asyncStringDecoder)
			{
				value = await asyncStringDecoder(decoder, source, context.StringEncoding, context.CancellationToken).ConfigureAwait(false);
			}
			else if (this._asyncMemberValueDecoders[index] is AsyncBinaryDecoder asyncBinaryDecoder)
			{
				value = await asyncBinaryDecoder(decoder, source, context.CancellationToken).ConfigureAwait(false);
			}
			else if (this._asyncMemberValueDecoders[index] is AsyncDeserialization asyncDeserializer)
			{
				if (this._memberIsCollections[index])
				{
					context.IncrementDepth();
				}

				value = await asyncDeserializer(context, source).ConfigureAwait(false);

				if (this._memberIsCollections[index])
				{
					context.DecrementDepth();
				}
			}
			else if (this._asyncMemberValueDecoders[index] is AsyncDeserializingFill asyncFiller)
			{
				Debug.Assert(result != null);

				if (this._memberIsCollections[index])
				{
					context.IncrementDepth();
				}

				value = this._memberValueGetters[index](result);
				if (value == null)
				{
					Throw.UnsettableCollectionMemberMustNotBeNull(this._target.Members[i].Member);
				}

				await asyncFiller(context, source, value!).ConfigureAwait(false);


				if (this._memberIsCollections[index])
				{
					context.DecrementDepth();
				}

				// Assign is not required.
				return;
			}
			else
			{
				ReflectionSerializerThrow.UnexpectedDecoderDelegate(this._memberValueDecoders[i]);
				value = null;
			}

			if (result != null)
			{
				Debug.Assert(this._memberValueSetters != null);
				this._memberValueSetters[index](result, value);
			}
			else
			{
				Debug.Assert(deserializedMembers != null);
				deserializedMembers.Add(property, value);
			}
		}

		private object? CreateInstanceFromDeserializedMembers(ParameterInfo[] constructorParameters, Dictionary<string, object?> deserializedMembers)
		{
			var constructorArguments =
				constructorParameters
				.Select((p, i) => (Type: p.ParameterType, CorrespondentMemberName: this._target.GetCorrespondentMemberName(i)))
				.Select(x => deserializedMembers.TryGetValue(x.CorrespondentMemberName ?? String.Empty, out var value) ? value : Activator.CreateInstance(x.Type))
				.ToList();

			if (this._tupleTypes != null)
			{
				object? rest = null;
				var tupleTypeStack = new Stack<Type>(this._tupleTypes);
				while (tupleTypeStack.Any())
				{
					var currentType = tupleTypeStack.Pop();
					var currentParameters = currentType.GetConstructors().Where(x => x.IsPublic && !x.IsStatic).Select(x => x.GetParameters()).SingleOrDefault(x => x.Length > 0);
					var fillingArgumentsCount = currentParameters.Length == 8 ? 7 : currentParameters.Length;
					var currentArguments = new object?[currentParameters.Length];
					var offset = constructorArguments.Count - fillingArgumentsCount;
					for (var i = 0; i < fillingArgumentsCount; i++)
					{
						currentArguments[i] = constructorArguments[offset + i];
					}
					constructorArguments.RemoveRange(offset, fillingArgumentsCount);

					if (currentArguments.Length == 8)
					{
						currentArguments[7] = rest;
					}

					rest = Activator.CreateInstance(currentType, currentArguments);
				}

				return rest;
			}
			else
			{
				return Activator.CreateInstance(this._target.Type, constructorArguments)!;
			}
		}

		public bool DeserializeTo(ref DeserializationOperationContext context, ref SequenceReader<byte> source, object obj)
		{
			Throw.DeserializeToOnlyAvailableForMutableCollection(this._target.Type);
			// never
			return default;
		}

		public ValueTask<bool> DeserializeToAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, object obj)
		{
			Throw.DeserializeToOnlyAvailableForMutableCollection(this._target.Type);
			// never
			return default;
		}
	}
}
