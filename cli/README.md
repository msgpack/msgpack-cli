# MessagePack for CLI

## What is it?

This is MessagePack serialization/deserialization for CLI (Common Language Infrastructure) implementations such as .NET Framework, Silverlight, Mono (including Moonlight.)
This library can be from ALL CLS compliant languages such as C#, F#, Visual Basic, Iron Pyhton, Iron Ruby, PowerShell, C++/CLI or so.

## Documentation

See [wiki](https://github.com/yfakariya/msgpack/wiki)

## How to build

### For .NET Framework

1. Install recent Windows SDK (at least, .NET Framework 4 Full Profile and MSBuild is needed.)
   Or install Visual Studio or Visual Studio Express.
    1. If you want to build unit test assemblies, install NuGet.
2. Run:
    msbuild MsgPack.csproj
   Or open MsgPack.sln in your IDE and run build command in it.

### For Mono

Open MsgPack.mono.sln in MonoDevelop and build it.
(You might be able to build via xbuild.)

## See also

  Wiki                  : https://github.com/yfakariya/msgpack/wiki
  Issue tracker         : https://github.com/yfakariya/msgpack/issues
  MSBuild reference     : http://msdn.microsoft.com/en-us/library/0k6kkbsd.aspx
  (You can see translated version by changing "en-us" to some locale as you like (e.g. "ja-jp".)
  Mono xbuild reference : http://www.mono-project.com/Microsoft.Build