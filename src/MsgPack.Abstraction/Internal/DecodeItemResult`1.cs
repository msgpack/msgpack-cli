// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace MsgPack.Internal
{
	/// <summary>
	///		Represetns a result of <see cref="Decoder.DecodeItem"/>.
	/// </summary>
	public readonly struct DecodeItemResult<TExtensionType>
	{
		public bool HasValue => this.ElementType != ElementType.None;
		public ElementType ElementType { get; }
		public ReadOnlySequence<byte> Value { get; }
		public CollectionItemIterator CollectionIterator { get; }
		public long CollectionLength { get; }
		public TExtensionType ExtensionType { get; }
		public ReadOnlySequence<byte> ExtensionBody => this.Value;
		public long RequestHint { get; }

		private DecodeItemResult(
			ElementType elementType,
			in ReadOnlySequence<byte> value = default,
			in CollectionItemIterator collectionIterator = default,
			long collectionLength = default,
			TExtensionType extensionType = default,
			long requestHint = default
		)
		{
			this.ElementType = elementType;
			this.Value = value;
			this.CollectionIterator = collectionIterator;
			this.CollectionLength = collectionLength;
			this.ExtensionType = extensionType;
			this.RequestHint = requestHint;
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult<TExtensionType> CollectionHeader(ElementType elementType, in CollectionItemIterator iterator, long length = -1)
			=> new DecodeItemResult<TExtensionType>(elementType, collectionIterator: iterator, collectionLength: length);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult<TExtensionType> ScalarOrSequence(ElementType elementType, ReadOnlyMemory<byte> value)
			=> ScalarOrSequence(elementType, new ReadOnlySequence<byte>(value));

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult<TExtensionType> ScalarOrSequence(ElementType elementType, in ReadOnlySequence<byte> value)
			=> new DecodeItemResult<TExtensionType>(elementType, value: value);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult<TExtensionType> Null()
			=> new DecodeItemResult<TExtensionType>(ElementType.Null);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult<TExtensionType> True()
			=> new DecodeItemResult<TExtensionType>(ElementType.True);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult<TExtensionType> False()
			=> new DecodeItemResult<TExtensionType>(ElementType.False);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult<TExtensionType> ExtensionTypeObject(TExtensionType extensionType, in ReadOnlySequence<byte> body)
			=> new DecodeItemResult<TExtensionType>(ElementType.Extension, extensionType : extensionType, value : body);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static DecodeItemResult<TExtensionType> InsufficientInput(long requestHint)
			=> new DecodeItemResult<TExtensionType>(ElementType.None, requestHint: requestHint);
	}
}
