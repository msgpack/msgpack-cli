// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Text;
using MsgPack.Internal;

namespace MsgPack
{
	internal static class Throw
	{
		public static void ObjectDisposed(string? name)
			=> throw new ObjectDisposedException(name);
		public static char? InvalidIso8601DecimalSeparator(char? value, [CallerArgumentExpression("value")] string paramName = null!)
			=> throw new ArgumentOutOfRangeException(paramName, $"Value '{StringEscape.ForDisplay(value.GetValueOrDefault().ToString())}' is not valid ISO 8601 decimal separator.");

		public static void CannotBeNilAs<T>()
			=> throw new InvalidOperationException($"Nil value cannot be converted to '{typeof(T)}' value.");

		public static void InvalidTypeAs<T>(MessagePackObject obj)
			=> throw new InvalidOperationException($"'{obj}' cannot be converted to '{typeof(T)}' value.");

		public static void ExtensionsIsNotSupported()
			=> throw new NotSupportedException($"Extension type is not supported in this encoder.");

		public static void StreamMustBeAbleToRead(string paramName)
			=> throw new ArgumentException($"The stream must be able to read.", paramName);

		public static void TooSmallBuffer(string paramName, int minimumInclusive)
			=> throw new ArgumentException($"The size of buffer must be greater than or equal to {minimumInclusive:#,0}.", paramName);

		public static void TooLargeLength(int length, int requestHint)
			=> throw new ArgumentException($"Requested buffer size {((long)length + requestHint):#,0} is too large. It must be less than or equal to {OptionsDefaults.MaxSingleByteCollectionLength:#,0} bytes.");

		public static void EmptyObject(Type type)
			=> throw new InvalidOperationException($"Cannot use empty '{type}' object.");

		public static void DepthExeeded(long position, int maxDepth)
			=> throw new LimitExceededException(position, $"The depth of collection exceeds max depth {maxDepth:#,0}.");

		public static void StringLengthExceeded(long position, long length, int maxLength)
			=> throw new LimitExceededException(position, $"The byte length of encoded string ({length:#,0}) exceeds max length ({maxLength:#,0}).");

		public static void BinaryLengthExceeded(long position, long length, int maxLength)
			=> throw new LimitExceededException(position, $"The byte length of encoded binary ({length:#,0}) exceeds max length ({maxLength:#,0}).");

		public static void DepthUnderflow()
			=> throw new InvalidOperationException("CurrentDepth is 0.");

		public static void TooLargeByteLength(long size, string encodingName)
			=> throw new InvalidOperationException($"Input ReadOnlySequence is too large. It will be encoded to {size:#,0} bytes with '{encodingName}' encoding, but it must be less than or equal to {UInt32.MaxValue:#,0} bytes.");

		public static void TooLargeByteLength(Exception innerException, string encodingName)
			=> throw new InvalidOperationException($"Input ReadOnlySequence is too large. It will be encoded to larger than {Int32.MaxValue:#,0} bytes with '{encodingName}' encoding, but it must be less than or equal to {UInt32.MaxValue:#,0} bytes.", innerException);

		public static void TooLargePropertyKey(long position, int length, int maxPropertyKeyLength)
			=> throw new LimitExceededException(position, $"Property key is too large. The size {length:#,0} is larger than configured limit {maxPropertyKeyLength:#,0}.");

		public static void InsufficientInput(long position, Type targetType, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException(position, $"It is required more bytes in input ReadOnlySequence to decode {targetType} value.");
			}
			else
			{
				throw new InsufficientInputException(position, $"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode {targetType} value.");
			}
		}

		public static void InsufficientInputForAnyItem(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException(position, $"It is required more bytes in input ReadOnlySequence to decode any value.");
			}
			else
			{
				throw new InsufficientInputException(position, $"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode any value.");
			}
		}

		public static void InsufficientInputForNull(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException(position, $"It is required more bytes in input ReadOnlySequence to decode null.");
			}
			else
			{
				throw new InsufficientInputException(position, $"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode null.");
			}
		}


		public static void InsufficientInputForDecodeArrayOrMapHeader(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException(position, $"It is required more bytes in input ReadOnlySequence to decode array or map header.");
			}
			else
			{
				throw new InsufficientInputException(position, $"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode array or map header.");
			}
		}

		public static void InsufficientInputForDecodeArrayHeader(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException(position, $"It is required more bytes in input ReadOnlySequence to decode array header.");
			}
			else
			{
				throw new InsufficientInputException(position, $"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode array header.");
			}
		}

		public static void InsufficientInputForDecodeMapHeader(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException(position, $"It is required more bytes in input ReadOnlySequence to decode map header.");
			}
			else
			{
				throw new InsufficientInputException(position, $"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode map header.");
			}
		}

		public static void InsufficientInputForString(long position, Type type, Encoding? encoding, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException(position, $"It is required more bytes in input ReadOnlySequence to decode string as {type} with '{encoding}' encoding.");
			}
			else
			{
				throw new InsufficientInputException(position, $"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode string as {type} with '{encoding}' encoding.");
			}
		}

		public static void InsufficientInputForRawString(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException(position, $"It is required more bytes in input ReadOnlySequence to fetch raw string.");
			}
			else
			{
				throw new InsufficientInputException(position, $"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to fetch raw string.");
			}
		}

		public static void InsufficientInputForSkip(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException(position, $"It is required more bytes in input ReadOnlySequence to skip current subtree.");
			}
			else
			{
				throw new InsufficientInputException(position, $"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to skip current subtree.");
			}
		}

		public static void InsufficientInputForDetectCollectionEnds(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException(position, $"It is required more bytes in input ReadOnlySequence to detect whether current collection is end.");
			}
			else
			{
				throw new InsufficientInputException(position, $"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to detect whether current collection is end.");
			}
		}

		public static void InsufficientInputForDrainCollectionItems(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException(position, $"It is required more bytes in input ReadOnlySequence to drain current collection items.");
			}
			else
			{
				throw new InsufficientInputException(position, $"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to current collection items.");
			}
		}

		public static void InvalidTimestampLength(long length, string paramName)
			=> throw new ArgumentException($"The value's length {length} bytes is not valid. It should be 4, 8, or 12 bytes.", paramName);

		public static void InvalidTimestampTypeCode(byte tag, string paramName)
			=> throw new ArgumentException($"Timestamp must have type code -1 (0xFF in byte), but actual value is {unchecked((sbyte)tag)}(0x{tag:x}).", paramName);

		public static void InvalidTimestampTypeCode(long tag, string paramName)
			=> throw new ArgumentException($"Timestamp must have type code -1 (0xFF in byte), but actual value is {tag}(0x{tag:x}).", paramName);
	}
}
