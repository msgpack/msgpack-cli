#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

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
		///		Gets a last unpacked data.
		/// </summary>
		/// <value>A last unpacked data.</value>
		/// <remarks>
		///		<note class="warning">
		///			In default implementation, this property never returning <c>null</c> even if it had not been unpacked any objects.
		///		</note>
		///		If you use any of direct APIs (methods which return non-<see cref="MessagePackObject"/>), 
		///		then this property to be invalidated.
		///		Note that the actual value of invalidated this property is undefined.
		/// </remarks>
		[Obsolete( "Consumer should not use this property. Query LastReadData instead." )]
		public abstract MessagePackObject? Data
		{
			get;
			protected set;
		}

		/// <summary>
		///		Gets a last unpacked data.
		/// </summary>
		/// <value>A last unpacked data. Initial value is <see cref="MessagePackObject.Nil"/>.</value>
		/// <remarks>
		///		If you use any of direct APIs (methods which return non-<see cref="MessagePackObject"/>), 
		///		then this property to be invalidated.
		///		Note that the actual value of invalidated this property is undefined.
		/// </remarks>
		public virtual MessagePackObject LastReadData
		{
#pragma warning disable 612,618
			get { return this.Data.GetValueOrDefault(); }
			protected set { this.Data = value; }
#pragma warning restore 612,618
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
		///		Gets a value indicating whether this instance is positioned to array or map header.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is positioned to array or map header; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsCollectionHeader
		{
			get { return this.IsArrayHeader || this.IsMapHeader; }
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
		// ReSharper disable once RedundantDefaultFieldInitializer
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
				this.ThrowInvalidModeException();
			}
		}

		/// <summary>
		///		Verifies this instance is not disposed.
		/// </summary>
		private void VerifyIsNotDisposed()
		{
			if ( this._mode == UnpackerMode.Disposed )
			{
				this.ThrowObjectDisposedException();
			}
		}

		private void ThrowObjectDisposedException()
		{
			throw new ObjectDisposedException( this.GetType().FullName );
		}

		private void ThrowInvalidModeException()
		{
			throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Reader is in '{0}' mode.", this._mode ) );
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
			return new ItemsUnpacker( stream, ownsStream );
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
		///		Or this unpacker already returned <see cref="Unpacker"/> for subtree and it has not been closed yet.
		///	</exception>
		///	<remarks>
		///		While subtree unpacker is used, this instance will be 'locked' (called 'subtree' mode) and be unavailable.
		///		When you finish to unpack subtree, you must invoke <see cref="Unpacker.Dispose()"/>, 
		///		or you faces <see cref="InvalidOperationException"/> when you use the parent instance.
		///		Subtree unpacker can only unpack subtree, so you can handle collection deserialization easily.
		///	</remarks>
		public Unpacker ReadSubtree()
		{
			if ( !this.IsCollectionHeader )
			{
				ThrowCannotBeSubtreeModeException();
			}

			if ( this._isSubtreeReading )
			{
				ThrowInSubtreeModeException();
			}

			var subtreeReader = this.ReadSubtreeCore();
			this._isSubtreeReading = !ReferenceEquals( subtreeReader, this );
			return subtreeReader;
		}

		private static void ThrowCannotBeSubtreeModeException()
		{
			throw new InvalidOperationException( "Unpacker does not locate on array nor map header." );
		}
		
		private static void ThrowInSubtreeModeException()
		{
			throw new InvalidOperationException( "Unpacker is in 'Subtree' mode." );
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
		/// <exception cref="InvalidMessagePackStreamException">
		///		The underying stream unexpectedly ended.
		/// </exception>
		public bool Read()
		{
			this.EnsureNotInSubtreeMode();

			bool result = this.ReadCore();
			if ( result && !this.IsCollectionHeader )
			{
				this.SetStable();
			}

			return result;
		}

		internal void EnsureNotInSubtreeMode()
		{
			this.VerifyMode( UnpackerMode.Streaming );
			if ( this._isSubtreeReading )
			{
				ThrowInSubtreeModeException();
			}
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
				yield return this.LastReadData;
				this.VerifyMode( UnpackerMode.Enumerating );
			}

			this.SetStable();
		}

		IEnumerator IEnumerable.GetEnumerator()
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

			if( this._mode == UnpackerMode.Enumerating )
			{
				this.ThrowInvalidModeException();
			}

			if ( this._isSubtreeReading )
			{
				ThrowInSubtreeModeException();
			}

			this._mode = UnpackerMode.Skipping;

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

		/// <summary>
		///		Gets a current item or collection as single <see cref="MessagePackObject"/> from the stream.
		/// </summary>
		/// <returns>
		///		A read item or collection from the stream.
		///		Or <c>null</c> when stream is ended.
		/// </returns>
		public MessagePackObject? ReadItem()
		{
			if ( !this.Read() )
			{
				return null;
			}

			this.UnpackSubtree();

#pragma warning disable 612,618
			return this.Data;
#pragma warning restore 612,618
		}

		/// <summary>
		///		Gets a current item or collection as single <see cref="MessagePackObject"/> from the stream.
		/// </summary>
		/// <returns>
		///		A read item or collection from the stream.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">The stream unexpectedly ends.</exception>
		public MessagePackObject ReadItemData()
		{
			if ( !this.Read() )
			{
				this.ThrowEofException();
			}

			return this.UnpackSubtreeData();
		}

		internal virtual void ThrowEofException()
		{
			throw new InvalidMessagePackStreamException( "Stream unexpectedly ends." );
		}

		/// <summary>
		///		Unpacks current subtree and returns subtree root as array or map.
		/// </summary>
		/// <returns>
		///		An unpacked array or map when current position is array or map header.
		///		<c>null</c> when current position is not array nor map header.
		/// </returns>
		public MessagePackObject? UnpackSubtree()
		{
			MessagePackObject result;
			if ( this.UnpackSubtreeDataCore( out result ) )
			{
				this.LastReadData = result;
				return result;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		///		Unpacks current subtree and returns subtree root as array or map.
		/// </summary>
		/// <returns>
		///		An unpacked array or map when current position is array or map header.
		///		Or <see cref="LastReadData"/> when current position is not array nor map header.
		/// </returns>
		public MessagePackObject UnpackSubtreeData()
		{
			MessagePackObject result;
			if ( this.UnpackSubtreeDataCore( out result ) )
			{
				this.LastReadData = result;
				return result;
			}
			else
			{
				return this.LastReadData;
			}
		}

		internal bool UnpackSubtreeDataCore( out MessagePackObject result )
		{
			if ( this.IsArrayHeader )
			{
				var array = new MessagePackObject[ checked( ( int )this.LastReadData.AsUInt32() ) ];
				using ( var subTreeReader = this.ReadSubtree() )
				{
					for ( int i = 0; i < array.Length; i++ )
					{
						array[ i ] = subTreeReader.ReadItemData();
					}
				}

				result = new MessagePackObject( array, true );
				return true;
			}
			else if ( this.IsMapHeader )
			{
				var capacity = checked( ( int )this.LastReadData.AsUInt32() );
				var map = new MessagePackObjectDictionary( capacity );
				using ( var subTreeReader = this.ReadSubtree() )
				{
					for ( int i = 0; i < capacity; i++ )
					{
						var key = subTreeReader.ReadItemData();
						var value = subTreeReader.ReadItemData();

						map.Add( key, value );
					}
				}

				result = new MessagePackObject( map, true );
				return true;
			}
			else
			{
				result = default( MessagePackObject );
				return false;
			}
		}

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
