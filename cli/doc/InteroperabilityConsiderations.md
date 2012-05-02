# Interoperability Considerations

There are some considerations to ensure interoperability.

## Serialization Method

When you think both of interoperability and versioning, you have to consider that which is better to use array or map to represent object. The MessagePack de facto standard is array (as RPC arguments, Java implementations, etc), so you should use array for serialization (it is default option). You should also specify field order, however, with `DataMemberAttribute` or `MessagePackMemberAttribute` (the latter is preferred). This is because there are no specifications for ordering of enumerated fields/properties, so it will depend runtime behavior without explicit indication, then portability as well as forward comatibility might be lost. In addition, you cannot insert field(s) between existent ones because it breaks backward compatibility of the data. You should consider inheritance for data type because base type field/property addition may break compatibility because new member must have the order which have been owned by the derived type member.

In contrust, When you used map for serialization method, ordering is not a matter, and it is easier to ensure compatibility because you only watch member names among type hierarchy never conflict. There are some drawbacks, however. First, serialization/deserialization is slower than array due to string key treatment. In addition, deserialization performance is not good because it have to lookup name table to deserialize members. Second, serialized data size will be improved for keys.

If you like to use map instead of array, you can specify `SerializationContext.SerializationMethod` to `SerializationMethod.Map`.

## Basic Type Serialization

You should know that binary representations of strings, dates, times, big integers/big decimals are different on each platforms.
Unfortunately, MessagePack prefer simplicity (it improves connetability because more developers easy to implement bindings), so there are no built in types for above.
However, there are de facto for these types, and default serializers of MessagePack for CLI respect them.
For more information for de facto standards and default serializer specifications, see [Built-in Serializer Specifications](./BuiltInSerializerSpecifications.md).