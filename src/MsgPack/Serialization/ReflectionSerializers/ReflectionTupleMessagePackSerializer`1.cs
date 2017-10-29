#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2017 FUJIWARA, Yusuke
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
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Linq;
using System.Reflection;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Implements reflection-based tuple serializer for restricted platforms.
	/// </summary>
	internal class ReflectionTupleMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly IList<Type> _tupleTypes;
		private readonly IList<ConstructorInfo> _tupleConstructors;
		private readonly IList<Func<T, object>> _getters;
		private readonly IList<MessagePackSerializer> _itemSerializers;

		public ReflectionTupleMessagePackSerializer( SerializationContext ownerContext, IList<PolymorphismSchema> itemSchemas )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
		{
			var itemTypes = TupleItems.GetTupleItemTypes( typeof( T ) );
			this._itemSerializers =
				itemTypes.Select(
					( itemType, i ) => ownerContext.GetSerializer( itemType, itemSchemas.Count == 0 ? null : itemSchemas[ i ] ) 
				).ToArray();
			this._tupleTypes = TupleItems.CreateTupleTypeList( typeof( T ) );
			this._tupleConstructors = this._tupleTypes.Select( tupleType => tupleType.GetConstructors().SingleOrDefault() ).ToArray();
			this._getters =
				typeof( T ).GetIsValueType()
					? GetGetters(
						itemTypes,
						this._tupleTypes,
						( type, name ) => type.GetField( name ),
						f => f,
						getters =>
							tuple =>
							{
								object current = tuple;
								foreach ( var getter in getters )
								{
									current = getter.GetValue( current );
								}

								return current;
							}
					).ToArray() 
					: GetGetters(
						itemTypes,
						this._tupleTypes,
						( type, name ) => type.GetProperty( name ),
						p => p.GetGetMethod(),
						getters =>
							tuple =>
							{
								object current = tuple;
								foreach ( var getter in getters )
								{
									current = getter.InvokePreservingExceptionType( current );
								}

								return current;
							}
					).ToArray();
		}

		private static IEnumerable<Func<T, object>> GetGetters<TInfo, TAccessor>(
			IList<Type> itemTypes,
			IList<Type> tupleTypes,
			Func<Type, string, TInfo> metadataFactory,
			Func<TInfo, TAccessor> accessorFactory,
			Func<IEnumerable<TAccessor>, Func<T, object>> chainedGetterFactory
		)
		{
			var depth = -1;
			var memberInvocationChain = new List<TInfo>( itemTypes.Count % 7 + 1 );
			for ( var i = 0; i < itemTypes.Count; i++ )
			{
				if ( i % 7 == 0 )
				{
					depth++;
				}

				for ( var j = 0; j < depth; j++ )
				{
					// .TRest.TRest ...
					var restMember = metadataFactory( tupleTypes[ j ], "Rest" );
#if DEBUG
					Contract.Assert( restMember != null, "restMember != null" );
#endif // DEBUG
					memberInvocationChain.Add( restMember );
				}

				var itemNMember = metadataFactory( tupleTypes[ depth ], "Item" + ( ( i % 7 ) + 1 ) );
				memberInvocationChain.Add( itemNMember );
#if DEBUG
				Contract.Assert(
					itemNMember != null,
					tupleTypes[ depth ].GetFullName() + "::Item" + ( ( i % 7 ) + 1 ) + " [ " + depth + " ] @ " + i );
#endif // DEBUG
				var getters = memberInvocationChain.Select( accessorFactory ).ToArray();
				yield return chainedGetterFactory( getters );

				memberInvocationChain.Clear();
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			// Put cardinality as array length.
			packer.PackArrayHeader( this._itemSerializers.Count );
			for ( var i = 0; i < this._itemSerializers.Count; i++ )
			{
				this._itemSerializers[ i ].PackTo( packer, this._getters[ i ]( objectTree ) );
			}
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, T objectTree, CancellationToken cancellationToken )
		{
			// Put cardinality as array length.
			await packer.PackArrayHeaderAsync( this._itemSerializers.Count, cancellationToken ).ConfigureAwait( false );
			for ( var i = 0; i < this._itemSerializers.Count; i++ )
			{
				await this._itemSerializers[ i ].PackToAsync( packer, this._getters[ i ]( objectTree ), cancellationToken ).ConfigureAwait( false );
			}
		}

#endif // FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );

			if ( itemsCount != this._itemSerializers.Count )
			{
				SerializationExceptions.ThrowTupleCardinarityIsNotMatch( this._itemSerializers.Count, itemsCount, unpacker );
			}

			var unpackedItems = new List<object>( this._itemSerializers.Count );

			for ( var i = 0; i < this._itemSerializers.Count; i++ )
			{
				if ( !unpacker.Read() )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
				}

				unpackedItems.Add( this._itemSerializers[ i ].UnpackFrom( unpacker ) );
			}

			return this.CreateTuple( unpackedItems );
		}

#if FEATURE_TAP

		protected internal override async Task<T> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );

			if ( itemsCount != this._itemSerializers.Count )
			{
				SerializationExceptions.ThrowTupleCardinarityIsNotMatch( this._itemSerializers.Count, itemsCount, unpacker );
			}

			var unpackedItems = new List<object>( this._itemSerializers.Count );

			for ( var i = 0; i < this._itemSerializers.Count; i++ )
			{
				if ( !await unpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					SerializationExceptions.ThrowMissingItem( i, unpacker );
				}

				unpackedItems.Add( await this._itemSerializers[ i ].UnpackFromAsync( unpacker, cancellationToken ).ConfigureAwait( false ) );
			}

			return this.CreateTuple( unpackedItems );
		}
#endif // FEATURE_TAP


		private T CreateTuple( IList<object> unpackedItems )
		{
			object currentTuple = null;
			for ( var nest = this._tupleTypes.Count - 1; nest >= 0; nest-- )
			{
				var items = unpackedItems.Skip( nest * 7 ).Take( Math.Min( unpackedItems.Count, 7 ) ).ToList();
				if ( currentTuple != null )
				{
					items.Add( currentTuple );
				}

				currentTuple =
					this._tupleConstructors[ nest ] == null
						? ReflectionExtensions.CreateInstancePreservingExceptionType( this._tupleTypes[ nest ] )
						: this._tupleConstructors[ nest ].InvokePreservingExceptionType( items.ToArray() );
			}

			return ( T )currentTuple;
		}
	}
}
