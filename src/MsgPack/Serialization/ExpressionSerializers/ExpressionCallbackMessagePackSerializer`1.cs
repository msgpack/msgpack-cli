#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		A helper <see cref="MessagePackSerializer{T}"/> for <see cref="ExpressionTreeSerializerBuilder{TObject}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the serialization target.</typeparam>
	internal class ExpressionCallbackMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> _packToCore;
		private readonly Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> _unpackFromCore;
		private readonly Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> _unpackToCore;

		/// <summary>
		///		Initializes a new instance of the <see cref="ExpressionCallbackMessagePackSerializer{T}"/> class.
		/// </summary>
		/// <param name="ownerContext">The serialization context.</param>
		/// <param name="packToCore">The delegate to <c>PackToCore</c> method body. This value must not be <c>null</c>.</param>
		/// <param name="unpackFromCore">The delegate to <c>UnpackFromCore</c> method body. This value must not be <c>null</c>.</param>
		/// <param name="unpackToCore">The delegate to <c>UnpackToCore</c> method body. This value can be <c>null</c>.</param>
		public ExpressionCallbackMessagePackSerializer(
			SerializationContext ownerContext,
			Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Packer, T> packToCore,
			Func<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> unpackFromCore,
			Action<ExpressionCallbackMessagePackSerializer<T>, SerializationContext, Unpacker, T> unpackToCore
		)
			: base( ownerContext )
		{
#if DEBUG
			Contract.Assert( packToCore != null );
			Contract.Assert( unpackFromCore != null );
#endif // DEBUG

			this._packToCore = packToCore;
			this._unpackFromCore = unpackFromCore;
			this._unpackToCore = unpackToCore;
		}

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			this._packToCore( this, this.OwnerContext, packer, objectTree );
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			return this._unpackFromCore( this, this.OwnerContext, unpacker );
		}

		protected internal override void UnpackToCore( Unpacker unpacker, T collection )
		{
			if ( this._unpackToCore != null )
			{
				this._unpackToCore( this, this.OwnerContext, unpacker, collection );
			}
			else
			{
				base.UnpackToCore( unpacker, collection );
			}
		}
	}
}