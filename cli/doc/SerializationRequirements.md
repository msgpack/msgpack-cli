# Serialization Requirements

If you want to serialize/deserialize objects to/from MessagePack stream, there are some requirements for the target type.

## Requirements

As a rule of thumb, how to determine whether the type is supported is simple: when the type can be serealized with `DataContractSerializer`, it should be sereialized by the MessagePack serealizer.

More strictly speaking, there are following requirements for target type.

* A type has default public constructor, it has the constructor which take one `Int32` parameter only if the type implements IList<T>, it is array, or it is CLI primitive value type.
* A type is concrete type and is not delegate type, that is, type is value type, non-abstract class, enum type. Abstract classes and interfaces cannot be serialized.
* All members (fields and/or properties) are public and are not read only. Note that there are exceptions for collection type member. See following section.
* All fields in the type follows above 2 rules.

Note that you can serialize/deserialize any types even if they are not met to above requirements with your own [custom serializer](./CustomSerialization.md).

## Serialization Rule Details

### Selecting Properties or Fields

T.B.D.

### Handling Collection

MessagePack for CLI treats collection type members in special manner. 
If the member type which implements `IEnumerable<T>` and it has the member which its signature is `TReturn Add(T)` or `void Add(T)`. It is considered as collection type.
If the member is collection type, the member has not to be writeable. Even if the member is initonly or does not have a setter, the serializer can deserialize collection items as long as following conition is true:
* The collection type is not fixed, typically is not an array.
* The collection instance is not read only. Namely, `ICollection.IsReadOnly` or `ICollection<T>.IsReadOnly` returns `false`.

### Handling Enums

Enums are serialized as string based on the name of the value. It is most interoperable.

### Handling MessagePackObject

It will be packed as is.

### Handling System.Object

It will be treated as boxed `MessagePackObject`. If it is not, the exception will be thrown.
