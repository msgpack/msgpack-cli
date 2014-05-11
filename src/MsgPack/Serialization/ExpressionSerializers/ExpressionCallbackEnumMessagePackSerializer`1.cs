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

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		A helper <see cref="EnumMessagePackSerializer{TEnum}"/> for <see cref="ExpressionTreeSerializerBuilder{TObject}"/>.
	/// </summary>
	/// <typeparam name="TEnum">The type of the serialization target.</typeparam>
	internal class ExpressionCallbackEnumMessagePackSerializer<TEnum> : EnumMessagePackSerializer<TEnum>
		where TEnum : struct 
	{
		private readonly Action<ExpressionCallbackEnumMessagePackSerializer<TEnum>, Packer, TEnum> _packUnderlyingValueTo;
		private readonly Func<ExpressionCallbackEnumMessagePackSerializer<TEnum>, MessagePackObject, TEnum> _unpackFromUnderlyingValue;

		public ExpressionCallbackEnumMessagePackSerializer(
			SerializationContext ownerContext,
			EnumSerializationMethod serializationMethod,
			Action<ExpressionCallbackEnumMessagePackSerializer<TEnum>, Packer, TEnum> packUnderlyingValueTo,
			Func<ExpressionCallbackEnumMessagePackSerializer<TEnum>, MessagePackObject, TEnum> unpackFromUnderlyingValue
		) : base( ownerContext, serializationMethod )
		{
			this._packUnderlyingValueTo = packUnderlyingValueTo;
			this._unpackFromUnderlyingValue = unpackFromUnderlyingValue;
		}

		protected internal override void PackUnderlyingValueTo( Packer packer, TEnum enumValue )
		{
			this._packUnderlyingValueTo( this, packer, enumValue );
		}
		
		protected internal override TEnum UnpackFromUnderlyingValue( MessagePackObject messagePackObject )
		{
			return this._unpackFromUnderlyingValue( this, messagePackObject );
		}
	}
}