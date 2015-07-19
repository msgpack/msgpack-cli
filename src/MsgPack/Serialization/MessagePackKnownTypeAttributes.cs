#region -- License Terms --
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
#endregion -- License Terms --

// This code is generated with T4Template from MessagePackKnownTypeAttributes.tt

using System;
using System.Runtime.Serialization;

using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Marks that the runtime type of this member should be encoded with closed type codes for polymorphism.
	/// 	
	/// </summary>
	/// <remarks>
	/// 	When you apply this attribute to a member, the member
	///		will be serialized as 2 element array as [ &lt;type-code&gt;, &lt;actual-value (array or map)&gt;] format 
	///		where the type-code is utf-8 encoded string representing type in your application (system) context.
	///		When you interop with other launages, the deserializer will be able to deserialize object which is actual type when serialized with interoperability.
	///		<note>
	///			You must use one-to-one relationship between type-code and the type.
	///		</note>
	/// </remarks>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true )]
	public sealed class MessagePackKnownTypeAttribute : Attribute, IPolymorphicKnownTypeAttribute
	{
		PolymorphismTarget IPolymorphicHelperAttribute.Target
		{
			get { return PolymorphismTarget.Member; }
		}

		/// <summary>
		///		Gets a type code to be bound.
		/// </summary>
		/// <value>
		///		A type code to be bound.
		/// </value>
		public string TypeCode { get; private set; }

		/// <summary>
		///		Gets the type of the binding <see cref="Type"/> for <see cref="TypeCode"/>.
		/// </summary>
		/// <value>
		///		The binding <see cref="Type"/> for <see cref="TypeCode"/>.
		/// </value>
		public Type BindingType { get; private set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackKnownTypeAttribute"/> class.
		/// </summary>
		/// <param name="typeCode">A string type code to be bound.</param>
		/// <param name="bindingType">The binding <see cref="Type"/> for <paramref name="typeCode"/>.</param>
		public MessagePackKnownTypeAttribute( string typeCode, Type bindingType )
		{
			this.TypeCode = typeCode;
			this.BindingType = bindingType;
		}
	}

	/// <summary>
	///		Marks that the runtime type of items/values of this collection/dictionary should be encoded with closed type codes for polymorphism.
	/// 	
	/// </summary>
	/// <remarks>
	/// 	When you apply this attribute to a member, the items/values of the collection/dictionary
	///		will be serialized as 2 element array as [ &lt;type-code&gt;, &lt;actual-value (array or map)&gt;] format 
	///		where the type-code is utf-8 encoded string representing type in your application (system) context.
	///		When you interop with other launages, the deserializer will be able to deserialize object which is actual type when serialized with interoperability.
	///		<note>
	///			You must use one-to-one relationship between type-code and the type.
	///		</note>
	/// </remarks>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true )]
	public sealed class MessagePackKnownCollectionItemTypeAttribute : Attribute, IPolymorphicKnownTypeAttribute
	{
		PolymorphismTarget IPolymorphicHelperAttribute.Target
		{
			get { return PolymorphismTarget.CollectionItem; }
		}

		/// <summary>
		///		Gets a type code to be bound.
		/// </summary>
		/// <value>
		///		A type code to be bound.
		/// </value>
		public string TypeCode { get; private set; }

		/// <summary>
		///		Gets the type of the binding <see cref="Type"/> for <see cref="TypeCode"/>.
		/// </summary>
		/// <value>
		///		The binding <see cref="Type"/> for <see cref="TypeCode"/>.
		/// </value>
		public Type BindingType { get; private set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackKnownCollectionItemTypeAttribute"/> class.
		/// </summary>
		/// <param name="typeCode">A string type code to be bound.</param>
		/// <param name="bindingType">The binding <see cref="Type"/> for <paramref name="typeCode"/>.</param>
		public MessagePackKnownCollectionItemTypeAttribute( string typeCode, Type bindingType )
		{
			this.TypeCode = typeCode;
			this.BindingType = bindingType;
		}
	}

	/// <summary>
	///		Marks that the runtime type of keys of this dictionary should be encoded with closed type codes for polymorphism.
	/// 	
	/// </summary>
	/// <remarks>
	/// 	When you apply this attribute to a member, the keys of the dictionary
	///		will be serialized as 2 element array as [ &lt;type-code&gt;, &lt;actual-value (array or map)&gt;] format 
	///		where the type-code is utf-8 encoded string representing type in your application (system) context.
	///		When you interop with other launages, the deserializer will be able to deserialize object which is actual type when serialized with interoperability.
	///		<note>
	///			You must use one-to-one relationship between type-code and the type.
	///		</note>
	/// </remarks>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true )]
	public sealed class MessagePackKnownDictionaryKeyTypeAttribute : Attribute, IPolymorphicKnownTypeAttribute
	{
		PolymorphismTarget IPolymorphicHelperAttribute.Target
		{
			get { return PolymorphismTarget.DictionaryKey; }
		}

		/// <summary>
		///		Gets a type code to be bound.
		/// </summary>
		/// <value>
		///		A type code to be bound.
		/// </value>
		public string TypeCode { get; private set; }

		/// <summary>
		///		Gets the type of the binding <see cref="Type"/> for <see cref="TypeCode"/>.
		/// </summary>
		/// <value>
		///		The binding <see cref="Type"/> for <see cref="TypeCode"/>.
		/// </value>
		public Type BindingType { get; private set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackKnownDictionaryKeyTypeAttribute"/> class.
		/// </summary>
		/// <param name="typeCode">A string type code to be bound.</param>
		/// <param name="bindingType">The binding <see cref="Type"/> for <paramref name="typeCode"/>.</param>
		public MessagePackKnownDictionaryKeyTypeAttribute( string typeCode, Type bindingType )
		{
			this.TypeCode = typeCode;
			this.BindingType = bindingType;
		}
	}

	/// <summary>
	///		Marks that the runtime type of specified item of the tuple should be encoded with closed type codes for polymorphism.
	/// 	
	/// </summary>
	/// <remarks>
	/// 	When you apply this attribute to a member, the item of tuple,
	///		will be serialized as 2 element array as [ &lt;type-code&gt;, &lt;actual-value (array or map)&gt;] format 
	///		where the type-code is utf-8 encoded string representing type in your application (system) context.
	///		When you interop with other launages, the deserializer will be able to deserialize object which is actual type when serialized with interoperability.
	///		<note>
	///			You must use one-to-one relationship between type-code and the type.
	///		</note>
	/// </remarks>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true )]
	public sealed partial class MessagePackKnownTupleItemTypeAttribute : Attribute, IPolymorphicKnownTypeAttribute
	{
		PolymorphismTarget IPolymorphicHelperAttribute.Target
		{
			get { return PolymorphismTarget.TupleItem; }
		}

		/// <summary>
		///		Gets a type code to be bound.
		/// </summary>
		/// <value>
		///		A type code to be bound.
		/// </value>
		public string TypeCode { get; private set; }

		/// <summary>
		///		Gets the type of the binding <see cref="Type"/> for <see cref="TypeCode"/>.
		/// </summary>
		/// <value>
		///		The binding <see cref="Type"/> for <see cref="TypeCode"/>.
		/// </value>
		public Type BindingType { get; private set; }

	}


	partial class MessagePackKnownTupleItemTypeAttribute : IPolymorphicTupleItemTypeAttribute
	{
		/// <summary>
		///		Gets the target tuple item's number.
		/// </summary>
		/// <value>
		///		The 1-based target tuple item's number.
		/// </value>
		/// <remarks>
		///		<para>
		///			If this value is not valid for the tuple, this whole instance should be ignored.
		///		</para>
		///		<para>
		///			If same values are specified multiply, the result is undefined.
		///		</para>
		/// </remarks>
		public int ItemNumber { get; private set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackKnownTupleItemTypeAttribute"/> class.
		/// </summary>
		/// <param name="itemNumber">The 1-based target item number of the tuple. The attribute which has invalid value should be ignored.</param>
		/// <param name="typeCode">A string type code to be bound.</param>
		/// <param name="bindingType">The binding <see cref="Type"/> for <paramref name="typeCode"/>.</param>
		public MessagePackKnownTupleItemTypeAttribute( int itemNumber, string typeCode, Type bindingType )
		{
			this.ItemNumber = itemNumber;
			this.TypeCode = typeCode;
			this.BindingType = bindingType;
		}
	}
}