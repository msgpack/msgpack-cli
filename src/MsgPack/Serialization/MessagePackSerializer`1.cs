#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke and contributors
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
// Contributors:
//    Samuel Cragg
//
#endregion -- License Terms --

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Globalization;
using System.IO;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization
{
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
	public abstract class MessagePackSerializer<T> : MessagePackSerializer
	{
		// ReSharper disable once StaticFieldInGenericType
		private static readonly bool IsNullable = JudgeNullable();

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackSerializer{T}"/> class with <see cref="T:PackerCompatibilityOptions.Classic"/>.
		/// </summary>
		/// <remarks>
		///		This method supports backword compatibility with 0.3.
		/// </remarks>
		[Obsolete( "Use MessagePackSerializer (SerlaizationContext) instead." )]
		protected MessagePackSerializer() : this( PackerCompatibilityOptions.Classic ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackSerializer{T}"/> class with default context..
		/// </summary>
		/// <param name="packerCompatibilityOptions">The <see cref="PackerCompatibilityOptions"/> for new packer creation.</param>
		/// <remarks>
		///		This method supports backword compatibility with 0.4.
		/// </remarks>
		[Obsolete( "Use MessagePackSerializer (SerlaizationContext, PackerCompatibilityOptions) instead." )]
		protected MessagePackSerializer( PackerCompatibilityOptions packerCompatibilityOptions )
			: this( null, packerCompatibilityOptions ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackSerializer{T}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <exception cref="ArgumentNullException"><paramref name="ownerContext"/> is <c>null</c>.</exception>
		protected MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, null, InferCapatibity() ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackSerializer{T}"/> class with explicitly specified compatibility option.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="packerCompatibilityOptions">The <see cref="PackerCompatibilityOptions"/> for new packer creation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="ownerContext"/> is <c>null</c>.</exception>
		/// <remarks>
		///		This method also supports backword compatibility with 0.4.
		/// </remarks>
		protected MessagePackSerializer( SerializationContext ownerContext, PackerCompatibilityOptions packerCompatibilityOptions )
			: base( ownerContext, packerCompatibilityOptions, InferCapatibity() ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackSerializer{T}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="capabilities">A serializer calability flags represents capabilities of this instance.</param>
		/// <exception cref="ArgumentNullException"><paramref name="ownerContext"/> is <c>null</c>.</exception>
		protected MessagePackSerializer( SerializationContext ownerContext, SerializerCapabilities capabilities )
			: base( ownerContext, null, capabilities ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackSerializer{T}"/> class with explicitly specified compatibility option.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="packerCompatibilityOptions">The <see cref="PackerCompatibilityOptions"/> for new packer creation.</param>
		/// <param name="capabilities">A serializer calability flags represents capabilities of this instance.</param>
		/// <exception cref="ArgumentNullException"><paramref name="ownerContext"/> is <c>null</c>.</exception>
		/// <remarks>
		///		This method also supports backword compatibility with 0.4.
		/// </remarks>
		protected MessagePackSerializer( SerializationContext ownerContext, PackerCompatibilityOptions packerCompatibilityOptions, SerializerCapabilities capabilities )
			: base( ownerContext, packerCompatibilityOptions, capabilities ) { }

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

		private static SerializerCapabilities InferCapatibity()
		{
			var result = SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom;
			var traits = typeof( T ).GetCollectionTraits( CollectionTraitOptions.WithAddMethod, allowNonCollectionEnumerableTypes: false );
			if ( traits.AddMethod != null )
			{
				result |= SerializerCapabilities.UnpackTo;
			}

			return result;
		}

		/// <summary>
		///		Serializes specified object to the <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">Destination <see cref="Stream"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		public void Pack( Stream stream, T objectTree )
		{
			// Packer does not have finalizer, so just avoiding packer disposing prevents stream closing.
			this.PackTo( Packer.Create( stream, this.PackerCompatibilityOptions ), objectTree );
		}

#if FEATURE_TAP

		/// <summary>
		///		Serializes specified object to the <see cref="Stream"/> asynchronously.
		/// </summary>
		/// <param name="stream">Destination <see cref="Stream"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		public Task PackAsync( Stream stream, T objectTree )
		{
			return this.PackAsync( stream, objectTree, CancellationToken.None );
		}

		/// <summary>
		///		Serializes specified object to the <see cref="Stream"/> asynchronously.
		/// </summary>
		/// <param name="stream">Destination <see cref="Stream"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		public async Task PackAsync( Stream stream, T objectTree, CancellationToken cancellationToken )
		{
			var packer = Packer.Create( stream, this.PackerCompatibilityOptions, PackerUnpackerStreamOptions.SingletonForAsync );
			try
			{
				await this.PackToAsync( packer, objectTree, cancellationToken ).ConfigureAwait( false );
			}
			finally
			{
				await packer.FlushAsync( cancellationToken ).ConfigureAwait( false );
			}
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Deserialize object from the <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">Source <see cref="Stream"/>.</param>
		/// <returns>The deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
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
		///		<typeparamref name="T"/> is not serializable even if it can be serialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		public T Unpack( Stream stream )
		{
			// Unpacker does not have finalizer, so just avoiding unpacker disposing prevents stream closing.
			var unpacker = Unpacker.Create( stream, PackerUnpackerStreamOptions.None, DefaultUnpackerOptions );
			if ( !unpacker.Read() )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}

			return this.UnpackFrom( unpacker );
		}

#if FEATURE_TAP

		/// <summary>
		///		Deserialize object from the <see cref="Stream"/> asynchronously.
		/// </summary>
		/// <param name="stream">Source <see cref="Stream"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the deserialized object.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
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
		///		<typeparamref name="T"/> is not serializable even if it can be serialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		public Task<T> UnpackAsync( Stream stream )
		{
			return this.UnpackAsync( stream, CancellationToken.None );
		}

		/// <summary>
		///		Deserialize object from the <see cref="Stream"/> asynchronously.
		/// </summary>
		/// <param name="stream">Source <see cref="Stream"/>.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the deserialized object.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
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
		///		<typeparamref name="T"/> is not serializable even if it can be serialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		public async Task<T> UnpackAsync( Stream stream, CancellationToken cancellationToken )
		{
			// Unpacker does not have finalizer, so just avoiding unpacker disposing prevents stream closing.
			var unpacker = Unpacker.Create( stream, PackerUnpackerStreamOptions.SingletonForAsync, DefaultUnpackerOptions );
			if ( !( await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) ) )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}

			return await this.UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false );
		}
#endif // FEATURE_TAP

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="packer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public void PackTo( Packer packer, T objectTree )
		{
			if ( packer == null )
			{
				ThrowArgumentNullException( "packer" );
			}

			// ReSharper disable once CompareNonConstrainedGenericWithNull
			if ( objectTree == null )
			{
				// ReSharper disable once PossibleNullReferenceException
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
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		protected internal abstract void PackToCore( Packer packer, T objectTree );

#if FEATURE_TAP

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/> asynchronously.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="packer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		public Task PackToAsync( Packer packer, T objectTree )
		{
			return this.PackToAsync( packer, objectTree, CancellationToken.None );
		}

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/> asynchronously.
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
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public async Task PackToAsync( Packer packer, T objectTree, CancellationToken cancellationToken )
		{
			if ( packer == null )
			{
				ThrowArgumentNullException( "packer" );
			}

			// ReSharper disable once CompareNonConstrainedGenericWithNull
			if ( objectTree == null )
			{
				// ReSharper disable once PossibleNullReferenceException
				await packer.PackNullAsync( cancellationToken ).ConfigureAwait( false );
				return;
			}

			await this.PackToAsyncCore( packer, objectTree, cancellationToken ).ConfigureAwait( false );
		}

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/> asynchronously.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		protected internal virtual Task PackToAsyncCore( Packer packer, T objectTree, CancellationToken cancellationToken )
		{
#if DEBUG
			SerializerDebugging.EnsureNaiveAsyncAllowed( this );
#endif // DEBUG
			return Task.Run( () => this.PackToCore( packer, objectTree ), cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Deserializes object with specified <see cref="Unpacker"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree.</param>
		/// <returns>The deserialized object.</returns>
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
		///		<typeparamref name="T"/> is not serializable even if it can be serialized.
		/// </exception>
		/// <remarks>
		///		You must call <see cref="Unpacker.Read()"/> at least once in advance.
		///		Or, you will get a default value of <typeparamref name="T"/>.
		/// </remarks>
		/// <seealso cref="P:Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public new T UnpackFrom( Unpacker unpacker )
		{
			if ( unpacker == null )
			{
				ThrowArgumentNullException( "unpacker" );
			}

			// ReSharper disable once PossibleNullReferenceException
			if ( unpacker.LastReadData.IsNil )
			{
				return this.UnpackNil();
			}

			return this.UnpackFromCore( unpacker );
		}

		/// <summary>
		///		Unpacks the nil value.
		/// </summary>
		/// <returns>
		///		A valid value of <typeparamref name="T"/> which represents 'null' state.
		/// </returns>
		/// <remarks>
		///		<para>
		///			This method is invoked from <see cref="UnpackFrom"/> method when the current <see cref="Unpacker.LastReadData"/> is <see cref="MessagePackObject.IsNil">nil</see>.
		///		</para>
		///		<para>
		///			The implementation of this class returns <c>null</c> for nullable types (that is, all reference types and <see cref="Nullable{T}"/>); otherwise, throws <see cref="System.Runtime.Serialization.SerializationException"/>.
		///		</para>
		///		<para>
		///			Custom serializers can override this method to provide custom nil representation. For example, built-in <c>DBNull</c> serializer overrides this method to return <c>DBNull.Value</c> instead of <c>null</c>.
		///		</para>
		/// </remarks>
		protected internal virtual T UnpackNil()
		{
			if ( !IsNullable )
			{
				ThrowNewValueTypeCannotBeNullException();
			}

			// null
			return default( T );
		}

		/// <summary>
		///		Deserializes object with specified <see cref="Unpacker"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <returns>The deserialized object.</returns>
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
		///		<typeparamref name="T"/> is not serializable even if it can be serialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		protected internal abstract T UnpackFromCore( Unpacker unpacker );

#if FEATURE_TAP

		/// <summary>
		///		Deserializes object with specified <see cref="Unpacker"/> asynchronously.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree.</param>
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
		///		<typeparamref name="T"/> is not serializable even if it can be serialized.
		/// </exception>
		/// <remarks>
		///		You must call <see cref="Unpacker.Read()"/> at least once in advance.
		///		Or, you will get a default value of <typeparamref name="T"/>.
		/// </remarks>
		/// <seealso cref="P:Capabilities"/>
		public Task<T> UnpackFromAsync( Unpacker unpacker )
		{
			return this.UnpackFromAsync( unpacker, CancellationToken.None );
		}

		/// <summary>
		///		Deserializes object with specified <see cref="Unpacker"/> asynchronously.
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
		///		<typeparamref name="T"/> is not serializable even if it can be serialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public new async Task<T> UnpackFromAsync( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				ThrowArgumentNullException( "unpacker" );
			}

			// ReSharper disable once PossibleNullReferenceException
			if ( unpacker.LastReadData.IsNil )
			{
				return this.UnpackNil();
			}

			return await this.UnpackFromAsyncCore( unpacker, cancellationToken ).ConfigureAwait( false );
		}

		/// <summary>
		///		Deserializes object with specified <see cref="Unpacker"/> asynchronously.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the deserialized object.
		/// </returns>
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
		///		<typeparamref name="T"/> is not serializable even if it can be serialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		protected internal virtual Task<T> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
#if DEBUG
			SerializerDebugging.EnsureNaiveAsyncAllowed( this );
#endif // DEBUG
			return Task.Run( () => this.UnpackFrom( unpacker ), cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Deserializes collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree.</param>
		/// <param name="collection">Collection that the items to be stored.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or <paramref name="collection"/> is <c>null</c>.
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
		///		<typeparamref name="T"/> is not mutable collection.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		public void UnpackTo( Unpacker unpacker, T collection )
		{
			if ( unpacker == null )
			{
				ThrowArgumentNullException( "unpacker" );
			}

			// ReSharper disable once CompareNonConstrainedGenericWithNull
			if ( collection == null )
			{
				ThrowArgumentNullException( "collection" );
			}

			// ReSharper disable once PossibleNullReferenceException
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
		///		<typeparamref name="T"/> is not mutable collection.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		protected internal virtual void UnpackToCore( Unpacker unpacker, T collection )
		{
			throw SerializationExceptions.NewUnpackToIsNotSupported( typeof( T ), null );
		}

#if FEATURE_TAP

		/// <summary>
		///		Deserializes collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/> asynchronously.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree.</param>
		/// <param name="collection">Collection that the items to be stored.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or <paramref name="collection"/> is <c>null</c>.
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
		///		<typeparamref name="T"/> is not mutable collection.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		public Task UnpackToAsync( Unpacker unpacker, T collection )
		{
			return this.UnpackToAsync( unpacker, collection, CancellationToken.None );
		}

		/// <summary>
		///		Deserializes collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/> asynchronously.
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
		///		<typeparamref name="T"/> is not mutable collection.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		public async Task UnpackToAsync( Unpacker unpacker, T collection, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				ThrowArgumentNullException( "unpacker" );
			}

			// ReSharper disable once CompareNonConstrainedGenericWithNull
			if ( collection == null )
			{
				ThrowArgumentNullException( "collection" );
			}

			// ReSharper disable once PossibleNullReferenceException
			if ( unpacker.LastReadData.IsNil )
			{
				return;
			}

			await this.UnpackToAsyncCore( unpacker, collection, cancellationToken ).ConfigureAwait( false );
		}

		/// <summary>
		///		Deserializes collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/> asynchronously.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <param name="collection">Collection that the items to be stored. This value will not be <c>null</c>.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		/// </returns>
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
		///		<typeparamref name="T"/> is not mutable collection.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		protected internal virtual Task UnpackToAsyncCore( Unpacker unpacker, T collection, CancellationToken cancellationToken )
		{
#if DEBUG
			SerializerDebugging.EnsureNaiveAsyncAllowed( this );
#endif // DEBUG

			return Task.Run( () => this.UnpackToCore( unpacker, collection ), cancellationToken );
		}

#endif // FEATURE_TAP

		/// <summary>
		///		Serializes specified object to the array of <see cref="Byte"/>.
		/// </summary>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>An array of <see cref="Byte"/> which stores serialized value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		public byte[] PackSingleObject( T objectTree )
		{
			var segment = this.PackSingleObjectAsBytes( objectTree );

			if ( segment.Count == segment.Array.Length )
			{
				return segment.Array;
			}
			else
			{
				var result = new byte[ segment.Count ];
				Buffer.BlockCopy( segment.Array, segment.Offset, result, 0, segment.Count );
				return result;
			}
		}

		/// <summary>
		///		Serializes specified object to the <see cref="ArraySegment{T}"/> of <see cref="Byte"/>.
		/// </summary>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>An array of <see cref="Byte"/> which stores serialized value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <remarks>
		///		This method is more efficient than <see cref="PackSingleObject(T)"/> because of less copying.
		/// </remarks>
		/// <seealso cref="P:Capabilities"/>
		public ArraySegment<byte> PackSingleObjectAsBytes( T objectTree )
		{
			// Packer does not have finalizer, so just avoiding unpacker disposing prevents stream closing.
			var packer = Packer.Create( BufferManager.NewByteBuffer( BufferSize ), /* allowExpansion */true, this.PackerCompatibilityOptions );

			this.PackTo( packer, objectTree );
			return packer.GetResultBytes();
		}

#if FEATURE_TAP

		/// <summary>
		///		Serializes specified object to the array of <see cref="Byte"/> asynchronously.
		/// </summary>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains an array of <see cref="Byte"/> which stores serialized value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		public Task<byte[]> PackSingleObjectAsync( T objectTree )
		{
			return this.PackSingleObjectAsync( objectTree, CancellationToken.None );
		}

		/// <summary>
		///		Serializes specified object to the array of <see cref="Byte"/> asynchronously.
		/// </summary>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains an array of <see cref="Byte"/> which stores serialized value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		public async Task<byte[]> PackSingleObjectAsync( T objectTree, CancellationToken cancellationToken )
		{
			var segment = await this.PackSingleObjectAsBytesAsync( objectTree, cancellationToken ).ConfigureAwait( false );
			if ( segment.Count == segment.Array.Length )
			{
				return segment.Array;
			}
			else
			{
				var result = new byte[ segment.Count ];
				Buffer.BlockCopy( segment.Array, segment.Offset, result, 0, segment.Count );
				return result;
			}
		}

		/// <summary>
		///		Serializes specified object to the <see cref="ArraySegment{T}"/> of <see cref="Byte"/> asynchronously.
		/// </summary>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains an array of <see cref="Byte"/> which stores serialized value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="T"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <remarks>
		///		This method is more efficient than <see cref="PackSingleObject(T)"/> because of less copying.
		/// </remarks>
		/// <seealso cref="P:Capabilities"/>
		public async Task<ArraySegment<byte>> PackSingleObjectAsBytesAsync( T objectTree, CancellationToken cancellationToken )
		{
			// Packer does not have finalizer, so just avoiding unpacker disposing prevents stream closing.
			var packer = Packer.Create( BufferManager.NewByteBuffer( BufferSize ),  /* allowExpansion */true, this.PackerCompatibilityOptions );

			await this.PackToAsync( packer, objectTree, cancellationToken ).ConfigureAwait( false );
			return packer.GetResultBytes();
		}
#endif // FEATURE_TAP

		/// <summary>
		///		Deserializes a single object from the array of <see cref="Byte"/> which contains a serialized object.
		/// </summary>
		/// <param name="buffer">An array of <see cref="Byte"/> serialized value to be stored.</param>
		/// <returns>The deserialized object.</returns>
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
		/// <seealso cref="P:Capabilities"/>
		/// <remarks>
		///		<para>
		///			This method assumes that <paramref name="buffer"/> contains single serialized object dedicatedly,
		///			so this method does not return any information related to actual consumed bytes.
		///		</para>
		///		<para>
		///			This method is a counter part of <see cref="PackSingleObject"/>.
		///		</para>
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public new T UnpackSingleObject( byte[] buffer )
		{
			if ( buffer == null )
			{
				ThrowArgumentNullException( "buffer" );
			}

			// Unpacker does not have finalizer, so just avoiding unpacker disposing prevents stream closing.
			// ReSharper disable once AssignNullToNotNullAttribute
			var unpacker = Unpacker.Create( buffer, DefaultUnpackerOptions );

			if ( !unpacker.Read() )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}

			return this.UnpackFrom( unpacker );
		}

#if FEATURE_TAP

		/// <summary>
		///		Deserializes a single object from the array of <see cref="Byte"/> which contains a serialized object asynchronously.
		/// </summary>
		/// <param name="buffer">An array of <see cref="Byte"/> serialized value to be stored.</param>
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
		/// <seealso cref="P:Capabilities"/>
		/// <remarks>
		///		<para>
		///			This method assumes that <paramref name="buffer"/> contains single serialized object dedicatedly,
		///			so this method does not return any information related to actual consumed bytes.
		///		</para>
		///		<para>
		///			This method is a counter part of <see cref="PackSingleObject"/>.
		///		</para>
		/// </remarks>
		public Task<T> UnpackSingleObjectAsync( byte[] buffer )
		{
			return this.UnpackSingleObjectAsync( buffer, CancellationToken.None );
		}

		/// <summary>
		///		Deserializes a single object from the array of <see cref="Byte"/> which contains a serialized object asynchronously.
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
		/// <seealso cref="P:Capabilities"/>
		/// <remarks>
		///		<para>
		///			This method assumes that <paramref name="buffer"/> contains single serialized object dedicatedly,
		///			so this method does not return any information related to actual consumed bytes.
		///		</para>
		///		<para>
		///			This method is a counter part of <see cref="PackSingleObject"/>.
		///		</para>
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public new Task<T> UnpackSingleObjectAsync( byte[] buffer, CancellationToken cancellationToken )
		{
			if ( buffer == null )
			{
				ThrowArgumentNullException( "buffer" );
			}

			// Unpacker does not have finalizer, so just avoiding unpacker disposing prevents stream closing.
			// ReSharper disable once AssignNullToNotNullAttribute
			var unpacker = Unpacker.Create( buffer, DefaultUnpackerOptions );

			if ( !unpacker.Read() )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}

			return this.UnpackFromAsync( unpacker, cancellationToken );
		}

#endif // FEATURE_TAP

		internal sealed override void InternalPackTo( Packer packer, object objectTree )
		{
			if ( packer == null )
			{
				ThrowArgumentNullException( "packer" );
			}

			if ( objectTree == null )
			{
				if ( typeof( T ).GetIsValueType() )
				{
					if ( !( typeof( T ).GetIsGenericType() && typeof( T ).GetGenericTypeDefinition() == typeof( Nullable<> ) ) )
					{
						ThrowNewValueTypeCannotBeNullException();
					}
				}

				// ReSharper disable once PossibleNullReferenceException
				packer.PackNull();
				return;
			}
			else
			{
				if ( !( objectTree is T ) )
				{
					ThrowArgumentException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not compatible for '{1}'.", objectTree.GetType(), typeof( T ) ), "objectTree" );
				}
			}

			this.PackToCore( packer, ( T )objectTree );
		}

		internal sealed override object InternalUnpackFrom( Unpacker unpacker )
		{
			return this.UnpackFrom( unpacker );
		}

		internal sealed override void InternalUnpackTo( Unpacker unpacker, object collection )
		{
			if ( unpacker == null )
			{
				ThrowArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				ThrowArgumentNullException( "collection" );
			}

			if ( !( collection is T ) )
			{
				// ReSharper disable once PossibleNullReferenceException
				ThrowArgumentException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not compatible for '{1}'.", collection.GetType(), typeof( T ) ), "collection" );
			}

			this.UnpackTo( unpacker, ( T )collection );
		}

		internal sealed override byte[] InternalPackSingleObject( object objectTree )
		{
			var isT = objectTree is T;
			if ( ( typeof( T ).GetIsValueType() && !isT )
				|| ( ( objectTree != null && !isT ) ) )
			{
				ThrowArgumentException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not compatible for '{1}'.", objectTree == null ? "(null)" : objectTree.GetType().FullName, typeof( T ) ), "objectTree" );
			}

			return this.PackSingleObject( ( T )objectTree );
		}

		internal sealed override object InternalUnpackSingleObject( byte[] buffer )
		{
			return this.UnpackSingleObject( buffer );
		}

#if FEATURE_TAP

		internal sealed override async Task InternalPackToAsync( Packer packer, object objectTree, CancellationToken cancellationToken )
		{
			if ( packer == null )
			{
				ThrowArgumentNullException( "packer" );
			}

			if ( objectTree == null )
			{
				if ( typeof( T ).GetIsValueType() )
				{
					if ( !( typeof( T ).GetIsGenericType() && typeof( T ).GetGenericTypeDefinition() == typeof( Nullable<> ) ) )
					{
						ThrowNewValueTypeCannotBeNullException();
					}
				}

				// ReSharper disable once PossibleNullReferenceException
				await packer.PackNullAsync( cancellationToken ).ConfigureAwait( false );
				return;
			}
			else
			{
				if ( !( objectTree is T ) )
				{
					ThrowArgumentException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not compatible for '{1}'.", objectTree.GetType(), typeof( T ) ), "objectTree" );
				}
			}

			await this.PackToAsyncCore( packer, ( T )objectTree, cancellationToken ).ConfigureAwait( false );
		}

		internal sealed override async Task<object> InternalUnpackFromAsync( Unpacker unpacker, CancellationToken cancellationToken )
		{
			return await this.UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false );
		}

		internal sealed override async Task InternalUnpackToAsync( Unpacker unpacker, object collection, CancellationToken cancellationToken )
		{
			if ( unpacker == null )
			{
				ThrowArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				ThrowArgumentNullException( "collection" );
			}

			if ( !( collection is T ) )
			{
				// ReSharper disable once PossibleNullReferenceException
				ThrowArgumentException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not compatible for '{1}'.", collection.GetType(), typeof( T ) ), "collection" );
			}

			await this.UnpackToAsync( unpacker, ( T )collection, cancellationToken ).ConfigureAwait( false );
		}

		internal sealed override async Task<byte[]> InternalPackSingleObjectAsync( object objectTree, CancellationToken cancellationToken )
		{
			var isT = objectTree is T;
			if ( ( typeof( T ).GetIsValueType() && !isT )
				|| ( ( objectTree != null && !isT ) ) )
			{
				ThrowArgumentException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not compatible for '{1}'.", objectTree == null ? "(null)" : objectTree.GetType().FullName, typeof( T ) ), "objectTree" );
			}

			return await this.PackSingleObjectAsync( ( T )objectTree, cancellationToken ).ConfigureAwait( false );
		}

		internal sealed override async Task<object> InternalUnpackSingleObjectAsync( byte[] buffer, CancellationToken cancellationToken )
		{
			return await this.UnpackSingleObjectAsync( buffer, cancellationToken ).ConfigureAwait( false );
		}
#endif // FEATURE_TAP

		private static void ThrowArgumentException( string message, string parameterName )
		{
			throw new ArgumentException( message, parameterName );
		}

		private static void ThrowNewValueTypeCannotBeNullException()
		{
			throw SerializationExceptions.NewValueTypeCannotBeNull( typeof( T ) );
		}

	}
}
