// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Configures compatibility options of the serialization runtime.
	/// </summary>
	/// <remarks>
	///		These options should affect serializer behavior even if you disables runtime or ahead-of-time code generation.
	/// </remarks>
	public sealed class SerializationCompatibilityOptionsBuilder
	{
		private static readonly Func<Type, ISerializerGenerationOptions, bool>[] DefaultSerializableAnywayInterfaceDetectors = Array.Empty<Func<Type, ISerializerGenerationOptions, bool>>();

		private static readonly Func<Type, ISerializerGenerationOptions, bool>[] DefaultDeserializableInterfaceDetectors = Array.Empty<Func<Type, ISerializerGenerationOptions, bool>>();

		/// <summary>
		///		Gets the value indicating whether array serialization method should use 1 bound order instead of default 0 bound order  for <see cref="MessagePackMemberAttribute.Id"/> or simular attribute properties.
		/// </summary>
		/// <value>
		///		<c>true</c>, if the array serialization method should use 1 bound order; <c>false</c>, otherwise.
		/// </value>
		/// <remarks>
		///		Using this value, you can switch between MessagePack for CLI and ProtoBuf.NET seamlessly.
		/// </remarks>
		public bool UsesOneBoundDataMemberOrder { get; private set; } = false;

		/// <summary>
		///		Gets the value indicating whether serialization runtime should serialize a type implementing <see cref="IEnumerable"/> as a normal type if a public <c>Add</c> method is not found
		/// </summary>
		/// <value>
		///		<c>true</c> if serialization runtime should serialize a type implementing <see cref="IEnumerable"/> as a normal type if a public <c>Add</c> method is not found; otherwise, <c>false</c>. The default is <c>true</c>.
		/// </value>
		/// <remarks>
		///		Historically, MessagePack for CLI always tried to serialize any type that implemented <see cref="IEnumerable"/> as a collection, throwing an exception
		///		if an <c>Add</c> method could not be found. However, for types that implement <see cref="IEnumerable"/> but don't have an <c>Add</c> method the generator will now
		///		serialize the type as a non-collection type. To restore the old behavior for backwards compatibility, set this option to <c>false</c>.
		/// </remarks>
		public bool AllowsNonCollectionEnumerableTypes { get; private set; } = true;

		/// <summary>
		///		Gets the delegate which provides <see cref="MessagePackMemberAttribute"/> compatible data from attributes for the member and its declaring type.
		/// </summary>
		/// <value>
		///		The delegate which provides <see cref="MessagePackMemberAttribute"/> compatible data from attributes for the member and its declaring type.
		///		Note that 1st argument is attributes of the type, and 2nd argument is of the member.
		///		If there are no attribute which is compatible to <see cref="MessagePackMemberAttribute"/>, the delegate must return <c>null</c>.
		///		<c>null</c> means usage of default logic which treats <see cref="System.Runtime.Serialization.DataMemberAttribute" /> as <see cref="MessagePackMemberAttribute"/> 
		///		only if the target type also marked with <see cref="System.Runtime.Serialization.DataContractAttribute" />.
		///		In addition, <see cref="System.Runtime.Serialization.DataMemberAttribute.Order"/> will be treated as <see cref="MessagePackMemberAttribute.Id" />,
		///		and <see cref="System.Runtime.Serialization.DataMemberAttribute.Name"/> will be treated as <see cref="MessagePackMemberAttribute.Name"/>.
		/// </value>
		/// <remarks>
		///		This property is used for compatibility with other well known libraries.
		/// </remarks>
		public Func<IEnumerable<CustomAttributeData>, IEnumerable<CustomAttributeData>, MessagePackMemberAttributeData?>? MessagePackMemberAttributeCompatibilityProvider { get; set; }

		/// <summary>
		///		Gets the delegate which provides <see cref="MessagePackIgnoreAttribute"/> compatible data from attributes for the member and its declaring type.
		/// </summary>
		/// <value>
		///		The delegate which provides <see cref="MessagePackIgnoreAttribute"/> compatible data from attributes for the member and its declaring type.
		///		Note that 1st argument is attributes of the type, and 2nd argument is of the member.
		///		If there are no attribute which is compatible to <see cref="MessagePackIgnoreAttribute"/>, the delegate must return <c>null</c>.
		///		<c>null</c> means usage of default logic which treats <see cref="System.Runtime.Serialization.IgnoreDataMemberAttribute" /> as <see cref="MessagePackIgnoreAttribute"/>.
		/// </value>
		/// <remarks>
		///		This property is used for compatibility with other well known libraries.
		///		Currently, the <see cref="MessagePackIgnoreAttributeData"/> struct has no members, so non-null value means the member is ignored.
		/// </remarks>
		public Func<IEnumerable<CustomAttributeData>, IEnumerable<CustomAttributeData>, MessagePackIgnoreAttributeData?>? IgnoringAttributeCompatibilityProvider { get; private set; }

		// These members used by compatibility layer.
#warning TODO: CompatiblityPackage:	typeof(IPackable).IsAssignableFrom(t) || typeof(IUnpackable).IsAssignableFrom(t) || (!o.WithAsync || typeof(IAsyncPackable).IsAssignableFrom(t) || typeof(IAsyncUnpackable).IsAssignableFrom(t))
		internal IList<Func<Type, ISerializerGenerationOptions, bool>> SerializableAnywayInterfaceDetectors { get; } = new List<Func<Type, ISerializerGenerationOptions, bool>>(DefaultSerializableAnywayInterfaceDetectors);

#warning TODO: CompatiblityPackage: typeof(IUnpackable).IsAssignableFrom(t) && (!o.WithAsync || typeof(IAsyncUnpackable).IsAssignable(t))
		internal IList<Func<Type, ISerializerGenerationOptions, bool>> DeserializableInterfaceDetectors { get; } = new List<Func<Type, ISerializerGenerationOptions, bool>>(DefaultDeserializableInterfaceDetectors);

		internal SerializationCompatibilityOptionsBuilder() { }

		/// <summary>
		///		Indicates using 1 bound order instead of 0 bound  for <see cref="MessagePackMemberAttribute.Id"/> or simular attribute properties.
		/// </summary>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		Using this value, you can switch between MessagePack for CLI and ProtoBuf.NET seamlessly.
		/// </remarks>
		public SerializationCompatibilityOptionsBuilder UseOneBoundDataMemberOrder()
		{
			this.UsesOneBoundDataMemberOrder = true;
			return this;
		}

		/// <summary>
		///		Indicates using 0 bound order for <see cref="MessagePackMemberAttribute.Id"/> or simular attribute properties.
		/// </summary>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="UsesOneBoundDataMemberOrder"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public SerializationCompatibilityOptionsBuilder UseZeroBoundDataMemberOrder()
		{
			this.UsesOneBoundDataMemberOrder = false;
			return this;
		}

		/// <summary>
		///		Indicates serialization runtime should NOT throw an exception for a type implementing <see cref="IEnumerable"/> and the type does not expose a public <c>Add</c> method.
		/// </summary>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="AllowsNonCollectionEnumerableTypes"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		/// </remarks>
		public SerializationCompatibilityOptionsBuilder AllowNonCollectionEnumerableTypes()
		{
			this.AllowsNonCollectionEnumerableTypes = true;
			return this;
		}

		/// <summary>
		///		Indicates serialization runtime should throw an exception for a type implementing <see cref="IEnumerable"/> and the type does not expose a public <c>Add</c> method.
		/// </summary>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		public SerializationCompatibilityOptionsBuilder DisallowNonCollectionEnumerableTypes()
		{
			this.AllowsNonCollectionEnumerableTypes = false;
			return this;
		}

		/// <summary>
		///		Indicates to use default logic to provide <see cref="MessagePackMemberAttribute"/> compatible data from attributes for the member and its declaring type.
		/// </summary>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="MessagePackMemberAttributeCompatibilityProvider"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		///		The default logic treats <see cref="System.Runtime.Serialization.DataMemberAttribute" /> as <see cref="MessagePackMemberAttribute"/> 
		///		only if the target type also marked with <see cref="System.Runtime.Serialization.DataContractAttribute" />.
		///		In addition, <see cref="System.Runtime.Serialization.DataMemberAttribute.Order"/> will be treated as <see cref="MessagePackMemberAttribute.Id" />,
		///		and <see cref="System.Runtime.Serialization.DataMemberAttribute.Name"/> will be treated as <see cref="MessagePackMemberAttribute.Name"/>.
		/// </remarks>
		public SerializationCompatibilityOptionsBuilder UseDefaultMessagePackMemberAttributeCompatibilityProvider()
		{
			this.MessagePackMemberAttributeCompatibilityProvider = null;
			return this;
		}

		/// <summary>
		///		Sets the delegate which provides <see cref="MessagePackMemberAttribute"/> compatible data from attributes for the member and its declaring type.
		/// </summary>
		/// <param name="value">
		///		The delegate which provides <see cref="MessagePackMemberAttribute"/> compatible data from attributes for the member and its declaring type.
		///		Note that 1st argument is attributes of the type, and 2nd argument is of the member.
		///		If there are no attribute which is compatible to <see cref="MessagePackMemberAttribute"/>, the delegate must return <c>null</c>.
		/// </param>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
		/// <remarks>
		///		<see cref="MessagePackMemberAttributeCompatibilityProvider"/> property is used for compatibility with other well known libraries.
		/// </remarks>
		public SerializationCompatibilityOptionsBuilder UseCustomMessagePackMemberAttributeCompatibilityProvider(
			Func<IEnumerable<CustomAttributeData>, IEnumerable<CustomAttributeData>, MessagePackMemberAttributeData?> value
		)
		{
			this.MessagePackMemberAttributeCompatibilityProvider = Ensure.NotNull(value);
			return this;
		}

		/// <summary>
		///		Indicates to use default logic to provide <see cref="MessagePackIgnoreAttribute"/> compatible data from attributes for the member and its declaring type.
		/// </summary>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <remarks>
		///		This method resets <see cref="IgnoringAttributeCompatibilityProvider"/> to default state.
		///		It is not required to call this method normally, but you can use this method to clarify option settings.
		///		The default logic treats <see cref="System.Runtime.Serialization.IgnoreDataMemberAttribute" /> as <see cref="MessagePackIgnoreAttribute"/>.
		/// </remarks>
		public SerializationCompatibilityOptionsBuilder UseDefaultIgnoringAttributeCompatibilityProvider()
		{
			this.IgnoringAttributeCompatibilityProvider = null;
			return this;
		}

		/// <summary>
		///		Sets the delegate which provides <see cref="MessagePackIgnoreAttribute"/> compatible data from attributes for the member and its declaring type.
		/// </summary>
		/// <param name="value">
		///		The delegate which provides <see cref="MessagePackIgnoreAttribute"/> compatible data from attributes for the member and its declaring type.
		///		Note that 1st argument is attributes of the type, and 2nd argument is of the member.
		///		If there are no attribute which is compatible to <see cref="MessagePackIgnoreAttribute"/>, the delegate must return <c>null</c>.
		/// </param>
		/// <returns>This <see cref="SerializationCompatibilityOptionsBuilder"/> instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
		/// <remarks>
		///		<see cref="IgnoringAttributeCompatibilityProvider"/> property is used for compatibility with other well known libraries.
		///		Currently, the <see cref="MessagePackIgnoreAttributeData"/> struct has no members, so non-null value means the member is ignored.
		/// </remarks>
		public SerializationCompatibilityOptionsBuilder UseCustomIgnoringAttributeCompatibilityProvider(
			Func<IEnumerable<CustomAttributeData>, IEnumerable<CustomAttributeData>, MessagePackIgnoreAttributeData?> value
		)
		{
			this.IgnoringAttributeCompatibilityProvider = Ensure.NotNull(value);
			return this;
		}
	}
}
