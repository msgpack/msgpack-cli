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
using System.Diagnostics;
using System.IO;

namespace MsgPack
{
	/// <summary>
	///		Implements streaming unpacking. This object is stateful.
	/// </summary>
	internal sealed class StreamUnpacker
	{
		private const int _assumedCollectionItemSize = 4;

		/// <summary>
		///		Stacked states for context collection.
		/// </summary>
		private readonly CollectionUnpackagingState _collectionState = new CollectionUnpackagingState();

		/// <summary>
		///		Stage of this state machine instance.
		/// </summary>
		private Stage _stage;

		/// <summary>
		///		Context header of unpackaging message.
		/// </summary>
		private MessagePackHeader _contextValueHeader;

		/// <summary>
		///		Buffer for unpackaging scalar or binary value.
		/// </summary>
		private BytesBuffer _bytesBuffer;

		private bool _isInTailOfCollection;

		/// <summary>
		///		Initialize new instance.
		/// </summary>
		public StreamUnpacker() { }

		public bool IsInRoot
		{
			get { return this._collectionState.IsEmpty; }
		}

		public bool HasMoreEntries
		{
			get { return this._collectionState.HasMoreEntries; }
		}

		public uint UnpackingItemsCount
		{
			get { return this._collectionState.UnpackingItemsCount; }
		}

		public bool IsInArrayHeader
		{
			get
			{
				if ( this._stage == Stage.UnpackContextCollection )
				{
					return this._collectionState.UnpackedItemsCount == 0 && ( this._collectionState.IsArray );
				}

				return false;
			}
		}

		public bool IsInMapHeader
		{
			get
			{
				if ( this._stage == Stage.UnpackContextCollection )
				{
					return this._collectionState.UnpackedItemsCount == 0 && ( this._collectionState.IsMap );
				}

				return false;
			}
		}

