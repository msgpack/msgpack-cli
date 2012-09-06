#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
	internal sealed class TypeKeyRepository : IDisposable
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
			this._lock.Dispose();
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

		public TEntry Get<T, TEntry>( SerializationContext context )
			where TEntry : class
		{
			object matched;
			object genericDefinitionMatched;
			if ( !this.Get<T>( out matched, out genericDefinitionMatched ) )
			{
				return null;
			}

			if ( matched != null )
			{
				return matched as TEntry;
			}
			else
			{
				Contract.Assert( typeof( T ).GetIsGenericType() );
				Contract.Assert( !typeof( T ).GetIsGenericTypeDefinition() );
				var type = genericDefinitionMatched as Type;
				Contract.Assert( type != null );
				Contract.Assert( type.GetIsGenericTypeDefinition() );
				var result = ( TEntry )Activator.CreateInstance( type.MakeGenericType( typeof( T ).GetGenericArguments() ), context );
				Contract.Assert( result != null );
				return result;
			}
		}

#if !PARTIAL_TRUST && !SILVERLIGHT
		[SecuritySafeCritical]
#endif
		private bool Get<T>( out object matched, out object genericDefinitionMatched )
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
				if ( this._table.TryGetValue( typeof( T ).TypeHandle, out result ) )
				{
					matched = result;
					genericDefinitionMatched = null;
					return true;
				}

				if ( typeof( T ).GetIsGenericType() )
				{
					if ( this._table.TryGetValue( typeof( T ).GetGenericTypeDefinition().TypeHandle, out result ) )
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

		public bool Register<T>( object entry )
		{
			Contract.Assert( entry != null );

			if ( this.IsFrozen )
			{
				throw new InvalidOperationException( "This repository is frozen." );
			}

			return this.Register( typeof( T ), entry );
		}

#if !PARTIAL_TRUST && !SILVERLIGHT
		[SecuritySafeCritical]
#endif
		private bool Register( Type key, object value )
		{
			if ( !this._table.ContainsKey( key.TypeHandle ) )
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
					if ( !this._table.ContainsKey( key.TypeHandle ) )
					{
						this._table.Add( key.TypeHandle, value );
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
	}
}
