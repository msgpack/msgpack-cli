// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Text;

namespace MsgPack.Internal
{
	internal static class Throw
	{
		public static void ArgumentNull(string paramName)
			=> throw new ArgumentNullException(paramName);

		public static void ArgumentOutOfRange(string paramName, string message)
			=> throw new ArgumentOutOfRangeException(paramName, message);

		public static void ExtensionsIsNotSupported()
			=> throw new NotSupportedException($"Extension type is not supported in this encoder.");

		public static void TooLargeCharLength(long size)
			=> throw new InvalidOperationException($"Input ReadOnlySequence is too large. It is {size:#,0} chars, but it must be lessor than {Int32.MaxValue:#,0} chars. Use EncodeLargeString instead.");

		public static void DepthExeeded(long position, int maxDepth)
			=> throw new LimitExceededException($"The depth of collection exceeds max depth {maxDepth:#,0} at {position:#,0}.");

		public static void StringLengthExceeded(long position, long length, int maxLength)
			=> throw new LimitExceededException($"The byte length of encoded string ({length:#,0}) exceeds max length ({maxLength:#,0}) at {position:#,0}.");

		public static void BinaryLengthExceeded(long position, long length, int maxLength)
			=> throw new LimitExceededException($"The byte length of encoded binary ({length:#,0}) exceeds max length ({maxLength:#,0}) at {position:#,0}.");

		public static void DepthUnderflow()
			=> throw new InvalidOperationException("CurrentDepth is 0.");

		public static void TooLargeByteLength(long size, string encodingName)
			=> throw new InvalidOperationException($"Input ReadOnlySequence is too large. It will be encoded to {size:#,0} bytes with '{encodingName}' encoding, but it must be less than or equal to {UInt32.MaxValue:#,0} bytes. Use EncodeLargeString instead.");

		public static void TooLargeByteLength(Exception innerException, string encodingName)
			=> throw new InvalidOperationException($"Input ReadOnlySequence is too large. It will be encoded to larger than {Int32.MaxValue:#,0} bytes with '{encodingName}' encoding, but it must be less than or equal to {UInt32.MaxValue:#,0} bytes. Use EncodeLargeString instead.", innerException);

		public static void TooLargeByteLengthForString(string encodingName)
			=> throw new InvalidOperationException($"Input ReadOnlySequence is too large. It will be decoded to larger than maximum System.String length with '{encodingName}' encoding.");

		internal static void TooLargePropertyKey(long position, int length, int maxPropertyKeyLength)
			=> throw new InvalidOperationException($"Property key is too large. The size {length:#,0} is larger than configured limit {maxPropertyKeyLength:#,0} at {position:#,0}.");

		public static void InsufficientInput(long position, Type targetType, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException($"It is required more bytes in input ReadOnlySequence to decode {targetType} value at {position:#,0}.");
			}
			else
			{
				throw new InsufficientInputException($"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode {targetType} value at {position:#,0}.");
			}
		}

		public static void InsufficientInputForNull(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException($"It is required more bytes in input ReadOnlySequence to decode null at {position:#,0}.");
			}
			else
			{
				throw new InsufficientInputException($"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode null at {position:#,0}.");
			}
		}


		public static void InsufficientInputForDecodeArrayOrMapHeader(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException($"It is required more bytes in input ReadOnlySequence to decode array or map header at {position:#,0}.");
			}
			else
			{
				throw new InsufficientInputException($"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode array or map header at {position:#,0}.");
			}
		}

		public static void InsufficientInputForDecodeArrayHeader(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException($"It is required more bytes in input ReadOnlySequence to decode array header at {position:#,0}.");
			}
			else
			{
				throw new InsufficientInputException($"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode array header at {position:#,0}.");
			}
		}

		public static void InsufficientInputForDecodeMapHeader(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException($"It is required more bytes in input ReadOnlySequence to decode map header at {position:#,0}.");
			}
			else
			{
				throw new InsufficientInputException($"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode map header at {position:#,0}.");
			}
		}

		public static void InsufficientInputForString(long position, Type type, Encoding? encoding, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException($"It is required more bytes in input ReadOnlySequence to decode string as {type} with '{encoding}' encoding at {position:#,0}.");
			}
			else
			{
				throw new InsufficientInputException($"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to decode string as {type} with '{encoding}' encoding at {position:#,0}.");
			}
		}

		public static void InsufficientInputForRawString(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException($"It is required more bytes in input ReadOnlySequence to fetch raw string at {position:#,0}.");
			}
			else
			{
				throw new InsufficientInputException($"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to fetch raw string at {position:#,0}.");
			}
		}

		public static void InsufficientInputForSkip(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException($"It is required more bytes in input ReadOnlySequence to skip current subtree at {position:#,0}.");
			}
			else
			{
				throw new InsufficientInputException($"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to skip current subtree at {position:#,0}.");
			}
		}

		public static void InsufficientInputForDetectCollectionEnds(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException($"It is required more bytes in input ReadOnlySequence to detect whether current collection is end at {position:#,0}.");
			}
			else
			{
				throw new InsufficientInputException($"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to detect whether current collection is end at {position:#,0}.");
			}
		}

		internal static void InsufficientInputForDrainCollectionItems(long position, int requestHint)
		{
			if (requestHint < 0)
			{
				throw new InsufficientInputException($"It is required more bytes in input ReadOnlySequence to drain current collection items at {position:#,0}.");
			}
			else
			{
				throw new InsufficientInputException($"It is required more {requestHint:#,0} bytes in input ReadOnlySequence to current collection items at {position:#,0}.");
			}
		}
	}
}
