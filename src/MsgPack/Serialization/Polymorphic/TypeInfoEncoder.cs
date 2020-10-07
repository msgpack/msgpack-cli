// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using MsgPack.Internal;

namespace MsgPack.Serialization.Polymorphic
{
	/// <summary>
	/// 	Implements type info encoding for type embedding.
	/// </summary>
	internal static class TypeInfoEncoder
	{
		public delegate T Deserialization<T>(Type decodedType, ref DeserializationOperationContext context, ref SequenceReader<byte> source);
		public delegate Type TypeDecoding(ref DeserializationOperationContext context, ref SequenceReader<byte> source);

		private const string Elipsis = ".";

		public static void EncodeStart(ref SerializationOperationContext context, IBufferWriter<byte> writer, string typeCode)
		{
			var encoder = context.Encoder;

			context.IncrementDepth();
			var collectionContext = context.CollectionContext;
			encoder.EncodeArrayStart(2, writer, collectionContext);
			encoder.EncodeArrayItemStart(0, writer, context.CollectionContext);
			encoder.EncodeString(typeCode, writer, context.Options.DefaultStringEncoding, context.CancellationToken);
			encoder.EncodeArrayItemEnd(0, writer, context.CollectionContext);
			encoder.EncodeArrayItemStart(1, writer, context.CollectionContext);
		}

		public static void EncodeEnd(ref SerializationOperationContext context, IBufferWriter<byte> writer)
		{
			var encoder = context.Encoder;

			encoder.EncodeArrayItemEnd(1, writer, context.CollectionContext);
			encoder.EncodeArrayEnd(2, writer, context.CollectionContext);
			context.DecrementDepth();
		}

		public static void EncodeStart(ref SerializationOperationContext context, IBufferWriter<byte> writer, Type type)
		{
			var encoder = context.Encoder;

			var assemblyName = new AssemblyName(type.GetAssembly().FullName!);

			context.IncrementDepth();
			encoder.EncodeArrayStart(2, writer, context.CollectionContext);
			context.IncrementDepth();
			encoder.EncodeArrayItemStart(0, writer, context.CollectionContext);
			encoder.EncodeArrayStart(6, writer, context.CollectionContext);

			encoder.EncodeArrayItemStart(0, writer, context.CollectionContext);
			encoder.EncodeUInt32((byte)TypeInfoEncoding.RawCompressed, writer);
			encoder.EncodeArrayItemEnd(0, writer, context.CollectionContext);

			// Omit namespace prefix when it equals to declaring assembly simple name.
			var compressedTypeName =
				(type.Namespace != null && type.Namespace.StartsWith(assemblyName.Name!, StringComparison.Ordinal))
					? Elipsis + type.FullName!.Substring(assemblyName.Name!.Length + 1)
					: type.FullName;
			Span<byte> version = stackalloc byte[16];
			BinaryPrimitives.WriteInt32LittleEndian(version, assemblyName.Version!.Major);
			BinaryPrimitives.WriteInt32LittleEndian(version.Slice(sizeof(int)), assemblyName.Version.Minor);
			BinaryPrimitives.WriteInt32LittleEndian(version.Slice(sizeof(int) * 2), assemblyName.Version.Build);
			BinaryPrimitives.WriteInt32LittleEndian(version.Slice(sizeof(int) * 3), assemblyName.Version.Revision);

			encoder.EncodeArrayItemStart(1, writer, context.CollectionContext);
			encoder.EncodeString(compressedTypeName, writer, context.Options.DefaultStringEncoding, context.CancellationToken);
			encoder.EncodeArrayEnd(1, writer, context.CollectionContext);
			encoder.EncodeArrayItemStart(2, writer, context.CollectionContext);
			encoder.EncodeString(assemblyName.Name, writer, context.Options.DefaultStringEncoding, context.CancellationToken);
			encoder.EncodeArrayEnd(2, writer, context.CollectionContext);
			encoder.EncodeArrayItemStart(3, writer, context.CollectionContext);
			encoder.EncodeBinary(version, writer, context.CancellationToken);
			encoder.EncodeArrayEnd(3, writer, context.CollectionContext);
			encoder.EncodeArrayItemStart(4, writer, context.CollectionContext);
			encoder.EncodeString(assemblyName.GetCultureName(), writer, context.Options.DefaultStringEncoding, context.CancellationToken);
			encoder.EncodeArrayEnd(4, writer, context.CollectionContext);
			encoder.EncodeArrayItemStart(5, writer, context.CollectionContext);
			encoder.EncodeBinary(assemblyName.GetPublicKeyToken(), writer, context.CancellationToken);
			encoder.EncodeArrayEnd(5, writer, context.CollectionContext);
			context.DecrementDepth();
			encoder.EncodeArrayItemEnd(0, writer, context.CollectionContext);
		}

