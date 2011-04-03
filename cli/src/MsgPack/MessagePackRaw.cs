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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MsgPack.Collections;
using System.Diagnostics;

namespace MsgPack
{
	// FIXME: Impl
	internal sealed class MessagePackRaw
	{
		private readonly ChunkBuffer _chunk;
		private readonly object _syncRoot;
		private Object _decoded;

		public sealed override bool Equals( object obj )
		{
			throw new NotImplementedException();
		}

		public bool Equals( String value )
		{
			throw new NotImplementedException();
		}

		public bool Equals( byte[] value )
		{
			throw new NotImplementedException();
		}

		public bool Equals( MessagePackRaw value )
		{
			throw new NotImplementedException();
		}

		public sealed override int GetHashCode()
		{
			throw new NotImplementedException();
		}

		public sealed override string ToString()
		{
			throw new NotImplementedException();
		}

		[DebuggerTypeProxy( typeof( MessagePackRaw ) )]
		internal sealed class DebuggingProxy
		{
			public sealed override string ToString()
			{
				throw new NotImplementedException();
			}
		}
	}
}