		/// <summary>
		///		Try unpack object from specified source.
		/// </summary>
		/// <param name="source">Input source to unpack.</param>
		/// <returns>
		/// 
		/// </returns>
		/// <remarks>
		///		<para>
		///			When this method returns null, caller can feed extra bytes to <paramref name="source"/> and invoke this again. 
		///			It could succeed because this instance preserves previous invocation state, and required bytes are supplied.
		///		</para>
		///		<para>
		///			When this method completes unpackaging single <see cref="MessagePackObject"/> tree,
		///			this method stops iterating <paramref name="source"/> (via <see cref="IEnumerator&lt;T&gt;"/>.
		///			This behavior is notified via <see cref="IDisposable.Dispose">IEnumerator&lt;T&gt;.Dispose()</see> method.
		///		</para>
		/// </remarks>
		public MessagePackObject? Unpack( Stream source, UnpackingMode unpackingMode )
		{
			// FIXME:BULK LOAD
			Contract.Assert( source != null );

			if ( unpackingMode == UnpackingMode.SubTree )
			{
				if ( this._isInTailOfCollection )
				{
					// This subtree ends.
					return null;
				}
			}

			var segmentatedSource = source as ISegmentLengthRecognizeable ?? NullSegmentLengthRecognizeable.Instance;

			while ( true )
			{
				MessagePackObject? collectionItemOrRoot = null;

				switch ( this._stage )
				{
					case Stage.UnpackCollectionLength:
					{
						if ( !this.UnpackCollectionLength( source, segmentatedSource, out collectionItemOrRoot ) )
						{
							// Need to get more data
							return null;
						}

						break;
					}
					case Stage.UnpackRawLength:
					{
						if ( !this.UnpackRaw( source, segmentatedSource, out collectionItemOrRoot ) )
						{
							// Need to get more data
							return null;
						}

						break;
					}
					case Stage.UnpackRawBytes:
					{
						if ( !this.UnpackRawBytes( source, out  collectionItemOrRoot ) )
						{
							// Need to get more data
							return null;
						}

						break;
					}
					case Stage.UnpackScalar:
					{
						if ( !this.UnpackScalar( source, out collectionItemOrRoot ) )
						{
							// Need to get more data.
							return null;
						}

						break;
					}
					default:
					{
						#region UnpackHeaderAndFixedValue

						var b = source.ReadByte();
						if ( b < 0 )
						{
							return null;
						}

						var header = _headerArray[ b ];
						this._contextValueHeader = header;
						switch ( header.Type )
						{
							case MessageType.Array16:
							case MessageType.Array32:
							case MessageType.Map16:
							case MessageType.Map32:
							{
								// Transit to UnpackCollectionLength
								this._stage = Stage.UnpackCollectionLength;
								this._bytesBuffer = new BytesBuffer( GetLength( header.Type ) );

								if ( !this.UnpackCollectionLength( source, segmentatedSource, out collectionItemOrRoot ) )
								{
									// Need to get more data
									return null;
								}

								break;
							}
							case MessageType.Raw16:
							case MessageType.Raw32:
							{
								this._stage = Stage.UnpackRawLength;
								this._bytesBuffer = new BytesBuffer( GetLength( header.Type ) );

								// Try to get length.
								if ( !this.UnpackRaw( source, segmentatedSource, out collectionItemOrRoot ) )
								{
									// Need to get more data
									return null;
								}

								break;
							}
							case MessageType.FixArray:
							case MessageType.FixMap:
							{
								if ( header.ValueOrLength == 0 )
								{
									collectionItemOrRoot = CreateEmptyCollection( header );
								}
								else
								{
									segmentatedSource.NotifySegmentLength( header.ValueOrLength * _assumedCollectionItemSize * ( ( header.Type & MessageType.IsMap ) != 0 ? 2 : 1 ) );
									this._collectionState.NewContextCollection( header, header.ValueOrLength );
									this.TransitToUnpackContextCollection();
								}

								break;
							}
							case MessageType.FixRaw:
							{
								if ( header.ValueOrLength == 0 )
								{
									collectionItemOrRoot = Binary.Empty;
								}
								else
								{
									this.TransitToUnpackRawBytes( segmentatedSource, header.ValueOrLength );
									// Try to get body.
									if ( !this.UnpackRawBytes( source, out collectionItemOrRoot ) )
									{
										return null;
									}
								}

								Contract.Assert( collectionItemOrRoot != null );
								break;
							}
							case MessageType.Nil:
							{
								collectionItemOrRoot = MessagePackObject.Nil;
								break;
							}
							case MessageType.True:
							{
								collectionItemOrRoot = new MessagePackObject( true );
								break;
							}
							case MessageType.False:
							{
								collectionItemOrRoot = new MessagePackObject( false );
								break;
							}
							case MessageType.NegativeFixNum:
							{
								collectionItemOrRoot = new MessagePackObject( unchecked( ( sbyte )b ) );
								break;
							}
							case MessageType.PositiveFixNum:
							{
								collectionItemOrRoot = new MessagePackObject( ( byte )b );
								break;
							}
							default:
							{
								// Transit to UnpackScalar
								this._stage = Stage.UnpackScalar;
								this._bytesBuffer = new BytesBuffer( GetLength( this._contextValueHeader.Type ) );

								// Try to get body.
								if ( !this.UnpackScalar( source, out collectionItemOrRoot ) )
								{
									// Need more data
									return null;
								}

								break;
							}
						} //default
						#endregion UnpackHeaderAndFixedValue

						break;
					} // default
				} // switch

				var oldCollectionItemOrRoot = collectionItemOrRoot;
				if ( collectionItemOrRoot != null )
				{
					collectionItemOrRoot = this.AddToContextCollection( collectionItemOrRoot.Value );
					this._isInTailOfCollection = !this.HasMoreEntries;

					if ( collectionItemOrRoot != null )
					{
#if DEBUG
						Contract.Assert( this._collectionState.IsEmpty );
#endif
						this._stage = Stage.Root;
#if DEBUG
						Contract.Assert( this._contextValueHeader.Type == MessageType.Unknown, this._contextValueHeader.ToString() );// null
						Contract.Assert( this._bytesBuffer.BackingStore == null, this._bytesBuffer.ToString() ); // null
#endif
						if ( unpackingMode == UnpackingMode.EntireTree )
						{
							// Entire collection
							return collectionItemOrRoot;
						}
						else
						{
							// Last item
							return oldCollectionItemOrRoot.Value;
						}
					}
				}

				if ( unpackingMode != UnpackingMode.EntireTree && this._stage == Stage.UnpackContextCollection )
				{
					if ( this._collectionState.UnpackedItemsCount == 0 )
					{
						// Count
						return this._collectionState.UnpackingItemsCount;
					}
					else
					{
						Contract.Assert( oldCollectionItemOrRoot.HasValue );
						return oldCollectionItemOrRoot.Value;
					}
				}
			}

			throw new InvalidMessagePackStreamException( "Unexpectedly end." );
		}

