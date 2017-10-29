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

using System;
using System.Collections;
using System.Collections.Generic;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Linq;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization;

namespace MsgPack
{
	// This file was generated from PackerUnpackerExtensions.tt T4Template.
	// Do not modify this file. Edit PackerUnpackerExtensions.tt instead.

	/// <summary>
	///		Defines extension method to pack or unpack various objects.
	/// </summary>
	public static class PackerUnpackerExtensions
	{
		#region -- Pack<T> --

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

			Contract.EndContractBlock();

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

			Contract.EndContractBlock();

			PackCore( source, value, context );
			return source;
		}


		private static void PackCore<T>( Packer source, T value, SerializationContext context )
		{
			// ReSharper disable once CompareNonConstrainedGenericWithNull
			if ( value == null )
			{
				source.PackNull();
				return;
			}

			// For 0.6.5 compatibility
			var asPackable = value as IPackable;
			if ( asPackable != null )
			{
				asPackable.PackToMessage( source, new PackingOptions() );
				return;
			}

			context.GetSerializer<T>().PackTo( source, value );
		}


#if FEATURE_TAP

		/// <summary>
		///		Packs specified value with the default context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static Task PackAsync<T>( this Packer source, T value )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return PackAsyncCore( source, value, SerializationContext.Default, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified value with the default context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static Task PackAsync<T>( this Packer source, T value, CancellationToken cancellationToken  )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return PackAsyncCore( source, value, SerializationContext.Default, cancellationToken );
		}

		/// <summary>
		///		Packs specified value with the specified context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static Task PackAsync<T>( this Packer source, T value, SerializationContext context )
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

			return PackAsyncCore( source, value, context, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified value with the specified context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static Task PackAsync<T>( this Packer source, T value, SerializationContext context, CancellationToken cancellationToken  )
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

			return PackAsyncCore( source, value, context, cancellationToken );
		}


		private static async Task PackAsyncCore<T>( Packer source, T value, SerializationContext context, CancellationToken cancellationToken  )
		{
			// ReSharper disable once CompareNonConstrainedGenericWithNull
			if ( value == null )
			{
				await source.PackNullAsync( cancellationToken ).ConfigureAwait( false );
				return;
			}

			var asAsyncPackable = value as IAsyncPackable;
			if ( asAsyncPackable != null )
			{
				await asAsyncPackable.PackToMessageAsync( source, new PackingOptions(), cancellationToken ).ConfigureAwait( false );
				return;
			}

			var asPackable = value as IPackable;
			if ( asPackable != null )
			{
				await Task.Run( () => asPackable.PackToMessage( source, new PackingOptions() ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await context.GetSerializer<T>().PackToAsync( source, value, cancellationToken ).ConfigureAwait( false );
		}


#endif // FEATURE_TAP

		#endregion -- Pack<T> --

		#region -- PackArray<T>/PackCollection<T> --

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
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

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
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			Contract.EndContractBlock();

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
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

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
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			Contract.EndContractBlock();

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


#if FEATURE_TAP

		/// <summary>
		///		Packs specified collection with the default context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of items of the collection.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="collection">The collection to be serialized.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="collection"/>.
		/// </exception>
		public static Task PackArrayAsync<T>( this Packer source, IEnumerable<T> collection )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return PackCollectionAsyncCore( source, collection, SerializationContext.Default, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified collection with the default context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of items of the collection.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="collection">The collection to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="collection"/>.
		/// </exception>
		public static Task PackArrayAsync<T>( this Packer source, IEnumerable<T> collection, CancellationToken cancellationToken  )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return PackCollectionAsyncCore( source, collection, SerializationContext.Default, cancellationToken );
		}

		/// <summary>
		///		Packs specified collection with the specified context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of items of the collection.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="collection">The collection to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="collection"/>.
		/// </exception>
		public static Task PackArrayAsync<T>( this Packer source, IEnumerable<T> collection, SerializationContext context )
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

			return PackCollectionAsyncCore( source, collection, context, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified collection with the specified context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of items of the collection.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="collection">The collection to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="collection"/>.
		/// </exception>
		public static Task PackArrayAsync<T>( this Packer source, IEnumerable<T> collection, SerializationContext context, CancellationToken cancellationToken  )
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

			return PackCollectionAsyncCore( source, collection, context, cancellationToken );
		}

		/// <summary>
		///		Packs specified collection with the default context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of items of the collection.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="collection">The collection to be serialized.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="collection"/>.
		/// </exception>
		public static Task PackCollectionAsync<T>( this Packer source, IEnumerable<T> collection )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return PackCollectionAsyncCore( source, collection, SerializationContext.Default, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified collection with the default context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of items of the collection.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="collection">The collection to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="collection"/>.
		/// </exception>
		public static Task PackCollectionAsync<T>( this Packer source, IEnumerable<T> collection, CancellationToken cancellationToken  )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return PackCollectionAsyncCore( source, collection, SerializationContext.Default, cancellationToken );
		}

		/// <summary>
		///		Packs specified collection with the specified context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of items of the collection.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="collection">The collection to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="collection"/>.
		/// </exception>
		public static Task PackCollectionAsync<T>( this Packer source, IEnumerable<T> collection, SerializationContext context )
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

			return PackCollectionAsyncCore( source, collection, context, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified collection with the specified context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of items of the collection.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="collection">The collection to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="collection"/>.
		/// </exception>
		public static Task PackCollectionAsync<T>( this Packer source, IEnumerable<T> collection, SerializationContext context, CancellationToken cancellationToken  )
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

			return PackCollectionAsyncCore( source, collection, context, cancellationToken );
		}


		private static Task PackCollectionAsyncCore<T>( Packer source, IEnumerable<T> collection, SerializationContext context, CancellationToken cancellationToken  )
		{
			return PackCollectionAsyncCore( source, collection, context.GetSerializer<T>(), cancellationToken );
		}

		internal static async Task PackCollectionAsyncCore<T>( Packer source, IEnumerable<T> collection, MessagePackSerializer<T> itemSerializer, CancellationToken cancellationToken  )
		{
			// ReSharper disable once CompareNonConstrainedGenericWithNull
			if ( collection == null )
			{
				await source.PackNullAsync( cancellationToken ).ConfigureAwait( false );
				return;
			}

			// ReSharper disable once SuspiciousTypeConversion.Global
			var asAsyncPackable = collection as IAsyncPackable;
			if ( asAsyncPackable != null )
			{
				await asAsyncPackable.PackToMessageAsync( source, new PackingOptions(), cancellationToken ).ConfigureAwait( false );
				return;
			}

			// ReSharper disable once SuspiciousTypeConversion.Global
			var asPackable = collection as IPackable;
			if ( asPackable != null )
			{
				await Task.Run( () => asPackable.PackToMessage( source, new PackingOptions() ), cancellationToken ).ConfigureAwait( false );
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

			await source.PackArrayHeaderAsync( count, cancellationToken ).ConfigureAwait( false );
			foreach ( var item in collection )
			{
				await itemSerializer.PackToAsync( source, item, cancellationToken ).ConfigureAwait( false );
			}
		}


#endif // FEATURE_TAP

		#endregion -- PackArray<T>/PackCollection<T> --

		#region -- PackMap<T>/PackDictionary<T> --

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
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

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
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			Contract.EndContractBlock();

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
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

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
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			Contract.EndContractBlock();

			PackDictionaryCore( source, dictionary, context );
			return source;
		}


		private static void PackDictionaryCore<TKey, TValue>( 
			Packer source, IDictionary<TKey, TValue> dictionary, 
			SerializationContext context
		)
		{
			PackDictionaryCore( source, dictionary, context.GetSerializer<TKey>(), context.GetSerializer<TValue>() );
		}

		internal static void PackDictionaryCore<TKey, TValue>( 
			Packer source, IDictionary<TKey, TValue> dictionary, 
			MessagePackSerializer<TKey> keySerializer, MessagePackSerializer<TValue> valueSerializer
		)
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
			foreach ( var item in dictionary )
			{
				keySerializer.PackTo( source, item.Key );
				valueSerializer.PackTo( source, item.Value );
			}
		}


#if FEATURE_TAP

		/// <summary>
		///		Packs specified dictionary with the default context asynchronously.
		/// </summary>
		/// <typeparam name="TKey">The type of keys of the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values of the dictionary.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="dictionary">The dictionary to be serialized.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="dictionary"/>.
		/// </exception>
		public static Task PackMapAsync<TKey, TValue>( this Packer source, IDictionary<TKey, TValue> dictionary )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return PackDictionaryAsyncCore( source, dictionary, SerializationContext.Default, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified dictionary with the default context asynchronously.
		/// </summary>
		/// <typeparam name="TKey">The type of keys of the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values of the dictionary.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="dictionary">The dictionary to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="dictionary"/>.
		/// </exception>
		public static Task PackMapAsync<TKey, TValue>( this Packer source, IDictionary<TKey, TValue> dictionary, CancellationToken cancellationToken  )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return PackDictionaryAsyncCore( source, dictionary, SerializationContext.Default, cancellationToken );
		}

		/// <summary>
		///		Packs specified dictionary with the specified context asynchronously.
		/// </summary>
		/// <typeparam name="TKey">The type of keys of the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values of the dictionary.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="dictionary">The dictionary to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="dictionary"/>.
		/// </exception>
		public static Task PackMapAsync<TKey, TValue>( this Packer source, IDictionary<TKey, TValue> dictionary, SerializationContext context )
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

			return PackDictionaryAsyncCore( source, dictionary, context, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified dictionary with the specified context asynchronously.
		/// </summary>
		/// <typeparam name="TKey">The type of keys of the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values of the dictionary.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="dictionary">The dictionary to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="dictionary"/>.
		/// </exception>
		public static Task PackMapAsync<TKey, TValue>( this Packer source, IDictionary<TKey, TValue> dictionary, SerializationContext context, CancellationToken cancellationToken  )
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

			return PackDictionaryAsyncCore( source, dictionary, context, cancellationToken );
		}

		/// <summary>
		///		Packs specified dictionary with the default context asynchronously.
		/// </summary>
		/// <typeparam name="TKey">The type of keys of the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values of the dictionary.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="dictionary">The dictionary to be serialized.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="dictionary"/>.
		/// </exception>
		public static Task PackDictionaryAsync<TKey, TValue>( this Packer source, IDictionary<TKey, TValue> dictionary )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return PackDictionaryAsyncCore( source, dictionary, SerializationContext.Default, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified dictionary with the default context asynchronously.
		/// </summary>
		/// <typeparam name="TKey">The type of keys of the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values of the dictionary.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="dictionary">The dictionary to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="dictionary"/>.
		/// </exception>
		public static Task PackDictionaryAsync<TKey, TValue>( this Packer source, IDictionary<TKey, TValue> dictionary, CancellationToken cancellationToken  )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return PackDictionaryAsyncCore( source, dictionary, SerializationContext.Default, cancellationToken );
		}

		/// <summary>
		///		Packs specified dictionary with the specified context asynchronously.
		/// </summary>
		/// <typeparam name="TKey">The type of keys of the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values of the dictionary.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="dictionary">The dictionary to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="dictionary"/>.
		/// </exception>
		public static Task PackDictionaryAsync<TKey, TValue>( this Packer source, IDictionary<TKey, TValue> dictionary, SerializationContext context )
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

			return PackDictionaryAsyncCore( source, dictionary, context, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified dictionary with the specified context asynchronously.
		/// </summary>
		/// <typeparam name="TKey">The type of keys of the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values of the dictionary.</typeparam>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="dictionary">The dictionary to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="dictionary"/>.
		/// </exception>
		public static Task PackDictionaryAsync<TKey, TValue>( this Packer source, IDictionary<TKey, TValue> dictionary, SerializationContext context, CancellationToken cancellationToken  )
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

			return PackDictionaryAsyncCore( source, dictionary, context, cancellationToken );
		}


		private static Task PackDictionaryAsyncCore<TKey, TValue>( 
			Packer source, IDictionary<TKey, TValue> dictionary, 
			SerializationContext context, CancellationToken cancellationToken 
		)
		{
			return PackDictionaryAsyncCore( source, dictionary, context.GetSerializer<TKey>(), context.GetSerializer<TValue>(), cancellationToken );
		}

		internal static async Task PackDictionaryAsyncCore<TKey, TValue>( 
			Packer source, IDictionary<TKey, TValue> dictionary, 
			MessagePackSerializer<TKey> keySerializer, MessagePackSerializer<TValue> valueSerializer, CancellationToken cancellationToken 
		)
		{
			// ReSharper disable once CompareNonConstrainedGenericWithNull
			if ( dictionary == null )
			{
				await source.PackNullAsync( cancellationToken ).ConfigureAwait( false );
				return;
			}

			// ReSharper disable once SuspiciousTypeConversion.Global
			var asAsyncPackable = dictionary as IAsyncPackable;
			if ( asAsyncPackable != null )
			{
				await asAsyncPackable.PackToMessageAsync( source, new PackingOptions(), cancellationToken ).ConfigureAwait( false );
				return;
			}

			// ReSharper disable once SuspiciousTypeConversion.Global
			var asPackable = dictionary as IPackable;
			if ( asPackable != null )
			{
				await Task.Run( () => asPackable.PackToMessage( source, new PackingOptions() ), cancellationToken ).ConfigureAwait( false );
				return;
			}

			await source.PackMapHeaderAsync( dictionary.Count, cancellationToken ).ConfigureAwait( false );
			foreach ( var item in dictionary )
			{
				await keySerializer.PackToAsync( source, item.Key, cancellationToken ).ConfigureAwait( false );
				await valueSerializer.PackToAsync( source, item.Value, cancellationToken ).ConfigureAwait( false );
			}
		}


#endif // FEATURE_TAP

		#endregion -- PackMap<T>/PackDictionary<T> --

		#region -- Pack<T> of IEnumerable --

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
		[Obsolete( "Use PackArray<T>, PackCollection<T>, PackMap<TKey, TValue>, or PackDictionary<TKey, TValue> instead." )]
		public static Packer Pack<T>( this Packer source, IEnumerable<T> items )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

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
		[Obsolete( "Use PackArray<T>, PackCollection<T>, PackMap<TKey, TValue>, or PackDictionary<TKey, TValue> instead." )]
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

			Contract.EndContractBlock();

			PackCollectionCore( source, items, context );
			return source;
		}

		#endregion -- Pack<T> of IEnumerable --

		#region -- PackObject --
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

			Contract.EndContractBlock();

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

			Contract.EndContractBlock();

			PackObjectCore( source, value, context );
			return source;
		}


		private static void PackObjectCore( Packer source, object value, SerializationContext context )
		{
			if ( value == null )
			{
				source.PackNull();
				return;
			}

			context.GetSerializer( value.GetType() ).PackTo( source, value );
		}


