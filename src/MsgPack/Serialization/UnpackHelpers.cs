#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
#if DEBUG && !NETFX_CORE
#define TRACING
#endif // DEBUG && !NETFX_CORE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif //!UNITY || MSGPACK_UNITY_FULL
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Reflection;
using System.Runtime.CompilerServices;

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

#if !UNITY && DEBUG
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
			Contract.Assert( array != null );
#endif // !UNITY && DEBUG

			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

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

#if !UNITY && DEBUG
			Contract.Assert( unpacker != null );
			Contract.Assert( collection != null );
			Contract.Assert( addition != null );
#endif // !UNITY && DEBUG

			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

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

#if !UNITY && DEBUG
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
			Contract.Assert( collection != null );
			Contract.Assert( addition != null );
#endif // !UNITY && DEBUG

			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

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

#if !UNITY && DEBUG
			Contract.Assert( unpacker != null );
			Contract.Assert( collection != null );
			Contract.Assert( addition != null );
#endif // !UNITY && DEBUG

			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

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

#if !UNITY && DEBUG
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
			Contract.Assert( collection != null );
			Contract.Assert( addition != null );
#endif // !UNITY && DEBUG

			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

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
				throw new ArgumentNullException( "dictionary" );
			}

#if !UNITY && DEBUG
			Contract.Assert( unpacker != null );
			Contract.Assert( keySerializer != null );
			Contract.Assert( valueSerializer != null );
			Contract.Assert( dictionary != null );
#endif // !UNITY && DEBUG

			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotMapHeader( unpacker );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

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

#if !UNITY && DEBUG
			Contract.Assert( unpacker != null );
			Contract.Assert( dictionary != null );
#endif // !UNITY && DEBUG

			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotMapHeader( unpacker );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

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
		public static T InvokeUnpackFrom<T>( MessagePackSerializer<T> serializer, Unpacker unpacker )
		{
			if ( serializer == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "serializer" );
			}

#if !UNITY && DEBUG
			Contract.Assert( unpacker != null );
			Contract.Assert( serializer != null );
#endif // !UNITY && DEBUG

			return serializer.UnpackFromCore( unpacker );
		}

		/// <summary>
		///		Retrieves a most appropriate constructor with <see cref="Int32"/> capacity parameter and <see cref="IEqualityComparer{T}"/> comparer parameter or both of them, >or default constructor of the <paramref name="instanceType"/>.
		/// </summary>
		/// <param name="instanceType">The target collection type to be instanciated.</param>
		/// <returns>A constructor of the <paramref name="instanceType"/>.</returns>
		internal static ConstructorInfo GetCollectionConstructor( Type instanceType )
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

			if ( constructor == null )
			{
				SerializationExceptions.ThrowTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity( instanceType );
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
#if DEBUG && !UNITY
			Contract.Assert( !type.GetIsGenericTypeDefinition(), "!(" + type + ").GetIsGenericTypeDefinition()" );
#endif // DEBUG && !UNITY

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
		///		Unpacks object from msgpack array.
		/// </summary>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which holds intermediate states. This value may be <c>null</c> when the caller implementation allows it.</param>
		/// <param name="factory">A delegate to the factory method which creates the result from the context.</param>
		/// <param name="itemNames">The names of the membesr for pretty exception.</param>
		/// <param name="operations">
		///		Delegates each ones unpack single member in order.
		///		The 1st argument will be <paramref name="unpacker"/>, 2nd argument will be <paramref name="context"/>,
		///		and 3rd argument is index of current item.
		/// </param>
		/// <returns>The unpacked object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="factory"/> is <c>null</c>.
		///		Or, <paramref name="operations"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static TResult UnpackFromArray<TContext, TResult>(
			Unpacker unpacker,
			TContext context,
			Func<TContext, TResult> factory,
			IList<string> itemNames,
			IList<Action<Unpacker, TContext, int>> operations
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( factory == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "factory" );
			}

			if ( operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "operations" );
			}

			var count = GetItemsCount( unpacker );

#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( factory != null );
			Contract.Assert( operations != null );
#endif // DEBUG && !UNITY

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			var limit = Math.Min( count, operations.Count );
			for ( var i = 0; i < limit; i++ )
			{
				operations[ i ]( unpacker, context, i );
				Trace( ctx, "ReadItem", unpacker, i, itemNames );
			}

			if ( count > limit )
			{
				for ( var i = limit; i < count; i++ )
				{
					unpacker.Read();
				}
			}

			return factory( context );
		}

		/// <summary>
		///		Unpacks object from msgpack map.
		/// </summary>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <typeparam name="TResult">The type of the unpacked object.</typeparam>
		/// <param name="unpacker">The unpacker.</param>
		/// <param name="context">The context which holds intermediate states. This value may be <c>null</c> when the caller implementation allows it.</param>
		/// <param name="factory">A delegate to the factory method which creates the result from the context.</param>
		/// <param name="operations">
		///		Delegates table each ones unpack single member and their keys correspond to unpacking membmer names.
		///		The 1st argument will be <paramref name="unpacker"/>, 2nd argument will be <paramref name="context"/>,
		///		and 3rd argument is index of current key value pair assuming map is sequence of key value pairs.
		/// </param>
		/// <returns>The unpacked object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or, <paramref name="factory"/> is <c>null</c>.
		///		Or, <paramref name="operations"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static TResult UnpackFromMap<TContext, TResult>(
			Unpacker unpacker,
			TContext context,
			Func<TContext, TResult> factory,
			IDictionary<string, Action<Unpacker, TContext, int>> operations
		)
		{
			if ( unpacker == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "unpacker" );
			}

			if ( factory == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "factory" );
			}

			if ( operations == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "operations" );
			}

