// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	internal partial class Throw
	{
		public static void CannotBeNull(Type type)
		 => throw new SerializationException($"Value of type '{type}' cannot be null.");

		public static void UnavailableMethod(string name, SerializationMethod method)
			=> throw new NotSupportedException($"SerializationMethod '{method}' is not supported in '{name}' format.");

		public static void NotSupportedBecauseCannotInstanciateAbstractType(Type type)
			=> throw new NotSupportedException($"This operation is not supported because '{type}' cannot be instanciated.");

		public static void DuplicatedAttributes(Dictionary<string, List<MemberInfo>> duplicated)
			=> throw new InvalidOperationException(
				$"Some member keys specified with custom attributes are duplicated. " +
				$"Details: {{{String.Join(", ", duplicated.Select(kv => $"\"{kv.Key}\":[{String.Join(", ", kv.Value.Select(m => $"{m.DeclaringType}.{m.Name}({m.MemberType})"))}]"))}}}"
			);

		public static void MemberIdIsDuplicated(int id, Type targetType)
			=> throw new SerializationException($"The member ID '{id}' is duplicated in the '{targetType}' elementType.");

		public static void DuplicatedOriginalName(string name)
			=> throw new SerializationException($"A mapping for original member name '{name}' is duplicated.");

		public static void IsNotEnum(Type type, string paramName)
			=> throw new ArgumentException($"Type '{type}' is not enum.", paramName);

		public static void UndefinedEnumName(string name, Type enumType, long position)
			=> throw new SerializationException($"Name '{name}' is not considered as valid member of enum type '{enumType}' at position {position:#,0}.");

		public static void UndefinedEnumMember<T>(T value, [CallerArgumentExpression("value")] string paramName = null!)
			where T : Enum
			=> throw new ArgumentOutOfRangeException(paramName, $"Value {value:D}(0x{value:X}) is not valid enum value of '{typeof(T)}' type.");

		public static char? InvalidIso8601DecimalMark(char? value, [CallerArgumentExpression("value")] string paramName = null!)
			=> throw new ArgumentOutOfRangeException(paramName, $"Value '{StringEscape.ForDisplay(value.GetValueOrDefault().ToString())}' is not valid ISO 8601 decimal separator.");

		public static void UnsupportedEnumUnderlyingType(Type underlyingType)
			=> throw new InvalidOperationException($"Type '{underlyingType}' cannot be underlying type of enum.");

		public static void IncompatibleEnumUnderlyingValue(Type sourceType, Type targetEnumType, Type targetUnderlyingValue)
			=> throw new SerializationException($"Cannot convert decoded '{sourceType}' type value to '{targetEnumType}' enum value. Decoded value type is not compatible with underlying type '{targetUnderlyingValue}'.");

		public static void OutOfRangeEnumUnderlyingValue(Type sourceType, Type targetEnumType, Type targetUnderlyingValue, long value)
			=> throw new SerializationException($"Cannot convert decoded '{sourceType}' type value to '{targetEnumType}' enum value. Decoded value '{value:#,0}' is not in range of valid underlying type '{targetUnderlyingValue}'.");

		public static void OutOfRangeEnumUnderlyingValue(Type sourceType, Type targetEnumType, Type targetUnderlyingValue, ulong value)
			=> throw new SerializationException($"Cannot convert decoded '{sourceType}' type value to '{targetEnumType}' enum value. Decoded value '{value:#,0}' is not in range of valid underlying type '{targetUnderlyingValue}'.");

		public static void OutOfRangeEnumUnderlyingValue(Type sourceType, Type targetEnumType, Type targetUnderlyingValue, float value)
			=> throw new SerializationException($"Cannot convert decoded '{sourceType}' type value to '{targetEnumType}' enum value. Decoded value '{value}' is not in range of valid underlying type '{targetUnderlyingValue}'.");
		
		public static void OutOfRangeEnumUnderlyingValue(Type sourceType, Type targetEnumType, Type targetUnderlyingValue, double value)
			=> throw new SerializationException($"Cannot convert decoded '{sourceType}' type value to '{targetEnumType}' enum value. Decoded value '{value}' is not in range of valid underlying type '{targetUnderlyingValue}'.");

		public static void DecodedTypeIsNotEnum(ElementType elementType, long position)
			=> throw new SerializationException($"Element type '{elementType}' is not underlying type of enum type at position {position:#,0}.");

		public static void DateTimeSerializerProviderIsNotRegistered(Type targetType)
			=> throw new SerializationException($"IObjectSerializerProvider for date-time like type '{targetType}' is not registered. It must be registered via SerializerProvider.RegisterDateTimeSerializerProvider in advance.");

		public static void IncompatibleTargetType(Type expected, Type actual)
			=> throw new SerializationException($"Target type '{actual}' is not assignable to type '{expected}' which is expected type by this provider.");

		public static void DuplicatedSerializedName(string name)
			=> throw new SerializationException($"A mapping for serialized member name '{name}' is duplicated");

		internal static void TypeIsMappedToMultipleExtensionTypes(Type type, string[] typeCodes)
			=> throw new SerializationException(
				$"Type '{type}' is mapped to multiple extension type codes({String.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, typeCodes)})."
			);

		public static void UnknownOriginalName(string name)
			=> throw new SerializationException($"Original member name '{name}' is not known.");

		public static void UnknownSerializedName(string name)
			=> throw new SerializationException($"Serialized member name '{name}' is not known.");

		public static void DataMemberAttributeCannotBeZeroWhenOneBoundDataMemberOrderIsTrue(Type declaringType, string? memberName)
			=> throw new NotSupportedException(
				$"Cannot specify order value 0 on DataMemberAttribute for '{declaringType}.{memberName}' " +
				$"when SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder is set to true."
			);

