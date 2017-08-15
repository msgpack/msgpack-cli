echo Retore netstandard 1.1/1.3 project...
echo %0 %1 %2
echo You must restore MsgPack.csproj manually...
echo Build netstandard 1.1/1.3 project...
dotnet build %1src\MsgPack\MsgPack.csproj -f netstandard1.1 -c %2 -o %1src\netstandard\bin\Debug\netstandard1.1 /p:SkipXamarin=true /p:DebugType=full
if not %ERRORLEVEL%==0 (
  echo "Failed to build netstandard1.1"
  exit 1
)
dotnet build %1src\MsgPack\MsgPack.csproj -f netstandard1.3 -c %2 -o %1src\netstandard\bin\Debug\netstandard1.3 /p:SkipXamarin=true /p:DebugType=full
if not %ERRORLEVEL%==0 (
  echo "Failed to build netstandard1.3"
  exit 1
)
