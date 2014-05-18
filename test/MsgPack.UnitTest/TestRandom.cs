#region -- Licence Terms --
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
#endregion  -- Licence Terms --

using System;

namespace MsgPack
{
	public sealed partial class TestRandom : Random
	{
		private readonly byte[] _castBuffer = new byte[ sizeof( ulong ) ];
		public TestRandom() {}

		public TestRandom( int seed ) : base( seed ) {}

		public bool NextBoolean()
		{
			return base.Next() % 2 == 0;
		}

		public Byte NextByte()
		{
			base.NextBytes( this._castBuffer );
			return this._castBuffer[ 0 ];
		}

		public Int16 NextInt16()
		{
			base.NextBytes( this._castBuffer );
			return BitConverter.ToInt16( this._castBuffer, 0 );
		}

		public Int32 NextInt32()
		{
			base.NextBytes( this._castBuffer );
			return BitConverter.ToInt32( this._castBuffer, 0 );
		}

		public Int64 NextInt64()
		{
			base.NextBytes( this._castBuffer );
			return BitConverter.ToInt64( this._castBuffer, 0 );
		}

		public Single NextSingle()
		{
			base.NextBytes( this._castBuffer );
			return BitConverter.ToSingle( this._castBuffer, 0 );
		}

		public Char NextChar()
		{
			base.NextBytes( this._castBuffer );
			return BitConverter.ToChar( this._castBuffer, 0 );
		}

#pragma warning disable 3021
		[CLSCompliant( false )]
#pragma warning restore 3021
		public SByte NextSByte()
		{
			base.NextBytes( this._castBuffer );
			return unchecked( ( sbyte )this._castBuffer[ 0 ] );
		}

#pragma warning disable 3021
		[CLSCompliant( false )]
#pragma warning restore 3021
		public UInt16 NextUInt16()
		{
			base.NextBytes( this._castBuffer );
			return BitConverter.ToUInt16( this._castBuffer, 0 );
		}

#pragma warning disable 3021
		[CLSCompliant( false )]
#pragma warning restore 3021
		public UInt32 NextUInt32()
		{
			base.NextBytes( this._castBuffer );
			return BitConverter.ToUInt32( this._castBuffer, 0 );
		}

#pragma warning disable 3021
		[CLSCompliant( false )]
#pragma warning restore 3021
		public UInt64 NextUInt64()
		{
			base.NextBytes( this._castBuffer );
			return BitConverter.ToUInt64( this._castBuffer, 0 );
		}
	}
}