		public static T Decode<T>(
			ref DeserializationOperationContext context,
			ref SequenceReader<byte> source,
			Func<string, long, Type> typeFinder,
			Deserialization<T> deserialization
		)
		{
			var decoder = context.Decoder;
			var initialPosition = source.Consumed;
			long typeCodePosition;
			string? typeCode;

			context.IncrementDepth();
			try
			{
				CollectionItemIterator iterator = default;
				var canOmitIterator = decoder.Options.Features.CanCountCollectionItems;
				if (canOmitIterator)
				{
					if (decoder.DecodeArrayOrMapHeader(ref source, out var itemsCount) != CollectionType.Array || itemsCount != 2)
					{
						Throw.UnknownTypeEmbedding(initialPosition);
					}
				}
				else
				{
					if (decoder.DecodeArrayOrMap(ref source, out iterator) != CollectionType.Array)
					{
						Throw.UnknownTypeEmbedding(initialPosition);
					}
				}

				typeCodePosition = source.Consumed;
				typeCode = decoder.DecodeString(ref source, context.Options.DefaultStringEncoding, context.CancellationToken);
				if (!canOmitIterator)
				{
					if (iterator.CollectionEnds(ref source) || !iterator.CollectionEnds(ref source))
					{
						Throw.UnknownTypeEmbedding(initialPosition);
					}
				}
			}
			catch (DecodeException ex)
			{
				Throw.DecodeFailure(ex);
				return default!; // Never reaches
			}

			var type = typeFinder(typeCode, typeCodePosition);
			var result = deserialization(type, ref context, ref source);
			context.DecrementDepth();
			return result;
		}

		public static T Decode<T>(
			ref DeserializationOperationContext context,
			ref SequenceReader<byte> source,
			TypeDecoding typeDecoding,
			Deserialization<T> deserialization
		)
		{
			var decoder = context.Decoder;
			var position = source.Consumed;

			context.IncrementDepth();
			CollectionItemIterator iterator = default;
			var canOmitIterator = decoder.Options.Features.CanCountCollectionItems;
			if (canOmitIterator)
			{
				if (decoder.DecodeArrayOrMapHeader(ref source, out var itemsCount) != CollectionType.Array || itemsCount != 2)
				{
					Throw.UnknownTypeEmbedding(position);
				}
			}
			else
			{
				if (decoder.DecodeArrayOrMap(ref source, out iterator) != CollectionType.Array)
				{
					Throw.UnknownTypeEmbedding(position);
				}
			}

			var type = typeDecoding(ref context, ref source);

			if (!canOmitIterator)
			{
				if (iterator.CollectionEnds(ref source))
				{
					Throw.UnknownTypeEmbedding(position);
				}
			}

			var result = deserialization(type, ref context, ref source);

			if (!canOmitIterator)
			{
				position = source.Consumed;
				if (!iterator.CollectionEnds(ref source))
				{
					Throw.UnknownTypeEmbedding(position);
				}
			}

			context.DecrementDepth();
			return result;
		}

