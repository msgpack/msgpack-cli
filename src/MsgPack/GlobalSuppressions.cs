#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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

using System.Diagnostics.CodeAnalysis;

[module: SuppressMessage( "Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "MsgPack.Collections", Justification = "Under construction." )]
[assembly: SuppressMessage( "Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "MsgPack.Serialization.UnpackHelpers+UnpackerTraceContext" )]
[assembly: SuppressMessage( "Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.IO.TextWriter.#ctor", Scope = "member", Target = "MsgPack.Serialization.NullTextWriter.#.ctor()", Justification = "NullTextWriter does not do any action." )]
#if SILVERLIGHT && WINDOWS_PHONE
[assembly: SuppressMessage( "Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames" )]
#endif // SILVERLIGHT && WINDOWS_PHONE