		private bool UnpackScalar( Stream source, out MessagePackObject? result )
		{
#if DEBUG
			Contract.Assert( ( this._contextValueHeader.Type & MessageType.IsVariable ) != 0, this._contextValueHeader.ToString() );
			Contract.Assert( ( this._contextValueHeader.Type & MessageType.IsCollection ) == 0, this._contextValueHeader.ToString() );
			Contract.Assert( this._bytesBuffer.BackingStore != null, this._bytesBuffer.ToString() );
#endif

			this._bytesBuffer = this._bytesBuffer.Feed( source );
			if ( this._bytesBuffer.IsFilled )
			{
				result = this._bytesBuffer.AsMessagePackObject( this._contextValueHeader.Type );
				return true;
			}

			// Need more data.
			result = null;
			return false;
		}

		private bool UnpackCollectionLength( Stream source, ISegmentLengthRecognizeable segmentatedSource, out MessagePackObject? unpacked )
		{
			this._bytesBuffer = this._bytesBuffer.Feed( source );

			if ( this._bytesBuffer.IsFilled )
			{
				// new collection

				var length = this._bytesBuffer.AsUInt32();
				if ( length == 0 )
				{
					// empty collection
					unpacked = CreateEmptyCollection( this._contextValueHeader );
					return true;
				}

				this._collectionState.NewContextCollection( this._contextValueHeader, length );
				segmentatedSource.NotifySegmentLength( length * _assumedCollectionItemSize * ( ( this._contextValueHeader.Type & MessageType.IsMap ) != 0 ? 2 : 1 ) );
				this.TransitToUnpackContextCollection();

				unpacked = null;
				return true;
			}

			// Try next iteration.
			unpacked = null;
			return false;
		}

		private bool UnpackRaw( Stream source, ISegmentLengthRecognizeable segmentatedSource, out MessagePackObject? unpacked )
		{
			this._bytesBuffer = this._bytesBuffer.Feed( source );

			if ( this._bytesBuffer.IsFilled )
			{
				var length = this._bytesBuffer.AsUInt32();
				if ( length == 0 )
				{
					// empty collection
					unpacked = CreateEmptyCollection( this._contextValueHeader );
					return true;
				}

				segmentatedSource.NotifySegmentLength( length );
				this.TransitToUnpackRawBytes( segmentatedSource, length );

				return this.UnpackRawBytes( source, out unpacked );
			}

			// Need more info.
			unpacked = null;
			return false;
		}

		private bool UnpackRawBytes( Stream source, out MessagePackObject? unpacked )
		{
#if DEBUG
			Contract.Assert( this._bytesBuffer.BackingStore != null, this._bytesBuffer.ToString() );
#endif

			this._bytesBuffer = this._bytesBuffer.Feed( source );
			if ( this._bytesBuffer.IsFilled )
			{
				unpacked = this._bytesBuffer.AsMessagePackObject( this._contextValueHeader.Type );
				return true;
			}

			// Need more info.
			unpacked = null;
			return false;
		}

		/// <summary>
		///		Transit current stage to <see cref="Stage.UnpackRawBytes"/> with cleanuping states.
		/// </summary>
		/// <param name="source"><see cref="ISegmentLengthRecognizeable"/> to be notified.</param>
		/// <param name="length">The known length of the source.</param>
		private void TransitToUnpackRawBytes( ISegmentLengthRecognizeable source, uint length )
		{
			this._stage = Stage.UnpackRawBytes;
			// Allocate buffer to store raw binaries.
			source.NotifySegmentLength( length );
			this._bytesBuffer = new BytesBuffer( length );
		}

		/// <summary>
		///		Transit current stage to <see cref="Stage.UnpackContextCollection"/> with cleanuping states.
		/// </summary>
		private void TransitToUnpackContextCollection()
		{
			this._stage = Stage.UnpackContextCollection;
			this._contextValueHeader = MessagePackHeader.Null;
			this._bytesBuffer = BytesBuffer.Null;
		}

		/// <summary>
		///		Create <see cref="MessagePackObject"/> which wraps appropriate empty collection.
		/// </summary>
		/// <param name="header">Header which has type information.</param>
		/// <returns><see cref="MessagePackObject"/> which wraps appropriate empty collection.</returns>
		private static MessagePackObject CreateEmptyCollection( MessagePackHeader header )
		{
			Contract.Assert( header.ValueOrLength == 0, header.ToString() );

			if ( ( header.Type & MessageType.IsArray ) != 0 )
			{
				return new MessagePackObject( new List<MessagePackObject>( 0 ) );
			}
			else
			{
				return new MessagePackObject( new MessagePackObjectDictionary( 0 ) );
			}
		}

