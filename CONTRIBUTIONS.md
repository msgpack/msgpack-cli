Contributions Guidelines
====

Executive Summary
----

* Let's contribute.
* Make red-green test for code.

You can contribute...
----

* Submit issues in github. But please ensure that your question has not beeny described in wiki or samples.
* Pull-request. Note that your request might be rejected if it break existing test or it become applicable because of other fix(es).
* Write wiki.
* Create samples.
* Request documentation / samples.
* Report articles you found or written.
* Improve unit tests more efficiently.
* Patches for English text.
* New platform support including Xamarin Mac, Compact Framework, Micro Framework, etc.
* Improve tools.
* etc.

Branching
----

MessagePack for CLIs branches are:
* Version branch for every major/minor release for bug fixes. Active branch is the latest minor version.
* Master branch which contains next major/minor release.
* Some ephemeral WIP branches. Do not send PR for WIP branch.

Guidelines
----

### General Style

* Keep backward compatibility for published API.
* Write Unit test for APIs, and optinally stable/complex internal module.
* Think interoperability. If you want full-feature serializer only working on .NET, use `BinaryFormatter`.  Interoperability for many languages is advantage of MessagePack.

### Coding Style

Follow existing styles, period.  Please keep existing style because changing style is not so valuable, and out times should be spent to improve software itself unless the coding style causes observable and measurable impact.  
If you modify existing source, watch above/bellow lines and use their styles.  
If you put new source, you should follow bellow guidelines, but I don't think fixing your new file is more valuable than your life as long as it has reasonable readability. So, keep consistant in your new source file.

#### Commonly Used Styles

These rules are commonly acceptted rules I think, so I omit their rationale.

* Published APIs follows Framework Design Guidelines.  The digest is [here](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/framework-design-guidelines-digest.md).  This rule can be checked by FxCop (CodeAnasys feature in VS).
* Allman style.
* Locals uses camelCasing, fields uses camelCasing precding underscore like '_foo'. 
    * Note that 's_', 'm_', 't_' prefixes are currently not used.
* All methods are PascalCasing even if they are private. camelCasingName(...) expression should be delegate invocation except P/Invoke.

#### Specific Styles

These rules are not so common (at least, they are differ from .NET Core/Mono style), so I should describe their reason:

* Specify 'this' keyword to distinguish instance fields and static fields. I think [TheradStatic] and ThreadLocal<T> is rare than mutable statics, so these should be distinguished by field name like `xxxPerThread`.
* Use PascalCasing for private constants.
* Put braces even if the statement is single line. It is multi-layer guard for a little but destroying bug.
* Use tab for indentation. Most tools supports tabs, developer can be configure spacing for a tab usually, code formatting tools act better for tab.
* Verbose spacing. It is a kind of multi-layer guard. 

### Unit Testing

* Please write unit tests to verify your reported issue is reproduced and solved.
* If you add new feature, write unit testing with testing methodology like border value analysis.

How To
----

### Add New File for Various Projects

#### Add the File for All Project with the Tool

MessagePack for CLI has various projects to support multiple platforms, so there is a tool to synchronize project assets -- `SyncProjects`.  
This tool is located in `tools/SyncProjects`, simple XLinq based tool.  
If you add new file to the project, run `SyncProjects` as following:

* If the file should be work in all platforms, just run `SyncProjects.bat`.
    * If you work on *nix shell, use `view` and `source` to run the exe.
* If the file should not be work in some platforms, edit `Sync.xml` and/or `Sync.Test.xml`.
    * This file defines that given project should be copied from another project.
    * This file also defines exclusion and preservation.  
      By default, the tool synchronize all source inclusion from base project.
      If the preservation is specified, the file will be preserved on synchronized project.

If you think this is messy, let's post pull request anyway.  
It will be postponed one or some weeks, but it will be merged if the fix is valid and contains red-green testing.

#### Add Test Cases for Serializer Generation

**TODO:**

Solution Organizations
----

* **MsgPack.sln** contains CLR 4 based projects. This is primary platform.
* **MsgPack.compat.sln** contains CLR 2 based projects, specifically .NET 3.5 and Unity, and the `mpu` tool.  
  These projects are separated because Visual Studio cannot load multiple CLRs for unit testing at a time.
* **MsgPack.Windows.sln** contains Windows specific projects, namely Silverlight5, UWP, Windows Phone.  
  This solution also contains test projects for .NET Core build because most stable .NET Standard implementations are WinRT/UWP.  
  These projects are separated because non-Windows users cannot open them.
* **MsgPack.Xamarin.sln** contains Xamarin Android and Xamarin iOS projects.
  These projects are separated because of license requirements.
* **MsgPack.Xamarin.sln** contains Xamarin Android and Xamarin iOS projects.
  These projects are separated because of license requirements.
* There are no solutions for .NET Core/.NET Standard Libraries. There is a `project.json` under `src/netstandard` directory.  

FAQ
----

* Q: I cannot build MsgPack.compat.sln on Mono w/ xbuild error.
** A: Currently, this issue is not trucked, so use msbuild.exe to build unity lib and/or .NET 3.5 port.
