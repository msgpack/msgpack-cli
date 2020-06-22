// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;

namespace MsgPack
{
	/// <summary>
	///		Represents extension type object data.
	/// </summary>
	/// <remarks>
	///		Valid range and sementics of <see cref="Type"/> depend on serialization codec.
	///		In addition, some codec does not support extension type at all.
	/// </remarks>
	public readonly struct ExtensionTypeObject
	{
		/// <summary>
		///		Gets a type of this extension type data.
		/// </summary>
		/// <value>Codec specific type of this extension type data.</value>
		public ExtensionType Type { get; }

		/// <summary>
		///		Gets byte sequence which is body of this extension type data.
		/// </summary>
		/// <value>Byte sequence which is body of this extension type data.</value>
		public ReadOnlySequence<byte> Body { get; }

		/// <summary>
		///		Initializes a new instance.
		/// </summary>
		/// <param name="type">Codec specific type of this extension type data.</param>
		/// <param name="body">Byte sequence which is body of this extension type data.</param>
		public ExtensionTypeObject(ExtensionType type, ReadOnlySequence<byte> body)
		{
			this.Type = type;
			this.Body = body;
		}
	}
}
