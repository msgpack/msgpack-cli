// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace MsgPack.Internal
{
	/// <summary>
	///		Represents a result of <see cref="FormatDecoder.DecodeItem"/>.
	/// </summary>
	public readonly struct DecodeItemResult
	{
		public bool HasValue => this.ElementType != ElementType.None;
		public ElementType ElementType { get; }
		public ReadOnlySequence<byte> Value { get; }
		public CollectionItemIterator CollectionIterator { get; }
		public long CollectionLength { get; }
		public ExtensionTypeObject ExtensionTypeObject { get; }
		public ReadOnlySequence<byte> ExtensionBody => this.Value;
		public long RequestHint { get; }

		private DecodeItemResult(
			ElementType elementType,
			in ReadOnlySequence<byte> value = default,
			in CollectionItemIterator collectionIterator = default,
			long collectionLength = default,
			ExtensionTypeObject extensionTypeObject = default,
			long requestHint = default
		)
		{
			this.ElementType = elementType;
			this.Value = value;
			this.CollectionIterator = collectionIterator;
			this.CollectionLength = collectionLength;
			this.ExtensionTypeObject = extensionTypeObject;
			this.RequestHint = requestHint;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult CollectionHeader(ElementType elementType, in CollectionItemIterator iterator, long length = -1)
			=> new DecodeItemResult(elementType, collectionIterator: iterator, collectionLength: length);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult ScalarOrSequence(ElementType elementType, ReadOnlyMemory<byte> value)
			=> ScalarOrSequence(elementType, new ReadOnlySequence<byte>(value));

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult ScalarOrSequence(ElementType elementType, in ReadOnlySequence<byte> value)
			=> new DecodeItemResult(elementType, value: value);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult Null()
			=> new DecodeItemResult(ElementType.Null);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult True()
			=> new DecodeItemResult(ElementType.True);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult False()
			=> new DecodeItemResult(ElementType.False);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult ExtensionType(ExtensionTypeObject extensionTypeObject)
			=> new DecodeItemResult(ElementType.Extension, extensionTypeObject : extensionTypeObject);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult InsufficientInput(long requestHint)
			=> new DecodeItemResult(ElementType.None, requestHint: requestHint);
	}
}
