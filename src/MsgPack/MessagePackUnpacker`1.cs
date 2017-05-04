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
#if DEBUG
#if CORE_CLR || UNITY || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || UNITY || NETSTANDARD1_1
#endif // DEBUG
using System.Globalization;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		Internal implementation for MessagePack unpacker.
	/// </summary>
	internal sealed partial class MessagePackUnpacker<TReader> : MessagePackUnpacker
		where TReader : UnpackerReader
	{
		public TReader Reader { get; private set; }

		public MessagePackUnpacker( TReader reader )
		{
			this.Reader = reader;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				this.Reader.Dispose();
			}

			base.Dispose( disposing );
		}

		public override bool Read()
		{
			MessagePackObject value;
			var success = this.ReadObject( /* isDeep */false, out value );
			if ( success )
			{
				this.Data = value;
				return true;
			}
			else
			{
				return false;
			}
		}

#if FEATURE_TAP

		public override async Task<bool> ReadAsync( CancellationToken cancellationToken )
		{
			var result = await this.ReadObjectAsync( /* isDeep */false, cancellationToken ).ConfigureAwait( false );
			if ( result.Success )
			{
				this.Data = result.Value;
				return true;
			}
			else
			{
				return false;
			}
		}

#endif // FEATURE_TAP

		public void ThrowEofException()
		{
			long offsetOrPosition;
			var isRealOffset = this.Reader.GetPreviousPosition( out offsetOrPosition );
			throw new InvalidMessagePackStreamException(
					String.Format(
						CultureInfo.CurrentCulture,
						isRealOffset
							? "Stream unexpectedly ends. Cannot read object from stream. Current position is {0:#,0}."
							: "Stream unexpectedly ends. Cannot read object from stream. Current offset is {0:#,0}.",
						offsetOrPosition
					)
				);
		}

		private void ThrowEofException( long reading )
		{
			long offsetOrPosition;
			var isRealOffset = this.Reader.GetPreviousPosition( out offsetOrPosition );
			throw new InvalidMessagePackStreamException(
					String.Format(
						CultureInfo.CurrentCulture,
						isRealOffset
							? "Stream unexpectedly ends. Cannot read {0:#,0} bytes from stream at position {1:#,0}."
							: "Stream unexpectedly ends. Cannot read {0:#,0} bytes from stream at offset {1:#,0}.",
						reading,
						offsetOrPosition
					)
				);
		}

		private void ThrowUnassignedMessageTypeException( int header )
		{
#if DEBUG
			Contract.Assert( header == 0xC1, "Unhandled header:" + header.ToString( "X2" ) );
#endif // DEBUG
			long offsetOrPosition;
			var isRealOffset = this.Reader.GetPreviousPosition( out offsetOrPosition );
			throw new UnassignedMessageTypeException(
				String.Format(
					CultureInfo.CurrentCulture,
					isRealOffset
						? "Unknown header value 0x{0:X} at position {1:#,0}"
						: "Unknown header value 0x{0:X} at offset {1:#,0}",
					header,
					offsetOrPosition
				)
			);
		}

		private void ThrowUnexpectedExtCodeException( ReadValueResult type )
		{
#if DEBUG
			Contract.Assert( false, "Unexpected ext-code type:" + type );
#endif // DEBUG
			// ReSharper disable HeuristicUnreachableCode
			long offsetOrPosition;
			var isRealOffset = this.Reader.GetPreviousPosition( out offsetOrPosition );

			throw new NotSupportedException(
				String.Format(
					CultureInfo.CurrentCulture,
					isRealOffset
						? "Unexpeded ext-code type {0} at position {1:#,0}"
						: "Unexpeded ext-code type {0} at offset {1:#,0}",
					type,
					offsetOrPosition
				)
			);
			// ReSharper restore HeuristicUnreachableCode
		}

		private void CheckLength( long length, ReadValueResult type )
		{
			if ( length > Int32.MaxValue )
			{
				this.ThrowTooLongLengthException( length, type );
			}
		}

		private void ThrowTooLongLengthException( long length, ReadValueResult type )
		{
			string message;
			long offsetOrPosition;
			var isRealOffset = this.Reader.GetPreviousPosition( out offsetOrPosition );

			switch ( type )
			{
				case ReadValueResult.ArrayLength:
				{
					message =
						isRealOffset
						? "MessagePack for CLI cannot handle large array (0x{0:X} elements) which has more than Int32.MaxValue elements, at position {1:#,0}"
						: "MessagePack for CLI cannot handle large array (0x{0:X} elements) which has more than Int32.MaxValue elements, at offset {1:#,0}";
					break;
				}
				case ReadValueResult.MapLength:
				{
					message =
						isRealOffset
						? "MessagePack for CLI cannot handle large map (0x{0:X} entries) which has more than Int32.MaxValue entries, at position {1:#,0}"
						: "MessagePack for CLI cannot handle large map (0x{0:X} entries) which has more than Int32.MaxValue entries, at offset {1:#,0}";
					break;
				}
				default:
				{
					message =
						isRealOffset
						? "MessagePack for CLI cannot handle large binary or string (0x{0:X} bytes) which has more than Int32.MaxValue bytes, at position {1:#,0}"
						: "MessagePack for CLI cannot handle large binary or string (0x{0:X} bytes) which has more than Int32.MaxValue bytes, at offset {1:#,0}";
					break;
				}
			}

			throw new MessageNotSupportedException(
				String.Format(
					CultureInfo.CurrentCulture,
					message,
					length,
					offsetOrPosition
				)
			);
		}

		private void ThrowTypeException( Type type, byte header )
		{
			long offsetOrPosition;
			var isRealOffset = this.Reader.GetPreviousPosition( out offsetOrPosition );
			throw new MessageTypeException(
				String.Format(
					CultureInfo.CurrentCulture,
					isRealOffset
					? "Cannot convert '{0}' type value from type '{2}'(0x{1:X}) in position {3:#,0}."
					: "Cannot convert '{0}' type value from type '{2}'(0x{1:X}) in offset {3:#,0}.",
					type,
					header,
					MessagePackCode.ToString( header ),
					offsetOrPosition
				)
			);
		}
	}
}
