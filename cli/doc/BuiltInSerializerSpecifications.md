# Built-in Serializer Specifications

There are some built-in serializers in MessagePack for CLI to improve usability and interoperability.
Default serializers of MessagePack for CLI respect de facto standards for some basic type representation.

You can override default behavior by registering custom serializer to the `SerializationContext` explicitly.

## Categories

Built-in serializers can be categolized as following:
* Primitives
* Tuples
* Known Value Objects

### Primitives

They behave as the Adapter between the `MessagePackSerializer` API and the `Packer` API. Note that the primitives in this context means "MessagePack" primitive, not CLI primitive. They are namely `Boolean`, `Byte`, `Int16`, `Int32`, `Int64`, `SByte`, `UInt16`, `UInt32`, `UInt64`, `Single`, `Double`, and `String` in the BCL types. Note that `Char`, `Object`, and pointers are not considered primitive.

### Tuples

A tuple can be considered as the strongly typed hetero-genious array, so the serializer emits an array where its length is equal to the arity.
Note that it is absolutely safe and efficient due to `MessagePack` objects' strongly-typed nature.

### Known Value Objects

Some value like objects in the FCL can be serialized with built-in serializer. The serializers basically use natural binary representation instead of key/value pairs because they are often embedded in the other objects.

## Specifications

These are the specifcation of the built-in serializers.

### System.ArraySegment<T>

Be emitted as T[] which equals to the segment represented by the ArraySegment<T>.

### System.Byte[]

Be emitted as raw.

### System.Char

Be emitted as uint16, UTF-16 BE encoded code point.

### System.DateTime

Be emitted as Unix epoc in UTC, as uint64.

### System.DateTimeOffset

Be emitted as two length array, which the first element is a `DateTime` value, and second  is a int16 offset in minutes.

### System.Decimal

Be emitted as a string with format 'R'.

### System.Nullable<T>

If the value is null, then it is emitted as nil(0xC0). Otherwise, follows the specification of T.

### System.Uri

Be emitted as its string representation as raw (UTF-8 without BOM).

### System.Version

Be emitted as four length int32 array, the element is Major, Minor, Revision, Build Number, respectedly.

### System.Collections.BitArray

Be emitted as an int32 value.

### System.Collections.Generic.KeyValuePair<TKey, TValue>

Be emitted as two length array, the first element is the key property, and the second component is the value property.

### System.Numerics.BigInteger

Be emitted as a raw value.

### System.Numerics.Complex

Be emitted as two length double array, the first element is the real component, and the second component is the imaginary component.

### System.Runtime.InteropServices.ComTypes.FILETIME

Be emitted as an Unix epoc in UTC, as int64.

### System.Text.StringBuilder

Be emitted as a raw (UTF-8 without BOM).