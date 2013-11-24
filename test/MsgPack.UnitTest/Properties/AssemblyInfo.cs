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

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;

[assembly: AssemblyTitle( "Unit test of MessagePack for CLI" )]
[assembly: AssemblyDescription( "Unit test of MessagePack CLI binding" )]
[assembly: AssemblyConfiguration( "Develop" )]
[assembly: AssemblyProduct( "MessagePack" )]
[assembly: AssemblyCopyright( "Copyright © FUJIWARA, Yusuke 2010" )]

[assembly: ComVisible( false )]
[assembly: CLSCompliant( true )]
[assembly: NeutralResourcesLanguage( "en-US" )]
[assembly: AssemblyVersion( "1.0.0.0" )]
[assembly: AssemblyFileVersion( "0.1.0.0" )]
[assembly: AssemblyInformationalVersion( "0.1" )]

#if DEBUG || PERFORMANCE_TEST
[assembly: InternalsVisibleTo( "MsgPack.UnitTest.CodeDom, PublicKey=0024000004800000940000000602000000240000525341310004000001000100a967de8de9d45380b93a6aa56f64fc2cb2d3c9d4b400e00de01f31ba9e15cf5ca95926dbf8760cce413eabd711e23df0c133193a570da8a3bb1bdc00ef170fccb2bc033266fa5346442c9cf0b071133d5b484845eab17095652aeafeeb71193506b8294d9c8c91e3fd01cc50bdbc2d0eb78dd655bb8cd0bd3cdbbcb192549cb4" )]
#endif
