#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015-2016 FUJIWARA, Yusuke
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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

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
		///		Initializes a new instance of the <see cref="NonGenericCollectionMessagePackSerializer{TCollection}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="schema">
		///		The schema for collection itself or its items for the member this instance will be used to. 
		///		<c>null</c> will be considered as <see cref="PolymorphismSchema.Default"/>.
		/// </param>
		/// <param name="capabilities">A serializer calability flags represents capabilities of this instance.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="ownerContext"/> is <c>null</c>.
		/// </exception>
		protected NonGenericEnumerableMessagePackSerializer( SerializationContext ownerContext, PolymorphismSchema schema, SerializerCapabilities capabilities )
			: base( ownerContext, schema, capabilities ) { }

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="SerializationException">
		///		<typeparamref name="TCollection"/> is not serializable etc.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, TCollection objectTree )
		{
			ICollection asICollection;
			if ( ( asICollection = objectTree as ICollection ) == null )
			{
				asICollection = objectTree.Cast<object>().ToArray();
			}

			packer.PackArrayHeader( asICollection.Count );

			foreach ( var item in asICollection )
			{
				this.ItemSerializer.PackTo( packer, item );
			}
		}

#if FEATURE_TAP

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/> asynchronously.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		<typeparamref name="TCollection"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="P:Capabilities"/>
		protected internal override async Task PackToAsyncCore( Packer packer, TCollection objectTree, CancellationToken cancellationToken )
		{
			ICollection asICollection;
			if ( ( asICollection = objectTree as ICollection ) == null )
			{
				asICollection = objectTree.Cast<object>().ToArray();
			}

			await packer.PackArrayHeaderAsync( asICollection.Count, cancellationToken ).ConfigureAwait( false );

			foreach ( var item in asICollection )
			{
				await this.ItemSerializer.PackToAsync( packer, item, cancellationToken ).ConfigureAwait( false );
			}
		}

#endif // FEATURE_TAP
	}

#if UNITY
#warning TODO: Remove if possible for maintenancibility.
	internal abstract class UnityNonGenericEnumerableMessagePackSerializer : UnityNonGenericEnumerableMessagePackSerializerBase
	{
		protected UnityNonGenericEnumerableMessagePackSerializer( SerializationContext ownerContext, Type targetType, PolymorphismSchema schema, SerializerCapabilities capabilities )
			: base( ownerContext, targetType, schema, capabilities ) { }

		protected internal override void PackToCore( Packer packer, object objectTree )
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

			foreach ( var item in asEnumerable )
			{
				this.ItemSerializer.PackTo( packer, item );
			}
		}
	}
#endif // UNITY
}