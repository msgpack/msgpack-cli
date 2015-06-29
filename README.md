# MessagePack for CLI

## What is it?

This is MessagePack serialization/deserialization for CLI (Common Language Infrastructure) implementations such as .NET Framework, Silverlight, Mono (including Moonlight.)
This library can be used from ALL CLS compliant languages such as C#, F#, Visual Basic, Iron Pyhton, Iron Ruby, PowerShell, C++/CLI or so.

## Usage

You can serialize/deserialize objects as following:
1. Create serializer via `MessagePackSerializer.Create` generic method. This method creates dependent types serializers as well.
1. Invoke serializer as following:
** `Pack` method with destination `Stream` and target object for serialization.
** `Unpack` method with source `Stream`.

```c#
// Creates serializer.
var serializer = SerializationContext.Default.GetSerializer<T>();
// Pack obj to stream.
serializer.Pack(stream, obj);
// Unpack from stream.
var unpackedObject = serializer.Unpack(stream);
```

```vb
' Creates serializer.
Dim serializer = SerializationContext.Default.GetSerializer(Of T)()
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
    * For mono, you can use `net45` or `net35` drops as you run with.
    * For Unity, `unity3d` drop is suitable.

## How to build

### For .NET Framework

1. Install recent Windows SDK (at least, .NET Framework 4 Client Profile and MSBuild is needed.) <br/>
   Or install Visual Studio or Visual Studio Express.
    1. If you want to build unit test assemblies, install NuGet and then restore NUnit packages.
2. Run:

    msbuild MsgPack.sln

  Or (for .NET 3.5 drops and Unity 3D drops):

    msbuild MsgPack.compats.sln

  Or (for Windows Runtime/Phone drops and Silverlight 5 drops):

    msbuild MsgPack.Windows.sln

  Or (for Xamarin drops, you must have Xamarin Business or upper license and Mac machine on the LAN to build on Windows):

    msbuild MsgPack.Xamarin.sln

Or open one of above solution files in your IDE and run build command in it.

### For Mono

Open MsgPack.mono.sln with MonoDevelop and then click **Build** menu item.
(Of cource, you can build via xbuild.)

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
