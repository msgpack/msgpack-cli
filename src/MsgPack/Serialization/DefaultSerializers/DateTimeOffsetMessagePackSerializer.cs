#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015-2017 FUJIWARA, Yusuke
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
using System.Runtime.Serialization;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		<see cref="DateTimeOffset"/> serializer using Unix Epoc or native representation.
	/// </summary>
	internal class DateTimeOffsetMessagePackSerializer : MessagePackSerializer<DateTimeOffset>
	{
		private readonly DateTimeConversionMethod _conversion;

		public DateTimeOffsetMessagePackSerializer( SerializationContext ownerContext, DateTimeConversionMethod conversion )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
		{
			this._conversion = conversion;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, DateTimeOffset objectTree )
		{
			if ( this._conversion == DateTimeConversionMethod.Timestamp )
			{
				packer.Pack( Timestamp.FromDateTimeOffset( objectTree ).Encode() );
			}
			else if ( this._conversion == DateTimeConversionMethod.Native )
			{
				packer.PackArrayHeader( 2 );
				packer.Pack( objectTree.DateTime.ToBinary() );
				unchecked
				{
					packer.Pack( ( short )( objectTree.Offset.Hours * 60 + objectTree.Offset.Minutes ) );
				}
			}
			else
			{
#if DEBUG
				Contract.Assert( this._conversion == DateTimeConversionMethod.UnixEpoc );
#endif // DEBUG
				packer.Pack( MessagePackConvert.FromDateTimeOffset( objectTree ) );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override DateTimeOffset UnpackFromCore( Unpacker unpacker )
		{
			if ( unpacker.LastReadData.IsTypeOf<MessagePackExtendedTypeObject>().GetValueOrDefault() )
			{
				return Timestamp.Decode( unpacker.LastReadData.DeserializeAsMessagePackExtendedTypeObject() ).ToDateTimeOffset();
			}
			else if ( unpacker.IsArrayHeader )
			{
				if ( UnpackHelpers.GetItemsCount( unpacker ) != 2 )
				{
					SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( DateTimeOffset ), 2 );
				}

				long ticks;
				if ( !unpacker.ReadInt64( out ticks ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				short offsetMinutes;
				if ( !unpacker.ReadInt16( out offsetMinutes ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				return new DateTimeOffset( DateTime.FromBinary( ticks ), TimeSpan.FromMinutes( offsetMinutes ) );
			}
			else
			{
				return MessagePackConvert.ToDateTimeOffset( unpacker.LastReadData.DeserializeAsInt64() );
			}
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, DateTimeOffset objectTree, CancellationToken cancellationToken )
		{
			if ( this._conversion == DateTimeConversionMethod.Timestamp )
			{
				await packer.PackAsync( Timestamp.FromDateTimeOffset( objectTree ).Encode(), cancellationToken ).ConfigureAwait( false );
			}
			else if ( this._conversion == DateTimeConversionMethod.Native )
			{
				await packer.PackArrayHeaderAsync( 2, cancellationToken ).ConfigureAwait( false );
				await packer.PackAsync( objectTree.DateTime.ToBinary(), cancellationToken ).ConfigureAwait( false );
				unchecked
				{
					await packer.PackAsync( ( short )( objectTree.Offset.Hours * 60 + objectTree.Offset.Minutes ), cancellationToken ).ConfigureAwait( false );
				}
			}
			else
			{
#if DEBUG
				Contract.Assert( this._conversion == DateTimeConversionMethod.UnixEpoc );
#endif // DEBUG
				await packer.PackAsync( MessagePackConvert.FromDateTimeOffset( objectTree ), cancellationToken ).ConfigureAwait( false );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<DateTimeOffset> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<DateTimeOffset>();
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
