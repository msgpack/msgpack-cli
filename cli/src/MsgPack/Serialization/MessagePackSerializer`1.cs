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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	// TODO: MessagePackEncoder/Decoder <|- ...NativeEncoder/Decoder, ...JsonEncoder/Decoder
	/// <summary>
	///		Defines base contract for object serialization.
	/// </summary>
	/// <typeparam name="T">Target type.</typeparam>
	public abstract class MessagePackSerializer<T>
	{
		// TODO: Metadata
		internal static MethodInfo UnpackToCoreMethod = FromExpression.ToMethod( ( MessagePackSerializer<T> @this, Unpacker unpacker, T collection ) => @this.UnpackToCore( unpacker, collection ) );

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
			return this.UnpackFrom( Unpacker.Create( stream ) );
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
		protected abstract void PackToCore( Packer packer, T objectTree );

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
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is abstract type.
		/// </exception>
		public T UnpackFrom( Unpacker unpacker )
		{
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( unpacker.IsInStart )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}
			}

			if ( typeof( T ) != typeof( MessagePackObject ) && unpacker.Data.Value.IsNil )
			{
				if ( typeof( T ).IsValueType )
				{
					if ( !( typeof( T ).IsGenericType && typeof( T ).GetGenericTypeDefinition() == typeof( Nullable<> ) ) )
					{
						throw SerializationExceptions.NewValueTypeCannotBeNull( typeof( T ) );
					}
				}

				// null
				return default( T );
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
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is abstract type.
		/// </exception>
		protected abstract T UnpackFromCore( Unpacker unpacker );

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
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not collection.
		/// </exception>
		public void UnpackTo( Unpacker unpacker, T collection )
		{
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( unpacker.IsInStart )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}
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
		protected virtual void UnpackToCore( Unpacker unpacker, T collection )
		{
			throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "This operation is not supported by '{0}'.", this.GetType() ) );
		}
	}
}
