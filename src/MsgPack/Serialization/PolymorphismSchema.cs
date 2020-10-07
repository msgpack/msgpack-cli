// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
#if !FEATURE_READ_ONLY_COLLECTION
using System.Diagnostics.CodeAnalysis;
#endif // !FEATURE_READ_ONLY_COLLECTION
using System.Linq;

namespace MsgPack.Serialization
{
	/// <summary>
	///		A provider parameter to support polymorphism.
	/// </summary>
	[DebuggerDisplay("{DebugString}")]
	public sealed partial class PolymorphismSchema
	{
		/// <summary>
		///		Gets the type of the serialization target.
		/// </summary>
		/// <value>
		///		The type of the serialization target. This value can be <c>null</c>.
		/// </value>
		internal Type? TargetType { get; private set; }

		/// <summary>
		///		Gets the type of the polymorphism.
		/// </summary>
		/// <value>
		///		The type of the polymorphism.
		/// </value>
		internal PolymorphismType PolymorphismType { get; private set; }

		private readonly ReadOnlyDictionary<string, Type> _codeTypeMapping;

		/// <summary>
		///		Gets the code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.
		/// </summary>
		/// <value>
		///		The code type mapping which maps between ext-type codes and .NET <see cref="Type"/>s.
		/// </value>
		internal IDictionary<string, Type> CodeTypeMapping { get { return this._codeTypeMapping; } }

		internal bool UseDefault { get { return this.PolymorphismType == PolymorphismType.None; } }

		internal bool UseTypeEmbedding { get { return this.PolymorphismType == PolymorphismType.RuntimeType; } }

		internal Func<PolymorphicTypeVerificationContext, bool> TypeVerifier { get; private set; }

		internal PolymorphismSchemaChildrenType ChildrenType { get; private set; }

		private readonly ReadOnlyCollection<PolymorphismSchema> _children;

		/// <summary>
		///		Gets the schema for child items of the serialization target collection/tuple.
		/// </summary>
		/// <value>
		///		The schema for child items of the serialization target collection/tuple.
		/// </value>
		internal IList<PolymorphismSchema> ChildSchemaList
			=> this._children;

		/// <summary>
		///		Gets the schema for collection items of the serialization target collection.
		/// </summary>
		/// <value>
		///		The schema for collection items of the serialization target collection.
		/// </value>
		internal PolymorphismSchema? ItemSchema
		{
			get
			{
				switch (this.ChildrenType)
				{
					case PolymorphismSchemaChildrenType.None:
					{
						return null;
					}
					case PolymorphismSchemaChildrenType.CollectionItems:
					{
						return this._children.FirstOrDefault();
					}
					case PolymorphismSchemaChildrenType.DictionaryKeyValues:
					{
						return this._children.Skip(1).FirstOrDefault();
					}
					default:
					{
						throw new NotSupportedException();
					}
				}
			}
		}

		private PolymorphismSchema? TryGetItemSchema()
		{
			switch (this.ChildrenType)
			{
				case PolymorphismSchemaChildrenType.CollectionItems:
				{
					return this._children.FirstOrDefault();
				}
				case PolymorphismSchemaChildrenType.DictionaryKeyValues:
				{
					return this._children.Skip(1).FirstOrDefault();
				}
				default:
				{
					return null;
				}
			}
		}

		/// <summary>
		///		Gets the schema for dictionary keys of the serialization target collection.
		/// </summary>
		/// <value>
		///		The schema for collection items of the serialization target collection.
		/// </value>
		internal PolymorphismSchema? KeySchema
		{
			get
			{
				switch (this.ChildrenType)
				{
					case PolymorphismSchemaChildrenType.None:
					{
						return null;
					}
					case PolymorphismSchemaChildrenType.DictionaryKeyValues:
					{
						return this._children.FirstOrDefault();
					}
					default:
					{
						throw new NotSupportedException();
					}
				}
			}
		}

		private PolymorphismSchema? TryGetKeySchema()
		{
			if (this.ChildrenType == PolymorphismSchemaChildrenType.DictionaryKeyValues)
			{
				return this._children.FirstOrDefault();
			}
			else
			{
				return null;
			}
		}

#if !FEATURE_READ_ONLY_COLLECTION
		private sealed class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
			where TKey : notnull
		{
			private readonly IDictionary<TKey, TValue> _underlying;

			ICollection<TKey> IDictionary<TKey, TValue>.Keys
				=> this._underlying.Keys;

			ICollection<TValue> IDictionary<TKey, TValue>.Values
				=> this._underlying.Values;

			TValue IDictionary<TKey, TValue>.this[TKey key]
			{
				get => this._underlying[key];
				set => throw new NotSupportedException();
			}

			int ICollection<KeyValuePair<TKey, TValue>>.Count => this._underlying.Count;

			bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => true;

			public ReadOnlyDictionary(IDictionary<TKey, TValue> underlying)
			{
				this._underlying = underlying;
			}

			bool IDictionary<TKey, TValue>.ContainsKey(TKey key)
				=> this._underlying.ContainsKey(key);

			bool IDictionary<TKey, TValue>.TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
				=> this._underlying.TryGetValue(key, out value);

			bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
				=> this._underlying.Contains(item);

			void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
				=> this._underlying.CopyTo(array, arrayIndex);

			IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
				=> this._underlying.GetEnumerator();

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
				=> this._underlying.GetEnumerator();

			void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
				=> throw new NotSupportedException();

			bool IDictionary<TKey, TValue>.Remove(TKey key)
				=> throw new NotSupportedException();

			void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
				=> throw new NotSupportedException();

			void ICollection<KeyValuePair<TKey, TValue>>.Clear()
				=> throw new NotSupportedException();

			bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
				=> throw new NotSupportedException();
		}
#endif // !FEATURE_READ_ONLY_COLLECTION
	}
}
