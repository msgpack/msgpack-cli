#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
#if !NETFX_CORE
using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.EmittingSerializers;
#endif
#if !WINDOWS_PHONE && !NETFX_35
using MsgPack.Serialization.ExpressionSerializers;
#endif

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines entry points for <see cref="MessagePackSerializer{T}"/> usage.
	/// </summary>
	public static class MessagePackSerializer
	{
#if DEBUG
		private static System.Collections.Generic.HashSet<Type> _infiniteRecursiveCallDetector =
			new HashSet<Type>();
#endif
		/// <summary>
		///		Creates new <see cref="MessagePackSerializer{T}"/> instance with <see cref="SerializationContext.Default"/>.
		/// </summary>
		/// <typeparam name="T">Target type.</typeparam>
		/// <returns>
		///		New <see cref="MessagePackSerializer{T}"/> instance to serialize/deserialize the object tree which the top is <typeparamref name="T"/>.
		/// </returns>
		public static MessagePackSerializer<T> Create<T>()
		{
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );

			return Create<T>( SerializationContext.Default );
		}

		/// <summary>
		///		Creates new <see cref="MessagePackSerializer{T}"/> instance with specified <see cref="SerializationContext"/>.
		/// </summary>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="context">
		///		<see cref="SerializationContext"/> to store known/created serializers.
		/// </param>
		/// <returns>
		///		New <see cref="MessagePackSerializer{T}"/> instance to serialize/deserialize the object tree which the top is <typeparamref name="T"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="context"/> is <c>null</c>.
		/// </exception>
		public static MessagePackSerializer<T> Create<T>( SerializationContext context )
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );

			if ( context.ContainsSerializer( typeof( T ) ) )
			{
#if DEBUG
				if ( _infiniteRecursiveCallDetector == null )
				{
					_infiniteRecursiveCallDetector = new HashSet<Type>();
				}
				
				if ( _infiniteRecursiveCallDetector.Contains( typeof( T ) ) )
				{
					throw new Exception( "Infite recursive call." );
				}
				else
				{
					_infiniteRecursiveCallDetector.Add( typeof( T ) );
				}
#endif
				var result = context.GetSerializer<T>();
#if DEBUG
				_infiniteRecursiveCallDetector.Remove( typeof( T ) );
#endif
				return result;
			}

			//Func<SerializationContext, SerializerBuilder<T>> builderProvider;
			ISerializerBuilder<T> builder;
#if NETFX_CORE
			builder = new ExpressionTreeSerializerBuilder<T>();
#else
#if !WINDOWS_PHONE && !NETFX_35
			if ( context.EmitterFlavor == EmitterFlavor.ExpressionBased )
			{
				builder = new ExpressionTreeSerializerBuilder<T>();
			}
			else
			{
#endif // !WINDOWS_PHONE && !NETFX_35
				if ( context.EmitterFlavor == EmitterFlavor.FieldBased )
				{
					builder = new AssemblyBuilderSerializerBuilder<T>();
				}
				else
				{
					builder = new DynamicMethodSerializerBuilder<T>();
				}

#if !WINDOWS_PHONE && !NETFX_35
			}
#endif // !WINDOWS_PHONE  && !NETFX_35
#endif // NETFX_CORE else

			return new AutoMessagePackSerializer<T>( context, builder );
		}

		/// <summary>
		///		Creates new <see cref="IMessagePackSerializer"/> instance with <see cref="SerializationContext.Default"/>.
		/// </summary>
		/// <param name="targetType">Target type.</param>
		/// <returns>
		///		New <see cref="IMessagePackSingleObjectSerializer"/> instance to serialize/deserialize the object tree which the top is <paramref name="targetType"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		To avoid boxing and strongly typed API is prefered, use <see cref="Create{T}()"/> instead when possible.
		/// </remarks>
		public static IMessagePackSingleObjectSerializer Create( Type targetType )
		{
			return Create( targetType, SerializationContext.Default );
		}

		/// <summary>
		///		Creates new <see cref="IMessagePackSerializer"/> instance with specified <see cref="SerializationContext"/>.
		/// </summary>
		/// <param name="targetType">Target type.</param>
		/// <param name="context">
		///		<see cref="SerializationContext"/> to store known/created serializers.
		/// </param>
		/// <returns>
		///		New <see cref="IMessagePackSingleObjectSerializer"/> instance to serialize/deserialize the object tree which the top is <paramref name="targetType"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="targetType"/> is <c>null</c>.
		///		Or, <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <remarks>
		///		To avoid boxing and strongly typed API is prefered, use <see cref="Create{T}(SerializationContext)"/> instead when possible.
		/// </remarks>
		public static IMessagePackSingleObjectSerializer Create( Type targetType, SerializationContext context )
		{
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			Contract.Ensures( Contract.Result<IMessagePackSerializer>() != null );
			return context.GetSerializer( targetType );
		}
	}
}
