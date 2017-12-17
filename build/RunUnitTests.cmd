dotnet test %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest/MsgPack.UnitTest.csproj
if not %errorlevel% == 0 exit /b 1
dotnet test %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.BclExtensions/MsgPack.UnitTest.BclExtensions.csproj
if not %errorlevel% == 0 exit /b 1
dotnet test %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.CodeDom/MsgPack.UnitTest.CodeDom.csproj
if not %errorlevel% == 0 exit /b 1
@rem WinRT related tests require developer license...
@rem vstest.console /logger:Appveyor /InIsolation %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.WinRT/AppPackages/MsgPack.UnitTest.WinRT_1.1.0.0_AnyCPU_Debug_Test/MsgPack.UnitTest.WinRT_1.1.0.0_AnyCPU_Debug.appx 
@rem vstest.console /logger:Appveyor /InIsolation %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.BclExtensions.WinRT/AppPackages/MsgPack.UnitTest.BclExtensions.WinRT_1.1.0.0_AnyCPU_Debug_Test/MsgPack.UnitTest.BclExtensions.WinRT_1.1.0.0_AnyCPU_Debug.appx
@rem vstest.console /logger:Appveyor /InIsolation %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.WinRT.WindowsPhone/AppPackages/MsgPack.UnitTest.WinRT.WindowsPhone_1.0.0.0_x86_Debug_Test/MsgPack.UnitTest.WinRT.WindowsPhone_1.0.0.0_x86_Debug.appx 
@rem UWP test with NUnit3 is not available in cli
@rem Xamarin tests are not available in cli