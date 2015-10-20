#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
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
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Repository should not be disposable because it may be shared so it is difficult to determine disposition timing" )]
#if !NETFX_35 && !UNITY
	[SecuritySafeCritical]
#endif
	internal class TypeKeyRepository
	{
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

#if !NETFX_35 && !UNITY
		[SecuritySafeCritical]
#endif
#if NETFX_35 || UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "CER is OK" )]
#endif // NETFX_35 || UNITY
		private Dictionary<RuntimeTypeHandle, object> GetClonedTable()
		{
			bool holdsReadLock = false;
#if !SILVERLIGHT && !NETFX_CORE
			RuntimeHelpers.PrepareConstrainedRegions();
#endif
			try
			{
#if !SILVERLIGHT && !NETFX_CORE
				RuntimeHelpers.PrepareConstrainedRegions();
#endif
				try { }
				finally
				{
					this._lock.EnterReadLock();
					holdsReadLock = true;
				}
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

#if !NETFX_35 && !UNITY
		[SecuritySafeCritical]
#endif
#if NETFX_35 || UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "CER is OK" )]
#endif // NETFX_35 || UNITY
		private bool GetCore( Type type, out object matched, out object genericDefinitionMatched )
		{
			bool holdsReadLock = false;
#if !SILVERLIGHT && !NETFX_CORE
			RuntimeHelpers.PrepareConstrainedRegions();
#endif
			try
			{
#if !SILVERLIGHT && !NETFX_CORE
				RuntimeHelpers.PrepareConstrainedRegions();
#endif
				try { }
				finally
				{
					this._lock.EnterReadLock();
					holdsReadLock = true;
				}
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

		public bool Register( Type type, object entry, Type nullableType, object nullableValue, SerializerRegistrationOptions options )
		{
#if !UNITY && DEBUG
			Contract.Assert( entry != null, "entry != null" );
#endif // !UNITY && DEBUG

			return this.RegisterCore( type, entry, nullableType, nullableValue, options );
		}

#if !NETFX_35 && !UNITY
		[SecuritySafeCritical]
#endif
#if NETFX_35 || UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "CER is OK" )]
#endif // NETFX_35 || UNITY
		private bool RegisterCore( Type key, object value, Type nullableType, object nullableValue, SerializerRegistrationOptions options )
		{
			var allowOverwrite = ( options & SerializerRegistrationOptions.AllowOverride ) != 0;

			if ( allowOverwrite || !this.ContainsType( key, nullableType ) )
			{
				bool holdsWriteLock = false;
#if !SILVERLIGHT && !NETFX_CORE
				RuntimeHelpers.PrepareConstrainedRegions();
#endif
				try
				{
#if !SILVERLIGHT && !NETFX_CORE
					RuntimeHelpers.PrepareConstrainedRegions();
#endif
					try { }
					finally
					{
						this._lock.EnterWriteLock();
						holdsWriteLock = true;
					}
					if ( allowOverwrite || !this.ContainsType( key, nullableType ) )
					{
						this._table[ key.TypeHandle ] = value;
						if ( nullableValue != null )
						{
							this._table[ nullableType.TypeHandle ] = nullableValue;
						}

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

		private bool ContainsType( Type baseType, Type nullableType )
		{
			if ( nullableType == null )
			{
				return this._table.ContainsKey( baseType.TypeHandle );
			}
			else
			{
				return
					this._table.ContainsKey( baseType.TypeHandle )
					&& this._table.ContainsKey( nullableType.TypeHandle );
			}
		}

		public bool Unregister( Type type )
		{
			return this.UnregisterCore( type );
		}

#if !NETFX_35 && !UNITY
		[SecuritySafeCritical]
#endif
#if NETFX_35 || UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Justification = "CER is OK" )]
#endif // NETFX_35 || UNITY
		private bool UnregisterCore( Type key )
		{
			if ( this._table.ContainsKey( key.TypeHandle ) )
			{
				bool holdsWriteLock = false;
#if !SILVERLIGHT && !NETFX_CORE
				RuntimeHelpers.PrepareConstrainedRegions();
#endif
				try
				{
#if !SILVERLIGHT && !NETFX_CORE
					RuntimeHelpers.PrepareConstrainedRegions();
#endif
					try { }
					finally
					{
						this._lock.EnterWriteLock();
						holdsWriteLock = true;
					}
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

#if !NETFX_35 && !UNITY
		[SecuritySafeCritical]
#endif
		internal bool Contains( Type type )
		{
			bool holdsReadLock = false;
#if !SILVERLIGHT && !NETFX_CORE
			RuntimeHelpers.PrepareConstrainedRegions();
#endif
			try
			{
#if !SILVERLIGHT && !NETFX_CORE
				RuntimeHelpers.PrepareConstrainedRegions();
#endif
				try { }
				finally
				{
					this._lock.EnterReadLock();
					holdsReadLock = true;
				}
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
