#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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
using System.Runtime.InteropServices.ComTypes;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		<see cref="FILETIME"/> serializer using timestamp representation.
	/// </summary>
	internal sealed class TimestampFileTimeMessagePackSerializer : MessagePackSerializer<FILETIME>
	{
		public TimestampFileTimeMessagePackSerializer( SerializationContext ownerContext ) : base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, FILETIME objectTree )
		{
			packer.Pack( Timestamp.FromDateTime( objectTree.ToDateTime() ).Encode() );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override FILETIME UnpackFromCore( Unpacker unpacker )
		{
			return Timestamp.Decode( unpacker.LastReadData.DeserializeAsMessagePackExtendedTypeObject() ).ToDateTime().ToWin32FileTimeUtc();
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Task PackToAsyncCore( Packer packer, FILETIME objectTree, CancellationToken cancellationToken )
		{
			return packer.PackAsync( Timestamp.FromDateTime( objectTree.ToDateTime() ).Encode(), cancellationToken );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<FILETIME> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<FILETIME>();
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