#warning TODO: Revise messages
		public static SerializerBuilder ForRuntimeCodeGenerationNotRegistered()
			=> throw new InvalidOperationException(
				"Runtime code generation engine is not configured. " +
				"Ensure that MsgPack.Serialization.ILGeneration package exists and SerializationContextBuilder.UseRuntimeCodeGeneration() extension method was called."
			);

		public static SerializerBuilder ForReflectionNotRegistered()
			=> throw new InvalidOperationException(
				"Reflection engine is not configured. " +
				"Ensure that MsgPack.Serialization.Reflection package exists and SerializationContextBuilder.UseReflection() extension method was called."
			);

		public static SerializerBuilder ForSourceCodeGenerationNotRegistered()
			=> throw new InvalidOperationException(
				"Source code generation engine is not configured. " +
				"Ensure that MsgPack.Serialization.SourceGeneration package exists and SerializationContextBuilder.UseSourceCodeGeneration() extension method was called."
			);

		public static void TypeCannotBeSerializedBecauseNoMembersAndNoParameterizedPublicConstructors(Type targetType)
			=> throw new SerializationException(
				$"Cannot serialize type '{targetType}' because it does not have any serializable fields nor properties, " +
				$"and it does not have any public constructors with parameters."
			);

		internal static void TargetTypeIsNotDateTimeLikeType(Type targetType, IEnumerable<Type> knownDateTimeTypes)
			=> throw new InvalidOperationException($"Type '{targetType}' is not known DateTime like type. Allowed types are [{String.Join(", ", knownDateTimeTypes)}] and their nullable wrapper types.");

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

		public static void TargetDoesNotHavePublicDefaultConstructorNorInitialCapacity(Type collectionInstanceType)
			=> throw new SerializationException($"Type '{collectionInstanceType}' does not have default any public collection constructors (.ctor(), .ctor(System.Int32), .ctor(System.Collections.Generic.IEqualityComparer<T>), .ctor(System.Int32, System.Collections.Generic.IEqualityComparer<T>), or .ctor(System.Collections.Generic.IEqualityComparer<T>, System.Int32)).");

		public static void IsNotSerializableAnyway(Type targetType)
			=> throw new SerializationException($"Cannot serialize type '{targetType}' because it does not have any serializable fields nor properties.");

		public static void UnpackToIsNotSupported(Type type, Exception inner)
			=> throw new NotSupportedException($"This operation is not supported for '{type}' because it does not have accessible Add(T) method.", inner);

		public static void UnexpectedBinaryType(object? value)
			=> throw new NotSupportedException($"Type '{value?.GetType()}' is unexpected for binary value.");

		public static void UnexpectedStringType(object? value)
			=> throw new NotSupportedException($"Type '{value?.GetType()}' is unexpected for string value.");

		public static void InvalidDateTimeFormat(ElementType elementType, Type type, long position)
			 => throw new SerializationException($"Element type '{elementType}' is not valid for serialized '{type}' value at position {position:#,0}.");

		public static void InvalidDateTimeOffsetArray(long position)
			 => throw new SerializationException($"Array length is not valid for serialized '{typeof(DateTimeOffset)}' value at position {position:#,0}. Valid length is 2.");

		public static void InvalidDateTimeOffsetArray(long length, long position)
			 => throw new SerializationException($"Array length {length} is not valid for serialized '{typeof(DateTimeOffset)}' value at position {position:#,0}. Valid length is 2.");

		public static void TooLongIso8601FormatString(long length, int maxLength, long position)
			 => throw new SerializationException($"String length {length} is too long as ISO 8601 extended string at position {position:#,0}. Valid length is less than or equal to {maxLength}.");

		public static void TooLongIso8601FormatString(long length, int maxLength, Encoding encoding, long position)
			 => throw new SerializationException($"String length {length} is too long as ISO 8601 extended string which is encoded by '{encoding.EncodingName}' at position {position:#,0}. Valid length is less than or equal to {maxLength}.");

		public static void InvalidIso8601FormatString(Span<byte> buffer, long position)
			 => throw new SerializationException($"String '{StringEscape.Stringify(new ReadOnlySequence<byte>(buffer.ToArray()))}' is not valid as ISO 8601 extended string at position {position:#,0}.");

		public static void InvalidIso8601FormatString(Span<byte> buffer, long position, Exception innerException)
			 => throw new SerializationException($"String '{StringEscape.Stringify(new ReadOnlySequence<byte>(buffer.ToArray()))}' is not valid as ISO 8601 extended string at position {position:#,0}.", innerException);

		public static void InvalidIso8601FormatString(string value, long position)
			 => throw new SerializationException($"String '{StringEscape.ForDisplay(value)}' is not valid as ISO 8601 extended string at position {position:#,0}.");

		public static void CannotSerializeNonPublicTypeUnlessPrivledgedAccessEnabled(Type targetType)
			=> throw new SerializationException($"Cannot serialize type '{targetType}' because it is not public to generating serializer.");

		public static void CannotDetectCorrepondentMemberNameUniquely(ParameterInfo parameterInfo, IEnumerable<string> members)
			=> throw new AmbiguousMatchException(
				$"There are multiple candidates for correspondent member for parameter '{parameterInfo}' of constructor. [{String.Join(", ", members)}]"
			);

		public static void NoConstructorForCollection(Type targetType)
			=> throw new SerializationException($"Cannot deserialize collection type '{targetType}' because it does not have both of public default constructor and public consctructor with Int32 type 'capacity' parameter.");

		public static void UndeserializableCollection(Type targetType)
			=> throw new NotSupportedException($"A generated serializer for '{targetType}' does not support deserialization because the type does not expose Add method.");

		public static void CannotSerializeTypeWhichDeclaresReadOnlySignificantNonCollectionMemberWhenAsymmetricSerializerIsNotAllowed(Type targetType, MemberInfo member)
			=> throw new SerializationException($"Cannot generate serializer for type '{targetType}' because read-only and non-collection member '{member}' is not ignored, and asymmetric serializer generation is not allowed. The member should be ignored via MessagePackIgnoreAttribute, be settable, or have correspond constructor parameter, or set SerializerGenerationOptionsBuilder.AllowsAsymmetricSerializer 'true'.");

		public static void ExtensionTypeKeyNotFound(string name)
			=> throw new KeyNotFoundException($"Extension type '{StringEscape.ForDisplay(name)}' is not found.");

		public static void DecodeFailure(Exception ex, long position)
			=> throw new SerializationException($"Failed to decode value at positon {position:#,0}.", ex);

		public static void EncodedTypeMustBeNonNilArray(long position)
			=> throw new SerializationException($"Type info must be non-nil array at position {position:#,0}.");
		
		public static void UnknownTypeEmbedding(long position)
			=> throw new SerializationException($"Cannot deserialize with type-embedding based serializer. Root object must be 2 element array at position {position:#,0}.");

		public static void EncodedTypeDoesNotHaveValidArrayItems(int required, int actual, long position)
			=> throw new SerializationException($"Components count of the type info is not valid. {required} is required, but actual value is {actual} at position {position:#,0}.");
		
		public static void FailedToDecodeEncodingType(Exception ex, long position)
			=> throw new SerializationException($"Failed to decode 'encode type' component at position {position:#,0}.", ex);

		public static void UnknownEncodingType(byte encodeType, long position)
			=> throw new SerializationException($"Unknown encoded type '{encodeType}' at position {position:#,0}.");

		public static void FailedToDecodeCompressedTypeName(Exception ex, long position)
			=> throw new SerializationException($"Failed to decode 'type name' component at position {position:#,0}.", ex);

		public static void FailedToDecodeAssemblySimpleName(Exception ex, long position)
			=> throw new SerializationException($"Failed to decode 'assembly name' component. at position {position:#,0}", ex);

		public static void FailedToDecodeAssemblyVersion(Exception ex, long position)
			=> throw new SerializationException($"Failed to decode 'version' component at position {position:#,0}.", ex);

		public static void FailedToDecodeAssemblyVersion(int expectedLength, int actualLength, long position)
			=> throw new SerializationException($"Failed to decode 'version' component. Expected length is {expectedLength} but actual length is {actualLength} at position {position:#,0}.");

		public static void FailedToDecodeAssemblyCulture(Exception ex, long position)
			=> throw new SerializationException($"Failed to decode 'culture' component at position {position:#,0}.", ex);

		public static void FailedToDecodeAssemblyPublicKeyToken(Exception ex, long position)
			=> throw new SerializationException($"Failed to decode 'public key token' component at position {position:#,0}.", ex);

		public static void FailedToDecodeAssemblyPublicKeyToken(int expectedLength, int actualLength, long position)
			=> throw new SerializationException($"Failed to decode 'public key token' component. Expected length is {expectedLength} but actual length is {actualLength} at position {position:#,0}.");

		public static void ValueTypeCannotBePolymorphic(Type type)
			=> throw new SerializationException($"Value type '{type}' cannot be polymorphic.");

		public static void CannotGetActualTypeSerializer(Type actualType)
			=> throw new SerializationException($"Cannot get serializer for actual type {actualType} from serialzier provider.");

		public static void TypeIsNotDefinedAsKnownType(Type type)
			=> throw new SerializationException($"Type '{type}' in assembly '{type.Assembly}' is not defined as known types.");

		public static void UnknownTypeCode(string typeCode, long position)
			=> throw new SerializationException($"Unknown type code '{typeCode}' at position {position:#,0}.");
	}
}
