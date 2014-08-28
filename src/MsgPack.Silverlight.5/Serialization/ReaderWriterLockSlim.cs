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

#if SILVERLIGHT
using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace MsgPack.Serialization
{
	// TODO: Port RWLS from Mono?
	// OK, this is NOT reader-writer lock at all. But it is enought to in browser/phone usage ... maybe.
	/// <summary>
	///		System.Threading.ReaderWriterLockSlim alternative.
	/// </summary>
	internal sealed class ReaderWriterLockSlim : IDisposable
	{
		private readonly object _syncRoot = new object();
		private int _lockCount = 0;
		private int _disposed = 0;

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "recursionPolocy", Justification = "For API compatibility" )]
		public ReaderWriterLockSlim( LockRecursionPolicy recursionPolocy ) { }

		~ReaderWriterLockSlim()
		{
			this.Dispose( false );
		}
		
		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "disposing", Justification = "For standard" )]
		private void Dispose( bool disposing )
		{
			if ( Interlocked.Exchange( ref this._disposed, 1 ) == 1 )
			{
				return;
			}

			while ( 0 < this._lockCount )
			{
				this.Release();
			}
		}

		public void EnterReadLock()
		{
			this.Take();
		}

		public void EnterWriteLock()
		{
			this.Take();
		}

		private void Take()
		{
			bool lockTaken = false;
			try
			{
				Monitor.Enter( this._syncRoot, ref lockTaken );
			}
			finally
			{
				if ( lockTaken )
				{
					Interlocked.Increment( ref this._lockCount );
				}
			}
		}

		public void ExitReadLock()
		{
			this.Release();
		}

		public void ExitWriteLock()
		{
			this.Release();
		}

		private void Release()
		{
			try { }
			finally
			{
				if ( 0 <= Interlocked.Decrement( ref this._lockCount ) )
				{
					Monitor.Exit( this._syncRoot );
				}
				else
				{
					Interlocked.Increment( ref this._lockCount );
				}
			}
		}
	}
}
#endif