		public static unsafe Type DecodeRuntimeTypeInfo(
			ref DeserializationOperationContext context,
			ref SequenceReader<byte> source,
			Func<PolymorphicTypeVerificationContext, bool> typeVerifier
		)
		{
			var decoder = context.Decoder;
			var canOmitIterator = decoder.Options.Features.CanCountCollectionItems;
			var initialPosition = source.Consumed;
			CollectionItemIterator iterator;

			var itemsCount = 0;

			context.IncrementDepth();
			if (canOmitIterator)
			{
				if (decoder.DecodeArrayOrMapHeader(ref source, out itemsCount) != CollectionType.Array)
				{
					Throw.EncodedTypeMustBeNonNilArray(initialPosition);
				}

				if (itemsCount != 6)
				{
					Throw.EncodedTypeDoesNotHaveValidArrayItems(6, itemsCount, initialPosition);
				}

				iterator = default;
			}
			else
			{
				if (decoder.DecodeArrayOrMap(ref source, out iterator) != CollectionType.Array)
				{
					Throw.EncodedTypeMustBeNonNilArray(initialPosition);
				}
			}

			if (!canOmitIterator)
			{
				if (iterator.CollectionEnds(ref source))
				{
					Throw.EncodedTypeDoesNotHaveValidArrayItems(6, itemsCount, initialPosition);
				}

				itemsCount++;
			}

			var position = source.Consumed;
			byte encodeType;
			try
			{
				encodeType = decoder.DecodeByte(ref source);
			}
			catch (Exception ex)
			{
				Throw.FailedToDecodeEncodingType(ex, position);
				encodeType = default; // Never reaches
			}

			if (encodeType != (byte)TypeInfoEncoding.RawCompressed)
			{
				Throw.UnknownEncodingType(encodeType, position);
			}

			if (!canOmitIterator)
			{
				if (iterator.CollectionEnds(ref source))
				{
					Throw.EncodedTypeDoesNotHaveValidArrayItems(6, itemsCount, initialPosition);
				}

				itemsCount++;
			}

			position = source.Consumed;
			string compressedTypeName;
			try
			{
				compressedTypeName = decoder.DecodeString(ref source, context.Options.DefaultStringEncoding, context.CancellationToken);
			}
			catch (Exception ex)
			{
				Throw.FailedToDecodeCompressedTypeName(ex, position);
				compressedTypeName = default!; // Never reaches
			}

			if (!canOmitIterator)
			{
				if (iterator.CollectionEnds(ref source))
				{
					Throw.EncodedTypeDoesNotHaveValidArrayItems(6, itemsCount, initialPosition);
				}

				itemsCount++;
			}

			position = source.Consumed;
			string assemblySimpleName;
			try
			{
				assemblySimpleName = decoder.DecodeString(ref source, context.Options.DefaultStringEncoding, context.CancellationToken);
			}
			catch (Exception ex)
			{
				Throw.FailedToDecodeAssemblySimpleName(ex, position);
				assemblySimpleName = default!; // Never reaches
			}

			if (!canOmitIterator)
			{
				if (iterator.CollectionEnds(ref source))
				{
					Throw.EncodedTypeDoesNotHaveValidArrayItems(6, itemsCount, initialPosition);
				}

				itemsCount++;
			}

			position = source.Consumed;
			// Workaround for C# escape analysis bug.
			byte* pVersion = stackalloc byte[16];
			var version = new Span<byte>(pVersion, 16);
			try
			{
				var length = decoder.DecodeBinary(ref source, version, context.CancellationToken);
				if (length != version.Length)
				{
					Throw.FailedToDecodeAssemblyVersion(version.Length, length, position);
				}

				version = version.Slice(0, length);
			}
			catch (Exception ex)
			{
				Throw.FailedToDecodeAssemblyVersion(ex, position);
				version = default; // Never reaches
			}

			if (!canOmitIterator)
			{
				if (iterator.CollectionEnds(ref source))
				{
					Throw.EncodedTypeDoesNotHaveValidArrayItems(6, itemsCount, initialPosition);
				}

				itemsCount++;
			}

			position = source.Consumed;
			string? culture;
			try
			{
				culture = decoder.DecodeNullableString(ref source, context.Options.DefaultStringEncoding, context.CancellationToken);
			}
			catch (Exception ex)
			{
				Throw.FailedToDecodeAssemblyCulture(ex, position);
				culture = default; // Never reaches
			}

			if (!canOmitIterator)
			{
				if (iterator.CollectionEnds(ref source))
				{
					Throw.EncodedTypeDoesNotHaveValidArrayItems(6, itemsCount, initialPosition);
				}

				itemsCount++;
			}

			position = source.Consumed;
			// Workaround for C# escape analysis bug.
			byte* pPublicKeyToken = stackalloc byte[8];
			var publicKeyToken = new Span<byte>(pPublicKeyToken, 8);
			try
			{
				var length = decoder.DecodeNullableBinary(ref source, publicKeyToken, context.CancellationToken);
				if (length is null)
				{
					publicKeyToken = default;
				}
				else
				{
					if (length != null && length.GetValueOrDefault() != publicKeyToken.Length)
					{
						Throw.FailedToDecodeAssemblyPublicKeyToken(publicKeyToken.Length, length.GetValueOrDefault(), position);
					}
				}
			}
			catch (Exception ex)
			{
				Throw.FailedToDecodeAssemblyPublicKeyToken(ex, position);
				publicKeyToken = default; // Never reaches
			}

			if (!canOmitIterator)
			{
				while (!iterator.CollectionEnds(ref source))
				{
					itemsCount++;
				}

				if (itemsCount != 6)
				{
					Throw.EncodedTypeDoesNotHaveValidArrayItems(6, itemsCount, initialPosition);
				}
			}

			context.DecrementDepth();

			var assemblyName = BuildAssemblyName(assemblySimpleName, version, culture, publicKeyToken);
			var typeFullName = DecompressTypeName(assemblyName.Name!, compressedTypeName);
			RuntimeTypeVerifier.Verify(assemblyName, typeFullName, typeVerifier);

			return LoadDecodedType(assemblyName, typeFullName);
		}

#if FEATURE_TAP
		public static async ValueTask<T> DecodeAsync<T>(
			AsyncDeserializationOperationContext context,
			ReadOnlyStreamSequence source,
			Func<string, long, Type> typeFinder,
			Func<Type, AsyncDeserializationOperationContext, ReadOnlyStreamSequence, ValueTask<T>> asyncDeserialization
		)
		{
			var decoder = context.Decoder;
			var initialPosition = source.Position;

			var requestHint = 0;
			CollectionType arrayOrMap;
			int itemsCount;
			CollectionItemIterator iterator;
			var canOmitIterator = context.Decoder.Options.Features.CanCountCollectionItems;

			context.IncrementDepth();
			while (!TryDecodeArrayOrMap(context, source, canOmitIterator, out itemsCount, out arrayOrMap, out iterator, out requestHint))
			{
				await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
			}

			if (arrayOrMap != CollectionType.Array)
			{
				Throw.UnknownTypeEmbedding(initialPosition);
			}

			if (canOmitIterator)
			{
				if (itemsCount != 2)
				{
					Throw.UnknownTypeEmbedding(source.Position);
				}
			}
			else
			{
				await EnsureIteratorHasNextAsync(context, source, iterator, initialPosition).ConfigureAwait(false);
			}

			var typeCodePosition = source.Position;
			string typeCode;
			while (!TryDecodeString(context, source, out typeCode, out requestHint))
			{
				await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
			}

			if (!canOmitIterator)
			{
				await EnsureIteratorHasNextAsync(context, source, iterator, initialPosition).ConfigureAwait(false);
			}

			var type = typeFinder(typeCode, typeCodePosition);
			var result = await asyncDeserialization(type, context, source).ConfigureAwait(false);
			if (!canOmitIterator)
			{
				await EnsureIteratorEndsAsync(context, source, iterator, initialPosition).ConfigureAwait(false);
			}
			context.DecrementDepth();
			return result;
		}

