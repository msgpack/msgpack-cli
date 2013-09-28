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

#if !SILVERLIGHT
using System;
using System.Runtime.InteropServices.ComTypes;

namespace MsgPack.Serialization.DefaultSerializers
{
	internal sealed class System_Runtime_InteropServices_ComTypes_FILETIMEMessagePackSerializer : MessagePackSerializer<FILETIME>
	{
		private static readonly DateTime _fileTimeEpocUtc = new DateTime( 1601, 1, 1, 0, 0, 0, DateTimeKind.Utc );

		public System_Runtime_InteropServices_ComTypes_FILETIMEMessagePackSerializer( PackerCompatibilityOptions packerCompatibilityOptions )
			: base( packerCompatibilityOptions ) { }

		protected internal sealed override void PackToCore( Packer packer, FILETIME value )
		{
			packer.Pack( 
				MessagePackConvert.FromDateTime(
					// DateTime.FromFileTimeUtc in Mono 2.10.x does not return Utc DateTime (Mono issue #2936), so do convert manually to ensure returned DateTime is UTC.
					_fileTimeEpocUtc.AddTicks( unchecked( ( ( long )value.dwHighDateTime << 32 ) | ( value.dwLowDateTime & 0xffffffff ) ) )
				)
			);
		}

		protected internal sealed override FILETIME UnpackFromCore( Unpacker unpacker )
		{
			var value = MessagePackConvert.ToDateTime( unpacker.LastReadData.AsInt64() ).ToFileTimeUtc();
			return new FILETIME() { dwHighDateTime = unchecked( ( int )( value >> 32 ) ), dwLowDateTime = unchecked( ( int )( value & 0xffffffff ) ) };
		}
	}
}
#endif