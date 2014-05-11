#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MsgPack.Serialization.DefaultSerializers
{
	internal sealed class ImmutableDictionarySerializer<T, TKey, TValue> : MessagePackSerializer<T>
		where T : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		private static readonly Func<KeyValuePair<TKey, TValue>[], T> _factory = FindFactory();

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
								typeof( IEnumerable<KeyValuePair<TKey, TValue>> )
							)
						);
					};
			}

			return
				method
				.MakeGenericMethod( typeof( TKey ), typeof( TValue ) )
				.CreateDelegate( typeof( Func<KeyValuePair<TKey, TValue>[], T> ) ) as Func<KeyValuePair<TKey, TValue>[], T>;
		}

		private readonly MessagePackSerializer<TKey> _keySerializer;
		private readonly MessagePackSerializer<TValue> _valueSerializer;

		public ImmutableDictionarySerializer( SerializationContext ownerContext )
			: base( ownerContext )
		{
			this._keySerializer = ownerContext.GetSerializer<TKey>();
			this._valueSerializer = ownerContext.GetSerializer<TValue>();
		}

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			packer.PackMapHeader( objectTree.Count() );

			foreach ( var item in objectTree )
			{
				this._keySerializer.PackTo( packer, item.Key );
				this._valueSerializer.PackTo( packer, item.Value );
			}
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsMapHeader )
			{
				throw SerializationExceptions.NewIsNotMapHeader();
			}

			var buffer = new KeyValuePair<TKey, TValue>[ UnpackHelpers.GetItemsCount( unpacker ) ];

			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				for ( int i = 0; i < buffer.Length; i++ )
				{
					if ( !subTreeUnpacker.Read() )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					var key = this._keySerializer.UnpackFrom( unpacker );

					if ( !subTreeUnpacker.Read() )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					var value = this._valueSerializer.UnpackFrom( unpacker );

					buffer[ i ] = new KeyValuePair<TKey, TValue>( key, value );
				}
			}

			return _factory( buffer );
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
	}
}