		private static readonly MessagePackHeader[] _headerArray = InitializeHeaderArray();

		private static MessagePackHeader[] InitializeHeaderArray()
		{
			MessagePackHeader[] result = new MessagePackHeader[ 0x100 ];

			for ( int i = 0; i < 0x80; i++ )
			{
				result[ i ] = new MessagePackHeader( MessageType.PositiveFixNum, i );
			}
			for ( int i = 0x80; i < 0x90; i++ )
			{
				result[ i ] = new MessagePackHeader( MessageType.FixMap, i & 0x0f );
			}
			for ( int i = 0x90; i < 0xa0; i++ )
			{
				result[ i ] = new MessagePackHeader( MessageType.FixArray, i & 0x0f );
			}
			for ( int i = 0xa0; i < 0xc0; i++ )
			{
				result[ i ] = new MessagePackHeader( MessageType.FixRaw, i & 0x1f );
			}
			result[ 0xc0 ] = MessageType.Nil;
			// 0xc1 : Undefined
			result[ 0xc2 ] = MessageType.False;
			result[ 0xc3 ] = MessageType.True;
			// 0xc4-0xc9 : Undefined
			result[ 0xca ] = MessageType.Single;
			result[ 0xcb ] = MessageType.Double;
			result[ 0xcc ] = MessageType.UInt8;
			result[ 0xcd ] = MessageType.UInt16;
			result[ 0xce ] = MessageType.UInt32;
			result[ 0xcf ] = MessageType.UInt64;
			result[ 0xd0 ] = MessageType.Int8;
			result[ 0xd1 ] = MessageType.Int16;
			result[ 0xd2 ] = MessageType.Int32;
			result[ 0xd3 ] = MessageType.Int64;
			// 0xd4-0xd9 : Undefined
			result[ 0xda ] = MessageType.Raw16;
			result[ 0xdb ] = MessageType.Raw32;
			result[ 0xdc ] = MessageType.Array16;
			result[ 0xdd ] = MessageType.Array32;
			result[ 0xde ] = MessageType.Map16;
			result[ 0xdf ] = MessageType.Map32;
			for ( int i = 0xe0; i < 0x100; i++ )
			{
				result[ i ] = new MessagePackHeader( MessageType.NegativeFixNum, i & 0x1f );
			}

			return result;
		}

		/// <summary>
		///		Add unpacked item to context collection.
		///		If context collection is fulfilled, then return it.
		/// </summary>
		/// <param name="item">Item to be added to context collection.</param>
		/// <returns>
		///		If context collection is fulfilled, then return it.
		///		Otherwise null.
		/// </returns>
		private MessagePackObject? AddToContextCollection( MessagePackObject item )
		{
			MessagePackObject current = item;
			while ( !this._collectionState.IsEmpty )
			{
				if ( !this._collectionState.FeedItem( current ) )
				{
					this.TransitToUnpackContextCollection();
					return null;
				}

				current = this._collectionState.PopCollection();
			}

			this.TransitToUnpackContextCollection();
			return current;
		}

		/// <summary>
		///		Get variable portion of header for specified type.
		/// </summary>
		/// <param name="type">Type of message which retrieved from header.</param>
		/// <returns>Size of variable type length. If type is collection or raw, this value indicates size of length portion.</returns>
		private static uint GetLength( MessageType type )
		{
			switch ( type )
			{
				case MessageType.Int8:
				case MessageType.UInt8:
				{
					return sizeof( byte );
				}
				case MessageType.Array16:
				case MessageType.Int16:
				case MessageType.Map16:
				case MessageType.Raw16:
				case MessageType.UInt16:
				{
					return sizeof( ushort );
				}
				case MessageType.Array32:
				case MessageType.Int32:
				case MessageType.Map32:
				case MessageType.Raw32:
				case MessageType.Single:
				case MessageType.UInt32:
				{
					return sizeof( uint );
				}
				case MessageType.Double:
				case MessageType.Int64:
				case MessageType.UInt64:
				{
					return sizeof( ulong );
				}
				default:
				{
					Contract.Assert( false, "FAIL" );
					return 0;
				}
			}
		}

		/// <summary>
		///		Represents state machine stage (state) of <see cref="StreamUnpacker"/>.
		/// </summary>
		private enum Stage
		{
			/// <summary>
			///		State machine stays root unpacking.
			///		<see cref="StreamUnpacker"/> does not have any intermediate state.
			///		This is initial state.
			/// </summary>
			Root = 0,