		public static async ValueTask<T> DecodeAsync<T>(
			AsyncDeserializationOperationContext context,
			ReadOnlyStreamSequence source,
			Func<AsyncDeserializationOperationContext, ReadOnlyStreamSequence, ValueTask<Type>> asyncTypeDecoding,
			Func<Type, AsyncDeserializationOperationContext, ReadOnlyStreamSequence, ValueTask<T>> asyncDeserialization
		)
		{
			int requestHint;
			CollectionType arrayOrMap;
			int itemsCount;
			CollectionItemIterator iterator;
			var canOmitIterator = context.Decoder.Options.Features.CanCountCollectionItems;

			var initialPotision = source.Position;

			context.IncrementDepth();
			while (!TryDecodeArrayOrMap(context, source, canOmitIterator, out itemsCount, out arrayOrMap, out iterator, out requestHint))
			{
				await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
			}

			if (arrayOrMap != CollectionType.Array)
			{
				Throw.UnknownTypeEmbedding(initialPotision);
			}

			if (canOmitIterator)
			{
				if (itemsCount != 2)
				{
					Throw.UnknownTypeEmbedding(initialPotision);
				}
			}

			var type = await asyncTypeDecoding(context, source).ConfigureAwait(false);

			if (!canOmitIterator)
			{
				await EnsureIteratorHasNextAsync(context, source, iterator, initialPotision).ConfigureAwait(false);
			}

			var result = await asyncDeserialization(type, context, source).ConfigureAwait(false);

			if (!canOmitIterator)
			{
				await EnsureIteratorEndsAsync(context, source, iterator, initialPotision).ConfigureAwait(false);
			}

			context.DecrementDepth();
			return result;
		}

