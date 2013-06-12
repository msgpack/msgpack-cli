param([Switch]$Rebuild)

[string]$temp = '.\nugettmp'
[string]$builder = "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

[string]$sln = 'MsgPack.sln'
[string]$slnCompat = 'MsgPack.compats.sln'
[string]$slnWP7 = 'MsgPack.wp7.sln'

[string]$nuspec = 'MsgPack.nuspec'

$buildOptions = @()
if( $Rebuild )
{
    $buildOptions += '/t:Rebuild'
}

$buildOptions += '/p:Configuration=Release'

&$builder $sln $buildOptions
&$builder $slnCompat $buildOptions
&$builder $slnWP7 $buildOptions

.\.nuget\nuget.exe pack $nuspec