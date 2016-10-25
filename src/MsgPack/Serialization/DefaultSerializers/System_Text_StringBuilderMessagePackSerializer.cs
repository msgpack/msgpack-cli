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
using System.Text;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class System_Text_StringBuilderMessagePackSerializer : MessagePackSerializer<StringBuilder>
	{
		public System_Text_StringBuilderMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom | SerializerCapabilities.UnpackTo ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected internal override void PackToCore( Packer packer, StringBuilder value )
		{
			// NOTE: More efficient?
			packer.PackString( value.ToString() );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override StringBuilder UnpackFromCore( Unpacker unpacker )
		{
			// NOTE: More efficient?
			var result = unpacker.LastReadData;
			return result.IsNil ? null : new StringBuilder( result.DeserializeAsString() );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally." )]
		protected internal override void UnpackToCore( Unpacker unpacker, StringBuilder collection )
		{
			// NOTE: More efficient?
			var result = unpacker.LastReadData;
			if ( !result.IsNil )
			{
				collection.Append( result.DeserializeAsString() );
			}
		}

#if FEATURE_TAP

		protected internal override Task PackToAsyncCore( Packer packer, StringBuilder objectTree, CancellationToken cancellationToken )
		{
			// NOTE: More efficient?
			return packer.PackStringAsync( objectTree.ToString(), cancellationToken );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<StringBuilder> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<StringBuilder>();
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transferring exception." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally." )]
		protected internal override Task UnpackToAsyncCore( Unpacker unpacker, StringBuilder collection, CancellationToken cancellationToken )
		{
			// NOTE: More efficient?
			var tcs = new TaskCompletionSource<object>();
			try
			{
				var result = unpacker.LastReadData;
				if ( !result.IsNil )
				{
					collection.Append( result.DeserializeAsString() );
				}

				tcs.SetResult( null );
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
