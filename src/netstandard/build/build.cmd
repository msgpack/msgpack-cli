echo Retore netstandard 1.1 project...
dotnet restore %2..\1.1\MsgPack\project.json
if not %ERRORLEVEL%==0 (
  echo "Failed to restore netstandard1.1"
  exit 1
)

echo Build netstandard 1.1 project...
dotnet build %2..\1.1\MsgPack\project.json -c %3
if not %ERRORLEVEL%==0 (
  echo "Failed to build netstandard1.1"
  exit 1
)

echo Retore netstandard 1.3 project...
dotnet restore %2..\1.3\MsgPack\project.json
if not %ERRORLEVEL%==0 (
  echo "Failed to restore netstandard1.3"
  exit 1
)

echo Build netstandard 1.1 project...
dotnet build %2..\1.3\MsgPack\project.json -c %3
if not %ERRORLEVEL%==0 (
  echo "Failed to build netstandard1.3"
  exit 1
)

if "Release"=="%3" (
  echo Copy release binaries...
  robocopy %2..\1.1\bin\Release\ %1bin\ /S
  robocopy %2..\1.3\bin\Release\ %1bin\ /S
)