// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Naive FIFO cache.
	/// </summary>
	/// <typeparam name="TKey">Type of key.</typeparam>
	/// <typeparam name="TValue">Type of value.</typeparam>
	internal sealed class NaiveConcurrentFifoCache<TKey, TValue> : IDisposable
		where TKey : notnull
	{
		private readonly ReaderWriterLockSlim _lock;
		private readonly int _capacity;
		private readonly Queue<TKey> _queue;
		private readonly Dictionary<TKey, TValue> _cache;

		public NaiveConcurrentFifoCache(int capacity)
		{
			Debug.Assert(capacity > 0);
			this._capacity = capacity;
			this._queue = new Queue<TKey>(capacity);
			this._cache = new Dictionary<TKey, TValue>(capacity);
			this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
		}

		public void Dispose()
		{
			this._lock.Dispose();
		}

		public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
		{
			this._lock.EnterReadLock();
			try
			{
				return this._cache.TryGetValue(key, out value);
			}
			finally
			{
				this._lock.ExitReadLock();
			}
		}

		public void Put(TKey key, TValue value, Func<TValue, TValue, TValue> merge)
		{
			this._lock.EnterWriteLock();
			try
			{
				if (this._cache.Count == this._capacity)
				{
					this._cache.Remove(this._queue.Dequeue());
				}

				if (this._cache.TryAdd(key, value))
				{
					this._queue.Enqueue(key);
				}
				else
				{
					this._cache[key] = merge(this._cache[key], value);
				}
			}
			finally
			{
				this._lock.ExitWriteLock();
			}
		}
	}
}
