#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
using System.Security;

[assembly: AssemblyTitle( "MessagePack for CLI(.NET/Mono)" )]
[assembly: AssemblyDescription( "MessagePack for CLI(.NET/Mono) packing/unpacking library for" +
#if NET4_5
".NET Framework 4.5"
#elif NET35
".NET Framework 3.5"
#elif WINDOWS_UWP
"UWP"
#elif NETSTANDARD1_1
".NET Standard 1.1"
#elif NETSTANDARD1_3
".NET Standard 1.3"
#elif NETSTANDARD2_0
".NET Standard 2.0"
#else
".NET Framework 4.6"
#endif // NET4_5..
 )]

[assembly: AssemblyFileVersion( "0.9.2259.1047" )]

#if !NETSTANDARD1_1 && !NETSTANDARD1_3
#if !NET35
[assembly: SecurityRules( SecurityRuleSet.Level2, SkipVerificationInFullTrust = true )]
#endif // !NET35
[assembly: AllowPartiallyTrustedCallers]
#endif

#if DEBUG || PERFORMANCE_TEST
[assembly: InternalsVisibleTo( "MsgPack.UnitTest, PublicKey=0024000004800000940000000602000000240000525341310004000001000100a967de8de9d45380b93a6aa56f64fc2cb2d3c9d4b400e00de01f31ba9e15cf5ca95926dbf8760cce413eabd711e23df0c133193a570da8a3bb1bdc00ef170fccb2bc033266fa5346442c9cf0b071133d5b484845eab17095652aeafeeb71193506b8294d9c8c91e3fd01cc50bdbc2d0eb78dd655bb8cd0bd3cdbbcb192549cb4" )]
[assembly: InternalsVisibleTo( "MsgPack.UnitTest.CodeDom, PublicKey=0024000004800000940000000602000000240000525341310004000001000100a967de8de9d45380b93a6aa56f64fc2cb2d3c9d4b400e00de01f31ba9e15cf5ca95926dbf8760cce413eabd711e23df0c133193a570da8a3bb1bdc00ef170fccb2bc033266fa5346442c9cf0b071133d5b484845eab17095652aeafeeb71193506b8294d9c8c91e3fd01cc50bdbc2d0eb78dd655bb8cd0bd3cdbbcb192549cb4" )]
#if NET35
[assembly: InternalsVisibleTo( "MsgPack.UnitTest.Net35, PublicKey=0024000004800000940000000602000000240000525341310004000001000100a967de8de9d45380b93a6aa56f64fc2cb2d3c9d4b400e00de01f31ba9e15cf5ca95926dbf8760cce413eabd711e23df0c133193a570da8a3bb1bdc00ef170fccb2bc033266fa5346442c9cf0b071133d5b484845eab17095652aeafeeb71193506b8294d9c8c91e3fd01cc50bdbc2d0eb78dd655bb8cd0bd3cdbbcb192549cb4" )]
[assembly: InternalsVisibleTo( "MsgPack.UnitTest.CodeDom.Net35, PublicKey=0024000004800000940000000602000000240000525341310004000001000100a967de8de9d45380b93a6aa56f64fc2cb2d3c9d4b400e00de01f31ba9e15cf5ca95926dbf8760cce413eabd711e23df0c133193a570da8a3bb1bdc00ef170fccb2bc033266fa5346442c9cf0b071133d5b484845eab17095652aeafeeb71193506b8294d9c8c91e3fd01cc50bdbc2d0eb78dd655bb8cd0bd3cdbbcb192549cb4" )]
#endif // NET35
[assembly: InternalsVisibleTo( "MsgPack.UnitTest.BclExtensions, PublicKey=0024000004800000940000000602000000240000525341310004000001000100a967de8de9d45380b93a6aa56f64fc2cb2d3c9d4b400e00de01f31ba9e15cf5ca95926dbf8760cce413eabd711e23df0c133193a570da8a3bb1bdc00ef170fccb2bc033266fa5346442c9cf0b071133d5b484845eab17095652aeafeeb71193506b8294d9c8c91e3fd01cc50bdbc2d0eb78dd655bb8cd0bd3cdbbcb192549cb4" )]
#if NETSTANDARD1_3 || NETSTANDARD2_0
[assembly:InternalsVisibleTo( "MsgPack.UnitTest.Xamarin.Android, PublicKey = 0024000004800000940000000602000000240000525341310004000001000100a967de8de9d45380b93a6aa56f64fc2cb2d3c9d4b400e00de01f31ba9e15cf5ca95926dbf8760cce413eabd711e23df0c133193a570da8a3bb1bdc00ef170fccb2bc033266fa5346442c9cf0b071133d5b484845eab17095652aeafeeb71193506b8294d9c8c91e3fd01cc50bdbc2d0eb78dd655bb8cd0bd3cdbbcb192549cb4" )]
[assembly:InternalsVisibleTo( "MsgPackUnitTestXamariniOS, PublicKey=0024000004800000940000000602000000240000525341310004000001000100a967de8de9d45380b93a6aa56f64fc2cb2d3c9d4b400e00de01f31ba9e15cf5ca95926dbf8760cce413eabd711e23df0c133193a570da8a3bb1bdc00ef170fccb2bc033266fa5346442c9cf0b071133d5b484845eab17095652aeafeeb71193506b8294d9c8c91e3fd01cc50bdbc2d0eb78dd655bb8cd0bd3cdbbcb192549cb4" )]
#endif // NETSTANDARD1_3 || NETSTANDARD2_0
#endif // DEBUG || PERFORMANCE_TEST
