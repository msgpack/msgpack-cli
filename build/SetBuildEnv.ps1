# Set versions for AssemblyInfo.cs
$env:PackageVersion = ( Get-Content .\Version.txt );
$env:AssemblyBaseVersion = $env:PackageVersion | foreach{ if( $_ -match "^\d+\.\d+" ){ $matches[0] } } 
# Update Android SDK
Echo 'y' | & "$env:ANDROID_HOME/tools/android.bat" update sdk --no-ui --all --filter android-10,android-23,platform-tools,tools,build-tools-23.0.3
