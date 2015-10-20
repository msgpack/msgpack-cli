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
	/// <summary>
	///		<see cref="DateTimeOffset"/> serializer using Unix Epoc or native representation.
	/// </summary>
	internal class DateTimeOffsetMessagePackSerializer : MessagePackSerializer<DateTimeOffset>
	{
		private readonly DateTimeConversionMethod _conversion;

		public DateTimeOffsetMessagePackSerializer( SerializationContext ownerContext, DateTimeConversionMethod conversion )
			: base( ownerContext )
		{
			this._conversion = conversion;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, DateTimeOffset objectTree )
		{
			if ( this._conversion == DateTimeConversionMethod.Native )
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
#if DEBUG && !UNITY
				Contract.Assert( this._conversion == DateTimeConversionMethod.UnixEpoc );
#endif // DEBUG && !UNITY
				packer.Pack( MessagePackConvert.FromDateTimeOffset( objectTree ) );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override DateTimeOffset UnpackFromCore( Unpacker unpacker )
		{
			if ( unpacker.IsArrayHeader )
			{
				if ( UnpackHelpers.GetItemsCount( unpacker ) != 2 )
				{
					throw new SerializationException( "Invalid DateTimeOffset serialization." );
				}

				long ticks;
				if ( !unpacker.ReadInt64( out ticks ) )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				short offsetMinutes;
				if ( !unpacker.ReadInt16( out offsetMinutes ) )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				return new DateTimeOffset( DateTime.FromBinary( ticks ), TimeSpan.FromMinutes( offsetMinutes ) );
			}
			else
			{
				return MessagePackConvert.ToDateTimeOffset( unpacker.LastReadData.AsInt64() );
			}
		}
	}
}