			/// <summary>
			///		State machine is unpacking some collection.
			///		<see cref="StreamUnpacker"/> will unpack next item of context collection.
			/// </summary>
			UnpackContextCollection,

			/// <summary>
			///		State machine is unpacking length of array or map.
			///		<see cref="StreamUnpacker"/> will unpack scalar as length of collection, 
			///		then add new context collection to the stack and unpack items.
			/// </summary>
			UnpackCollectionLength,

			/// <summary>
			///		State machine is unpacking length of raw binaries.
			///		<see cref="StreamUnpacker"/> will unpack scalar as length of binaries, 
			///		then get following bytes as value.
			/// </summary>
			UnpackRawLength,

			/// <summary>
			///		State machine is getting bytes as raw binaries.
			/// </summary>
			UnpackRawBytes,

			/// <summary>
			///		State machine is unpacking body of scalar value.
			/// </summary>
			UnpackScalar
		}

		/// <summary>
		///		Represents type of message.
		/// </summary>
		private enum MessageType : ushort
		{
			/// <summary>
			///		Type is not known yet.
			/// </summary>
			Unknown = 0,
			Nil = 10,
			PositiveFixNum = 20,
			NegativeFixNum = 21,
			UInt8 = IsVariable | 30,
			UInt16 = IsVariable | 31,
			UInt32 = IsVariable | 32,
			UInt64 = IsVariable | 33,
			Int8 = IsVariable | 40,
			Int16 = IsVariable | 41,
			Int32 = IsVariable | 42,
			Int64 = IsVariable | 43,
			FixRaw = IsRawBinary | IsCollection | 50,
			Raw16 = IsVariable | IsRawBinary | IsCollection | 51,
			Raw32 = IsVariable | IsRawBinary | IsCollection | 52,
			Single = IsVariable | 60,
			Double = IsVariable | 61,
			False = 70,
			True = 71,
			FixArray = IsArray | IsCollection | 80,
			Array16 = IsVariable | IsArray | IsCollection | 81,
			Array32 = IsVariable | IsArray | IsCollection | 82,
			FixMap = IsMap | IsCollection | 90,
			Map16 = IsVariable | IsMap | IsCollection | 91,
			Map32 = IsVariable | IsMap | IsCollection | 92,
			// TODO: string
			// TODO: Fixed-Typed Array

			/// <summary>
			///		Flag indicates type is variable, so length unpacking is required.
			/// </summary>
			IsVariable = 0x400,

			/// <summary>
			///		Flag indicates type is collection, so context collection management for nesting is required.
			/// </summary>
			IsCollection = 0x800,

			/// <summary>
			///		Flag indicates type is a type of array.
			/// </summary>
			IsArray = 0x1000,

			/// <summary>
			///		Flag indicates type is a type of map.
			/// </summary>
			IsMap = 0x2000,

			/// <summary>
			///		Flag indicates type is a type of raw binary.
			/// </summary>
			IsRawBinary = 0x4000,
		}

		/// <summary>
		///		Lightweight structured header representation.
		///		Note that this is VALUE type.
		/// </summary>
		private struct MessagePackHeader
		{
			/// <summary>
			///		Null value.
			/// </summary>
			public static readonly MessagePackHeader Null = new MessagePackHeader();

			private readonly MessageType _type;

			/// <summary>
			///		Get type of message.
			/// </summary>
			/// <value>Type of message.</value>
			public MessageType Type
			{
				get { return this._type; }
			}

			private readonly uint _valueOrLength;

			/// <summary>
			///		Get value of fixed scalar value, length of fixed collections,
			///		length of fixed raw binary, or length of variable length.
			/// </summary>
			public uint ValueOrLength
			{
				get { return this._valueOrLength; }
			}

			public MessagePackHeader( MessageType type, int valueOrLength )
				: this( type, ToUInt32( valueOrLength ) ) { }

			private static uint ToUInt32( int valueOrLength )
			{
				Contract.Assert( valueOrLength >= 0 );
				return unchecked( ( uint )valueOrLength );
			}

			public MessagePackHeader( MessageType type, uint valueOrLength )
			{
				this._type = type;
				this._valueOrLength = valueOrLength;
			}

			public override string ToString()
			{
				return this._type + ":" + this._valueOrLength;
			}

			public static implicit operator MessagePackHeader( MessageType type )
			{
				return new MessagePackHeader( type, 0 );
			}
		}

