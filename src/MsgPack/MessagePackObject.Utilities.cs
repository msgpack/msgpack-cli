// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

using MsgPack.Internal;

namespace MsgPack
{
#if FEATURE_BINARY_SERIALIZATION
#warning TODO: Enable in .NET Standard 2.0 in Changes.txt
	[Serializable]
#endif // FEATURE_BINARY_SERIALIZATION
	public partial struct MessagePackObject
	{
		#region -- Type Code Constants --

		private static readonly ValueTypeCode SByteTypeCode = new ValueTypeCode(typeof(sbyte), MessagePackValueTypeCode.Int8);
		private static readonly ValueTypeCode ByteTypeCode = new ValueTypeCode(typeof(byte), MessagePackValueTypeCode.UInt8);
		private static readonly ValueTypeCode Int16TypeCode = new ValueTypeCode(typeof(short), MessagePackValueTypeCode.Int16);
		private static readonly ValueTypeCode UInt16TypeCode = new ValueTypeCode(typeof(ushort), MessagePackValueTypeCode.UInt16);
		private static readonly ValueTypeCode Int32TypeCode = new ValueTypeCode(typeof(int), MessagePackValueTypeCode.Int32);
		private static readonly ValueTypeCode UInt32TypeCode = new ValueTypeCode(typeof(uint), MessagePackValueTypeCode.UInt32);
		private static readonly ValueTypeCode Int64TypeCode = new ValueTypeCode(typeof(long), MessagePackValueTypeCode.Int64);
		private static readonly ValueTypeCode UInt64TypeCode = new ValueTypeCode(typeof(ulong), MessagePackValueTypeCode.UInt64);
		private static readonly ValueTypeCode SingleTypeCode = new ValueTypeCode(typeof(float), MessagePackValueTypeCode.Single);
		private static readonly ValueTypeCode DoubleTypeCode = new ValueTypeCode(typeof(double), MessagePackValueTypeCode.Double);
		private static readonly ValueTypeCode BooleanTypeCode = new ValueTypeCode(typeof(bool), MessagePackValueTypeCode.Boolean);

		#endregion -- Type Code Constants --

		/// <summary>
		///		Instance represents nil. This is equal to default value.
		/// </summary>
		public static readonly MessagePackObject Nil = default(MessagePackObject);

		#region -- Type Code Fields & Properties --

		private readonly object? _handleOrTypeCode;

		internal object? HandleOrTypeCode => this._handleOrTypeCode;

		/// <summary>
		///		Get whether this instance represents nil.
		/// </summary>
		/// <value>If this instance represents nil object, then true.</value>
		public bool IsNil
		{
			get { return this._handleOrTypeCode == null; }
		}

		private readonly ulong _value;

		#endregion -- Type Code Fields & Properties --

		#region -- Constructors --

		/// <summary>
		///		Initializes a new instance wraps <see cref="IList&lt;MessagePackObject&gt;"/>.
		/// </summary>
		/// <param name="value">
		///		The collection to be copied.
		/// </param>
		public MessagePackObject(IList<MessagePackObject> value)
			: this(value, false) { }

		/// <summary>
		///		Initializes a new instance wraps <see cref="IList&lt;MessagePackObject&gt;"/>.
		/// </summary>
		/// <param name="value">
		///		The collection to be copied or used.
		/// </param>
		/// <param name="isImmutable">
		///		<c>true</c> if the <paramref name="value"/> is immutable collection;
		///		othereise, <c>false</c>.
		/// </param>
		/// <remarks>
		///		When the collection is truely immutable or dedicated, you can specify <c>true</c> to the <paramref name="isImmutable"/>.
		///		When <paramref name="isImmutable"/> is <c>true</c>, this constructor does not copy its contents,
		///		or copies its contents otherwise.
		///		<note>
		///			Note that both of IReadOnlyList and <see cref="System.Collections.ObjectModel.ReadOnlyCollection{T}"/> is NOT immutable
		///			because the modification to the underlying collection will be reflected to the read-only collection.
		///		</note>
		/// </remarks>
		public MessagePackObject(IList<MessagePackObject> value, bool isImmutable)
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			if (isImmutable)
			{
				this._handleOrTypeCode = value;
			}
			else
			{
				if (value == null)
				{
					this._handleOrTypeCode = null;
				}
				else
				{
					var copy = new MessagePackObject[value.Count];
					value.CopyTo(copy, 0);
					this._handleOrTypeCode = copy;
				}
			}
		}

		/// <summary>
		///		Initializes a new instance wraps <see cref="IReadOnlyList&lt;MessagePackObject&gt;"/>.
		/// </summary>
		/// <param name="value">
		///		The collection to be copied.
		/// </param>
		public MessagePackObject(IReadOnlyList<MessagePackObject> value)
			: this(value, false) { }

