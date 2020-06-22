// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	internal partial class Throw
	{
		public static void NotSupportedBecauseCannotInstanciateAbstractType(Type type)
			=> throw new NotSupportedException($"This operation is not supported because '{type}' cannot be instanciated.");

		public static void DuplicatedAttributes(Dictionary<string, List<MemberInfo>> duplicated)
			=> throw new InvalidOperationException(
				$"Some member keys specified with custom attributes are duplicated. " +
				$"Details: {{{String.Join(", ", duplicated.Select(kv => $"\"{kv.Key}\":[{String.Join(", ", kv.Value.Select(m => $"{m.DeclaringType}.{m.Name}({m.MemberType})"))}]"))}}}"
			);

		public static void MemberIdIsDuplicated(int id, Type targetType)
			=> throw new SerializationException($"The member ID '{id}' is duplicated in the '{targetType}' elementType.");

		public static void DataMemberAttributeCannotBeZeroWhenOneBoundDataMemberOrderIsTrue(Type declaringType, string? memberName)
			=> throw new NotSupportedException(
				$"Cannot specify order value 0 on DataMemberAttribute for '{declaringType}.{memberName}' " +
				$"when SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder is set to true."
			);

		public static void TypeCannotBeSerializedBecauseNoMembersAndNoParameterizedPublicConstructors(Type targetType)
			=> new SerializationException(
				$"Cannot serialize type '{targetType}' because it does not have any serializable fields nor properties, " +
				$"and it does not have any public constructors with parameters."
			);

		public static void TypeCannotBeSerializedBecauseNoMembersAndAmbigiousConstructors(Type targetType, IEnumerable<ConstructorInfo> mostRichConstructors)
			=> throw new SerializationException(
				$"Cannot serialize type '{targetType}' because it does not have any serializable fields nor properties, " +
				$"and serializer generator failed to determine constructor to deserialize among declared constructors" +
				$"({String.Join(", ", mostRichConstructors.Select(ctor => ctor.ToString()).ToArray())})."
			);

		public static void TypeCannotBeSerializedBecauseThereAreMultipleMessagePackDeserializationConstrutorAttribute(Type targetType)
			=> throw new SerializationException(
				$"There are multiple constructors marked with MessagePackDeserializationConstrutorAttribute in type '{targetType}'."
			);

		public static void FailedToGetProperty(string propertyName, string? assemblyQualifiedName)
			=> throw new SerializationException($"Failed to get {propertyName} property from {assemblyQualifiedName} type.");

		public static void MemberIsMarkedWithMemberAndIgnoreAttribute(Type type, string memberName)
			=> throw new SerializationException($"A member '{memberName}' of type '{type}' is marked with both MessagePackMemberAttribute and MessagePackIgnoreAttribute.");

		public static void TargetDoesNotHavePublicDefaultConstructor(Type targetType)
			=> throw new SerializationException($"Type '{targetType}' does not have default (parameterless) public constructor.");

		public static void IsNotSerializableAnyway(Type targetType)
			=> throw new SerializationException($"Cannot serialize type '{targetType}' because it does not have any serializable fields nor properties.");

		public static void UnpackToIsNotSupported(Type type, Exception inner)
			=> throw new NotSupportedException($"This operation is not supported for '{type}' because it does not have accessible Add(T) method.", inner);

		public static void UnexpectedBinaryType(object? value)
			=> throw new NotSupportedException($"Type '{value?.GetType()}' is unexpected for binary value.");

		public static void UnexpectedStringType(object? value)
			=> throw new NotSupportedException($"Type '{value?.GetType()}' is unexpected for string value.");

		public static void CannotSerializeNonPublicTypeUnlessPrivledgedAccessEnabled(Type targetType)
			=> throw new SerializationException($"Cannot serialize type '{targetType}' because it is not public to the serializer.");

		public static void NoConstructorForCollection(Type targetType)
			=> throw new SerializationException($"Cannot deserialize collection type '{targetType}' because it does not have both of public default constructor and public consctructor with Int32 capacity parameter.");

		public static object? UndeserializableCollection(Type targetType)
			=> throw new NotSupportedException($"A generated serialzier for '{targetType}' does not supprot deserialization because the type does not expose Add method.");
	}

#warning EXTRACT
	internal static class DiagnosticsSourceExtensions
	{
		private const string Prefix = "MsgPack.Serialization.";
		private const string PolymorphicPrefix = Prefix + "Polymorphic.";

		public static void DeserializationConstructorFound(this DiagnosticSource source, object value)
			=> source.Write(Prefix + "DeserializationConstructorFound", value);

		public static void DetectedAsDeserializable(this DiagnosticSource source, object value)
			=> source.Write(Prefix + "DetectedAsDeserializable", value);

		public static void DefaultPolymorphicSchemaForValueType(this DiagnosticSource source, object value)
			=> source.Write(PolymorphicPrefix + "DefaultSchemaForValueType", value);

		public static void PolymorphicSchemaCreatedForRootType(this DiagnosticSource source, object value)
			=> source.Write(PolymorphicPrefix + "SchemaCreatedForRootType", value);

		public static void DefaultPolymorphicSchemaForUnqualifiedCollectionMember(this DiagnosticSource source, object value)
			=> source.Write(PolymorphicPrefix + "DefaultSchemaForUnqualifiedCollectionMember", value);

		public static void PolymorphicSchemaCreatedForCollectionMember(this DiagnosticSource source, object value)
			=> source.Write(PolymorphicPrefix + "SchemaCreatedForCollectionMember", value);

		public static void DefaultPolymorphicSchemaForUnqualifiedDictionaryMember(this DiagnosticSource source, object value)
			=> source.Write(PolymorphicPrefix + "DefaultSchemaForUnqualifiedDictionaryMember", value);

		public static void PolymorphicSchemaCreatedForDictionaryMember(this DiagnosticSource source, object value)
			=> source.Write(PolymorphicPrefix + "SchemaCreatedForDictionaryMember", value);

		public static void DefaultPolymorphicSchemaForUnqualifiedTupleMember(this DiagnosticSource source, object value)
			=> source.Write(PolymorphicPrefix + "DefaultSchemaForUnqualifiedTupleMember", value);

		public static void PolymorphicSchemaCreatedForTupleMember(this DiagnosticSource source, object value)
			=> source.Write(PolymorphicPrefix + "SchemaCreatedForTupleMember", value);

		public static void DefaultPolymorphicSchemaForUnqualifiedObjectMember(this DiagnosticSource source, object value)
			=> source.Write(PolymorphicPrefix + "DefaultSchemaForUnqualifiedObjectMember", value);

		public static void PolymorphicSchemaCreatedForObjectMember(this DiagnosticSource source, object value)
			=> source.Write(PolymorphicPrefix + "SchemaCreatedForObjectMember", value);
	}
}