		/// <summary>
		///		Represents a set of states for unpackaging context collection.
		/// </summary>
		private sealed class CollectionUnpackagingState
		{
			/// <summary>
			///		Stack of collection context.
			/// </summary>
			private readonly Stack<CollectionContextState> _collectionContextStack = new Stack<CollectionContextState>();

			/// <summary>
			///		Get the value indicates whether internal context stack is empty.
			/// </summary>
			/// <value>
			///		If internal context stack is empty then true.
			/// </value>
			/// <remarks>
			///		If this property returns true when you complete unpackaging context collection,
			///		it indicates that entire object tree has been unpackaged.
			/// </remarks>
			public bool IsEmpty
			{
				get { return this._collectionContextStack.Count == 0; }
			}

			/// <summary>
			///		Gets the unpacking items count.
			/// </summary>
			/// <value>
			///		The unpacking items count.
			/// </value>
			public uint UnpackingItemsCount
			{
				get { return this._collectionContextStack.Peek().Capacity; }
			}

			/// <summary>
			///		Gets the unpacked items count.
			/// </summary>
			/// <value>
			///		The unpacked items count.
			/// </value>
			public uint UnpackedItemsCount
			{
				get { return this._collectionContextStack.Peek().Unpacked; }
			}

			public bool IsArray
			{
				get { return this._collectionContextStack.Peek().Items != null; }
			}

			public bool IsMap
			{
				get { return this._collectionContextStack.Peek().Dictionary != null; }
			}

			public bool HasMoreEntries
			{
				get
				{
					if ( this._collectionContextStack.Count == 0 )
					{
						return false;
					}
					else
					{
						return !this._collectionContextStack.Peek().IsFilled;
					}
				}
			}

			/// <summary>
			///		Initialize new instance.
			/// </summary>
			public CollectionUnpackagingState() { }

			/// <summary>
			///		Push new context collection state to internal stack.
			/// </summary>
			/// <param name="header">Header of collection object.</param>
			/// <param name="count">Items count of collection object. If collection is map, this value indicates count of entries.</param>
			public void NewContextCollection( MessagePackHeader header, uint count )
			{
#if DEBUG
				Contract.Assert( ( header.Type & MessageType.IsRawBinary ) == 0, header.Type.ToString() );
#endif

				this._collectionContextStack.Push( new CollectionContextState( header, count ) );
			}

			/// <summary>
			///		Pop context collection state from internal stack, 
			///		and return <see cref="MessagePackObject"/> which represents popped context collection.
			/// </summary>
			/// <returns></returns>
			public MessagePackObject PopCollection()
			{
#if DEBUG
				Contract.Assert( !this.IsEmpty );
#endif
				var context = this._collectionContextStack.Pop();
				if ( ( context.Header.Type & MessageType.IsArray ) != 0 )
				{
#if DEBUG
					Contract.Assert( context.Items != null );
					Contract.Assert( context.Unpacked == context.Capacity, context.Unpacked + "!=" + context.Capacity );
#endif
					return new MessagePackObject( context.Items );
				}
				else if ( ( context.Header.Type & MessageType.IsMap ) != 0 )
				{
#if DEBUG
					Contract.Assert( context.Dictionary != null );
					Contract.Assert( context.Unpacked == context.Capacity * 2, context.Unpacked + "!=" + ( context.Capacity * 2 ) );
#endif
					return new MessagePackObject( context.Dictionary );
				}
				else
				{
					throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Unknown collection type: {0}(0x{0:x})", context.Header.Type ) );
				}
			}

			/// <summary>
			///		Feed new collection item to context collection state.
			/// </summary>
			/// <param name="item">New item to feed.</param>
			public bool FeedItem( MessagePackObject item )
			{
				var top = this._collectionContextStack.Pop();
				var feeded = top.AddUnpackedItem( item );
				this._collectionContextStack.Push( feeded );
				return feeded.IsFilled;
			}

			/// <summary>
			///		Represents context collection state.
			/// </summary>
			public struct CollectionContextState
			{
				private readonly MessagePackHeader _header;

				/// <summary>
				///		Get header of this collection.
				/// </summary>
				/// <value>Header of this collection.</value>
				public MessagePackHeader Header
				{
					get { return this._header; }
				}

				private readonly MessagePackObjectDictionary _dictionary;

				public MessagePackObjectDictionary Dictionary
				{
					get { return this._dictionary; }
				}

				private MessagePackObject? _key;

				private readonly MessagePackObject[] _items;

