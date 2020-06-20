// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Runtime.CompilerServices;

namespace MsgPack.Internal
{
	/// <summary>
	///		Represents collection type.
	/// </summary>
	public readonly struct CollectionType : IEquatable<CollectionType>
	{
		public static readonly CollectionType None = default;
		public static readonly CollectionType Null = new CollectionType(1);
		public static readonly CollectionType Array = new CollectionType(2);
		public static readonly CollectionType Map = new CollectionType(3);

		private readonly int _type;

		public bool IsNull
		{
			[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
			get => this._type == 1;
		}

		public bool IsArray
		{
			[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
			get => this._type == 2;
		}

		public bool IsMap
		{
			[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
			get => this._type == 3;
		}

		public bool IsNone
		{
			[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
			get => this._type == 0;
		}

		private CollectionType(int type)
		{
			this._type = type;
		}

		public override bool Equals(object? obj)
			=> obj is CollectionType other ? this.Equals(other) : false;

		public bool Equals(CollectionType other)
			=> this._type == other._type;

		public override int GetHashCode()
			=> this._type;

		public override string ToString()
			=> this._type switch
			{
				1 => "Array",
				2 => "Map",
				_ => String.Empty
			};

		public static bool operator ==(CollectionType left, CollectionType right)
			=> left.Equals(right);

		public static bool operator !=(CollectionType left, CollectionType right)
			=> !left.Equals(right);
	}
}
