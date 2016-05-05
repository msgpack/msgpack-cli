MessagePack for CLI Unit Test for Unity IL2CPP
===

Overview
---
This directory contains unit test framework for Unity IL2CPP backend.
The idea of the framework borrowed from UniRx, and many codes are based on UniRx unit testing.

How to build
---

 1. Open MsgPack.UnitTest.Unity.Il2cpp.Full.sln first (specifically, you need to build MsgPack.UnitTest.Unity.Il2cpp.Full.Desktop project).
 2. Build all. Three dlls (MsgPack.dll, MsgPack.UnitTest.dll, nunitlite.dll) will be copied to Assets/Plugins/Dlls
 3. Open this directory in Unity Editor.