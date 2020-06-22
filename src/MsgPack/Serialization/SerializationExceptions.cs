// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif // !UNITY || MSGPACK_UNITY_FULL
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Globalization;
#if !UNITY
using System.Reflection;
#endif // !UNITY
using System.Runtime.Serialization;

using MsgPack.Serialization.CollectionSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Defines common exception factory methods.
	/// </summary>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable(EditorBrowsableState.Never)]
#endif // !UNITY || MSGPACK_UNITY_FULL
	public static class SerializationExceptions
	{
#if !AOT
		internal static readonly MethodInfo ThrowValueTypeCannotBeNull3Method = typeof(SerializationExceptions).GetMethod(nameof(ThrowValueTypeCannotBeNull), new[] { typeof(string), typeof(Type), typeof(Type) });
#endif // !AOT

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot be <c>null</c> on deserialization.
		/// </summary>
		/// <param name="name">The name of the member.</param>
		/// <param name="memberType">The type of the member.</param>
		/// <param name="declaringType">The type that declares the member.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewValueTypeCannotBeNull(string name, Type memberType, Type declaringType)
		{
#if DEBUG
			Contract.Requires(!String.IsNullOrEmpty(name));
			Contract.Requires(memberType != null);
			Contract.Requires(declaringType != null);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "Member '{0}' of type '{1}' cannot be null because it is value type('{2}').", name, declaringType, memberType));
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Throws an exception to notify that value type cannot be <c>null</c> on deserialization.
		/// </summary>
		/// <param name="name">The name of the member.</param>
		/// <param name="memberType">The type of the member.</param>
		/// <param name="declaringType">The type that declares the member.</param>
		/// <exception cref="Exception">Always thrown.</exception>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static void ThrowValueTypeCannotBeNull(string name, Type memberType, Type declaringType)
		{
			throw NewValueTypeCannotBeNull(name, memberType, declaringType);
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot be <c>null</c> on deserialization.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewValueTypeCannotBeNull(Type type)
		{
#if DEBUG
			Contract.Requires(type != null);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "Cannot be null '{0}' type value.", type));
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot serialize.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewTypeCannotSerialize(Type type)
		{
#if DEBUG
			Contract.Requires(type != null);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "Cannot serialize '{0}' type.", type));
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot deserialize.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewTypeCannotDeserialize(Type type)
		{
#if DEBUG
			Contract.Requires(type != null);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "Cannot deserialize '{0}' type.", type));
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that value type cannot deserialize.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <param name="memberName">The name of deserializing member.</param>
		/// <param name="inner">The inner exception.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewTypeCannotDeserialize(Type type, string memberName, Exception inner)
		{
#if DEBUG
			Contract.Requires(type != null);
			Contract.Requires(!String.IsNullOrEmpty(memberName));
			Contract.Requires(inner != null);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "Cannot deserialize member '{1}' of type '{0}'.", type, memberName), inner);
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that item is not found on the unpacking stream.
		/// </summary>
		/// <param name="index">The index to be unpacking.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
#if DEBUG
		[Obsolete("Use ThrowMissingItem(int, Unpacker) instead.")]
