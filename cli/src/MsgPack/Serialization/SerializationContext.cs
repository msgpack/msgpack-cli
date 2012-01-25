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
using System.Threading;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Represents serialization context information for internal serialization logic.
	/// </summary>
	public sealed class SerializationContext
	{
		private readonly SerializerRepository _serializers;
		private readonly HashSet<Type> _typeLock;

		/// <summary>
		///		Gets the current <see cref="SerializerRepository"/>.
		/// </summary>
		/// <value>
		///		The  current <see cref="SerializerRepository"/>.
		/// </value>
		public SerializerRepository Serializers
		{
			get { return this._serializers; }
		}

		private EmitterFlavor _emitterFlavor = EmitterFlavor.FieldBased;

		internal EmitterFlavor EmitterFlavor
		{
			get { return this._emitterFlavor; }
			set { this._emitterFlavor = value; }
		}

		private SerializationMethod _serializationMethod;

		public SerializationMethod SerializationMethod
		{
			get { return this._serializationMethod; }
			set { this._serializationMethod = value; }
		}

		private SerializationMethodGeneratorOption _generatorOption;

		public SerializationMethodGeneratorOption GeneratorOption
		{
			get { return this._generatorOption; }
			set { this._generatorOption = value; }
		}
		
		/// <summary>
		///		Initializes a new instance of the <see cref="SerializationContext"/> class with copy of <see cref="SerializerRepository.Default"/>.
		/// </summary>
		public SerializationContext()
			: this( new SerializerRepository( SerializerRepository.Default ) ) { }

		internal SerializationContext( SerializerRepository serializers )
		{
			this._serializers = serializers;
			this._typeLock = new HashSet<Type>();
		}

		/// <summary>
		///		Gets the <see cref="MessagePackSerializer{T}"/> with this instance.
		/// </summary>
		/// <typeparam name="T">Type of serialization/deserialization target.</typeparam>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <remarks>
		///		This method automatically register new instance via <see cref="M:SerializationRepository.Register{T}"/>.
		/// </remarks>
		public MessagePackSerializer<T> GetSerializer<T>()
		{
			var serializer = this._serializers.Get<T>( this );
			if ( serializer == null )
			{
				// TODO: Configurable
				if ( !this._typeLock.Add( typeof( T ) ) )
				{
					return new LazyDelegatingMessagePackSerializer<T>( this );
				}
				serializer = MessagePackSerializer.Create<T>( this );
				this._typeLock.Remove( typeof( T ) );
				if ( !this._serializers.Register<T>( serializer ) )
				{
					serializer = this._serializers.Get<T>( this );
				}
			}

			return serializer;
		}

		/// <summary>
		///		Gets the serializer for the specified <see cref="Type"/>.
		/// </summary>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <returns>
		///		<see cref="IMessagePackSerializer"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		Although <see cref="GetSerializer{T}"/> is preferred,
		///		this method can be used from non-generic type or methods.
		/// </remarks>
		public IMessagePackSerializer GetSerializer( Type targetType )
		{
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

			return SerializerGetter.Instance.Get( this, targetType );
		}

		private sealed class SerializerGetter
		{
			public static readonly SerializerGetter Instance = new SerializerGetter();

			private readonly Dictionary<RuntimeTypeHandle, Func<SerializationContext, IMessagePackSerializer>> _cache =
				new Dictionary<RuntimeTypeHandle, Func<SerializationContext, IMessagePackSerializer>>();

			private SerializerGetter() { }

			public IMessagePackSerializer Get( SerializationContext context, Type targetType )
			{
				Func<SerializationContext, IMessagePackSerializer> func;
				if ( !this._cache.TryGetValue( targetType.TypeHandle, out func ) || func == null )
				{
					func =
						Delegate.CreateDelegate(
							typeof( Func<SerializationContext, IMessagePackSerializer> ),
							typeof( SerializerGetter<> ).MakeGenericType( targetType ).GetMethod( "Get" )
						) as Func<SerializationContext, IMessagePackSerializer>;
					this._cache[ targetType.TypeHandle ] = func;
				}

				return func( context );
			}
		}

		private static class SerializerGetter<T>
		{
			private static readonly Func<SerializationContext, MessagePackSerializer<T>> _func =
				Delegate.CreateDelegate(
					typeof( Func<SerializationContext, MessagePackSerializer<T>> ),
					Metadata._SerializationContext.GetSerializer1_Method.MakeGenericMethod( typeof( T ) )
				) as Func<SerializationContext, MessagePackSerializer<T>>;

			public static IMessagePackSerializer Get( SerializationContext context )
			{
				return _func( context );
			}
		}
	}
}
