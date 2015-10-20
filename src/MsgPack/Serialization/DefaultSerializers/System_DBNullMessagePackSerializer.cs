#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if !UNITY
using System.Diagnostics.Contracts;
#endif // !UNITY
using System.Runtime.Serialization;

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal class System_DBNullMessagePackSerializer : MessagePackSerializer<DBNull>
	{
		public System_DBNullMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext ) {}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, DBNull objectTree )
		{
			packer.PackNull();
		}

		protected internal override DBNull UnpackFromCore( Unpacker unpacker )
		{
#if DEBUG && !UNITY
			Contract.Assert( !unpacker.LastReadData.IsNil );
#endif // DEBUG && !UNITY
			throw new SerializationException("DBNull value should be nil.");
		}

		protected internal override DBNull UnpackNil()
		{
			return DBNull.Value;
		}
	}
}