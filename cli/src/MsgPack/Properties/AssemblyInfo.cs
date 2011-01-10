#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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

using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle( "MessagePack for CLI(.NET/Mono)" )]
[assembly: AssemblyDescription( "MessagePack for CLI(.NET/Mono) packing/unpacking library." )]
[assembly: AssemblyConfiguration( "Experimental" )]
[assembly: AssemblyCopyright( "Copyright © FUJIWARA, Yusuke 2010" )]

// TODO: use script. Major = Informational-Major, Minor = Informational-Minor, Build = Epoc days from 2010/1/1, Revision = Epoc minutes from 00:00:00
[assembly: AssemblyFileVersion( "0.1.0.0" )]

[assembly: InternalsVisibleTo( "MsgPack.RPC" )]
[assembly: InternalsVisibleTo( "MsgPack.RPC.Client" )]
[assembly: InternalsVisibleTo( "MsgPack.RPC.Server" )]

#if DEBUG || PERFORMANCE_TEST
[assembly: InternalsVisibleTo( "MsgPack.UnitTest" )]
[assembly: InternalsVisibleTo( "MsgPack.RPC.UnitTest" )]
#endif


