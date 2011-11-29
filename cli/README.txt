MessagePack for CLI Readme

What is it?

  This is MessagePack serialization/deserialization for CLI (Common Language 
  Infrastructure) implementations such as .NET Framework, Silverlight, Mono
  (including Moonlight.)
  This library can be from ALL CLS compliant languages such as C#, F#,
  Visual Basic, Iron Pyhton, Iron Ruby, PowerShell, C++/CLI or so.

How to build

  For .NET Framework
     1.  Install recent Windows SDK (at least, .NET Framework 4 Full Profile and MSBuild is needed.)
         Or install Visual Studio or Visual Studio Express.
     1.1.   If you want to build unit test assemblies, install recent NUnit (2.5.2 or later).
            (Put nunit.core.dll and nunit.framework.dll in /msgpack/cli/lib directory.)
     2.  Run:
           msbuild MsgPack.csproj
         Or open MsgPack.csproj in your IDE and run build command in it.
     2.1.   You must specify NUnit assemblies to build unit test assemblies.

  For Mono
     T.B.D. (xbuild and Mono Develop might help you...)

See also

  Wiki                  : https://github.com/yfakariya/msgpack/wiki
  Issue tracker         : https://github.com/yfakariya/msgpack/issues
  MSBuild reference     : http://msdn.microsoft.com/en-us/library/0k6kkbsd.aspx
  (You can see translated version by changing "en-us" to some locale as you like (e.g. "ja-jp".)
  Mono xbuild reference : http://www.mono-project.com/Microsoft.Build