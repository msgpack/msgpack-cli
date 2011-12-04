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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace MsgPack
{
	partial class Packer
	{
		private static readonly Dictionary<RuntimeTypeHandle, Action< Packer, object>> _packDispatchTable =
			new Dictionary<RuntimeTypeHandle, Action< Packer, object>>( 11 * 2 + 3 )
			{
				{ typeof( byte[] ).TypeHandle, ( packer, value ) => packer.PrivatePackRawCore( value as byte[], false ) },
				{ typeof( string ).TypeHandle, ( packer, value ) => packer.PrivatePackStringCore( value as string, Encoding.UTF8 ) },
				{ typeof( MessagePackString ).TypeHandle, ( packer, value ) => packer.PrivatePackRawCore( ( value as MessagePackString ).GetBytes(), true ) },
				{ typeof( System.Boolean ).TypeHandle, ( packer, value ) => packer.PrivatePackCore( ( System.Boolean )value ) },
				{ typeof( System.Boolean? ).TypeHandle, ( packer, value ) => packer.PrivatePackCore( ( System.Boolean )value ) },
				{ 
					typeof( System.SByte ).TypeHandle, 
					( packer, value ) => 
					{
						packer.PrivatePackCore( ( System.SByte )value );
					}
				},
				{ 
					typeof( System.SByte? ).TypeHandle, 
					( packer, value ) =>
					{
						packer.PrivatePackCore( ( System.SByte? )value );
					}
				},
				{ 
					typeof( System.Int16 ).TypeHandle, 
					( packer, value ) => 
					{
						packer.PrivatePackCore( ( System.Int16 )value );
					}
				},
				{ 
					typeof( System.Int16? ).TypeHandle, 
					( packer, value ) =>
					{
						packer.PrivatePackCore( ( System.Int16? )value );
					}
				},
				{ 
					typeof( System.Int32 ).TypeHandle, 
					( packer, value ) => 
					{
						packer.PrivatePackCore( ( System.Int32 )value );
					}
				},
				{ 
					typeof( System.Int32? ).TypeHandle, 
					( packer, value ) =>
					{
						packer.PrivatePackCore( ( System.Int32? )value );
					}
				},
				{ 
					typeof( System.Int64 ).TypeHandle, 
					( packer, value ) => 
					{
						packer.PrivatePackCore( ( System.Int64 )value );
					}
				},
				{ 
					typeof( System.Int64? ).TypeHandle, 
					( packer, value ) =>
					{
						packer.PrivatePackCore( ( System.Int64? )value );
					}
				},
				{ 
					typeof( System.Byte ).TypeHandle, 
					( packer, value ) => 
					{
						packer.PrivatePackCore( ( System.Byte )value );
					}
				},
				{ 
					typeof( System.Byte? ).TypeHandle, 
					( packer, value ) =>
					{
						packer.PrivatePackCore( ( System.Byte? )value );
					}
				},
				{ 
					typeof( System.UInt16 ).TypeHandle, 
					( packer, value ) => 
					{
						packer.PrivatePackCore( ( System.UInt16 )value );
					}
				},
				{ 
					typeof( System.UInt16? ).TypeHandle, 
					( packer, value ) =>
					{
						packer.PrivatePackCore( ( System.UInt16? )value );
					}
				},
				{ 
					typeof( System.UInt32 ).TypeHandle, 
					( packer, value ) => 
					{
						packer.PrivatePackCore( ( System.UInt32 )value );
					}
				},
				{ 
					typeof( System.UInt32? ).TypeHandle, 
					( packer, value ) =>
					{
						packer.PrivatePackCore( ( System.UInt32? )value );
					}
				},
				{ 
					typeof( System.UInt64 ).TypeHandle, 
					( packer, value ) => 
					{
						packer.PrivatePackCore( ( System.UInt64 )value );
					}
				},
				{ 
					typeof( System.UInt64? ).TypeHandle, 
					( packer, value ) =>
					{
						packer.PrivatePackCore( ( System.UInt64? )value );
					}
				},
				{ typeof( System.Single ).TypeHandle, ( packer, value ) => packer.PrivatePackCore( ( System.Single )value ) },
				{ typeof( System.Single? ).TypeHandle, ( packer, value ) => packer.PrivatePackCore( ( System.Single )value ) },
				{ typeof( System.Double ).TypeHandle, ( packer, value ) => packer.PrivatePackCore( ( System.Double )value ) },
				{ typeof( System.Double? ).TypeHandle, ( packer, value ) => packer.PrivatePackCore( ( System.Double )value ) },
			};

		/// <summary>
		///		Pack specified <see cref="Object"/> as apporipriate value.
		/// </summary>
		/// <param name="boxedValue">Boxed value.</param>
		/// <param name="options">Options.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="MessageTypeException">There is no approptiate MessagePack type to represent specified object.</exception>
		public Packer PackObject( object boxedValue, PackingOptions options )
		{
			this.VerifyNotDisposed();
			this.PrivatePackObject( boxedValue, options );
			return this;
		}
		
		private void PrivatePackObject( object boxedValue, PackingOptions options )
		{
			if ( boxedValue == null )
			{
				this.PrivatePackNullCore();
				return;
			}
			
			var asPackable = boxedValue as IPackable;
			if ( asPackable != null )
			{
				asPackable.PackToMessage( this, options );
				return;
			}
			
			Action< Packer, object> pack;
			if( _packDispatchTable.TryGetValue( boxedValue.GetType().TypeHandle, out pack ) )
			{
				pack( this, boxedValue );
				return;
			}
			
			var collectionType = ExtractCollectionType( boxedValue.GetType() );
			if ( collectionType != null )
			{
				ValuePacker.GetInstance( collectionType ).PackObject( this, boxedValue, options );
				return;
			}
			
			throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Unknown object '{0}'.", boxedValue.GetType() ) );
		}
		
		private abstract class ValuePacker
		{
			private static readonly object _syncRoot = new object();
			private static volatile Dictionary<RuntimeTypeHandle, ValuePacker> _cache = new Dictionary<RuntimeTypeHandle, ValuePacker>();
			
			public static ValuePacker GetInstance( Type targetType )
			{
				ValuePacker result;
				if ( !_cache.TryGetValue( targetType.TypeHandle, out result ) )
				{
					lock( _syncRoot )
					{
						if ( !_cache.TryGetValue( targetType.TypeHandle, out result ) )
						{
							var packerType = typeof( ValuePacker<> ).MakeGenericType( targetType );
							result = ( ValuePacker )packerType.GetField( "Instance" ).GetValue( null );
							// This line causes volatile read, so memory fence is leaded.
							_cache[ targetType.TypeHandle ] = result;
						}
					}
				}
				
				return result;
			}
			
			protected ValuePacker() { }
			
			public abstract void PackObject( Packer packer, object value, PackingOptions options );			
		}
		
		private abstract class ValuePacker<T> : ValuePacker
		{
			public static readonly ValuePacker<T> Instance = CreateInstance();
			
			private static ValuePacker<T> CreateInstance()
			{
				if ( typeof( T ) == typeof( MessagePackObject ) )
				{
					return Activator.CreateInstance( typeof( MessagePackObjectValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( IPackable ).IsAssignableFrom( typeof( T ) ) )
				{
					return Activator.CreateInstance( typeof( PackableValuePacker<> ).MakeGenericType( typeof( T ) ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( byte[] ) )
				{
					return Activator.CreateInstance( typeof( ByteArrayValuePacker ) ) as ValuePacker<T>;
				}
				else if( typeof( T ) == typeof( string ) )
				{
					return Activator.CreateInstance( typeof( StringValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Boolean ) )
				{
					return Activator.CreateInstance( typeof( BooleanValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Boolean? ) )
				{
					return Activator.CreateInstance( typeof( NullableBooleanValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.SByte ) )
				{
					return Activator.CreateInstance( typeof( SByteValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.SByte? ) )
				{
					return Activator.CreateInstance( typeof( NullableSByteValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Int16 ) )
				{
					return Activator.CreateInstance( typeof( Int16ValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Int16? ) )
				{
					return Activator.CreateInstance( typeof( NullableInt16ValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Int32 ) )
				{
					return Activator.CreateInstance( typeof( Int32ValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Int32? ) )
				{
					return Activator.CreateInstance( typeof( NullableInt32ValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Int64 ) )
				{
					return Activator.CreateInstance( typeof( Int64ValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Int64? ) )
				{
					return Activator.CreateInstance( typeof( NullableInt64ValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Byte ) )
				{
					return Activator.CreateInstance( typeof( ByteValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Byte? ) )
				{
					return Activator.CreateInstance( typeof( NullableByteValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.UInt16 ) )
				{
					return Activator.CreateInstance( typeof( UInt16ValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.UInt16? ) )
				{
					return Activator.CreateInstance( typeof( NullableUInt16ValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.UInt32 ) )
				{
					return Activator.CreateInstance( typeof( UInt32ValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.UInt32? ) )
				{
					return Activator.CreateInstance( typeof( NullableUInt32ValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.UInt64 ) )
				{
					return Activator.CreateInstance( typeof( UInt64ValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.UInt64? ) )
				{
					return Activator.CreateInstance( typeof( NullableUInt64ValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Single ) )
				{
					return Activator.CreateInstance( typeof( SingleValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Single? ) )
				{
					return Activator.CreateInstance( typeof( NullableSingleValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Double ) )
				{
					return Activator.CreateInstance( typeof( DoubleValuePacker ) ) as ValuePacker<T>;
				}
				else if ( typeof( T ) == typeof( System.Double? ) )
				{
					return Activator.CreateInstance( typeof( NullableDoubleValuePacker ) ) as ValuePacker<T>;
				}

				var collectionType = ExtractCollectionType( typeof( T ) );
				if ( collectionType != null )
				{
					if ( collectionType.IsGenericType )
					{
						var genericTypeDefinition = collectionType.GetGenericTypeDefinition();
						if ( genericTypeDefinition == typeof( IList<> ) )
						{
							return 
								Activator.CreateInstance( 
									typeof( CollectionValuePacker<> ).MakeGenericType( typeof( T ) ),
									Activator.CreateInstance( typeof( ArrayValuePacker<> ).MakeGenericType( collectionType.GetGenericArguments() ) )
								)as ValuePacker<T>;
						}
						else if ( genericTypeDefinition == typeof( IDictionary<,> ) )
						{
							return 
								Activator.CreateInstance( 
									typeof( CollectionValuePacker<> ).MakeGenericType( typeof( T ) ),
									Activator.CreateInstance( typeof( DictionaryValuePacker<,> ).MakeGenericType( collectionType.GetGenericArguments() ) ) 
								) as ValuePacker<T>;
						}
					}
					else
					{
						if ( collectionType == typeof( IList ) )
						{
							return
								Activator.CreateInstance( 
									typeof( CollectionValuePacker<> ).MakeGenericType( typeof( T ) ),
									Activator.CreateInstance( typeof( ArrayValuePacker ) )
								) as ValuePacker<T>;
						}
						else if ( collectionType == typeof( IDictionary ) )
						{
							return 
								Activator.CreateInstance( 
									typeof( CollectionValuePacker<> ).MakeGenericType( typeof( T ) ),
									Activator.CreateInstance( typeof( DictionaryValuePacker ) )
								) as ValuePacker<T>;
						}
					}
				}
				
				return new FallbackValuePacker<T>();
			}
			
			protected ValuePacker() { }
			
			public sealed override void PackObject( Packer packer, object value, PackingOptions options )
			{
				this.Pack( packer, ( T )value, options );
			}
			
			public abstract void Pack( Packer packer, T value, PackingOptions options );
		}
		
		private sealed class CollectionValuePacker<T> : ValuePacker<T>
			where T : class, IEnumerable
		{
			private readonly ValuePacker _realPacker;
			
			public CollectionValuePacker( ValuePacker realPacker )
			{
				this._realPacker = realPacker;
			}
			
			public sealed override void Pack( Packer packer, T value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );

				if ( value == null )
				{
					packer.PrivatePackNullCore();
					return;
				}
				
				this._realPacker.PackObject( packer, value, options );
			}
		}
		

		private sealed class BooleanValuePacker : ValuePacker< System.Boolean >
		{
			public BooleanValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Boolean value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.PrivatePackCore( value );
			}
		}
		
		private sealed class NullableBooleanValuePacker : ValuePacker< System.Boolean? >
		{
			public NullableBooleanValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Boolean? value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.Pack( value );
			}
		}

		private sealed class SByteValuePacker : ValuePacker< System.SByte >
		{
			public SByteValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.SByte value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.PrivatePackCore( value );
			}
		}
		
		private sealed class NullableSByteValuePacker : ValuePacker< System.SByte? >
		{
			public NullableSByteValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.SByte? value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.Pack( value );
			}
		}

		private sealed class Int16ValuePacker : ValuePacker< System.Int16 >
		{
			public Int16ValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Int16 value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.PrivatePackCore( value );
			}
		}
		
		private sealed class NullableInt16ValuePacker : ValuePacker< System.Int16? >
		{
			public NullableInt16ValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Int16? value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.Pack( value );
			}
		}

		private sealed class Int32ValuePacker : ValuePacker< System.Int32 >
		{
			public Int32ValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Int32 value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.PrivatePackCore( value );
			}
		}
		
		private sealed class NullableInt32ValuePacker : ValuePacker< System.Int32? >
		{
			public NullableInt32ValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Int32? value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.Pack( value );
			}
		}

		private sealed class Int64ValuePacker : ValuePacker< System.Int64 >
		{
			public Int64ValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Int64 value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.PrivatePackCore( value );
			}
		}
		
		private sealed class NullableInt64ValuePacker : ValuePacker< System.Int64? >
		{
			public NullableInt64ValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Int64? value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.Pack( value );
			}
		}

		private sealed class ByteValuePacker : ValuePacker< System.Byte >
		{
			public ByteValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Byte value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.PrivatePackCore( value );
			}
		}
		
		private sealed class NullableByteValuePacker : ValuePacker< System.Byte? >
		{
			public NullableByteValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Byte? value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.Pack( value );
			}
		}

		private sealed class UInt16ValuePacker : ValuePacker< System.UInt16 >
		{
			public UInt16ValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.UInt16 value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.PrivatePackCore( value );
			}
		}
		
		private sealed class NullableUInt16ValuePacker : ValuePacker< System.UInt16? >
		{
			public NullableUInt16ValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.UInt16? value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.Pack( value );
			}
		}

		private sealed class UInt32ValuePacker : ValuePacker< System.UInt32 >
		{
			public UInt32ValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.UInt32 value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.PrivatePackCore( value );
			}
		}
		
		private sealed class NullableUInt32ValuePacker : ValuePacker< System.UInt32? >
		{
			public NullableUInt32ValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.UInt32? value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.Pack( value );
			}
		}

		private sealed class UInt64ValuePacker : ValuePacker< System.UInt64 >
		{
			public UInt64ValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.UInt64 value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.PrivatePackCore( value );
			}
		}
		
		private sealed class NullableUInt64ValuePacker : ValuePacker< System.UInt64? >
		{
			public NullableUInt64ValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.UInt64? value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.Pack( value );
			}
		}

		private sealed class SingleValuePacker : ValuePacker< System.Single >
		{
			public SingleValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Single value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.PrivatePackCore( value );
			}
		}
		
		private sealed class NullableSingleValuePacker : ValuePacker< System.Single? >
		{
			public NullableSingleValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Single? value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.Pack( value );
			}
		}

		private sealed class DoubleValuePacker : ValuePacker< System.Double >
		{
			public DoubleValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Double value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.PrivatePackCore( value );
			}
		}
		
		private sealed class NullableDoubleValuePacker : ValuePacker< System.Double? >
		{
			public NullableDoubleValuePacker() { }
			
			public sealed override void Pack( Packer packer, System.Double? value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );
				packer.Pack( value );
			}
		}
		private sealed class MessagePackObjectValuePacker : ValuePacker<MessagePackObject>
		{
			public MessagePackObjectValuePacker() { }
			
			public sealed override void Pack( Packer packer, MessagePackObject value, PackingOptions options )
			{
				value.PackToMessage( packer, options );
			}
		}

		private sealed class PackableValuePacker<TPackable> : ValuePacker<TPackable>
			where TPackable : IPackable
		{
			public PackableValuePacker() { }
			
			public sealed override void Pack( Packer packer, TPackable value, PackingOptions options )
			{
				if ( value == null )
				{
					packer.PrivatePackNullCore();
					return;
				}
				
				value.PackToMessage( packer, options );
			}
		}

		private sealed class ByteArrayValuePacker : ValuePacker<byte[]>
		{
			public ByteArrayValuePacker() { }
			
			public sealed override void Pack( Packer packer, byte[] value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );

				if ( value == null )
				{
					packer.PrivatePackNullCore();
					return;
				}
				
				packer.PrivatePackRawCore( value, false );
			}
		}

		private sealed class StringValuePacker : ValuePacker<string>
		{
			public StringValuePacker() { }
			
			public sealed override void Pack( Packer packer, string value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );

				if ( value == null )
				{
					packer.PrivatePackNullCore();
					return;
				}
				
				packer.PrivatePackStringCore( value, options == null ? MessagePackConvert.Utf8NonBom : options.StringEncoding  );
			}
		}
		
		private sealed class ArrayValuePacker : ValuePacker<IList>
		{
			public ArrayValuePacker() { }
			
			public sealed override void Pack( Packer packer, IList value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );

				if ( value == null )
				{
					packer.PrivatePackNullCore();
					return;
				}
				
				packer.PrivatePackArrayHeaderCore( value.Count );
				foreach ( var item in value )
				{
					packer.PrivatePackObject( item, options );
				}
			}
		}
				
		private sealed class ArrayValuePacker<TItem> : ValuePacker<IList<TItem>>
		{
			public ArrayValuePacker() { }
			
			public sealed override void Pack( Packer packer, IList<TItem> value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );

				if ( value == null )
				{
					packer.PrivatePackNullCore();
					return;
				}
				
				packer.PrivatePackArrayHeaderCore( value.Count );
				foreach ( var item in value )
				{
					ValuePacker<TItem>.Instance.Pack( packer, item, options );
				}
			}
		}
		
		private sealed class DictionaryValuePacker : ValuePacker<IDictionary>
		{
			public DictionaryValuePacker() { }
			
			public sealed override void Pack( Packer packer, IDictionary value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );

				if ( value == null )
				{
					packer.PrivatePackNullCore();
					return;
				}
				
				packer.PrivatePackMapHeaderCore( value.Count );
				foreach ( DictionaryEntry item in value )
				{
					packer.PrivatePackObject( item.Key, options );
					packer.PrivatePackObject( item.Value, options );
				}
			}
		}

		private sealed class DictionaryValuePacker<TKey,TValue> : ValuePacker<IDictionary<TKey,TValue>>
		{
			public DictionaryValuePacker() { }
			
			public sealed override void Pack( Packer packer, IDictionary<TKey,TValue> value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );

				if ( value == null )
				{
					packer.PackNull();
					return;
				}
				
				packer.PrivatePackMapHeaderCore( value.Count );
				foreach ( var item in value )
				{
					ValuePacker<TKey>.Instance.Pack( packer, item.Key, options );
					ValuePacker<TValue>.Instance.Pack( packer, item.Value, options );
				}
			}
		}

		private sealed class FallbackValuePacker<T> : ValuePacker<T>
		{
			public FallbackValuePacker() { }
			
			public sealed override void Pack( Packer packer, T value, PackingOptions options )
			{
				Contract.Assert( !packer._isDisposed );

				if ( value == null )
				{
					packer.PrivatePackNullCore();
					return;
				}
				
				throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "Cannot pack '{0}'({1} type).", value, value.GetType() ) );
			}
		}
	}
}