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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq.Expressions;

namespace MsgPack.Serialization
{
	internal static class Closures
	{
		public static Func<T> Construct<T>()
		{
			Contract.Assert( !typeof( T ).IsArray );

			if ( typeof( T ).IsValueType )
			{
				return () => default( T );
			}

			var ctor = typeof( T ).GetConstructor( Type.EmptyTypes );
			if ( ctor == null )
			{
				throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructor( typeof( T ) );
			}

			return Expression.Lambda<Func<T>>( Expression.New( ctor ) ).Compile();
		}

		public static Func<uint, T> CreateArray<T>()
		{
			Contract.Assert( typeof( T ).IsArray );
			var length = Expression.Parameter( typeof( uint ), "length" );
			return Expression.Lambda<Func<uint, T>>( Expression.NewArrayBounds( typeof( T ).GetElementType(), length ), length ).Compile();
		}

		public static Action<Packer, T, SerializationContext> Pack<T>( Action<Packer, T> target )
		{
			return ( packer, value, context ) => target( packer, value );
		}

		public static Action<Packer, T, SerializationContext> PackObject<T>( SerlializingMember[] entries, Action<Packer, T, SerializationContext>[] packings )
		{
			return
				( packer, target, context ) =>
				{
					// TODO: Array for ordered.
					packer.PackMapHeader( entries.Length );
					for ( int i = 0; i < entries.Length; i++ )
					{
						packer.PackString( entries[ i ].Contract.Name );
						packings[ i ]( packer, target, context );
					}
				};
		}

		public static Func<Unpacker, SerializationContext, T> Unpack<T>( Func<Unpacker, T> target )
		{
			return ( unpacker, context ) => target( unpacker );
		}

		public static Func<Unpacker, SerializationContext, T> Unpack<T>( Func<MessagePackObject, T> target )
		{
			if ( typeof( T ).IsValueType && !( typeof( T ).IsGenericType && typeof( T ).GetGenericTypeDefinition() == typeof( Nullable<> ) ) )
			{
				return
					( unpacker, context ) =>
					{
						if ( !unpacker.Read() )
						{
							throw SerializationExceptions.NewUnexpectedEndOfStream();
						}

						if ( unpacker.Data.Value.IsNil )
						{
							throw SerializationExceptions.NewValiueTypeCannotBeNull( typeof( T ) );
						}

						return target( unpacker.Data.Value );
					};
			}
			else
			{
				return
					( unpacker, context ) =>
					{
						if ( !unpacker.Read() )
						{
							throw SerializationExceptions.NewUnexpectedEndOfStream();
						}

						if ( unpacker.Data.Value.IsNil )
						{
							return default( T );
						}

						return target( unpacker.Data.Value );
					};
			}
		}

		public static Func<Unpacker, SerializationContext, T> UnpackObject<T>( SerlializingMember[] entries, Action<Unpacker, T, SerializationContext>[] unpackings )
		{
			var ctor = Construct<T>();
			return
				( unpacker, context ) =>
				{
					T target = ctor();

					using ( var subTreeUnpacker = unpacker.ReadSubtree() )
					{
						// TODO: Array for ordered.
						long count = subTreeUnpacker.ItemsCount;
						// TODO: For big struct, use Dictionary<String,SM>
						for ( long i = 0; i < count; i++ )
						{
							if ( !subTreeUnpacker.Read() )
							{
								throw new InvalidMessagePackStreamException( String.Format( CultureInfo.CurrentCulture, "Some map entries are missing. Declared size is {0}, but actual is {1}.", count, i ) );
							}

							string memberName = subTreeUnpacker.Data.Value.AsString();
							int index = Array.FindIndex( entries, entry => entry.Contract.Name == memberName );
							if ( index < 0 )
							{
								// TODO: OK?
								// Ignore
								continue;
							}

							if ( !subTreeUnpacker.Read() )
							{
								// TODO: message fix
								throw new InvalidMessagePackStreamException( String.Format( CultureInfo.CurrentCulture, "Some map entries are missing. Declared size is {0}, but actual is {1}.", count, i ) );
							}

							if ( subTreeUnpacker.IsArrayHeader || subTreeUnpacker.IsMapHeader )
							{
								using ( var subSubTreeUnpacker = subTreeUnpacker.ReadSubtree() )
								{
									unpackings[ index ]( subSubTreeUnpacker, target, context );
								}
							}
							else
							{
								unpackings[ index ]( subTreeUnpacker, target, context );
							}
						}
					}

					return target;
				};
		}

		public static Func<Unpacker, SerializationContext, T> UnpackWithForwarding<T>( Func<Unpacker, T> target )
		{
			return
				( unpacker, context ) =>
				{
					return target( unpacker );
				};
		}

		public static Func<Unpacker, SerializationContext, T> UnpackWithForwarding<T>( Action<Unpacker, T, SerializationContext> target )
		{
			if ( typeof( T ).IsArray )
			{
				var ctor = CreateArray<T>();
				return
					( unpacker, context ) =>
					{
						var array = ctor( unpacker.Data.Value.AsUInt32() );
						target( unpacker, array, context );
						return array;
					};
			}
			else
			{
				var ctor = Construct<T>();
				return
					( unpacker, context ) =>
					{
						var collection = ctor();
						target( unpacker, collection, context );
						return collection;
					};
			}
		}
	}
}
