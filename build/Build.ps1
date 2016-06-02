param([Switch]$Rebuild)

[string]$temp = './nugettmp'

if ( $env:APPVEYOR -eq "True" )
{
	[string]$builder = "MSBuild.exe"
	[string]$winBuilder = "MSBuild.exe"
	
	# AppVeyor should have right MSBuild and dotnet-cli...
}
else
{
	./SetBuildEnv.ps1
	[string]$builder = "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
	[string]$winBuilder = "${env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MSBuild.exe"

	if ( !( Test-Path( "$winBuilder" ) ) )
	{
		$winBuilder = "${env:ProgramFiles}\MSBuild\14.0\Bin\MSBuild.exe"
	}
	if ( !( Test-Path( "$winBuilder" ) ) )
	{
		Write-Error "MSBuild v14 is required."
		exit 1
	}

	if ( !( Test-Path( "${env:ProgramFiles}\dotnet\dotnet.exe" ) ) )
	{
		Write-Error "DotNet CLI is required."
		exit 1
	}
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
[string]$projNetStandard11 = "../src/netstandard/1.1/MsgPack"
[string]$projNetStandard13 = "../src/netstandard/1.3/MsgPack"

$buildOptions = @( '/v:minimal' )
if( $Rebuild )
{
    $buildOptions += '/t:Rebuild'
}

$buildOptions += "/p:Configuration=${buildConfig}"

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
&$builder $sln $buildOptions
if ( $LastExitCode -ne 0 )
{
	exit $LastExitCode
}

&$builder $slnCompat $buildOptions
if ( $LastExitCode -ne 0 )
{
	exit $LastExitCode
}

&$winBuilder $slnWindows $buildOptions
if ( $LastExitCode -ne 0 )
{
	exit $LastExitCode
}

&$builder $slnXamarin $buildOptions
if ( $LastExitCode -ne 0 )
{
	exit $LastExitCode
}
Copy-Item ../bin/MonoTouch10 ../bin/Xamarin.iOS10 -Recurse

dotnet restore $projNetStandard11
if ( $LastExitCode -ne 0 )
{
	exit $LastExitCode
}

dotnet build $projNetStandard11 -o ../bin/netstandard1.1 -f netstandard11 -c $buildConfig
if ( $LastExitCode -ne 0 )
{
	exit $LastExitCode
}

dotnet restore $projNetStandard13
if ( $LastExitCode -ne 0 )
{
	exit $LastExitCode
}

dotnet build $projNetStandard13 -o ../bin/netstandard1.3 -f netstandard13 -c $buildConfig
if ( $LastExitCode -ne 0 )
{
	exit $LastExitCode
}

if ( $buildConfig -eq 'Release' )
{
	if ( $env:APPVEYPOR -eq "True" )
	{
		[string]$nuget = "nuget"
	}
	else
	{
		[string]$nuget = "../.nuget/nuget.exe"
	}
	
	[string]$zipVersion = $env:PackageVersion
	&$nuget pack ../MsgPack.nuspec -Symbols -Version $env:PackageVersion -OutputDirectory ../dist

	Copy-Item ../bin/ ./MsgPack-CLI/ -Recurse -Exclude @("*.vshost.*")
	Copy-Item ../tools/mpu/bin/ ./MsgPack-CLI/mpu/ -Recurse -Exclude @("*.vshost.*")
	[Reflection.Assembly]::LoadWithPartialName( "System.IO.Compression.FileSystem" ) | Out-Null
	# 'latest' should be rewritten with semver manually.
	if ( ( Test-Path "../dist/MsgPack.Cli.${zipVersion}.zip" ) )
	{
		Remove-Item ../dist/MsgPack.Cli.${zipVersion}.zip
	}
	[IO.Compression.ZipFile]::CreateFromDirectory( ( Convert-Path './MsgPack-CLI' ), ( Convert-Path '../dist/' ) + "MsgPack.Cli.${zipVersion}.zip" )
	Remove-Item ./MsgPack-CLI -Recurse -Force

	if ( $env:APPVEYOR -ne "True" )
	{
		Write-Host "Package creation finished. Ensure AssemblyInfo.cs is updated and ./SetFileVersions.ps1 was executed."
	}
}
