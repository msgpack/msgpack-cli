# Serialization Attributes

You can use custom attributes to control member serialization.

## Specifying Serializable Members

You have three options to specify a type is serializable with MessagePack serializers. The runtime trying followling order.

* If the target type has any members which are marked with `MessagePackMemberAttribute`, then only those members will be serialized in opt-in manner.
* If the target type is marked with `DataContractAttribute`, then members with `DataMemberAttribute` will only be serialized in opt-in manner.
* Else, members which are NOT marked with `NonSerializedAttribute` will be serialized in opt-out manner.

Note that using `MessagePackMemberAttribute` is prefered. Remainings exist just for interoperability for existent frameworks.

### MessagePackMemberAttribute

This attribute specifies following MessagePack dedicated metadata. 

#### ID

Specifies numeric identifier of the member. IDs should be continuous and shall be started with 0. Missing IDs are considered that those members are omitted in the runtime (CTS type) representation. That is, the nil will be emittes in serialization, the values at those IDs will be skipped in deserialization.

#### Nil Implication

There are three options for what the nil implies.

* `MemberDefault` means that the nil will be interpreted as the default value of the type of that member (that is, `0` for value types or `null` for reference types). This value corresponds to `nullable required` in the IDL. As you see, the type should be reference type or `Nullable<T>` for clarification.
* `Omitted` means that the nil will be interpred that the member is omitted, thus the value initialized via instance default contructor or field initializer will be preserved after deserialization. This option will not affect serialization. This value corresponds to `optional` in the IDL.
* `Prohibit`means that serialized value must not be nil even if the member type is reference type. This value corresponds to `required` in the IDL. This option will affect both of serialization and deserialization process. By the way, when the nil is emitting or deserializing, `SerializationException` will be thrown.

**Note:** Respecting to the IDL design, members are required by default. 

### DataContract Interoperability

As mentioned above, you can use `DataContractAttribute` to specify MessagePack serialization. It may be usuful when you expose entities as Web Services on the same time, for example.

### Without Any Attributes

If any members of the type are not marked with `MessagePackSerializerAttribute` and the type itself is not qualiged with `DataContractAttribute`, all read/write PUBLIC members (that is, fields which are not initonly and properties which have both of getter and etter) are serialized. This is useful for casual cases, but you should explicitly annotate members for many production scenarios.

####  NonSerialized

You can exclude members from serialization via `NonSerializedAttribute`.
