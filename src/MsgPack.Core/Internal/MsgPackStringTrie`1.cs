// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MsgPack.Internal
{
	// Basic idea is borrwed from AutomataDictionary of Message Pack C#
	// https://github.com/neuecc/MessagePack-CSharp/blob/51649e0d7b8641ad5d3cdd6dfdc130c7671066fc/src/MessagePack.UnityClient/Assets/Scripts/MessagePack/Internal/AutomataDictionary.cs#L1

	public sealed class MsgPackStringTrie<T>
	{
		private readonly T _default;
		private readonly Node _root;

		public MsgPackStringTrie(T defaultValue)
		{
			this._default = defaultValue;
			this._root = new Node(defaultValue, Array.Empty<Node>(), Array.Empty<ulong>());
		}

		public bool TryAdd(ReadOnlySpan<byte> utf8Key, T value)
			=> this.TryAdd(this._root, GenerateKeyHead(ref utf8Key), utf8Key, value);

		private static int BinarySearch(ulong[] nodes, ulong key)
		{
			// Span<T>.BinarySearch is slower maybe because of ComapreTo method overhead, so we implement binary search manually.
			var low = 0;
			var high = nodes.Length - 1;
			while (low <= high)
			{
				// Peformance trick borrowed from https://github.com/dotnet/runtime/blob/f2786223508c0c70040fbf48ec3a39a607dd7f75/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.BinarySearch.cs#L42
				var index = unchecked((int)(((uint)high + (uint)low) >> 1));
				var found = nodes[index];
				if (found == key)
				{
					return index;
				}
				else if (found < key)
				{
					low = index + 1;
				}
				else
				{
					high = index - 1;
				}
			}

			return ~low;
		}

		public T GetOrDefault(ReadOnlySpan<byte> msgPackStringKey)
		{
			var result = this.Find(this._root, GenerateKeyRest(ref msgPackStringKey), msgPackStringKey);
			return result != null ? result.Value : this._default;
		}

		private bool TryAdd(Node parent, ulong key, ReadOnlySpan<byte> trailingKey, T value)
		{
			var nodes = parent.ChildNodes;
			var keys = parent.ChildKeys;

			while (true)
			{
				var index = BinarySearch(keys, key);

				if (index < 0)
				{
					// No matching leaf.
					this.AddNode(parent, key, trailingKey, ~index, value, out _);
					return true;
				}

				var found = nodes[index];
				if (trailingKey.IsEmpty)
				{
					// The leaf matches.
					return false;
				}

				if (found.ChildNodes.Length == 0)
				{
					// Search key is longer than trie path.
					this.AddNode(found, key, trailingKey, 0, value, out _);
					return true;
				}

				nodes = found.ChildNodes;
				keys = found.ChildKeys;

				key = GenerateKeyRest(ref trailingKey);
			}
		}

		private Node? Find(Node parent, ulong key, ReadOnlySpan<byte> trailingKey)
		{
			var nodes = parent.ChildNodes;
			var keys = parent.ChildKeys;

			while (true)
			{
				var index = BinarySearch(keys, key);

				if (index < 0)
				{
					// No matching leaf.
					return null;
				}

				var found = nodes[index];
				if (trailingKey.IsEmpty)
				{
					// The leaf matches.
					return found;
				}

				if (found.ChildNodes.Length == 0)
				{
					// Search key is longer than trie path.
					return null;
				}

				nodes = found.ChildNodes;
				keys = found.ChildKeys;
				key = GenerateKeyRest(ref trailingKey);
			}
		}

		private void AddNode(Node node, ulong key, ReadOnlySpan<byte> trailingKey, int targetIndex, T value, out Node result)
		{
			Array.Resize(ref node.ChildKeys, node.ChildKeys.Length + 1);
			Array.Resize(ref node.ChildNodes, node.ChildNodes.Length + 1);
			var nodes = node.ChildNodes;
			var keys = node.ChildKeys;

			while (true)
			{
				var child =
					trailingKey.IsEmpty ?
						new Node(value, Array.Empty<Node>(), Array.Empty<ulong>()) :
						new Node(this._default, new Node[1], new ulong[1]);

				if (nodes.Length > 1 && targetIndex < nodes.Length - 1)
				{
					// Shift existing.
					Array.Copy(nodes, targetIndex, nodes, targetIndex + 1, nodes.Length - targetIndex - 1);
					Array.Copy(keys, targetIndex, keys, targetIndex + 1, keys.Length - targetIndex - 1);
				}

				nodes[targetIndex] = child;
				keys[targetIndex] = key;

				if (trailingKey.IsEmpty)
				{
					// This is leaf.
					result = child;
					return;
				}

				nodes = child.ChildNodes;
				keys = child.ChildKeys;

				Debug.Assert(nodes.Length == 1);
				Debug.Assert(keys.Length == 1);
				targetIndex = 0;

				key = GenerateKeyRest(ref trailingKey);
			}
		}

		private static ulong GenerateKeyHead(ref ReadOnlySpan<byte> source)
		{
			Span<byte> bytes = stackalloc byte[sizeof(ulong)];
			var buffer = bytes;
			int consumes;
			if (source.Length < 16)
			{
				buffer[0] = (byte)(0xA0 | source.Length);
				buffer = buffer.Slice(1);
				consumes = Math.Min(source.Length, sizeof(long) - 1);
			}
			else if (source.Length <= Byte.MaxValue)
			{
				buffer[0] = 0xD9;
				buffer[1] = (byte)source.Length;
				buffer = buffer.Slice(2);
				consumes = Math.Min(source.Length, sizeof(long) - 1 - 1);
			}
			else if (source.Length <= UInt16.MaxValue)
			{
				buffer[0] = 0xDA;
				buffer = buffer.Slice(1);
				BinaryPrimitives.WriteUInt16BigEndian(buffer, (ushort)source.Length);
				buffer = buffer.Slice(sizeof(ushort));
				consumes = Math.Min(source.Length, sizeof(long) - 1 - sizeof(ushort));
			}
			else
			{
				buffer[0] = 0xDB;
				buffer = buffer.Slice(1);
				BinaryPrimitives.WriteInt32BigEndian(buffer, source.Length);
				buffer = buffer.Slice(sizeof(int));
				consumes = Math.Min(source.Length, sizeof(long) - 1 - sizeof(int));
			}

			source.Slice(0, consumes).CopyTo(buffer);
			source = source.Slice(consumes);
			return MemoryMarshal.Cast<byte, ulong>(bytes)[0];
		}

		private static ulong GenerateKeyRest(ref ReadOnlySpan<byte> source)
		{
			Debug.Assert(!source.IsEmpty);
			if (source.Length >= sizeof(ulong))
			{
				var result = MemoryMarshal.Cast<byte, ulong>(source)[0];
				source = source.Slice(sizeof(ulong));
				return result;
			}
			else
			{
				ulong result;
				unchecked
				{
					switch (source.Length)
					{
						case 1:
						{
							result = source[0];
							break;
						}
						case 2:
						{
							result = BinaryPrimitives.ReadUInt16LittleEndian(source);
							break;
						}
						case 3:
						{
							result = BinaryPrimitives.ReadUInt16LittleEndian(source);
							source = source.Slice(2);
							result |= (uint)(source[0] << 16);
							break;
						}
						case 4:
						{
							result = BinaryPrimitives.ReadUInt32LittleEndian(source);
							break;
						}
						case 5:
						{
							result = BinaryPrimitives.ReadUInt32LittleEndian(source);
							source = source.Slice(4);
							result |= ((ulong)source[0] << 32);
							break;
						}
						case 6:
						{
							result = BinaryPrimitives.ReadUInt32LittleEndian(source);
							source = source.Slice(4);
							result |= ((ulong)BinaryPrimitives.ReadUInt16LittleEndian(source) << 32);
							break;
						}
						default: // 7
						{
							result = BinaryPrimitives.ReadUInt32LittleEndian(source);
							source = source.Slice(4);
							result |= ((ulong)BinaryPrimitives.ReadUInt16LittleEndian(source) << 32);
							source = source.Slice(2);
							result |= ((ulong)source[0] << 48);
							break;
						}
					}
				}

				source = ReadOnlySpan<byte>.Empty;
				return result;
			}
		}

		private sealed class Node
		{
			// There 2 separate array to improve search performance due to CPU cache line and prediction.
			public ulong[] ChildKeys;
			public Node[] ChildNodes;
			public readonly T Value;

			public Node(T value, Node[] childNodes, ulong[] childKeys)
			{
				this.Value = value;
				this.ChildNodes = childNodes;
				this.ChildKeys = childKeys;
			}
		}
	}
}
