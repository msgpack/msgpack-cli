# Serialization Quick start

MessagePack is interoperable binary encoding format. You can reduce the size of stream dramatically when you have a data of which contains many binary (for example, numeric) data.

## Usage

It's simple. Create, do, done.

There are only 2 steps to serialize types.

1. Invoke `MessagePackSerializer.Create<T>()` with type of the root object type to instantiate `MessagePackSerializer<T>` object.
2. Invoke `Pack()` or `Unpack()` of it.

A small sample is here:

_[C#]_
    var serializer = MessagePackSerializer.Create<Foo>();
    serializer.Pack(foo, stream);
    stream.Position = 0;
    var value = serializer.Unpack(stream);

_[Visual Basic]_
    Dim serializer = MessagePackSerializer.Create(Of Foo)()
    serializer.Pack(foo, stream)
    stream.Position = 0
    Dim value = serializer.Unpack(stream)

**Note:** Of course, you must add assembly reference to `MsgPack.dll` to your project.

**Note:** For Silverlight and WindowsPhone, there are 2 assemblies, and both of them are required to build above code. If you want to reduce assembly size, see [Manual Packings](./ManualPackings.md) -- it is the reason for two assemblies exist.