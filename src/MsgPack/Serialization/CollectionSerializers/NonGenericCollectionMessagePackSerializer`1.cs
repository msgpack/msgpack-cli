#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015 FUJIWARA, Yusuke
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
using System.Collections;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.CollectionSerializers
{
	/// <summary>
	///		Provides common implementation of <see cref="NonGenericEnumerableMessagePackSerializerBase{TCollection}"/> 
	///		for collection types which implement <see cref="ICollection"/>.
	/// </summary>
	/// <typeparam name="TCollection">The type of the collection.</typeparam>
	public abstract class NonGenericCollectionMessagePackSerializer<TCollection> : NonGenericEnumerableMessagePackSerializerBase<TCollection>
		where TCollection : ICollection
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="NonGenericCollectionMessagePackSerializer{TCollection}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="schema">
		///		The schema for collection itself or its items for the member this instance will be used to. 
		///		<c>null</c> will be considered as <see cref="PolymorphismSchema.Default"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="ownerContext"/> is <c>null</c>.
		/// </exception>
		protected NonGenericCollectionMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema )
			: base( ownerContext, schema ) { }

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="SerializationException">
		///		<typeparamref name="TCollection"/> is not serializable etc.
		/// </exception>
		protected internal sealed override void PackToCore( Packer packer, TCollection objectTree )
		{
			packer.PackArrayHeader( objectTree.Count );

			var itemSerializer = this.ItemSerializer;
			foreach ( var item in objectTree )
			{
				itemSerializer.PackTo( packer, item );
			}
		}
	}
}