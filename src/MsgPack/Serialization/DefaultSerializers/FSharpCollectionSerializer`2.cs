#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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
	internal sealed class FSharpCollectionSerializer<T, TItem> : MessagePackSerializer<T>
		where T : IEnumerable<TItem>
	{
		private readonly Func<TItem[], T> _factory;

		private static Func<TItem[], T> FindFactory( string factoryTypeName )
		{
			var factoryType =
				typeof( T ).GetAssembly().GetType(
					typeof( T ).Namespace + "." + factoryTypeName
				);

			if ( factoryType == null )
			{
				return
					_ =>
					{
						throw new NotSupportedException(
							String.Format(
								CultureInfo.CurrentCulture,
								"Cannot find {1}. '{0}' may not be an fsharp collection.",
								typeof( T ).AssemblyQualifiedName,
								factoryTypeName
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
					&& m.Name == "OfSeq"
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
								"'{0}' does not have OfSeq({1}) public static method.",
								factoryType.AssemblyQualifiedName,
								typeof( IEnumerable<TItem> )
							)
						);
					};
			}

			var result = method.MakeGenericMethod( typeof( TItem ) );
#if !UNITY
			return result.CreateDelegate( typeof( Func<IEnumerable<TItem>, T> ) ) as Func<IEnumerable<TItem>, T>;
#else
			return Delegate.CreateDelegate( typeof( Func<IEnumerable<TItem>, T> ), result ) as Func<IEnumerable<TItem>, T>;
#endif // !UNITY
		}

		private readonly MessagePackSerializer<TItem> _itemSerializer;

		public FSharpCollectionSerializer( SerializationContext ownerContext, PolymorphismSchema itemsSchema, string factoryTypeName )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
		{
			this._itemSerializer = ownerContext.GetSerializer<TItem>( itemsSchema );
			this._factory = FindFactory( factoryTypeName );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			packer.PackArrayHeader( objectTree.Count() );

			foreach ( var item in objectTree )
			{
				this._itemSerializer.PackTo( packer, item );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var buffer = new TItem[ UnpackHelpers.GetItemsCount( unpacker ) ];

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				for ( int i = 0; i < buffer.Length; i++ )
				{
					if ( !subTreeUnpacker.Read() )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					buffer[ i ] = this._itemSerializer.UnpackFrom( subTreeUnpacker );
				}
			}

			return this._factory( buffer );
		}

		protected internal override void UnpackToCore( Unpacker unpacker, T collection )
		{
			throw new NotSupportedException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Unable to unpack items to existing immutable collection '{0}'.",
					typeof( T )
				)
			);
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, T objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( objectTree.Count(), cancellationToken ).ConfigureAwait( false );

			foreach ( var item in objectTree )
			{
				await this._itemSerializer.PackToAsync( packer, item, cancellationToken ).ConfigureAwait( false );
			}
		}

		protected internal override async Task<T> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var buffer = new TItem[ UnpackHelpers.GetItemsCount( unpacker ) ];

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				for ( int i = 0; i < buffer.Length; i++ )
				{
					if ( !await subTreeUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					buffer[ i ] = await this._itemSerializer.UnpackFromAsync( subTreeUnpacker, cancellationToken ).ConfigureAwait( false );
				}
			}

			return this._factory( buffer );
		}

		protected internal override Task UnpackToAsyncCore( Unpacker unpacker, T collection, CancellationToken cancellationToken )
		{
			throw new NotSupportedException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Unable to unpack items to existing immutable collection '{0}'.",
					typeof( T )
				)
			);
		}

#endif // FEATURE_TAP

	}
}