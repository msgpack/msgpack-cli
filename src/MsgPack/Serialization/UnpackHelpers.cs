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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reflection;
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
			new MsgPack_MessagePackObjectMessagePackSerializer( PackerCompatibilityOptions.None );

		/// <summary>
		///		Unpacks the array to the specified array.
		/// </summary>
		/// <typeparam name="T">The type of the array element.</typeparam>
		/// <param name="unpacker">The unpacker to unpack the underlying stream.</param>
		/// <param name="serializer">The serializer to deserialize array.</param>
		/// <param name="array">The array instance to be filled.</param>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialization.
		/// </exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackArrayTo<T>( Unpacker unpacker, MessagePackSerializer<T> serializer, T[] array )
		{
#if DEBUG
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( array == null )
			{
				throw new ArgumentNullException( "array" );
			}

			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			Contract.EndContractBlock();
#endif

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

		/// <summary>
		///		Unpacks the collection with the specified method as colletion of <see cref="MessagePackObject"/>.
		/// </summary>
		/// <param name="unpacker">The unpacker to unpack the underlying stream.</param>
		/// <param name="collection">The non-generic collection instance to be added unpacked elements.</param>
		/// <param name="addition">The delegate which contains the instance method of the <paramref name="collection"/>. The parameter is unpacked object.</param>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialization.
		/// </exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackCollectionTo( Unpacker unpacker, IEnumerable collection, Action<object> addition )
		{
#if DEBUG
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
#endif

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

		/// <summary>
		///		Unpacks the dictionary with the specified method as colletion of <see cref="MessagePackObject"/>.
		/// </summary>
		/// <typeparam name="T">The type of elements.</typeparam>
		/// <param name="unpacker">The unpacker to unpack the underlying stream.</param>
		/// <param name="serializer">The serializer to deserialize elements.</param>
		/// <param name="collection">The generic collection instance to be added unpacked elements.</param>
		/// <param name="addition">The delegate which contains the instance method of the <paramref name="collection"/>. The parameter is unpacked object.</param>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialization.
		/// </exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackCollectionTo<T>( Unpacker unpacker, MessagePackSerializer<T> serializer, IEnumerable<T> collection, Action<T> addition )
		{
#if DEBUG
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
#endif

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

		/// <summary>
		///		Unpacks the collection with the specified method as colletion of <see cref="MessagePackObject"/>.
		/// </summary>
		/// <typeparam name="TDiscarded">The return type of Add method.</typeparam>
		/// <param name="unpacker">The unpacker to unpack the underlying stream.</param>
		/// <param name="collection">The non-generic collection instance to be added unpacked elements.</param>
		/// <param name="addition">The delegate which contains the instance method of the <paramref name="collection"/>. The parameter is unpacked object.</param>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialization.
		/// </exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackCollectionTo<TDiscarded>( Unpacker unpacker, IEnumerable collection, Func<object, TDiscarded> addition )
		{
#if DEBUG
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
#endif

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

		/// <summary>
		///		Unpacks the dictionary with the specified method as colletion of <see cref="MessagePackObject"/>.
		/// </summary>
		/// <typeparam name="T">The type of elements.</typeparam>
		/// <typeparam name="TDiscarded">The return type of Add method.</typeparam>
		/// <param name="unpacker">The unpacker to unpack the underlying stream.</param>
		/// <param name="serializer">The serializer to deserialize elements.</param>
		/// <param name="collection">The generic collection instance to be added unpacked elements.</param>
		/// <param name="addition">The delegate which contains the instance method of the <paramref name="collection"/>. The parameter is unpacked object.</param>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialization.
		/// </exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackCollectionTo<T, TDiscarded>( Unpacker unpacker, MessagePackSerializer<T> serializer, IEnumerable<T> collection, Func<T, TDiscarded> addition )
		{
#if DEBUG
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
#endif

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

		/// <summary>
		///		Unpacks the dictionary with the specified method as colletion of <see cref="MessagePackObject"/>.
		/// </summary>
		/// <typeparam name="TKey">The type of keys.</typeparam>
		/// <typeparam name="TValue">The type of values.</typeparam>
		/// <param name="unpacker">The unpacker to unpack the underlying stream.</param>
		/// <param name="keySerializer">The serializer to deserialize key elements.</param>
		/// <param name="valueSerializer">The serializer to deserialize value elements.</param>
		/// <param name="dictionary">The generic dictionary instance to be added unpacked elements.</param>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialization.
		/// </exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackMapTo<TKey, TValue>( Unpacker unpacker, MessagePackSerializer<TKey> keySerializer, MessagePackSerializer<TValue> valueSerializer, IDictionary<TKey, TValue> dictionary )
		{
#if DEBUG
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( dictionary == null )
			{
				throw new ArgumentNullException( "dictionary" );
			}

			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			Contract.EndContractBlock();
#endif

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

		/// <summary>
		///		Unpacks the dictionary with the specified method as colletion of <see cref="MessagePackObject"/>.
		/// </summary>
		/// <param name="unpacker">The unpacker to unpack the underlying stream.</param>
		/// <param name="dictionary">The non-generic dictionary instance to be added unpacked elements.</param>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialization.
		/// </exception>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static void UnpackMapTo( Unpacker unpacker, IDictionary dictionary )
		{
#if DEBUG
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( dictionary == null )
			{
				throw new ArgumentNullException( "dictionary" );
			}

			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			Contract.EndContractBlock();
#endif

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

		internal static int GetItemsCount( Unpacker unpacker )
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

		internal static bool IsReadOnlyAppendableCollectionMember( MemberInfo memberInfo )
		{
			Contract.Requires( memberInfo != null );

			if ( memberInfo.CanSetValue() )
			{
				// Not read only
				return false;
			}

			Type memberValueType = memberInfo.GetMemberValueType();
			if ( memberValueType.IsArray )
			{
				// Not appendable
				return false;
			}

			CollectionTraits traits = memberValueType.GetCollectionTraits();
			return traits.CollectionType != CollectionKind.NotCollection && traits.AddMethod != null;
		}

		/// <summary>
		///		Ensures the boxed type is not null thus it cannot be unboxing.
		/// </summary>
		/// <typeparam name="T">The type of the member.</typeparam>
		/// <param name="boxed">The boxed deserializing value.</param>
		/// <param name="name">The name of the member.</param>
		/// <param name="targetType">The type of the target.</param>
		/// <returns>The unboxed value.</returns>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static T ConvertWithEnsuringNotNull<T>( object boxed, string name, Type targetType )
		{
			if ( typeof( T ).GetIsValueType() && boxed == null && Nullable.GetUnderlyingType( typeof( T ) ) == null )
			{
				throw SerializationExceptions.NewValueTypeCannotBeNull( name, typeof( T ), targetType );
			}

			return ( T )boxed;
		}

		/// <summary>
		///		Invokes <see cref="MessagePackSerializer{T}.UnpackFromCore"/> FAMANDASM method directly.
		/// </summary>
		/// <typeparam name="T">The type of deserializing object.</typeparam>
		/// <param name="serializer">The invocation target <see cref="MessagePackSerializer{T}"/>.</param>
		/// <param name="unpacker">The unpacker to be passed to the method.</param>
		/// <returns>A deserialized value.</returns>
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static T InvokeUnpackFrom<T>( MessagePackSerializer<T> serializer, Unpacker unpacker )
		{
			return serializer.UnpackFromCore( unpacker );
		}
	}
}
