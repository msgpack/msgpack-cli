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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.CollectionSerializers
{
	/// <summary>
	///		Provides common implementation of <see cref="NonGenericEnumerableMessagePackSerializerBase{TCollection}"/> 
	///		for collection types which implement <see cref="IEnumerable"/>.
	/// </summary>
	/// <typeparam name="TCollection">The type of the collection.</typeparam>
	public abstract class NonGenericEnumerableMessagePackSerializer<TCollection> : NonGenericEnumerableMessagePackSerializerBase<TCollection>
		where TCollection : IEnumerable
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
		protected NonGenericEnumerableMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema )
			: base( ownerContext, schema ) { }

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="SerializationException">
		///		<typeparamref name="TCollection"/> is not serializable etc.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal sealed override void PackToCore( Packer packer, TCollection objectTree )
		{
			ICollection asICollection;
			if ( ( asICollection = objectTree as ICollection ) == null )
			{
				asICollection = objectTree.Cast<object>().ToArray();
			}

			packer.PackArrayHeader( asICollection.Count );
		}
	}

#if UNITY
	internal abstract class UnityNonGenericEnumerableMessagePackSerializer : UnityNonGenericEnumerableMessagePackSerializerBase
	{
		protected UnityNonGenericEnumerableMessagePackSerializer( SerializationContext ownerContext, Type targetType, PolymorphismSchema schema )
			: base( ownerContext, targetType, schema ) { }

		protected internal sealed override void PackToCore( Packer packer, object objectTree )
		{
			var asEnumerable = objectTree as IEnumerable;
			int count;
			ICollection asCollection;
			if ( ( asCollection = objectTree as ICollection ) == null )
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				count = asEnumerable.OfType<object>().ToArray().Length;
			}
			else
			{
				count = asCollection.Count;
			}

			packer.PackArrayHeader( count );
		}
	}
#endif // UNITY
}