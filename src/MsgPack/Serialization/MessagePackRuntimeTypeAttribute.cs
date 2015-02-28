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

using MsgPack.Serialization.Polymorphic;

namespace MsgPack.Serialization
{
	/// <summary>
	/// 	Marks that the runtime type of this member should be encoded with type information for polymorphism.
	/// </summary>
	/// <remarks>
	/// 	When you apply this attribute to a member, the member will be serialized with .NET specific type information,
	/// 	so deserializer will be able to deserialize object which is actual type when serialized instead of interoperability.
	/// 	Because non-.NET enviroments (Java, Ruby, Go, etc.) cannot interpret .NET native type identifier, 
	///		you should not use this attribute when the serialized stream will be possible to be used from non-.NET environment.
	///		The typed object will be encoded as 3 elements array as follows, so your deserializer can skip type information as needed:
	///		[ &lt;ext1 - 127(customizable via SerializationContext.TypeEmbeddingSettings) &lt;kind&gt;&gt;, &lt;type-info&gt;, &lt;actual-value (array or map)&gt;]
	///		In this point, type-info will be encoded as compressed assembly qualified name as follows:
	///		[ &lt;compressed type full name&gt;, &lt;assembly simple name&gt;, &lt;version binary&gt;, &lt;culture string&gt;, &lt;public key token binary&gt;]
	///		If the type full name starts with its assembly simple name, then the prefix matched to assembly simple name will be omitted 
	///		(as a result, compressed type name starts with dot).
	/// </remarks>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true )]
	public sealed class MessagePackRuntimeTypeAttribute : Attribute, IPolymorhicHelperAttribute
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
		///		Initializes a new instance of the <see cref="MessagePackRuntimeTypeAttribute"/> class.
		/// </summary>
		public MessagePackRuntimeTypeAttribute() { }

		byte IPolymorhicHelperAttribute.GetBindingCode( SerializationContext context )
		{
			return context.TypeEmbeddingSettings.TypeEmbeddingIdentifier;
		}
	}
}