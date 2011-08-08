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
using System.Globalization;

namespace MsgPack
{
	partial class Packer
	{

		/// <summary>
		///		Pack specified <see cref="Object"/> as apporipriate value.
		/// </summary>
		/// <param name="boxedValue">Boxed value.</param>
		/// <param name="options">Options.</param>
		/// <returns>This instance.</returns>
		/// <exception cref="MessageTypeException">There is no approptiate MessagePack type to represent specified object.</exception>
		public Packer PackObject( object boxedValue, PackingOptions options )
		{
			if ( boxedValue == null )
			{
				return this.PackNull();
			}

			var asPackable = boxedValue as IPackable;
			if ( asPackable != null )
			{
				asPackable.PackToMessage( this, options );
				return this;
			}
			if ( boxedValue is System.Boolean )
			{
				return this.Pack( ( System.Boolean )boxedValue );
			}

			if ( boxedValue is System.Boolean? )
			{
				return this.Pack( ( System.Boolean )boxedValue );
			}
			if ( boxedValue is System.SByte )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.SByte )boxedValue )
					: this.PackStrict( ( System.SByte )boxedValue );
			}

			if ( boxedValue is System.SByte? )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.SByte? )boxedValue )
					: this.PackStrict( ( System.SByte? )boxedValue );
			}
			if ( boxedValue is System.Int16 )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.Int16 )boxedValue )
					: this.PackStrict( ( System.Int16 )boxedValue );
			}

			if ( boxedValue is System.Int16? )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.Int16? )boxedValue )
					: this.PackStrict( ( System.Int16? )boxedValue );
			}
			if ( boxedValue is System.Int32 )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.Int32 )boxedValue )
					: this.PackStrict( ( System.Int32 )boxedValue );
			}

			if ( boxedValue is System.Int32? )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.Int32? )boxedValue )
					: this.PackStrict( ( System.Int32? )boxedValue );
			}
			if ( boxedValue is System.Int64 )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.Int64 )boxedValue )
					: this.PackStrict( ( System.Int64 )boxedValue );
			}

			if ( boxedValue is System.Int64? )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.Int64? )boxedValue )
					: this.PackStrict( ( System.Int64? )boxedValue );
			}
			if ( boxedValue is System.Byte )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.Byte )boxedValue )
					: this.PackStrict( ( System.Byte )boxedValue );
			}

			if ( boxedValue is System.Byte? )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.Byte? )boxedValue )
					: this.PackStrict( ( System.Byte? )boxedValue );
			}
			if ( boxedValue is System.UInt16 )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.UInt16 )boxedValue )
					: this.PackStrict( ( System.UInt16 )boxedValue );
			}

			if ( boxedValue is System.UInt16? )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.UInt16? )boxedValue )
					: this.PackStrict( ( System.UInt16? )boxedValue );
			}
			if ( boxedValue is System.UInt32 )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.UInt32 )boxedValue )
					: this.PackStrict( ( System.UInt32 )boxedValue );
			}

			if ( boxedValue is System.UInt32? )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.UInt32? )boxedValue )
					: this.PackStrict( ( System.UInt32? )boxedValue );
			}
			if ( boxedValue is System.UInt64 )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.UInt64 )boxedValue )
					: this.PackStrict( ( System.UInt64 )boxedValue );
			}

			if ( boxedValue is System.UInt64? )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.UInt64? )boxedValue )
					: this.PackStrict( ( System.UInt64? )boxedValue );
			}
			if ( boxedValue is System.Single )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.Single )boxedValue )
					: this.PackStrict( ( System.Single )boxedValue );
			}

			if ( boxedValue is System.Single? )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.Single? )boxedValue )
					: this.PackStrict( ( System.Single? )boxedValue );
			}
			if ( boxedValue is System.Double )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.Double )boxedValue )
					: this.PackStrict( ( System.Double )boxedValue );
			}

			if ( boxedValue is System.Double? )
			{
				return
					( options == null || !options.IsStrict )
					? this.Pack( ( System.Double? )boxedValue )
					: this.PackStrict( ( System.Double? )boxedValue );
			}

			var asMps = boxedValue as MessagePackString;
			if ( asMps != null )
			{
				if ( asMps.UnsafeGetBuffer() != null )
				{
					return this.PackRaw( asMps.UnsafeGetBuffer() );
				}

				if ( options == null )
				{
					return this.PackRaw( asMps.GetBytes() );
				}
				else
				{
					return this.PackRaw( options.StringEncoding.GetBytes( asMps.TryGetString() ) );
				}
			}

			var asByteArray = boxedValue as byte[];
			if ( asByteArray != null )
			{
				return this.PackRaw( asByteArray );
			}

			var asString = boxedValue as string;
			if ( asString != null )
			{
				return this.PackString( asString, options == null ? MessagePackConvert.Utf8NonBom : options.StringEncoding );
			}

			var collectionType = ExtractCollectionType( boxedValue.GetType() );
			if ( collectionType != null )
			{
				ValuePacker.GetInstance( collectionType ).PackObject( this, boxedValue, options );
				return this;
			}

			throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Unknown object '{0}'.", boxedValue.GetType() ) );
		}

		private abstract class ValuePacker
		{
			private static readonly Dictionary<Type, ValuePacker> _cache = new Dictionary<Type, ValuePacker>();

			public static ValuePacker GetInstance( Type targetType )
			{
				ValuePacker result;
				if ( !_cache.TryGetValue( targetType, out result ) )
				{
					var packerType = typeof( ValuePacker<> ).MakeGenericType( targetType );
					result = ( ValuePacker )packerType.GetField( "Instance" ).GetValue( null );
					_cache[ targetType ] = result;
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
				else if ( typeof( T ) == typeof( string ) )
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
								) as ValuePacker<T>;
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
				if ( value == null )
				{
					packer.PackNull();
					return;
				}

				this._realPacker.PackObject( packer, value, options );
			}
		}


		private sealed class BooleanValuePacker : ValuePacker<System.Boolean>
		{
			public BooleanValuePacker() { }

			public sealed override void Pack( Packer packer, System.Boolean value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class NullableBooleanValuePacker : ValuePacker<System.Boolean?>
		{
			public NullableBooleanValuePacker() { }

			public sealed override void Pack( Packer packer, System.Boolean? value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class SByteValuePacker : ValuePacker<System.SByte>
		{
			public SByteValuePacker() { }

			public sealed override void Pack( Packer packer, System.SByte value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class NullableSByteValuePacker : ValuePacker<System.SByte?>
		{
			public NullableSByteValuePacker() { }

			public sealed override void Pack( Packer packer, System.SByte? value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class Int16ValuePacker : ValuePacker<System.Int16>
		{
			public Int16ValuePacker() { }

			public sealed override void Pack( Packer packer, System.Int16 value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class NullableInt16ValuePacker : ValuePacker<System.Int16?>
		{
			public NullableInt16ValuePacker() { }

			public sealed override void Pack( Packer packer, System.Int16? value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class Int32ValuePacker : ValuePacker<System.Int32>
		{
			public Int32ValuePacker() { }

			public sealed override void Pack( Packer packer, System.Int32 value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class NullableInt32ValuePacker : ValuePacker<System.Int32?>
		{
			public NullableInt32ValuePacker() { }

			public sealed override void Pack( Packer packer, System.Int32? value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class Int64ValuePacker : ValuePacker<System.Int64>
		{
			public Int64ValuePacker() { }

			public sealed override void Pack( Packer packer, System.Int64 value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class NullableInt64ValuePacker : ValuePacker<System.Int64?>
		{
			public NullableInt64ValuePacker() { }

			public sealed override void Pack( Packer packer, System.Int64? value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class ByteValuePacker : ValuePacker<System.Byte>
		{
			public ByteValuePacker() { }

			public sealed override void Pack( Packer packer, System.Byte value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class NullableByteValuePacker : ValuePacker<System.Byte?>
		{
			public NullableByteValuePacker() { }

			public sealed override void Pack( Packer packer, System.Byte? value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class UInt16ValuePacker : ValuePacker<System.UInt16>
		{
			public UInt16ValuePacker() { }

			public sealed override void Pack( Packer packer, System.UInt16 value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class NullableUInt16ValuePacker : ValuePacker<System.UInt16?>
		{
			public NullableUInt16ValuePacker() { }

			public sealed override void Pack( Packer packer, System.UInt16? value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class UInt32ValuePacker : ValuePacker<System.UInt32>
		{
			public UInt32ValuePacker() { }

			public sealed override void Pack( Packer packer, System.UInt32 value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class NullableUInt32ValuePacker : ValuePacker<System.UInt32?>
		{
			public NullableUInt32ValuePacker() { }

			public sealed override void Pack( Packer packer, System.UInt32? value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class UInt64ValuePacker : ValuePacker<System.UInt64>
		{
			public UInt64ValuePacker() { }

			public sealed override void Pack( Packer packer, System.UInt64 value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class NullableUInt64ValuePacker : ValuePacker<System.UInt64?>
		{
			public NullableUInt64ValuePacker() { }

			public sealed override void Pack( Packer packer, System.UInt64? value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class SingleValuePacker : ValuePacker<System.Single>
		{
			public SingleValuePacker() { }

			public sealed override void Pack( Packer packer, System.Single value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class NullableSingleValuePacker : ValuePacker<System.Single?>
		{
			public NullableSingleValuePacker() { }

			public sealed override void Pack( Packer packer, System.Single? value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class DoubleValuePacker : ValuePacker<System.Double>
		{
			public DoubleValuePacker() { }

			public sealed override void Pack( Packer packer, System.Double value, PackingOptions options )
			{
				packer.Pack( value );
			}
		}

		private sealed class NullableDoubleValuePacker : ValuePacker<System.Double?>
		{
			public NullableDoubleValuePacker() { }

			public sealed override void Pack( Packer packer, System.Double? value, PackingOptions options )
			{
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
					packer.PackNull();
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
				if ( value == null )
				{
					packer.PackNull();
					return;
				}

				packer.PackRaw( value );
			}
		}

		private sealed class StringValuePacker : ValuePacker<string>
		{
			public StringValuePacker() { }

			public sealed override void Pack( Packer packer, string value, PackingOptions options )
			{
				if ( value == null )
				{
					packer.PackNull();
					return;
				}

				packer.PackString( value, options == null ? MessagePackConvert.Utf8NonBom : options.StringEncoding );
			}
		}

		private sealed class ArrayValuePacker : ValuePacker<IList>
		{
			public ArrayValuePacker() { }

			public sealed override void Pack( Packer packer, IList value, PackingOptions options )
			{
				if ( value == null )
				{
					packer.PackNull();
					return;
				}

				packer.PackArrayHeader( value.Count );
				foreach ( var item in value )
				{
					packer.PackObject( item, options );
				}
			}
		}

		private sealed class ArrayValuePacker<TItem> : ValuePacker<IList<TItem>>
		{
			public ArrayValuePacker() { }

			public sealed override void Pack( Packer packer, IList<TItem> value, PackingOptions options )
			{
				if ( value == null )
				{
					packer.PackNull();
					return;
				}

				packer.PackArrayHeader( value.Count );
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
				if ( value == null )
				{
					packer.PackNull();
					return;
				}

				packer.PackMapHeader( value.Count );
				foreach ( DictionaryEntry item in value )
				{
					packer.PackObject( item.Key, options );
					packer.PackObject( item.Value, options );
				}
			}
		}

		private sealed class DictionaryValuePacker<TKey, TValue> : ValuePacker<IDictionary<TKey, TValue>>
		{
			public DictionaryValuePacker() { }

			public sealed override void Pack( Packer packer, IDictionary<TKey, TValue> value, PackingOptions options )
			{
				if ( value == null )
				{
					packer.PackNull();
					return;
				}

				packer.PackMapHeader( value.Count );
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
				if ( value == null )
				{
					packer.PackNull();
					return;
				}

				throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "Cannot pack '{0}'({1} type).", value, value.GetType() ) );
			}
		}
	}
}