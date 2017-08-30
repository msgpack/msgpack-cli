dotnet test %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest/MsgPack.UnitTest.csproj
dotnet test %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.BclExtensions/MsgPack.UnitTest.BclExtensions.csproj
dotnet test %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.CodeDom/MsgPack.UnitTest.CodeDom.csproj
nunit3-console %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.Net35/bin/Debug/net35/MsgPack.UnitTest.Net35.dll --framework:net-3.5 --result=test-result-net35.xml;format=AppVeyor
nunit3-console %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.CodeDom.Net35/bin/Debug/net35/MsgPack.UnitTest.CodeDom.Net35.dll --framework:net-3.5 --result=test-result-net35-codedom.xml;format=AppVeyor
@rem WinRT related tests require developer license...
@rem vstest.console /logger:Appveyor /InIsolation %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.WinRT/AppPackages/MsgPack.UnitTest.WinRT_1.1.0.0_AnyCPU_Debug_Test/MsgPack.UnitTest.WinRT_1.1.0.0_AnyCPU_Debug.appx 
@rem vstest.console /logger:Appveyor /InIsolation %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.BclExtensions.WinRT/AppPackages/MsgPack.UnitTest.BclExtensions.WinRT_1.1.0.0_AnyCPU_Debug_Test/MsgPack.UnitTest.BclExtensions.WinRT_1.1.0.0_AnyCPU_Debug.appx
@rem vstest.console /logger:Appveyor /InIsolation %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.WinRT.WindowsPhone/AppPackages/MsgPack.UnitTest.WinRT.WindowsPhone_1.0.0.0_x86_Debug_Test/MsgPack.UnitTest.WinRT.WindowsPhone_1.0.0.0_x86_Debug.appx 
@rem UWP test with NUnit3 is not available in cli
@rem Xamarin tests are not available in cli