#endif
		public static Exception NewMissingItem(int index) // For compatibility only.
		{
#if DEBUG
			Contract.Requires(index >= 0);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new InvalidMessagePackStreamException(String.Format(CultureInfo.CurrentCulture, "Items at index '{0}' is missing.", index));
		}

		/// <summary>
		/// 	<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		/// 	Throws a exception to notify that item is not found on the unpacking stream.
		/// </summary>
		/// <param name="index">The index to be unpacking.</param>
		/// <param name="unpacker">The unpacker for pretty message.</param>
		/// <exception cref="Exception">Always thrown.</exception>
		public static void ThrowMissingItem(int index, Unpacker unpacker)
		{
			ThrowMissingItem(index, null, unpacker);
		}

		/// <summary>
		/// 	<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		/// 	Throws a exception to notify that item is not found on the unpacking stream.
		/// </summary>
		/// <param name="index">The index to be unpacking.</param>
		/// <param name="name">The name of the item to be unpacking.</param>
		/// <param name="unpacker">The unpacker for pretty message.</param>
		/// <exception cref="Exception">Always thrown.</exception>
		public static void ThrowMissingItem(int index, string name, Unpacker unpacker)
		{
			long offsetOrPosition = -1;
			bool isRealPosition = false;
			if (unpacker != null)
			{
				isRealPosition = unpacker.GetPreviousPosition(out offsetOrPosition);
			}

			if (String.IsNullOrEmpty(name))
			{
				if (offsetOrPosition >= 0L)
				{
					if (isRealPosition)
					{
						throw new InvalidMessagePackStreamException(
							String.Format(
								CultureInfo.CurrentCulture,
								"Value for '{0}' at index {1} is missing, at position {2}",
								name,
								index,
								offsetOrPosition
							)
						);
					}
					else
					{
						throw new InvalidMessagePackStreamException(
							String.Format(
								CultureInfo.CurrentCulture,
								"Value for '{0}' at index {1} is missing, at offset {2}",
								name,
								index,
								offsetOrPosition
							)
						);
					}
				}
				else
				{
					throw new InvalidMessagePackStreamException(
						String.Format(CultureInfo.CurrentCulture, "Value for '{0}' at index {1} is missing.", name, index)
					);
				}
			}
			else
			{
				if (offsetOrPosition >= 0L)
				{
					if (isRealPosition)
					{
						throw new InvalidMessagePackStreamException(
							String.Format(
								CultureInfo.CurrentCulture,
								"Item at index {0} is missing, at position {1}",
								index,
								offsetOrPosition
							)
						);
					}
					else
					{
						throw new InvalidMessagePackStreamException(
							String.Format(
								CultureInfo.CurrentCulture,
								"Item at index {0} is missing, at offset {1}",
								index,
								offsetOrPosition
							)
						);
					}
				}
				else
				{
					throw new InvalidMessagePackStreamException(
						String.Format(CultureInfo.CurrentCulture, "Item at index '{0}' is missing.", index)
					);
				}
			}
		}

		internal static void ThrowMissingKey(int index, Unpacker unpacker)
		{
			long offsetOrPosition;
			var isRealPosition = unpacker.GetPreviousPosition(out offsetOrPosition);
			if (offsetOrPosition >= 0L)
			{
				if (isRealPosition)
				{
					throw new InvalidMessagePackStreamException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Key of map entry at index {0} is missing, at position {1}",
							index,
							offsetOrPosition
						)
					);
				}
				else
				{
					throw new InvalidMessagePackStreamException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Key of map entry at index {0} is missing, at offset {1}",
							index,
							offsetOrPosition
						)
					);
				}
			}
			else
			{
				throw new InvalidMessagePackStreamException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Key of map entry at index {0} is missing.",
						index
					)
				);
			}
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that target type is not serializable because it does not have public default constructor.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		internal static Exception NewTargetDoesNotHavePublicDefaultConstructor(Type type)
		{
#if DEBUG
			Contract.Requires(type != null);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "Type '{0}' does not have default (parameterless) public constructor.", type));
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that target type is not serializable because it does not have both of public default constructor and public constructor with an Int32 parameter.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		internal static Exception NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity(Type type)
		{
#if DEBUG
			Contract.Requires(type != null);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "Type '{0}' does not have both of default (parameterless) public constructor and  public constructor with an Int32 parameter.", type));
		}

		internal static void ThrowTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity(Type type)
		{
			throw NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity(type);
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that required field is not found on the unpacking stream.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewMissingProperty(string name)
		{
#if DEBUG
			Contract.Requires(!String.IsNullOrEmpty(name));
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "Property '{0}' is missing.", name));
		}

		internal static void ThrowMissingProperty(string name)
		{
#pragma warning disable 612
			throw NewMissingProperty(name);
#pragma warning restore 612
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that unpacking stream ends on unexpectedly position.
		/// </summary>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		[Obsolete("This method is no longer used internally. So this internal API will be removed in future.")]
		public static Exception NewUnexpectedEndOfStream()
		{
#if DEBUG
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException("Stream unexpectedly ends.");
		}

		internal static void ThrowUnexpectedEndOfStream(Unpacker unpacker)
		{
			long offsetOrPosition;
			var isRealPosition = unpacker.GetPreviousPosition(out offsetOrPosition);
			if (offsetOrPosition >= 0L)
			{
				if (isRealPosition)
				{
					throw new InvalidMessagePackStreamException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Stream unexpectedly ends at position {0}",
							offsetOrPosition
						)
					);
				}
				else
				{
					throw new InvalidMessagePackStreamException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Stream unexpectedly ends at offset {0}",
							offsetOrPosition
						)
					);
				}
			}
			else
			{
				throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
			}
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that target collection type does not declare appropriate Add(T) method.
		/// </summary>
		/// <param name="type">The target type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewMissingAddMethod(Type type)
		{
#if DEBUG
			Contract.Requires(type != null);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "Type '{0}' does not have appropriate Add method.", type));
		}

