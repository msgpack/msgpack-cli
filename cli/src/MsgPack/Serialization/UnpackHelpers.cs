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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Defines serialization helper APIs.
	/// </summary>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public static class UnpackHelpers
	{
		private static readonly MessagePackSerializer<MessagePackObject> _messagePackObjectSerializer =
			new MsgPack_MessagePackObjectMessagePackSerializer();

		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackArrayTo<T>( Unpacker unpacker, MessagePackSerializer<T> serializer, T[] array )
		{
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( array == null )
			{
				throw new ArgumentNullException( "collection" );
			}

			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			Contract.EndContractBlock();

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				T item;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					item = serializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						item = serializer.UnpackFrom( subtreeUnpacker );
					}
				}

				array[ i ] = item;
			}
		}

		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackCollectionTo( Unpacker unpacker, IEnumerable collection, Action<object> addition )
		{
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				throw new ArgumentNullException( "collection" );
			}

			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			Contract.EndContractBlock();

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				MessagePackObject item;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					item = _messagePackObjectSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						item = _messagePackObjectSerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				addition( item );
			}
		}

		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackCollectionTo<T>( Unpacker unpacker, MessagePackSerializer<T> serializer, IEnumerable<T> collection, Action<T> addition )
		{
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				throw new ArgumentNullException( "collection" );
			}

			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			Contract.EndContractBlock();

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				T item;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					item = serializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						item = serializer.UnpackFrom( subtreeUnpacker );
					}
				}

				addition( item );
			}
		}

		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackCollectionTo<TDiscarded>( Unpacker unpacker, IEnumerable collection, Func<object, TDiscarded> addition )
		{
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				throw new ArgumentNullException( "collection" );
			}

			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			Contract.EndContractBlock();

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				MessagePackObject item;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					item = _messagePackObjectSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						item = _messagePackObjectSerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				addition( item );
			}
		}

		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackCollectionTo<T, TDiscarded>( Unpacker unpacker, MessagePackSerializer<T> serializer, IEnumerable<T> collection, Func<T, TDiscarded> addition )
		{
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				throw new ArgumentNullException( "collection" );
			}

			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			Contract.EndContractBlock();

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				T item;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					item = serializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						item = serializer.UnpackFrom( subtreeUnpacker );
					}
				}

				addition( item );
			}
		}

		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackMapTo<TKey, TValue>( Unpacker unpacker, MessagePackSerializer<TKey> keySerializer, MessagePackSerializer<TValue> valueSerializer, IDictionary<TKey, TValue> dictionary )
		{
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( dictionary == null )
			{
				throw new ArgumentNullException( "collection" );
			}

			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			Contract.EndContractBlock();

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				TKey key;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					key = keySerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						key = keySerializer.UnpackFrom( subtreeUnpacker );
					}
				}


				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				TValue value;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					value = valueSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						value = valueSerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				dictionary.Add( key, value );
			}
		}

		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackMapTo( Unpacker unpacker, IDictionary dictionary )
		{
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( dictionary == null )
			{
				throw new ArgumentNullException( "collection" );
			}

			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			Contract.EndContractBlock();

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				MessagePackObject key;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					key = _messagePackObjectSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						key = _messagePackObjectSerializer.UnpackFrom( subtreeUnpacker );
					}
				}


				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				MessagePackObject value;
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					value = _messagePackObjectSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( Unpacker subtreeUnpacker = unpacker.ReadSubtree() )
					{
						value = _messagePackObjectSerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				dictionary.Add( key, value );
			}
		}

		private static int GetItemsCount( Unpacker unpacker )
		{
			long rawItemsCount;
			try
			{
				rawItemsCount = unpacker.ItemsCount;
			}
			catch ( InvalidOperationException ex )
			{
				throw SerializationExceptions.NewIsIncorrectStream( ex );
			}

			if ( rawItemsCount > Int32.MaxValue )
			{
				throw SerializationExceptions.NewIsTooLargeCollection();
			}

			int count = unchecked( ( int )rawItemsCount );
			return count;
		}
	}
}
