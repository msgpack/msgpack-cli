nunit3-console %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest/bin/Debug/MsgPack.UnitTest.dll --framework:net-4.5 --result=test-result-net45.xml;format=AppVeyor
nunit3-console %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.BclExtensions/bin/Debug/MsgPack.UnitTest.BclExtensions.dll --framework:net-4.5 --result=test-result-net45-bclext.xml;format=AppVeyor
nunit3-console %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.CodeDom/bin/Debug/MsgPack.UnitTest.CodeDom.dll --framework:net-4.5 --result=test-result-net45-codedom.xml;format=AppVeyor
nunit3-console %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.Net35/bin/Debug/MsgPack.UnitTest.Net35.dll --framework:net-3.5 --result=test-result-net35.xml;format=AppVeyor
nunit3-console %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.CodeDom.Net35/bin/Debug/MsgPack.UnitTest.CodeDom.Net35.dll --framework:net-3.5 --result=test-result-net35-codedom.xml;format=AppVeyor
nunit3-console %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.Unity.Il2Cpp.Full.Desktop/bin/Debug/MsgPack.UnitTest.dll --framework:net-3.5 --result=test-result-unity-il2cpp-desktop.xml;format=AppVeyor
vstest.console /logger:Appveyor %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.WinRT/bin/Debug/MsgPack.UnitTest.dll
vstest.console /logger:Appveyor %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.BclExtensions.WinRT/bin/Debug/MsgPack.UnitTest.BclExtensions.dll
vstest.console /logger:Appveyor %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.WinRT.WindowsPhone/bin/x86/Debug/MsgPack.UnitTest.dll
:: UWP test with NUnit3 is not available in cli
:: Xamarin tests are not available in cli