				/// <summary>
				///		Get storage for items of this collection.
				/// </summary>
				/// <value>
				///		Storage for items of this collection.
				///		Do not modify this array directly.
				/// </value>
				public MessagePackObject[] Items
				{
					get { return this._items; }
				}

				private readonly uint _capacity;

				private uint _unpacked;

				public uint Capacity
				{
					get { return this._capacity; }
				}

				public uint Unpacked
				{
					get { return this._unpacked; }
				}

				/// <summary>
				///		Get the value which indicates <see cref="Items"/> are filled.
				/// </summary>
				/// <value>If <see cref="Items"/> are filled then true.</value>
				public bool IsFilled
				{
					get { return this._unpacked == this._capacity * ( this._items != null ? 1 : 2 ); }
				}

				/// <summary>
				///		Initialize new instance.
				/// </summary>
				/// <param name="header">Header of collection.</param>
				/// <param name="count">Recognized count of items in this collection.</param>
				public CollectionContextState( MessagePackHeader header, uint count )
				{
					Contract.Assert( header.Type != MessageType.Unknown );
					this = default( CollectionContextState );

					this._header = header;
					if ( ( header.Type & MessageType.IsMap ) == 0 )
					{
						Contract.Assert( ( header.Type & MessageType.IsArray ) != 0 );
						this._items = new MessagePackObject[ count ];
					}
					else
					{
						Contract.Assert( ( header.Type & MessageType.IsArray ) == 0 );
						if ( Int32.MaxValue < count )
						{
							throw new NotImplementedException( "Maps over 2^31 items are not supported yet." );
						}

						this._dictionary = new MessagePackObjectDictionary( unchecked( ( int )( count & 0x7fffffff ) ) );
					}

					this._capacity = count;
					this._unpacked = 0;
				}

				/// <summary>
				///		Returns string representation of this object.
				/// </summary>
				/// <returns>
				///		String which format is "<em>Header</em>(<em>Unpacked</em>/<em>Length</em>)".
				/// </returns>
				public override string ToString()
				{
					if ( this._items != null )
					{
						return String.Format( CultureInfo.CurrentCulture, "{0}({1}/{2})", this._header, this._unpacked, this._capacity );
					}
					else
					{
						return String.Format( CultureInfo.CurrentCulture, "{0}({1}/{2})", this._header, this._unpacked / 2.0, this._capacity );
					}
				}

				/// <summary>
				///		Add unpackaged item to this collection.
				/// </summary>
				/// <param name="item">Item to be added.</param>
				/// <returns>New context state to replace this instance.</returns>
				public CollectionContextState AddUnpackedItem( MessagePackObject item )
				{
#if DEBUG
					Contract.Assert(
						( ( this._items != null && this._unpacked < this._capacity )
						|| ( this._dictionary != null && this._unpacked < this._capacity * 2 )
						),
						this._items != null
						? this._unpacked + "<" + this._capacity
						: this._unpacked + "<" + this._capacity * 2
					);
#endif

					if ( this._items != null )
					{
						this._items[ this._unpacked ] = item;
					}
					else
					{
						if ( this._key == null )
						{
							this._key = item;
						}
						else
						{
							try
							{
								this._dictionary.Add( this._key.Value, item );
							}
							catch ( ArgumentException )
							{
								throw new InvalidMessagePackStreamException( String.Format( CultureInfo.CurrentCulture, "Key '{0}' is duplicated.", this._key ) );
							}

							this._key = null;
						}
					}

					this._unpacked++;
					return this;
				}
			}
		}

		/// <summary>
		///		Represents buffer as value type.
		/// </summary>
		private struct BytesBuffer
		{
			/// <summary>
			///		Represents null buffer.
			/// </summary>
			public static readonly BytesBuffer Null = new BytesBuffer();

			private readonly byte[] _backingStore;

#if DEBUG
			internal byte[] BackingStore
			{
				get { return this._backingStore; }
			}
#endif

			private readonly int _position;

			/// <summary>
			///		Get the value which indicates this buffer is filled.
			/// </summary>
			/// <value>If  this buffer is filled then true.</value>
			public bool IsFilled
			{
				get
				{
					return this._backingStore == null ? false : this._position == this._backingStore.Length;
				}
			}

			/// <summary>
			///		Initialize new instance.
			/// </summary>
			/// <param name="length">Length of bytes.</param>
			public BytesBuffer( uint length )
			{
				this._backingStore = new byte[ length ];
				this._position = 0;
			}

