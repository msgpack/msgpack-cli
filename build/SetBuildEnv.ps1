# Set versions for AssemblyInfo.cs
$version = ( Get-Content .\Version.txt );
$env:AssemblyBaseVersion = $version | foreach{ if( $_ -match "^\d+\.\d+" ){ $matches[0] } }
if ( $env:APPVEYOR_REPO_TAG -ne "True" )
{
	if ( ${env:APPVEYOR_BUILD_NUMBER} -eq $null )
	{
		$now = [DateTime]::UtcNow
		$daysSpan = $now - ( New-Object DateTime( $now.Year, 1, 1 ) )
		$env:PackageVersion = "${version}-{0:yy}{1:000}-{2:000}" -f @( $now, $daysSpan.Days, ( $now.TimeOfDay.TotalMinutes / 2 ) )
	}
	elseif ( ${version} -match "^[\d.]+$" )
	{
		$env:PackageVersion = "${version}-${env:APPVEYOR_BUILD_NUMBER}"
	}
	else
	{
		$env:PackageVersion = "${version}-${env:APPVEYOR_BUILD_NUMBER}"
	}
}
else
{
	$env:PackageVersion = $version
}

Write-Host "version:'${version}', AssemblyBaseVersion:'${env:AssemblyBaseVersion}', PackageVersion:'${env:PackageVersion}'"
