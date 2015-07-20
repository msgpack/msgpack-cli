# TODOs

There are TODOs to be implemented in the future.

* If you find an issue which should be added this list, comment in the issue.
* If you implement the item of the list, send PR. Please remember you run UnitTests.
* They are ordered "personal" priority based on their effect and cost.

## Should be implemented in next minor versions.

### Documentation and Samples

Issue #94.

* Add wiki and samples about polymorphism.
* Add architecture document.

### Restructuring

* ContextBased-serializer should be removed.
* .NET 4 build should be removed in 2016/02.

### Debuggability Improvements

Issue #84.

* Add stream-position in the exception when the stream's `CanSeek` returns `true`.
* Add trace-enabled serializer and trace-enabled `Unpacker`.

### MessagePackKnownSubTypeAttribute

* This attribute defines "default known subtypes" in the base type itself.
* It reduces scattered `[MessagePackKnownAttribute]` in places.

### ObjectUnpacker

Issue #90.

* Add `Unpacker.Create(MessagePackObject)`.

### Efficiency

* Make `AssemblyBuilderAccess` to `RunAndSave`.

## Next Major

### Add New Platform Support

Issue #72.

* Unity stripping mode
* IL2CPP of Unity
* UWP
* .NET Native

## Long-term

### Asymmmetric Serializer

#Issue 68.

* Supports asymmetric (serialize-only/deserialize-only) serializer.
* Add properties like `CanPack` and `CanUnpack`.

### Async API

Issue #67.

* Add async methods to the `Packer` and `Unpacker`, thus add `MessagePackSerializer<T>` and new interface `IAsyncMessagePackSerializer` and `IAsyncMessagePackSingleObjectSerializer`.

## Continuously

### Refactoring

* Core API inlining with T4 is not very effective, so refactoring for maintenancibility is reasonable now.
* Serializer generator should be refactored continuously.