		/// <summary>
		///		Initializes a new instance wraps <see cref="IReadOnlyList&lt;MessagePackObject&gt;"/>.
		/// </summary>
		/// <param name="value">
		///		The collection to be copied or used.
		/// </param>
		/// <param name="isImmutable">
		///		<c>true</c> if the <paramref name="value"/> is immutable collection;
		///		othereise, <c>false</c>.
		/// </param>
		/// <remarks>
		///		When the collection is truely immutable or dedicated, you can specify <c>true</c> to the <paramref name="isImmutable"/>.
		///		When <paramref name="isImmutable"/> is <c>true</c>, this constructor does not copy its contents,
		///		or copies its contents otherwise.
		///		<note>
		///			Note that both of IReadOnlyList and <see cref="System.Collections.ObjectModel.ReadOnlyCollection{T}"/> is NOT immutable
		///			because the modification to the underlying collection will be reflected to the read-only collection.
		///		</note>
		/// </remarks>
		public MessagePackObject(IReadOnlyList<MessagePackObject> value, bool isImmutable)
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			if (isImmutable)
			{
				this._handleOrTypeCode = value;
			}
			else
			{
				if (value == null)
				{
					this._handleOrTypeCode = null;
				}
				else
				{
					var copy = new MessagePackObject[value.Count];
					for (var i = 0; i < copy.Length; i++)
					{
						copy[i] = value[i];
					}

					this._handleOrTypeCode = copy;
				}
			}
		}

		/// <summary>
		///		Initializes a new instance wraps <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <param name="value">
		///		The dictitonary to be copied.
		/// </param>
		public MessagePackObject(MessagePackObjectDictionary value)
			: this(value, false) { }

		/// <summary>
		///		Initializes a new instance wraps <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <param name="value">
		///		The dictitonary to be copied or used.
		/// </param>
		/// <param name="isImmutable">
		///		<c>true</c> if the <paramref name="value"/> is immutable collection;
		///		othereise, <c>false</c>.
		/// </param>
		/// <remarks>
		///		When the collection is truely immutable or dedicated, you can specify <c>true</c> to the <paramref name="isImmutable"/>.
		///		When <paramref name="isImmutable"/> is <c>true</c>, this constructor does not copy its contents,
		///		or copies its contents otherwise.
		///		<note>
		///			Note that both of IReadOnlyDictionary and ReadOnlyDictionary is NOT immutable
		///			because the modification to the underlying collection will be reflected to the read-only collection.
		///		</note>
		/// </remarks>
		public MessagePackObject(MessagePackObjectDictionary value, bool isImmutable)
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			if (isImmutable)
			{
				this._handleOrTypeCode = value;
			}
			else
			{
				if (value == null)
				{
					this._handleOrTypeCode = null;
				}
				else
				{
					this._handleOrTypeCode = new MessagePackObjectDictionary(value);
				}
			}
		}

		/// <summary>
		///		Initializes a new instance wraps <see cref="MessagePackString"/>.
		/// </summary>
		/// <param name="messagePackString"><see cref="MessagePackString"/> which represents byte array or UTF-8 encoded string.</param>
#if UNITY && DEBUG
		public
#else
		internal