#if FEATURE_TAP

		/// <summary>
		///		Packs specified value with the default context asynchronously.
		/// </summary>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static Task PackObjectAsync( this Packer source, object value )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return PackObjectAsyncCore( source, value, SerializationContext.Default, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified value with the default context asynchronously.
		/// </summary>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static Task PackObjectAsync( this Packer source, object value, CancellationToken cancellationToken  )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return PackObjectAsyncCore( source, value, SerializationContext.Default, cancellationToken );
		}

		/// <summary>
		///		Packs specified value with the specified context asynchronously.
		/// </summary>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static Task PackObjectAsync( this Packer source, object value, SerializationContext context )
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

			return PackObjectAsyncCore( source, value, context, CancellationToken.None );
		}

		/// <summary>
		///		Packs specified value with the specified context asynchronously.
		/// </summary>
		/// <param name="source">The <see cref="Packer"/>.</param>
		/// <param name="value">The value to be serialized.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot serialize <paramref name="value"/>.
		/// </exception>
		public static Task PackObjectAsync( this Packer source, object value, SerializationContext context, CancellationToken cancellationToken  )
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

			return PackObjectAsyncCore( source, value, context, cancellationToken );
		}


		private static async Task PackObjectAsyncCore( Packer source, object value, SerializationContext context, CancellationToken cancellationToken  )
		{
			if ( value == null )
			{
				await source.PackNullAsync( cancellationToken ).ConfigureAwait( false );
				return;
			}

			await context.GetSerializer( value.GetType() ).PackToAsync( source, value, cancellationToken ).ConfigureAwait( false );
		}


