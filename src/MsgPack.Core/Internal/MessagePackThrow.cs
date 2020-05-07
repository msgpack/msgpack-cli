// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache 2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Runtime.CompilerServices;

namespace MsgPack.Internal
{
	internal static class MessagePackThrow
	{
		public static void IsNotType(byte header, long position, Type requestType)
			=> throw new MessageTypeException($"The type is {MessagePackCode.ToString(header)}(0x{header:X2}) but {requestType} is required at {position:#,0}.");

		public static void RealCannotBeInteger(byte header, long position, Type requestType)
			=> throw new MessageTypeException($"The type is {MessagePackCode.ToString(header)}(0x{header:X2}) is not compatible for {requestType} in current configuration at {position:#,0}.");

		public static void TypeIsNotArray(byte header, long position)
			=> throw new MessageTypeException($"The type is {MessagePackCode.ToString(header)}(0x{header:X2}) but array is required at {position:#,0}.");

		public static void TypeIsNotMap(byte header, long position)
			=> throw new MessageTypeException($"The type is {MessagePackCode.ToString(header)}(0x{header:X2}) but map is required at {position:#,0}.");

		public static void TypeIsNotArrayNorMap(byte header, long position)
			=> throw new MessageTypeException($"The type is {MessagePackCode.ToString(header)}(0x{header:X2}) but array or map is required at {position:#,0}.");

		public static void IsNotNumber(byte header, long position, Type requestType)
			=> throw new MessageTypeException($"The type is {MessagePackCode.ToString(header)}(0x{header:X2}) but it is not compatible for {requestType} at {position:#,0}.");

		public static void TooLargeByteLength(byte header, long position, long byteLength)
			=> throw new MessageTypeException($"The length of string {MessagePackCode.ToString(header)}(0x{header:X2}) {byteLength:#,0}(0x{byteLength:X8}) exceeds Int32.MaxValue (0x7FFFFFFF) at {position:#,0}.");

		public static void TooLargeArrayOrMapLength(byte header, long position, long byteLength)
			=> throw new MessageTypeException($"The length of array or map {MessagePackCode.ToString(header)}(0x{header:X2}) {byteLength:#,0}(0x{byteLength:X8}) exceeds Int32.MaxValue (0x7FFFFFFF) at {position:#,0}.");

		public static void OutOfRangeExtensionTypeCode(int typeCode, [CallerArgumentExpression("typeCode")] string? paramName = default)
			=> throw new ArgumentOutOfRangeException(paramName, $"A type code of MessagePack must be non negative 1byte integer (between 0 to 127 inclusive). '{typeCode}' is too large.");

		public static void IsNotUtf8String(byte header, long position)
			=> throw new MessageTypeException($"The type is {MessagePackCode.ToString(header)}(0x{header:X2}) but it is not compatible for UTF8String at {position:#,0}.");

		public static void IsNotExtension(byte header, long position)
			=> throw new MessageTypeException($"The type is {MessagePackCode.ToString(header)}(0x{header:X2}) but it is not extension at {position:#,0}.");
	}
}
