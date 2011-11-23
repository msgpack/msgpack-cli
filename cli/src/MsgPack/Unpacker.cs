#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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

		/// <summary>
		///	 Gets a value indicating whether this instance is in start position.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is in start; otherwise, <c>false</c>.
		/// </value>
		public abstract bool IsInStart
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
			if ( this._mode == UnpackerMode.Disposed )
			{
				throw new ObjectDisposedException( this.GetType().FullName );
			}

			if ( this._mode == UnpackerMode.Unknown )
			{
				this._mode = mode;
				return;
			}

			if ( this._mode != mode )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Reader is in '{0}' mode.", this._mode ) );
			}
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
		/// <param name="stream">The stream to be unpacked.</param>
		/// <returns><see cref="Unpacker"/> instance.</returns>
		public static Unpacker Create( Stream stream )
		{
			return new StreamUnpacker( stream );
		}

		#endregion -- Factories --

		#region -- Ctor / Dispose --

		/// <summary>
		///		Initializes a new instance of the <see cref="Unpacker"/> class.
		/// </summary>
		protected Unpacker() { }

		/// <summary>
		///		Releases all managed resources
		/// </summary>
		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		///		Releases unmanaged and optionally managed resources
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

			var subTreeReader = this.ReadSubtreeCore();
			this._isSubtreeReading = !Object.ReferenceEquals( subTreeReader, this );
			return subTreeReader;
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
		protected internal void EndReadSubtree()
		{
			this._isSubtreeReading = false;
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

			return this.ReadCore();
		}

		/// <summary>
		///		Reads next Message Pack entry.
		/// </summary>
		/// <returns>
		///		<c>true</c>, if position is sucessfully move to next entry;
		///		<c>false</c>, if position reaches the tail of the Message Pack stream.
		/// </returns>
		protected abstract bool ReadCore();

		// FIXME: Quota

		/// <summary>
		///		Gets <see cref="IEnumerator&lt;T&gt;"/> to enumerate <see cref="MessagePackObject"/> from source stream.
		/// </summary>
		/// <returns><see cref="IEnumerator&lt;T&gt;"/> to enumerate <see cref="MessagePackObject"/> from source stream.</returns>
		public IEnumerator<MessagePackObject> GetEnumerator()
		{
			this.VerifyMode( UnpackerMode.Streaming );
			while ( this.Read() )
			{
				if ( this.Data != null )
				{
					yield return this.Data.Value;
				}
				this.VerifyMode( UnpackerMode.Streaming );
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion -- Streaming API --

		#region -- Feeding API --

		/// <summary>
		///		Feeds new data source.
		/// </summary>
		/// <param name="newData">New data source to feed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="newData"/> is null.</exception>
		public void Feed( IEnumerable<byte> newData )
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
		public void Feed( Stream stream, bool ownsStream )
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
		protected abstract void FeedCore( Stream stream, bool ownsStream );

		#endregion -- Feeding API --

		#region -- Direct API --

		/// <summary>
		///		Unpack length of array from current buffer.
		/// </summary>
		/// <returns>Length of array. This is up to <see cref="UInt32.MaxValue"/>.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Byte"/>.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public long UnpackArrayLength()
		{
			this.VerifyMode( UnpackerMode.Direct );

			return Unpacking.UnpackArrayLength( this.UnderlyingStream );
		}

		/// <summary>
		///		Unpack count of list from current buffer.
		/// </summary>
		/// <returns>Count of list. This is up to <see cref="UInt32.MaxValue"/>.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Byte"/>.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public long UnpackListCount()
		{
			return this.UnpackArrayLength();
		}

		/// <summary>
		///		Unpack count of map pairs from current buffer.
		/// </summary>
		/// <returns>Count of map pairs. This is up to <see cref="UInt32.MaxValue"/>.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Byte"/>.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public long UnpackMapCount()
		{
			this.VerifyMode( UnpackerMode.Direct );

			return Unpacking.UnpackDictionaryCount( this.UnderlyingStream );
		}

		/// <summary>
		///		Unpack count of map pairs from current buffer.
		/// </summary>
		/// <returns>Count of map pairs. This is up to <see cref="UInt32.MaxValue"/>.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Byte"/>.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public long UnpackDictionaryCount()
		{
			return this.UnpackMapCount();
		}

		/// <summary>
		///		Unpack length of raw binary from current buffer.
		/// </summary>
		/// <returns>Length of raw binary. This is up to <see cref="UInt32.MaxValue"/>.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Byte"/>.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public long UnpackRawLength()
		{
			this.VerifyMode( UnpackerMode.Direct );

			return Unpacking.UnpackRawLength( this.UnderlyingStream );
		}

		/// <summary>
		///		Unpack null from current buffer.
		/// </summary>
		/// <returns>null.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Byte"/>.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public Object UnpackNull()
		{
			this.VerifyMode( UnpackerMode.Direct );

			return Unpacking.UnpackNull( this.UnderlyingStream );
		}

		/// <summary>
		///		Unpack null from current buffer.
		/// </summary>
		/// <returns>If value is null then true.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Byte"/>.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public Boolean TryUnpackNull()
		{
			this.VerifyMode( UnpackerMode.Direct );

			return Unpacking.TryUnpackNull( this.UnderlyingStream );
		}

		/// <summary>
		///		Unpack <see cref="Boolean"/> from current buffer.
		/// </summary>
		/// <returns><see cref="Boolean"/> value.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Byte"/>.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public Boolean UnpackBoolean()
		{
			this.VerifyMode( UnpackerMode.Direct );

			return Unpacking.UnpackBoolean( this.UnderlyingStream );
		}

		/// <summary>
		///		Unpack raw byte stream.
		/// </summary>
		/// <returns>Raw byte stream.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Byte"/>.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public IEnumerable<byte> UnpackRaw()
		{
			this.VerifyMode( UnpackerMode.Direct );

			return Unpacking.UnpackRaw( this.UnderlyingStream );
		}

		/// <summary>
		///		Unpack raw byte stream as string using UTF-8 <see cref="Encoding"/>.
		/// </summary>
		/// <returns>String.</returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Byte"/>.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public String UnpackString()
		{
			this.VerifyMode( UnpackerMode.Direct );

			return Unpacking.UnpackString( this.UnderlyingStream );
		}

		/// <summary>
		///		Unpack raw byte stream as string using specified <see cref="Encoding"/>.
		/// </summary>
		/// <param name="encoding"><see cref="Encoding"/> to be used.</param>
		/// <returns>String.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="encoding"/> is null.</exception>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="MessageTypeException">Current value is not <see cref="Byte"/>.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public String UnpackString( Encoding encoding )
		{
			if ( encoding == null )
			{
				throw new ArgumentNullException( "encoding" );
			}

			this.VerifyMode( UnpackerMode.Direct );

			return Unpacking.UnpackString( this.UnderlyingStream, encoding );
		}

		/// <summary>
		///		Unpack <see cref="MessagePackObject"/> from current stream.
		/// </summary>
		/// <returns>
		///		<see cref="MessagePackObject"/>.
		///		If current stream does not contain enough bytes, so this value may be null.
		/// </returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public MessagePackObject? TryUnpackObject()
		{
			this.VerifyMode( UnpackerMode.Direct );

			return Unpacking.UnpackObject( this.UnderlyingStream );
		}

		/// <summary>
		///		Unpack <see cref="MessagePackObject"/> from current stream.
		/// </summary>
		/// <returns>
		///		<see cref="MessagePackObject"/>.
		///		If current stream does not contain enough bytes, so this value may be null.
		/// </returns>
		/// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
		/// <exception cref="UnpackException">Failed to unpack due to malformed or collapsed source.</exception>
		/// <exception cref="InvalidOperationException">
		///		There is no data on the current stream.
		///		Or this instance is in 'streaming' or 'subtree' mode.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		This instance does not supoort direct API.
		/// </exception>
		public MessagePackObject UnpackObject()
		{
			return this.TryUnpackObject().Value;
		}

		#endregion -- Direct API --

		private enum UnpackerMode
		{
			Unknown = 0,
			Direct,
			Streaming,
			Disposed,
			Subtree
		}
	}
}
