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
using System.Diagnostics.Contracts;
#if !NETFX_CORE
using MsgPack.Serialization.EmittingSerializers;
#endif
#if !WINDOWS_PHONE
using MsgPack.Serialization.ExpressionSerializers;
#endif

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines entry points for <see cref="MessagePackSerializer{T}"/> usage.
	/// </summary>
	public static class MessagePackSerializer
	{
		/// <summary>
		///		Creates new <see cref="MessagePackSerializer{T}"/> instance with <see cref="SerializationContext.Default"/>.
		/// </summary>
		/// <typeparam name="T">Target type.</typeparam>
		/// <returns>
		///		New <see cref="MessagePackSerializer{T}"/> instance to serialize/deserialize the object tree which the top is <typeparamref name="T"/>.
		/// </returns>
		public static MessagePackSerializer<T> Create<T>()
		{
			return MessagePackSerializer.Create<T>( SerializationContext.Default );
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

			Func<SerializationContext, SerializerBuilder<T>> builderProvider;
#if NETFX_CORE
			builderProvider = c => new ExpressionSerializerBuilder<T>( c );
#else
#if !WINDOWS_PHONE
			if ( context.EmitterFlavor == EmitterFlavor.ExpressionBased )
			{
				builderProvider = c => new ExpressionSerializerBuilder<T>( c );
			}
			else
			{
#endif // !WINDOWS_PHONE
				if ( context.SerializationMethod == SerializationMethod.Map )
				{
					builderProvider = c => new MapEmittingSerializerBuilder<T>( c );
				}
				else
				{
					builderProvider = c => new ArrayEmittingSerializerBuilder<T>( c );
				}
#if !WINDOWS_PHONE
			}
#endif // !WINDOWS_PHONE
#endif // NETFX_CORE else

			return new AutoMessagePackSerializer<T>( context, builderProvider );
		}

	}
}
