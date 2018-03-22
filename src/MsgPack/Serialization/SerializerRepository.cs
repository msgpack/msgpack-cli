#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2018 FUJIWARA, Yusuke
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
using System.Collections.Generic;
using System.Linq;

using MsgPack.Serialization.DefaultSerializers;
using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Repository of known <see cref="MessagePackSerializer{T}"/>s.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Class coupling caused by default serializers registration." )]
	public sealed partial class SerializerRepository : IDisposable
	{
		private static readonly object SyncRoot = new object();
		private static SerializerRepository _internalDefault;

		internal static SerializerRepository InternalDefault
		{
			get
			{
				// Lazy init to avoid .cctor recursion from SerializationContext.cctor()
				lock ( SyncRoot )
				{
					if ( _internalDefault == null )
					{
						_internalDefault = GetDefault( SerializationContext.Default );
					}

					return _internalDefault;
				}
			}
		}

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
			return this.Get<T>( context, null );
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
#if !UNITY
			return ( asProvider != null ? asProvider.Get( context, providerParameter ) : result ) as MessagePackSerializer<T>;
#else
			var asSerializer =
				( asProvider != null ? asProvider.Get( context, providerParameter ) : result ) as MessagePackSerializer;
			return asSerializer != null ? MessagePackSerializer.Wrap<T>( context, asSerializer ) : null;
#endif // !UNITY
		}

#if UNITY
		internal MessagePackSerializer Get( SerializationContext context, Type targetType, object providerParameter )
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
			return ( asProvider != null ? asProvider.Get( context, providerParameter ) : result ) as MessagePackSerializer;
		}
#endif // UNITY

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
		/// <remarks>
		///		This method invokes <see cref="Register{T}(MsgPack.Serialization.MessagePackSerializer{T},SerializerRegistrationOptions)"/> with <see cref="SerializerRegistrationOptions.None"/>.
		///		<note>
		///			If you register serializer for value type, using <see cref="SerializerRegistrationOptions.WithNullable"/> is recommended because auto-generated deserializers use them to handle nil value.
		///			You can use <see cref="Register{T}(MsgPack.Serialization.MessagePackSerializer{T},SerializerRegistrationOptions)"/> with <see cref="SerializerRegistrationOptions.WithNullable"/> to
		///			get equivalant behavior for this method with registering nullable serializer automatically.
		///		</note>
		/// </remarks>
		public bool Register<T>( MessagePackSerializer<T> serializer )
		{
			return this.Register( serializer, SerializerRegistrationOptions.None );
		}

		/// <summary>
		///		Registers a <see cref="MessagePackSerializer{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of serialization target.</typeparam>
		/// <param name="serializer"><see cref="MessagePackSerializer{T}"/> instance.</param>
		/// <param name="options">A <see cref="SerializerRegistrationOptions"/> to control this registration process.</param>
		/// <returns>
		///		<c>true</c> if success to register; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="serializer"/> is <c>null</c>.
		/// </exception>
		public bool Register<T>( MessagePackSerializer<T> serializer, SerializerRegistrationOptions options )
		{
			if ( serializer == null )
			{
				throw new ArgumentNullException( "serializer" );
			}

			Type nullableType = null;
			MessagePackSerializerProvider nullableSerializerProvider = null;
#if !UNITY
			if ( ( options & SerializerRegistrationOptions.WithNullable ) != 0 )
			{
				GetNullableCompanion( typeof( T ), serializer.OwnerContext, serializer, out nullableType, out nullableSerializerProvider );
			}
#endif // !UNITY
#if !UNITY
			return this.Register( typeof( T ), new PolymorphicSerializerProvider<T>( serializer ), nullableType, nullableSerializerProvider, options );
#else
			return this.Register( typeof( T ), new PolymorphicSerializerProvider<T>(  serializer.OwnerContext, serializer ), nullableType, nullableSerializerProvider, options );
#endif // !UNITY
		}

#if !UNITY
		internal static void GetNullableCompanion(
			Type targetType,
			SerializationContext context,
			object serializer,
			out Type nullableType,
			out MessagePackSerializerProvider nullableSerializerProvider 
		)
		{
			if ( targetType.GetIsValueType() && Nullable.GetUnderlyingType( targetType ) == null )
			{
				nullableType = typeof( Nullable<> ).MakeGenericType( targetType );
				var nullableCtor =
					typeof( NullableMessagePackSerializer<> ).MakeGenericType( targetType ).GetConstructor(
						new[]
						{
							typeof( SerializationContext ),
							typeof( MessagePackSerializer<> ).MakeGenericType( targetType )
						}
					);

				nullableSerializerProvider =
					( MessagePackSerializerProvider ) ReflectionExtensions.CreateInstancePreservingExceptionType(
						typeof( PolymorphicSerializerProvider<> ).MakeGenericType( nullableType ),
						nullableCtor.InvokePreservingExceptionType(
							context,
							serializer
						)
					);
			}
			else
			{
				nullableType = null;
				nullableSerializerProvider = null;
			}
		}