			/// <summary>
			///		Initialize new instance.
			/// </summary>
			/// <param name="backingStore">Existent backing store.</param>
			/// <param name="newPosition">Position where this buffer is filled.</param>
			private BytesBuffer( byte[] backingStore, int newPosition )
			{
				this._backingStore = backingStore;
				this._position = newPosition;
			}

			/// <summary>
			///		Returns string representation of this object.
			/// </summary>
			/// <returns>
			///		String which format is "byte[<em>Length</em>]@<em>Position</em>".
			/// </returns>
			public override string ToString()
			{
				if ( this._backingStore == null )
				{
					return "(null)";
				}
				else
				{
					return String.Format( CultureInfo.InvariantCulture, "byte[{0}]@{1}", this._backingStore.Length, this._position );
				}
			}

			/// <summary>
			///		Feed specified <see cref="Stream"/> to this buffer, and increment position.
			/// </summary>
			/// <param name="stream"><see cref="Stream"/> to be feeded.</param>
			/// <returns>New buffer to replace this object.</returns>
			public BytesBuffer Feed( Stream stream )
			{
				int reading = this._backingStore.Length - this._position;
				return new BytesBuffer( this._backingStore, this._position + stream.Read( this._backingStore, this._position, reading ) );
			}

			/// <summary>
			///		Get internal value as <see cref="UInt32"/>.
			/// </summary>
			/// <returns><see cref="UInt32"/> value of this buffer.</returns>
			public uint AsUInt32()
			{
				Contract.Assert( this.IsFilled, "Not filled yet:" + this );

				switch ( this._backingStore.Length )
				{
					case 1:
					{
						return this._backingStore[ 0 ];
					}
					case 2:
					{
						return BigEndianBinary.ToUInt16( this._backingStore, 0 );
					}
					default:
					{
						Contract.Assert( this._backingStore.Length == sizeof( uint ), this._backingStore.Length.ToString() );
						return BigEndianBinary.ToUInt32( this._backingStore, 0 );
					}
				}
			}

			/// <summary>
			///		Get internal buffer as specified <see cref="MessagePackObject"/> numeric primitive.
			/// </summary>
			/// <param name="type">Type of value to be deserialized.</param>
			/// <returns><see cref="MessagePackObject"/> which wraps deserialized numeric primitive.</returns>
			public MessagePackObject AsMessagePackObject( MessageType type )
			{
#if DEBUG
				Contract.Assert( this.IsFilled, "Not filled yet:" + this );
#endif

				return AsMessagePackObject( this._backingStore, type );
			}

			public static MessagePackObject AsMessagePackObject( byte[] buffer, MessageType type )
			{
				switch ( type )
				{
					case MessageType.Double:
					{
						return new MessagePackObject( BigEndianBinary.ToDouble( buffer, 0 ) );
					}
					case MessageType.Int16:
					{
						return new MessagePackObject( BigEndianBinary.ToInt16( buffer, 0 ) );
					}
					case MessageType.Int32:
					{
						return new MessagePackObject( BigEndianBinary.ToInt32( buffer, 0 ) );
					}
					case MessageType.Int64:
					{
						return new MessagePackObject( BigEndianBinary.ToInt64( buffer, 0 ) );
					}
					case MessageType.Int8:
					{
						return new MessagePackObject( BigEndianBinary.ToSByte( buffer, 0 ) );
					}
					case MessageType.Single:
					{
						return new MessagePackObject( BigEndianBinary.ToSingle( buffer, 0 ) );
					}
					case MessageType.UInt16:
					{
						return new MessagePackObject( BigEndianBinary.ToUInt16( buffer, 0 ) );
					}
					case MessageType.UInt32:
					{
						return new MessagePackObject( BigEndianBinary.ToUInt32( buffer, 0 ) );
					}
					case MessageType.UInt64:
					{
						return new MessagePackObject( BigEndianBinary.ToUInt64( buffer, 0 ) );
					}
					case MessageType.UInt8:
					{
						return new MessagePackObject( BigEndianBinary.ToByte( buffer, 0 ) );
					}
					default:
					{
						return new MessagePackObject( buffer );
					}
				}
			}
		}

		/// <summary>
		///		Null object for <see cref="ISegmentLengthRecognizeable"/>.
		/// </summary>
		private sealed class NullSegmentLengthRecognizeable : ISegmentLengthRecognizeable
		{
			public static readonly NullSegmentLengthRecognizeable Instance = new NullSegmentLengthRecognizeable();

			private NullSegmentLengthRecognizeable()
			{
			}

			public void NotifySegmentLength( long lengthFromCurrent )
			{
				// nop
			}
		}

	}
}
