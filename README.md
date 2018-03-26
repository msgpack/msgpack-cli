# MessagePack for CLI

## What is it?

This is MessagePack serialization/deserialization for CLI (Common Language Infrastructure) implementations such as .NET Framework, Silverlight, Mono (including Moonlight.)
This library can be used from ALL CLS compliant languages such as C#, F#, Visual Basic, Iron Python, Iron Ruby, PowerShell, C++/CLI or so.

## Usage

You can serialize/deserialize objects as following:
1. Create serializer via `MessagePackSerializer.Get` generic method. This method creates dependent types serializers as well.
1. Invoke serializer as following:
** `Pack` method with destination `Stream` and target object for serialization.
** `Unpack` method with source `Stream`.

```c#
// Creates serializer.
var serializer = MessagePackSerializer.Get<T>();
// Pack obj to stream.
serializer.Pack(stream, obj);
// Unpack from stream.
var unpackedObject = serializer.Unpack(stream);
```

```vb
' Creates serializer.
Dim serializer = MessagePackSerializer.Get(Of T)()
' Pack obj to stream.
serializer.Pack(stream, obj)
' Unpack from stream.
Dim unpackedObject = serializer.Unpack(stream)
```

## Features

* Fast and interoperable binary format serialization with simple API.
* Generating pre-compiled assembly for rapid start up.
* Flexible MessagePackObject which represents MessagePack type system naturally.

## Documentation

See [wiki](https://github.com/msgpack/msgpack-cli/wiki)

## Installation

* Binary files distributed via the NuGet package [MsgPack.Cli](http://www.nuget.org/packages/MsgPack.Cli/).
* You can extract binary (DLL) file as following:
  1. Download *.zip file from [GitHub Release page](https://github.com/msgpack/msgpack-cli/releases/).
  2. Extract it.
  3. Under the `bin` directory, binaries are there!
    * For mono, you can use `net461` or `net35` drops as you run with.
    * For Unity, `unity3d` drop is suitable.

## How to build

### For .NET Framework

1. Install Visual Studio 2017 (Community edition is OK) and 2015 (for MsgPack.Windows.sln).
    * You must install .NET Framework 3.5, 4.x, .NET Core, and Xamarin dev tools to build all builds successfully.
      If you do not want to install options, edit `<TargetFrameworks>` element in `*.csproj` files to exclude platforms you want to exclude.
2. Install latest .NET Core SDK.
3. Run with Visual Studio Developer Command Prompt:

    msbuild MsgPack.sln /t:Restore
    msbuild MsgPack.sln

  Or (for Unity 3D drops):

    msbuild MsgPack.compats.sln /t:Restore
    msbuild MsgPack.compats.sln

  Or (for Windows Runtime/Phone drops and Silverlight 5 drops):

    msbuild MsgPack.Windows.sln /t:Restore
    msbuild MsgPack.Windows.sln

  Or (for Xamarin unit testing, you must have Xamarin Business or upper license and Mac machine on the LAN to build on Windows):

    msbuild MsgPack.Xamarin.sln /t:Restore
    msbuild MsgPack.Xamarin.sln

Or open one of above solution files in your IDE and run build command in it.

### For Mono

1. Install latest Mono and .NET Core SDK.
2. Now, you can build MsgPack.sln and MsgPack.Xamarin.sln with above instructions and `msbuild` in latest Mono. Note that `xbuild` does not work because it does not support latest csproj format.

### Own Unity 3D Build

First of all, there are binary drops on github release page, you should use it to save your time. 
Because we will not guarantee source code organization compatibilities, we might add/remove non-public types or members, which should break source code build.
If you want to import sources, you must include just only described on MsgPack.Unity3D.csproj.
If you want to use ".NET 2.0 Subset" settings, you must use just only described on MsgPack.Unity3D.CorLibOnly.csproj file, and define `CORLIB_ONLY` compiler constants.

## See also

*  GitHub Page           : http://cli.msgpack.org/
*  Wiki (documentation)  : https://github.com/msgpack/msgpack-cli/wiki
*  API Reference         : http://cli.msgpack.org/doc/top.html
*  Issue tracker         : https://github.com/msgpack/msgpack-cli/issues
*  MSBuild reference     : http://msdn.microsoft.com/en-us/library/0k6kkbsd.aspx
*  Mono xbuild reference : http://www.mono-project.com/Microsoft.Build
