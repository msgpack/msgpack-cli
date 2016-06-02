$env:PackageVersion = ( Get-Content .\Version.txt );
$env:AssemblyBaseVersion = $env:PackageVersion | foreach{ if( $_ -match "^\d+\.\d+" ){ $matches[0] } } 
