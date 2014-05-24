using System;
using System.Collections;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		Dictionary interface serializer.
	/// </summary>
	internal sealed class NonGenericDictionarySerializer : MessagePackSerializer<IDictionary>
	{
		private readonly IMessagePackSerializer _collectionDeserializer;

		public NonGenericDictionarySerializer( SerializationContext ownerContext, Type targetType )
			: base( ownerContext )
		{
			this._collectionDeserializer = ownerContext.GetSerializer( targetType );
		}

		protected internal override void PackToCore( Packer packer, IDictionary objectTree )
		{
			packer.PackMapHeader( objectTree.Count );
			foreach ( DictionaryEntry item in objectTree )
			{
				if ( !( item.Key is MessagePackObject ) )
				{
					throw new SerializationException("Non generic dictionary may contain only MessagePackObject typed key.");
				}

				( item.Key as IPackable ).PackToMessage( packer, null );

				if ( !( item.Value is MessagePackObject ) )
				{
					throw new SerializationException("Non generic dictionary may contain only MessagePackObject typed value.");
				}

				( item.Value as IPackable ).PackToMessage( packer, null );			}
		}

		protected internal override IDictionary UnpackFromCore( Unpacker unpacker )
		{
			return this._collectionDeserializer.UnpackFrom( unpacker ) as IDictionary;
		}
	}
}