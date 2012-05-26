[string]$temp = '.\nugettmp'
[string]$builder
[string]$sln
if ( ![String]::IsNullOrEmpty( "$env:windir" ) )
{
    $builder = "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
    $sln = ".\MsgPack.sln"
}
else
{
    $builder = 'xbuild'
    $sln = "./MsgPack.mono.sln"
}

&$builder $sln /p:Configuration=Release
[string]$source =  [IO.Path]::GetFullPath( '.\bin' )
$excludes = @('*.pdb', '*.resources.dll', 'System.*', 'Microsoft.*', 'Mono.*')

# PS cannot handle exclusion in directory copying like Ant...
ls $source -Recurse -Exclude $excludes | copy -Destination { Join-Path ".\$temp\lib" $_.FullName.Substring( $source.Length ) }
# Remove empty directories to exclude extra Silverlight related resources...
ls ".\$temp\lib" -Recurse | where { ( $_.Attributes -band [IO.FileAttributes]::Directory ) -and ( ( [IO.DirectoryInfo]$_ ).GetFiles().Length -eq 0 ) } | Remove-Item

.\.nuget\nuget.exe pack MsgPack.nuspec -BasePath .\nugettmp
Remove-Item $temp -Force -Recurse
