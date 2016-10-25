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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class System_ByteArrayMessagePackSerializer : MessagePackSerializer<byte[]>
	{
		public System_ByteArrayMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override void PackToCore( Packer packer, byte[] value )
		{
			packer.PackBinary( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override byte[] UnpackFromCore( Unpacker unpacker )
		{
			var result = unpacker.LastReadData;
			return result.IsNil ? null : result.AsBinary();
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Task PackToAsyncCore( Packer packer, byte[] objectTree, CancellationToken cancellationToken )
		{
			return packer.PackBinaryAsync( objectTree, cancellationToken );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<byte[]> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<byte[]>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch ( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}
#endif // FEATURE_TAP

	}
}