#if !AOT
		internal static readonly MethodInfo ThrowIsNotArrayHeaderMethod =
			typeof(SerializationExceptions).GetMethod(nameof(ThrowIsNotArrayHeader), new[] { typeof(Unpacker) });
#endif // !AOT

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that unpacker is not in the array header, that is the state is invalid.
		/// </summary>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		[Obsolete("This method is no longer used internally. So this internal API will be removed in future.")]
		public static Exception NewIsNotArrayHeader()
		{
			return new SerializationException("Unpacker is not in the array header. The stream may not be array.");
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Throws an exception to notify that unpacker is not in the array header, that is the state is invalid.
		/// </summary>
		/// <param name="unpacker">The unpacker for pretty message.</param>
		/// <exception cref="Exception">Always thrown.</exception>
		public static void ThrowIsNotArrayHeader(Unpacker unpacker)
		{
			long offsetOrPosition;
			if (unpacker != null)
			{
				if (unpacker.GetPreviousPosition(out offsetOrPosition))
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Unpacker is not in the array header at position {0}. The stream may not be array.",
							offsetOrPosition
						)
					);
				}
				else
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Unpacker is not in the array header at offset {0}. The stream may not be array.",
							offsetOrPosition
						)
					);
				}
			}
			else
			{
				throw new SerializationException(
					"Unpacker is not in the array header. The stream may not be array."
				);
			}
		}

#if !AOT
		internal static readonly MethodInfo ThrowIsNotMapHeaderMethod =
			typeof(SerializationExceptions).GetMethod(nameof(ThrowIsNotMapHeader), new[] { typeof(Unpacker) });
#endif // !AOT

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that unpacker is not in the array header, that is the state is invalid.
		/// </summary>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
#if DEBUG
		[Obsolete]
#endif // DEBUG
		public static Exception NewIsNotMapHeader()
		{
#if DEBUG
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException("Unpacker is not in the map header. The stream may not be map.");
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Throws an exception to notify that unpacker is not in the map header, that is the state is invalid.
		/// </summary>
		/// <param name="unpacker">The unpacker for pretty message.</param>
		/// <exception cref="Exception">Always thrown.</exception>
		public static void ThrowIsNotMapHeader(Unpacker unpacker)
		{
			long offsetOrPosition;
			if (unpacker != null)
			{
				if (unpacker.GetPreviousPosition(out offsetOrPosition))
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Unpacker is not in the map header at position {0}. The stream may not be map.",
							offsetOrPosition
						)
					);
				}
				else
				{
					throw new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Unpacker is not in the map header at offset {0}. The stream may not be map.",
							offsetOrPosition
						)
					);
				}
			}
			else
			{
				throw new SerializationException(
					"Unpacker is not in the map header. The stream may not be map."
				);
			}
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that operation is not supported because <paramref name="type"/> cannot be instanciated.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewNotSupportedBecauseCannotInstanciateAbstractType(Type type)
		{
#if DEBUG
			Contract.Requires(type != null);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new NotSupportedException(String.Format(CultureInfo.CurrentCulture, "This operation is not supported because '{0}' cannot be instanciated.", type));
		}