		public static async ValueTask<Type> DecodeRuntimeTypeInfoAsync(
			AsyncDeserializationOperationContext context,
			ReadOnlyStreamSequence source,
			Func<PolymorphicTypeVerificationContext, bool> typeVerifier
		)
		{
			var decoder = context.Decoder;
			var initialPosition = source.Position;
			var canOmitIterator = decoder.Options.Features.CanCountCollectionItems;

			context.IncrementDepth();
			var itemsCount = 0;
			int requestHint;
			CollectionType arrayOrMap;
			CollectionItemIterator iterator;

			while (!TryDecodeArrayOrMap(context, source, canOmitIterator, out itemsCount, out arrayOrMap, out iterator, out requestHint))
			{
				await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
			}

			if (arrayOrMap != CollectionType.Array)
			{
				Throw.EncodedTypeMustBeNonNilArray(initialPosition);
			}

			if (canOmitIterator)
			{
				if (itemsCount != 6)
				{
					Throw.EncodedTypeDoesNotHaveValidArrayItems(6, itemsCount, initialPosition);
				}
			}

			if (!canOmitIterator)
			{
				await EnsureIteratorHasNextAsync(context, source, iterator, initialPosition).ConfigureAwait(false);
				itemsCount++;
			}

			byte encodeType;
			try
			{
				while (!TryDecodeByte(context, source, out encodeType, out requestHint))
				{
					await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
				}
			}
			catch (Exception ex)
			{
				Throw.FailedToDecodeEncodingType(ex, source.Position);
				encodeType = default; // Never reaches
			}

			if (encodeType != (byte)TypeInfoEncoding.RawCompressed)
			{
				Throw.UnknownEncodingType(encodeType, source.Position);
			}

			if (!canOmitIterator)
			{
				await EnsureIteratorHasNextAsync(context, source, iterator, initialPosition).ConfigureAwait(false);
				itemsCount++;
			}

			string compressedTypeName;
			try
			{
				while (!TryDecodeString(context, source, out compressedTypeName, out requestHint))
				{
					await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
				}
			}
			catch (Exception ex)
			{
				Throw.FailedToDecodeCompressedTypeName(ex, source.Position);
				compressedTypeName = default!; // Never reaches
			}

			if (!canOmitIterator)
			{
				await EnsureIteratorHasNextAsync(context, source, iterator, initialPosition).ConfigureAwait(false);
				itemsCount++;
			}

			string assemblySimpleName;
			try
			{
				while (!TryDecodeString(context, source, out assemblySimpleName, out requestHint))
				{
					await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
				}
			}
			catch (Exception ex)
			{
				Throw.FailedToDecodeAssemblySimpleName(ex, source.Position);
				assemblySimpleName = default!; // Never reaches
			}

			if (!canOmitIterator)
			{
				await EnsureIteratorHasNextAsync(context, source, iterator, initialPosition).ConfigureAwait(false);
				itemsCount++;
			}

			// Workaround for C# escape analysis bug.
			var version = context.ByteBufferPool.Rent(16);
			try
			{
				try
				{
					while (!TryDecodeBinaryTo(context, source, version, out requestHint, (expected, actual, position) => Throw.FailedToDecodeAssemblyVersion(expected, actual, position)))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}
				}
				catch (Exception ex)
				{
					Throw.FailedToDecodeAssemblyVersion(ex, source.Position);
				}

				if (!canOmitIterator)
				{
					await EnsureIteratorHasNextAsync(context, source, iterator, initialPosition).ConfigureAwait(false);
					itemsCount++;
				}

				string? culture;
				try
				{
					while (!TryDecodeNullableString(context, source, out culture, out requestHint))
					{
						await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
					}
				}
				catch (Exception ex)
				{
					Throw.FailedToDecodeAssemblyCulture(ex, source.Position);
					culture = default; // Never reaches
				}

				if (!canOmitIterator)
				{
					await EnsureIteratorHasNextAsync(context, source, iterator, initialPosition).ConfigureAwait(false);
					itemsCount++;
				}

				// Workaround for C# escape analysis bug.
				var publicKeyToken = context.ByteBufferPool.Rent(8);
				try
				{
					try
					{
						while (true)
						{
							var nullOrDecoded = TryDecodeNullableBinaryTo(context, source, publicKeyToken, out requestHint, (expected, actual, position) => Throw.FailedToDecodeAssemblyPublicKeyToken(expected, actual, position));
							if (nullOrDecoded.GetValueOrDefault(true))
							{
								break;
							}

							await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
						}
					}
					catch (Exception ex)
					{
						Throw.FailedToDecodeAssemblyPublicKeyToken(ex, source.Position);
					}

					if (!canOmitIterator)
					{
						await EnsureIteratorEndsAsync(context, source, iterator, initialPosition).ConfigureAwait(false);

						if (itemsCount != 6)
						{
							Throw.EncodedTypeDoesNotHaveValidArrayItems(6, itemsCount, initialPosition);
						}
					}

					context.DecrementDepth();

					var assemblyName = BuildAssemblyName(assemblySimpleName, version, culture, publicKeyToken);
					var typeFullName = DecompressTypeName(assemblyName.Name!, compressedTypeName);
					RuntimeTypeVerifier.Verify(assemblyName, typeFullName, typeVerifier);

					return LoadDecodedType(assemblyName, typeFullName);
				}
				finally
				{
					context.ByteBufferPool.Return(publicKeyToken, clearArray: context.Options.ClearsByteBufferOnReturn);
				}
			}
			finally
			{
				context.ByteBufferPool.Return(version, clearArray: context.Options.ClearsByteBufferOnReturn);
			}
		}

