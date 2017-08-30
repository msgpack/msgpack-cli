@setlocal
@rem src
@dotnet run -p .\tools\SyncProjects2\SyncProjects2\SyncProjects2.csproj
@if not ERRORLEVEL 0 exit /b %ERRORLEVEL%

@rem test
@dotnet run -p .\tools\SyncProjects2\SyncProjects2\SyncProjects2.csproj -d Sync.Test.json -s test
@exit /b %ERRORLEVEL%
