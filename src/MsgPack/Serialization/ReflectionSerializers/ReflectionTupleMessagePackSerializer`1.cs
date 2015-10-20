#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2015 FUJIWARA, Yusuke
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
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Linq;
using System.Reflection;

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
		private readonly IList<Func<T, Object>> _getters;
		private readonly IList<IMessagePackSingleObjectSerializer> _itemSerializers;

		public ReflectionTupleMessagePackSerializer( SerializationContext ownerContext, IList<PolymorphismSchema> itemSchemas )
			: base( ownerContext )
		{
			var itemTypes = TupleItems.GetTupleItemTypes( typeof( T ) );
			this._itemSerializers =
				itemTypes.Select(
					( itemType, i ) => ownerContext.GetSerializer( itemType, itemSchemas.Count == 0 ? null : itemSchemas[ i ] ) 
				).ToArray();
			this._tupleTypes = TupleItems.CreateTupleTypeList( itemTypes );
			this._tupleConstructors = this._tupleTypes.Select( tupleType => tupleType.GetConstructors().Single() ).ToArray();
			this._getters = GetGetters( itemTypes, this._tupleTypes ).ToArray();
		}

		private static IEnumerable<Func<T, Object>> GetGetters( IList<Type> itemTypes, IList<Type> tupleTypes )
		{
			var depth = -1;
			var propertyInvocationChain = new List<PropertyInfo>( itemTypes.Count % 7 + 1 );
			for ( int i = 0; i < itemTypes.Count; i++ )
			{
				if ( i % 7 == 0 )
				{
					depth++;
				}

				for ( int j = 0; j < depth; j++ )
				{
					// .TRest.TRest ...
					var restProperty = tupleTypes[ j ].GetProperty( "Rest" );
#if DEBUG && !UNITY
					Contract.Assert( restProperty != null, "restProperty != null" );
#endif // DEBUG && !UNITY
					propertyInvocationChain.Add( restProperty );
				}

				var itemNProperty = tupleTypes[ depth ].GetProperty( "Item" + ( ( i % 7 ) + 1 ) );
				propertyInvocationChain.Add( itemNProperty );
#if DEBUG && !UNITY
				Contract.Assert(
					itemNProperty != null,
					tupleTypes[ depth ].GetFullName() + "::Item" + ( ( i % 7 ) + 1 ) + " [ " + depth + " ] @ " + i );
#endif // DEBUG && !UNITY
				var getters = propertyInvocationChain.Select( property => property.GetGetMethod() ).ToArray();
				yield return
					 tuple =>
					 {
						 object current = tuple;
						 foreach ( var getter in getters )
						 {
							 current = getter.InvokePreservingExceptionType( current );
						 }

						 return current;
					 };

				propertyInvocationChain.Clear();
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			// Put cardinality as array length.
			packer.PackArrayHeader( this._itemSerializers.Count );
			for ( int i = 0; i < this._itemSerializers.Count; i++ )
			{
				this._itemSerializers[ i ].PackTo( packer, this._getters[ i ]( objectTree ) );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			var itemsCount = UnpackHelpers.GetItemsCount( unpacker );

			if ( itemsCount != this._itemSerializers.Count )
			{
				throw SerializationExceptions.NewTupleCardinarityIsNotMatch( this._itemSerializers.Count, itemsCount );
			}

			var unpackedItems = new List<object>( this._itemSerializers.Count );

			for ( var i = 0; i < this._itemSerializers.Count; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				unpackedItems.Add( this._itemSerializers[ i ].UnpackFrom( unpacker ) );
			}

			return this.CreateTuple( unpackedItems );
		}

		private T CreateTuple( IList<object> unpackedItems )
		{
			object currentTuple = null;
			for ( int nest = this._tupleTypes.Count - 1; nest >= 0; nest-- )
			{
				var items = unpackedItems.Skip( nest * 7 ).Take( Math.Min( unpackedItems.Count, 7 ) ).ToList();
				if ( currentTuple != null )
				{
					items.Add( currentTuple );
				}

				currentTuple =
					this._tupleConstructors[ nest ].InvokePreservingExceptionType( items.ToArray() );
			}

			return ( T )currentTuple;
		}
	}
}
