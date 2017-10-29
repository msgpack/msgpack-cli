#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015-2016 FUJIWARA, Yusuke
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
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		<see cref="Timestamp"/> serializer using Unix Epoc or native representation.
	/// </summary>
	internal class TimestampMessagePackSerializer : MessagePackSerializer<Timestamp>
	{
		private readonly DateTimeConversionMethod _conversion;

		public TimestampMessagePackSerializer( SerializationContext ownerContext, DateTimeConversionMethod conversion )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
		{
			this._conversion = conversion;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, Timestamp objectTree )
		{
			if ( this._conversion == DateTimeConversionMethod.Timestamp )
			{
				packer.Pack( objectTree.Encode() );
			}
			else if ( this._conversion == DateTimeConversionMethod.Native )
			{
				packer.Pack( objectTree.ToDateTime().ToBinary() );
			}
			else
			{
#if DEBUG
				Contract.Assert( this._conversion == DateTimeConversionMethod.UnixEpoc );
#endif // DEBUG
				packer.Pack( MessagePackConvert.FromDateTimeOffset( objectTree.ToDateTimeOffset() ) );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Timestamp UnpackFromCore( Unpacker unpacker )
		{
			if ( unpacker.LastReadData.IsTypeOf<MessagePackExtendedTypeObject>().GetValueOrDefault() )
			{
				return Timestamp.Decode( unpacker.LastReadData.AsMessagePackExtendedTypeObject() );
			}
			else if ( this._conversion == DateTimeConversionMethod.UnixEpoc )
			{
				return MessagePackConvert.ToDateTimeOffset( unpacker.LastReadData.DeserializeAsInt64() );
			}
			else
			{
				return new DateTimeOffset( DateTime.FromBinary( unpacker.LastReadData.DeserializeAsInt64() ), TimeSpan.Zero );
			}
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, Timestamp objectTree, CancellationToken cancellationToken )
		{
			if ( this._conversion == DateTimeConversionMethod.Timestamp )
			{
				await packer.PackAsync( objectTree.Encode(), cancellationToken ).ConfigureAwait( false );
			}
			else if ( this._conversion == DateTimeConversionMethod.Native )
			{
				await packer.PackAsync( objectTree.ToDateTime().ToBinary(), cancellationToken ).ConfigureAwait( false );
			}
			else
			{
#if DEBUG
				Contract.Assert( this._conversion == DateTimeConversionMethod.UnixEpoc );
#endif // DEBUG
				await packer.PackAsync( MessagePackConvert.FromDateTimeOffset( objectTree.ToDateTimeOffset() ), cancellationToken ).ConfigureAwait( false );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<Timestamp> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<Timestamp>();
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
