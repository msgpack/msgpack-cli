$assemblyInfoVersionRegex = New-Object Text.RegularExpressions.Regex("^\s*\[\s*assembly\s*:\s*AssemblyInformationalVersion\(\s*`"(?<Version>[0-9\.]+)`"\s*\)\s*\]\s*$", ( [Text.RegularExpressions.RegexOptions]::Compiled -bor [Text.RegularExpressions.RegexOptions]::ExplicitCapture -bor [Text.RegularExpressions.RegexOptions]::CultureInvariant -bor [Text.RegularExpressions.RegexOptions]::MultiLine ) );
$versionMatch = $assemblyInfoVersionRegex.Match([IO.File]::ReadAllText( ".\src\CommonAssemblyInfo.Pack.cs" ));
if ( !$versionMatch.Success )
{
	Write-Error "Cannot extract AssemblyInformationalVersion from CommonAssemblyInfo.Pack.cs."
	return
}

$version = New-Object Version( $versionMatch.Groups[ "Version" ].Value );

# Major = Informational-Major, Minor = Informational-Minor, 
# Build = Epoc days from 2010/1/1, Revision = Epoc minutes from 00:00:00
$today = [DateTime]::UtcNow;
$build = [int]( ( $today.Date - ( New-Object DateTime( 2010, 1, 1 ) ).ToUniversalTime() ).TotalDays );
$revision = [int]( ( $today - $today.Date ).TotalMinutes );
$newVersion = New-Object Version( $version.Major, $version.Minor, $build, $revision );

$newVersionString = "[assembly: AssemblyFileVersion( `"{0}`" )]`r`n"  -f $newVersion

$assemblyFileVersionRegex = New-Object Text.RegularExpressions.Regex( "\[\s*assembly\s*:\s*AssemblyFileVersion\([^)]+\)\s*\]\r\n", ( [Text.RegularExpressions.RegexOptions]::Compiled -bor [Text.RegularExpressions.RegexOptions]::ExplicitCapture -bor [Text.RegularExpressions.RegexOptions]::CultureInvariant -bor [Text.RegularExpressions.RegexOptions]::MultiLine ) );

foreach( $assemblyInfo in ( ls "src" -Recurse AssemblyInfo.cs ) )
{
	[IO.File]::WriteAllText( 
		$assemblyInfo.FullName,
		$assemblyFileVersionRegex.Replace( 
			[IO.File]::ReadAllText( $assemblyInfo.FullName ),
			$newVersionString 
		)
	);
}