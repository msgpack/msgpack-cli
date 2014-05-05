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
using System.Collections.Generic;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		A helper <see cref="EnumMessagePackSerializer{TEnum}"/> for <see cref="ExpressionTreeSerializerBuilder{TObject}"/>.
	/// </summary>
	/// <typeparam name="TEnum">The type of the serialization target.</typeparam>
	internal class ExpressionCallbackEnumMessagePackSerializer<TEnum> : EnumMessagePackSerializer<TEnum>
	{
		private readonly Action<ExpressionCallbackEnumMessagePackSerializer<TEnum>, Packer, TEnum> _packUnderlyingValueTo;
		private readonly Func<ExpressionCallbackEnumMessagePackSerializer<TEnum>, TEnum, string> _getUnderlyingValueString;
		private readonly Func<ExpressionCallbackEnumMessagePackSerializer<TEnum>, MessagePackObject, TEnum> _unpackFromUnderlyingValue;
		private readonly Func<ExpressionCallbackEnumMessagePackSerializer<TEnum>, string, TEnum> _parse;

		public ExpressionCallbackEnumMessagePackSerializer(
			PackerCompatibilityOptions packerCompatibilityOptions,
			EnumSerializationMethod serializationMethod,
			IList<string> enumNames,
			IList<TEnum> enumValues,
			Action<ExpressionCallbackEnumMessagePackSerializer<TEnum>, Packer, TEnum> packUnderlyingValueTo,
			Func<ExpressionCallbackEnumMessagePackSerializer<TEnum>, TEnum, String> getUnderlyingValueString,
			Func<ExpressionCallbackEnumMessagePackSerializer<TEnum>, MessagePackObject, TEnum> unpackFromUnderlyingValue,
			Func<ExpressionCallbackEnumMessagePackSerializer<TEnum>, String, TEnum> parse
			)
			: base( packerCompatibilityOptions, serializationMethod, enumNames, enumValues )
		{
			this._packUnderlyingValueTo = packUnderlyingValueTo;
			this._getUnderlyingValueString = getUnderlyingValueString;
			this._unpackFromUnderlyingValue = unpackFromUnderlyingValue;
			this._parse = parse;
		}

		protected override void PackUnderlyingValueTo( Packer packer, TEnum enumValue )
		{
			this._packUnderlyingValueTo( this, packer, enumValue );
		}

		protected override string GetUnderlyingValueString( TEnum enumValue )
		{
			return this._getUnderlyingValueString( this, enumValue );
		}

		protected override TEnum UnpackFromUnderlyingValue( MessagePackObject messagePackObject )
		{
			return this._unpackFromUnderlyingValue( this, messagePackObject );
		}

		protected override TEnum Parse( string integralValue )
		{
			return this._parse( this, integralValue );
		}
	}
}