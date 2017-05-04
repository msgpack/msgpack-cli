#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		Encapsulates reading from underlying data source.
	/// </summary>
	internal abstract partial class UnpackerReader : IDisposable
	{
		public abstract long Offset { get; }

		protected UnpackerReader() { }

		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing ) { }

		public abstract bool GetPreviousPosition( out long positionOrOffset );

		// TODO: Use Span<T>
		public abstract void Read( byte[] buffer, int size );

		public abstract string ReadString( int length );

		public abstract bool Drain( uint size );

#if FEATURE_TAP

		public abstract Task ReadAsync( byte[] buffer, int size, CancellationToken cancellationToken );

		public abstract Task<string> ReadStringAsync( int length, CancellationToken cancellationToken );

		public abstract Task<bool> DrainAsync( uint size, CancellationToken cancellationToken );

#endif // FEATURE_TAP
	}
}
