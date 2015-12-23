param([Switch]$Rebuild)

[string]$temp = '.\nugettmp'
[string]$builder = "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

[string]$sln = 'MsgPack.sln'
[string]$slnCompat = 'MsgPack.compats.sln'
[string]$slnWindows = 'MsgPack.Windows.sln'

[string]$nuspec = 'MsgPack.nuspec'

$buildOptions = @()
if( $Rebuild )
{
    $buildOptions += '/t:Rebuild'
}

$buildOptions += '/p:Configuration=Release'

# Unity
if ( ![IO.Directory]::Exists( ".\MsgPack-CLI" ) )
{
	New-Item .\MsgPack-CLI -Type Directory | Out-Null
}
else
{
	Remove-Item .\MsgPack-CLI\* -Recurse -Force
}

if ( ![IO.Directory]::Exists( ".\MsgPack-CLI\mpu" ) )
{
	New-Item .\MsgPack-CLI\mpu -Type Directory | Out-Null
}

# build
&$builder $sln $buildOptions
&$builder $slnCompat $buildOptions
&$builder $slnWindows $buildOptions

$winFile = New-Object IO.FileInfo( ".\bin\portable-net45+win+wpa81\MsgPack.dll" )
$xamarinFile = New-Object IO.FileInfo( ".\bin\MonoTouch10\MsgPack.dll" )
if( ( $winFile.LastWriteTime - $xamarinFile.LastWriteTime ).Days -ne 0 )
{
	# It might that I forgot building in xamarin when winRT build and xamarin build last write time are differ more than 1day.
	Write-Error "Last write times between WinRT binary and Xamarin library are very differ. Do you forget to place latest Xamarin build (on Mac) or latest WinRT build (on Windows) on ./bin ?"
	return
}

.\.nuget\nuget.exe pack $nuspec -Symbols

Copy-Item .\bin\* .\MsgPack-CLI\ -Recurse -Exclude @("*.vshost.*")
Copy-Item .\tools\mpu\bin\* .\MsgPack-CLI\mpu\ -Recurse -Exclude @("*.vshost.*")
[Reflection.Assembly]::LoadWithPartialName( "System.IO.Compression.FileSystem" ) | Out-Null
# 'latest' should be rewritten with semver manually.
if ( [IO.File]::Exists( ".\MsgPack.Cli.latest.zip" ) )
{
	Remove-Item .\MsgPack.Cli.latest.zip
}
[IO.Compression.ZipFile]::CreateFromDirectory( ".\MsgPack-CLI", ".\MsgPack.Cli.latest.zip" )
Remove-Item .\MsgPack-CLI -Recurse -Force

Write-Host "Package creation finished. Ensure AssemblyInfo.cs is updated and .\SetFileVersions.ps1 was executed."