		private static bool TryDecodeArrayOrMap(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, bool canOmitIterator, out int itemsCount, out CollectionType arrayOrMap, out CollectionItemIterator propertyIterator, out int requestHint)
		{
			var reader = new SequenceReader<byte>(source.Sequence);

			if (canOmitIterator)
			{
				arrayOrMap = context.Decoder.DecodeArrayOrMapHeader(ref reader, out itemsCount, out requestHint);
				propertyIterator = default;
			}
			else
			{
				arrayOrMap = context.Decoder.DecodeArrayOrMap(ref reader, out propertyIterator, out requestHint);
				itemsCount = -1;
			}

			if (requestHint != 0)
			{
				return false;
			}

			source.Advance(reader.Consumed);

			return true;
		}

		private static bool TryDecodeByte(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, out byte value, out int requestHint)
		{
			var reader = new SequenceReader<byte>(source.Sequence);

			value = context.Decoder.DecodeByte(ref reader, out requestHint)!;
			if (requestHint != 0)
			{
				return false;
			}

			source.Advance(reader.Consumed);

			return true;
		}

		private static bool TryDecodeString(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, out string value, out int requestHint)
		{
			var reader = new SequenceReader<byte>(source.Sequence);

			value = context.Decoder.DecodeString(ref reader, out requestHint, context.Options.DefaultStringEncoding, context.CancellationToken)!;
			if (requestHint != 0)
			{
				return false;
			}

			source.Advance(reader.Consumed);

			return true;
		}

		private static bool TryDecodeNullableString(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, out string? value, out int requestHint)
		{
			var reader = new SequenceReader<byte>(source.Sequence);

			value = context.Decoder.DecodeNullableString(ref reader, out requestHint, context.Options.DefaultStringEncoding, context.CancellationToken)!;
			if (requestHint != 0)
			{
				return false;
			}

			source.Advance(reader.Consumed);

			return true;
		}

