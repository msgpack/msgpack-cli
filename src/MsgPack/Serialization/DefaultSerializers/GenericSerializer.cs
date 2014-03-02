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
using System.Diagnostics.Contracts;

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		Defines serializer factory for well known structured types.
	/// </summary>
	internal static class GenericSerializer
	{
		public static MessagePackSerializer<T> CreateArraySerializer<T>( SerializationContext context )
		{
#if DEBUG
			Contract.Assert( typeof( T ).IsArray );
#endif // if DEBUG
			return ArraySerializer.Create<T>( context );
		}

		public static MessagePackSerializer<T> CreateNullableSerializer<T>( SerializationContext context )
		{
#if DEBUG
			Contract.Assert( Nullable.GetUnderlyingType( typeof( T ) ) != null );
#endif // if DEBUG
#if XAMIOS || UNIOS
#warning TODO: IMPL
			throw new NotImplementedException("Nullable<T> has not been supported in iOS yet.");
#else
			return new NullableMessagePackSerializer<T>( context );
#endif // if XAMIOS || UNIOS
		}

		public static MessagePackSerializer<T> CreateEnumSerializer<T>( SerializationContext context )
		{
#if DEBUG
			Contract.Assert( typeof( T ).GetIsEnum() );
#endif // if DEBUG
			return
				new EnumMessagePackSerializer<T>(
					( context ?? SerializationContext.Default ).CompatibilityOptions.PackerCompatibilityOptions
				);
		}

		public static MessagePackSerializer<T> TryCreateImmutableCollectionSerializer<T>( SerializationContext context )
		{
#if NETFX_35 || NETFX_40 || SILVERLIGHT
			// ImmutableCollections does not support above platforms.
			return null;
#else
			if ( typeof( T ).Namespace != "System.Collections.Immutable" )
			{
				return null;
			}

			if ( !typeof( T ).GetIsGenericType() )
			{
				return null;
			}

			switch ( typeof( T ).GetGenericTypeDefinition().Name )
			{
				case "ImmutableList`1":
				case "ImmutableHashSet`1":
				case "ImmutableSortedSet`1":
				case "ImmutableQueue`1":
				{
					return
						Activator.CreateInstance(
							typeof( ImmutableCollectionSerializer<,> )
							.MakeGenericType( typeof( T ), typeof( T ).GetGenericArguments()[ 0 ] ),
							context
						) as MessagePackSerializer<T>;
				}
				case "ImmutableStack`1":
				{
					return
						Activator.CreateInstance(
							typeof( ImmutableStackSerializer<,> )
							.MakeGenericType( typeof( T ), typeof( T ).GetGenericArguments()[ 0 ] ),
							context
						) as MessagePackSerializer<T>;
				}
				case "ImmutableDictionary`2":
				case "ImmutableSortedDictionary`2":
				{
					return
						Activator.CreateInstance(
							typeof( ImmutableDictionarySerializer<,,> )
							.MakeGenericType( typeof( T ), typeof( T ).GetGenericArguments()[ 0 ], typeof( T ).GetGenericArguments()[ 1 ] ),
							context
						) as MessagePackSerializer<T>;
				}
				default:
				{
#if DEBUG
					Contract.Assert( false, "Unknown type:" + typeof( T ) );
#endif
// ReSharper disable HeuristicUnreachableCode
					return null;
// ReSharper restore HeuristicUnreachableCode
				}
			}
#endif
		}
	}
}
