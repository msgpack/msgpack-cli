#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015-2018 FUJIWARA, Yusuke
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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines non-generic interface of serializers and provides entry points for <see cref="MessagePackSerializer{T}"/> usage.
	/// </summary>
	/// <remarks>
	///		You cannot derived from this class directly, use <see cref="MessagePackSerializer{T}"/> instead.
	///		This class is intended to guarantee backward compatibilities of non generic API.
	/// </remarks>
#pragma warning disable 0618
	public abstract partial class MessagePackSerializer : IMessagePackSingleObjectSerializer
#pragma warning restore 0618
	{
		internal static readonly UnpackerOptions DefaultUnpackerOptions = new UnpackerOptions { ValidationLevel = UnpackerValidationLevel.None };

#if UNITY && DEBUG
		public
#else
		internal
#endif
		const int BufferSize = 256;

		private readonly SerializationContext _ownerContext;

		/// <summary>
		///		Gets a <see cref="SerializationContext"/> which owns this serializer.
		/// </summary>
		/// <value>
		///		A <see cref="SerializationContext"/> which owns this serializer.
		/// </value>
		protected internal SerializationContext OwnerContext
		{
			get { return this._ownerContext; }
		}

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

		private readonly SerializerCapabilities _capabilities;

		/// <summary>
		///		Gets the capability flags for this instance.
		/// </summary>
		/// <value>
		///		The capability flags for this instance.
		/// </value>
		public SerializerCapabilities Capabilities
		{
			get { return this.InternalGetCapabilities(); }
		}

		// For LazyDelegatingMessagePackSerializer
		internal virtual SerializerCapabilities InternalGetCapabilities()
		{
			return this._capabilities;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackSerializer"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="packerCompatibilityOptions">The <see cref="PackerCompatibilityOptions"/> for new packer creation.</param>
		/// <param name="capabilities">The capability flags for this instance.</param>
		internal MessagePackSerializer( SerializationContext ownerContext, PackerCompatibilityOptions? packerCompatibilityOptions, SerializerCapabilities capabilities )
		{
			if ( ownerContext == null )
			{
				ThrowArgumentNullException( "ownerContext" );
			}

			this._ownerContext = ownerContext;
			this._packerCompatibilityOptionsForCompatibility = packerCompatibilityOptions;
			this._capabilities = capabilities;
		}

		/// <summary>
		///		Serialize specified object with specified <see cref="Packer" />.
		/// </summary>
		/// <param name="packer"><see cref="Packer" /> which packs values in <paramref name="objectTree" />.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <seealso cref="Capabilities" />
		public void PackTo( Packer packer, object objectTree )
		{
			this.InternalPackTo( packer, objectTree );
		}

		internal abstract void InternalPackTo( Packer packer, object objectTree );

		/// <summary>
		///		Deserialize object with specified <see cref="Unpacker" />.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker" /> which unpacks values of resulting object tree.</param>
		/// <returns>
		///		The deserialized object.
		/// </returns>
		/// <remarks>
		///		You must call <see cref="Unpacker.Read()"/> at least once in advance.
		///		Or, you will get a default value of the object.
		/// </remarks>
		/// <seealso cref="Capabilities" />
		public object UnpackFrom( Unpacker unpacker )
		{
			return this.InternalUnpackFrom( unpacker );
		}

		internal abstract object InternalUnpackFrom( Unpacker unpacker );

		/// <summary>
		///		Deserialize collection items with specified <see cref="Unpacker" /> and stores them to <paramref name="collection" />.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker" /> which unpacks values of resulting object tree.</param>
		/// <param name="collection">Collection that the items to be stored.</param>
		/// <seealso cref="Capabilities" />
		public void UnpackTo( Unpacker unpacker, object collection )
		{
			this.InternalUnpackTo( unpacker, collection );
		}

		internal abstract void InternalUnpackTo( Unpacker unpacker, object collection );

		/// <summary>
		///		Serialize specified object to the array of <see cref="Byte" />.
		/// </summary>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>
		///		An array of <see cref="Byte" /> which stores serialized value.
		/// </returns>
		/// <seealso cref="Capabilities" />
		public byte[] PackSingleObject( object objectTree )
		{
			return this.InternalPackSingleObject( objectTree );
		}

		internal abstract byte[] InternalPackSingleObject( object objectTree );

		/// <summary>
		///		Deserialize a single object from the array of <see cref="Byte" /> which contains a serialized object.
		/// </summary>
		/// <param name="buffer">An array of <see cref="Byte" /> serialized value to be stored.</param>
		/// <returns>
		///		The deserialized object.
		/// </returns>
		/// <remarks>
		/// <para>
		///		This method assumes that <paramref name="buffer" /> contains single serialized object dedicatedly,
		///		so this method does not return any information related to actual consumed bytes.
		/// </para>
		/// <para>
		///		This method is a counter part of <see cref="PackSingleObject" />.
		/// </para>
		/// </remarks>
		/// <seealso cref="Capabilities" />
		public object UnpackSingleObject( byte[] buffer )
		{
			return this.InternalUnpackSingleObject( buffer );
		}

		internal abstract object InternalUnpackSingleObject( byte[] buffer );

#if FEATURE_TAP
		/// <summary>
		///		Serialize specified object with specified <see cref="Packer"/> asynchronously.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="packer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="objectTree"/> is not compatible for this serializer.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		The type of <paramref name="objectTree"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = "objectTree is more meningful." )]
		public Task PackToAsync( Packer packer, object objectTree, CancellationToken cancellationToken )
		{
			return this.InternalPackToAsync( packer, objectTree, cancellationToken );
		}

		internal abstract Task InternalPackToAsync( Packer packer, object objectTree, CancellationToken cancellationToken );

		/// <summary>
		///		Deserialize object with specified <see cref="Unpacker"/> asynchronously.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the deserialized object.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialize object.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid stream.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		The type of deserializing is not serializable even if it can be serialized.
		/// </exception>
		/// <remarks>
		///		You must call <see cref="Unpacker.Read()"/> at least once in advance.
		///		Or, you will get a default value of the object.
		/// </remarks>
		/// <seealso cref="Capabilities"/>
		public Task<object> UnpackFromAsync( Unpacker unpacker, CancellationToken cancellationToken )
		{
			return this.InternalUnpackFromAsync( unpacker, cancellationToken );
		}

		internal abstract Task<object> InternalUnpackFromAsync( Unpacker unpacker, CancellationToken cancellationToken );

		/// <summary>
		///		Deserialize collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/> asynchronously.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree.</param>
		/// <param name="collection">Collection that the items to be stored.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or <paramref name="collection"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="collection"/> is not compatible for this serializer.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialize object.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid stream.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		The type of deserializing is not mutable collection.
		/// </exception>
		/// <seealso cref="Capabilities"/>
		public Task UnpackToAsync( Unpacker unpacker, object collection, CancellationToken cancellationToken )
		{
			return this.InternalUnpackToAsync( unpacker, collection, cancellationToken );
		}

		internal abstract Task InternalUnpackToAsync( Unpacker unpacker, object collection, CancellationToken cancellationToken );

		/// <summary>
		///		Serialize specified object to the array of <see cref="Byte"/>.
		/// </summary>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>An array of <see cref="Byte"/> which stores serialized value.</returns>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains an array of <see cref="Byte"/> which stores serialized value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize <paramref name="objectTree"/>.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		The type of <paramref name="objectTree"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = "objectTree is more meningful." )]
		public Task<byte[]> PackSingleObjectAsync( object objectTree, CancellationToken cancellationToken )
		{
			return this.InternalPackSingleObjectAsync( objectTree, cancellationToken );
		}

		internal abstract Task<byte[]> InternalPackSingleObjectAsync( object objectTree, CancellationToken cancellationToken );

		/// <summary>
		///		Deserialize a single object from the array of <see cref="Byte"/> which contains a serialized object.
		/// </summary>
		/// <param name="buffer">An array of <see cref="Byte"/> serialized value to be stored.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the deserialized object.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialize object.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid stream.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		The type of deserializing is not serializable even if it can be serialized.
		/// </exception>
		/// <seealso cref="Capabilities"/>
		/// <remarks>
		///		<para>
		///			This method assumes that <paramref name="buffer"/> contains single serialized object dedicatedly,
		///			so this method does not return any information related to actual consumed bytes.
		///		</para>
		///		<para>
		///			This method is a counter part of <see cref="PackSingleObjectAsync"/>.
		///		</para>
		/// </remarks>	
		public Task<object> UnpackSingleObjectAsync( byte[] buffer, CancellationToken cancellationToken )
		{
			return this.InternalUnpackSingleObjectAsync( buffer, cancellationToken );
		}

		internal abstract Task<object> InternalUnpackSingleObjectAsync( byte[] buffer, CancellationToken cancellationToken );
#endif // FEATURE_TAP

		/* protected and */internal static void ThrowArgumentNullException( string parameterName )
		{
			throw new ArgumentNullException( parameterName );
		}
	}
}
