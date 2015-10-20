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
using System.Globalization;
using System.Linq;

namespace MsgPack.Serialization.DefaultSerializers
{
	internal class ImmutableCollectionSerializer<T, TItem> : MessagePackSerializer<T>
		where T : IEnumerable<TItem>
	{
		protected static readonly Func<TItem[], T> factory = FindFactory();

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

		private readonly MessagePackSerializer<TItem> _itemSerializer;

		public ImmutableCollectionSerializer( SerializationContext ownerContext, PolymorphismSchema itemsSchema )
			: base( ownerContext )
		{
			this._itemSerializer = ownerContext.GetSerializer<TItem>( itemsSchema );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			packer.PackArrayHeader( objectTree.Count() );

			foreach ( var item in objectTree )
			{
				this._itemSerializer.PackTo( packer, item );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			var buffer = new TItem[ UnpackHelpers.GetItemsCount( unpacker ) ];

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				for ( int i = 0; i < buffer.Length; i++ )
				{
					if ( !subTreeUnpacker.Read() )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					buffer[ i ] = this._itemSerializer.UnpackFrom( subTreeUnpacker );
				}
			}

			return factory( buffer );
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
	}
}