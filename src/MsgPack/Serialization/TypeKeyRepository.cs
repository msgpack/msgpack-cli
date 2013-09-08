#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
#if NETFX_CORE
using System.Reflection;
#endif
using System.Runtime.CompilerServices;
using System.Threading;
using System.Security;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Repository for key type with RWlock scheme.
	/// </summary>
	internal class TypeKeyRepository : IDisposable
	{
		private volatile int _isFrozen;

		public bool IsFrozen
		{
			get { return this._isFrozen != 0; }
		}

		private readonly ReaderWriterLockSlim _lock;

		private readonly Dictionary<RuntimeTypeHandle, object> _table;

		public TypeKeyRepository() : this( default( TypeKeyRepository ) ) { }

		public TypeKeyRepository( TypeKeyRepository copiedFrom )
		{
			this._lock = new ReaderWriterLockSlim( LockRecursionPolicy.NoRecursion );

			if ( copiedFrom == null )
			{
				this._table = new Dictionary<RuntimeTypeHandle, object>();
			}
			else
			{
				this._table = copiedFrom.GetClonedTable();
			}
		}

		public TypeKeyRepository( Dictionary<RuntimeTypeHandle, object> table )
		{
			this._lock = new ReaderWriterLockSlim( LockRecursionPolicy.NoRecursion );
			this._table = table;
		}

		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			if ( disposing )
			{
				this._lock.Dispose();
			}
		}

#if !PARTIAL_TRUST && !SILVERLIGHT
		[SecuritySafeCritical]
#endif
		private Dictionary<RuntimeTypeHandle, object> GetClonedTable()
		{
			bool holdsReadLock = false;
#if !SILVERLIGHT && !NETFX_CORE
			RuntimeHelpers.PrepareConstrainedRegions();
#endif
			try { }
			finally
			{
				this._lock.EnterReadLock();
				holdsReadLock = true;
			}
#if !SILVERLIGHT && !NETFX_CORE
			RuntimeHelpers.PrepareConstrainedRegions();
#endif
			try
			{
				return new Dictionary<RuntimeTypeHandle, object>( this._table );
			}
			finally
			{
				if ( holdsReadLock )
				{
					this._lock.ExitReadLock();
				}
			}
		}

		public bool Get( Type type, out object matched, out object genericDefinitionMatched )
		{
			return this.GetCore( type, out matched, out genericDefinitionMatched );
		}

#if !PARTIAL_TRUST && !SILVERLIGHT
		[SecuritySafeCritical]
#endif
		private bool GetCore( Type type, out object matched, out object genericDefinitionMatched )
		{
			bool holdsReadLock = false;
#if !SILVERLIGHT && !NETFX_CORE
			RuntimeHelpers.PrepareConstrainedRegions();
#endif
			try { }
			finally
			{
				this._lock.EnterReadLock();
				holdsReadLock = true;
			}
#if !SILVERLIGHT && !NETFX_CORE
			RuntimeHelpers.PrepareConstrainedRegions();
#endif
			try
			{
				object result;
				if ( this._table.TryGetValue( type.TypeHandle, out result ) )
				{
					matched = result;
					genericDefinitionMatched = null;
					return true;
				}

				if ( type.GetIsGenericType() )
				{
					if ( this._table.TryGetValue( type.GetGenericTypeDefinition().TypeHandle, out result ) )
					{
						matched = null;
						genericDefinitionMatched = result;
						return true;
					}
				}

				matched = null;
				genericDefinitionMatched = null;
				return false;
			}
			finally
			{
				if ( holdsReadLock )
				{
					this._lock.ExitReadLock();
				}
			}
		}

		public void Freeze()
		{
			this._isFrozen = 1;
		}

		public bool Register( Type type, object entry, bool allowOverwrite )
		{
			Contract.Assert( entry != null );

			if ( this.IsFrozen )
			{
				throw new InvalidOperationException( "This repository is frozen." );
			}

			return this.RegisterCore( type, entry, allowOverwrite );
		}

#if !PARTIAL_TRUST && !SILVERLIGHT
		[SecuritySafeCritical]
#endif
		private bool RegisterCore( Type key, object value, bool allowOverwrite )
		{
			if ( allowOverwrite || !this._table.ContainsKey( key.TypeHandle ) )
			{
				bool holdsWriteLock = false;
#if !SILVERLIGHT && !NETFX_CORE
				RuntimeHelpers.PrepareConstrainedRegions();
#endif
				try { }
				finally
				{
					this._lock.EnterWriteLock();
					holdsWriteLock = true;
				}
#if !SILVERLIGHT && !NETFX_CORE
				RuntimeHelpers.PrepareConstrainedRegions();
#endif
				try
				{
					if ( allowOverwrite || !this._table.ContainsKey( key.TypeHandle ) )
					{
						this._table[ key.TypeHandle ] = value;
						return true;
					}
				}
				finally
				{
					if ( holdsWriteLock )
					{
						this._lock.ExitWriteLock();
					}
				}
			}

			return false;
		}



		public bool Unregister( Type type )
		{
			if ( this.IsFrozen )
			{
				throw new InvalidOperationException( "This repository is frozen." );
			}

			return this.UnregisterCore( type );
		}

#if !PARTIAL_TRUST && !SILVERLIGHT
		[SecuritySafeCritical]
#endif
		private bool UnregisterCore( Type key )
		{
			if ( this._table.ContainsKey( key.TypeHandle ) )
			{
				bool holdsWriteLock = false;
#if !SILVERLIGHT && !NETFX_CORE
				RuntimeHelpers.PrepareConstrainedRegions();
#endif
				try { }
				finally
				{
					this._lock.EnterWriteLock();
					holdsWriteLock = true;
				}
#if !SILVERLIGHT && !NETFX_CORE
				RuntimeHelpers.PrepareConstrainedRegions();
#endif
				try
				{
					return this._table.Remove( key.TypeHandle );
				}
				finally
				{
					if ( holdsWriteLock )
					{
						this._lock.ExitWriteLock();
					}
				}
			}

			return false;
		}

#if !PARTIAL_TRUST && !SILVERLIGHT
		[SecuritySafeCritical]
#endif
		internal bool Coontains( Type type )
		{
			bool holdsReadLock = false;
#if !SILVERLIGHT && !NETFX_CORE
			RuntimeHelpers.PrepareConstrainedRegions();
#endif
			try { }
			finally
			{
				this._lock.EnterReadLock();
				holdsReadLock = true;
			}
#if !SILVERLIGHT && !NETFX_CORE
			RuntimeHelpers.PrepareConstrainedRegions();
#endif
			try
			{
				return this._table.ContainsKey( type.TypeHandle );
			}
			finally
			{
				if ( holdsReadLock )
				{
					this._lock.ExitReadLock();
				}
			}
		}
	}
}
