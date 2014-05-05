#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Callback based <see cref="EnumMessagePackSerializer{T}"/> to implement context-based serialization.
	/// </summary>
	/// <typeparam name="TEnum">The type of target emum type.</typeparam>
	internal class CallbackEnumMessagePackSerializer<TEnum> : EnumMessagePackSerializer<TEnum>
	{
		private readonly Action<Packer, TEnum> _packUnderlyingValueTo;
		private readonly Func<TEnum, String> _getUnderlyingValueString;
		private readonly Func<MessagePackObject, TEnum> _unpackFromUnderlyingValue;
		private readonly Func<String, TEnum> _parse;

		public CallbackEnumMessagePackSerializer(
			PackerCompatibilityOptions packerCompatibilityOptions,
			EnumSerializationMethod serializationMethod,
			IList<string> enumNames,
			IList<TEnum> enumValues,
			Action<Packer, TEnum> packUnderlyingValueTo,
			Func<TEnum, string> getUnderlyingValueString,
			Func<MessagePackObject, TEnum> unpackFromUnderlyingValue,
			Func<string, TEnum> parse
			)
			: base(
				packerCompatibilityOptions,
				serializationMethod,
				enumNames,
				enumValues )
		{
			this._packUnderlyingValueTo = packUnderlyingValueTo;
			this._getUnderlyingValueString = getUnderlyingValueString;
			this._unpackFromUnderlyingValue = unpackFromUnderlyingValue;
			this._parse = parse;
		}

		protected override void PackUnderlyingValueTo( Packer packer, TEnum enumValue )
		{
			this._packUnderlyingValueTo( packer, enumValue );
		}

		protected override string GetUnderlyingValueString( TEnum enumValue )
		{
			return this._getUnderlyingValueString( enumValue );
		}

		protected override TEnum UnpackFromUnderlyingValue( MessagePackObject messagePackObject )
		{
			return this._unpackFromUnderlyingValue( messagePackObject );
		}

		protected override TEnum Parse( string integralValue )
		{
			return this._parse( integralValue );
		}
	}
}