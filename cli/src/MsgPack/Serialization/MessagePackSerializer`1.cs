#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	// TODO: MessagePackEncoder/Decoder <|- ...NativeEncoder/Decoder, ...JsonEncoder/Decoder
	/// <summary>
	///		Defines base contract for object serialization.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class MessagePackSerializer<T>
	{
		/// <summary>
		///		Serialize specified object to the <see cref="Stream"/>.
		/// </summary>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <param name="stream">Destination <see cref="Stream"/>.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
		/// </exception>
		public void Pack( T objectTree, Stream stream )
		{
			this.PackTo( Packer.Create( stream ), objectTree );
		}

		/// <summary>
		///		Deserialize object from the <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">Source <see cref="Stream"/>.</param>
		/// <returns>Deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="stream"/> is <c>null</c>.
		/// </exception>
		public T Unpack( Stream stream )
		{
			return this.UnpackFrom( Unpacker.Create( stream ) );
		}

		/// <summary>
		///		Serialize specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="packer"/> is <c>null</c>.
		/// </exception>
		public void PackTo( Packer packer, T objectTree )
		{
			if ( packer == null )
			{
				throw new ArgumentNullException( "packer" );
			}

			this.PackToCore( packer, objectTree );
		}

		/// <summary>
		///		Serialize specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		protected abstract void PackToCore( Packer packer, T objectTree );

		/// <summary>
		///		Deserialize object with specified <see cref="Unpacker"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree.</param>
		/// <returns>Deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		/// </exception>
		public T UnpackFrom( Unpacker unpacker )
		{
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			return this.UnpackFromCore( unpacker );
		}

		/// <summary>
		///		Deserialize object with specified <see cref="Unpacker"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <returns>Deserialized object.</returns>
		protected abstract T UnpackFromCore( Unpacker unpacker );
	}

	/*
	public sealed class UnpackingContext
	{
		public bool IsArray
		{
			get { throw new NotImplementedException(); }
		}

		public bool IsMap
		{
			get { throw new NotImplementedException(); }
		}

		public int ItemsCount
		{
			get { throw new NotImplementedException(); }
		}

		public IEnumerable<MessagePackObject> GetArrayItems()
		{
			throw new NotImplementedException();
		}

		public MessagePackObject[] GetArray()
		{
			return this.GetArrayItems().ToArray();
		}

		public IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>> GetMapItems()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<KeyValuePair<string, MessagePackObject>> GetStringMapItems()
		{
			return this.GetMapItems().Select( kv => new KeyValuePair<string, MessagePackObject>( kv.Key.AsString(), kv.Value ) );
		}

		public MessagePackObjectDictionary GetDictionary()
		{
			var result = new MessagePackObjectDictionary( this.ItemsCount );
			foreach ( var item in this.GetMapItems() )
			{
				result.Add( item.Key, item.Value );
			}
			return result;
		}

		public void ApplyObjectBuilder( UnpackedObjectBuilder builder )
		{
			throw new NotImplementedException();
		}
	}

	public sealed class PackingContext
	{

	}

	public abstract class PackedMessageBuilder
	{
		public void PackObject( PackingContext context, Packer packer )
		{
			throw new NotImplementedException();
		}
	}

	public abstract class UnpackedObjectBuilder
	{
		public void UnpackMember( UnpackingContext context, string name, MessagePackObject value )
		{
			throw new NotImplementedException();
		}
	}
	 */
}
