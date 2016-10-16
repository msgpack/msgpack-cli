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

using System;
using System.IO;

namespace MsgPack
{
	internal static class StreamExtensions
	{
		public static void Write( this Stream source, byte[] buffer )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( buffer == null )
			{
				throw new ArgumentNullException( "buffer" );
			}

			source.Write( buffer, 0, buffer.Length );
		}

		public static void Feed( this Stream source, byte[] buffer )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( buffer == null )
			{
				throw new ArgumentNullException( "buffer" );
			}

			if ( !source.CanSeek )
			{
				throw new NotSupportedException();
			}

			source.Write( buffer, 0, buffer.Length );
			source.Position -= buffer.Length;
		}

		public static void Feed( this Stream source, byte value )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if ( !source.CanSeek )
			{
				throw new NotSupportedException();
			}

			source.WriteByte( value );
			source.Position--;
		}
	}
}