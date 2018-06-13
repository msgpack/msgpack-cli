nunit3-console %APPVEYOR_BUILD_FOLDER%/test/MsgPack.UnitTest.Net35/bin/Debug/net35/MsgPack.UnitTest.Net35.dll --framework:net-3.5 --result=test-result-net35.xml;format=AppVeyor
if not %errorlevel% == 0 exit /b 1
