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
#if UNITY
using System.Collections;
#endif // UNITY

namespace MsgPack.Serialization.DefaultSerializers
{
#if !UNITY
	internal sealed class ArraySerializer<T> : MessagePackSerializer<T[]>
#else
	internal sealed class UnityArraySerializer : NonGenericMessagePackSerializer
#endif // !UNITY
	{
#if !UNITY
		private readonly MessagePackSerializer<T> _itemSerializer;
#else
		private readonly IMessagePackSingleObjectSerializer _itemSerializer;
		private readonly Type _itemType;
#endif // !UNITY

#if !UNITY
		public ArraySerializer( SerializationContext ownerContext, PolymorphismSchema itemsSchema )
			: base( ownerContext )
		{
			this._itemSerializer = ownerContext.GetSerializer<T>( itemsSchema );
		}
#else
		public UnityArraySerializer( SerializationContext ownerContext, Type itemType, PolymorphismSchema itemsSchema )
			: base( ownerContext, itemType.MakeArrayType() )
		{
			this._itemSerializer = ownerContext.GetSerializer( itemType, itemsSchema );
			this._itemType = itemType;
		}
#endif // !UNITY

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
#if !UNITY
		protected internal override void PackToCore( Packer packer, T[] objectTree )
		{
			packer.PackArrayHeader( objectTree.Length );
			foreach ( var item in objectTree )
			{
				this._itemSerializer.PackTo( packer, item );
			}
		}
#else
		protected internal override void PackToCore( Packer packer, object objectTree )
		{
			var asList = objectTree as IList;
			// ReSharper disable PossibleNullReferenceException
			packer.PackArrayHeader( asList.Count );
			foreach ( var item in asList )
			{
				this._itemSerializer.PackTo( packer, item );
			}
			// ReSharper restore PossibleNullReferenceException
		}
#endif // !UNITY

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
#if !UNITY
		protected internal override T[] UnpackFromCore( Unpacker unpacker )
#else
		protected internal override object UnpackFromCore( Unpacker unpacker )
#endif // !UNITY
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			var count = UnpackHelpers.GetItemsCount( unpacker );
#if !UNITY
			var result = new T[ count ];
#else
			var result = Array.CreateInstance( this._itemType, count );
#endif // !UNITY
			this.UnpackToCore( unpacker, result, count );
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
#if !UNITY
		protected internal override void UnpackToCore( Unpacker unpacker, T[] collection )
#else
		protected internal override void UnpackToCore( Unpacker unpacker, object collection )
#endif // !UNITY
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

#if !UNITY
			this.UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
#else
			this.UnpackToCore( unpacker, collection as IList, UnpackHelpers.GetItemsCount( unpacker ) );
#endif // !UNITY
		}

#if !UNITY
		private void UnpackToCore( Unpacker unpacker, T[] collection, int count )
#else
		private void UnpackToCore( Unpacker unpacker, IList collection, int count )
#endif // !UNITY
		{
			for ( int i = 0; i < count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

#if !UNITY
				T item;
#else
				object item;
#endif // !UNITY
				if ( !unpacker.IsArrayHeader && !unpacker.IsMapHeader )
				{
					item = this._itemSerializer.UnpackFrom( unpacker );
				}
				else
				{
					using ( var subtreeUnpacker = unpacker.ReadSubtree() )
					{
						item = this._itemSerializer.UnpackFrom( subtreeUnpacker );
					}
				}

				collection[ i ] = item;
			}
		}
	}
}