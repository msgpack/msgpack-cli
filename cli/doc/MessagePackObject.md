# MessagePackObject

`MessagePackObject` is low level representation of message pack object. It is lightweight from the space efficiency perspective, and it has a lot of conversion operators. You can represent 'dynamic' type entry using it.

## Type Conversion

`MessagePackObject` allows you to dynamic casting. If you used boxing, you cannot cast from a boxed `Int32` value to an `UInt32` value because unboxing itself does not handle any type conversion. In contrast, if you cast an `Int32` value to a `MessagePackObject`, then you can cast it to an `UInt32` as long as the value is not negative.

### Conversion Operators

* From CLI primitive type to `MessagePackObject` is always safe, so conversion is done with implicit conversion. (Note that native int (`void*` in C#), pointers are not supported. And also, note that `IntPtr`, `UIntPtr`, `Decimal` are not CLI primitive.)
* From `MessagePackObject` is not always safe, so conversion is done with explicit conversion.
* `System.String` is same as CLI primitives. Note that MessagePack APIs uses UTF-8 encoding (without BOM) as default.
* From a collection of `MessagePackObject` (`IList<T>`), map (dictionary) of `MessagePackObject` (`MessagePackDictionary), or bytes array to single `MessagePackObject` is actually always safe, but there is an immutability related issue (see following "Immutability" section). So, it is done with explicit conversion. Note that constructor is better because it takes boolean parameter which hints collection immutability.

* There are constructors which take acceptable CLI objects. They correspond to implicit operators. (However, collection taking ones have extra immutable hints.)
* There are `AsXxx()` methods which correspond to explicit operators.
* There are `IsTypeOf()` methods which allow you to check type compatibility.

## String Representation

`MessagePackObject.ToString()` returns JSON style string for its content.
Note that when the object represents very large collections or binary, it will cause significant resource consumption (CPU and memory).

## Immutability

`MessagePackObject` is value type object, so it is immutable compliant to Framework Design Guidelines. Therefore, the constructor and conversion operator copy entire collection which is passed to as the parameter. As you can see, it causes performance problem for large collection.
`MessagePackObject` has constructors which take second boolean type parameter which indicates the first argument can be considered as immutable. If the value is `true`, the constructor just store passed collection without copying, so you can avoid extra collection copying.