How to genearete sources in this directory:

1) Delete all  ../../bin/Debug/MsgPack/Serialization/GeneratedSerializers/*.cs
2) Run ../OreGeneratedSerializerGeneartor's unit test.
3) Add all from ../../bin/Debug/MsgPack/Serialization/GeneratedSerializers/*.cs
4) Replace "protected internal override" with "protected internal overrride" (this is necessary because of InternalVisibleTo)