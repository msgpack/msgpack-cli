// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents compatibility options of serialization runtime.
	/// </summary>
	/// <remarks>
	///		A term 'compatibility' has 2 meanings:
	///		<list type="number">
	///			<item>Backward compatibility for old design.</item>
	///			<item>User code compatibility for other wellknown libraries.</item>
	///		</list>
	/// </remarks>
	internal interface ISerializationCompatibilityOptions
	{
		/// <summary>
		///		Gets the value indicating whether array serialization method should use 1 bound order instead of default 0 bound order.
		/// </summary>
		/// <value>
		///		<c>true</c>, if the array serialization method should use 1 bound order; <c>false</c>, otherwise.
		/// </value>
		/// <remarks>
		///		Using this value, you can switch between MessagePack for CLI and ProtoBuf.NET seamlessly.
		/// </remarks>
		bool UsesOneBoundDataMemberOrder { get; }

		/// <summary>
		///		Gets the value indicating whether collection serialization runtime should ignore adapter interfaces.
		/// </summary>
		/// <value>
		///		<c>true</c>, if the collection serialization runtime should ignore adapter interfaces; <c>false</c>, otherwise.
		/// </value>
		/// <remarks>
		///		<para>
		///			Adapter interfaces are interfaces which are qualified with <see cref="CompatibilityAdapterAttribute"/>.
		///		</para>
		///		<para>
		///			Historically, MessagePack for CLI ignored packability interfaces (<c>IPackable</c>, <c>IUnpackable</c>, 
		///			<c>IAsyncPackable"</c> and <c>IAsyncUnpackable</c>) for collection which implements <see cref="IEquatable{T}"/> (except <see cref="String"/> and its kinds).
		///			As of 0.7, the generator respects such interfaces even if the target type is collection.
		///			Although this behavior is desirable and correct, setting this property <c>true</c> turn out the new behavior for backward compatibility.
		///		</para>
		/// </remarks>
		bool IgnoresAdapterForCollection { get; }

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
		bool AllowsNonCollectionEnumerableTypes { get; }

		/// <summary>
		///		Gets the collection of delegates which detects 'Serializable' interface from an interface type and current options.
		/// </summary>
		/// <value>
		///		The collection of delegates which detects 'Serializable' interface from an interface type and current options.
		/// </value>
		/// <remarks>
		///		This property is used for backward compatibility.
		///		Note that appropriate <see cref="ObjectSerializer" /> for the interface must be registered to success serialization of types which implements the 'Serializable' interface.
		/// </remarks>
		IEnumerable<Func<Type, ISerializerGenerationOptions, bool>> SerializableInterfaceDetectors { get; }

		/// <summary>
		///		Gets the collection of delegates which detects 'Deserializable' interface from an interface type and current options.
		/// </summary>
		/// <value>
		///		The collection of delegates which detects 'Deserializable' interface from an interface type and current options.
		/// </value>
		/// <remarks>
		///		This property is used for backward compatibility.
		///		Note that appropriate <see cref="ObjectSerializer" /> for the interface must be registered to success deserialization of types which implements the 'Deserializable' interface.
		///		In addition, the <see cref="ObjectSerializer"/> implements <see cref="ObjectSerializer.DeserializeObjectTo"/> and related methods
		///		(<see cref="ObjectSerializer.DeserializeObjectToAsync"/>, <see cref="ObjectSerializer{T}.DeserializeTo"/>, and <see cref="ObjectSerializer{T}.DeserializeToAsync"/>).
		/// </remarks>
		IEnumerable<Func<Type, ISerializerGenerationOptions, bool>> DeserializableInterfaceDetectors { get; }

		/// <summary>
		///		Gets the delegate which provides <see cref="MessagePackMemberAttribute"/> compatible data from attributes for the member and its declaring type.
		/// </summary>
		/// <value>
		///		The delegate which provides <see cref="MessagePackMemberAttribute"/> compatible data from attributes for the member and its declaring type.
		///		Note that 1st argument is attributes of the type, and 2nd argument is of the member.
		///		If there are no attribute which is compatible to <see cref="MessagePackMemberAttribute"/>, the delegate must return <c>null</c>.
		/// </value>
		/// <remarks>
		///		This property is used for compatibility with other well known libraries.
		/// </remarks>
		Func<IEnumerable<CustomAttributeData>, IEnumerable<CustomAttributeData>, MessagePackMemberAttributeData?> MessagePackMemberAttributeCompatibilityProvider { get; }

		/// <summary>
		///		Gets the delegate which provides <see cref="MessagePackIgnoreAttribute"/> compatible data from attributes for the member and its declaring type.
		/// </summary>
		/// <value>
		///		The delegate which provides <see cref="MessagePackIgnoreAttribute"/> compatible data from attributes for the member and its declaring type.
		///		Note that 1st argument is attributes of the type, and 2nd argument is of the member.
		///		If there are no attribute which is compatible to <see cref="MessagePackIgnoreAttribute"/>, the delegate must return <c>null</c>.
		/// </value>
		/// <remarks>
		///		This property is used for compatibility with other well known libraries.
		///		Currently, the <see cref="MessagePackIgnoreAttributeData"/> struct has no members, so non-null value means the member is ignored.
		/// </remarks>
		Func<IEnumerable<CustomAttributeData>, IEnumerable<CustomAttributeData>, MessagePackIgnoreAttributeData?> IgnoringAttributeCompatibilityProvider { get; }
	}
}