#endif
		MessagePackObject(MessagePackString messagePackString)
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			this._handleOrTypeCode = messagePackString;
		}

		#endregion -- Constructors --

		#region -- Structure Methods --

		/// <summary>
		///		Compare two instances are equal.
		/// </summary>
		/// <param name="obj"><see cref="MessagePackObject"/> instance.</param>
		/// <returns>
		///		If <paramref name="obj"/> is <see cref="MessagePackObject"/> and its value is equal to this instance, then true.
		///		Otherwise false.
		/// </returns>
		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(obj, null))
			{
				return this.IsNil;
			}
			else if (obj is MessagePackObjectDictionary asDictionary)
			{
				return this.Equals(new MessagePackObject(asDictionary));
			}
			else if (obj is MessagePackObject other)
			{
				return this.Equals(other);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		///		Compare two instances are equal.
		/// </summary>
		/// <param name="other"><see cref="MessagePackObject"/> instance.</param>
		/// <returns>
		///		Whether value of <paramref name="other"/> is equal to this instance or not.
		/// </returns>
		public bool Equals(MessagePackObject other)
		{
			if (this._handleOrTypeCode == null)
			{
				return other._handleOrTypeCode == null;
			}
			else if (other._handleOrTypeCode == null)
			{
				return false;
			}

			if (this._handleOrTypeCode is ValueTypeCode valueTypeCode)
			{
				if (!(this._handleOrTypeCode is ValueTypeCode otherValuetypeCode))
				{
					return false;
				}

				return this.EqualsWhenValueType(other, valueTypeCode, otherValuetypeCode);
			}

			if (this._handleOrTypeCode is MessagePackString asMps)
			{
				return asMps.Equals(other._handleOrTypeCode as MessagePackString);
			}

			if (this._handleOrTypeCode is IList<MessagePackObject> asList)
			{
				if (!(other._handleOrTypeCode is IList<MessagePackObject> otherAsList))
				{
					return false;
				}

				return asList.SequenceEqual(otherAsList, MessagePackObjectEqualityComparer.Instance);
			}

			if (this._handleOrTypeCode is IReadOnlyList<MessagePackObject> asReadOnlyList)
			{
				if (!(other._handleOrTypeCode is IReadOnlyList<MessagePackObject> otherAsReadOnlyList))
				{
					return false;
				}

				return asReadOnlyList.SequenceEqual(otherAsReadOnlyList, MessagePackObjectEqualityComparer.Instance);
			}

			if (this._handleOrTypeCode is MessagePackObjectDictionary asMap)
			{
				if (!(this._handleOrTypeCode is MessagePackObjectDictionary otherAsMap))
				{
					return false;
				}

				if (asMap.Count != otherAsMap.Count)
				{
					return false;
				}

				foreach (var kv in asMap)
				{
					if (!otherAsMap.TryGetValue(kv.Key, out var value))
					{
						return false;
					}

					if (kv.Value != value)
					{
						return false;
					}
				}

				return true;
			}

			if (this._handleOrTypeCode is ReadOnlySequence<byte> asExtensionTypeObjectBody)
			{
				if (!(other._handleOrTypeCode is ReadOnlySequence<byte> otherAsExtensionTypeObjectBody))
				{
					return false;
				}

				return
					new ExtensionTypeObject(new ExtensionType(unchecked((long)this._value)), asExtensionTypeObjectBody) ==
						new ExtensionTypeObject(new ExtensionType(unchecked((long)this._value)), otherAsExtensionTypeObjectBody);
			}

			Debug.Fail($"Unknown handle type this:'{this._handleOrTypeCode.GetType()}'(value: '{this._value}'), other:'{other._handleOrTypeCode.GetType()}'(value: '{other._value}')");
			return this._handleOrTypeCode.Equals(other._handleOrTypeCode);
		}

		private bool EqualsWhenValueType(
			MessagePackObject other,
			ValueTypeCode valueTypeCode,
			ValueTypeCode otherValuetypeCode)
		{
			if (valueTypeCode.TypeCode == MessagePackValueTypeCode.Boolean)
			{
				if (otherValuetypeCode.TypeCode != MessagePackValueTypeCode.Boolean)
				{
					return false;
				}

				return (bool)this == (bool)other;
			}
			else if (otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Boolean)
			{
				return false;
			}

			if (valueTypeCode.IsInteger)
			{
				if (otherValuetypeCode.IsInteger)
				{
					return IntegerIntegerEquals(this._value, valueTypeCode, other._value, otherValuetypeCode);
				}
				else if (otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Single)
				{
					return IntegerSingleEquals(this, other);
				}
				else if (otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Double)
				{
					return IntegerDoubleEquals(this, other);
				}
			}
			else if (valueTypeCode.TypeCode == MessagePackValueTypeCode.Double)
			{
				if (otherValuetypeCode.IsInteger)
				{
					return IntegerDoubleEquals(other, this);
				}
				else if (otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Single)
				{
					// ReSharper disable once CompareOfFloatsByEqualityOperator
					return (double)this == (float)other;
				}
				else if (otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Double)
				{
					// ReSharper disable once CompareOfFloatsByEqualityOperator
					// Cannot compare _value because there might be not normalized.
					return (double)this == (double)other;
				}
			}
			else if (valueTypeCode.TypeCode == MessagePackValueTypeCode.Single)
			{
				if (otherValuetypeCode.IsInteger)
				{
					return IntegerSingleEquals(other, this);
				}
				else if (otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Single)
				{
					// ReSharper disable once CompareOfFloatsByEqualityOperator
					// Cannot compare _value because there might be not normalized.
					return (float)this == (float)other;
				}
				else if (otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Double)
				{
					// ReSharper disable once CompareOfFloatsByEqualityOperator
					return (float)this == (double)other;
				}
			}

			return false;
		}

		private static bool IntegerIntegerEquals(ulong left, ValueTypeCode leftTypeCode, ulong right, ValueTypeCode rightTypeCode)
		{
			if (leftTypeCode.IsSigned)
			{
				if (rightTypeCode.IsSigned)
				{
					return left == right;
				}
				else
				{
					var leftAsInt64 = unchecked((long)left);
					if (leftAsInt64 < 0L)
					{
						return false;
					}

					return left == right;
				}
			}
			else
			{
				if (rightTypeCode.IsSigned)
				{
					var rightAsInt64 = unchecked((long)right);
					if (rightAsInt64 < 0L)
					{
						return false;
					}

					return left == right;
				}
				else
				{
					return left == right;
				}
			}
		}

		private static bool IntegerSingleEquals(MessagePackObject integer, MessagePackObject real)
		{
			Debug.Assert(integer._handleOrTypeCode is ValueTypeCode, "integer._handleOrTypeCode is ValueTypeCode");

			if (((integer._handleOrTypeCode as ValueTypeCode)?.IsSigned).GetValueOrDefault())
			{
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				return unchecked((long)integer._value) == (float)real;
			}
			else
			{
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				return integer._value == (float)real;
			}
		}

		private static bool IntegerDoubleEquals(MessagePackObject integer, MessagePackObject real)
		{
			Debug.Assert(integer._handleOrTypeCode is ValueTypeCode, "integer._handleOrTypeCode is ValueTypeCode");

			if (((integer._handleOrTypeCode as ValueTypeCode)?.IsSigned).GetValueOrDefault())
			{
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				return unchecked((long)integer._value) == (double)real;
			}
			else
			{
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				return integer._value == (double)real;
			}
		}

		/// <summary>
		///		Get hash code of this instance.
		/// </summary>
		/// <returns>Hash code of this instance.</returns>
		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyFieldInGetHashCode
			if (this._handleOrTypeCode == null)
			{
				return 0;
			}

			if (this._handleOrTypeCode is ValueTypeCode)
			{
				return this._value.GetHashCode();
			}

			if (this._handleOrTypeCode is MessagePackString asMps)
			{
				return asMps.GetHashCode();
			}

			if (this._handleOrTypeCode is IEnumerable<MessagePackObject> asList)
			{
				return asList.Aggregate(0, (hash, item) => hash ^ item.GetHashCode());
			}

			if (this._handleOrTypeCode is MessagePackObjectDictionary asMap)
			{
#warning TODO: item.GetHashCode() -> item.Key.GetHashCode() ^ item.Value.GetHashCode()
				return asMap.Aggregate(0, (hash, item) => hash ^ item.GetHashCode());
			}

			if (this._handleOrTypeCode is ReadOnlySequence<byte> asExtensionTypeObjectBody)
			{
				return new ExtensionTypeObject(new ExtensionType(unchecked((long)this._value)), asExtensionTypeObjectBody).GetHashCode();
			}

			Debug.Fail($"Unexpected handle type: {this._handleOrTypeCode.GetType()}");
			return 0;
		}

		/// <summary>
		/// 	Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// 	A string that represents the current object.
		/// </returns>
		/// <remarks>
		///		<note>
		///			DO NOT use this value programmically. 
		///			The purpose of this method is informational, so format of this value subject to change.
		///		</note>
		/// </remarks>
		public override string ToString()
		{
			var buffer = StringBuilderCache.Acquire();
			try
			{
				this.ToString(buffer, false);
				return buffer.ToString();
			}
			finally
			{
				StringBuilderCache.Release(buffer);
			}
		}

		private void ToString(StringBuilder buffer, bool isJson)
		{
			if (this._handleOrTypeCode == null)
			{
				if (isJson)
				{
					buffer.Append("null");
				}

				return;
			}

			if (this._handleOrTypeCode is ValueTypeCode valueTypeCode)
			{
				switch (valueTypeCode.TypeCode)
				{
					case MessagePackValueTypeCode.Boolean:
					{
						if (isJson)
						{
							buffer.Append(this.AsBoolean() ? "true" : "false");
						}
						else
						{
							buffer.Append(this.AsBoolean());
						}

						break;
					}
					case MessagePackValueTypeCode.Double:
					{
						buffer.Append(this.AsDouble().ToString(CultureInfo.InvariantCulture));
						break;
					}
					case MessagePackValueTypeCode.Single:
					{
						buffer.Append(this.AsSingle().ToString(CultureInfo.InvariantCulture));
						break;
					}
					default:
					{
						if (valueTypeCode.IsSigned)
						{
							buffer.Append(unchecked((long)(this._value)).ToString(CultureInfo.InvariantCulture));
						}
						else
						{
							buffer.Append(this._value.ToString(CultureInfo.InvariantCulture));
						}

						break;
					}
				}

				return;
			}

			if (this._handleOrTypeCode is IEnumerable<MessagePackObject> asList)
			{
				// TODO: big array support...
				buffer.Append('[');
				var isFirst = true;
				foreach (var item in asList)
				{
					if (isFirst)
					{
						isFirst = false;
					}
					else
					{
						buffer.Append(',');
					}

					buffer.Append(' ');
					item.ToString(buffer, isJson);
				}

				if (!isFirst)
				{
					buffer.Append(' ');
				}

				buffer.Append(']');
				return;
			}

			if (this._handleOrTypeCode is MessagePackObjectDictionary asMap)
			{
				buffer.Append('{');
				if (asMap.Count > 0)
				{
					bool isFirst = true;
					foreach (var entry in asMap)
					{
						if (isFirst)
						{
							isFirst = false;
						}
						else
						{
							buffer.Append(',');
						}

						buffer.Append(' ');
						entry.Key.ToString(buffer, isJson);
						buffer.Append(" : ");
						entry.Value.ToString(buffer, isJson);
					}

					buffer.Append(' ');
				}

				buffer.Append('}');
				return;
			}

			if (this._handleOrTypeCode is MessagePackString asBinary)
			{
				ToStringBinary(buffer, isJson, asBinary);
				return;
			}

			if (this._handleOrTypeCode is ReadOnlySequence<byte> asExtensionTypeObjectBody)
			{
				new ExtensionTypeObject(new ExtensionType(unchecked((long)this._value)), asExtensionTypeObjectBody).ToString(buffer, isJson);
				return;
			}

			// may be string
			Debug.Fail($"Unexpected handle type: {this._handleOrTypeCode.GetType()}");
			// ReSharper disable HeuristicUnreachableCode
			if (isJson)
			{
				buffer.Append('"').Append(this._handleOrTypeCode).Append('"');
			}
			else
			{
				buffer.Append(this._handleOrTypeCode);
			}
			// ReSharper restore HeuristicUnreachableCode
		}

		private static void ToStringBinary(StringBuilder buffer, bool isJson, MessagePackString asBinary)
		{
			var asString = asBinary.TryGetString();
			if (asString != null)
			{
				if (isJson)
				{
					buffer.Append('"');
					foreach (var c in asString)
					{
						switch (c)
						{
							case '"':
							{
								buffer.Append('\\').Append('"');
								break;
							}
							case '\\':
							{
								buffer.Append('\\').Append('\\');
								break;
							}
							case '/':
							{
								buffer.Append('\\').Append('/');
								break;
							}
							case '\b':
							{
								buffer.Append('\\').Append('b');
								break;
							}
							case '\f':
							{
								buffer.Append('\\').Append('f');
								break;
							}
							case '\n':
							{
								buffer.Append('\\').Append('n');
								break;
							}
							case '\r':
							{
								buffer.Append('\\').Append('r');
								break;
							}
							case '\t':
							{
								buffer.Append('\\').Append('t');
								break;
							}
							case ' ':
							{
								buffer.Append(' ');
								break;
							}
							default:
							{
								switch (CharUnicodeInfo.GetUnicodeCategory(c))
								{
									case UnicodeCategory.Control:
									case UnicodeCategory.OtherNotAssigned:
									case UnicodeCategory.Format:
									case UnicodeCategory.LineSeparator:
									case UnicodeCategory.ParagraphSeparator:
									case UnicodeCategory.SpaceSeparator:
									case UnicodeCategory.PrivateUse:
									case UnicodeCategory.Surrogate:
									{
										buffer.Append('\\').Append('u').Append(((ushort)c).ToString("X", CultureInfo.InvariantCulture));
										break;
									}
									default:
									{
										buffer.Append(c);
										break;
									}
								}

								break;
							}
						}
					}

					buffer.Append('"');
				}
				else
				{
					buffer.Append(asString);
				}

				return;
			}

			var asBlob = asBinary.UnsafeGetMemory();
			if (isJson)
			{
				buffer.Append('"');

				const int byteSpanLength = 96;
				const int charSpanLength = byteSpanLength / 3 * 4;
				var byteSpan = asBlob.Span;
				Span<char> charSpan = stackalloc char[charSpanLength];
				while (!byteSpan.IsEmpty)
				{
					var byteLength = (int)Math.Min(byteSpan.Length, byteSpanLength);
					Convert.TryToBase64Chars(byteSpan.Slice(0, byteLength), charSpan, out var charsWritten, Base64FormattingOptions.None);
					buffer.Append(charSpan.Slice(0, charsWritten));
					byteSpan = byteSpan.Slice(byteLength);
				}

				buffer.Append('"');
			}
			else
			{
				buffer.Append("0x");
				Binary.ToHexString(asBlob.Span, buffer);
			}
		}

		#endregion -- Structure Methods --

		#region -- Type Of Methods --

		/// <summary>
		///		Determine whether the underlying value of this instance is specified type or not.
		/// </summary>
		/// <typeparam name="T">Target type.</typeparam>
		/// <returns>If the underlying value of this instance is <typeparamref name="T"/> then true, otherwise false.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "There are no meaningful parameter.")]
		public bool? IsTypeOf<T>()
		{
			return this.IsTypeOf(typeof(T));
		}

		/// <summary>
		///		Determine whether the underlying value of this instance is specified type or not.
		/// </summary>
		/// <param name="type">Target type.</param>
		/// <returns>If the underlying value of this instance is <paramref name="type"/> then true, otherwise false.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Switch")]
		public bool? IsTypeOf(Type type)
		{
			type = Ensure.NotNull(type);

			if (this._handleOrTypeCode == null)
			{
				return (type.IsValueType && Nullable.GetUnderlyingType(type) == null) ? false : default(bool?);
			}

			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if (typeCode == null)
			{
				if (type == typeof(ExtensionTypeObject))
				{
					return this._handleOrTypeCode is ReadOnlySequence<byte>;
				}

				if (type == typeof(string) || type == typeof(IList<char>) || type == typeof(IEnumerable<char>))
				{
					var asMessagePackString = this._handleOrTypeCode as MessagePackString;
					return asMessagePackString != null && asMessagePackString.GetUnderlyingType() == typeof(string);
				}

				if (type == typeof(byte[]) || type == typeof(IList<byte>) || type == typeof(IEnumerable<byte>))
				{
					return this._handleOrTypeCode is MessagePackString;
				}

				// Can IEnumerable<byte>
				if (typeof(IEnumerable<MessagePackObject>).IsAssignableFrom(type)
					&& this._handleOrTypeCode is MessagePackString)
				{
					// Raw is NOT array.
					return false;
				}

				return type.IsAssignableFrom(this._handleOrTypeCode.GetType());
			}

			// Lifting support.
#if (NETSTANDARD1_1 || NETSTANDARD1_3)
			switch ( NetStandardCompatibility.GetTypeCode( type ) )
#else
			switch (Type.GetTypeCode(type))
#endif // NETSTANDARD1_1 || NETSTANDARD1_3
			{
				case TypeCode.SByte:
				{
					return typeCode.IsInteger && (this._value < 0x80 || (0xFFFFFFFFFFFFFF80 <= this._value && typeCode.IsSigned));
				}
				case TypeCode.Byte:
				{
					return typeCode.IsInteger && this._value < 0x100;
				}
				case TypeCode.Int16:
				{
					return typeCode.IsInteger && (this._value < 0x8000 || (0xFFFFFFFFFFFF8000 <= this._value && typeCode.IsSigned));
				}
				case TypeCode.UInt16:
				{
					return typeCode.IsInteger && this._value < 0x10000;
				}
				case TypeCode.Int32:
				{
					return typeCode.IsInteger && (this._value < 0x80000000 || (0xFFFFFFFF80000000 <= this._value && typeCode.IsSigned));
				}
				case TypeCode.UInt32:
				{
					return typeCode.IsInteger && this._value < 0x100000000L;
				}
				case TypeCode.Int64:
				{
					return typeCode.IsInteger && (this._value < 0x8000000000000000L || typeCode.IsSigned);
				}
				case TypeCode.UInt64:
				{
					return typeCode.IsInteger && (this._value < 0x8000000000000000L || !typeCode.IsSigned);
				}
				case TypeCode.Double:
				{
					return
						typeCode.Type == typeof(float)
						|| typeCode.Type == typeof(double);
				}
			}

			return typeCode.Type == type;
		}

		/// <summary>
		///		Get the value indicates whether this instance wraps raw binary (or string) or not.
		/// </summary>
		/// <value>This instance wraps raw binary (or string) then true.</value>
		public bool IsRaw
		{
			get { return !this.IsNil && (this._handleOrTypeCode is MessagePackString); }
		}

		/// <summary>
		///		Get the value indicates whether this instance wraps list (array) or not.
		/// </summary>
		/// <value>This instance wraps list (array) then true.</value>
		public bool IsList
		{
			get { return !this.IsNil && this._handleOrTypeCode is IList<MessagePackObject>; }
		}

		/// <summary>
		///		Get the value indicates whether this instance wraps list (array) or not.
		/// </summary>
		/// <value>This instance wraps list (array) then true.</value>
		public bool IsArray
		{
			get { return this.IsList; }
		}

		/// <summary>
		///		Get the value indicates whether this instance wraps dictionary (map) or not.
		/// </summary>
		/// <value>This instance wraps dictionary (map) then true.</value>
		public bool IsDictionary
		{
			get { return !this.IsNil && this._handleOrTypeCode is IDictionary<MessagePackObject, MessagePackObject>; }
		}

		/// <summary>
		///		Get the value indicates whether this instance wraps dictionary (map) or not.
		/// </summary>
		/// <value>This instance wraps dictionary (map) then true.</value>
		public bool IsMap
		{
			get { return this.IsDictionary; }
		}

		/// <summary>
		///		Get underlying type of this instance.
		/// </summary>
		/// <returns>Underlying <see cref="Type"/>. <c>null</c> for <see cref="Nil"/>.</returns>
		public Type? UnderlyingType
		{
			get
			{
				if (this._handleOrTypeCode == null)
				{
					return null;
				}

				if (!(this._handleOrTypeCode is ValueTypeCode typeCode))
				{
					if (this._handleOrTypeCode is MessagePackString asMps)
					{
						return asMps.GetUnderlyingType();
					}
					else if (this._handleOrTypeCode is ReadOnlySequence<byte>)
					{
						return typeof(ExtensionTypeObject);
					}
					else
					{
						return this._handleOrTypeCode.GetType();
					}
				}
				else
				{
					return typeCode.Type;
				}
			}
		}

		#endregion -- Type Of Methods --

		#region -- Primitive Type Conversion Methods --

		/// <summary>
		///		Gets the underlying value as string encoded with specified <see cref="Encoding"/>.
		/// </summary>
		/// <returns>
		///		The string.
		///		Note that some <see cref="Encoding"/> returns <c>null</c> if the binary is not valid encoded string.
		///	</returns>
		public string? AsString(Encoding encoding)
		{
			encoding = Ensure.NotNull(encoding);

			if (this.IsNil)
			{
				return null;
			}

			VerifyUnderlyingRawType<string>(this, null);

			try
			{
				var asMessagePackString = this._handleOrTypeCode as MessagePackString;
				Debug.Assert(asMessagePackString != null, "asMessagePackString != null");

				if (encoding is UTF8Encoding)
				{
					return asMessagePackString.GetString();
				}

				return encoding.GetString(asMessagePackString.UnsafeGetMemory().Span);
			}
			catch (ArgumentException ex)
			{
				throw new InvalidOperationException($"Not a '{encoding.WebName}' string.", ex);
			}
		}

		/// <summary>
		///		Get underlying value as UTF-8 string.
		/// </summary>
		/// <returns>Underlying raw binary as <see cref="String"/> decoded with UTF-8 encoding. <c>null</c> for <see cref="Nil"/>.</returns>
		public string? AsStringUtf8()
			=> this.AsString(Utf8EncodingNonBomStrict.Instance);

		/// <summary>
		///		Get underlying value as UTF-16 string.
		/// </summary>
		/// <returns>Underlying string.</returns>
		/// <remarks>
		///		This method detects BOM. If BOM is not exist, them bytes should be Big-Endian UTF-16.
		/// </remarks>
		public string? AsStringUtf16()
		{
			VerifyUnderlyingRawType<string>(this, null);

			if (this.IsNil)
			{
				return null;
			}

			try
			{
				var asMessagePackString = this._handleOrTypeCode as MessagePackString;
				Debug.Assert(asMessagePackString != null, "asMessagePackString != null");

				if (asMessagePackString.UnsafeGetString() != null)
				{
					return asMessagePackString.UnsafeGetString();
				}

				var asBytes = asMessagePackString.UnsafeGetMemory();

				if (asBytes.Length == 0)
				{
					return String.Empty;
				}

				if (asBytes.Length % 2 != 0)
				{
					throw new InvalidOperationException("Not a UTF-16 string.");
				}

				if (asBytes.Span[0] == 0xff && asBytes.Span[1] == 0xfe)
				{
					return Encoding.Unicode.GetString(asBytes.Span.Slice(2));
				}
				else if (asBytes.Span[0] == 0xfe && asBytes.Span[1] == 0xff)
				{
					return Encoding.BigEndianUnicode.GetString(asBytes.Span.Slice(2));
				}
				else
				{
					return Encoding.BigEndianUnicode.GetString(asBytes.Span);
				}
			}
			catch (ArgumentException ex)
			{
				throw new InvalidOperationException("Not a UTF-16 string.", ex);
			}
		}

		/// <summary>
		///		Get underlying value as UTF-16 charcter array.
		/// </summary>
		/// <returns>Underlying string. <c>null</c> for <see cref="Nil"/>.</returns>
		public char[]? AsCharArray()
		{
			// TODO: More efficient
#warning TODO: NRE
			return this.AsString().ToCharArray();
		}

		#endregion -- Primitive Type Conversion Methods --

		#region -- Container Type Conversion Methods --

		/// <summary>
		///		Get underlying value as <see cref="IEnumerable&lt;MessagePackObject&gt;"/>.
		/// </summary>
		/// <returns>Underlying <see cref="IEnumerable&lt;MessagePackObject&gt;"/>. <c>null</c> for <see cref="Nil"/>.</returns>
		public IEnumerable<MessagePackObject>? AsEnumerable()
		{
			if (this.IsNil)
			{
				return null;
			}

			VerifyUnderlyingType<IEnumerable<MessagePackObject>>(this, null);

			return this._handleOrTypeCode as IEnumerable<MessagePackObject>;
		}

		/// <summary>
		///		Get underlying value as <see cref="IList&lt;MessagePackObject&gt;"/>.
		/// </summary>
		/// <returns>Underlying <see cref="IList&lt;MessagePackObject&gt;"/>.</returns>
		public IList<MessagePackObject>? AsList()
		{
			if (this.IsNil)
			{
				return null;
			}

			VerifyUnderlyingType<IList<MessagePackObject>>(this, null);

			return this._handleOrTypeCode as IList<MessagePackObject>;
		}

		/// <summary>
		///		Get underlying value as <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <returns>Underlying <see cref="MessagePackObjectDictionary"/>.</returns>
		public MessagePackObjectDictionary? AsDictionary()
		{
			if (this.IsNil)
			{
				return null;
			}

			VerifyUnderlyingType<MessagePackObjectDictionary>(this, null);

			return this._handleOrTypeCode as MessagePackObjectDictionary;
		}

		#endregion -- Container Type Conversion Methods --

		#region -- Utility Methods --

		private static void VerifyUnderlyingType<T>(MessagePackObject instance, string? parameterName)
		{
			Debug.Assert(typeof(T) != typeof(MessagePackString), "Should use VerifyUnderlyingRawType()");

			if (instance.IsNil)
			{
				if (!typeof(T).IsValueType || Nullable.GetUnderlyingType(typeof(T)) != null)
				{
					return;
				}

				// TODO: localize
				if (parameterName != null)
				{
					throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Do not convert nil MessagePackObject to {0}.", typeof(T)), parameterName);
				}
				else
				{
					ThrowCannotBeNilAs<T>();
				}
			}

			if (!instance.IsTypeOf<T>().GetValueOrDefault())
			{
				if (parameterName != null)
				{
					throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Do not convert {0} MessagePackObject to {1}.", instance.UnderlyingType, typeof(T)), parameterName);
				}
				else
				{
					ThrowInvalidTypeAs<T>(instance);
				}
			}
		}

		private static void VerifyUnderlyingRawType<T>(MessagePackObject instance, string? parameterName)
		{
			if (instance._handleOrTypeCode == null || instance._handleOrTypeCode is MessagePackString)
			{
				// nil or MPS (eventually string or byte[])
				return;
			}

			if (parameterName != null)
			{
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Do not convert {0} MessagePackObject to {1}.", instance.UnderlyingType, typeof(T)), parameterName);
			}
			else
			{
				ThrowInvalidTypeAs<T>(instance);
			}
		}

		private static void ThrowCannotBeNilAs<T>()
		{
			throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "Do not convert nil MessagePackObject to {0}.", typeof(T)));
		}

		private static void ThrowInvalidTypeAs<T>(MessagePackObject instance)
		{
			if (instance._handleOrTypeCode is ValueTypeCode)
			{
				throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "Do not convert {0} (binary:0x{2:x}) MessagePackObject to {1}.", instance.UnderlyingType, typeof(T), instance._value));
			}
			else
			{
				throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "Do not convert {0} MessagePackObject to {1}.", instance.UnderlyingType, typeof(T)));
			}
		}

		#endregion -- Utility Methods --

		/// <summary>
		///		Wraps specified object as <see cref="MessagePackObject"/> recursively.
		/// </summary>
		/// <param name="boxedValue">Object to be wrapped.</param>
		/// <returns><see cref="MessagePackObject"/> wrapps <paramref name="boxedValue"/>.</returns>
		/// <exception cref="MessageTypeException">
		///		<paramref name="boxedValue"/> is not primitive value type, list of <see cref="MessagePackObject"/>,
		///		dictionary of <see cref="MessagePackObject"/>, <see cref="String"/>, <see cref="Byte"/>[], or null.
		/// </exception>
		public static MessagePackObject FromObject(object boxedValue)
		{
			// Nullable<T> is boxed as null or underlying value type, 
			// so ( obj is Nullable<T> ) is always false.
			if (boxedValue == null)
			{
				return Nil;
			}
			else if (boxedValue is MessagePackObject)
			{
				return (MessagePackObject)boxedValue;
			}
			else if (boxedValue is sbyte)
			{
				return (sbyte)boxedValue;
			}
			else if (boxedValue is byte)
			{
				return (byte)boxedValue;
			}
			else if (boxedValue is short)
			{
				return (short)boxedValue;
			}
			else if (boxedValue is ushort)
			{
				return (ushort)boxedValue;
			}
			else if (boxedValue is int)
			{
				return (int)boxedValue;
			}
			else if (boxedValue is uint)
			{
				return (uint)boxedValue;
			}
			else if (boxedValue is long)
			{
				return (long)boxedValue;
			}
			else if (boxedValue is ulong)
			{
				return (ulong)boxedValue;
			}
			else if (boxedValue is float)
			{
				return (float)boxedValue;
			}
			else if (boxedValue is double)
			{
				return (double)boxedValue;
			}
			else if (boxedValue is bool)
			{
				return (bool)boxedValue;
			}
			else if (boxedValue is byte[] asByteArray)
			{
				return new MessagePackObject(asByteArray);
			}
			else if (boxedValue is string asString)
			{
				return new MessagePackObject(asString);
			}
			else if (boxedValue is IEnumerable<byte> asByteEnumerable)
			{
				return new MessagePackObject((asByteEnumerable).ToArray());
			}
			else if (boxedValue is Memory<byte> asByteMemory)
			{
				return new MessagePackObject(asByteMemory);
			}
			else if (boxedValue is ReadOnlyMemory<byte> asReadOnlyByteMemory)
			{
				return new MessagePackObject(asReadOnlyByteMemory);
			}
			else if (boxedValue is IEnumerable<char> asCharEnumerable)
			{
				return new MessagePackObject(new string((asCharEnumerable).ToArray()));
			}
			else if (boxedValue is Memory<char> asCharMemory)
			{
				return new MessagePackObject(asCharMemory);
			}
			else if (boxedValue is ReadOnlyMemory<char> asReadOnlyCharMemory)
			{
				return new MessagePackObject(asReadOnlyCharMemory);
			}
			else if (boxedValue is IList<MessagePackObject> asList)
			{
				return new MessagePackObject(asList, false);
			}
			else if (boxedValue is IReadOnlyList<MessagePackObject> asReadOnlyList)
			{
				return new MessagePackObject(asReadOnlyList, true);
			}
			else if (boxedValue is IEnumerable<MessagePackObject> asEnumerable)
			{
				return new MessagePackObject((asEnumerable).ToArray() as IList<MessagePackObject>, true);
			}
			else if (boxedValue is MessagePackObjectDictionary asDictionary)
			{
				return new MessagePackObject(asDictionary, false);
			}
#pragma warning disable CS0618
			else if (boxedValue is MessagePackExtendedTypeObject mpext)
			{
				return new MessagePackObject(mpext);
			}
#pragma warning restore CS0618
			else if (boxedValue is ExtensionTypeObject ext)
			{
				return new MessagePackObject(ext);
			}

			throw new MessageTypeException(String.Format(CultureInfo.CurrentCulture, "Type '{0}' is not supported.", boxedValue.GetType()));
		}

		/// <summary>
		///		Get boxed underlying value for this object.
		/// </summary>
		/// <returns>Boxed underlying value for this object.</returns>
		public object? ToObject()
		{
			if (this._handleOrTypeCode == null)
			{
				return null;
			}

			if (!(this._handleOrTypeCode is ValueTypeCode asType))
			{
				if (this._handleOrTypeCode is MessagePackString asBinary)
				{
					var asString = asBinary.TryGetString();
					if (asString != null)
					{
						return asString;
					}

					return asBinary.UnsafeGetMemory().ToArray();
				}

				if (this._handleOrTypeCode is IList<MessagePackObject> asList)
				{
					return asList;
				}

				if (this._handleOrTypeCode is IDictionary<MessagePackObject, MessagePackObject> asDictionary)
				{
					return asDictionary;
				}

				if (this._handleOrTypeCode is ReadOnlySequence<byte> asExtendedTypeObject)
				{
					return new ExtensionTypeObject(new ExtensionType(unchecked((long)this._value)), asExtendedTypeObject);
				}

				Debug.Fail($"Unexpected handle type: {this._handleOrTypeCode.GetType()}");
				// ReSharper disable once HeuristicUnreachableCode
				return null;
			}
			else
			{
				switch (asType.TypeCode)
				{
					case MessagePackValueTypeCode.Boolean:
					{
						return this.AsBoolean();
					}
					case MessagePackValueTypeCode.Int8:
					{
						return this.AsSByte();
					}
					case MessagePackValueTypeCode.Int16:
					{
						return this.AsInt16();
					}
					case MessagePackValueTypeCode.Int32:
					{
						return this.AsInt32();
					}
					case MessagePackValueTypeCode.Int64:
					{
						return this.AsInt64();
					}
					case MessagePackValueTypeCode.UInt8:
					{
						return this.AsByte();
					}
					case MessagePackValueTypeCode.UInt16:
					{
						return this.AsUInt16();
					}
					case MessagePackValueTypeCode.UInt32:
					{
						return this.AsUInt32();
					}
					case MessagePackValueTypeCode.UInt64:
					{
						return this.AsUInt64();
					}
					case MessagePackValueTypeCode.Single:
					{
						return this.AsSingle();
					}
					case MessagePackValueTypeCode.Double:
					{
						return this.AsDouble();
					}
					default:
					{
						Debug.Assert(false, $"Unknwon type code: {asType.TypeCode}");
						// ReSharper disable once HeuristicUnreachableCode
						return null;
					}
				}
			}
		}

		/// <summary>
		///		Gets a this object as a <see cref="Timestamp"/> value.
		/// </summary>
		/// <returns>A <see cref="Timestamp"/> value.</returns>
		/// <exception cref="InvalidOperationException">This object does not represent <see cref="Timestamp"/> value.</exception>
		public Timestamp AsTimestamp()
		{
			try
			{
				return Timestamp.Decode(this.AsMessagePackExtendedTypeObject());
			}
			catch (ArgumentException ex)
			{
				throw new InvalidOperationException(ex.Message, ex);
			}
		}

		#region -- Structure Operator Overloads --

		/// <summary>
		///		Compare two instances are equal.
		/// </summary>
		/// <param name="left"><see cref="MessagePackObject"/> instance.</param>
		/// <param name="right"><see cref="MessagePackObject"/> instance.</param>
		/// <returns>
		///		Whether value of <paramref name="left"/> and <paramref name="right"/> are equal each other or not.
		/// </returns>
		public static bool operator ==(MessagePackObject left, MessagePackObject right)
		{
			return left.Equals(right);
		}

		/// <summary>
		///		Compare two instances are not equal.
		/// </summary>
		/// <param name="left"><see cref="MessagePackObject"/> instance.</param>
		/// <param name="right"><see cref="MessagePackObject"/> instance.</param>
		/// <returns>
		///		Whether value of <paramref name="left"/> and <paramref name="right"/> are not equal each other or are equal.
		/// </returns>
		public static bool operator !=(MessagePackObject left, MessagePackObject right)
		{
			return !left.Equals(right);
		}

		#endregion -- Structure Operator Overloads --


		#region -- Conversion Operator Overloads --

		/// <summary>
		///		Convert <see cref="MessagePackObject"/>[] instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/>[] instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject(MessagePackObject[] value)
		{
			return new MessagePackObject(value as IList<MessagePackObject>, false);
		}

		#endregion -- Conversion Operator Overloads --

