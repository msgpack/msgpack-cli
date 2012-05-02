# Custom Serialization

MessagePack for CLI built-in serialization mechanism can support most cases, but there are edge case which it does not support. For example:
* The type defines own binary representation.
* The requirement for serialization/deserialization speed is critical (note that default implementation generates straitforward code -- it has good performance).
* You do not refer *.Serialization assembly in Silverlight/WindowsPhone project to reduce application size. In this case, you must use [manually packing/unpacking](./ManualPackings.md) because built-in serialization mechanism is contained in those assembly.

This article discuss first and second items above. See [Manual Packings](./ManualPackings.md) for third item.

There are two options to implement custom serialization -- `IPackable/IUnpackable` and Custom Serializer.

## IPackable

The easiest way to implement custom serialization is implementing `IPackable` and `IUnpackable` on your class. In this case, you will interatct with packers/unpackers via `MessagePackObject`.

Note that serialization mechanism prefer to use registered (custom) serializer even if the type implements `IPackable` and `IUnpackable` because the runtime generate dedicated serializer for the type, which delegates actual serialization process to `IPackable`/`IUnpackable` methods.

The type must have default public constructor even if it implements `IPackable` and `IUnpackable`.

## Custom Serializer

If you want to serialize existing types, want to share the serialization logic in a number of `MessagePackSerialier`, or the type does not have default public constructor, you can implement own custom `MessagePackSerializer`.
In this case, you must register its instance to the using serialization context.
To use custom serializer, you must do following:

1. Create dedicated `SerializationContext` instance.
2. Implement custom serializer. The serializer must derived from `MessagePackSerializer<T>`, and implements its abstract methods.
    * `PackToCore` method. It is similar to `IPackable.PackTo` method. Implement packing logic from the instance to the stream via `Packer` object.
    * `UnpackFromCore` method. It is similar to `IUnpackable.UnpackFrom` method. Implement unpacking logic which pulls data from the stream via `Unpacker` object and deserialize to newly created instance.
    * Optinal `UnpackToCore` method. If `T` is collection and it has public default constructor, this method implements item feeding style deserialization. Specifically, if the hosted type has `T` type member which is read only and initialized in its constructor, the serializer will use `UnpackTo` to deserialize elements for the read only collection member.
3. Create custom serializer using dedicated serialization context.
4. Register serializer instance to `SerialzationRepository` via `SerializationContext.Serializers` property.

If you use `MessagePackSerializer.Create` to get built-in serializer, you must pass dedicated serialization context so that the generated serializers use registered custom serializers. Also, you must pass dedicated serialization context to use custom serializers in MessagePack RPC APIs.
   
