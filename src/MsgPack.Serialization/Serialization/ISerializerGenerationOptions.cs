// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines internal interface for generator.
	/// </summary>
	internal interface ISerializerGenerationOptions
	{
		/// <summary>
		///		Gets the value indicating whether the serializer runtime allows serialization even if feature complete serializer cannot be generated due to lack of some requirement.
		/// </summary>
		/// <value>
		///		<c>true</c> if the serializer runtime allows serialization even if feature complete serializer cannot be generated due to lack of some requirement; otherwise, <c>false</c>. The default is <c>false</c>.
		/// </value>
		/// <remarks>
		///		Currently, the lack of constructor (default or parameterized) or lack of settable members are considerd as "cannot generate feature complete serializer".
		///		Therefore, you can get serialization only serializer if this property is set to <c>true</c>.
		///		This is useful for logging, telemetry injestion, or so.
		///		You can investigate serializer capability via <see cref="Serializer.Capabilities"/> property.
		/// </remarks>
		bool AllowsAsymmetricSerializer { get; }

		/// <summary>
		///		Gets the value which indicates whether on-the-fly runtime code generation is disabled.
		/// </summary>
		/// <value>
		///		<c>true</c> if runtime code generation (via <c>System.Reflection.Emit)</c>) is disabled; <c>false</c>, otherwise.
		/// </value>
		/// <remarks>
		///		If this value set to <c>false</c>, reflection based serializers will be used if they are activated.
		///		If it was not activated, only pre-registered serializers will be used.
		///		Typically, this value will be used for debugging purposes.
		/// </remarks>
		bool IsRuntimeCodeGenerationDisabled { get; }

		/// <summary>
		///		Gets the name of the assembly which holds generated serializer types.
		/// </summary>
		/// <value>
		///		The name of the assembly which holds generated serializer types.
		/// </value>
		string RuntimeCodeGenerationAssemblyName { get; }

		/// <summary>
		///		Gets the <see cref="IBindingOptions"/> object which provides binding related options.
		/// </summary>
		/// <value>
		///		The <see cref="IBindingOptions"/> object which provides binding related options.
		/// </value>
		IBindingOptions BindingOptions { get; }

		/// <summary>
		///		Gets the <see cref="ISerializationCompatibilityOptions"/> object which provides backward or well-known library compatibilities.
		/// </summary>
		/// <value>
		///		The <see cref="ISerializationCompatibilityOptions"/> object which provides backward or well-known library compatibilities.
		/// </value>
		ISerializationCompatibilityOptions CompatibilityOptions { get; }

		/// <summary>
		///		Gets the <see cref="IDictionarySerializationOptions"/> object which provides dictionary serialization options.
		/// </summary>
		/// <value>
		///		The <see cref="IDictionarySerializationOptions"/> object which provides dictionary serialization options.
		/// </value>
		IDictionarySerializationOptions DictionaryOptions { get; }

		/// <summary>
		///		Gets the <see cref="IEnumSerializationOptions"/> object which provides enum serialization options.
		/// </summary>
		/// <value>
		///		The <see cref="IEnumSerializationOptions"/> object which provides enum serialization options.
		/// </value>
		IEnumSerializationOptions EnumOptions { get; }

		/// <summary>
		///		Gets the <see cref="IDateTimeSerializationOptions"/> object which provides <see cref="System.DateTime"/> and simular types serialization options.
		/// </summary>
		/// <value>
		///		The <see cref="IDateTimeSerializationOptions"/> object which provides <see cref="System.DateTime"/> and simular types serialization options.
		/// </value>
		IDateTimeSerializationOptions DateTimeOptions { get; }

		/// <summary>
		///		Gets the <see cref="IDefaultConcreteTypeRepository"/> object which provides concrete type mappings for collection interfaces.
		/// </summary>
		/// <value>
		///		The <see cref="IDefaultConcreteTypeRepository"/> object which provides concrete type mappings for collection interfaces.
		/// </value>
		IDefaultConcreteTypeRepository DefaultCollectionTypes { get; }

		/// <summary>
		///		Gets the <see cref="T:SerializationOptions"/> object wich provides serialization operation level options.
		/// </summary>
		/// <value>
		///		The <see cref="T:SerializationOptions"/> object wich provides serialization operation level options.
		/// </value>
		SerializationOptions SerializationOptions { get; }

		/// <summary>
		///		Gets the <see cref="T:DeserializationOptions"/> object wich provides deserialization operation level options.
		/// </summary>
		/// <value>
		///		The <see cref="T:DeserializationOptions"/> object wich provides deserialization operation level options.
		/// </value>
		DeserializationOptions DeserializationOptions { get; }
	}
}
