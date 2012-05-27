param([Switch]$Rebuild)

[string]$temp = '.\nugettmp'
[string]$builder = "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

[string]$sln = 'MsgPack.sln'

[string]$nuspec = 'MsgPack.nuspec'

$buildOptions = @()
if( $Rebuild )
{
    $buildOptions += '/t:Rebuild'
}

$buildOptions += '/p:Configuration=Release'

&$builder $sln $buildOptions

[string]$source =  [IO.Path]::GetFullPath( '.\bin' )
$excludes = @('*.pdb', '*.resources.dll', 'System.*', 'Microsoft.*', 'Mono.*')

# PS cannot handle exclusion in directory copying like Ant...
ls $source -Recurse -Exclude $excludes | copy -Destination { Join-Path ".\$temp\lib" $_.FullName.Substring( $source.Length ) }
# Remove empty directories to exclude extra Silverlight related resources...
ls ".\$temp\lib" -Recurse | where { ( $_.Attributes -band [IO.FileAttributes]::Directory ) -and ( ( [IO.DirectoryInfo]$_ ).GetFiles().Length -eq 0 ) } | Remove-Item

.\.nuget\nuget.exe pack $nuspec -BasePath .\nugettmp
Remove-Item $temp -Force -Recurse
