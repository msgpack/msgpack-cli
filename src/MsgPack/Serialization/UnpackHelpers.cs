#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

#if DEBUG
#define ASSERT
#endif // DEBUG

#if DEBUG && !NETFX_CORE
#define TRACING
#endif // DEBUG && !NETFX_CORE

using System;
using System.Collections;
using System.Collections.Generic;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif //!UNITY || MSGPACK_UNITY_FULL
using System.Diagnostics;
#if ASSERT
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
#endif // ASSERT
using System.Reflection;
using System.Runtime.CompilerServices;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.DefaultSerializers;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Defines serialization helper APIs.
	/// </summary>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Aggregating facade." )]
	public static partial class UnpackHelpers
	{
		private static readonly MessagePackSerializer<MessagePackObject> _messagePackObjectSerializer =
			new MsgPack_MessagePackObjectMessagePackSerializer( SerializationContext.Default );

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
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[Obsolete( "This API is not used at generated serializers in current release, so this API will be removed future." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		public static void UnpackArrayTo<T>( Unpacker unpacker, MessagePackSerializer<T> serializer, T[] array )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( serializer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "serializer" );
			}

			if ( array == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "array" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
			Contract.Assert( array != null );
#endif // ASSERT

			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

#if ASSERT
			Contract.EndContractBlock();
#endif // ASSERT

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
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
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[Obsolete( "This API is not used at generated serializers in current release, so this API will be removed future." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		public static void UnpackCollectionTo( Unpacker unpacker, IEnumerable collection, Action<object> addition )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "collection" );
			}

			if ( addition == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "addition" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( collection != null );
			Contract.Assert( addition != null );
#endif // ASSERT

			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

#if ASSERT
			Contract.EndContractBlock();
#endif // ASSERT

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
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
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[Obsolete( "This API is not used at generated serializers in current release, so this API will be removed future." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "False positive because never reached." )]
		public static void UnpackCollectionTo<T>( Unpacker unpacker, MessagePackSerializer<T> serializer, IEnumerable<T> collection, Action<T> addition )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( serializer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "serializer" );
			}

			if ( collection == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "collection" );
			}

			if ( addition == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "addition" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
			Contract.Assert( collection != null );
			Contract.Assert( addition != null );
#endif // ASSERT

			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

#if ASSERT
			Contract.EndContractBlock();
#endif // ASSERT

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
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
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[Obsolete( "This API is not used at generated serializers in current release, so this API will be removed future." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		public static void UnpackCollectionTo<TDiscarded>( Unpacker unpacker, IEnumerable collection, Func<object, TDiscarded> addition )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( collection == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "collection" );
			}

			if ( addition == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "addition" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( collection != null );
			Contract.Assert( addition != null );
#endif // ASSERT

			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

#if ASSERT
			Contract.EndContractBlock();
#endif // ASSERT

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
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
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[Obsolete( "This API is not used at generated serializers in current release, so this API will be removed future." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "False positive because never reached." )]
		public static void UnpackCollectionTo<T, TDiscarded>( Unpacker unpacker, MessagePackSerializer<T> serializer, IEnumerable<T> collection, Func<T, TDiscarded> addition )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( serializer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "serializer" );
			}

			if ( collection == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "collection" );
			}

			if ( addition == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "addition" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
			Contract.Assert( collection != null );
			Contract.Assert( addition != null );
#endif // ASSERT

			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

#if ASSERT
			Contract.EndContractBlock();
#endif // ASSERT

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
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
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[Obsolete( "This API is not used at generated serializers in current release, so this API will be removed future." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "False positive because never reached." )]
		public static void UnpackMapTo<TKey, TValue>( Unpacker unpacker, MessagePackSerializer<TKey> keySerializer, MessagePackSerializer<TValue> valueSerializer, IDictionary<TKey, TValue> dictionary )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( keySerializer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "keySerializer" );
			}

			if ( valueSerializer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "valueSerializer" );
			}

			if ( dictionary == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "dictionary" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( keySerializer != null );
			Contract.Assert( valueSerializer != null );
			Contract.Assert( dictionary != null );
#endif // ASSERT

			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotMapHeader( unpacker );
			}

#if ASSERT
			Contract.EndContractBlock();
#endif // ASSERT

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
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
					SerializationExceptions.ThrowMissingItem( i, unpacker );
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
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[Obsolete( "This API is not used at generated serializers in current release, so this API will be removed future." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "False positive because never reached." )]
		public static void UnpackMapTo( Unpacker unpacker, IDictionary dictionary )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( dictionary == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "dictionary" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( dictionary != null );
#endif // ASSERT

			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotMapHeader( unpacker );
			}

#if ASSERT
			Contract.EndContractBlock();
#endif // ASSERT

			int count = GetItemsCount( unpacker );
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
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
					SerializationExceptions.ThrowMissingItem( i, unpacker );
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

		/// <summary>
		///		Gets the items count as <see cref="Int32"/>.
		/// </summary>
		/// <param name="unpacker">The unpacker.</param>
		/// <returns>The items count as <see cref="Int32"/>.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="unpacker"/> is <c>null.</c></exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">The items count is greater than <see cref="Int32.MaxValue"/>.</exception>
		/// <remarks>
		///		The items count of the collection can be between <see cref="Int32.MaxValue"/> and <see cref="UInt32.MaxValue"/>,
		///		but most collections do not support so big count.
		/// </remarks>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static int GetItemsCount( Unpacker unpacker )
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			long rawItemsCount = 0L;
			try
			{
				// ReSharper disable once PossibleNullReferenceException
				rawItemsCount = unpacker.ItemsCount;
			}
			catch ( InvalidOperationException ex )
			{
				SerializationExceptions.ThrowIsIncorrectStream( ex );
			}

			if ( rawItemsCount > Int32.MaxValue )
			{
				SerializationExceptions.ThrowIsTooLargeCollection();
			}

			int count = unchecked( ( int )rawItemsCount );
			return count;
		}

		/// <summary>
		///		Ensures the boxed type is not null thus it cannot be unboxing.
		/// </summary>
		/// <typeparam name="T">The type of the member.</typeparam>
		/// <param name="boxed">The boxed deserializing value.</param>
		/// <param name="name">The name of the member.</param>
		/// <param name="targetType">The type of the target.</param>
		/// <returns>The unboxed value.</returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[Obsolete( "This API is not used at generated serializers in current release, so this API will be removed future." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static T ConvertWithEnsuringNotNull<T>( object boxed, string name, Type targetType )
		{
			if ( typeof( T ).GetIsValueType() && boxed == null && Nullable.GetUnderlyingType( typeof( T ) ) == null )
			{
				SerializationExceptions.ThrowValueTypeCannotBeNull( name, typeof( T ), targetType );
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
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[Obsolete( "This API is not used at generated serializers in current release, so this API will be removed future." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "False positive because never reached." )]
		public static T InvokeUnpackFrom<T>( MessagePackSerializer<T> serializer, Unpacker unpacker )
		{
			if ( serializer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "serializer" );
			}

#if ASSERT
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
#endif // ASSERT

			return serializer.UnpackFromCore( unpacker );
		}
		internal static SerializationTarget DetermineCollectionSerializationStrategy( Type instanceType, bool allowAsymmetricSerializer )
		{
			bool canDeserialize;
			var collectionConstructor = UnpackHelpers.TryGetCollectionConstructor( instanceType );
			if ( collectionConstructor == null )
			{
				if ( !allowAsymmetricSerializer )
				{
					SerializationExceptions.ThrowTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity( instanceType );
				}

				// Pack only.
				canDeserialize = false;
			}
			else
			{
				canDeserialize = true;
			}

			return SerializationTarget.CreateForCollection( collectionConstructor, canDeserialize );
		}

		/// <summary>
		///		Retrieves a most appropriate constructor with <see cref="Int32"/> capacity parameter and <see cref="IEqualityComparer{T}"/> comparer parameter or both of them, >or default constructor of the <paramref name="instanceType"/>.
		/// </summary>
		/// <param name="instanceType">The target collection type to be instanciated.</param>
		/// <returns>A constructor of the <paramref name="instanceType"/>.</returns>
		private static ConstructorInfo TryGetCollectionConstructor( Type instanceType )
		{
			const int noParameters = 0;
			const int withCapacity = 10;
			const int withComparer = 11;
			const int withComparerAndCapacity = 20;
			const int withCapacityAndComparer = 21;

			ConstructorInfo constructor = null;
			var currentScore = -1;

			foreach ( var candidate in instanceType.GetConstructors() )
			{
				var parameters = candidate.GetParameters();
				switch ( parameters.Length )
				{
					case 0:
					{
						if ( currentScore < noParameters )
						{
							constructor = candidate;
							currentScore = noParameters;
						}

						break;
					}
					case 1:
					{
						if ( currentScore < withCapacity && parameters[ 0 ].ParameterType == typeof( int ) )
						{
							constructor = candidate;
							currentScore = noParameters;
						}
						else if ( currentScore < withComparer && IsIEqualityComparer( parameters[ 0 ].ParameterType ) )
						{
							constructor = candidate;
							currentScore = noParameters;
						}
						break;
					}
					case 2:
					{
						if ( currentScore < withCapacityAndComparer && parameters[ 0 ].ParameterType == typeof( int ) && IsIEqualityComparer( parameters[ 1 ].ParameterType ) )
						{
							constructor = candidate;
							currentScore = withCapacityAndComparer;
						}
						else if ( currentScore < withComparerAndCapacity && parameters[ 1 ].ParameterType == typeof( int ) && IsIEqualityComparer( parameters[ 0 ].ParameterType ) )
						{
							constructor = candidate;
							currentScore = withComparerAndCapacity;
						}

						break;
					}
				}
			}

			return constructor;
		}

		/// <summary>
		///		Determines the type is <see cref="IEqualityComparer{T}"/>.
		/// </summary>
		/// <param name="type">The type should be <see cref="IEqualityComparer{T}"/>.</param>
		/// <returns>
		///		<c>true</c>, if <paramref name="type"/> is open <see cref="IEqualityComparer{T}"/> generic type; <c>false</c>, otherwise.
		/// </returns>
		internal static bool IsIEqualityComparer( Type type )
		{
#if ASSERT
			Contract.Assert( !type.GetIsGenericTypeDefinition(), "!(" + type + ").GetIsGenericTypeDefinition()" );
#endif // ASSERT

			return type.GetIsGenericType() && type.GetGenericTypeDefinition() == typeof( IEqualityComparer<> );
		}

#if UNITY
		internal static object GetEqualityComparer( Type comparerType )
		{
			return AotHelper.GetEqualityComparer( comparerType );
		}
#endif // UNITY


		/// <summary>
		///		Gets an <see cref="IEqualityComparer{T}"/> with platform safe fashion.
		/// </summary>
		/// <typeparam name="T">The type to be compared.</typeparam>
		/// <returns>
		///		An <see cref="IEqualityComparer{T}"/> instance.
		/// </returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static IEqualityComparer<T> GetEqualityComparer<T>()
		{
#if !UNITY
			return EqualityComparer<T>.Default;
#else
			// AotHelper is internal because it should not be API -- it is subject to change when the Unity's Mono is updated or IL2CPP becomes stable.
			return AotHelper.GetEqualityComparer<T>();
#endif // !UNITY
		}

		/// <summary>
		///		Gets the delegate which just returns the input ('identity' function).
		/// </summary>
		/// <typeparam name="T">The type of input and output.</typeparam>
		/// <returns>
		///		<see cref="Func{T,T}"/> delegate which just returns the input. This value will not be <c>null</c>.
		/// </returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static Func<T, T> GetIdentity<T>()
		{
			return v => v;
		}

		/// <summary>
		///		Gets the delegate which returns the input ('identity' function) as output type.
		/// </summary>
		/// <typeparam name="T">The type of output.</typeparam>
		/// <returns>
		///		<see cref="Func{Object,T}"/> delegate which returns the converted input. This value will not be <c>null</c>.
		/// </returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static Func<object, T> Unbox<T>()
		{
			return v => ( T )v;
		}

#if FEATURE_TAP
		/// <summary>
		///		Returns <see cref="Task{T}"/> which returns nullable wrapper for the value returned from specified <see cref="Task{T}" />.
		/// </summary>
		/// <typeparam name="T">The type of value.</typeparam>
		/// <param name="task">The <see cref="Task{T}"/> which returns non nullable one.</param>
		/// <returns>The <see cref="Task{T}"/> which returns nullable one.</returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<T> for nullables essentially must be nested generic." )]
		public static async Task<T?> ToNullable<T>( Task<T> task )
			where T : struct
		{
			// Use async for exception handling.
			return await task.ConfigureAwait( false );
		}

		/// <summary>
		///		Returns <see cref="Task{T}"/> which returns the value returned from specified <paramref name="target"/>.
		/// </summary>
		/// <typeparam name="T">The type of deserializing object.</typeparam>
		/// <param name="target">The deserializing object.</param>
		/// <param name="unpacker">The <see cref="Unpacker"/> which points deserializing data.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="T:CancellationToken.None"/>.</param>
		/// <returns>The <see cref="Task{T}"/> which returns deserialized <paramref name="target" />.</returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static async Task<T> UnpackFromMessageAsync<T>( T target, Unpacker unpacker, CancellationToken cancellationToken )
			where T : IAsyncUnpackable
		{
			await target.UnpackFromMessageAsync( unpacker, cancellationToken ).ConfigureAwait( false );
			return target;
		}

#endif // FEATURE_TAP

		[Conditional( "TRACING" )]
		// ReSharper disable once RedundantAssignment
		private static void InitializeUnpackerTrace( Unpacker unpacker, ref UnpackerTraceContext context,
#if TRACING
			[CallerMemberName]
#endif // TRACING
			string callerMemberName = null
		)
		{
#if TRACING
			long positionOrOffset;
			unpacker.GetPreviousPosition( out positionOrOffset );
			context = new UnpackerTraceContext( positionOrOffset, callerMemberName );
#endif // TRACING
		}

		[Conditional( "TRACING" )]
		private static void Trace( UnpackerTraceContext context, string label, Unpacker unpacker )
		{
#if TRACING
			long positionOrOffset;
			unpacker.GetPreviousPosition( out positionOrOffset );
			TraceCore(
				"{0}.{1}::{2:#,0}->{3:#,0}",
				context.MethodName,
				label,
				context.PositionOrOffset,
				positionOrOffset
			);
			context.PositionOrOffset = positionOrOffset;
#endif // TRACING
		}

		[Conditional( "TRACING" )]
		private static void Trace( UnpackerTraceContext context, string label, Unpacker unpacker, int index )
		{
#if TRACING
			long positionOrOffset;
			unpacker.GetPreviousPosition( out positionOrOffset );
			TraceCore(
				"{0}.{1}@{4}::{2:#,0}->{3:#,0}",
				context.MethodName,
				label,
				context.PositionOrOffset,
				positionOrOffset,
				index
			);
			context.PositionOrOffset = positionOrOffset;
#endif // TRACING
		}

		[Conditional( "TRACING" )]
		private static void Trace( UnpackerTraceContext context, string label, Unpacker unpacker, int index, IList<string> itemNames )
		{
#if TRACING
			long positionOrOffset;
			unpacker.GetPreviousPosition( out positionOrOffset );
			TraceCore(
				"{0}.{1}[{4}]@{5}::{2:#,0}->{3:#,0}",
				context.MethodName,
				label,
				context.PositionOrOffset,
				positionOrOffset,
				itemNames[ index ],
				index
			);
			context.PositionOrOffset = positionOrOffset;
#endif // TRACING
		}

		[Conditional( "TRACING" )]
		private static void Trace( UnpackerTraceContext context, string label, Unpacker unpacker, int index, string key )
		{
#if TRACING
			long positionOrOffset;
			unpacker.GetPreviousPosition( out positionOrOffset );
			TraceCore(
				"{0}.{1}[{4}]@{5}::{2:#,0}->{3:#,0}",
				context.MethodName,
				label,
				context.PositionOrOffset,
				positionOrOffset,
				key,
				index
			);
			context.PositionOrOffset = positionOrOffset;
#endif // TRACING
		}

		[Conditional( "TRACING" )]
		private static void Trace( UnpackerTraceContext context, string label, Unpacker unpacker, string key )
		{
#if TRACING
			long positionOrOffset;
			unpacker.GetPreviousPosition( out positionOrOffset );
			TraceCore(
				"{0}.{1}[{4}]::{2:#,0}->{3:#,0}",
				context.MethodName,
				label,
				context.PositionOrOffset,
				positionOrOffset,
				key
			);
			context.PositionOrOffset = positionOrOffset;
#endif // TRACING
		}

		[Conditional( "TRACING" )]
		private static void TraceCore( string format, params object[] args )
		{
#if !UNITY && !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
			Tracer.Tracing.TraceEvent( Tracer.EventType.Trace, Tracer.EventId.Trace, format, args );
#endif // !UNITY && !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
		}

		private sealed class UnpackerTraceContext
		{
#if TRACING
			public long PositionOrOffset { get; set; }
			public string MethodName { get; private set; }

			public UnpackerTraceContext( long positionOrOffset, string methodName )
			{
				this.PositionOrOffset = positionOrOffset;
				this.MethodName = methodName;
			}
#endif // TRACING
		}
	}
}
#if TRACING
#if ( SILVERLIGHT && !WINDOWS_PHONE ) || NET35 || NET40 || UNITY
namespace System.Runtime.CompilerServices
{
	[AttributeUsage( AttributeTargets.Parameter, Inherited = false )]
	internal sealed class CallerMemberNameAttribute : Attribute { }
}
#endif // SILVERLIGHT || NET35 || NET40 || UNITY
#endif // TRACING
