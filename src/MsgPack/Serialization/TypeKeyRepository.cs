// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
#if FEATURE_CODE_ACCESS_SECURITY
using System.Security;
#endif // FEATURE_CODE_ACCESS_SECURITY
using System.Threading;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Repository for key type with RWlock scheme.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Repository should not be disposable because it may be shared so it is difficult to determine disposition timing")]
#if FEATURE_CODE_ACCESS_SECURITY
	[SecuritySafeCritical]
#endif // FEATURE_CODE_ACCESS_SECURITY
	internal class TypeKeyRepository
	{
		private readonly ReaderWriterLockSlim _lock;

		// We use RuntimeTypeHandle because it is bit faster than Type.
		private readonly Dictionary<RuntimeTypeHandle, object> _table;

		public TypeKeyRepository() : this(default(TypeKeyRepository)) { }

		public TypeKeyRepository(TypeKeyRepository? copiedFrom)
		{
			this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

			if (copiedFrom == null)
			{
				this._table = new Dictionary<RuntimeTypeHandle, object>();
			}
			else
			{
				this._table = copiedFrom.GetClonedTable();
			}
		}

		public TypeKeyRepository(Dictionary<RuntimeTypeHandle, object> table)
		{
			this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
			this._table = table;
		}

#if FEATURE_CODE_ACCESS_SECURITY
		[SecuritySafeCritical]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "CER is OK" )]
#endif // FEATURE_CODE_ACCESS_SECURITY
		internal Dictionary<RuntimeTypeHandle, object> GetClonedTable()
		{
			var holdsReadLock = false;
#if FEATURE_CER
			RuntimeHelpers.PrepareConstrainedRegions();
#endif // FEATURE_CER
			try
			{
#if FEATURE_CER
				RuntimeHelpers.PrepareConstrainedRegions();
#endif // FEATURE_CER
				try { }
				finally
				{
					this._lock.EnterReadLock();
					holdsReadLock = true;
				}
				return new Dictionary<RuntimeTypeHandle, object>(this._table);
			}
			finally
			{
				if (holdsReadLock)
				{
					this._lock.ExitReadLock();
				}
			}
		}

		public bool Get(Type type, out object? matched, out object? genericDefinitionMatched)
			=> this.GetCore(type, out matched, out genericDefinitionMatched);

#if FEATURE_CODE_ACCESS_SECURITY
		[SecuritySafeCritical]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "CER is OK" )]
#endif // FEATURE_CODE_ACCESS_SECURITY
		private bool GetCore(Type type, out object? matched, out object? genericDefinitionMatched)
		{
			var holdsReadLock = false;
#if !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
			RuntimeHelpers.PrepareConstrainedRegions();
#endif // !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
			try
			{
#if !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
				RuntimeHelpers.PrepareConstrainedRegions();
#endif // !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
				try { }
				finally
				{
					this._lock.EnterReadLock();
					holdsReadLock = true;
				}

				return TypeKeyRepositoryLogics.TryGet(this._table, type, out matched, out genericDefinitionMatched);
			}
			finally
			{
				if (holdsReadLock)
				{
					this._lock.ExitReadLock();
				}
			}
		}

		public bool Register(Type type, object entry, Type? nullableType, object? nullableValue, SerializerRegistrationOptions options)
		{
			Debug.Assert(entry != null, "entry != null");

			return this.RegisterCore(type, entry, nullableType, nullableValue, options);
		}

#if FEATURE_CODE_ACCESS_SECURITY
		[SecuritySafeCritical]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "CER is OK" )]
#endif //FEATURE_CODE_ACCESS_SECURITY
		private bool RegisterCore(Type key, object value, Type? nullableType, object? nullableValue, SerializerRegistrationOptions options)
		{
			var allowOverwrite = (options & SerializerRegistrationOptions.AllowOverride) != 0;

			if (allowOverwrite || !this.ContainsType(key, nullableType))
			{
				var holdsWriteLock = false;
#if FEATURE_CER
				RuntimeHelpers.PrepareConstrainedRegions();
#endif // FEATURE_CER
				try
				{
#if FEATURE_CER
					RuntimeHelpers.PrepareConstrainedRegions();
#endif // FEATURE_CER
					try { }
					finally
					{
						this._lock.EnterWriteLock();
						holdsWriteLock = true;
					}
					if (allowOverwrite || !this.ContainsType(key, nullableType))
					{
						this._table[key.TypeHandle] = value;
						if (nullableValue != null)
						{
							Debug.Assert(nullableType != null);
							this._table[nullableType.TypeHandle] = nullableValue;
						}

						return true;
					}
				}
				finally
				{
					if (holdsWriteLock)
					{
						this._lock.ExitWriteLock();
					}
				}
			}

			return false;
		}