#if !AOT
		/// <summary>
		///		<see cref="ThrowTupleCardinarityIsNotMatch(int,long,Unpacker)"/>
		/// </summary>
		internal static readonly MethodInfo ThrowTupleCardinarityIsNotMatchMethod =
			typeof(SerializationExceptions).GetMethod(nameof(ThrowTupleCardinarityIsNotMatch), new[] { typeof(int), typeof(long), typeof(Unpacker) });
#endif // !AOT

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the array length does not match to expected tuple cardinality.
		/// </summary>
		/// <param name="expectedTupleCardinality">The expected cardinality of the tuple.</param>
		/// <param name="actualArrayLength">The actual serialized array length.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
#if DEBUG
		[Obsolete]
#endif // DEBUG
		public static Exception NewTupleCardinarityIsNotMatch(int expectedTupleCardinality, int actualArrayLength)
		{
#if DEBUG
			Contract.Requires(expectedTupleCardinality > 0);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "The length of array ({0}) does not match to tuple cardinality ({1}).", actualArrayLength, expectedTupleCardinality));
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Throws an exception to notify that the array length does not match to expected tuple cardinality.
		/// </summary>
		/// <param name="expectedTupleCardinality">The expected cardinality of the tuple.</param>
		/// <param name="actualArrayLength">The actual serialized array length.</param>
		/// <param name="unpacker">The unpacker for pretty message.</param>
		/// <exception cref="Exception">Always thrown.</exception>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static void ThrowTupleCardinarityIsNotMatch(
			int expectedTupleCardinality,
			long actualArrayLength,
			Unpacker unpacker
		)
		{
#if DEBUG
			Contract.Requires(expectedTupleCardinality > 0);
#endif // DEBUG
			long offsetOrPosition = -1;
			bool isRealPosition = false;
			if (unpacker != null)
			{
				isRealPosition = unpacker.GetPreviousPosition(out offsetOrPosition);
			}

			if (offsetOrPosition >= 0L)
			{
				if (isRealPosition)
				{
					throw new InvalidMessagePackStreamException(
						String.Format(
							CultureInfo.CurrentCulture,
							"The length of array ({0}) does not match to tuple cardinality ({1}), at position {2}",
							actualArrayLength,
							expectedTupleCardinality,
							offsetOrPosition
						)
					);
				}
				else
				{
					throw new InvalidMessagePackStreamException(
						String.Format(
							CultureInfo.CurrentCulture,
							"The length of array ({0}) does not match to tuple cardinality ({1}), at offset {2}",
							actualArrayLength,
							expectedTupleCardinality,
							offsetOrPosition
						)
					);
				}
			}
			else
			{
				throw new InvalidMessagePackStreamException(
					String.Format(
						CultureInfo.CurrentCulture,
						"The length of array ({0}) does not match to tuple cardinality ({1}).",
						actualArrayLength,
						expectedTupleCardinality
					)
				);
			}
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the underlying stream is not correct semantically because failed to unpack items count of array/map.
		/// </summary>
		/// <param name="innerException">The inner exception for the debug. The value is implementation specific.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewIsIncorrectStream(Exception innerException)
		{
#if DEBUG
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException("Failed to unpack items count of the collection.", innerException);
		}

		internal static void ThrowIsIncorrectStream(Exception innerException)
		{
			throw NewIsIncorrectStream(innerException);
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the unpacking collection is too large to represents in the current runtime environment.
		/// </summary>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewIsTooLargeCollection()
		{
#if DEBUG
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new MessageNotSupportedException("The collection which has more than Int32.MaxValue items is not supported.");
		}

		internal static void ThrowIsTooLargeCollection()
		{
			throw NewIsTooLargeCollection();
		}

#if !AOT
		internal static readonly MethodInfo ThrowNullIsProhibitedMethod = typeof(SerializationExceptions).GetMethod(nameof(ThrowNullIsProhibited), new[] { typeof(string) });
#endif // !AOT

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the member cannot be <c>null</c> or the unpacking value cannot be nil because nil value is explicitly prohibitted.
		/// </summary>
		/// <param name="memberName">The name of the member.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewNullIsProhibited(string memberName)
		{
#if DEBUG
			Contract.Requires(!String.IsNullOrEmpty(memberName));
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "The member '{0}' cannot be nil.", memberName));
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Throws an exception to notify that the member cannot be <c>null</c> or the unpacking value cannot be nil because nil value is explicitly prohibitted.
		/// </summary>
		/// <param name="memberName">The name of the member.</param>
		/// <exception cref="Exception">Always thrown.</exception>
		public static void ThrowNullIsProhibited(string memberName)
		{
			throw NewNullIsProhibited(memberName);
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the unpacking value cannot be nil because the target member is read only and its type is collection.
		/// </summary>
		/// <param name="memberName">The name of the member.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewReadOnlyMemberItemsMustNotBeNull(string memberName)
		{
#if DEBUG
			Contract.Requires(!String.IsNullOrEmpty(memberName));
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "The member '{0}' cannot be nil because it is read only member.", memberName));
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the unpacking collection value is not a collection.
		/// </summary>
		/// <param name="memberName">The name of the member.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewStreamDoesNotContainCollectionForMember(string memberName)
		{
#if DEBUG
			Contract.Requires(!String.IsNullOrEmpty(memberName));
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "Cannot deserialize member '{0}' because the underlying stream does not contain collection.", memberName));
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that the unpacking array size is not expected length.
		/// </summary>
		/// <param name="expectedLength">Expected, required for deserialization array length.</param>
		/// <param name="actualLength">Actual array length.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewUnexpectedArrayLength(int expectedLength, int actualLength)
		{
#if DEBUG
			Contract.Requires(expectedLength >= 0);
			Contract.Requires(actualLength >= 0);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "The MessagePack stream is invalid. Expected array length is {0}, but actual is {1}.", expectedLength, actualLength));
		}

		/// <summary>
		///		<strong>This is intended to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
		///		Returns new exception to notify that it is failed to deserialize member.
		/// </summary>
		/// <param name="targetType">Deserializing type.</param>
		/// <param name="memberName">The name of the deserializing member.</param>
		/// <param name="inner">The exception which caused current error.</param>
		/// <returns><see cref="Exception"/> instance. It will not be <c>null</c>.</returns>
		public static Exception NewFailedToDeserializeMember(Type targetType, string memberName, Exception inner)
		{
#if DEBUG
			Contract.Requires(targetType != null);
			Contract.Requires(!String.IsNullOrEmpty(memberName));
			Contract.Requires(inner != null);
			Contract.Ensures(Contract.Result<Exception>() != null);
#endif // DEBUG

			return new SerializationException(String.Format(CultureInfo.CurrentCulture, "Cannot deserialize member '{0}' of type '{1}'.", memberName, targetType), inner);
		}

		/// <summary>
		///		Throws an exception to notify that it is failed to deserialize member.
		/// </summary>
		/// <param name="targetType">Deserializing type.</param>
		/// <param name="memberName">The name of the deserializing member.</param>
		/// <param name="inner">The exception which caused current error.</param>
		internal static void ThrowFailedToDeserializeMember(Type targetType, string memberName, Exception inner)
		{
			throw NewFailedToDeserializeMember(targetType, memberName, inner);
		}

