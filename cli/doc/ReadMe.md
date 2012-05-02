# MessagePack for Common Language Infrastructure

MessagePack for CLI (Common Language Infrastructure) is a binding of the MessagePack for CLI. That is, this is not only a binding for C#,but also is able to be used from Visual Basic, F#, PowerShell, Iron Python, Iron Ruby, and so on. 
Currently supported runtime is only Full CLR(v4.0 or above, including client profile), but drops for Silverlight(v4 or v5), Mono and Windows Phone Runtime(v7.1) is available. Note that there are no automated test suite for 'not supported' projects.

## Contents

* Usage:
    * [Serialization Quick start](./SerializationQuickStart.md) - Quick start for serialization.
    * [MessagePackObject](./MessagePackObject.md) - `MessagePackObject` supports _MessagePack dynamic typing nature_ in strongly typed CLI style.
    * [Serialization Attributes](./SerializationAttributes.md) - Describes custom attributes which control serialization for improve interoperability.
    * [Serialization Requirements](./SerializationRequirements.md) - Describes requirements of serialization for your own types.

* Advanced:
    * [Interoperability Considerations](./InteroperabilityConsiderations.md) - Desribes some considerations for iteroperability.
    * [Built-in Serializer Specifications](./BuiltInSerializerSpecifications.md) - Desribes some considerations for iteroperability.
    * [Manual Packings](./ManualPackings.md) - Describes manual packings for advanced scenario.
    * [Serializer Lifetimes](./SerializerLifetimes.md) - Describes serializer life times and how to share serializers.
    * [Custom Serialization](./CustomSerialization.md) - Describe how to create custom serializer to support own serialization logic.
    * [Implementation Notes](./ImplementationNotes.md) - Implementation notes for code reader.

## Features

* Dynamic MessagePack type (`MessagePackObject`)
* Fast
* Customizable serialization
* Easy to use for basic scenario
* Supports many basic types
* Supports partially trusted environments
* Drops for Mono, Silverlight, Windows Phone (it is not stable because I cannot test them...bug reports welcome)

## RPC

If you are instresting for MessagePack RPC for CLI, please see [document on the RPC repository](https://github.com/yfakariya/msgpack-rpc-cli/doc/ReadMe.md).

## Limitations

### Serialization

#### Cyclic References

MessagePack for CLI does **NOT** support cyclic references. This is differ from runtime serializer like `BinaryFormatter`. This is intended specification, because many bindings also do not support cyclic references, it is nonsense to support cyclic reference. When you want to serialize object in compact binary format supporting cyclic reference, built-in `BinaryFormatter` is reasonable choice.

#### Date/Time

_This is a limitation of the MessagePack spec._ There are no canonical specification to serialize date/time value, but the de facto is usage of the Unix epoc which is an epoc from January 1st, 1970 by milliseconds. MessagePack for CLI is compliant for this, so less significant digits and timezone related information of `DateTime`/`DateTimeOffset` will be lost. Note that built-in serealiers pack them in UTC time.

##### Leap Seconds

Many environments does not consider leap seconds. Therefore, in terms of interoperability, you SHOULD NOT expect the destination handles it, espicially using epoc time as a serealization format(it is default behavior). If you have to handle leap second, you can use ISO-XXX formatted date-time string to tell it, but remember the destination might not be able to interpret it.

#### String

_This is a limitation of the MessagePack spec._ There are no canonical specification for string, but de facto is packing string as raw type containing UTF-8 encoded Unicode character sequence without BOM.

#### Big Array/Map

By design, collections of CLI implementation can have their items up to Int32.MaxValue (about 2 billions), but the MessagePack specification says that Array32 or Map32 may contain up to UInt32.MaxValue (about 4 billions). Currently, MessagePack for CLI does not suppot such big collection. Note that many implementation, including Java, also do not support them.

### APIs

#### NUnit assertion

NUnit sometimes reports assertion error for comparing MessagePackObject and compatible type. This is because NUnit does not handle implicit type conversion on the asserion logic. You can avoid it with direct invokation to the MessagePackObject.Equals().

#### Windows Phone 7.1

Currently, typed serializer is not available in Windows Phone runtime because MessagePack for CLI uses TypeBuilder API which is not included in the runtime. This limitation will be removed in the future. You can use MessagePack via IPackable mechanism, however.

### Enviroment

#### The Runtime Version

There are unsuppored version of runtime, because of developping work around cost.

* __CLR:__ CLR v2.0 is not supported yet. It means that .NET Framework 2.0, 3.0, 3.5 is not supported yet. If you think it is critical, please contact me. Note that CLR v1.x should never supported.
* __Silverlight:__ V2 and v3 is not supported. <!--For timing reason, v4 is supported, but you cannot use *-Async API because the runtime does not support Task.-->*RPC build does not exist yet, but it will be come in few months*
* __Windows Phone:__ V7.0 is not supported.
* __Mono:__ Prior to Mono 2.10 is not supported, but may work if you can build against them.

## FAQ

* _Q_ Is there `DynamicObject` version of message pack object?
    * _A_ No. I think it is not very useful. But if you want to it, please tell me your opinion.

* _Q_ Are there 'template' like Java binding ?
    * _A_ No. CLI generics feature works fine. But if you want to it, please tell me your opinion.


## Translations

_T.B.D._
