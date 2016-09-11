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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	[Preserve( AllMembers = true )]
	internal sealed class ImmutableDictionarySerializer<T, TKey, TValue> : MessagePackSerializer<T>
		where T : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		private readonly Func<KeyValuePair<TKey, TValue>[], T> _factory;

		private static Func<KeyValuePair<TKey, TValue>[], T> FindFactory()
		{
			var factoryType =
				typeof( T ).GetAssembly().GetType(
					typeof( T ).FullName.Remove( typeof( T ).FullName.IndexOf( '`' ) )
				);

			if ( factoryType == null )
			{
				return
					_ =>
					{
						throw new NotSupportedException(
							String.Format(
								CultureInfo.CurrentCulture,
								"'{0}' may not be an immutable collection.",
								typeof( T ).AssemblyQualifiedName
							)
						);
					};
			}

			var methods =
				factoryType
				.GetMethods()
				.Where(
					m =>
					m.IsStatic
					&& m.IsPublic
					&& m.IsGenericMethod
					&& m.Name == "CreateRange"
					&& m.GetParameters().Length == 1
				);

			var method = methods.FirstOrDefault();

			if ( method == null )
			{
				return
					_ =>
					{
						throw new NotSupportedException(
							String.Format(
								CultureInfo.CurrentCulture,
								"'{0}' does not have CreateRange({1}[]) public static method.",
								factoryType.AssemblyQualifiedName,
								typeof( KeyValuePair<TKey, TValue> )
							)
						);
					};
			}

			var result = method.MakeGenericMethod( typeof( TKey ), typeof( TValue ) );
#if !UNITY
			return result.CreateDelegate( typeof( Func<KeyValuePair<TKey, TValue>[], T> ) ) as Func<KeyValuePair<TKey, TValue>[], T>;
#else
			return Delegate.CreateDelegate( typeof( Func<KeyValuePair<TKey, TValue>[], T> ), result ) as Func<KeyValuePair<TKey, TValue>[], T>;
#endif // !UNITY
		}

		private readonly MessagePackSerializer<TKey> _keySerializer;
		private readonly MessagePackSerializer<TValue> _valueSerializer;

		public ImmutableDictionarySerializer( SerializationContext ownerContext, PolymorphismSchema keysSchema, PolymorphismSchema valuesSchema )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
		{
			this._keySerializer = ownerContext.GetSerializer<TKey>( keysSchema );
			this._valueSerializer = ownerContext.GetSerializer<TValue>( valuesSchema );
			this._factory = FindFactory();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			packer.PackMapHeader( objectTree.Count() );

			foreach ( var item in objectTree )
			{
				this._keySerializer.PackTo( packer, item.Key );
				this._valueSerializer.PackTo( packer, item.Value );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotMapHeader( unpacker );
			}

			var buffer = new KeyValuePair<TKey, TValue>[ UnpackHelpers.GetItemsCount( unpacker ) ];

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				for ( int i = 0; i < buffer.Length; i++ )
				{
					if ( !subTreeUnpacker.Read() )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					var key = this._keySerializer.UnpackFrom( unpacker );

					if ( !subTreeUnpacker.Read() )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					var value = this._valueSerializer.UnpackFrom( unpacker );

					buffer[ i ] = new KeyValuePair<TKey, TValue>( key, value );
				}
			}

			return this._factory( buffer );
		}

		protected internal override void UnpackToCore( Unpacker unpacker, T collection )
		{
			throw new NotSupportedException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Unable to unpack items to existing immutable dictioary '{0}'.",
					typeof( T )
				)
			);
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, T objectTree, CancellationToken cancellationToken )
		{
			await packer.PackMapHeaderAsync( objectTree.Count(), cancellationToken ).ConfigureAwait( false );

			foreach ( var item in objectTree )
			{
				await this._keySerializer.PackToAsync( packer, item.Key, cancellationToken ).ConfigureAwait( false );
				await this._valueSerializer.PackToAsync( packer, item.Value, cancellationToken ).ConfigureAwait( false );
			}
		}

		protected internal override async Task<T> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsMapHeader )
			{
				SerializationExceptions.ThrowIsNotMapHeader( unpacker );
			}

			var buffer = new KeyValuePair<TKey, TValue>[ UnpackHelpers.GetItemsCount( unpacker ) ];

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				for ( int i = 0; i < buffer.Length; i++ )
				{
					if ( !await subTreeUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					var key = await this._keySerializer.UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false );

					if ( !await subTreeUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					var value = await this._valueSerializer.UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false );

					buffer[ i ] = new KeyValuePair<TKey, TValue>( key, value );
				}
			}

			return this._factory( buffer );
		}

		protected internal override Task UnpackToAsyncCore( Unpacker unpacker, T collection, CancellationToken cancellationToken )
		{
			throw new NotSupportedException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Unable to unpack items to existing immutable dictioary '{0}'.",
					typeof( T )
				)
			);
		}

#endif // FEATURE_TAP

	}
}