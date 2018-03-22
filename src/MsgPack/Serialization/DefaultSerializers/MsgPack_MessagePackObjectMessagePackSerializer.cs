#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2018 FUJIWARA, Yusuke
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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
#if UNITY && DEBUG
	public
#else
	internal
#endif
	sealed class MsgPack_MessagePackObjectMessagePackSerializer : MessagePackSerializer<MessagePackObject>
	{
		public MsgPack_MessagePackObjectMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		protected internal override void PackToCore( Packer packer, MessagePackObject value )
		{
			value.PackToMessage( packer, null );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override MessagePackObject UnpackFromCore( Unpacker unpacker )
		{
			if ( unpacker.IsArrayHeader || unpacker.IsMapHeader )
			{
				return unpacker.UnpackSubtreeData();
			}
			else
			{
				return unpacker.LastReadData;
			}
		}

#if FEATURE_TAP

		protected internal override Task PackToAsyncCore( Packer packer, MessagePackObject objectTree, CancellationToken cancellationToken )
		{
			return objectTree.PackToMessageAsync( packer, null, cancellationToken );
		}

		protected internal override async Task<MessagePackObject> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( unpacker.IsArrayHeader || unpacker.IsMapHeader )
			{
				return await unpacker.UnpackSubtreeDataAsync( cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				return unpacker.LastReadData;
			}
		}
#endif // FEATURE_TAP

	}
}
