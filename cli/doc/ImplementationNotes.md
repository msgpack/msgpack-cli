# Implementation Notes

## Design Goals

* The top requirement for MessagePack for CLI is interoperability. Performance is second, compatibility/correctiveness are third.
* API usability is crucial as performance.
* Mainly supports C# because it is major, VB can invoke it seamlessly, and Iron* can use their dedicated binding.
  It is better to implement F# dedicated binding than MessagePack for CLI consider F# by halves.

## Unpacker

Currently, unpacker is state machine. There are some tweaks, but they are not very effective from performance perspective. Speed-up patches are welcome.

## On-the-fly Serializer Generation

At first, ad-hoc reflection is very slow. So, we should use runtime code generation to improve performance.
Caching serializers for the target type members is effective because avoding thread synchronization on the repository, so `FieldBuilder` is required. Therefore, basic strategy is using Reflection.Emit APIs, not Expression Trees nor Dynamic Methods.
There is Dynamic Method mode, but it is only for WindowPhone.

## Composite Type Support

To avoid infinite recusive type definition, put surrogate serializer on subsequent appearance. It delegates to "first" generated serializer at run time.

## Float to Int32 Bits

Using unsafe code for performance, but it is not very efficient.

## String

Using CRT funcation for performance.
