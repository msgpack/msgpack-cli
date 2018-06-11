Remove-Item ./Assets -Force -Recurse

if (!(Test-Path ./bin/Debug/MsgPack.dll))
{
    Write-Error "Build MsgPack.UnitTest.Unity.Full.Desktop project with Debug configuration first."
    exit 1
}

[string[]]$additionalDefines = ("#define AOT", "#define NET35", "#define UNITY_WORKAROUND")
[xml]$csproj = Get-Content ./MsgPack.UnitTest.Unity.Il2cpp.Full.Desktop.csproj
foreach($c in $csproj.Project.ItemGroup.Compile)
{
    if ($c.Include -eq $null)
    {
        continue
    }

    if ($c.Link -ne $null)
    {
        $destination = "Assets/UnitTests/$($c.Link)".Replace("\", "/")
    }
    else
    {
        $destination = "Assets/UnitTests/$($c.Include)".Replace("\", "/")
    }

    $destinationDirectory = [IO.Path]::GetDirectoryName($destination);
    if (!(Test-Path $destinationDirectory))
    {
        New-Item $destinationDirectory -ItemType Directory | Out-Null
    }

    Copy-Item $c.Include $destination -Force

    if ($destination.EndsWith(".cs"))
    {
        $code = [IO.File]::ReadAllLines($destination)
        [IO.File]::WriteAllLines($destination, "#define UNITY")
        [IO.File]::AppendAllLines($destination, $additionalDefines)

        # Change "protected internal" to "protected" because Unity build drop is not "InternalsVisibleTo" target.
        if ($destination.Contains("gen35") -or $destination.Contains("AutoMessagePackSerializerTest.Types.cs") -or $destination.Contains("RegressionTests.cs") -or $destination.Contains("SerializationContextTest.cs"))
        {
            $appender = [IO.File]::AppendText($destination)

            foreach ($line in $code)
            {
                if ($destination.Contains("gen35"))
                {
                    # Change FILETIME to DateTime because FILETIME in Unity is not supported.
                    $appender.WriteLine($line.Replace("protected internal", "protected").Replace("System.Runtime.InteropServices.ComTypes.FILETIME", "System.DateTime"))
                }
                else
                {
                    $appender.WriteLine($line.Replace("protected internal", "protected"))
                }
            }

            $appender.Flush()
            $appender.Dispose()
        }
        else
        {
            [IO.File]::AppendAllLines($destination, $code)           
        }
    }
}

New-Item ./Assets/Dll -ItemType Directory | Out-Null
Copy-Item ./bin/Debug/MsgPack.dll ./Assets/Dll/MsgPack.dll -Force
Copy-Item ./link.xml ./Assets/link.xml -Force
