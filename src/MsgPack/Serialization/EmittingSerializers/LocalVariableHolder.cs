#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Reflection.Emit;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Holds reusable(temporal) local variable info.
	/// </summary>
	internal sealed class LocalVariableHolder
	{
		private readonly TracingILGenerator _il;

		private LocalBuilder _rawItemsCount;

		public LocalBuilder RawItemsCount
		{
			get
			{
				if ( this._rawItemsCount == null )
				{
					this._rawItemsCount = this._il.DeclareLocal( typeof( long ), "rawItemsCount" );
				}

				return this._rawItemsCount;
			}
		}

		private LocalBuilder _itemsCount;

		public LocalBuilder ItemsCount
		{
			get
			{
				if ( this._itemsCount == null )
				{
					this._itemsCount = this._il.DeclareLocal( typeof( int ), "itemsCount" );
				}

				return this._itemsCount;
			}
		}

		private LocalBuilder _memberName;

		public LocalBuilder MemberName
		{
			get
			{
				if ( this._memberName == null )
				{
					this._memberName = this._il.DeclareLocal( typeof( string ), "memberName" );
				}

				return this._memberName;
			}
		}


		private LocalBuilder _packingCollectionCount;

		public LocalBuilder PackingCollectionCount
		{
			get
			{
				if ( this._packingCollectionCount == null )
				{
					this._packingCollectionCount = this._il.DeclareLocal( typeof( int ), "packingCollectionCount" );
				}

				return this._packingCollectionCount;
			}
		}

		private readonly Dictionary<Type, LocalBuilder> _serializingValues = new Dictionary<Type, LocalBuilder>();

		public LocalBuilder GetSerializingValue( Type type )
		{
			LocalBuilder result;
			if ( !this._serializingValues.TryGetValue( type, out result ) )
			{
				result = this._il.DeclareLocal( type, "serializingValue" );
				this._serializingValues[ type ] = result;
			}

			return result;
		}

		private readonly Dictionary<Type, LocalBuilder> _serializingCollections = new Dictionary<Type, LocalBuilder>();

		public LocalBuilder GetSerializingCollection( Type type )
		{
			LocalBuilder result;
			if ( !this._serializingCollections.TryGetValue( type, out result ) )
			{
				result = this._il.DeclareLocal( type, type.IsArray ? "array" : "collection" );
				this._serializingCollections[ type ] = result;
			}

			return result;
		}

		private readonly Dictionary<Type, LocalBuilder> _serializingCollectionItems = new Dictionary<Type, LocalBuilder>();

		public LocalBuilder GetSerializingCollectionItem( Type type )
		{
			LocalBuilder result;
			if ( !this._serializingCollectionItems.TryGetValue( type, out result ) )
			{
				result = this._il.DeclareLocal( type, "item" );
				this._serializingCollectionItems[ type ] = result;
			}

			return result;
		}

		// for UnpackTo
		private readonly Dictionary<Type, LocalBuilder> _deserializingCollectios = new Dictionary<Type, LocalBuilder>();

		public LocalBuilder GetDeserializingCollection( Type type )
		{
			LocalBuilder result;
			if ( !this._deserializingCollectios.TryGetValue( type, out result ) )
			{
				result = this._il.DeclareLocal( type, type.IsArray ? "array" : "collection" );
				this._deserializingCollectios[ type ] = result;
			}

			return result;
		}

		private readonly Dictionary<Type, LocalBuilder> _deserializedValues = new Dictionary<Type, LocalBuilder>();

		public LocalBuilder GetDeserializedValue( Type type )
		{
			LocalBuilder result;
			if ( !this._deserializedValues.TryGetValue( type, out result ) )
			{
				result = this._il.DeclareLocal( type, "deserializedValue" );
				this._deserializedValues[ type ] = result;
			}

			return result;
		}

		private LocalBuilder _unpackedData;

		public LocalBuilder UnpackedData
		{
			get
			{
				if ( this._unpackedData == null )
				{
					this._unpackedData = this._il.DeclareLocal( typeof( MessagePackObject ), "unpackedData" );
				}

				return this._unpackedData;
			}
		}

		private LocalBuilder _isDeserializationSucceeded;

		public LocalBuilder IsDeserializationSucceeded
		{
			get
			{
				if ( this._isDeserializationSucceeded == null )
				{
					this._isDeserializationSucceeded = this._il.DeclareLocal( typeof( bool ), "isDeserializationSucceeded" );
				}

				return this._isDeserializationSucceeded;
			}
		}

		private LocalBuilder _subtreeUnpacker;

		public LocalBuilder SubtreeUnpacker
		{
			get
			{
				if ( this._subtreeUnpacker == null )
				{
					this._subtreeUnpacker = this._il.DeclareLocal( typeof( Unpacker ), "subtreeUnpacker" );
				}

				return this._subtreeUnpacker;
			}
		}

		private readonly Dictionary<Type, LocalBuilder> _catchedExceptions = new Dictionary<Type, LocalBuilder>();

		public LocalBuilder GetCatchedException( Type type )
		{
			LocalBuilder result;
			if ( !this._catchedExceptions.TryGetValue( type, out result ) )
			{
				result = this._il.DeclareLocal( type, "ex" );
				this._catchedExceptions[ type ] = result;
			}

			return result;
		}

		public LocalVariableHolder( TracingILGenerator il )
		{
			this._il = il;
		}
	}
}