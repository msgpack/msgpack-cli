param([Switch]$Rebuild)

[string]$temp = '.\nugettmp'
[string]$builder = "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

[string]$sln = 'MsgPack.Xamarin.sln'

[string]$nuspec = 'MsgPack.nuspec'

$buildOptions = @()
if( $Rebuild )
{
    $buildOptions += '/t:Rebuild'
}

$buildOptions += '/p:Configuration=Release'
# cleanup
if ( [IO.Directory]::Exists( ".\bin\Xamarin.iOS10" ) )
{
	Remove-Item .\bin\Xamarin.iOS10 -Recurse
}
# build
&$builder $sln $buildOptions
Copy-Item .\bin\MonoTouch10 .\bin\Xamarin.iOS10 -Recurse