#if DEBUG
#if UNITY && DEBUG
		public
#else
		internal
#endif
		string DebugDump()
		{
			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if (typeCode != null)
			{
				return String.Format(CultureInfo.InvariantCulture, "{0}([{1}]0x{0:X})", this._value, typeCode.Type);
			}
			else if (this._handleOrTypeCode != null)
			{
				return String.Format(CultureInfo.InvariantCulture, "{0}([{1}])", this._handleOrTypeCode, this._handleOrTypeCode.GetType());
			}
			else
			{
				return "(null)";
			}
		}
#endif // DEBUG

#if !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
		[Serializable]
#endif // !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
		internal enum MessagePackValueTypeCode
		{
			// ReSharper disable once UnusedMember.Local
			Nil = 0,
			Int8 = 1,
			Int16 = 3,
			Int32 = 5,
			Int64 = 7,
			UInt8 = 2,
			UInt16 = 4,
			UInt32 = 6,
			UInt64 = 8,
			Boolean = 10,
			Single = 11,
			Double = 13,
			Object = 16
		}

#if !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
		[Serializable]
#endif // !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
		internal sealed class ValueTypeCode
		{
			private readonly MessagePackValueTypeCode _typeCode;

			public MessagePackValueTypeCode TypeCode
			{
				get { return this._typeCode; }
			}

			public bool IsSigned
			{
				get { return ((int)this._typeCode) % 2 != 0; }
			}

			public bool IsInteger
			{
				get { return ((int)this._typeCode) < 10; }
			}

			private readonly Type _type;

			public Type Type
			{
				get { return this._type; }
			}

			internal ValueTypeCode(Type type, MessagePackValueTypeCode typeCode)
			{
				this._type = type;
				this._typeCode = typeCode;
			}

			public override string ToString() =>
				// For debuggability.
				this._typeCode == MessagePackValueTypeCode.Object ? this._type.ToString() : this._typeCode.ToString()!;
		}
	}
}
