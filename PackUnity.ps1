# Build Unity package

[string]$unity = "${env:ProgramFiles}\Unity\Editor\Unity.exe"

if ( !( Test-Path $unity ) )
{
	$unity = "${env:ProgramFiles(X86)}\Unity\Editor\Unity.exe"
}

if ( !( Test-Path $unity ) )
{
	Write-Error "Unity.exe is not found."
	return;
}

[string]$mpu = ".\tools\mpu\bin\mpu.exe"

if ( !( Test-Path $mpu ) )
{
	& "${env:WinDir}\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" .\src\mpu\mpu.csproj /p:Configuration=Release
}

del .\src\MsgPack.Unity\Assets\MsgPack\* -Recurse -Force
del .\MsgPack.unitypackage
& $mpu -l -p .\src\MsgPack.Xamarin.iOS\MsgPack.Xamarin.iOS.csproj -o .\src\MsgPack.Unity\Assets\MsgPack\ -w
& $unity -quit -projectPath ([IO.Path]::GetFullPath(".\src\MsgPack.Unity")) -exportPackage Assets\MsgPack ([IO.Path]::GetFullPath(".\MsgPack.unitypackage"))
