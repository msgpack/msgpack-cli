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
using System.Globalization;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines basic features for enum object serializers.
	/// </summary>
	/// <typeparam name="TEnum">The type of enum type itself.</typeparam>
	/// <remarks>
	///		This class supports auto-detect on deserialization. So the constructor parameter only affects serialization behavior.
	/// </remarks>
	public abstract class EnumMessagePackSerializer<TEnum> : MessagePackSerializer<TEnum>, ICustomizableEnumSerializer
	{
		private readonly Type _underlyingType;
		private EnumSerializationMethod _serializationMethod; // not readonly -- changed in cloned instance in GetCopyAs()
		private readonly IDictionary<TEnum, string> _valueNameMap;
		private readonly IDictionary<string, TEnum> _nameValueMap;

		/// <summary>
		///	 Initializes a new instance of the <see cref="EnumMessagePackSerializer{TEnum}"/> class.
		/// </summary>
		/// <param name="packerCompatibilityOptions">The <see cref="PackerCompatibilityOptions"/> for new packer creation.</param>
		/// <param name="serializationMethod">The <see cref="EnumSerializationMethod"/> which determines serialization form of the enums.</param>
		/// <param name="enumNames">The names of enum members. The elements are corresponds to <paramref name="enumValues"/>.</param>
		/// <param name="enumValues">The names of enum values. The elements are corresponds to <paramref name="enumNames"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="enumNames"/> is <c>null</c>.
		/// Or <paramref name="enumValues "/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">Count of <paramref name="enumNames"/> and <paramref name="enumValues "/> are not equal.</exception>
		protected EnumMessagePackSerializer( PackerCompatibilityOptions packerCompatibilityOptions, EnumSerializationMethod serializationMethod, IList<string> enumNames, IList<TEnum> enumValues )
			: base( packerCompatibilityOptions )
		{
			if ( enumNames == null )
			{
				throw new ArgumentNullException( "enumNames" );
			}

			if ( enumValues == null )
			{
				throw new ArgumentNullException( "enumValues" );
			}

			if ( enumNames.Count != enumValues.Count )
			{
				throw new ArgumentException( "Count of 'enumNames' and 'enumValues' are not equal." );
			}

			if ( !typeof( TEnum ).GetIsEnum() )
			{
				throw new InvalidOperationException(
					String.Format( CultureInfo.CurrentCulture, "Type '{0}' is not enum.", typeof( TEnum ) )
				);
			}

			this._serializationMethod = serializationMethod;
			this._underlyingType = Enum.GetUnderlyingType( typeof( TEnum ) );

			var valueNameMap = new Dictionary<TEnum, string>( enumNames.Count );
			var nameValueMap = new Dictionary<string, TEnum>( enumNames.Count );
			for ( int i = 0; i < enumNames.Count; i++ )
			{
				valueNameMap[ enumValues[ i ] ] = enumNames[ i ];
				nameValueMap[ enumNames[ i ] ] = enumValues[ i ];
			}

			this._valueNameMap = valueNameMap;
			this._nameValueMap = nameValueMap;
		}

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		protected internal sealed override void PackToCore( Packer packer, TEnum objectTree )
		{
			if ( this._serializationMethod == EnumSerializationMethod.ByUnderlyingValue )
			{
				this.PackUnderlyingValueTo( packer, objectTree );
			}
			else
			{
				string asString;
				if ( !this._valueNameMap.TryGetValue( objectTree, out asString ) )
				{
					asString = this.GetUnderlyingValueString( objectTree );
				}

				packer.PackString( asString );
			}
		}

		/// <summary>
		///		Packs enum value as its underlying value.
		/// </summary>
		/// <param name="packer">The packer.</param>
		/// <param name="enumValue">The enum value to be packed.</param>
		protected abstract void PackUnderlyingValueTo( Packer packer, TEnum enumValue );

		/// <summary>
		///		Gets the string representation of the underlying integral value.
		/// </summary>
		/// <param name="enumValue">The enum value to be stringified.</param>
		/// <returns>A string representation of the underlying integral value.</returns>
		protected abstract string GetUnderlyingValueString( TEnum enumValue );

		/// <summary>
		///		Deserializes object with specified <see cref="Unpacker"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree. This value will not be <c>null</c>.</param>
		/// <returns>Deserialized object.</returns>
		/// <exception cref="SerializationException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid unpacker state, stream content, or so.
		/// </exception>
		protected internal sealed override TEnum UnpackFromCore( Unpacker unpacker )
		{
			if ( unpacker.LastReadData.IsRaw )
			{
				var asString = unpacker.LastReadData.AsString();
				TEnum result;
				if ( !this._nameValueMap.TryGetValue( asString, out result ) )
				{
					try
					{
						result = this.Parse( asString );
					}
					catch ( Exception ex )
					{
						throw new SerializationException(
							String.Format(
								CultureInfo.CurrentCulture,
								"Name '{0}' is not member of enum type '{1}'.",
								asString,
								typeof( TEnum )
								),
							ex
						);
					}
				}

				return result;
			}
			// ReSharper disable once RedundantIfElseBlock
			else if ( unpacker.LastReadData.IsTypeOf( this._underlyingType ).GetValueOrDefault() )
			{
				return this.UnpackFromUnderlyingValue( unpacker.LastReadData );
			}
			// ReSharper disable once RedundantIfElseBlock
			else
			{
				throw new SerializationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Type '{0}' is not underlying type of enum type '{1}'.",
						unpacker.LastReadData.UnderlyingType,
						typeof( TEnum )
						)
					);
			}
		}

		/// <summary>
		///		Unpacks enum value from underlying integral value.
		/// </summary>
		/// <param name="messagePackObject">The message pack object which represents some integral value.</param>
		/// <returns>
		///		An enum value.
		/// </returns>
		/// <exception cref="SerializationException">The type of integral value is not compatible with underlying type of the enum.</exception>
		protected abstract TEnum UnpackFromUnderlyingValue( MessagePackObject messagePackObject );

		/// <summary>
		///		Parses the specified string formed integral value and returns as an enum value.
		/// </summary>
		/// <param name="integralValue">The string formed integral value.</param>
		/// <returns>An enum value.</returns>
		/// <exception cref="SerializationException">The type of integral value is not compatible with underlying type of the enum.</exception>
		/// <remarks>
		///		Currently, only form 'D'(decimal) is supported.
		/// </remarks>
		protected abstract TEnum Parse( string integralValue );


		ICustomizableEnumSerializer ICustomizableEnumSerializer.GetCopyAs( EnumSerializationMethod method )
		{
			if ( method == this._serializationMethod )
			{
				return this;
			}

			var clone = this.MemberwiseClone() as EnumMessagePackSerializer<TEnum>;
			// ReSharper disable once PossibleNullReferenceException
			clone._serializationMethod = method;
			return clone;
		}
	}
}