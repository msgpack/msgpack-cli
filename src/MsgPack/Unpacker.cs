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
using System.Globalization;
using System.IO;
using System.Text;

namespace MsgPack
{
	/// <summary>
	///		Implements deserializing feature of MsgPack.
	/// </summary>
	/// <include file='remarks.xml' path='/doc/remarks[@name="MsgPack.Unpacker"]'/>
	/// <seealso cref="Unpacking"/>
	public abstract partial class Unpacker : IEnumerable<MessagePackObject>, IDisposable
	{
		#region -- Properties --

		/// <summary>
		///		Get last unpacked data.
		/// </summary>
		/// <value>Last unpacked data or null.</value>
		/// <remarks>
		///		If you use any of directory APIs (methods which return non-<see cref="MessagePackObject"/>), 
		///		then this property to be invalidated.
		/// </remarks>
		public abstract MessagePackObject? Data
		{
			get;
		}

		/// <summary>
		///		Gets a value indicating whether this instance is positioned to array header.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is positioned to array header; otherwise, <c>false</c>.
		/// </value>
		public abstract bool IsArrayHeader
		{
			get;
		}

		/// <summary>
		///		Gets a value indicating whether this instance is positioned to map header.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is positioned to map header; otherwise, <c>false</c>.
		/// </value>
		public abstract bool IsMapHeader
		{
			get;
		}

		/// <summary>
		///		Gets the items count for current array or map.
		/// </summary>
		/// <value>
		///		The items count for current array or map.
		/// </value>
		/// <exception cref="InvalidOperationException">
		///		Both of the <see cref="IsArrayHeader"/> and <see cref="IsMapHeader"/> are <c>false</c>.
		/// </exception>
		public abstract long ItemsCount
		{
			get;
		}

		private UnpackerMode _mode = UnpackerMode.Unknown;
		private bool _isSubtreeReading = false;

		/// <summary>
		///		Verifies the mode.
		/// </summary>
		/// <param name="mode">The mode to be.</param>
		/// <exception cref="ObjectDisposedException">
		///		Already disposed.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		///		Is in incompatible mode.
		/// </exception>
		private void VerifyMode( UnpackerMode mode )
		{
			this.VerifyIsNotDisposed();

			if ( this._mode == UnpackerMode.Unknown )
			{
				this._mode = mode;
				return;
			}

			if ( this._mode != mode )
			{
				throw this.NewInvalidModeException();
			}
		}

		/// <summary>
		///		Verifies this instance is not disposed.
		/// </summary>
		private void VerifyIsNotDisposed()
		{
			if ( this._mode == UnpackerMode.Disposed )
			{
				throw new ObjectDisposedException( this.GetType().FullName );
			}
		}

		/// <summary>
		///		Returns new exception instance to notify invalid mode transition.
		/// </summary>
		/// <returns>New exception instance to notify invalid mode transition.</returns>
		private Exception NewInvalidModeException()
		{
			return new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Reader is in '{0}' mode.", this._mode ) );
		}

		/// <summary>
		///		Gets the underlying stream to handle direct API.
		/// </summary>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		protected virtual Stream UnderlyingStream
		{
			get { throw new NotSupportedException(); }
		}

#if DEBUG
		internal virtual long? UnderlyingStreamPosition
		{
			get { return null; }
		}
#endif

		#endregion -- Properties --

		#region -- Factories --

		/// <summary>
		///		 Creates the new <see cref="Unpacker"/> with internal buffer which has default size.
		/// </summary>
		/// <returns><see cref="Unpacker"/> instance.</returns>
		public static Unpacker Create()
		{
			return new StreamUnpacker();
		}

		/// <summary>
		///		 Creates the new <see cref="Unpacker"/> from specified stream.
		/// </summary>
		/// <param name="stream">The stream to be unpacked. This stream will be closed when <see cref="Packer.Dispose(Boolean)"/> is called.</param>
		/// <returns><see cref="Unpacker"/> instance.</returns>
		public static Unpacker Create( Stream stream )
		{
			return Create( stream, true );
		}

		/// <summary>
		///		 Creates the new <see cref="Unpacker"/> from specified stream.
		/// </summary>
		/// <param name="stream">The stream to be unpacked.</param>
		/// <param name="ownsStream">
		///		<c>true</c> to close <paramref name="stream"/> when this instance is disposed;
		///		<c>false</c>, otherwise.
		/// </param>
		/// <returns><see cref="Unpacker"/> instance.</returns>
		public static Unpacker Create( Stream stream, bool ownsStream )
		{
			return new StreamUnpacker( stream, ownsStream );
		}

		#endregion -- Factories --

		#region -- Ctor / Dispose --

		/// <summary>
		///		Initializes a new instance of the <see cref="Unpacker"/> class.
		/// </summary>
		protected Unpacker() { }

		/// <summary>
		///		Releases all managed resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		///		Releases unmanaged and optionally managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose( bool disposing )
		{
			// nop
		}

		#endregion -- Ctor / Dispose --

		#region -- Streaming API --

		//		<note>
		//			<strong>For Implementers</strong>
		//			This method must be returns <see cref="SubtreeUnpacker"/>.
		//		</note>
		/// <summary>
		///		Starts unpacking of current subtree.
		/// </summary>
		/// <returns>
		///		<see cref="Unpacker"/> to unpack current subtree.
		///		This will not be <c>null</c>.
		///	</returns>
		///	<exception cref="InvalidOperationException">
		///		This unpacker is not positioned on the header of array nor map.
		///	</exception>
		///	<remarks>
		///		While subtree unpacker is used, this instance will be 'locked' (called 'subtree' mode) and be unavailable.
		///		When you finish to unpack subtree, you must invoke <see cref="Unpacker.Dispose()"/>, 
		///		or you faces <see cref="InvalidOperationException"/> when you use the parent instance.
		///		Subtree unpacker can only unpack subtree, so you can handle collection deserialization easily.
		///	</remarks>
		public Unpacker ReadSubtree()
		{
			if ( !this.IsArrayHeader && !this.IsMapHeader )
			{
				throw new InvalidOperationException( "Unpacker does not locate on array nor map header." );
			}

			var subtreeReader = this.ReadSubtreeCore();
			this._isSubtreeReading = !Object.ReferenceEquals( subtreeReader, this );
			return subtreeReader;
		}

