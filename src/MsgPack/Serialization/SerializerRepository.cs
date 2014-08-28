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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Repository of known <see cref="MessagePackSerializer{T}"/>s.
	/// </summary>
	public sealed partial class SerializerRepository : IDisposable
	{
		private readonly SerializerTypeKeyRepository _repository;

		/// <summary>
		/// Initializes a new empty instance of the <see cref="SerializerRepository"/> class.
		/// </summary>
		public SerializerRepository()
		{
			this._repository = new SerializerTypeKeyRepository();
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializerRepository"/> class  which has copied serializers.
		/// </summary>
		/// <param name="copiedFrom">The repository which will be copied its contents.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="copiedFrom"/> is <c>null</c>.
		/// </exception>
		public SerializerRepository( SerializerRepository copiedFrom )
		{
			if ( copiedFrom == null )
			{
				throw new ArgumentNullException( "copiedFrom" );
			}

			this._repository = new SerializerTypeKeyRepository( copiedFrom._repository );
		}

		private SerializerRepository( Dictionary<RuntimeTypeHandle, object> table )
		{
			this._repository = new SerializerTypeKeyRepository( table );
			this._repository.Freeze();
		}

		/// <summary>
		///		This method does not perform any operation.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_repository" )]
		[Obsolete( "This class should not be disposable, so IDisposable will be removed in future." )]
		public void Dispose()
		{
			// nop
		}

		/// <summary>
		///		Gets the registered <see cref="MessagePackSerializer{T}"/> from this repository without provider parameter.
		/// </summary>
		/// <typeparam name="T">Type of the object to be marshaled/unmarshaled.</typeparam>
		/// <param name="context">A serialization context.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>. If no appropriate mashalers has benn registered, then <c>null</c>.
		/// </returns>
		public MessagePackSerializer<T> Get<T>( SerializationContext context )
		{
			return Get<T>( context, null );
		}

		/// <summary>
		///		Gets the registered <see cref="MessagePackSerializer{T}"/> from this repository with specified provider parameter.
		/// </summary>
		/// <typeparam name="T">Type of the object to be marshaled/unmarshaled.</typeparam>
		/// <param name="context">A serialization context.</param>
		/// <param name="providerParameter">A provider specific parameter. See remarks section of <see cref="SerializationContext.GetSerializer{T}(Object)"/> for details.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>. If no appropriate mashalers has benn registered, then <c>null</c>.
		/// </returns>
		/// <see cref="SerializationContext.GetSerializer{T}(Object)"/>
		public MessagePackSerializer<T> Get<T>( SerializationContext context, object providerParameter )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			var result = this._repository.Get( context, typeof( T ) );
			var asProvider = result as MessagePackSerializerProvider;
			return ( asProvider != null ? asProvider.Get( context, providerParameter ) : result ) as MessagePackSerializer<T>;
		}

#if UNITY || XAMIOS || XAMDROID
		internal IMessagePackSingleObjectSerializer Get( SerializationContext context, Type targetType, object providerParameter )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

			var result = this._repository.Get( context, targetType );
			var asProvider = result as MessagePackSerializerProvider;
			return ( asProvider != null ? asProvider.Get( context, providerParameter ) : result ) as IMessagePackSingleObjectSerializer;
		}
#endif // UNITY || XAMIOS || XAMDROID

		/// <summary>
		///		Registers a <see cref="MessagePackSerializer{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of serialization target.</typeparam>
		/// <param name="serializer"><see cref="MessagePackSerializer{T}"/> instance.</param>
		/// <returns>
		///		<c>true</c> if success to register; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="serializer"/> is <c>null</c>.
		/// </exception>
		public bool Register<T>( MessagePackSerializer<T> serializer )
		{
			return this.Register( typeof( T ), serializer );
		}

		internal bool Register( Type targetType, object serializer )
		{
			if ( serializer == null )
			{
				throw new ArgumentNullException( "serializer" );
			}

			var asEnumSerializer = serializer as ICustomizableEnumSerializer;
			// ReSharper disable RedundantIfElseBlock
			if ( asEnumSerializer != null )
			{
				return this._repository.Register( targetType, new EnumMessagePackSerializerProvider( targetType, asEnumSerializer ), /*allowOverwrite:*/ false );
			}
			else
			{
				return this._repository.Register( targetType, serializer, /*allowOverwrite:*/ false );
			}
			// ReSharper restore RedundantIfElseBlock
		}

		/// <summary>
		///		Registers a <see cref="MessagePackSerializer{T}"/> forcibley.
		/// </summary>
		/// <typeparam name="T">The type of serialization target.</typeparam>
		/// <param name="serializer"><see cref="MessagePackSerializer{T}"/> instance.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="serializer"/> is <c>null</c>.
		/// </exception>
		public void RegisterOverride<T>( MessagePackSerializer<T> serializer )
		{
			if ( serializer == null )
			{
				throw new ArgumentNullException( "serializer" );
			}

			this._repository.Register( typeof( T ), serializer, /*allowOverwrite:*/ true );
		}

		/// <summary>
		///		Gets the system default repository bound to default context.
		/// </summary>
		/// <value>
		///		The system default repository.
		///		This value will not be <c>null</c>.
		///		Note that the repository is frozen.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Historical reason" )]
		public static SerializerRepository Default
		{
			get { return GetDefault( PackerCompatibilityOptions.Classic ); }
		}

		/// <summary>
		///		Gets the system default repository bound to default context.
		/// </summary>
		/// <param name="packerCompatibilityOptions"><see cref="PackerCompatibilityOptions"/> for default serializers must use.</param>
		/// <returns>
		///		The system default repository.
		///		This value will not be <c>null</c>.
		///		Note that the repository is frozen.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="packerCompatibilityOptions"/> is invalid.</exception>
		public static SerializerRepository GetDefault( PackerCompatibilityOptions packerCompatibilityOptions )
		{
			var newContext = new SerializationContext( SerializationContext.Default.Serializers, packerCompatibilityOptions );
			return GetDefault( newContext );
		}

		/// <summary>
		///		Gets the system default repository bound for specified context.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which will be bound to default serializers.</param>
		/// <exception cref="ArgumentNullException"><paramref name="ownerContext"/> is <c>null</c>.</exception>
		/// <returns>
		///		The system default repository.
		///		This value will not be <c>null</c>.
		///		Note that the repository is frozen.
		/// </returns>
		public static SerializerRepository GetDefault( SerializationContext ownerContext )
		{
			if ( ownerContext == null )
			{
				throw new ArgumentNullException( "ownerContext" );
			}

			return new SerializerRepository( InitializeDefaultTable( ownerContext ) );
		}

		internal bool Contains( Type rootType )
		{
			return this._repository.Coontains( rootType );
		}
	}
}