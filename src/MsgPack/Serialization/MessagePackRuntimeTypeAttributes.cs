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

// This code is generated with T4Template from MessagePackRuntimeTypeAttributes.tt

using System;

using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Marks that the runtime type of this member should be encoded with type information for polymorphism.
	/// 	
	/// </summary>
	/// <remarks>
	/// 	When you apply this attribute to a member, the member will be serialized with .NET specific type information,
	/// 	so deserializer will be able to deserialize object which is actual type when serialized instead of interoperability.
	/// 	Because non-.NET enviroments (Java, Ruby, Go, etc.) cannot interpret .NET native type identifier, 
	///		you should not use this attribute when the serialized stream will be possible to be used from non-.NET environment.
	///		The typed object will be encoded as 2 elements array as follows, so your deserializer can skip type information as needed:
	///		[ &lt;type-info&gt;, &lt;actual-value (array or map)&gt;]
	///		In this point, type-info will be encoded as compressed assembly qualified name as follows:
	///		[ &lt;compressed type full name&gt;, &lt;assembly simple name&gt;, &lt;version binary&gt;, &lt;culture string&gt;, &lt;public key token binary&gt;]
	///		If the type full name starts with its assembly simple name, then the prefix matched to assembly simple name will be omitted 
	///		(as a result, compressed type name starts with dot).
	///		<note>
	///			You should use this attribute CAREFULLY when you deserialize data from outside, because this feature can inject arbitary process in your code through
	///			constructor and some virtual methods if exist.
	///			It is highly recommended avoid using <see cref="Object" /> type as member's declaring type, 
	///			you should specify your base class which and derived typed are fully controled under your organization instead.
	///			It mitigate chance of potential exploits.
	///		</note>
	/// </remarks>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property )]
	public sealed class MessagePackRuntimeTypeAttribute : Attribute, IPolymorphicRuntimeTypeAttribute
	{
		PolymorphismTarget IPolymorphicHelperAttribute.Target
		{
			get { return PolymorphismTarget.Member; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackRuntimeTypeAttribute"/> class.
		/// </summary>
		public MessagePackRuntimeTypeAttribute()
		{
		}
	}

	/// <summary>
	///		Marks that the runtime type of items/values of this collection/dictionary should be encoded with type information for polymorphism.
	/// 	
	/// </summary>
	/// <remarks>
	/// 	When you apply this attribute to a member, the items/values of the collection/dictionary will be serialized with .NET specific type information,
	/// 	so deserializer will be able to deserialize object which is actual type when serialized instead of interoperability.
	/// 	Because non-.NET enviroments (Java, Ruby, Go, etc.) cannot interpret .NET native type identifier, 
	///		you should not use this attribute when the serialized stream will be possible to be used from non-.NET environment.
	///		The typed object will be encoded as 2 elements array as follows, so your deserializer can skip type information as needed:
	///		[ &lt;type-info&gt;, &lt;actual-value (array or map)&gt;]
	///		In this point, type-info will be encoded as compressed assembly qualified name as follows:
	///		[ &lt;compressed type full name&gt;, &lt;assembly simple name&gt;, &lt;version binary&gt;, &lt;culture string&gt;, &lt;public key token binary&gt;]
	///		If the type full name starts with its assembly simple name, then the prefix matched to assembly simple name will be omitted 
	///		(as a result, compressed type name starts with dot).
	///		<note>
	///			You should use this attribute CAREFULLY when you deserialize data from outside, because this feature can inject arbitary process in your code through
	///			constructor and some virtual methods if exist.
	///			It is highly recommended avoid using <see cref="Object" /> type as member's declaring type, 
	///			you should specify your base class which and derived typed are fully controled under your organization instead.
	///			It mitigate chance of potential exploits.
	///		</note>
	/// </remarks>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property )]
	public sealed class MessagePackRuntimeCollectionItemTypeAttribute : Attribute, IPolymorphicRuntimeTypeAttribute
	{
		PolymorphismTarget IPolymorphicHelperAttribute.Target
		{
			get { return PolymorphismTarget.CollectionItem; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackRuntimeCollectionItemTypeAttribute"/> class.
		/// </summary>
		public MessagePackRuntimeCollectionItemTypeAttribute()
		{
		}
	}

	/// <summary>
	///		Marks that the runtime type of keys of this dictionary should be encoded with type information for polymorphism.
	/// 	
	/// </summary>
	/// <remarks>
	/// 	When you apply this attribute to a member, the keys of the dictionary will be serialized with .NET specific type information,
	/// 	so deserializer will be able to deserialize object which is actual type when serialized instead of interoperability.
	/// 	Because non-.NET enviroments (Java, Ruby, Go, etc.) cannot interpret .NET native type identifier, 
	///		you should not use this attribute when the serialized stream will be possible to be used from non-.NET environment.
	///		The typed object will be encoded as 2 elements array as follows, so your deserializer can skip type information as needed:
	///		[ &lt;type-info&gt;, &lt;actual-value (array or map)&gt;]
	///		In this point, type-info will be encoded as compressed assembly qualified name as follows:
	///		[ &lt;compressed type full name&gt;, &lt;assembly simple name&gt;, &lt;version binary&gt;, &lt;culture string&gt;, &lt;public key token binary&gt;]
	///		If the type full name starts with its assembly simple name, then the prefix matched to assembly simple name will be omitted 
	///		(as a result, compressed type name starts with dot).
	///		<note>
	///			You should use this attribute CAREFULLY when you deserialize data from outside, because this feature can inject arbitary process in your code through
	///			constructor and some virtual methods if exist.
	///			It is highly recommended avoid using <see cref="Object" /> type as member's declaring type, 
	///			you should specify your base class which and derived typed are fully controled under your organization instead.
	///			It mitigate chance of potential exploits.
	///		</note>
	/// </remarks>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property )]
	public sealed class MessagePackRuntimeDictionaryKeyTypeAttribute : Attribute, IPolymorphicRuntimeTypeAttribute
	{
		PolymorphismTarget IPolymorphicHelperAttribute.Target
		{
			get { return PolymorphismTarget.DictionaryKey; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackRuntimeDictionaryKeyTypeAttribute"/> class.
		/// </summary>
		public MessagePackRuntimeDictionaryKeyTypeAttribute()
		{
		}
	}

	/// <summary>
	///		Marks that the runtime type of specified item of the tuple should be encoded with type information for polymorphism.
	/// 	
	/// </summary>
	/// <remarks>
	/// 	When you apply this attribute to a member, the item of tuple will be serialized with .NET specific type information,
	/// 	so deserializer will be able to deserialize object which is actual type when serialized instead of interoperability.
	/// 	Because non-.NET enviroments (Java, Ruby, Go, etc.) cannot interpret .NET native type identifier, 
	///		you should not use this attribute when the serialized stream will be possible to be used from non-.NET environment.
	///		The typed object will be encoded as 2 elements array as follows, so your deserializer can skip type information as needed:
	///		[ &lt;type-info&gt;, &lt;actual-value (array or map)&gt;]
	///		In this point, type-info will be encoded as compressed assembly qualified name as follows:
	///		[ &lt;compressed type full name&gt;, &lt;assembly simple name&gt;, &lt;version binary&gt;, &lt;culture string&gt;, &lt;public key token binary&gt;]
	///		If the type full name starts with its assembly simple name, then the prefix matched to assembly simple name will be omitted 
	///		(as a result, compressed type name starts with dot).
	///		<note>
	///			You should use this attribute CAREFULLY when you deserialize data from outside, because this feature can inject arbitary process in your code through
	///			constructor and some virtual methods if exist.
	///			It is highly recommended avoid using <see cref="Object" /> type as member's declaring type, 
	///			you should specify your base class which and derived typed are fully controled under your organization instead.
	///			It mitigate chance of potential exploits.
	///		</note>
	/// </remarks>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true )]
	public sealed partial class MessagePackRuntimeTupleItemTypeAttribute : Attribute, IPolymorphicRuntimeTypeAttribute
	{
		PolymorphismTarget IPolymorphicHelperAttribute.Target
		{
			get { return PolymorphismTarget.TupleItem; }
		}

	}


	partial class MessagePackRuntimeTupleItemTypeAttribute : IPolymorphicTupleItemTypeAttribute
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
		public MessagePackRuntimeTupleItemTypeAttribute( int itemNumber )
		{
			this.ItemNumber = itemNumber;
		}
	}
}