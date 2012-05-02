# Manual Packings

You can serialize/deserialize object without serializers. Instead, you will use `Packer` and `Unpacker`, which are low level API of serialization.
If you run on Silverlight/WindowsPhone and you does not refer *.Serialization* assembly, only possible option is this method.

There are four significant types.

* `Packer` - Implements serialization from primitives, `MessagePackObject`s, or `IPackable`s to MessagePack stream. See following description.
* `Unpacker` - Implements deserialization to primitives, `MessagePackObject`s, or `IUnpackable`s from MessagePack stream. See following description.
* `IPackable` - An interface for the type which has own serialization logic.
* `IUnpackable` - An interface for the type which has own deserialization logic.

There are some utility classes for low level serialization support, see API documentation for details:

* `Unpacking` - Provides utility unpacking static methods. See following description.
* `MessagePackConvert` - Implements some de facto conversions between primitives and "basic" types like `DateTime`.

## API Details

### Packer

A `Packer` class defines writer for MessagePack stream like a `XmlWriter`. Note that a `Packer` prefers most compact type. For example, if you pass the integer value `1` as `Int64` type, a packer will pack it as _Positive Fix Num_ value.

### Unpacker

A `Unpacker` class defines "pull-style" reader for MessagePack stream like a `XmlReader`. This object holds unpacked values as `MessagePackObject`. 
For arrays and maps, it unpacks their length first, then unpacks elements subsequently. There is a convinient API, `ReadSubtree`, which limits reading scope to the current collection.
Note that an unpacker does not distinguish the actual form for a value in the stream, that is, the value `1` is able to be unpacked even if it is packed as `Int16`, `UInt64`, etc., and client cannot know the actual packed value was. 

#### Feeding

There are no "Feeding" API in the Unpacker because the source `Stream` object should handle it such as `NetworkStream` does. 

### Unpacking

The `Unpacking` class defines static methods to directly unpack various type values from the source stream. This API is intended to usually be used in unit testing. 