		/// <summary>
		///		Starts unpacking of current subtree.
		/// </summary>
		/// <returns>
		///		<see cref="Unpacker"/> to unpack current subtree.
		///		This will not be <c>null</c>.
		///	</returns>
		protected abstract Unpacker ReadSubtreeCore();

		/// <summary>
		///		Ends the read subtree.
		/// </summary>
		/// <remarks>
		///		This method only be called from subtree unpacker.
		///		Custom subtree unpacker implementation must call this method from its <see cref="Dispose(bool)"/> method.
		/// </remarks>
		protected internal virtual void EndReadSubtree()
		{
			this._isSubtreeReading = false;
			this.SetStable();
		}

		/// <summary>
		///		Reads next Message Pack entry.
		/// </summary>
		/// <returns>
		///		<c>true</c>, if position is sucessfully move to next entry;
		///		<c>false</c>, if position reaches the tail of the Message Pack stream.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		This instance is in 'subtree' mode.
		/// </exception>
		public bool Read()
		{
			this.VerifyMode( UnpackerMode.Streaming );
			if ( this._isSubtreeReading )
			{
				throw new InvalidOperationException( "Unpacker is in 'Subtree' mode." );
			}

			bool result = this.ReadCore();
			if ( result && !this.IsArrayHeader && !this.IsMapHeader )
			{
				this.SetStable();
			}

			return result;
		}

		private void SetStable()
		{
			// Now, this instance can transit another mode.
			this._mode = UnpackerMode.Unknown;
		}

		/// <summary>
		///		Reads next Message Pack entry.
		/// </summary>
		/// <returns>
		///		<c>true</c>, if position is sucessfully move to next entry;
		///		<c>false</c>, if position reaches the tail of the Message Pack stream.
		/// </returns>
		protected abstract bool ReadCore();

		/// <summary>
		///		Gets <see cref="IEnumerator&lt;T&gt;"/> to enumerate <see cref="MessagePackObject"/> from source stream.
		/// </summary>
		/// <returns><see cref="IEnumerator&lt;T&gt;"/> to enumerate <see cref="MessagePackObject"/> from source stream.</returns>
		public IEnumerator<MessagePackObject> GetEnumerator()
		{
			this.VerifyMode( UnpackerMode.Enumerating );
			while ( this.ReadCore() )
			{
				if ( this.Data != null )
				{
					yield return this.Data.Value;
				}

				this.VerifyMode( UnpackerMode.Enumerating );
			}

			this.SetStable();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		///		Skips the subtree where the root is the current entry, and returns skipped byte length.
		/// </summary>
		/// <returns>
		///		Skipped byte length.
		///		If the subtree is not completed, then <c>null</c>.
		/// </returns>
		public long? Skip()
		{
			this.VerifyIsNotDisposed();

			switch ( this._mode )
			{
				case UnpackerMode.Enumerating:
				{
					throw this.NewInvalidModeException();
				}
				case UnpackerMode.Streaming:
				{
					if ( !this.Data.HasValue )
					{
						throw this.NewInvalidModeException();
					}

					// If the value exists, safe to transit skipping.
					break;
				}
			}

			this._mode = UnpackerMode.Skipping;

			if ( this._isSubtreeReading )
			{
				throw new InvalidOperationException( "Unpacker is in 'Subtree' mode." );
			}

			var result = this.SkipCore();
			if ( result != null )
			{
				this.SetStable();
			}

			return result;
		}

		/// <summary>
		///		Skips the subtree where the root is the current entry, and returns skipped byte length.
		/// </summary>
		/// <returns>
		///		Skipped byte length.
		///		If the subtree is not completed, then <c>null</c>.
		/// </returns>
		protected abstract long? SkipCore();

		#endregion -- Streaming API --

		#region -- Feeding API --

		// TODO: Feeding API might be useless...

		/// <summary>
		///		Feeds new data source.
		/// </summary>
		/// <param name="newData">New data source to feed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="newData"/> is null.</exception>
		internal void Feed( IEnumerable<byte> newData )
		{
			if ( newData == null )
			{
				throw new ArgumentNullException( "newData" );
			}

			Contract.EndContractBlock();

			this.FeedCore( new EnumerableStream( newData ), true );
		}

		/// <summary>
		///		Feeds new data source.
		/// </summary>
		/// <param name="stream">New data source to feed.</param>
		/// <param name="ownsStream">If <paramref name="stream"/> should be disposed in this instance then true.</param>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
		internal void Feed( Stream stream, bool ownsStream )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException( "stream" );
			}

			Contract.EndContractBlock();

			this.FeedCore( stream, ownsStream );
		}

		/// <summary>
		///		Feeds new data source.
		/// </summary>
		/// <param name="stream">New data source to feed. This will not be <c>null</c>.</param>
		/// <param name="ownsStream">If <paramref name="stream"/> should be disposed in this instance then true.</param>
		protected virtual void FeedCore( Stream stream, bool ownsStream )
		{
			throw new NotSupportedException();
		}

		#endregion -- Feeding API --

		private enum UnpackerMode
		{
			Unknown = 0,
			Skipping,
			Streaming,
			Enumerating,
			Disposed
		}
	}
}
