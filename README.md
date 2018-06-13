# MessagePack for CLI

[![Build status release](https://ci.appveyor.com/api/projects/status/5ln7u7efwjepj6o8?svg=true)](https://ci.appveyor.com/project/yfakariya/msgpack-cli-x2p85)
[![Build status debug](https://ci.appveyor.com/api/projects/status/dlc0v4rrolwj0t2t?svg=true)](https://ci.appveyor.com/project/yfakariya/msgpack-cli)
[![Build status debug net35](https://ci.appveyor.com/api/projects/status/cjp8phlnbwj7gkj9?svg=true)](https://ci.appveyor.com/project/yfakariya/msgpack-cli-3jme9)
[![Build status debug net35 CodeDOM](https://ci.appveyor.com/api/projects/status/1mw78wkxx50jvab1?svg=true)](https://ci.appveyor.com/project/yfakariya/msgpack-cli-rhnh0)

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

**For production environment, you should instantiate own `SerializationCOntext` and manage its lifetime. It is good idea to treat it as singleton because `SerializationContext` is thread-safe.**

## Features

* Fast and interoperable binary format serialization with simple API.
* Generating pre-compiled assembly for rapid start up.
* Flexible MessagePackObject which represents MessagePack type system naturally.

**Note: AOT support is limited yet. Use [serializer pre-generation](https://github.com/msgpack/msgpack-cli/wiki/Xamarin-and-Unity) with `mpu -s` utility or API.**  
If you do not pre-generated serializers, MsgPack for CLI uses reflection in AOT environments, it is slower and it sometimes causes AOT related error (`ExecutionEngineException` for runtime JIT compilation).

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

### Xamarin Android testing

If you run on Windows, it is recommended to use HXM instead of Hyper-V based emulator.  
You can disable Hyper-V from priviledged (administrator) powershell as follows:
```powershell
Disable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V-Hypervisor
```

If you want to use Hyper-V again (such as for Docker for Windows etc.), you can do it by following in priviledged (administrator) powershell:
```powershell
Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V-Hypervisor
```

#### Xamarin Android Trouble shooting tips

* Javac shows compilation error.
    * Rebuild the test project and try it run again.

### Xamarin iOS testing

You must create provisoning profiles in your MacOS devices.  
See [Xamarin documents about provisining](https://developer.xamarin.com/guides/ios/getting_started/installation/device_provisioning/free-provisioning/) for details.

There are bundle IDs of current iOS tests:
* `org.msgpack.msgpack-cli-xamarin-ios-test`
* `org.msgpack.msgpack-cli-xamarin-ios-test-packer`
* `org.msgpack.msgpack-cli-xamarin-ios-test-unpacker`
* `org.msgpack.msgpack-cli-xamarin-ios-test-unpacking`
* `org.msgpack.msgpack-cli-xamarin-ios-test-timestamp`
* `org.msgpack.msgpack-cli-xamarin-ios-test-arrayserialization`
* `org.msgpack.msgpack-cli-xamarin-ios-test-mapserialization`

*Note that some reflection based serializer tests failed with AOT related limitation.*

#### Xamarin iOS Trouble shooting tips

See [Xamarin's official trouble shooting docs first.](https://developer.xamarin.com/guides/ios/getting_started/installation/windows/connecting-to-mac/troubleshooting/)

* An error occurred while running unit test project.
    * Rebuild the project and rerun it. Or, login your Mac again, ant retry it.
* It is hard to read English.
    * You can read localized Xamarin docs with putting `{region}-{lang}` as the first component of URL path such as `https://developer.xamarin.com/ja-jp/guides/...`.

## See also

*  GitHub Page           : http://cli.msgpack.org/
*  Wiki (documentation)  : https://github.com/msgpack/msgpack-cli/wiki
*  API Reference         : http://cli.msgpack.org/doc/top.html
*  Issue tracker         : https://github.com/msgpack/msgpack-cli/issues
*  MSBuild reference     : http://msdn.microsoft.com/en-us/library/0k6kkbsd.aspx
*  Mono xbuild reference : http://www.mono-project.com/Microsoft.Build