#endif // FEATURE_TAP

		#endregion -- PackObject --

		#region -- Unpack<T> --

		/// <summary>
		///		Unpacks specified type value with the default context.
		/// </summary>
		/// <typeparam name="T">The type of the deserializing object.</typeparam>
		/// <param name="source">The <see cref="Unpacker"/>.</param>
		/// <returns>The deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot deserialize object.
		/// </exception>
		public static T Unpack<T>( this Unpacker source )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return UnpackCore<T>( source, SerializationContext.Default );
		}

		/// <summary>
		///		Unpacks specified type value with the specified context.
		/// </summary>
		/// <typeparam name="T">The type of the deserializing object.</typeparam>
		/// <param name="source">The <see cref="Unpacker"/>.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns>The deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot deserialize object.
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
			return context.GetSerializer<T>().UnpackFrom( source );
		}


#if FEATURE_TAP

		/// <summary>
		///		Unpacks specified type value with the default context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of the deserializing object.</typeparam>
		/// <param name="source">The <see cref="Unpacker"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation. The value of the <c>TResult</c> parameter contains a deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot deserialize object.
		/// </exception>
		public static Task<T> UnpackAsync<T>( this Unpacker source )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return UnpackAsyncCore<T>( source, SerializationContext.Default, CancellationToken.None );
		}

		/// <summary>
		///		Unpacks specified type value with the default context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of the deserializing object.</typeparam>
		/// <param name="source">The <see cref="Unpacker"/>.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation. The value of the <c>TResult</c> parameter contains a deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot deserialize object.
		/// </exception>
		public static Task<T> UnpackAsync<T>( this Unpacker source, CancellationToken cancellationToken  )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			Contract.EndContractBlock();

			return UnpackAsyncCore<T>( source, SerializationContext.Default, cancellationToken );
		}

		/// <summary>
		///		Unpacks specified type value with the specified context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of the deserializing object.</typeparam>
		/// <param name="source">The <see cref="Unpacker"/>.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation. The value of the <c>TResult</c> parameter contains a deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot deserialize object.
		/// </exception>
		public static Task<T> UnpackAsync<T>( this Unpacker source, SerializationContext context )
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

			return UnpackAsyncCore<T>( source, context, CancellationToken.None );
		}

		/// <summary>
		///		Unpacks specified type value with the specified context asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of the deserializing object.</typeparam>
		/// <param name="source">The <see cref="Unpacker"/>.</param>
		/// <param name="context">The <see cref="SerializationContext"/> holds shared serializers.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation. The value of the <c>TResult</c> parameter contains a deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///		Or <paramref name="context"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Cannot deserialize object.
		/// </exception>
		public static Task<T> UnpackAsync<T>( this Unpacker source, SerializationContext context, CancellationToken cancellationToken  )
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

			return UnpackAsyncCore<T>( source, context, cancellationToken );
		}


		private static async Task<T> UnpackAsyncCore<T>( Unpacker source, SerializationContext context, CancellationToken cancellationToken  )
		{
			return await context.GetSerializer<T>().UnpackFromAsync( source, cancellationToken ).ConfigureAwait( false );
		}


#endif // FEATURE_TAP

		#endregion -- Unpack<T> --
	}
}
