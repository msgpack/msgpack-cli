# Contributions

## You can contribute...

* Submit issues in github. But please ensure that your question is already described in wiki(Åö) or samples(Åö)
* Pull-request. Note that your request might be rejected if it break existing test or it become applicable because of other fix(es).
* Write wiki (documentation request in github issues is OK, but please note that telling why/what documentation needed is harder than code, so it is happy if you write the purpose and example for the request).
* Improve unit tests more efficiently.
* Patches for English are welcome :)
* etc.

## Branching

MessagePack for CLIs branches are:
* Version branch for every *.n release. Active branch is the latest minor version having release tags.
** If the latest version branch does not have any tags, so it is developing branch.
* Mater branch is synchronized with active branch when the new release is done.
** You should submit pull-request for active branch. It will be merged newest develop branch and master branch in the future.

## Guidelines

### General Style

* Keep backward compatibility for published API.
* Write Unit test for APIs, and optinally stable/complex internal module.
* Think interoperability. If you want full-feature serializer only working on .NET, use `BinaryFormatter`. Interoperability for many languages is advantage of MessagePack.

### Coding Style

Follow existing styles, period. Please keep existing style because changing style is not so valuable, and out times should be spent to improve software itself unless the coding style causes observable and measurable impact.

#### Commonly Used Styles
These rules are commonly acceptted rules I think, so I omit their rationale.

* Published APIs follows Framework Design Guidelines. See, T.B.D. This rule can be checked by FxCop (CodeAnasys feature in VS).
* Allman style.
* Locals uses camelCasing, fields uses camelCasing precding underscore like '_foo'. 
** Note that 's_', 'm_', 't_' prefixes are not used.
* All methods are PascalCasing even if they are private. camelCasingName(...) expression should be delegate invocation except P/Invoke.

#### Specific Styles

These rules are not so common (at least, they are differ from .NET Core/Mono style), so I should describe their reason:

* Specify 'this' keyword to distinguish instance fields and static fields. I think [TheradStatic] and ThreadLocal<T> is rare than mutable statics, so these should be distinguished by field name like `xxxPerThread`.
* Use PascalCasing for private constants. In reality, I do not have policy for this, just following R# default.
* Put braces even if the statement is single line. It is multi-layer guard for a little but destroying bug.
* Use tab for indentation. Most tools supports tabs, developer can be configure spacing for a tab usually, code formatting tools act better for tab.
* Verbose spacing. It is a kind of multi-layer guard. 

OK, use formatting files below:
* Visual Studio Åö

### Unit Testing

* Please write unit testing to verify your reported issue is reproduced and solved.
* If you add new feature, write unit testing with testing methodology like border value analysis.

## Trouble Shooting

* Q: I cannot build MsgPack.compat.sln on Mono w/ xbuild error.
** A: Currently, this issue is not trucked, so use msbuild.exe to build unity lib and/or .NET 3.5 port.

* Q: I cannot run Windows Phone unit tests.
** A: The tool looks like not so stable, but you ensure that:
*** Recent Visual Studio Update is applied
*** And recent Windows Phone Tools are applied http://www.microsoft.com/en-us/download/details.aspx?id=43719Åö