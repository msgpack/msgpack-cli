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

using System;
using System.Collections.Generic;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Linq;
#if NETFX_CORE
using System.Reflection;
#endif
using MsgPack.Serialization;
using System.Collections;

namespace MsgPack
{
	/// <summary>
	///		Defines extension method to pack or unpack various objects.
	/// </summary>
	public static class PackerUnpackerExtensions
	{
		/// <summary>
		///		Packs specified value with the default context.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static Packer Pack<T>( this Packer source, T value )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			PackCore( source, value, SerializationContext.Default );
			return source;
		}

		/// <summary>
		///		Packs specified value with the specified context.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static Packer Pack<T>( this Packer source, T value, SerializationContext context )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			PackCore( source, value, context );
			return source;
		}

		private static void PackCore<T>( Packer source, T value, SerializationContext context )
		{
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if ( value == null )
			// ReSharper restore CompareNonConstrainedGenericWithNull
			{
				source.PackNull();
				return;
			}

			var asPackable = value as IPackable;
			if ( asPackable != null )
			{
				asPackable.PackToMessage( source, new PackingOptions() );
				return;
			}

			context.GetSerializer<T>().PackTo( source, value );
		}

		/// <summary>
		///		Packs specified collection with the default context.
		/// </summary>
		/// <typeparam name="T">The type of items of the collection.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="collection">The collection to be serialized.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="collection"/>.
		/// </exception>
		public static Packer PackArray<T>( this Packer source, IEnumerable<T> collection )
		{
			PackCollectionCore( source, collection, SerializationContext.Default );
			return source;
		}

		/// <summary>
		///		Packs specified collection with the specified context.
		/// </summary>
		/// <typeparam name="T">The type of items of the collection.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="collection">The collection to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="collection"/>.
		/// </exception>
		public static Packer PackArray<T>( this Packer source, IEnumerable<T> collection, SerializationContext context )
		{
			PackCollectionCore( source, collection, context );
			return source;
		}

		/// <summary>
		///		Packs specified collection with the default context.
		/// </summary>
		/// <typeparam name="T">The type of items of the collection.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="collection">The collection to be serialized.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="collection"/>.
		/// </exception>
		public static Packer PackCollection<T>( this Packer source, IEnumerable<T> collection )
		{
			PackCollectionCore( source, collection, SerializationContext.Default );
			return source;
		}

		/// <summary>
		///		Packs specified collection with the specified context.
		/// </summary>
		/// <typeparam name="T">The type of items of the collection.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="collection">The collection to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="collection"/>.
		/// </exception>
		public static Packer PackCollection<T>( this Packer source, IEnumerable<T> collection, SerializationContext context )
		{
			PackCollectionCore( source, collection, context );
			return source;
		}

		private static void PackCollectionCore<T>( Packer source, IEnumerable<T> collection, SerializationContext context )
		{
			PackCollectionCore( source, collection, context.GetSerializer<T>() );
		}

		internal static void PackCollectionCore<T>( Packer source, IEnumerable<T> collection, MessagePackSerializer<T> itemSerializer )
		{
			// ReSharper disable once CompareNonConstrainedGenericWithNull
			if ( collection == null )
			{
				source.PackNull();
				return;
			}

			// ReSharper disable once SuspiciousTypeConversion.Global
			var asPackable = collection as IPackable;
			if ( asPackable != null )
			{
				asPackable.PackToMessage( source, new PackingOptions() );
				return;
			}

			int count;
			ICollection<T> asCollectionT;
			ICollection asCollection;
			if ( ( asCollectionT = collection as ICollection<T> ) != null )
			{
				count = asCollectionT.Count;
			}
			else if ( ( asCollection = collection as ICollection ) != null )
			{
				count = asCollection.Count;
			}
			else
			{
				var asArray = collection.ToArray();
				count = asArray.Length;
				collection = asArray;
			}

			source.PackArrayHeader( count );
			foreach ( var item in collection )
			{
				itemSerializer.PackTo( source, item );
			}
		}

		/// <summary>
		///		Packs specified dictionary with the default context.
		/// </summary>
		/// <typeparam name="TKey">The type of keys of the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values of the dictionary.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="dictionary">The dictionary to be serialized.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="dictionary"/>.
		/// </exception>
		public static Packer PackMap<TKey, TValue>( this Packer source, IDictionary<TKey, TValue> dictionary )
		{
			PackDictionaryCore( source, dictionary, SerializationContext.Default );
			return source;
		}

		/// <summary>
		///		Packs specified dictionary with the specified context.
		/// </summary>
		/// <typeparam name="TKey">The type of keys of the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values of the dictionary.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="dictionary">The dictionary to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="dictionary"/>.
		/// </exception>
		public static Packer PackMap<TKey, TValue>( this Packer source, IDictionary<TKey, TValue> dictionary, SerializationContext context )
		{
			PackDictionaryCore( source, dictionary, context );
			return source;
		}

		/// <summary>
		///		Packs specified dictionary with the default context.
		/// </summary>
		/// <typeparam name="TKey">The type of keys of the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values of the dictionary.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="dictionary">The dictionary to be serialized.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="dictionary"/>.
		/// </exception>
		public static Packer PackDictionary<TKey, TValue>( this Packer source, IDictionary<TKey, TValue> dictionary )
		{
			PackDictionaryCore( source, dictionary, SerializationContext.Default );
			return source;
		}

		/// <summary>
		///		Packs specified dictionary with the specified context.
		/// </summary>
		/// <typeparam name="TKey">The type of keys of the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values of the dictionary.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="dictionary">The dictionary to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="dictionary"/>.
		/// </exception>
		public static Packer PackDictionary<TKey, TValue>( this Packer source, IDictionary<TKey, TValue> dictionary, SerializationContext context )
		{
			PackDictionaryCore( source, dictionary, context );
			return source;
		}

		private static void PackDictionaryCore<TKey, TValue>(
			Packer source,
			IDictionary<TKey, TValue> dictionary,
			SerializationContext context )
		{
			PackDictionaryCore( source, dictionary, context.GetSerializer<TKey>(), context.GetSerializer<TValue>() );
		}

		internal static void PackDictionaryCore<TKey, TValue>(
			Packer source,
			IDictionary<TKey, TValue> dictionary,
			MessagePackSerializer<TKey> keySerializer,
			MessagePackSerializer<TValue> valueSerializer )
		{
			// ReSharper disable once CompareNonConstrainedGenericWithNull
			if ( dictionary == null )
			{
				source.PackNull();
				return;
			}

			// ReSharper disable once SuspiciousTypeConversion.Global
			var asPackable = dictionary as IPackable;
			if ( asPackable != null )
			{
				asPackable.PackToMessage( source, new PackingOptions() );
				return;
			}

			source.PackMapHeader( dictionary.Count );
			foreach ( var entry in dictionary )
			{
				keySerializer.PackTo( source, entry.Key );
				valueSerializer.PackTo( source, entry.Value );
			}
		}

		/// <summary>
		///		Packs specified collection with the default context.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="items">The collection to be serialized.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize the item of <paramref name="items"/>.
		/// </exception>
		public static Packer Pack<T>( this Packer source, IEnumerable<T> items )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			PackCore( source, items, SerializationContext.Default );
			return source;
		}

		/// <summary>
		///		Packs specified value with the specified context.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="items">The collection to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize the item of <paramref name="items"/>.
		/// </exception>
		public static Packer Pack<T>( this Packer source, IEnumerable<T> items, SerializationContext context )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			PackCore( source, items, context );
			return source;
		}

		private static void PackCore<T>( this Packer source, IEnumerable<T> items, SerializationContext context )
		{
			if ( items == null )
			{
				source.PackNull();
				return;
			}

			// ReSharper disable SuspiciousTypeConversion.Global
			var asPackable = items as IPackable;
			// ReSharper restore SuspiciousTypeConversion.Global
			if ( asPackable != null )
			{
				asPackable.PackToMessage( source, new PackingOptions() );
				return;
			}

			var asCollection = items as ICollection<T>;
			if ( asCollection == null )
			{
				asCollection = items.ToArray();
			}

			var itemSerializer = context.GetSerializer<T>();

			source.PackArrayHeader( asCollection.Count );
			foreach ( var item in asCollection )
			{
				itemSerializer.PackTo( source, item );
			}
		}


		/// <summary>
		///		Packs specified value with the default context.
		/// </summary>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static Packer PackObject( this Packer source, object value )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			PackObjectCore( source, value, SerializationContext.Default );
			return source;
		}

		/// <summary>
		///		Packs specified value with the specified context.
		/// </summary>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns><paramref name="source"/>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static Packer PackObject( this Packer source, object value, SerializationContext context )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			PackObjectCore( source, value, context );
			return source;
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

			context.GetSerializer( type ).PackTo( source, value );
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

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


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

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			return UnpackCore<T>( source, context );
		}

		private static T UnpackCore<T>( Unpacker source, SerializationContext context )
		{
			return context.GetSerializer<T>().UnpackFrom( source );
		}
	}
}