#if !AOT
		/// <summary>
		///		<see cref="NewUnpackFromIsNotSupported(Type)"/>
		/// </summary>
		internal static readonly MethodInfo NewUnpackFromIsNotSupportedMethod =
			typeof(SerializationExceptions).GetMethod(nameof(NewUnpackFromIsNotSupported), new[] { typeof(Type) });
#endif // !AOT

		/// <summary>
		/// 	Returns a new exception which represents <c>UnpackFrom</c> is not supported in this asymmetric serializer.
		/// </summary>
		/// <param name="targetType">Deserializing type.</param>
		/// <returns>The exception. This value will not be <c>null</c>.</returns>
		public static Exception NewUnpackFromIsNotSupported(Type targetType)
		{
#if DEBUG
			Contract.Requires(targetType != null);
#endif // DEBUG
			return new NotSupportedException(String.Format(CultureInfo.CurrentCulture, "This operation is not supported for '{0}' because the serializer does not support UnpackFrom method.", targetType));
		}

#if !AOT
		/// <summary>
		///		<see cref="NewCreateInstanceIsNotSupported(Type)"/>
		/// </summary>
		internal static readonly MethodInfo NewCreateInstanceIsNotSupportedMethod =
			typeof(SerializationExceptions).GetMethod(nameof(NewCreateInstanceIsNotSupported), new[] { typeof(Type) });
