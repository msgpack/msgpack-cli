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
using System.Diagnostics.Contracts;
using System.Globalization;

namespace MsgPack
{
	/// <summary>
	///		Common validtion utility.
	/// </summary>
	internal static class Validation
	{
		public static void ValidateBuffer<T>( T[] byteArray, int offset, long length, string nameOfByteArray, string nameOfLength, bool validateBufferSize )
		{
			Contract.Assume( !String.IsNullOrWhiteSpace( nameOfByteArray ) );
			Contract.Assume( !String.IsNullOrWhiteSpace( nameOfLength ) );

			if ( byteArray == null )
			{
				throw new ArgumentNullException( nameOfByteArray );
			}

			if ( offset < 0 )
			{
				throw new ArgumentOutOfRangeException( "offset", offset, String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "offset" ) );
			}

			if ( length < 0 )
			{
				throw new ArgumentOutOfRangeException( "nameOfLength", length, String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", nameOfLength ) );
			}

			if ( validateBufferSize && byteArray.Length < offset + length )
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"'{0}' is too small for specified '{1}' and '{2}'. Length of '{0}' is {3}, '{1}' is {4}, '{2}' is {5}.",
						nameOfByteArray,
						"offset",
						nameOfLength,
						byteArray.Length,
						offset,
						length
					)
				);
			}
		}
	}
}
