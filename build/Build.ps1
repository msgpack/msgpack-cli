param([Switch]$Rebuild)

[string]$msbuild = "msbuild"

if ( $env:APPVEYOR -eq "True" )
{
	
	# AppVeyor should have right MSBuild and dotnet-cli...
	# Android SDK should be installed in init and ANDROID_HOME should be initialized before this script.
}
else
{
	[string]$VSMSBuildExtensionsPath = $null
	
	$msbuildCandidates = @(
		"${env:ProgramFiles(x86)}\Microsoft Visual Studio\2017\Community\MSBuild\15.0\bin\MSBuild.exe",
		"${env:ProgramFiles(x86)}\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\bin\MSBuild.exe",
		"${env:ProgramFiles(x86)}\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\bin\MSBuild.exe",
		"${env:ProgramFiles}\Microsoft Visual Studio\2017\Community\MSBuild\15.0\bin\MSBuild.exe",
		"${env:ProgramFiles}\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\bin\MSBuild.exe",
		"${env:ProgramFiles}\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\bin\MSBuild.exe"
	)
	foreach ( $msbuildCandidate in $msbuildCandidates )
	{
		if ( ( Test-Path $msbuildCandidate ) )
		{
			$VSMSBuildExtensionsPath = [IO.Path]::GetFullPath( [IO.Path]::GetDirectoryName( $msbuildCandidate ) + "\..\..\" )
			break;
		}
	}
	
	if ( $VSMSBuildExtensionsPath -eq $null )
	{
		Write-Error "Failed to locate MSBuild.exe which can build .NET Core and .NET 3.5. VS2017 is required."
		exit 1
	}

	./SetBuildEnv.ps1
}

[string]$buildConfig = 'Release'
if ( ![String]::IsNullOrWhitespace( $env:CONFIGURATION ) )
{
	$buildConfig = $env:CONFIGURATION
}

[string]$sln = '../MsgPack.sln'
[string]$slnCompat = '../MsgPack.compats.sln'
[string]$slnWindows = '../MsgPack.Windows.sln'
[string]$slnXamarin = '../MsgPack.Xamarin.sln'

$buildOptions = @( '/v:minimal' )
if( $Rebuild )
{
    $buildOptions += '/t:Rebuild'
}

$buildOptions += "/p:Configuration=${buildConfig}"
$restoreOptions = "/v:minimal"

Write-Host "Clean up directories..."

# Unity
if ( !( Test-Path "./MsgPack-CLI" ) )
{
	New-Item ./MsgPack-CLI -Type Directory | Out-Null
}
else
{
	Remove-Item ./MsgPack-CLI/* -Recurse -Force
}

if ( !( Test-Path "../dist" ) )
{
	New-Item ../dist -Type Directory | Out-Null
}
else
{
	Remove-Item ../dist/* -Recurse -Force
}

if ( ( Test-Path "../bin/Xamarin.iOS10" ) )
{
	Remove-Item ../bin/Xamarin.iOS10 -Recurse
}

if ( !( Test-Path "./MsgPack-CLI/mpu" ) )
{
	New-Item ./MsgPack-CLI/mpu -Type Directory | Out-Null
}

# build

Write-Host "Restore $sln packages..."

& $msbuild /t:restore $sln $restoreOptions
if ( $LastExitCode -ne 0 )
{
	Write-Error "Failed to restore $sln"
	exit $LastExitCode
}

Write-Host "Build $sln..."

& $msbuild $sln $buildOptions
if ( $LastExitCode -ne 0 )
{
	Write-Error "Failed to build $sln"
	exit $LastExitCode
}

Write-Host "Restore $slnCompat packages..."

& $msbuild /t:restore $slnCompat $restoreOptions
if ( $LastExitCode -ne 0 )
{
	Write-Error "Failed to restore $slnCompat"
	exit $LastExitCode
}

Write-Host "Build $slnCompat..."

& $msbuild $slnCompat $buildOptions
if ( $LastExitCode -ne 0 )
{
	Write-Error "Failed to build $slnCompat"
	exit $LastExitCode
}

Write-Host "Restore $slnWindows packages..."

if ( $env:APPVEYOR -eq "True" )
{
	# Use nuget for legacy environments.
	nuget restore $slnWindows -Verbosity quiet
}
else
{
	& $msbuild /t:restore $slnWindows $restoreOptions
}

if ( $LastExitCode -ne 0 )
{
	Write-Error "Failed to restore $slnWindows"
	exit $LastExitCode
}

Write-Host "Build $slnWindows..."

& $msbuild $slnWindows $buildOptions
if ( $LastExitCode -ne 0 )
{
	Write-Error "Failed to build $slnWindows"
	exit $LastExitCode
}

if ( $buildConfig -eq 'Release' )
{
	Write-Host "Build NuGet packages..."

	& $msbuild ../src/MsgPack/MsgPack.csproj /t:pack /v:minimal /p:Configuration=$buildConfig /p:IncludeSource=true /p:IncludeSymbols=true /p:NuspecProperties=version=$env:PackageVersion

	Move-Item ../bin/*.nupkg ../dist/
	Copy-Item ../bin/* ./MsgPack-CLI/ -Recurse -Exclude @("*.vshost.*")
	Copy-Item ../tools/mpu/bin/* ./MsgPack-CLI/mpu/ -Recurse -Exclude @("*.vshost.*")
	[Reflection.Assembly]::LoadWithPartialName( "System.IO.Compression.FileSystem" ) | Out-Null
	# 'latest' should be rewritten with semver manually.
	if ( ( Test-Path "../dist/MsgPack.Cli.${env:PackageVersion}.zip" ) )
	{
		Remove-Item ../dist/MsgPack.Cli.${env:PackageVersion}.zip
	}
	[IO.Compression.ZipFile]::CreateFromDirectory( ( Convert-Path './MsgPack-CLI' ), ( Convert-Path '../dist/' ) + "MsgPack.Cli.${env:PackageVersion}.zip" )
	Remove-Item ./MsgPack-CLI -Recurse -Force
}
