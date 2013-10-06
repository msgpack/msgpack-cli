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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Callback based <see cref="MessagePackSerializer{T}"/> to implement context-based serialization.
	/// </summary>
	/// <typeparam name="T">The type of target type.</typeparam>
	internal sealed class CallbackMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly SerializationContext _context;
		private readonly Action<SerializationContext, Packer, T> _packToCore;
		private readonly Func<SerializationContext, Unpacker, T> _unpackFromCore;
		private readonly Action<SerializationContext, Unpacker, T> _unpackToCore;

		public CallbackMessagePackSerializer(
			SerializationContext context,
			Action<SerializationContext, Packer, T> packToCore,
			Func<SerializationContext, Unpacker, T> unpackFromCore,
			Action<SerializationContext, Unpacker, T> unpackToCore
		)
			: base( ( context ?? SerializationContext.Default ).CompatibilityOptions.PackerCompatibilityOptions )
		{
			this._context = context;
			this._packToCore = packToCore;
			this._unpackFromCore = unpackFromCore;
			this._unpackToCore = unpackToCore;
		}

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			this._packToCore( this._context, packer, objectTree );
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			return this._unpackFromCore( this._context, unpacker );
		}

		protected internal sealed override void UnpackToCore( Unpacker unpacker, T collection )
		{
			if ( this._unpackToCore != null )
			{
				this._unpackToCore( this._context, unpacker, collection );
			}
			else
			{
				base.UnpackToCore( unpacker, collection );
			}
		}
	}
}
