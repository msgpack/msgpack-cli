# Serializer Lifetimes

Actual serialier types are generated on-the-fly in runtime for the performance. 

## Lifetime

Generated serializers are implemented as immutable, so all instances of them are generated only once in the specific `SerializationContext`. 
And basically the instance will live until the context is collected by GC.

Built-in custom serializers (like DateTimeSerializer) are loaded in the first time (jitting time).

## Sharing Serializers

On-the-fly generated serializers have great runtime performance and flexibility responding to any types due to the specialized code, but type generation is heavy process. Therefore, you should reuse existent types to avoid extra initialization. You can achieve it with `SerializationContext` class. The `SerializationContext` internally holds a repository which stores key-value pair for the serializer instance, which the key is the `Type` of the serealizing target type.

## Type Metadata

Each `SerializationContext`s are isolated, so one does not know the serializer type is already generated on others.
So it is recommended to share `SerializationContext` as possible to reduce type generation time and loaeder heap size.

## Discard Generated Serializers

You can discard generated serializers to delete all root references to the hosting serialization context.
Although you can discard generated type metadata (and ILs) by unloading AppDomain, as you know, it is not very useful except you implement own application server which takes advantage of worker AppDomain. If you will treat many types and want to free loader heap, you can set `SerializationContext.SerializationMethodGeneratorOption` property to `CanCollect`. For sacrificing a bit of performance, the generated code will be collected by GC if the all instances are freed.