		private static bool TryDecodeBinaryTo(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, Span<byte> buffer, out int requestHint, Action<int, int, long> onLengthMismatch)
		{
			var reader = new SequenceReader<byte>(source.Sequence);

			var consumed = context.Decoder.DecodeBinary(ref reader, buffer, out requestHint, context.CancellationToken)!;
			if (requestHint != 0)
			{
				return false;
			}

			if (consumed != buffer.Length)
			{
				onLengthMismatch(buffer.Length, consumed, source.Position);
			}

			source.Advance(reader.Consumed);

			return true;
		}

		private static bool? TryDecodeNullableBinaryTo(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, Span<byte> buffer, out int requestHint, Action<int, int, long> onLengthMismatch)
		{
			var reader = new SequenceReader<byte>(source.Sequence);

			var consumed = context.Decoder.DecodeNullableBinary(ref reader, buffer, out requestHint, context.CancellationToken)!;
			if (requestHint != 0)
			{
				return false;
			}

			if (consumed != null && consumed.GetValueOrDefault() != buffer.Length)
			{
				onLengthMismatch(buffer.Length, consumed.GetValueOrDefault(), source.Position);
			}

			source.Advance(reader.Consumed);

			return consumed == null ? default(bool?) : true;
		}

		private static async ValueTask EnsureIteratorHasNextAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, CollectionItemIterator iterator, long initialPosition)
		{
			while (true)
			{
				var ends = iterator.CollectionEnds(source.Sequence, out var requestHint);
				if (requestHint == 0)
				{
					if (!ends)
					{
						return;
					}

					Throw.UnknownTypeEmbedding(initialPosition);
				}

				await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
			}
		}

		private static async ValueTask EnsureIteratorEndsAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence source, CollectionItemIterator iterator, long initialPosition)
		{
			while (true)
			{
				var ends = iterator.CollectionEnds(source.Sequence, out var requestHint);
				if (requestHint == 0)
				{
					if (ends)
					{
						// OK.
						return;
					}

					Throw.UnknownTypeEmbedding(initialPosition);
				}
				else
				{
					await source.FetchAsync(requestHint, context.CancellationToken).ConfigureAwait(false);
				}
			}
		}

#endif // FEATURE_TAP

		private static AssemblyName BuildAssemblyName(string assemblySimpleName, ReadOnlySpan<byte> version, string? culture, ReadOnlySpan<byte> publicKeyToken)
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			var assemblyName =
				new AssemblyName
				{
					Name = assemblySimpleName,
					Version =
						new Version(
							BinaryPrimitives.ReadInt32LittleEndian(version),
							BinaryPrimitives.ReadInt32LittleEndian(version.Slice(4)),
							BinaryPrimitives.ReadInt32LittleEndian(version.Slice(8)),
							BinaryPrimitives.ReadInt32LittleEndian(version.Slice(12))
						),
					CultureInfo =
						String.IsNullOrEmpty(culture) ?
							null :
							CultureInfo.GetCultureInfo(culture),
				};
			if (!publicKeyToken.IsEmpty)
			{
				assemblyName.SetPublicKeyToken(publicKeyToken.ToArray());
			}

			return assemblyName;
#else
			return
				new AssemblyName(
					String.Format( 
						CultureInfo.InvariantCulture, 
						"{0},Version={1},Culture={2},PublicKeyToken={3}",
						assemblySimpleName,
						new Version(
							BinaryPrimitives.ReadInt32LittleEndian(version),
							BinaryPrimitives.ReadInt32LittleEndian(version.Slice(4)),
							BinaryPrimitives.ReadInt32LittleEndian(version.Slice(8)),
							BinaryPrimitives.ReadInt32LittleEndian(version.Slice(12))
						),
						String.IsNullOrEmpty(culture) ? "neutral" : culture,
						publicKeyToken.IsEmpty ? "null" : Binary.ToHexString(publicKeyToken, false)
					)
				);
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
		}

		private static string DecompressTypeName(string assemblySimpleName, string compressedTypeName)
			=> compressedTypeName.StartsWith(Elipsis, StringComparison.Ordinal) ?
				assemblySimpleName + compressedTypeName :
				compressedTypeName;

		private static Type LoadDecodedType(AssemblyName assemblyName, string typeFullName)
			=> Assembly.Load(
					assemblyName
				).GetType(
					typeFullName,
					throwOnError: true
				)!;
	}
}
