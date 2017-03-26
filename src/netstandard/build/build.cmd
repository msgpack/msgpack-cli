echo Retore netstandard 1.1/1.3 project...
echo %0 %1 %2
dotnet restore %1src\MsgPack\MsgPack.csproj
if not %ERRORLEVEL%==0 (
  echo "Failed to restore netstandard1.1/1.3"
  exit 1
)

echo Build netstandard 1.1/1.3 project...
dotnet build %1src\MsgPack\MsgPack.csproj -c %2
if not %ERRORLEVEL%==0 (
  echo "Failed to build netstandard1.1/1.3"
  exit 1
)
