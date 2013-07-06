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
using System.Diagnostics.Contracts;
#if NETFX_CORE
using System.Linq;
#endif
using System.Linq.Expressions;
#if NETFX_CORE
using System.Reflection;
#endif
using MsgPack.Serialization;

namespace MsgPack
{
	/// <summary>
	///		Defines extension method to pack or unpack various objects.
	/// </summary>
	public static class PackerUnpackerExtensions
	{
		private static readonly Type[] _messagePackSerializer_Create_ParameterTypes = new[] { typeof( SerializationContext ) };

		/// <summary>
		///		Packs specified value with the default context.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static void Pack<T>( this Packer source, T value )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			PackCore( source, value, SerializationContext.Default );
		}

		/// <summary>
		///		Packs specified value with the specified context.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static void Pack<T>( this Packer source, T value, SerializationContext context )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			Contract.EndContractBlock();

			PackCore( source, value, context );
		}

		private static void PackCore<T>( Packer source, T value, SerializationContext context )
		{
			if ( value == null )
			{
				source.PackNull();
				return;
			}

			if ( typeof( T ) != typeof( MessagePackObject ) && typeof( IPackable ).IsAssignableFrom( typeof( T ) ) )
			{
				( value as IPackable ).PackToMessage( source, new PackingOptions() );
				return;
			}

			MessagePackSerializer.Create<T>( context ).PackTo( source, value );
		}

		/// <summary>
		///		Packs specified value with the default context.
		/// </summary>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static void PackObject( this Packer source, object value )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			PackObjectCore( source, value, new SerializationContext() );
		}

		/// <summary>
		///		Packs specified value with the specified context.
		/// </summary>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static void PackObject( this Packer source, object value, SerializationContext context )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			Contract.EndContractBlock();

			PackObjectCore( source, value, context );
		}

		private static void PackObjectCore( Packer source, object value, SerializationContext context )
		{
			/*
			 * MessagePackSerializer.Create<T>( context ).PackTo( source, value );
			 */

			if ( value == null )
			{
				source.PackNull();
				return;
			}

			var type = value.GetType();
			var contextParameter = Expression.Parameter( typeof( SerializationContext ), "context" );
			var packerParameter = Expression.Parameter( typeof( Packer ), "packer" );
			var valueParameter = Expression.Parameter( typeof( object ), "value" );

			Expression.Lambda<Action<SerializationContext, Packer, object>>(
				Expression.Call(
					Expression.Call(
						typeof( MessagePackSerializer ).GetMethod( "Create", _messagePackSerializer_Create_ParameterTypes ).MakeGenericMethod( type ),
						contextParameter
					),
					typeof( MessagePackSerializer<> ).MakeGenericType( type ).GetMethod( "PackTo" ),
					packerParameter,
					Expression.Convert(
						valueParameter,
						type
					)
				),
				contextParameter,
				packerParameter,
				valueParameter
			).Compile()(context, source, value );
		}

		/// <summary>
		///		Unpacks specified type value with the default context.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="source">The <see cref="Unpacker"/>.</param>
		/// <returns>The deserialized value.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot deserialize <typeparamref name="T"/> value.
		/// </exception>
		public static T Unpack<T>( this Unpacker source )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return UnpackCore<T>( source, new SerializationContext() );
		}

		/// <summary>
		///		Unpacks specified type value with the specified context.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="source">The <see cref="Unpacker"/>.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns>The deserialized value.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot deserialize <typeparamref name="T"/> value.
		/// </exception>
		public static T Unpack<T>( this Unpacker source, SerializationContext context )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			Contract.EndContractBlock();

			return UnpackCore<T>( source, context );
		}

		private static T UnpackCore<T>( Unpacker source, SerializationContext context )
		{
			return MessagePackSerializer.Create<T>( context ).UnpackFrom( source );
		}
	}
}
