param([Switch]$Rebuild)

[string]$temp = '.\nugettmp'
[string]$builder = "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

[string]$sln = 'MsgPack.sln'
[string]$slnCompat = 'MsgPack.compats.sln'

[string]$nuspec = 'MsgPack.nuspec'

$buildOptions = @()
if( $Rebuild )
{
    $buildOptions += '/t:Rebuild'
}

$buildOptions += '/p:Configuration=Release'

&$builder $sln $buildOptions
&$builder $slnCompat $buildOptions

$winFile = New-Object IO.FileInfo( ".\bin\portable-windows8+wpa\MsgPack.dll" )
$xamarinFile = New-Object IO.FileInfo( ".\bin\monotouch\MsgPack.dll" )
if( ( $winFile.LastWriteTime - $xamarinFile.LastWriteTime ).Days -ne 0 )
{
	# It might that I forgot building in xamarin when winRT build and xamarin build last write time are differ more than 1day.
	#Write-Error "Last write times between WinRT binary and Xamarin library are very differ. Do you forget to place latest Xamarin build (on Mac) or latest WinRT build (on Windows) on ./bin ?"
	#return
}

.\.nuget\nuget.exe pack $nuspec

# Unity
Remove-Item .\MsgPack-CLI -Recurse
Copy-Item .\bin\ .\MsgPack-CLI\ -Recurse
[Reflection.Assembly]::LoadWithPartialName( "System.IO.Compression.FileSystem" ) | Out-Null
# 'latest' should be rewritten with semver manually.
Remove-Item .\MsgPack.Cli.latest.zip
[IO.Compression.ZipFile]::CreateFromDirectory( ".\MsgPack-CLI", ".\MsgPack.Cli.latest.zip" )
Remove-Item .\MsgPack-CLI -Recurse

Write-Host "Package creation finished. Ensure AssemblyInfo.cs is updated and .\SetFileVersions.ps1 was executed."