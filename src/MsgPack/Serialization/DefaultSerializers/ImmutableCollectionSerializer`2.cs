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
	internal class ImmutableCollectionSerializer<T, TItem> : MessagePackSerializer<T>
		where T : IEnumerable<TItem>
	{
		protected readonly Func<TItem[], T> Factory;

		private static Func<TItem[], T> FindFactory()
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

			var method =
				factoryType
				.GetMethods()
				.SingleOrDefault(
					m =>
					m.IsStatic
					&& m.IsPublic
					&& m.IsGenericMethod
					&& m.Name == "Create"
					&& m.GetParameters().Length == 1
					&& m.GetParameters()[ 0 ].ParameterType.IsArray
				);

			if ( method == null )
			{
				return
					_ =>
					{
						throw new NotSupportedException(
							String.Format(
								CultureInfo.CurrentCulture,
								"'{0}' does not have Create({1}) public static method.",
								factoryType.AssemblyQualifiedName,
								typeof( TItem[] )
							)
						);
					};
			}

			var result = method.MakeGenericMethod( typeof( TItem ) );
#if !UNITY
			return result.CreateDelegate( typeof( Func<TItem[], T> ) ) as Func<TItem[], T>;
#else
			return Delegate.CreateDelegate( typeof( Func<TItem[], T> ), result ) as Func<TItem[], T>;
#endif // !UNITY
		}

		protected readonly MessagePackSerializer<TItem> ItemSerializer;

		public ImmutableCollectionSerializer( SerializationContext ownerContext, PolymorphismSchema itemsSchema )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
		{
			this.ItemSerializer = ownerContext.GetSerializer<TItem>( itemsSchema );
			this.Factory = FindFactory();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			packer.PackArrayHeader( objectTree.Count() );

			foreach ( var item in objectTree )
			{
				this.ItemSerializer.PackTo( packer, item );
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

					buffer[ i ] = this.ItemSerializer.UnpackFrom( subTreeUnpacker );
				}
			}

			return this.Factory( buffer );
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
				await this.ItemSerializer.PackToAsync( packer, item, cancellationToken ).ConfigureAwait( false );
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

					buffer[ i ] = await this.ItemSerializer.UnpackFromAsync( subTreeUnpacker, cancellationToken ).ConfigureAwait( false );
				}
			}

			return this.Factory( buffer );
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