#endif // !AOT

		/// <summary>
		/// 	Returns a new exception which represents <c>UnpackFrom</c> is not supported in this asymmetric serializer.
		/// </summary>
		/// <param name="targetType">Deserializing type.</param>
		/// <returns>The exception. This value will not be <c>null</c>.</returns>
		public static Exception NewCreateInstanceIsNotSupported(Type targetType)
		{
#if DEBUG
			Contract.Requires(targetType != null);
#endif // DEBUG
			return new NotSupportedException(String.Format(CultureInfo.CurrentCulture, "This operation is not supported for '{0}' because the serializer does not support CreateInstance method.", targetType));
		}

		internal static Exception NewUnpackToIsNotSupported(Type type, Exception inner)
		{
#if DEBUG
			Contract.Requires(type != null);
#endif // DEBUG
			return new NotSupportedException(String.Format(CultureInfo.CurrentCulture, "This operation is not supported for '{0}' because it does not have accesible Add(T) method.", type), inner);
		}

		internal static Exception NewValueTypeCannotBePolymorphic(Type type)
		{
			return
				new SerializationException(
					String.Format(CultureInfo.CurrentCulture, "Value type '{0}' cannot be polymorphic.", type)
				);
		}

		internal static Exception NewUnknownTypeEmbedding()
		{
			return new SerializationException("Cannot deserialize with type-embedding based serializer. Root object must be 3 element array.");
		}

		internal static Exception NewIncompatibleCollectionSerializer(Type targetType, Type incompatibleType, Type exampleClass)
		{
			return
				new SerializationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Cannot serialize type '{0}' because registered or generated serializer '{1}' does not implement '{2}', which is implemented by '{3}', for example.",
						targetType.GetFullName(),
						incompatibleType.GetFullName(),
						typeof(ICollectionInstanceFactory),
						exampleClass.GetFullName()
					)
				);
		}

		internal static void ThrowArgumentNullException(string parameterName)
		{
			throw new ArgumentNullException(parameterName);
		}

		internal static void ThrowArgumentNullException(string parameterName, string fieldName)
		{
			throw new ArgumentNullException(parameterName, String.Format(CultureInfo.CurrentCulture, "Field '{0}' of parameter '{1}' cannot be null.", fieldName, parameterName));
		}

		internal static void ThrowArgumentCannotBeNegativeException(string parameterName)
		{
			throw new ArgumentOutOfRangeException(parameterName, "The value cannot be negative number.");
		}

		internal static void ThrowArgumentCannotBeNegativeException(string parameterName, string fieldName)
		{
			throw new ArgumentOutOfRangeException(parameterName, String.Format(CultureInfo.CurrentCulture, "Field '{0}' of parameter '{1}' cannot be negative number.", fieldName, parameterName));
		}

		internal static void ThrowArgumentException(string parameterName, string message)
		{
			throw new ArgumentException(message, parameterName);
		}

		internal static void ThrowSerializationException(string message)
		{
			throw new SerializationException(message);
		}

		internal static void ThrowSerializationException(string message, Exception innerException)
		{
			throw new SerializationException(message, innerException);
		}

#if UNITY && DEBUG
		public
#else
		internal
#endif
		static void ThrowInvalidArrayItemsCount(Unpacker unpacker, Type targetType, int requiredCount)
		{
			throw
				unpacker.IsCollectionHeader
					? new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Cannot deserialize type '{0}' because stream is not {1} elements array. Current type is {2} and its element count is {3}.",
							targetType,
							requiredCount,
							unpacker.IsArrayHeader ? "array" : "map",
							unpacker.LastReadData.AsInt64()
						)
					)
					: new SerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Cannot deserialize type '{0}' because stream is not {1} elements array. Current type is {2}.",
							targetType,
							requiredCount,
							unpacker.LastReadData.UnderlyingType
						)
					);

		}
	}
}
