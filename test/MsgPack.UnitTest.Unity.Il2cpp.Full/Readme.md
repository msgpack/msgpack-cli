MessagePack for CLI Unit Test for Unity IL2CPP
===

Overview
---
This directory contains unit test framework for Unity IL2CPP backend.

How to build
---

1. Go to `../MsgPack.UnitTest.Unity.Il2cpp.Full.Desktop` directory.
2. Run `MakeAssets.ps1` PowerShell script (it may work on PowerShell Core)
3. Ensure `../MsgPack.UnitTest.Unity.Il2cpp.Full.Desktop/Assets` directory and its subtree have been generated.
4. Open this folder with Unity.
5. Import `../MsgPack.UnitTest.Unity.Il2cpp.Full.Desktop/Assets/Dll` directory into `Assets` of Unity project with Unity Editor.
6. Import `../MsgPack.UnitTest.Unity.Il2cpp.Full.Desktop/Assets/UnitTests` directory into `Assets` of Unity project with Unity Editor.

How to run
---

See [Unity official document](https://docs.unity3d.com/Manual/testing-editortestsrunner.html) for details.

1. Select [[Window]] &gt; [[Unit Test Runner]]
2. Click [[PlayMode]] tab.
    * If you see [[Enable playmode tests]] button, click it and then restart the Unity editor.
3. Ensure one more unit tests are shown in the dialog.
4. Click [[Run all]] to run in the Editor.
5. Open [[Build Settings]] dialog, and select a platform you want to run tests.
    * Note that author only tests in iOS (AOT).
6. Click [[Run all in player ({YourSelectedPlatform})]] button to run tests in play-mode.
    * If you face an error that the editor cannot find scene file, do following:
        1. Ensure that auto generated scene (its name should be "InitTestScene{RandomNumber}").
        2. In [[Build Settings]] dialog, click [[Add Open Scenes]] and check *only* above auto generated scene.
        3. Click [[Build and Run]] (or [[Build]] and then run the player built).
