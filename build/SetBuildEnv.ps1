# Set versions for AssemblyInfo.cs
$version = ( Get-Content .\Version.txt );
$env:AssemblyBaseVersion = $version | foreach{ if( $_ -match "^\d+\.\d+" ){ $matches[0] } } 
if ( $env:APPVEYOR_REPO_TAG -ne "True" )
{
	if ( ${env:APPVEYOR_BUILD_NUMBER} -eq $null )
	{
		$now = [DateTime]::UtcNow
		$daysSpan = $now - ( New-Object DateTime( $now.Year, 1, 1 ) )
		$env:PackageVersion = "${version}-{0:yy}{1:000}" -f @( $now, $daysSpan.Days )
	}
	else if ( ${env:APPVEYOR_BUILD_NUMBER} -match "^[a-zA-Z].+" )
	{
		$env:PackageVersion = "${version}-${env:APPVEYOR_BUILD_NUMBER}"
	}
	else
	{
		$env:PackageVersion = "${version}-final-${env:APPVEYOR_BUILD_NUMBER}"
	}
}
else
{
	$env:PackageVersion = $version
}