#if DEBUG && !UNITY
			Contract.Assert( unpacker != null );
			Contract.Assert( factory != null );
			Contract.Assert( operations != null );
#endif // DEBUG && !UNITY

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			var count = GetItemsCount( unpacker );
			for ( var i = 0; i < count; i++ )
			{
				var key = UnpackStringValue( unpacker, typeof( TResult ), "MemberName" );
				Trace( ctx, "ReadKey", unpacker, i, key );

				Action<Unpacker, TContext, int> operation;
				if ( key != null && operations.TryGetValue( key, out operation ) )
				{
					operation( unpacker, context, i );
					Trace( ctx, "ReadValue", unpacker, i, key );
				}
				else
				{
					// skip unknown item.
					unpacker.Skip();
					Trace( ctx, "Skip", unpacker, i, key ?? "(null)" );
				}
			}

			return factory( context );
		}

		/// <summary>
		///		Unpacks the collection from msgpack stream.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="unpacker">The unpacker where position is located at array or map header.</param>
		/// <param name="itemsCount">The collection count gotten from the <paramref name="unpacker"/>.</param>
		/// <param name="collection">The collection instance to be added unpacked items.</param>
		/// <param name="bulkOperation">
		///		A delegate to the bulk operation (typically UnpackToCore call). 
		///		The 1st argument will be <paramref name="unpacker"/>, 2nd argument will be <paramref name="collection"/>,
		///		and 3rd argument will be <paramref name="itemsCount"/>.
		///		If this parameter is <c>null</c>, <paramref name="eachOperation"/> will be used.
		/// </param>
		/// <param name="eachOperation">
		///		A delegate to the operation for each items, which typically unpack value and append it to the <paramref name="collection"/>.
		///		The 1st argument will be <paramref name="unpacker"/>, 2nd argument will be <paramref name="collection"/>,
		///		and 3rd argument will be index of the current item.
		///		If <paramref name="bulkOperation"/> parameter is not <c>null</c>, this parameter will be ignored.
		/// </param>
		/// <returns></returns>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static T UnpackCollection<T>( Unpacker unpacker, int itemsCount, T collection, Action<Unpacker, T, int> bulkOperation, Action<Unpacker, T, int> eachOperation )
		{
			if ( collection == null )
			{
				SerializationExceptions.ThrowArgumentNullException( "collection" );
			}

			// ReSharper disable once RedundantAssignment
			var ctx = default( UnpackerTraceContext );
			InitializeUnpackerTrace( unpacker, ref ctx );

			if ( bulkOperation != null )
			{
				bulkOperation( unpacker, collection, itemsCount );

				Trace( ctx, "UnpackTo", unpacker );
			}
			else
			{
				if ( eachOperation == null )
				{
					SerializationExceptions.ThrowArgumentException( "bulkOperation or eachOperation must not be null." );
				}

#if DEBUG && !UNITY
				Contract.Assert( eachOperation != null );
#endif // DEBUG && !UNITY

				for ( var i = 0; i < itemsCount; i++ )
				{
					eachOperation( unpacker, collection, i );
					Trace( ctx, "ReadItem", unpacker, i );
				}
			}

			return collection;
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

		[Conditional( "TRACING" )]
		[MethodImpl( MethodImplOptions.NoInlining )]
		// ReSharper disable once RedundantAssignment
		private static void InitializeUnpackerTrace( Unpacker unpacker, ref UnpackerTraceContext context )
		{
			long positionOrOffset;
			unpacker.GetPreviousPosition( out positionOrOffset );
#if !NETFX_CORE
			context = new UnpackerTraceContext( positionOrOffset, new StackFrame( 1 ).GetMethod().Name );
#endif // !NETFX_CORE
		}

		[Conditional( "TRACING" )]
		private static void Trace( UnpackerTraceContext context, string label, Unpacker unpacker )
		{
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
		}

		[Conditional( "TRACING" )]
		private static void Trace( UnpackerTraceContext context, string label, Unpacker unpacker, int index )
		{
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
		}

		[Conditional( "TRACING" )]
		private static void Trace( UnpackerTraceContext context, string label, Unpacker unpacker, int index, IList<string> itemNames )
		{
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
		}

		[Conditional( "TRACING" )]
		private static void Trace( UnpackerTraceContext context, string label, Unpacker unpacker, int index, string key )
		{
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
		}

		[Conditional( "TRACING" )]
		private static void Trace( UnpackerTraceContext context, string label, Unpacker unpacker, string key )
		{
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
		}

		[Conditional( "TRACING" )]
		private static void TraceCore( string format, params object[] args )
		{
#if !UNITY && !SILVERLIGHT && !NETFX_CORE && !XAMIOS && !XAMDROID
			Tracer.Tracing.TraceEvent( Tracer.EventType.Trace, Tracer.EventId.Trace, format, args );
#endif // !UNITY && !SILVERLIGHT && !NETFX_CORE && !XAMIOS && !XAMDROID
		}

		private sealed class UnpackerTraceContext
		{
			public long PositionOrOffset { get; set; }
			public string MethodName { get; private set; }

			public UnpackerTraceContext( long positionOrOffset, string methodName )
			{
				this.PositionOrOffset = positionOrOffset;
				this.MethodName = methodName;
			}
		}
	}
}
