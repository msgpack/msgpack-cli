// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Runtime.CompilerServices;

#if DEBUG
[assembly: InternalsVisibleTo("MsgPack.Serialization.UnitTest")]
[assembly: InternalsVisibleTo("MsgPack.Serialization.ILGeneration.UnitTest")]
[assembly: InternalsVisibleTo("MsgPack.Serialization.Reflection.UnitTest")]
#endif // DEBUG
[assembly: InternalsVisibleTo("MsgPack.Compatibility")]
[assembly: InternalsVisibleTo("MsgPack.Serialization.ILGeneration")]
[assembly: InternalsVisibleTo("MsgPack.Serialization.Reflection")]

#warning TODO: Rename this package to MsgPack.Serialization.Core
