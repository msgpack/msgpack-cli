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
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	// TODO: MessagePackEncoder/Decoder <|- ...NativeEncoder/Decoder, ...JsonEncoder/Decoder
	/// <summary>
	///		Defines base contract for object serialization.
	/// </summary>
	/// <typeparam name="T">Target type.</typeparam>
	/// <remarks>
	///		<para>
	///			This class implements strongly typed serialization and deserialization.
	///		</para>
	///		<para>
	///			When the underlying stream does not contain strongly typed or contains dynamically typed objects,
	///			you should use <see cref="Unpacker"/> directly and take advantage of <see cref="MessagePackObject"/>.
	///		</para>
	/// </remarks>
	/// <seealso cref="Unpacker"/>
	/// <seealso cref="Unpacking"/>
	public abstract class MessagePackSerializer<T> : IMessagePackSerializer
	{
		private static readonly bool _isNullable = JudgeNullable();
		private static readonly string _memoryStreamExceptionSourceName = typeof( MemoryStream ).Assembly.GetName().Name;

		private static bool JudgeNullable()
		{
			if ( !typeof( T ).GetIsValueType() )
			{
				// reference type.
				return true;
			}

			if ( typeof( T ) == typeof( MessagePackObject ) )
			{
				// can be MPO.Nil.
				return true;
			}

			if ( typeof( T ).GetIsGenericType() && typeof( T ).GetGenericTypeDefinition() == typeof( Nullable<> ) )
			{
				// Nullable<T>
				return true;
			}

			return false;
		}

		/// <summary>
		///		Serialize specified object to the <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">Destination <see cref="Stream"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="SerializationException">
		///		<typeparamref name="T"/> is not serializable etc.
		/// </exception>
		public void Pack( Stream stream, T objectTree )
		{
			this.PackTo( Packer.Create( stream ), objectTree );
		}

		/// <summary>
		///		Deserialize object from the <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">Source <see cref="Stream"/>.</param>
		/// <returns>Deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="SerializationException">
		///		<typeparamref name="T"/> is not serializable etc.
		/// </exception>
		public T Unpack( Stream stream )
		{
			var unpacker = Unpacker.Create( stream );
			unpacker.Read();
			return this.UnpackFrom( unpacker );
		}

		/// <summary>
		///		Serialize specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="packer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="SerializationException">
		///		<typeparamref name="T"/> is not serializable etc.
		/// </exception>
		public void PackTo( Packer packer, T objectTree )
		{
			// TODO: Hot-Path-Optimization
			if ( packer == null )
			{
				throw new ArgumentNullException( "packer" );
			}

			if ( objectTree == null )
			{
				packer.PackNull();
				return;
			}

			this.PackToCore( packer, objectTree );
		}

		/// <summary>
		///		Serialize specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="SerializationException">
		///		<typeparamref name="T"/> is not serializable etc.
		/// </exception>
		protected internal abstract void PackToCore( Packer packer, T objectTree );

		/// <summary>
		///		Deserialize object with specified <see cref="Unpacker"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree.</param>
		/// <returns>Deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="SerializationException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is abstract type.
		/// </exception>
		public T UnpackFrom( Unpacker unpacker )
		{
			// TODO: Hot-Path-Optimization
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( !unpacker.Data.HasValue )
			{
				throw SerializationExceptions.NewEmptyOrUnstartedUnpacker();
			}

			if ( unpacker.Data.GetValueOrDefault().IsNil )
			{
				if ( _isNullable )
				{
					// null
					return default( T );
				}
				else
				{
					throw SerializationExceptions.NewValueTypeCannotBeNull( typeof( T ) );
				}
			}

			return this.UnpackFromCore( unpacker );
		}

		/// <summary>
		///		Deserialize object with specified <see cref="Unpacker"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <returns>Deserialized object.</returns>
		/// <exception cref="SerializationException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is abstract type.
		/// </exception>
		protected internal abstract T UnpackFromCore( Unpacker unpacker );

		/// <summary>
		///		Deserialize collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree.</param>
		/// <param name="collection">Collection that the items to be stored.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or <paramref name="collection"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="SerializationException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not collection.
		/// </exception>
		public void UnpackTo( Unpacker unpacker, T collection )
		{
			// TODO: Hot-Path-Optimization
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( !unpacker.Data.HasValue )
			{
				throw SerializationExceptions.NewEmptyOrUnstartedUnpacker();
			}

			if ( unpacker.Data.Value.IsNil )
			{
				return;
			}

			this.UnpackToCore( unpacker, collection );
		}

		/// <summary>
		///		Deserialize collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <param name="collection">Collection that the items to be stored. This value will not be <c>null</c>.</param>
		/// <exception cref="SerializationException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not collection.
		/// </exception>
		protected internal virtual void UnpackToCore( Unpacker unpacker, T collection )
		{
			throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "This operation is not supported by '{0}'.", this.GetType() ) );
		}

		/// <summary>
		///		Serialize specified object to the array of <see cref="Byte"/>.
		/// </summary>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>An array of <see cref="Byte"/> which stores serialized value.</returns>
		/// <exception cref="SerializationException">
		///		<typeparamref name="T"/> is not serializable etc.
		/// </exception>
		public byte[] Pack( T objectTree )
		{
			using ( var buffer = new MemoryStream() )
			{
				this.Pack( buffer, objectTree );
				return buffer.ToArray();
			}
		}

		/// <summary>
		///		Serialize specified object to the specified buffer.
		/// </summary>
		/// <param name="buffer">An array of <see cref="Byte"/> serialized value to be stored.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>A bytes of serialized binary.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="SerializationException">
		///		<paramref name="buffer"/> is too small.
		///		<typeparamref name="T"/> is not serializable etc.
		/// </exception>
		/// <remarks>
		///		This overload is equivalant to <see cref="Pack(Byte[], int, T)"/> with offset is 0.
		/// </remarks>
		public int Pack( byte[] buffer, T objectTree )
		{
			return this.Pack( buffer, 0, objectTree );
		}

		/// <summary>
		///		Serialize specified object to the specified buffer.
		/// </summary>
		/// <param name="buffer">An array of <see cref="Byte"/> serialized value to be stored.</param>
		/// <param name="offset">An offset of the <paramref name="buffer"/> to be used.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>A bytes of serialized binary.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative.
		/// </exception>
		/// <exception cref="SerializationException">
		///		<paramref name="buffer"/> is too small to the specified <paramref name="offset"/> and <paramref name="objectTree"/>.
		///		<typeparamref name="T"/> is not serializable etc.
		/// </exception>
		public int Pack( byte[] buffer, int offset, T objectTree )
		{
			if ( buffer == null )
			{
				throw new ArgumentNullException( "buffer" );
			}

			return this.Pack( new ArraySegment<byte>( buffer, offset, buffer.Length - offset ), objectTree ).Offset - offset;
		}

		/// <summary>
		///		Serialize specified object to the specified buffer.
		/// </summary>
		/// <param name="buffer">An <see cref="ArraySegment{T}"/> of <see cref="Byte"/> serialized value to be stored.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>A new <see cref="ArraySegment{T}"/> which points next position of used region.</returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="buffer"/> does not have non-null array.
		/// </exception>
		/// <exception cref="SerializationException">
		///		<paramref name="buffer"/> is too small.
		///		<typeparamref name="T"/> is not serializable etc.
		/// </exception>
		/// <remarks>
		///		The returning value reflects used bytes length, 
		///		it means its <see cref="ArraySegment{T}.Offset"/> will be incremented by used bytes length,
		///		and its <see cref="ArraySegment{T}.Count"/> will be decremented by used bytes length,
		/// </remarks>
		public ArraySegment<byte> Pack( ArraySegment<byte> buffer, T objectTree )
		{
			if ( buffer.Array == null )
			{
				throw new ArgumentException( "buffer does not contain valid array.", "buffer" );
			}

			using ( var stream = new MemoryStream( buffer.Array, buffer.Offset, buffer.Count, true ) )
			{
				long initialPosition = stream.Position;
				try
				{
					this.Pack( stream, objectTree );
				}
				catch ( NotSupportedException ex )
				{
					if ( ex.Source == _memoryStreamExceptionSourceName )
					{
						throw new SerializationException( "Buffer is to small.", ex );
					}
					else
					{
						throw;
					}
				}

				int used = unchecked( ( int )( stream.Position - initialPosition ) );
				return new ArraySegment<byte>( buffer.Array, buffer.Offset + used, buffer.Count - used );
			}
		}

		/// <summary>
		///		Deserialize object from the array of <see cref="Byte"/>.
		/// </summary>
		/// <param name="buffer">An array of <see cref="Byte"/> serialized value to be stored.</param>
		/// <param name="offset">
		///		An offset of the <paramref name="buffer"/> to be used. 
		///		This value will be updated to reflect used bytes length.
		/// </param>
		/// <returns>A bytes of serialized binary.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative.
		/// </exception>
		/// <exception cref="SerializationException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		public T Unpack( byte[] buffer, ref int offset )
		{
			if ( buffer == null )
			{
				throw new ArgumentNullException( "buffer" );
			}

			var arraySegment = new ArraySegment<byte>( buffer, offset, buffer.Length - offset );
			var result = this.Unpack( ref arraySegment );
			offset = arraySegment.Offset;
			return result;
		}

		/// <summary>
		///		Serialize specified object to the specified buffer.
		/// </summary>
		/// <param name="buffer">
		///		An <see cref="ArraySegment{T}"/> of <see cref="Byte"/> serialized value to be stored.
		///		This value will be updated to reflect used bytes length.
		/// </param>
		/// <returns>A new <see cref="ArraySegment{T}"/> which points next position of used region.</returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="buffer"/> does not have non-null array.
		/// </exception>
		/// <exception cref="SerializationException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <remarks>
		///		The returning value reflects used bytes length, 
		///		it means its <see cref="ArraySegment{T}.Offset"/> will be incremented by used bytes length,
		///		and its <see cref="ArraySegment{T}.Count"/> will be decremented by used bytes length,
		/// </remarks>
		public T Unpack( ref ArraySegment<byte> buffer )
		{
			if( buffer.Array == null )
			{
				throw new ArgumentException( "buffer does not contain valid array.", "buffer" );
			}

			using ( var stream = new ByteArraySegmentStream( buffer ) )
			{
				long initialPosition = stream.Position;
				var result = this.Unpack( stream );
				int used = unchecked( ( int )( stream.Position - initialPosition ) );
				buffer = new ArraySegment<byte>( buffer.Array, buffer.Offset + used, buffer.Count - used );
				return result;
			}
		}

		void IMessagePackSerializer.PackTo( Packer packer, object objectTree )
		{
			// TODO: Hot-Path-Optimization
			if ( packer == null )
			{
				throw new ArgumentNullException( "packer" );
			}

			if ( objectTree == null )
			{
				if ( typeof( T ).GetIsValueType() )
				{
					if ( !( typeof( T ).GetIsGenericType() && typeof( T ).GetGenericTypeDefinition() == typeof( Nullable<> ) ) )
					{
						throw SerializationExceptions.NewValueTypeCannotBeNull( typeof( T ) );
					}
				}

				packer.PackNull();
				return;
			}
			else
			{
				if ( !( objectTree is T ) )
				{
					throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not compatible for '{1}'.", objectTree.GetType(), typeof( T ) ), "objectTree" );
				}
			}

			this.PackToCore( packer, ( T )objectTree );
		}

		object IMessagePackSerializer.UnpackFrom( Unpacker unpacker )
		{
			return this.UnpackFrom( unpacker );
		}

		void IMessagePackSerializer.UnpackTo( Unpacker unpacker, object collection )
		{
			// TODO: Hot-Path-Optimization
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				throw new ArgumentNullException( "collection" );
			}

			if ( !( collection is T ) )
			{
				throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not compatible for '{1}'.", collection.GetType(), typeof( T ) ), "collection" );
			}

			if ( !unpacker.Data.HasValue )
			{
				throw SerializationExceptions.NewEmptyOrUnstartedUnpacker();
			}

			this.UnpackToCore( unpacker, ( T )collection );
		}
	}
}
