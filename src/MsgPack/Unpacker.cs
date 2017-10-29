#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
using System.Collections;
using System.Collections.Generic;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Globalization;
using System.IO;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

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
#pragma warning disable 612, 618
			get { return this.Data.GetValueOrDefault(); }
			protected set { this.Data = value; }
#pragma warning restore 612, 618
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
		internal void VerifyMode( UnpackerMode mode )
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
		internal void VerifyIsNotDisposed()
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

		internal void ThrowInvalidModeException()
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

		/// <summary>
		///		Gets the previous position before last operation for debugging.
		/// </summary>
		/// <param name="offsetOrPosition">The offset or position.</param>
		/// <returns><c>true</c> for the <paramref name="offsetOrPosition"/> is real position of the underlying stream;<c>false</c> if the value is offset from the root unpaker instance was created.</returns>
		internal virtual bool GetPreviousPosition( out long offsetOrPosition )
		{
			offsetOrPosition = -1;
			return false;
		}

		#endregion -- Properties --

		#region -- Factories --

		/// <summary>
		///		 Creates the new <see cref="Unpacker"/> from specified stream.
		/// </summary>
		/// <param name="stream">The stream to be unpacked. This stream will be closed when <see cref="Packer.Dispose(Boolean)"/> is called.</param>
		/// <returns><see cref="Unpacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is <c>null</c>.</exception>
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
		/// <returns><see cref="Unpacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is <c>null</c>.</exception>
		public static Unpacker Create( Stream stream, bool ownsStream )
		{
			return Create( stream, ownsStream ? PackerUnpackerStreamOptions.SingletonOwnsStream : null, null );
		}

		/// <summary>
		///		 Creates the new <see cref="Unpacker"/> from specified stream.
		/// </summary>
		/// <param name="stream">The stream to be unpacked.</param>
		/// <param name="streamOptions"><see cref="PackerUnpackerStreamOptions"/> which specifies stream handling options.</param>
		/// <returns><see cref="Unpacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is <c>null</c>.</exception>
		public static Unpacker Create( Stream stream, PackerUnpackerStreamOptions streamOptions )
		{
			return Create( stream, streamOptions, null );
		}

		/// <summary>
		///		 Creates the new <see cref="Unpacker"/> from specified stream.
		/// </summary>
		/// <param name="stream">The stream to be unpacked.</param>
		/// <param name="streamOptions"><see cref="PackerUnpackerStreamOptions"/> which specifies stream handling options.</param>
		/// <param name="unpackerOptions"><see cref="UnpackerOptions"/> which specifies various options. Specify <c>null</c> to use default options.</param>
		/// <returns><see cref="Unpacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is <c>null</c>.</exception>
		public static Unpacker Create( Stream stream, PackerUnpackerStreamOptions streamOptions, UnpackerOptions unpackerOptions )
		{
			if ( unpackerOptions == null || unpackerOptions.ValidationLevel == UnpackerValidationLevel.Collection )
			{
				return new CollectionValidatingStreamUnpacker( stream, streamOptions );
			}
			else
			{
				return new FastStreamUnpacker( stream, streamOptions );
			}
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayUnpacker"/> from specified byte array.
		/// </summary>
		/// <param name="source">The source byte array.</param>
		/// <returns><see cref="ByteArrayUnpacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
		public static ByteArrayUnpacker Create( byte[] source )
		{
			return Create( source, null );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayUnpacker"/> from specified byte array.
		/// </summary>
		/// <param name="source">The source byte array.</param>
		/// <param name="startOffset">The effective start offset of the <paramref name="source"/>.</param>
		/// <returns><see cref="ByteArrayUnpacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startOffset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="source"/> is too small.</exception>
		public static ByteArrayUnpacker Create( byte[] source, int startOffset )
		{
			return Create( source, startOffset, null );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayUnpacker"/> from specified byte array.
		/// </summary>
		/// <param name="source">The source byte array.</param>
		/// <param name="unpackerOptions"><see cref="UnpackerOptions"/> which specifies various options. Specify <c>null</c> to use default options.</param>
		/// <returns><see cref="ByteArrayUnpacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
		public static ByteArrayUnpacker Create( byte[] source, UnpackerOptions unpackerOptions )
		{
			return Create( source, 0, unpackerOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayUnpacker"/> from specified byte array.
		/// </summary>
		/// <param name="source">The source byte array.</param>
		/// <param name="startOffset">The effective start offset of the <paramref name="source"/>.</param>
		/// <param name="unpackerOptions"><see cref="UnpackerOptions"/> which specifies various options. Specify <c>null</c> to use default options.</param>
		/// <returns><see cref="ByteArrayUnpacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startOffset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="source"/> is too small.</exception>
		public static ByteArrayUnpacker Create( byte[] source, int startOffset, UnpackerOptions unpackerOptions )
		{
			if ( unpackerOptions == null || unpackerOptions.ValidationLevel == UnpackerValidationLevel.Collection )
			{
				return new CollectionValidatingByteArrayUnpacker( source, startOffset );
			}
			else
			{
				return new FastByteArrayUnpacker( source, startOffset );
			}
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

		#region -- Draining --

		/// <summary>
		///		Drains remaining items in current context.
		/// </summary>
		/// <remarks>
		///		This method drains remaining items in context of subtree mode unpacker.
		///		This method does not any effect for other types of unpacker.
		/// </remarks>
		public virtual void Drain()
		{
			// nop
		}

#if FEATURE_TAP

		/// <summary>
		///		Drains remaining items in current context.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains 
		///		an unpacked array or map when current position is array or map header.
		///		Or <see cref="LastReadData"/> when current position is not array nor map header.
		/// </returns>
		/// <remarks>
		///		This method drains remaining items in context of subtree mode unpacker.
		///		This method does not any effect for other types of unpacker.
		/// </remarks>
		public Task DrainAsync()
		{
			return this.DrainAsync( CancellationToken.None );
		}

		/// <summary>
		///		Drains remaining items in current context.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains 
		///		an unpacked array or map when current position is array or map header.
		///		Or <see cref="LastReadData"/> when current position is not array nor map header.
		/// </returns>
		/// <remarks>
		///		This method drains remaining items in context of subtree mode unpacker.
		///		This method does not any effect for other types of unpacker.
		/// </remarks>
		public virtual Task DrainAsync( CancellationToken cancellationToken )
		{
			return Task.FromResult( default( object ) );
		}

#endif // FEATURE_TAP

		#endregion -- Draining --

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
			return this.InternalReadSubtree();
		}

		internal virtual Unpacker InternalReadSubtree()
		{
			return this.ReadSubtreeCore();
		}

		internal static void ThrowCannotBeSubtreeModeException()
		{
			throw new InvalidOperationException( "Unpacker does not locate on array nor map header." );
		}

		internal static void ThrowInSubtreeModeException()
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
			return this.ReadInternal();
		}

		internal bool ReadInternal()
		{
			bool result = this.ReadCore();
			if ( result && !this.IsCollectionHeader )
			{
				this.SetStable();
			}

			return result;
		}

		internal virtual void EnsureNotInSubtreeMode() { }

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

#if FEATURE_TAP

		/// <summary>
		///		Reads next Message Pack entry asynchronously.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains whether
		///		the position is sucessfully move to next entry or not(which means this object reached the tail of the Message Pack stream).
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		This instance is in 'subtree' mode.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		The underying stream unexpectedly ended.
		/// </exception>
		public Task<bool> ReadAsync()
		{
			return this.ReadAsync( CancellationToken.None );
		}

		/// <summary>
		///		Reads next Message Pack entry asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains whether
		///		the position is sucessfully move to next entry or not(which means this object reached the tail of the Message Pack stream).
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		This instance is in 'subtree' mode.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		The underying stream unexpectedly ended.
		/// </exception>
		public Task<bool> ReadAsync( CancellationToken cancellationToken )
		{
			this.EnsureNotInSubtreeMode();
			return this.ReadInternalAsync( cancellationToken );
		}

		internal async Task<bool> ReadInternalAsync( CancellationToken cancellationToken )
		{
			bool result = await this.ReadAsyncCore( cancellationToken ).ConfigureAwait( false );
			if ( result && !this.IsCollectionHeader )
			{
				this.SetStable();
			}

			return result;
		}

		/// <summary>
		///		Reads next Message Pack entry asynchronously.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains whether
		///		the position is sucessfully move to next entry or not(which means this object reached the tail of the Message Pack stream).
		/// </returns>
		protected Task<bool> ReadAsyncCore()
		{
			return this.ReadAsyncCore( CancellationToken.None );
		}

		/// <summary>
		///		Reads next Message Pack entry asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains whether
		///		the position is sucessfully move to next entry or not(which means this object reached the tail of the Message Pack stream).
		/// </returns>
		protected virtual Task<bool> ReadAsyncCore( CancellationToken cancellationToken )
		{
			return Task.Run( () => this.ReadCore(), cancellationToken );
		}

#endif // FEATURE_TAP

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
			this.BeginSkip();

			var result = this.SkipCore();
			this.EndSkip( result );

			return result;
		}

		private void BeginSkip()
		{
			this.VerifyIsNotDisposed();

			if ( this._mode == UnpackerMode.Enumerating )
			{
				this.ThrowInvalidModeException();
			}

			this._mode = UnpackerMode.Skipping;
		}

		internal virtual void BeginSkipCore() { }

		private void EndSkip( long? result )
		{
			if ( result != null )
			{
				this.SetStable();
			}
		}

		/// <summary>
		///		Skips the subtree where the root is the current entry, and returns skipped byte length.
		/// </summary>
		/// <returns>
		///		Skipped byte length.
		///		If the subtree is not completed, then <c>null</c>.
		/// </returns>
		protected abstract long? SkipCore();

#if FEATURE_TAP

		/// <summary>
		///		Skips the subtree where the root is the current entry, and returns skipped byte length asynchronously.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains 
		///		a kipped byte length.
		///		Or, if the subtree is not completed, then <c>null</c>.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		public Task<long?> SkipAsync()
		{
			return this.SkipAsync( CancellationToken.None );
		}

		/// <summary>
		///		Skips the subtree where the root is the current entry, and returns skipped byte length asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains 
		///		a kipped byte length.
		///		Or, if the subtree is not completed, then <c>null</c>.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		public async Task<long?> SkipAsync( CancellationToken cancellationToken )
		{
			this.BeginSkip();

			var result = await this.SkipAsyncCore( cancellationToken ).ConfigureAwait( false );
			this.EndSkip( result );

			return result;
		}

		/// <summary>
		///		Skips the subtree where the root is the current entry, and returns skipped byte length asynchronously.
		/// </summary>
		/// <returns>
		///		Skipped byte length.
		///		If the subtree is not completed, then <c>null</c>.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		protected Task<long?> SkipAsyncCore()
		{
			return this.SkipAsyncCore( CancellationToken.None );
		}

		/// <summary>
		///		Skips the subtree where the root is the current entry, and returns skipped byte length asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		Skipped byte length.
		///		If the subtree is not completed, then <c>null</c>.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		protected virtual Task<long?> SkipAsyncCore( CancellationToken cancellationToken )
		{
			return Task.Run( () => this.SkipCore(), cancellationToken );
		}

#endif // FEATURE_TAP

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

#pragma warning disable 612, 618
			return this.Data;
#pragma warning restore 612, 618
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

#if FEATURE_TAP

		/// <summary>
		///		Gets a current item or collection as single <see cref="MessagePackObject"/> from the stream asynchronously.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains 
		///		a read item or collection from the stream.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">The stream unexpectedly ends.</exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		public Task<MessagePackObject?> ReadItemAsync()
		{
			return this.ReadItemAsync( CancellationToken.None );
		}

		/// <summary>
		///		Gets a current item or collection as single <see cref="MessagePackObject"/> from the stream asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains 
		///		a read item or collection from the stream.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">The stream unexpectedly ends.</exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		public async Task<MessagePackObject?> ReadItemAsync( CancellationToken cancellationToken )
		{
			if ( !( await this.ReadAsync( cancellationToken ).ConfigureAwait( false ) ) )
			{
				return null;
			}

			await this.UnpackSubtreeAsync( cancellationToken ).ConfigureAwait( false );

#pragma warning disable 612, 618
			return this.Data;
#pragma warning restore 612, 618
		}

		/// <summary>
		///		Gets a current item or collection as single <see cref="MessagePackObject"/> from the stream asynchronously.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains 
		///		a read item or collection from the stream.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">The stream unexpectedly ends.</exception>
		public Task<MessagePackObject> ReadItemDataAsync()
		{
			return this.ReadItemDataAsync( CancellationToken.None );
		}

		/// <summary>
		///		Gets a current item or collection as single <see cref="MessagePackObject"/> from the stream asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains 
		///		a read item or collection from the stream.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">The stream unexpectedly ends.</exception>
		public async Task<MessagePackObject> ReadItemDataAsync( CancellationToken cancellationToken )
		{
			if ( !( await this.ReadAsync( cancellationToken ).ConfigureAwait( false ) ) )
			{
				this.ThrowEofException();
			}

			return await this.UnpackSubtreeDataAsync( cancellationToken ).ConfigureAwait( false );
		}

#endif // FEATURE_TAP

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

#if FEATURE_TAP

		/// <summary>
		///		Unpacks current subtree and returns subtree root as array or map asynchronously.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains 
		///		an unpacked array or map when current position is array or map header.
		///		Or <c>null</c> when current position is not array nor map header.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		public Task<MessagePackObject?> UnpackSubtreeAsync()
		{
			return this.UnpackSubtreeAsync( CancellationToken.None );
		}

		/// <summary>
		///		Unpacks current subtree and returns subtree root as array or map asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains 
		///		an unpacked array or map when current position is array or map header.
		///		Or <c>null</c> when current position is not array nor map header.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Collections/Delegates/Nullables/Task<T> essentially must be nested generic." )]
		public async Task<MessagePackObject?> UnpackSubtreeAsync( CancellationToken cancellationToken )
		{
			var result = await this.UnpackSubtreeDataAsyncCore( cancellationToken ).ConfigureAwait( false );
			if ( result.Success )
			{
				this.LastReadData = result.Value;
				return result.Value;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		///		Unpacks current subtree and returns subtree root as array or map asynchronously.
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains 
		///		an unpacked array or map when current position is array or map header.
		///		Or <see cref="LastReadData"/> when current position is not array nor map header.
		/// </returns>
		public Task<MessagePackObject> UnpackSubtreeDataAsync()
		{
			return this.UnpackSubtreeDataAsync( CancellationToken.None );
		}

		/// <summary>
		///		Unpacks current subtree and returns subtree root as array or map asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains 
		///		an unpacked array or map when current position is array or map header.
		///		Or <see cref="LastReadData"/> when current position is not array nor map header.
		/// </returns>
		public async Task<MessagePackObject> UnpackSubtreeDataAsync( CancellationToken cancellationToken )
		{
			var result = await this.UnpackSubtreeDataAsyncCore( cancellationToken ).ConfigureAwait( false );
			if ( result.Success )
			{
				this.LastReadData = result.Value;
				return result.Value;
			}
			else
			{
				return this.LastReadData;
			}
		}

		internal async Task<AsyncReadResult<MessagePackObject>> UnpackSubtreeDataAsyncCore( CancellationToken cancellationToken )
		{
			if ( this.IsArrayHeader )
			{
				var array = new MessagePackObject[ checked( ( int )this.LastReadData.AsUInt32() ) ];
				using ( var subTreeReader = this.ReadSubtree() )
				{
					for ( int i = 0; i < array.Length; i++ )
					{
						array[ i ] = await subTreeReader.ReadItemDataAsync( cancellationToken ).ConfigureAwait( false );
					}
				}

				return AsyncReadResult.Success( new MessagePackObject( array, true ) );
			}
			else if ( this.IsMapHeader )
			{
				var capacity = checked( ( int )this.LastReadData.AsUInt32() );
				var map = new MessagePackObjectDictionary( capacity );
				using ( var subTreeReader = this.ReadSubtree() )
				{
					for ( int i = 0; i < capacity; i++ )
					{
						var key = await subTreeReader.ReadItemDataAsync( cancellationToken ).ConfigureAwait( false );
						var value = await subTreeReader.ReadItemDataAsync( cancellationToken ).ConfigureAwait( false );

						map.Add( key, value );
					}
				}

				return AsyncReadResult.Success( new MessagePackObject( map, true ) );
			}
			else
			{
				return AsyncReadResult.Fail<MessagePackObject>();
			}
		}

#endif // FEATURE_TAP

		internal enum UnpackerMode
		{
			Unknown = 0,
			Skipping,
			Streaming,
			Enumerating,
			Disposed
		}
	}
}
