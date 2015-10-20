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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
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
		where TEnum : struct
	{
		private readonly Type _underlyingType;
		private EnumSerializationMethod _serializationMethod; // not readonly -- changed in cloned instance in GetCopyAs()

		/// <summary>
		///	 Initializes a new instance of the <see cref="EnumMessagePackSerializer{TEnum}"/> class.
		/// </summary>
		/// <param name="ownerContext">A <see cref="SerializationContext"/> which owns this serializer.</param>
		/// <param name="serializationMethod">The <see cref="EnumSerializationMethod"/> which determines serialization form of the enums.</param>
		/// <exception cref="InvalidOperationException"><c>TEnum</c> is not enum type.</exception>
		protected EnumMessagePackSerializer( SerializationContext ownerContext, EnumSerializationMethod serializationMethod )
			: base( ownerContext )
		{
			if ( !typeof( TEnum ).GetIsEnum() )
			{
				throw new InvalidOperationException(
					String.Format( CultureInfo.CurrentCulture, "Type '{0}' is not enum.", typeof( TEnum ) )
				);
			}

			this._serializationMethod = serializationMethod;
			this._underlyingType = Enum.GetUnderlyingType( typeof( TEnum ) );
		}

		/// <summary>
		///		Serializes specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>. This value will not be <c>null</c>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal sealed override void PackToCore( Packer packer, TEnum objectTree )
		{
			if ( this._serializationMethod == EnumSerializationMethod.ByUnderlyingValue )
			{
				this.PackUnderlyingValueTo( packer, objectTree );
			}
			else
			{
				packer.PackString( objectTree.ToString() );
			}
		}

		/// <summary>
		///		Packs enum value as its underlying value.
		/// </summary>
		/// <param name="packer">The packer.</param>
		/// <param name="enumValue">The enum value to be packed.</param>
		protected internal abstract void PackUnderlyingValueTo( Packer packer, TEnum enumValue );

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
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal sealed override TEnum UnpackFromCore( Unpacker unpacker )
		{
			if ( unpacker.LastReadData.IsRaw )
			{
				var asString = unpacker.LastReadData.AsString();

				TEnum result;
#if NETFX_35 || UNITY
				try
				{
					result = ( TEnum ) Enum.Parse( typeof( TEnum ), asString, false );
				}
				catch ( ArgumentException ex )
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
#else
				if ( !Enum.TryParse( asString, false, out result ) )
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Name '{0}' is not member of enum type '{1}'.",
							asString,
							typeof( TEnum )
						)
					);
				}
#endif // NETFX_35 || UNITY

				return result;
			}
			else if ( unpacker.LastReadData.IsTypeOf( this._underlyingType ).GetValueOrDefault() )
			{
				return this.UnpackFromUnderlyingValue( unpacker.LastReadData );
			}
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
		protected internal abstract TEnum UnpackFromUnderlyingValue( MessagePackObject messagePackObject );

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

#if UNITY
	internal abstract class UnityEnumMessagePackSerializer : NonGenericMessagePackSerializer, ICustomizableEnumSerializer
	{
		private readonly Type _underlyingType;
		private EnumSerializationMethod _serializationMethod; // not readonly -- changed in cloned instance in GetCopyAs()

		protected UnityEnumMessagePackSerializer( SerializationContext ownerContext, Type targetType, EnumSerializationMethod serializationMethod )
			: base( ownerContext, targetType )
		{
			if ( !targetType.GetIsEnum() )
			{
				throw new InvalidOperationException(
					String.Format( CultureInfo.CurrentCulture, "Type '{0}' is not enum.", targetType )
				);
			}

			this._serializationMethod = serializationMethod;
			this._underlyingType = Enum.GetUnderlyingType( targetType );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal sealed override void PackToCore( Packer packer, object objectTree )
		{
			if ( this._serializationMethod == EnumSerializationMethod.ByUnderlyingValue )
			{
				this.PackUnderlyingValueTo( packer, objectTree );
			}
			else
			{
				packer.PackString( objectTree.ToString() );
			}
		}

		protected internal abstract void PackUnderlyingValueTo( Packer packer, object enumValue );

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal sealed override object UnpackFromCore( Unpacker unpacker )
		{
			if ( unpacker.LastReadData.IsRaw )
			{
				var asString = unpacker.LastReadData.AsString();

				try
				{
					return Enum.Parse( this.TargetType, asString, false );
				}
				catch ( ArgumentException ex )
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Name '{0}' is not member of enum type '{1}'.",
							asString,
							this.TargetType
							),
						ex
					);
				}
			}
			else if ( unpacker.LastReadData.IsTypeOf( this._underlyingType ).GetValueOrDefault() )
			{
				return this.UnpackFromUnderlyingValue( unpacker.LastReadData );
			}
			else
			{
				throw new SerializationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Type '{0}' is not underlying type of enum type '{1}'.",
						unpacker.LastReadData.UnderlyingType,
						this.TargetType
					)
				);
			}
		}

		protected internal abstract object UnpackFromUnderlyingValue( MessagePackObject messagePackObject );

		ICustomizableEnumSerializer ICustomizableEnumSerializer.GetCopyAs( EnumSerializationMethod method )
		{
			if ( method == this._serializationMethod )
			{
				return this;
			}

			var clone = this.MemberwiseClone() as UnityEnumMessagePackSerializer;
			// ReSharper disable once PossibleNullReferenceException
			clone._serializationMethod = method;
			return clone;
		}
	}
#endif // UNITY
}