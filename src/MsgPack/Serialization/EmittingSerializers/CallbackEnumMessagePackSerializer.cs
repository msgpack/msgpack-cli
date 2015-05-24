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

using System;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Callback based <see cref="EnumMessagePackSerializer{T}"/> to implement context-based serialization.
	/// </summary>
	/// <typeparam name="TEnum">The type of target emum type.</typeparam>
	internal class CallbackEnumMessagePackSerializer<TEnum> : EnumMessagePackSerializer<TEnum>
		where TEnum : struct 
	{
		private readonly Action<SerializationContext, Packer, TEnum> _packUnderlyingValueTo;
		private readonly Func<SerializationContext, MessagePackObject, TEnum> _unpackFromUnderlyingValue;

		public CallbackEnumMessagePackSerializer(
			SerializationContext ownerContext,
			EnumSerializationMethod serializationMethod,
			Action<SerializationContext, Packer, TEnum> packUnderlyingValueTo,
			Func<SerializationContext, MessagePackObject, TEnum> unpackFromUnderlyingValue
			) : base( ownerContext, serializationMethod )
		{
			this._packUnderlyingValueTo = packUnderlyingValueTo;
			this._unpackFromUnderlyingValue = unpackFromUnderlyingValue;
		}

		protected internal override void PackUnderlyingValueTo( Packer packer, TEnum enumValue )
		{
			this._packUnderlyingValueTo( this.OwnerContext, packer, enumValue );
		}

		protected internal override TEnum UnpackFromUnderlyingValue( MessagePackObject messagePackObject )
		{
			return this._unpackFromUnderlyingValue( this.OwnerContext, messagePackObject );
		}
	}
}