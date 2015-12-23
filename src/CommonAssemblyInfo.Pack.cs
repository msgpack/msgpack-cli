#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

// Global informations for all MsgPack assemblies(except -RPC assemblies).

using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible( false )]

// This version represents API compatibility.
//   Major : Represents Major update like re-architecting, remove obsoleted APIs etc.
//   Minor : Represents Minor update like adding new feature, obsoleting APIs, fix specification issues, etc.
//   Build/Revision : Always 0 since CLI implementations does not care these number, so these changes cause some binding failures.
[assembly: AssemblyVersion( "0.6.0.0" )]

// This version represents libarary 'version' for human beings.
//   Major : Same as AssemblyVersion.
//   Minor : Same as AssemblyVersion.
//   Build : Bug fixes and improvements, which does not break API contract, but may break some code depends on internal implementation behaviors.
//           For example, some programs use reflection to retrieve private fields, analyse human readable exception messages or stack trace, or so.
//   Revision : Reserced. It might be used to indicate target platform or patch.
[assembly: AssemblyInformationalVersion( "0.6.7" )]
