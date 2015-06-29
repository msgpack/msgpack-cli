#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015 FUJIWARA, Yusuke
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
using System.IO;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	internal abstract class NonGenericMessagePackSerializer : IMessagePackSingleObjectSerializer
	{
		private readonly Type _targetType;

		protected Type TargetType
		{
			get { return this._targetType; }
		}

		private readonly bool _isNullable;

		private readonly PackerCompatibilityOptions? _packerCompatibilityOptionsForCompatibility;

		/// <summary>
		///		Gets the packer compatibility options for this instance.
		/// </summary>
		/// <value>
		///		The packer compatibility options for this instance
		/// </value>
		protected internal PackerCompatibilityOptions PackerCompatibilityOptions
		{
			get { return this._packerCompatibilityOptionsForCompatibility.GetValueOrDefault( this.OwnerContext.CompatibilityOptions.PackerCompatibilityOptions ); }
		}

		private readonly SerializationContext _ownerContext;

		/// <summary>
		///		Gets a <see cref="SerializationContext"/> which owns this serializer.
		/// </summary>
		/// <value>
		///		A <see cref="SerializationContext"/> which owns this serializer.
		/// </value>
		protected internal SerializationContext OwnerContext
		{
			get{ return this._ownerContext; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="NonGenericMessagePackSerializer"/> class with explicitly specified compatibility option.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="targetType">The type to be serialized.</param>
		/// <exception cref="ArgumentNullException"><paramref name="ownerContext"/> is <c>null</c>.</exception>
		protected NonGenericMessagePackSerializer( SerializationContext ownerContext, Type targetType )
			: this( ownerContext, targetType, null ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="NonGenericMessagePackSerializer"/> class with explicitly specified compatibility option.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="targetType">The type to be serialized.</param>
		/// <param name="packerCompatibilityOptions">The <see cref="PackerCompatibilityOptions"/> for new packer creation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="ownerContext"/> is <c>null</c>.</exception>
		/// <remarks>
		///		This method also supports backword compatibility with 0.4.
		/// </remarks>
		protected NonGenericMessagePackSerializer( SerializationContext ownerContext, Type targetType, PackerCompatibilityOptions packerCompatibilityOptions )
			: this( ownerContext, targetType, new PackerCompatibilityOptions?( packerCompatibilityOptions ) ) { }

		private NonGenericMessagePackSerializer( SerializationContext ownerContext, Type targetType, PackerCompatibilityOptions? packerCompatibilityOptions )
		{
			if ( ownerContext == null )
			{
				throw new ArgumentNullException( "ownerContext" );
			}

			this._packerCompatibilityOptionsForCompatibility = packerCompatibilityOptions;
			this._ownerContext = ownerContext;
			this._targetType = targetType;
			this._isNullable = JudgeNullable( targetType );
		}

		private static bool JudgeNullable( Type type )
		{
			if ( !type.GetIsValueType() )
			{
				// reference type.
				return true;
			}

			if ( type == typeof( MessagePackObject ) )
			{
				// can be MPO.Nil.
				return true;
			}

			if ( type.GetIsGenericType() && type.GetGenericTypeDefinition() == typeof( Nullable<> ) )
			{
				// Nullable<T>
				return true;
			}

			return false;
		}

		/// <summary>
		///		Serializes specified object to the <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">Destination <see cref="Stream"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="SerializationException">
		///		The type of <paramref name="objectTree" /> is not serializable etc.
		/// </exception>
		public void Pack( Stream stream, object objectTree )
		{
			// Packer does not have finalizer, so just avoiding packer disposing prevents stream closing.
			this.PackTo( Packer.Create( stream, this.PackerCompatibilityOptions ), objectTree );
		}

		/// <summary>
		///		Deserialize object from the <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">Source <see cref="Stream"/>.</param>
		/// <returns>Deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
		/// </exception>
		public object Unpack( Stream stream )
		{
			// Unpacker does not have finalizer, so just avoiding unpacker disposing prevents stream closing.
			var unpacker = Unpacker.Create( stream );
			if ( !unpacker.Read() )
			{
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}

			return this.UnpackFrom( unpacker );
		}

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="packer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="SerializationException">
		///		The type of <paramref name="objectTree" /> is not serializable etc.
		/// </exception>
		public void PackTo( Packer packer, object objectTree )
		{
			// TODO: Hot-Path-Optimization
			if ( packer == null )
			{
				throw new ArgumentNullException( "packer" );
			}

			// ReSharper disable once CompareNonConstrainedGenericWithNull
			if ( objectTree == null )
			{
				packer.PackNull();
				return;
			}

			this.PackToCore( packer, objectTree );
		}

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="SerializationException">
		///		The type of <paramref name="objectTree" /> is not serializable etc.
		/// </exception>
		protected internal abstract void PackToCore( Packer packer, object objectTree );

		/// <summary>
		///		Deserializes object with specified <see cref="Unpacker"/>.
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
		public object UnpackFrom( Unpacker unpacker )
		{
			// TODO: Hot-Path-Optimization
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( unpacker.LastReadData.IsNil )
			{
				if ( this._isNullable )
				{
					// null
					return null;
				}
				else
				{
					throw SerializationExceptions.NewValueTypeCannotBeNull( this._targetType );
				}
			}

			return this.UnpackFromCore( unpacker );
		}

		/// <summary>
		///		Deserializes object with specified <see cref="Unpacker"/>.
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
		protected internal abstract object UnpackFromCore( Unpacker unpacker );

		/// <summary>
		///		Deserializes collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/>.
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
		///		The type of <paramref name="collection" /> is not a collection.
		/// </exception>
		public void UnpackTo( Unpacker unpacker, object collection )
		{
			// TODO: Hot-Path-Optimization
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			// ReSharper disable once CompareNonConstrainedGenericWithNull
			if ( collection == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( unpacker.LastReadData.IsNil )
			{
				return;
			}

			this.UnpackToCore( unpacker, collection );
		}

		/// <summary>
		///		Deserializes collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <param name="collection">Collection that the items to be stored. This value will not be <c>null</c>.</param>
		/// <exception cref="SerializationException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		The type of <paramref name="collection" /> is not a collection.
		/// </exception>
		protected internal virtual void UnpackToCore( Unpacker unpacker, object collection )
		{
			throw SerializationExceptions.NewUnpackToIsNotSupported( this._targetType, null );
		}

		/// <summary>
		///		Serializes specified object to the array of <see cref="Byte"/>.
		/// </summary>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>An array of <see cref="Byte"/> which stores serialized value.</returns>
		/// <exception cref="SerializationException">
		///		The type of <paramref name="objectTree" /> is not serializable etc.
		/// </exception>
		public byte[] PackSingleObject( object objectTree )
		{
			using ( var buffer = new MemoryStream() )
			{
				this.Pack( buffer, objectTree );
				return buffer.ToArray();
			}
		}

		/// <summary>
		///		Deserializes a single object from the array of <see cref="Byte"/> which contains a serialized object.
		/// </summary>
		/// <param name="buffer">An array of <see cref="Byte"/> serialized value to be stored.</param>
		/// <returns>A bytes of serialized binary.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffer"/> is <c>null</c>.
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
		///		<para>
		///			This method assumes that <paramref name="buffer"/> contains single serialized object dedicatedly,
		///			so this method does not return any information related to actual consumed bytes.
		///		</para>
		///		<para>
		///			This method is a counter part of <see cref="PackSingleObject"/>.
		///		</para>
		/// </remarks>
		public object UnpackSingleObject( byte[] buffer )
		{
			if ( buffer == null )
			{
				throw new ArgumentNullException( "buffer" );
			}

			using ( var stream = new MemoryStream( buffer ) )
			{
				return this.Unpack( stream );
			}
		}
	}
}