#if FEATURE_CODE_ACCESS_SECURITY
		[SecuritySafeCritical]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "CER is OK" )]
#endif //FEATURE_CODE_ACCESS_SECURITY
		internal void Import(IEnumerable<KeyValuePair<RuntimeTypeHandle, object>> tableData)
		{
			var holdsWriteLock = false;
#if FEATURE_CER
			RuntimeHelpers.PrepareConstrainedRegions();
#endif // FEATURE_CER
			try
			{
#if FEATURE_CER
				RuntimeHelpers.PrepareConstrainedRegions();
#endif // FEATURE_CER
				try { }
				finally
				{
					this._lock.EnterWriteLock();
					holdsWriteLock = true;
				}

				this._table.Clear();
				foreach(var entry in tableData)
				{
					this._table.Add(entry.Key, entry.Value);
				}
			}
			finally
			{
				if (holdsWriteLock)
				{
					this._lock.ExitWriteLock();
				}
			}
		}

		private bool ContainsType(Type baseType, Type? nullableType)
		{
			if (nullableType == null)
			{
				return this._table.ContainsKey(baseType.TypeHandle);
			}
			else
			{
				return
					this._table.ContainsKey(baseType.TypeHandle)
					&& this._table.ContainsKey(nullableType.TypeHandle);
			}
		}

		public bool Unregister(Type type)
			=> this.UnregisterCore(type);

#if FEATURE_CODE_ACCESS_SECURITY
		[SecuritySafeCritical]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "CER is OK" )]
#endif // FEATURE_CODE_ACCESS_SECURITY
		private bool UnregisterCore(Type key)
		{
			if (this._table.ContainsKey(key.TypeHandle))
			{
				var holdsWriteLock = false;
#if FEATURE_CER
				RuntimeHelpers.PrepareConstrainedRegions();
#endif // FEATURE_CER
				try
				{
#if FEATURE_CER
					RuntimeHelpers.PrepareConstrainedRegions();
#endif // FEATURE_CER
					try { }
					finally
					{
						this._lock.EnterWriteLock();
						holdsWriteLock = true;
					}
					return this._table.Remove(key.TypeHandle);
				}
				finally
				{
					if (holdsWriteLock)
					{
						this._lock.ExitWriteLock();
					}
				}
			}

			return false;
		}

#if FEATURE_CODE_ACCESS_SECURITY
		[SecuritySafeCritical]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "CER is OK" )]
#endif // FEATURE_CODE_ACCESS_SECURITY
		internal bool Contains(Type type)
		{
			var holdsReadLock = false;
#if FEATURE_CER
			RuntimeHelpers.PrepareConstrainedRegions();
#endif // FEATURE_CER
			try
			{
#if FEATURE_CER
				RuntimeHelpers.PrepareConstrainedRegions();
#endif // FEATURE_CER
				try { }
				finally
				{
					this._lock.EnterReadLock();
					holdsReadLock = true;
				}
				return this._table.ContainsKey(type.TypeHandle);
			}
			finally
			{
				if (holdsReadLock)
				{
					this._lock.ExitReadLock();
				}
			}
		}

#if FEATURE_CODE_ACCESS_SECURITY
		[SecuritySafeCritical]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "CER is OK")]
#endif // FEATURE_CODE_ACCESS_SECURITY
		internal IEnumerable<KeyValuePair<Type, object>> GetEntries()
		{
			var holdsReadLock = false;
#if FEATURE_CER
			RuntimeHelpers.PrepareConstrainedRegions();
#endif // FEATURE_CER
			try
			{
#if FEATURE_CER
				RuntimeHelpers.PrepareConstrainedRegions();
#endif // FEATURE_CER
				try { }
				finally
				{
					this._lock.EnterReadLock();
					holdsReadLock = true;
				}
				return this._table.Select(kv => new KeyValuePair<Type, object>(Type.GetTypeFromHandle(kv.Key), kv.Value)).ToArray();
			}
			finally
			{
				if (holdsReadLock)
				{
					this._lock.ExitReadLock();
				}
			}
		}
	}
}
