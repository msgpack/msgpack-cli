// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MsgPack.Internal;

namespace MsgPack.Serialization.BuiltinSerializers
{
	/// <summary>
	///		<see cref="IObjectSerializerProvider"/> for enums.
	/// </summary>
	internal sealed class EnumSerializerProvider : IObjectSerializerProvider
	{
		private readonly EnumSerializationMethod? _methodOnType;
		private readonly ObjectSerializer _byName;
		private readonly ObjectSerializer _byUnderlyingValue;
		private readonly ObjectSerializer _dynamic;

		/// <summary>
		///		Initializes a new instance of <see cref="EnumSerializerProvider"/> object.
		/// </summary>
		/// <param name="targetType">The type of the target enum type.</param>
		/// <param name="ownerProvider">The provider which will own this instance.</param>
		/// <param name="builder"><see cref="SerializerBuilder"/> which generated actual enum serializers.</param>
		public EnumSerializerProvider(
			Type targetType,
			SerializerProvider ownerProvider
		)
		{
			Debug.Assert(targetType.GetIsEnum());

			this._methodOnType = targetType.GetCustomAttribute<MessagePackEnumAttribute>()?.SerializationMethod;

			var nameMapper = SerializationMetadataFactory.GetEnumMetadata(targetType, ownerProvider.SerializerGenerationOptions.EnumOptions);
			typeof(EnumHelper<>).MakeGenericType(targetType)
				.GetMethod("CreateNameMapping")!
				.CreateDelegate<CreateNameMapping>()
				.Invoke(ownerProvider.SerializerGenerationOptions, out var serializationNameMapping, out var deserializationNameMapping);

			this._byName =
				SerializerFactory.CreateEnumSerializer(
					targetType, 
					ownerProvider, 
					EnumSerializationMethod.ByName,
					nameMapper,
					serializationNameMapping,
					deserializationNameMapping
				);
			this._byUnderlyingValue =
				SerializerFactory.CreateEnumSerializer(
					targetType,
					ownerProvider,
					EnumSerializationMethod.ByUnderlyingValue,
					nameMapper,
					serializationNameMapping,
					deserializationNameMapping
				);
			this._dynamic = 
				SerializerFactory.CreateObjectSerializer(
					typeof(DynamicEnumSerializer<>), 
					targetType, 
					ownerProvider, 
					this._byName, 
					this._byUnderlyingValue
				);
		}

		/// <inheritdoc />
		public ObjectSerializer GetSerializer(Type targetType, object? providerParameter)
		{
			// Specified by caller -- based on MessagePackEnumMemberAttribute
			if (providerParameter is BoxedEnumSerializationMethod boxed)
			{
				return this.GetSerializer(boxed.Value);
			}

			if (providerParameter is EnumSerializationMethod enumSerializationMethod)
			{
				return this.GetSerializer(enumSerializationMethod);
			}

			switch (this._methodOnType)
			{
				case EnumSerializationMethod.ByName:
				{
					return this._byName;
				}
				case EnumSerializationMethod.ByUnderlyingValue:
				{
					return this._byUnderlyingValue;
				}
				default: // null
				{
					// It will determine method with codec and the option.
					return this._dynamic;
				}
			}
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private ObjectSerializer GetSerializer(EnumSerializationMethod enumSerializationMethod)
			=> enumSerializationMethod == EnumSerializationMethod.ByUnderlyingValue ? this._byUnderlyingValue : this._byName;

		private delegate void CreateNameMapping(ISerializerGenerationOptions options, out object serializationNameMapping, out object deserializationNameMapping);

		private static class EnumHelper<TEnum>
		{
			public static void CreateNameMapping(ISerializerGenerationOptions options, out object serializationNameMapping, out object deserializationNameMapping)
			{
				var members = (Enum.GetValues(typeof(TEnum)) as TEnum[])!;
#pragma warning disable CS8714
				var serializationNameMappingT = new Dictionary<TEnum, string>(members.Length);
#pragma warning restore CS8714
				var deserializationNameMappingT = new Dictionary<string, TEnum>(members.Length);

				foreach (var member in members)
				{
					var asString = options.EnumOptions.NameTransformer(member!.ToString()!);
					serializationNameMappingT[member] = asString;
					deserializationNameMappingT[asString] = member;
				}

				serializationNameMapping = serializationNameMappingT;
				deserializationNameMapping = deserializationNameMappingT;
			}
		}

	}
}
