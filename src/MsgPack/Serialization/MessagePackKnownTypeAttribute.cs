// #region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015 FUJIWARA, Yusuke
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
// #endregion -- License Terms --

using System;
using System.Runtime.Serialization;

using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization
{
	/// <summary>
	/// 	Marks that the runtime type of this member should be encoded with closed type codes for polymorphism.
	/// </summary>
	/// <remarks>
	/// 	When you apply this attribute to a member, the member will be serialized using MessagePack ext-type,
	/// 	so deserializer will be able to deserialize object which is actual type when serialized with interoperability.
	///		<note>
	///			You must use one-to-one relationship between type-code and the type.
	///		</note>
	/// </remarks>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true )]
	public sealed class MessagePackKnownTypeAttribute : Attribute, IPolymorhicHelperAttribute
	{
		/// <summary>
		///		Gets or sets the target of this attribute.
		/// </summary>
		/// <value>
		///		The <see cref="PolymorphismTarget"/> of this attribute. The default is <see cref="PolymorphismTarget.Default"/>.
		/// </value>
		/// <remarks>
		///		You can specify only following combination per qualifying members:
		///		<list type="bullet">
		///			<item>Use only single attribute with <see cref="PolymorphismTarget.Default"/> (or no <see cref="Target"/> named argument) per member.</item>
		///			<item>Use at most one <see cref="PolymorphismTarget.Member"/> and at most one <see cref="PolymorphismTarget.CollectionItem"/> per member.</item>
		///		</list>
		///		If you violate above condition, <see cref="System.Runtime.Serialization.SerializationException"/> will be thrown.
		/// </remarks>
		public PolymorphismTarget Target { get; set; }

		/// <summary>
		///		Gets the ext-type code to be bound.
		/// </summary>
		/// <value>
		///		The ext-type code to be bound.
		/// </value>
		public byte BindingCode { get; private set; }

		/// <summary>
		///		Gets the type of the binding <see cref="Type"/> for <see cref="BindingCode"/>.
		/// </summary>
		/// <value>
		///		The binding <see cref="Type"/> for <see cref="BindingCode"/>.
		/// </value>
		public Type BindingType { get; private set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackKnownTypeAttribute"/> class.
		/// </summary>
		/// <param name="bindingCode">The ext-type code to be bound.</param>
		/// <param name="bindingType">The binding <see cref="Type"/> for <paramref name="bindingCode"/>.</param>
		public MessagePackKnownTypeAttribute( byte bindingCode, Type bindingType )
		{
			this.BindingCode = bindingCode;
			this.BindingType = bindingType;
		}

		byte IPolymorhicHelperAttribute.GetBindingCode( SerializationContext context )
		{
			if ( this.BindingCode > 127 )
			{
				throw new SerializationException( "BindingCode must be under 128(0x80)." );
			}

			return this.BindingCode;
		}
	}
}