#endif // !UNITY

#if UNITY && DEBUG
		public
#else
		internal
#endif
		bool Register( Type targetType, MessagePackSerializerProvider serializerProvider, Type nullableType, MessagePackSerializerProvider nullableSerializerProvider, SerializerRegistrationOptions options )
		{
			return this._repository.Register( targetType, serializerProvider, nullableType, nullableSerializerProvider, options );
		}

		/// <summary>
		///		Registers a <see cref="MessagePackSerializer{T}"/> forcibley.
		/// </summary>
		/// <typeparam name="T">The type of serialization target.</typeparam>
		/// <param name="serializer"><see cref="MessagePackSerializer{T}"/> instance.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="serializer"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		This method invokes <see cref="Register{T}(MsgPack.Serialization.MessagePackSerializer{T},SerializerRegistrationOptions)"/> with <see cref="SerializerRegistrationOptions.AllowOverride"/>.
		///		<note>
		///			If you register serializer for value type, using <see cref="SerializerRegistrationOptions.WithNullable"/> is recommended because auto-generated deserializers use them to handle nil value.
		///			You can use <see cref="Register{T}(MsgPack.Serialization.MessagePackSerializer{T},SerializerRegistrationOptions)"/> 
		///			with <see cref="SerializerRegistrationOptions.AllowOverride"/> and <see cref="SerializerRegistrationOptions.WithNullable"/> to
		///			get equivalant behavior for this method with registering nullable serializer automatically.
		///		</note>
		/// </remarks>
		public void RegisterOverride<T>( MessagePackSerializer<T> serializer )
		{
			this.Register( serializer, SerializerRegistrationOptions.AllowOverride );
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
		[Obsolete( "Use GetDefault()")]
		public static SerializerRepository Default
		{
			get { return GetDefault( SerializationContext.Default ); }
		}

		/// <summary>
		///		Gets the system default repository bound to default context.
		/// </summary>
		/// <returns>
		///		The system default repository.
		///		This value will not be <c>null</c>.
		///		Note that the repository is frozen.
		/// </returns>
		public static SerializerRepository GetDefault()
		{
			return GetDefault( SerializationContext.Default );
		}

		/// <summary>
		///		Gets the system default repository bound to default context.
		/// </summary>
		/// <param name="packerCompatibilityOptions">Not used.</param>
		/// <returns>
		///		The system default repository.
		///		This value will not be <c>null</c>.
		///		Note that the repository is frozen.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "packerCompatibilityOptions", Justification = "Historical reason" )]
		[Obsolete( "Use GetDefault()" )]
		public static SerializerRepository GetDefault( PackerCompatibilityOptions packerCompatibilityOptions )
		{
			return GetDefault( SerializationContext.Default );
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

		/// <summary>
		///		Determines whether this repository contains serializer for the specified target type.
		/// </summary>
		/// <param name="targetType">Type of the target.</param>
		/// <returns>
		///		<c>true</c> if this repository contains serializer for the specified target type; otherwise, <c>false</c>.
		///		This method returns <c>false</c> for <c>null</c>.
		/// </returns>
		public bool ContainsFor( Type targetType )
		{
			return this._repository.Contains( targetType );
		}

		/// <summary>
		///		Gets the copy of registered serializer entries.
		/// </summary>
		/// <returns>
		///		The copy of registered serializer entries.
		///		This value will not be <c>null</c> and consistent in the invoked timing.
		/// </returns>
		/// <remarks>
		///		This method returns snapshot of the invoked timing, so the result may not reflect latest status.
		///		You should use the result for debugging or tooling purpose only.
		///		Use <c>Get()</c> overloads to get proper serializer.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This method causes collection copying." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design" )]
		public IEnumerable<KeyValuePair<Type, MessagePackSerializer>> GetRegisteredSerializers()
		{
			return
				this._repository.GetEntries()
					.Select( kv => new KeyValuePair<Type, MessagePackSerializer>( kv.Key, kv.Value as MessagePackSerializer ) )
					.Where( kV => kV.Value != null );
		}
	}
}
