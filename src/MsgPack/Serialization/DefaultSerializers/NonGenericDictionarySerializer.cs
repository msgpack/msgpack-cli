using System;
using System.Collections;

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		Dictionary interface serializer.
	/// </summary>
	internal sealed class NonGenericDictionarySerializer : MessagePackSerializer<IDictionary>
	{
		private readonly System_Collections_DictionaryEntryMessagePackSerializer _entrySerializer;
		private readonly IMessagePackSerializer _collectionDeserializer;

		public NonGenericDictionarySerializer( SerializationContext ownerContext, Type targetType )
			: base( ownerContext )
		{
			this._entrySerializer = new System_Collections_DictionaryEntryMessagePackSerializer( ownerContext );
			this._collectionDeserializer = ownerContext.GetSerializer( targetType );
		}

		protected internal override void PackToCore( Packer packer, IDictionary objectTree )
		{
			packer.PackMapHeader( objectTree.Count );
			foreach ( DictionaryEntry item in objectTree )
			{
				this._entrySerializer.PackToCore( packer, item );
			}
		}

		protected internal override IDictionary UnpackFromCore( Unpacker unpacker )
		{
			return this._collectionDeserializer.UnpackFrom( unpacker ) as IDictionary;
		}
	}
}