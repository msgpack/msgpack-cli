﻿#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2013 FUJIWARA, Yusuke
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
using System.Diagnostics;
using System.Text;

namespace MsgPack
{
	/// <summary>
	///		Defines extension methods to achieve compatiblity between .NET 3.5 and .NET 4.0.
	/// </summary>
	internal static class NetFxCompatibilities
	{
		public static void Clear( this StringBuilder source )
		{
			source.Length = 0;
		}

#if DEBUG
		public static void Restart( this Stopwatch source )
		{
			source.Stop();
			source.Reset();
			source.Start();
		}
#endif // DEBUG
	}
}
